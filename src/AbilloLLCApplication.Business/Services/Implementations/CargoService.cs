using AbilloLLCApplication.Business.DTOs.CargoDTOs;
using AbilloLLCApplication.Business.Exceptions.CargoExceptions;
using AbilloLLCApplication.Business.Services.Interfaces;
using AbilloLLCApplication.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Net;
using System.Text.Json;
using AbilloLLCApplication.Business.Tools;
using Microsoft.AspNetCore.Identity;
using AbilloLLCApplication.Core.Entities.Identity;
using AbilloLLCApplication.Business.Exceptions.UserExceptions;
using AbilloLLCApplication.Business.Exceptions.DriverExceptions;
using AbilloLLCApplication.Business.Exceptions.OfferExceptions;
using AbilloLLCApplication.Business.PaginationParameters;
using System.Text.RegularExpressions;
using AbilloLLCApplication.Business.DTOs.Common;
using Telegram.Bot;
using Microsoft.Extensions.Configuration;

namespace AbilloLLCApplication.Business.Services.Implementations
{
    public class CargoService : ICargoService
    {
        //private readonly TelegramBotClient telegramBotClient;
        private const string forbiddenWordsPattern = @"\b(?:Email|email|gmail|rate|pays|Gmail|Rate|Pays|\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b|\$?\d+(?:\.\d+)?\$?)\b";
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICargoRepository _cargoRepository;
        private readonly IMapper _mapper;
        private readonly IDriverRepository _driverRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userMananger;
        private readonly IOfferRepository _offerRepository;
        public CargoService(IConfiguration configuration,IHttpContextAccessor contextAccessor,IOfferRepository offerRepository,IDriverRepository driverRepository,UserManager<AppUser> userManager,ICargoRepository cargoRepository,IMapper mapper,IHttpContextAccessor httpContextAccessor,LinkGenerator linkGenerator)
        {
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            _offerRepository = offerRepository;
            _driverRepository = driverRepository;
            _userMananger = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _cargoRepository = cargoRepository;
        }
        public async Task<ResponseForPaginationDTO<GetAllCargosDTO>> GetAllCargosAsync(CargoParameters cargoParameters)
        {
           

            var  allCargos =  _cargoRepository.GetFiltered(c => c.CreatedAt.AddHours(1) > DateTime.UtcNow, true,"Offers");
            int cargosCount = allCargos.Count(); 
            var cargos = await allCargos.Skip((cargoParameters.CurrentPage - 1)*CargoParameters.MaxPageSize).Take(CargoParameters.MaxPageSize).ToListAsync();
            var cargosDTO =_mapper.Map<List<GetAllCargosDTO>>(cargos);
            int pagesNumber = (cargosCount / CargoParameters.MaxPageSize);
            if (cargosCount % CargoParameters.MaxPageSize != 0)
            {
                pagesNumber++;
            }
            ResponseForPaginationDTO<GetAllCargosDTO> response = new ResponseForPaginationDTO<GetAllCargosDTO>(cargosDTO,pagesNumber,10,10);

            return response;


        }
        public async Task<ResponseForPaginationDTO<GetCargosForDriverDTO>> GetNotTakenCargosForDriverAsync(CargoParameters cargoParameters)
        {
            var username = _httpContextAccessor?.HttpContext.User.Identity?.Name;
            
            var user = await _userMananger.FindByNameAsync(username);
            if (user is null) 
            {
                throw new UserNotFoundByIdException("User not found");
            }
            var driver = await _driverRepository.GetSingleAsync(d=>d.AppUserId == user.Id);
            if(driver is null)
            {
                throw new DriverNotFoundByIdException("Driver not found");
            }
           
            var allCargos = await _cargoRepository.GetFiltered(c=>c.IsTaken == false && c.CreatedAt.AddMinutes(15) > DateTime.UtcNow ,false,"Offers").ToListAsync();
            int cargosCount = allCargos.Count();

            var cargos =  allCargos.Skip((cargoParameters.CurrentPage - 1) * CargoParameters.MaxPageSize).Take(CargoParameters.MaxPageSize).ToList();
            
            var cargosDTO = new List<GetCargosForDriverDTO>();
            if(driver.Zipcode is not null && driver.Latitude is not null && driver.Longitude is not null)
            {
                foreach (var cargo in allCargos)
                {
                    if (cargo.PickUpZipcode is not null && cargo.Latitude is not null && cargo.Longitude is not null)
                    {
                        double pickUpLatitude;
                        double pickUpLongitude;
                        if (double.TryParse(cargo.Latitude, out pickUpLatitude) && double.TryParse(cargo.Longitude, out pickUpLongitude))
                        {
                            pickUpLongitude /= 10000;
                            pickUpLatitude /= 10000;


                            double distance = GeoCalculator.CalculateDistance(driver.Latitude ?? 1, driver.Longitude ?? 1, pickUpLatitude, pickUpLongitude);
                            if (distance <= 200) 
                            {
                                var cargoDTO = _mapper.Map<GetCargosForDriverDTO>(cargo);
                                cargoDTO.Latitude = pickUpLatitude;
                                cargoDTO.Longitude = pickUpLongitude;
                                if (cargo.Notes is not null)
                                {
                                    cargoDTO.Notes = Regex.Replace(cargo.Notes, forbiddenWordsPattern, "*");

                                }
                                cargoDTO.DistanceToDriver = Math.Ceiling(distance);
                                if (await _offerRepository.IsExistsAsync(o => o.DriverId == driver.Id && o.CargoId == cargo.Id))
                                {
                                    cargoDTO.HasOffer = true;
                                }
                                if (cargo.Length * cargo.Pieces <= driver.Length)
                                {
                                    cargosDTO.Add(cargoDTO);
                                }

                            }



                        }




                    }

                }
            }
            int pagesNumber = (cargosDTO.Count / CargoParameters.MaxPageSize);
            if (cargosDTO.Count % CargoParameters.MaxPageSize != 0)
            {
                pagesNumber++;
            }

            ResponseForPaginationDTO<GetCargosForDriverDTO> response = new ResponseForPaginationDTO<GetCargosForDriverDTO>(cargosDTO, pagesNumber,driver.Latitude ?? 1,driver.Longitude ?? 1);



            return response;
        }

        public async Task<GetCargoDTO> GetCargoByIdAsync(Guid Id)
        {
            var cargo = await _cargoRepository.GetSingleAsync(c=>c.Id == Id);
            if(cargo is null)
            {
                throw new CargoNotFoundByIdException("Cargo not found by Id");
            }
            var cargoDTO = _mapper.Map<GetCargoDTO>(cargo);
            return cargoDTO;
        }

        public async Task<GetCargoForOfferPostDTO> GetCargoForOfferPostAsync(Guid Id)
        {
            var userName = _contextAccessor?.HttpContext.User.Identity?.Name;
            var user = await _userMananger.FindByNameAsync(userName);
            if (user is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }
            var driver  = await _driverRepository.GetSingleAsync(d=>d.AppUserId == user.Id);
            if (driver is null)
            {
                throw new DriverNotFoundByIdException("Driver not found");
            }

            var cargo = await _cargoRepository.GetSingleAsync(c=>c.Id == Id,"Offers.Driver");
            if(cargo is null)
            {
                throw new CargoNotFoundByIdException("Cargo not found");
            }
            if(cargo.Offers.Any(o=>o.DriverId == driver.Id && o.Accepted is null)) 
            {
                throw new OfferAlreadyExistsException("You have already made offer");
            }
            if (cargo.IsTaken)
            {
                throw new CargoIsAlreadyTakenException("Cargo is already taken");
            }
            var cargoDTO = _mapper.Map<GetCargoForOfferPostDTO>(cargo);
            return cargoDTO;
        }

        
    }
}

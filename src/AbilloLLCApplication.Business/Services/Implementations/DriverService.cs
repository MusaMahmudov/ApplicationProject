using AbilloLLCApplication.Business.DTOs.DriversDTOs;
using AbilloLLCApplication.Business.Exceptions.DriverExceptions;
using AbilloLLCApplication.Business.Exceptions.UserExceptions;
using AbilloLLCApplication.Business.Services.Interfaces;
using AbilloLLCApplication.Business.Tools;
using AbilloLLCApplication.Core.Entities;
using AbilloLLCApplication.Core.Entities.Identity;
using AbilloLLCApplication.Database.Enums;
using AbilloLLCApplication.Database.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Services.Implementations
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _accessContext;
        private readonly ICargoRepository _cargoRepository;
        private readonly IConfiguration _configuration;
        public DriverService(IConfiguration configuration,ICargoRepository cargoRepository,IHttpContextAccessor contextAccessor,IDriverRepository driverRepository,IMapper mapper,UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _cargoRepository = cargoRepository;
            _accessContext = contextAccessor;
            _userManager = userManager;
            _mapper = mapper;
            _driverRepository = driverRepository;
        }

        public List<GetDriverDTO> GetDriversAll()
        {
            var drivers = _driverRepository.GetAll("AppUser");
            var driversDTO = _mapper.Map<List<GetDriverDTO>>(drivers);

            return driversDTO;

        }
        public async Task<GetDriverDTO> GetDriverByIdAsync(Guid id)
        {
            var driver = await _driverRepository.GetSingleAsync(d => d.Id == id,"AppUser");
            if (driver is null)
            {
                throw new DriverNotFoundByIdException("Driver not found by Id");
            }
            var driverDTO = _mapper.Map<GetDriverDTO>(driver);
            return driverDTO;
        }
        public async Task<PutDriverDTO> GetDriverForUpdateAsync(Guid id)
        {
            var driver = await _driverRepository.GetSingleAsync(d => d.Id == id);
            if(driver is null)
            {
                throw new DriveNotFoundException("Driver not found");
            }
            var driverDTO = _mapper.Map<PutDriverDTO>(driver);

            return driverDTO;
        }
        public async Task<GetDriverForChangeDimensionsDTO> GetDriverForChangeDimensionsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }

            var driver = await _driverRepository.GetSingleAsync(d=>d.AppUserId == user.Id);
            if(driver is null)
            {
                throw new DriveNotFoundException("Driver not found");
            }
            var driverDTO = _mapper.Map<GetDriverForChangeDimensionsDTO>(driver);
            return driverDTO;
        }
        public async Task ChangeDriverLengthAsync(ChangeDriverLengthDTO changeDriverLengthDTO,string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }

            var driver = await _driverRepository.GetSingleAsync(d => d.AppUserId == user.Id);
            if (driver is null)
            {
                throw new DriveNotFoundException("Driver not found");
            }
            driver = _mapper.Map(changeDriverLengthDTO,driver);
            _driverRepository.Update(driver);
           await _driverRepository.SaveChangesAsync();
        }
        public async Task CreateDriverAsync(PostDriverDTO postDriverDTO)
        {
            AppUser user;
            if(postDriverDTO.AppUserId is not null)
            {
                user = await _userManager.FindByIdAsync(postDriverDTO.AppUserId);

                if (user is null)
                {
                    throw new UserNotFoundByIdException("User not found");
                }
                if (await _driverRepository.IsExistsAsync(d => d.AppUserId == postDriverDTO.AppUserId))
                {
                    throw new UserAlreadyHasDriverException("This user is already taken");
                }
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Any(r => r == Roles.Driver.ToString()))
                {
                    throw new UserHasToBeDriverException("This doesn't have driver role,change role of the user");
                }
            }
            if(postDriverDTO.Width <= 0 || postDriverDTO.Length <= 0 || postDriverDTO.Height <= 0)
            {
                throw new DriverDimsProblemException("Dims have to be more than 0");
            }
           
            
            var newDriver = _mapper.Map<Driver>(postDriverDTO);
            await _driverRepository.CreateAsync(newDriver);
            await _driverRepository.SaveChangesAsync();

        }

     

        public async Task UpdateDriverByIdAsync(PutDriverDTO putDriverDTO,Guid Id)
        {
            var driver = await _driverRepository.GetSingleAsync(d=>d.Id == Id,"AppUser");
            if (driver is null)
            {
                throw new DriverNotFoundByIdException("Driver not found by Id");
            }
            AppUser? user;
            if (putDriverDTO.AppUserId is not null)
            {
                if(driver.AppUserId != putDriverDTO.AppUserId)
                {
                    user = await _userManager.FindByIdAsync(putDriverDTO.AppUserId);
                    if (user is null)
                    {
                        throw new UserNotFoundByIdException("User not found by Id");
                    }
                    if (await _driverRepository.IsExistsAsync(d => d.AppUserId == putDriverDTO.AppUserId))
                    {
                        throw new UserAlreadyHasDriverException("This user is already taken");
                    }
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Any(r => r == Roles.Driver.ToString()))
                    {
                        throw new UserHasToBeDriverException("This user doesn't have driver role,change role of the user");
                    }
                }


            }
            else
            {
                if (driver.AppUser is not null)
                {
                    driver.AppUser = null;
                }
            }
            if (putDriverDTO.Width <= 0 || putDriverDTO.Length <= 0 || putDriverDTO.Height <= 0)
            {
                throw new DriverDimsProblemException("Dims have to be more than 0");
            }


            driver = _mapper.Map(putDriverDTO,driver);
            _driverRepository.Update(driver);
            await _driverRepository.SaveChangesAsync();
        }

        public async Task DeleteDriverByIdAsync(Guid id)
        {
            var driver = await _driverRepository.GetSingleAsync(d=>d.Id == id);
            if(driver is null)
            {
                throw new DriverNotFoundByIdException("Driver not found by Id");
            }
            _driverRepository.Delete(driver);
            await _driverRepository.SaveChangesAsync();

        }

        public async Task<GetDriverForOfferCreateDTO> GetDriverForOfferCreateAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }
            var driver = await _driverRepository.GetSingleAsync(d=>d.AppUserId == userId);
            if(driver is null)
            {
                throw new DriverNotFoundByIdException("driver not found");
            }
            var driverDTO = _mapper.Map<GetDriverForOfferCreateDTO>(driver);
            return driverDTO;
        }

        public async Task ChangeDriverLocationAsync(ChangeDriverLocationDTO changeDriverLocationDTO, string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }
            var driver = await _driverRepository.GetSingleAsync(d=>d.AppUserId == user.Id);
            if (driver is null)
            {
                throw new DriverNotFoundByIdException("Driver not found");
            }
            driver = _mapper.Map(changeDriverLocationDTO,driver);
            _driverRepository.Update(driver);
            await _driverRepository.SaveChangesAsync();
            var cargos = _cargoRepository.GetFiltered(c => c.IsTaken == false && c.CreatedAt.AddMinutes(15) > DateTime.UtcNow,false,"Offers");

            foreach (var cargo in cargos)
            {
                double pickUpLatitude;
                double pickUpLongitude;
                if (double.TryParse(cargo.Latitude, out pickUpLatitude) && double.TryParse(cargo.Longitude, out pickUpLongitude))
                {

                   
                        if (GeoCalculator.CalculateDistance(driver.Latitude ?? 1, driver.Longitude ?? 1, pickUpLatitude, pickUpLongitude) <= 200 && driver.TelegramUserId is not null)
                        {

                            string text = $"({cargo.PickUpCity}-{cargo.DeliverToCity}, Miles: {cargo.Miles}, {cargo.Weight} lbs.)\n" +
                            $"PickUp:  {cargo.PickUpCity} {cargo.PickUpZipcode}\n" +
                            $"DeliverTo: {cargo.DeliverToCity} {cargo.DeliverToZipcode}\n" +
                            $"Link:  ";

                            try
                            {
                                HttpClient client = new HttpClient();

                                await client.GetAsync($"telegramURL");

                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Exception occured while sending message");
                            }


                        }
                    

                }

            }

        }

        public async Task<GetDriverForChangeLocationDTO> GetDriverForChangeLocationAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }
            var driver = await _driverRepository.GetSingleAsync(d=>d.AppUserId == user.Id);
            if(driver is null)
            {
                throw new DriveNotFoundException("Driver not found");
            }
            var driverDTO  = _mapper.Map<GetDriverForChangeLocationDTO>(driver);
            return driverDTO;
        }

       
    }
}

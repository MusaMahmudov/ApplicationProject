using AbilloLLCApplication.Business.DTOs;
using AbilloLLCApplication.Business.DTOs.OfferDTOs;
using AbilloLLCApplication.Business.Exceptions.CargoExceptions;
using AbilloLLCApplication.Business.Exceptions.DriverExceptions;
using AbilloLLCApplication.Business.Exceptions.OfferExceptions;
using AbilloLLCApplication.Business.HelperServices.Interfaces;
using AbilloLLCApplication.Business.Services.Interfaces;
using AbilloLLCApplication.Core.Entities;
using AbilloLLCApplication.Core.Entities.Identity;
using AbilloLLCApplication.Database.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AbilloLLCApplication.Business.Services.Implementations
{
    public class OfferService : IOfferService
    {
        private readonly IMapper _mapper;
        private readonly IOfferRepository _offerRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ICargoRepository _cargoRepository;
        private readonly IMailService _mailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public OfferService(UserManager<AppUser> userManager,IHttpContextAccessor httpContextAccessor,IMailService mailService,IMapper mapper, IOfferRepository offerRepository,IDriverRepository driverRepository,ICargoRepository cargoRepository)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailService;
            _cargoRepository = cargoRepository;
            _driverRepository = driverRepository;
            _mapper = mapper;
            _offerRepository = offerRepository;
        }

        public async Task<List<GetOfferDTO>> GetAllOffersAsync()
        {
            var offers = await _offerRepository.GetAll("Driver.AppUser","Cargo").ToListAsync();
            var offersDTO = _mapper.Map<List<GetOfferDTO>>(offers);
            return offersDTO;

        }
        public async Task<List<GetOfferByDriverIdDTO>> GetOffersByDriverIdAsync(Guid driverId)
        {
            if(!await _driverRepository.IsExistsAsync(d=>d.Id == driverId))
            {
                throw new DriverNotFoundByIdException("Driver not found");
            }
            var offers = await _offerRepository.GetFiltered(o=>o.DriverId == driverId,true,"Cargo").ToListAsync();
            var offersDTO = _mapper.Map<List<GetOfferByDriverIdDTO>>(offers);
            return offersDTO;
        }
        public async Task<List<GetOfferDTO>> GetOffersByCargoIdAsync(Guid cargoId)
        {
            var cargo = await _cargoRepository.GetSingleAsync(c=>c.Id == cargoId);
            if(cargo is null)
            {
                throw new CargoNotFoundByIdException("Cargo not found");
            }
            
            
                var offers = await _offerRepository.GetFiltered(o => o.CargoId == cargoId,true, "Driver.AppUser", "Cargo").ToListAsync();
                var offersDTO = _mapper.Map<List<GetOfferDTO>>(offers);
            

            

            return offersDTO;
        }

        public async Task CreateOfferAsync(PostOfferDTO postOfferDTO)
        {
            if(!await _driverRepository.IsExistsAsync(d=>d.Id == postOfferDTO.DriverId))
            {
                throw new DriverNotFoundByIdException("Driver not found");
            }
            var cargo = await _cargoRepository.GetSingleAsync(c=>c.Id == postOfferDTO.CargoId);
            if(cargo is null)
            {
                throw new CargoNotFoundByIdException("Cargo not found");
            }
            if(cargo.IsTaken)
            {
                throw new CargoIsAlreadyTakenException("Cargo is already taken");
            }
            var offer = await _offerRepository.GetSingleAsync(o=>o.DriverId == postOfferDTO.DriverId && o.CargoId == postOfferDTO.CargoId);

            if (offer is not null && (offer.Accepted is null || offer.Accepted == true))
            {
                throw new OfferAlreadyExistsException("You have already made offer");
            }
            var newOffer = _mapper.Map<Offer>(postOfferDTO);
            newOffer.WithPercent = Decimal.Multiply(newOffer.Price,1.1m);
          
           await _offerRepository.CreateAsync(newOffer);
           await _offerRepository.SaveChangesAsync();
        }

        public async Task UpdateOfferAsync(PatchOfferDTO patchOfferDTO,Guid OfferId)
        {
            var offer = await _offerRepository.GetSingleAsync(o=>o.Id == OfferId);
            if(offer is null)
            {
                throw new OfferNotFoundByIdException("Offer not found");
            }
            offer = _mapper.Map(patchOfferDTO,offer);
            offer.WithPercent = Decimal.Multiply(offer.Price, 1.1m);
            _offerRepository.Update(offer);
            await _offerRepository.SaveChangesAsync();
        }

        public async Task DeleteOfferAsync(Guid Id)
        {
            
            var offer = await _offerRepository.GetSingleAsync(o=>o.Id == Id);
            if (offer is null)
            {
                throw new OfferNotFoundByIdException("Offer not found");
            }
            _offerRepository.Delete(offer);
           await _offerRepository.SaveChangesAsync();
        }

        public async Task AcceptOfferAsync(Guid Id,decimal? ownPrice)
        {
            if(ownPrice <= 0)
            {
                throw new Exception("Price cannot be less than 0 or equal to 0");
            }
            var offer = await _offerRepository.GetSingleAsync(o=>o.Id == Id,"Driver.AppUser","Cargo");

            if(offer is null)
            {
                throw new OfferNotFoundByIdException("Offer not found");
            }
            if(offer.Accepted is not null)
            {
                throw new OfferAlreadyHasResponseException("Offer already has response");
            }
            if(ownPrice is not null && ownPrice > 0)
            {
                offer.WithPercent = ownPrice.Value;
                offer.Accepted = true;
                _offerRepository.Update(offer);
               await _cargoRepository.SaveChangesAsync();
            }

            var cargo = await _cargoRepository.GetSingleAsync(c => c.Id == offer.CargoId);
            var offers = _offerRepository.GetFiltered(o=>o.CargoId == offer.CargoId && o.Id != Id,false).ToList();
            foreach(var off in offers)
            {
                off.IsDeleted = true;
                off.Accepted = false;
                _offerRepository.Update(off);
            }
           await _offerRepository.SaveChangesAsync();
            
           
            cargo.IsTaken = true;
            _cargoRepository.Update(cargo);
           await _cargoRepository.SaveChangesAsync();
            MailRequestDTO mailRequestDTO = new MailRequestDTO()
            {
                ToEmail = "Example@mail.ru",
                Subject = "Test",
                Body = "<h1>Musa <h1>",
            };
            
          
            await _mailService.SendEmail(mailRequestDTO);

        }

        

        public async Task RejectOfferAsync(Guid Id)
        {
            var offer = await _offerRepository.GetSingleAsync(o=>o.Id == Id);
            if(offer is null)
            {
                throw new OfferNotFoundByIdException("Offer not found");
            }
            if (offer.Accepted is not null)
            {
                throw new OfferAlreadyHasResponseException("Offer already has response");
            }
            offer.Accepted = false;
            _offerRepository.Update(offer);
            await _offerRepository.SaveChangesAsync();
        }

        public async Task<GetOfferByIdForResponseDTO> GetOfferByIdForResponseAsync(Guid Id)
        {
            var offer = await _offerRepository.GetSingleAsync(o=>o.Id == Id);
            if(offer is null)
            {
                throw new OfferNotFoundByIdException("Offer not found");
            }
            if (offer.Accepted == true || offer.Accepted == false)
            {
                throw new OfferAlreadyHasResponseException("Offer already has response");
            }
            var offerDTO = _mapper.Map<GetOfferByIdForResponseDTO>(offer);
            return offerDTO;
        }
    }
}

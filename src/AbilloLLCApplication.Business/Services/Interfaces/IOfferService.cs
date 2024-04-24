using AbilloLLCApplication.Business.DTOs.OfferDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Services.Interfaces
{
    public interface IOfferService
    {
        Task<List<GetOfferDTO>> GetAllOffersAsync();
        Task<List<GetOfferDTO>> GetOffersByCargoIdAsync(Guid cargoId);
        Task<List<GetOfferByDriverIdDTO>> GetOffersByDriverIdAsync(Guid driverId);
        Task<GetOfferByIdForResponseDTO> GetOfferByIdForResponseAsync(Guid Id);
        Task CreateOfferAsync(PostOfferDTO postOfferDTO);
        Task UpdateOfferAsync(PatchOfferDTO patchOfferDTO,Guid OfferId);
        Task DeleteOfferAsync(Guid Id);
        Task AcceptOfferAsync(Guid Id,decimal? ownPrice);
        Task RejectOfferAsync(Guid Id);
    }
}

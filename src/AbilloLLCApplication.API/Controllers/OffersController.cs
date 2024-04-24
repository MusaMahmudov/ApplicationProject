using AbilloLLCApplication.Business.DTOs.OfferDTOs;
using AbilloLLCApplication.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AbilloLLCApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;
        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var offers = await _offerService.GetAllOffersAsync();
            return Ok(offers);
        }
        [HttpGet("[Action]/{CargoId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetOffersByCargoId(Guid CargoId)
        {
           var offers = await _offerService.GetOffersByCargoIdAsync(CargoId);
            return Ok(offers);
        }
        [HttpGet("[Action]/{OfferId}")]
        public async Task<IActionResult> GetOfferByIdForResponse(Guid OfferId)
        {
            var offer = await _offerService.GetOfferByIdForResponseAsync(OfferId);
            return Ok(offer);
        }
        [HttpGet("{DriverId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,Driver")]
        public async Task<IActionResult> GetOffersByDriverId(Guid DriverId)
        {
            var offers = await _offerService.GetOffersByDriverIdAsync(DriverId);
            return Ok(offers);
        }
        [HttpPatch("{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> UpdateOffer(PatchOfferDTO patchOfferDTO,Guid Id)
        {
            await _offerService.UpdateOfferAsync(patchOfferDTO,Id);
            return Ok("Offer was updated");
        }
        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> DeleteOffer(Guid Id)
        {
           await _offerService.DeleteOfferAsync(Id);
            return Ok("Offer was deleted successefully");
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
        public async Task<IActionResult> CreateOffer(PostOfferDTO postOfferDTO)
        {
           await _offerService.CreateOfferAsync(postOfferDTO);
            return Ok("Offer was created");
        }
        [HttpPatch("[Action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> AcceptOffer(Guid Id,decimal? ownPrice)
        {
            await _offerService.AcceptOfferAsync(Id, ownPrice);
            return Ok("Offer was sent");
        }
        [HttpPatch("[Action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> RejectOffer(Guid Id)
        {
           await _offerService.RejectOfferAsync(Id);
            return Ok("Offer has been rejected");
        }
    }
}

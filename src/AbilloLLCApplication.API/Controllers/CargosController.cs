using AbilloLLCApplication.Business.DTOs.CargoDTOs;
using AbilloLLCApplication.Business.DTOs.Common;
using Microsoft.AspNetCore.Hosting;
using AbilloLLCApplication.Business.PaginationParameters;
using AbilloLLCApplication.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AbilloLLCApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargosController : ControllerBase
    {
        private readonly ICargoService _cargoService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CargosController(ICargoService cargoService)
        {
            _cargoService = cargoService;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery]CargoParameters cargoParameters) 
        {
            var response = await _cargoService.GetAllCargosAsync(cargoParameters);
            
            return Ok(response);
        }
       
        [HttpGet("{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetCargoById(Guid Id)
        {
            var cargo = await _cargoService.GetCargoByIdAsync(Id);
            return Ok(cargo);
        }
        [HttpGet("[Action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer",Roles = "Driver")]
        public async Task<IActionResult> GetCargoForOfferPost(Guid Id)
        {
            var cargo = await _cargoService.GetCargoForOfferPostAsync(Id);
            return Ok(cargo);
            
        }
        [HttpGet("[Action]")]
        [Authorize(AuthenticationSchemes = "Bearer",Roles ="Driver")]
        public async Task<IActionResult> GetCargosForDriver([FromQuery]CargoParameters cargoParameters)
        {
            var response = await _cargoService.GetNotTakenCargosForDriverAsync(cargoParameters);

            return Ok(response);
        }
    }
}

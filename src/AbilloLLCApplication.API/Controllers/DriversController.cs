using AbilloLLCApplication.Business.DTOs.Common;
using AbilloLLCApplication.Business.DTOs.DriversDTOs;
using AbilloLLCApplication.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AbilloLLCApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DriversController : ControllerBase
    {
        private readonly IDriverService _driverService;
        public DriversController(IDriverService driverService)
        {
            _driverService = driverService;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public IActionResult GetAllDrivers() 
        {
            var drivers = _driverService.GetDriversAll();
            return Ok(drivers);
        }
        [HttpGet("{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetDriverById(Guid Id) 
        {
         var driver = await _driverService.GetDriverByIdAsync(Id);
            return Ok(driver);
        }
        [HttpGet("[Action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
        public async Task<IActionResult> GetDriverForLocationChange(string Id)
        {
            var driver = await _driverService.GetDriverForChangeLocationAsync(Id);
            return Ok(driver);
        }
        [HttpGet("GetDriverForUpdate/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetDriverForUpdate(Guid Id)
        {
            var driver = await _driverService.GetDriverForUpdateAsync(Id);
            return Ok(driver);
        }
        [HttpGet("GetDriverForOfferCreate/{UserId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
        public async Task<IActionResult> GetDriverForOfferCreate(string UserId)
        {
          var driver =  await _driverService.GetDriverForOfferCreateAsync(UserId);
            return Ok(driver);
        }
        [HttpGet("GetDriverForDimensionsChange/{UserId}")]
        [Authorize(AuthenticationSchemes ="Bearer",Roles = "Driver")]
        public async Task<IActionResult> GetDriverForDimensionsChange(string UserId)
        {
            var driver = await _driverService.GetDriverForChangeDimensionsAsync(UserId);
            return Ok(driver);

        }
        [HttpPatch("ChangeLength/{UserId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
        public async Task<IActionResult> ChangeDriverLength(ChangeDriverLengthDTO changeDriverLengthDTO, string UserId)
        {
              await _driverService.ChangeDriverLengthAsync(changeDriverLengthDTO, UserId);
            return Ok("Driver updated");

        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> CreateDriver(PostDriverDTO postDriverDTO) 
        {
           await _driverService.CreateDriverAsync(postDriverDTO);
            return StatusCode((int)HttpStatusCode.OK,new ResponseDTO("Driver created successefully",HttpStatusCode.OK));

        }
        [HttpPut("{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> UpdateDriver(PutDriverDTO putDriverDTO,Guid Id)
        {
           await _driverService.UpdateDriverByIdAsync(putDriverDTO,Id);
            return Ok("Driver updated");
        }
        [HttpPut("[Action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer",Roles = "Driver")]
        public async Task<IActionResult> ChangeDriverLocation(ChangeDriverLocationDTO changeDriverLocationDTO,string Id)
        {
            await _driverService.ChangeDriverLocationAsync(changeDriverLocationDTO,Id);
            return Ok("Driver's location has been changed successefully");
        }
        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> DeleteDriver(Guid Id)
        {
            await _driverService.DeleteDriverByIdAsync(Id);
            return Ok("Driver deleted successefully");
        }

    }
}

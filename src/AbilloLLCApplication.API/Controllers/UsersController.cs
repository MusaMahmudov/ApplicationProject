using AbilloLLCApplication.Business.DTOs;
using AbilloLLCApplication.Business.DTOs.Common;
using AbilloLLCApplication.Business.DTOs.UserDTOs;
using AbilloLLCApplication.Business.Exceptions.UserExceptions;
using AbilloLLCApplication.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AbilloLLCApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {



        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? userFilter)
        {
            var users =await _userService.GetAllUserAsync(userFilter);

            return Ok(users);
        }
        [HttpGet("{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetUserById(string Id)
        {

            var user = await _userService.GetUserAsync(Id);
            return Ok(user);
        }
        [HttpGet("GetUserForUpdate/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetUserForUpdate(string Id)
        {
            var user = await _userService.GetUserForUpdateAsync(Id);
            return Ok(user);
        }

        [HttpGet("UsersForDriverUpdate/{driverId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetUsersForDriverUpdate(Guid driverId)
        {
            var users = await _userService.GetUsersForDriverUpdateAsync(driverId);
            return Ok(users);
        }
        [HttpGet("Chat")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,Driver")]
        public async Task<IActionResult> GetUsersForChat()
        {
            var users = await _userService.GetUsersForChatAsync();
            return  Ok(users);
        }

        [HttpGet("UsersForDriverCreate/")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetUsersForDriverCreate()
        {
            var users = await _userService.GetUsersForDriverCreateAsync();
            return Ok(users);
        }
        [HttpGet("AllRoles")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _userService.GetAllRolesAsync();
            return Ok(roles);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> CreateUser(PostUserDTO postUserDTO)
        {
            await _userService.CreateUserAsync(postUserDTO);
            return StatusCode((int)HttpStatusCode.OK, new ResponseDTO("User created successefully", HttpStatusCode.OK));

        }
        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> DeleteUserById([FromRoute]string Id)
        {
            await _userService.DeleteUserAsync(Id);
            return StatusCode((int)HttpStatusCode.OK, new ResponseDTO("User is deleted successefully",HttpStatusCode.OK));
        }
        [HttpPut("{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> UpdateUserById([FromRoute]string Id,PutUserDTO putUserDTO)
        {
           await _userService.UpdateUserByIdAsync(putUserDTO,Id);
            return StatusCode((int)HttpStatusCode.OK,new ResponseDTO("User updated successefully",HttpStatusCode.OK));
        }
        [HttpPut("ChangePassword/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,Driver")]
        public async Task<IActionResult> ChangePassword(string Id,ChangePasswordDTO changePasswordDTO)
        {
          await _userService.ChangePasswordAsync(Id,changePasswordDTO);
            return Ok("Password was changed");
        }
    }
}

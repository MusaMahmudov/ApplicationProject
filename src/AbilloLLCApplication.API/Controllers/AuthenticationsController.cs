using AbilloLLCApplication.Business.DTOs.AuthDTOs;
using AbilloLLCApplication.Business.DTOs.Common;
using AbilloLLCApplication.Business.Exceptions.SignInExceptions;
using AbilloLLCApplication.Business.Exceptions.UserExceptions;
using AbilloLLCApplication.Business.HelperServices.Interfaces;
using AbilloLLCApplication.Core.Entities.Identity;
using AbilloLLCApplication.Database.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AbilloLLCApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly SignInManager<AppUser > _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ISmsService _smsService;
        public AuthenticationsController(ISmsService smsService,ITokenService tokenService,SignInManager<AppUser> signInManager,UserManager<AppUser> userManager)
        {
            _smsService = smsService;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDTO signInDTO)
        {
            var user = await _userManager.Users.Include(u=>u.driver).FirstOrDefaultAsync(u=>u.UserName == signInDTO.UserName);
            if(user is null)
            {
                throw new SignInException("Username or Password incorrect");
            }
            var roles  = await _userManager.GetRolesAsync(user);
            if (roles.Any(r=>r == Roles.Driver.ToString()) && user.driver is null)
            {
                throw new UserDoesntHaveDriverException("User doent have driver");
            }
            var result = await _signInManager.PasswordSignInAsync(user,signInDTO.Password,false,false);

            if (!result.Succeeded)
            {
                throw new SignInException("Username or Password incorrect");
            }
            var token = await _tokenService.GenerateTokenAsync(user);
            token.RefreshTokenExpirationTime =DateTime.UtcNow.AddMonths(2);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpiryTime = token.RefreshTokenExpirationTime;
            user.LastVisit = DateTime.UtcNow;
           await _userManager.UpdateAsync(user);
           

            return Ok(token);
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> GenerateNewAccessToken(GenerateNewTokenDTO generateNewTokenDTO)
        {
            string? token = generateNewTokenDTO.Token;
            string refreshToken = generateNewTokenDTO?.RefreshToken;

            ClaimsPrincipal? principal = _tokenService.GetPrincipalFromJwtToken(token);
            if (principal is null)
            {
                return BadRequest("Invalid token ");
            }

          string email = principal.FindFirstValue(ClaimTypes.Email);
          if (email is null) 
            {
                return BadRequest("Invalid token " );

            }
            var user =await  _userManager.FindByEmailAsync(email);
          if(user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return BadRequest("Invalid token ");
            }
           var tokenDTO = await _tokenService.GenerateTokenAsync(user);
          user.RefreshToken = tokenDTO.RefreshToken;
           await _userManager.UpdateAsync(user);

            return Ok(tokenDTO);
        }
    }
}

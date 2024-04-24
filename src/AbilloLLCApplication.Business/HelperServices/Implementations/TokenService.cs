using AbilloLLCApplication.Business.DTOs.Common;
using AbilloLLCApplication.Business.HelperServices.Interfaces;
using AbilloLLCApplication.Business.Services.Interfaces;
using AbilloLLCApplication.Core.Entities.Identity;
using AbilloLLCApplication.Database.Enums;
using AbilloLLCApplication.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.HelperServices.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IDriverRepository _driverRepository;
        public TokenService(IDriverRepository driverRepository,UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            _driverRepository = driverRepository;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<TokenDTO> GenerateTokenAsync(AppUser user)
        {
           var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };
            foreach(var role in roles)
            {
                if(role == Roles.Driver.ToString())
                {
                    var driver = await _driverRepository.GetSingleAsync(d=>d.AppUserId == user.Id);
                    if (driver is not null)
                    {
                       claims.Add(new Claim("DriverId",driver.Id.ToString()));

                    }
                }
                claims.Add(new Claim(ClaimTypes.Role,role));
            }

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);


            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                audience: _configuration["JWT:Audience"],
                issuer: _configuration["JWT:Issuer"],
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(15)
                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = handler.WriteToken(jwtSecurityToken);

            return new TokenDTO(token,jwtSecurityToken.ValidTo,jwtSecurityToken.ValidFrom,GenerateRefreshToken(),DateTime.UtcNow.AddMonths(2));
           

        }

        public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
        {
            var TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:Audience"],
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])),
                ValidateLifetime = false,
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal claimsPrincipal = handler.ValidateToken(token,TokenValidationParameters,out  SecurityToken securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCulture  ))
            {
                throw new SecurityTokenException("Invalid security token");
            }
            return claimsPrincipal;

        }
    }
}

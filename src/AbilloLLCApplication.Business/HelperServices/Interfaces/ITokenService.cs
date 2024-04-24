using AbilloLLCApplication.Business.DTOs.Common;
using AbilloLLCApplication.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.HelperServices.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDTO> GenerateTokenAsync(AppUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.Common
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime ValidFrom { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationTime { get; set; }
        public TokenDTO(string token, DateTime expiresAt, DateTime validFrom, string refreshToken,DateTime refreshTokenExpirationTime)
        {
            RefreshTokenExpirationTime = refreshTokenExpirationTime;
            ValidFrom = validFrom;
            Token = token;
            ExpiresAt = expiresAt;
            RefreshToken = refreshToken;
        }
    }
}

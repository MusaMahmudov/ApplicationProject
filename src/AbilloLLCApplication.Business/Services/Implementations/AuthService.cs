using AbilloLLCApplication.Business.DTOs.AuthDTOs;
using AbilloLLCApplication.Business.DTOs.Common;
using AbilloLLCApplication.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        
        
        public Task<TokenDTO> SignInAsync(SignInDTO signInDTO)
        {
            
            throw new NotImplementedException();
        }
    }
}

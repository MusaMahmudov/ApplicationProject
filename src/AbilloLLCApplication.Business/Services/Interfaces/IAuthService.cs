using AbilloLLCApplication.Business.DTOs.AuthDTOs;
using AbilloLLCApplication.Business.DTOs.Common;
using AbilloLLCApplication.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Services.Interfaces
{
    public interface IAuthService  
    {
        Task<TokenDTO> SignInAsync(SignInDTO signInDTO);
    }
}

using AbilloLLCApplication.Business.DTOs.MessagesDTOs;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Services.Interfaces
{
    public interface IMessageService
    {
        Task<List<GetMessageDTO>> GetMessageAsync(string SecretFilePath, string SecretFilePathToken);
    }
}

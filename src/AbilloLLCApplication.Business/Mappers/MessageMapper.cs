using AbilloLLCApplication.Business.DTOs.MessagesDTOs;
using AutoMapper;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Mappers
{
    public class MessageMapper : AutoMapper.Profile
    { 
        public MessageMapper() 
        {
            CreateMap<Message, GetMessageDTO>() ;
        }
    }
}

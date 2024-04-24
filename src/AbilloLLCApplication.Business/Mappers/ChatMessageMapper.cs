using AbilloLLCApplication.Business.DTOs.ChatMessageDTOs;
using AbilloLLCApplication.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Mappers
{
    public class ChatMessageMapper : Profile
    {
        public ChatMessageMapper() 
        {
            CreateMap<Message, GetChatMessagesDTO>().ReverseMap();
        
        }  
    }
}

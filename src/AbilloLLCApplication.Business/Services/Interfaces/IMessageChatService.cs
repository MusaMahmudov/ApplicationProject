using AbilloLLCApplication.Business.DTOs.ChatMessageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Services.Interfaces
{
    public  interface IMessageChatService
    {
        Task SendMessageAsync(string content,string senderId,string receiverId);
        Task<List<GetChatMessagesDTO>> GetChatMessagesAsync(string userId,string otherUserId);
    }
}

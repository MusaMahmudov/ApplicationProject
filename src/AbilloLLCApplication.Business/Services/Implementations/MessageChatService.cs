using AbilloLLCApplication.Business.DTOs.ChatMessageDTOs;
using AbilloLLCApplication.Business.Exceptions.MessageExceptions;
using AbilloLLCApplication.Business.Exceptions.UserExceptions;
using AbilloLLCApplication.Business.Services.Interfaces;
using AbilloLLCApplication.Core.Entities;
using AbilloLLCApplication.Core.Entities.Identity;
using AbilloLLCApplication.Database.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Services.Implementations
{
    public class MessageChatService : IMessageChatService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public MessageChatService(IMapper mapper,IMessageRepository messageRepository,IHttpContextAccessor httpContextAccessor,UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _messageRepository = messageRepository;
        }

        public async Task<List<GetChatMessagesDTO>> GetChatMessagesAsync(string userId, string otherUserId)
        {
            var messages = await _messageRepository.GetFiltered(m=> (m.SenderId == userId && m.ReceiverId == otherUserId) || (m.ReceiverId == userId && m.SenderId == otherUserId) , false,"Receiver","Sender").ToListAsync();
            var messagesDTO = _mapper.Map<List<GetChatMessagesDTO>>(messages);
            return messagesDTO;
        }

        public async Task SendMessageAsync(string content,string senderId, string receiverId)
        {
           if(string.IsNullOrWhiteSpace(content))
            {
                throw new NullContentException("Message is empty");
            }

            var senderUser = await _userManager.FindByIdAsync(senderId);
            if(senderUser is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }
            var receiverUser =await _userManager.FindByIdAsync(receiverId);
            if (receiverUser is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }
            var newMessage = new Message()
            {
                Content = content,
                SenderId = senderId,
                ReceiverId = receiverId,

            };
          await  _messageRepository.CreateAsync(newMessage);
           await _messageRepository.SaveChangesAsync();

            


        }
    }
}

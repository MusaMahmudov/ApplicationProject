using AbilloLLCApplication.Business.Exceptions.MessageExceptions;
using AbilloLLCApplication.Business.Exceptions.UserExceptions;
using AbilloLLCApplication.Business.Services.Interfaces;
using AbilloLLCApplication.Core.Entities.Identity;
using AbilloLLCApplication.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AbilloLLCApplication.Business.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IMessageRepository _messageRepostory;
        public UserManager<AppUser> _userManager;
        private readonly IMessageChatService _messageChatService;
        public ChatHub(IMessageRepository messageRepository,IMessageChatService messageChatService,IHttpContextAccessor httpContextAccessor,UserManager<AppUser> userManager)
        {
            _messageRepostory = messageRepository;
            _messageChatService = messageChatService;
            _contextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task ReadMessage(Guid? messageId,string userId)
        {

            var message = await _messageRepostory.GetSingleAsync(m=>m.Id == messageId);
            if(message is null)
            {
                throw new MessageNotFoundByIdException("Message not found");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }
            if(!message.IsRead && user.ConnectionId is not null) 
            {
               message.IsRead = true;
               message.IsReadTime = DateTime.UtcNow;
               _messageRepostory.Update(message);
               await _messageRepostory.SaveChangesAsync();
               await Clients.Client(user.ConnectionId).SendAsync("ReceiveLastMessage",true,message.IsReadTime);
            }




        }
        public async Task SendMessagePrivate(string message, string senderId,string receiverId)
        {

            var user = await _userManager.FindByIdAsync(receiverId);
            if (user is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }
           await _messageChatService.SendMessageAsync(message,senderId, receiverId);
            if (user.ConnectionId is not null)
            {
                await Clients.Client(user.ConnectionId).SendAsync("ReceivePrivate", message);

            }

        }
        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(U=>U.ConnectionId == Context.ConnectionId);
            if(user is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }
            user.isOnline = false;
            user.LastVisit = DateTime.UtcNow;
            user.ConnectionId = null;
            await _userManager.UpdateAsync(user);

            return base.OnDisconnectedAsync(exception);
        }
        public async Task JoinGroup(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                throw new UserNotFoundByIdException("User not found");
            }
            user.ConnectionId = Context.ConnectionId;
            user.LastVisit = DateTime.UtcNow;
            user.isOnline= true;
            await _userManager.UpdateAsync(user);

          
            await Groups.AddToGroupAsync(Context.ConnectionId, user.UserName);
        }

    }
}

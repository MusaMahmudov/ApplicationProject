using AbilloLLCApplication.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AbilloLLCApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IMessageChatService _messageChatService;
        public ChatMessagesController(IMessageChatService messageChatService)
        {
            _messageChatService = messageChatService;
        }
        [HttpGet("{SenderId}/{ReceiverId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,Driver")]
        public async Task<IActionResult> GetMessages(string SenderId,string ReceiverId) 
        {
            var messages = await _messageChatService.GetChatMessagesAsync(SenderId, ReceiverId);
            return Ok(messages);
        
        }
    }
}

using AbilloLLCApplication.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AbilloLLCApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public MessagesController(IMessageService messageService, IWebHostEnvironment webHostEnviroment)
        {
            _messageService = messageService;
            _webHostEnviroment = webHostEnviroment;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public IActionResult GetAllMessages()
        {
            string SecretFilePath = Path.Combine(_webHostEnviroment.WebRootPath, "", "");
            string SecretFilePathToken = Path.Combine(_webHostEnviroment.WebRootPath, "", "");

            var messages = _messageService.GetMessageAsync(SecretFilePath,SecretFilePathToken);
            return Ok(messages);
        }
    }
}

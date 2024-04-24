using AbilloLLCApplication.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.ChatMessageDTOs
{
    public class GetChatMessagesDTO
    {
        public Guid Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? IsReadTime { get; set; }


    }
}

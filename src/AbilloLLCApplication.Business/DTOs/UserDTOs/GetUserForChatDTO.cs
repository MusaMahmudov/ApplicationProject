using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.UserDTOs
{
    public class GetUserForChatDTO
    {
        public string? lastMessage { get; set; }
        public string senderId { get; set; }
        public bool isReadLastMessage { get; set; }
        public string Id { get; set; }   
        public string UserName { get; set; }
        public DateTime LastVisit {  get; set; }
        public bool isOnline { get; set; }
        public string? ConnectionId { get; set; }
    }
}

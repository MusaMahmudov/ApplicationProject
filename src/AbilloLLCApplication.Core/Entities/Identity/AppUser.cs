using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
        public DateTime LastVisit {  get; set; }
        public Driver? driver { get; set; }
        public string? ConnectionId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool isOnline { get; set; }
        public List<Message>? SendedMessages { get; set; }
        public List<Message>? ReceiveMessages { get; set; }

        


    }
}

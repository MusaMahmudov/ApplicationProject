using AbilloLLCApplication.Core.Entities.Common;
using AbilloLLCApplication.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Core.Entities
{
    public class Message : BaseSectionEntity
    {
        public AppUser Sender { get; set; }
        public string SenderId { get; set; }
        public AppUser Receiver { get; set; }
        public string ReceiverId { get; set; }

        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime IsReadTime { get; set; }
        //public Guid ChatId { get ; set; }
        //public Chat Chat { get; set; }

    }
}

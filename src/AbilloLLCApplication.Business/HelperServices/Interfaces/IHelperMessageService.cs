using Google.Apis.Gmail.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.HelperServices.Interfaces
{
    public interface IHelperMessageService
    {
        DateTime GetMessageSentTime(GmailService service, string userId, string messageId);
    }
}

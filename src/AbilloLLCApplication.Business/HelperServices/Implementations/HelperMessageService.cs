using AbilloLLCApplication.Business.HelperServices.Interfaces;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.HelperServices.Implementations
{
    public class HelperMessageService : IHelperMessageService
    {
        public DateTime GetMessageSentTime(GmailService service, string userId, string messageId)
        {
            // Retrieve the message
            Message message = service.Users.Messages.Get(userId, messageId).Execute();

            // Extract the sent time from the message headers
            string sentTimeHeader = message.Payload.Headers.FirstOrDefault(header => header.Name == "Date")?.Value;

            // Parse the sent time
            if (DateTime.TryParse(sentTimeHeader, out DateTime sentTime))
            {
                return sentTime;
            }
            else
            {
                // Default to the current time if sent time cannot be parsed
                return DateTime.Now;
            }
        }
    }
}

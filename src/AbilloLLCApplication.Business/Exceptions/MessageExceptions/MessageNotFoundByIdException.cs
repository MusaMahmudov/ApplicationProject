using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.MessageExceptions
{
    public class MessageNotFoundByIdException : Exception, IBaseException
    {
        public string ErrorMessage { get; set; }

        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public MessageNotFoundByIdException(string message): base(message) 
        {
            ErrorMessage = message;
        }

    }
}

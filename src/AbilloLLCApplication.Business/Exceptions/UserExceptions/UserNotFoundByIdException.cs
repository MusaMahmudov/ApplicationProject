using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.UserExceptions
{
    public class UserNotFoundByIdException : Exception, IBaseException
    {
        public string ErrorMessage { get; }

        public HttpStatusCode statusCode => HttpStatusCode.NotFound;
        public UserNotFoundByIdException(string message) : base(message) 
        {
         ErrorMessage = message;
        }
    }
}

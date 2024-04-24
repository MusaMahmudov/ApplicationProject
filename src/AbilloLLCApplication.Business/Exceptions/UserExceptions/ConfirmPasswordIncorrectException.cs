using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.UserExceptions
{
    public class ConfirmPasswordIncorrectException : Exception, IBaseException
    {
        public string ErrorMessage { get; }

        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public ConfirmPasswordIncorrectException(string message) 
        {
         ErrorMessage = message;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.UserExceptions
{
    public class OldPasswordIncorrectException : Exception, IBaseException
    {
        public string ErrorMessage { get; set; }

        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public OldPasswordIncorrectException(string message) : base(message) 
        {
         ErrorMessage = message;
        }
    }
}

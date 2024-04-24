using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.SignInExceptions
{
    public class SignInException : Exception, IBaseException
    {
        public string ErrorMessage { get; }

        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public SignInException(string message) : base(message) 
        {
         ErrorMessage = message;
        }
    }
}

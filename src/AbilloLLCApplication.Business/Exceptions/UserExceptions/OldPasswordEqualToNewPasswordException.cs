using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.UserExceptions
{
    internal class OldPasswordEqualToNewPasswordException : Exception, IBaseException
    {
        public string ErrorMessage  {get;set; }

        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public OldPasswordEqualToNewPasswordException(string message) : base(message) 
        {
         ErrorMessage = message;
        }
    }
}

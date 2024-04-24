using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.RoleExceptions
{
    public class RoleNotFoundByIdException : Exception, IBaseException
    {
        public string ErrorMessage  {get;}

        public HttpStatusCode statusCode => HttpStatusCode.NotFound;
        public RoleNotFoundByIdException(string message) : base(message)
        {
            ErrorMessage = message;
            
        }
    }
}

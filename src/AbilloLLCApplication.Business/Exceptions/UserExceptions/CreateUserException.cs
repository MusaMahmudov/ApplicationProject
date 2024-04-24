using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.UserExceptions
{
    public class CreateUserException : Exception, IBaseException
    {
        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public string ErrorMessage { get;  }


        public CreateUserException(IEnumerable<IdentityError> errors)
        {
            ErrorMessage = string.Join(",", errors.Select(e=>e.Description));
        }

    }
}

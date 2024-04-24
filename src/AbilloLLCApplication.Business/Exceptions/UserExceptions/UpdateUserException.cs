using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.UserExceptions
{
    public class UpdateUserException : Exception, IBaseException
    {
        public string ErrorMessage { get; }

        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public UpdateUserException(IEnumerable<IdentityError> errors)
        {
            ErrorMessage = string.Join(",",errors.Select(e=>e.Description));
        }
    }
}

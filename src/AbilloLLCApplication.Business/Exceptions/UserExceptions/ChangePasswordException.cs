using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.UserExceptions
{
    public class ChangePasswordException : Exception, IBaseException
    {
        public string ErrorMessage { get; set; }

        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public ChangePasswordException(IEnumerable<IdentityError> Errors)
        {
            ErrorMessage = string.Join(" ",Errors.Select(e=>e.Description)); 
        }
    }
}

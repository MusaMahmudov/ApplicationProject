using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.RoleExceptions
{
    public class Admin_sRoleCannotBeChangedException : Exception, IBaseException
    {
        public string ErrorMessage { get; }

        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public Admin_sRoleCannotBeChangedException(string message) :base(message) 
        {
          ErrorMessage = message;

        }
    }

}

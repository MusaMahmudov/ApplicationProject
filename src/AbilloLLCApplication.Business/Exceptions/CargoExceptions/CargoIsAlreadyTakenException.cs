using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.CargoExceptions
{
    public class CargoIsAlreadyTakenException : Exception, IBaseException
    {
        public string ErrorMessage { get; set; }

        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public CargoIsAlreadyTakenException(string message): base(message) 
        {
         ErrorMessage = message;
        }
    }
}

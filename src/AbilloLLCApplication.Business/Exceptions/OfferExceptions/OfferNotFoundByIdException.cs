using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.OfferExceptions
{
    public class OfferNotFoundByIdException : Exception, IBaseException
    {
        public string ErrorMessage { get; set; }

        public HttpStatusCode statusCode => HttpStatusCode.NotFound;
        public OfferNotFoundByIdException(string message)
        {
         ErrorMessage = message;
        }

    }
}

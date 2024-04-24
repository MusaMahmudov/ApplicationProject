using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.OfferExceptions
{
    public class OfferAlreadyHasResponseException : Exception, IBaseException
    {
        public string ErrorMessage { get; set; }

        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public OfferAlreadyHasResponseException(string message) : base(message) 
        {
            ErrorMessage = message;
        }
    }
}

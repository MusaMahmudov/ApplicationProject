
using System.Net;


namespace AbilloLLCApplication.Business.Exceptions.OfferExceptions
{
    public class OfferAlreadyExistsException : Exception, IBaseException
    {
        public string ErrorMessage { get; set; }

        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public OfferAlreadyExistsException(string message)  : base(message)
        { 
        ErrorMessage = message;
        }
    }
}

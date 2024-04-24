using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions
{
    public interface IBaseException
    {
        public string ErrorMessage { get; }
        public HttpStatusCode statusCode { get; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.Common
{
    public class ResponseDTO
    {
        public string Message { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public ResponseDTO(string message, HttpStatusCode httpStatusCode)
        {
            Message = message;
            HttpStatusCode = httpStatusCode;
        }
    }
}

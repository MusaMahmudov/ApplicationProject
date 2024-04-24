﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Exceptions.DriverExceptions
{
    public class UserHasToBeDriverException : Exception, IBaseException
    {
        public string ErrorMessage { get; }
        public HttpStatusCode statusCode => HttpStatusCode.BadRequest;
        public UserHasToBeDriverException(string message) : base(message) 
        {
            ErrorMessage = message;
        }
    }
}

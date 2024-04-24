using AbilloLLCApplication.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.HelperServices.Interfaces
{
    public interface IMailService
    {

        Task SendEmail(MailRequestDTO mailRequest);
    }
}

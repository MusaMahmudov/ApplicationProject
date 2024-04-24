using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.UserDTOs
{
    public class GetUserForUpdateDTO
    {
        public string UserName { get; set; }
        public string Fullname { get; set; }
        
        public string Email { get; set; }
        
        public IList<string> RolesId { get; set; }
    }
}

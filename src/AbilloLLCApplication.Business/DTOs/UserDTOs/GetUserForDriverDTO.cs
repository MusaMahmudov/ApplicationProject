using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.UserDTOs
{
    public class GetUserForDriverDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public DateTime LastVisit { get; set; }
       
    }
}

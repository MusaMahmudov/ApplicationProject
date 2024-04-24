using AbilloLLCApplication.Business.DTOs.UserDTOs;
using AbilloLLCApplication.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.DriversDTOs
{
    public class GetDriverDTO
    {
        public Guid driverId { get; set; }
        public GetUserForDriverDTO? appUser { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public string? Zipcode { get; set; }
        public string? CurrentLocation { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string PhoneNumber { get; set; }
        public string? TelegramUserName { get; set; }
        public string? TelegramUserId { get; set; }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.DriversDTOs
{
    public class GetDriverForOfferDTO
    {
        public Guid DriverId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
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

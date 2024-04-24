using AbilloLLCApplication.Core.Entities.Common;
using AbilloLLCApplication.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Core.Entities
{
    public class Driver : BaseSectionEntity
    {      
        public string PhoneNumber { get; set; }
        public AppUser? AppUser { get; set; }
        public string? AppUserId { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public string? Zipcode { get; set; }
        public string? CurrentLocation { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public IList<Offer>? Offers { get; set; }
        public string? TelegramUserName { get; set; }
        public string? TelegramUserId { get; set; }


    }
}

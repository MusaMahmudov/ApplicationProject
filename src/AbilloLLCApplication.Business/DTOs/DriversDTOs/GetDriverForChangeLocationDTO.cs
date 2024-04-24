using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.DriversDTOs
{
    public class GetDriverForChangeLocationDTO
    {
        public string? Zipcode { get; set; }
        public string? CurrentLocation { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}

using AbilloLLCApplication.Business.DTOs.CargoDTOs;
using AbilloLLCApplication.Business.DTOs.DriversDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.OfferDTOs
{
    public class GetOfferDTO
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public decimal WithPercent { get; set; }
        public bool? Accepted { get; set; }
        public GetDriverForOfferDTO Driver { get; set; }
        public GetCargoForOfferDTO Cargo { get; set; }
    }
}

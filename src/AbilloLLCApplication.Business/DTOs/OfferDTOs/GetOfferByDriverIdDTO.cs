using AbilloLLCApplication.Business.DTOs.CargoDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.OfferDTOs
{
    public class GetOfferByDriverIdDTO
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public bool? Accepted { get; set; }
        public GetCargoForOfferDTO Cargo { get; set; }

    }
}

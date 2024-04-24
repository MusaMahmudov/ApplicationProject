using AbilloLLCApplication.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.OfferDTOs
{
    public class PostOfferDTO
    {
        public decimal Price { get; set; }
        public Guid DriverId { get; set; }
        public Guid CargoId { get; set; }
    }
}

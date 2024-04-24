using AbilloLLCApplication.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Core.Entities
{
    public class Offer : BaseSectionEntity
    {
        public decimal Price { get; set; }
        public decimal WithPercent { get; set; }
        public bool? Accepted { get; set; } 
        public Driver Driver { get; set; }
        public Guid DriverId { get; set; }
        public Cargo Cargo { get; set; }
        public Guid CargoId { get; set;}

    }
}

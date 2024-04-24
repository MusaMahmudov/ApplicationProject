using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.CargoDTOs
{
    public class PostCargoDTO
    {
        public int Miles { get; set; }
        public int Pieces { get; set; }
        public int Weight { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public string PickUpZipcode { get; set; }
        public string DeliverToZipcode { get; set; }
        public string Notes { get; set; }
    }
}

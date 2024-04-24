using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.DriversDTOs
{
    public class GetDriverForChangeDimensionsDTO
    {
        public Guid Id { get; set; }
        public string userName { get; set; }
        public double Length { get; set; }
    }
}

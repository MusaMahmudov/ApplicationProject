using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.MessagesDTOs
{
    public class GetMessageDTO
    {

        public double? Length { get; set; }

        public string extractedText { get; set; }
        public double? Width{ get; set; }
        public double? Height { get; set; }




        public string CheckId { get; set; }
        public int? Miles { get; set; }
        public bool hasMiles { get; set; }



        public int? Pieces { get; set; }
        public bool hasPieces { get; set; }

        public int? Weight { get; set; }
        public bool HasWeight { get; set; }


        public string? PickUpZipcode { get; set; }
        public string? PickUpCity { get; set; }
        public bool HasPickUpAtZipcode { get; set; }

        public string? Longitude { get; set; }
        public string? Latitude { get; set; }

        public string? DeliverToZipcode { get; set; }
        public string? DeliverToCity { get; set; }
        public bool HasDeliverToZipcode { get; set; }

        public string? FromEmail { get; set; }
        public string? ToEmail { get; set; }
        public string Notes {  get; set; }
        public bool HasNotes { get; set; }



    }
}

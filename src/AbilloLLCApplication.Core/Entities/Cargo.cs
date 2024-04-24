﻿using AbilloLLCApplication.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Core.Entities
{
    public class Cargo : BaseSectionEntity
    {
        public string checkId { get; set; }
        public bool IsTaken { get; set; }
        public int? Miles { get; set; }
        public int? Pieces { get; set; }
        public int? Weight { get; set; }

        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public string? PickUpZipcode { get; set; }
        public string? PickUpCity { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? FromEmail { get; set; }
        public string? ToEmail { get; set; }
        public string? DeliverToZipcode { get; set; }
        public string? DeliverToCity { get; set; }
        public string? Notes { get; set; }
        public IList<Offer>? Offers { get; set; }

    }
}

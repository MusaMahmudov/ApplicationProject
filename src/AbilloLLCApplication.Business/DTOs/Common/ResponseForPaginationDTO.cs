using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.Common
{
    public class ResponseForPaginationDTO<T> 
    {
       public  List<T> Items { get; set; }    
        public int PageNumber { get; set; }
        public double Latitude{ get; set; }
        public double Longitude { get; set; }
        public ResponseForPaginationDTO(List<T> items,int pageNumber,double latitude,double longitude) 
        {
            Longitude = longitude; 
            Latitude = latitude;
           Items = items;
            PageNumber = pageNumber;
        }
    }
}

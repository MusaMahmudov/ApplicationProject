using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.PaginationParameters
{
    public class CargoParameters
    {
        public const int MaxPageSize = 50;
        public int CurrentPage { get; set; } = 1;
    }
}

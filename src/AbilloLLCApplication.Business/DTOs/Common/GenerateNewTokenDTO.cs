using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.DTOs.Common
{
    public class GenerateNewTokenDTO
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}

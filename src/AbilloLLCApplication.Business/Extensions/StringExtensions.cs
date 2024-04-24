using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Extensions
{
    public static class StringExtensions
    {
        public static string ExtractNumbers(this string message)
        {
            Regex regex = new Regex(@"\d+");
            MatchCollection matches = regex.Matches(message);

            string[] numbers = new string[matches.Count];
            string result = "";
            for (int i = 0; i < matches.Count; i++)
            {
                numbers[i] = matches[i].Value;
            }
            for(int i = 0; i < numbers.Length; i++)
            {
                result += numbers[i];
            }
            return result;
        } 
    }
}

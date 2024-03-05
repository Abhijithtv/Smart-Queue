using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Common.Library.Helpers
{
    public class StringHelper
    {
        public static string Between(string fullString, string start, string end)
        {
            int Pos1 = fullString.IndexOf(start) + start.Length;
            int Pos2 = fullString.IndexOf(end);

            return (Pos2 > Pos1) ? fullString.Substring(Pos1, Pos2 - Pos1) : string.Empty;
        }

        public static string GroupDataEncoder(string key, string value)
        {
           
            return Environment.NewLine + KeyEncoder(key) +  Environment.NewLine + value;
        }

        public static string KeyEncoder(string key)
        {
            return Resource.KeyStart + key + Resource.KeyEnd;
        }
    }
}

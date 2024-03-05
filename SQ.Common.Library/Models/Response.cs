using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Common.Library.Models
{
    public class Response
    {
        public string Result { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}

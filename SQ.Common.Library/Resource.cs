using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Common.Library
{
    public class Resource
    {

        public static readonly string KeyStart = "@#key*start";
        public static readonly string KeyEnd = "@#key*end";
        public static readonly string ValueStart = "@#Value*start";
        public static readonly string ValueEnd = "@#Value*end";
        public static readonly string GroupStart = "@#Group*start";
        public static readonly string GroupEnd = "@#Group*end";
        public enum RequestType {Insert = 1, Read = 2 , Invalid = 0}

        public static int TotalValidReq_Count = 2;
        public enum StatusCode { BadRequestType = 501, BadKey = 502, BadRequest = 503, Success =200 }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.DTO
{
    public class ResponseBase
    {
        public int total { get; set; }

        public int code { get; set; }

        public string msg { get; set; } 
    }
}

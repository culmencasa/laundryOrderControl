using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.DTO
{
    public class MemberReceiveBagOrderRequest : RequestBase
    {
        public Data data  { get; set; }
        public class Data
        {
            public string clCode { get; set; }
        }
    }
}

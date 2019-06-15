using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.DTO
{
    public class FactoryReceiveBagOrdersRequest : RequestBase
    {
        public FactoryReceiveBagOrdersRequest()
        {
            cmd = "factory/factoryReceiveBagOrders.do";
            version = 1;
        }

        public Data data { get; set; }
        public class Data
        {
            /// <summary>
            /// 芯片编号
            /// </summary>
            public string clCode { get; set; }
        }
    }
}

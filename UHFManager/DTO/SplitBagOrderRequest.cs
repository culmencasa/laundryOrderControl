using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.DTO
{
    public class SplitBagOrderRequest : RequestBase
    {
        public Data data { get; set; }
        public class Data
        {
            /// <summary>
            /// 订单芯片编号
            /// </summary>
            public string clCode { get; set; }

            /// <summary>
            /// 子芯片编号列表
            /// </summary>
            public List<String> clCodeList { get; set; }

        } 
    }
}

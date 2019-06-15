using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.DTO
{
    public class RecorganizationBagOrderRequest : RequestBase
    {
        public Data data { get; set; }
        public class Data
        {
            /// <summary>
            /// 新洗衣袋芯片编号
            /// </summary>
            public string clCode { get; set; }

            /// <summary>
            /// 子芯片编号列表
            /// </summary>
            public List<string> clCodeList { get; set; }

            /// <summary>
            /// 单号
            /// </summary>
            public int bagOrderId { get; set; }
                  
        }
    }
}

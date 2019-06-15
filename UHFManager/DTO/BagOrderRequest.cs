using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.DTO
{
    /// <summary>
    /// 通过洗衣袋子单获取订单信息
    /// </summary>
    public class BagOrderRequest : RequestBase
    {
        public Data data { get; set; }

        public class Data
        {
            /// <summary>
            /// 衣袋芯片编号
            /// </summary>
            public List<string> clCodeList { get; set; } = new List<string>(); 
        }       
        
    }
}

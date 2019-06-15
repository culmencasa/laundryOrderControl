using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.DTO
{
    /// <summary>
    /// 通过洗衣袋子单获取订单信息
    /// </summary>
    public class BagOrderResponse : ResponseBase
    {
        public List<Data> data { get; set; }
        public class Data
        {
            /// <summary>
            /// 子订单芯片
            /// </summary>
            public string itemClCodeStr { get; set; }

            /// <summary>
            /// 下单区域
            /// </summary>
            public string houseName { get; set; }

            /// <summary>
            /// 子订单数量
            /// </summary>
            public int bagOrdersItemCount { get; set; }


            /// <summary>
            /// 子订单芯片
            /// </summary>
            public string bagOrdersItemClCode { get; set; }

            /// <summary>
            /// 会员名称
            /// </summary>
            public string memberName { get; set; }

            /// <summary>
            /// 订单编号
            /// </summary>
            public int bagOrderId { get; set; }
        }
    }
}

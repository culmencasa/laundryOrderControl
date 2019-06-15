using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.POCO
{
    public class OrderTag
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// 标签Id
        /// </summary>
        public string TagSN { get; set; }

        /// <summary>
        /// 读取次数
        /// </summary>
        public int ReadTimes { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 下单区域
        /// </summary>
        public string OrderArea { get; set; }
    }
}

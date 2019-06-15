using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.DTO
{
    /// <summary>
    /// 请求被套状态修改
    /// </summary>
    public class UpdateQuiltOrderStatusRequest : RequestBase
    {
        public UpdateQuiltOrderStatusRequest()
        {
            cmd = "factory/updateQuiltOrderStatus.do";
            version = 1;
        }

        public Data data { get; set; }

        /// <summary>
        /// 数据参数
        /// </summary>
        public class Data
        {
            /// <summary>
            /// 被套芯片编号
            /// </summary>
            public string clCode { get; set; }
            /// <summary>
            /// 状态[1 - 送达工厂；2 - 开始清洗；3 - 清洗完成；4 - 开始熨烫；5 - 熨烫完成；6 - 完成]
            /// </summary>
            public int status { get; set; }
        }
    } 
}

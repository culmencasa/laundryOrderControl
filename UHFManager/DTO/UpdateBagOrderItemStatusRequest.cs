using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.DTO
{
    public class UpdateBagOrderItemStatusRequest : RequestBase
    {
        public UpdateBagOrderItemStatusRequest()
        {
            cmd = "factory/updateBagOrderItemStatus.do";
            version = 1;
        }

        public Data data { get; set; }
        public class Data
        {
            /// <summary>
            /// 子订单芯片编号
            /// </summary>
            public string clCode { get; set; }

            /// <summary>
            /// 修改状态[1 - 清洗中；2 - 清洗完成；3 - 清洗完成；4 - 开始熨烫；5 - 熨烫完成]
            /// </summary>
            public int status { get; set; }
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.POCO
{
    public class TaskItem
    {
        public string UID { get; set; }

        /// <summary>
        /// 主芯片
        /// </summary>
        public string TagCode { get; set; }


        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderSN { get; set; }

        
        /// <summary>
        /// 当前状态
        /// </summary>
        public Dictionary<TaskStates,DateTime?> StatesLog { get; set; }

        /// <summary>
        /// 拆单芯片
        /// </summary>
        public List<SubTaskItem> Children { get; set; }


        /// <summary>
        /// 类型 0 衣物 1 被套
        /// </summary>
        public int GoodsType { get; set; }

        public TaskItem()
        { 
            UID = Guid.NewGuid().ToString("N");

            StatesLog = new Dictionary<TaskStates, DateTime?>();
            StatesLog.Add(TaskStates.Enter, null);
            StatesLog.Add(TaskStates.Wash, null);
            StatesLog.Add(TaskStates.Dry, null);
            StatesLog.Add(TaskStates.Exit, null);

            Children = new List<SubTaskItem>();
        }
    }

}

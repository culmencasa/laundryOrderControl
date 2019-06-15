using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.POCO
{
    public class SubTaskItem
    {
        public string ParentUID { get; set; }

        public string UID { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }


        public SubTaskItem()
        {

        }

    }
}

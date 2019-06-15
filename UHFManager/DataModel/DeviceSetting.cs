using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using UHFManager.POCO;

namespace UHFManager.DataModel
{
    
    public class DeviceSetting : EntityBase
    {
        public DeviceUsageEnum Usage { get; set; }
        public string DeviceIpAddress { get; set; }
        public string DevicePort { get; set; }

        [NotMapped]
        public Object Tag { get; set; }
    }

}

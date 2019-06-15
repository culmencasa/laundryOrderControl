using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UHFManager.DataModel;

namespace UHFManager.DataContext
{
    class DeviceSettingService :BusinessService
    {
        public IList<DeviceSetting> Load()
        {
            List<DeviceSetting> settingList = new List<DeviceSetting>();
            UsingGlobalDb((context) =>
            {
                settingList = context.DeviceSettings.ToList();
            });

            return settingList;
             
        }

        public DeviceSetting SaveOrUpdate(DeviceSetting entity)
        {
            DeviceSetting returnObject = null;

            UsingGlobalDb((context) => {
                DeviceSetting overridingSetting = context.DeviceSettings.FirstOrDefault(a => a.Usage == entity.Usage);
                if (overridingSetting != null)
                {
                    overridingSetting.DeviceIpAddress = entity.DeviceIpAddress;
                    overridingSetting.DevicePort = entity.DevicePort;

                    context.SaveChanges();

                    // 返回对象 
                    returnObject = overridingSetting;
                }
                else
                {
                    context.DeviceSettings.Add(entity);
                    context.SaveChanges();

                    returnObject = entity;
                }
            });

            return returnObject;
        }
    }
}

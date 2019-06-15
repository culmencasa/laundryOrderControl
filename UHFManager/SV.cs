using DLL;
using NLog;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UHFManager.DataContext;
using UHFManager.DataModel;
using UHFManager.POCO;
using UHFManager.REST;

namespace UHFManager
{
    internal static class SV // Static Variable
    {
        /// <summary>
        /// 表示多台用途设备的字典
        /// </summary>
        public static Dictionary<DeviceSetting, DeviceProxy> DeviceDictonary = new Dictionary<DeviceSetting, DeviceProxy>();

        /// <summary>
        /// 任务列表
        /// </summary>
        public static ConcurrentDictionary<string, TaskItem> TaskQueue { get; set; } = new ConcurrentDictionary<string, TaskItem>();

        /// <summary>
        /// 接口访问
        /// </summary>
        public static RestInterfaceAccess Ria = new RestInterfaceAccess(ConfigurationManager.AppSettings["RemoteEntry"]);

        /// <summary>
        /// 获取业务类的单例
        /// </summary>
        /// <typeparam name="Service"></typeparam>
        /// <returns></returns>
        public static Service DbEntry<Service>() where Service : BusinessService, new()
        {
            Service instance = default(Service);

            string key = typeof(Service).FullName;
            lock (_controllerPool)
            {
                if (_controllerPool.ContainsKey(key))
                {
                    instance = (Service)_controllerPool[key];
                }
                else
                {
                    instance = new Service();
                    _controllerPool.Add(key, instance);
                }
            }

            return instance;
        }


        static SV()
        {
            try
            {
                // 从数据库加载配置
                var list = SV.DbEntry<DeviceSettingService>().Load();
                if (list.Count > 0)
                {
                    foreach (var setting in list)
                    {
                        SV.DeviceDictonary.Add(setting, new POCO.DeviceProxy());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }




        public static Logger Logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 用于缓存Service类
        /// </summary>
        private static Hashtable _controllerPool = new Hashtable();
    }

}

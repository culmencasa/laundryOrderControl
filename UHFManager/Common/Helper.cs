using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using UHFManager.DataModel;
using UHFManager.POCO;

namespace UHFManager
{
    static class Helper
    {
        public static bool IsSettingValid(this DeviceSetting setting)
        {
            IPAddress testIP = IPAddress.Any;
            if (!IPAddress.TryParse(setting.DeviceIpAddress, out testIP))
            {
                return false;
            }

            return IsIPAddressValid(testIP) &&  IsPortValid(setting.DevicePort);
        }


        /// <summary>
        /// 检查IP
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsIPAddressValid(IPAddress target)
        {
            if (target == null)
                return false;

            if (target.Equals(IPAddress.Any))
                return false;

            if (target.Equals(IPAddress.Broadcast))
                return false;

            if (target.Equals(IPAddress.None))
                return false;

            if (target.Equals(IPAddress.Loopback))
                return false;

            return true;
        }



        /// <summary>
        /// 检查端口
        /// </summary>
        /// <param name="portString"></param>
        /// <returns></returns>
        public static bool IsPortValid(string portString)
        {
            int portValue = 0;
            if (!Int32.TryParse(portString, out portValue))
                return false;

            if (portValue < 0 || portValue > 65535)
            {
                return false;
            }

            return true;
        }



        /// <summary>
        /// 获取窗体的方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="createIfNotFound"></param>
        /// <param name="titleFilter"></param>
        /// <returns></returns>
        public static T GetForm<T>(this Form context, bool createIfNotFound = false, string titleFilter = null) where T : Form, new()
        {
            T target = default(T);
            foreach (Form window in Application.OpenForms)
            {
                if (window is T)
                {
                    if (titleFilter != null && window.Text != titleFilter)
                    {
                        continue;
                    }
                    target = window as T;
                    break;
                }
            }

            if (target == null && createIfNotFound)
            {
                target = new T();
            }

            return target;
        }


    }
}

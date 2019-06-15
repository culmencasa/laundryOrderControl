using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UHFManager.DTO;
using UHFManager.REST;

namespace UHFManager
{
    static class Program
    { 
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
                       
            Init();

            Application.Run(new MainForm());
        }



        public static void Init()
        {
            //using (var db = new GlobalDbContext())
            //{
            //    db.DeviceSettings.Add(new DataModel.DeviceSetting()
            //    {
            //        DeviceIpAddress = "192.168.1.200",
            //        DevicePort = "100",
            //        Usage = POCO.DeviceUsageEnum.入库
            //    });

            //    db.SaveChanges();
            //}
        }
    }
}

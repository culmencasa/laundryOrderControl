using NLog.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UHFManager.DTO;
using UHFManager.Interface;
using UHFManager.POCO;

namespace UHFManager
{
    public partial class WashingForm : Form, IDeviceForm
    {
        #region 静态成员

        private static object myLocker = new object();

        #endregion

        #region 字段

        DeviceProxy _deviceProxy;

        public List<OrderTag> testData { get; set; } = new List<OrderTag>();

        #endregion

        #region 属性

        public DeviceUsageEnum Function
        {
            get
            {
                return DeviceUsageEnum.洗涤;
            }
        }
        #endregion


        #region 构造

        public WashingForm()
        {
            InitializeComponent();
            //RichTextBoxTarget.ReInitializeAllTextboxes(this);

            this.Load += WashingForm_Load;
        }

        #endregion

        #region 窗体控件事件处理

        private void WashingForm_VisibleChanged(object sender, EventArgs e)
        {
            CheckDeviceStatus();
        }

        private void WashingForm_Load(object sender, EventArgs e)
        {
            this.testClassBindingSource.DataSource = testData;
        }

        private void btnTryConnect_Click(object sender, EventArgs e)
        {
            var mainForm = this.GetForm<MainForm>();
            if (mainForm != null)
            {
                mainForm.ConnectDevice2();
            }
        }

        private void btnGotoSetting_Click(object sender, EventArgs e)
        {
            var mainForm = this.GetForm<MainForm>();
            if (mainForm != null)
            {
                mainForm.LoadSettingForm(true);
            }
        }

        #endregion

        #region 后台线程

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgw = sender as BackgroundWorker;

            var deviceCache = SV.DeviceDictonary.FirstOrDefault(p => p.Key.Usage == Function);
            DeviceProxy device = deviceCache.Value;

            while (true)
            {
                if (bgw.CancellationPending)
                    break;

                device.ClearIDBuffer();
                List<string> tags = device.EPCMultiTagInventory();

                foreach (string tagCode in tags)
                {
                    TaskItem item = null;
                    if (!SV.TaskQueue.ContainsKey(tagCode))
                    {
                        // test
                        item = new TaskItem()
                        {
                            TagCode = tagCode,
                            GoodsType = 1, //todo: 以后做判断
                            OrderSN = "ORDER_" + DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo)
                        };
                        SV.TaskQueue.TryAdd(tagCode, item);

                        //bgw.ReportProgress(2, item);
                    }
                    else
                    {
                        item = SV.TaskQueue[tagCode];
                        if (item.StatesLog[TaskStates.Wash] != null)
                        {
                            // 表示已走过这个流程
                        }
                        else
                        {
                            bgw.ReportProgress(2, item);
                        }
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 2:
                    {
                            TaskItem taskItem = e.UserState as TaskItem;
                            if (testData.Count(p => p.TagSN == taskItem.TagCode) == 0)
                            {
                                testData.Add(new OrderTag()
                                {
                                    BusinessId = taskItem.OrderSN,
                                    OrderArea = "武汉",
                                    ReadTimes = 2,
                                    TagSN = taskItem.TagCode,
                                    UserName = "张三"
                                });
                            }


                            // 标记“已入厂”
                            Task.Factory.StartNew(() =>
                            {
                                if (taskItem.StatesLog[TaskStates.Wash] == null)
                                {
                                    lock (taskItem) // double  check
                                    {
                                        if (taskItem.StatesLog[TaskStates.Wash] == null)
                                        {
                                            if (taskItem.GoodsType == 0)
                                            {
                                                CallLaundryBagUpdating(taskItem);
                                            }
                                            else
                                            {
                                                CallBlanketCoverUpdating(taskItem);
                                            }
                                        }
                                    }
                                }
                            });
                        

                    }
                    break;
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        #endregion

        #region 外部委托处理
        
        #endregion

        #region 公开方法

        public void DoWork()
        {
            if (!bgWorker.IsBusy)
            {
                CheckDeviceStatus();

                bgWorker.RunWorkerAsync();
            }
        }

        public void StopWork()
        {
            if (bgWorker.IsBusy)
            {
                bgWorker.CancelAsync();
            }
        }

        #endregion

        #region 私有方法

        private void CallLaundryBagUpdating(TaskItem taskItem)
        {
            var request = new UpdateBagOrderItemStatusRequest()
            {
                data = new UpdateBagOrderItemStatusRequest.Data()
                {
                    clCode = taskItem.TagCode,
                    status = 2
                }
            };

            var response = SV.Ria.Post<UpdateBagOrderItemStatusRequest, UpdateBagOrderItemStatusResponse>(request);
            if (response?.code == 1)
            {
                taskItem.StatesLog[TaskStates.Wash] = DateTime.Now;
#if DEBUG
                Console.WriteLine("订单状态更新成功。");
#endif
            }
            else
            {
                //SV.Logger.Info(response?.msg);
            }
        }
        
        private void CallBlanketCoverUpdating(TaskItem taskItem)
        {
            var request = new UpdateQuiltOrderStatusRequest()
            {
                data = new UpdateQuiltOrderStatusRequest.Data()
                {
                    clCode = taskItem.TagCode,
                    status = 2 
                }
            };

            var response = SV.Ria.Post<UpdateQuiltOrderStatusRequest, UpdateQuiltOrderStatusResponse>(request);
            if (response?.code == 1)
            {
                taskItem.StatesLog[TaskStates.Wash] = DateTime.Now;
#if DEBUG
                Console.WriteLine("订单状态更新成功。");
#endif
            }
            else
            {

            }
        }
        /// <summary>
        /// 检查设备状况
        /// </summary>
        private void CheckDeviceStatus()
        {
            var item = SV.DeviceDictonary.FirstOrDefault(p => p.Key.Usage == Function);
            if (!item.Key.IsSettingValid())
            {
                pnlTopHint.Visible = true;
                lblHint.Text = $"未正确配置{Function.ToString()}设备。";
            }
            else
            {
                if (_deviceProxy == null)
                {
                    _deviceProxy = item.Value;
                }
                if (!_deviceProxy.IsConnected)
                {
                    pnlTopHint.Visible = true;
                    lblHint.Text = "设备已配置，但目前未建立连接。";
                }
                else
                {
                    pnlTopHint.Visible = false;
                }
            }
        }

        #endregion
    }
}

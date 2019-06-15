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
    public partial class ExWarehouseForm : Form, IDeviceForm
    {
        #region 静态成员

        private static object myLocker = new object();

        #endregion

        #region 字段

        DeviceProxy _deviceProxy;

        public ObservableCollection<OrderTag> testData { get; set; } = new ObservableCollection<OrderTag>();

        #endregion

        #region 属性

        public DeviceUsageEnum Function
        {
            get
            {
                return DeviceUsageEnum.出库;
            }
        }
        #endregion

        #region 构造

        public ExWarehouseForm()
        {
            InitializeComponent();

            //RichTextBoxTarget.ReInitializeAllTextboxes(this);

            this.Load += ExWarehouseForm_Load;
            this.VisibleChanged += ExWarehouseForm_VisibleChanged;
            this.dgvMainOrder.CellContentClick += dgvMainOrder_CellContentClick;
        }
         

        #endregion

        #region 窗体控件事件处理

        private void ExWarehouseForm_Load(object sender, EventArgs e)
        {
            dgvMainOrder.RowStateChanged += DgvMainOrder_RowStateChanged;

            this.testClassBindingSource.DataSource = testData;
        }

        private void ExWarehouseForm_VisibleChanged(object sender, EventArgs e)
        {
            CheckDeviceStatus();
        }

        private void DgvMainOrder_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }
        
        private void dgvMainOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dgvMainOrder.Columns[e.ColumnIndex];
                if (column is DataGridViewButtonColumn)
                {
                    // 查看
                }
            }
        }

        private void btnTryConnect_Click(object sender, EventArgs e)
        {
            var mainForm = this.GetForm<MainForm>();
            if (mainForm != null)
            {
                mainForm.ConnectDevice4();
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
                    TaskItem item = new TaskItem()
                    {
                        TagCode = tagCode,
                        GoodsType = 1,

                    };

                    if (!SV.TaskQueue.ContainsKey(tagCode))
                    {
                        SV.TaskQueue.TryAdd(tagCode, item);

                        //bgw.ReportProgress(2, item);

#if DEBUG
                        Console.WriteLine($"{Function.ToString()} ： " + tagCode);
#endif

                    }
                    else
                    {

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
                                BusinessId = "ORDER_" + DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo),
                                OrderArea = "武汉",
                                ReadTimes = 1,
                                TagSN = taskItem.TagCode,
                                UserName = "张三"
                            });
                        }


                        // 标记“已出库”
                        if (taskItem.StatesLog[TaskStates.Dry] == null)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                lock (taskItem)
                                {
                                    if (taskItem.StatesLog[TaskStates.Dry] == null)
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
                            });
                        }
                        

                    }
                    break;
            }
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
                    status = 5
                }
            };

            var response = SV.Ria.Post<UpdateBagOrderItemStatusRequest, UpdateBagOrderItemStatusResponse>(request);
            if (response?.code == 1)
            {
                taskItem.StatesLog[TaskStates.Exit] = DateTime.Now;
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
                    status = 6
                }
            };

            var response = SV.Ria.Post<UpdateQuiltOrderStatusRequest, UpdateQuiltOrderStatusResponse>(request);
            if (response?.code == 1)
            {
                taskItem.StatesLog[TaskStates.Exit] = DateTime.Now;
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
            var item = SV.DeviceDictonary.FirstOrDefault(p => p.Key.Usage == POCO.DeviceUsageEnum.洗涤);
            if (!item.Key.IsSettingValid())
            {
                pnlTopHint.Visible = true;
                lblHint.Text = "未正确配置出库设备。";
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
                    lblHint.Text = "设备已配置，但目前尚未开始工作。";
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

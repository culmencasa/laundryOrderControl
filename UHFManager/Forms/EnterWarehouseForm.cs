using NLog.Windows.Forms;
using System;
using System.Collections.Concurrent;
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
    public partial class EnterWarehouseForm : Form, IDeviceForm
    {
        #region 静态成员

        private static object myLocker = new object();

        #endregion

        #region 字段

        DeviceProxy _deviceProxy;

        public IList<OrderTag> testData { get; set; } = new List<OrderTag>();
        public DeviceUsageEnum Function { get => DeviceUsageEnum.入库; }

        #endregion

        #region 构造

        public EnterWarehouseForm()
        {
            InitializeComponent();

            //RichTextBoxTarget.ReInitializeAllTextboxes(this);

            this.VisibleChanged += EnterWarehouseForm_VisibleChanged;
        }

        #endregion

        #region 窗体控件事件处理

        private void EnterWarehouseForm_VisibleChanged(object sender, EventArgs e)
        {
            // 在窗体显示时(form嵌入其他控件时Activated事件无效), 检查连接状况 
            CheckDeviceStatus();
        }

        private void EnterWarehouseForm_Load(object sender, EventArgs e)
        {
            dgvMainOrder.RowStateChanged += DgvMainOrder_RowStateChanged;

            this.testClassBindingSource.DataSource = testData;


            //this.dgvMainOrder.DataSource = testData;
        }

        private void EnterWarehouseForm_Activated(object sender, EventArgs e)
        {
        }

        private void EnterWarehouseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 关闭后台，断开连接
            StopWork();
        }

        private void btnTryConnect_Click(object sender, EventArgs e)
        {
            var mainForm = this.GetForm<MainForm>();
            if (mainForm != null)
            {
                mainForm.ConnectDevice1();
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

        private void DgvMainOrder_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }

        private void dgvMainOrder_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgvMainOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dgvMainOrder.Columns[e.ColumnIndex];
                if (column is DataGridViewButtonColumn)
                {
                    // 拆单
                    OrderSplitForm form = new OrderSplitForm();
                    form.ShowDialog();
                }
            }
        }

        #endregion

        #region 公开方法

        public void DoWork()
        {
            if (!bgwEnterWarehouse.IsBusy)
            {
                CheckDeviceStatus();

                bgwEnterWarehouse.RunWorkerAsync();
            }
        }

        public void StopWork()
        {
            if (bgwEnterWarehouse.IsBusy)
            {
                bgwEnterWarehouse.CancelAsync();
            }
        }

        #endregion

        #region 外部委托处理

        #endregion


        #region 后台线程

        private void bgwEnterWarehouse_DoWork(object sender, DoWorkEventArgs e)
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
                        GoodsType = 1, //todo: 以后做判断
                        OrderSN = "ORDER_" + DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo)
                    };

                    // 新订单
                    if (!SV.TaskQueue.ContainsKey(tagCode))
                    {
                        SV.TaskQueue.TryAdd(tagCode, item);

                        bgw.ReportProgress(2, item);

#if DEBUG
                        Console.WriteLine($"{Function.ToString()}： " + tagCode);
#endif

                    }
                    // 非新订单
                    else
                    {
                        item = SV.TaskQueue[tagCode];
                        if (item.StatesLog[TaskStates.Enter] != null)
                        {
                            // 表示已走过这个流程
                        }
                        else
                        {
                            //todo: 测试时暂时关闭
                            //bgw.ReportProgress(2, item);
                        }
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private void bgwEnterWarehouse_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 2:
                    {
                        lock (myLocker)
                        {
                            TaskItem taskItem = e.UserState as TaskItem;
                            
                            //todo: 从接口拿订单详情

                            if (testData.Count(p => p.TagSN == taskItem.TagCode) == 0)
                            {
                                testData.Add(new OrderTag()
                                {
                                    BusinessId = taskItem.OrderSN,
                                    OrderArea = "武汉",
                                    ReadTimes = 1,
                                    TagSN = taskItem.TagCode,
                                    UserName = "张三"
                                });
                                testClassBindingSource.ResetBindings(false);
                            }



                            // 标记“已入厂”
                            if (taskItem.StatesLog[TaskStates.Enter] == null)
                            {
                                Task.Factory.StartNew(() =>
                                {
                                    lock (taskItem)
                                    {
                                        if (taskItem.StatesLog[TaskStates.Enter] == null)
                                        {
                                            if (taskItem.GoodsType == 0)
                                            {
                                                CallFactoryReceive(taskItem);
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

                    }
                    break;
            }
        }

        private void bgwEnterWarehouse_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        #endregion


        #region 私有方法

        private void CallFactoryReceive(TaskItem taskItem)
        {
            var response = SV.Ria.Post<FactoryReceiveBagOrdersRequest, FactoryReceiveBagOrdersResponse>(new FactoryReceiveBagOrdersRequest()
            {
                data = new FactoryReceiveBagOrdersRequest.Data() { clCode = taskItem.TagCode }
            });
            if (response != null && response.code == 1)
            {
                taskItem.StatesLog[TaskStates.Enter] = DateTime.Now;
                SV.Logger.Info($"订单 - {taskItem.TagCode} 已入厂。");

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
                    status = 1
                }
            };

            var response = SV.Ria.Post<UpdateQuiltOrderStatusRequest, UpdateQuiltOrderStatusResponse>(request);
            if (response?.code == 1)
            {
                taskItem.StatesLog[TaskStates.Enter] = DateTime.Now;
                SV.Logger.Info($"订单 - {taskItem.TagCode} 已入厂。");
#if DEBUG
                Console.WriteLine("订单状态更新成功。");
#endif
            }
            else
            {

            }
        }

        private void CheckDeviceStatus()
        {
            var item = SV.DeviceDictonary.FirstOrDefault(p => p.Key.Usage == Function);
            if (!item.Key.IsSettingValid())
            {
                pnlTopHint.Visible = true;
                lblHint.Text = $"未正确配置 {Function.ToString() }设备。";
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

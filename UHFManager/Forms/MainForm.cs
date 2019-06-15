using DLL;
using NLog.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using UHFManager.DataModel;
using UHFManager.Interface;
using UHFManager.POCO;

namespace UHFManager
{
    public partial class MainForm : Form
    {
        #region 构造

        public MainForm()
        {
            InitializeComponent();

        }

        #endregion

        #region 字段 

        /* 加下划线_的变量表示自定义的实例变量 */

        private SettingForm _frmSetting;
        private EnterWarehouseForm _frmEnterWarehouse;
        private WashingForm _frmWash;
        private DryForm _frmDry;
        private ExWarehouseForm _frmExWarehouse;

        #endregion

        #region 属性



        #endregion

        #region 窗体控件事件处理

        private void MainForm_Load(object sender, EventArgs e)
        {
            RichTextBoxTarget.ReInitializeAllTextboxes(this);

            this.tcMain.SelectedIndex = 1;

            if (SV.DeviceDictonary == null || SV.DeviceDictonary.Count == 0)
            {
                MessageBox.Show("首次运行，需要设置设备连接。");
                LoadSettingForm();
                tcMain.SelectedIndex = 0;
                return;
            }
            else
            {

            }

            LoadSettingForm();
            LoadEnterWarehouseForm();
            LoadWashingForm();
            LoadDryForm();
            LoadExWarehouseForm();

            ConnectDevice1();
            ConnectDevice2();
            ConnectDevice3();
            ConnectDevice4();
        }


        /// <summary>
        /// 标签栏切换时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        private void tsmiShowLog_Click(object sender, EventArgs e)
        {
            var state = tsmiShowLog.Checked = !tsmiShowLog.Checked;

            this.pnlConnectInfo.Visible = state;

        }

        private void tsmiClearLog_Click(object sender, EventArgs e)
        {
            rtbMain.Clear();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var dr = MessageBox.Show(this, "确定要关闭程序吗？", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                DisconnectAllDevice();
            }
        }
        #endregion

        #region 外部委托处理

        /// <summary>
        /// 设置更新后， 重新加载窗体
        /// </summary>
        /// <param name="newSetting"></param>
        private void FrmSetting_SettingUpdated(DeviceSetting newSetting)
        {
            
            if (newSetting.Usage == DeviceUsageEnum.入库)
            {
                LoadEnterWarehouseForm();
                //ConnectDevice1(); 改为手动连接 
            }
            else if (newSetting.Usage == DeviceUsageEnum.洗涤)
            {
                LoadWashingForm();
                //ConnectDevice2();
            }
            else if (newSetting.Usage == DeviceUsageEnum.烘干)
            {
                LoadDryForm();
                //ConnectDevice3();
            }
            else if (newSetting.Usage == DeviceUsageEnum.出库)
            {
                LoadExWarehouseForm();
                //ConnectDevice4();
            }
        }
               
        private void OnDeviceConnected<T>(T instance) where T : Form, IDeviceForm
        {
            if (!instance.IsDisposed)
            {
                if (instance.InvokeRequired)
                {
                    instance.Invoke((Action)delegate
                    {
                        instance.DoWork();
                    });
                }
                else
                {
                    instance.DoWork();
                }
                SV.Logger.Info( $"{instance.Function.ToString()}设备开始工作......");
            }
        }
        
        private void OnDeviceDisconnected<T>(T instance) where T : Form, IDeviceForm
        {
            if (!instance.IsDisposed)
            {
                if (instance.InvokeRequired)
                {
                    instance.Invoke((Action)delegate
                    {
                        instance.StopWork();
                    });
                }
                else
                {
                    instance.StopWork();
                }
                SV.Logger.Info($"{instance.Function.ToString()}设备停止工作......");
            }
        }

        #endregion

        #region 设备连接

        public void ConnectDevice1()
        {
            DeviceUsageEnum currentUsage = DeviceUsageEnum.入库;
            var item = SV.DeviceDictonary.FirstOrDefault(p => p.Key.Usage == currentUsage);

            DeviceSetting setting = item.Key;
            if (!setting.IsSettingValid())
            {
                SV.Logger.Info($"{currentUsage.ToString()}设备: 配置无效，无法连接。");
                return;
            }
            DeviceProxy deviceProxy = item.Value;
            deviceProxy.Connected += () => { this.OnDeviceConnected(_frmEnterWarehouse); };
            deviceProxy.Closing += () => { this.OnDeviceDisconnected(_frmEnterWarehouse); };
            int status = deviceProxy.ConnectViaNetwork(item.Key.DeviceIpAddress, item.Key.DevicePort);
            if (status == 0)
            {
                deviceProxy.IsConnected = true;
                SV.Logger.Info($"{currentUsage.ToString()}设备: 成功建立连接。");
            }
        }
        public void ConnectDevice2()
        {
            DeviceUsageEnum currentUsage = DeviceUsageEnum.洗涤;
            var item = SV.DeviceDictonary.FirstOrDefault(p => p.Key.Usage == currentUsage);

            DeviceSetting setting = item.Key;
            if (!setting.IsSettingValid())
            {
                SV.Logger.Info($"{currentUsage.ToString()}设备: 配置无效，无法连接。");
                return;
            }

            DeviceProxy deviceProxy = item.Value;
            deviceProxy.Connected += () => { this.OnDeviceConnected(_frmWash); };
            deviceProxy.Closing += () => { this.OnDeviceDisconnected(_frmWash); };
            int status = deviceProxy.ConnectViaNetwork(item.Key.DeviceIpAddress, item.Key.DevicePort);
            if (status == 0)
            {
                deviceProxy.IsConnected = true;
                SV.Logger.Info($"{currentUsage.ToString()}设备: 成功建立连接。");
            }
        }
        public void ConnectDevice3()
        {
            DeviceUsageEnum currentUsage = DeviceUsageEnum.烘干;
            var item = SV.DeviceDictonary.FirstOrDefault(p => p.Key.Usage == currentUsage);

            DeviceSetting setting = item.Key;
            if (!setting.IsSettingValid())
            {
                SV.Logger.Info($"{currentUsage.ToString()}设备: 配置无效，无法连接。");
                return;
            }
            DeviceProxy deviceProxy = item.Value;
            deviceProxy.Connected += () => { this.OnDeviceConnected(_frmDry); };
            deviceProxy.Closing += () => { this.OnDeviceDisconnected(_frmDry); };
            int status = deviceProxy.ConnectViaNetwork(item.Key.DeviceIpAddress, item.Key.DevicePort);
            if (status == 0)
            {
                deviceProxy.IsConnected = true;
                SV.Logger.Info($"{currentUsage.ToString()}设备: 成功建立连接。");
            }
        }
        public void ConnectDevice4()
        {
            DeviceUsageEnum currentUsage = DeviceUsageEnum.出库;
            var item = SV.DeviceDictonary.FirstOrDefault(p => p.Key.Usage == currentUsage);

            DeviceSetting setting = item.Key;
            if (!setting.IsSettingValid())
            {
                SV.Logger.Info($"{currentUsage.ToString()}设备: 配置无效，无法连接。");
                return;
            }

            DeviceProxy deviceProxy = item.Value;
            deviceProxy.Connected += () => { this.OnDeviceConnected(_frmExWarehouse); };
            deviceProxy.Closing += () => { this.OnDeviceDisconnected(_frmExWarehouse); };
            int status = deviceProxy.ConnectViaNetwork(item.Key.DeviceIpAddress, item.Key.DevicePort);
            if (status == 0)
            {
                deviceProxy.IsConnected = true;
                SV.Logger.Info($"{currentUsage.ToString()}设备: 成功建立连接。");
            }
        }


        public void DisconnectAllDevice()
        {
            foreach (var kp in SV.DeviceDictonary)
            {
                DeviceProxy deviceProxy = kp.Value;
                if (deviceProxy.IsConnected)
                {
                    int status = deviceProxy.Disconnect();
                    if (status == 0)
                    {
                        deviceProxy.IsConnected = false;
                    }
                    else
                    {
                        Debug.WriteLine("设备关闭失败。");
                    }
                }
            }
        }

        #endregion

        #region 几个窗体初始化

        public void LoadSettingForm(bool activate = false)
        {
            if (_frmSetting == null)
            {
                _frmSetting = new SettingForm();
                _frmSetting.TopLevel = false;
                _frmSetting.Dock = DockStyle.Fill;
                _frmSetting.FormBorderStyle = FormBorderStyle.None;
                _frmSetting.SettingUpdated += FrmSetting_SettingUpdated;

                tpSetting.Controls.Add(_frmSetting);
                _frmSetting.Show();
            }

            if (activate)
            {
                tcMain.SelectedTab = tpSetting;
            }
        }

        public void LoadEnterWarehouseForm(bool activate = false)
        {
            if (_frmEnterWarehouse == null)
            {
                _frmEnterWarehouse = new EnterWarehouseForm();
                _frmEnterWarehouse.TopLevel = false;
                _frmEnterWarehouse.Dock = DockStyle.Fill;
                _frmEnterWarehouse.FormBorderStyle = FormBorderStyle.None;
                tpEnteringWarehouse.Controls.Add(_frmEnterWarehouse);

                _frmEnterWarehouse.Show();
            }

            if (activate)
            {
                tcMain.SelectedTab = tpEnteringWarehouse;
            }
        }

        public void LoadWashingForm(bool activate = false)
        {
            if (_frmWash == null)
            {
                _frmWash = new WashingForm();
                _frmWash.TopLevel = false;
                _frmWash.Dock = DockStyle.Fill;
                _frmWash.FormBorderStyle = FormBorderStyle.None;
                tpWash.Controls.Add(_frmWash);

                _frmWash.Show();
            }

            if (activate)
            {
                tcMain.SelectedTab = tpWash;
            }
        }

        public void LoadDryForm(bool activate = false)
        {
            if (_frmDry == null)
            {
                _frmDry = new DryForm();
                _frmDry.TopLevel = false;
                _frmDry.Dock = DockStyle.Fill;
                _frmDry.FormBorderStyle = FormBorderStyle.None;
                tpDry.Controls.Add(_frmDry);

                _frmDry.Show();
            }

            if (activate)
            {
                tcMain.SelectedTab = tpDry;
            }
        }

        public void LoadExWarehouseForm(bool activate = false)
        {
            if (_frmExWarehouse == null)
            {
                _frmExWarehouse = new ExWarehouseForm();
                _frmExWarehouse.TopLevel = false;
                _frmExWarehouse.Dock = DockStyle.Fill;
                _frmExWarehouse.FormBorderStyle = FormBorderStyle.None;

                tpExitWarehouse.Controls.Add(_frmExWarehouse);

                _frmExWarehouse.Show();
            }


            if (activate)
            {
                tcMain.SelectedTab = tpExitWarehouse;
            }
        }

        #endregion
    }
}

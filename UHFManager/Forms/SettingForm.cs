using DLL;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using UHFManager.Common;
using UHFManager.DataContext;
using UHFManager.DataModel;
using UHFManager.POCO;
using Utils;

namespace UHFManager
{
    public partial class SettingForm : Form
    {


        #region 构造

        public SettingForm()
        {
            InitializeComponent();

            LoadSettingsFromDb();
        }

        #endregion

        #region 字段

        private DeviceSettingService svc = SV.DbEntry<DeviceSettingService>();

        #endregion

        public event Action<DeviceSetting> SettingUpdated;

        private void SettingForm_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 点击保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 判断数据有效性

            // 检测后台是不是正在运行
            if (SV.DeviceDictonary.Any(p => p.Value.IsConnected == true)){ 
                var warning = MessageBox.Show(this, "设备正在运行，修改IP地址会停止设备，要继续吗?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (warning != DialogResult.Yes)
                    return;
            }


            #region 停止设备连接 

            foreach (var dic in SV.DeviceDictonary)
            {
                DeviceProxy dev = dic.Value;
                if (dev.IsConnected == true)
                {
                    dev.Disconnect();
                    dev.IsConnected = false;
                }
            }

            #endregion


            if (!CheckInput())
                return;

            #endregion
                    
            // 保存第一个
            if (txtIP1.IPAddress != IPAddress.None)
            {
                DeviceSetting entity1 = new DeviceSetting();
                entity1.DeviceIpAddress = txtIP1.IPAddress.ToString();
                entity1.DevicePort = txtPort1.Text;
                entity1.Usage = POCO.DeviceUsageEnum.入库;
                svc.SaveOrUpdate(entity1);

                if (SettingUpdated != null)
                {
                    SettingUpdated(entity1);
                }
            }

            // 保存第二个
            if (txtIP2.IPAddress != IPAddress.None)
            {
                DeviceSetting entity2 = new DeviceSetting();
                entity2.DeviceIpAddress = txtIP2.IPAddress.ToString();
                entity2.DevicePort = txtPort2.Text;
                entity2.Usage = POCO.DeviceUsageEnum.洗涤;
                svc.SaveOrUpdate(entity2);

                if (SettingUpdated != null)
                {
                    SettingUpdated(entity2);
                }
            }

            // 保存第三个
            if (txtIP3.IPAddress != IPAddress.None)
            {
                DeviceSetting entity3 = new DeviceSetting();
                entity3.DeviceIpAddress = txtIP3.IPAddress.ToString();
                entity3.DevicePort = txtPort3.Text;
                entity3.Usage = POCO.DeviceUsageEnum.烘干;
                svc.SaveOrUpdate(entity3);

                if (SettingUpdated != null)
                {
                    SettingUpdated(entity3);
                }
            }

            // 保存第四个
            if (txtIP4.IPAddress != IPAddress.None)
            {
                DeviceSetting entity4 = new DeviceSetting();
                entity4.DeviceIpAddress = txtIP4.IPAddress.ToString();
                entity4.DevicePort = txtPort4.Text;
                entity4.Usage = POCO.DeviceUsageEnum.出库;
                svc.SaveOrUpdate(entity4);


                if (SettingUpdated != null)
                {
                    SettingUpdated(entity4);
                }

            }

            MessageBox.Show("保存成功");
        }
        
        /// <summary>
        /// 从数据库加载设置
        /// </summary>
        private void LoadSettingsFromDb()
        {
            var list = svc.Load(); 
            foreach (var setting in list)
            {
                switch (setting.Usage)
                {
                    case POCO.DeviceUsageEnum.入库:
                        txtIP1.IPAddress = GetIP(setting.DeviceIpAddress);
                        txtPort1.Text = setting.DevicePort;
                        break;
                    case POCO.DeviceUsageEnum.洗涤: 
                        txtIP2.IPAddress = GetIP(setting.DeviceIpAddress);
                        txtPort2.Text = setting.DevicePort;
                        break;
                    case POCO.DeviceUsageEnum.烘干:
                        txtIP3.IPAddress = GetIP(setting.DeviceIpAddress);
                        txtPort3.Text = setting.DevicePort;
                        break;
                    case POCO.DeviceUsageEnum.出库:
                        txtIP4.IPAddress = GetIP(setting.DeviceIpAddress);
                        txtPort4.Text = setting.DevicePort;
                        break;
                } 
            }

            
        }

        /// <summary>
        /// 转换IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private IPAddress GetIP(string ip)
        {
            IPAddress temp = IPAddress.Any;
            if (!IPAddress.TryParse(ip, out temp))
            {
                temp = IPAddress.Any;
            }

            return temp;
        }

        private bool CheckInput()
        {
            bool isOK = true;

            StringBuilder outputString = new StringBuilder();

            int index = 0;
            if (!Helper.IsIPAddressValid(txtIP1.IPAddress))
            {
                outputString.Append("设置的IP地址无效: 入厂设备");
                outputString.AppendLine();
                index++;
            }
            if (!Helper.IsIPAddressValid(txtIP2.IPAddress))
            {
                outputString.Append("设置的IP地址无效: 洗涤设备");
                outputString.AppendLine();
                index++;
            }
            if (!Helper.IsIPAddressValid(txtIP3.IPAddress))
            {
                outputString.Append("设置的IP地址无效: 烘干设备");
                outputString.AppendLine();
                index++;
            }
            if (!Helper.IsIPAddressValid(txtIP4.IPAddress))
            {
                outputString.Append("设置的IP地址无效: 出库设备");
                outputString.AppendLine();
                index++;
            }

            if (index == 4)
            {
                isOK = false;
            }

            if (txtIP1.IPAddress == txtIP2.IPAddress 
                || txtIP2.IPAddress == txtIP3.IPAddress 
                || txtIP3.IPAddress == txtIP4.IPAddress 
                || txtIP1.IPAddress == txtIP4.IPAddress)
            {
                outputString.Append("设备的IP地址不能相同");
                outputString.AppendLine();
                isOK = false;                
            }

            // 端口无效时默认取100
            if (!Helper.IsPortValid(txtPort1.Text))
            {
                txtPort1.Text = "100";
            }
            if (!Helper.IsPortValid(txtPort2.Text))
            {
                txtPort2.Text = "100";
            }
            if (!Helper.IsPortValid(txtPort3.Text))
            {
                txtPort3.Text = "100";
            }
            if (!Helper.IsPortValid(txtPort4.Text))
            {
                txtPort4.Text = "100";
            }
             
            if (outputString.Length > 0)
            {
                // 3秒自动关闭
                new System.Threading.Timer((o) => { 
                    MessageBoxHelper.FindAndKillWindow("配置错误");
                }, null, 3000, 0);

                MessageBox.Show(this,
                    outputString.ToString(),
                    "配置错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return isOK;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //if (!CheckInput())
            //{
            //    return;
            //}




            Task.Factory.StartNew(() =>
            {
                return TestSocketConnect(txtIP1.Text, txtPort1.Text);
            }).ContinueWith((task)=> {
                int status = task.Result;

                this.Invoke((Action)delegate {
                    lblStatus1.Text = status == 0 ? "PASS" : "FAIL";
                    ShowTimer(lblStatus1);
                });
            });


            Task.Factory.StartNew(() =>
            {
                return TestSocketConnect(txtIP2.Text, txtPort2.Text);
            }).ContinueWith((task) => {
                int status = task.Result;

                this.Invoke((Action)delegate {
                    lblStatus2.Text = status == 0 ? "PASS" : "FAIL";
                    ShowTimer(lblStatus2);
                });
            });


            Task.Factory.StartNew(() =>
            {
                return TestSocketConnect(txtIP3.Text, txtPort3.Text);
            }).ContinueWith((task) => {
                int status = task.Result;

                this.Invoke((Action)delegate {
                    lblStatus3.Text = status == 0 ? "PASS" : "FAIL";
                    ShowTimer(lblStatus3);
                });
            });


            Task.Factory.StartNew(() =>
            {
                return TestSocketConnect(txtIP4.Text, txtPort4.Text);
            }).ContinueWith((task) => {
                int status = task.Result;

                this.Invoke((Action)delegate {
                    lblStatus4.Text = status == 0 ? "PASS" : "FAIL";
                    ShowTimer(lblStatus4);
                });
            });

        }
               
        public int TestSocketConnect(string ipString, string portString)
        {
            DeviceSetting setting = new DeviceSetting() { DeviceIpAddress = ipString, DevicePort = portString };
            if (!setting.IsSettingValid())
            {
                return -2;
            } 
            else
            {
                RFID_StandardProtocol Reader = new RFID_StandardProtocol();
                Socket temp = null;

                int status = Reader.Socket_ConnectSocket(ref temp, ipString, portString.ToDefaultInt32());
                if (status == RFID_StandardProtocol.SUCCESS)
                {
                    temp.Close();
                    temp.Dispose();
                    return 0;       //连接成功(connect success)
                }
                else
                {
                    temp.Close();
                    temp.Dispose();
                    return -1;      //连接失败(connect fail)
                }
            }
        }

        private void ShowTimer(Control target)
        {
            target.ForeColor = target.BackColor;

            Timer t = new Timer();
            t.Interval = 500;
            t.Tag = Environment.TickCount;
            t.Tick += (a, b) => {
                int oldTick = (int)t.Tag;
                int newTick = Environment.TickCount;

                int time = newTick - oldTick;
                //Debug.WriteLine(time / 1000);
                //Debug.WriteLine(time % 1000);

                if (time % 1000 == 0)
                {
                    Color c1 = (target.Text == "PASS") ? Color.Green : Color.Red;
                    target.ForeColor = c1; 
                }
                else
                {
                    target.ForeColor = target.BackColor; 
                }

                // 闪烁三次
                if (time / 1000 >= 3)
                {
                    t.Enabled = false;
                }
            };
            t.Enabled = true;
        }


    }
}

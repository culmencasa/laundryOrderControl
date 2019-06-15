using DLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UHFManager.POCO
{
    class DeviceProxy
    {
        /// <summary>
        /// 通信句柄结构体(connect handle struct)
        /// </summary>
        public hComSocket CS;

        /// <summary>
        /// 网口句柄
        /// </summary>
        Socket ClientSocket = null;

        /// <summary>
        /// 串口句柄(comm handle)
        /// </summary>
        public int hCom = -1;

        public RFID_StandardProtocol Reader { get; set; } = new RFID_StandardProtocol();

        public bool IsConnected { get; set; }

        public static int Freqnum = 0;
        public static int[] Freqpoints = new int[124];


        public event Action Closing;
        public event Action Connected;


        #region 公开方法

        public int ConnectViaNetwork(string ip, string port)
        {
            int status = this.SocketConnect(ip, port);
            if (status == 0)
            {
                CS.hCom = -1;
                CS.sockClient = ClientSocket;

                GetVersion(255);
                GetSerialNum(255);
                GetANTENNA();
                GetRFPWR();
                GetFREQ();
                GetInternet();
                GetSFTM();
                GetOUTPUT(0);
                GetOUTPUT(1);
                GetOUTPUT(2);
                GetInterface();

                if (Connected != null)
                {
                    Connected();
                }

                return 0;
            }

            return -1;
        }

        public int Disconnect()
        {
            if (Closing != null)
            {
                Closing();
            }

            int status = SocketDisConnect();
            return status;
        }

        private object epcLock = new object();

        /// <summary>
        /// EPC GEN2启动标签盘询
        /// </summary>
        public List<string> EPCMultiTagInventory()
        {
            List<string> tagList = new List<string>();

            int TagCount = 0;
            int GetCount = 0;
            int Count = 0;
            int i, j = 0;
            int Cnt = 0;
            int status;
            string EPC = "";
            BufferData[] Data = new BufferData[256];
            for (int index = 0; index < Data.Length; index++)
            {
                Data[index].Data = new byte[512];
            }

            status = Reader.GEN2_MultiTagInventory(CS, ref TagCount, 0xFF);
            if (0x00 == status)
            {
                if (0 != TagCount)		//标签识别成功//tag identify success
                {
                    status = Reader.BufferM_GetTagData(CS, ref GetCount, Data, 0xFF);
                    if (0x00 == status)
                    {
                        for (i = 0; i < GetCount; i++)
                        {
                            EPC = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}{6:X2}{7:X2}{8:X2}{9:X2}{10:X2}{11:X2}",
                                Data[i].Data[0], Data[i].Data[1], Data[i].Data[2], Data[i].Data[3],
                                Data[i].Data[4], Data[i].Data[5], Data[i].Data[6], Data[i].Data[7],
                                Data[i].Data[8], Data[i].Data[9], Data[i].Data[10], Data[i].Data[11]);


                            if (!tagList.Contains(EPC))
                            {
                                lock (epcLock)
                                {
                                    tagList.Add(EPC);
                                }
                            }

                            //if (SHOWDATA_listView.Items.Count <= 0)
                            //{
                            //    EPCDisplayNewTag(EPC);
                            //    DisplayCnt++;
                            //}
                            //else
                            //{
                            //    int flg = -1;
                            //    for (j = 0; j < SHOWDATA_listView.Items.Count; j++)
                            //    {
                            //        if (EPC == SHOWDATA_listView.Items[j].SubItems[1].Text)
                            //        {
                            //            SHOWDATA_listView.Items[j].SubItems[2].Text =
                            //                Convert.ToString(Convert.ToInt32(SHOWDATA_listView.Items[j].SubItems[2].Text) + 1);
                            //            flg = i;
                            //        }
                            //    }
                            //    if (flg < 0)
                            //    {
                            //        EPCDisplayNewTag(EPC);
                            //        DisplayCnt++;
                            //    }
                            //}
                        }
                    }
                }
            }

            return tagList;
        }


        /// <summary>
        /// EPC GEN2多标签读取(4个扇区)
        /// </summary>
        void EPCMultiTagRead(Zone zone, MemAddress startAddress, MemAddress endAddress)
        {
            int TagCount = 0;
            int GetCount = 0;
            int Count = 0;
            int i, j;
            int status;
            int Cnt = 0;
            int RCnt = 0;                           //Reserve区读取数据字节数(Reserve memory read data numbers of byte)
            int ECnt = 0;                           //Epc区读取数据字节数(EPC memory read data numbers of byte)
            int TCnt = 0;                           //Tid区读取数据字节数(TID memory read data numbers of byte)
            int UCnt = 0;                           //User区读取数据字节数(User memory read data numbers of byte)
            byte[] Reserve = new byte[100];         //Reserve区读取数据(Reserve memory read data)
            byte[] Epc = new byte[100];             //Epc区读取数据(EPC memory read data)
            byte[] Tid = new byte[100];             //Tid区读取数据(TID memory read data)
            byte[] User = new byte[100];            //User区读取数据(User memory read data)
            string Tempstr = "";
            BufferData[] Data = new BufferData[256];
            for (int index = 0; index < Data.Length; index++)
            {
                Data[index].Data = new byte[512];
            }

            string SNo = "";
            string EPC = "";
            //初始化(initialize)
            WordptrAndLength WpALen;
            WpALen.ReserveWordPtr = 0;
            WpALen.ReserveWordCnt = 0;
            WpALen.EpcWordPtr = 0;
            WpALen.EpcWordCnt = 0;
            WpALen.TidWordPtr = 0;
            WpALen.TidWordCnt = 0;
            WpALen.UserWordPtr = 0;
            WpALen.UserWordCnt = 0;
            WpALen.MembankMask = 0;

            // RESERVE区
            if ((zone & Zone.RESERVE) == Zone.RESERVE)
            {
                WpALen.MembankMask += 1;
                WpALen.ReserveWordPtr = (int)startAddress;
                WpALen.ReserveWordCnt = (int)endAddress + 1;
                RCnt = WpALen.ReserveWordCnt * 2;
            }
            if ((zone & Zone.EPC) == Zone.EPC)
            {
                WpALen.MembankMask += 2;
                WpALen.EpcWordPtr = (int)startAddress;
                WpALen.EpcWordCnt = (int)endAddress + 1;
                ECnt = WpALen.EpcWordCnt * 2;
            }
            if ((zone & Zone.TID) == Zone.TID)
            {
                WpALen.MembankMask += 4;
                WpALen.TidWordPtr = (int)startAddress;
                WpALen.TidWordCnt = (int)endAddress + 1;
                TCnt = WpALen.TidWordCnt * 2;
            }
            if ((zone & Zone.USER) == Zone.USER)
            {
                WpALen.MembankMask += 8;
                WpALen.UserWordPtr = (int)startAddress;
                WpALen.UserWordCnt = (int)endAddress + 1;
                UCnt = WpALen.UserWordCnt * 2;
            }

            status = Reader.GEN2_MultiTagRead(CS, WpALen, ref TagCount, 0xFF);
            if (0x00 == status)
            {
                if (0 != TagCount)		//标签读取成功//tag read success
                {
                    status = Reader.BufferM_GetTagData(CS, ref GetCount, Data, 0xFF);
                    if (0x00 == status)
                    {
                        for (i = 0; i < GetCount; i++)
                        {
                            EPC = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}{6:X2}{7:X2}{8:X2}{9:X2}{10:X2}{11:X2}",
                                Data[i].Data[0], Data[i].Data[1], Data[i].Data[2], Data[i].Data[3],
                                Data[i].Data[4], Data[i].Data[5], Data[i].Data[6], Data[i].Data[7],
                                Data[i].Data[8], Data[i].Data[9], Data[i].Data[10], Data[i].Data[11]);

                            //todo:
                            //if (SHOWDATA_listView.Items.Count <= 0)
                            //{
                            //    if (RCnt != 0)
                            //    {
                            //        Array.Copy(Data[i].Data, 12, Reserve, 0, RCnt);
                            //        string RStr = ByteToHexStr(Reserve, RCnt);
                            //        Array.Clear(Reserve, 0, 100);
                            //    }
                            //    if (ECnt != 0)
                            //    {
                            //        Array.Copy(Data[i].Data, 12 + RCnt, Epc, 0, ECnt);
                            //        string EStr = ByteToHexStr(Epc, ECnt);
                            //        Array.Clear(Epc, 0, 100);
                            //    }
                            //    if (TCnt != 0)
                            //    {
                            //        Array.Copy(Data[i].Data, 12 + RCnt + ECnt, Tid, 0, TCnt);
                            //        string TStr = ByteToHexStr(Tid, TCnt);
                            //        Array.Clear(Tid, 0, 100);
                            //    }
                            //    if (UCnt != 0)
                            //    {
                            //        Array.Copy(Data[i].Data, 12 + RCnt + ECnt + TCnt, User, 0, UCnt);
                            //        string UStr = ByteToHexStr(User, UCnt);
                            //        Array.Clear(User, 0, 100);
                            //    }
                            //    //EPCDisplayNewTag(EPC);
                            //    //DisplayCnt++;
                            //}
                            //else
                            //{
                            //    int flg = -1;
                            //    for (j = 0; j < SHOWDATA_listView.Items.Count; j++)
                            //    {
                            //        if (EPC == SHOWDATA_listView.Items[j].SubItems[6].Text)
                            //        {
                            //            SHOWDATA_listView.Items[j].SubItems[5].Text =
                            //                Convert.ToString(Convert.ToInt32(SHOWDATA_listView.Items[j].SubItems[5].Text) + 1);
                            //            flg = i;
                            //        }
                            //    }
                            //    if (flg < 0)
                            //    {
                            //        if (RCnt != 0)
                            //        {
                            //            Array.Copy(Data[i].Data, 12, Reserve, 0, RCnt);
                            //            RStr = ByteToHexStr(Reserve, RCnt);
                            //            Array.Clear(Reserve, 0, 100);
                            //        }
                            //        if (ECnt != 0)
                            //        {
                            //            Array.Copy(Data[i].Data, 12 + RCnt, Epc, 0, ECnt);
                            //            EStr = ByteToHexStr(Epc, ECnt);
                            //            Array.Clear(Epc, 0, 100);
                            //        }
                            //        if (TCnt != 0)
                            //        {
                            //            Array.Copy(Data[i].Data, 12 + RCnt + ECnt, Tid, 0, TCnt);
                            //            TStr = ByteToHexStr(Tid, TCnt);
                            //            Array.Clear(Tid, 0, 100);
                            //        }
                            //        if (UCnt != 0)
                            //        {
                            //            Array.Copy(Data[i].Data, 12 + RCnt + ECnt + TCnt, User, 0, UCnt);
                            //            UStr = ByteToHexStr(User, UCnt);
                            //            Array.Clear(User, 0, 100);
                            //        }
                            //        EPCDisplayNewTag(EPC);
                            //        DisplayCnt++;
                            //    }
                            //}
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 安全读
        /// </summary>
        public void SECRead(MemBank memType, string password, MemAddress startAddress, MemLength dataLength)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            int status = 0;
            int SMembank = 0;
            string PWstr = "";
            int RSAddr = 0;
            int RCnt = 0;
            byte[] PassWord = new byte[4];
            byte[] RData = new byte[100];

            SMembank = (int)memType;
            PWstr = password;
            RSAddr = (int)startAddress; // 0-35
            RCnt = (int)dataLength;

            HexStrToByte(PassWord, PWstr);

            status = Reader.GEN2_SecRead(CS, SMembank, PassWord, RSAddr, RCnt, RData, 0xFF);
            if (0 == status)
            {
                TempStr = "标签读取成功";
                TempStrEnglish = "Tag read success";
                string TStr = "";
                TStr = ByteToHexStr(RData, RCnt * 2);

                // 读取结果
                Console.WriteLine(TStr);
            }
            else
            {
                TempStr = "标签读取失败";
                TempStrEnglish = "Tag read failed";

                Console.WriteLine(TempStr);
            }

        }
               
        /// <summary>
        /// 安全写
        /// </summary>
        public void SECWrite(MemBank memType, string password, MemAddress startAddress, MemLength dataLength, string content)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            int status = 0;
            int SMembank = 0;
            string PWstr = "";
            string WDataStr = "";
            int WSAddr = 0;
            int WCnt = 0;
            byte[] PassWord = new byte[4];
            byte[] WData = new byte[100];

            SMembank = (int)memType;
            PWstr = password;
            WSAddr = (int)startAddress;
            WCnt = (int)dataLength;
            WDataStr = content;

            if (WDataStr.Length == WCnt * 4)
            {
                HexStrToByte(PassWord, PWstr);
                HexStrToByte(WData, WDataStr);
                status = Reader.GEN2_SecWrite(CS, SMembank, PassWord, WSAddr, WCnt, WData, 0xFF);
                if (0 == status)
                {
                    TempStr = "标签写入成功";
                    TempStrEnglish = "tag write success";
                }
                else
                {
                    TempStr = "标签写入失败";
                    TempStrEnglish = "tag write failed";
                }

                Console.WriteLine(TempStr);
            }
            else
            {
                Console.WriteLine("写入标签的数据与写入的长度不匹配!请检查后重试");

            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearIDBuffer()
        {
            Reader.BufferM_ClearBuffer(CS, 0xFF);
        }
        
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="ReaderNum"></param>
        public void GetVersion(int ReaderNum)
        {
            byte major = 0;
            byte minor = 0;
            string TempStr = "";
            string TempStrEnglish = "";
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            //int errcod = rfid_sp.Config_GetLocatorVersion(CS, ref major, ref minor, ReaderAddr);
            //ListBoxAdd("GetVersion errcod = " + errcod);
            if (0x00 == Reader.Config_GetLocatorVersion(CS, ref major, ref minor, ReaderAddr))
            {
                string Major = string.Format("{0:D2}", (int)major); ;
                string Minor = string.Format("{0:D2}", (int)minor); ;
                TempStr = "V" + Major + "." + Minor;

                Console.WriteLine("读写器固件版本号为: " + TempStr);

                //SetVersionText(TempStr);
                //SetText(TempStr);
            }
            else
            {
                TempStr = "获取版本号失败!";
                TempStrEnglish = "Get version fail!";

                Console.WriteLine(TempStr);
            }
        }

        /// <summary>
        /// 获取序列号
        /// </summary>
        /// <param name="ReaderNum"></param>
        private void GetSerialNum(int ReaderNum)
        {
            string TempStr = "";
            string TempStrEnglish = "";

            byte StartAddr = 0x10;
            byte PLen = 6;
            byte[] GData = new byte[6];
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            //int errcod = rfid_sp.Parameter_GetReader(CS, StartAddr, PLen, GData, ReaderAddr);
            //ListBoxAdd("GetSerialNum errcod = " + errcod);
            if (0x00 == Reader.Parameter_GetReader(CS, StartAddr, PLen, GData, ReaderAddr))
            {
                TempStr = ByteToHexStr(GData, PLen);
                
                Console.WriteLine("读写器序列号为: " + TempStr);


                //SetSerialNoText(TempStr);
                //SetText(TempStr);
            }
            else
            {
                TempStr = "获取序列号号失败!";
                TempStrEnglish = "Get serial No. fail!";

                Console.WriteLine(TempStr);
            }
        }
        
        /// <summary>
        /// 查询天线工作状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetANTENNA()
        {
            string TempStr = "";
            string TempStrEnglish = "";
            int Workant = 0;
            int antStatus = 0;

            if (0x00 == Reader.Config_GetAntenna(CS, ref Workant, ref antStatus, 0xFF))
            {
                TempStr = "天线工作状态查询成功!";
                TempStrEnglish = "antenna work state query success";
                if ((Workant & (1 << 0)) != 0)
                {
                    Console.WriteLine("天线1 OK"); 
                }
                else
                {
                    Console.WriteLine("天线1 关"); 
                }
                if ((Workant & (1 << 1)) != 0)
                {
                    Console.WriteLine("天线2 OK"); 
                }
                else
                {
                    Console.WriteLine("天线2 关");
                }
                if ((Workant & (1 << 2)) != 0)
                {
                    Console.WriteLine("天线3 OK"); 
                }
                else
                {
                    Console.WriteLine("天线3 关");
                }
                if ((Workant & (1 << 3)) != 0)
                {
                    Console.WriteLine("天线4 OK"); 
                }
                else
                {
                    Console.WriteLine("天线4 关");
                }
            }
            else
            {
                TempStr = "天线工作状态查询失败!";
                TempStrEnglish = "antenna work state query failed";
            }

            Console.WriteLine(TempStr);
        }

        /// <summary>
        /// 查询功率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetRFPWR()
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte[] aPwr = new byte[4];

            if (0x00 == Reader.Config_GetRfPower(CS, aPwr, 0xFF))
            {
                TempStr = "天线功率查询成功!";
                TempStrEnglish = "antenna power query success";

                Console.WriteLine(string.Format("天线1 {0:D} dBm", (int)aPwr[0]));
                Console.WriteLine(string.Format("天线2 {0:D} dBm", (int)aPwr[1]));
                Console.WriteLine(string.Format("天线3 {0:D} dBm", (int)aPwr[2]));
                Console.WriteLine(string.Format("天线4 {0:D} dBm", (int)aPwr[3]));

            }
            else
            {
                TempStr = "天线功率查询失败!";
                TempStrEnglish = "antenna power query failed";

                Console.WriteLine(TempStr);
            }

        }
        
        /// <summary>
        /// 查询读写器射频参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetFREQ()
        {
            string TempStr = "";
            string TempStrEnglish = "";
            if (0x00 == Reader.Config_GetFreqPoint(CS, ref Freqnum, Freqpoints, 0xFF))
            {
                TempStr = "天线频率查询成功!";
                TempStrEnglish = "antenna frequency query success";
                if (0 == Freqnum)
                {
                    if (0 == Freqpoints[0])
                    {
                        Console.WriteLine("频率-频域：中国");
                    }
                    else if (1 == Freqpoints[0])
                    {
                        Console.WriteLine("频率-频域：北美");
                    }
                    else if (2 == Freqpoints[0])
                    {
                        Console.WriteLine("频率-频域：欧洲");
                    }
                    else
                    {
                        Console.WriteLine("频率-频域：其他");
                    }
                }
                else
                {
                    Console.WriteLine("未知频率");

                    //CHINA_radioButton.Checked = false;
                    //NORTHAMERICA_radioButton.Checked = false;
                    //EUROPE_radioButton.Checked = false;
                    //OTHERS_radioButton.Checked = true;
                    //FreqPointsForm freqPoints = new FreqPointsForm();
                    //freqPoints.ShowDialog();
                }
            }
            else
            {
                TempStr = "天线频率查询失败!";
                TempStrEnglish = "antenna frequency query failed";

                Console.WriteLine(TempStr);

            }
        }
        
        /// <summary>
        /// 查询网口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetInternet()
        {
            string TempStr = "";
            string TempStrEnglish = "";
            string IPAddr = "";
            string MaskCode = "";
            string GateWay = "";
            int InternetPort = 0;
            byte[] IPAddrbuf = new byte[4];
            byte[] MaskCodebuf = new byte[4];
            byte[] GateWaybuf = new byte[4];
            byte[] InternetPortbuf = new byte[2];

            if (0x00 == Reader.Config_GetIntrnetAccess(CS, IPAddrbuf, MaskCodebuf, GateWaybuf, InternetPortbuf, 0xFF))
            {
                TempStr = "查询读写器网口参数成功!";
                TempStrEnglish = "Reader ethernet query success!";
                ByteToDecimalstr(ref IPAddr, IPAddrbuf);
                ByteToDecimalstr(ref MaskCode, MaskCodebuf);
                ByteToDecimalstr(ref GateWay, GateWaybuf);
                InternetPort = (InternetPortbuf[0] << 8) + (int)InternetPortbuf[1];

                Console.WriteLine($"IP: {IPAddr}:{InternetPort}");
                Console.WriteLine($"MaskCode: {MaskCode}");
                Console.WriteLine($"Gateway: {GateWay}");
                
            }
            else
            {
                TempStr = "查询读写器网口参数失败!";
                TempStrEnglish = "Reader ethernet query fail!";

                Console.WriteLine(TempStr);

            }

        }
               
        /// <summary>
        /// 查询读写器载波抵消策略
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetSFTM()
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte Mode = 0x00;
            if (0x00 == Reader.Config_GetSingleFastTagMode(CS, ref Mode, 0xFF))
            {
                TempStr = "查询读写器载波抵消策略成功!";
                TempStrEnglish = "query reader single fast tag mode success";
                if (0x00 == Mode)
                {
                    Console.WriteLine("单卡快读模式");

                }
                else
                {
                    Console.WriteLine("多卡模式");
                }
            }
            else
            {
                TempStr = "查询读写器载波抵消策略失败!";
                TempStrEnglish = "query reader single fast tag mode failed";

                Console.WriteLine(TempStr);

            }

        }
                          
        /// <summary>
        /// 查询可编程IO口
        /// </summary>
        /// <param name="ioPort">0 IO1, 1 IO2, 2 继电器</param>
        public void GetOUTPUT(int ioPort)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte Num = 0x00;
            byte Level = 0x00;
            if (ioPort == 0)
                Num = 0x00;
            if (ioPort == 1)
                Num = 0x01;
            if (ioPort == 2)
                Num = 0x02;

            if (0x00 == Reader.Config_GetInPort(CS, Num, ref Level, 0xFF))
            {
                TempStr = "查询读写器设置可编程IO口成功!";
                TempStrEnglish = "query reader set outport IO port success";
            }
            else
            {
                TempStr = "查询读写器设置可编程IO口失败!";
                TempStrEnglish = "query reader set outport IO port failed";
            }


            if (0x00 == Level)
            {
                Console.WriteLine("低电平");
            }
            if (0x01 == Level)
            {
                Console.WriteLine("高电平");
            }

            Console.WriteLine(TempStr);
        }
        
        /// <summary>
        /// 查询模式-接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetInterface()
        {
            GetWorkModel(255);
            this.GetOutInterface(255);
        }
        
        /// <summary>
        /// 查询工作模式
        /// </summary>
        /// <param name="ReaderNum"></param>
        public void GetWorkModel(int ReaderNum)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            byte PAddr = 0x90;
            int PLen = 1;
            int workmodel = 0;
            int time = 0;
            byte[] PSetData = new byte[1];
            byte[] PRefData = new byte[1024];
            
            if (0x00 == Reader.Parameter_GetReader(CS, PAddr, PLen, PRefData, ReaderAddr))
            {
                workmodel = (int)PRefData[0];
                if (1 == workmodel)
                {
                    Console.WriteLine("自动模式");
                }
                else if (2 == workmodel)
                {
                    Console.WriteLine("触发模式");
                }
                else
                {
                    Console.WriteLine("命令模式");
                }
                TempStr = "查询工作模式成功!";
                TempStrEnglish = "Work model query success!";
            }
            else
            {
                TempStr = "查询工作模式失败!";
                TempStrEnglish = "Work model query fail!";
            }

            Console.WriteLine(TempStr);
            
        }

        /// <summary>
        /// 设置工作模式
        /// </summary>
        /// <param name="mode">0 命令模式 1 自动模式 2 触发模式</param>
        /// <param name="ReaderNum"></param>
        /// <returns></returns>
        public void SetWorkModel(int mode, int ReaderNum, int triggerTime=0)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            byte PAddr = 0x90;
            int PLen = 1;
            int workmodel = mode;
            int time = 0;
            byte[] PSetData = new byte[1];
            byte[] PRefData = new byte[1024];

            PSetData[0] = (byte)(workmodel & 0xFF);
            if (0x00 == Reader.Parameter_SetReader(CS, PAddr, PLen, PSetData, ReaderAddr))
            {
                TempStr = "工作模式设置成功!";
                TempStrEnglish = "Work model set success!";
                if (mode == 2 && triggerTime !=0)
                    SetTriggerTime(255, triggerTime);
            }
            else
            {
                TempStr = "工作模式设置失败!";
                TempStrEnglish = "Work model set fail!";
            }

            Console.WriteLine(TempStr);
        }
        
        /// <summary>
        /// 查询输出接口
        /// </summary>
        /// <param name="ReaderNum"></param>
        /// <returns></returns>
        public void GetOutInterface(int ReaderNum)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            byte PAddr = 0x00;
            int PLen = 1;
            byte[] PSetData = new byte[1];
            byte[] PRefData = new byte[1024];

            int OutPort = 0;


            PAddr = 0x97;
            if (0x00 == Reader.Parameter_GetReader(CS, PAddr, PLen, PRefData, ReaderAddr))
            {
                OutPort = (int)PRefData[0];
                if ((OutPort & (1 << 0)) != 0)
                {
                    Console.WriteLine("输出接口包含： RS485端口");
                }
                else
                {
                    Console.WriteLine("输出接口不包含： RS485端口");
                }
                if ((OutPort & (1 << 1)) != 0)
                {
                    Console.WriteLine("输出接口包含：韦根");
                }
                else
                {
                    Console.WriteLine("输出接口不包含：韦根");
                }
                if ((OutPort & (1 << 2)) != 0)
                {
                    Console.WriteLine("输出接口包含：RS232");
                }
                else
                {
                    Console.WriteLine("输出接口不包含：RS232");
                }
                if ((OutPort & (1 << 3)) != 0)
                {
                    Console.WriteLine("输出接口包含：网口");
                }
                else
                {
                    Console.WriteLine("输出接口不包含：网口");
                }
                if ((OutPort & (1 << 4)) != 0)
                {
                    Console.WriteLine("输出接口包含：继电器");
                }
                else
                {
                    Console.WriteLine("输出接口不包含：继电器");
                }
                TempStr = "查询输出接口成功!";
                TempStrEnglish = "OutInterface query success!";


                Console.WriteLine(TempStr);

                if ((OutPort & (1 << 1)) != 0)
                    GetWeigenStyle(ReaderNum);
                if ((OutPort & (1 << 4)) != 0)
                    GetRelayDelayTime(ReaderNum);
            }
            else
            {
                TempStr = "查询输出接口失败!";
                TempStrEnglish = "OutInterface query fail!";

                Console.WriteLine(TempStr);
            }

        }

        /// <summary>
        /// 设置输出接口
        /// </summary>
        /// <param name="interfaceType">接口类别: 0 RS485 1 韦根 2 RS232 3 网口 4 继电器</param>
        /// <param name="ReaderNum"></param> 
        public void SetOutInterface(int interfaceType, int ReaderNum, int weigenStyle=0, int delayTime=0)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            byte PAddr = 0x00;
            int PLen = 1;
            byte[] PSetData = new byte[1];
            byte[] PRefData = new byte[1024];

            int OutPort = 0; 
            if (interfaceType == 0)
                OutPort = OutPort + 1;
            if (interfaceType == 1)
                OutPort = OutPort + 2;
            if (interfaceType == 2)
                OutPort = OutPort + 4;
            if (interfaceType == 3)
                OutPort = OutPort + 8;
            if (interfaceType == 4)
                OutPort = OutPort + 16;
            PAddr = 0x97;
            PSetData[0] = (byte)(OutPort & 0xFF);
            if (0x00 == Reader.Parameter_SetReader(CS, PAddr, PLen, PSetData, ReaderAddr))
            {
                TempStr = "设置输出接口成功!";
                TempStrEnglish = "OutInterface set success!";
            }
            else
            {
                TempStr = "设置输出接口失败!";
                TempStrEnglish = "OutInterface set fail!";
            }


            Console.WriteLine(TempStr);

            // 韦根
            if (interfaceType == 1)
            {
                SetWeigenStyle(ReaderNum, weigenStyle);
            }

            // 继电器
            if (interfaceType == 4)
            {
                SetRelayDelayTime(ReaderNum, delayTime);
            }
              
        }
        
        /// <summary>
        /// 获取韦根类型
        /// </summary>
        /// <param name="ReaderNum"></param> 
        public void GetWeigenStyle(int ReaderNum)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            byte PAddr = 0x00;
            int PLen = 1;
            byte[] PSetData = new byte[1];
            byte[] PRefData = new byte[1024];
            int Weigen = 0;


            PAddr = 0x98;
            if (0x00 == Reader.Parameter_GetReader(CS, PAddr, PLen, PRefData, ReaderAddr))
            {
                Weigen = (int)PRefData[0];

                if (Weigen == 0)
                {
                    Console.WriteLine("韦根类型为weigen26");
                }
                else
                {
                    Console.WriteLine("韦根类型为weigen34");
                }

                TempStr = "查询韦根类型成功!";
                TempStrEnglish = "Weigen style query success!";
            }
            else
            {
                TempStr = "查询韦根类型失败!";
                TempStrEnglish = "Weigen style query fail!";
            }

            Console.WriteLine(TempStr);
        }
        
        /// <summary>
        /// 设置韦根类型
        /// </summary>
        /// <param name="ReaderNum"></param>
        /// <param name="styleValue">0 weigen26 1 weigen34</param>
        public void SetWeigenStyle(int ReaderNum, int styleValue)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            byte PAddr = 0x00;
            int PLen = 1;
            byte[] PSetData = new byte[1];
            byte[] PRefData = new byte[1024];
            int Weigen = 0;

            Weigen = styleValue;
            PSetData[0] = (byte)(Weigen & 0xFF);
            PAddr = 0x98;
            if (0x00 == Reader.Parameter_SetReader(CS, PAddr, PLen, PSetData, ReaderAddr))
            {
                TempStr = "设置韦根类型成功!";
                TempStrEnglish = "Weigen style set success!";
            }
            else
            {
                TempStr = "设置韦根类型失败!";
                TempStrEnglish = "Weigen style set fail!";
            } 

            Console.WriteLine(TempStr);
        }
               
        /// <summary>
        /// 获取继电器时间
        /// </summary>
        /// <param name="ReaderNum"></param>
        public void GetRelayDelayTime(int ReaderNum)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            byte PAddr = 0x00;
            int PLen = 1;
            byte[] PSetData = new byte[1];
            byte[] PRefData = new byte[1024];
            int Relay = 0;
             
            PAddr = 0x99;
            if (0x00 == Reader.Parameter_GetReader(CS, PAddr, PLen, PRefData, ReaderAddr))
            {
                Relay = (int)PRefData[0];

                Console.WriteLine("保持时间设置为" + Relay + "s");

                TempStr = "查询继电器保持时间成功!";
                TempStrEnglish = "Relay delaytime query success!";
            }
            else
            {
                TempStr = "查询继电器保持时间成功!";
                TempStrEnglish = "Relay delaytime query success!";
            }

            Console.WriteLine(TempStr);
        }
        
        /// <summary>
        /// 设置继电器时间
        /// </summary>
        /// <param name="ReaderNum"></param>
        /// <param name="value">1s ~ 180s</param>
        public void SetRelayDelayTime(int ReaderNum, int value)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            byte PAddr = 0x00;
            int PLen = 1;
            byte[] PSetData = new byte[1];
            byte[] PRefData = new byte[1024];
            int Relay = 0;


            Relay = value;
            if (Relay == 0)
            {
                TempStr = "继电器保持时间不能为空或者0!";
                TempStrEnglish = "Relay delaytime doesn't null or zero!";
            }
            else
            {
                PSetData[0] = (byte)(Relay & 0xFF);
                PAddr = 0x99;
                if (0x00 == Reader.Parameter_SetReader(CS, PAddr, PLen, PSetData, ReaderAddr))
                {
                    TempStr = "设置继电器保持时间成功!";
                    TempStrEnglish = "Relay delaytime set success!";
                }
                else
                {
                    TempStr = "设置继电器保持时间失败!";
                    TempStrEnglish = "Relay delaytime set fail!";
                }
            }

            Console.WriteLine(TempStr);
        }
        
        /// <summary>
        /// 获取触发模式的时间
        /// </summary>
        /// <param name="ReaderNum"></param>
        /// <param name="Text"></param>
        public void GetTriggerTime(int ReaderNum, string Text)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            byte PAddr = 0x92;
            int PLen = 1;
            int triggertime = 0;
            byte[] PSetData = new byte[1];
            byte[] PRefData = new byte[1024];

            if (0x00 == Reader.Parameter_GetReader(CS, PAddr, PLen, PRefData, ReaderAddr))
            {
                triggertime = (int)PRefData[0];
                Console.WriteLine($"触发时间： {triggertime}s");
                TempStr = "查询触发时间成功!";
                TempStrEnglish = "Trigger time query success!";
            }
            else
            {
                TempStr = "查询触发时间失败!";
                TempStrEnglish = "Trigger time query fail!";
            }
        }
                       
        /// <summary>
        /// 设置触发模式的时间
        /// </summary>
        /// <param name="ReaderNum"></param>
        /// <param name="value">1 - 180s</param>
        /// <returns></returns>
        public void SetTriggerTime(int ReaderNum, int value)
        {
            string TempStr = "";
            string TempStrEnglish = "";
            byte ReaderAddr = (byte)(ReaderNum & 0xFF);
            byte PAddr = 0x92;
            int PLen = 1;
            int triggertime = 0;
            byte[] PSetData = new byte[1];
            byte[] PRefData = new byte[1024];
            triggertime = value;
            if (triggertime <= 0)
            {
                TempStr = "触发时间不能为空或者0!";
                TempStrEnglish = "Trigger time doesn't null or zero!";
            }
            else
            {
                PSetData[0] = (byte)(triggertime & 0xFF);

                if (0x00 == Reader.Parameter_SetReader(CS, PAddr, PLen, PSetData, ReaderAddr))
                {
                    TempStr = "设置触发时间成功!";
                    TempStrEnglish = "Trigger time set success!";
                }
                else
                {
                    TempStr = "设置触发时间失败!";
                    TempStrEnglish = "Trigger time set fail!";
                }

            }

            Console.WriteLine(TempStr);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 16进制字符串转字节数组
        /// </summary>
        /// <param name="byteT">目标数组</param>
        /// <param name="str">源字符串</param>
        static void HexStrToByte(byte[] byteT, string str)
        {
            string tmp = "";
            for (int i = 0; i < str.Length / 2; i++)
            {
                tmp = str.Substring(i * 2, 2);
                byteT[i] = Convert.ToByte(tmp, 16);
            }
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="byte_arr"></param>
        /// <param name="arr_len"></param>
        /// <returns></returns>
        static string ByteToHexStr(byte[] byte_arr, int arr_len)
        {
            string hexstr = "";
            for (int i = 0; i < arr_len; i++)
            {
                char hex1;
                char hex2;
                int value = byte_arr[i];
                int v1 = value / 16;
                int v2 = value % 16;
                //将商转换为字母(change consult to letter)
                if (v1 >= 0 && v1 <= 9)
                {
                    hex1 = (char)(48 + v1);
                }
                else
                {
                    hex1 = (char)(55 + v1);
                }
                //将余数转成字母(change remainder to letter)
                if (v2 >= 0 && v2 <= 9)
                {
                    hex2 = (char)(48 + v2);
                }
                else
                {
                    hex2 = (char)(55 + v2);
                }
                //将字母连成一串(make letter a string)
                hexstr = hexstr + hex1 + hex2;
            }
            return hexstr;
        }

        /// <summary>
        /// 将16进制的BYTE转换为十进制的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static string IntToInt_Str(int value)
        {
            string str = string.Format("{0:D}", value);
            return str;
        }
        /// <summary>
        /// 将十六进制BYTE转换为IP地址型字符串
        /// </summary>
        /// <param name="ToStr"></param>
        /// <param name="Buf"></param>
        static void ByteToDecimalstr(ref string ToStr, byte[] Buf)
        {
            for (int i = 0; i < 4; i++)
            {
                ToStr = ToStr + IntToInt_Str((int)Buf[i]);
                if (3 != i)
                    ToStr = ToStr + ".";
            }
        }

        #endregion


        #region 私有方法
               
        private int ComConnect(string comNo, int baud)
        {
            int status = Reader.Serial_OpenSeries(ref hCom, comNo, baud);
            if (status == RFID_StandardProtocol.SUCCESS)
            {
                return 0;       //连接成功(connect success)
            }
            else
            {
                return -1;      //连接失败(connect fail)
            }
        }

        private int SocketConnect(string ipString, string portString)
        {
            IPAddress ipaddr = IPAddress.Any;
            if (!IPAddress.TryParse(ipString, out ipaddr))
            {
                return -3;
            }

            int port = 0;
            if (!Int32.TryParse(portString, out port))
            {
                return -3;
            }

            else
            {
                int status = Reader.Socket_ConnectSocket(ref ClientSocket, ipString, port);
                if (status == RFID_StandardProtocol.SUCCESS)
                {
                    return 0;       //连接成功(connect success)
                }
                else
                {
                    return -1;      //连接失败(connect fail)
                }
            }
        }

        private int SocketDisConnect()
        {
            if (ClientSocket != null)
            {
                if (Reader.Socket_CloseSocket(ClientSocket) == RFID_StandardProtocol.SUCCESS)
                {
                    CS.sockClient = null;
                    return 0;		//关闭成功(close success)
                }
                else
                    return -1;		//关闭失败(close fail)
            }
            else
            {
                return -2;			//句柄无效(handle unavailable)
            }
        }
        #endregion
    }
}

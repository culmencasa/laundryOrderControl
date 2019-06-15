using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace DLL
{
	public class RFID_StandardProtocol
	{
		private struct DCB
		{
			public int DCBlength;

			public int BaudRate;

			public uint flags;

			public ushort wReserved;

			public ushort XonLim;

			public ushort XoffLim;

			public byte ByteSize;

			public byte Parity;

			public byte StopBits;

			public byte XonChar;

			public byte XoffChar;

			public byte ErrorChar;

			public byte EofChar;

			public byte EvtChar;

			public ushort wReserved1;
		}

		private struct COMMTIMEOUTS
		{
			public int ReadIntervalTimeout;

			public int ReadTotalTimeoutMultiplier;

			public int ReadTotalTimeoutConstant;

			public int WriteTotalTimeoutMultiplier;

			public int WriteTotalTimeoutConstant;
		}

		private struct OVERLAPPED
		{
			public int Internal;

			public int InternalHigh;

			public int Offset;

			public int OffsetHigh;

			public int hEvent;
		}

		private const string DLLPATH = "kernel32.dll";

		public const int SUCCESS = 0;

		public const int HANDELNULL = 1;

		public const int LOADWSADATAERROR = -1;

		public const int CONNECTERROR = -2;

		public const int OPENCOMERROR = -3;

		public const int GETCOMSTATEERROR = -4;

		public const int SETCOMSTATEERROR = -5;

		public const int GETCOMTIMEOUTERROR = -6;

		public const int SETCOMTIMEOUTERROR = -7;

		public const int CLOSEHANDELERROR = -8;

		public const int SENDERROR = 101;

		public const int RECVERROR = 201;

		public const int DATAERROR = 301;

		public const int CHECKERROR = 401;

		private const uint GENERIC_READ = 2147483648u;

		private const uint GENERIC_WRITE = 1073741824u;

		private const int OPEN_EXISTING = 3;

		private const int INVALID_HANDLE_VALUE = -1;

		private const int PURGE_RXABORT = 2;

		private const int PURGE_RXCLEAR = 8;

		private const int PURGE_TXABORT = 1;

		private const int PURGE_TXCLEAR = 4;

		public int BaudRate = 115200;

		public byte ByteSize = 8;

		public byte Parity = 0;

		public byte StopBits = 0;

		public int ReadTimeout = 4000;

		public bool Opened = false;

		[DllImport("kernel32.dll")]
		private static extern int CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode, int lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);

		[DllImport("kernel32.dll")]
		private static extern bool GetCommState(int hFile, ref DCB lpDCB);

		[DllImport("kernel32.dll")]
		private static extern bool SetCommState(int hFile, ref DCB lpDCB);

		[DllImport("kernel32.dll")]
		private static extern bool GetCommTimeouts(int hFile, ref COMMTIMEOUTS lpCommTimeouts);

		[DllImport("kernel32.dll")]
		private static extern bool SetCommTimeouts(int hFile, ref COMMTIMEOUTS lpCommTimeouts);

		[DllImport("kernel32.dll")]
		private static extern bool SetupComm(int hFile, int nInQueue, int nOutQueue);

		[DllImport("kernel32.dll")]
		private static extern bool ReadFile(int hFile, byte[] lpBuffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, ref OVERLAPPED lpOverlapped);

		[DllImport("kernel32.dll")]
		private static extern bool WriteFile(int hFile, byte[] lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, ref OVERLAPPED lpOverlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FlushFileBuffers(int hFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool PurgeComm(int hFile, uint dwFlags);

		[DllImport("kernel32.dll")]
		private static extern bool CloseHandle(int hObject);

		[DllImport("kernel32.dll")]
		private static extern uint GetLastError();

		public int Socket_ConnectSocket(ref Socket Sock, string IPAddr, int Port)
		{
            ManualResetEvent TimeoutObject = new ManualResetEvent(false);
            TimeoutObject.Reset();

            IPAddress address = IPAddress.Parse(IPAddr);
			IPEndPoint remoteEP = new IPEndPoint(address, Port);
			Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
                Sock.BeginConnect(remoteEP, (asyncresult) => {            
                    //使阻塞的线程继续        
                    TimeoutObject.Set();
                }, Sock);
                //阻塞当前线程           
                if (TimeoutObject.WaitOne(4000, false))
                {
                    //MessageBox.Show("网络正常");
                    return 0;
                }
                else
                {
                    //MessageBox.Show("连接超时");
                    return -2;
                }
			}
			catch (Exception)
			{
				return -2;
			}
		}

		public int Socket_SendAndRecv(Socket Sock, byte[] SendBuf, byte[] RecvBuf, int send_len)
		{
			int num = send_len - 1;
			byte[] array = new byte[3];
			SendBuf[num] = 0;
			for (int i = 0; i < num; i++)
			{
				SendBuf[num] += SendBuf[i];
			}
			SendBuf[num] = (byte)(~SendBuf[num] + 1);
			if (Sock.Send(SendBuf, send_len, SocketFlags.None) != send_len)
			{
				return 101;
			}
			if (Sock.Receive(array, 3, SocketFlags.None) != 3)
			{
				return 201;
			}
			if (array[0] == 11)
			{
				int num2 = array[2];
				if (Sock.Receive(RecvBuf, num2, SocketFlags.None) == num2 && RecvBuf[0] == 0)
				{
					return 0;
				}
				return 301;
			}
			return 0;
		}

		public int Socket_CloseSocket(Socket Sock)
		{
			if (Sock != null)
			{
				Sock.Close();
				return 0;
			}
			return -8;
		}

		private void SetDcbFlag(int whichFlag, int setting, DCB dcb)
		{
			setting <<= whichFlag;
			uint num = (whichFlag == 4 || whichFlag == 12) ? 3u : ((whichFlag != 15) ? 1u : 131071u);
			dcb.flags &= ~(num << whichFlag);
			dcb.flags |= (uint)setting;
		}

		public int Serial_OpenSeries(ref int hComm, string ComName, int Baud)
		{
			DCB lpDCB = default(DCB);
			COMMTIMEOUTS lpCommTimeouts = default(COMMTIMEOUTS);
			hComm = CreateFile(ComName, 3221225472u, 0, 0, 3, 0, 0);
			if (hComm == -1)
			{
				return -3;
			}
			if (!GetCommTimeouts(hComm, ref lpCommTimeouts))
			{
				return -6;
			}
			lpCommTimeouts.ReadTotalTimeoutConstant = 3000;
			lpCommTimeouts.ReadTotalTimeoutMultiplier = 1;
			lpCommTimeouts.WriteTotalTimeoutMultiplier = 1;
			lpCommTimeouts.WriteTotalTimeoutConstant = 100;
			if (!SetCommTimeouts(hComm, ref lpCommTimeouts))
			{
				return -7;
			}
			if (!GetCommState(hComm, ref lpDCB))
			{
				return -4;
			}
			lpDCB.DCBlength = Marshal.SizeOf(lpDCB);
			lpDCB.BaudRate = Baud;
			lpDCB.flags = 0u;
			lpDCB.ByteSize = ByteSize;
			lpDCB.StopBits = StopBits;
			lpDCB.Parity = Parity;
			if (!SetCommState(hComm, ref lpDCB))
			{
				return -5;
			}
			Opened = true;
			return 0;
		}

		public int Serial_SendAndRecv(int hComm, byte[] SendBuf, byte[] RecvBuf, int send_len)
		{
			int num = send_len - 1;
			SendBuf[num] = 0;
			for (int i = 0; i < num; i++)
			{
				SendBuf[num] += SendBuf[i];
			}
			SendBuf[num] = (byte)(~SendBuf[num] + 1);
			byte[] array = new byte[3];
			int lpNumberOfBytesWritten = 0;
			int lpNumberOfBytesRead = 0;
			OVERLAPPED lpOverlapped = default(OVERLAPPED);
			if (hComm != -1)
			{
				PurgeComm(hComm, 10u);
				PurgeComm(hComm, 5u);
			}
			if (WriteFile(hComm, SendBuf, send_len, ref lpNumberOfBytesWritten, ref lpOverlapped))
			{
				if (lpNumberOfBytesWritten != send_len)
				{
					PurgeComm(hComm, 8u);
					return 101;
				}
				if (ReadFile(hComm, array, 3, ref lpNumberOfBytesRead, ref lpOverlapped))
				{
					if (lpNumberOfBytesRead != 3)
					{
						PurgeComm(hComm, 4u);
						return 201;
					}
					if (array[0] == 11)
					{
						int num2 = array[2];
						if (ReadFile(hComm, RecvBuf, num2, ref lpNumberOfBytesRead, ref lpOverlapped))
						{
							if (lpNumberOfBytesRead == num2 && RecvBuf[0] == 0)
							{
								return 0;
							}
							return 301;
						}
					}
				}
				return 0;
			}
			return 201;
		}

		public int Serial_CloseSeries(int hComm)
		{
			if (hComm != -1)
			{
				if (CloseHandle(hComm))
				{
					return 0;
				}
				return 1;
			}
			return -8;
		}

		public int Config_GetLocatorVersion(hComSocket CS, ref byte major, ref byte minor, byte ReaderAddr)
		{
			int num = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[5]
			{
				10,
				0,
				2,
				34,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				num = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				num = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			if (0 != num)
			{
				return num;
			}
			major = array[1];
			minor = array[2];
			return num;
		}

		public int Config_SetReaderBaud(hComSocket CS, byte Parameter, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[6]
			{
				10,
				0,
				3,
				32,
				0,
				0
			};
			array[1] = ReaderAddr;
			array[4] = Parameter;
			byte[] sendBuf = array;
			int send_len = 6;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
			}
			return result;
		}

		public int Config_ResetReader(hComSocket CS, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[5]
			{
				10,
				0,
				2,
				33,
				0
			};
			array[1] = ReaderAddr;
			byte[] sendBuf = array;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
			}
			return result;
		}

		public int Config_SetRfPower(hComSocket CS, byte[] Pwr, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[9]
			{
				10,
				0,
				6,
				37,
				0,
				0,
				0,
				0,
				0
			};
			array[1] = ReaderAddr;
			array[4] = Pwr[0];
			array[5] = Pwr[1];
			array[6] = Pwr[2];
			array[7] = Pwr[3];
			byte[] sendBuf = array;
			int send_len = 9;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
			}
			return result;
		}

		public int Config_GetRfPower(hComSocket CS, byte[] Pwr, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[5]
			{
				10,
				0,
				2,
				38,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			Pwr[0] = array[1];
			Pwr[1] = array[2];
			Pwr[2] = array[3];
			Pwr[3] = array[4];
			return result;
		}

		public int Config_SetFreqPoint(hComSocket CS, int Freq_num, int[] Freq_points, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[150];
			int send_len;
			if (0 == Freq_num)
			{
				array[0] = 10;
				array[1] = ReaderAddr;
				array[2] = 4;
				array[3] = 39;
				array[4] = 0;
				array[5] = (byte)(Freq_points[0] & 0xFF);
				array[6] = 0;
				send_len = 7;
			}
			else
			{
				array[0] = 10;
				array[1] = ReaderAddr;
				array[2] = (byte)((Freq_num + 3) & 0xFF);
				array[3] = 39;
				array[4] = (byte)(Freq_num & 0xFF);
				for (int i = 0; i < Freq_num; i++)
				{
					array[5 + i] = (byte)(Freq_points[i] & 0xFF);
				}
				send_len = Freq_num + 6;
			}
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array, recvBuf, send_len);
			}
			return result;
		}

		public int Config_GetFreqPoint(hComSocket CS, ref int Freq_num, int[] Freq_points, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[5]
			{
				10,
				0,
				2,
				40,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			Freq_num = array[1];
			if (0 == Freq_num)
			{
				Freq_points[0] = array[2];
			}
			else
			{
				for (int i = 0; i < Freq_num; i++)
				{
					Freq_points[i] = array[2 + i];
				}
			}
			return result;
		}

		public int Config_SetAntenna(hComSocket CS, int WorkAnt, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[6]
			{
				10,
				0,
				3,
				41,
				0,
				0
			};
			array[1] = ReaderAddr;
			array[4] = (byte)(WorkAnt & 0xFF);
			byte[] sendBuf = array;
			int send_len = 6;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
			}
			return result;
		}

		public int Config_GetAntenna(hComSocket CS, ref int WorkAnt, ref int AntStatus, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[5]
			{
				10,
				0,
				2,
				42,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			WorkAnt = array[1];
			AntStatus = array[2];
			return result;
		}

		public int Config_SetSingleFastTagMode(hComSocket CS, byte Mode, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[6]
			{
				10,
				0,
				3,
				21,
				0,
				0
			};
			array[1] = ReaderAddr;
			array[4] = Mode;
			byte[] sendBuf = array;
			int send_len = 6;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
			}
			return result;
		}

		public int Config_GetSingleFastTagMode(hComSocket CS, ref byte Mode, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[5]
			{
				10,
				0,
				2,
				22,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			Mode = array[1];
			return result;
		}

		public int Config_SetTestMode(hComSocket CS, byte TestMode, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[6]
			{
				10,
				0,
				3,
				47,
				0,
				0
			};
			array[1] = ReaderAddr;
			array[4] = TestMode;
			byte[] sendBuf = array;
			int send_len = 6;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
			}
			return result;
		}

		public int Config_SetOutPort(hComSocket CS, byte Num, byte Level, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[7]
			{
				10,
				0,
				4,
				45,
				0,
				0,
				0
			};
			array[1] = ReaderAddr;
			array[4] = Num;
			array[5] = Level;
			byte[] sendBuf = array;
			int send_len = 7;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
			}
			return result;
		}

		public int Config_GetInPort(hComSocket CS, byte InPortNum, ref byte InPortLevel, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[6]
			{
				10,
				0,
				3,
				46,
				0,
				0
			};
			array2[1] = ReaderAddr;
			array2[4] = InPortNum;
			byte[] sendBuf = array2;
			int send_len = 6;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			InPortLevel = array[1];
			return result;
		}

		public int Config_SetIntrnetAccess(hComSocket CS, byte[] IPAddr, byte[] MaskCode, byte[] GateWay, byte[] InternetPort, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] sendBuf = new byte[19]
			{
				10,
				ReaderAddr,
				16,
				44,
				IPAddr[0],
				IPAddr[1],
				IPAddr[2],
				IPAddr[3],
				MaskCode[0],
				MaskCode[1],
				MaskCode[2],
				MaskCode[3],
				GateWay[0],
				GateWay[1],
				GateWay[2],
				GateWay[3],
				InternetPort[0],
				InternetPort[1],
				0
			};
			int send_len = 19;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
			}
			return result;
		}

		public int Config_GetIntrnetAccess(hComSocket CS, byte[] IPAddr, byte[] MaskCode, byte[] GateWay, byte[] InternetPort, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[5]
			{
				10,
				0,
				2,
				43,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			IPAddr[0] = array[1];
			IPAddr[1] = array[2];
			IPAddr[2] = array[3];
			IPAddr[3] = array[4];
			MaskCode[0] = array[5];
			MaskCode[1] = array[6];
			MaskCode[2] = array[7];
			MaskCode[3] = array[8];
			GateWay[0] = array[9];
			GateWay[1] = array[10];
			GateWay[2] = array[11];
			GateWay[3] = array[12];
			InternetPort[0] = array[13];
			InternetPort[1] = array[14];
			return result;
		}

		public int Config_SetReaderTime(hComSocket CS, ReaderTime RTime, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[12]
			{
				10,
				0,
				9,
				16,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			array[1] = ReaderAddr;
			byte[] array2 = array;
			int send_len = 12;
			array2[4] = RTime.YearH;
			array2[5] = RTime.YearL;
			array2[6] = RTime.Month;
			array2[7] = RTime.Day;
			array2[8] = RTime.Hour;
			array2[9] = RTime.Minute;
			array2[10] = RTime.Second;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array2, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array2, recvBuf, send_len);
			}
			return result;
		}

		public int Config_GetReaderTime(hComSocket CS, ref ReaderTime RTime, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[12]
			{
				10,
				0,
				2,
				17,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			RTime.YearH = array[1];
			RTime.YearL = array[2];
			RTime.Month = array[3];
			RTime.Day = array[4];
			RTime.Hour = array[5];
			RTime.Minute = array[6];
			RTime.Second = array[7];
			return result;
		}

		public int ISO_MultiTagIdentify(hComSocket CS, ref int TagCount, byte ReaderAddr)
		{
			int num = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[5]
			{
				10,
				0,
				2,
				96,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				num = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				num = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			if (0 != num)
			{
				return num;
			}
			TagCount = array[1];
			return num;
		}

		public int ISO_MultiTagRead(hComSocket CS, int RAddr, ref int RCount, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[6]
			{
				10,
				0,
				3,
				97,
				0,
				0
			};
			array2[1] = ReaderAddr;
			byte[] array3 = array2;
			int send_len = 6;
			array3[4] = (byte)(RAddr & 0xFF);
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array3, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array3, array, send_len);
			}
			RCount = array[1];
			return result;
		}

		public int ISO_TagRead(hComSocket CS, byte RAddr, byte[] RData, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[6]
			{
				10,
				0,
				3,
				104,
				0,
				0
			};
			array2[1] = ReaderAddr;
			byte[] array3 = array2;
			int send_len = 6;
			array3[4] = (byte)(RAddr & 0xFF);
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array3, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array3, array, send_len);
			}
			for (int i = 0; i < 9; i++)
			{
				RData[i] = array[1 + i];
			}
			return result;
		}

		public int ISO_TagWrite(hComSocket CS, int WAddr, byte[] WData, int DataLen, byte ReaderAddr)
		{
			int num = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[7]
			{
				10,
				0,
				4,
				98,
				0,
				0,
				0
			};
			array2[1] = ReaderAddr;
			byte[] array3 = array2;
			int num2 = 0;
			for (int i = 0; i < DataLen; i++)
			{
				int send_len = 7;
				array3[4] = (byte)((WAddr + i) & 0xFF);
				array3[5] = WData[i];
				if (CS.hCom != -1 && CS.sockClient == null)
				{
					num = Serial_SendAndRecv(CS.hCom, array3, array, send_len);
				}
				if (CS.hCom == -1 && CS.sockClient != null)
				{
					num = Socket_SendAndRecv(CS.sockClient, array3, array, send_len);
				}
				if (0 == num)
				{
					num2 = 0;
				}
				else
				{
					num2++;
					if (num2 == 8)
					{
						return num;
					}
					i--;
				}
				Array.Clear(array, 0, 1024);
			}
			return num;
		}

		public int ISO_TagReadWithUID(hComSocket CS, byte[] UID, int RAddr, byte[] RData, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[14]
			{
				10,
				0,
				11,
				99,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			array2[1] = ReaderAddr;
			byte[] array3 = array2;
			int send_len = 14;
			for (int i = 0; i < 8; i++)
			{
				array3[4 + i] = UID[i];
			}
			array3[12] = (byte)(RAddr & 0xFF);
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array3, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array3, array, send_len);
			}
			for (int i = 0; i < 9; i++)
			{
				RData[i] = array[1 + i];
			}
			return result;
		}

		public int ISO_TagWriteWithUID(hComSocket CS, byte[] UID, int WAddr, byte[] WData, int DataLen, byte ReaderAddr)
		{
			int num = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[15]
			{
				10,
				0,
				12,
				100,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			array2[1] = ReaderAddr;
			byte[] array3 = array2;
			for (int i = 0; i < 8; i++)
			{
				array3[4 + i] = UID[i];
			}
			int num2 = 0;
			for (int i = 0; i < DataLen; i++)
			{
				array3[12] = (byte)((WAddr + i) & 0xFF);
				array3[13] = WData[i];
				int send_len = 15;
				if (CS.hCom != -1 && CS.sockClient == null)
				{
					num = Serial_SendAndRecv(CS.hCom, array3, array, send_len);
				}
				if (CS.hCom == -1 && CS.sockClient != null)
				{
					num = Socket_SendAndRecv(CS.sockClient, array3, array, send_len);
				}
				if (0 == num)
				{
					num2 = 0;
				}
				else
				{
					num2++;
					if (num2 == 8)
					{
						return num;
					}
					i--;
				}
				Array.Clear(array, 0, 1024);
			}
			return num;
		}

		public int ISO_SetTagLock(hComSocket CS, int LAddr, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[6]
			{
				10,
				0,
				3,
				101,
				0,
				0
			};
			array[1] = ReaderAddr;
			byte[] array2 = array;
			int send_len = 6;
			array2[4] = (byte)(LAddr & 0xFF);
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array2, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array2, recvBuf, send_len);
			}
			return result;
		}

		public int ISO_QueryTagLock(hComSocket CS, int QAddr, ref int LStatus, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[6]
			{
				10,
				0,
				3,
				102,
				0,
				0
			};
			array2[1] = ReaderAddr;
			byte[] array3 = array2;
			int send_len = 6;
			array3[4] = (byte)(QAddr & 0xFF);
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array3, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array3, array, send_len);
			}
			LStatus = array[1];
			return result;
		}

		public int ISO_SetTagLockWithUID(hComSocket CS, byte[] UID, int LAddr, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[14]
			{
				10,
				0,
				11,
				105,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			array[1] = ReaderAddr;
			byte[] array2 = array;
			int send_len = 14;
			for (int i = 0; i < 8; i++)
			{
				array2[4 + i] = UID[i];
			}
			array2[12] = (byte)(LAddr & 0xFF);
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array2, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array2, recvBuf, send_len);
			}
			return result;
		}

		public int ISO_QueryTagLockWithUID(hComSocket CS, byte[] UID, int QAddr, ref int LStatus, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[14]
			{
				10,
				0,
				11,
				106,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			array2[1] = ReaderAddr;
			byte[] array3 = array2;
			int send_len = 14;
			for (int i = 0; i < 8; i++)
			{
				array3[4 + i] = UID[i];
			}
			array3[12] = (byte)(QAddr & 0xFF);
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array3, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array3, array, send_len);
			}
			LStatus = array[1];
			return result;
		}

		public int GEN2_MultiTagInventory(hComSocket CS, ref int TagCount, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[6]
			{
				10,
				0,
				3,
				128,
				0,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 6;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			TagCount = (array[1] << 8) + array[2];
			return result;
		}

		public int GEN2_MultiTagRead(hComSocket CS, WordptrAndLength WpALen, ref int RCount, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[14]
			{
				10,
				0,
				11,
				132,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			array2[1] = ReaderAddr;
			byte[] array3 = array2;
			int send_len = 14;
			array3[4] = (byte)(WpALen.MembankMask & 0xFF);
			if (0 != WpALen.ReserveWordCnt)
			{
				array3[5] = (byte)(WpALen.ReserveWordPtr & 0xFF);
				array3[6] = (byte)(WpALen.ReserveWordCnt & 0xFF);
			}
			if (0 != WpALen.EpcWordCnt)
			{
				array3[7] = (byte)(WpALen.EpcWordPtr & 0xFF);
				array3[8] = (byte)(WpALen.EpcWordCnt & 0xFF);
			}
			if (0 != WpALen.TidWordCnt)
			{
				array3[9] = (byte)(WpALen.TidWordPtr & 0xFF);
				array3[10] = (byte)(WpALen.TidWordCnt & 0xFF);
			}
			if (0 != WpALen.UserWordCnt)
			{
				array3[11] = (byte)(WpALen.UserWordPtr & 0xFF);
				array3[12] = (byte)(WpALen.UserWordCnt & 0xFF);
			}
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array3, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array3, array, send_len);
			}
			RCount = (array[1] << 8) + array[2];
			return result;
		}

		public int GEN2_MultiTagWrite(hComSocket CS, MutiWriteParam MutiWP, ref int WCount, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[36]
			{
				10,
				ReaderAddr,
				(byte)((5 + MutiWP.WriteLen * 2) & 0xFF),
				133,
				(byte)(MutiWP.MemBank & 0xFF),
				(byte)(MutiWP.StartAddr & 0xFF),
				(byte)(MutiWP.WriteLen & 0xFF),
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			for (int i = 0; i < MutiWP.WriteLen * 2; i++)
			{
				array2[i + 7] = MutiWP.WriteValue[i];
			}
			int send_len = MutiWP.WriteLen * 2 + 8;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array2, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array2, array, send_len);
			}
			WCount = (array[1] << 8) + array[2];
			return result;
		}

		public int GEN2_KillTag(hComSocket CS, byte[] KPassWord, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[9]
			{
				10,
				0,
				6,
				131,
				0,
				0,
				0,
				0,
				0
			};
			array[1] = ReaderAddr;
			array[4] = KPassWord[0];
			array[5] = KPassWord[1];
			array[6] = KPassWord[2];
			array[7] = KPassWord[3];
			byte[] sendBuf = array;
			int send_len = 9;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
			}
			return result;
		}

		public int GEN2_SecRead(hComSocket CS, int Membank, byte[] RPassWord, int RAddr, int RCnt, byte[] RData, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[12]
			{
				10,
				0,
				9,
				136,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			array2[1] = ReaderAddr;
			byte[] array3 = array2;
			int send_len = 12;
			array3[4] = RPassWord[0];
			array3[5] = RPassWord[1];
			array3[6] = RPassWord[2];
			array3[7] = RPassWord[3];
			array3[8] = (byte)(Membank & 0xFF);
			array3[9] = (byte)(RAddr & 0xFF);
			array3[10] = (byte)(RCnt & 0xFF);
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array3, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array3, array, send_len);
			}
			for (int i = 0; i < RCnt * 2; i++)
			{
				RData[i] = array[i + 2];
			}
			return result;
		}

		public int GEN2_SecWrite(hComSocket CS, int Membank, byte[] WPassWord, int WAddr, int WCnt, byte[] WData, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[13];
			int i;
			for (i = 0; i < WCnt; i++)
			{
				Array.Clear(array, 0, 1024);
				Array.Clear(array2, 0, 13);
				array2[0] = 10;
				array2[1] = ReaderAddr;
				array2[2] = 10;
				array2[3] = 137;
				array2[4] = WPassWord[0];
				array2[5] = WPassWord[1];
				array2[6] = WPassWord[2];
				array2[7] = WPassWord[3];
				array2[8] = (byte)(Membank & 0xFF);
				array2[9] = (byte)((i + WAddr) & 0xFF);
				array2[10] = WData[i * 2];
				array2[11] = WData[i * 2 + 1];
				array2[12] = 0;
				int send_len = 13;
				if (CS.hCom != -1 && CS.sockClient == null)
				{
					result = Serial_SendAndRecv(CS.hCom, array2, array, send_len);
				}
				if (CS.hCom == -1 && CS.sockClient != null)
				{
					result = Socket_SendAndRecv(CS.sockClient, array2, array, send_len);
				}
			}
			if (i == WCnt)
			{
				return result;
			}
			return result;
		}

		public int GEN2_SecLock(hComSocket CS, int Membank, byte[] LPassWord, int LockLevel, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[11]
			{
				10,
				0,
				8,
				138,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			array[1] = ReaderAddr;
			byte[] array2 = array;
			int send_len = 11;
			array2[4] = LPassWord[0];
			array2[5] = LPassWord[1];
			array2[6] = LPassWord[2];
			array2[7] = LPassWord[3];
			array2[8] = (byte)(Membank & 0xFF);
			array2[9] = (byte)(LockLevel & 0xFF);
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array2, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array2, recvBuf, send_len);
			}
			return result;
		}

		public int GEN2_SecSelectConfig(hComSocket CS, int Membank, int CAciton, int CAddr, int CLen, byte[] CData, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[36]
			{
				10,
				ReaderAddr,
				(byte)((CLen + 1 + 7) & 0xFF),
				143,
				(byte)(CAciton & 0xFF),
				(byte)(Membank & 0xFF),
				(byte)(CAddr * 16 >> 8),
				(byte)((CAddr * 16) & 0xFF),
				(byte)(((CLen + 1) * 8) & 0xFF),
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			for (int i = 0; i < CLen + 1; i++)
			{
				array[i + 9] = CData[i];
			}
			int send_len = CLen + 1 + 10;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array, recvBuf, send_len);
			}
			return result;
		}

		public int BufferM_GetTagData(hComSocket CS, ref int GetCount, BufferData[] BData, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[6]
			{
				10,
				0,
				3,
				65,
				16,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 6;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			int num = 0;
			int num2 = array[1];
			for (int i = 0; i < num2; i++)
			{
				BData[i].Len = array[2 + num];
				BData[i].Ant = array[3 + num];
				for (int j = 0; j < BData[i].Len - 1; j++)
				{
					BData[i].Data[j] = array[4 + j + num];
				}
				num += BData[i].Len + 1;
			}
			GetCount = num2;
			return result;
		}

		public int BufferM_QueryIDCount(hComSocket CS, ref int TagCount, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[5]
			{
				10,
				0,
				2,
				67,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			TagCount = (array[1] << 8) + array[2];
			return result;
		}

		public int BufferM_ClearBuffer(hComSocket CS, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[5]
			{
				10,
				0,
				2,
				68,
				0
			};
			array[1] = ReaderAddr;
			byte[] sendBuf = array;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
			}
			return result;
		}

		public int BufferM_ClearExternalBuffer(hComSocket CS, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[5]
			{
				10,
				0,
				2,
				72,
				0
			};
			array[1] = ReaderAddr;
			byte[] sendBuf = array;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
			}
			return result;
		}

		public int BufferM_GetExternalBufferCount(hComSocket CS, ref int ETagCount, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[5]
			{
				10,
				0,
				2,
				73,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			ETagCount = (array[1] << 8) + array[2];
			return result;
		}

		public int BufferM_GetExternalBufferData(hComSocket CS, BufferData[] BData, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			byte[] array2 = new byte[5]
			{
				10,
				0,
				2,
				74,
				0
			};
			array2[1] = ReaderAddr;
			byte[] sendBuf = array2;
			int send_len = 5;
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
			}
			int num = 0;
			int num2 = array[1];
			for (int i = 0; i < num2; i++)
			{
				BData[i].Len = array[2 + i * 8 + num];
				BData[i].Ant = array[3 + i * 8 + num];
				int len = BData[i].Len;
				for (int j = 0; j < len; j++)
				{
					BData[i].Data[j] = array[4 + j + num];
				}
				num = len + 1;
			}
			return result;
		}

		public int Parameter_SetReader(hComSocket CS, byte PAddr, int PLen, byte[] PSData, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			for (int i = 0; i < PLen; i++)
			{
				byte[] array = new byte[7]
				{
					10,
					0,
					4,
					35,
					0,
					0,
					0
				};
				array[1] = ReaderAddr;
				array[4] = PAddr;
				array[5] = PSData[i];
				byte[] sendBuf = array;
				int send_len = 7;
				if (CS.hCom != -1 && CS.sockClient == null)
				{
					result = Serial_SendAndRecv(CS.hCom, sendBuf, recvBuf, send_len);
				}
				if (CS.hCom == -1 && CS.sockClient != null)
				{
					result = Socket_SendAndRecv(CS.sockClient, sendBuf, recvBuf, send_len);
				}
				PAddr = (byte)(PAddr + 1);
			}
			return result;
		}

		public int Parameter_GetReader(hComSocket CS, byte PAddr, int PLen, byte[] PGData, byte ReaderAddr)
		{
			int result = -1;
			byte[] array = new byte[1024];
			for (int i = 0; i < PLen; i++)
			{
				byte[] array2 = new byte[6]
				{
					10,
					0,
					3,
					36,
					0,
					0
				};
				array2[1] = ReaderAddr;
				array2[4] = PAddr;
				byte[] sendBuf = array2;
				int send_len = 6;
				if (CS.hCom != -1 && CS.sockClient == null)
				{
					result = Serial_SendAndRecv(CS.hCom, sendBuf, array, send_len);
				}
				if (CS.hCom == -1 && CS.sockClient != null)
				{
					result = Socket_SendAndRecv(CS.sockClient, sendBuf, array, send_len);
				}
				PGData[i] = array[1];
				PAddr = (byte)(PAddr + 1);
			}
			return result;
		}

		public int CheckManagePW(hComSocket CS, byte[] MPW, byte ReaderAddr)
		{
			int result = -1;
			byte[] recvBuf = new byte[1024];
			byte[] array = new byte[11]
			{
				10,
				0,
				8,
				48,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			array[1] = ReaderAddr;
			byte[] array2 = array;
			int send_len = 11;
			array2[4] = MPW[0];
			array2[5] = MPW[1];
			array2[6] = MPW[2];
			array2[7] = MPW[3];
			array2[8] = MPW[4];
			array2[9] = MPW[5];
			if (CS.hCom != -1 && CS.sockClient == null)
			{
				result = Serial_SendAndRecv(CS.hCom, array2, recvBuf, send_len);
			}
			if (CS.hCom == -1 && CS.sockClient != null)
			{
				result = Socket_SendAndRecv(CS.sockClient, array2, recvBuf, send_len);
			}
			return result;
		}
	}
}

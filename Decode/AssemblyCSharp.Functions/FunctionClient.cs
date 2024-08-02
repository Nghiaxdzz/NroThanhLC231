using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace AssemblyCSharp.Functions;

internal class FunctionClient
{
	private static FunctionClient _Instance;

	public static string currentUsername;

	public static bool isConnected = false;

	public static bool IsSendMsg = false;

	private static byte[] receivedBuf = new byte[2048];

	public static Socket sender;

	public static FunctionClient gI()
	{
		if (_Instance == null)
		{
			_Instance = new FunctionClient();
		}
		return _Instance;
	}

	public static void Connect(int Port)
	{
		Thread thread = new Thread((ThreadStart)delegate
		{
			try
			{
				sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				sender.Connect(IPAddress.Loopback, Port);
				sender.BeginReceive(receivedBuf, 0, receivedBuf.Length, SocketFlags.None, ReceiveData, sender);
				if (!IsSendMsg)
				{
					gI().sendMessage(new vMessage
					{
						cmd = 0,
						data = currentUsername
					});
					IsSendMsg = true;
				}
				Thread.Sleep(200);
			}
			catch (Exception ex)
			{
				File.WriteAllText("Data/Errors/logSocket.txt", ex.Message);
			}
		});
		thread.IsBackground = true;
		thread.Start();
	}

	private static void onMessage(string data)
	{
		if (!FunctionMain.enableConnectToClient)
		{
			return;
		}
		try
		{
			vMessage vMessage2 = JsonConvert.DeserializeObject<vMessage>(data);
			if (vMessage2 != null)
			{
				FunctionMain.gI().doFireClient(vMessage2);
			}
		}
		catch
		{
		}
	}

	public void sendMessage(object obj)
	{
		string s = JsonConvert.SerializeObject(obj);
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		try
		{
			sender.Send(bytes);
		}
		catch (ObjectDisposedException)
		{
		}
	}

	public static void ReceiveData(IAsyncResult ar)
	{
		Socket socket = (Socket)ar.AsyncState;
		if (socket.Connected)
		{
			int num = 0;
			try
			{
				num = socket.EndReceive(ar);
			}
			catch
			{
			}
			if (num != 0)
			{
				byte[] array = new byte[num];
				Array.Copy(receivedBuf, array, num);
				onMessage(Encoding.UTF8.GetString(array));
				sender.BeginReceive(receivedBuf, 0, receivedBuf.Length, SocketFlags.None, ReceiveData, sender);
				return;
			}
		}
		GameScr.info1.addInfo("Đã ngắt kết nối", 0);
		Connect(FunctionLogin.PortClient);
	}
}

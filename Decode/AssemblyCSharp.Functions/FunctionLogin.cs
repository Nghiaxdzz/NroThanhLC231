using System.IO;
using System.Threading;
using UnityEngine;

namespace AssemblyCSharp.Functions;

internal class FunctionLogin
{
	private static FunctionLogin _Instance;

	public static string Username;

	public static string Password;

	public static string Server;

	public static int PortClient = int.Parse(File.ReadAllText("Data/QLTK/Port.txt"));

	public static bool enableToChangeLogo;

	public static bool enableProxyConnection;

	public static string Proxy_Username;

	public static string Proxy_Password;

	public static string Proxy_IP;

	public static int Proxy_Port;

	public static string typeConnect;

	public static string GraphicsSetting = "Data/QLTK/GraphicsSetting.ini";

	public static string FunctionsSetting = "Data/QLTK/FunctionsSetting.ini";

	public static string Size = "Data/QLTK/Size.ini";

	public static string UUID;

	public static bool enableAutoLogin = true;

	public static bool isLogin;

	public static long timeWaitLogin;

	public static bool updateServer;

	public static long CountLogin;

	public static bool CheckLogin;

	public static long LastTimeUpdateScreen;

	public static FunctionLogin gI()
	{
		if (_Instance == null)
		{
			_Instance = new FunctionLogin();
		}
		return _Instance;
	}

	public static void GetData()
	{
		Username = Main.arguments["acc"];
		Password = Main.arguments["pass"];
		Server = Main.arguments["server"];
		if (FunctionMain.enableConnectToClient)
		{
			FunctionClient.Connect(PortClient);
		}
	}

	public void Login()
	{
		while (!ServerListScreen.loadScreen)
		{
			FileInfo[] files = new DirectoryInfo(Rms.GetiPhoneDocumentsPath() + "/").GetFiles();
			if (files.Length < 100)
			{
				if (!ServerListScreen.isGetData)
				{
					Service.gI().getResource(1, null);
					ServerListScreen.isGetData = true;
				}
				Thread.Sleep(5000);
			}
			else
			{
				Thread.Sleep(100);
			}
		}
		if (ServerListScreen.nameServer[ServerListScreen.ipSelect].ToLower().Replace(" ", "") != Server)
		{
			for (int i = 0; i < ServerListScreen.nameServer.Length; i++)
			{
				if (ServerListScreen.nameServer[i].ToLower().Replace(" ", "") == Server)
				{
					Rms.saveRMSInt("svselect", i);
					ServerListScreen.ipSelect = i;
					GameCanvas.serverScreen.selectServer();
					while (!ServerListScreen.loadScreen)
					{
						Thread.Sleep(10);
					}
					while (!Session_ME.gI().isConnected())
					{
						GameCanvas.serverScreen.switchToMe();
						Thread.Sleep(100);
					}
					Thread.Sleep(100);
					while (!ServerListScreen.loadScreen)
					{
						Thread.Sleep(10);
					}
					Thread.Sleep(1000);
					break;
				}
			}
		}
		if (GameCanvas.loginScr == null)
		{
			GameCanvas.loginScr = new LoginScr();
		}
		GameCanvas.loginScr.switchToMe();
		Service.gI().login(Username, Password, GameMidlet.VERSION, 0);
	}

	public static bool ReturnSetting(string path, int index)
	{
		if (File.Exists(path))
		{
			string[] array = File.ReadAllText(path).Split('|');
			if (array[index] == "True")
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static void LoadDefaultSetting()
	{
		try
		{
			FunctionGraphic.enableHideChar = ReturnSetting(GraphicsSetting, 0);
			FunctionGraphic.enableHideMob = ReturnSetting(GraphicsSetting, 1);
			FunctionGraphic.enableHideNpc = ReturnSetting(GraphicsSetting, 2);
			FunctionGraphic.enableHideItem = ReturnSetting(GraphicsSetting, 3);
			FunctionGraphic.enableHideEffect = ReturnSetting(GraphicsSetting, 4);
			FunctionGraphic.enableHideBgItem = ReturnSetting(GraphicsSetting, 5);
			FunctionGraphic.enableHideTileMap = ReturnSetting(GraphicsSetting, 6);
			FunctionGraphic.enableHideServerNofitication = ReturnSetting(GraphicsSetting, 7);
			FunctionGraphic.enableHideGameUI = ReturnSetting(GraphicsSetting, 8);
			FunctionGraphic.enablePaintImage_Wallpaper = ReturnSetting(GraphicsSetting, 9);
			FunctionGraphic.enablePaintColor_Wallpaper = ReturnSetting(GraphicsSetting, 10);
			if (File.ReadAllText(GraphicsSetting).Split('|')[11] != "")
			{
				string[] array = File.ReadAllText(GraphicsSetting).Split('|')[11].Split(',');
				int num = int.Parse(array[0]);
				int num2 = int.Parse(array[1]);
				int num3 = int.Parse(array[2]);
				FunctionGraphic.ColorRGB = num * 65536 + num2 * 256 + num3;
			}
			FunctionGraphic.enableHideBag = ReturnSetting(GraphicsSetting, 12);
			FunctionGraphic.enableHideImage = ReturnSetting(GraphicsSetting, 13);
			FunctionGraphic.enableOptimizingTileMap = ReturnSetting(GraphicsSetting, 14);
			FunctionGraphic.enableFreezeMob = ReturnSetting(GraphicsSetting, 15);
			FunctionMain.enableHide = ReturnSetting(GraphicsSetting, 16);
			FunctionGraphic.enableSpecialUI = ReturnSetting(GraphicsSetting, 17);
			FunctionChar.enablePaintListCharInMap = ReturnSetting(FunctionsSetting, 0);
			ListBossInformation.enableListBossInformation = ReturnSetting(FunctionsSetting, 1);
			FunctionChar.enablePaintFocusInformation = ReturnSetting(FunctionsSetting, 2);
			FunctionPet.enablePaintPetInformation = ReturnSetting(FunctionsSetting, 3);
			FunctionPet.enableAutoJump = ReturnSetting(FunctionsSetting, 4);
			FunctionTrainMob.IsAutoPickItems = ReturnSetting(FunctionsSetting, 5);
			FunctionMain.enableConnectToClient = ReturnSetting(FunctionsSetting, 6);
			FunctionGraphic.enableOptimizingCPU = ReturnSetting(FunctionsSetting, 7);
			FunctionTrainMob.IsItemMe = ReturnSetting(FunctionsSetting, 8);
			if (File.ReadAllText(FunctionsSetting).Split('|')[9] != "" && File.ReadAllText(FunctionsSetting).Split('|')[10] != "")
			{
				Time.timeScale = int.Parse(File.ReadAllText(FunctionsSetting).Split('|')[10]);
				FunctionChar.runSpeed = int.Parse(File.ReadAllText(FunctionsSetting).Split('|')[9]);
			}
			FunctionCaptcha.enablePhaCaptcha = ReturnSetting(FunctionsSetting, 12);
			FunctionTrainMob.IsVuotDiaHinh = ReturnSetting(FunctionsSetting, 13);
			if (ReturnSetting(FunctionsSetting, 14))
			{
				FunctionTrainMob.IdItemPicks.Add(74);
			}
		}
		catch
		{
		}
	}

	public static void paintWaitLogin(mGraphics g)
	{
		if (enableAutoLogin && (GameCanvas.currentScreen == GameCanvas.loginScr || GameCanvas.currentScreen == GameCanvas.serverScreen))
		{
			if (CountLogin < 26000 && CountLogin > 0)
			{
				StringHandle.paint(mFont.tahoma_7_white, g, "Đăng nhập sau " + CountLogin + "s nữa...", 5, 3, 0, mFont.tahoma_7, "noborder", mGraphics.zoomLevel);
			}
			else
			{
				StringHandle.paint(mFont.tahoma_7_white, g, "Đang chờ đăng nhập....", 5, 3, 0, mFont.tahoma_7, "noborder", mGraphics.zoomLevel);
			}
		}
	}

	public bool CheckBlackScreen()
	{
		return Char.isLoadingMap || LoginScr.isContinueToLogin || ServerListScreen.waitToLogin || ServerListScreen.isWait || ServerListScreen.isGetData;
	}

	public void AutoLogin()
	{
		if (!enableAutoLogin || CheckBlackScreen())
		{
			return;
		}
		if (GameCanvas.currentScreen == GameCanvas.loginScr || GameCanvas.currentScreen == GameCanvas.serverScreen)
		{
			if (!isLogin)
			{
				isLogin = true;
				timeWaitLogin = mSystem.currentTimeMillis();
			}
			else if (!updateServer && mSystem.currentTimeMillis() - timeWaitLogin >= 21000)
			{
				updateServer = true;
				ServerListScreen.doUpdateServer();
			}
			else if (mSystem.currentTimeMillis() - timeWaitLogin >= 26000)
			{
				isLogin = false;
				updateServer = false;
				timeWaitLogin = mSystem.currentTimeMillis();
				doLogin();
			}
			CountLogin = 25 - (mSystem.currentTimeMillis() - timeWaitLogin) / 1000;
		}
		else if (isLogin)
		{
			isLogin = false;
			updateServer = false;
			timeWaitLogin = mSystem.currentTimeMillis();
		}
	}

	public void doLogin()
	{
		if (GameCanvas.loginScr == null)
		{
			GameCanvas.loginScr = new LoginScr();
		}
		GameCanvas.loginScr.switchToMe();
		GameCanvas.loginScr.doLogin();
	}

	public void FixBlackScreen()
	{
		if (enableAutoLogin && mSystem.currentTimeMillis() - LastTimeUpdateScreen > 1000)
		{
			GameCanvas.timeout++;
			if (GameCanvas.timeout > 30)
			{
				Char.isLoadingMap = false;
				LoginScr.isContinueToLogin = false;
				ServerListScreen.waitToLogin = false;
				ServerListScreen.isWait = false;
				GameCanvas.gI().onDisconnected();
			}
			LastTimeUpdateScreen = mSystem.currentTimeMillis();
		}
	}
}

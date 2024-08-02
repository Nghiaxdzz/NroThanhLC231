using System;
using System.IO;

namespace AssemblyCSharp.Functions;

internal class FunctionMap : IActionListener
{
	private static FunctionMap _Instance;

	public static bool Teleported;

	public static int x = -1;

	public static int y = -1;

	public static int zoneID = -1;

	public static int mapID = -1;

	public static long TIME_WAIT_CHANGE_ZONE;

	public static long TIME_WAIT_MOVE_POSITION;

	public static long TIME_WAIT_XMAP;

	public static bool enableReturnHomeWhenEmptyMP;

	private static int[] wayPointMapLeft;

	private static int[] wayPointMapCenter;

	private static int[] wayPointMapRight;

	public static int ZoneRequest = -1;

	public static long TIME_DELAY_REQUEST_CHANGE_ZONE;

	public static long TIME_DELAY_OPEN_ZONEUI;

	public static bool enableLockChangeMap;

	public static bool enableLockChangeZone;

	public static bool enableAutoOpenBDKB;

	public static long TIME_DELAY_OPEN_BDKB;

	public static bool enableAutoOpenDTDN;

	public static long TIME_DELAY_OPEN_DTDN;

	public static FunctionMap gI()
	{
		if (_Instance == null)
		{
			_Instance = new FunctionMap();
		}
		return _Instance;
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 10201:
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command((x != -1 && y != -1) ? ("Lưu vị trí\nX: " + x + "\nY: " + y) : "Ấn để lưu\n Vị trí", gI(), 50231, null));
			myVector.addElement(new Command((zoneID != -1) ? ("Lưu khu\nKhu: " + zoneID) : "Ấn để lưu\n Khu", gI(), 50232, null));
			myVector.addElement(new Command((mapID != -1) ? ("Lưu map\n" + TileMap.mapNames[mapID]) : "Ấn để lưu\n Map", gI(), 50233, null));
			myVector.addElement(new Command("Lưu tất cả", gI(), 50234, null));
			myVector.addElement(new Command("Xóa \nthông tin", gI(), 50235, null));
			if (File.Exists("Data/QLTK/key.ini") && (File.ReadAllText("Data/QLTK/key.ini").Contains("843F9ECE43B2AFD3C2BF8956EF7CB7BE") || File.ReadAllText("Data/QLTK/key.ini").Contains("FB720487DDF040F3E2B39BC8777546CB") || File.ReadAllText("Data/QLTK/key.ini").Contains("62C06D2DE44D1FD1D56B727D7C0223B9")))
			{
				myVector.addElement(new Command("Goback khi\n Hết KI\n" + StringHandle.StatusMenu(enableReturnHomeWhenEmptyMP), gI(), 50236, null));
			}
			GameCanvas.menu.startAt(myVector, 0);
			break;
		}
		case 50231:
			chat("/spos");
			break;
		case 50232:
			chat("/szone");
			break;
		case 50233:
			chat("/smap");
			break;
		case 50234:
			x = Char.myCharz().cx;
			y = Char.myCharz().cy;
			zoneID = TileMap.zoneID;
			mapID = TileMap.mapID;
			GameScr.info1.addInfo("[ThanhLc] Đã lưu thông tin Goback", 0);
			break;
		case 50235:
			x = -1;
			y = -1;
			zoneID = -1;
			mapID = -1;
			GameScr.info1.addInfo("[ThanhLc] Đã xóa thông tin Goback", 0);
			break;
		case 50236:
			enableReturnHomeWhenEmptyMP = !enableReturnHomeWhenEmptyMP;
			GameScr.info1.addInfo("[ThanhLc] Goback khi hết KI: " + StringHandle.Status(enableReturnHomeWhenEmptyMP), 0);
			break;
		case 10202:
			enableAutoOpenDTDN = !enableAutoOpenDTDN;
			GameScr.info1.addInfo("[ThanhLc] Auto mở doanh trại độc nhãn : " + StringHandle.Status(enableAutoOpenDTDN), 0);
			break;
		case 10203:
			enableAutoOpenBDKB = !enableAutoOpenBDKB;
			GameScr.info1.addInfo("[ThanhLc] Auto mở bản đồ kho báu : " + StringHandle.Status(enableAutoOpenBDKB), 0);
			break;
		}
	}

	public static bool chat(string text)
	{
		if (StringHandle.IsGetInfoChat<int>(text, "/kz"))
		{
			ZoneRequest = StringHandle.GetInfoChat<int>(text, "/kz");
			GameScr.info1.addInfo("[ThanhLc] Auto vào khu: " + ZoneRequest + " " + ((ZoneRequest != -1) ? "Bật" : "Tắt"), 0);
		}
		else
		{
			switch (text)
			{
			case "/szone":
				if (zoneID == -1)
				{
					zoneID = TileMap.zoneID;
					GameScr.info1.addInfo("[ThanhLc] Goback - Lưu khu [" + zoneID + "]", 0);
				}
				else
				{
					zoneID = -1;
					GameScr.info1.addInfo("[ThanhLc] Goback - Xóa lưu khu", 0);
				}
				break;
			case "/spos":
				if (x == -1 && y == -1)
				{
					x = Char.myCharz().cx;
					y = Char.myCharz().cy;
					GameScr.info1.addInfo("[ThanhLc] Goback - Lưu vị trí [X: " + x + " -Y: " + y + "]", 0);
				}
				else
				{
					x = -1;
					y = -1;
					GameScr.info1.addInfo("[ThanhLc] Goback - Xóa lưu vị trí", 0);
				}
				break;
			case "/smap":
				if (mapID == -1)
				{
					mapID = TileMap.mapID;
					GameScr.info1.addInfo("[ThanhLc] Goback - Lưu map [" + mapID + "]", 0);
				}
				else
				{
					mapID = -1;
					GameScr.info1.addInfo("[ThanhLc] Goback - Xóa map", 0);
				}
				break;
			case "/kz":
				ZoneRequest = -1;
				GameScr.info1.addInfo("[ThanhLc] Đã tắt auto vào khu", 0);
				break;
			case "/lcz":
				enableLockChangeZone = !enableLockChangeZone;
				GameScr.info1.addInfo("[ThanhLc] Khóa chuyển khu: " + StringHandle.Status(enableLockChangeZone), 0);
				break;
			case "/lcm":
				enableLockChangeMap = !enableLockChangeMap;
				GameScr.info1.addInfo("[ThanhLc] Khóa chuyển map: " + StringHandle.Status(enableLockChangeMap), 0);
				break;
			default:
				if (StringHandle.IsGetInfoChat<int>(text, "/k"))
				{
					int infoChat = StringHandle.GetInfoChat<int>(text, "/k");
					GameScr.info1.addInfo("[ThanhLc] Đổi khu: " + infoChat, 0);
					Service.gI().requestChangeZone(infoChat, -1);
					break;
				}
				return false;
			}
		}
		return true;
	}

	public static void Update()
	{
		try
		{
			AutoChangeZone();
		}
		catch (Exception ex)
		{
			FunctionMain.WriteError("Data/Errors/AutoChangeZone.txt", ex.Message);
		}
		try
		{
			UpdateZone();
		}
		catch (Exception ex2)
		{
			FunctionMain.WriteError("Data/Errors/UpdateZone.txt", ex2.Message);
		}
		try
		{
			AutoReturnLocation();
		}
		catch (Exception ex3)
		{
			FunctionMain.WriteError("Data/Errors/AutoReturnLocation.txt", ex3.Message);
		}
		try
		{
			AutoOpenBDKB();
		}
		catch (Exception ex4)
		{
			FunctionMain.WriteError("Data/Errors/AutoOpenBDKB.txt", ex4.Message);
		}
		try
		{
			AutoOpenDTDN();
		}
		catch (Exception ex5)
		{
			FunctionMain.WriteError("Data/Errors/AutoOpenDTDN.txt", ex5.Message);
		}
	}

	public static int NumPlayersInZone()
	{
		int num = 0;
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char @char = (Char)GameScr.vCharInMap.elementAt(i);
			if (@char != null && @char.cName != null && @char.cName != "" && !@char.isPet && !@char.isMiniPet && !@char.cName.StartsWith("#") && !@char.cName.StartsWith("$") && @char.cName != "Trọng tài" && @char.charID > 0)
			{
				num++;
			}
		}
		return num;
	}

	public static void GoToChest()
	{
		if (GameCanvas.gameTick % 10 != 0 || FunctionXmap.IsXmapRunning)
		{
			return;
		}
		if (TileMap.mapID == Char.myCharz().cgender + 21)
		{
			for (int i = 0; i < GameScr.vNpc.size(); i++)
			{
				Npc npc = (Npc)GameScr.vNpc.elementAt(i);
				if (npc != null)
				{
					if (npc.template.name == "Rương đồ" && Char.myCharz().cx != npc.cx && Teleported)
					{
						Char.myCharz().cx = npc.cx;
						Char.myCharz().cy = ((Char.myCharz().cy != npc.cy) ? (npc.cy - 3) : npc.cy);
						Service.gI().charMove();
						Char.myCharz().charFocus = null;
						Char.myCharz().itemFocus = null;
						Char.myCharz().mobFocus = null;
						Char.myCharz().npcFocus = npc;
						break;
					}
					if (npc.template.name == "Rương đồ" && Char.myCharz().cx == npc.cx && Teleported)
					{
						Teleported = false;
					}
				}
			}
		}
		else
		{
			Teleported = true;
		}
	}

	public static void AutoReturnLocation()
	{
		bool flag = false;
		if (x == -1 && y == -1 && zoneID == -1 && mapID == -1)
		{
			return;
		}
		if (Char.myCharz().meDead)
		{
			flag = false;
			TIME_WAIT_XMAP = -1L;
			Service.gI().returnTownFromDead();
			return;
		}
		if (enableReturnHomeWhenEmptyMP && Char.myCharz().cMP < Char.myCharz().cMPFull * 5 / 100 && TileMap.mapID != Char.myCharz().cgender + 21)
		{
			XmapController.StartRunToMapId(21 + Char.myCharz().cgender);
			return;
		}
		if (TileMap.mapID == Char.myCharz().cgender + 21 && !flag)
		{
			for (int i = 0; i < GameScr.vItemMap.size(); i++)
			{
				ItemMap itemMap = GameScr.vItemMap.elementAt(i) as ItemMap;
				if (itemMap.template.id == 74)
				{
					Service.gI().pickItem(-1);
					flag = true;
					TIME_WAIT_XMAP = mSystem.currentTimeMillis();
					return;
				}
			}
		}
		if (mapID != -1 && TileMap.mapID != mapID && mSystem.currentTimeMillis() - TIME_WAIT_XMAP > 3000 && TIME_WAIT_XMAP != -1)
		{
			XmapController.StartRunToMapId(mapID);
			return;
		}
		if (zoneID != -1 && mSystem.currentTimeMillis() - TIME_WAIT_CHANGE_ZONE > 1000 && TileMap.zoneID != zoneID && 15 - GameScr.gI().numPlayer[zoneID] > 0)
		{
			Service.gI().requestChangeZone(zoneID, -1);
			TIME_WAIT_CHANGE_ZONE = mSystem.currentTimeMillis();
		}
		if (x == -1 || y == -1)
		{
			return;
		}
		if (Char.myCharz().cx != x || Char.myCharz().cy != y)
		{
			if (TIME_WAIT_MOVE_POSITION == -1)
			{
				TIME_WAIT_MOVE_POSITION = mSystem.currentTimeMillis();
			}
			if (mSystem.currentTimeMillis() - TIME_WAIT_MOVE_POSITION > 5000 && TIME_WAIT_MOVE_POSITION != -1)
			{
				MoveToPosition(x, y);
			}
		}
		if (Char.myCharz().cx == x && Char.myCharz().cy == y)
		{
			TIME_WAIT_MOVE_POSITION = -1L;
		}
	}

	public static int getHeight(int cx, int cy)
	{
		for (int i = cy; i < TileMap.pxh - 24; i += 24)
		{
			if (TileMap.tileTypeAt(cx, i, 2))
			{
				return i - i % 24;
			}
		}
		for (int num = cy; num > 24; num -= 24)
		{
			if (TileMap.tileTypeAt(cx, num, 2))
			{
				return num - num % 24;
			}
		}
		return cy;
	}

	public static void MoveToPosition(int x, int y)
	{
		Char.myCharz().cx = x;
		Char.myCharz().cy = y;
		Service.gI().charMove();
		Char.myCharz().cx = x;
		Char.myCharz().cy = getHeight(x, y) + 12;
		Service.gI().charMove();
		Char.myCharz().cx = x;
		Char.myCharz().cy = y;
		Service.gI().charMove();
	}

	private static void LoadWaypointsInMap()
	{
		ResetSavedWaypoints();
		int num = TileMap.vGo.size();
		if (num != 2)
		{
			for (int i = 0; i < num; i++)
			{
				Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
				if (waypoint.maxX < 60)
				{
					wayPointMapLeft[0] = waypoint.minX + 15;
					wayPointMapLeft[1] = waypoint.maxY;
				}
				else if (waypoint.maxX > TileMap.pxw - 60)
				{
					wayPointMapRight[0] = waypoint.maxX - 15;
					wayPointMapRight[1] = waypoint.maxY;
				}
				else
				{
					wayPointMapCenter[0] = waypoint.minX + 15;
					wayPointMapCenter[1] = waypoint.maxY;
				}
			}
			return;
		}
		Waypoint waypoint2 = (Waypoint)TileMap.vGo.elementAt(0);
		Waypoint waypoint3 = (Waypoint)TileMap.vGo.elementAt(1);
		if ((waypoint2.maxX < 60 && waypoint3.maxX < 60) || (waypoint2.minX > TileMap.pxw - 60 && waypoint3.minX > TileMap.pxw - 60))
		{
			wayPointMapLeft[0] = waypoint2.minX + 15;
			wayPointMapLeft[1] = waypoint2.maxY;
			wayPointMapRight[0] = waypoint3.maxX - 15;
			wayPointMapRight[1] = waypoint3.maxY;
		}
		else if (waypoint2.maxX < waypoint3.maxX)
		{
			wayPointMapLeft[0] = waypoint2.minX + 15;
			wayPointMapLeft[1] = waypoint2.maxY;
			wayPointMapRight[0] = waypoint3.maxX - 15;
			wayPointMapRight[1] = waypoint3.maxY;
		}
		else
		{
			wayPointMapLeft[0] = waypoint3.minX + 15;
			wayPointMapLeft[1] = waypoint3.maxY;
			wayPointMapRight[0] = waypoint2.maxX - 15;
			wayPointMapRight[1] = waypoint2.maxY;
		}
	}

	public static int GetYGround(int x)
	{
		int num = 50;
		int num2 = 0;
		while (num2 < 30)
		{
			num2++;
			num += 24;
			if (TileMap.tileTypeAt(x, num, 2))
			{
				if (num % 24 != 0)
				{
					num -= num % 24;
				}
				break;
			}
		}
		return num;
	}

	public static void TeleportTo(int x, int y)
	{
		if (GameScr.canAutoPlay)
		{
			Char.myCharz().cx = x;
			Char.myCharz().cy = y;
			Service.gI().charMove();
			return;
		}
		Char.myCharz().cx = x;
		Char.myCharz().cy = y;
		Service.gI().charMove();
		Char.myCharz().cx = x;
		Char.myCharz().cy = y + 1;
		Service.gI().charMove();
		Char.myCharz().cx = x;
		Char.myCharz().cy = y;
		Service.gI().charMove();
	}

	private static void ResetSavedWaypoints()
	{
		wayPointMapLeft = new int[2];
		wayPointMapCenter = new int[2];
		wayPointMapRight = new int[2];
	}

	public static bool isMeInNRDMap()
	{
		return TileMap.mapID >= 85 && TileMap.mapID <= 91;
	}

	public static void LoadMap(int position)
	{
		if (isMeInNRDMap())
		{
			TeleportInNRDMap(position);
			return;
		}
		LoadWaypointsInMap();
		switch (position)
		{
		case 0:
			if (wayPointMapLeft[0] != 0 && wayPointMapLeft[1] != 0)
			{
				TeleportTo(wayPointMapLeft[0], wayPointMapLeft[1]);
			}
			else
			{
				TeleportTo(60, GetYGround(60));
			}
			break;
		case 1:
			if (wayPointMapRight[0] != 0 && wayPointMapRight[1] != 0)
			{
				TeleportTo(wayPointMapRight[0], wayPointMapRight[1]);
			}
			else
			{
				TeleportTo(TileMap.pxw - 60, GetYGround(TileMap.pxw - 60));
			}
			break;
		case 2:
			if (wayPointMapCenter[0] != 0 && wayPointMapCenter[1] != 0)
			{
				TeleportTo(wayPointMapCenter[0], wayPointMapCenter[1]);
			}
			else
			{
				TeleportTo(TileMap.pxw / 2, GetYGround(TileMap.pxw / 2));
			}
			break;
		}
		if (TileMap.mapID != 7 && TileMap.mapID != 14 && TileMap.mapID != 0)
		{
			Service.gI().requestChangeMap();
		}
		else
		{
			Service.gI().getMapOffline();
		}
	}

	private static void TeleportInNRDMap(int position)
	{
		switch (position)
		{
		case 0:
			TeleportTo(60, GetYGround(60));
			break;
		case 2:
		{
			int num = 0;
			Npc npc;
			while (true)
			{
				if (num < GameScr.vNpc.size())
				{
					npc = (Npc)GameScr.vNpc.elementAt(num);
					if (npc.template.npcTemplateId >= 30 && npc.template.npcTemplateId <= 36)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			Char.myCharz().npcFocus = npc;
			TeleportTo(npc.cx, npc.cy - 3);
			break;
		}
		default:
			TeleportTo(TileMap.pxw - 60, GetYGround(TileMap.pxw - 60));
			break;
		}
	}

	public static void AutoChangeZone()
	{
		if (ZoneRequest != -1 && !FunctionXmap.IsXmapRunning && mSystem.currentTimeMillis() - TIME_DELAY_REQUEST_CHANGE_ZONE > 1000 && ((FunctionNRSD.isMeInNRDMap() && GameScr.gI().numPlayer[ZoneRequest] < 25) || (!FunctionNRSD.isMeInNRDMap() && 15 - GameScr.gI().numPlayer[ZoneRequest] > 0)) && TileMap.zoneID != ZoneRequest)
		{
			Service.gI().requestChangeZone(ZoneRequest, -1);
			TIME_DELAY_REQUEST_CHANGE_ZONE = mSystem.currentTimeMillis();
		}
	}

	public static void UpdateZone()
	{
		if (mSystem.currentTimeMillis() - TIME_DELAY_OPEN_ZONEUI > 1001)
		{
			Service.gI().openUIZone();
			TIME_DELAY_OPEN_ZONEUI = mSystem.currentTimeMillis();
		}
	}

	public static void AutoOpenBDKB()
	{
		if (enableAutoOpenBDKB && mSystem.currentTimeMillis() - TIME_DELAY_OPEN_BDKB > 200)
		{
			if (TileMap.mapName == "Đảo Kamê")
			{
				Service.gI().confirmMenu(13, 0);
				TIME_DELAY_OPEN_BDKB = mSystem.currentTimeMillis();
			}
			else if (TileMap.mapName == "Động hải tặc")
			{
				GameScr.info1.addInfo("Bạn đã vào bản đồ kho báu", 0);
				GameScr.info1.addInfo("Đã tắt auto mở bản đồ kho báu", 0);
				enableAutoOpenBDKB = false;
			}
		}
	}

	public static void AutoOpenDTDN()
	{
		if (enableAutoOpenDTDN && mSystem.currentTimeMillis() - TIME_DELAY_OPEN_DTDN > 100)
		{
			if (TileMap.mapID == 27)
			{
				Service.gI().openMenu(25);
				Service.gI().confirmMenu(25, 0);
				TIME_DELAY_OPEN_DTDN = mSystem.currentTimeMillis();
			}
			else
			{
				GameScr.info1.addInfo("Bạn đã vào doanh trại", 0);
				GameScr.info1.addInfo("Đã tắt auto mở doanh trại độc nhãn", 0);
				enableAutoOpenDTDN = false;
			}
		}
	}
}

using System;

namespace AssemblyCSharp.Functions;

public class FunctionXmap
{
	public static bool IsXmapRunning = false;

	public static bool IsMapTransAsXmap = false;

	public static bool IsShowPanelMapTrans = true;

	public static bool IsUseCapsuleNormal = true;

	public static bool IsUseCapsuleVip = true;

	public static int IdMapCapsuleReturn = -1;

	public static bool chat(string text)
	{
		if (text == "/xmp")
		{
			if (IsXmapRunning)
			{
				XmapController.FinishXmap();
				GameScr.info1.addInfo("Đã huỷ Xmap", 0);
			}
			else
			{
				XmapController.ShowXmapMenu();
			}
		}
		else if (StringHandle.IsGetInfoChat<int>(text, "/xmp"))
		{
			if (IsXmapRunning)
			{
				XmapController.FinishXmap();
				GameScr.info1.addInfo("Đã huỷ Xmap", 0);
			}
			else
			{
				int infoChat = StringHandle.GetInfoChat<int>(text, "/xmp");
				XmapController.StartRunToMapId(infoChat);
			}
		}
		else if (StringHandle.IsGetInfoChat<int>(text, "/nrd", 2))
		{
			int[] infoChat2 = StringHandle.GetInfoChat<int>(text, "/nrd", 2);
			XmapController.ZoneID = infoChat2[1];
			XmapController.StartRunToMapId(infoChat2[0] + 84);
		}
		else
		{
			switch (text)
			{
			case "/nrd":
				XmapController.ZoneID = -1;
				XmapController.FinishXmap();
				break;
			case "/csb":
				IsUseCapsuleNormal = !IsUseCapsuleNormal;
				GameScr.info1.addInfo("Sử dụng capsule thường Xmap: " + (IsUseCapsuleNormal ? "Bật" : "Tắt"), 0);
				break;
			case "/csdb":
				IsUseCapsuleVip = !IsUseCapsuleVip;
				GameScr.info1.addInfo("Sử dụng capsule đặc biệt Xmap: " + (IsUseCapsuleVip ? "Bật" : "Tắt"), 0);
				break;
			default:
				return false;
			}
		}
		return true;
	}

	public static bool HotKey(int keyPress)
	{
		try
		{
			switch (keyPress)
			{
			case 120:
				if (!FunctionChar.enableAutoTeleport)
				{
					chat("/xmp");
					break;
				}
				FunctionChar.enableAutoTeleport = false;
				GameScr.info1.addInfo("[ThanhLc] Đã tắt Auto giảm sức mạnh !", 0);
				break;
			case 99:
				Service.gI().useItem(0, 1, -1, 193);
				Service.gI().useItem(0, 1, -1, 194);
				GameScr.info1.addInfo("Đã mở capsule bay", 0);
				break;
			case 106:
				FunctionMap.LoadMap(0);
				break;
			case 107:
				if (TileMap.mapID != 135 && TileMap.mapID != 138 && TileMap.mapID != 136)
				{
					FunctionMap.LoadMap(2);
				}
				else if (TileMap.mapID == 135)
				{
					XmapController.MoveMyChar(600, 600);
				}
				else if (TileMap.mapID == 138)
				{
					XmapController.MoveMyChar(360, 336);
				}
				else if (TileMap.mapID == 136)
				{
					XmapController.MoveMyChar(720, 576);
				}
				break;
			case 108:
				FunctionMap.LoadMap(1);
				break;
			default:
				return false;
			}
		}
		catch (Exception)
		{
		}
		return true;
	}

	public static void Update()
	{
		if (XmapData.Instance().IsLoading)
		{
			XmapData.Instance().Update();
		}
		if (IsXmapRunning)
		{
			XmapController.Update();
		}
	}

	public static void Info(string text)
	{
		if (text.Equals("Bạn chưa thể đến khu vực này") || text.Equals("You can not travel to this area"))
		{
			XmapController.FinishXmap();
		}
	}

	public static bool XoaTauBay(object obj)
	{
		Teleport teleport = (Teleport)obj;
		if (teleport.isMe)
		{
			Char.myCharz().isTeleport = false;
			if (teleport.type == 0)
			{
				Controller.isStopReadMessage = false;
				Char.ischangingMap = true;
			}
			Teleport.vTeleport.removeElement(teleport);
			return true;
		}
		return false;
	}

	public static void SelectMapTrans(int selected)
	{
		if (IsMapTransAsXmap)
		{
			XmapController.HideInfoDlg();
			string mapName = GameCanvas.panel.mapNames[selected];
			int idMapFromPanelXmap = XmapData.GetIdMapFromPanelXmap(mapName);
			XmapController.StartRunToMapId(idMapFromPanelXmap);
		}
		else
		{
			XmapController.SaveIdMapCapsuleReturn();
			Service.gI().requestMapSelect(selected);
		}
	}

	public static void ShowPanelMapTrans()
	{
		IsMapTransAsXmap = false;
		if (IsShowPanelMapTrans)
		{
			GameCanvas.panel.setTypeMapTrans();
			GameCanvas.panel.show();
		}
		else
		{
			IsShowPanelMapTrans = true;
		}
	}

	public static void FixBlackScreen()
	{
		Controller.gI().loadCurrMap(0);
		Service.gI().finishLoadMap();
		Char.isLoadingMap = false;
	}
}

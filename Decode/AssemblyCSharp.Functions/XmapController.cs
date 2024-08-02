using System;
using System.Collections.Generic;
using System.IO;

namespace AssemblyCSharp.Functions;

internal class XmapController : IActionListener
{
	public struct MenuMap
	{
		public string groupName;

		public List<int> IDMaps;

		public MenuMap(string groupName, List<int> idMap)
		{
			this.groupName = groupName;
			IDMaps = idMap;
		}
	}

	public static int TIME_DELAY_NEXTMAP = 350;

	private const int TIME_DELAY_RENEXTMAP = 350;

	private const int ID_ITEM_CAPSULE_VIP = 194;

	private const int ID_ITEM_CAPSULE = 193;

	private const int ID_ICON_ITEM_TDLT = 4387;

	private static readonly XmapController _Instance = new XmapController();

	public static int IdMapEnd;

	private static List<int> WayXmap;

	private static int IndexWay;

	private static bool IsNextMapFailed;

	private static bool IsWait;

	private static long TimeStartWait;

	private static long TimeWait;

	private static bool IsWaitNextMap;

	public static int ZoneID = -1;

	public static int step;

	public static void Update()
	{
		try
		{
			if (IsWaiting() || XmapData.Instance().IsLoading)
			{
				return;
			}
			if (IsWaitNextMap)
			{
				if (TileMap.mapID == 21 + Char.myCharz().cgender && GameScr.vItemMap.size() > 0)
				{
					Service.gI().pickItem(-1);
				}
				Wait(TIME_DELAY_NEXTMAP);
				IsWaitNextMap = false;
				return;
			}
			if (IsNextMapFailed)
			{
				XmapData.Instance().MyLinkMaps = null;
				WayXmap = null;
				IsNextMapFailed = false;
				return;
			}
			if (WayXmap == null)
			{
				if (XmapData.Instance().MyLinkMaps == null)
				{
					XmapData.Instance().LoadLinkMaps();
					return;
				}
				WayXmap = XmapAlgorithm.FindWay(TileMap.mapID, IdMapEnd);
				IndexWay = 0;
				if (WayXmap == null)
				{
					GameScr.info1.addInfo("Không thể tìm thấy đường đi", 0);
					FinishXmap();
					return;
				}
			}
			if ((TileMap.mapID == WayXmap[WayXmap.Count - 1] || TileMap.mapID == IdMapEnd) && !XmapData.IsMyCharDie())
			{
				if (ZoneID != -1 && TileMap.zoneID != ZoneID)
				{
					Wait(1000);
					if ((GameScr.gI().numPlayer[ZoneID] < 25 && FunctionNRSD.isMeInNRDMap()) || (GameScr.gI().numPlayer[ZoneID] < 15 && !FunctionNRSD.isMeInNRDMap()))
					{
						Service.gI().requestChangeZone(ZoneID, -1);
					}
				}
				else
				{
					ZoneID = ((TileMap.zoneID == ZoneID && ZoneID != -1) ? (-1) : ZoneID);
					FinishXmap();
				}
			}
			else if (TileMap.mapID == WayXmap[IndexWay])
			{
				if (XmapData.IsMyCharDie())
				{
					Service.gI().returnTownFromDead();
					IsWaitNextMap = (IsNextMapFailed = true);
					WayXmap = null;
				}
				else if (XmapData.CanNextMap())
				{
					NextMap(WayXmap[IndexWay + 1]);
					IsWaitNextMap = true;
				}
				Wait(350);
			}
			else if (TileMap.mapID == WayXmap[IndexWay + 1])
			{
				IndexWay++;
			}
			else
			{
				IsNextMapFailed = true;
				WayXmap = null;
			}
		}
		catch (Exception ex)
		{
			File.WriteAllText("Data/Error/XmapErrorUpdate.txt", ex.Message);
		}
	}

	public void perform(int idAction, object p)
	{
		if (idAction == 1)
		{
			List<int> idMaps = (List<int>)p;
			ShowPanelXmap(idMaps);
		}
	}

	private static void Wait(int time)
	{
		IsWait = true;
		TimeStartWait = mSystem.currentTimeMillis();
		TimeWait = time;
	}

	private static bool IsWaiting()
	{
		if (IsWait && mSystem.currentTimeMillis() - TimeStartWait >= TimeWait)
		{
			IsWait = false;
		}
		return IsWait;
	}

	public static void ShowXmapMenu()
	{
		GameCanvas.panel2 = new Panel();
		GameCanvas.panel2.show();
		XmapData.Instance().LoadGroupMapsFromFile("TextData\\GroupMapsXmap.txt");
		FunctionMenu.setTypeMenuMod(0);
		GameCanvas.panel.currentTabIndex = 3;
		GameCanvas.panel.currentListLength = XmapData.Instance().GroupMaps.Count;
		GameCanvas.panel.show();
	}

	public static string get_map_names(int id)
	{
		string text = "";
		return id switch
		{
			105 => "Cánh đồng tuyết", 
			106 => "Rừng tuyết", 
			107 => "Núi tuyết", 
			108 => "Dòng sông băng", 
			109 => "Rừng băng", 
			110 => "Hang băng", 
			_ => TileMap.mapNames[id], 
		};
	}

	public static void ShowPanelXmap(List<int> idMaps)
	{
		FunctionXmap.IsMapTransAsXmap = true;
		int count = idMaps.Count;
		GameCanvas.panel.mapNames = new string[count];
		GameCanvas.panel.planetNames = new string[count];
		for (int i = 0; i < count; i++)
		{
			string text = get_map_names(idMaps[i]);
			GameCanvas.panel.mapNames[i] = idMaps[i] + ": " + text;
			GameCanvas.panel.planetNames[i] = "";
		}
		GameCanvas.panel.setTypeMapTrans();
		GameCanvas.panel.show();
	}

	public static void StartRunToMapId(int idMap)
	{
		IdMapEnd = idMap;
		FunctionXmap.IsXmapRunning = true;
	}

	public static void FinishXmap()
	{
		FunctionXmap.IsXmapRunning = false;
		IsNextMapFailed = false;
		XmapData.Instance().MyLinkMaps = null;
		WayXmap = null;
	}

	public static void SaveIdMapCapsuleReturn()
	{
		FunctionXmap.IdMapCapsuleReturn = TileMap.mapID;
	}

	private static void NextMap(int idMapNext)
	{
		List<MapNext> mapNexts = XmapData.Instance().GetMapNexts(TileMap.mapID);
		if (mapNexts != null)
		{
			foreach (MapNext item in mapNexts)
			{
				if (item.MapID == idMapNext)
				{
					NextMap(item);
					return;
				}
			}
		}
		GameScr.info1.addInfo("Lỗi tại dữ liệu", 0);
	}

	private static void NextMap(MapNext mapNext)
	{
		switch (mapNext.Type)
		{
		case TypeMapNext.AutoWaypoint:
			NextMapAutoWaypoint(mapNext);
			break;
		case TypeMapNext.NpcMenu:
			NextMapNpcMenu(mapNext);
			break;
		case TypeMapNext.NpcPanel:
			NextMapNpcPanel(mapNext);
			break;
		case TypeMapNext.Position:
			NextMapPosition(mapNext);
			break;
		case TypeMapNext.Capsule:
			NextMapCapsule(mapNext);
			break;
		}
	}

	private static void NextMapAutoWaypoint(MapNext mapNext)
	{
		Waypoint waypoint = XmapData.FindWaypoint(mapNext.MapID);
		if (waypoint != null)
		{
			int posWaypointX = XmapData.GetPosWaypointX(waypoint);
			int posWaypointY = XmapData.GetPosWaypointY(waypoint);
			MoveMyChar(posWaypointX, posWaypointY);
			RequestChangeMap(waypoint);
		}
	}

	public static void findNpc()
	{
		if (TileMap.mapID == 27)
		{
			NextMap(28);
			IsWaitNextMap = true;
			step = 0;
		}
		else if (TileMap.mapID == 29)
		{
			NextMap(28);
			IsWaitNextMap = true;
			step = 1;
		}
		else if (step == 0)
		{
			NextMap(29);
			IsWaitNextMap = true;
		}
		else if (step == 1)
		{
			NextMap(27);
			IsWaitNextMap = true;
		}
	}

	public static void CloseMenu()
	{
		ChatPopup.currChatPopup = null;
		Effect2.vEffect2.removeAllElements();
		Effect2.vEffect2Outside.removeAllElements();
		InfoDlg.hide();
		GameCanvas.menu.doCloseMenu();
		GameCanvas.panel.cp = null;
	}

	private static void NextMapNpcMenu(MapNext mapNext)
	{
		int num = mapNext.Info[0];
		if (GameScr.findNPCInMap((short)num) == null)
		{
			findNpc();
		}
		else if (num == 13)
		{
			if (!GameCanvas.menu.showMenu)
			{
				Service.gI().openMenu(num);
				CloseMenu();
				return;
			}
			for (int i = 0; i < GameCanvas.menu.menuItems.size(); i++)
			{
				Command command = (Command)GameCanvas.menu.menuItems.elementAt(i);
				if (command.caption.ToLower().StartsWith("nói") || command.caption.ToLower().StartsWith("talk"))
				{
					Service.gI().confirmMenu((short)num, (sbyte)i);
					CloseMenu();
					break;
				}
				if (mapNext.MapID == 135)
				{
					if (command.caption.ToLower().StartsWith("kho báu") || command.caption.ToLower().StartsWith("treasure"))
					{
						Service.gI().confirmMenu((short)num, (sbyte)i);
						CloseMenu();
						break;
					}
					if (command.caption.ToLower().StartsWith("đồng ý") || command.caption.ToLower().StartsWith("Accept"))
					{
						Service.gI().confirmMenu((short)num, (sbyte)i);
						CloseMenu();
						break;
					}
				}
				if (mapNext.MapID == 153 && (command.caption.ToLower().StartsWith("về khu") || command.caption.ToLower().StartsWith("return to")))
				{
					Service.gI().confirmMenu((short)num, (sbyte)i);
					CloseMenu();
					break;
				}
			}
		}
		else
		{
			Service.gI().openMenu(num);
			for (int j = 1; j < mapNext.Info.Length; j++)
			{
				int num2 = mapNext.Info[j];
				Service.gI().confirmMenu((short)num, (sbyte)num2);
			}
		}
	}

	private static void NextMapNpcPanel(MapNext mapNext)
	{
		int num = mapNext.Info[0];
		int num2 = mapNext.Info[1];
		int selected = mapNext.Info[2];
		if (num == 29)
		{
			Service.gI().openMenu(num);
			for (int i = 0; i < GameCanvas.menu.menuItems.size(); i++)
			{
				Command command = (Command)GameCanvas.menu.menuItems.elementAt(i);
				if (command.caption.ToLower().StartsWith("tham gia") || command.caption.ToLower().StartsWith("join"))
				{
					Service.gI().confirmMenu((short)num, (sbyte)i);
					CloseMenu();
				}
			}
			if (GameCanvas.panel.isShow)
			{
				CloseMenu();
				Service.gI().requestMapSelect(selected);
			}
			CloseMenu();
		}
		else
		{
			Service.gI().openMenu(num);
			Service.gI().confirmMenu((short)num, (sbyte)num2);
			Service.gI().requestMapSelect(selected);
		}
	}

	private static void NextMapPosition(MapNext mapNext)
	{
		int x = mapNext.Info[0];
		int y = mapNext.Info[1];
		MoveMyChar(x, y);
		Service.gI().requestChangeMap();
		Service.gI().getMapOffline();
	}

	private static void NextMapCapsule(MapNext mapNext)
	{
		SaveIdMapCapsuleReturn();
		int selected = mapNext.Info[0];
		Service.gI().requestMapSelect(selected);
	}

	public static void UseCapsuleNormal()
	{
		FunctionXmap.IsShowPanelMapTrans = false;
		Service.gI().useItem(0, 1, -1, 193);
	}

	public static void UseCapsuleVip()
	{
		FunctionXmap.IsShowPanelMapTrans = false;
		Service.gI().useItem(0, 1, -1, 194);
	}

	public static void HideInfoDlg()
	{
		InfoDlg.hide();
	}

	public static void MoveMyChar(int x, int y)
	{
		Char.myCharz().cx = x;
		Char.myCharz().cy = y;
		Service.gI().charMove();
		if (!ItemTime.isExistItem(4387))
		{
			Char.myCharz().cx = x;
			Char.myCharz().cy = y + 1;
			Service.gI().charMove();
			Char.myCharz().cx = x;
			Char.myCharz().cy = y;
			Service.gI().charMove();
		}
	}

	private static void RequestChangeMap(Waypoint waypoint)
	{
		if (waypoint.isOffline)
		{
			Service.gI().getMapOffline();
		}
		else
		{
			Service.gI().requestChangeMap();
		}
	}
}

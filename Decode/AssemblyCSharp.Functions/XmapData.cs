using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AssemblyCSharp.Functions;

public class XmapData
{
	private const int ID_MAP_HOME_BASE = 21;

	private const int ID_MAP_TTVT_BASE = 24;

	private const int ID_ITEM_CAPSULE_VIP = 194;

	private const int ID_ITEM_CAPSULE_NORMAL = 193;

	private const int ID_MAP_TPVGT = 19;

	private const int ID_MAP_TO_COLD = 109;

	public List<GroupMap> GroupMaps;

	public Dictionary<int, List<MapNext>> MyLinkMaps;

	public bool IsLoading;

	private bool IsLoadingCapsule;

	private static XmapData _Instance;

	private XmapData()
	{
		GroupMaps = new List<GroupMap>();
		MyLinkMaps = null;
		IsLoading = false;
		IsLoadingCapsule = false;
	}

	public static XmapData Instance()
	{
		if (_Instance == null)
		{
			_Instance = new XmapData();
		}
		return _Instance;
	}

	public void LoadLinkMaps()
	{
		IsLoading = true;
	}

	public void Update()
	{
		if (!IsLoadingCapsule)
		{
			LoadLinkMapBase();
			if (CanUseCapsuleVip())
			{
				XmapController.UseCapsuleVip();
				IsLoadingCapsule = true;
			}
			else if (CanUseCapsuleNormal())
			{
				XmapController.UseCapsuleNormal();
				IsLoadingCapsule = true;
			}
			else
			{
				IsLoading = false;
			}
		}
		else if (!IsWaitInfoMapTrans())
		{
			LoadLinkMapCapsule();
			IsLoadingCapsule = false;
			IsLoading = false;
		}
	}

	public void LoadGroupMapsFromFile(string path)
	{
		GroupMaps.Clear();
		try
		{
			StreamReader streamReader = new StreamReader(path);
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				text = text.Trim();
				if (!text.StartsWith("#") && !text.Equals(""))
				{
					string text2 = streamReader.ReadLine().Trim();
					string[] array = text2.Split(' ');
					List<int> idMaps = Array.ConvertAll(array, (string s) => int.Parse(s)).ToList();
					GroupMaps.Add(new GroupMap(text, idMaps));
				}
			}
		}
		catch (Exception ex)
		{
			GameScr.info1.addInfo(ex.Message, 0);
		}
		RemoveMapsHomeInGroupMaps();
	}

	private void RemoveMapsHomeInGroupMaps()
	{
		int cgender = Char.myCharz().cgender;
		foreach (GroupMap groupMap in GroupMaps)
		{
			switch (cgender)
			{
			case 0:
				groupMap.IdMaps.Remove(22);
				groupMap.IdMaps.Remove(23);
				break;
			case 1:
				groupMap.IdMaps.Remove(21);
				groupMap.IdMaps.Remove(23);
				break;
			default:
				groupMap.IdMaps.Remove(21);
				groupMap.IdMaps.Remove(22);
				break;
			}
		}
	}

	private void LoadLinkMapCapsule()
	{
		AddKeyLinkMaps(TileMap.mapID);
		string[] mapNames = GameCanvas.panel.mapNames;
		for (int i = 0; i < mapNames.Length; i++)
		{
			int idMapFromName = GetIdMapFromName(mapNames[i]);
			if (idMapFromName != -1)
			{
				int[] info = new int[1] { i };
				MyLinkMaps[TileMap.mapID].Add(new MapNext(idMapFromName, TypeMapNext.Capsule, info));
			}
		}
	}

	private void LoadLinkMapBase()
	{
		MyLinkMaps = new Dictionary<int, List<MapNext>>();
		LoadLinkMapsFromFile("TextData\\LinkMapsXmap.txt");
		LoadLinkMapsAutoWaypointFromFile("TextData\\AutoLinkMapsWaypoint.txt");
		LoadLinkMapsHome();
		LoadLinkMapSieuThi();
		LoadLinkMapToCold();
	}

	private void LoadLinkMapsFromFile(string path)
	{
		try
		{
			StreamReader streamReader = new StreamReader(path);
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				text = text.Trim();
				if (!text.StartsWith("#") && !text.Equals(""))
				{
					string[] array = text.Split(' ');
					int[] array2 = Array.ConvertAll(array, (string s) => int.Parse(s));
					int num = array2.Length - 3;
					int[] array3 = new int[num];
					Array.Copy(array2, 3, array3, 0, num);
					LoadLinkMap(array2[0], array2[1], (TypeMapNext)array2[2], array3);
				}
			}
		}
		catch (Exception ex)
		{
			GameScr.info1.addInfo(ex.Message, 0);
		}
	}

	private void LoadLinkMapsAutoWaypointFromFile(string path)
	{
		try
		{
			StreamReader streamReader = new StreamReader(path);
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				text = text.Trim();
				if (text.StartsWith("#") || text.Equals(""))
				{
					continue;
				}
				string[] array = text.Split(' ');
				int[] array2 = Array.ConvertAll(array, (string s) => int.Parse(s));
				for (int i = 0; i < array2.Length; i++)
				{
					if (i != 0)
					{
						LoadLinkMap(array2[i], array2[i - 1], TypeMapNext.AutoWaypoint, null);
					}
					if (i != array2.Length - 1)
					{
						LoadLinkMap(array2[i], array2[i + 1], TypeMapNext.AutoWaypoint, null);
					}
				}
			}
		}
		catch (Exception ex)
		{
			GameScr.info1.addInfo(ex.Message, 0);
		}
	}

	private void LoadLinkMapsHome()
	{
		int cgender = Char.myCharz().cgender;
		int num = 21 + cgender;
		int num2 = 7 * cgender;
		LoadLinkMap(num2, num, TypeMapNext.AutoWaypoint, null);
		LoadLinkMap(num, num2, TypeMapNext.AutoWaypoint, null);
	}

	private void LoadLinkMapSieuThi()
	{
		int cgender = Char.myCharz().cgender;
		int idMapNext = 24 + cgender;
		int[] info = new int[2] { 10, 0 };
		LoadLinkMap(84, idMapNext, TypeMapNext.NpcMenu, info);
	}

	private void LoadLinkMapToCold()
	{
		if (Char.myCharz().taskMaint.taskId > 30)
		{
			int[] info = new int[2] { 12, 0 };
			LoadLinkMap(19, 109, TypeMapNext.NpcMenu, info);
		}
	}

	public List<MapNext> GetMapNexts(int idMap)
	{
		if (CanGetMapNexts(idMap))
		{
			return MyLinkMaps[idMap];
		}
		return null;
	}

	public bool CanGetMapNexts(int idMap)
	{
		return MyLinkMaps.ContainsKey(idMap);
	}

	private void LoadLinkMap(int idMapStart, int idMapNext, TypeMapNext type, int[] info)
	{
		if (idMapNext == 135 && idMapStart == 5 && Char.myCharz().role == 0)
		{
			info = new int[4] { 13, 0, 4, 2 };
			AddKeyLinkMaps(idMapStart);
			MapNext item = new MapNext(idMapNext, type, info);
			MyLinkMaps[idMapStart].Add(item);
		}
		else if (idMapNext == 135 && idMapStart == 5 && Char.myCharz().role != 0)
		{
			info = new int[4] { 13, 0, 3, 2 };
			AddKeyLinkMaps(idMapStart);
			MapNext item2 = new MapNext(idMapNext, type, info);
			MyLinkMaps[idMapStart].Add(item2);
		}
		else if (idMapNext == 153 && idMapStart == 5)
		{
			info = new int[3] { 13, 0, 2 };
			AddKeyLinkMaps(idMapStart);
			MapNext item3 = new MapNext(idMapNext, type, info);
			MyLinkMaps[idMapStart].Add(item3);
		}
		else if (idMapNext == 156 && idMapStart == 153)
		{
			info = new int[2] { 47, 1 };
			AddKeyLinkMaps(idMapStart);
			MapNext item4 = new MapNext(idMapNext, type, info);
			MyLinkMaps[idMapStart].Add(item4);
		}
		else if (idMapNext >= 85 && idMapNext <= 91 && idMapStart == 26)
		{
			info = new int[3]
			{
				29,
				0,
				idMapNext - 85
			};
			AddKeyLinkMaps(idMapStart);
			MapNext item5 = new MapNext(idMapNext, TypeMapNext.NpcPanel, info);
			MyLinkMaps[idMapStart].Add(item5);
		}
		else
		{
			AddKeyLinkMaps(idMapStart);
			MapNext item6 = new MapNext(idMapNext, type, info);
			MyLinkMaps[idMapStart].Add(item6);
		}
	}

	private void AddKeyLinkMaps(int idMap)
	{
		if (!MyLinkMaps.ContainsKey(idMap))
		{
			MyLinkMaps.Add(idMap, new List<MapNext>());
		}
	}

	private bool IsWaitInfoMapTrans()
	{
		return !FunctionXmap.IsShowPanelMapTrans;
	}

	public static int GetIdMapFromPanelXmap(string mapName)
	{
		return int.Parse(mapName.Split(':')[0]);
	}

	public static string getTextPopup(PopUp popUp)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < popUp.says.Length; i++)
		{
			stringBuilder.Append(popUp.says[i]);
			stringBuilder.Append(" ");
		}
		return stringBuilder.ToString().Trim();
	}

	public static Waypoint FindWaypoint(int idMap)
	{
		for (int i = 0; i < TileMap.vGo.size(); i++)
		{
			Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
			string textPopup = getTextPopup(waypoint.popup);
			if (textPopup.Equals(TileMap.mapNames[idMap]))
			{
				return waypoint;
			}
		}
		return null;
	}

	public static int GetPosWaypointX(Waypoint waypoint)
	{
		if (waypoint.maxX < 60)
		{
			return waypoint.minX + 15;
		}
		if (waypoint.maxX > TileMap.pxw - 60)
		{
			return waypoint.maxX - 15;
		}
		return waypoint.minX + 15;
	}

	public static int GetPosWaypointY(Waypoint waypoint)
	{
		return waypoint.maxY;
	}

	public static bool IsMyCharDie()
	{
		return Char.myCharz().statusMe == 14 || Char.myCharz().cHP <= 0 || Char.myCharz().meDead;
	}

	public static bool CanNextMap()
	{
		return !Char.isLoadingMap && !Char.ischangingMap && !Controller.isStopReadMessage;
	}

	private static int GetIdMapFromName(string mapName)
	{
		int cgender = Char.myCharz().cgender;
		if (mapName.Equals("Về nhà"))
		{
			return 21 + cgender;
		}
		if (mapName.Equals("Trạm tàu vũ trụ"))
		{
			return 24 + cgender;
		}
		if (mapName.Contains("Về chỗ cũ: "))
		{
			mapName = mapName.Replace("Về chỗ cũ: ", "");
			if (XmapController.get_map_names(FunctionXmap.IdMapCapsuleReturn).Equals(mapName))
			{
				return FunctionXmap.IdMapCapsuleReturn;
			}
			if (mapName.Equals("Rừng đá"))
			{
				return -1;
			}
		}
		for (int i = 0; i < TileMap.mapNames.Length; i++)
		{
			if (mapName.Equals(XmapController.get_map_names(i)))
			{
				return i;
			}
		}
		return -1;
	}

	private static string GetTextPopup(PopUp popUp)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < popUp.says.Length; i++)
		{
			stringBuilder.Append(popUp.says[i]);
			stringBuilder.Append(" ");
		}
		return stringBuilder.ToString().Trim();
	}

	private static bool CanUseCapsuleNormal()
	{
		return !IsMyCharDie() && FunctionXmap.IsUseCapsuleNormal && HasItemCapsuleNormal();
	}

	private static bool HasItemCapsuleNormal()
	{
		Item[] arrItemBag = Char.myCharz().arrItemBag;
		for (int i = 0; i < arrItemBag.Length; i++)
		{
			if (arrItemBag[i] != null && arrItemBag[i].template.id == 193)
			{
				return true;
			}
		}
		return false;
	}

	private static bool CanUseCapsuleVip()
	{
		return !IsMyCharDie() && FunctionXmap.IsUseCapsuleVip && HasItemCapsuleVip();
	}

	private static bool HasItemCapsuleVip()
	{
		Item[] arrItemBag = Char.myCharz().arrItemBag;
		for (int i = 0; i < arrItemBag.Length; i++)
		{
			if (arrItemBag[i] != null && arrItemBag[i].template.id == 194)
			{
				return true;
			}
		}
		return false;
	}
}

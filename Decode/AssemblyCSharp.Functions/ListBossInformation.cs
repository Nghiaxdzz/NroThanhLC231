using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AssemblyCSharp.Functions;

public class ListBossInformation
{
	public string name;

	public string map;

	public int mapId;

	public int zoneId = -1;

	public DateTime AppearTime;

	public static List<ListBossInformation> ListBossOnScreen = new List<ListBossInformation>();

	public static List<ListBossInformation> ListBossOnPanel = new List<ListBossInformation>();

	public static bool enableListBossInformation;

	private static int distanceBetweenLines = 8;

	private static int x = 5;

	private static int y = 40;

	private static int maxLength = 0;

	public static int lastBoss = -1;

	private ListBossInformation(string name, string map)
	{
		this.name = name;
		this.map = map;
		if (map == "Trạm tàu vũ trụ")
		{
			if (name.StartsWith("Số ") || name.StartsWith("Tiểu đội"))
			{
				mapId = 25;
			}
			else if (name.Contains("Bojack") || name.StartsWith("Bujin") || name.StartsWith("Bido") || name.StartsWith("Zangya") || name.StartsWith("Bido"))
			{
				mapId = 24;
			}
		}
		else
		{
			mapId = GetMapID(map);
		}
		AppearTime = DateTime.Now;
	}

	public static void AddBoss(string chatVip)
	{
		if (enableListBossInformation && chatVip.StartsWith("BOSS"))
		{
			chatVip = chatVip.Replace("BOSS ", "").Replace(" vừa xuất hiện tại ", "|").Replace(" appear at ", "|")
				.Replace(" khu vực ", "|")
				.Replace(" zone ", "|");
			string[] array = chatVip.Split('|');
			ListBossOnScreen.Add(new ListBossInformation(array[0].Trim(), array[1].Trim()));
			if (array.Length == 3)
			{
				ListBossOnScreen.Last().zoneId = int.Parse(array[2].Trim());
			}
			if (ListBossOnScreen.Count > 5)
			{
				ListBossOnScreen.RemoveAt(0);
			}
			ListBossOnPanel.Add(new ListBossInformation(array[0].Trim(), array[1].Trim()));
			if (array.Length == 3)
			{
				ListBossOnPanel.Last().zoneId = int.Parse(array[2].Trim());
			}
			if (ListBossOnPanel.Count > 40)
			{
				ListBossOnPanel.RemoveAt(0);
			}
		}
	}

	public override string ToString()
	{
		TimeSpan timeSpan = DateTime.Now.Subtract(AppearTime);
		string text = name + "-" + map + "- ";
		if (zoneId > -1)
		{
			text += $"khu {zoneId} - ";
		}
		int num = (int)System.Math.Floor((decimal)timeSpan.TotalHours);
		if (num > 0)
		{
			text += $"{num}h";
		}
		if (timeSpan.Minutes > 0)
		{
			text += $"{timeSpan.Minutes}m";
		}
		return text + $"{timeSpan.Seconds}s";
	}

	public static void paintListBossOnScreen(mGraphics g)
	{
		if (!enableListBossInformation || FunctionNRSD.isMeInNRDMap())
		{
			return;
		}
		g.reset();
		if (mGraphics.zoomLevel == 2)
		{
			int num = 0;
			GUIStyle[] array = new GUIStyle[ListBossOnScreen.Count];
			for (int i = num; i < ListBossOnScreen.Count; i++)
			{
				array[i] = new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.UpperLeft,
					fontSize = 6 * mGraphics.zoomLevel,
					fontStyle = FontStyle.Bold
				};
				ListBossInformation listBossInformation = ListBossOnScreen[i];
				array[i].normal.textColor = Color.yellow;
				if (TileMap.mapID == listBossInformation.mapId)
				{
					array[i].normal.textColor = Color.green;
					for (int j = 0; j < GameScr.vCharInMap.size(); j++)
					{
						if (((Char)GameScr.vCharInMap.elementAt(j)).cName == listBossInformation.name)
						{
							array[i].normal.textColor = Color.red;
							break;
						}
					}
				}
				int width = StringHandle.getWidth(array[i], $"{i + 1}. {listBossInformation}");
				maxLength = Math.max(width, maxLength);
			}
			int num2 = GameCanvas.w - x - maxLength;
			for (int k = num; k < ListBossOnScreen.Count; k++)
			{
				int num3 = y + distanceBetweenLines * k;
				ListBossInformation arg = ListBossOnScreen[k];
				g.setColor(0, 0.3f);
				g.fillRect(num2, num3 + 1, maxLength, 7);
				g.drawString($"{arg}", GameCanvas.w - x - maxLength + 2, mGraphics.zoomLevel - 3 + num3, array[k]);
			}
		}
		if (mGraphics.zoomLevel != 1)
		{
			return;
		}
		for (int l = 0; l < ListBossOnScreen.Count; l++)
		{
			ListBossInformation listBossInformation2 = ListBossOnScreen[l];
			string text = $"{listBossInformation2}";
			mFont mFont = mFont.tahoma_7_yellow;
			if (TileMap.mapID == listBossInformation2.mapId)
			{
				mFont = mFont.tahoma_7_red;
				for (int m = 0; m < GameScr.vCharInMap.size(); m++)
				{
					if (((Char)GameScr.vCharInMap.elementAt(m)).cName == listBossInformation2.name)
					{
						mFont = mFont.tahoma_7b_red;
						break;
					}
				}
			}
			g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.4f));
			g.fillRect(GameCanvas.w - 31, y + 10 * l, 26, 9);
			mFont.drawString(g, text + " [Đến]", GameCanvas.w - 7, mGraphics.zoomLevel - 3 + y + 10 * l + 1, mFont.RIGHT);
		}
	}

	private static int GetMapID(string mapName)
	{
		for (int i = 0; i < TileMap.mapNames.Length; i++)
		{
			if (TileMap.mapNames[i].Equals(mapName))
			{
				return i;
			}
		}
		return -1;
	}

	public static long GetLastTimePress()
	{
		return (long)typeof(GameCanvas).GetField("lastTimePress", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
	}

	public static void updateKeyTouchController()
	{
		if (!enableListBossInformation)
		{
			return;
		}
		if (lastBoss != -1 && mSystem.currentTimeMillis() - GetLastTimePress() > 200)
		{
			lastBoss = -1;
		}
		if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
		{
			return;
		}
		int num = 0;
		for (int i = num; i < ListBossOnScreen.Count; i++)
		{
			if ((mGraphics.zoomLevel != 2 || !GameCanvas.isPointerHoldIn(GameCanvas.w - x - maxLength, y + 1 + distanceBetweenLines * i, maxLength, 7) || GameCanvas.isPointerDown) && (mGraphics.zoomLevel != 1 || !GameCanvas.isPointerHoldIn(GameCanvas.w - 31, y + 10 * i, 26, 9) || GameCanvas.isPointerDown))
			{
				continue;
			}
			if (GameCanvas.isPointerClick)
			{
				if (lastBoss == i && mSystem.currentTimeMillis() - GetLastTimePress() <= 200)
				{
					if (TileMap.mapID != ListBossOnScreen[i].mapId)
					{
						if (FunctionXmap.IsXmapRunning)
						{
							XmapController.FinishXmap();
						}
						SoundMn.gI().buttonClick();
						Char.myCharz().currentMovePoint = null;
						GameCanvas.clearAllPointerEvent();
						XmapController.StartRunToMapId(ListBossOnScreen[i].mapId);
						lastBoss = -1;
						break;
					}
				}
				else
				{
					lastBoss = i;
				}
			}
			GameCanvas.clearAllPointerEvent();
			break;
		}
	}

	public static void updateListBoss()
	{
		if (!enableListBossInformation)
		{
			return;
		}
		foreach (ListBossInformation item in ListBossOnScreen)
		{
			if (item.zoneId != -1)
			{
				continue;
			}
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				Char @char = GameScr.vCharInMap.elementAt(i) as Char;
				if (@char.cName == item.name)
				{
					item.zoneId = TileMap.zoneID;
					break;
				}
			}
			if (item.zoneId != TileMap.zoneID)
			{
				continue;
			}
			break;
		}
	}
}

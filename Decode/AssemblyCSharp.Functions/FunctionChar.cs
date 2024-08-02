using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp.Functions;

public class FunctionChar : IActionListener
{
	public class ListCharsInMap
	{
		private static string longestStr = string.Empty;

		private static int maxLength = 0;

		private static int x = 5;

		private static int y = 40;

		private static readonly int MAX_CHAR = 6;

		private static int distanceBetweenLines = 8;

		private static int offset = 0;

		public static List<Char> ListCharInMap = new List<Char>();

		public static int TYPE_SORT = 2;

		public static void Update()
		{
			if (!enablePaintListCharInMap)
			{
				return;
			}
			if (mGraphics.zoomLevel == 2)
			{
				if (ListBossInformation.enableListBossInformation)
				{
					if (y != 40 + 8 * Mathf.Clamp(ListBossInformation.ListBossOnScreen.Count, 0, 5) + 3)
					{
						y = 40 + 8 * Mathf.Clamp(ListBossInformation.ListBossOnScreen.Count, 0, 5) + 3;
					}
				}
				else if (y != 40)
				{
					y = 40;
				}
			}
			else if (ListBossInformation.enableListBossInformation)
			{
				if (y != 55 + 8 * Mathf.Clamp(ListBossInformation.ListBossOnScreen.Count, 0, 5) + 3)
				{
					y = 55 + 8 * Mathf.Clamp(ListBossInformation.ListBossOnScreen.Count, 0, 5) + 3;
				}
			}
			else if (y != 55)
			{
				y = 55;
			}
			AddChar();
		}

		public static void updateTouch()
		{
			if (!enablePaintListCharInMap)
			{
				return;
			}
			try
			{
				if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
				{
					return;
				}
				for (int i = 0; i < ListCharInMap.Count; i++)
				{
					if (mGraphics.zoomLevel == 2 && GameCanvas.isPointer(GameCanvas.w - x - maxLength, y + 8 * i, maxLength, 7) && !GameCanvas.isPointerDown)
					{
						if (Char.myCharz().charFocus != ListCharInMap[i])
						{
							SoundMn.gI().buttonClick();
							Char.myCharz().currentMovePoint = null;
							GameCanvas.clearAllPointerEvent();
							Char.myCharz().mobFocus = null;
							Char.myCharz().npcFocus = null;
							Char.myCharz().itemFocus = null;
							Char.myCharz().charFocus = ListCharInMap[i];
						}
						else
						{
							SoundMn.gI().buttonClick();
							Char.myCharz().currentMovePoint = null;
							GameCanvas.clearAllPointerEvent();
							teleportMyChar(ListCharInMap[i]);
						}
						break;
					}
					if (mGraphics.zoomLevel == 1 && GameCanvas.isPointer(GameCanvas.w - 190, y + 10 * i, 120, 9) && !GameCanvas.isPointerDown)
					{
						if (Char.myCharz().charFocus != ListCharInMap[i])
						{
							SoundMn.gI().buttonClick();
							Char.myCharz().currentMovePoint = null;
							GameCanvas.clearAllPointerEvent();
							Char.myCharz().mobFocus = null;
							Char.myCharz().npcFocus = null;
							Char.myCharz().itemFocus = null;
							Char.myCharz().charFocus = ListCharInMap[i];
						}
						else
						{
							SoundMn.gI().buttonClick();
							Char.myCharz().currentMovePoint = null;
							GameCanvas.clearAllPointerEvent();
							teleportMyChar(ListCharInMap[i]);
						}
						break;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public static void AddChar()
		{
			ListCharInMap.Clear();
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				Char @char = (Char)GameScr.vCharInMap.elementAt(i);
				if (@char.cName != null && @char.cName != "" && !@char.isPet && !@char.isMiniPet && !@char.cName.StartsWith("#") && !@char.cName.StartsWith("$") && @char.cName != "Trọng tài" && !ListCharInMap.Contains(@char))
				{
					ListCharInMap.Add(@char);
					ListCharInMap = Sorting();
				}
			}
		}

		public static void Swap(List<Char> myList, int i, int m)
		{
			Char value = myList[i];
			myList[i] = myList[m];
			myList[m] = value;
		}

		public static List<Char> Sorting()
		{
			for (int i = 0; i < ListCharInMap.Count - 1; i++)
			{
				int m = i;
				int cFlag = ListCharInMap[i].cFlag;
				string text = ((ListCharInMap[i].clanID > 0) ? ListCharInMap[i].cName.Split(']')[0].Replace("[", "") : "");
				char[] array = text.ToCharArray();
				int num = 0;
				for (int j = 0; j < array.Length; j++)
				{
					num += array[j];
				}
				string text2 = ((ListCharInMap[i].clanID > 0) ? ListCharInMap[i].cName.Split(']')[0].Replace("[", "") : "");
				for (int k = i + 1; k < ListCharInMap.Count; k++)
				{
					switch (TYPE_SORT)
					{
					case 1:
						if (ListCharInMap[k].cFlag < cFlag)
						{
							m = k;
							cFlag = ListCharInMap[k].cFlag;
						}
						break;
					case 2:
					{
						char[] array2 = ((ListCharInMap[k].clanID > 0) ? ListCharInMap[k].cName.Split(']')[0].Replace("[", "") : "").ToCharArray();
						int num2 = 0;
						for (int l = 0; l < array2.Length; l++)
						{
							num2 += array2[l];
						}
						if (num2 < num)
						{
							m = k;
							num = num2;
						}
						break;
					}
					}
				}
				Swap(ListCharInMap, i, m);
			}
			return ListCharInMap;
		}

		public static void paint(mGraphics g)
		{
			if (!enablePaintListCharInMap)
			{
				return;
			}
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				Char @char = (Char)GameScr.vCharInMap.elementAt(i);
				if (@char != null && @char.charEffectTime.hasNRD && Char.myCharz().charFocus != @char)
				{
					StringHandle.paint(mFont.tahoma_7b_yellow, g, @char.cName + " [" + NinjaUtil.getMoneys(@char.cHP) + "/" + NinjaUtil.getMoneys(@char.cHPFull) + "]", GameCanvas.w / 2, 23, mFont.CENTER, mFont.tahoma_7b_dark, "border", mGraphics.zoomLevel);
					StringHandle.paint(mFont.tahoma_7b_yellow, g, "NRD còn: " + @char.charEffectTime.timeHoldingNRD + " giây", GameCanvas.w / 2, 33, mFont.CENTER, mFont.tahoma_7b_dark, "border", mGraphics.zoomLevel);
				}
			}
			if (mGraphics.zoomLevel == 2)
			{
				try
				{
					GUIStyle[] array = new GUIStyle[ListCharInMap.Count];
					int num = 1;
					for (int j = 0; j < ListCharInMap.Count; j++)
					{
						Char char2 = ListCharInMap[j];
						if (ListCharInMap[j] == null)
						{
							array[j] = null;
						}
						if (char2 == null)
						{
							continue;
						}
						string text = num + "." + char2.cName + " [" + NinjaUtil.getMoneys(char2.cHP) + ((char2.charID > 0 || TileMap.mapID == 113) ? (" - " + char2.getGender()) : "") + "] ";
						array[j] = new GUIStyle(GUI.skin.label)
						{
							alignment = TextAnchor.UpperLeft,
							fontSize = 6 * mGraphics.zoomLevel + mGraphics.zoomLevel / 2,
							fontStyle = FontStyle.Bold
						};
						array[j].normal.textColor = Color.black;
						if ((Char.myCharz().isStandAndCharge || (!Char.myCharz().isDie && Char.myCharz().cgender == 2 && Char.myCharz().myskill == Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]))) && SuicideRange.mapObjsInMyRange.Contains(char2))
						{
							text += " [Trong tầm]";
							array[j].normal.textColor = Color.blue;
						}
						if (Char.myCharz().charFocus == char2)
						{
							array[j].normal.textColor = Color.yellow;
							if (Res.distance(Char.myCharz().cx, Char.myCharz().cy, char2.cx, char2.cy) > 5)
							{
								g.setColor(Color.yellow);
								g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy, char2.cx - GameScr.cmx, char2.cy - GameScr.cmy);
							}
						}
						if (char2.charID < 0)
						{
							array[j].normal.textColor = Color.red;
							if (Res.distance(Char.myCharz().cx, Char.myCharz().cy, char2.cx, char2.cy) > 5)
							{
								g.setColor(Color.red);
								g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy, char2.cx - GameScr.cmx, char2.cy - GameScr.cmy);
							}
						}
						int width = StringHandle.getWidth(array[j], text);
						maxLength = Math.max(width, maxLength);
						int num2 = GameCanvas.w - x - maxLength;
						int num3 = y + 8 * j;
						g.setColor(52428, 0.8f);
						if (char2.charID < 0)
						{
							g.setColor(16777215, 0.4f);
						}
						if (GameCanvas.isMouseFocus(num2, num3, maxLength, 7))
						{
							g.setColor(16777215, 0.6f);
						}
						if (Char.myCharz().charFocus == char2)
						{
							g.drawRegion(Mob.imgHP, 0, 24, 9, 6, 4, GameCanvas.w - x - maxLength - ((char2.cFlag != 0) ? 19 : 8), mGraphics.zoomLevel - 1 + num3, 0);
							array[j].normal.textColor = Color.white;
							g.setColor(new Color(1f, 0.5f, 0f, 0.5f));
						}
						if (char2.charEffectTime.hasNRD)
						{
							g.setColor(0, 0.6f);
							array[j].normal.textColor = Color.yellow;
							array[j].fontStyle = FontStyle.BoldAndItalic;
						}
						g.fillRect(num2, num3 + 1, maxLength, 7);
						g.drawString(text, GameCanvas.w - x - maxLength + 2, mGraphics.zoomLevel - 3 + num3, array[j]);
						num++;
					}
				}
				catch (Exception ex)
				{
					FunctionMain.WriteError("Data/Errors/paintListChar.txt", ex.Message);
				}
			}
			if (mGraphics.zoomLevel == 1)
			{
				int num4 = 1;
				for (int k = 0; k < ListCharInMap.Count; k++)
				{
					Char char3 = ListCharInMap[k];
					string st = num4 + ". [" + char3.getGender() + "] " + char3.cName + " [" + NinjaUtil.getMoneys(char3.cHP) + "] ";
					g.setColor(52428, 0.4f);
					if (char3.charID < 0)
					{
						g.setColor(16777215, 0.4f);
					}
					if (GameCanvas.isMouseFocus(GameCanvas.w - 190, y + 10 * k, 120, 9))
					{
						g.setColor(16777215, 0.6f);
					}
					if (Char.myCharz().charFocus == char3)
					{
						g.setColor(new Color(1f, 0.5f, 0f, 0.5f));
					}
					g.fillRect(GameCanvas.w - 150, y + 10 * k, 190, 9);
					if (Char.myCharz().charFocus == char3)
					{
						mFont.tahoma_7_white.drawString(g, st, GameCanvas.w - 149, mGraphics.zoomLevel - 3 + y + 10 * k + 1, mFont.LEFT);
					}
					if (char3.charID < 0 && Char.myCharz().charFocus != char3)
					{
						mFont.tahoma_7_red.drawString(g, st, GameCanvas.w - 149, mGraphics.zoomLevel - 3 + y + 10 * k + 1, mFont.LEFT);
					}
					if (char3.charID > 0 && Char.myCharz().charFocus != char3)
					{
						mFont.tahoma_7.drawString(g, st, GameCanvas.w - 149, mGraphics.zoomLevel - 3 + y + 10 * k + 1, mFont.LEFT);
					}
					num4++;
				}
			}
			for (int l = 0; l < ListCharInMap.Count; l++)
			{
				Char char4 = ListCharInMap[l];
				if (char4.cFlag != 0)
				{
					if (mGraphics.zoomLevel == 2)
					{
						g.setColor(FunctionCharEffect.CharExtensions.getFlagColor(char4));
						g.fillRect(GameCanvas.w - x - 9 - maxLength, y + 1 + 8 * l, 7, 7);
						g.setColor(FunctionCharEffect.CharExtensions.getFlagColor(char4));
						g.fillRect(GameCanvas.w - x - 9 - maxLength, y + 1 + 8 * l, 7, 7);
						g.setColor(FunctionCharEffect.CharExtensions.getFlagColor(char4));
						g.fillRect(GameCanvas.w - x - 9 - maxLength, y + 1 + 8 * l, 7, 7);
						g.setColor(FunctionCharEffect.CharExtensions.getFlagColor(char4));
						g.fillRect(GameCanvas.w - x - 9 - maxLength, y + 1 + 8 * l, 7, 7);
					}
					else
					{
						g.setColor(FunctionCharEffect.CharExtensions.getFlagColor(char4));
						g.fillRect(GameCanvas.w - 158, y + 1 + 10 * l, 7, 7);
						g.setColor(FunctionCharEffect.CharExtensions.getFlagColor(char4));
						g.fillRect(GameCanvas.w - 158, y + 1 + 10 * l, 7, 7);
						g.setColor(FunctionCharEffect.CharExtensions.getFlagColor(char4));
						g.fillRect(GameCanvas.w - 158, y + 1 + 10 * l, 7, 7);
						g.setColor(FunctionCharEffect.CharExtensions.getFlagColor(char4));
						g.fillRect(GameCanvas.w - 158, y + 1 + 10 * l, 7, 7);
					}
				}
			}
		}

		internal static int getWidth(GUIStyle gUIStyle, string s)
		{
			return (int)(gUIStyle.CalcSize(new GUIContent(s)).x * 1.05f / (float)mGraphics.zoomLevel);
		}

		internal static int getHeight(GUIStyle gUIStyle, string content)
		{
			return (int)gUIStyle.CalcSize(new GUIContent(content)).y / mGraphics.zoomLevel;
		}

		public static void teleportMyChar(int x, int y)
		{
			Char.myCharz().currentMovePoint = null;
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

		public static void teleportMyChar(IMapObject obj)
		{
			teleportMyChar(obj.getX(), obj.getY());
		}
	}

	public class SuicideRange
	{
		public static bool isShowSuicideRange;

		public static List<Char> ListUseHold = new List<Char>();

		public static int yPaint;

		public static List<IMapObject> mapObjsInRange { get; private set; } = new List<IMapObject>();


		public static List<IMapObject> mapObjsInMyRange { get; private set; } = new List<IMapObject>();


		public static int getYGround(int x)
		{
			int num = 50;
			for (int i = 0; i < 30; i++)
			{
				num += 24;
				if (TileMap.tileTypeAt(x, num, 2))
				{
					if (num % 24 != 0)
					{
						num -= num % 24;
					}
					return num;
				}
			}
			return -1;
		}

		public static int getDistance(IMapObject mapObject1, IMapObject mapObject2)
		{
			return Res.distance(mapObject1.getX(), mapObject1.getY(), mapObject2.getX(), mapObject2.getY());
		}

		public static void update()
		{
			if (!enablePaintListCharInMap)
			{
				return;
			}
			mapObjsInRange.Clear();
			mapObjsInMyRange.Clear();
			if (!Char.myCharz().isDie && Char.myCharz().myskill == Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]) && Char.myCharz().cgender == 2)
			{
				FindMapObjInRange(Char.myCharz());
			}
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				Char @char = GameScr.vCharInMap.elementAt(i) as Char;
				if (@char.isStandAndCharge && @char.cName != null && @char.cName != "" && !@char.isPet && !@char.isMiniPet && !@char.cName.StartsWith("#") && !@char.cName.StartsWith("$") && @char.cName != "Trọng tài" && @char.cgender == 2)
				{
					FindMapObjInRange(@char);
					if (getDistance(Char.myCharz(), @char) <= FunctionCharEffect.CharExtensions.getSuicideRange(@char) && !mapObjsInRange.Contains(Char.myCharz()))
					{
						mapObjsInRange.Add(Char.myCharz());
					}
				}
			}
		}

		public static void paint(mGraphics g)
		{
			if (!enablePaintListCharInMap)
			{
				return;
			}
			g.reset();
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				Char @char = GameScr.vCharInMap.elementAt(i) as Char;
				string empty = string.Empty;
				if (@char.isStandAndCharge && getDistance(@char, Char.myCharz()) <= FunctionCharEffect.CharExtensions.getSuicideRange(@char))
				{
					empty = FunctionCharEffect.CharExtensions.getNameWithoutClanTag(@char) + " [" + NinjaUtil.getMoneys(@char.cHPFull) + "] đang sử dụng bom";
					if ((@char.cFlag != 0 && Char.myCharz().cFlag != 0 && (@char.cFlag != Char.myCharz().cFlag || (@char.cFlag == 8 && Char.myCharz().cFlag == 8))) || Char.myCharz().cTypePk == 5 || @char.cTypePk == 5)
					{
						empty += "- Trong tầm";
					}
					StringHandle.drawStringBd(mFont.tahoma_7_yellow, g, empty, 5, yPaint, mFont.LEFT, mFont.tahoma_7);
					yPaint += 9;
				}
				if (@char != null && @char.holder && @char.charHold != null)
				{
					empty = @char.cName + " trói [" + @char.charHold.cName + "]";
					StringHandle.drawStringBd(mFont.tahoma_7_yellow, g, empty, 5, yPaint, mFont.LEFT, mFont.tahoma_7);
					yPaint += 9;
				}
			}
		}

		private static void paintMapObjInRange(mGraphics g, IMapObject mapObject)
		{
			if (mapObject is Char)
			{
				Char @char = mapObject as Char;
				int num = 35;
				int num2 = 12;
				if (FunctionCharEffect.CharExtensions.isPet(@char))
				{
					num = 30;
				}
				if (@char.cTypePk == 5)
				{
					num2 = 15;
					num = 40;
				}
			}
			if (mapObject is Mob)
			{
				Mob mob = mapObject as Mob;
			}
		}

		private static void FindMapObjInRange(Char suicidingChar)
		{
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				Char @char = GameScr.vCharInMap.elementAt(i) as Char;
				if (@char.cName == null || !(@char.cName != "") || @char.isPet || @char.isMiniPet || @char.cName.StartsWith("#") || @char.cName.StartsWith("$") || !(@char.cName != "Trọng tài") || getDistance(@char, suicidingChar) > FunctionCharEffect.CharExtensions.getSuicideRange(suicidingChar) || ((suicidingChar.cFlag == 0 || @char.cFlag == 0 || (@char.cFlag == suicidingChar.cFlag && (@char.cFlag != 8 || suicidingChar.cFlag != 8))) && suicidingChar.cTypePk != 5 && @char.cTypePk != 5))
				{
					continue;
				}
				if (suicidingChar.me)
				{
					if (!mapObjsInMyRange.Contains(@char))
					{
						mapObjsInMyRange.Add(@char);
					}
				}
				else if (!mapObjsInRange.Contains(@char))
				{
					mapObjsInRange.Add(@char);
				}
			}
			for (int j = 0; j < GameScr.vMob.size(); j++)
			{
				Mob mob = GameScr.vMob.elementAt(j) as Mob;
				if (getDistance(mob, suicidingChar) > FunctionCharEffect.CharExtensions.getSuicideRange(suicidingChar) || mapObjsInRange.Contains(mob) || mob.isMobMe)
				{
					continue;
				}
				if (suicidingChar.me)
				{
					if (!mapObjsInMyRange.Contains(mob))
					{
						mapObjsInMyRange.Add(mob);
					}
				}
				else if (!mapObjsInRange.Contains(mob))
				{
					mapObjsInRange.Add(mob);
				}
			}
		}

		public static void setState(bool value)
		{
			isShowSuicideRange = value;
		}
	}

	private static FunctionChar _Instance;

	public static int runSpeed = 7;

	public static bool enablePaintListCharInMap;

	public static bool enablePaintFocusInformation;

	public static bool enableAutoTeleport = false;

	public static int CharIDTeleport;

	public static long TIME_DELAY_TELEPORT;

	public static long CountTeleport;

	public static bool enableLockMobFocus;

	public static bool enableLockCharFocus;

	public static int MobID;

	public static int CharID;

	public static FunctionChar gI()
	{
		if (_Instance == null)
		{
			_Instance = new FunctionChar();
		}
		return _Instance;
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 10204:
			GameCanvas.gI().keyPressedz(98);
			GameScr.info1.addInfo("Hãy chọn đối tượng để teleport tới", 0);
			break;
		case 10005:
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command("Thông tin\n đối thủ\n" + StringHandle.StatusMenu(enablePaintFocusInformation), gI(), 20301, null));
			myVector.addElement(new Command("Chế độ\n hiển thị\n" + ((ListCharsInMap.TYPE_SORT == 1) ? "màu cờ" : "Bang hội"), gI(), 20302, null));
			myVector.addElement(new Command("D/s nhân vật\nTrong map\n" + StringHandle.StatusMenu(enablePaintListCharInMap), gI(), 20303, null));
			GameCanvas.menu.startAt(myVector, 0);
			break;
		}
		case 20301:
			enablePaintFocusInformation = !enablePaintFocusInformation;
			GameScr.info1.addInfo("[ThanhLc] Hiển thị thông tin đối thủ: " + StringHandle.Status(enablePaintFocusInformation), 0);
			break;
		case 20302:
			if (ListCharsInMap.TYPE_SORT == 1)
			{
				ListCharsInMap.TYPE_SORT = 2;
			}
			else
			{
				ListCharsInMap.TYPE_SORT = 1;
			}
			break;
		case 20303:
			enablePaintListCharInMap = !enablePaintListCharInMap;
			GameScr.info1.addInfo("[ThanhLc] Hiển thị d/s nhân vật trong map: " + StringHandle.Status(enablePaintListCharInMap), 0);
			break;
		}
	}

	public static bool chat(string text)
	{
		if (text == "/alg")
		{
			FunctionLogin.enableAutoLogin = !FunctionLogin.enableAutoLogin;
			GameScr.info1.addInfo("[ThanhLc] Auto Login :" + StringHandle.Status(FunctionLogin.enableAutoLogin), 0);
		}
		else if (StringHandle.IsGetInfoChat<int>(text, "/kmt"))
		{
			CharID = StringHandle.GetInfoChat<int>(text, "/kmt");
			enableLockCharFocus = !enableLockCharFocus;
			GameScr.info1.addInfo("[ThanhLc] Khóa mục tiêu id [" + CharID + "] :" + StringHandle.Status(enableLockCharFocus), 0);
		}
		else if (StringHandle.IsGetInfoChat<int>(text, "/c"))
		{
			runSpeed = StringHandle.GetInfoChat<int>(text, "/c");
			GameScr.info1.addInfo("[ThanhLc] Tốc độ chạy đã được chỉnh thành " + runSpeed, 0);
		}
		else if (StringHandle.IsGetInfoChat<int>(text, "/s"))
		{
			Time.timeScale = StringHandle.GetInfoChat<int>(text, "/s");
			GameScr.info1.addInfo("[ThanhLc] Tốc độ game đã được chỉnh thành " + Time.timeScale, 0);
		}
		else if (StringHandle.IsGetInfoChat<int>(text, "/l"))
		{
			Char.myCharz().cx -= StringHandle.GetInfoChat<int>(text, "/l");
			GameScr.info1.addInfo("[ThanhLc] Dịch trái", 0);
		}
		else if (StringHandle.IsGetInfoChat<int>(text, "/r"))
		{
			Char.myCharz().cx += StringHandle.GetInfoChat<int>(text, "/r");
			GameScr.info1.addInfo("[ThanhLc] Dịch phải", 0);
		}
		else if (StringHandle.IsGetInfoChat<int>(text, "/d"))
		{
			Char.myCharz().cy += StringHandle.GetInfoChat<int>(text, "/d");
			GameScr.info1.addInfo("[ThanhLc] Dịch xuống", 0);
		}
		else
		{
			if (!StringHandle.IsGetInfoChat<int>(text, "/u"))
			{
				return false;
			}
			Char.myCharz().cy -= StringHandle.GetInfoChat<int>(text, "/u");
			GameScr.info1.addInfo("[ThanhLc] Dịch lên", 0);
		}
		return true;
	}

	public static void Update()
	{
		Char.myCharz().cspeed = runSpeed;
		FunctionCharEffect.CharEffect.Update();
		ListCharsInMap.Update();
		SuicideRange.update();
		try
		{
			AutoHoldFocus();
		}
		catch (Exception ex)
		{
			FunctionMain.WriteError("Data/Errors/AutoHoldFocus.txt", ex.Message);
		}
		try
		{
			AutoTeleport();
		}
		catch (Exception ex2)
		{
			FunctionMain.WriteError("Data/Errors/AutoTeleport.txt", ex2.Message);
		}
	}

	public static bool HotKey(int KeyPress)
	{
		switch (KeyPress)
		{
		case 61:
			SoundMn.gI().buttonClick();
			Char.myCharz().currentMovePoint = null;
			GameCanvas.clearAllPointerEvent();
			GameCanvas.panel2 = new Panel();
			GameCanvas.panel2.show();
			GameCanvas.panel.setTypeBox();
			GameCanvas.panel.show();
			break;
		case 105:
			if (Char.myCharz().charFocus != null)
			{
				CharID = Char.myCharz().charFocus.charID;
				enableLockCharFocus = !enableLockCharFocus;
				GameScr.info1.addInfo("[ThanhLc] Giữ mục tiêu: " + Char.myCharz().charFocus.cName + " " + StringHandle.Status(enableLockCharFocus), 0);
			}
			if (Char.myCharz().mobFocus != null)
			{
				MobID = Char.myCharz().mobFocus.mobId;
				enableLockMobFocus = !enableLockMobFocus;
				GameScr.info1.addInfo("[ThanhLc] Giữ mục tiêu: " + Char.myCharz().mobFocus.mobName + " " + StringHandle.Status(enableLockMobFocus), 0);
			}
			break;
		default:
			return false;
		}
		return true;
	}

	public static void paint(mGraphics g)
	{
		FunctionCharEffect.CharEffect.paint(g);
		ListCharsInMap.paint(g);
		SuicideRange.paint(g);
		paintLockFocus(g);
	}

	public static void AutoTeleport()
	{
		if (enableAutoTeleport)
		{
			if (Char.myCharz().meDead && TileMap.mapID != Char.myCharz().cgender + 21)
			{
				Service.gI().returnTownFromDead();
			}
			if (TileMap.mapID != Char.myCharz().cgender + 21 && mSystem.currentTimeMillis() - TIME_DELAY_TELEPORT < 10000)
			{
				FunctionTrainMob.IsTanSat = true;
			}
			else if ((TileMap.mapID == Char.myCharz().cgender + 21 || GameScr.findCharInMap(CharIDTeleport) == null) && mSystem.currentTimeMillis() - TIME_DELAY_TELEPORT > 10100)
			{
				Teleport(CharIDTeleport);
				TIME_DELAY_TELEPORT = mSystem.currentTimeMillis();
			}
			CountTeleport = 10 - (mSystem.currentTimeMillis() - TIME_DELAY_TELEPORT) / 1000;
		}
	}

	public static void teleportMyChar(int x, int y)
	{
		Char.myCharz().currentMovePoint = null;
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

	public static void teleportMyChar(IMapObject obj)
	{
		teleportMyChar(obj.getX(), obj.getY());
	}

	public static void AutoHoldFocus()
	{
		if (enableLockCharFocus)
		{
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				Char @char = (Char)GameScr.vCharInMap.elementAt(i);
				if (!@char.isMiniPet && @char.charID == CharID)
				{
					Char.myCharz().mobFocus = null;
					Char.myCharz().npcFocus = null;
					Char.myCharz().itemFocus = null;
					Char.myCharz().charFocus = @char;
					break;
				}
			}
		}
		if (!enableLockMobFocus)
		{
			return;
		}
		for (int j = 0; j < GameScr.vMob.size(); j++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(j);
			if (mob != null && mob.mobId == MobID)
			{
				Char.myCharz().npcFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().charFocus = null;
				Char.myCharz().mobFocus = mob;
				break;
			}
		}
	}

	public static void paintLockFocus(mGraphics g)
	{
		try
		{
			if (enableLockCharFocus)
			{
				for (int i = 0; i < GameScr.vCharInMap.size(); i++)
				{
					Char @char = (Char)GameScr.vCharInMap.elementAt(i);
					if (!@char.isMiniPet && @char.charID == CharID && Res.distance(Char.myCharz().cx, Char.myCharz().cy, @char.cx, @char.cy) > 5)
					{
						g.setColor(Color.red);
						g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy, @char.cx - GameScr.cmx, @char.cy - GameScr.cmy);
					}
				}
			}
			if (!enableLockMobFocus)
			{
				return;
			}
			for (int j = 0; j < GameScr.vMob.size(); j++)
			{
				Mob mob = (Mob)GameScr.vMob.elementAt(j);
				if (mob.mobId == MobID && Res.distance(Char.myCharz().cx, Char.myCharz().cy, mob.x, mob.y) > 5)
				{
					g.setColor(Color.red);
					g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy, mob.x - GameScr.cmx, mob.y - GameScr.cmy);
				}
			}
		}
		catch (Exception ex)
		{
			FunctionMain.WriteError("Data/Errors/paintLockFocus.txt", ex.Message);
		}
	}

	public static void Teleport(int CharID)
	{
		Item[] arrItemBody = Char.myCharz().arrItemBody;
		if (arrItemBody[5] == null)
		{
			Service.gI().getItem(4, (sbyte)FindYardrat());
			Service.gI().gotoPlayer(CharID);
			Service.gI().getItem(5, 5);
		}
		else if (arrItemBody[5].template.name.Contains("Yardrat"))
		{
			Service.gI().gotoPlayer(CharID);
		}
		else if (!arrItemBody[5].template.name.Contains("Yardrat"))
		{
			Service.gI().getItem(4, (sbyte)FindYardrat());
			Service.gI().gotoPlayer(CharID);
			Service.gI().getItem(4, (sbyte)FindYardrat());
		}
	}

	public static bool isCharInMap(int charID)
	{
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			if (GameScr.vCharInMap.elementAt(i) is Char @char && @char.charID == CharID)
			{
				return true;
			}
		}
		return false;
	}

	public static int FindYardrat()
	{
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			if (Char.myCharz().arrItemBag[i].template.name.Contains("Yardrat"))
			{
				return i;
			}
		}
		return -1;
	}
}

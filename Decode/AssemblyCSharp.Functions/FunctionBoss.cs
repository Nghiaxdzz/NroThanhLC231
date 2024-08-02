using System;
using System.Collections.Generic;

namespace AssemblyCSharp.Functions;

public class FunctionBoss : IActionListener
{
	private static FunctionBoss _Instance;

	public static bool enableKillBoss;

	public static bool enableAutoPorata;

	public static long TIME_DELAY_USE_PORATA;

	public static short ID_PORATA_LV1 = 454;

	public static short ID_PORATA_LV2 = 921;

	public static bool enableAutoChangeFocus;

	public static int IndexFocus = 0;

	public static long TIME_DELAY_CHANGE_FOCUS;

	public static List<Char> ListYardarts = new List<Char>();

	public static int IndexNPCFocus;

	public static int IndexBossFocus;

	public static List<Npc> ListNPCs = new List<Npc>();

	public static List<Char> ListBosses = new List<Char>();

	public static int NPC_INDEX;

	public static int BOSS_INDEX;

	public static FunctionBoss gI()
	{
		if (_Instance == null)
		{
			_Instance = new FunctionBoss();
		}
		return _Instance;
	}

	public static bool chat(string text)
	{
		switch (text)
		{
		case "/tbb":
			ListBossInformation.enableListBossInformation = !ListBossInformation.enableListBossInformation;
			GameScr.info1.addInfo("[ThanhLc] D/s Boss xuất hiện: " + StringHandle.Status(ListBossInformation.enableListBossInformation), 0);
			break;
		case "/kssp":
			enableKillBoss = !enableKillBoss;
			GameScr.info1.addInfo("[ThanhLc] KS Super Broly: " + StringHandle.Status(enableKillBoss), 0);
			break;
		case "/dmt":
			enableAutoChangeFocus = !enableAutoChangeFocus;
			GameScr.info1.addInfo("[ThanhLc] Auto chuyển mục tiêu: " + StringHandle.Status(enableAutoChangeFocus), 0);
			break;
		case "/abt":
			enableAutoPorata = !enableAutoPorata;
			GameScr.info1.addInfo("[ThanhLc] Auto Porata: " + StringHandle.Status(enableAutoPorata), 0);
			break;
		default:
			return false;
		}
		return true;
	}

	public static bool HotKey(int KeyPress)
	{
		switch (KeyPress)
		{
		case 119:
			ChangeFocus("npc");
			break;
		case 101:
			if (!FunctionNRSD.isMeInNRDMap())
			{
				ChangeFocus("boss");
			}
			else
			{
				Service.gI().chat("/ahs");
			}
			break;
		default:
			return false;
		}
		return true;
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 10001:
		{
			MyVector myVector2 = new MyVector();
			myVector2.addElement(new Command("Thông báo\n Boss\n" + StringHandle.StatusMenu(ListBossInformation.enableListBossInformation), _Instance, 101201, null));
			myVector2.addElement(new Command("KS\n Super Broly\n" + StringHandle.StatusMenu(enableKillBoss), _Instance, 101202, null));
			GameCanvas.menu.startAt(myVector2, 0);
			break;
		}
		case 101201:
			Service.gI().chat("/tbb");
			break;
		case 101202:
			Service.gI().chat("/kssp");
			break;
		case 10002:
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command("Auto\nPorata\n" + StringHandle.StatusMenu(enableAutoPorata), _Instance, 101203, null));
			myVector.addElement(new Command("Auto chuyển\nMục tiêu\n" + StringHandle.StatusMenu(enableAutoChangeFocus), _Instance, 101204, null));
			GameCanvas.menu.startAt(myVector, 0);
			break;
		}
		case 101203:
			Service.gI().chat("/abt");
			break;
		case 101204:
			Service.gI().chat("/dmt");
			break;
		}
	}

	public static void Update()
	{
		try
		{
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				if (GameScr.vCharInMap.elementAt(i) is Char { cHP: <=0, charID: <0 } @char && GameScr.vCharInMap.contains(@char))
				{
					GameScr.vCharInMap.removeElement(@char);
				}
			}
		}
		catch
		{
		}
		try
		{
			KillBoss();
		}
		catch (Exception ex)
		{
			FunctionMain.WriteError("Data/Errors/KillBoss.txt", ex.Message);
		}
		try
		{
			ListBossInformation.updateListBoss();
		}
		catch (Exception ex2)
		{
			FunctionMain.WriteError("Data/Errors/updateListBoss.txt", ex2.Message);
		}
		try
		{
			AddYardarts();
			AutoChangeFocus();
		}
		catch (Exception ex3)
		{
			FunctionMain.WriteError("Data/Errors/Yardart.txt", ex3.Message);
		}
		try
		{
			AddBossInMap();
		}
		catch (Exception ex4)
		{
			FunctionMain.WriteError("Data/Errors/NPC_Boss.txt", ex4.Message);
		}
		try
		{
			AutoPorata();
		}
		catch (Exception ex5)
		{
			FunctionMain.WriteError("Data/Errors/AutoPorata.txt", ex5.Message);
		}
		AddNPCInMap();
	}

	public static void paint(mGraphics g)
	{
		ListBossInformation.paintListBossOnScreen(g);
	}

	public static void KillBoss()
	{
		if (!enableKillBoss)
		{
			return;
		}
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			if (!(GameScr.vCharInMap.elementAt(i) is Char { charID: <0 } @char) || !@char.cName.ToLower().StartsWith("super broly"))
			{
				continue;
			}
			bool flag = ItemTime.isExistItem(4387);
			int cx = @char.cx;
			int cy = @char.cy;
			if (Char.myCharz().charFocus != @char)
			{
				Char.myCharz().mobFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().npcFocus = null;
				Char.myCharz().charFocus = @char;
				break;
			}
			if (@char.cHP != 1)
			{
				continue;
			}
			if (Char.myCharz().cx != cx)
			{
				Char.myCharz().cx = cx;
				Char.myCharz().cy = cy;
				Service.gI().charMove();
				if (!flag)
				{
					Char.myCharz().cx = cx;
					Char.myCharz().cy = cy + 1;
					Service.gI().charMove();
					Char.myCharz().cx = cx;
					Char.myCharz().cy = cy;
					Service.gI().charMove();
				}
				break;
			}
			FunctionSkill.gI().AutoSendAttack();
		}
	}

	public static void AutoPorata()
	{
		if (!enableAutoPorata || Char.myCharz().cHP > Char.myCharz().cHPFull * 50 / 100 || mSystem.currentTimeMillis() - TIME_DELAY_USE_PORATA <= 10120)
		{
			return;
		}
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			Item item = Char.myCharz().arrItemBag[i];
			if (item != null)
			{
				if (item.template.id == ID_PORATA_LV1)
				{
					Service.gI().useItem(0, 1, -1, ID_PORATA_LV1);
					Service.gI().useItem(0, 1, -1, ID_PORATA_LV1);
					TIME_DELAY_USE_PORATA = mSystem.currentTimeMillis();
					Service.gI().petStatus(3);
				}
				else if (item.template.id == ID_PORATA_LV2)
				{
					Service.gI().useItem(0, 1, -1, ID_PORATA_LV2);
					Service.gI().useItem(0, 1, -1, ID_PORATA_LV2);
					TIME_DELAY_USE_PORATA = mSystem.currentTimeMillis();
					Service.gI().petStatus(3);
				}
			}
		}
	}

	public static void AddYardarts()
	{
		if (!enableAutoChangeFocus)
		{
			return;
		}
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char @char = (Char)GameScr.vCharInMap.elementAt(i);
			if (@char != null && @char.cTypePk == 5 && @char.charID < 0 && Res.distance(Char.myCharz().cx, Char.myCharz().cy, @char.cx, @char.cy) <= 100 && !ListYardarts.Contains(@char))
			{
				ListYardarts.Add(@char);
			}
			for (int j = 0; j < ListYardarts.Count; j++)
			{
				if (Res.distance(Char.myCharz().cx, Char.myCharz().cy, ListYardarts[j].cx, ListYardarts[j].cy) > 100)
				{
					ListYardarts.RemoveAt(j);
				}
			}
		}
	}

	public static void AutoChangeFocus()
	{
		if (!enableAutoChangeFocus || mSystem.currentTimeMillis() - TIME_DELAY_CHANGE_FOCUS <= 1500)
		{
			return;
		}
		if (ListYardarts.Count == 1)
		{
			Char.myCharz().itemFocus = null;
			Char.myCharz().npcFocus = null;
			Char.myCharz().mobFocus = null;
			Char.myCharz().charFocus = ListYardarts[0];
			TIME_DELAY_CHANGE_FOCUS = mSystem.currentTimeMillis();
		}
		else if (ListYardarts.Count > 1)
		{
			if (IndexFocus <= ListYardarts.Count)
			{
				Char.myCharz().mobFocus = null;
				Char.myCharz().npcFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().charFocus = ListYardarts[IndexFocus];
				IndexFocus++;
				TIME_DELAY_CHANGE_FOCUS = mSystem.currentTimeMillis();
			}
			if (IndexFocus == ListYardarts.Count)
			{
				IndexFocus = 0;
			}
		}
	}

	public static void AddNPCInMap()
	{
		for (int i = 0; i < GameScr.vNpc.size(); i++)
		{
			Npc npc = (Npc)GameScr.vNpc.elementAt(i);
			if (npc != null && !ListNPCs.Contains(npc))
			{
				ListNPCs.Add(npc);
			}
			for (int j = 0; j < ListNPCs.Count; j++)
			{
				if (GameScr.findNPCInMap((short)ListNPCs[j].template.npcTemplateId) == null)
				{
					ListNPCs.RemoveAt(j);
				}
			}
		}
	}

	public static void ChangeFocus(string type)
	{
		if (!(type == "npc"))
		{
			if (!(type == "boss"))
			{
				return;
			}
			List<Char> listBosses = ListBosses;
			if (BOSS_INDEX > listBosses.Count)
			{
				BOSS_INDEX = 0;
			}
			if (listBosses.Count <= 0 || BOSS_INDEX >= listBosses.Count)
			{
				return;
			}
			if (Char.myCharz().charFocus != listBosses[BOSS_INDEX])
			{
				Char.myCharz().mobFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().npcFocus = null;
				Char.myCharz().charFocus = listBosses[BOSS_INDEX];
			}
			if (Char.myCharz().charFocus == listBosses[BOSS_INDEX])
			{
				if (listBosses.Count - BOSS_INDEX > 1)
				{
					BOSS_INDEX++;
				}
				else
				{
					BOSS_INDEX = 0;
				}
			}
			return;
		}
		List<Npc> listNPCs = ListNPCs;
		if (NPC_INDEX > listNPCs.Count)
		{
			NPC_INDEX = 0;
		}
		if (listNPCs.Count <= 0 || NPC_INDEX >= listNPCs.Count)
		{
			return;
		}
		if (Char.myCharz().npcFocus != listNPCs[NPC_INDEX])
		{
			Char.myCharz().charFocus = null;
			Char.myCharz().mobFocus = null;
			Char.myCharz().itemFocus = null;
			Char.myCharz().npcFocus = listNPCs[NPC_INDEX];
		}
		if (Char.myCharz().npcFocus == listNPCs[NPC_INDEX])
		{
			Char.myCharz().cx = listNPCs[NPC_INDEX].cx;
			Char.myCharz().cy = listNPCs[NPC_INDEX].cy - 3;
			Service.gI().charMove();
			Char.myCharz().cx = listNPCs[NPC_INDEX].cx;
			Char.myCharz().cy = listNPCs[NPC_INDEX].cy;
			Service.gI().charMove();
			Char.myCharz().cx = listNPCs[NPC_INDEX].cx;
			Char.myCharz().cy = listNPCs[NPC_INDEX].cy - 3;
			Service.gI().charMove();
			if (listNPCs.Count - NPC_INDEX > 1)
			{
				NPC_INDEX++;
			}
			else
			{
				NPC_INDEX = 0;
			}
		}
	}

	public static void AddBossInMap()
	{
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char @char = (Char)GameScr.vCharInMap.elementAt(i);
			if (@char != null && @char.cTypePk == 5 && @char.charID < 0 && !ListBosses.Contains(@char) && @char.cHP > 0)
			{
				ListBosses.Add(@char);
			}
			for (int j = 0; j < ListBosses.Count; j++)
			{
				if (ListBosses.Count == 0)
				{
					return;
				}
				if (GameScr.findCharInMap(ListBosses[j].charID) == null)
				{
					ListBosses.RemoveAt(j);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp.Functions;

public class FunctionPet : IActionListener
{
	private static FunctionPet _Instance;

	public static int yDrawPetInfo = 0;

	public static bool enablePaintPetInformation;

	public static int MaxLength;

	public static long TIME_DELAY_DOFIREPET;

	public static bool enableGiveBean;

	public static long TIME_DELAY_GIVE_BEAN;

	public static List<string> ListMembers = new List<string>();

	public static bool enableRequestBean;

	public static long TIME_DELAY_REQUEST_BEAN;

	public static bool enableCollectBean;

	public static long TIME_DELAY_COLLECT_BEAN;

	public static bool enableAutoJump;

	public static long TIME_DELAY_JUMP;

	public static bool IsFindMob;

	public static bool enablePetFollow;

	public static long TIME_DELAY_CALL_PET;

	public static bool enableAutoGetFlag;

	public static long TIME_DELAY_GET_FLAG;

	public static bool enableSpamRequestBean;

	public static int countBean;

	public static bool enableAutoRecovering;

	public static List<Char> ListChar = new List<Char>();

	public static bool enableAutoPoint;

	public static int[] Points = new int[3];

	public static long TIME_DELAY_POINT;

	public static int PET_PERCENT_HP = 0;

	public static long TIME_DELAY_USE_BEAN;

	public static bool enableAutoRevive;

	public static bool enableSetPetGoHome;

	public static long PET_LIMIT_POWER = 0L;

	public static FunctionPet gI()
	{
		if (_Instance == null)
		{
			_Instance = new FunctionPet();
		}
		return _Instance;
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 10003:
		{
			MyVector myVector2 = new MyVector();
			myVector2.addElement(new Command("Cài đặt\nĐậu thần", this, 20201, null));
			myVector2.addElement(new Command("Thông tin\nĐệ tử\n" + StringHandle.StatusMenu(enablePaintPetInformation), this, 20202, null));
			myVector2.addElement(new Command("Auto Jump\n" + StringHandle.StatusMenu(enableAutoJump), this, 20203, null));
			myVector2.addElement(new Command("Auto hồi sinh\n" + StringHandle.StatusMenu(enableAutoRevive), this, 20204, null));
			myVector2.addElement(new Command("Auto bật\n cờ xám \n" + StringHandle.StatusMenu(enableAutoGetFlag), this, 20205, null));
			myVector2.addElement(new Command("Up đệ né\n Siêu quái\n" + StringHandle.StatusMenu(enablePetFollow), this, 20206, null));
			myVector2.addElement(new Command("Auto cho\nđệ về nhà\n" + StringHandle.StatusMenu(enableSetPetGoHome), this, 20207, null));
			if (Char.myCharz().cgender == 1)
			{
				myVector2.addElement(new Command("Trị thương\nTheo d/s\n" + StringHandle.StatusMenu(enableAutoRecovering), this, 20208, null));
			}
			GameCanvas.menu.startAt(myVector2, 0);
			break;
		}
		case 20201:
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command("Auto xin đậu\n" + StringHandle.StatusMenu(enableRequestBean), this, 20101, null));
			myVector.addElement(new Command("Auto thu đậu\n" + StringHandle.StatusMenu(enableCollectBean), this, 20102, null));
			myVector.addElement(new Command("Auto cho đậu\n" + StringHandle.StatusMenu(enableGiveBean), this, 20103, null));
			myVector.addElement(new Command("Xin đậu\nĐặc biệt\n" + StringHandle.StatusMenu(enableSpamRequestBean), this, 20104, null));
			GameCanvas.menu.startAt(myVector, 0);
			break;
		}
		case 20101:
			chat("/xd");
			break;
		case 20102:
			chat("/td");
			break;
		case 20103:
			chat("/cd");
			break;
		case 20104:
			chat("/axd");
			break;
		case 20202:
			chat("/ttdt");
			break;
		case 20203:
			chat("/akok");
			break;
		case 20204:
			chat("/ahs");
			break;
		case 20205:
			chat("/aflag");
			break;
		case 20206:
			chat("/dtnsq");
			break;
		case 20207:
			chat("/petgohome");
			break;
		case 20208:
			chat("/arc");
			break;
		}
	}

	public static bool chat(string text)
	{
		switch (text)
		{
		case "/cd":
			enableGiveBean = !enableGiveBean;
			GameScr.info1.addInfo("[ThanhLc] Auto cho đậu: " + StringHandle.Status(enableGiveBean), 0);
			break;
		case "/xd":
			enableRequestBean = !enableRequestBean;
			GameScr.info1.addInfo("[ThanhLc] Auto xin đậu: " + StringHandle.Status(enableRequestBean), 0);
			break;
		case "/td":
			enableCollectBean = !enableCollectBean;
			GameScr.info1.addInfo("[ThanhLc] Auto thu đậu: " + StringHandle.Status(enableCollectBean), 0);
			break;
		case "/akok":
			enableAutoJump = !enableAutoJump;
			GameScr.info1.addInfo("[ThanhLc] Auto jump: " + StringHandle.Status(enableAutoJump), 0);
			break;
		case "/dtnsq":
			enablePetFollow = !enablePetFollow;
			GameScr.info1.addInfo("[ThanhLc] Up đệ tử né siêu quái: " + StringHandle.Status(enablePetFollow), 0);
			break;
		case "/aflag":
			enableAutoGetFlag = !enableAutoGetFlag;
			GameScr.info1.addInfo("[ThanhLc] Auto bật cờ xám: " + StringHandle.Status(enableAutoGetFlag), 0);
			break;
		case "/petgohome":
			enableSetPetGoHome = !enableSetPetGoHome;
			GameScr.info1.addInfo("[ThanhLc] Auto cho đệ về nhà khi tách hợp thể: " + StringHandle.Status(enableSetPetGoHome), 0);
			break;
		case "/axd":
			enableSpamRequestBean = !enableSpamRequestBean;
			GameScr.info1.addInfo("[ThanhLc] Auto xin đậu liên tục: " + StringHandle.Status(enableSpamRequestBean), 0);
			break;
		case "/ttdt":
			enablePaintPetInformation = !enablePaintPetInformation;
			GameScr.info1.addInfo("[ThanhLc] Hiển thị thông tin đệ tử: " + StringHandle.Status(enablePaintPetInformation), 0);
			break;
		case "/addc":
			if (Char.myCharz().cgender == 1)
			{
				Char charFocus = Char.myCharz().charFocus;
				if (!ListChar.Contains(charFocus))
				{
					ListChar.Add(charFocus);
					GameScr.info1.addInfo("[ThanhLc] Đã thêm [" + charFocus.cName + "] vào d/s trị thương", 0);
				}
				else
				{
					ListChar.Remove(charFocus);
					GameScr.info1.addInfo("[ThanhLc] Đã xóa [" + charFocus.cName + "] khỏi d/s trị thương", 0);
				}
			}
			break;
		case "/arc":
			if (Char.myCharz().cgender == 1)
			{
				enableAutoRecovering = !enableAutoRecovering;
				GameScr.info1.addInfo("[ThanhLc] Auto trị thương theo danh sách: " + StringHandle.Status(enableAutoRecovering), 0);
			}
			break;
		case "/ahs":
			enableAutoRevive = !enableAutoRevive;
			GameScr.info1.addInfo("[ThanhLc] Tự động hồi sinh bằng ngọc: " + StringHandle.Status(enableAutoRevive), 0);
			break;
		default:
			if (StringHandle.IsGetInfoChat<int>(text, "/php"))
			{
				PET_PERCENT_HP = StringHandle.GetInfoChat<int>(text, "/php");
				GameScr.info1.addInfo("[ThanhLc] Auto dùng đậu khi đệ dưới " + PET_PERCENT_HP + "% HP", 0);
				break;
			}
			if (StringHandle.IsGetInfoChat<int>(text, "/hp"))
			{
				Points[0] = StringHandle.GetInfoChat<int>(text, "/hp");
				enableAutoPoint = true;
				break;
			}
			if (StringHandle.IsGetInfoChat<int>(text, "/ki"))
			{
				Points[1] = StringHandle.GetInfoChat<int>(text, "/ki");
				enableAutoPoint = true;
				break;
			}
			if (StringHandle.IsGetInfoChat<int>(text, "/sd"))
			{
				Points[2] = StringHandle.GetInfoChat<int>(text, "/sd");
				enableAutoPoint = true;
				break;
			}
			return false;
		}
		return true;
	}

	public static void Update()
	{
		try
		{
			AutoGiveBean();
		}
		catch (Exception ex)
		{
			FunctionMain.WriteError("Data/Errors/GiveBean.txt", ex.Message);
		}
		try
		{
			AutoRequestBean();
		}
		catch (Exception ex2)
		{
			FunctionMain.WriteError("Data/Errors/AutoRequestBean.txt", ex2.Message);
		}
		try
		{
			AutoCollectBean();
		}
		catch (Exception ex3)
		{
			FunctionMain.WriteError("Data/Errors/AutoCollectBean.txt", ex3.Message);
		}
		try
		{
			AutoJump();
		}
		catch (Exception ex4)
		{
			FunctionMain.WriteError("Data/Errors/AutoJump.txt", ex4.Message);
		}
		try
		{
			PetFollow();
		}
		catch (Exception ex5)
		{
			FunctionMain.WriteError("Data/Errors/PetFollow.txt", ex5.Message);
		}
		try
		{
			AutoGetFlag();
		}
		catch (Exception ex6)
		{
			FunctionMain.WriteError("Data/Errors/AutoGetFlag.txt", ex6.Message);
		}
		try
		{
			AutoSpamRequestBean();
		}
		catch (Exception ex7)
		{
			FunctionMain.WriteError("Data/Errors/AutoSpamRequestBean.txt", ex7.Message);
		}
		try
		{
			AutoRecovering();
		}
		catch (Exception ex8)
		{
			FunctionMain.WriteError("Data/Errors/AutoRecovering.txt", ex8.Message);
		}
		try
		{
			AutoPoint();
		}
		catch (Exception ex9)
		{
			FunctionMain.WriteError("Data/Errors/AutoRecovering.txt", ex9.Message);
		}
		try
		{
			AutoUseBeanForPet();
		}
		catch (Exception ex10)
		{
			FunctionMain.WriteError("Data/Errors/AutoUseBeanForPet.txt", ex10.Message);
		}
		try
		{
			AutoRevive();
		}
		catch (Exception ex11)
		{
			FunctionMain.WriteError("Data/Errors/AutoRevive.txt", ex11.Message);
		}
		try
		{
			SetPetGoHome();
		}
		catch (Exception ex12)
		{
			FunctionMain.WriteError("Data/Errors/SetPetGoHome.txt", ex12.Message);
		}
		try
		{
			CountPetPower();
		}
		catch (Exception ex13)
		{
			FunctionMain.WriteError("Data/Errors/CountPetPower.txt", ex13.Message);
		}
	}

	public static void UpdateTouch()
	{
		if (!Char.myCharz().havePet)
		{
			return;
		}
		try
		{
			if (GameCanvas.isPointerHoldIn(0, 150, mGraphics.getImageWidth(GameScr.imgMenu), mGraphics.getImageHeight(GameScr.imgMenu)) && !GameCanvas.isPointerDown)
			{
				SoundMn.gI().buttonClick();
				GameCanvas.panel.currentTabIndex = 4;
				GameCanvas.panel.setTypeMain();
				GameCanvas.panel.show();
				Service.gI().petInfo();
				GameCanvas.panel2 = new Panel();
				GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
				GameCanvas.panel2.setTypeBodyOnly();
				GameCanvas.panel2.show();
				if (GameCanvas.panel2.isShow)
				{
					GameCanvas.panel.currentTabIndex = 0;
					GameCanvas.panel.tabName[21] = mResources.petMainTab;
					GameCanvas.panel.setTypePetMain();
					GameCanvas.panel.show();
				}
				Char.myCharz().currentMovePoint = null;
				GameCanvas.clearAllPointerEvent();
			}
		}
		catch
		{
		}
	}

	public static void paintPetInformation(mGraphics g)
	{
		if (mGraphics.zoomLevel == 2)
		{
			if (enablePaintPetInformation)
			{
				string[] array = new string[6]
				{
					"- Sức mạnh: " + NinjaUtil.getMoneys(Char.myPetz().cPower),
					"- Tiềm năng: " + NinjaUtil.getMoneys(Char.myPetz().cTiemNang),
					"- HP: " + NinjaUtil.getMoneys(Char.myPetz().cHP) + "/" + NinjaUtil.getMoneys(Char.myPetz().cHPFull) + " (" + Math.abs(Char.myPetz().cHP * 100 / Char.myPetz().cHPFull) + "%)",
					"- KI: " + NinjaUtil.getMoneys(Char.myPetz().cMP) + "/" + NinjaUtil.getMoneys(Char.myPetz().cMPFull) + " (" + Math.abs(Char.myPetz().cMP * 100 / Char.myPetz().cMPFull) + "%)",
					"- SĐ: " + NinjaUtil.getMoneys(Char.myPetz().cDamFull) + " - Giáp: " + NinjaUtil.getMoneys(Char.myPetz().cDefull),
					null
				};
				if (Char.myPetz().cMaxStamina > 0)
				{
					array[5] = "- Thể lực: " + NinjaUtil.getMoneys(Char.myPetz().cStamina) + "/" + NinjaUtil.getMoneys(Char.myPetz().cMaxStamina);
				}
				GUIStyle[] array2 = new GUIStyle[array.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = new GUIStyle(GUI.skin.label)
					{
						alignment = TextAnchor.UpperLeft,
						fontSize = 6 * mGraphics.zoomLevel,
						fontStyle = FontStyle.Bold
					};
					array2[i].normal.textColor = Color.yellow;
				}
				for (int j = 0; j < array.Length; j++)
				{
					int width = StringHandle.getWidth(array2[j], array[j]);
					MaxLength = Math.max(width, MaxLength);
					int num = 10;
					int num2 = yDrawPetInfo + 8 * j;
					g.setColor(0, 0.8f);
					g.fillRect(num, num2 + 1, MaxLength, 7);
					g.drawString(array[j], num + 2, mGraphics.zoomLevel - 3 + num2, array2[j]);
				}
			}
		}
		else if (enablePaintPetInformation)
		{
			string[] array3 = new string[6]
			{
				"- Sức mạnh: " + NinjaUtil.getMoneys(Char.myPetz().cPower),
				"- Tiềm năng: " + NinjaUtil.getMoneys(Char.myPetz().cTiemNang),
				"- HP: " + NinjaUtil.getMoneys(Char.myPetz().cHP) + "/" + NinjaUtil.getMoneys(Char.myPetz().cHPFull) + " (" + Math.abs(Char.myPetz().cHP * 100 / Char.myPetz().cHPFull) + "%)",
				"- KI: " + NinjaUtil.getMoneys(Char.myPetz().cMP) + "/" + NinjaUtil.getMoneys(Char.myPetz().cMPFull) + " (" + Math.abs(Char.myPetz().cMP * 100 / Char.myPetz().cMPFull) + "%)",
				"- SĐ: " + NinjaUtil.getMoneys(Char.myPetz().cDamFull) + " - Giáp: " + NinjaUtil.getMoneys(Char.myPetz().cDefull),
				null
			};
			if (Char.myPetz().cMaxStamina > 0)
			{
				array3[5] = "- Thể lực: " + NinjaUtil.getMoneys(Char.myPetz().cStamina) + "/" + NinjaUtil.getMoneys(Char.myPetz().cMaxStamina);
			}
			for (int k = 0; k < array3.Length; k++)
			{
				int num3 = 10;
				int num4 = yDrawPetInfo + 10 * k;
				mFont.tahoma_7_yellow.drawString(g, array3[k], 6, mGraphics.zoomLevel - 3 + num4 + 1, mFont.LEFT);
			}
		}
	}

	public static void AutoGiveBean()
	{
		if (!enableGiveBean || mSystem.currentTimeMillis() - TIME_DELAY_GIVE_BEAN <= 1000)
		{
			return;
		}
		if (ListMembers.Count <= 0)
		{
			for (int i = 0; i < ClanMessage.vMessage.size(); i++)
			{
				ClanMessage clanMessage = (ClanMessage)ClanMessage.vMessage.elementAt(i);
				if (clanMessage.maxCap != 0 && clanMessage.playerName != Char.myCharz().cName && clanMessage.recieve != clanMessage.maxCap)
				{
					for (int j = 0; j < 5 - clanMessage.recieve; j++)
					{
						Service.gI().clanDonate(clanMessage.id);
					}
				}
			}
			TIME_DELAY_GIVE_BEAN = mSystem.currentTimeMillis();
			return;
		}
		for (int k = 0; k < ListMembers.Count; k++)
		{
			string text = ListMembers[k];
			for (int l = 0; l < ClanMessage.vMessage.size(); l++)
			{
				ClanMessage clanMessage2 = (ClanMessage)ClanMessage.vMessage.elementAt(l);
				if (clanMessage2.maxCap != 0 && clanMessage2.playerName == text && clanMessage2.recieve != clanMessage2.maxCap)
				{
					for (int m = 0; m < 5 - clanMessage2.recieve; m++)
					{
						Service.gI().clanDonate(clanMessage2.id);
					}
				}
			}
			TIME_DELAY_GIVE_BEAN = mSystem.currentTimeMillis();
		}
	}

	public static void AutoRequestBean()
	{
		if (enableRequestBean && mSystem.currentTimeMillis() - TIME_DELAY_REQUEST_BEAN > 302000)
		{
			Service.gI().clanMessage(1, "", -1);
			TIME_DELAY_REQUEST_BEAN = mSystem.currentTimeMillis();
		}
	}

	public static void AutoCollectBean()
	{
		if (!enableCollectBean || TileMap.mapID != 21 + Char.myCharz().cgender || mSystem.currentTimeMillis() - TIME_DELAY_COLLECT_BEAN <= 1000)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < Char.myCharz().arrItemBox.Length; i++)
		{
			Item item = Char.myCharz().arrItemBox[i];
			if (item != null && item.template.type == 6)
			{
				num = item.quantity;
			}
		}
		if (num < 20)
		{
			for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
			{
				Item item2 = Char.myCharz().arrItemBag[j];
				if (item2 != null && item2.template.type == 6)
				{
					Service.gI().getItem(1, (sbyte)j);
				}
			}
		}
		if (GameScr.gI().magicTree.currPeas > 0 && (GameScr.hpPotion < 10 || num < 20))
		{
			Service.gI().openMenu(4);
			Service.gI().confirmMenu(4, 0);
		}
		TIME_DELAY_COLLECT_BEAN = mSystem.currentTimeMillis();
	}

	public static void AutoJump()
	{
		if (enableAutoJump && !Char.myCharz().meDead && Char.myCharz().statusMe == 1 && !FunctionXmap.IsXmapRunning && Char.myCharz().myskill.template.id != 23 && mSystem.currentTimeMillis() - TIME_DELAY_JUMP > (FunctionNRSD.isMeInNRDMap() ? 7000 : 5000))
		{
			Char.myCharz().cy = Char.myCharz().cy - 5;
			Service.gI().charMove();
			Char.myCharz().cy = Char.myCharz().cy + 5;
			Service.gI().charMove();
			TIME_DELAY_JUMP = mSystem.currentTimeMillis();
		}
	}

	public static void AttackMob()
	{
		if (!IsFindMob)
		{
			return;
		}
		for (int i = 0; i < GameScr.keySkill.Length; i++)
		{
			Skill skill = GameScr.keySkill[i];
			if (skill != null && skill.template.id == 19)
			{
				GameScr.gI().doUseSkillNotFocus(skill);
			}
		}
		MyVector myVector = new MyVector();
		IsFindMob = false;
		myVector.addElement(ClosestMob());
		GameScr.gI().doUseSkillNotFocus(GameScr.onScreenSkill[0]);
		Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
	}

	private static Mob ClosestMob()
	{
		Mob result = null;
		int num = 9999;
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (mob.status != 0 && mob.status != 1 && mob.hp > 0 && !mob.isMobMe && mob.levelBoss == 0 && mob.getTemplate().type != 4)
			{
				int num2 = Res.distance(mob.x, mob.y, Char.myCharz().cx, Char.myCharz().cy);
				if (num > num2)
				{
					num = num2;
					result = mob;
				}
			}
		}
		return result;
	}

	public static bool CheckMob(Mob mob)
	{
		if (mob.status == 0 || mob.status == 1 || mob.hp <= 0 || mob.isMobMe)
		{
			return false;
		}
		bool flag = enablePetFollow && !ItemTime.isExistItem(4387);
		if (mob.levelBoss != 0 && flag)
		{
			return false;
		}
		if (!MobCondition(mob))
		{
			return false;
		}
		return true;
	}

	public static bool MobCondition(Mob mob)
	{
		if (FunctionTrainMob.IdMobsTanSat.Count != 0 && !FunctionTrainMob.IdMobsTanSat.Contains(mob.mobId))
		{
			return false;
		}
		if (FunctionTrainMob.TypeMobsTanSat.Count != 0 && !FunctionTrainMob.TypeMobsTanSat.Contains(mob.templateId))
		{
			return false;
		}
		return true;
	}

	public static Mob GetMob()
	{
		Mob result = null;
		int num = int.MaxValue;
		Char @char = Char.myCharz();
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			int num2 = (mob.xFirst - @char.cx) * (mob.xFirst - @char.cx) + (mob.yFirst - @char.cy) * (mob.yFirst - @char.cy);
			if (CheckMob(mob) && num2 < num && mob.getTemplate().type != 4)
			{
				result = mob;
				num = num2;
			}
		}
		return result;
	}

	public static Mob GetMobNext()
	{
		Mob result = null;
		long num = mSystem.currentTimeMillis();
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (CheckMob(mob) && mob.timeLastDie < num)
			{
				result = mob;
				num = mob.timeLastDie;
			}
		}
		return result;
	}

	public static bool CheckMobNext(Mob mob)
	{
		if (mob.isMobMe)
		{
			return false;
		}
		if (!MobCondition(mob))
		{
			return false;
		}
		if (enablePetFollow && !ItemTime.isExistItem(4387) && mob.getTemplate().hp >= 3000)
		{
			if (mob.levelBoss != 0)
			{
				Mob mob2 = null;
				bool flag = false;
				for (int i = 0; i < GameScr.vMob.size(); i++)
				{
					mob2 = (Mob)GameScr.vMob.elementAt(i);
					if (mob2.countDie == 10 && (mob2.status == 0 || mob2.status == 1))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				mob.timeLastDie = mob2.timeLastDie;
			}
			else if (mob.countDie == 10 && (mob.status == 0 || mob.status == 1))
			{
				return false;
			}
		}
		return true;
	}

	public static void PetFollow()
	{
		if (!enablePetFollow)
		{
			return;
		}
		bool flag = GameScr.findCharInMap(-Char.myCharz().charID) != null;
		bool flag2 = Char.myPetz().petStatus == 1;
		if (mSystem.currentTimeMillis() - TIME_DELAY_CALL_PET > 1000)
		{
			if (!flag2)
			{
				Service.gI().petStatus(1);
				TIME_DELAY_CALL_PET = mSystem.currentTimeMillis();
				return;
			}
			if (!flag)
			{
				for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
				{
					Item item = Char.myCharz().arrItemBag[i];
					if (item != null)
					{
						if (item.template.id == 454)
						{
							Service.gI().useItem(0, 1, -1, 454);
							TIME_DELAY_CALL_PET = mSystem.currentTimeMillis();
						}
						else if (item.template.id == 921)
						{
							Service.gI().useItem(0, 1, -1, 921);
							TIME_DELAY_CALL_PET = mSystem.currentTimeMillis();
						}
						else
						{
							Service.gI().requestChangeZone(TileMap.zoneID, -1);
							TIME_DELAY_CALL_PET = mSystem.currentTimeMillis();
						}
						return;
					}
				}
			}
		}
		bool flag3 = ItemTime.isExistItem(4387);
		Char @char = Char.myCharz();
		@char.clearFocus(0);
		if (@char.mobFocus != null && !CheckMob(@char.mobFocus))
		{
			@char.mobFocus = null;
		}
		if (@char.mobFocus == null)
		{
			@char.mobFocus = GetMob();
			if (flag3 && @char.mobFocus != null)
			{
				FunctionChar.teleportMyChar(@char.mobFocus.xFirst - 24, @char.mobFocus.yFirst);
			}
		}
		if (@char.mobFocus != null)
		{
			Mob mobFocus = @char.mobFocus;
			mobFocus.x = mobFocus.xFirst;
			mobFocus.y = mobFocus.yFirst;
			if (Res.distance(mobFocus.xFirst, mobFocus.yFirst, @char.cx, @char.cy) > 48 || mobFocus.hp <= 1)
			{
				TrainMobController.Move(mobFocus.xFirst, mobFocus.yFirst);
			}
		}
		else if (!flag3)
		{
			Mob mobNext = GetMobNext();
			if (mobNext != null)
			{
				TrainMobController.Move(mobNext.xFirst - 24, mobNext.yFirst);
			}
		}
	}

	public static void AutoGetFlag()
	{
		if (enableAutoGetFlag && Char.myCharz().cFlag != 8 && mSystem.currentTimeMillis() - TIME_DELAY_GET_FLAG > 1000)
		{
			Service.gI().getFlag(1, 8);
			TIME_DELAY_GET_FLAG = mSystem.currentTimeMillis();
		}
	}

	public static void AutoSpamRequestBean()
	{
		if (!enableSpamRequestBean || GameCanvas.gameTick % 25 != 0)
		{
			return;
		}
		if (countBean == 0)
		{
			countBean++;
			Service.gI().clanMessage(1, "", -1);
			return;
		}
		countBean = 0;
		if (GameCanvas.loginScr == null)
		{
			GameCanvas.loginScr = new LoginScr();
		}
		Session_ME.gI().close();
		GameCanvas.currentScreen = GameCanvas.loginScr;
	}

	public static void AutoRecovering()
	{
		if (!enableAutoRecovering)
		{
			return;
		}
		for (int i = 0; i < ListChar.Count && GameScr.findCharInMap(ListChar[i].charID) != null && (ListChar[i].cHP < ListChar[i].cHPFull * 10 / 100 || ListChar[i].cHP <= 1 || ListChar[i].meDead); i++)
		{
			Skill[] array = (GameCanvas.isTouch ? GameScr.onScreenSkill : GameScr.keySkill);
			foreach (Skill skill in array)
			{
				if (skill == null || mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill <= skill.coolDown + 100 || skill.template.type != 2)
				{
					continue;
				}
				int num = 0;
				num = ((skill.template.manaUseType == 2) ? 1 : ((skill.template.manaUseType == 1) ? (skill.manaUse * Char.myCharz().cMPFull / 100) : skill.manaUse));
				if (Char.myCharz().cMP >= num)
				{
					if (skill != Char.myCharz().myskill)
					{
						GameScr.gI().doSelectSkill(skill, isShortcut: true);
						return;
					}
					MyVector myVector = new MyVector();
					myVector.addElement(Char.myCharz());
					Service.gI().sendPlayerAttack(new MyVector(), myVector, -1);
					skill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
					break;
				}
			}
		}
	}

	public static void AutoPoint()
	{
		if (!enableAutoPoint || mSystem.currentTimeMillis() - TIME_DELAY_POINT <= 500)
		{
			return;
		}
		int num = Points[0];
		int num2 = Points[1];
		int num3 = Points[2];
		bool flag = Char.myCharz().cTiemNang > 100 * (2 * (Char.myCharz().cHPGoc + 1000) + 1980) / 2;
		bool flag2 = Char.myCharz().cTiemNang > 10 * (2 * (Char.myCharz().cHPGoc + 1000) + 180) / 2;
		bool flag3 = Char.myCharz().cTiemNang > Char.myCharz().cHPGoc + 1000;
		bool flag4 = Char.myCharz().cTiemNang > 100 * (2 * (Char.myCharz().cMPGoc + 1000) + 1980) / 2;
		bool flag5 = Char.myCharz().cTiemNang > 10 * (2 * (Char.myCharz().cMPGoc + 1000) + 180) / 2;
		bool flag6 = Char.myCharz().cTiemNang > Char.myCharz().cMPGoc + 1000;
		bool flag7 = 100 * (2 * Char.myCharz().cDamGoc + 99) / 2 * Char.myCharz().expForOneAdd < Char.myCharz().cTiemNang;
		bool flag8 = 10 * (2 * Char.myCharz().cDamGoc + 9) / 2 * Char.myCharz().expForOneAdd < Char.myCharz().cTiemNang;
		bool flag9 = Char.myCharz().cDamGoc * 100 < Char.myCharz().cTiemNang;
		bool flag10 = ((Char.myCharz().cHPGoc >= num && num > 0) ? true : false);
		bool flag11 = ((Char.myCharz().cMPGoc >= num2 && num2 > 0) ? true : false);
		bool flag12 = ((Char.myCharz().cDamGoc >= num3 && num3 > 0) ? true : false);
		if (flag10)
		{
			Points[0] = 0;
			GameScr.info1.addInfo("Đã nâng xong HP", 0);
		}
		if (flag11)
		{
			Points[1] = 0;
			GameScr.info1.addInfo("Đã nâng xong KI", 0);
		}
		if (flag12)
		{
			Points[2] = 0;
			GameScr.info1.addInfo("Đã nâng xong SĐ", 0);
		}
		if (Points[0] + Points[1] + Points[2] == 0)
		{
			enableAutoPoint = false;
			GameCanvas.startOKDlg("Auto cộng chỉ số đã hoàn thành!");
			return;
		}
		if (num > 0 && num > Char.myCharz().cHPGoc)
		{
			if (num > 0 && Char.myCharz().cHPGoc <= num - 2000 && Char.myCharz().cTiemNang > 100 * (2 * (Char.myCharz().cHPGoc + 1000) + 1980) / 2)
			{
				Service.gI().upPotential(0, 100);
				TIME_DELAY_POINT = mSystem.currentTimeMillis();
				return;
			}
			if (num > 0 && Char.myCharz().cHPGoc <= num - 200 && Char.myCharz().cTiemNang > 10 * (2 * (Char.myCharz().cHPGoc + 1000) + 180) / 2)
			{
				Service.gI().upPotential(0, 10);
				TIME_DELAY_POINT = mSystem.currentTimeMillis();
				return;
			}
			if (num > 0 && Char.myCharz().cHPGoc <= num - 20 && Char.myCharz().cTiemNang > Char.myCharz().cHPGoc + 1000)
			{
				Service.gI().upPotential(0, 1);
				TIME_DELAY_POINT = mSystem.currentTimeMillis();
				return;
			}
		}
		if (num2 > 0 && num2 > Char.myCharz().cMPGoc)
		{
			if (num2 > 0 && Char.myCharz().cMPGoc <= num2 - 2000 && Char.myCharz().cTiemNang > 100 * (2 * (Char.myCharz().cMPGoc + 1000) + 1980) / 2)
			{
				Service.gI().upPotential(1, 100);
				TIME_DELAY_POINT = mSystem.currentTimeMillis();
				return;
			}
			if (num2 > 0 && Char.myCharz().cMPGoc <= num2 - 200 && Char.myCharz().cTiemNang > 10 * (2 * (Char.myCharz().cMPGoc + 1000) + 180) / 2)
			{
				Service.gI().upPotential(1, 10);
				TIME_DELAY_POINT = mSystem.currentTimeMillis();
				return;
			}
			if (num2 > 0 && Char.myCharz().cMPGoc <= num2 - 20 && Char.myCharz().cTiemNang > Char.myCharz().cMPGoc + 1000)
			{
				Service.gI().upPotential(1, 1);
				TIME_DELAY_POINT = mSystem.currentTimeMillis();
				return;
			}
		}
		if (num3 > 0 && num3 > Char.myCharz().cDamGoc)
		{
			if (num3 > 0 && Char.myCharz().cDamGoc <= num3 - 100 && 100 * (2 * Char.myCharz().cDamGoc + 99) / 2 * Char.myCharz().expForOneAdd < Char.myCharz().cTiemNang)
			{
				Service.gI().upPotential(2, 100);
				TIME_DELAY_POINT = mSystem.currentTimeMillis();
			}
			else if (num3 > 0 && Char.myCharz().cDamGoc <= num3 - 10 && 10 * (2 * Char.myCharz().cDamGoc + 9) / 2 * Char.myCharz().expForOneAdd < Char.myCharz().cTiemNang)
			{
				Service.gI().upPotential(2, 10);
				TIME_DELAY_POINT = mSystem.currentTimeMillis();
			}
			else if (num3 > 0 && Char.myCharz().cDamGoc <= num3 - 1 && Char.myCharz().cDamGoc * 100 < Char.myCharz().cTiemNang)
			{
				Service.gI().upPotential(2, 1);
				TIME_DELAY_POINT = mSystem.currentTimeMillis();
			}
		}
	}

	public static void AutoUseBeanForPet()
	{
		if (PET_PERCENT_HP != 0 && mSystem.currentTimeMillis() - TIME_DELAY_USE_BEAN > 5000 && Char.myPetz().cHP <= Char.myPetz().cHPFull * PET_PERCENT_HP / 100)
		{
			GameScr.gI().doUseHP();
			TIME_DELAY_USE_BEAN = mSystem.currentTimeMillis();
		}
	}

	public static void AutoRevive()
	{
		if (Char.myCharz().luong + Char.myCharz().luongKhoa > 0 && Char.myCharz().meDead && Char.myCharz().cHP <= 0 && GameCanvas.gameTick % 20 == 0 && enableAutoRevive)
		{
			Service.gI().wakeUpFromDead();
			Char.myCharz().meDead = false;
			Char.myCharz().statusMe = 1;
			Char.myCharz().cHP = Char.myCharz().cHPFull;
			Char.myCharz().cMP = Char.myCharz().cMPFull;
			Char @char = Char.myCharz();
			Char char2 = Char.myCharz();
			Char.myCharz().cp3 = 0;
			char2.cp2 = 0;
			@char.cp1 = 0;
			ServerEffect.addServerEffect(109, Char.myCharz(), 2);
			GameScr.gI().center = null;
			GameScr.isHaveSelectSkill = true;
		}
	}

	public static void SetPetGoHome()
	{
		if (GameCanvas.gameTick % 10 == 0 && enableSetPetGoHome && !Char.myCharz().isNhapThe && Char.myPetz().petStatus != 3 && Char.myPetz().petStatus != 3)
		{
			Service.gI().petStatus(3);
		}
	}

	public static void CountPetPower()
	{
		if (PET_LIMIT_POWER != 0 && Char.myPetz().cPower >= PET_LIMIT_POWER)
		{
			Application.Quit();
		}
	}
}

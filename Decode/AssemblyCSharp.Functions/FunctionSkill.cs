using System;
using System.Collections.Generic;

namespace AssemblyCSharp.Functions;

public class FunctionSkill : IActionListener
{
	public struct SkillCoolDown
	{
		public int skillID;

		public int coolDown;

		public SkillCoolDown(int skillID, int coolDown)
		{
			this.skillID = skillID;
			this.coolDown = coolDown;
		}
	}

	private static FunctionSkill _Instance;

	public static MyVector ListSkill = new MyVector();

	public static long TIME_DELAY_CHANGE_SKILL;

	public static bool enableAutoSkill;

	public static long TIME_DELAY_SEND_ATTACK;

	public static long TIME_WAIT_SEND_ATTACK;

	public long[] currTimeAK = new long[8];

	public static bool enableSendAttack;

	public static bool enableRecoveringSkill;

	public static bool isRegister;

	public static bool SkillIsRecoverd;

	public static List<SkillCoolDown> ListSkillCoolDown = new List<SkillCoolDown>();

	public static FunctionSkill gI()
	{
		if (_Instance == null)
		{
			_Instance = new FunctionSkill();
		}
		return _Instance;
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 10004:
			ShowMenuSkills();
			break;
		case 50001:
		{
			Skill skill = (Skill)p;
			if (ListSkill.contains(skill))
			{
				ListSkill.removeElement(skill);
				GameScr.info1.addInfo("Đã xóa " + skill.template.name, 0);
			}
			else
			{
				ListSkill.addElement(skill);
				GameScr.info1.addInfo("Đã thêm " + skill.template.name, 0);
			}
			break;
		}
		}
	}

	public static bool HotKey(int keyAscii)
	{
		switch (keyAscii)
		{
		case 97:
			enableSendAttack = !enableSendAttack;
			GameScr.info1.addInfo("[ThanhLc] Tự đánh: " + StringHandle.Status(enableSendAttack), 0);
			break;
		case 111:
			FakeSkillCoolDown();
			break;
		default:
			return false;
		}
		return true;
	}

	public static bool chat(string text)
	{
		if (text == "/ak")
		{
			enableAutoSkill = false;
			TIME_WAIT_SEND_ATTACK = 0L;
			GameScr.info1.addInfo("[ThanhLc] Tự dùng skill sau: " + TIME_WAIT_SEND_ATTACK + " " + StringHandle.Status(enableAutoSkill), 0);
		}
		else if (StringHandle.IsGetInfoChat<long>(text, "/ak"))
		{
			if (StringHandle.GetInfoChat<long>(text, "/ak") == 0)
			{
				enableAutoSkill = false;
				TIME_WAIT_SEND_ATTACK = 0L;
				GameScr.info1.addInfo("[ThanhLc] Tự dùng skill sau: " + TIME_WAIT_SEND_ATTACK + " " + StringHandle.Status(enableAutoSkill), 0);
			}
			else
			{
				TIME_WAIT_SEND_ATTACK = StringHandle.GetInfoChat<long>(text, "/ak");
				enableAutoSkill = true;
				GameScr.info1.addInfo("[ThanhLc] Tự dùng skill sau: " + TIME_WAIT_SEND_ATTACK + " " + StringHandle.Status(enableAutoSkill), 0);
			}
		}
		else if (StringHandle.IsGetInfoChat<int>(text, "/frz"))
		{
			if (StringHandle.GetInfoChat<int>(text, "/frz") == 0)
			{
				Char.myCharz().myskill.coolDown = 0;
				GameScr.info1.addInfo("[ThanhLc] Đóng băng: " + Char.myCharz().myskill.template.name, 0);
			}
			else
			{
				Char.myCharz().myskill.coolDown = StringHandle.GetInfoChat<int>(text, "/frz");
				GameScr.info1.addInfo("[ThanhLc] Fake time hồi chiều: " + Char.myCharz().myskill.template.name + " :" + StringHandle.GetInfoChat<int>(text, "/frz"), 0);
			}
		}
		else
		{
			if (!(text == "/hskill"))
			{
				return false;
			}
			enableRecoveringSkill = !enableRecoveringSkill;
			isRegister = false;
			SkillIsRecoverd = false;
			GameScr.info1.addInfo("[ThanhLc] Đi hồi skill: " + StringHandle.Status(enableRecoveringSkill), 0);
		}
		return true;
	}

	public static void Update()
	{
		try
		{
			AutoSkillNotFocus();
		}
		catch (Exception ex)
		{
			FunctionMain.WriteError("Data/Errors/AutoSkillNotFocus.txt", ex.Message);
		}
		try
		{
			AutoSkill();
		}
		catch (Exception ex2)
		{
			FunctionMain.WriteError("Data/Errors/AutoSkill.txt", ex2.Message);
		}
		try
		{
			if (enableSendAttack)
			{
				gI().AutoSendAttack();
			}
		}
		catch (Exception ex3)
		{
			FunctionMain.WriteError("Data/Errors/AutoSendAttack.txt", ex3.Message);
		}
		try
		{
			RecoveringSkill();
		}
		catch (Exception ex4)
		{
			FunctionMain.WriteError("Data/Errors/RecoveringSkill.txt", ex4.Message);
		}
	}

	public void ShowMenuSkills()
	{
		MyVector myVector = new MyVector();
		Skill[] keySkill = GameScr.keySkill;
		foreach (Skill skill in keySkill)
		{
			if (skill != null && (skill.template.type == 3 || skill.template.type == 2))
			{
				myVector.addElement(new Command(skill.template.name + "\n" + (ListSkill.contains(skill) ? "Đang Bật" : "Đang Tắt"), this, 50001, skill));
			}
		}
		GameCanvas.menu.startAt(myVector, 0);
	}

	public static void AutoSkillNotFocus()
	{
		if (mSystem.currentTimeMillis() - TIME_DELAY_CHANGE_SKILL < 1000 || Char.myCharz().isWaitMonkey || GameScr.isChangeZone || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5 || Char.myCharz().isCharge || Char.myCharz().isFlyAndCharge || Char.myCharz().isUseChargeSkill())
		{
			return;
		}
		Skill[] array = (GameCanvas.isTouch ? GameScr.onScreenSkill : GameScr.keySkill);
		foreach (Skill skill in array)
		{
			if (skill != null && ListSkill.contains(skill) && mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill > skill.coolDown + 100 && skill.template.type != 2)
			{
				int num = 0;
				bool flag = Char.myCharz().charEffectTime.isTDHS && Char.myCharz().charEffectTime.timeTDHS > 1;
				bool flag2 = Char.myCharz().charEffectTime.isTeleported && Char.myCharz().charEffectTime.timeTeleported > 1;
				bool isTied = Char.myCharz().charEffectTime.isTied;
				bool isStone = Char.myCharz().charEffectTime.isStone;
				bool isHypnotized = Char.myCharz().charEffectTime.isHypnotized;
				if ((flag && skill.template.id != 19 && skill.template.id != 6 && skill.template.id != 8 && skill.template.id != 13 && skill.template.id != 14 && skill.template.id != 21) || flag2 || isTied || isStone || isHypnotized)
				{
					break;
				}
				num = ((skill.template.manaUseType == 2) ? 1 : ((skill.template.manaUseType == 1) ? (skill.manaUse * Char.myCharz().cMPFull / 100) : skill.manaUse));
				if (Char.myCharz().cMP >= num)
				{
					if (skill != Char.myCharz().myskill)
					{
						GameScr.gI().doSelectSkill(skill, isShortcut: true);
						TIME_DELAY_CHANGE_SKILL = mSystem.currentTimeMillis();
					}
					else
					{
						GameScr.gI().doUseSkillNotFocus(skill);
						skill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
					}
					break;
				}
			}
			else
			{
				if (skill == null || !ListSkill.contains(skill) || mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill <= skill.coolDown + 100 || skill.template.type != 2)
				{
					continue;
				}
				int num2 = 0;
				num2 = ((skill.template.manaUseType == 2) ? 1 : ((skill.template.manaUseType == 1) ? (skill.manaUse * Char.myCharz().cMPFull / 100) : skill.manaUse));
				if (Char.myCharz().cMP >= num2)
				{
					if (skill != Char.myCharz().myskill)
					{
						GameScr.gI().doSelectSkill(skill, isShortcut: true);
						TIME_DELAY_CHANGE_SKILL = mSystem.currentTimeMillis();
						break;
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

	public static void AutoSkill()
	{
		if (enableAutoSkill && mSystem.currentTimeMillis() - TIME_DELAY_SEND_ATTACK > TIME_WAIT_SEND_ATTACK)
		{
			if (Char.myCharz().myskill.template.type == 3)
			{
				GameScr.gI().doUseSkillNotFocus(Char.myCharz().myskill);
				TIME_DELAY_SEND_ATTACK = mSystem.currentTimeMillis();
			}
			else if (Char.myCharz().mobFocus != null)
			{
				MyVector myVector = new MyVector();
				myVector.addElement(Char.myCharz().mobFocus);
				Service.gI().sendPlayerAttack(myVector, new MyVector(), -1);
				TIME_DELAY_SEND_ATTACK = mSystem.currentTimeMillis();
			}
			else if (Char.myCharz().charFocus != null)
			{
				MyVector myVector2 = new MyVector();
				myVector2.addElement(Char.myCharz().charFocus);
				Service.gI().sendPlayerAttack(new MyVector(), myVector2, -1);
				TIME_DELAY_SEND_ATTACK = mSystem.currentTimeMillis();
			}
		}
	}

	public int getSkill()
	{
		for (int i = 0; i < GameScr.keySkill.Length; i++)
		{
			if (GameScr.keySkill[i] == Char.myCharz().myskill)
			{
				return i;
			}
		}
		return 0;
	}

	public long getTimeSkill(Skill skill)
	{
		if (skill.template.id != 20 && skill.template.id != 22 && skill.template.id != 7 && skill.template.id != 18 && skill.template.id != 23)
		{
			return skill.coolDown + 100;
		}
		return (long)skill.coolDown + 500L;
	}

	public void AutoSendAttack()
	{
		Char @char = Char.myCharz();
		Char charFocus = @char.charFocus;
		Mob mobFocus = @char.mobFocus;
		if (@char.meDead || @char.statusMe == 14 || @char.statusMe == 5 || @char.myskill.template.type == 3 || @char.myskill.template.id == 10 || @char.myskill.template.id == 11 || @char.myskill.paintCanNotUseSkill)
		{
			return;
		}
		int skill = getSkill();
		sbyte id = @char.myskill.template.id;
		sbyte b = id;
		if (b == 7)
		{
			if (mSystem.currentTimeMillis() - currTimeAK[skill] > getTimeSkill(@char.myskill) && charFocus != null)
			{
				@char.myskill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
				try
				{
					MyVector myVector = new MyVector();
					myVector.addElement(charFocus);
					Service.gI().sendPlayerAttack(new MyVector(), myVector, 2);
				}
				catch (Exception)
				{
				}
				currTimeAK[skill] = mSystem.currentTimeMillis();
			}
		}
		else
		{
			if (mSystem.currentTimeMillis() - currTimeAK[skill] <= getTimeSkill(@char.myskill) + (FunctionTrainMob.IsTanSat ? 10 : 40) || @char.myskill.template.id == 23)
			{
				return;
			}
			if (mobFocus != null && GameScr.gI().isMeCanAttackMob(mobFocus))
			{
				Char.myCharz().myskill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
				try
				{
					MyVector myVector2 = new MyVector();
					myVector2.addElement(mobFocus);
					Service.gI().sendPlayerAttack(myVector2, new MyVector(), 1);
				}
				catch (Exception)
				{
				}
				currTimeAK[skill] = mSystem.currentTimeMillis();
			}
			else if (charFocus != null && Char.myCharz().isMeCanAttackOtherPlayer(charFocus))
			{
				Char.myCharz().myskill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
				try
				{
					MyVector myVector3 = new MyVector();
					myVector3.addElement(charFocus);
					Service.gI().sendPlayerAttack(new MyVector(), myVector3, 2);
				}
				catch (Exception)
				{
				}
				currTimeAK[skill] = mSystem.currentTimeMillis();
			}
		}
	}

	public static void RecoveringSkill()
	{
		if (!enableRecoveringSkill || SkillIsRecoverd)
		{
			return;
		}
		if (TileMap.mapID != 129)
		{
			XmapController.StartRunToMapId(129);
		}
		else if (Char.myCharz().cy == 360 && !isRegister)
		{
			if (!GameCanvas.menu.showMenu)
			{
				Service.gI().openMenu(23);
			}
			else if (GameCanvas.menu.showMenu)
			{
				Service.gI().confirmMenu(23, 2);
				isRegister = true;
			}
		}
		else if (Char.myCharz().cy == 264)
		{
			SkillIsRecoverd = true;
			enableRecoveringSkill = false;
		}
	}

	public static void FakeSkillCoolDown()
	{
		if (Char.myCharz().myskill.coolDown != 0)
		{
			if (!ListSkillCoolDown.Contains(new SkillCoolDown(Char.myCharz().myskill.skillId, Char.myCharz().myskill.coolDown)))
			{
				ListSkillCoolDown.Add(new SkillCoolDown(Char.myCharz().myskill.skillId, Char.myCharz().myskill.coolDown));
			}
			else
			{
				chat("/frz0");
			}
			return;
		}
		foreach (SkillCoolDown item in ListSkillCoolDown)
		{
			if (item.skillID == Char.myCharz().myskill.skillId)
			{
				Char.myCharz().myskill.coolDown = item.coolDown;
				ListSkillCoolDown.Remove(item);
				GameScr.info1.addInfo("Skill đã trở về trạng thái bình thường", 0);
			}
		}
	}
}

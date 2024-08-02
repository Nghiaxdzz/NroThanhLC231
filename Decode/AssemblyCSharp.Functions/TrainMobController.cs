using System.Collections.Generic;
using System.Linq;

namespace AssemblyCSharp.Functions;

public class TrainMobController
{
	private enum TpyePickItem
	{
		CanNotPickItem,
		PickItemNormal,
		PickItemTDLT,
		PickItemTanSat
	}

	private const int TIME_REPICKITEM = 500;

	private const int TIME_DELAY_TANSAT = 500;

	private const int ID_ICON_ITEM_TDLT = 4387;

	private static readonly sbyte[] IdSkillsMelee = new sbyte[5] { 0, 9, 2, 17, 4 };

	private static readonly sbyte[] IdSkillsCanNotAttack = new sbyte[5] { 10, 11, 14, 23, 7 };

	private static readonly TrainMobController _Instance = new TrainMobController();

	public static bool IsPickingItems;

	private static bool IsWait;

	private static long TimeStartWait;

	private static long TimeWait;

	public static List<ItemMap> ItemPicks = new List<ItemMap>();

	private static int IndexItemPick = 0;

	public static long LastTimeReturn;

	public static void Update()
	{
		if (IsWaiting())
		{
			return;
		}
		Char @char = Char.myCharz();
		if (@char.statusMe == 14 || @char.cHP <= 0)
		{
			return;
		}
		if ((@char.cHP <= @char.cHPFull * FunctionTrainMob.HpBuff / 100 && FunctionTrainMob.HpBuff != 0) || (@char.cMP <= @char.cMPFull * FunctionTrainMob.MpBuff / 100 && FunctionTrainMob.MpBuff != 0))
		{
			GameScr.gI().doUseHP();
		}
		bool flag = ItemTime.isExistItem(4387);
		bool flag2 = FunctionTrainMob.IsTanSat && flag;
		if (FunctionTrainMob.IsAutoPickItems && !flag2 && !FunctionMap.isMeInNRDMap())
		{
			if (TileMap.mapID == Char.myCharz().cgender + 21 && GameScr.vItemMap.size() > 0)
			{
				Service.gI().pickItem(-1);
				return;
			}
			if (FunctionTrainMob.IsItemMe && FunctionTrainMob.TimesAutoPickItemMax != 2)
			{
				FunctionTrainMob.TimesAutoPickItemMax = 2;
				return;
			}
			if (!FunctionTrainMob.IsItemMe && FunctionTrainMob.TimesAutoPickItemMax != 100)
			{
				FunctionTrainMob.TimesAutoPickItemMax = 100;
				return;
			}
			if (IsPickingItems)
			{
				if (IndexItemPick >= ItemPicks.Count)
				{
					IsPickingItems = false;
					return;
				}
				ItemMap itemMap = ItemPicks[IndexItemPick];
				switch (GetTpyePickItem(itemMap))
				{
				case TpyePickItem.PickItemTDLT:
					@char.cx = itemMap.xEnd;
					@char.cy = itemMap.yEnd;
					Service.gI().charMove();
					Service.gI().pickItem(itemMap.itemMapID);
					itemMap.countAutoPick++;
					IndexItemPick++;
					Wait(500);
					return;
				case TpyePickItem.PickItemTanSat:
					Move(itemMap.xEnd, itemMap.yEnd);
					@char.mobFocus = null;
					Wait(500);
					return;
				case TpyePickItem.PickItemNormal:
					Service.gI().charMove();
					Service.gI().pickItem(itemMap.itemMapID);
					itemMap.countAutoPick++;
					Wait(500);
					IndexItemPick++;
					return;
				case TpyePickItem.CanNotPickItem:
					IndexItemPick++;
					return;
				}
			}
			ItemPicks.Clear();
			IndexItemPick = 0;
			for (int i = 0; i < GameScr.vItemMap.size(); i++)
			{
				ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(i);
				if (GetTpyePickItem(itemMap2) != 0)
				{
					ItemPicks.Add(itemMap2);
				}
			}
			if (ItemPicks.Count > 0)
			{
				IsPickingItems = true;
				return;
			}
		}
		if (!FunctionTrainMob.IsTanSat)
		{
			return;
		}
		if (@char.isCharge)
		{
			Wait(500);
			return;
		}
		@char.clearFocus(0);
		if (@char.mobFocus != null && !IsMobTanSat(@char.mobFocus))
		{
			@char.mobFocus = null;
		}
		if (@char.mobFocus == null)
		{
			@char.mobFocus = GetMobTanSat();
			if (flag && @char.mobFocus != null)
			{
				Char.myCharz().cx = @char.mobFocus.xFirst - 24;
				Char.myCharz().cy = @char.mobFocus.yFirst;
				Service.gI().charMove();
			}
		}
		if (@char.mobFocus != null)
		{
			if (@char.skillInfoPaint() == null)
			{
				Skill skillAttack = GetSkillAttack();
				if (skillAttack != null && !skillAttack.paintCanNotUseSkill)
				{
					Mob mobFocus = @char.mobFocus;
					mobFocus.x = mobFocus.xFirst;
					mobFocus.y = mobFocus.yFirst;
					bool enableTrainingSendAttack = FunctionTrainMob.enableTrainingSendAttack;
					bool isOneHitMob = FunctionTrainMob.IsOneHitMob;
					if (Char.myCharz().myskill != skillAttack || Char.myCharz().statusMe == 5 || Char.myCharz().myskill.template.type == 3 || Char.myCharz().myskill.template.id == 10 || Char.myCharz().myskill.template.id == 11 || Char.myCharz().myskill.template.id == 20)
					{
						GameScr.gI().doSelectSkill(skillAttack, isShortcut: true);
					}
					if (Res.distance(mobFocus.xFirst, mobFocus.yFirst, @char.cx, @char.cy) <= 48)
					{
						GameScr.gI().doSelectSkill(skillAttack, isShortcut: true);
						if (Res.distance(mobFocus.xFirst, mobFocus.yFirst, @char.cx, @char.cy) <= 48)
						{
							GameScr.gI().doDoubleClickToObj(mobFocus);
						}
					}
					if (Res.distance(mobFocus.xFirst, mobFocus.yFirst, @char.cx, @char.cy) > 48 && Char.myCharz().myskill.template.id != 20)
					{
						if (!flag)
						{
							Move(mobFocus.xFirst, mobFocus.yFirst);
						}
						else if (mSystem.currentTimeMillis() - LastTimeReturn > 500)
						{
							Char.myCharz().cx = mobFocus.xFirst - 24;
							Char.myCharz().cy = mobFocus.yFirst;
							Service.gI().charMove();
							LastTimeReturn = mSystem.currentTimeMillis();
						}
					}
				}
			}
		}
		else if (!flag)
		{
			Mob mobNext = GetMobNext();
			if (mobNext != null)
			{
				Move(mobNext.xFirst - 24, mobNext.yFirst);
			}
		}
		Wait(300);
	}

	public static void Move(int x, int y)
	{
		Char @char = Char.myCharz();
		if (!FunctionTrainMob.IsVuotDiaHinh)
		{
			@char.currentMovePoint = new MovePoint(x, y);
			return;
		}
		int[] pointYsdMax = GetPointYsdMax(@char.cx, x);
		if (pointYsdMax[1] >= y || (pointYsdMax[1] >= @char.cy && (@char.statusMe == 2 || @char.statusMe == 1)))
		{
			pointYsdMax[0] = x;
			pointYsdMax[1] = y;
		}
		@char.currentMovePoint = new MovePoint(pointYsdMax[0], pointYsdMax[1]);
	}

	private static TpyePickItem GetTpyePickItem(ItemMap itemMap)
	{
		Char @char = Char.myCharz();
		bool flag = itemMap.playerId == @char.charID || itemMap.playerId == -1;
		if (FunctionTrainMob.IsItemMe && !flag)
		{
			return TpyePickItem.CanNotPickItem;
		}
		if (FunctionTrainMob.IsLimitTimesPickItem && itemMap.countAutoPick > FunctionTrainMob.TimesAutoPickItemMax)
		{
			return TpyePickItem.CanNotPickItem;
		}
		if (!FilterItemPick(itemMap))
		{
			return TpyePickItem.CanNotPickItem;
		}
		if (Res.abs(@char.cx - itemMap.xEnd) < 60 && Res.abs(@char.cy - itemMap.yEnd) < 60)
		{
			return TpyePickItem.PickItemNormal;
		}
		if (ItemTime.isExistItem(4387))
		{
			return TpyePickItem.PickItemTDLT;
		}
		if (FunctionTrainMob.IsTanSat)
		{
			return TpyePickItem.PickItemTanSat;
		}
		return TpyePickItem.CanNotPickItem;
	}

	private static bool FilterItemPick(ItemMap itemMap)
	{
		if (FunctionTrainMob.IdItemPicks.Count != 0 && !FunctionTrainMob.IdItemPicks.Contains(itemMap.template.id))
		{
			return false;
		}
		if (FunctionTrainMob.IdItemBlocks.Count != 0 && FunctionTrainMob.IdItemBlocks.Contains(itemMap.template.id))
		{
			return false;
		}
		if (FunctionTrainMob.TypeItemPicks.Count != 0 && !FunctionTrainMob.TypeItemPicks.Contains(itemMap.template.type))
		{
			return false;
		}
		if (FunctionTrainMob.TypeItemBlock.Count != 0 && FunctionTrainMob.TypeItemBlock.Contains(itemMap.template.type))
		{
			return false;
		}
		return true;
	}

	private static Mob GetMobTanSat()
	{
		Mob result = null;
		int num = int.MaxValue;
		Char @char = Char.myCharz();
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			int num2 = (mob.xFirst - @char.cx) * (mob.xFirst - @char.cx) + (mob.yFirst - @char.cy) * (mob.yFirst - @char.cy);
			if (FunctionTrainMob.IsOneHitMob)
			{
				if (IsMobTanSat(mob) && num2 < num && FunctionTrainMob.MobLimitHP == 0)
				{
					result = mob;
					num = num2;
				}
				else if (IsMobTanSat(mob) && num2 < num && FunctionTrainMob.MobLimitHP != 0 && FunctionTrainMob.MobLimitHP >= mob.hp)
				{
					result = mob;
					num = num2;
				}
			}
			else if (IsMobTanSat(mob) && num2 < num && FunctionTrainMob.MobLimitHP == 0)
			{
				result = mob;
				num = num2;
			}
			else if (IsMobTanSat(mob) && num2 < num && FunctionTrainMob.MobLimitHP != 0 && FunctionTrainMob.MobLimitHP >= mob.hp)
			{
				result = mob;
				num = num2;
			}
		}
		return result;
	}

	private static Mob GetMobNext()
	{
		Mob result = null;
		long num = mSystem.currentTimeMillis();
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (IsMobNext(mob) && mob.timeLastDie < num)
			{
				result = mob;
				num = mob.timeLastDie;
			}
		}
		return result;
	}

	private static bool IsMobTanSat(Mob mob)
	{
		if (mob.status == 0 || mob.status == 1 || mob.hp <= 0 || mob.isMobMe)
		{
			return false;
		}
		bool flag = FunctionTrainMob.IsNeSieuQuai && !ItemTime.isExistItem(4387);
		if (mob.levelBoss != 0 && flag)
		{
			return false;
		}
		if (!FilterMobTanSat(mob))
		{
			return false;
		}
		if (FunctionTrainMob.MobLimitHP != 0 && FunctionTrainMob.MobLimitHP < mob.hp)
		{
			return false;
		}
		return true;
	}

	private static bool IsMobNext(Mob mob)
	{
		if (mob.isMobMe)
		{
			return false;
		}
		if (!FilterMobTanSat(mob))
		{
			return false;
		}
		if (FunctionTrainMob.IsNeSieuQuai && !ItemTime.isExistItem(4387) && mob.getTemplate().hp >= 3000)
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

	private static bool FilterMobTanSat(Mob mob)
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

	public static Skill GetSkillAttack()
	{
		Skill skill = null;
		SkillTemplate skillTemplate = new SkillTemplate();
		foreach (sbyte item in FunctionTrainMob.IdSkillsTanSat)
		{
			skillTemplate.id = item;
			Skill skill2 = Char.myCharz().getSkill(skillTemplate);
			if (IsSkillBetter(skill2, skill))
			{
				skill = skill2;
			}
		}
		return skill;
	}

	private static bool IsSkillBetter(Skill SkillBetter, Skill skill)
	{
		if (SkillBetter == null)
		{
			return false;
		}
		if (SkillBetter.paintCanNotUseSkill)
		{
			return false;
		}
		if (!CanUseSkill(SkillBetter))
		{
			return false;
		}
		bool flag = SkillBetter.template.id == 17 && skill.template.id == 2;
		if (skill != null && skill.coolDown >= SkillBetter.coolDown && !flag)
		{
			return false;
		}
		return true;
	}

	private static bool CanUseSkill(Skill skill)
	{
		if (mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill > skill.coolDown)
		{
			skill.paintCanNotUseSkill = false;
		}
		if (skill.paintCanNotUseSkill && !IdSkillsMelee.Contains(skill.template.id))
		{
			return false;
		}
		if (IdSkillsCanNotAttack.Contains(skill.template.id))
		{
			return false;
		}
		if (Char.myCharz().cMP < GetManaUseSkill(skill))
		{
			return false;
		}
		return true;
	}

	private static int GetManaUseSkill(Skill skill)
	{
		if (skill.template.manaUseType == 2)
		{
			return 1;
		}
		if (skill.template.manaUseType == 1)
		{
			return skill.manaUse * Char.myCharz().cMPFull / 100;
		}
		return skill.manaUse;
	}

	private static int GetYsd(int xsd)
	{
		Char @char = Char.myCharz();
		int num = TileMap.pxh;
		int result = -1;
		for (int i = 24; i < TileMap.pxh; i += 24)
		{
			if (TileMap.tileTypeAt(xsd, i, 2))
			{
				int num2 = Res.abs(i - @char.cy);
				if (num2 < num)
				{
					num = num2;
					result = i;
				}
			}
		}
		return result;
	}

	private static int[] GetPointYsdMax(int xStart, int xEnd)
	{
		int num = TileMap.pxh;
		int num2 = -1;
		if (xStart > xEnd)
		{
			for (int i = xEnd; i < xStart; i += 24)
			{
				int ysd = GetYsd(i);
				if (ysd < num)
				{
					num = ysd;
					num2 = i;
				}
			}
		}
		else
		{
			for (int num3 = xEnd; num3 > xStart; num3 -= 24)
			{
				int ysd2 = GetYsd(num3);
				if (ysd2 < num)
				{
					num = ysd2;
					num2 = num3;
				}
			}
		}
		return new int[2] { num2, num };
	}

	public static void Wait(int time)
	{
		IsWait = true;
		TimeStartWait = mSystem.currentTimeMillis();
		TimeWait = time;
	}

	public static bool IsWaiting()
	{
		if (IsWait && mSystem.currentTimeMillis() - TimeStartWait >= TimeWait)
		{
			IsWait = false;
		}
		return IsWait;
	}
}

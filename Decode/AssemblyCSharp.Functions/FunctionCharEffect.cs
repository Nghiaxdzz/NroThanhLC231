using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AssemblyCSharp.Functions;

public class FunctionCharEffect
{
	public static class CharEffect
	{
		public static List<Char> storedChars = new List<Char>();

		public static bool isNRDAdded;

		public static bool isTieAdded;

		public static bool isTDHSAdded;

		public static bool isMobMeAdded;

		public static bool isMonkeyAdded;

		private static short NRSDImageId;

		public static void updateMe()
		{
			CharEffectTime charEffectTime = Char.myCharz().charEffectTime;
			if (charEffectTime.hasMobMe)
			{
				if (!isMobMeAdded)
				{
					isMobMeAdded = true;
					Char.vItemTime.addElement(new ItemTime(722, CharExtensions.getTimeMobMe(Char.myCharz())));
				}
			}
			else if (isMobMeAdded)
			{
				isMobMeAdded = false;
				removeElement(new ItemTime(722, 0));
			}
			if (charEffectTime.isTied)
			{
				if (!isTieAdded)
				{
					isTieAdded = true;
					Char.vItemTime.addElement(new ItemTime(3779, 35, isEquivalence: true));
				}
			}
			else if (isTieAdded)
			{
				isTieAdded = false;
				removeElement(new ItemTime(3779, 0, isEquivalence: true));
			}
			if (charEffectTime.isTDHS)
			{
				if (!isTDHSAdded)
				{
					isTDHSAdded = true;
					Char.vItemTime.addElement(new ItemTime(717, Char.myCharz().freezSeconds));
				}
			}
			else if (isTDHSAdded)
			{
				isTDHSAdded = false;
				removeElement(new ItemTime(717, 0));
			}
			if (charEffectTime.hasMonkey)
			{
				if (!isMonkeyAdded)
				{
					isMonkeyAdded = true;
					Char.vItemTime.addElement(new ItemTime(718, CharExtensions.getTimeMonkey(Char.myCharz())));
				}
			}
			else if (isMonkeyAdded)
			{
				isMonkeyAdded = false;
				removeElement(new ItemTime(718, 0));
			}
			if (charEffectTime.hasNRD)
			{
				if (!isNRDAdded)
				{
					isNRDAdded = true;
					NRSDImageId = FunctionNRSD.getNRSDId();
					Char.vItemTime.addElement(new ItemTime(NRSDImageId, 300));
				}
			}
			else if (isNRDAdded)
			{
				isNRDAdded = false;
				removeElement(new ItemTime(NRSDImageId, 0));
			}
		}

		public static void removeElement(ItemTime item)
		{
			for (int i = 0; i < Char.vItemTime.size(); i++)
			{
				ItemTime itemTime = Char.vItemTime.elementAt(i) as ItemTime;
				if (itemTime.idIcon == item.idIcon && itemTime.isEquivalence == item.isEquivalence && itemTime.isInfinity == item.isInfinity)
				{
					Char.vItemTime.removeElementAt(i);
					break;
				}
			}
		}

		public static void Update()
		{
			updateMe();
			for (int num = storedChars.Count - 1; num >= 0; num--)
			{
				Char @char = storedChars.ElementAt(num);
				Char char2 = GameScr.findCharInMap(@char.charID);
				if (!@char.charEffectTime.HasAnyEffect())
				{
					storedChars.RemoveAt(num);
				}
				else if (char2 == null)
				{
					@char.charEffectTime.Update();
				}
				else
				{
					GameScr.findCharInMap(@char.charID).charEffectTime = @char.charEffectTime;
					storedChars[num] = char2;
				}
			}
		}

		public static bool isContains(int charId)
		{
			foreach (Char storedChar in storedChars)
			{
				if (storedChar.charID == charId)
				{
					return true;
				}
			}
			return false;
		}

		public static void paint(mGraphics g)
		{
			if (!FunctionChar.enablePaintFocusInformation)
			{
				return;
			}
			if (Char.myCharz().mobFocus != null)
			{
				List<string> list = new List<string>();
				int num = 62;
				Mob mobFocus = Char.myCharz().mobFocus;
				list.Add(mobFocus.getTemplate().name + " [" + NinjaUtil.getMoneys(mobFocus.hp) + "/" + NinjaUtil.getMoneys(mobFocus.maxHp) + "]");
				if (mobFocus.mobEffectTime.isTied)
				{
					list.Add("Đang bị trói " + mobFocus.mobEffectTime.timeTied + " giây");
				}
				if (mobFocus.mobEffectTime.isTDHS)
				{
					list.Add("Đang bị TDHS " + mobFocus.mobEffectTime.timeTDHS + " giây");
				}
				if (mobFocus.mobEffectTime.isTeleported)
				{
					list.Add("Đang bị DCTT " + mobFocus.mobEffectTime.timeTeleported + " giây");
				}
				if (mobFocus.mobEffectTime.isHypnotized)
				{
					list.Add("Đang bị Thôi miên " + mobFocus.mobEffectTime.timeHypnotized + " giây");
				}
				foreach (string item in list)
				{
					StringHandle.paint(mFont.tahoma_7b_yellow, g, item, GameCanvas.w / 2, num, mFont.CENTER, mFont.tahoma_7b_dark, "border", mGraphics.zoomLevel);
					num += 10;
				}
			}
			Char charFocus = Char.myCharz().charFocus;
			if (charFocus == null)
			{
				return;
			}
			List<string> list2 = new List<string>();
			int num2 = 62;
			list2.Add(charFocus.cName + " [" + NinjaUtil.getMoneys(charFocus.cHP) + "/" + NinjaUtil.getMoneys(charFocus.cHPFull) + "]");
			if (charFocus.charEffectTime != null && charFocus.charEffectTime.HasAnyEffect())
			{
				if (charFocus.charEffectTime.hasNRD)
				{
					list2.Add("NRD còn: " + charFocus.charEffectTime.timeHoldingNRD + " giây");
				}
				if (charFocus.charEffectTime.hasShield)
				{
					list2.Add("Khiên còn: " + charFocus.charEffectTime.timeShield + " giây");
				}
				if (charFocus.charEffectTime.hasMonkey)
				{
					list2.Add("Khỉ còn: " + charFocus.charEffectTime.timeMonkey + " giây");
				}
				if (charFocus.charEffectTime.hasHuytSao)
				{
					list2.Add("Huýt sáo còn: " + charFocus.charEffectTime.timeHuytSao + " giây");
				}
				if (charFocus.charEffectTime.hasMobMe)
				{
					list2.Add("Đẻ trứng còn: " + charFocus.charEffectTime.timeMobMe + " giây");
				}
				if (charFocus.charEffectTime.isHypnotized)
				{
					list2.Add((charFocus.charEffectTime.isHypnotizedByMe ? "Bị bạn thôi miên: " : "Bị thôi miên: ") + charFocus.charEffectTime.timeHypnotized + " giây");
				}
				if (charFocus.charEffectTime.isTeleported)
				{
					list2.Add("Bị DCTT: " + charFocus.charEffectTime.timeTeleported + " giây");
				}
				if (charFocus.charEffectTime.isTDHS)
				{
					list2.Add("Bị TDHS: " + charFocus.charEffectTime.timeTDHS + " giây");
				}
				if (charFocus.charEffectTime.isTied)
				{
					list2.Add((charFocus.charEffectTime.isTiedByMe ? "Bị bạn trói: " : "Bị trói: ") + charFocus.charEffectTime.timeTied + " giây");
				}
				if (charFocus.charEffectTime.isStone)
				{
					list2.Add("Bị hóa đá: " + charFocus.charEffectTime.timeStone + " giây");
				}
				if (charFocus.charEffectTime.isChocolate)
				{
					list2.Add("Bị biến Sôcôla: " + charFocus.charEffectTime.timeChocolate + " giây");
				}
			}
			foreach (string item2 in list2)
			{
				if (item2[0].ToString().Split(' ')[0] != Char.myCharz().cName)
				{
					StringHandle.paint(mFont.tahoma_7b_red, g, item2, GameCanvas.w / 2, num2, mFont.CENTER, mFont.tahoma_7b_dark, "border", mGraphics.zoomLevel);
				}
				else
				{
					StringHandle.paint(mFont.tahoma_7b_yellow, g, item2, GameCanvas.w / 2, num2, mFont.CENTER, mFont.tahoma_7b_dark, "border", mGraphics.zoomLevel);
				}
				num2 += 10;
			}
			List<string> list3 = new List<string>();
			int num3 = 32;
			if (Char.myCharz().charEffectTime != null && Char.myCharz().charEffectTime.HasAnyEffect())
			{
				if (Char.myCharz().charEffectTime.hasNRD)
				{
					list3.Add("NRD còn: " + Char.myCharz().charEffectTime.timeHoldingNRD + " giây");
				}
				if (Char.myCharz().charEffectTime.isTied)
				{
					for (int i = 0; i < GameScr.vCharInMap.size(); i++)
					{
						if (GameScr.vCharInMap.elementAt(i) is Char @char && @char != Char.myCharz() && @char.holder && @char.charHold == Char.myCharz())
						{
							list3.Add("Bị " + @char.cName + " trói: " + Char.myCharz().charEffectTime.timeTied + " giây");
						}
					}
				}
			}
			foreach (string item3 in list3)
			{
				StringHandle.paint(mFont.tahoma_7b_yellow, g, item3, GameCanvas.w / 2, num3, mFont.CENTER, mFont.tahoma_7b_dark, "border", mGraphics.zoomLevel);
				num3 += 10;
			}
		}

		public static void AddEffectCreatedByMe(Skill skill)
		{
			if (Char.myCharz().charFocus != null)
			{
				if (skill.template.id == 22)
				{
					Char.myCharz().charFocus.charEffectTime.isHypnotizedByMe = true;
				}
				if (skill.template.id == 23)
				{
					Char.myCharz().charFocus.charEffectTime.isTiedByMe = true;
				}
			}
		}
	}

	public static class CharExtensions
	{
		public static bool isBoss(Char @char)
		{
			bool flag = (bool)typeof(Char).GetField("isPet", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(@char);
			bool flag2 = (bool)typeof(Char).GetField("isMiniPet", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(@char);
			return !flag && !flag2 && @char.cName != "Trọng tài" && char.IsUpper(getNameWithoutClanTag(@char)[0]);
		}

		public static bool isPet(Char @char)
		{
			bool flag = (bool)typeof(Char).GetField("isPet", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(@char);
			bool flag2 = (bool)typeof(Char).GetField("isMiniPet", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(@char);
			return flag || flag2 || @char.cName.StartsWith("#") || @char.cName.StartsWith("$");
		}

		public static int getTimeHold(Char @char)
		{
			int result = 35;
			try
			{
				if (!@char.me)
				{
					result = 35;
					if (@char.charEffectTime.isTiedByMe && Char.myCharz().cgender == 2)
					{
						result = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point * 5;
					}
				}
				else if (Char.myCharz().cgender == 2)
				{
					result = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point * 5;
				}
			}
			catch
			{
				result = 35;
			}
			return result;
		}

		public static int getTimeMonkey(Char @char)
		{
			int result = 60;
			try
			{
				if (!@char.me)
				{
					switch (@char.head)
					{
					case 192:
						result = 60;
						break;
					case 195:
						result = 70;
						break;
					case 196:
						result = 80;
						break;
					case 199:
						result = 90;
						break;
					case 197:
						result = 100;
						break;
					case 200:
						result = 110;
						break;
					case 198:
						result = 120;
						break;
					case 193:
					case 194:
						break;
					}
				}
				else if (Char.myCharz().cgender == 2)
				{
					result = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[3]).point * 10 + 50;
				}
			}
			catch
			{
				result = 121;
			}
			return result;
		}

		public static int getTimeShield(Char @char)
		{
			try
			{
				if (!@char.me)
				{
					return 45;
				}
				return Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[7]).point * 5 + 10;
			}
			catch
			{
				return 45;
			}
		}

		public static int getTimeMobMe(Char @char)
		{
			int result = 60;
			try
			{
				if (!@char.me)
				{
					switch (@char.mobMe.templateId)
					{
					case 8:
						result = 60;
						break;
					case 11:
						result = 95;
						break;
					case 32:
						result = 130;
						break;
					case 25:
						result = 165;
						break;
					case 43:
						result = 200;
						break;
					case 49:
						result = 235;
						break;
					case 50:
						result = 270;
						break;
					}
				}
				else if (Char.myCharz().cgender == 1)
				{
					result = (Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]).point - 1) * 35 + 60;
				}
			}
			catch
			{
				result = 270;
			}
			return result;
		}

		public static int getTimeHypnotize(Char @char)
		{
			int result = 12;
			try
			{
				if (!@char.me)
				{
					result = 12;
					if (@char.charEffectTime.isHypnotizedByMe)
					{
						result = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point + 5;
					}
				}
				else if (Char.myCharz().cgender == 0)
				{
					result = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]).point + 5;
				}
			}
			catch
			{
				result = 12;
			}
			return result;
		}

		public static int getTimeStone(Char @char)
		{
			return 5;
		}

		public static int getTimeHuytSao(Char @char)
		{
			return 31;
		}

		public static int getTimeChocolate(Char @char)
		{
			return 31;
		}

		public static string getNameWithoutClanTag(Char @char)
		{
			return @char.cName.Remove(0, @char.cName.IndexOf(']') + 1).Replace(" ", "");
		}

		public static string getGender(Char @char)
		{
			if (@char.cgender == 0)
			{
				return "TĐ";
			}
			if (@char.cgender == 1)
			{
				return "NM";
			}
			if (@char.cgender == 2)
			{
				return "XD";
			}
			return "BĐ";
		}

		public static Color getFlagColor(Char @char)
		{
			return @char.cFlag switch
			{
				1 => Color.cyan, 
				2 => Color.red, 
				3 => new Color(0.56f, 0.19f, 0.77f), 
				4 => Color.yellow, 
				5 => Color.green, 
				6 => Color.magenta, 
				7 => new Color(1f, 0.5f, 0f), 
				8 => new Color(0.18f, 0.18f, 0.18f), 
				9 => Color.blue, 
				10 => Color.red, 
				11 => Color.blue, 
				12 => Color.white, 
				13 => Color.black, 
				_ => Color.clear, 
			};
		}

		public static int getSuicideRange(Char @char)
		{
			int result = 880;
			if (@char.me && @char.cgender == 2)
			{
				Skill skill = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]);
				if (skill == null)
				{
					return 0;
				}
				result = 340 * (skill.point - 1) / 3 + 200;
			}
			return result;
		}
	}

	public class CharEffectTime
	{
		public bool hasNRD;

		public int timeHoldingNRD;

		public long lastTimeHoldNRD;

		public bool isHypnotized;

		public int timeHypnotized;

		public long lastTimeHypnotized;

		public bool isHypnotizedByMe;

		public bool hasMonkey;

		public int timeMonkey;

		public long lastTimeMonkey;

		public bool hasHuytSao;

		public int timeHuytSao;

		public long lastTimeHuytSao;

		public bool hasShield;

		public int timeShield;

		public long lastTimeShield;

		public bool isTeleported;

		public int timeTeleported;

		public long lastTimeTeleported;

		public bool isTied;

		public int timeTied;

		public long lastTimeTied;

		public bool isTiedByMe;

		public bool hasMobMe;

		public int timeMobMe;

		public long lastTimeMobMe;

		public bool isTDHS;

		public int timeTDHS;

		public long lastTimeTDHS;

		public bool isStone;

		public int timeStone;

		public long lastTimeStoned;

		public bool isChocolate;

		public int timeChocolate;

		public long lastTimeChocolated;

		public void Update()
		{
			if (timeHoldingNRD > 0 && mSystem.currentTimeMillis() - lastTimeHoldNRD >= 1000)
			{
				timeHoldingNRD--;
				lastTimeHoldNRD = mSystem.currentTimeMillis();
			}
			if (timeHypnotized > 0 && mSystem.currentTimeMillis() - lastTimeHypnotized >= 1000)
			{
				timeHypnotized--;
				if (timeHypnotized == 0)
				{
					isHypnotizedByMe = false;
				}
				lastTimeHypnotized = mSystem.currentTimeMillis();
			}
			if (timeMonkey > 0 && mSystem.currentTimeMillis() - lastTimeMonkey >= 1000)
			{
				timeMonkey--;
				lastTimeMonkey = mSystem.currentTimeMillis();
			}
			if (timeHuytSao > 0 && mSystem.currentTimeMillis() - lastTimeHuytSao >= 1000)
			{
				timeHuytSao--;
				lastTimeHuytSao = mSystem.currentTimeMillis();
			}
			if (timeShield > 0 && mSystem.currentTimeMillis() - lastTimeShield >= 1000)
			{
				timeShield--;
				lastTimeShield = mSystem.currentTimeMillis();
			}
			if (timeTeleported > 0 && mSystem.currentTimeMillis() - lastTimeTeleported >= 1000)
			{
				timeTeleported--;
				lastTimeTeleported = mSystem.currentTimeMillis();
			}
			if (timeTied > 0 && mSystem.currentTimeMillis() - lastTimeTied >= 1000)
			{
				timeTied--;
				if (timeTied == 0)
				{
					isTiedByMe = false;
				}
				lastTimeTied = mSystem.currentTimeMillis();
			}
			if (timeMobMe > 0 && mSystem.currentTimeMillis() - lastTimeMobMe >= 1000)
			{
				timeMobMe--;
				lastTimeMobMe = mSystem.currentTimeMillis();
			}
			if (timeTDHS > 0 && mSystem.currentTimeMillis() - lastTimeTDHS >= 1000)
			{
				timeTDHS--;
				lastTimeTDHS = mSystem.currentTimeMillis();
			}
			if (timeStone > 0 && mSystem.currentTimeMillis() - lastTimeStoned >= 1000)
			{
				timeStone--;
				lastTimeStoned = mSystem.currentTimeMillis();
			}
			if (timeChocolate > 0 && mSystem.currentTimeMillis() - lastTimeChocolated >= 1000)
			{
				timeChocolate--;
				lastTimeChocolated = mSystem.currentTimeMillis();
			}
			if (timeHuytSao <= 0)
			{
				hasHuytSao = false;
			}
		}

		public bool HasAnyEffect()
		{
			return timeTeleported + timeTied + timeHoldingNRD + timeHuytSao + timeMobMe + timeMonkey + timeShield + timeHypnotized + timeTDHS + timeStone + timeChocolate > 0;
		}
	}
}

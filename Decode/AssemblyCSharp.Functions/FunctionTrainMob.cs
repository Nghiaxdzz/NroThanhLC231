using System.Collections.Generic;

namespace AssemblyCSharp.Functions;

internal class FunctionTrainMob : IActionListener
{
	private static FunctionTrainMob _Instance;

	public static bool enableTrainingSendAttack = true;

	public static bool IsBlockTDLT = true;

	public static bool IsOneHitMob;

	public static int MobLimitHP;

	private const int ID_ITEM_GEM = 77;

	private const int ID_ITEM_GEM_LOCK = 861;

	private const int DEFAULT_HP_BUFF = 20;

	private const int DEFAULT_MP_BUFF = 20;

	private static readonly sbyte[] IdSkillsBase = new sbyte[4] { 0, 2, 17, 4 };

	private static readonly short[] IdItemBlockBase = new short[11]
	{
		225, 353, 354, 355, 356, 357, 358, 359, 360, 362,
		726
	};

	public static bool IsTanSat = false;

	public static bool IsNeSieuQuai = true;

	public static bool IsVuotDiaHinh = true;

	public static List<int> IdMobsTanSat = new List<int>();

	public static List<int> TypeMobsTanSat = new List<int>();

	public static List<sbyte> IdSkillsTanSat = new List<sbyte>(IdSkillsBase);

	public static bool IsAutoPickItems = false;

	public static bool IsItemMe = false;

	public static bool IsLimitTimesPickItem = false;

	public static int TimesAutoPickItemMax = 100;

	public static List<short> IdItemPicks = new List<short>();

	public static List<short> IdItemBlocks = new List<short>(IdItemBlockBase);

	public static List<sbyte> TypeItemPicks = new List<sbyte>();

	public static List<sbyte> TypeItemBlock = new List<sbyte>();

	public static int HpBuff = 0;

	public static int MpBuff = 0;

	public static FunctionTrainMob gI()
	{
		if (_Instance == null)
		{
			_Instance = new FunctionTrainMob();
		}
		return _Instance;
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 10000:
		{
			MyVector myVector4 = new MyVector();
			myVector4.addElement(new Command("Danh sách \n loại quái\n tàn sát", this, 101101, null));
			myVector4.addElement(new Command("Danh sách \n Skill \n tàn sát", this, 101102, null));
			myVector4.addElement(new Command("Cài đặt\n auto nhặt\n vật phẩm", this, 101103, null));
			myVector4.addElement(new Command((TypeMobsTanSat.Count > 0) ? "XÓA \n Danh sách \n Quái" : "Danh sách\n Quái\n Đang trống", this, 101106, null));
			myVector4.addElement(new Command("Ngưng TDLT\n Của Game\n" + StringHandle.StatusMenu(IsBlockTDLT), this, 101107, null));
			GameCanvas.menu.startAt(myVector4, 0);
			break;
		}
		case 101101:
		{
			MyVector myVector = new MyVector();
			List<int> list = new List<int>();
			for (int i = 0; i < GameScr.vMob.size(); i++)
			{
				Mob mob = (Mob)GameScr.vMob.elementAt(i);
				if (!list.Contains(mob.getTemplate().mobTemplateId))
				{
					string text = ((!TypeMobsTanSat.Contains(mob.getTemplate().mobTemplateId)) ? "Chưa thêm" : "Đã thêm");
					list.Add(mob.getTemplate().mobTemplateId);
					myVector.addElement(new Command(mob.getTemplate().name + "\n" + text, this, 170202, mob));
				}
			}
			GameCanvas.menu.startAt(myVector, 0);
			break;
		}
		case 101102:
		{
			MyVector myVector2 = new MyVector();
			for (int j = 0; j < Char.myCharz().nClass.skillTemplates.Length; j++)
			{
				SkillTemplate skillTemplate2 = Char.myCharz().nClass.skillTemplates[j];
				if (skillTemplate2 != null)
				{
					myVector2.addElement(new Command(string.Concat(str2: (!IdSkillsTanSat.Contains(skillTemplate2.id)) ? "Chưa thêm" : "ĐÃ THÊM", str0: skillTemplate2.name, str1: "\n"), this, 170203, skillTemplate2));
				}
			}
			GameCanvas.menu.startAt(myVector2, 0);
			break;
		}
		case 170203:
		{
			SkillTemplate skillTemplate = (SkillTemplate)p;
			if (IdSkillsTanSat.Contains(skillTemplate.id))
			{
				IdSkillsTanSat.Remove(skillTemplate.id);
				GameScr.info1.addInfo("Đã xóa " + skillTemplate.name, 0);
			}
			else
			{
				IdSkillsTanSat.Add(skillTemplate.id);
				GameScr.info1.addInfo("Đã thêm " + skillTemplate.name, 0);
			}
			break;
		}
		case 170202:
		{
			Mob mob2 = (Mob)p;
			if (TypeMobsTanSat.Contains(mob2.getTemplate().mobTemplateId))
			{
				TypeMobsTanSat.Remove(mob2.getTemplate().mobTemplateId);
				GameScr.info1.addInfo("Đã xóa " + mob2.getTemplate().name, 0);
			}
			else
			{
				TypeMobsTanSat.Add(mob2.getTemplate().mobTemplateId);
				GameScr.info1.addInfo("Đã thêm " + mob2.getTemplate().name, 0);
			}
			break;
		}
		case 101103:
		{
			MyVector myVector3 = new MyVector();
			myVector3.addElement(new Command("Tự động \n nhặt item \n" + StringHandle.StatusMenu(IsAutoPickItems), this, 170204, null));
			myVector3.addElement(new Command("Chỉ nhặt item của mình\n" + StringHandle.StatusMenu(IsItemMe), this, 170205, null));
			GameCanvas.menu.startAt(myVector3, 0);
			break;
		}
		case 170204:
			Service.gI().chat("/anhat");
			break;
		case 170205:
			Service.gI().chat("/itm");
			break;
		case 101104:
			enableTrainingSendAttack = !enableTrainingSendAttack;
			GameScr.info1.addInfo("[ThanhLc] Chuyển qua chế độ tàn sát: " + (enableTrainingSendAttack ? "AK" : "Đấm thường"), 0);
			break;
		case 101105:
			FunctionMain.gI().OpenChat("Nhập giới hạn HP quái");
			break;
		case 101106:
			TypeMobsTanSat.Clear();
			break;
		case 101107:
			IsBlockTDLT = !IsBlockTDLT;
			GameScr.info1.addInfo("[ThanhLc]: Ngăn TDLT của game: " + StringHandle.Status(IsBlockTDLT), 0);
			break;
		case 101108:
			IsVuotDiaHinh = !IsVuotDiaHinh;
			GameScr.info1.addInfo("[ThanhLc]: Vượt địa hình: " + StringHandle.Status(IsVuotDiaHinh), 0);
			break;
		}
	}

	public static bool chat(string text)
	{
		switch (text)
		{
		case "/add":
		{
			Mob mobFocus2 = Char.myCharz().mobFocus;
			ItemMap itemFocus3 = Char.myCharz().itemFocus;
			if (mobFocus2 != null)
			{
				if (IdMobsTanSat.Contains(mobFocus2.mobId))
				{
					IdMobsTanSat.Remove(mobFocus2.mobId);
					GameScr.info1.addInfo("Đã xoá mob: " + mobFocus2.mobId, 0);
				}
				else
				{
					IdMobsTanSat.Add(mobFocus2.mobId);
					GameScr.info1.addInfo("Đã thêm mob: " + mobFocus2.mobId, 0);
				}
			}
			else if (itemFocus3 != null)
			{
				if (IdItemPicks.Contains(itemFocus3.template.id))
				{
					IdItemPicks.Remove(itemFocus3.template.id);
					GameScr.info1.addInfo($"Đã xoá khỏi danh sách chỉ tự động nhặt item: {itemFocus3.template.name}[{itemFocus3.template.id}]", 0);
				}
				else
				{
					IdItemPicks.Add(itemFocus3.template.id);
					GameScr.info1.addInfo($"Đã thêm vào danh sách chỉ tự động nhặt item: {itemFocus3.template.name}[{itemFocus3.template.id}]", 0);
				}
			}
			else
			{
				GameScr.info1.addInfo("Cần trỏ vào quái hay vật phẩm cần thêm vào danh sách", 0);
			}
			break;
		}
		case "/addt":
		{
			Mob mobFocus = Char.myCharz().mobFocus;
			ItemMap itemFocus2 = Char.myCharz().itemFocus;
			if (mobFocus != null)
			{
				if (TypeMobsTanSat.Contains(mobFocus.templateId))
				{
					TypeMobsTanSat.Remove(mobFocus.templateId);
					GameScr.info1.addInfo($"Đã xoá loại mob: {mobFocus.getTemplate().name}[{mobFocus.templateId}]", 0);
				}
				else
				{
					TypeMobsTanSat.Add(mobFocus.templateId);
					GameScr.info1.addInfo($"Đã thêm loại mob: {mobFocus.getTemplate().name}[{mobFocus.templateId}]", 0);
				}
			}
			else if (itemFocus2 != null)
			{
				if (TypeItemPicks.Contains(itemFocus2.template.type))
				{
					TypeItemPicks.Remove(itemFocus2.template.type);
					GameScr.info1.addInfo("Đã xoá khỏi danh sách chỉ tự động nhặt loại item:" + itemFocus2.template.type, 0);
				}
				else
				{
					TypeItemPicks.Add(itemFocus2.template.type);
					GameScr.info1.addInfo("Đã thêm vào danh sách chỉ tự động nhặt loại item:" + itemFocus2.template.type, 0);
				}
			}
			else
			{
				GameScr.info1.addInfo("Cần trỏ vào quái hay vật phẩm cần thêm vào danh sách", 0);
			}
			break;
		}
		case "/anhat":
			IsAutoPickItems = !IsAutoPickItems;
			GameScr.info1.addInfo("Tự động nhặt vật phẩm: " + (IsAutoPickItems ? "Bật" : "Tắt"), 0);
			break;
		case "/itm":
			IsItemMe = !IsItemMe;
			GameScr.info1.addInfo("Lọc không nhặt vật phẩm của người khác: " + (IsItemMe ? "Bật" : "Tắt"), 0);
			break;
		default:
			if (StringHandle.IsGetInfoChat<int>(text, "/sln"))
			{
				TimesAutoPickItemMax = StringHandle.GetInfoChat<int>(text, "/sln");
				GameScr.info1.addInfo("Số lần nhặt giới hạn là: " + TimesAutoPickItemMax, 0);
				break;
			}
			if (StringHandle.IsGetInfoChat<short>(text, "/addi"))
			{
				short infoChat = StringHandle.GetInfoChat<short>(text, "/addi");
				if (IdItemPicks.Contains(infoChat))
				{
					IdItemPicks.Remove(infoChat);
					GameScr.info1.addInfo($"Đã xoá khỏi danh sách chỉ tự động nhặt item: {ItemTemplates.get(infoChat).name}[{infoChat}]", 0);
				}
				else
				{
					IdItemPicks.Add(infoChat);
					GameScr.info1.addInfo($"Đã thêm vào danh sách chỉ tự động nhặt item: {ItemTemplates.get(infoChat).name}[{infoChat}]", 0);
				}
				break;
			}
			if (text == "/blocki")
			{
				ItemMap itemFocus = Char.myCharz().itemFocus;
				if (itemFocus != null)
				{
					if (IdItemBlocks.Contains(itemFocus.template.id))
					{
						IdItemBlocks.Remove(itemFocus.template.id);
						GameScr.info1.addInfo($"Đã xoá khỏi danh sách không tự động nhặt item: {itemFocus.template.name}[{itemFocus.template.id}]", 0);
					}
					else
					{
						IdItemBlocks.Add(itemFocus.template.id);
						GameScr.info1.addInfo($"Đã thêm vào danh sách không tự động nhặt item: {itemFocus.template.name}[{itemFocus.template.id}]", 0);
					}
				}
				else
				{
					GameScr.info1.addInfo("Cần trỏ vào vật phẩm cần chặn khi auto nhặt", 0);
				}
				break;
			}
			if (StringHandle.IsGetInfoChat<short>(text, "/blocki"))
			{
				short infoChat2 = StringHandle.GetInfoChat<short>(text, "/blocki");
				if (IdItemBlocks.Contains(infoChat2))
				{
					IdItemBlocks.Remove(infoChat2);
					GameScr.info1.addInfo($"Đã thêm vào danh sách không tự động nhặt item: {ItemTemplates.get(infoChat2).name}[{infoChat2}]", 0);
				}
				else
				{
					IdItemBlocks.Add(infoChat2);
					GameScr.info1.addInfo($"Đã xoá khỏi danh sách không tự động nhặt item: {ItemTemplates.get(infoChat2).name}[{infoChat2}]", 0);
				}
				break;
			}
			if (StringHandle.IsGetInfoChat<sbyte>(text, "/addti"))
			{
				sbyte infoChat3 = StringHandle.GetInfoChat<sbyte>(text, "/addti");
				if (TypeItemPicks.Contains(infoChat3))
				{
					TypeItemPicks.Remove(infoChat3);
					GameScr.info1.addInfo("Đã xoá khỏi danh sách chỉ tự động nhặt loại item: " + infoChat3, 0);
				}
				else
				{
					TypeItemPicks.Add(infoChat3);
					GameScr.info1.addInfo("Đã thêm vào danh sách chỉ tự động nhặt loại item: " + infoChat3, 0);
				}
				break;
			}
			if (StringHandle.IsGetInfoChat<sbyte>(text, "/blockti"))
			{
				sbyte infoChat4 = StringHandle.GetInfoChat<sbyte>(text, "/blockti");
				if (TypeItemBlock.Contains(infoChat4))
				{
					TypeItemBlock.Remove(infoChat4);
					GameScr.info1.addInfo("Đã xoá khỏi danh sách không tự động nhặt loại item: " + infoChat4, 0);
				}
				else
				{
					TypeItemBlock.Add(infoChat4);
					GameScr.info1.addInfo("Đã thêm vào danh sách không tự động nhặt loại item: " + infoChat4, 0);
				}
				break;
			}
			switch (text)
			{
			case "/pem1hit":
				IsOneHitMob = !IsOneHitMob;
				GameScr.info1.addInfo("[ThanhLc] Pem 1 hit: " + StringHandle.Status(IsOneHitMob), 0);
				break;
			case "/clri":
				IdItemPicks.Clear();
				TypeItemPicks.Clear();
				TypeItemBlock.Clear();
				IdItemBlocks.Clear();
				IdItemBlocks.AddRange(IdItemBlockBase);
				GameScr.info1.addInfo("Danh sách lọc item đã được đặt lại mặc định", 0);
				break;
			case "/cnn":
				IdItemPicks.Clear();
				TypeItemPicks.Clear();
				TypeItemBlock.Clear();
				IdItemBlocks.Clear();
				IdItemBlocks.AddRange(IdItemBlockBase);
				IdItemPicks.Add(77);
				IdItemPicks.Add(861);
				GameScr.info1.addInfo("Đã cài đặt chỉ nhặt ngọc", 0);
				break;
			case "/ts":
				IsTanSat = !IsTanSat;
				GameScr.info1.addInfo("Tự động đánh quái: " + (IsTanSat ? "Bật" : "Tắt"), 0);
				break;
			case "/nsq":
				IsNeSieuQuai = !IsNeSieuQuai;
				GameScr.info1.addInfo("Tàn sát né siêu quái: " + (IsNeSieuQuai ? "Bật" : "Tắt"), 0);
				break;
			default:
				if (StringHandle.IsGetInfoChat<int>(text, "/addm"))
				{
					int infoChat5 = StringHandle.GetInfoChat<int>(text, "/addm");
					if (IdMobsTanSat.Contains(infoChat5))
					{
						IdMobsTanSat.Remove(infoChat5);
						GameScr.info1.addInfo("Đã xoá mob: " + infoChat5, 0);
					}
					else
					{
						IdMobsTanSat.Add(infoChat5);
						GameScr.info1.addInfo("Đã thêm mob: " + infoChat5, 0);
					}
					text = string.Empty;
				}
				else if (StringHandle.IsGetInfoChat<int>(text, "/addtm"))
				{
					int infoChat6 = StringHandle.GetInfoChat<int>(text, "/addtm");
					if (TypeMobsTanSat.Contains(infoChat6))
					{
						TypeMobsTanSat.Remove(infoChat6);
						GameScr.info1.addInfo($"Đã xoá loại mob: {Mob.arrMobTemplate[infoChat6].name}[{infoChat6}]", 0);
					}
					else
					{
						TypeMobsTanSat.Add(infoChat6);
						GameScr.info1.addInfo($"Đã thêm loại mob: {Mob.arrMobTemplate[infoChat6].name}[{infoChat6}]", 0);
					}
				}
				else if (text == "/clrm")
				{
					IdMobsTanSat.Clear();
					TypeMobsTanSat.Clear();
					GameScr.info1.addInfo("Đã xoá danh sách đánh quái", 0);
				}
				else if (text == "/skill")
				{
					SkillTemplate template = Char.myCharz().myskill.template;
					if (IdSkillsTanSat.Contains(template.id))
					{
						IdSkillsTanSat.Remove(template.id);
						GameScr.info1.addInfo($"Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
					}
					else
					{
						IdSkillsTanSat.Add(template.id);
						GameScr.info1.addInfo($"Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
					}
				}
				else if (StringHandle.IsGetInfoChat<int>(text, "/skill"))
				{
					int num = StringHandle.GetInfoChat<int>(text, "/skill") - 1;
					SkillTemplate skillTemplate = Char.myCharz().nClass.skillTemplates[num];
					if (IdSkillsTanSat.Contains(skillTemplate.id))
					{
						IdSkillsTanSat.Remove(skillTemplate.id);
						GameScr.info1.addInfo($"Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: {skillTemplate.name}[{skillTemplate.id}]", 0);
					}
					else
					{
						IdSkillsTanSat.Add(skillTemplate.id);
						GameScr.info1.addInfo($"Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: {skillTemplate.name}[{skillTemplate.id}]", 0);
					}
				}
				else if (StringHandle.IsGetInfoChat<sbyte>(text, "/skillid"))
				{
					sbyte infoChat7 = StringHandle.GetInfoChat<sbyte>(text, "/skillid");
					if (IdSkillsTanSat.Contains(infoChat7))
					{
						IdSkillsTanSat.Remove(infoChat7);
						GameScr.info1.addInfo("Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: " + infoChat7, 0);
					}
					else
					{
						IdSkillsTanSat.Add(infoChat7);
						GameScr.info1.addInfo("Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: " + infoChat7, 0);
					}
				}
				else if (text == "/clrs")
				{
					IdSkillsTanSat.Clear();
					IdSkillsTanSat.AddRange(IdSkillsBase);
					GameScr.info1.addInfo("Đã đặt danh sách skill sử dụng tự động đánh quái về mặc định", 0);
				}
				else if (text == "/abf")
				{
					if (HpBuff == 0 && MpBuff == 0)
					{
						GameScr.info1.addInfo("Tự động sử dụng đậu thần: Tắt", 0);
						break;
					}
					HpBuff = 20;
					MpBuff = 20;
					GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%, MP dưới {MpBuff}%", 0);
				}
				else if (StringHandle.IsGetInfoChat<int>(text, "/abf"))
				{
					HpBuff = StringHandle.GetInfoChat<int>(text, "abf");
					MpBuff = 0;
					GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%", 0);
				}
				else if (StringHandle.IsGetInfoChat<int>(text, "/abf", 2))
				{
					int[] infoChat8 = StringHandle.GetInfoChat<int>(text, "/abf", 2);
					HpBuff = infoChat8[0];
					MpBuff = infoChat8[1];
					GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%, MP dưới {MpBuff}%", 0);
				}
				else if (StringHandle.IsGetInfoChat<int>(text, "/mhp"))
				{
					MobLimitHP = StringHandle.GetInfoChat<int>(text, "/mhp");
					GameScr.info1.addInfo("Cài đặt chỉ đánh quái dưới " + NinjaUtil.getMoneys(MobLimitHP) + " HP", 0);
				}
				else
				{
					if (!(text == "/vdh"))
					{
						return false;
					}
					IsVuotDiaHinh = !IsVuotDiaHinh;
					GameScr.info1.addInfo("Tự động đánh quái vượt địa hình: " + (IsVuotDiaHinh ? "Bật" : "Tắt"), 0);
				}
				break;
			}
			break;
		}
		return true;
	}

	public static bool HotKey(int KeyAscii)
	{
		switch (KeyAscii)
		{
		case 110:
			chat("/anhat");
			break;
		case 116:
			IsTanSat = !IsTanSat;
			GameScr.info1.addInfo("[ThanhLc] Tàn sát quái: " + StringHandle.Status(IsTanSat), 0);
			break;
		default:
			return false;
		}
		return true;
	}

	public static void Update()
	{
		if (!FunctionXmap.IsXmapRunning)
		{
			TrainMobController.Update();
		}
	}

	public static void MobStartDie(object obj)
	{
		Mob mob = (Mob)obj;
		if (mob.status != 1 && mob.status != 0)
		{
			mob.timeLastDie = mSystem.currentTimeMillis();
			mob.countDie++;
			if (mob.countDie > 10)
			{
				mob.countDie = 0;
			}
		}
	}

	public static void UpdateCountDieMob(Mob mob)
	{
		if (mob.levelBoss != 0)
		{
			mob.countDie = 0;
		}
	}
}

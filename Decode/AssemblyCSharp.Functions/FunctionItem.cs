using System;
using System.Collections.Generic;
using System.Threading;

namespace AssemblyCSharp.Functions;

internal class FunctionItem : IActionListener, IChatable
{
	public class ItemAuto
	{
		public int templateId;

		public string Name;

		public int Quantity;

		public short Index;

		public bool IsGold;

		public bool IsSell;

		public int Delay;

		public long LastTimeUse;

		public ItemAuto()
		{
		}

		public ItemAuto(int int_1, string string_0)
		{
			templateId = int_1;
			Name = string_0;
		}

		public ItemAuto(int int_1, short short_0, bool bool_0, bool bool_1)
		{
			templateId = int_1;
			IsGold = bool_0;
			Index = short_0;
			IsSell = bool_1;
		}
	}

	public struct Equipment1
	{
		public string info;

		public int type;

		public Equipment1(int type, string info)
		{
			this.type = type;
			this.info = info;
		}
	}

	public struct Equipment2
	{
		public string info;

		public int type;

		public Equipment2(int type, string info)
		{
			this.type = type;
			this.info = info;
		}
	}

	public struct Items
	{
		public int iconID;

		public string name;

		public Items(int id, string name)
		{
			iconID = id;
			this.name = name;
		}
	}

	private static FunctionItem _Instance;

	private static List<ItemAuto> listItemAuto;

	private static ItemAuto itemAuto;

	public static List<string> set1;

	public static List<string> set2;

	private static bool isChangingClothes;

	private static string[] inputDelay;

	private static string[] inputSellQuantity;

	private static string[] inputBuyQuantity;

	public static List<Equipment1> ListEquipment1;

	public static List<Equipment2> ListEquipment2;

	public static int IndexList;

	public static string OBJECT;

	public static List<Items> ListItemAuto;

	public static bool enableAutoUseItem;

	public static long TIME_DELAY_USE_ITEM;

	public static bool enableAutoSellItem;

	public static bool HotKey(int KeyPress)
	{
		if (KeyPress == 115)
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command("Mặc Set 1\n[Sư phụ]", gI(), 11821, null));
			myVector.addElement(new Command("Mặc Set 2\n[Sư phụ]", gI(), 11822, null));
			GameCanvas.menu.startAt(myVector, 0);
			return true;
		}
		return false;
	}

	public static bool chat(string text)
	{
		if (text == "/autoitem")
		{
			if (ListItemAuto.Count == 0)
			{
				GameScr.info1.addInfo("Danh sách item đang trống..", 0);
			}
			else
			{
				enableAutoUseItem = !enableAutoUseItem;
				GameScr.info1.addInfo("[ThanhLc] Tự động sử dụng item: " + StringHandle.Status(enableAutoUseItem), 0);
			}
			return true;
		}
		return false;
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1:
			OpenTFAutoUseItem((ItemAuto)p);
			break;
		case 3:
			OpenTFAutoTradeItem((ItemAuto)p);
			break;
		case 11821:
			new Thread((ThreadStart)delegate
			{
				EquipItems(1, 4);
			}).Start();
			break;
		case 11822:
			new Thread((ThreadStart)delegate
			{
				EquipItems(2, 4);
			}).Start();
			break;
		}
	}

	public static FunctionItem gI()
	{
		if (_Instance == null)
		{
			_Instance = new FunctionItem();
		}
		return _Instance;
	}

	public static void update()
	{
		if (listItemAuto.Count <= 0)
		{
			return;
		}
		int num = 0;
		ItemAuto itemAuto;
		while (true)
		{
			if (num < listItemAuto.Count)
			{
				itemAuto = listItemAuto[num];
				if (mSystem.currentTimeMillis() - itemAuto.LastTimeUse > itemAuto.Delay * 1000)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		itemAuto.LastTimeUse = mSystem.currentTimeMillis();
		Service.gI().useItem(0, 1, -1, (short)itemAuto.templateId);
	}

	public void onChatFromMe(string text, string to)
	{
		if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
		{
			ChatTextField.gI().isShow = false;
		}
		else if (ChatTextField.gI().strChat.Equals(inputDelay[0]))
		{
			try
			{
				int delay = int.Parse(ChatTextField.gI().tfChat.getText());
				itemAuto.Delay = delay;
				GameScr.info1.addInfo("Auto " + itemAuto.Name + ": " + delay + " giây", 0);
				listItemAuto.Add(itemAuto);
			}
			catch
			{
				GameScr.info1.addInfo("Delay Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
			}
			ResetTF();
		}
		else if (ChatTextField.gI().strChat.Equals(inputBuyQuantity[0]))
		{
			try
			{
				int quantity = int.Parse(ChatTextField.gI().tfChat.getText());
				itemAuto.Quantity = quantity;
				new Thread((ThreadStart)delegate
				{
					AutoBuy(itemAuto);
				}).Start();
			}
			catch
			{
				GameScr.info1.addInfo("Số Lượng Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
			}
			ResetTF();
		}
		else
		{
			if (!ChatTextField.gI().strChat.Equals(inputSellQuantity[0]))
			{
				return;
			}
			try
			{
				int quantity2 = int.Parse(ChatTextField.gI().tfChat.getText());
				itemAuto.Quantity = quantity2;
				new Thread((ThreadStart)delegate
				{
					AutoSell(itemAuto);
				}).Start();
			}
			catch
			{
				GameScr.info1.addInfo("Số Lượng Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
			}
			ResetTF();
		}
	}

	public void infoMe(string s)
	{
		if (s.ToLower().StartsWith("mua thành công") || s.ToLower().StartsWith("buy successful"))
		{
			itemAuto.Quantity--;
		}
	}

	public void onCancelChat()
	{
	}

	private static void ResetTF()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().isShow = false;
	}

	private static void OpenTFAutoUseItem(ItemAuto item)
	{
		itemAuto = item;
		ChatTextField.gI().strChat = inputDelay[0];
		ChatTextField.gI().tfChat.name = inputDelay[1];
		GameCanvas.panel.isShow = false;
		ChatTextField.gI().startChat2(gI(), string.Empty);
	}

	private static void OpenTFAutoTradeItem(ItemAuto item)
	{
		itemAuto = item;
		GameCanvas.panel.isShow = false;
		if (item.IsSell)
		{
			ChatTextField.gI().strChat = inputSellQuantity[0];
			ChatTextField.gI().tfChat.name = inputSellQuantity[1];
		}
		else
		{
			ChatTextField.gI().strChat = inputBuyQuantity[0];
			ChatTextField.gI().tfChat.name = inputBuyQuantity[1];
		}
		ChatTextField.gI().startChat2(gI(), string.Empty);
	}

	private static void AutoSell(ItemAuto item)
	{
		Thread.Sleep(100);
		short index = item.Index;
		while (true)
		{
			if (item.Quantity > 0)
			{
				if (Char.myCharz().arrItemBag[index] == null || (Char.myCharz().arrItemBag[index] != null && Char.myCharz().arrItemBag[index].template.id != (short)item.templateId))
				{
					break;
				}
				Service.gI().saleItem(0, 1, index);
				Thread.Sleep(100);
				Service.gI().saleItem(1, 1, index);
				Thread.Sleep(1000);
				item.Quantity--;
				if (Char.myCharz().xu > 1963100000)
				{
					GameScr.info1.addInfo("Xong!", 0);
					return;
				}
				continue;
			}
			GameScr.info1.addInfo("Xong!", 0);
			return;
		}
		GameScr.info1.addInfo("Không Tìm Thấy Item!", 0);
	}

	private void AutoBuy(ItemAuto item)
	{
		while (item.Quantity > 0 && !GameScr.gI().isBagFull())
		{
			Service.gI().buyItem((!item.IsGold) ? ((sbyte)1) : ((sbyte)0), item.templateId, 0);
			Thread.Sleep(1000);
		}
		GameScr.info1.addInfo("Xong!", 0);
	}

	static FunctionItem()
	{
		ListEquipment1 = new List<Equipment1>();
		ListEquipment2 = new List<Equipment2>();
		ListItemAuto = new List<Items>();
		listItemAuto = new List<ItemAuto>();
		set1 = new List<string>();
		set2 = new List<string>();
		inputDelay = new string[2] { "Nhập delay", "giây" };
		inputSellQuantity = new string[2] { "Nhập số lượng bán", "số lượng" };
		inputBuyQuantity = new string[2] { "Nhập số lượng mua", "số lượng" };
	}

	public static void AddEquipmentstoList(Item item, int ListIndex)
	{
		switch (ListIndex)
		{
		case 1:
			foreach (Equipment1 item2 in ListEquipment1)
			{
				if (item2.type == item.template.type)
				{
					ListEquipment1.Remove(item2);
				}
			}
			ListEquipment1.Add(new Equipment1(item.template.type, item.info));
			GameScr.info1.addInfo("Đã thêm " + item.template.name + " vào set đồ 1", 0);
			break;
		case 2:
			foreach (Equipment2 item3 in ListEquipment2)
			{
				if (item3.type == item.template.type)
				{
					ListEquipment2.Remove(item3);
				}
			}
			ListEquipment2.Add(new Equipment2(item.template.type, item.info));
			GameScr.info1.addInfo("Đã thêm " + item.template.name + " vào set đồ 2", 0);
			break;
		}
	}

	public static void EquipItems(int type, sbyte get)
	{
		switch (type)
		{
		case 1:
		{
			foreach (Equipment1 item in ListEquipment1)
			{
				Item[] arrItemBag2 = Char.myCharz().arrItemBag;
				try
				{
					for (int j = 0; j < arrItemBag2.Length; j++)
					{
						if (arrItemBag2[j].template.type == item.type && arrItemBag2[j].info == item.info)
						{
							Service.gI().getItem(get, (sbyte)j);
							Thread.Sleep(100);
						}
					}
				}
				catch (Exception)
				{
				}
			}
			break;
		}
		case 2:
		{
			foreach (Equipment2 item2 in ListEquipment2)
			{
				Item[] arrItemBag = Char.myCharz().arrItemBag;
				try
				{
					for (int i = 0; i < arrItemBag.Length; i++)
					{
						if (arrItemBag[i].template.type == item2.type && arrItemBag[i].info == item2.info)
						{
							Service.gI().getItem(get, (sbyte)i);
							Thread.Sleep(100);
						}
					}
				}
				catch (Exception)
				{
				}
			}
			break;
		}
		}
	}

	public static void AddItemstoList(Item item)
	{
		foreach (Items item2 in ListItemAuto)
		{
			if (item2.iconID == item.template.iconID)
			{
				ListItemAuto.Remove(item2);
				GameScr.info1.addInfo("Đã xóa " + item.template.name + " khỏi d/s item", 0);
			}
		}
		ListItemAuto.Add(new Items(item.template.iconID, item.template.name));
		GameScr.info1.addInfo("Đã thêm " + item.template.name + " vào d/s item", 0);
	}

	public static void AutoUseItem()
	{
		if (!enableAutoUseItem || GameCanvas.gameTick % 5 != 0)
		{
			return;
		}
		for (int i = 0; i < ListItemAuto.Count; i++)
		{
			Items items = ListItemAuto[i];
			if (ItemTime.isExistItem(items.iconID))
			{
				continue;
			}
			for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
			{
				Item item = Char.myCharz().arrItemBag[j];
				if (item != null && item.template.iconID == items.iconID && mSystem.currentTimeMillis() - TIME_DELAY_USE_ITEM > 10000)
				{
					Service.gI().useItem(0, 1, -1, item.template.id);
					TIME_DELAY_USE_ITEM = mSystem.currentTimeMillis();
				}
			}
		}
	}

	public static int ItemQuantity(int id, string type)
	{
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			Item item = Char.myCharz().arrItemBag[i];
			if (type == "id")
			{
				if (item != null && item.template.id == id && id != 590 && id != 933)
				{
					return item.quantity;
				}
				if (item != null && item.template.id == id && id == 933)
				{
					string[] array = item.itemOption[0].getOptionString().Split(' ');
					return int.Parse(array[2]);
				}
				if (item != null && item.template.id == id && id == 590)
				{
					string[] array2 = item.itemOption[0].getOptionString().Split(' ');
					return int.Parse(array2[2]);
				}
			}
			else if (type == "iconID" && item != null && item.template.iconID == id && id != 590 && id != 933)
			{
				return item.quantity;
			}
		}
		return 0;
	}

	public static void AutoUseGrape()
	{
		if (Char.myCharz().cStamina > 5 || GameCanvas.gameTick % 100 != 0)
		{
			return;
		}
		int num = 0;
		Item item;
		while (true)
		{
			if (num < Char.myCharz().arrItemBag.Length)
			{
				item = Char.myCharz().arrItemBag[num];
				if (item != null && item.template.id == 212)
				{
					break;
				}
				num++;
				continue;
			}
			int num2 = 0;
			Item item2;
			while (true)
			{
				if (num2 < Char.myCharz().arrItemBag.Length)
				{
					item2 = Char.myCharz().arrItemBag[num2];
					if (item2 != null && item2.template.id == 211)
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			Service.gI().useItem(0, 1, (sbyte)item2.indexUI, -1);
			return;
		}
		Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
	}

	public static void AutoSellTrashItem()
	{
		if (!enableAutoSellItem || !GameScr.gI().isBagFull())
		{
			return;
		}
		if (TileMap.mapID != 24 + Char.myCharz().cgender)
		{
			XmapController.StartRunToMapId(24 + Char.myCharz().cgender);
			return;
		}
		for (int i = 0; i < GameScr.vNpc.size(); i++)
		{
			Npc npc = GameScr.vNpc.elementAt(i) as Npc;
			int cx = npc.cx;
			int cy = npc.cy;
			int cx2 = Char.myCharz().cx;
			int cy2 = Char.myCharz().cy;
			if (npc != null && npc.template.npcTemplateId == 16 && Res.distance(cx2, cy2, cx, cy) > 10)
			{
				Char.myCharz().cx = cx;
				Char.myCharz().cy = cy - 3;
				Service.gI().charMove();
				Char.myCharz().cx = cx;
				Char.myCharz().cy = cy;
				Service.gI().charMove();
				Char.myCharz().cx = cx;
				Char.myCharz().cy = cy - 3;
				Service.gI().charMove();
				return;
			}
		}
		for (int num = Char.myCharz().arrItemBag.Length; num > 0; num--)
		{
			Item item = Char.myCharz().arrItemBag[num];
			if (item == null || isItemKichHoat(item) || !isItemHaveStar(item))
			{
			}
		}
	}

	public static bool isItemKichHoat(Item item)
	{
		for (int i = 0; i < item.itemOption.Length; i++)
		{
			if (item.itemOption[i].optionTemplate.name.StartsWith("$"))
			{
				return true;
			}
		}
		return false;
	}

	public static bool isItemHaveStar(Item item)
	{
		for (int i = 0; i < item.itemOption.Length; i++)
		{
			if (item.itemOption[i].optionTemplate.id == 107)
			{
				return true;
			}
		}
		return false;
	}

	public static void Update()
	{
		try
		{
			AutoUseGrape();
		}
		catch (Exception ex)
		{
			FunctionMain.WriteError("Data/Errors/AutoUseGrape.txt", ex.Message);
		}
		try
		{
			AutoUseItem();
		}
		catch (Exception ex2)
		{
			FunctionMain.WriteError("Data/Errors/AutoItem.txt", ex2.Message);
		}
		update();
	}
}

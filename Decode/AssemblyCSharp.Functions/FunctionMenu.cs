using System.Collections.Generic;

namespace AssemblyCSharp.Functions;

internal class FunctionMenu
{
	public static int[] LIST_ITEM_ICONID = new int[20]
	{
		2755, 2756, 2754, 2757, 2760, 6324, 6325, 6326, 6327, 6328,
		2758, 7149, 8060, 8061, 8062, 10714, 10715, 10716, 10712, 10717
	};

	public static string[] LIST_ITEM_NAME = new string[20]
	{
		"Bổ huyết", "Bổ khí", "Cuồng nộ", "Giáp Xên bọ hung", "Ẩn danh", "Bánh Pudding", "Xúc xích", "Kem dâu", "Mì ly", "Sushi",
		"Máy dò Capsule kì bí", "Khẩu trang", "Cua rang me", "Bạch tuộc nướng", "Tôm tẩm bột chiên xù", "Bổ huyết 2", "Bổ khí 2", "Cuồng nộ 2", "Giáp xên bọ hung 2", "Ẩn danh 2"
	};

	public static string[] GRAPHIC_SETTING_LIST_NAME = new string[16]
	{
		"Ẩn người", "Ẩn quái", "Ẩn Npc", "Ẩn item", "Ẩn hiệu ứng", "Ẩn cây cối, đá, ...", "Ẩn phụ kiện đeo lưng", "Ẩn thông báo máy chủ", "Ẩn giao diện game", "Ẩn địa hình",
		"Tinh giảm địa hình", "Ẩn hình ảnh game", "Bật nền màu RGB", "Bật hình nền", "Tối ưu CPU", "Đóng băng quái"
	};

	public const int TYPE_MOD_MENU = 23;

	public static string[][] MenuOption = new string[5][]
	{
		new string[2] { "D.sách", "Item" },
		new string[2] { "Cài đặt", "Đồ họa" },
		new string[2] { "T.báo", "Boss" },
		new string[2] { "D.sách", "Map" },
		new string[2] { "Coming", "Soon!!" }
	};

	public static int PANEL_TYPE { get; set; }

	public static void paintItemList(mGraphics g)
	{
		g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
		g.translate(0, -GameCanvas.panel.cmy);
		for (int i = 0; i < LIST_ITEM_NAME.Length; i++)
		{
			int xScroll = GameCanvas.panel.xScroll;
			int num = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
			int w = GameCanvas.panel.wScroll - 1;
			int h = GameCanvas.panel.ITEM_HEIGHT - 1;
			if (num - GameCanvas.panel.cmy > GameCanvas.panel.yScroll + GameCanvas.panel.hScroll || num - GameCanvas.panel.cmy < GameCanvas.panel.yScroll - GameCanvas.panel.ITEM_HEIGHT)
			{
				continue;
			}
			g.setColor((i != GameCanvas.panel.selected) ? 0 : 0, 0.5f);
			g.fillRect(xScroll, num, w, h);
			if (mGraphics.zoomLevel == 2)
			{
				mFont.tahoma_7_white.drawStringBd(g, LIST_ITEM_NAME[i], xScroll + 30, num, 0, mFont.tahoma_7b_dark);
			}
			else if (mGraphics.zoomLevel == 1)
			{
				mFont.tahoma_7b_green.drawString(g, LIST_ITEM_NAME[i], xScroll + 30, num, 0);
			}
			SmallImage.drawSmallImage(g, LIST_ITEM_ICONID[i], xScroll + 2, num + 2, 0, 0);
			string st = ((FunctionItem.ItemQuantity(LIST_ITEM_ICONID[i], "iconID") > 0) ? ("Số lượng: x" + FunctionItem.ItemQuantity(LIST_ITEM_ICONID[i], "iconID")) : "Không có ITEM");
			mFont mFont = ((FunctionItem.ItemQuantity(LIST_ITEM_ICONID[i], "iconID") > 0) ? mFont.tahoma_7_yellow : mFont.tahoma_7b_dark);
			foreach (FunctionItem.Items item in FunctionItem.ListItemAuto)
			{
				if (item.iconID == LIST_ITEM_ICONID[i])
				{
					mFont = mFont.tahoma_7b_red;
					g.setColor((i != GameCanvas.panel.selected) ? 0 : 0, 0.5f);
					g.fillRect(xScroll, num, w, h);
					st = "ẤN ĐỂ XÓA KHỎI DANH SÁCH ! ! !";
				}
			}
			mFont.drawString(g, st, xScroll + 30, num + 11, 0);
		}
		GameCanvas.panel.paintScrollArrow(g);
	}

	public static void doFireItem(int selected)
	{
		if (selected == -1)
		{
			return;
		}
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			Item item = Char.myCharz().arrItemBag[i];
			if (item == null)
			{
				break;
			}
			if (FunctionItem.ItemQuantity(LIST_ITEM_ICONID[selected], "iconID").Equals(0))
			{
				GameScr.info1.addInfo("Mày làm đéo gì có Item ?", 0);
				break;
			}
			if (FunctionItem.ItemQuantity(LIST_ITEM_ICONID[selected], "iconID") > 0 && item.template.name == LIST_ITEM_NAME[selected])
			{
				FunctionItem.AddItemstoList(item);
			}
		}
	}

	public static string Setting_SubNames(int index)
	{
		return index switch
		{
			0 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableHideChar), 
			1 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableHideMob), 
			2 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableHideNpc), 
			3 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableHideItem), 
			4 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableHideEffect), 
			5 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableHideBgItem), 
			6 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableHideBag), 
			7 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableHideServerNofitication), 
			8 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableHideGameUI), 
			9 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableHideTileMap), 
			10 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableOptimizingTileMap), 
			11 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableHideImage), 
			12 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enablePaintColor_Wallpaper), 
			13 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enablePaintImage_Wallpaper), 
			14 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableOptimizingCPU), 
			15 => "Trạng thái: " + StringHandle.StatusMenu(FunctionGraphic.enableFreezeMob), 
			_ => string.Empty, 
		};
	}

	public static void paintGraphicSetting(mGraphics g)
	{
		g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
		g.translate(0, -GameCanvas.panel.cmy);
		for (int i = 0; i < GRAPHIC_SETTING_LIST_NAME.Length; i++)
		{
			int xScroll = GameCanvas.panel.xScroll;
			int num = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
			int w = GameCanvas.panel.wScroll - 1;
			int h = GameCanvas.panel.ITEM_HEIGHT - 1;
			if (num - GameCanvas.panel.cmy <= GameCanvas.panel.yScroll + GameCanvas.panel.hScroll && num - GameCanvas.panel.cmy >= GameCanvas.panel.yScroll - GameCanvas.panel.ITEM_HEIGHT)
			{
				g.setColor((i != GameCanvas.panel.selected) ? 0 : 0, 0.5f);
				g.fillRect(xScroll, num, w, h);
				mFont.tahoma_7_white.drawString(g, i + 1 + ". " + GRAPHIC_SETTING_LIST_NAME[i], xScroll + 5, num, 0);
				if (Setting_SubNames(i).Contains("Trạng thái: Đang Bật"))
				{
					g.setColor((i != GameCanvas.panel.selected) ? 0 : 0, 0.5f);
					g.fillRect(xScroll, num, w, h);
				}
				((Setting_SubNames(i) == "Trạng thái: Đang Bật") ? mFont.tahoma_7_yellow : mFont.tahoma_7).drawString(g, Setting_SubNames(i), xScroll + 5, num + 11, 0);
			}
		}
	}

	public static void doFireGraphicSetting(int selected)
	{
		switch (selected)
		{
		case 0:
			FunctionGraphic.enableHideChar = !FunctionGraphic.enableHideChar;
			GameScr.info1.addInfo("[ThanhLc] Ẩn người: " + StringHandle.Status(FunctionGraphic.enableHideChar), 0);
			break;
		case 1:
			FunctionGraphic.enableHideMob = !FunctionGraphic.enableHideMob;
			GameScr.info1.addInfo("[ThanhLc] Ẩn quái: " + StringHandle.Status(FunctionGraphic.enableHideMob), 0);
			break;
		case 2:
			FunctionGraphic.enableHideNpc = !FunctionGraphic.enableHideNpc;
			GameScr.info1.addInfo("[ThanhLc] Ẩn NPC: " + StringHandle.Status(FunctionGraphic.enableHideNpc), 0);
			break;
		case 3:
			FunctionGraphic.enableHideItem = !FunctionGraphic.enableHideItem;
			GameScr.info1.addInfo("[ThanhLc] Ẩn Item: " + StringHandle.Status(FunctionGraphic.enableHideItem), 0);
			break;
		case 4:
			FunctionGraphic.enableHideEffect = !FunctionGraphic.enableHideEffect;
			GameScr.info1.addInfo("[ThanhLc] Ẩn hiệu ứng: " + StringHandle.Status(FunctionGraphic.enableHideEffect), 0);
			break;
		case 5:
			FunctionGraphic.enableHideBgItem = !FunctionGraphic.enableHideBgItem;
			GameScr.info1.addInfo("[ThanhLc] Ẩn cây cối, đá,....: " + StringHandle.Status(FunctionGraphic.enableHideBgItem), 0);
			break;
		case 6:
			FunctionGraphic.enableHideBag = !FunctionGraphic.enableHideBag;
			GameScr.info1.addInfo("[ThanhLc] Ẩn phụ kiện đeo lưng: " + StringHandle.Status(FunctionGraphic.enableHideBag), 0);
			break;
		case 7:
			FunctionGraphic.enableHideServerNofitication = !FunctionGraphic.enableHideServerNofitication;
			GameScr.info1.addInfo("[ThanhLc] Ẩn thông báo máy chủ: " + StringHandle.Status(FunctionGraphic.enableHideServerNofitication), 0);
			break;
		case 8:
			FunctionGraphic.enableHideGameUI = !FunctionGraphic.enableHideGameUI;
			GameScr.info1.addInfo("[ThanhLc] Ẩn giao diện game: " + StringHandle.Status(FunctionGraphic.enableHideGameUI), 0);
			break;
		case 9:
			FunctionGraphic.enableHideTileMap = !FunctionGraphic.enableHideTileMap;
			GameScr.info1.addInfo("[ThanhLc] Ẩn địa hình: " + StringHandle.Status(FunctionGraphic.enableHideTileMap), 0);
			break;
		case 10:
			FunctionGraphic.enableOptimizingTileMap = !FunctionGraphic.enableOptimizingTileMap;
			GameScr.info1.addInfo("[ThanhLc] Tinh giảm địa hình: " + StringHandle.Status(FunctionGraphic.enableOptimizingTileMap), 0);
			break;
		case 11:
			FunctionGraphic.enableHideImage = !FunctionGraphic.enableHideImage;
			GameScr.info1.addInfo("[ThanhLc] Ẩn hình ảnh game: " + StringHandle.Status(FunctionGraphic.enableHideImage), 0);
			break;
		case 12:
			if (!FunctionGraphic.enablePaintColor_Wallpaper)
			{
				FunctionChat.gI().OpenChat("Nhập mã màu nền");
				FunctionGraphic.enablePaintColor_Wallpaper = true;
			}
			else
			{
				FunctionGraphic.enablePaintColor_Wallpaper = false;
			}
			GameScr.info1.addInfo("[ThanhLc] Bật hình nền Color RGB: " + StringHandle.Status(FunctionGraphic.enablePaintColor_Wallpaper), 0);
			break;
		case 13:
			FunctionGraphic.enablePaintImage_Wallpaper = !FunctionGraphic.enablePaintImage_Wallpaper;
			GameScr.info1.addInfo("[ThanhLc] Bật hình nền ảnh: " + StringHandle.Status(FunctionGraphic.enablePaintImage_Wallpaper), 0);
			break;
		case 14:
			FunctionGraphic.enableOptimizingCPU = !FunctionGraphic.enableOptimizingCPU;
			GameScr.info1.addInfo("[ThanhLc] Tối ưu CPU: " + StringHandle.Status(FunctionGraphic.enableOptimizingCPU), 0);
			break;
		case 15:
			FunctionGraphic.enableFreezeMob = !FunctionGraphic.enableFreezeMob;
			GameScr.info1.addInfo("[ThanhLc] Đóng băng quái: " + StringHandle.Status(FunctionGraphic.enableFreezeMob), 0);
			break;
		}
	}

	public static void paintTabBoss(mGraphics g)
	{
		g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
		g.translate(0, -GameCanvas.panel.cmy);
		for (int i = 0; i < ListBossInformation.ListBossOnPanel.Count; i++)
		{
			int xScroll = GameCanvas.panel.xScroll;
			int num = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
			int w = GameCanvas.panel.wScroll - 1;
			int h = GameCanvas.panel.ITEM_HEIGHT - 1;
			if (num - GameCanvas.panel.cmy <= GameCanvas.panel.yScroll + GameCanvas.panel.hScroll && num - GameCanvas.panel.cmy >= GameCanvas.panel.yScroll - GameCanvas.panel.ITEM_HEIGHT)
			{
				g.setColor((i == GameCanvas.panel.selected) ? 16383818 : 0, 0.5f);
				g.fillRect(xScroll, num, w, h);
				mFont.tahoma_7b_white.drawString(g, ListBossInformation.ListBossOnPanel[i].name, xScroll + 5, num, 0);
				mFont.tahoma_7_yellow.drawString(g, ListBossInformation.ListBossOnPanel[i].map + " - " + ListBossInformation.ListBossOnPanel[i].AppearTime.ToString("HH:mm:ss"), xScroll + 5, num + 10, 0);
			}
		}
		GameCanvas.panel.paintScrollArrow(g);
	}

	public static void doFireTabBoss(int selected)
	{
		if (selected == -1)
		{
			return;
		}
		for (int i = 0; i < ListBossInformation.ListBossOnPanel.Count; i++)
		{
			if (i == selected)
			{
				XmapController.StartRunToMapId(ListBossInformation.ListBossOnPanel[i].mapId);
			}
		}
	}

	public static void setTypeMenuMod(int panelType)
	{
		GameCanvas.panel.type = 23;
		PANEL_TYPE = panelType;
		setTypeModMenu();
	}

	public static void setTypeModMenu()
	{
		SoundMn.gI().getSoundOption();
		GameCanvas.panel.setType(0);
		if (PANEL_TYPE == 0)
		{
			GameCanvas.panel.tabName[23] = MenuOption;
			GameCanvas.panel.setType(0);
			setTabMenuMod();
		}
	}

	public static void setTabMenuMod()
	{
		if (PANEL_TYPE == 0)
		{
			setTabModMenu();
		}
	}

	public static void setTabModMenu()
	{
		switch (GameCanvas.panel.currentTabIndex)
		{
		case 0:
			GameCanvas.panel.currentListLength = LIST_ITEM_NAME.Length;
			break;
		case 1:
			GameCanvas.panel.currentListLength = GRAPHIC_SETTING_LIST_NAME.Length;
			break;
		case 2:
			GameCanvas.panel.currentListLength = ListBossInformation.ListBossOnPanel.Count;
			break;
		case 3:
			XmapData.Instance().LoadGroupMapsFromFile("TextData\\GroupMapsXmap.txt");
			GameCanvas.panel.currentListLength = XmapData.Instance().GroupMaps.Count;
			break;
		}
		GameCanvas.panel.ITEM_HEIGHT = 24;
		GameCanvas.panel.selected = (GameCanvas.isTouch ? (-1) : 0);
		GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
		if (GameCanvas.panel.cmyLim < 0)
		{
			GameCanvas.panel.cmyLim = 0;
		}
		GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex]);
		if (GameCanvas.panel.cmy < 0)
		{
			GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
		}
		if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim)
		{
			GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim);
		}
	}

	public static void doFireMenu()
	{
		switch (GameCanvas.panel.currentTabIndex)
		{
		case 0:
			doFireItem(GameCanvas.panel.selected);
			break;
		case 1:
			doFireGraphicSetting(GameCanvas.panel.selected);
			break;
		case 2:
			doFireTabBoss(GameCanvas.panel.selected);
			break;
		case 3:
			doFireXmap(GameCanvas.panel.selected);
			break;
		}
	}

	public static void paintMenuMod(mGraphics g)
	{
		if (PANEL_TYPE == 0)
		{
			paintModMenu(g);
		}
	}

	public static void paintModMenu(mGraphics g)
	{
		switch (GameCanvas.panel.currentTabIndex)
		{
		case 0:
			paintItemList(g);
			break;
		case 1:
			paintGraphicSetting(g);
			break;
		case 2:
			paintTabBoss(g);
			break;
		case 3:
			paintXmapMenu(g);
			break;
		}
	}

	public static void paintXmapMenu(mGraphics g)
	{
		g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
		g.translate(0, -GameCanvas.panel.cmy);
		for (int i = 0; i < XmapData.Instance().GroupMaps.Count; i++)
		{
			int xScroll = GameCanvas.panel.xScroll;
			int num = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
			int w = GameCanvas.panel.wScroll - 1;
			int h = GameCanvas.panel.ITEM_HEIGHT - 1;
			if (num - GameCanvas.panel.cmy <= GameCanvas.panel.yScroll + GameCanvas.panel.hScroll && num - GameCanvas.panel.cmy >= GameCanvas.panel.yScroll - GameCanvas.panel.ITEM_HEIGHT)
			{
				g.setColor((i == GameCanvas.panel.selected) ? 16383818 : 0, 0.5f);
				g.fillRect(xScroll, num, w, h);
				mFont.tahoma_7b_white.drawString(g, XmapData.Instance().GroupMaps[i].NameGroup, GameCanvas.panel.xScroll + GameCanvas.panel.wScroll / 2, num + 6, mFont.CENTER);
			}
		}
		GameCanvas.panel.paintScrollArrow(g);
	}

	public static void doFireXmap(int selected)
	{
		if (selected == -1)
		{
			return;
		}
		for (int i = 0; i < XmapData.Instance().GroupMaps.Count; i++)
		{
			if (i == selected)
			{
				List<int> idMaps = XmapData.Instance().GroupMaps[i].IdMaps;
				XmapController.ShowPanelXmap(idMaps);
			}
		}
	}
}

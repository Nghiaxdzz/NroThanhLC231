using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

namespace AssemblyCSharp.Functions;

public class FunctionMain : IActionListener, IChatable
{
	private static FunctionMain _Instance;

	public static Image Logo2;

	public static bool enableHide;

	public static bool enableConnectToClient;

	private static bool IsWait;

	private static long TimeStartWait;

	private static long TimeWait;

	public static FunctionMain gI()
	{
		if (_Instance == null)
		{
			_Instance = new FunctionMain();
		}
		return _Instance;
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 10006:
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command("Chức năng\nGoback", FunctionMap.gI(), 10201, null));
			myVector.addElement(new Command("Auto Click\n Doanh trại\n", FunctionMap.gI(), 10202, null));
			myVector.addElement(new Command("Auto Click\n BDKB\n", FunctionMap.gI(), 10203, null));
			myVector.addElement(new Command("Auto GSM\n", FunctionChar.gI(), 10204, null));
			myVector.addElement(new Command("Unikey\n" + StringHandle.StatusMenu(StringHandle.Unikey.enableUnikey), gI(), 10205, null));
			GameCanvas.menu.startAt(myVector, 0);
			break;
		}
		case 10205:
			StringHandle.Unikey.enableUnikey = !StringHandle.Unikey.enableUnikey;
			GameScr.info1.addInfo("[ThanhLc] Chat tiếng Việt: " + StringHandle.Status(StringHandle.Unikey.enableUnikey), 0);
			break;
		}
	}

	private static void ResetTF()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().isShow = false;
	}

	public void onCancelChat()
	{
	}

	public void onChatFromMe(string text, string to)
	{
		if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
		{
			ChatTextField.gI().isShow = false;
		}
		else if (ChatTextField.gI().strChat.Equals("Nhập mã màu nền"))
		{
			FunctionGraphic.ColorRGB = int.Parse(ChatTextField.gI().tfChat.getText());
			ResetTF();
		}
		else if (ChatTextField.gI().strChat.Equals("Nhập giới hạn HP quái"))
		{
			FunctionTrainMob.MobLimitHP = int.Parse(ChatTextField.gI().tfChat.getText());
			ResetTF();
		}
		ResetTF();
	}

	public void OpenChat(string text)
	{
		ChatTextField.gI().strChat = text;
		ChatTextField.gI().tfChat.name = mResources.CHAT;
		GameCanvas.panel.isShow = false;
		ChatTextField.gI().startChat2(gI(), string.Empty);
	}

	public static bool chat(string text)
	{
		return FunctionBoss.chat(text) || FunctionChar.chat(text) || FunctionPet.chat(text) || FunctionNRSD.chat(text) || FunctionXmap.chat(text) || FunctionTrainMob.chat(text) || FunctionMap.chat(text) || FunctionItem.chat(text) || FunctionChat.chat(text) || FunctionSkill.chat(text);
	}

	public static bool HotKeys(int KeyPress)
	{
		return HotKey(KeyPress) || FunctionXmap.HotKey(KeyPress) || FunctionTrainMob.HotKey(KeyPress) || FunctionSkill.HotKey(KeyPress) || FunctionChar.HotKey(KeyPress) || FunctionItem.HotKey(KeyPress) || FunctionNRSD.HotKey(KeyPress) || FunctionBoss.HotKey(KeyPress);
	}

	public static bool HotKey(int KeyAscii)
	{
		switch (KeyAscii)
		{
		case 109:
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command("Menu\n Training\nMob", FunctionTrainMob.gI(), 10000, null));
			myVector.addElement(new Command("Menu\n Săn boss", FunctionBoss.gI(), 10001, null));
			myVector.addElement(new Command("Menu\n Yardart", FunctionBoss.gI(), 10002, null));
			myVector.addElement(new Command("Menu\n Đệ tử", FunctionPet.gI(), 10003, null));
			myVector.addElement(new Command("Menu\n Auto\nSkills", FunctionSkill.gI(), 10004, null));
			myVector.addElement(new Command("Menu\n Hỗ trợ\nPK", FunctionChar.gI(), 10005, null));
			myVector.addElement(new Command("Menu\n Chức năng\nkhác", gI(), 10006, null));
			GameCanvas.menu.startAt(myVector, 0);
			break;
		}
		case 98:
			Service.gI().friend(0, -1);
			GameScr.info1.addInfo("Đã mở danh sách bạn bè", 0);
			break;
		case 102:
		{
			for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
			{
				Item item = Char.myCharz().arrItemBag[i];
				if (item != null && (item.template.id == 454 || item.template.id == 921))
				{
					Service.gI().useItem(0, 1, -1, item.template.id);
				}
			}
			break;
		}
		case 47:
			ChatTextField.gI().startChat(47, GameScr.gI(), string.Empty);
			break;
		case 122:
			GameCanvas.panel2 = new Panel();
			GameCanvas.panel2.show();
			GameCanvas.panel.setTypeZone();
			GameCanvas.panel.show();
			break;
		case 118:
		{
			Char @char = Char.myCharz();
			if (@char.charFocus != null)
			{
				@char.cx = @char.charFocus.cx;
				@char.cy = @char.charFocus.cy + 1;
				Service.gI().charMove();
				@char.cx = @char.charFocus.cx;
				@char.cy = @char.charFocus.cy;
				Service.gI().charMove();
				@char.cx = @char.charFocus.cx;
				@char.cy = @char.charFocus.cy + 1;
				Service.gI().charMove();
			}
			else if (@char.mobFocus != null)
			{
				@char.cx = @char.mobFocus.x;
				@char.cy = @char.mobFocus.y + 1;
				Service.gI().charMove();
				@char.cx = @char.mobFocus.x;
				@char.cy = @char.mobFocus.y;
				Service.gI().charMove();
				@char.cx = @char.mobFocus.x;
				@char.cy = @char.mobFocus.y + 1;
				Service.gI().charMove();
			}
			else if (@char.npcFocus != null)
			{
				@char.cx = @char.npcFocus.cx;
				@char.cy = @char.npcFocus.cy - 3;
				Service.gI().charMove();
				@char.cx = @char.npcFocus.cx;
				@char.cy = @char.npcFocus.cy;
				Service.gI().charMove();
				@char.cx = @char.npcFocus.cx;
				@char.cy = @char.npcFocus.cy - 3;
				Service.gI().charMove();
			}
			break;
		}
		case 104:
		{
			for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
			{
				Item item2 = Char.myCharz().arrItemBag[j];
				if (item2 != null && item2.template.id == 521)
				{
					Service.gI().useItem(0, 1, -1, item2.template.id);
				}
			}
			break;
		}
		case 103:
			Service.gI().giaodich(0, Char.myCharz().charFocus.charID, -1, -1);
			break;
		case 117:
			Service.gI().chat("/hskill");
			break;
		case 113:
			if (enableConnectToClient)
			{
				FunctionClient.gI().sendMessage(new vMessage
				{
					cmd = 3,
					data = ""
				});
				FunctionClient.IsSendMsg = false;
				enableConnectToClient = false;
				GameScr.info1.addInfo("[ThanhLc] Liên kết với QLTK: " + StringHandle.Status(enableConnectToClient), 0);
			}
			else
			{
				FunctionClient.Connect(FunctionLogin.PortClient);
				enableConnectToClient = true;
				GameScr.info1.addInfo("[ThanhLc] Liên kết với QLTK: " + StringHandle.Status(enableConnectToClient), 0);
			}
			break;
		case 46:
			enableHide = !enableHide;
			GameScr.info1.addInfo("[ThanhLc] Ẩn thông tin: " + StringHandle.Status(enableHide), 0);
			break;
		case 44:
			FunctionGraphic.enableOptimizingCPU = !FunctionGraphic.enableOptimizingCPU;
			GameScr.info1.addInfo("[ThanhLc] Tối ưu CPU: " + StringHandle.Status(FunctionGraphic.enableOptimizingCPU), 0);
			break;
		default:
			return false;
		}
		return true;
	}

	public static void update()
	{
		if (!FunctionXmap.IsXmapRunning)
		{
			FunctionTrainMob.Update();
		}
		FunctionSkill.Update();
		FunctionBoss.Update();
		FunctionPet.Update();
		FunctionChar.Update();
		FunctionMap.Update();
		FunctionItem.Update();
		FunctionChat.Update();
		FunctionNRSD.Update();
	}

	public static void updateKeyTouchController()
	{
		FunctionPet.UpdateTouch();
		ListBossInformation.updateKeyTouchController();
		FunctionChar.ListCharsInMap.updateTouch();
	}

	public static void paint(mGraphics g)
	{
		if (!GameCanvas.panel.isShow && Char.myCharz().havePet)
		{
			g.drawImage(GameScr.imgMenu, 0, 150, 0);
		}
		paintInfo(g);
		FunctionChar.paint(g);
		FunctionBoss.paint(g);
		FunctionPet.paintPetInformation(g);
	}

	public static void paintInfo(mGraphics g)
	{
		if (TileMap.mapID != 129)
		{
			if (Logo2 == null)
			{
				Logo2 = FunctionGraphic.createImage(FunctionLogin.enableToChangeLogo ? "Data/logo.png" : "logoGameScr", mGraphics.zoomLevel);
			}
			else
			{
				g.drawRegion(Logo2, 0, 0, mGraphics.getImageWidth(Logo2), mGraphics.getImageHeight(Logo2), 0, (GameCanvas.w / 2 < mGraphics.getImageWidth(GameScr.imgPanel)) ? (mGraphics.getImageWidth(GameScr.imgPanel) + 25) : (GameCanvas.w / 2), 5, (GameCanvas.w / 2 < mGraphics.getImageWidth(GameScr.imgPanel)) ? mGraphics.LEFT : mGraphics.HCENTER);
			}
		}
		if (enableHide)
		{
			return;
		}
		GUIStyle[] array = new GUIStyle[30];
		GUIStyle[] array2 = new GUIStyle[30];
		int num = 1;
		List<string> list = new List<string>();
		list.Add("[" + TileMap.mapID + "] " + TileMap.mapName + " - Khu " + TileMap.zoneID);
		list.Add("TG: " + DateTime.Now.ToString("HH:mm:ss") + " - [" + (FunctionMap.NumPlayersInZone() + 1) + (FunctionNRSD.isMeInNRDMap() ? "/20]" : "/15]"));
		list.Add("X: " + Char.myCharz().cx + " Y: " + Char.myCharz().cy);
		if (FunctionSkill.enableSendAttack)
		{
			list.Add("Tự đánh: " + StringHandle.Status(FunctionSkill.enableSendAttack));
		}
		if (FunctionTrainMob.IsAutoPickItems)
		{
			list.Add("Tự nhặt: " + StringHandle.Status(FunctionTrainMob.IsAutoPickItems));
		}
		if (FunctionPet.enableAutoRevive)
		{
			list.Add("Tự hồi sinh: " + StringHandle.Status(FunctionPet.enableAutoRevive));
		}
		if (FunctionPet.enableAutoJump)
		{
			list.Add("Autojump: " + StringHandle.Status(FunctionPet.enableAutoJump));
		}
		if (FunctionChar.enableLockCharFocus)
		{
			list.Add("Đang khóa ID: " + FunctionChar.CharID);
		}
		if (enableConnectToClient)
		{
			list.Add("Đã kết nối với QLTK!");
		}
		int num2 = 104;
		if (mGraphics.zoomLevel == 2)
		{
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.UpperLeft,
					fontSize = 7 * mGraphics.zoomLevel,
					fontStyle = FontStyle.Bold
				};
				array2[i] = new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.UpperLeft,
					fontSize = 7 * mGraphics.zoomLevel,
					fontStyle = FontStyle.Bold
				};
				array[i].normal.textColor = Color.white;
				if (list[i].StartsWith("Đang khóa ID"))
				{
					array[i].normal.textColor = Color.red;
				}
				if (list[i].StartsWith("Auto phù HP: "))
				{
					array[i].normal.textColor = Color.red;
				}
				if (list[i].StartsWith("Đã kết nối với QLTK!"))
				{
					array[i].normal.textColor = Color.red;
				}
				array2[i].normal.textColor = Color.black;
				g.drawString(list[i], 24.5f, (float)(num2 + i * 8) - 0.5f, array2[i]);
				g.drawString(list[i], 24.5f, (float)(num2 + i * 8) + 0.5f, array2[i]);
				g.drawString(list[i], 25.5f, (float)(num2 + i * 8) - 0.5f, array2[i]);
				g.drawString(list[i], 25.5f, (float)(num2 + i * 8) + 0.5f, array2[i]);
				g.drawString(list[i], 25f, (float)(num2 + i * 8) - 0.5f, array2[i]);
				g.drawString(list[i], 25f, (float)(num2 + i * 8) + 0.5f, array2[i]);
				g.drawString(list[i], 24.5f, num2 + i * 8, array2[i]);
				g.drawString(list[i], 25.5f, num2 + i * 8, array2[i]);
				g.drawString(list[i], 25f, num2 + i * 8, array[i]);
				FunctionChar.SuicideRange.yPaint = num2 + list.Count * 8;
				FunctionPet.yDrawPetInfo = num2 + list.Count * 8;
				num++;
			}
			return;
		}
		for (int j = 0; j < list.Count; j++)
		{
			mFont mFont = (list[j].StartsWith("Đang khóa ID") ? mFont.tahoma_7b_red : (list[j].StartsWith("Auto phù HP: ") ? mFont.tahoma_7b_red : ((!list[j].StartsWith("Đã kết nối với QLTK!")) ? mFont.tahoma_7_white : mFont.tahoma_7b_red)));
			if (mFont == mFont.tahoma_7_white)
			{
				mFont.tahoma_7.drawString(g, list[j], 24, num2 + j * 10 - 1, 0);
				mFont.tahoma_7.drawString(g, list[j], 24, num2 + j * 10 + 1, 0);
				mFont.tahoma_7.drawString(g, list[j], 26, num2 + j * 10 - 1, 0);
				mFont.tahoma_7.drawString(g, list[j], 26, num2 + j * 10 + 1, 0);
				mFont.tahoma_7.drawString(g, list[j], 25, num2 + j * 10 - 1, 0);
				mFont.tahoma_7.drawString(g, list[j], 25, num2 + j * 10 + 1, 0);
				mFont.tahoma_7.drawString(g, list[j], 24, num2 + j * 10, 0);
				mFont.tahoma_7.drawString(g, list[j], 26, num2 + j * 10, 0);
			}
			else
			{
				mFont.tahoma_7b_dark.drawString(g, list[j], 24, num2 + j * 10 - 1, 0);
				mFont.tahoma_7b_dark.drawString(g, list[j], 24, num2 + j * 10 + 1, 0);
				mFont.tahoma_7b_dark.drawString(g, list[j], 26, num2 + j * 10 - 1, 0);
				mFont.tahoma_7b_dark.drawString(g, list[j], 26, num2 + j * 10 + 1, 0);
				mFont.tahoma_7b_dark.drawString(g, list[j], 25, num2 + j * 10 - 1, 0);
				mFont.tahoma_7b_dark.drawString(g, list[j], 25, num2 + j * 10 + 1, 0);
				mFont.tahoma_7b_dark.drawString(g, list[j], 24, num2 + j * 10, 0);
				mFont.tahoma_7b_dark.drawString(g, list[j], 26, num2 + j * 10, 0);
			}
			mFont.drawString(g, list[j], 25, num2 + j * 10, 0);
			FunctionChar.SuicideRange.yPaint = num2 + list.Count * 10;
			FunctionPet.yDrawPetInfo = num2 + list.Count * 10;
			num++;
		}
	}

	public static void WriteError(string path, string message)
	{
		File.WriteAllText(path, message);
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

	public void doFireClient(vMessage msg)
	{
		try
		{
			switch (msg.cmd)
			{
			case 1:
				XmapController.StartRunToMapId(int.Parse(msg.data.ToString()));
				msg.data = null;
				break;
			case 2:
				Service.gI().requestChangeZone(int.Parse(msg.data.ToString()), -1);
				msg.data = null;
				break;
			case 6:
			{
				int num3 = Convert.ToInt32(msg.data);
				Skill skill = GameScr.keySkill[num3 - 1];
				if (skill != null && Char.myCharz().myskill != skill)
				{
					GameScr.gI().doSelectSkill(skill, isShortcut: true);
				}
				Thread.Sleep(100);
				GameScr.gI().doUseSkill(skill, isShortcut: true);
				break;
			}
			case 8:
				try
				{
					int num2 = 0;
					if (num2 >= ListBossInformation.ListBossOnScreen.Count)
					{
						break;
					}
					if (TileMap.mapID != ListBossInformation.ListBossOnScreen[ListBossInformation.ListBossOnScreen.Count - 1].mapId)
					{
						if (FunctionXmap.IsXmapRunning)
						{
							XmapController.FinishXmap();
						}
						XmapController.StartRunToMapId(ListBossInformation.ListBossOnScreen[ListBossInformation.ListBossOnScreen.Count - 1].mapId);
					}
					else if (ListBossInformation.ListBossOnScreen[num2].zoneId != -1 && TileMap.zoneID != ListBossInformation.ListBossOnScreen[num2].zoneId)
					{
						Service.gI().requestChangeZone(ListBossInformation.ListBossOnScreen[num2].zoneId, 0);
					}
					else
					{
						Char.myCharz().currentMovePoint = null;
						GameCanvas.clearAllPointerEvent();
						msg.data = null;
					}
					break;
				}
				catch
				{
					break;
				}
			case 10:
				FunctionPet.enableRequestBean = (msg.data.ToString().Contains("true") ? true : false);
				msg.data = null;
				break;
			case 11:
				FunctionPet.enableCollectBean = (msg.data.ToString().Contains("true") ? true : false);
				msg.data = null;
				break;
			case 12:
				FunctionPet.enableGiveBean = (msg.data.ToString().Contains("true") ? true : false);
				msg.data = null;
				break;
			case 13:
				if (msg.data.ToString().Contains("true"))
				{
					Service.gI().getFlag(1, 8);
				}
				else
				{
					Service.gI().getFlag(1, 0);
				}
				msg.data = null;
				break;
			case 14:
				FunctionPet.enableAutoJump = (msg.data.ToString().Contains("true") ? true : false);
				break;
			case 15:
				FunctionGraphic.enableHideChar = (msg.data.ToString().Contains("true") ? true : false);
				FunctionGraphic.enableHideNpc = (msg.data.ToString().Contains("true") ? true : false);
				FunctionGraphic.enableHideMob = (msg.data.ToString().Contains("true") ? true : false);
				FunctionGraphic.enableHideItem = (msg.data.ToString().Contains("true") ? true : false);
				FunctionGraphic.enableHideEffect = (msg.data.ToString().Contains("true") ? true : false);
				FunctionGraphic.enableHideBgItem = (msg.data.ToString().Contains("true") ? true : false);
				FunctionGraphic.enableHideBag = (msg.data.ToString().Contains("true") ? true : false);
				FunctionGraphic.enableHideServerNofitication = (msg.data.ToString().Contains("true") ? true : false);
				FunctionGraphic.enableHideGameUI = (msg.data.ToString().Contains("true") ? true : false);
				FunctionGraphic.enableHideTileMap = (msg.data.ToString().Contains("true") ? true : false);
				FunctionGraphic.enableOptimizingCPU = (msg.data.ToString().Contains("true") ? true : false);
				FunctionGraphic.enableHideImage = (msg.data.ToString().Contains("true") ? true : false);
				msg.data = null;
				break;
			case 16:
				if (Char.myCharz().meDead)
				{
					Service.gI().returnTownFromDead();
				}
				else
				{
					XmapController.StartRunToMapId(21 + Char.myCharz().cgender);
				}
				msg.data = null;
				break;
			case 17:
				if (Char.myCharz().luong + Char.myCharz().luongKhoa > 0 && Char.myCharz().meDead && Char.myCharz().cHP <= 0)
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
					msg.data = null;
				}
				break;
			case 18:
				if (msg.data.ToString().Equals("true"))
				{
					Item[] arrItemBag = Char.myCharz().arrItemBag;
					try
					{
						for (int j = 0; j < arrItemBag.Length; j++)
						{
							if (arrItemBag[j].template.iconID == 4387 && !ItemTime.isExistItem(4387))
							{
								Service.gI().useItem(0, 1, -1, arrItemBag[j].template.id);
							}
						}
					}
					catch
					{
					}
				}
				else
				{
					Item[] arrItemBag2 = Char.myCharz().arrItemBag;
					try
					{
						for (int k = 0; k < arrItemBag2.Length; k++)
						{
							if (arrItemBag2[k].template.iconID == 4387 && ItemTime.isExistItem(4387))
							{
								Service.gI().useItem(0, 1, -1, arrItemBag2[k].template.id);
							}
						}
					}
					catch
					{
					}
				}
				msg.data = null;
				break;
			case 19:
			{
				int num = int.Parse(msg.data.ToString());
				if (!ItemTime.isExistItem(num))
				{
					for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
					{
						Item item = Char.myCharz().arrItemBag[i];
						if (item != null && item.template.iconID == num)
						{
							Service.gI().useItem(0, 1, -1, item.template.id);
						}
					}
				}
				msg.data = null;
				break;
			}
			case 25:
				CapsuleBackwardMap();
				break;
			case 3:
			case 4:
			case 5:
			case 7:
			case 9:
			case 20:
			case 21:
			case 22:
			case 23:
			case 24:
				break;
			}
		}
		catch
		{
		}
	}

	public static void CapsuleBackwardMap()
	{
		GameCanvas.gI().keyPressedz(99);
		Thread.Sleep(100);
		Service.gI().requestMapSelect(0);
	}

	public static string GetMacAddress()
	{
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		for (int i = 0; i < allNetworkInterfaces.Length; i++)
		{
			PhysicalAddress physicalAddress = allNetworkInterfaces[i].GetPhysicalAddress();
			if (physicalAddress.ToString() != string.Empty)
			{
				return physicalAddress.ToString();
			}
		}
		return string.Empty;
	}

	public static string CreateMD5(string input)
	{
		using MD5 mD = MD5.Create();
		byte[] bytes = Encoding.ASCII.GetBytes(input);
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}
}

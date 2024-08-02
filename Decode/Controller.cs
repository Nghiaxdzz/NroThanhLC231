using System;
using AssemblyCSharp.Functions;
using Assets.src.e;
using Assets.src.f;
using Assets.src.g;
using UnityEngine;

public class Controller : IMessageHandler
{
	protected static Controller me;

	protected static Controller me2;

	public Message messWait;

	public static bool isLoadingData;

	public static bool isConnectOK;

	public static bool isConnectionFail;

	public static bool isDisconnected;

	public static bool isMain;

	private float demCount;

	private int move;

	private int total;

	public static bool isStopReadMessage;

	public static MyHashTable frameHT_NEWBOSS = new MyHashTable();

	public const sbyte PHUBAN_TYPE_CHIENTRUONGNAMEK = 0;

	public const sbyte PHUBAN_START = 0;

	public const sbyte PHUBAN_UPDATE_POINT = 1;

	public const sbyte PHUBAN_END = 2;

	public const sbyte PHUBAN_LIFE = 4;

	public const sbyte PHUBAN_INFO = 5;

	public static Controller gI()
	{
		if (me == null)
		{
			me = new Controller();
		}
		return me;
	}

	public static Controller gI2()
	{
		if (me2 == null)
		{
			me2 = new Controller();
		}
		return me2;
	}

	public void onConnectOK(bool isMain1)
	{
		isMain = isMain1;
		mSystem.onConnectOK();
	}

	public void onConnectionFail(bool isMain1)
	{
		isMain = isMain1;
		mSystem.onConnectionFail();
	}

	public void onDisconnected(bool isMain1)
	{
		isMain = isMain1;
		mSystem.onDisconnected();
	}

	public void requestItemPlayer(Message msg)
	{
		try
		{
			int num = msg.reader().readUnsignedByte();
			Item item = GameScr.currentCharViewInfo.arrItemBody[num];
			item.saleCoinLock = msg.reader().readInt();
			item.sys = msg.reader().readByte();
			item.options = new MyVector();
			try
			{
				while (true)
				{
					item.options.addElement(new ItemOption(msg.reader().readUnsignedByte(), msg.reader().readUnsignedShort()));
				}
			}
			catch (Exception ex)
			{
				Cout.println("Loi tairequestItemPlayer 1" + ex.ToString());
			}
		}
		catch (Exception ex2)
		{
			Cout.println("Loi tairequestItemPlayer 2" + ex2.ToString());
		}
	}

	public void onMessage(Message msg)
	{
		GameCanvas.debugSession.removeAllElements();
		GameCanvas.debug("SA1", 2);
		try
		{
			Res.outz("<<<Read cmd= " + msg.command);
			Char @char = null;
			Mob mob = null;
			MyVector myVector = new MyVector();
			int num = 0;
			Controller2.readMessage(msg);
			switch (msg.command)
			{
			case 24:
				read_opt(msg);
				break;
			case 20:
				phuban_Info(msg);
				break;
			case 66:
				readGetImgByName(msg);
				break;
			case 65:
			{
				sbyte id2 = msg.reader().readSByte();
				string text6 = msg.reader().readUTF();
				short num130 = msg.reader().readShort();
				if (ItemTime.isExistMessage(id2))
				{
					if (num130 != 0)
					{
						ItemTime.getMessageById(id2).initTimeText(id2, text6, num130);
					}
					else
					{
						GameScr.textTime.removeElement(ItemTime.getMessageById(id2));
					}
				}
				else
				{
					ItemTime itemTime = new ItemTime();
					itemTime.initTimeText(id2, text6, num130);
					GameScr.textTime.addElement(itemTime);
				}
				break;
			}
			case 112:
			{
				sbyte b54 = msg.reader().readByte();
				Res.outz("spec type= " + b54);
				switch (b54)
				{
				case 0:
					Panel.spearcialImage = msg.reader().readShort();
					Panel.specialInfo = msg.reader().readUTF();
					break;
				case 1:
				{
					sbyte b55 = msg.reader().readByte();
					Char.myCharz().infoSpeacialSkill = new string[b55][];
					Char.myCharz().imgSpeacialSkill = new short[b55][];
					GameCanvas.panel.speacialTabName = new string[b55][];
					for (int num149 = 0; num149 < b55; num149++)
					{
						GameCanvas.panel.speacialTabName[num149] = new string[2];
						string[] array15 = Res.split(msg.reader().readUTF(), "\n", 0);
						if (array15.Length == 2)
						{
							GameCanvas.panel.speacialTabName[num149] = array15;
						}
						if (array15.Length == 1)
						{
							GameCanvas.panel.speacialTabName[num149][0] = array15[0];
							GameCanvas.panel.speacialTabName[num149][1] = string.Empty;
						}
						int num150 = msg.reader().readByte();
						Char.myCharz().infoSpeacialSkill[num149] = new string[num150];
						Char.myCharz().imgSpeacialSkill[num149] = new short[num150];
						for (int num151 = 0; num151 < num150; num151++)
						{
							Char.myCharz().imgSpeacialSkill[num149][num151] = msg.reader().readShort();
							Char.myCharz().infoSpeacialSkill[num149][num151] = msg.reader().readUTF();
						}
					}
					GameCanvas.panel.tabName[25] = GameCanvas.panel.speacialTabName;
					GameCanvas.panel.setTypeSpeacialSkill();
					GameCanvas.panel.show();
					break;
				}
				}
				break;
			}
			case -98:
			{
				sbyte b38 = msg.reader().readByte();
				GameCanvas.menu.showMenu = false;
				if (b38 == 0)
				{
					GameCanvas.startYesNoDlg(msg.reader().readUTF(), new Command(mResources.YES, GameCanvas.instance, 888397, msg.reader().readUTF()), new Command(mResources.NO, GameCanvas.instance, 888396, null));
				}
				break;
			}
			case -97:
				Char.myCharz().cNangdong = msg.reader().readInt();
				break;
			case -96:
			{
				sbyte typeTop = msg.reader().readByte();
				GameCanvas.panel.vTop.removeAllElements();
				string topName = msg.reader().readUTF();
				sbyte b58 = msg.reader().readByte();
				for (int num157 = 0; num157 < b58; num157++)
				{
					int rank = msg.reader().readInt();
					int pId = msg.reader().readInt();
					short headID = msg.reader().readShort();
					short headICON = msg.reader().readShort();
					short body = msg.reader().readShort();
					short leg = msg.reader().readShort();
					string name = msg.reader().readUTF();
					string info3 = msg.reader().readUTF();
					TopInfo topInfo = new TopInfo();
					topInfo.rank = rank;
					topInfo.headID = headID;
					topInfo.headICON = headICON;
					topInfo.body = body;
					topInfo.leg = leg;
					topInfo.name = name;
					topInfo.info = info3;
					topInfo.info2 = msg.reader().readUTF();
					topInfo.pId = pId;
					GameCanvas.panel.vTop.addElement(topInfo);
				}
				GameCanvas.panel.topName = topName;
				GameCanvas.panel.setTypeTop(typeTop);
				GameCanvas.panel.show();
				break;
			}
			case -94:
				while (msg.reader().available() > 0)
				{
					short num15 = msg.reader().readShort();
					int num16 = msg.reader().readInt();
					for (int l = 0; l < Char.myCharz().vSkill.size(); l++)
					{
						Skill skill = (Skill)Char.myCharz().vSkill.elementAt(l);
						if (skill != null && skill.skillId == num15)
						{
							if (num16 < skill.coolDown)
							{
								skill.lastTimeUseThisSkill = mSystem.currentTimeMillis() - (skill.coolDown - num16);
							}
							Res.outz("1 chieu id= " + skill.template.id + " cooldown= " + num16 + " curr cool down= " + skill.coolDown);
						}
					}
				}
				break;
			case -95:
			{
				sbyte b28 = msg.reader().readByte();
				Res.outz("type= " + b28);
				if (b28 == 0)
				{
					int num69 = msg.reader().readInt();
					short templateId = msg.reader().readShort();
					int num70 = msg.readInt3Byte();
					SoundMn.gI().explode_1();
					if (num69 == Char.myCharz().charID)
					{
						Char.myCharz().mobMe = new Mob(num69, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num70, 0, num70, (short)(Char.myCharz().cx + ((Char.myCharz().cdir != 1) ? (-40) : 40)), (short)Char.myCharz().cy, 4, 0);
						Char.myCharz().mobMe.isMobMe = true;
						EffecMn.addEff(new Effect(18, Char.myCharz().mobMe.x, Char.myCharz().mobMe.y, 2, 10, -1));
						Char.myCharz().tMobMeBorn = 30;
						GameScr.vMob.addElement(Char.myCharz().mobMe);
					}
					else
					{
						@char = GameScr.findCharInMap(num69);
						if (@char != null)
						{
							Mob mob4 = new Mob(num69, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num70, 0, num70, (short)@char.cx, (short)@char.cy, 4, 0);
							mob4.isMobMe = true;
							@char.mobMe = mob4;
							GameScr.vMob.addElement(@char.mobMe);
						}
						else
						{
							Mob mob5 = GameScr.findMobInMap(num69);
							if (mob5 == null)
							{
								mob5 = new Mob(num69, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num70, 0, num70, -100, -100, 4, 0);
								mob5.isMobMe = true;
								GameScr.vMob.addElement(mob5);
							}
						}
					}
				}
				if (b28 == 1)
				{
					int num71 = msg.reader().readInt();
					int mobId = msg.reader().readByte();
					Res.outz("mod attack id= " + num71);
					if (num71 == Char.myCharz().charID)
					{
						if (GameScr.findMobInMap(mobId) != null)
						{
							Char.myCharz().mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
						}
					}
					else
					{
						@char = GameScr.findCharInMap(num71);
						if (@char != null && GameScr.findMobInMap(mobId) != null)
						{
							@char.mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
						}
					}
				}
				if (b28 == 2)
				{
					int num72 = msg.reader().readInt();
					int num73 = msg.reader().readInt();
					int num74 = msg.readInt3Byte();
					int cHPNew = msg.readInt3Byte();
					if (num72 == Char.myCharz().charID)
					{
						Res.outz("mob dame= " + num74);
						@char = GameScr.findCharInMap(num73);
						if (@char != null)
						{
							@char.cHPNew = cHPNew;
							if (Char.myCharz().mobMe.isBusyAttackSomeOne)
							{
								@char.doInjure(num74, 0, isCrit: false, isMob: true);
							}
							else
							{
								Char.myCharz().mobMe.dame = num74;
								Char.myCharz().mobMe.setAttack(@char);
							}
						}
					}
					else
					{
						mob = GameScr.findMobInMap(num72);
						if (mob != null)
						{
							if (num73 == Char.myCharz().charID)
							{
								Char.myCharz().cHPNew = cHPNew;
								if (mob.isBusyAttackSomeOne)
								{
									Char.myCharz().doInjure(num74, 0, isCrit: false, isMob: true);
								}
								else
								{
									mob.dame = num74;
									mob.setAttack(Char.myCharz());
								}
							}
							else
							{
								@char = GameScr.findCharInMap(num73);
								if (@char != null)
								{
									@char.cHPNew = cHPNew;
									if (mob.isBusyAttackSomeOne)
									{
										@char.doInjure(num74, 0, isCrit: false, isMob: true);
									}
									else
									{
										mob.dame = num74;
										mob.setAttack(@char);
									}
								}
							}
						}
					}
				}
				if (b28 == 3)
				{
					int num75 = msg.reader().readInt();
					int mobId2 = msg.reader().readInt();
					int hp = msg.readInt3Byte();
					int num76 = msg.readInt3Byte();
					@char = null;
					@char = ((Char.myCharz().charID != num75) ? GameScr.findCharInMap(num75) : Char.myCharz());
					if (@char != null)
					{
						mob = GameScr.findMobInMap(mobId2);
						if (@char.mobMe != null)
						{
							@char.mobMe.attackOtherMob(mob);
						}
						if (mob != null)
						{
							mob.hp = hp;
							mob.updateHp_bar();
							if (num76 == 0)
							{
								mob.x = mob.xFirst;
								mob.y = mob.yFirst;
								GameScr.startFlyText(mResources.miss, mob.x, mob.y - mob.h, 0, -2, mFont.MISS);
							}
							else
							{
								GameScr.startFlyText("-" + num76, mob.x, mob.y - mob.h, 0, -2, mFont.ORANGE);
							}
						}
					}
				}
				if (b28 == 4)
				{
				}
				if (b28 == 5)
				{
					int num77 = msg.reader().readInt();
					sbyte b29 = msg.reader().readByte();
					int mobId3 = msg.reader().readInt();
					int num78 = msg.readInt3Byte();
					int hp2 = msg.readInt3Byte();
					@char = null;
					@char = ((num77 != Char.myCharz().charID) ? GameScr.findCharInMap(num77) : Char.myCharz());
					if (@char == null)
					{
						return;
					}
					if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
					{
						@char.setSkillPaint(GameScr.sks[b29], 0);
					}
					else
					{
						@char.setSkillPaint(GameScr.sks[b29], 1);
					}
					Mob mob6 = GameScr.findMobInMap(mobId3);
					if (@char.cx <= mob6.x)
					{
						@char.cdir = 1;
					}
					else
					{
						@char.cdir = -1;
					}
					@char.mobFocus = mob6;
					mob6.hp = hp2;
					mob6.updateHp_bar();
					GameCanvas.debug("SA83v2", 2);
					if (num78 == 0)
					{
						mob6.x = mob6.xFirst;
						mob6.y = mob6.yFirst;
						GameScr.startFlyText(mResources.miss, mob6.x, mob6.y - mob6.h, 0, -2, mFont.MISS);
					}
					else
					{
						GameScr.startFlyText("-" + num78, mob6.x, mob6.y - mob6.h, 0, -2, mFont.ORANGE);
					}
				}
				if (b28 == 6)
				{
					int num79 = msg.reader().readInt();
					if (num79 == Char.myCharz().charID)
					{
						Char.myCharz().mobMe.startDie();
					}
					else
					{
						@char = GameScr.findCharInMap(num79);
						@char?.mobMe.startDie();
					}
				}
				if (b28 != 7)
				{
					break;
				}
				int num80 = msg.reader().readInt();
				if (num80 == Char.myCharz().charID)
				{
					Char.myCharz().mobMe = null;
					for (int num81 = 0; num81 < GameScr.vMob.size(); num81++)
					{
						if (((Mob)GameScr.vMob.elementAt(num81)).mobId == num80)
						{
							GameScr.vMob.removeElementAt(num81);
						}
					}
					break;
				}
				@char = GameScr.findCharInMap(num80);
				for (int num82 = 0; num82 < GameScr.vMob.size(); num82++)
				{
					if (((Mob)GameScr.vMob.elementAt(num82)).mobId == num80)
					{
						GameScr.vMob.removeElementAt(num82);
					}
				}
				if (@char != null)
				{
					@char.mobMe = null;
				}
				break;
			}
			case -92:
				Main.typeClient = msg.reader().readByte();
				Rms.clearAll();
				Rms.saveRMSInt("clienttype", Main.typeClient);
				Rms.saveRMSInt("lastZoomlevel", mGraphics.zoomLevel);
				GameCanvas.startOK(mResources.plsRestartGame, 8885, null);
				break;
			case -91:
			{
				sbyte b44 = msg.reader().readByte();
				GameCanvas.panel.mapNames = new string[b44];
				GameCanvas.panel.planetNames = new string[b44];
				for (int num121 = 0; num121 < b44; num121++)
				{
					GameCanvas.panel.mapNames[num121] = msg.reader().readUTF();
					GameCanvas.panel.planetNames[num121] = msg.reader().readUTF();
				}
				FunctionXmap.ShowPanelMapTrans();
				break;
			}
			case -90:
			{
				sbyte b41 = msg.reader().readByte();
				int num109 = msg.reader().readInt();
				Res.outz("===> UPDATE_BODY:    type = " + b41);
				@char = ((Char.myCharz().charID != num109) ? GameScr.findCharInMap(num109) : Char.myCharz());
				if (b41 != -1)
				{
					short num110 = msg.reader().readShort();
					short num111 = msg.reader().readShort();
					short num112 = msg.reader().readShort();
					sbyte isMonkey = msg.reader().readByte();
					Res.err("====> Cmd: -90 UPDATE_BODY   \n  isMonkey= " + isMonkey + " head=  " + num110 + " body= " + num111 + " legU= " + num112);
					if (@char != null)
					{
						if (@char.charID == num109)
						{
							@char.isMask = true;
							@char.isMonkey = isMonkey;
							if (@char.isMonkey != 0)
							{
								@char.isWaitMonkey = false;
								@char.isLockMove = false;
							}
						}
						else if (@char != null)
						{
							@char.isMask = true;
							@char.isMonkey = isMonkey;
						}
						if (num110 != -1)
						{
							@char.head = num110;
						}
						if (num111 != -1)
						{
							@char.body = num111;
						}
						if (num112 != -1)
						{
							@char.leg = num112;
						}
					}
				}
				if (b41 == -1 && @char != null)
				{
					@char.isMask = false;
					@char.isMonkey = 0;
				}
				if (@char != null)
				{
				}
				break;
			}
			case -88:
				GameCanvas.endDlg();
				GameCanvas.serverScreen.switchToMe();
				break;
			case -87:
			{
				Res.outz("GET UPDATE_DATA " + msg.reader().available() + " bytes");
				msg.reader().mark(100000);
				createData(msg.reader(), isSaveRMS: true);
				msg.reader().reset();
				sbyte[] data = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data);
				sbyte[] data2 = new sbyte[1] { GameScr.vcData };
				Rms.saveRMS("NRdataVersion", data2);
				LoginScr.isUpdateData = false;
				if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
				{
					Res.outz(GameScr.vsData + "," + GameScr.vsMap + "," + GameScr.vsSkill + "," + GameScr.vsItem);
					GameScr.gI().readDart();
					GameScr.gI().readEfect();
					GameScr.gI().readArrow();
					GameScr.gI().readSkill();
					Service.gI().clientOk();
					return;
				}
				break;
			}
			case -86:
			{
				sbyte b51 = msg.reader().readByte();
				Res.outz("server gui ve giao dich action = " + b51);
				if (b51 == 0)
				{
					int playerID = msg.reader().readInt();
					GameScr.gI().giaodich(playerID);
				}
				if (b51 == 1)
				{
					int num143 = msg.reader().readInt();
					Char char10 = GameScr.findCharInMap(num143);
					if (char10 == null)
					{
						return;
					}
					GameCanvas.panel.setTypeGiaoDich(char10);
					GameCanvas.panel.show();
					Service.gI().getPlayerMenu(num143);
				}
				if (b51 == 2)
				{
					sbyte b52 = msg.reader().readByte();
					for (int num144 = 0; num144 < GameCanvas.panel.vMyGD.size(); num144++)
					{
						Item item2 = (Item)GameCanvas.panel.vMyGD.elementAt(num144);
						if (item2.indexUI == b52)
						{
							GameCanvas.panel.vMyGD.removeElement(item2);
							break;
						}
					}
				}
				if (b51 == 5)
				{
				}
				if (b51 == 6)
				{
					GameCanvas.panel.isFriendLock = true;
					if (GameCanvas.panel2 != null)
					{
						GameCanvas.panel2.isFriendLock = true;
					}
					GameCanvas.panel.vFriendGD.removeAllElements();
					if (GameCanvas.panel2 != null)
					{
						GameCanvas.panel2.vFriendGD.removeAllElements();
					}
					int friendMoneyGD = msg.reader().readInt();
					sbyte b53 = msg.reader().readByte();
					Res.outz("item size = " + b53);
					for (int num145 = 0; num145 < b53; num145++)
					{
						Item item3 = new Item();
						item3.template = ItemTemplates.get(msg.reader().readShort());
						item3.quantity = msg.reader().readInt();
						int num146 = msg.reader().readUnsignedByte();
						if (num146 != 0)
						{
							item3.itemOption = new ItemOption[num146];
							for (int num147 = 0; num147 < item3.itemOption.Length; num147++)
							{
								int num148 = msg.reader().readUnsignedByte();
								int param5 = msg.reader().readUnsignedShort();
								if (num148 != -1)
								{
									item3.itemOption[num147] = new ItemOption(num148, param5);
									item3.compare = GameCanvas.panel.getCompare(item3);
								}
							}
						}
						if (GameCanvas.panel2 != null)
						{
							GameCanvas.panel2.vFriendGD.addElement(item3);
						}
						else
						{
							GameCanvas.panel.vFriendGD.addElement(item3);
						}
					}
					if (GameCanvas.panel2 != null)
					{
						GameCanvas.panel2.setTabGiaoDich(isMe: false);
						GameCanvas.panel2.friendMoneyGD = friendMoneyGD;
					}
					else
					{
						GameCanvas.panel.friendMoneyGD = friendMoneyGD;
						if (GameCanvas.panel.currentTabIndex == 2)
						{
							GameCanvas.panel.setTabGiaoDich(isMe: false);
						}
					}
				}
				if (b51 == 7)
				{
					InfoDlg.hide();
					if (GameCanvas.panel.isShow)
					{
						GameCanvas.panel.hide();
					}
				}
				break;
			}
			case -85:
			{
				Res.outz("CAP CHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
				sbyte b43 = msg.reader().readByte();
				if (b43 == 0)
				{
					int num120 = msg.reader().readUnsignedShort();
					Res.outz("lent =" + num120);
					sbyte[] data4 = new sbyte[num120];
					msg.reader().read(ref data4, 0, num120);
					GameScr.imgCapcha = Image.createImage(data4, 0, num120);
					GameScr.gI().keyInput = "-----";
					GameScr.gI().strCapcha = msg.reader().readUTF();
					GameScr.gI().keyCapcha = new int[GameScr.gI().strCapcha.Length];
					GameScr.gI().mobCapcha = new Mob();
					GameScr.gI().right = null;
				}
				if (b43 == 1)
				{
					MobCapcha.isAttack = true;
				}
				if (b43 == 2)
				{
					MobCapcha.explode = true;
					GameScr.gI().right = GameScr.gI().cmdFocus;
				}
				break;
			}
			case -112:
			{
				sbyte b37 = msg.reader().readByte();
				if (b37 == 0)
				{
					sbyte mobIndex = msg.reader().readByte();
					GameScr.findMobInMap(mobIndex).clearBody();
				}
				if (b37 == 1)
				{
					sbyte mobIndex2 = msg.reader().readByte();
					GameScr.findMobInMap(mobIndex2).setBody(msg.reader().readShort());
				}
				break;
			}
			case -84:
			{
				int index4 = msg.reader().readUnsignedByte();
				Mob mob12 = null;
				try
				{
					mob12 = (Mob)GameScr.vMob.elementAt(index4);
				}
				catch (Exception)
				{
				}
				if (mob12 != null)
				{
					mob12.maxHp = msg.reader().readInt();
				}
				break;
			}
			case -83:
			{
				sbyte b9 = msg.reader().readByte();
				if (b9 == 0)
				{
					int num17 = msg.reader().readShort();
					int bgRID = msg.reader().readShort();
					int num18 = msg.reader().readUnsignedByte();
					int num19 = msg.reader().readInt();
					string text = msg.reader().readUTF();
					int xR = msg.reader().readShort();
					int yR = msg.reader().readShort();
					sbyte b10 = msg.reader().readByte();
					if (b10 == 1)
					{
						GameScr.gI().isRongNamek = true;
					}
					else
					{
						GameScr.gI().isRongNamek = false;
					}
					GameScr.gI().xR = xR;
					GameScr.gI().yR = yR;
					Res.outz("xR= " + xR + " yR= " + yR + " +++++++++++++++++++++++++++++++++++++++");
					if (Char.myCharz().charID == num19)
					{
						GameCanvas.panel.hideNow();
						GameScr.gI().activeRongThanEff(isMe: true);
					}
					else if (TileMap.mapID == num17 && TileMap.zoneID == num18)
					{
						GameScr.gI().activeRongThanEff(isMe: false);
					}
					else if (mGraphics.zoomLevel > 1)
					{
						GameScr.gI().doiMauTroi();
					}
					GameScr.gI().mapRID = num17;
					GameScr.gI().bgRID = bgRID;
					GameScr.gI().zoneRID = num18;
				}
				if (b9 == 1)
				{
					Res.outz("map RID = " + GameScr.gI().mapRID + " zone RID= " + GameScr.gI().zoneRID);
					Res.outz("map ID = " + TileMap.mapID + " zone ID= " + TileMap.zoneID);
					if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
					{
						GameScr.gI().hideRongThanEff();
					}
					else
					{
						GameScr.gI().isRongThanXuatHien = false;
						if (GameScr.gI().isRongNamek)
						{
							GameScr.gI().isRongNamek = false;
						}
					}
				}
				if (b9 == 2)
				{
				}
				break;
			}
			case -82:
			{
				sbyte b33 = msg.reader().readByte();
				TileMap.tileIndex = new int[b33][][];
				TileMap.tileType = new int[b33][];
				for (int num101 = 0; num101 < b33; num101++)
				{
					sbyte b34 = msg.reader().readByte();
					TileMap.tileType[num101] = new int[b34];
					TileMap.tileIndex[num101] = new int[b34][];
					for (int num102 = 0; num102 < b34; num102++)
					{
						TileMap.tileType[num101][num102] = msg.reader().readInt();
						sbyte b35 = msg.reader().readByte();
						TileMap.tileIndex[num101][num102] = new int[b35];
						for (int num103 = 0; num103 < b35; num103++)
						{
							TileMap.tileIndex[num101][num102][num103] = msg.reader().readByte();
						}
					}
				}
				break;
			}
			case -81:
			{
				sbyte b11 = msg.reader().readByte();
				if (b11 == 0)
				{
					string src = msg.reader().readUTF();
					string src2 = msg.reader().readUTF();
					GameCanvas.panel.setTypeCombine();
					GameCanvas.panel.combineInfo = mFont.tahoma_7b_blue.splitFontArray(src, Panel.WIDTH_PANEL);
					GameCanvas.panel.combineTopInfo = mFont.tahoma_7.splitFontArray(src2, Panel.WIDTH_PANEL);
					GameCanvas.panel.show();
				}
				if (b11 == 1)
				{
					GameCanvas.panel.vItemCombine.removeAllElements();
					sbyte b12 = msg.reader().readByte();
					for (int n = 0; n < b12; n++)
					{
						sbyte b13 = msg.reader().readByte();
						for (int num20 = 0; num20 < Char.myCharz().arrItemBag.Length; num20++)
						{
							Item item = Char.myCharz().arrItemBag[num20];
							if (item != null && item.indexUI == b13)
							{
								item.isSelect = true;
								GameCanvas.panel.vItemCombine.addElement(item);
							}
						}
					}
					if (GameCanvas.panel.isShow)
					{
						GameCanvas.panel.setTabCombine();
					}
				}
				if (b11 == 2)
				{
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(0);
				}
				if (b11 == 3)
				{
					GameCanvas.panel.combineSuccess = 1;
					GameCanvas.panel.setCombineEff(0);
				}
				if (b11 == 4)
				{
					short iconID = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(1);
				}
				if (b11 == 5)
				{
					short iconID2 = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID2;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(2);
				}
				if (b11 == 6)
				{
					short iconID3 = msg.reader().readShort();
					short iconID4 = msg.reader().readShort();
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(3);
					GameCanvas.panel.iconID1 = iconID3;
					GameCanvas.panel.iconID3 = iconID4;
				}
				if (b11 == 7)
				{
					short iconID5 = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID5;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(4);
				}
				if (b11 == 8)
				{
					GameCanvas.panel.iconID3 = -1;
					GameCanvas.panel.combineSuccess = 1;
					GameCanvas.panel.setCombineEff(4);
				}
				short num21 = 21;
				try
				{
					num21 = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				for (int num22 = 0; num22 < GameScr.vNpc.size(); num22++)
				{
					Npc npc = (Npc)GameScr.vNpc.elementAt(num22);
					if (npc.template.npcTemplateId == num21)
					{
						GameCanvas.panel.xS = npc.cx - GameScr.cmx;
						GameCanvas.panel.yS = npc.cy - GameScr.cmy;
						GameCanvas.panel.idNPC = num21;
						break;
					}
				}
				break;
			}
			case -80:
			{
				sbyte b42 = msg.reader().readByte();
				InfoDlg.hide();
				if (b42 == 0)
				{
					GameCanvas.panel.vFriend.removeAllElements();
					int num113 = msg.reader().readUnsignedByte();
					for (int num114 = 0; num114 < num113; num114++)
					{
						Char char8 = new Char();
						char8.charID = msg.reader().readInt();
						char8.head = msg.reader().readShort();
						char8.headICON = msg.reader().readShort();
						char8.body = msg.reader().readShort();
						char8.leg = msg.reader().readShort();
						char8.bag = msg.reader().readUnsignedByte();
						char8.cName = msg.reader().readUTF();
						bool isOnline2 = msg.reader().readBoolean();
						InfoItem infoItem2 = new InfoItem(mResources.power + ": " + msg.reader().readUTF());
						infoItem2.charInfo = char8;
						infoItem2.isOnline = isOnline2;
						GameCanvas.panel.vFriend.addElement(infoItem2);
					}
					GameCanvas.panel.setTypeFriend();
					GameCanvas.panel.show();
				}
				if (b42 == 3)
				{
					MyVector vFriend = GameCanvas.panel.vFriend;
					int num115 = msg.reader().readInt();
					Res.outz("online offline id=" + num115);
					for (int num116 = 0; num116 < vFriend.size(); num116++)
					{
						InfoItem infoItem3 = (InfoItem)vFriend.elementAt(num116);
						if (infoItem3.charInfo != null && infoItem3.charInfo.charID == num115)
						{
							Res.outz("online= " + infoItem3.isOnline);
							infoItem3.isOnline = msg.reader().readBoolean();
							break;
						}
					}
				}
				if (b42 != 2)
				{
					break;
				}
				MyVector vFriend2 = GameCanvas.panel.vFriend;
				int num117 = msg.reader().readInt();
				for (int num118 = 0; num118 < vFriend2.size(); num118++)
				{
					InfoItem infoItem4 = (InfoItem)vFriend2.elementAt(num118);
					if (infoItem4.charInfo != null && infoItem4.charInfo.charID == num117)
					{
						vFriend2.removeElement(infoItem4);
						break;
					}
				}
				if (GameCanvas.panel.isShow)
				{
					GameCanvas.panel.setTabFriend();
				}
				break;
			}
			case -99:
				InfoDlg.hide();
				if (msg.reader().readByte() == 0)
				{
					GameCanvas.panel.vEnemy.removeAllElements();
					int num107 = msg.reader().readUnsignedByte();
					for (int num108 = 0; num108 < num107; num108++)
					{
						Char char7 = new Char();
						char7.charID = msg.reader().readInt();
						char7.head = msg.reader().readShort();
						char7.headICON = msg.reader().readShort();
						char7.body = msg.reader().readShort();
						char7.leg = msg.reader().readShort();
						char7.bag = msg.reader().readShort();
						char7.cName = msg.reader().readUTF();
						InfoItem infoItem = new InfoItem(msg.reader().readUTF());
						bool isOnline = msg.reader().readBoolean();
						infoItem.charInfo = char7;
						infoItem.isOnline = isOnline;
						Res.outz("isonline = " + isOnline);
						GameCanvas.panel.vEnemy.addElement(infoItem);
					}
					GameCanvas.panel.setTypeEnemy();
					GameCanvas.panel.show();
				}
				break;
			case -79:
			{
				InfoDlg.hide();
				int num34 = msg.reader().readInt();
				Char charMenu = GameCanvas.panel.charMenu;
				if (charMenu == null)
				{
					return;
				}
				charMenu.cPower = msg.reader().readLong();
				charMenu.currStrLevel = msg.reader().readUTF();
				break;
			}
			case -93:
			{
				short num67 = msg.reader().readShort();
				BgItem.newSmallVersion = new sbyte[num67];
				for (int num68 = 0; num68 < num67; num68++)
				{
					BgItem.newSmallVersion[num68] = msg.reader().readByte();
				}
				break;
			}
			case -77:
			{
				short num13 = msg.reader().readShort();
				SmallImage.newSmallVersion = new sbyte[num13];
				SmallImage.maxSmall = num13;
				SmallImage.imgNew = new Small[num13];
				for (int k = 0; k < num13; k++)
				{
					SmallImage.newSmallVersion[k] = msg.reader().readByte();
				}
				break;
			}
			case -76:
				switch (msg.reader().readByte())
				{
				case 0:
				{
					sbyte b63 = msg.reader().readByte();
					if (b63 <= 0)
					{
						return;
					}
					Char.myCharz().arrArchive = new Archivement[b63];
					for (int num170 = 0; num170 < b63; num170++)
					{
						Char.myCharz().arrArchive[num170] = new Archivement();
						Char.myCharz().arrArchive[num170].info1 = num170 + 1 + ". " + msg.reader().readUTF();
						Char.myCharz().arrArchive[num170].info2 = msg.reader().readUTF();
						Char.myCharz().arrArchive[num170].money = msg.reader().readShort();
						Char.myCharz().arrArchive[num170].isFinish = msg.reader().readBoolean();
						Char.myCharz().arrArchive[num170].isRecieve = msg.reader().readBoolean();
					}
					GameCanvas.panel.setTypeArchivement();
					GameCanvas.panel.show();
					break;
				}
				case 1:
				{
					int num169 = msg.reader().readUnsignedByte();
					if (Char.myCharz().arrArchive[num169] != null)
					{
						Char.myCharz().arrArchive[num169].isRecieve = true;
					}
					break;
				}
				}
				break;
			case -74:
			{
				if (ServerListScreen.stopDownload)
				{
					return;
				}
				if (!GameCanvas.isGetResourceFromServer())
				{
					Service.gI().getResource(3, null);
					SmallImage.loadBigRMS();
					SplashScr.imgLogo = null;
					if (Rms.loadRMSString("acc") != null || Rms.loadRMSString("userAo" + ServerListScreen.ipSelect) != null)
					{
						LoginScr.isContinueToLogin = true;
					}
					GameCanvas.loginScr = new LoginScr();
					GameCanvas.loginScr.switchToMe();
					return;
				}
				bool flag5 = true;
				sbyte b25 = msg.reader().readByte();
				Res.outz("action = " + b25);
				if (b25 == 0)
				{
					int num57 = msg.reader().readInt();
					string text3 = Rms.loadRMSString("ResVersion");
					int num58 = ((text3 == null || !(text3 != string.Empty)) ? (-1) : int.Parse(text3));
					if (num58 == -1 || num58 != num57)
					{
						ServerListScreen.loadScreen = false;
						GameCanvas.serverScreen.show2();
					}
					else
					{
						Res.outz("login ngay");
						SmallImage.loadBigRMS();
						SplashScr.imgLogo = null;
						ServerListScreen.loadScreen = true;
						if (GameCanvas.currentScreen != GameCanvas.loginScr)
						{
							GameCanvas.serverScreen.switchToMe();
						}
					}
				}
				if (b25 == 1)
				{
					ServerListScreen.strWait = mResources.downloading_data;
					short num59 = (short)(ServerListScreen.nBig = msg.reader().readShort());
					Service.gI().getResource(2, null);
				}
				if (b25 == 2)
				{
					try
					{
						isLoadingData = true;
						GameCanvas.endDlg();
						ServerListScreen.demPercent++;
						ServerListScreen.percent = ServerListScreen.demPercent * 100 / ServerListScreen.nBig;
						string original = msg.reader().readUTF();
						string[] array5 = Res.split(original, "/", 0);
						string filename = "x" + mGraphics.zoomLevel + array5[array5.Length - 1];
						int num60 = msg.reader().readInt();
						sbyte[] data3 = new sbyte[num60];
						msg.reader().read(ref data3, 0, num60);
						Rms.saveRMS(filename, data3);
					}
					catch (Exception)
					{
						GameCanvas.startOK(mResources.pls_restart_game_error, 8885, null);
					}
				}
				if (b25 == 3 && flag5)
				{
					isLoadingData = false;
					int num61 = msg.reader().readInt();
					Res.outz("last version= " + num61);
					Rms.saveRMSString("ResVersion", num61 + string.Empty);
					Service.gI().getResource(3, null);
					GameCanvas.endDlg();
					SplashScr.imgLogo = null;
					SmallImage.loadBigRMS();
					mSystem.gcc();
					ServerListScreen.bigOk = true;
					ServerListScreen.loadScreen = true;
					GameScr.gI().loadGameScr();
					if (GameCanvas.currentScreen != GameCanvas.loginScr)
					{
						GameCanvas.serverScreen.switchToMe();
					}
				}
				break;
			}
			case -43:
			{
				sbyte itemAction = msg.reader().readByte();
				sbyte where = msg.reader().readByte();
				sbyte index = msg.reader().readByte();
				string info2 = msg.reader().readUTF();
				GameCanvas.panel.itemRequest(itemAction, info2, where, index);
				break;
			}
			case -59:
			{
				sbyte typePK = msg.reader().readByte();
				GameScr.gI().player_vs_player(msg.reader().readInt(), msg.reader().readInt(), msg.reader().readUTF(), typePK);
				break;
			}
			case -62:
			{
				int num65 = msg.reader().readUnsignedByte();
				sbyte b27 = msg.reader().readByte();
				if (b27 <= 0)
				{
					break;
				}
				ClanImage clanImage2 = ClanImage.getClanImage((short)num65);
				if (clanImage2 == null)
				{
					break;
				}
				clanImage2.idImage = new short[b27];
				for (int num66 = 0; num66 < b27; num66++)
				{
					clanImage2.idImage[num66] = msg.reader().readShort();
					if (clanImage2.idImage[num66] > 0)
					{
						SmallImage.vKeys.addElement(clanImage2.idImage[num66] + string.Empty);
					}
				}
				break;
			}
			case -65:
			{
				Res.outz("TELEPORT ...................................................");
				InfoDlg.hide();
				int num104 = msg.reader().readInt();
				sbyte b36 = msg.reader().readByte();
				if (b36 == 0)
				{
					break;
				}
				if (Char.myCharz().charID == num104)
				{
					isStopReadMessage = true;
					GameScr.lockTick = 500;
					GameScr.gI().center = null;
					if (b36 == 0 || b36 == 1 || b36 == 3)
					{
						Teleport p = new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 0, isMe: true, (b36 != 1) ? b36 : Char.myCharz().cgender);
						Teleport.addTeleport(p);
					}
					if (b36 == 2)
					{
						GameScr.lockTick = 50;
						Char.myCharz().hide();
					}
				}
				else
				{
					Char char5 = GameScr.findCharInMap(num104);
					if ((b36 == 0 || b36 == 1 || b36 == 3) && char5 != null)
					{
						char5.isUsePlane = true;
						Teleport teleport = new Teleport(char5.cx, char5.cy, char5.head, char5.cdir, 0, isMe: false, (b36 != 1) ? b36 : char5.cgender);
						teleport.id = num104;
						Teleport.addTeleport(teleport);
					}
					if (b36 == 2)
					{
						char5.hide();
					}
				}
				break;
			}
			case -64:
			{
				int num31 = msg.reader().readInt();
				int bag = msg.reader().readUnsignedByte();
				@char = null;
				@char = ((num31 != Char.myCharz().charID) ? GameScr.findCharInMap(num31) : Char.myCharz());
				@char.bag = bag;
				if (@char.bag >= 201 && @char.bag < 255)
				{
					Effect effect = new Effect(@char.bag, @char, 2, -1, 10, 1);
					effect.typeEff = 5;
					@char.addEffChar(effect);
				}
				else
				{
					for (int num32 = 0; num32 < 54; num32++)
					{
						@char.removeEffChar(0, 201 + num32);
					}
				}
				Res.outz("cmd:-64 UPDATE BAG PLAER = " + ((@char != null) ? @char.cName : string.Empty) + num31 + " BAG ID= " + bag);
				break;
			}
			case -63:
			{
				Res.outz("GET BAG");
				int iD = msg.reader().readUnsignedByte();
				sbyte b7 = msg.reader().readByte();
				ClanImage clanImage = new ClanImage();
				clanImage.ID = iD;
				if (b7 > 0)
				{
					clanImage.idImage = new short[b7];
					for (int j = 0; j < b7; j++)
					{
						clanImage.idImage[j] = msg.reader().readShort();
						Res.outz("ID=  " + iD + " frame= " + clanImage.idImage[j]);
					}
					ClanImage.idImages.put(iD + string.Empty, clanImage);
				}
				break;
			}
			case -57:
			{
				string strInvite = msg.reader().readUTF();
				int clanID = msg.reader().readInt();
				int code = msg.reader().readInt();
				GameScr.gI().clanInvite(strInvite, clanID, code);
				break;
			}
			case -51:
				InfoDlg.hide();
				readClanMsg(msg, 0);
				if (GameCanvas.panel.isMessage && GameCanvas.panel.type == 5)
				{
					GameCanvas.panel.initTabClans();
				}
				break;
			case -53:
			{
				InfoDlg.hide();
				bool flag7 = false;
				int num97 = msg.reader().readInt();
				Res.outz("clanId= " + num97);
				if (num97 == -1)
				{
					flag7 = true;
					Char.myCharz().clan = null;
					ClanMessage.vMessage.removeAllElements();
					if (GameCanvas.panel.member != null)
					{
						GameCanvas.panel.member.removeAllElements();
					}
					if (GameCanvas.panel.myMember != null)
					{
						GameCanvas.panel.myMember.removeAllElements();
					}
					if (GameCanvas.currentScreen == GameScr.gI())
					{
						GameCanvas.panel.setTabClans();
					}
					return;
				}
				GameCanvas.panel.tabIcon = null;
				if (Char.myCharz().clan == null)
				{
					Char.myCharz().clan = new Clan();
				}
				Char.myCharz().clan.ID = num97;
				Char.myCharz().clan.name = msg.reader().readUTF();
				Char.myCharz().clan.slogan = msg.reader().readUTF();
				Char.myCharz().clan.imgID = msg.reader().readUnsignedByte();
				Char.myCharz().clan.powerPoint = msg.reader().readUTF();
				Char.myCharz().clan.leaderName = msg.reader().readUTF();
				Char.myCharz().clan.currMember = msg.reader().readUnsignedByte();
				Char.myCharz().clan.maxMember = msg.reader().readUnsignedByte();
				Char.myCharz().role = msg.reader().readByte();
				Char.myCharz().clan.clanPoint = msg.reader().readInt();
				Char.myCharz().clan.level = msg.reader().readByte();
				GameCanvas.panel.myMember = new MyVector();
				for (int num98 = 0; num98 < Char.myCharz().clan.currMember; num98++)
				{
					Member member5 = new Member();
					member5.ID = msg.reader().readInt();
					member5.head = msg.reader().readShort();
					member5.headICON = msg.reader().readShort();
					member5.leg = msg.reader().readShort();
					member5.body = msg.reader().readShort();
					member5.name = msg.reader().readUTF();
					member5.role = msg.reader().readByte();
					member5.powerPoint = msg.reader().readUTF();
					member5.donate = msg.reader().readInt();
					member5.receive_donate = msg.reader().readInt();
					member5.clanPoint = msg.reader().readInt();
					member5.curClanPoint = msg.reader().readInt();
					member5.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					GameCanvas.panel.myMember.addElement(member5);
				}
				int num99 = msg.reader().readUnsignedByte();
				for (int num100 = 0; num100 < num99; num100++)
				{
					readClanMsg(msg, -1);
				}
				if (GameCanvas.panel.isSearchClan || GameCanvas.panel.isViewMember || GameCanvas.panel.isMessage)
				{
					GameCanvas.panel.setTabClans();
				}
				if (flag7)
				{
					GameCanvas.panel.setTabClans();
				}
				Res.outz("=>>>>>>>>>>>>>>>>>>>>>> -537 MY CLAN INFO");
				break;
			}
			case -52:
			{
				sbyte b19 = msg.reader().readByte();
				if (b19 == 0)
				{
					Member member2 = new Member();
					member2.ID = msg.reader().readInt();
					member2.head = msg.reader().readShort();
					member2.headICON = msg.reader().readShort();
					member2.leg = msg.reader().readShort();
					member2.body = msg.reader().readShort();
					member2.name = msg.reader().readUTF();
					member2.role = msg.reader().readByte();
					member2.powerPoint = msg.reader().readUTF();
					member2.donate = msg.reader().readInt();
					member2.receive_donate = msg.reader().readInt();
					member2.clanPoint = msg.reader().readInt();
					member2.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					if (GameCanvas.panel.myMember == null)
					{
						GameCanvas.panel.myMember = new MyVector();
					}
					GameCanvas.panel.myMember.addElement(member2);
					GameCanvas.panel.initTabClans();
				}
				if (b19 == 1)
				{
					GameCanvas.panel.myMember.removeElementAt(msg.reader().readByte());
					Panel panel = GameCanvas.panel;
					panel.currentListLength--;
					GameCanvas.panel.initTabClans();
				}
				if (b19 == 2)
				{
					Member member3 = new Member();
					member3.ID = msg.reader().readInt();
					member3.head = msg.reader().readShort();
					member3.headICON = msg.reader().readShort();
					member3.leg = msg.reader().readShort();
					member3.body = msg.reader().readShort();
					member3.name = msg.reader().readUTF();
					member3.role = msg.reader().readByte();
					member3.powerPoint = msg.reader().readUTF();
					member3.donate = msg.reader().readInt();
					member3.receive_donate = msg.reader().readInt();
					member3.clanPoint = msg.reader().readInt();
					member3.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					for (int num35 = 0; num35 < GameCanvas.panel.myMember.size(); num35++)
					{
						Member member4 = (Member)GameCanvas.panel.myMember.elementAt(num35);
						if (member4.ID == member3.ID)
						{
							if (Char.myCharz().charID == member3.ID)
							{
								Char.myCharz().role = member3.role;
							}
							Member o = member3;
							GameCanvas.panel.myMember.removeElement(member4);
							GameCanvas.panel.myMember.insertElementAt(o, num35);
							return;
						}
					}
				}
				Res.outz("=>>>>>>>>>>>>>>>>>>>>>> -52  MY CLAN UPDSTE");
				break;
			}
			case -50:
			{
				InfoDlg.hide();
				GameCanvas.panel.member = new MyVector();
				sbyte b18 = msg.reader().readByte();
				for (int num33 = 0; num33 < b18; num33++)
				{
					Member member = new Member();
					member.ID = msg.reader().readInt();
					member.head = msg.reader().readShort();
					member.headICON = msg.reader().readShort();
					member.leg = msg.reader().readShort();
					member.body = msg.reader().readShort();
					member.name = msg.reader().readUTF();
					member.role = msg.reader().readByte();
					member.powerPoint = msg.reader().readUTF();
					member.donate = msg.reader().readInt();
					member.receive_donate = msg.reader().readInt();
					member.clanPoint = msg.reader().readInt();
					member.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					GameCanvas.panel.member.addElement(member);
				}
				GameCanvas.panel.isViewMember = true;
				GameCanvas.panel.isSearchClan = false;
				GameCanvas.panel.isMessage = false;
				GameCanvas.panel.currentListLength = GameCanvas.panel.member.size() + 2;
				GameCanvas.panel.initTabClans();
				break;
			}
			case -47:
			{
				InfoDlg.hide();
				sbyte b8 = msg.reader().readByte();
				Res.outz("clan = " + b8);
				if (b8 == 0)
				{
					GameCanvas.panel.clanReport = mResources.cannot_find_clan;
					GameCanvas.panel.clans = null;
				}
				else
				{
					GameCanvas.panel.clans = new Clan[b8];
					Res.outz("clan search lent= " + GameCanvas.panel.clans.Length);
					for (int m = 0; m < GameCanvas.panel.clans.Length; m++)
					{
						GameCanvas.panel.clans[m] = new Clan();
						GameCanvas.panel.clans[m].ID = msg.reader().readInt();
						GameCanvas.panel.clans[m].name = msg.reader().readUTF();
						GameCanvas.panel.clans[m].slogan = msg.reader().readUTF();
						GameCanvas.panel.clans[m].imgID = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[m].powerPoint = msg.reader().readUTF();
						GameCanvas.panel.clans[m].leaderName = msg.reader().readUTF();
						GameCanvas.panel.clans[m].currMember = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[m].maxMember = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[m].date = msg.reader().readInt();
					}
				}
				GameCanvas.panel.isSearchClan = true;
				GameCanvas.panel.isViewMember = false;
				GameCanvas.panel.isMessage = false;
				if (GameCanvas.panel.isSearchClan)
				{
					GameCanvas.panel.initTabClans();
				}
				break;
			}
			case -46:
			{
				InfoDlg.hide();
				sbyte b62 = msg.reader().readByte();
				if (b62 == 1 || b62 == 3)
				{
					GameCanvas.endDlg();
					ClanImage.vClanImage.removeAllElements();
					int num164 = msg.reader().readUnsignedByte();
					for (int num165 = 0; num165 < num164; num165++)
					{
						ClanImage clanImage3 = new ClanImage();
						clanImage3.ID = msg.reader().readUnsignedByte();
						clanImage3.name = msg.reader().readUTF();
						clanImage3.xu = msg.reader().readInt();
						clanImage3.luong = msg.reader().readInt();
						if (!ClanImage.isExistClanImage(clanImage3.ID))
						{
							ClanImage.addClanImage(clanImage3);
							continue;
						}
						ClanImage.getClanImage((short)clanImage3.ID).name = clanImage3.name;
						ClanImage.getClanImage((short)clanImage3.ID).xu = clanImage3.xu;
						ClanImage.getClanImage((short)clanImage3.ID).luong = clanImage3.luong;
					}
					if (Char.myCharz().clan != null)
					{
						GameCanvas.panel.changeIcon();
					}
				}
				if (b62 == 4)
				{
					Char.myCharz().clan.imgID = msg.reader().readUnsignedByte();
					Char.myCharz().clan.slogan = msg.reader().readUTF();
				}
				break;
			}
			case -61:
			{
				int num132 = msg.reader().readInt();
				if (num132 != Char.myCharz().charID)
				{
					if (GameScr.findCharInMap(num132) != null)
					{
						GameScr.findCharInMap(num132).clanID = msg.reader().readInt();
						if (GameScr.findCharInMap(num132).clanID == -2)
						{
							GameScr.findCharInMap(num132).isCopy = true;
						}
					}
				}
				else if (Char.myCharz().clan != null)
				{
					Char.myCharz().clan.ID = msg.reader().readInt();
				}
				break;
			}
			case -42:
				Char.myCharz().cHPGoc = msg.readInt3Byte();
				Char.myCharz().cMPGoc = msg.readInt3Byte();
				Char.myCharz().cDamGoc = msg.reader().readInt();
				Char.myCharz().cHPFull = msg.readInt3Byte();
				Char.myCharz().cMPFull = msg.readInt3Byte();
				Char.myCharz().cHP = msg.readInt3Byte();
				Char.myCharz().cMP = msg.readInt3Byte();
				Char.myCharz().cspeed = msg.reader().readByte();
				Char.myCharz().hpFrom1000TiemNang = msg.reader().readByte();
				Char.myCharz().mpFrom1000TiemNang = msg.reader().readByte();
				Char.myCharz().damFrom1000TiemNang = msg.reader().readByte();
				Char.myCharz().cDamFull = msg.reader().readInt();
				Char.myCharz().cDefull = msg.reader().readInt();
				Char.myCharz().cCriticalFull = msg.reader().readByte();
				Char.myCharz().cTiemNang = msg.reader().readLong();
				Char.myCharz().expForOneAdd = msg.reader().readShort();
				Char.myCharz().cDefGoc = msg.reader().readShort();
				Char.myCharz().cCriticalGoc = msg.reader().readByte();
				InfoDlg.hide();
				break;
			case 1:
			{
				bool flag8 = msg.reader().readBool();
				Res.outz("isRes= " + flag8);
				if (!flag8)
				{
					GameCanvas.startOKDlg(msg.reader().readUTF());
					break;
				}
				GameCanvas.loginScr.isLogin2 = false;
				Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, string.Empty);
				GameCanvas.endDlg();
				GameCanvas.loginScr.doLogin();
				break;
			}
			case 2:
				Char.isLoadingMap = true;
				LoginScr.isLoggingIn = false;
				if (!GameScr.isLoadAllData)
				{
					GameScr.gI().initSelectChar();
				}
				BgItem.clearHashTable();
				GameCanvas.endDlg();
				CreateCharScr.isCreateChar = true;
				CreateCharScr.gI().switchToMe();
				break;
			case -107:
			{
				sbyte b24 = msg.reader().readByte();
				if (b24 == 0)
				{
					Char.myCharz().havePet = false;
				}
				if (b24 == 1)
				{
					Char.myCharz().havePet = true;
				}
				if (b24 != 2)
				{
					break;
				}
				InfoDlg.hide();
				Char.myPetz().head = msg.reader().readShort();
				Char.myPetz().setDefaultPart();
				int num49 = msg.reader().readUnsignedByte();
				Res.outz("num body = " + num49);
				Char.myPetz().arrItemBody = new Item[num49];
				for (int num50 = 0; num50 < num49; num50++)
				{
					short num51 = msg.reader().readShort();
					Res.outz("template id= " + num51);
					if (num51 == -1)
					{
						continue;
					}
					Res.outz("1");
					Char.myPetz().arrItemBody[num50] = new Item();
					Char.myPetz().arrItemBody[num50].template = ItemTemplates.get(num51);
					int type2 = Char.myPetz().arrItemBody[num50].template.type;
					Char.myPetz().arrItemBody[num50].quantity = msg.reader().readInt();
					Res.outz("3");
					Char.myPetz().arrItemBody[num50].info = msg.reader().readUTF();
					Char.myPetz().arrItemBody[num50].content = msg.reader().readUTF();
					int num52 = msg.reader().readUnsignedByte();
					Res.outz("option size= " + num52);
					if (num52 != 0)
					{
						Char.myPetz().arrItemBody[num50].itemOption = new ItemOption[num52];
						for (int num53 = 0; num53 < Char.myPetz().arrItemBody[num50].itemOption.Length; num53++)
						{
							int num54 = msg.reader().readUnsignedByte();
							int param3 = msg.reader().readUnsignedShort();
							if (num54 != -1)
							{
								Char.myPetz().arrItemBody[num50].itemOption[num53] = new ItemOption(num54, param3);
							}
						}
					}
					switch (type2)
					{
					case 0:
						Char.myPetz().body = Char.myPetz().arrItemBody[num50].template.part;
						break;
					case 1:
						Char.myPetz().leg = Char.myPetz().arrItemBody[num50].template.part;
						break;
					}
				}
				Char.myPetz().cHP = msg.readInt3Byte();
				Char.myPetz().cHPFull = msg.readInt3Byte();
				Char.myPetz().cMP = msg.readInt3Byte();
				Char.myPetz().cMPFull = msg.readInt3Byte();
				Char.myPetz().cDamFull = msg.readInt3Byte();
				Char.myPetz().cName = msg.reader().readUTF();
				Char.myPetz().currStrLevel = msg.reader().readUTF();
				Char.myPetz().cPower = msg.reader().readLong();
				Char.myPetz().cTiemNang = msg.reader().readLong();
				Char.myPetz().petStatus = msg.reader().readByte();
				Char.myPetz().cStamina = msg.reader().readShort();
				Char.myPetz().cMaxStamina = msg.reader().readShort();
				Char.myPetz().cCriticalFull = msg.reader().readByte();
				Char.myPetz().cDefull = msg.reader().readShort();
				Char.myPetz().arrPetSkill = new Skill[msg.reader().readByte()];
				Res.outz("SKILLENT = " + Char.myPetz().arrPetSkill);
				for (int num55 = 0; num55 < Char.myPetz().arrPetSkill.Length; num55++)
				{
					short num56 = msg.reader().readShort();
					if (num56 != -1)
					{
						Char.myPetz().arrPetSkill[num55] = Skills.get(num56);
						continue;
					}
					Char.myPetz().arrPetSkill[num55] = new Skill();
					Char.myPetz().arrPetSkill[num55].template = null;
					Char.myPetz().arrPetSkill[num55].moreInfo = msg.reader().readUTF();
				}
				break;
			}
			case -37:
			{
				sbyte b20 = msg.reader().readByte();
				Res.outz("cAction= " + b20);
				if (b20 != 0)
				{
					break;
				}
				Char.myCharz().head = msg.reader().readShort();
				Char.myCharz().setDefaultPart();
				int num36 = msg.reader().readUnsignedByte();
				Res.outz("num body = " + num36);
				Char.myCharz().arrItemBody = new Item[num36];
				for (int num37 = 0; num37 < num36; num37++)
				{
					short num38 = msg.reader().readShort();
					if (num38 == -1)
					{
						continue;
					}
					Char.myCharz().arrItemBody[num37] = new Item();
					Char.myCharz().arrItemBody[num37].template = ItemTemplates.get(num38);
					int type = Char.myCharz().arrItemBody[num37].template.type;
					Char.myCharz().arrItemBody[num37].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBody[num37].info = msg.reader().readUTF();
					Char.myCharz().arrItemBody[num37].content = msg.reader().readUTF();
					int num39 = msg.reader().readUnsignedByte();
					if (num39 != 0)
					{
						Char.myCharz().arrItemBody[num37].itemOption = new ItemOption[num39];
						for (int num40 = 0; num40 < Char.myCharz().arrItemBody[num37].itemOption.Length; num40++)
						{
							int num41 = msg.reader().readUnsignedByte();
							int param = msg.reader().readUnsignedShort();
							if (num41 != -1)
							{
								Char.myCharz().arrItemBody[num37].itemOption[num40] = new ItemOption(num41, param);
							}
						}
					}
					switch (type)
					{
					case 0:
						Char.myCharz().body = Char.myCharz().arrItemBody[num37].template.part;
						break;
					case 1:
						Char.myCharz().leg = Char.myCharz().arrItemBody[num37].template.part;
						break;
					}
				}
				break;
			}
			case -36:
			{
				sbyte b21 = msg.reader().readByte();
				Res.outz("cAction= " + b21);
				if (b21 == 0)
				{
					int num42 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemBag = new Item[num42];
					GameScr.hpPotion = 0;
					Res.outz("numC=" + num42);
					for (int num43 = 0; num43 < num42; num43++)
					{
						short num44 = msg.reader().readShort();
						if (num44 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemBag[num43] = new Item();
						Char.myCharz().arrItemBag[num43].template = ItemTemplates.get(num44);
						Char.myCharz().arrItemBag[num43].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBag[num43].info = msg.reader().readUTF();
						Char.myCharz().arrItemBag[num43].content = msg.reader().readUTF();
						Char.myCharz().arrItemBag[num43].indexUI = num43;
						int num45 = msg.reader().readUnsignedByte();
						if (num45 != 0)
						{
							Char.myCharz().arrItemBag[num43].itemOption = new ItemOption[num45];
							for (int num46 = 0; num46 < Char.myCharz().arrItemBag[num43].itemOption.Length; num46++)
							{
								int num47 = msg.reader().readUnsignedByte();
								int param2 = msg.reader().readUnsignedShort();
								if (num47 != -1)
								{
									Char.myCharz().arrItemBag[num43].itemOption[num46] = new ItemOption(num47, param2);
								}
							}
							Char.myCharz().arrItemBag[num43].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemBag[num43]);
						}
						if (Char.myCharz().arrItemBag[num43].template.type == 11)
						{
						}
						if (Char.myCharz().arrItemBag[num43].template.type == 6)
						{
							GameScr.hpPotion += Char.myCharz().arrItemBag[num43].quantity;
						}
					}
				}
				if (b21 == 2)
				{
					sbyte b22 = msg.reader().readByte();
					int quantity = msg.reader().readInt();
					int quantity2 = Char.myCharz().arrItemBag[b22].quantity;
					Char.myCharz().arrItemBag[b22].quantity = quantity;
					if (Char.myCharz().arrItemBag[b22].quantity < quantity2 && Char.myCharz().arrItemBag[b22].template.type == 6)
					{
						GameScr.hpPotion -= quantity2 - Char.myCharz().arrItemBag[b22].quantity;
					}
					if (Char.myCharz().arrItemBag[b22].quantity == 0)
					{
						Char.myCharz().arrItemBag[b22] = null;
					}
				}
				break;
			}
			case -35:
			{
				sbyte b59 = msg.reader().readByte();
				Res.outz("cAction= " + b59);
				if (b59 == 0)
				{
					int num158 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemBox = new Item[num158];
					GameCanvas.panel.hasUse = 0;
					for (int num159 = 0; num159 < num158; num159++)
					{
						short num160 = msg.reader().readShort();
						if (num160 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemBox[num159] = new Item();
						Char.myCharz().arrItemBox[num159].template = ItemTemplates.get(num160);
						Char.myCharz().arrItemBox[num159].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBox[num159].info = msg.reader().readUTF();
						Char.myCharz().arrItemBox[num159].content = msg.reader().readUTF();
						int num161 = msg.reader().readUnsignedByte();
						if (num161 != 0)
						{
							Char.myCharz().arrItemBox[num159].itemOption = new ItemOption[num161];
							for (int num162 = 0; num162 < Char.myCharz().arrItemBox[num159].itemOption.Length; num162++)
							{
								int num163 = msg.reader().readUnsignedByte();
								int param6 = msg.reader().readUnsignedShort();
								if (num163 != -1)
								{
									Char.myCharz().arrItemBox[num159].itemOption[num162] = new ItemOption(num163, param6);
								}
							}
						}
						Panel panel = GameCanvas.panel;
						panel.hasUse++;
					}
				}
				if (b59 == 1)
				{
					bool isBoxClan = false;
					try
					{
						sbyte b60 = msg.reader().readByte();
						if (b60 == 1)
						{
							isBoxClan = true;
						}
					}
					catch (Exception)
					{
					}
					GameCanvas.panel.setTypeBox();
					GameCanvas.panel.isBoxClan = isBoxClan;
					GameCanvas.panel.show();
				}
				if (b59 == 2)
				{
					sbyte b61 = msg.reader().readByte();
					int quantity3 = msg.reader().readInt();
					Char.myCharz().arrItemBox[b61].quantity = quantity3;
					if (Char.myCharz().arrItemBox[b61].quantity == 0)
					{
						Char.myCharz().arrItemBox[b61] = null;
					}
				}
				break;
			}
			case -45:
			{
				sbyte b46 = msg.reader().readByte();
				int num137 = msg.reader().readInt();
				short num138 = msg.reader().readShort();
				Res.outz(">.SKILL_NOT_FOCUS  skill type= " + b46 + "   player use= " + num137);
				if (b46 == 20)
				{
					sbyte typeFrame = msg.reader().readByte();
					sbyte dir = msg.reader().readByte();
					short timeGong = msg.reader().readShort();
					bool isFly = ((msg.reader().readByte() != 0) ? true : false);
					sbyte typePaint = msg.reader().readByte();
					sbyte typeItem = -1;
					try
					{
						typeItem = msg.reader().readByte();
					}
					catch (Exception)
					{
					}
					Res.outz(">.SKILL_NOT_FOCUS  skill playerDir= " + dir);
					@char = ((Char.myCharz().charID != num137) ? GameScr.findCharInMap(num137) : Char.myCharz());
					@char.SetSkillPaint_NEW(num138, isFly, typeFrame, typePaint, dir, timeGong, typeItem);
				}
				if (b46 == 21)
				{
					Point point = new Point();
					point.x = msg.reader().readShort();
					point.y = msg.reader().readShort();
					short timeDame = msg.reader().readShort();
					short rangeDame = msg.reader().readShort();
					sbyte typePaint2 = 0;
					sbyte typeItem2 = -1;
					Point[] array14 = null;
					@char = ((Char.myCharz().charID != num137) ? GameScr.findCharInMap(num137) : Char.myCharz());
					try
					{
						typePaint2 = msg.reader().readByte();
						sbyte b47 = msg.reader().readByte();
						array14 = new Point[b47];
						for (int num139 = 0; num139 < array14.Length; num139++)
						{
							array14[num139] = new Point();
							array14[num139].type = msg.reader().readByte();
							if (array14[num139].type == 0)
							{
								array14[num139].id = msg.reader().readByte();
							}
							else
							{
								array14[num139].id = msg.reader().readInt();
							}
						}
					}
					catch (Exception)
					{
					}
					try
					{
						typeItem2 = msg.reader().readByte();
					}
					catch (Exception)
					{
					}
					Res.outz(">.SKILL_NOT_FOCUS  skill targetDame= " + point.x + ":" + point.y + "    c:" + @char.cx + ":" + @char.cy + "   cdir:" + @char.cdir);
					@char.SetSkillPaint_STT(1, num138, point, timeDame, rangeDame, typePaint2, array14, typeItem2);
				}
				if (b46 == 0)
				{
					Res.outz("id use= " + num137);
					if (Char.myCharz().charID != num137)
					{
						@char = GameScr.findCharInMap(num137);
						if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
						{
							@char.setSkillPaint(GameScr.sks[num138], 0);
						}
						else
						{
							@char.setSkillPaint(GameScr.sks[num138], 1);
							@char.delayFall = 20;
						}
					}
					else
					{
						Char.myCharz().saveLoadPreviousSkill();
						Res.outz("LOAD LAST SKILL");
					}
					sbyte b48 = msg.reader().readByte();
					Res.outz("npc size= " + b48);
					for (int num140 = 0; num140 < b48; num140++)
					{
						sbyte index3 = msg.reader().readByte();
						sbyte seconds = msg.reader().readByte();
						Res.outz("index= " + index3);
						if (num138 >= 42 && num138 <= 48)
						{
							((Mob)GameScr.vMob.elementAt(index3)).isFreez = true;
							((Mob)GameScr.vMob.elementAt(index3)).seconds = seconds;
							((Mob)GameScr.vMob.elementAt(index3)).last = (((Mob)GameScr.vMob.elementAt(index3)).cur = mSystem.currentTimeMillis());
						}
					}
					sbyte b49 = msg.reader().readByte();
					for (int num141 = 0; num141 < b49; num141++)
					{
						int num142 = msg.reader().readInt();
						sbyte b50 = msg.reader().readByte();
						Res.outz("player ID= " + num142 + " my ID= " + Char.myCharz().charID);
						if (num138 < 42 || num138 > 48)
						{
							continue;
						}
						if (num142 == Char.myCharz().charID)
						{
							if (!Char.myCharz().isFlyAndCharge && !Char.myCharz().isStandAndCharge)
							{
								GameScr.gI().isFreez = true;
								Char.myCharz().isFreez = true;
								Char.myCharz().freezSeconds = b50;
								Char.myCharz().lastFreez = (Char.myCharz().currFreez = mSystem.currentTimeMillis());
								Char.myCharz().isLockMove = true;
							}
						}
						else
						{
							@char = GameScr.findCharInMap(num142);
							if (@char != null && !@char.isFlyAndCharge && !@char.isStandAndCharge)
							{
								@char.isFreez = true;
								@char.seconds = b50;
								@char.freezSeconds = b50;
								@char.lastFreez = (GameScr.findCharInMap(num142).currFreez = mSystem.currentTimeMillis());
							}
						}
					}
				}
				if (b46 == 1 && num137 != Char.myCharz().charID)
				{
					GameScr.findCharInMap(num137).isCharge = true;
				}
				if (b46 == 3)
				{
					if (num137 == Char.myCharz().charID)
					{
						Char.myCharz().isCharge = false;
						SoundMn.gI().taitaoPause();
						Char.myCharz().saveLoadPreviousSkill();
					}
					else
					{
						GameScr.findCharInMap(num137).isCharge = false;
					}
				}
				if (b46 == 4)
				{
					if (num137 == Char.myCharz().charID)
					{
						Char.myCharz().seconds = msg.reader().readShort() - 1000;
						Char.myCharz().last = mSystem.currentTimeMillis();
						Res.outz("second= " + Char.myCharz().seconds + " last= " + Char.myCharz().last);
					}
					else if (GameScr.findCharInMap(num137) != null)
					{
						switch (GameScr.findCharInMap(num137).cgender)
						{
						case 0:
							GameScr.findCharInMap(num137).useChargeSkill(isGround: false);
							break;
						case 1:
							GameScr.findCharInMap(num137).useChargeSkill(isGround: true);
							break;
						}
						GameScr.findCharInMap(num137).skillTemplateId = num138;
						GameScr.findCharInMap(num137).isUseSkillAfterCharge = true;
						GameScr.findCharInMap(num137).seconds = msg.reader().readShort();
						GameScr.findCharInMap(num137).last = mSystem.currentTimeMillis();
					}
				}
				if (b46 == 5)
				{
					if (num137 == Char.myCharz().charID)
					{
						Char.myCharz().stopUseChargeSkill();
					}
					else if (GameScr.findCharInMap(num137) != null)
					{
						GameScr.findCharInMap(num137).stopUseChargeSkill();
					}
				}
				if (b46 == 6)
				{
					if (num137 == Char.myCharz().charID)
					{
						Char.myCharz().setAutoSkillPaint(GameScr.sks[num138], 0);
					}
					else if (GameScr.findCharInMap(num137) != null)
					{
						GameScr.findCharInMap(num137).setAutoSkillPaint(GameScr.sks[num138], 0);
						SoundMn.gI().gong();
					}
				}
				if (b46 == 7)
				{
					if (num137 == Char.myCharz().charID)
					{
						Char.myCharz().seconds = msg.reader().readShort();
						Res.outz("second = " + Char.myCharz().seconds);
						Char.myCharz().last = mSystem.currentTimeMillis();
					}
					else if (GameScr.findCharInMap(num137) != null)
					{
						GameScr.findCharInMap(num137).useChargeSkill(isGround: true);
						GameScr.findCharInMap(num137).seconds = msg.reader().readShort();
						GameScr.findCharInMap(num137).last = mSystem.currentTimeMillis();
						SoundMn.gI().gong();
					}
				}
				if (b46 == 8 && num137 != Char.myCharz().charID && GameScr.findCharInMap(num137) != null)
				{
					GameScr.findCharInMap(num137).setAutoSkillPaint(GameScr.sks[num138], 0);
				}
				break;
			}
			case -44:
			{
				bool flag6 = false;
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
				{
					flag6 = true;
				}
				sbyte b30 = msg.reader().readByte();
				int num88 = msg.reader().readUnsignedByte();
				Char.myCharz().arrItemShop = new Item[num88][];
				GameCanvas.panel.shopTabName = new string[num88 + ((!flag6) ? 1 : 0)][];
				for (int num89 = 0; num89 < GameCanvas.panel.shopTabName.Length; num89++)
				{
					GameCanvas.panel.shopTabName[num89] = new string[2];
				}
				if (b30 == 2)
				{
					GameCanvas.panel.maxPageShop = new int[num88];
					GameCanvas.panel.currPageShop = new int[num88];
				}
				if (!flag6)
				{
					GameCanvas.panel.shopTabName[num88] = mResources.inventory;
				}
				for (int num90 = 0; num90 < num88; num90++)
				{
					string[] array8 = Res.split(msg.reader().readUTF(), "\n", 0);
					if (b30 == 2)
					{
						GameCanvas.panel.maxPageShop[num90] = msg.reader().readUnsignedByte();
					}
					if (array8.Length == 2)
					{
						GameCanvas.panel.shopTabName[num90] = array8;
					}
					if (array8.Length == 1)
					{
						GameCanvas.panel.shopTabName[num90][0] = array8[0];
						GameCanvas.panel.shopTabName[num90][1] = string.Empty;
					}
					int num91 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemShop[num90] = new Item[num91];
					Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
					if (b30 == 1)
					{
						Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy2;
					}
					for (int num92 = 0; num92 < num91; num92++)
					{
						short num93 = msg.reader().readShort();
						if (num93 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemShop[num90][num92] = new Item();
						Char.myCharz().arrItemShop[num90][num92].template = ItemTemplates.get(num93);
						Res.outz("name " + num90 + " = " + Char.myCharz().arrItemShop[num90][num92].template.name + " id templat= " + Char.myCharz().arrItemShop[num90][num92].template.id);
						switch (b30)
						{
						case 8:
							Char.myCharz().arrItemShop[num90][num92].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num90][num92].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num90][num92].quantity = msg.reader().readInt();
							break;
						case 4:
							Char.myCharz().arrItemShop[num90][num92].reason = msg.reader().readUTF();
							break;
						case 0:
							Char.myCharz().arrItemShop[num90][num92].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num90][num92].buyGold = msg.reader().readInt();
							break;
						case 1:
							Char.myCharz().arrItemShop[num90][num92].powerRequire = msg.reader().readLong();
							break;
						case 2:
							Char.myCharz().arrItemShop[num90][num92].itemId = msg.reader().readShort();
							Char.myCharz().arrItemShop[num90][num92].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num90][num92].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num90][num92].buyType = msg.reader().readByte();
							Char.myCharz().arrItemShop[num90][num92].quantity = msg.reader().readInt();
							Char.myCharz().arrItemShop[num90][num92].isMe = msg.reader().readByte();
							break;
						case 3:
							Char.myCharz().arrItemShop[num90][num92].isBuySpec = true;
							Char.myCharz().arrItemShop[num90][num92].iconSpec = msg.reader().readShort();
							Char.myCharz().arrItemShop[num90][num92].buySpec = msg.reader().readInt();
							break;
						}
						int num94 = msg.reader().readUnsignedByte();
						if (num94 != 0)
						{
							Char.myCharz().arrItemShop[num90][num92].itemOption = new ItemOption[num94];
							for (int num95 = 0; num95 < Char.myCharz().arrItemShop[num90][num92].itemOption.Length; num95++)
							{
								int num96 = msg.reader().readUnsignedByte();
								int param4 = msg.reader().readUnsignedShort();
								if (num96 != -1)
								{
									Char.myCharz().arrItemShop[num90][num92].itemOption[num95] = new ItemOption(num96, param4);
									Char.myCharz().arrItemShop[num90][num92].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[num90][num92]);
								}
							}
						}
						sbyte b31 = msg.reader().readByte();
						Char.myCharz().arrItemShop[num90][num92].newItem = ((b31 != 0) ? true : false);
						sbyte b32 = msg.reader().readByte();
						if (b32 == 1)
						{
							int headTemp = msg.reader().readShort();
							int bodyTemp = msg.reader().readShort();
							int legTemp = msg.reader().readShort();
							int bagTemp = msg.reader().readShort();
							Char.myCharz().arrItemShop[num90][num92].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
						}
					}
				}
				if (flag6)
				{
					if (b30 != 2)
					{
						GameCanvas.panel2 = new Panel();
						GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
						GameCanvas.panel2.setTypeBodyOnly();
						GameCanvas.panel2.show();
					}
					else
					{
						GameCanvas.panel2 = new Panel();
						GameCanvas.panel2.setTypeKiGuiOnly();
						GameCanvas.panel2.show();
					}
				}
				GameCanvas.panel.tabName[1] = GameCanvas.panel.shopTabName;
				if (b30 == 2)
				{
					string[][] array9 = GameCanvas.panel.tabName[1];
					if (flag6)
					{
						GameCanvas.panel.tabName[1] = new string[4][]
						{
							array9[0],
							array9[1],
							array9[2],
							array9[3]
						};
					}
					else
					{
						GameCanvas.panel.tabName[1] = new string[5][]
						{
							array9[0],
							array9[1],
							array9[2],
							array9[3],
							array9[4]
						};
					}
				}
				GameCanvas.panel.setTypeShop(b30);
				GameCanvas.panel.show();
				break;
			}
			case -41:
			{
				sbyte b23 = msg.reader().readByte();
				Char.myCharz().strLevel = new string[b23];
				for (int num48 = 0; num48 < b23; num48++)
				{
					string text2 = msg.reader().readUTF();
					Char.myCharz().strLevel[num48] = text2;
				}
				Res.outz("---   xong  level caption cmd : " + msg.command);
				break;
			}
			case -34:
			{
				sbyte b14 = msg.reader().readByte();
				Res.outz("act= " + b14);
				if (b14 == 0 && GameScr.gI().magicTree != null)
				{
					Res.outz("toi duoc day");
					MagicTree magicTree = GameScr.gI().magicTree;
					magicTree.id = msg.reader().readShort();
					magicTree.name = msg.reader().readUTF();
					magicTree.name = Res.changeString(magicTree.name);
					magicTree.x = msg.reader().readShort();
					magicTree.y = msg.reader().readShort();
					magicTree.level = msg.reader().readByte();
					magicTree.currPeas = msg.reader().readShort();
					magicTree.maxPeas = msg.reader().readShort();
					Res.outz("curr Peas= " + magicTree.currPeas);
					magicTree.strInfo = msg.reader().readUTF();
					magicTree.seconds = msg.reader().readInt();
					magicTree.timeToRecieve = magicTree.seconds;
					sbyte b15 = msg.reader().readByte();
					magicTree.peaPostionX = new int[b15];
					magicTree.peaPostionY = new int[b15];
					for (int num23 = 0; num23 < b15; num23++)
					{
						magicTree.peaPostionX[num23] = msg.reader().readByte();
						magicTree.peaPostionY[num23] = msg.reader().readByte();
					}
					magicTree.isUpdate = msg.reader().readBool();
					magicTree.last = (magicTree.cur = mSystem.currentTimeMillis());
					GameScr.gI().magicTree.isUpdateTree = true;
				}
				if (b14 == 1)
				{
					myVector = new MyVector();
					try
					{
						while (msg.reader().available() > 0)
						{
							string caption = msg.reader().readUTF();
							myVector.addElement(new Command(caption, GameCanvas.instance, 888392, null));
						}
					}
					catch (Exception ex5)
					{
						Cout.println("Loi MAGIC_TREE " + ex5.ToString());
					}
					GameCanvas.menu.startAt(myVector, 3);
				}
				if (b14 == 2)
				{
					GameScr.gI().magicTree.remainPeas = msg.reader().readShort();
					GameScr.gI().magicTree.seconds = msg.reader().readInt();
					GameScr.gI().magicTree.last = (GameScr.gI().magicTree.cur = mSystem.currentTimeMillis());
					GameScr.gI().magicTree.isUpdateTree = true;
					GameScr.gI().magicTree.isPeasEffect = true;
				}
				break;
			}
			case 11:
			{
				GameCanvas.debug("SA9", 2);
				int num9 = msg.reader().readByte();
				sbyte b6 = msg.reader().readByte();
				if (b6 != 0)
				{
					Mob.arrMobTemplate[num9].data.readDataNewBoss(NinjaUtil.readByteArray(msg), b6);
				}
				else
				{
					Mob.arrMobTemplate[num9].data.readData(NinjaUtil.readByteArray(msg));
				}
				for (int i = 0; i < GameScr.vMob.size(); i++)
				{
					mob = (Mob)GameScr.vMob.elementAt(i);
					if (mob.templateId == num9)
					{
						mob.w = Mob.arrMobTemplate[num9].data.width;
						mob.h = Mob.arrMobTemplate[num9].data.height;
					}
				}
				sbyte[] array2 = NinjaUtil.readByteArray(msg);
				Image img = Image.createImage(array2, 0, array2.Length);
				Mob.arrMobTemplate[num9].data.img = img;
				int num10 = msg.reader().readByte();
				Mob.arrMobTemplate[num9].data.typeData = num10;
				if (num10 == 1 || num10 == 2)
				{
					readFrameBoss(msg, num9);
				}
				break;
			}
			case -69:
				Char.myCharz().cMaxStamina = msg.reader().readShort();
				break;
			case -68:
				Char.myCharz().cStamina = msg.reader().readShort();
				break;
			case -67:
			{
				Res.outz("RECIEVE ICON");
				demCount += 1f;
				int num175 = msg.reader().readInt();
				sbyte[] array19 = null;
				try
				{
					array19 = NinjaUtil.readByteArray(msg);
					Res.outz("request hinh icon = " + num175);
					if (num175 == 3896)
					{
						Res.outz("SIZE CHECK= " + array19.Length);
					}
					SmallImage.imgNew[num175].img = createImage(array19);
				}
				catch (Exception)
				{
					array19 = null;
					SmallImage.imgNew[num175].img = Image.createRGBImage(new int[1], 1, 1, bl: true);
				}
				if (array19 != null)
				{
					Rms.saveRMS(mGraphics.zoomLevel + "Small" + num175, array19);
				}
				break;
			}
			case -66:
			{
				short num153 = msg.reader().readShort();
				sbyte[] data5 = NinjaUtil.readByteArray(msg);
				EffectData effDataById = Effect.getEffDataById(num153);
				sbyte b56 = msg.reader().readSByte();
				if (b56 == 0)
				{
					effDataById.readData(data5);
				}
				else
				{
					effDataById.readDataNewBoss(data5, b56);
				}
				sbyte[] array17 = NinjaUtil.readByteArray(msg);
				effDataById.img = Image.createImage(array17, 0, array17.Length);
				Res.outz("err5 ");
				if (num153 != 78)
				{
					break;
				}
				sbyte b57 = msg.reader().readByte();
				short[][] array18 = new short[b57][];
				for (int num154 = 0; num154 < b57; num154++)
				{
					int num155 = msg.reader().readUnsignedByte();
					array18[num154] = new short[num155];
					for (int num156 = 0; num156 < num155; num156++)
					{
						array18[num154][num156] = msg.reader().readShort();
					}
				}
				effDataById.anim_data = array18;
				break;
			}
			case -32:
			{
				short id3 = msg.reader().readShort();
				int num135 = msg.reader().readInt();
				sbyte[] array13 = null;
				Image image = null;
				try
				{
					array13 = new sbyte[num135];
					for (int num136 = 0; num136 < num135; num136++)
					{
						array13[num136] = msg.reader().readByte();
					}
					image = Image.createImage(array13, 0, num135);
					BgItem.imgNew.put(id3 + string.Empty, image);
				}
				catch (Exception)
				{
					array13 = null;
					BgItem.imgNew.put(id3 + string.Empty, Image.createRGBImage(new int[1], 1, 1, bl: true));
				}
				if (array13 != null)
				{
					if (mGraphics.zoomLevel > 1)
					{
						Rms.saveRMS(mGraphics.zoomLevel + "bgItem" + id3, array13);
					}
					BgItemMn.blendcurrBg(id3, image);
				}
				break;
			}
			case 92:
			{
				if (GameCanvas.currentScreen == GameScr.instance)
				{
					GameCanvas.endDlg();
				}
				string text4 = msg.reader().readUTF();
				string str2 = msg.reader().readUTF();
				str2 = Res.changeString(str2);
				string empty = string.Empty;
				Char char6 = null;
				sbyte b40 = 0;
				if (!text4.Equals(string.Empty))
				{
					char6 = new Char();
					char6.charID = msg.reader().readInt();
					char6.head = msg.reader().readShort();
					char6.headICON = msg.reader().readShort();
					char6.body = msg.reader().readShort();
					char6.bag = msg.reader().readShort();
					char6.leg = msg.reader().readShort();
					b40 = msg.reader().readByte();
					char6.cName = text4;
				}
				empty += str2;
				InfoDlg.hide();
				if (text4.Equals(string.Empty))
				{
					GameScr.info1.addInfo(empty, 0);
					break;
				}
				GameScr.info2.addInfoWithChar(empty, char6, b40 == 0);
				if (GameCanvas.panel.isShow && GameCanvas.panel.type == 8)
				{
					GameCanvas.panel.initLogMessage();
				}
				break;
			}
			case -26:
				ServerListScreen.testConnect = 2;
				GameCanvas.debug("SA2", 2);
				GameCanvas.startOKDlg(msg.reader().readUTF());
				InfoDlg.hide();
				LoginScr.isContinueToLogin = false;
				Char.isLoadingMap = false;
				if (GameCanvas.currentScreen == GameCanvas.loginScr)
				{
					GameCanvas.serverScreen.switchToMe();
				}
				break;
			case -25:
				GameCanvas.debug("SA3", 2);
				GameScr.info1.addInfo(msg.reader().readUTF(), 0);
				break;
			case 94:
				GameCanvas.debug("SA3", 2);
				GameScr.info1.addInfo(msg.reader().readUTF(), 0);
				break;
			case 47:
				GameCanvas.debug("SA4", 2);
				GameScr.gI().resetButton();
				break;
			case 81:
			{
				GameCanvas.debug("SXX4", 2);
				Mob mob11 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob11.isDisable = msg.reader().readBool();
				break;
			}
			case 82:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob10 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob10.isDontMove = msg.reader().readBool();
				break;
			}
			case 85:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob9.isFire = msg.reader().readBool();
				break;
			}
			case 86:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob7 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob7.isIce = msg.reader().readBool();
				if (!mob7.isIce)
				{
					ServerEffect.addServerEffect(77, mob7.x, mob7.y - 9, 1);
				}
				break;
			}
			case 87:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob8.isWind = msg.reader().readBool();
				break;
			}
			case 56:
			{
				GameCanvas.debug("SXX6", 2);
				@char = null;
				int num26 = msg.reader().readInt();
				if (num26 == Char.myCharz().charID)
				{
					bool flag3 = false;
					@char = Char.myCharz();
					@char.cHP = msg.readInt3Byte();
					int num27 = msg.readInt3Byte();
					Res.outz("dame hit = " + num27);
					if (num27 != 0)
					{
						@char.doInjure();
					}
					int num28 = 0;
					try
					{
						flag3 = msg.reader().readBoolean();
						sbyte b16 = msg.reader().readByte();
						if (b16 != -1)
						{
							Res.outz("hit eff= " + b16);
							EffecMn.addEff(new Effect(b16, @char.cx, @char.cy, 3, 1, -1));
						}
					}
					catch (Exception)
					{
					}
					num27 += num28;
					if (Char.myCharz().cTypePk != 4)
					{
						if (num27 == 0)
						{
							GameScr.startFlyText(mResources.miss, @char.cx, @char.cy - @char.ch, 0, -3, mFont.MISS_ME);
						}
						else
						{
							GameScr.startFlyText("-" + num27, @char.cx, @char.cy - @char.ch, 0, -3, flag3 ? mFont.FATAL : mFont.RED);
						}
					}
					break;
				}
				@char = GameScr.findCharInMap(num26);
				if (@char == null)
				{
					return;
				}
				@char.cHP = msg.readInt3Byte();
				bool flag4 = false;
				int num29 = msg.readInt3Byte();
				if (num29 != 0)
				{
					@char.doInjure();
				}
				int num30 = 0;
				try
				{
					flag4 = msg.reader().readBoolean();
					sbyte b17 = msg.reader().readByte();
					if (b17 != -1)
					{
						Res.outz("hit eff= " + b17);
						EffecMn.addEff(new Effect(b17, @char.cx, @char.cy, 3, 1, -1));
					}
				}
				catch (Exception)
				{
				}
				num29 += num30;
				if (@char.cTypePk != 4)
				{
					if (num29 == 0)
					{
						GameScr.startFlyText(mResources.miss, @char.cx, @char.cy - @char.ch, 0, -3, mFont.MISS);
					}
					else
					{
						GameScr.startFlyText("-" + num29, @char.cx, @char.cy - @char.ch, 0, -3, flag4 ? mFont.FATAL : mFont.ORANGE);
					}
				}
				break;
			}
			case 83:
			{
				GameCanvas.debug("SXX8", 2);
				int num14 = msg.reader().readInt();
				@char = ((num14 != Char.myCharz().charID) ? GameScr.findCharInMap(num14) : Char.myCharz());
				if (@char == null)
				{
					return;
				}
				Mob mobToAttack = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				if (@char.mobMe != null)
				{
					@char.mobMe.attackOtherMob(mobToAttack);
				}
				break;
			}
			case 84:
			{
				int num11 = msg.reader().readInt();
				if (num11 == Char.myCharz().charID)
				{
					@char = Char.myCharz();
				}
				else
				{
					@char = GameScr.findCharInMap(num11);
					if (@char == null)
					{
						return;
					}
				}
				@char.cHP = @char.cHPFull;
				@char.cMP = @char.cMPFull;
				@char.cx = msg.reader().readShort();
				@char.cy = msg.reader().readShort();
				@char.liveFromDead();
				break;
			}
			case 46:
				GameCanvas.debug("SA5", 2);
				Cout.LogWarning("Controler RESET_POINT  " + Char.ischangingMap);
				Char.isLockKey = false;
				Char.myCharz().setResetPoint(msg.reader().readShort(), msg.reader().readShort());
				break;
			case -29:
				messageNotLogin(msg);
				break;
			case -28:
				messageNotMap(msg);
				break;
			case -30:
				messageSubCommand(msg);
				break;
			case 62:
				GameCanvas.debug("SZ3", 2);
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
				{
					@char.killCharId = Char.myCharz().charID;
					Char.myCharz().npcFocus = null;
					Char.myCharz().mobFocus = null;
					Char.myCharz().itemFocus = null;
					Char.myCharz().charFocus = @char;
					Char.isManualFocus = true;
					GameScr.info1.addInfo(@char.cName + mResources.CUU_SAT, 0);
				}
				break;
			case 63:
				GameCanvas.debug("SZ4", 2);
				Char.myCharz().killCharId = msg.reader().readInt();
				Char.myCharz().npcFocus = null;
				Char.myCharz().mobFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().charFocus = GameScr.findCharInMap(Char.myCharz().killCharId);
				Char.isManualFocus = true;
				break;
			case 64:
				GameCanvas.debug("SZ5", 2);
				@char = Char.myCharz();
				try
				{
					@char = GameScr.findCharInMap(msg.reader().readInt());
				}
				catch (Exception ex2)
				{
					Cout.println("Loi CLEAR_CUU_SAT " + ex2.ToString());
				}
				@char.killCharId = -9999;
				break;
			case 39:
				GameCanvas.debug("SA49", 2);
				GameScr.gI().typeTradeOrder = 2;
				if (GameScr.gI().typeTrade >= 2 && GameScr.gI().typeTradeOrder >= 2)
				{
					InfoDlg.showWait();
				}
				break;
			case 57:
			{
				GameCanvas.debug("SZ6", 2);
				MyVector myVector2 = new MyVector();
				myVector2.addElement(new Command(msg.reader().readUTF(), GameCanvas.instance, 88817, null));
				GameCanvas.menu.startAt(myVector2, 3);
				break;
			}
			case 58:
			{
				GameCanvas.debug("SZ7", 2);
				int num172 = msg.reader().readInt();
				Char char11 = ((num172 != Char.myCharz().charID) ? GameScr.findCharInMap(num172) : Char.myCharz());
				char11.moveFast = new short[3];
				char11.moveFast[0] = 0;
				short num173 = msg.reader().readShort();
				short num174 = msg.reader().readShort();
				char11.moveFast[1] = num173;
				char11.moveFast[2] = num174;
				try
				{
					num172 = msg.reader().readInt();
					Char char12 = ((num172 != Char.myCharz().charID) ? GameScr.findCharInMap(num172) : Char.myCharz());
					char12.cx = num173;
					char12.cy = num174;
				}
				catch (Exception ex23)
				{
					Cout.println("Loi MOVE_FAST " + ex23.ToString());
				}
				break;
			}
			case 88:
			{
				string info4 = msg.reader().readUTF();
				short num171 = msg.reader().readShort();
				GameCanvas.inputDlg.show(info4, new Command(mResources.ACCEPT, GameCanvas.instance, 88818, num171), TField.INPUT_TYPE_ANY);
				break;
			}
			case 27:
			{
				myVector = new MyVector();
				string text7 = msg.reader().readUTF();
				int num166 = msg.reader().readByte();
				for (int num167 = 0; num167 < num166; num167++)
				{
					string caption4 = msg.reader().readUTF();
					short num168 = msg.reader().readShort();
					myVector.addElement(new Command(caption4, GameCanvas.instance, 88819, num168));
				}
				GameCanvas.menu.startWithoutCloseButton(myVector, 3);
				break;
			}
			case 33:
			{
				GameCanvas.debug("SA51", 2);
				InfoDlg.hide();
				GameCanvas.clearKeyHold();
				GameCanvas.clearKeyPressed();
				myVector = new MyVector();
				try
				{
					while (true)
					{
						string caption3 = msg.reader().readUTF();
						myVector.addElement(new Command(caption3, GameCanvas.instance, 88822, null));
					}
				}
				catch (Exception ex21)
				{
					Cout.println("Loi OPEN_UI_MENU " + ex21.ToString());
				}
				if (Char.myCharz().npcFocus == null)
				{
					return;
				}
				for (int num152 = 0; num152 < Char.myCharz().npcFocus.template.menu.Length; num152++)
				{
					string[] array16 = Char.myCharz().npcFocus.template.menu[num152];
					myVector.addElement(new Command(array16[0], GameCanvas.instance, 88820, array16));
				}
				GameCanvas.menu.startAt(myVector, 3);
				break;
			}
			case 40:
			{
				GameCanvas.debug("SA52", 2);
				GameCanvas.taskTick = 150;
				short taskId = msg.reader().readShort();
				sbyte index2 = msg.reader().readByte();
				string str3 = msg.reader().readUTF();
				str3 = Res.changeString(str3);
				string str4 = msg.reader().readUTF();
				str4 = Res.changeString(str4);
				string[] array10 = new string[msg.reader().readByte()];
				string[] array11 = new string[array10.Length];
				GameScr.tasks = new int[array10.Length];
				GameScr.mapTasks = new int[array10.Length];
				short[] array12 = new short[array10.Length];
				short count = -1;
				for (int num133 = 0; num133 < array10.Length; num133++)
				{
					string str5 = msg.reader().readUTF();
					str5 = Res.changeString(str5);
					GameScr.tasks[num133] = msg.reader().readByte();
					GameScr.mapTasks[num133] = msg.reader().readShort();
					string str6 = msg.reader().readUTF();
					str6 = Res.changeString(str6);
					array12[num133] = -1;
					if (!str5.Equals(string.Empty))
					{
						array10[num133] = str5;
						array11[num133] = str6;
					}
				}
				try
				{
					count = msg.reader().readShort();
					for (int num134 = 0; num134 < array10.Length; num134++)
					{
						array12[num134] = msg.reader().readShort();
					}
				}
				catch (Exception ex16)
				{
					Cout.println("Loi TASK_GET " + ex16.ToString());
				}
				Char.myCharz().taskMaint = new Task(taskId, index2, str3, str4, array10, array12, count, array11);
				if (Char.myCharz().npcFocus != null)
				{
					Npc.clearEffTask();
				}
				Char.taskAction(isNextStep: false);
				break;
			}
			case 41:
			{
				GameCanvas.debug("SA53", 2);
				GameCanvas.taskTick = 100;
				Res.outz("TASK NEXT");
				Task taskMaint = Char.myCharz().taskMaint;
				taskMaint.index++;
				Char.myCharz().taskMaint.count = 0;
				Npc.clearEffTask();
				Char.taskAction(isNextStep: true);
				break;
			}
			case 50:
			{
				sbyte b45 = msg.reader().readByte();
				Panel.vGameInfo.removeAllElements();
				for (int num131 = 0; num131 < b45; num131++)
				{
					GameInfo gameInfo = new GameInfo();
					gameInfo.id = msg.reader().readShort();
					gameInfo.main = msg.reader().readUTF();
					gameInfo.content = msg.reader().readUTF();
					Panel.vGameInfo.addElement(gameInfo);
					bool flag9 = (gameInfo.hasRead = Rms.loadRMSInt(gameInfo.id + string.Empty) != -1);
				}
				break;
			}
			case 43:
				GameCanvas.taskTick = 50;
				GameCanvas.debug("SA55", 2);
				Char.myCharz().taskMaint.count = msg.reader().readShort();
				if (Char.myCharz().npcFocus != null)
				{
					Npc.clearEffTask();
				}
				try
				{
					short x_hint = msg.reader().readShort();
					short y_hint = msg.reader().readShort();
					Char.myCharz().x_hint = x_hint;
					Char.myCharz().y_hint = y_hint;
					Res.outz("CMD   TASK_UPDATE:43_mapID =    x|y " + x_hint + "|" + y_hint);
					for (int num129 = 0; num129 < TileMap.vGo.size(); num129++)
					{
						Res.outz("===> " + TileMap.vGo.elementAt(num129));
					}
				}
				catch (Exception)
				{
				}
				break;
			case 90:
				GameCanvas.debug("SA577", 2);
				requestItemPlayer(msg);
				break;
			case 29:
				GameCanvas.debug("SA58", 2);
				GameScr.gI().openUIZone(msg);
				break;
			case -21:
			{
				GameCanvas.debug("SA60", 2);
				short num127 = msg.reader().readShort();
				for (int num128 = 0; num128 < GameScr.vItemMap.size(); num128++)
				{
					if (((ItemMap)GameScr.vItemMap.elementAt(num128)).itemMapID == num127)
					{
						GameScr.vItemMap.removeElementAt(num128);
						break;
					}
				}
				break;
			}
			case -20:
			{
				GameCanvas.debug("SA61", 2);
				Char.myCharz().itemFocus = null;
				short num125 = msg.reader().readShort();
				for (int num126 = 0; num126 < GameScr.vItemMap.size(); num126++)
				{
					ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(num126);
					if (itemMap2.itemMapID != num125)
					{
						continue;
					}
					itemMap2.setPoint(Char.myCharz().cx, Char.myCharz().cy - 10);
					string text5 = msg.reader().readUTF();
					num = 0;
					try
					{
						num = msg.reader().readShort();
						if (itemMap2.template.type == 9)
						{
							num = msg.reader().readShort();
							Char char9 = Char.myCharz();
							char9.xu += num;
							Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
						}
						else if (itemMap2.template.type == 10)
						{
							num = msg.reader().readShort();
							Char char9 = Char.myCharz();
							char9.luong += num;
							Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
						}
						else if (itemMap2.template.type == 34)
						{
							num = msg.reader().readShort();
							Char char9 = Char.myCharz();
							char9.luongKhoa += num;
							Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
						}
					}
					catch (Exception)
					{
					}
					if (text5.Equals(string.Empty))
					{
						if (itemMap2.template.type == 9)
						{
							GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.YELLOW);
							SoundMn.gI().getItem();
						}
						else if (itemMap2.template.type == 10)
						{
							GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.GREEN);
							SoundMn.gI().getItem();
						}
						else if (itemMap2.template.type == 34)
						{
							GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.RED);
							SoundMn.gI().getItem();
						}
						else
						{
							GameScr.info1.addInfo(mResources.you_receive + " " + ((num <= 0) ? string.Empty : (num + " ")) + itemMap2.template.name, 0);
							SoundMn.gI().getItem();
						}
						if (num > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 4683)
						{
							ServerEffect.addServerEffect(55, Char.myCharz().petFollow.cmx, Char.myCharz().petFollow.cmy, 1);
							ServerEffect.addServerEffect(55, Char.myCharz().cx, Char.myCharz().cy, 1);
						}
					}
					else if (text5.Length == 1)
					{
						Cout.LogError3("strInf.Length =1:  " + text5);
					}
					else
					{
						GameScr.info1.addInfo(text5, 0);
					}
					break;
				}
				break;
			}
			case -19:
			{
				GameCanvas.debug("SA62", 2);
				short num123 = msg.reader().readShort();
				@char = GameScr.findCharInMap(msg.reader().readInt());
				for (int num124 = 0; num124 < GameScr.vItemMap.size(); num124++)
				{
					ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(num124);
					if (itemMap.itemMapID != num123)
					{
						continue;
					}
					if (@char == null)
					{
						return;
					}
					itemMap.setPoint(@char.cx, @char.cy - 10);
					if (itemMap.x < @char.cx)
					{
						@char.cdir = -1;
					}
					else if (itemMap.x > @char.cx)
					{
						@char.cdir = 1;
					}
					break;
				}
				break;
			}
			case -18:
			{
				GameCanvas.debug("SA63", 2);
				int num122 = msg.reader().readByte();
				GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), Char.myCharz().arrItemBag[num122].template.id, Char.myCharz().cx, Char.myCharz().cy, msg.reader().readShort(), msg.reader().readShort()));
				Char.myCharz().arrItemBag[num122] = null;
				break;
			}
			case 68:
			{
				Res.outz("ADD ITEM TO MAP --------------------------------------");
				GameCanvas.debug("SA6333", 2);
				short itemMapID = msg.reader().readShort();
				short itemTemplateID = msg.reader().readShort();
				int x = msg.reader().readShort();
				int y = msg.reader().readShort();
				int num119 = msg.reader().readInt();
				short r = 0;
				if (num119 == -2)
				{
					r = msg.reader().readShort();
				}
				ItemMap o2 = new ItemMap(num119, itemMapID, itemTemplateID, x, y, r);
				GameScr.vItemMap.addElement(o2);
				break;
			}
			case 69:
				SoundMn.IsDelAcc = ((msg.reader().readByte() != 0) ? true : false);
				break;
			case -14:
				GameCanvas.debug("SA64", 2);
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
				{
					return;
				}
				GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), msg.reader().readShort(), @char.cx, @char.cy, msg.reader().readShort(), msg.reader().readShort()));
				break;
			case -22:
				GameCanvas.debug("SA65", 2);
				Char.isLockKey = true;
				Char.ischangingMap = true;
				GameScr.gI().timeStartMap = 0;
				GameScr.gI().timeLengthMap = 0;
				Char.myCharz().mobFocus = null;
				Char.myCharz().npcFocus = null;
				Char.myCharz().charFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().focus.removeAllElements();
				Char.myCharz().testCharId = -9999;
				Char.myCharz().killCharId = -9999;
				GameCanvas.resetBg();
				GameScr.gI().resetButton();
				GameScr.gI().center = null;
				break;
			case -70:
			{
				Res.outz("BIG MESSAGE .......................................");
				GameCanvas.endDlg();
				int avatar = msg.reader().readShort();
				string chat3 = msg.reader().readUTF();
				Npc npc6 = new Npc(-1, 0, 0, 0, 0, 0);
				npc6.avatar = avatar;
				ChatPopup.addBigMessage(chat3, 100000, npc6);
				sbyte b39 = msg.reader().readByte();
				if (b39 == 0)
				{
					ChatPopup.serverChatPopUp.cmdMsg1 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null);
					ChatPopup.serverChatPopUp.cmdMsg1.x = GameCanvas.w / 2 - 35;
					ChatPopup.serverChatPopUp.cmdMsg1.y = GameCanvas.h - 35;
				}
				if (b39 == 1)
				{
					string p2 = msg.reader().readUTF();
					string caption2 = msg.reader().readUTF();
					ChatPopup.serverChatPopUp.cmdMsg1 = new Command(caption2, ChatPopup.serverChatPopUp, 1000, p2);
					ChatPopup.serverChatPopUp.cmdMsg1.x = GameCanvas.w / 2 - 75;
					ChatPopup.serverChatPopUp.cmdMsg1.y = GameCanvas.h - 35;
					ChatPopup.serverChatPopUp.cmdMsg2 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null);
					ChatPopup.serverChatPopUp.cmdMsg2.x = GameCanvas.w / 2 + 11;
					ChatPopup.serverChatPopUp.cmdMsg2.y = GameCanvas.h - 35;
				}
				break;
			}
			case 38:
			{
				GameCanvas.debug("SA67", 2);
				InfoDlg.hide();
				int num105 = msg.reader().readShort();
				Res.outz("OPEN_UI_SAY ID= " + num105);
				string str = msg.reader().readUTF();
				str = Res.changeString(str);
				for (int num106 = 0; num106 < GameScr.vNpc.size(); num106++)
				{
					Npc npc4 = (Npc)GameScr.vNpc.elementAt(num106);
					Res.outz("npc id= " + npc4.template.npcTemplateId);
					if (npc4.template.npcTemplateId == num105)
					{
						ChatPopup.addChatPopupMultiLine(str, 100000, npc4);
						GameCanvas.panel.hideNow();
						return;
					}
				}
				Npc npc5 = new Npc(num105, 0, 0, 0, num105, GameScr.info1.charId[Char.myCharz().cgender][2]);
				if (npc5.template.npcTemplateId == 5)
				{
					npc5.charID = 5;
				}
				try
				{
					npc5.avatar = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				ChatPopup.addChatPopupMultiLine(str, 100000, npc5);
				GameCanvas.panel.hideNow();
				break;
			}
			case 32:
			{
				GameCanvas.debug("SA68", 2);
				int num83 = msg.reader().readShort();
				for (int num84 = 0; num84 < GameScr.vNpc.size(); num84++)
				{
					Npc npc2 = (Npc)GameScr.vNpc.elementAt(num84);
					if (npc2.template.npcTemplateId == num83 && npc2.Equals(Char.myCharz().npcFocus))
					{
						string chat = msg.reader().readUTF();
						string[] array6 = new string[msg.reader().readByte()];
						for (int num85 = 0; num85 < array6.Length; num85++)
						{
							array6[num85] = msg.reader().readUTF();
						}
						GameScr.gI().createMenu(array6, npc2);
						ChatPopup.addChatPopup(chat, 100000, npc2);
						return;
					}
				}
				Npc npc3 = new Npc(num83, 0, -100, 100, num83, GameScr.info1.charId[Char.myCharz().cgender][2]);
				Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
				string chat2 = msg.reader().readUTF();
				string[] array7 = new string[msg.reader().readByte()];
				for (int num86 = 0; num86 < array7.Length; num86++)
				{
					array7[num86] = msg.reader().readUTF();
				}
				try
				{
					short num87 = (short)(npc3.avatar = msg.reader().readShort());
				}
				catch (Exception)
				{
				}
				Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
				GameScr.gI().createMenu(array7, npc3);
				ChatPopup.addChatPopup(chat2, 100000, npc3);
				break;
			}
			case 7:
			{
				sbyte type3 = msg.reader().readByte();
				short id = msg.reader().readShort();
				string info = msg.reader().readUTF();
				GameCanvas.panel.saleRequest(type3, info, id);
				break;
			}
			case 6:
				GameCanvas.debug("SA70", 2);
				Char.myCharz().xu = msg.reader().readLong();
				Char.myCharz().luong = msg.reader().readInt();
				Char.myCharz().luongKhoa = msg.reader().readInt();
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
				Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
				GameCanvas.endDlg();
				break;
			case -24:
				Char.isLoadingMap = true;
				Cout.println("GET MAP INFO");
				GameScr.gI().magicTree = null;
				GameCanvas.isLoading = true;
				GameCanvas.debug("SA75", 2);
				GameScr.resetAllvector();
				GameCanvas.endDlg();
				TileMap.vGo.removeAllElements();
				PopUp.vPopups.removeAllElements();
				mSystem.gcc();
				TileMap.mapID = msg.reader().readUnsignedByte();
				TileMap.planetID = msg.reader().readByte();
				TileMap.tileID = msg.reader().readByte();
				TileMap.bgID = msg.reader().readByte();
				Cout.println("load planet from server: " + TileMap.planetID + "bgType= " + TileMap.bgType + ".............................");
				TileMap.typeMap = msg.reader().readByte();
				TileMap.mapName = msg.reader().readUTF();
				TileMap.zoneID = msg.reader().readByte();
				GameCanvas.debug("SA75x1", 2);
				try
				{
					TileMap.loadMapFromResource(TileMap.mapID);
				}
				catch (Exception)
				{
					Service.gI().requestMaptemplate(TileMap.mapID);
					messWait = msg;
					return;
				}
				loadInfoMap(msg);
				try
				{
					TileMap.isMapDouble = ((msg.reader().readByte() != 0) ? true : false);
				}
				catch (Exception)
				{
				}
				GameScr.cmx = GameScr.cmtoX;
				GameScr.cmy = GameScr.cmtoY;
				break;
			case -31:
			{
				TileMap.vItemBg.removeAllElements();
				short num62 = msg.reader().readShort();
				Cout.LogError2("nItem= " + num62);
				for (int num63 = 0; num63 < num62; num63++)
				{
					BgItem bgItem = new BgItem();
					bgItem.id = num63;
					bgItem.idImage = msg.reader().readShort();
					bgItem.layer = msg.reader().readByte();
					bgItem.dx = msg.reader().readShort();
					bgItem.dy = msg.reader().readShort();
					sbyte b26 = msg.reader().readByte();
					bgItem.tileX = new int[b26];
					bgItem.tileY = new int[b26];
					for (int num64 = 0; num64 < b26; num64++)
					{
						bgItem.tileX[num63] = msg.reader().readByte();
						bgItem.tileY[num63] = msg.reader().readByte();
					}
					TileMap.vItemBg.addElement(bgItem);
				}
				break;
			}
			case -4:
			{
				GameCanvas.debug("SA76", 2);
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
				{
					return;
				}
				GameCanvas.debug("SA76v1", 2);
				if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
				{
					@char.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 0);
				}
				else
				{
					@char.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 1);
				}
				GameCanvas.debug("SA76v2", 2);
				@char.attMobs = new Mob[msg.reader().readByte()];
				for (int num24 = 0; num24 < @char.attMobs.Length; num24++)
				{
					Mob mob3 = (Mob)GameScr.vMob.elementAt(msg.reader().readByte());
					@char.attMobs[num24] = mob3;
					if (num24 == 0)
					{
						if (@char.cx <= mob3.x)
						{
							@char.cdir = 1;
						}
						else
						{
							@char.cdir = -1;
						}
					}
				}
				GameCanvas.debug("SA76v3", 2);
				@char.charFocus = null;
				@char.mobFocus = @char.attMobs[0];
				Char[] array4 = new Char[10];
				num = 0;
				try
				{
					for (num = 0; num < array4.Length; num++)
					{
						int num25 = msg.reader().readInt();
						Char char4 = (array4[num] = ((num25 != Char.myCharz().charID) ? GameScr.findCharInMap(num25) : Char.myCharz()));
						if (num == 0)
						{
							if (@char.cx <= char4.cx)
							{
								@char.cdir = 1;
							}
							else
							{
								@char.cdir = -1;
							}
						}
					}
				}
				catch (Exception ex6)
				{
					Cout.println("Loi PLAYER_ATTACK_N_P " + ex6.ToString());
				}
				GameCanvas.debug("SA76v4", 2);
				if (num > 0)
				{
					@char.attChars = new Char[num];
					for (num = 0; num < @char.attChars.Length; num++)
					{
						@char.attChars[num] = array4[num];
					}
					@char.charFocus = @char.attChars[0];
					@char.mobFocus = null;
				}
				GameCanvas.debug("SA76v5", 2);
				break;
			}
			case 54:
			{
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
				{
					return;
				}
				int num12 = msg.reader().readUnsignedByte();
				if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
				{
					@char.setSkillPaint(GameScr.sks[num12], 0);
				}
				else
				{
					@char.setSkillPaint(GameScr.sks[num12], 1);
				}
				Mob[] array3 = new Mob[10];
				num = 0;
				try
				{
					for (num = 0; num < array3.Length; num++)
					{
						Mob mob2 = (array3[num] = (Mob)GameScr.vMob.elementAt(msg.reader().readByte()));
						if (num == 0)
						{
							if (@char.cx <= mob2.x)
							{
								@char.cdir = 1;
							}
							else
							{
								@char.cdir = -1;
							}
						}
					}
				}
				catch (Exception ex3)
				{
					Cout.println("Loi PLAYER_ATTACK_NPC " + ex3.ToString());
				}
				if (num > 0)
				{
					@char.attMobs = new Mob[num];
					for (num = 0; num < @char.attMobs.Length; num++)
					{
						@char.attMobs[num] = array3[num];
					}
					@char.charFocus = null;
					@char.mobFocus = @char.attMobs[0];
				}
				break;
			}
			case -60:
			{
				GameCanvas.debug("SA7666", 2);
				int num2 = msg.reader().readInt();
				int num3 = -1;
				if (num2 != Char.myCharz().charID)
				{
					Char char2 = GameScr.findCharInMap(num2);
					if (char2 == null)
					{
						return;
					}
					if (char2.currentMovePoint != null)
					{
						char2.createShadow(char2.cx, char2.cy, 10);
						char2.cx = char2.currentMovePoint.xEnd;
						char2.cy = char2.currentMovePoint.yEnd;
					}
					int num4 = msg.reader().readUnsignedByte();
					Res.outz("player skill ID= " + num4);
					if ((TileMap.tileTypeAtPixel(char2.cx, char2.cy) & 2) == 2)
					{
						char2.setSkillPaint(GameScr.sks[num4], 0);
					}
					else
					{
						char2.setSkillPaint(GameScr.sks[num4], 1);
					}
					sbyte b = msg.reader().readByte();
					Res.outz("nAttack = " + b);
					Char[] array = new Char[b];
					for (num = 0; num < array.Length; num++)
					{
						num3 = msg.reader().readInt();
						Char char3;
						if (num3 == Char.myCharz().charID)
						{
							char3 = Char.myCharz();
							if (!GameScr.isChangeZone && GameScr.isAutoPlay && GameScr.canAutoPlay)
							{
								Service.gI().requestChangeZone(-1, -1);
								GameScr.isChangeZone = true;
							}
						}
						else
						{
							char3 = GameScr.findCharInMap(num3);
						}
						array[num] = char3;
						if (num == 0)
						{
							if (char2.cx <= char3.cx)
							{
								char2.cdir = 1;
							}
							else
							{
								char2.cdir = -1;
							}
						}
					}
					if (num > 0)
					{
						char2.attChars = new Char[num];
						for (num = 0; num < char2.attChars.Length; num++)
						{
							char2.attChars[num] = array[num];
						}
						char2.mobFocus = null;
						char2.charFocus = char2.attChars[0];
					}
				}
				else
				{
					sbyte b2 = msg.reader().readByte();
					sbyte b3 = msg.reader().readByte();
					num3 = msg.reader().readInt();
				}
				try
				{
					sbyte b4 = msg.reader().readByte();
					Res.outz("isRead continue = " + b4);
					if (b4 != 1)
					{
						break;
					}
					sbyte b5 = msg.reader().readByte();
					Res.outz("type skill = " + b5);
					if (num3 == Char.myCharz().charID)
					{
						bool flag = false;
						@char = Char.myCharz();
						int num5 = msg.readInt3Byte();
						Res.outz("dame hit = " + num5);
						@char.isDie = msg.reader().readBoolean();
						if (@char.isDie)
						{
							Char.isLockKey = true;
						}
						Res.outz("isDie=" + @char.isDie + "---------------------------------------");
						int num6 = 0;
						flag = (@char.isCrit = msg.reader().readBoolean());
						@char.isMob = false;
						num5 = (@char.damHP = num5 + num6);
						if (b5 == 0)
						{
							@char.doInjure(num5, 0, flag, isMob: false);
						}
					}
					else
					{
						@char = GameScr.findCharInMap(num3);
						if (@char == null)
						{
							return;
						}
						bool flag2 = false;
						int num7 = msg.readInt3Byte();
						Res.outz("dame hit= " + num7);
						@char.isDie = msg.reader().readBoolean();
						Res.outz("isDie=" + @char.isDie + "---------------------------------------");
						int num8 = 0;
						flag2 = (@char.isCrit = msg.reader().readBoolean());
						@char.isMob = false;
						num7 = (@char.damHP = num7 + num8);
						if (b5 == 0)
						{
							@char.doInjure(num7, 0, flag2, isMob: false);
						}
					}
				}
				catch (Exception)
				{
				}
				break;
			}
			}
			switch (msg.command)
			{
			case -2:
			{
				GameCanvas.debug("SA77", 22);
				int num199 = msg.reader().readInt();
				Char char9 = Char.myCharz();
				char9.yen += num199;
				GameScr.startFlyText((num199 <= 0) ? (string.Empty + num199) : ("+" + num199), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case 95:
			{
				GameCanvas.debug("SA77", 22);
				int num186 = msg.reader().readInt();
				Char char9 = Char.myCharz();
				char9.xu += num186;
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				GameScr.startFlyText((num186 <= 0) ? (string.Empty + num186) : ("+" + num186), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case 96:
				GameCanvas.debug("SA77a", 22);
				Char.myCharz().taskOrders.addElement(new TaskOrder(msg.reader().readByte(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readUTF(), msg.reader().readUTF(), msg.reader().readByte(), msg.reader().readByte()));
				break;
			case 97:
			{
				sbyte b68 = msg.reader().readByte();
				for (int num192 = 0; num192 < Char.myCharz().taskOrders.size(); num192++)
				{
					TaskOrder taskOrder = (TaskOrder)Char.myCharz().taskOrders.elementAt(num192);
					if (taskOrder.taskId == b68)
					{
						taskOrder.count = msg.reader().readShort();
						break;
					}
				}
				break;
			}
			case -1:
			{
				GameCanvas.debug("SA77", 222);
				int num198 = msg.reader().readInt();
				Char char9 = Char.myCharz();
				char9.xu += num198;
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				char9 = Char.myCharz();
				char9.yen -= num198;
				GameScr.startFlyText("+" + num198, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case -3:
			{
				GameCanvas.debug("SA78", 2);
				sbyte b65 = msg.reader().readByte();
				int num182 = msg.reader().readInt();
				if (b65 == 0)
				{
					Char char9 = Char.myCharz();
					char9.cPower += num182;
				}
				if (b65 == 1)
				{
					Char char9 = Char.myCharz();
					char9.cTiemNang += num182;
				}
				if (b65 == 2)
				{
					Char char9 = Char.myCharz();
					char9.cPower += num182;
					char9 = Char.myCharz();
					char9.cTiemNang += num182;
				}
				Char.myCharz().applyCharLevelPercent();
				if (Char.myCharz().cTypePk != 3)
				{
					GameScr.startFlyText(((num182 <= 0) ? string.Empty : "+") + num182, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -4, mFont.GREEN);
					if (num182 > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5002)
					{
						ServerEffect.addServerEffect(55, Char.myCharz().petFollow.cmx, Char.myCharz().petFollow.cmy, 1);
						ServerEffect.addServerEffect(55, Char.myCharz().cx, Char.myCharz().cy, 1);
					}
				}
				break;
			}
			case -73:
			{
				sbyte b70 = msg.reader().readByte();
				for (int num197 = 0; num197 < GameScr.vNpc.size(); num197++)
				{
					Npc npc7 = (Npc)GameScr.vNpc.elementAt(num197);
					if (npc7.template.npcTemplateId == b70)
					{
						if (msg.reader().readByte() == 0)
						{
							npc7.isHide = true;
						}
						else
						{
							npc7.isHide = false;
						}
						break;
					}
				}
				break;
			}
			case -5:
			{
				GameCanvas.debug("SA79", 2);
				int charID = msg.reader().readInt();
				int num188 = msg.reader().readInt();
				Char char16;
				if (num188 != -100)
				{
					char16 = new Char();
					char16.charID = charID;
					char16.clanID = num188;
				}
				else
				{
					char16 = new Mabu();
					char16.charID = charID;
					char16.clanID = num188;
				}
				if (char16.clanID == -2)
				{
					char16.isCopy = true;
				}
				if (readCharInfo(char16, msg))
				{
					sbyte b67 = msg.reader().readByte();
					if (char16.cy <= 10 && b67 != 0 && b67 != 2)
					{
						Res.outz("nhân vật bay trên trời xuống x= " + char16.cx + " y= " + char16.cy);
						Teleport teleport2 = new Teleport(char16.cx, char16.cy, char16.head, char16.cdir, 1, isMe: false, (b67 != 1) ? b67 : char16.cgender);
						teleport2.id = char16.charID;
						char16.isTeleport = true;
						Teleport.addTeleport(teleport2);
					}
					if (b67 == 2)
					{
						char16.show();
					}
					for (int num189 = 0; num189 < GameScr.vMob.size(); num189++)
					{
						Mob mob19 = (Mob)GameScr.vMob.elementAt(num189);
						if (mob19 != null && mob19.isMobMe && mob19.mobId == char16.charID)
						{
							Res.outz("co 1 con quai");
							char16.mobMe = mob19;
							char16.mobMe.x = char16.cx;
							char16.mobMe.y = char16.cy - 40;
							break;
						}
					}
					if (GameScr.findCharInMap(char16.charID) == null)
					{
						GameScr.vCharInMap.addElement(char16);
					}
					char16.isMonkey = msg.reader().readByte();
					short num190 = msg.reader().readShort();
					Res.outz("mount id= " + num190 + "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
					if (num190 != -1)
					{
						char16.isHaveMount = true;
						switch (num190)
						{
						case 346:
						case 347:
						case 348:
							char16.isMountVip = false;
							break;
						case 349:
						case 350:
						case 351:
							char16.isMountVip = true;
							break;
						case 396:
							char16.isEventMount = true;
							break;
						case 532:
							char16.isSpeacialMount = true;
							break;
						default:
							if (num190 >= Char.ID_NEW_MOUNT)
							{
								char16.idMount = num190;
							}
							break;
						}
					}
					else
					{
						char16.isHaveMount = false;
					}
				}
				sbyte cFlag = msg.reader().readByte();
				Res.outz("addplayer:   " + cFlag);
				char16.cFlag = cFlag;
				char16.isNhapThe = msg.reader().readByte() == 1;
				try
				{
					char16.idAuraEff = msg.reader().readShort();
					char16.idEff_Set_Item = msg.reader().readSByte();
					char16.idHat = msg.reader().readShort();
					if (char16.bag >= 201 && char16.bag < 255)
					{
						Effect effect2 = new Effect(char16.bag, char16, 2, -1, 10, 1);
						effect2.typeEff = 5;
						char16.addEffChar(effect2);
					}
					else
					{
						for (int num191 = 0; num191 < 54; num191++)
						{
							char16.removeEffChar(0, 201 + num191);
						}
					}
				}
				catch (Exception ex37)
				{
					Res.outz("cmd: -5 err: " + ex37.StackTrace);
				}
				GameScr.gI().getFlagImage(char16.charID, char16.cFlag);
				Res.outz("Cmd: -5 PLAYER_ADD: cID| cName| cFlag| cBag|    " + @char.charID + " | " + @char.cName + " | " + @char.cFlag + " | " + @char.bag);
				break;
			}
			case -7:
			{
				GameCanvas.debug("SA80", 2);
				int num183 = msg.reader().readInt();
				Cout.println("RECEVED MOVE OF " + num183);
				for (int num184 = 0; num184 < GameScr.vCharInMap.size(); num184++)
				{
					Char char15 = null;
					try
					{
						char15 = (Char)GameScr.vCharInMap.elementAt(num184);
					}
					catch (Exception ex29)
					{
						Cout.println("Loi PLAYER_MOVE " + ex29.ToString());
					}
					if (char15 == null)
					{
						break;
					}
					if (char15.charID == num183)
					{
						GameCanvas.debug("SA8x2y" + num184, 2);
						char15.moveTo(msg.reader().readShort(), msg.reader().readShort(), 0);
						char15.lastUpdateTime = mSystem.currentTimeMillis();
						break;
					}
				}
				GameCanvas.debug("SA80x3", 2);
				break;
			}
			case -6:
			{
				GameCanvas.debug("SA81", 2);
				int num180 = msg.reader().readInt();
				for (int num181 = 0; num181 < GameScr.vCharInMap.size(); num181++)
				{
					Char char14 = (Char)GameScr.vCharInMap.elementAt(num181);
					if (char14 != null && char14.charID == num180)
					{
						if (!char14.isInvisiblez && !char14.isUsePlane)
						{
							ServerEffect.addServerEffect(60, char14.cx, char14.cy, 1);
						}
						if (!char14.isUsePlane)
						{
							GameScr.vCharInMap.removeElementAt(num181);
						}
						return;
					}
				}
				break;
			}
			case -13:
			{
				GameCanvas.debug("SA82", 2);
				int num193 = msg.reader().readUnsignedByte();
				if (num193 > GameScr.vMob.size() - 1 || num193 < 0)
				{
					return;
				}
				Mob mob20 = (Mob)GameScr.vMob.elementAt(num193);
				mob20.sys = msg.reader().readByte();
				mob20.levelBoss = msg.reader().readByte();
				if (mob20.levelBoss != 0)
				{
					mob20.typeSuperEff = Res.random(0, 3);
				}
				mob20.x = mob20.xFirst;
				mob20.y = mob20.yFirst;
				mob20.status = 5;
				mob20.injureThenDie = false;
				mob20.hp = msg.reader().readInt();
				mob20.maxHp = mob20.hp;
				mob20.updateHp_bar();
				ServerEffect.addServerEffect(60, mob20.x, mob20.y, 1);
				break;
			}
			case -75:
			{
				Mob mob17 = null;
				try
				{
					mob17 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
				}
				if (mob17 != null)
				{
					mob17.levelBoss = msg.reader().readByte();
					if (mob17.levelBoss > 0)
					{
						mob17.typeSuperEff = Res.random(0, 3);
					}
				}
				break;
			}
			case -9:
			{
				GameCanvas.debug("SA83", 2);
				Mob mob16 = null;
				try
				{
					mob16 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
				}
				GameCanvas.debug("SA83v1", 2);
				if (mob16 != null)
				{
					mob16.hp = msg.readInt3Byte();
					mob16.updateHp_bar();
					int num185 = msg.readInt3Byte();
					if (num185 == 1)
					{
						return;
					}
					if (num185 > 1)
					{
						mob16.setInjure();
					}
					bool flag10 = false;
					try
					{
						flag10 = msg.reader().readBoolean();
					}
					catch (Exception)
					{
					}
					sbyte b66 = msg.reader().readByte();
					if (b66 != -1)
					{
						EffecMn.addEff(new Effect(b66, mob16.x, mob16.getY(), 3, 1, -1));
					}
					GameCanvas.debug("SA83v2", 2);
					if (flag10)
					{
						GameScr.startFlyText("-" + num185, mob16.x, mob16.getY() - mob16.getH(), 0, -2, mFont.FATAL);
					}
					else if (num185 == 0)
					{
						mob16.x = mob16.xFirst;
						mob16.y = mob16.yFirst;
						GameScr.startFlyText(mResources.miss, mob16.x, mob16.getY() - mob16.getH(), 0, -2, mFont.MISS);
					}
					else if (num185 > 1)
					{
						GameScr.startFlyText("-" + num185, mob16.x, mob16.getY() - mob16.getH(), 0, -2, mFont.ORANGE);
					}
				}
				GameCanvas.debug("SA83v3", 2);
				break;
			}
			case 45:
			{
				GameCanvas.debug("SA84", 2);
				Mob mob14 = null;
				try
				{
					mob14 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception ex28)
				{
					Cout.println("Loi tai NPC_MISS  " + ex28.ToString());
				}
				if (mob14 != null)
				{
					mob14.hp = msg.reader().readInt();
					mob14.updateHp_bar();
					GameScr.startFlyText(mResources.miss, mob14.x, mob14.y - mob14.h, 0, -2, mFont.MISS);
				}
				break;
			}
			case -12:
			{
				Res.outz("SERVER SEND MOB DIE");
				GameCanvas.debug("SA85", 2);
				Mob mob21 = null;
				try
				{
					mob21 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
					Cout.println("LOi tai NPC_DIE cmd " + msg.command);
				}
				if (mob21 == null || mob21.status == 0 || mob21.status == 0)
				{
					break;
				}
				mob21.startDie();
				try
				{
					int num194 = msg.readInt3Byte();
					if (msg.reader().readBool())
					{
						GameScr.startFlyText("-" + num194, mob21.x, mob21.y - mob21.h, 0, -2, mFont.FATAL);
					}
					else
					{
						GameScr.startFlyText("-" + num194, mob21.x, mob21.y - mob21.h, 0, -2, mFont.ORANGE);
					}
					sbyte b69 = msg.reader().readByte();
					for (int num195 = 0; num195 < b69; num195++)
					{
						ItemMap itemMap4 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob21.x, mob21.y, msg.reader().readShort(), msg.reader().readShort());
						int num196 = (itemMap4.playerId = msg.reader().readInt());
						Res.outz("playerid= " + num196 + " my id= " + Char.myCharz().charID);
						GameScr.vItemMap.addElement(itemMap4);
						if (Res.abs(itemMap4.y - Char.myCharz().cy) < 24 && Res.abs(itemMap4.x - Char.myCharz().cx) < 24)
						{
							Char.myCharz().charFocus = null;
						}
					}
				}
				catch (Exception ex39)
				{
					Cout.println("LOi tai NPC_DIE " + ex39.ToString() + " cmd " + msg.command);
				}
				break;
			}
			case 74:
			{
				GameCanvas.debug("SA85", 2);
				Mob mob15 = null;
				try
				{
					mob15 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
					Cout.println("Loi tai NPC CHANGE " + msg.command);
				}
				if (mob15 != null && mob15.status != 0 && mob15.status != 0)
				{
					mob15.status = 0;
					ServerEffect.addServerEffect(60, mob15.x, mob15.y, 1);
					ItemMap itemMap3 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob15.x, mob15.y, msg.reader().readShort(), msg.reader().readShort());
					GameScr.vItemMap.addElement(itemMap3);
					if (Res.abs(itemMap3.y - Char.myCharz().cy) < 24 && Res.abs(itemMap3.x - Char.myCharz().cx) < 24)
					{
						Char.myCharz().charFocus = null;
					}
				}
				break;
			}
			case -11:
			{
				GameCanvas.debug("SA86", 2);
				Mob mob13 = null;
				try
				{
					int index5 = msg.reader().readUnsignedByte();
					mob13 = (Mob)GameScr.vMob.elementAt(index5);
				}
				catch (Exception ex26)
				{
					Res.outz("Loi tai NPC_ATTACK_ME " + msg.command + " err= " + ex26.StackTrace);
				}
				if (mob13 != null)
				{
					Char.myCharz().isDie = false;
					Char.isLockKey = false;
					int num177 = msg.readInt3Byte();
					int num178;
					try
					{
						num178 = msg.readInt3Byte();
					}
					catch (Exception)
					{
						num178 = 0;
					}
					if (mob13.isBusyAttackSomeOne)
					{
						Char.myCharz().doInjure(num177, num178, isCrit: false, isMob: true);
						break;
					}
					mob13.dame = num177;
					mob13.dameMp = num178;
					mob13.setAttack(Char.myCharz());
				}
				break;
			}
			case -10:
			{
				GameCanvas.debug("SA87", 2);
				Mob mob18 = null;
				try
				{
					mob18 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
				}
				GameCanvas.debug("SA87x1", 2);
				if (mob18 != null)
				{
					GameCanvas.debug("SA87x2", 2);
					@char = GameScr.findCharInMap(msg.reader().readInt());
					if (@char == null)
					{
						return;
					}
					GameCanvas.debug("SA87x3", 2);
					int num187 = msg.readInt3Byte();
					mob18.dame = @char.cHP - num187;
					@char.cHPNew = num187;
					GameCanvas.debug("SA87x4", 2);
					try
					{
						@char.cMP = msg.readInt3Byte();
					}
					catch (Exception)
					{
					}
					GameCanvas.debug("SA87x5", 2);
					if (mob18.isBusyAttackSomeOne)
					{
						@char.doInjure(mob18.dame, 0, isCrit: false, isMob: true);
					}
					else
					{
						mob18.setAttack(@char);
					}
					GameCanvas.debug("SA87x6", 2);
				}
				break;
			}
			case -17:
				GameCanvas.debug("SA88", 2);
				Char.myCharz().meDead = true;
				Char.myCharz().cPk = msg.reader().readByte();
				Char.myCharz().startDie(msg.reader().readShort(), msg.reader().readShort());
				try
				{
					Char.myCharz().cPower = msg.reader().readLong();
					Char.myCharz().applyCharLevelPercent();
				}
				catch (Exception)
				{
					Cout.println("Loi tai ME_DIE " + msg.command);
				}
				Char.myCharz().countKill = 0;
				break;
			case 66:
				Res.outz("ME DIE XP DOWN NOT IMPLEMENT YET!!!!!!!!!!!!!!!!!!!!!!!!!!");
				break;
			case -8:
				GameCanvas.debug("SA89", 2);
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
				{
					return;
				}
				@char.cPk = msg.reader().readByte();
				@char.waitToDie(msg.reader().readShort(), msg.reader().readShort());
				break;
			case -16:
				GameCanvas.debug("SA90", 2);
				if (Char.myCharz().wdx != 0 || Char.myCharz().wdy != 0)
				{
					Char.myCharz().cx = Char.myCharz().wdx;
					Char.myCharz().cy = Char.myCharz().wdy;
					Char.myCharz().wdx = (Char.myCharz().wdy = 0);
				}
				Char.myCharz().liveFromDead();
				Char.myCharz().isLockMove = false;
				Char.myCharz().meDead = false;
				break;
			case 44:
			{
				GameCanvas.debug("SA91", 2);
				int num179 = msg.reader().readInt();
				string text8 = msg.reader().readUTF();
				Res.outz("user id= " + num179 + " text= " + text8);
				@char = ((Char.myCharz().charID != num179) ? GameScr.findCharInMap(num179) : Char.myCharz());
				if (@char == null)
				{
					return;
				}
				@char.addInfo(text8);
				break;
			}
			case 18:
			{
				sbyte b64 = msg.reader().readByte();
				for (int num176 = 0; num176 < b64; num176++)
				{
					int charId = msg.reader().readInt();
					int cx = msg.reader().readShort();
					int cy = msg.reader().readShort();
					int cHPShow = msg.readInt3Byte();
					Char char13 = GameScr.findCharInMap(charId);
					if (char13 != null)
					{
						char13.cx = cx;
						char13.cy = cy;
						char13.cHP = (char13.cHPShow = cHPShow);
						char13.lastUpdateTime = mSystem.currentTimeMillis();
					}
				}
				break;
			}
			case 19:
				Char.myCharz().countKill = msg.reader().readUnsignedShort();
				Char.myCharz().countKillMax = msg.reader().readUnsignedShort();
				break;
			}
			GameCanvas.debug("SA92", 2);
		}
		catch (Exception ex40)
		{
			Res.outz("Controller = " + ex40.StackTrace);
		}
		finally
		{
			msg?.cleanup();
		}
	}

	private void createItem(myReader d)
	{
		GameScr.vcItem = d.readByte();
		ItemTemplates.itemTemplates.clear();
		GameScr.gI().iOptionTemplates = new ItemOptionTemplate[d.readUnsignedByte()];
		for (int i = 0; i < GameScr.gI().iOptionTemplates.Length; i++)
		{
			GameScr.gI().iOptionTemplates[i] = new ItemOptionTemplate();
			GameScr.gI().iOptionTemplates[i].id = i;
			GameScr.gI().iOptionTemplates[i].name = d.readUTF();
			GameScr.gI().iOptionTemplates[i].type = d.readByte();
		}
		int num = d.readShort();
		for (int j = 0; j < num; j++)
		{
			ItemTemplate it = new ItemTemplate((short)j, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBool());
			ItemTemplates.add(it);
		}
	}

	private void createSkill(myReader d)
	{
		GameScr.vcSkill = d.readByte();
		GameScr.gI().sOptionTemplates = new SkillOptionTemplate[d.readByte()];
		for (int i = 0; i < GameScr.gI().sOptionTemplates.Length; i++)
		{
			GameScr.gI().sOptionTemplates[i] = new SkillOptionTemplate();
			GameScr.gI().sOptionTemplates[i].id = i;
			GameScr.gI().sOptionTemplates[i].name = d.readUTF();
		}
		GameScr.nClasss = new NClass[d.readByte()];
		for (int j = 0; j < GameScr.nClasss.Length; j++)
		{
			GameScr.nClasss[j] = new NClass();
			GameScr.nClasss[j].classId = j;
			GameScr.nClasss[j].name = d.readUTF();
			GameScr.nClasss[j].skillTemplates = new SkillTemplate[d.readByte()];
			for (int k = 0; k < GameScr.nClasss[j].skillTemplates.Length; k++)
			{
				GameScr.nClasss[j].skillTemplates[k] = new SkillTemplate();
				GameScr.nClasss[j].skillTemplates[k].id = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].name = d.readUTF();
				GameScr.nClasss[j].skillTemplates[k].maxPoint = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].manaUseType = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].type = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].iconId = d.readShort();
				GameScr.nClasss[j].skillTemplates[k].damInfo = d.readUTF();
				int lineWidth = 130;
				if (GameCanvas.w == 128 || GameCanvas.h <= 208)
				{
					lineWidth = 100;
				}
				GameScr.nClasss[j].skillTemplates[k].description = mFont.tahoma_7_green2.splitFontArray(d.readUTF(), lineWidth);
				GameScr.nClasss[j].skillTemplates[k].skills = new Skill[d.readByte()];
				for (int l = 0; l < GameScr.nClasss[j].skillTemplates[k].skills.Length; l++)
				{
					GameScr.nClasss[j].skillTemplates[k].skills[l] = new Skill();
					GameScr.nClasss[j].skillTemplates[k].skills[l].skillId = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].template = GameScr.nClasss[j].skillTemplates[k];
					GameScr.nClasss[j].skillTemplates[k].skills[l].point = d.readByte();
					GameScr.nClasss[j].skillTemplates[k].skills[l].powRequire = d.readLong();
					GameScr.nClasss[j].skillTemplates[k].skills[l].manaUse = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].coolDown = d.readInt();
					GameScr.nClasss[j].skillTemplates[k].skills[l].dx = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].dy = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].maxFight = d.readByte();
					GameScr.nClasss[j].skillTemplates[k].skills[l].damage = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].price = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].moreInfo = d.readUTF();
					Skills.add(GameScr.nClasss[j].skillTemplates[k].skills[l]);
				}
			}
		}
	}

	private void createMap(myReader d)
	{
		GameScr.vcMap = d.readByte();
		TileMap.mapNames = new string[d.readUnsignedByte()];
		for (int i = 0; i < TileMap.mapNames.Length; i++)
		{
			TileMap.mapNames[i] = d.readUTF();
		}
		Npc.arrNpcTemplate = new NpcTemplate[d.readByte()];
		for (sbyte b = 0; b < Npc.arrNpcTemplate.Length; b++)
		{
			Npc.arrNpcTemplate[b] = new NpcTemplate();
			Npc.arrNpcTemplate[b].npcTemplateId = b;
			Npc.arrNpcTemplate[b].name = d.readUTF();
			Npc.arrNpcTemplate[b].headId = d.readShort();
			Npc.arrNpcTemplate[b].bodyId = d.readShort();
			Npc.arrNpcTemplate[b].legId = d.readShort();
			Npc.arrNpcTemplate[b].menu = new string[d.readByte()][];
			for (int j = 0; j < Npc.arrNpcTemplate[b].menu.Length; j++)
			{
				Npc.arrNpcTemplate[b].menu[j] = new string[d.readByte()];
				for (int k = 0; k < Npc.arrNpcTemplate[b].menu[j].Length; k++)
				{
					Npc.arrNpcTemplate[b].menu[j][k] = d.readUTF();
				}
			}
		}
		Mob.arrMobTemplate = new MobTemplate[d.readByte()];
		for (sbyte b2 = 0; b2 < Mob.arrMobTemplate.Length; b2++)
		{
			Mob.arrMobTemplate[b2] = new MobTemplate();
			Mob.arrMobTemplate[b2].mobTemplateId = b2;
			Mob.arrMobTemplate[b2].type = d.readByte();
			Mob.arrMobTemplate[b2].name = d.readUTF();
			Mob.arrMobTemplate[b2].hp = d.readInt();
			Mob.arrMobTemplate[b2].rangeMove = d.readByte();
			Mob.arrMobTemplate[b2].speed = d.readByte();
			Mob.arrMobTemplate[b2].dartType = d.readByte();
		}
	}

	private void createData(myReader d, bool isSaveRMS)
	{
		GameScr.vcData = d.readByte();
		if (isSaveRMS)
		{
			Rms.saveRMS("NR_dart", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_arrow", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_effect", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_image", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_part", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_skill", NinjaUtil.readByteArray(d));
			Rms.DeleteStorage("NRdata");
		}
	}

	private Image createImage(sbyte[] arr)
	{
		try
		{
			return Image.createImage(arr, 0, arr.Length);
		}
		catch (Exception)
		{
		}
		return null;
	}

	public int[] arrayByte2Int(sbyte[] b)
	{
		int[] array = new int[b.Length];
		for (int i = 0; i < b.Length; i++)
		{
			int num = b[i];
			if (num < 0)
			{
				num += 256;
			}
			array[i] = num;
		}
		return array;
	}

	public void readClanMsg(Message msg, int index)
	{
		try
		{
			ClanMessage clanMessage = new ClanMessage();
			sbyte b = (sbyte)(clanMessage.type = msg.reader().readByte());
			clanMessage.id = msg.reader().readInt();
			clanMessage.playerId = msg.reader().readInt();
			clanMessage.playerName = msg.reader().readUTF();
			clanMessage.role = msg.reader().readByte();
			clanMessage.time = msg.reader().readInt() + 1000000000;
			bool flag = false;
			GameScr.isNewClanMessage = false;
			switch (b)
			{
			case 0:
			{
				string text = msg.reader().readUTF();
				GameScr.isNewClanMessage = true;
				if (mFont.tahoma_7.getWidth(text) > Panel.WIDTH_PANEL - 60)
				{
					clanMessage.chat = mFont.tahoma_7.splitFontArray(text, Panel.WIDTH_PANEL - 10);
				}
				else
				{
					clanMessage.chat = new string[1];
					clanMessage.chat[0] = text;
				}
				clanMessage.color = msg.reader().readByte();
				break;
			}
			case 1:
				clanMessage.recieve = msg.reader().readByte();
				clanMessage.maxCap = msg.reader().readByte();
				flag = msg.reader().readByte() == 1;
				if (flag)
				{
					GameScr.isNewClanMessage = true;
				}
				if (clanMessage.playerId != Char.myCharz().charID)
				{
					if (clanMessage.recieve < clanMessage.maxCap)
					{
						clanMessage.option = new string[1] { mResources.donate };
					}
					else
					{
						clanMessage.option = null;
					}
				}
				if (GameCanvas.panel.cp != null)
				{
					GameCanvas.panel.updateRequest(clanMessage.recieve, clanMessage.maxCap);
				}
				break;
			case 2:
				if (Char.myCharz().role == 0)
				{
					GameScr.isNewClanMessage = true;
					clanMessage.option = new string[2]
					{
						mResources.CANCEL,
						mResources.receive
					};
				}
				break;
			}
			if (GameCanvas.currentScreen != GameScr.instance)
			{
				GameScr.isNewClanMessage = false;
			}
			else if (GameCanvas.panel.isShow && GameCanvas.panel.type == 0 && GameCanvas.panel.currentTabIndex == 3)
			{
				GameScr.isNewClanMessage = false;
			}
			ClanMessage.addMessage(clanMessage, index, flag);
		}
		catch (Exception)
		{
			Cout.println("LOI TAI CMD -= " + msg.command);
		}
	}

	public void loadCurrMap(sbyte teleport3)
	{
		Res.outz("is loading map = " + Char.isLoadingMap);
		GameScr.gI().auto = 0;
		GameScr.isChangeZone = false;
		CreateCharScr.instance = null;
		GameScr.info1.isUpdate = false;
		GameScr.info2.isUpdate = false;
		GameScr.lockTick = 0;
		GameCanvas.panel.isShow = false;
		SoundMn.gI().stopAll();
		if (!GameScr.isLoadAllData && !CreateCharScr.isCreateChar)
		{
			GameScr.gI().initSelectChar();
		}
		GameScr.loadCamera(fullmScreen: false, (teleport3 != 1) ? (-1) : Char.myCharz().cx, (teleport3 == 0) ? (-1) : 0);
		TileMap.loadMainTile();
		TileMap.loadMap(TileMap.tileID);
		Res.outz("LOAD GAMESCR 2");
		Char.myCharz().cvx = 0;
		Char.myCharz().statusMe = 4;
		Char.myCharz().currentMovePoint = null;
		Char.myCharz().mobFocus = null;
		Char.myCharz().charFocus = null;
		Char.myCharz().npcFocus = null;
		Char.myCharz().itemFocus = null;
		Char.myCharz().skillPaint = null;
		Char.myCharz().setMabuHold(m: false);
		Char.myCharz().skillPaintRandomPaint = null;
		GameCanvas.clearAllPointerEvent();
		if (Char.myCharz().cy >= TileMap.pxh - 100)
		{
			Char.myCharz().isFlyUp = true;
			Char.myCharz().cx += Res.abs(Res.random(0, 80));
			Service.gI().charMove();
		}
		GameScr.gI().loadGameScr();
		GameCanvas.loadBG(TileMap.bgID);
		Char.isLockKey = false;
		Res.outz("cy= " + Char.myCharz().cy + "---------------------------------------------");
		for (int i = 0; i < Char.myCharz().vEff.size(); i++)
		{
			EffectChar effectChar = (EffectChar)Char.myCharz().vEff.elementAt(i);
			if (effectChar.template.type == 10)
			{
				Char.isLockKey = true;
				break;
			}
		}
		GameCanvas.clearKeyHold();
		GameCanvas.clearKeyPressed();
		GameScr.gI().dHP = Char.myCharz().cHP;
		GameScr.gI().dMP = Char.myCharz().cMP;
		Char.ischangingMap = false;
		GameScr.gI().switchToMe();
		if (Char.myCharz().cy <= 10 && teleport3 != 0 && teleport3 != 2)
		{
			Teleport p = new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 1, isMe: true, (teleport3 != 1) ? teleport3 : Char.myCharz().cgender);
			Teleport.addTeleport(p);
			Char.myCharz().isTeleport = true;
		}
		if (teleport3 == 2)
		{
			Char.myCharz().show();
		}
		if (GameScr.gI().isRongThanXuatHien)
		{
			if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
			{
				GameScr.gI().callRongThan(GameScr.gI().xR, GameScr.gI().yR);
			}
			if (mGraphics.zoomLevel > 1)
			{
				GameScr.gI().doiMauTroi();
			}
		}
		InfoDlg.hide();
		InfoDlg.show(TileMap.mapName, mResources.zone + " " + TileMap.zoneID, 30);
		GameCanvas.endDlg();
		GameCanvas.isLoading = false;
		Hint.clickMob();
		Hint.clickNpc();
		GameCanvas.debug("SA75x9", 2);
	}

	public void loadInfoMap(Message msg)
	{
		try
		{
			if (mGraphics.zoomLevel == 1)
			{
				SmallImage.clearHastable();
			}
			Char.myCharz().cx = (Char.myCharz().cxSend = (Char.myCharz().cxFocus = msg.reader().readShort()));
			Char.myCharz().cy = (Char.myCharz().cySend = (Char.myCharz().cyFocus = msg.reader().readShort()));
			Char.myCharz().xSd = Char.myCharz().cx;
			Char.myCharz().ySd = Char.myCharz().cy;
			Res.outz("head= " + Char.myCharz().head + " body= " + Char.myCharz().body + " left= " + Char.myCharz().leg + " x= " + Char.myCharz().cx + " y= " + Char.myCharz().cy + " chung toc= " + Char.myCharz().cgender);
			if (Char.myCharz().cx >= 0 && Char.myCharz().cx <= 100)
			{
				Char.myCharz().cdir = 1;
			}
			else if (Char.myCharz().cx >= TileMap.tmw - 100 && Char.myCharz().cx <= TileMap.tmw)
			{
				Char.myCharz().cdir = -1;
			}
			GameCanvas.debug("SA75x4", 2);
			int num = msg.reader().readByte();
			Res.outz("vGo size= " + num);
			if (!GameScr.info1.isDone)
			{
				GameScr.info1.cmx = Char.myCharz().cx - GameScr.cmx;
				GameScr.info1.cmy = Char.myCharz().cy - GameScr.cmy;
			}
			for (int i = 0; i < num; i++)
			{
				Waypoint waypoint = new Waypoint(msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readUTF());
				if ((TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23) || waypoint.minX < 0 || waypoint.minX <= 24)
				{
				}
			}
			Resources.UnloadUnusedAssets();
			GC.Collect();
			GameCanvas.debug("SA75x5", 2);
			num = msg.reader().readByte();
			Mob.newMob.removeAllElements();
			for (sbyte b = 0; b < num; b++)
			{
				Mob mob = new Mob(b, msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readByte(), msg.reader().readByte(), msg.reader().readInt(), msg.reader().readByte(), msg.reader().readInt(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readByte(), msg.reader().readByte());
				mob.xSd = mob.x;
				mob.ySd = mob.y;
				mob.isBoss = msg.reader().readBoolean();
				if (Mob.arrMobTemplate[mob.templateId].type != 0)
				{
					if (b % 3 == 0)
					{
						mob.dir = -1;
					}
					else
					{
						mob.dir = 1;
					}
					mob.x += 10 - b % 20;
				}
				mob.isMobMe = false;
				BigBoss bigBoss = null;
				BachTuoc bachTuoc = null;
				BigBoss2 bigBoss2 = null;
				NewBoss newBoss = null;
				if (mob.templateId == 70)
				{
					bigBoss = new BigBoss(b, (short)mob.x, (short)mob.y, 70, mob.hp, mob.maxHp, mob.sys);
				}
				if (mob.templateId == 71)
				{
					bachTuoc = new BachTuoc(b, (short)mob.x, (short)mob.y, 71, mob.hp, mob.maxHp, mob.sys);
				}
				if (mob.templateId == 72)
				{
					bigBoss2 = new BigBoss2(b, (short)mob.x, (short)mob.y, 72, mob.hp, mob.maxHp, 3);
				}
				if (mob.isBoss)
				{
					newBoss = new NewBoss(b, (short)mob.x, (short)mob.y, mob.templateId, mob.hp, mob.maxHp, mob.sys);
				}
				if (newBoss != null)
				{
					GameScr.vMob.addElement(newBoss);
				}
				else if (bigBoss != null)
				{
					GameScr.vMob.addElement(bigBoss);
				}
				else if (bachTuoc != null)
				{
					GameScr.vMob.addElement(bachTuoc);
				}
				else if (bigBoss2 != null)
				{
					GameScr.vMob.addElement(bigBoss2);
				}
				else
				{
					GameScr.vMob.addElement(mob);
				}
			}
			for (int j = 0; j < Mob.lastMob.size(); j++)
			{
				string text = (string)Mob.lastMob.elementAt(j);
				if (!Mob.isExistNewMob(text))
				{
					Mob.arrMobTemplate[int.Parse(text)].data = null;
					Mob.lastMob.removeElementAt(j);
					j--;
				}
			}
			if (Char.myCharz().mobMe != null && GameScr.findMobInMap(Char.myCharz().mobMe.mobId) == null)
			{
				Char.myCharz().mobMe.getData();
				Char.myCharz().mobMe.x = Char.myCharz().cx;
				Char.myCharz().mobMe.y = Char.myCharz().cy - 40;
				GameScr.vMob.addElement(Char.myCharz().mobMe);
			}
			num = msg.reader().readByte();
			for (byte b2 = 0; b2 < num; b2++)
			{
			}
			GameCanvas.debug("SA75x6", 2);
			num = msg.reader().readByte();
			Res.outz("NPC size= " + num);
			for (int k = 0; k < num; k++)
			{
				sbyte status = msg.reader().readByte();
				short cx = msg.reader().readShort();
				short num2 = msg.reader().readShort();
				sbyte b3 = msg.reader().readByte();
				short num3 = msg.reader().readShort();
				if (b3 != 6 && ((Char.myCharz().taskMaint.taskId >= 7 && (Char.myCharz().taskMaint.taskId != 7 || Char.myCharz().taskMaint.index > 1)) || (b3 != 7 && b3 != 8 && b3 != 9)) && (Char.myCharz().taskMaint.taskId >= 6 || b3 != 16))
				{
					if (b3 == 4)
					{
						GameScr.gI().magicTree = new MagicTree(k, status, cx, num2, b3, num3);
						Service.gI().magicTree(2);
						GameScr.vNpc.addElement(GameScr.gI().magicTree);
					}
					else
					{
						Npc o = new Npc(k, status, cx, num2 + 3, b3, num3);
						GameScr.vNpc.addElement(o);
					}
				}
			}
			GameCanvas.debug("SA75x7", 2);
			num = msg.reader().readByte();
			Res.outz("item size = " + num);
			for (int l = 0; l < num; l++)
			{
				short itemMapID = msg.reader().readShort();
				short itemTemplateID = msg.reader().readShort();
				int x = msg.reader().readShort();
				int y = msg.reader().readShort();
				int num4 = msg.reader().readInt();
				short r = 0;
				if (num4 == -2)
				{
					r = msg.reader().readShort();
				}
				ItemMap itemMap = new ItemMap(num4, itemMapID, itemTemplateID, x, y, r);
				bool flag = false;
				for (int m = 0; m < GameScr.vItemMap.size(); m++)
				{
					ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(m);
					if (itemMap2.itemMapID == itemMap.itemMapID)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					GameScr.vItemMap.addElement(itemMap);
				}
			}
			TileMap.vCurrItem.removeAllElements();
			if (mGraphics.zoomLevel == 1)
			{
				BgItem.clearHashTable();
			}
			BgItem.vKeysNew.removeAllElements();
			if (!GameCanvas.lowGraphic || (GameCanvas.lowGraphic && TileMap.isVoDaiMap()) || TileMap.mapID == 45 || TileMap.mapID == 46 || TileMap.mapID == 47 || TileMap.mapID == 48)
			{
				short num5 = msg.reader().readShort();
				Res.outz("nItem= " + num5);
				for (int n = 0; n < num5; n++)
				{
					short id = msg.reader().readShort();
					short num6 = msg.reader().readShort();
					short num7 = msg.reader().readShort();
					if (TileMap.getBIById(id) == null)
					{
						continue;
					}
					BgItem bIById = TileMap.getBIById(id);
					BgItem bgItem = new BgItem();
					bgItem.id = id;
					bgItem.idImage = bIById.idImage;
					bgItem.dx = bIById.dx;
					bgItem.dy = bIById.dy;
					bgItem.x = num6 * TileMap.size;
					bgItem.y = num7 * TileMap.size;
					bgItem.layer = bIById.layer;
					if (TileMap.isExistMoreOne(bgItem.id))
					{
						bgItem.trans = ((n % 2 != 0) ? 2 : 0);
						if (TileMap.mapID == 45)
						{
							bgItem.trans = 0;
						}
					}
					Image image = null;
					if (!BgItem.imgNew.containsKey(bgItem.idImage + string.Empty))
					{
						if (mGraphics.zoomLevel == 1)
						{
							image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
							if (image == null)
							{
								image = Image.createRGBImage(new int[1], 1, 1, bl: true);
								Service.gI().getBgTemplate(bgItem.idImage);
							}
							BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
						}
						else
						{
							bool flag2 = false;
							sbyte[] array = Rms.loadRMS(mGraphics.zoomLevel + "bgItem" + bgItem.idImage);
							if (array != null)
							{
								if (BgItem.newSmallVersion != null)
								{
									Res.outz("Small  last= " + array.Length % 127 + "new Version= " + BgItem.newSmallVersion[bgItem.idImage]);
									if (array.Length % 127 != BgItem.newSmallVersion[bgItem.idImage])
									{
										flag2 = true;
									}
								}
								if (!flag2)
								{
									image = Image.createImage(array, 0, array.Length);
									if (image != null)
									{
										BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
									}
									else
									{
										flag2 = true;
									}
								}
							}
							else
							{
								flag2 = true;
							}
							if (flag2)
							{
								image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
								if (image == null)
								{
									image = Image.createRGBImage(new int[1], 1, 1, bl: true);
									Service.gI().getBgTemplate(bgItem.idImage);
								}
								BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
							}
						}
						BgItem.vKeysLast.addElement(bgItem.idImage + string.Empty);
					}
					if (!BgItem.isExistKeyNews(bgItem.idImage + string.Empty))
					{
						BgItem.vKeysNew.addElement(bgItem.idImage + string.Empty);
					}
					bgItem.changeColor();
					TileMap.vCurrItem.addElement(bgItem);
				}
				for (int num8 = 0; num8 < BgItem.vKeysLast.size(); num8++)
				{
					string text2 = (string)BgItem.vKeysLast.elementAt(num8);
					if (!BgItem.isExistKeyNews(text2))
					{
						BgItem.imgNew.remove(text2);
						if (BgItem.imgNew.containsKey(text2 + "blend" + 1))
						{
							BgItem.imgNew.remove(text2 + "blend" + 1);
						}
						if (BgItem.imgNew.containsKey(text2 + "blend" + 3))
						{
							BgItem.imgNew.remove(text2 + "blend" + 3);
						}
						BgItem.vKeysLast.removeElementAt(num8);
						num8--;
					}
				}
				BackgroudEffect.isFog = false;
				BackgroudEffect.nCloud = 0;
				EffecMn.vEff.removeAllElements();
				BackgroudEffect.vBgEffect.removeAllElements();
				Effect.newEff.removeAllElements();
				short num9 = msg.reader().readShort();
				for (int num10 = 0; num10 < num9; num10++)
				{
					string key = msg.reader().readUTF();
					string value = msg.reader().readUTF();
					keyValueAction(key, value);
				}
				for (int num11 = 0; num11 < Effect.lastEff.size(); num11++)
				{
					string text3 = (string)Effect.lastEff.elementAt(num11);
					if (!Effect.isExistNewEff(text3))
					{
						Effect.removeEffData(int.Parse(text3));
						Effect.lastEff.removeElementAt(num11);
						num11--;
					}
				}
			}
			else
			{
				short num12 = msg.reader().readShort();
				for (int num13 = 0; num13 < num12; num13++)
				{
					short num14 = msg.reader().readShort();
					short num15 = msg.reader().readShort();
					short num16 = msg.reader().readShort();
				}
				short num17 = msg.reader().readShort();
				for (int num18 = 0; num18 < num17; num18++)
				{
					string text4 = msg.reader().readUTF();
					string text5 = msg.reader().readUTF();
				}
			}
			TileMap.bgType = msg.reader().readByte();
			sbyte teleport = msg.reader().readByte();
			loadCurrMap(teleport);
			Char.isLoadingMap = false;
			GameCanvas.debug("SA75x8", 2);
			Resources.UnloadUnusedAssets();
			GC.Collect();
			Cout.LogError("----------DA CHAY XONG LOAD INFO MAP");
		}
		catch (Exception ex)
		{
			FunctionXmap.FixBlackScreen();
			Res.err("LOI TAI LOADMAP INFO " + ex.StackTrace);
		}
	}

	public void keyValueAction(string key, string value)
	{
		if (key.Equals("eff"))
		{
			if (Panel.graphics > 0)
			{
				return;
			}
			string[] array = Res.split(value, ".", 0);
			int id = int.Parse(array[0]);
			int layer = int.Parse(array[1]);
			int x = int.Parse(array[2]);
			int y = int.Parse(array[3]);
			int loop;
			int loopCount;
			if (array.Length <= 4)
			{
				loop = -1;
				loopCount = 1;
			}
			else
			{
				loop = int.Parse(array[4]);
				loopCount = int.Parse(array[5]);
			}
			Effect effect = new Effect(id, x, y, layer, loop, loopCount);
			if (array.Length > 6)
			{
				effect.typeEff = int.Parse(array[6]);
				if (array.Length > 7)
				{
					effect.indexFrom = int.Parse(array[7]);
					effect.indexTo = int.Parse(array[8]);
				}
			}
			EffecMn.addEff(effect);
		}
		else if (key.Equals("beff") && Panel.graphics <= 1)
		{
			BackgroudEffect.addEffect(int.Parse(value));
		}
	}

	public void messageNotMap(Message msg)
	{
		GameCanvas.debug("SA6", 2);
		try
		{
			sbyte b = msg.reader().readByte();
			Res.outz("---messageNotMap : " + b);
			switch (b)
			{
			case 16:
				MoneyCharge.gI().switchToMe();
				break;
			case 17:
				GameCanvas.debug("SYB123", 2);
				Char.myCharz().clearTask();
				break;
			case 18:
			{
				GameCanvas.isLoading = false;
				GameCanvas.endDlg();
				int num2 = msg.reader().readInt();
				GameCanvas.inputDlg.show(mResources.changeNameChar, new Command(mResources.OK, GameCanvas.instance, 88829, num2), TField.INPUT_TYPE_ANY);
				break;
			}
			case 20:
				Char.myCharz().cPk = msg.reader().readByte();
				GameScr.info1.addInfo(mResources.PK_NOW + " " + Char.myCharz().cPk, 0);
				break;
			case 35:
				GameCanvas.endDlg();
				GameScr.gI().resetButton();
				GameScr.info1.addInfo(msg.reader().readUTF(), 0);
				break;
			case 36:
				GameScr.typeActive = msg.reader().readByte();
				Res.outz("load Me Active: " + GameScr.typeActive);
				break;
			case 4:
			{
				GameCanvas.debug("SA8", 2);
				GameCanvas.loginScr.savePass();
				GameScr.isAutoPlay = false;
				GameScr.canAutoPlay = false;
				LoginScr.isUpdateAll = true;
				LoginScr.isUpdateData = true;
				LoginScr.isUpdateMap = true;
				LoginScr.isUpdateSkill = true;
				LoginScr.isUpdateItem = true;
				GameScr.vsData = msg.reader().readByte();
				GameScr.vsMap = msg.reader().readByte();
				GameScr.vsSkill = msg.reader().readByte();
				GameScr.vsItem = msg.reader().readByte();
				sbyte b2 = msg.reader().readByte();
				if (GameCanvas.loginScr.isLogin2)
				{
					Rms.saveRMSString("acc", string.Empty);
					Rms.saveRMSString("pass", string.Empty);
				}
				else
				{
					Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, string.Empty);
				}
				if (GameScr.vsData != GameScr.vcData)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateData();
				}
				else
				{
					try
					{
						LoginScr.isUpdateData = false;
					}
					catch (Exception)
					{
						GameScr.vcData = -1;
						Service.gI().updateData();
					}
				}
				if (GameScr.vsMap != GameScr.vcMap)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateMap();
				}
				else
				{
					try
					{
						if (!GameScr.isLoadAllData)
						{
							DataInputStream dataInputStream = new DataInputStream(Rms.loadRMS("NRmap"));
							createMap(dataInputStream.r);
						}
						LoginScr.isUpdateMap = false;
					}
					catch (Exception)
					{
						GameScr.vcMap = -1;
						Service.gI().updateMap();
					}
				}
				if (GameScr.vsSkill != GameScr.vcSkill)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateSkill();
				}
				else
				{
					try
					{
						if (!GameScr.isLoadAllData)
						{
							DataInputStream dataInputStream2 = new DataInputStream(Rms.loadRMS("NRskill"));
							createSkill(dataInputStream2.r);
						}
						LoginScr.isUpdateSkill = false;
					}
					catch (Exception)
					{
						GameScr.vcSkill = -1;
						Service.gI().updateSkill();
					}
				}
				if (GameScr.vsItem != GameScr.vcItem)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateItem();
				}
				else
				{
					try
					{
						DataInputStream dataInputStream3 = new DataInputStream(Rms.loadRMS("NRitem0"));
						loadItemNew(dataInputStream3.r, 0, isSave: false);
						DataInputStream dataInputStream4 = new DataInputStream(Rms.loadRMS("NRitem1"));
						loadItemNew(dataInputStream4.r, 1, isSave: false);
						DataInputStream dataInputStream5 = new DataInputStream(Rms.loadRMS("NRitem2"));
						loadItemNew(dataInputStream5.r, 2, isSave: false);
						DataInputStream dataInputStream6 = new DataInputStream(Rms.loadRMS("NRitem100"));
						loadItemNew(dataInputStream6.r, 100, isSave: false);
						LoginScr.isUpdateItem = false;
					}
					catch (Exception)
					{
						GameScr.vcItem = -1;
						Service.gI().updateItem();
					}
				}
				if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
				{
					if (!GameScr.isLoadAllData)
					{
						GameScr.gI().readDart();
						GameScr.gI().readEfect();
						GameScr.gI().readArrow();
						GameScr.gI().readSkill();
					}
					Service.gI().clientOk();
				}
				sbyte b3 = msg.reader().readByte();
				Res.outz("CAPTION LENT= " + b3);
				GameScr.exps = new long[b3];
				for (int j = 0; j < GameScr.exps.Length; j++)
				{
					GameScr.exps[j] = msg.reader().readLong();
				}
				break;
			}
			case 6:
			{
				Res.outz("GET UPDATE_MAP " + msg.reader().available() + " bytes");
				msg.reader().mark(100000);
				createMap(msg.reader());
				msg.reader().reset();
				sbyte[] data3 = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data3);
				Rms.saveRMS("NRmap", data3);
				sbyte[] data4 = new sbyte[1] { GameScr.vcMap };
				Rms.saveRMS("NRmapVersion", data4);
				LoginScr.isUpdateMap = false;
				if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
				{
					GameScr.gI().readDart();
					GameScr.gI().readEfect();
					GameScr.gI().readArrow();
					GameScr.gI().readSkill();
					Service.gI().clientOk();
				}
				break;
			}
			case 7:
			{
				Res.outz("GET UPDATE_SKILL " + msg.reader().available() + " bytes");
				msg.reader().mark(100000);
				createSkill(msg.reader());
				msg.reader().reset();
				sbyte[] data = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data);
				Rms.saveRMS("NRskill", data);
				sbyte[] data2 = new sbyte[1] { GameScr.vcSkill };
				Rms.saveRMS("NRskillVersion", data2);
				LoginScr.isUpdateSkill = false;
				if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
				{
					GameScr.gI().readDart();
					GameScr.gI().readEfect();
					GameScr.gI().readArrow();
					GameScr.gI().readSkill();
					Service.gI().clientOk();
				}
				break;
			}
			case 8:
				Res.outz("GET UPDATE_ITEM " + msg.reader().available() + " bytes");
				createItemNew(msg.reader());
				break;
			case 10:
				try
				{
					Char.isLoadingMap = true;
					Res.outz("REQUEST MAP TEMPLATE");
					GameCanvas.isLoading = true;
					TileMap.maps = null;
					TileMap.types = null;
					mSystem.gcc();
					GameCanvas.debug("SA99", 2);
					TileMap.tmw = msg.reader().readByte();
					TileMap.tmh = msg.reader().readByte();
					TileMap.maps = new int[TileMap.tmw * TileMap.tmh];
					Res.outz("   M apsize= " + TileMap.tmw * TileMap.tmh);
					for (int i = 0; i < TileMap.maps.Length; i++)
					{
						int num = msg.reader().readByte();
						if (num < 0)
						{
							num += 256;
						}
						TileMap.maps[i] = (ushort)num;
					}
					TileMap.types = new int[TileMap.maps.Length];
					msg = messWait;
					loadInfoMap(msg);
					try
					{
						TileMap.isMapDouble = ((msg.reader().readByte() != 0) ? true : false);
					}
					catch (Exception)
					{
					}
				}
				catch (Exception ex2)
				{
					Cout.LogError("LOI TAI CASE REQUEST_MAPTEMPLATE " + ex2.ToString());
				}
				msg.cleanup();
				messWait.cleanup();
				msg = (messWait = null);
				break;
			case 12:
				GameCanvas.debug("SA10", 2);
				break;
			case 9:
				GameCanvas.debug("SA11", 2);
				break;
			}
		}
		catch (Exception)
		{
			Cout.LogError("LOI TAI messageNotMap + " + msg.command);
		}
		finally
		{
			msg?.cleanup();
		}
	}

	public void messageNotLogin(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			Res.outz("---messageNotLogin : " + b);
			if (b != 2)
			{
				return;
			}
			string text = msg.reader().readUTF();
			if (mSystem.isTest)
			{
				text = "88:192.168.1.88:20000:0,53:112.213.85.53:20000:0," + text;
			}
			if (Rms.loadRMSInt("AdminLink") == 1)
			{
				return;
			}
			if (mSystem.clientType == 1)
			{
				ServerListScreen.linkDefault = text;
			}
			else
			{
				ServerListScreen.linkDefault = text;
			}
			ServerListScreen.getServerList(ServerListScreen.linkDefault);
			try
			{
				sbyte b2 = msg.reader().readByte();
				Panel.CanNapTien = b2 == 1;
				sbyte x = msg.reader().readByte();
				Rms.saveRMSInt("AdminLink", x);
			}
			catch (Exception)
			{
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			msg?.cleanup();
		}
	}

	public void messageSubCommand(Message msg)
	{
		try
		{
			GameCanvas.debug("SA12", 2);
			sbyte b = msg.reader().readByte();
			Res.outz("---messageSubCommand : " + b);
			switch (b)
			{
			case 63:
			{
				sbyte b5 = msg.reader().readByte();
				if (b5 > 0)
				{
					InfoDlg.showWait();
					MyVector vPlayerMenu = GameCanvas.panel.vPlayerMenu;
					for (int j = 0; j < b5; j++)
					{
						string caption = msg.reader().readUTF();
						string caption2 = msg.reader().readUTF();
						short menuSelect = msg.reader().readShort();
						Char.myCharz().charFocus.menuSelect = menuSelect;
						Command command = new Command(caption, 11115, Char.myCharz().charFocus);
						command.caption2 = caption2;
						vPlayerMenu.addElement(command);
					}
					InfoDlg.hide();
					GameCanvas.panel.setTabPlayerMenu();
				}
				break;
			}
			case 1:
				GameCanvas.debug("SA13", 2);
				Char.myCharz().nClass = GameScr.nClasss[msg.reader().readByte()];
				Char.myCharz().cTiemNang = msg.reader().readLong();
				Char.myCharz().vSkill.removeAllElements();
				Char.myCharz().vSkillFight.removeAllElements();
				Char.myCharz().myskill = null;
				break;
			case 2:
			{
				GameCanvas.debug("SA14", 2);
				if (Char.myCharz().statusMe != 14 && Char.myCharz().statusMe != 5)
				{
					Char.myCharz().cHP = Char.myCharz().cHPFull;
					Char.myCharz().cMP = Char.myCharz().cMPFull;
					Cout.LogError2(" ME_LOAD_SKILL");
				}
				Char.myCharz().vSkill.removeAllElements();
				Char.myCharz().vSkillFight.removeAllElements();
				sbyte b2 = msg.reader().readByte();
				for (sbyte b3 = 0; b3 < b2; b3++)
				{
					short skillId = msg.reader().readShort();
					Skill skill2 = Skills.get(skillId);
					useSkill(skill2);
				}
				GameScr.gI().sortSkill();
				if (GameScr.isPaintInfoMe)
				{
					GameScr.indexRow = -1;
					GameScr.gI().left = (GameScr.gI().center = null);
				}
				break;
			}
			case 19:
				GameCanvas.debug("SA17", 2);
				Char.myCharz().boxSort();
				break;
			case 21:
			{
				GameCanvas.debug("SA19", 2);
				int num3 = msg.reader().readInt();
				Char.myCharz().xuInBox -= num3;
				Char.myCharz().xu += num3;
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				break;
			}
			case 0:
			{
				GameCanvas.debug("SA21", 2);
				RadarScr.list = new MyVector();
				Teleport.vTeleport.removeAllElements();
				GameScr.vCharInMap.removeAllElements();
				GameScr.vItemMap.removeAllElements();
				Char.vItemTime.removeAllElements();
				GameScr.loadImg();
				GameScr.currentCharViewInfo = Char.myCharz();
				Char.myCharz().charID = msg.reader().readInt();
				Char.myCharz().ctaskId = msg.reader().readByte();
				Char.myCharz().cgender = msg.reader().readByte();
				Char.myCharz().head = msg.reader().readShort();
				Char.myCharz().cName = msg.reader().readUTF();
				Char.myCharz().cPk = msg.reader().readByte();
				Char.myCharz().cTypePk = msg.reader().readByte();
				Char.myCharz().cPower = msg.reader().readLong();
				Char.myCharz().applyCharLevelPercent();
				Char.myCharz().eff5BuffHp = msg.reader().readShort();
				Char.myCharz().eff5BuffMp = msg.reader().readShort();
				Char.myCharz().nClass = GameScr.nClasss[msg.reader().readByte()];
				Char.myCharz().vSkill.removeAllElements();
				Char.myCharz().vSkillFight.removeAllElements();
				GameScr.gI().dHP = Char.myCharz().cHP;
				GameScr.gI().dMP = Char.myCharz().cMP;
				sbyte b6 = msg.reader().readByte();
				for (sbyte b7 = 0; b7 < b6; b7++)
				{
					Skill skill3 = Skills.get(msg.reader().readShort());
					useSkill(skill3);
				}
				GameScr.gI().sortSkill();
				GameScr.gI().loadSkillShortcut();
				Char.myCharz().xu = msg.reader().readLong();
				Char.myCharz().luongKhoa = msg.reader().readInt();
				Char.myCharz().luong = msg.reader().readInt();
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
				Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
				Char.myCharz().arrItemBody = new Item[msg.reader().readByte()];
				try
				{
					Char.myCharz().setDefaultPart();
					for (int k = 0; k < Char.myCharz().arrItemBody.Length; k++)
					{
						short num5 = msg.reader().readShort();
						if (num5 == -1)
						{
							continue;
						}
						ItemTemplate itemTemplate = ItemTemplates.get(num5);
						int type = itemTemplate.type;
						Char.myCharz().arrItemBody[k] = new Item();
						Char.myCharz().arrItemBody[k].template = itemTemplate;
						Char.myCharz().arrItemBody[k].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBody[k].info = msg.reader().readUTF();
						Char.myCharz().arrItemBody[k].content = msg.reader().readUTF();
						int num6 = msg.reader().readUnsignedByte();
						if (num6 != 0)
						{
							Char.myCharz().arrItemBody[k].itemOption = new ItemOption[num6];
							for (int l = 0; l < Char.myCharz().arrItemBody[k].itemOption.Length; l++)
							{
								int num7 = msg.reader().readUnsignedByte();
								int param = msg.reader().readUnsignedShort();
								if (num7 != -1)
								{
									Char.myCharz().arrItemBody[k].itemOption[l] = new ItemOption(num7, param);
								}
							}
						}
						switch (type)
						{
						case 0:
							Res.outz("toi day =======================================" + Char.myCharz().body);
							Char.myCharz().body = Char.myCharz().arrItemBody[k].template.part;
							break;
						case 1:
							Char.myCharz().leg = Char.myCharz().arrItemBody[k].template.part;
							Res.outz("toi day =======================================" + Char.myCharz().leg);
							break;
						}
					}
				}
				catch (Exception)
				{
				}
				Char.myCharz().arrItemBag = new Item[msg.reader().readByte()];
				GameScr.hpPotion = 0;
				for (int m = 0; m < Char.myCharz().arrItemBag.Length; m++)
				{
					short num8 = msg.reader().readShort();
					if (num8 == -1)
					{
						continue;
					}
					Char.myCharz().arrItemBag[m] = new Item();
					Char.myCharz().arrItemBag[m].template = ItemTemplates.get(num8);
					Char.myCharz().arrItemBag[m].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBag[m].info = msg.reader().readUTF();
					Char.myCharz().arrItemBag[m].content = msg.reader().readUTF();
					Char.myCharz().arrItemBag[m].indexUI = m;
					sbyte b8 = msg.reader().readByte();
					if (b8 != 0)
					{
						Char.myCharz().arrItemBag[m].itemOption = new ItemOption[b8];
						for (int n = 0; n < Char.myCharz().arrItemBag[m].itemOption.Length; n++)
						{
							int num9 = msg.reader().readUnsignedByte();
							int param2 = msg.reader().readUnsignedShort();
							if (num9 != -1)
							{
								Char.myCharz().arrItemBag[m].itemOption[n] = new ItemOption(num9, param2);
								Char.myCharz().arrItemBag[m].getCompare();
							}
						}
					}
					if (Char.myCharz().arrItemBag[m].template.type == 6)
					{
						GameScr.hpPotion += Char.myCharz().arrItemBag[m].quantity;
					}
				}
				Char.myCharz().arrItemBox = new Item[msg.reader().readByte()];
				GameCanvas.panel.hasUse = 0;
				for (int num10 = 0; num10 < Char.myCharz().arrItemBox.Length; num10++)
				{
					short num11 = msg.reader().readShort();
					if (num11 == -1)
					{
						continue;
					}
					Char.myCharz().arrItemBox[num10] = new Item();
					Char.myCharz().arrItemBox[num10].template = ItemTemplates.get(num11);
					Char.myCharz().arrItemBox[num10].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBox[num10].info = msg.reader().readUTF();
					Char.myCharz().arrItemBox[num10].content = msg.reader().readUTF();
					Char.myCharz().arrItemBox[num10].itemOption = new ItemOption[msg.reader().readByte()];
					for (int num12 = 0; num12 < Char.myCharz().arrItemBox[num10].itemOption.Length; num12++)
					{
						int num13 = msg.reader().readUnsignedByte();
						int param3 = msg.reader().readUnsignedShort();
						if (num13 != -1)
						{
							Char.myCharz().arrItemBox[num10].itemOption[num12] = new ItemOption(num13, param3);
							Char.myCharz().arrItemBox[num10].getCompare();
						}
					}
					GameCanvas.panel.hasUse++;
				}
				Char.myCharz().statusMe = 4;
				int num14 = Rms.loadRMSInt(Char.myCharz().cName + "vci");
				if (num14 < 1)
				{
					GameScr.isViewClanInvite = false;
				}
				else
				{
					GameScr.isViewClanInvite = true;
				}
				short num15 = msg.reader().readShort();
				Char.idHead = new short[num15];
				Char.idAvatar = new short[num15];
				for (int num16 = 0; num16 < num15; num16++)
				{
					Char.idHead[num16] = msg.reader().readShort();
					Char.idAvatar[num16] = msg.reader().readShort();
				}
				for (int num17 = 0; num17 < GameScr.info1.charId.Length; num17++)
				{
					GameScr.info1.charId[num17] = new int[3];
				}
				GameScr.info1.charId[Char.myCharz().cgender][0] = msg.reader().readShort();
				GameScr.info1.charId[Char.myCharz().cgender][1] = msg.reader().readShort();
				GameScr.info1.charId[Char.myCharz().cgender][2] = msg.reader().readShort();
				Char.myCharz().isNhapThe = msg.reader().readByte() == 1;
				Res.outz("NHAP THE= " + Char.myCharz().isNhapThe);
				GameScr.deltaTime = mSystem.currentTimeMillis() - (long)msg.reader().readInt() * 1000L;
				GameScr.isNewMember = msg.reader().readByte();
				Service.gI().updateCaption((sbyte)Char.myCharz().cgender);
				Service.gI().androidPack();
				try
				{
					Char.myCharz().idAuraEff = msg.reader().readShort();
					Char.myCharz().idEff_Set_Item = msg.reader().readSByte();
					Char.myCharz().idHat = msg.reader().readShort();
					break;
				}
				catch (Exception)
				{
					break;
				}
			}
			case 4:
				GameCanvas.debug("SA23", 2);
				Char.myCharz().xu = msg.reader().readLong();
				Char.myCharz().luong = msg.reader().readInt();
				Char.myCharz().cHP = msg.readInt3Byte();
				Char.myCharz().cMP = msg.readInt3Byte();
				Char.myCharz().luongKhoa = msg.reader().readInt();
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
				Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
				break;
			case 5:
			{
				GameCanvas.debug("SA24", 2);
				int cHP = Char.myCharz().cHP;
				Char.myCharz().cHP = msg.readInt3Byte();
				if (Char.myCharz().cHP > cHP && Char.myCharz().cTypePk != 4)
				{
					GameScr.startFlyText("+" + (Char.myCharz().cHP - cHP) + " " + mResources.HP, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 20, 0, -1, mFont.HP);
					SoundMn.gI().HP_MPup();
					if (Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5003)
					{
						MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, isBoss: true, -1, -1, Char.myCharz(), 29);
					}
				}
				if (Char.myCharz().cHP < cHP)
				{
					GameScr.startFlyText("-" + (cHP - Char.myCharz().cHP) + " " + mResources.HP, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 20, 0, -1, mFont.HP);
				}
				GameScr.gI().dHP = Char.myCharz().cHP;
				if (!GameScr.isPaintInfoMe)
				{
				}
				break;
			}
			case 6:
			{
				GameCanvas.debug("SA25", 2);
				if (Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5)
				{
					break;
				}
				int cMP = Char.myCharz().cMP;
				Char.myCharz().cMP = msg.readInt3Byte();
				if (Char.myCharz().cMP > cMP)
				{
					GameScr.startFlyText("+" + (Char.myCharz().cMP - cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
					SoundMn.gI().HP_MPup();
					if (Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5001)
					{
						MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, isBoss: true, -1, -1, Char.myCharz(), 29);
					}
				}
				if (Char.myCharz().cMP < cMP)
				{
					GameScr.startFlyText("-" + (cMP - Char.myCharz().cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
				}
				Res.outz("curr MP= " + Char.myCharz().cMP);
				GameScr.gI().dMP = Char.myCharz().cMP;
				if (!GameScr.isPaintInfoMe)
				{
				}
				break;
			}
			case 7:
			{
				Char char9 = GameScr.findCharInMap(msg.reader().readInt());
				if (char9 == null)
				{
					break;
				}
				char9.clanID = msg.reader().readInt();
				if (char9.clanID == -2)
				{
					char9.isCopy = true;
				}
				readCharInfo(char9, msg);
				try
				{
					char9.idAuraEff = msg.reader().readShort();
					char9.idEff_Set_Item = msg.reader().readSByte();
					char9.idHat = msg.reader().readShort();
					if (char9.bag >= 201)
					{
						Effect effect = new Effect(char9.bag, char9, 2, -1, 10, 1);
						effect.typeEff = 5;
						char9.addEffChar(effect);
					}
					else
					{
						char9.removeEffChar(0, 201);
					}
					break;
				}
				catch (Exception)
				{
					break;
				}
			}
			case 8:
			{
				GameCanvas.debug("SA26", 2);
				Char char10 = GameScr.findCharInMap(msg.reader().readInt());
				if (char10 != null)
				{
					char10.cspeed = msg.reader().readByte();
				}
				break;
			}
			case 9:
			{
				GameCanvas.debug("SA27", 2);
				Char char8 = GameScr.findCharInMap(msg.reader().readInt());
				if (char8 != null)
				{
					char8.cHP = msg.readInt3Byte();
					char8.cHPFull = msg.readInt3Byte();
				}
				break;
			}
			case 10:
			{
				GameCanvas.debug("SA28", 2);
				Char char5 = GameScr.findCharInMap(msg.reader().readInt());
				if (char5 != null)
				{
					char5.cHP = msg.readInt3Byte();
					char5.cHPFull = msg.readInt3Byte();
					char5.eff5BuffHp = msg.reader().readShort();
					char5.eff5BuffMp = msg.reader().readShort();
					char5.wp = msg.reader().readShort();
					if (char5.wp == -1)
					{
						char5.setDefaultWeapon();
					}
				}
				break;
			}
			case 11:
			{
				GameCanvas.debug("SA29", 2);
				Char char2 = GameScr.findCharInMap(msg.reader().readInt());
				if (char2 != null)
				{
					char2.cHP = msg.readInt3Byte();
					char2.cHPFull = msg.readInt3Byte();
					char2.eff5BuffHp = msg.reader().readShort();
					char2.eff5BuffMp = msg.reader().readShort();
					char2.body = msg.reader().readShort();
					if (char2.body == -1)
					{
						char2.setDefaultBody();
					}
				}
				break;
			}
			case 12:
			{
				GameCanvas.debug("SA30", 2);
				Char char11 = GameScr.findCharInMap(msg.reader().readInt());
				if (char11 != null)
				{
					char11.cHP = msg.readInt3Byte();
					char11.cHPFull = msg.readInt3Byte();
					char11.eff5BuffHp = msg.reader().readShort();
					char11.eff5BuffMp = msg.reader().readShort();
					char11.leg = msg.reader().readShort();
					if (char11.leg == -1)
					{
						char11.setDefaultLeg();
					}
				}
				break;
			}
			case 13:
			{
				GameCanvas.debug("SA31", 2);
				int num2 = msg.reader().readInt();
				Char @char = ((num2 != Char.myCharz().charID) ? GameScr.findCharInMap(num2) : Char.myCharz());
				if (@char != null)
				{
					@char.cHP = msg.readInt3Byte();
					@char.cHPFull = msg.readInt3Byte();
					@char.eff5BuffHp = msg.reader().readShort();
					@char.eff5BuffMp = msg.reader().readShort();
				}
				break;
			}
			case 14:
			{
				GameCanvas.debug("SA32", 2);
				Char char4 = GameScr.findCharInMap(msg.reader().readInt());
				if (char4 != null)
				{
					char4.cHP = msg.readInt3Byte();
					sbyte b4 = msg.reader().readByte();
					Res.outz("player load hp type= " + b4);
					if (b4 == 1)
					{
						ServerEffect.addServerEffect(11, char4, 5);
						ServerEffect.addServerEffect(104, char4, 4);
					}
					if (b4 == 2)
					{
						char4.doInjure();
					}
					try
					{
						char4.cHPFull = msg.readInt3Byte();
						break;
					}
					catch (Exception)
					{
						break;
					}
				}
				break;
			}
			case 15:
			{
				GameCanvas.debug("SA33", 2);
				Char char3 = GameScr.findCharInMap(msg.reader().readInt());
				if (char3 != null)
				{
					char3.cHP = msg.readInt3Byte();
					char3.cHPFull = msg.readInt3Byte();
					char3.cx = msg.reader().readShort();
					char3.cy = msg.reader().readShort();
					char3.statusMe = 1;
					char3.cp3 = 3;
					ServerEffect.addServerEffect(109, char3, 2);
				}
				break;
			}
			case 35:
			{
				GameCanvas.debug("SY3", 2);
				int num4 = msg.reader().readInt();
				Res.outz("CID = " + num4);
				if (TileMap.mapID == 130)
				{
					GameScr.gI().starVS();
				}
				if (num4 == Char.myCharz().charID)
				{
					Char.myCharz().cTypePk = msg.reader().readByte();
					if (GameScr.gI().isVS() && Char.myCharz().cTypePk != 0)
					{
						GameScr.gI().starVS();
					}
					Res.outz("type pk= " + Char.myCharz().cTypePk);
					Char.myCharz().npcFocus = null;
					if (!GameScr.gI().isMeCanAttackMob(Char.myCharz().mobFocus))
					{
						Char.myCharz().mobFocus = null;
					}
					Char.myCharz().itemFocus = null;
				}
				else
				{
					Char char6 = GameScr.findCharInMap(num4);
					if (char6 != null)
					{
						Res.outz("type pk= " + char6.cTypePk);
						char6.cTypePk = msg.reader().readByte();
						if (char6.isAttacPlayerStatus())
						{
							Char.myCharz().charFocus = char6;
						}
					}
				}
				for (int i = 0; i < GameScr.vCharInMap.size(); i++)
				{
					Char char7 = GameScr.findCharInMap(i);
					if (char7 != null && char7.cTypePk != 0 && char7.cTypePk == Char.myCharz().cTypePk)
					{
						if (!Char.myCharz().mobFocus.isMobMe)
						{
							Char.myCharz().mobFocus = null;
						}
						Char.myCharz().npcFocus = null;
						Char.myCharz().itemFocus = null;
						break;
					}
				}
				Res.outz("update type pk= ");
				break;
			}
			case 61:
			{
				string text = msg.reader().readUTF();
				sbyte[] data = new sbyte[msg.reader().readInt()];
				msg.reader().read(ref data);
				if (data.Length == 0)
				{
					data = null;
				}
				if (text.Equals("KSkill"))
				{
					GameScr.gI().onKSkill(data);
				}
				else if (text.Equals("OSkill"))
				{
					GameScr.gI().onOSkill(data);
				}
				else if (text.Equals("CSkill"))
				{
					GameScr.gI().onCSkill(data);
				}
				break;
			}
			case 23:
			{
				short num = msg.reader().readShort();
				Skill skill = Skills.get(num);
				useSkill(skill);
				if (num != 0 && num != 14 && num != 28)
				{
					GameScr.info1.addInfo(mResources.LEARN_SKILL + " " + skill.template.name, 0);
				}
				break;
			}
			case 62:
				Res.outz("ME UPDATE SKILL");
				read_UpdateSkill(msg);
				break;
			}
		}
		catch (Exception ex5)
		{
			Cout.println("Loi tai Sub : " + ex5.ToString());
		}
		finally
		{
			msg?.cleanup();
		}
	}

	private void useSkill(Skill skill)
	{
		if (Char.myCharz().myskill == null)
		{
			Char.myCharz().myskill = skill;
		}
		else if (skill.template.Equals(Char.myCharz().myskill.template))
		{
			Char.myCharz().myskill = skill;
		}
		Char.myCharz().vSkill.addElement(skill);
		if ((skill.template.type == 1 || skill.template.type == 4 || skill.template.type == 2 || skill.template.type == 3) && (skill.template.maxPoint == 0 || (skill.template.maxPoint > 0 && skill.point > 0)))
		{
			if (skill.template.id == Char.myCharz().skillTemplateId)
			{
				Service.gI().selectSkill(Char.myCharz().skillTemplateId);
			}
			Char.myCharz().vSkillFight.addElement(skill);
		}
	}

	public bool readCharInfo(Char c, Message msg)
	{
		try
		{
			c.clevel = msg.reader().readByte();
			c.isInvisiblez = msg.reader().readBoolean();
			c.cTypePk = msg.reader().readByte();
			Res.outz("ADD TYPE PK= " + c.cTypePk + " to player " + c.charID + " @@ " + c.cName);
			c.nClass = GameScr.nClasss[msg.reader().readByte()];
			c.cgender = msg.reader().readByte();
			c.head = msg.reader().readShort();
			c.cName = msg.reader().readUTF();
			c.cHP = msg.readInt3Byte();
			c.dHP = c.cHP;
			if (c.cHP == 0)
			{
				c.statusMe = 14;
			}
			c.cHPFull = msg.readInt3Byte();
			if (c.cy >= TileMap.pxh - 100)
			{
				c.isFlyUp = true;
			}
			c.body = msg.reader().readShort();
			c.leg = msg.reader().readShort();
			c.bag = msg.reader().readUnsignedByte();
			Res.outz(" body= " + c.body + " leg= " + c.leg + " bag=" + c.bag + "BAG ==" + c.bag + "*********************************");
			c.isShadown = true;
			sbyte b = msg.reader().readByte();
			if (c.wp == -1)
			{
				c.setDefaultWeapon();
			}
			if (c.body == -1)
			{
				c.setDefaultBody();
			}
			if (c.leg == -1)
			{
				c.setDefaultLeg();
			}
			c.cx = msg.reader().readShort();
			c.cy = msg.reader().readShort();
			c.xSd = c.cx;
			c.ySd = c.cy;
			c.eff5BuffHp = msg.reader().readShort();
			c.eff5BuffMp = msg.reader().readShort();
			int num = msg.reader().readByte();
			for (int i = 0; i < num; i++)
			{
				EffectChar effectChar = new EffectChar(msg.reader().readByte(), msg.reader().readInt(), msg.reader().readInt(), msg.reader().readShort());
				c.vEff.addElement(effectChar);
				if (effectChar.template.type == 12 || effectChar.template.type == 11)
				{
					c.isInvisiblez = true;
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		return false;
	}

	private void readGetImgByName(Message msg)
	{
		try
		{
			string text = msg.reader().readUTF();
			sbyte nFrame = msg.reader().readByte();
			sbyte[] array = null;
			array = NinjaUtil.readByteArray(msg);
			Image img = createImage(array);
			ImgByName.SetImage(text, img, nFrame);
			if (array != null)
			{
				ImgByName.saveRMS(text, nFrame, array);
			}
		}
		catch (Exception)
		{
		}
	}

	private void createItemNew(myReader d)
	{
		try
		{
			loadItemNew(d, -1, isSave: true);
		}
		catch (Exception)
		{
		}
	}

	private void loadItemNew(myReader d, sbyte type, bool isSave)
	{
		try
		{
			d.mark(100000);
			GameScr.vcItem = d.readByte();
			type = d.readByte();
			switch (type)
			{
			case 0:
			{
				GameScr.gI().iOptionTemplates = new ItemOptionTemplate[d.readUnsignedByte()];
				for (int k = 0; k < GameScr.gI().iOptionTemplates.Length; k++)
				{
					GameScr.gI().iOptionTemplates[k] = new ItemOptionTemplate();
					GameScr.gI().iOptionTemplates[k].id = k;
					GameScr.gI().iOptionTemplates[k].name = d.readUTF();
					GameScr.gI().iOptionTemplates[k].type = d.readByte();
				}
				if (isSave)
				{
					d.reset();
					sbyte[] data5 = new sbyte[d.available()];
					d.readFully(ref data5);
					Rms.saveRMS("NRitem0", data5);
				}
				break;
			}
			case 1:
			{
				ItemTemplates.itemTemplates.clear();
				int num = d.readShort();
				for (int i = 0; i < num; i++)
				{
					ItemTemplate it = new ItemTemplate((short)i, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBoolean());
					ItemTemplates.add(it);
				}
				if (isSave)
				{
					d.reset();
					sbyte[] data2 = new sbyte[d.available()];
					d.readFully(ref data2);
					Rms.saveRMS("NRitem1", data2);
				}
				break;
			}
			case 2:
			{
				int num2 = d.readShort();
				int num3 = d.readShort();
				for (int j = num2; j < num3; j++)
				{
					ItemTemplate it2 = new ItemTemplate((short)j, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBoolean());
					ItemTemplates.add(it2);
				}
				if (isSave)
				{
					d.reset();
					sbyte[] data3 = new sbyte[d.available()];
					d.readFully(ref data3);
					Rms.saveRMS("NRitem2", data3);
					sbyte[] data4 = new sbyte[1] { GameScr.vcItem };
					Rms.saveRMS("NRitemVersion", data4);
					LoginScr.isUpdateItem = false;
					if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
					{
						GameScr.gI().readDart();
						GameScr.gI().readEfect();
						GameScr.gI().readArrow();
						GameScr.gI().readSkill();
						Service.gI().clientOk();
					}
				}
				break;
			}
			case 100:
				Char.Arr_Head_2Fr = readArrHead(d);
				if (isSave)
				{
					d.reset();
					sbyte[] data = new sbyte[d.available()];
					d.readFully(ref data);
					Rms.saveRMS("NRitem100", data);
				}
				break;
			}
		}
		catch (Exception ex)
		{
			ex.ToString();
		}
	}

	private void readFrameBoss(Message msg, int mobTemplateId)
	{
		try
		{
			int num = msg.reader().readByte();
			int[][] array = new int[num][];
			for (int i = 0; i < num; i++)
			{
				int num2 = msg.reader().readByte();
				array[i] = new int[num2];
				for (int j = 0; j < num2; j++)
				{
					array[i][j] = msg.reader().readByte();
				}
			}
			frameHT_NEWBOSS.put(mobTemplateId + string.Empty, array);
		}
		catch (Exception)
		{
		}
	}

	private int[][] readArrHead(myReader d)
	{
		int[][] array = new int[1][] { new int[2] { 542, 543 } };
		try
		{
			int num = d.readShort();
			array = new int[num][];
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = d.readByte();
				array[i] = new int[num2];
				for (int j = 0; j < num2; j++)
				{
					array[i][j] = d.readShort();
				}
			}
			return array;
		}
		catch (Exception)
		{
			return array;
		}
	}

	public void phuban_Info(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			if (b == 0)
			{
				readPhuBan_CHIENTRUONGNAMEK(msg, b);
			}
		}
		catch (Exception)
		{
		}
	}

	private void readPhuBan_CHIENTRUONGNAMEK(Message msg, int type_PB)
	{
		try
		{
			switch (msg.reader().readByte())
			{
			case 0:
			{
				short idmapPaint = msg.reader().readShort();
				string nameTeam = msg.reader().readUTF();
				string nameTeam2 = msg.reader().readUTF();
				int maxPoint = msg.reader().readInt();
				short timeSecond = msg.reader().readShort();
				int maxLife = msg.reader().readByte();
				GameScr.phuban_Info = new InfoPhuBan(type_PB, idmapPaint, nameTeam, nameTeam2, maxPoint, timeSecond);
				GameScr.phuban_Info.maxLife = maxLife;
				GameScr.phuban_Info.updateLife(type_PB, 0, 0);
				break;
			}
			case 1:
			{
				int pointTeam = msg.reader().readInt();
				int pointTeam2 = msg.reader().readInt();
				if (GameScr.phuban_Info != null)
				{
					GameScr.phuban_Info.updatePoint(type_PB, pointTeam, pointTeam2);
				}
				break;
			}
			case 2:
			{
				sbyte b = msg.reader().readByte();
				short type = 0;
				short num = -1;
				switch (b)
				{
				case 1:
					type = 1;
					num = 3;
					break;
				case 2:
					type = 2;
					break;
				}
				num = -1;
				GameScr.phuban_Info = null;
				GameScr.addEffectEnd(type, num, 0, GameCanvas.hw, GameCanvas.hh, 0, 0, -1, null);
				break;
			}
			case 5:
			{
				short timeSecond2 = msg.reader().readShort();
				if (GameScr.phuban_Info != null)
				{
					GameScr.phuban_Info.updateTime(type_PB, timeSecond2);
				}
				break;
			}
			case 4:
			{
				int lifeTeam = msg.reader().readByte();
				int lifeTeam2 = msg.reader().readByte();
				if (GameScr.phuban_Info != null)
				{
					GameScr.phuban_Info.updateLife(type_PB, lifeTeam, lifeTeam2);
				}
				break;
			}
			}
		}
		catch (Exception)
		{
		}
	}

	public void read_opt(Message msg)
	{
		try
		{
			if (msg.reader().readByte() == 0)
			{
				short idHat = msg.reader().readShort();
				Char.myCharz().idHat = idHat;
				SoundMn.gI().getStrOption();
			}
		}
		catch (Exception)
		{
		}
	}

	public void read_UpdateSkill(Message msg)
	{
		try
		{
			short num = msg.reader().readShort();
			sbyte b = -1;
			try
			{
				b = msg.reader().readSByte();
			}
			catch (Exception)
			{
			}
			switch (b)
			{
			case 0:
			{
				short curExp = msg.reader().readShort();
				for (int m = 0; m < Char.myCharz().vSkill.size(); m++)
				{
					Skill skill4 = (Skill)Char.myCharz().vSkill.elementAt(m);
					if (skill4.skillId == num)
					{
						skill4.curExp = curExp;
						break;
					}
				}
				break;
			}
			case 1:
			{
				sbyte b2 = msg.reader().readByte();
				for (int n = 0; n < Char.myCharz().vSkill.size(); n++)
				{
					Skill skill5 = (Skill)Char.myCharz().vSkill.elementAt(n);
					if (skill5.skillId == num)
					{
						for (int num2 = 0; num2 < 20; num2++)
						{
							string nameImg = "Skills_" + skill5.template.id + "_" + b2 + "_" + num2;
							MainImage imagePath = ImgByName.getImagePath(nameImg, ImgByName.hashImagePath);
						}
						break;
					}
				}
				break;
			}
			case -1:
			{
				Skill skill = Skills.get(num);
				for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
				{
					Skill skill2 = (Skill)Char.myCharz().vSkill.elementAt(i);
					if (skill2.template.id == skill.template.id)
					{
						Char.myCharz().vSkill.setElementAt(skill, i);
						break;
					}
				}
				for (int j = 0; j < Char.myCharz().vSkillFight.size(); j++)
				{
					Skill skill3 = (Skill)Char.myCharz().vSkillFight.elementAt(j);
					if (skill3.template.id == skill.template.id)
					{
						Char.myCharz().vSkillFight.setElementAt(skill, j);
						break;
					}
				}
				for (int k = 0; k < GameScr.onScreenSkill.Length; k++)
				{
					if (GameScr.onScreenSkill[k] != null && GameScr.onScreenSkill[k].template.id == skill.template.id)
					{
						GameScr.onScreenSkill[k] = skill;
						break;
					}
				}
				for (int l = 0; l < GameScr.keySkill.Length; l++)
				{
					if (GameScr.keySkill[l] != null && GameScr.keySkill[l].template.id == skill.template.id)
					{
						GameScr.keySkill[l] = skill;
						break;
					}
				}
				if (Char.myCharz().myskill.template.id == skill.template.id)
				{
					Char.myCharz().myskill = skill;
				}
				GameScr.info1.addInfo(mResources.hasJustUpgrade1 + skill.template.name + mResources.hasJustUpgrade2 + skill.point, 0);
				break;
			}
			}
		}
		catch (Exception)
		{
		}
	}
}

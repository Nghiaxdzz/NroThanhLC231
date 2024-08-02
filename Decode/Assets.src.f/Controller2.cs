using System;
using Assets.src.g;

namespace Assets.src.f;

internal class Controller2
{
	public static void readMessage(Message msg)
	{
		try
		{
			switch (msg.command)
			{
			case sbyte.MinValue:
				readInfoEffChar(msg);
				break;
			case sbyte.MaxValue:
				readInfoRada(msg);
				break;
			case 114:
				try
				{
					string text4 = msg.reader().readUTF();
					mSystem.curINAPP = msg.reader().readByte();
					mSystem.maxINAPP = msg.reader().readByte();
					break;
				}
				catch (Exception)
				{
					break;
				}
			case 113:
			{
				int loop = 0;
				int layer = 0;
				int id2 = 0;
				short x = 0;
				short y = 0;
				short loopCount = -1;
				try
				{
					loop = msg.reader().readByte();
					layer = msg.reader().readByte();
					id2 = msg.reader().readUnsignedByte();
					x = msg.reader().readShort();
					y = msg.reader().readShort();
					loopCount = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				EffecMn.addEff(new Effect(id2, x, y, layer, loop, loopCount));
				break;
			}
			case 48:
			{
				sbyte ipSelect = msg.reader().readByte();
				ServerListScreen.ipSelect = ipSelect;
				GameCanvas.instance.doResetToLoginScr(GameCanvas.serverScreen);
				Session_ME.gI().close();
				GameCanvas.endDlg();
				ServerListScreen.waitToLogin = true;
				break;
			}
			case 31:
			{
				int num38 = msg.reader().readInt();
				sbyte b27 = msg.reader().readByte();
				if (b27 == 1)
				{
					short smallID = msg.reader().readShort();
					sbyte b28 = -1;
					int[] array11 = null;
					short wimg = 0;
					short himg = 0;
					try
					{
						b28 = msg.reader().readByte();
						if (b28 > 0)
						{
							sbyte b29 = msg.reader().readByte();
							array11 = new int[b29];
							for (int num39 = 0; num39 < b29; num39++)
							{
								array11[num39] = msg.reader().readByte();
							}
							wimg = msg.reader().readShort();
							himg = msg.reader().readShort();
						}
					}
					catch (Exception)
					{
					}
					if (num38 == Char.myCharz().charID)
					{
						Char.myCharz().petFollow = new PetFollow();
						Char.myCharz().petFollow.smallID = smallID;
						if (b28 > 0)
						{
							Char.myCharz().petFollow.SetImg(b28, array11, wimg, himg);
						}
						break;
					}
					Char char4 = GameScr.findCharInMap(num38);
					char4.petFollow = new PetFollow();
					char4.petFollow.smallID = smallID;
					if (b28 > 0)
					{
						char4.petFollow.SetImg(b28, array11, wimg, himg);
					}
				}
				else if (num38 == Char.myCharz().charID)
				{
					Char.myCharz().petFollow.remove();
					Char.myCharz().petFollow = null;
				}
				else
				{
					Char char5 = GameScr.findCharInMap(num38);
					char5.petFollow.remove();
					char5.petFollow = null;
				}
				break;
			}
			case -89:
				GameCanvas.open3Hour = msg.reader().readByte() == 1;
				break;
			case 42:
			{
				GameCanvas.endDlg();
				LoginScr.isContinueToLogin = false;
				Char.isLoadingMap = false;
				sbyte haveName = msg.reader().readByte();
				if (GameCanvas.registerScr == null)
				{
					GameCanvas.registerScr = new RegisterScreen(haveName);
				}
				GameCanvas.registerScr.switchToMe();
				break;
			}
			case 52:
			{
				sbyte b25 = msg.reader().readByte();
				if (b25 == 1)
				{
					int num32 = msg.reader().readInt();
					if (num32 == Char.myCharz().charID)
					{
						Char.myCharz().setMabuHold(m: true);
						Char.myCharz().cx = msg.reader().readShort();
						Char.myCharz().cy = msg.reader().readShort();
					}
					else
					{
						Char char3 = GameScr.findCharInMap(num32);
						if (char3 != null)
						{
							char3.setMabuHold(m: true);
							char3.cx = msg.reader().readShort();
							char3.cy = msg.reader().readShort();
						}
					}
				}
				if (b25 == 0)
				{
					int num33 = msg.reader().readInt();
					if (num33 == Char.myCharz().charID)
					{
						Char.myCharz().setMabuHold(m: false);
					}
					else
					{
						GameScr.findCharInMap(num33)?.setMabuHold(m: false);
					}
				}
				if (b25 == 2)
				{
					int charId2 = msg.reader().readInt();
					int id3 = msg.reader().readInt();
					Mabu mabu = (Mabu)GameScr.findCharInMap(charId2);
					mabu.eat(id3);
				}
				if (b25 == 3)
				{
					GameScr.mabuPercent = msg.reader().readByte();
				}
				break;
			}
			case 51:
			{
				int charId3 = msg.reader().readInt();
				Mabu mabu2 = (Mabu)GameScr.findCharInMap(charId3);
				sbyte id4 = msg.reader().readByte();
				short x2 = msg.reader().readShort();
				short y2 = msg.reader().readShort();
				sbyte b26 = msg.reader().readByte();
				Char[] array9 = new Char[b26];
				int[] array10 = new int[b26];
				for (int num34 = 0; num34 < b26; num34++)
				{
					int num35 = msg.reader().readInt();
					Res.outz("char ID=" + num35);
					array9[num34] = null;
					if (num35 != Char.myCharz().charID)
					{
						array9[num34] = GameScr.findCharInMap(num35);
					}
					else
					{
						array9[num34] = Char.myCharz();
					}
					array10[num34] = msg.reader().readInt();
				}
				mabu2.setSkill(id4, x2, y2, array9, array10);
				break;
			}
			case -127:
				readLuckyRound(msg);
				break;
			case -126:
			{
				sbyte b14 = msg.reader().readByte();
				Res.outz("type quay= " + b14);
				if (b14 == 1)
				{
					sbyte b15 = msg.reader().readByte();
					string num18 = msg.reader().readUTF();
					string finish = msg.reader().readUTF();
					GameScr.gI().showWinNumber(num18, finish);
				}
				if (b14 == 0)
				{
					GameScr.gI().showYourNumber(msg.reader().readUTF());
				}
				break;
			}
			case -122:
			{
				short id6 = msg.reader().readShort();
				Npc npc = GameScr.findNPCInMap(id6);
				sbyte b30 = msg.reader().readByte();
				npc.duahau = new int[b30];
				Res.outz("N DUA HAU= " + b30);
				for (int num42 = 0; num42 < b30; num42++)
				{
					npc.duahau[num42] = msg.reader().readShort();
				}
				npc.setStatus(msg.reader().readByte(), msg.reader().readInt());
				break;
			}
			case 102:
			{
				sbyte b4 = msg.reader().readByte();
				if (b4 == 0 || b4 == 1 || b4 == 2 || b4 == 6)
				{
					BigBoss2 bigBoss = Mob.getBigBoss2();
					if (bigBoss == null)
					{
						break;
					}
					if (b4 == 6)
					{
						bigBoss.x = (bigBoss.y = (bigBoss.xTo = (bigBoss.yTo = (bigBoss.xFirst = (bigBoss.yFirst = -1000)))));
						break;
					}
					sbyte b5 = msg.reader().readByte();
					Char[] array = new Char[b5];
					int[] array2 = new int[b5];
					for (int k = 0; k < b5; k++)
					{
						int num5 = msg.reader().readInt();
						array[k] = null;
						if (num5 != Char.myCharz().charID)
						{
							array[k] = GameScr.findCharInMap(num5);
						}
						else
						{
							array[k] = Char.myCharz();
						}
						array2[k] = msg.reader().readInt();
					}
					bigBoss.setAttack(array, array2, b4);
				}
				if (b4 == 3 || b4 == 4 || b4 == 5 || b4 == 7)
				{
					BachTuoc bachTuoc = Mob.getBachTuoc();
					if (bachTuoc == null)
					{
						break;
					}
					if (b4 == 7)
					{
						bachTuoc.x = (bachTuoc.y = (bachTuoc.xTo = (bachTuoc.yTo = (bachTuoc.xFirst = (bachTuoc.yFirst = -1000)))));
						break;
					}
					if (b4 == 3 || b4 == 4)
					{
						sbyte b6 = msg.reader().readByte();
						Char[] array3 = new Char[b6];
						int[] array4 = new int[b6];
						for (int l = 0; l < b6; l++)
						{
							int num6 = msg.reader().readInt();
							array3[l] = null;
							if (num6 != Char.myCharz().charID)
							{
								array3[l] = GameScr.findCharInMap(num6);
							}
							else
							{
								array3[l] = Char.myCharz();
							}
							array4[l] = msg.reader().readInt();
						}
						bachTuoc.setAttack(array3, array4, b4);
					}
					if (b4 == 5)
					{
						short xMoveTo = msg.reader().readShort();
						bachTuoc.move(xMoveTo);
					}
				}
				if (b4 > 9 && b4 < 30)
				{
					readActionBoss(msg, b4);
				}
				break;
			}
			case 101:
			{
				Res.outz("big boss--------------------------------------------------");
				BigBoss bigBoss2 = Mob.getBigBoss();
				if (bigBoss2 == null)
				{
					break;
				}
				sbyte b7 = msg.reader().readByte();
				if (b7 == 0 || b7 == 1 || b7 == 2 || b7 == 4 || b7 == 3)
				{
					if (b7 == 3)
					{
						bigBoss2.xTo = (bigBoss2.xFirst = msg.reader().readShort());
						bigBoss2.yTo = (bigBoss2.yFirst = msg.reader().readShort());
						bigBoss2.setFly();
					}
					else
					{
						sbyte b8 = msg.reader().readByte();
						Res.outz("CHUONG nChar= " + b8);
						Char[] array5 = new Char[b8];
						int[] array6 = new int[b8];
						for (int m = 0; m < b8; m++)
						{
							int num7 = msg.reader().readInt();
							Res.outz("char ID=" + num7);
							array5[m] = null;
							if (num7 != Char.myCharz().charID)
							{
								array5[m] = GameScr.findCharInMap(num7);
							}
							else
							{
								array5[m] = Char.myCharz();
							}
							array6[m] = msg.reader().readInt();
						}
						bigBoss2.setAttack(array5, array6, b7);
					}
				}
				if (b7 == 5)
				{
					bigBoss2.haftBody = true;
					bigBoss2.status = 2;
				}
				if (b7 == 6)
				{
					bigBoss2.getDataB2();
					bigBoss2.x = msg.reader().readShort();
					bigBoss2.y = msg.reader().readShort();
				}
				if (b7 == 7)
				{
					bigBoss2.setAttack(null, null, b7);
				}
				if (b7 == 8)
				{
					bigBoss2.xTo = (bigBoss2.xFirst = msg.reader().readShort());
					bigBoss2.yTo = (bigBoss2.yFirst = msg.reader().readShort());
					bigBoss2.status = 2;
				}
				if (b7 == 9)
				{
					bigBoss2.x = (bigBoss2.y = (bigBoss2.xTo = (bigBoss2.yTo = (bigBoss2.xFirst = (bigBoss2.yFirst = -1000)))));
				}
				break;
			}
			case -120:
			{
				long num41 = mSystem.currentTimeMillis();
				Service.logController = num41 - Service.curCheckController;
				Service.gI().sendCheckController();
				break;
			}
			case -121:
			{
				long num4 = mSystem.currentTimeMillis();
				Service.logMap = num4 - Service.curCheckMap;
				Service.gI().sendCheckMap();
				break;
			}
			case 100:
			{
				sbyte b17 = msg.reader().readByte();
				sbyte b18 = msg.reader().readByte();
				Item item2 = null;
				if (b17 == 0)
				{
					item2 = Char.myCharz().arrItemBody[b18];
				}
				if (b17 == 1)
				{
					item2 = Char.myCharz().arrItemBag[b18];
				}
				short num21 = msg.reader().readShort();
				if (num21 == -1)
				{
					break;
				}
				item2.template = ItemTemplates.get(num21);
				item2.quantity = msg.reader().readInt();
				item2.info = msg.reader().readUTF();
				item2.content = msg.reader().readUTF();
				sbyte b19 = msg.reader().readByte();
				if (b19 == 0)
				{
					break;
				}
				item2.itemOption = new ItemOption[b19];
				for (int num22 = 0; num22 < item2.itemOption.Length; num22++)
				{
					int num23 = msg.reader().readUnsignedByte();
					Res.outz("id o= " + num23);
					int param3 = msg.reader().readUnsignedShort();
					if (num23 != -1)
					{
						item2.itemOption[num22] = new ItemOption(num23, param3);
					}
				}
				break;
			}
			case -123:
			{
				int charId = msg.reader().readInt();
				if (GameScr.findCharInMap(charId) != null)
				{
					GameScr.findCharInMap(charId).perCentMp = msg.reader().readByte();
				}
				break;
			}
			case -119:
				Char.myCharz().rank = msg.reader().readInt();
				break;
			case -117:
				GameScr.gI().tMabuEff = 0;
				GameScr.gI().percentMabu = msg.reader().readByte();
				if (GameScr.gI().percentMabu == 100)
				{
					GameScr.gI().mabuEff = true;
				}
				if (GameScr.gI().percentMabu == 101)
				{
					Npc.mabuEff = true;
				}
				break;
			case -116:
				GameScr.canAutoPlay = msg.reader().readByte() == 1;
				break;
			case -115:
				Char.myCharz().setPowerInfo(msg.reader().readUTF(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort());
				break;
			case -113:
			{
				sbyte[] array7 = new sbyte[10];
				for (int n = 0; n < 10; n++)
				{
					array7[n] = msg.reader().readByte();
					Res.outz("vlue i= " + array7[n]);
				}
				GameScr.gI().onKSkill(array7);
				GameScr.gI().onOSkill(array7);
				GameScr.gI().onCSkill(array7);
				break;
			}
			case -111:
			{
				short num36 = msg.reader().readShort();
				ImageSource.vSource = new MyVector();
				for (int num37 = 0; num37 < num36; num37++)
				{
					string iD = msg.reader().readUTF();
					sbyte version = msg.reader().readByte();
					ImageSource.vSource.addElement(new ImageSource(iD, version));
				}
				ImageSource.checkRMS();
				ImageSource.saveRMS();
				break;
			}
			case 125:
			{
				sbyte fusion = msg.reader().readByte();
				int num40 = msg.reader().readInt();
				if (num40 == Char.myCharz().charID)
				{
					Char.myCharz().setFusion(fusion);
				}
				else if (GameScr.findCharInMap(num40) != null)
				{
					GameScr.findCharInMap(num40).setFusion(fusion);
				}
				break;
			}
			case 124:
			{
				short id5 = msg.reader().readShort();
				string text3 = msg.reader().readUTF();
				Res.outz("noi chuyen = " + text3 + "npc ID= " + id5);
				GameScr.findNPCInMap(id5)?.addInfo(text3);
				break;
			}
			case 123:
			{
				Res.outz("SET POSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSss");
				int num25 = msg.reader().readInt();
				short xPos = msg.reader().readShort();
				short yPos = msg.reader().readShort();
				sbyte b22 = msg.reader().readByte();
				Char char2 = null;
				if (num25 == Char.myCharz().charID)
				{
					char2 = Char.myCharz();
				}
				else if (GameScr.findCharInMap(num25) != null)
				{
					char2 = GameScr.findCharInMap(num25);
				}
				if (char2 != null)
				{
					ServerEffect.addServerEffect((b22 != 0) ? 173 : 60, char2, 1);
					char2.setPos(xPos, yPos, b22);
				}
				break;
			}
			case 122:
			{
				short timeLogin = msg.reader().readShort();
				Res.outz("second login = " + timeLogin);
				LoginScr.timeLogin = timeLogin;
				LoginScr.currTimeLogin = (LoginScr.lastTimeLogin = mSystem.currentTimeMillis());
				GameCanvas.endDlg();
				break;
			}
			case 121:
				mSystem.publicID = msg.reader().readUTF();
				mSystem.strAdmob = msg.reader().readUTF();
				Res.outz("SHOW AD public ID= " + mSystem.publicID);
				mSystem.createAdmob();
				break;
			case -124:
			{
				sbyte b23 = msg.reader().readByte();
				sbyte b24 = msg.reader().readByte();
				if (b24 == 0)
				{
					if (b23 == 2)
					{
						int num26 = msg.reader().readInt();
						if (num26 == Char.myCharz().charID)
						{
							Char.myCharz().removeEffect();
						}
						else if (GameScr.findCharInMap(num26) != null)
						{
							GameScr.findCharInMap(num26).removeEffect();
						}
					}
					int num27 = msg.reader().readUnsignedByte();
					int num28 = msg.reader().readInt();
					if (num27 == 32)
					{
						if (b23 == 1)
						{
							int num29 = msg.reader().readInt();
							if (num28 == Char.myCharz().charID)
							{
								Char.myCharz().holdEffID = num27;
								GameScr.findCharInMap(num29).setHoldChar(Char.myCharz());
							}
							else if (GameScr.findCharInMap(num28) != null && num29 != Char.myCharz().charID)
							{
								GameScr.findCharInMap(num28).holdEffID = num27;
								GameScr.findCharInMap(num29).setHoldChar(GameScr.findCharInMap(num28));
							}
							else if (GameScr.findCharInMap(num28) != null && num29 == Char.myCharz().charID)
							{
								GameScr.findCharInMap(num28).holdEffID = num27;
								Char.myCharz().setHoldChar(GameScr.findCharInMap(num28));
							}
						}
						else if (num28 == Char.myCharz().charID)
						{
							Char.myCharz().removeHoleEff();
						}
						else if (GameScr.findCharInMap(num28) != null)
						{
							GameScr.findCharInMap(num28).removeHoleEff();
						}
					}
					if (num27 == 33)
					{
						if (b23 == 1)
						{
							if (num28 == Char.myCharz().charID)
							{
								Char.myCharz().protectEff = true;
							}
							else if (GameScr.findCharInMap(num28) != null)
							{
								GameScr.findCharInMap(num28).protectEff = true;
							}
						}
						else if (num28 == Char.myCharz().charID)
						{
							Char.myCharz().removeProtectEff();
						}
						else if (GameScr.findCharInMap(num28) != null)
						{
							GameScr.findCharInMap(num28).removeProtectEff();
						}
					}
					if (num27 == 39)
					{
						if (b23 == 1)
						{
							if (num28 == Char.myCharz().charID)
							{
								Char.myCharz().huytSao = true;
							}
							else if (GameScr.findCharInMap(num28) != null)
							{
								GameScr.findCharInMap(num28).huytSao = true;
							}
						}
						else if (num28 == Char.myCharz().charID)
						{
							Char.myCharz().removeHuytSao();
						}
						else if (GameScr.findCharInMap(num28) != null)
						{
							GameScr.findCharInMap(num28).removeHuytSao();
						}
					}
					if (num27 == 40)
					{
						if (b23 == 1)
						{
							if (num28 == Char.myCharz().charID)
							{
								Char.myCharz().blindEff = true;
							}
							else if (GameScr.findCharInMap(num28) != null)
							{
								GameScr.findCharInMap(num28).blindEff = true;
							}
						}
						else if (num28 == Char.myCharz().charID)
						{
							Char.myCharz().removeBlindEff();
						}
						else if (GameScr.findCharInMap(num28) != null)
						{
							GameScr.findCharInMap(num28).removeBlindEff();
						}
					}
					if (num27 == 41)
					{
						if (b23 == 1)
						{
							if (num28 == Char.myCharz().charID)
							{
								Char.myCharz().sleepEff = true;
							}
							else if (GameScr.findCharInMap(num28) != null)
							{
								GameScr.findCharInMap(num28).sleepEff = true;
							}
						}
						else if (num28 == Char.myCharz().charID)
						{
							Char.myCharz().removeSleepEff();
						}
						else if (GameScr.findCharInMap(num28) != null)
						{
							GameScr.findCharInMap(num28).removeSleepEff();
						}
					}
					if (num27 == 42)
					{
						if (b23 == 1)
						{
							if (num28 == Char.myCharz().charID)
							{
								Char.myCharz().stone = true;
							}
						}
						else if (num28 == Char.myCharz().charID)
						{
							Char.myCharz().stone = false;
						}
					}
				}
				if (b24 != 1)
				{
					break;
				}
				int num30 = msg.reader().readUnsignedByte();
				sbyte mobIndex = msg.reader().readByte();
				Res.outz("modbHoldID= " + mobIndex + " skillID= " + num30 + "eff ID= " + b23);
				if (num30 == 32)
				{
					if (b23 == 1)
					{
						int num31 = msg.reader().readInt();
						if (num31 == Char.myCharz().charID)
						{
							GameScr.findMobInMap(mobIndex).holdEffID = num30;
							Char.myCharz().setHoldMob(GameScr.findMobInMap(mobIndex));
						}
						else if (GameScr.findCharInMap(num31) != null)
						{
							GameScr.findMobInMap(mobIndex).holdEffID = num30;
							GameScr.findCharInMap(num31).setHoldMob(GameScr.findMobInMap(mobIndex));
						}
					}
					else
					{
						GameScr.findMobInMap(mobIndex).removeHoldEff();
					}
				}
				if (num30 == 40)
				{
					if (b23 == 1)
					{
						GameScr.findMobInMap(mobIndex).blindEff = true;
					}
					else
					{
						GameScr.findMobInMap(mobIndex).removeBlindEff();
					}
				}
				if (num30 == 41)
				{
					if (b23 == 1)
					{
						GameScr.findMobInMap(mobIndex).sleepEff = true;
					}
					else
					{
						GameScr.findMobInMap(mobIndex).removeSleepEff();
					}
				}
				break;
			}
			case -125:
			{
				ChatTextField.gI().isShow = false;
				string text2 = msg.reader().readUTF();
				Res.outz("titile= " + text2);
				sbyte b20 = msg.reader().readByte();
				ClientInput.gI().setInput(b20, text2);
				for (int num24 = 0; num24 < b20; num24++)
				{
					ClientInput.gI().tf[num24].name = msg.reader().readUTF();
					sbyte b21 = msg.reader().readByte();
					if (b21 == 0)
					{
						ClientInput.gI().tf[num24].setIputType(TField.INPUT_TYPE_NUMERIC);
					}
					if (b21 == 1)
					{
						ClientInput.gI().tf[num24].setIputType(TField.INPUT_TYPE_ANY);
					}
					if (b21 == 2)
					{
						ClientInput.gI().tf[num24].setIputType(TField.INPUT_TYPE_PASSWORD);
					}
				}
				break;
			}
			case -110:
			{
				sbyte b16 = msg.reader().readByte();
				if (b16 == 1)
				{
					int id = msg.reader().readInt();
					sbyte[] array8 = Rms.loadRMS(id + string.Empty);
					if (array8 == null)
					{
						Service.gI().sendServerData(1, -1, null);
					}
					else
					{
						Service.gI().sendServerData(1, id, array8);
					}
				}
				if (b16 == 0)
				{
					int num19 = msg.reader().readInt();
					short num20 = msg.reader().readShort();
					sbyte[] data = new sbyte[num20];
					msg.reader().read(ref data, 0, num20);
					Rms.saveRMS(num19 + string.Empty, data);
				}
				break;
			}
			case 93:
			{
				string str = msg.reader().readUTF();
				str = Res.changeString(str);
				GameScr.gI().chatVip(str);
				break;
			}
			case -106:
			{
				short num16 = msg.reader().readShort();
				int num17 = msg.reader().readShort();
				if (ItemTime.isExistItem(num16))
				{
					ItemTime.getItemById(num16).initTime(num17);
					break;
				}
				ItemTime o = new ItemTime(num16, num17);
				Char.vItemTime.addElement(o);
				break;
			}
			case -105:
				TransportScr.gI().time = 0;
				TransportScr.gI().maxTime = msg.reader().readShort();
				TransportScr.gI().last = (TransportScr.gI().curr = mSystem.currentTimeMillis());
				TransportScr.gI().type = msg.reader().readByte();
				TransportScr.gI().switchToMe();
				break;
			case -103:
				switch (msg.reader().readByte())
				{
				case 0:
				{
					GameCanvas.panel.vFlag.removeAllElements();
					sbyte b11 = msg.reader().readByte();
					for (int num11 = 0; num11 < b11; num11++)
					{
						Item item = new Item();
						short num12 = msg.reader().readShort();
						if (num12 != -1)
						{
							item.template = ItemTemplates.get(num12);
							sbyte b12 = msg.reader().readByte();
							if (b12 != -1)
							{
								item.itemOption = new ItemOption[b12];
								for (int num13 = 0; num13 < item.itemOption.Length; num13++)
								{
									int num14 = msg.reader().readUnsignedByte();
									int param2 = msg.reader().readUnsignedShort();
									if (num14 != -1)
									{
										item.itemOption[num13] = new ItemOption(num14, param2);
									}
								}
							}
						}
						GameCanvas.panel.vFlag.addElement(item);
					}
					GameCanvas.panel.setTypeFlag();
					GameCanvas.panel.show();
					break;
				}
				case 1:
				{
					int num15 = msg.reader().readInt();
					sbyte b13 = msg.reader().readByte();
					Res.outz("---------------actionFlag1:  " + num15 + " : " + b13);
					if (num15 == Char.myCharz().charID)
					{
						Char.myCharz().cFlag = b13;
					}
					else if (GameScr.findCharInMap(num15) != null)
					{
						GameScr.findCharInMap(num15).cFlag = b13;
					}
					GameScr.gI().getFlagImage(num15, b13);
					break;
				}
				case 2:
				{
					sbyte b10 = msg.reader().readByte();
					int num8 = msg.reader().readShort();
					PKFlag pKFlag = new PKFlag();
					pKFlag.cflag = b10;
					pKFlag.IDimageFlag = num8;
					GameScr.vFlag.addElement(pKFlag);
					for (int num9 = 0; num9 < GameScr.vFlag.size(); num9++)
					{
						PKFlag pKFlag2 = (PKFlag)GameScr.vFlag.elementAt(num9);
						Res.outz("i: " + num9 + "  cflag: " + pKFlag2.cflag + "   IDimageFlag: " + pKFlag2.IDimageFlag);
					}
					for (int num10 = 0; num10 < GameScr.vCharInMap.size(); num10++)
					{
						Char @char = (Char)GameScr.vCharInMap.elementAt(num10);
						if (@char != null && @char.cFlag == b10)
						{
							@char.flagImage = num8;
						}
					}
					if (Char.myCharz().cFlag == b10)
					{
						Char.myCharz().flagImage = num8;
					}
					break;
				}
				}
				break;
			case -102:
			{
				sbyte b9 = msg.reader().readByte();
				if (b9 != 0 && b9 == 1)
				{
					GameCanvas.loginScr.isLogin2 = false;
					Service.gI().login(Rms.loadRMSString("acc"), Rms.loadRMSString("pass"), GameMidlet.VERSION, 0);
					LoginScr.isLoggingIn = true;
				}
				break;
			}
			case -101:
			{
				GameCanvas.loginScr.isLogin2 = true;
				GameCanvas.connect();
				string text = msg.reader().readUTF();
				Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, text);
				Service.gI().setClientType();
				Service.gI().login(text, string.Empty, GameMidlet.VERSION, 1);
				break;
			}
			case -100:
			{
				InfoDlg.hide();
				bool flag = false;
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
				{
					flag = true;
				}
				sbyte b = msg.reader().readByte();
				Res.outz("t Indxe= " + b);
				GameCanvas.panel.maxPageShop[b] = msg.reader().readByte();
				GameCanvas.panel.currPageShop[b] = msg.reader().readByte();
				Res.outz("max page= " + GameCanvas.panel.maxPageShop[b] + " curr page= " + GameCanvas.panel.currPageShop[b]);
				int num = msg.reader().readUnsignedByte();
				Char.myCharz().arrItemShop[b] = new Item[num];
				for (int i = 0; i < num; i++)
				{
					short num2 = msg.reader().readShort();
					if (num2 == -1)
					{
						continue;
					}
					Res.outz("template id= " + num2);
					Char.myCharz().arrItemShop[b][i] = new Item();
					Char.myCharz().arrItemShop[b][i].template = ItemTemplates.get(num2);
					Char.myCharz().arrItemShop[b][i].itemId = msg.reader().readShort();
					Char.myCharz().arrItemShop[b][i].buyCoin = msg.reader().readInt();
					Char.myCharz().arrItemShop[b][i].buyGold = msg.reader().readInt();
					Char.myCharz().arrItemShop[b][i].buyType = msg.reader().readByte();
					Char.myCharz().arrItemShop[b][i].quantity = msg.reader().readInt();
					Char.myCharz().arrItemShop[b][i].isMe = msg.reader().readByte();
					Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
					sbyte b2 = msg.reader().readByte();
					if (b2 != -1)
					{
						Char.myCharz().arrItemShop[b][i].itemOption = new ItemOption[b2];
						for (int j = 0; j < Char.myCharz().arrItemShop[b][i].itemOption.Length; j++)
						{
							int num3 = msg.reader().readUnsignedByte();
							int param = msg.reader().readUnsignedShort();
							if (num3 != -1)
							{
								Char.myCharz().arrItemShop[b][i].itemOption[j] = new ItemOption(num3, param);
								Char.myCharz().arrItemShop[b][i].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[b][i]);
							}
						}
					}
					sbyte b3 = msg.reader().readByte();
					if (b3 == 1)
					{
						int headTemp = msg.reader().readShort();
						int bodyTemp = msg.reader().readShort();
						int legTemp = msg.reader().readShort();
						int bagTemp = msg.reader().readShort();
						Char.myCharz().arrItemShop[b][i].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
					}
				}
				if (flag)
				{
					GameCanvas.panel2.setTabKiGui();
				}
				GameCanvas.panel.setTabShop();
				GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
				break;
			}
			}
		}
		catch (Exception ex4)
		{
			Res.outz("=====> Controller2 " + ex4.StackTrace);
		}
	}

	private static void readLuckyRound(Message msg)
	{
		try
		{
			switch (msg.reader().readByte())
			{
			case 0:
			{
				sbyte b2 = msg.reader().readByte();
				short[] array2 = new short[b2];
				for (int j = 0; j < b2; j++)
				{
					array2[j] = msg.reader().readShort();
				}
				sbyte b3 = msg.reader().readByte();
				int price = msg.reader().readInt();
				short idTicket = msg.reader().readShort();
				CrackBallScr.gI().SetCrackBallScr(array2, (byte)b3, price, idTicket);
				break;
			}
			case 1:
			{
				sbyte b = msg.reader().readByte();
				short[] array = new short[b];
				for (int i = 0; i < b; i++)
				{
					array[i] = msg.reader().readShort();
				}
				CrackBallScr.gI().DoneCrackBallScr(array);
				break;
			}
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readInfoRada(Message msg)
	{
		try
		{
			switch (msg.reader().readByte())
			{
			case 0:
			{
				RadarScr.gI();
				MyVector myVector = new MyVector(string.Empty);
				short num2 = msg.reader().readShort();
				int num3 = 0;
				for (int i = 0; i < num2; i++)
				{
					Info_RadaScr info_RadaScr = new Info_RadaScr();
					int id = msg.reader().readShort();
					int no = i + 1;
					int idIcon = msg.reader().readShort();
					sbyte rank = msg.reader().readByte();
					sbyte amount = msg.reader().readByte();
					sbyte max_amount = msg.reader().readByte();
					short templateId = -1;
					Char charInfo = null;
					sbyte b = msg.reader().readByte();
					if (b == 0)
					{
						templateId = msg.reader().readShort();
					}
					else
					{
						int head = msg.reader().readShort();
						int body = msg.reader().readShort();
						int leg = msg.reader().readShort();
						int bag = msg.reader().readShort();
						charInfo = Info_RadaScr.SetCharInfo(head, body, leg, bag);
					}
					string name = msg.reader().readUTF();
					string info = msg.reader().readUTF();
					sbyte b2 = msg.reader().readByte();
					sbyte use = msg.reader().readByte();
					sbyte b3 = msg.reader().readByte();
					ItemOption[] array = null;
					if (b3 != 0)
					{
						array = new ItemOption[b3];
						for (int j = 0; j < array.Length; j++)
						{
							int num4 = msg.reader().readUnsignedByte();
							int param = msg.reader().readUnsignedShort();
							sbyte activeCard = msg.reader().readByte();
							if (num4 != -1)
							{
								array[j] = new ItemOption(num4, param);
								array[j].activeCard = activeCard;
							}
						}
					}
					info_RadaScr.SetInfo(id, no, idIcon, rank, b, templateId, name, info, charInfo, array);
					info_RadaScr.SetLevel(b2);
					info_RadaScr.SetUse(use);
					info_RadaScr.SetAmount(amount, max_amount);
					myVector.addElement(info_RadaScr);
					if (b2 > 0)
					{
						num3++;
					}
				}
				RadarScr.gI().SetRadarScr(myVector, num3, num2);
				RadarScr.gI().switchToMe();
				break;
			}
			case 1:
			{
				int id3 = msg.reader().readShort();
				sbyte use2 = msg.reader().readByte();
				if (Info_RadaScr.GetInfo(RadarScr.list, id3) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.list, id3).SetUse(use2);
				}
				RadarScr.SetListUse();
				break;
			}
			case 2:
			{
				int num5 = msg.reader().readShort();
				sbyte level = msg.reader().readByte();
				int num6 = 0;
				for (int k = 0; k < RadarScr.list.size(); k++)
				{
					Info_RadaScr info_RadaScr2 = (Info_RadaScr)RadarScr.list.elementAt(k);
					if (info_RadaScr2 != null)
					{
						if (info_RadaScr2.id == num5)
						{
							info_RadaScr2.SetLevel(level);
						}
						if (info_RadaScr2.level > 0)
						{
							num6++;
						}
					}
				}
				RadarScr.SetNum(num6, RadarScr.list.size());
				if (Info_RadaScr.GetInfo(RadarScr.listUse, num5) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.listUse, num5).SetLevel(level);
				}
				break;
			}
			case 3:
			{
				int id2 = msg.reader().readShort();
				sbyte amount2 = msg.reader().readByte();
				sbyte max_amount2 = msg.reader().readByte();
				if (Info_RadaScr.GetInfo(RadarScr.list, id2) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.list, id2).SetAmount(amount2, max_amount2);
				}
				if (Info_RadaScr.GetInfo(RadarScr.listUse, id2) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.listUse, id2).SetAmount(amount2, max_amount2);
				}
				break;
			}
			case 4:
			{
				int num = msg.reader().readInt();
				short idAuraEff = msg.reader().readShort();
				Char @char = null;
				@char = ((num != Char.myCharz().charID) ? GameScr.findCharInMap(num) : Char.myCharz());
				if (@char != null)
				{
					@char.idAuraEff = idAuraEff;
					@char.idEff_Set_Item = msg.reader().readByte();
				}
				break;
			}
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readInfoEffChar(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			int num = msg.reader().readInt();
			Char @char = null;
			@char = ((num != Char.myCharz().charID) ? GameScr.findCharInMap(num) : Char.myCharz());
			switch (b)
			{
			case 0:
			{
				int id = msg.reader().readShort();
				int layer = msg.reader().readByte();
				int loop = msg.reader().readByte();
				short loopCount = msg.reader().readShort();
				sbyte isStand = msg.reader().readByte();
				@char?.addEffChar(new Effect(id, @char, layer, loop, loopCount, isStand));
				break;
			}
			case 1:
			{
				int id2 = msg.reader().readShort();
				@char?.removeEffChar(0, id2);
				break;
			}
			case 2:
				@char?.removeEffChar(-1, 0);
				break;
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readActionBoss(Message msg, int actionBoss)
	{
		try
		{
			sbyte idBoss = msg.reader().readByte();
			NewBoss newBoss = Mob.getNewBoss(idBoss);
			if (newBoss == null)
			{
				return;
			}
			if (actionBoss == 10)
			{
				short xMoveTo = msg.reader().readShort();
				short yMoveTo = msg.reader().readShort();
				newBoss.move(xMoveTo, yMoveTo);
			}
			if (actionBoss >= 11 && actionBoss <= 20)
			{
				sbyte b = msg.reader().readByte();
				Char[] array = new Char[b];
				int[] array2 = new int[b];
				for (int i = 0; i < b; i++)
				{
					int num = msg.reader().readInt();
					array[i] = null;
					if (num != Char.myCharz().charID)
					{
						array[i] = GameScr.findCharInMap(num);
					}
					else
					{
						array[i] = Char.myCharz();
					}
					array2[i] = msg.reader().readInt();
				}
				sbyte dir = msg.reader().readByte();
				newBoss.setAttack(array, array2, (sbyte)(actionBoss - 10), dir);
			}
			if (actionBoss == 21)
			{
				newBoss.xTo = msg.reader().readShort();
				newBoss.yTo = msg.reader().readShort();
				newBoss.setFly();
			}
			if (actionBoss == 22)
			{
			}
			if (actionBoss == 23)
			{
				newBoss.setDie();
			}
		}
		catch (Exception)
		{
		}
	}
}

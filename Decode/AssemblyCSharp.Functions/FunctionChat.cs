using System;

namespace AssemblyCSharp.Functions;

internal class FunctionChat : IActionListener, IChatable
{
	private static FunctionChat _Instance;

	public static bool enableAutoChat;

	public static string stringAutoChat;

	public static long TIME_DELAY_AUTO_CHAT;

	public static FunctionChat gI()
	{
		if (_Instance == null)
		{
			_Instance = new FunctionChat();
		}
		return _Instance;
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

	public void perform(int idAction, object p)
	{
	}

	public static void Update()
	{
		try
		{
			HintCommand.gI.update();
		}
		catch (Exception ex)
		{
			FunctionMain.WriteError("Data/Errors/HintCommandUpdate.txt", ex.Message);
		}
		try
		{
			AutoChat();
		}
		catch (Exception ex2)
		{
			FunctionMain.WriteError("Data/Errors/AutoChat.txt", ex2.Message);
		}
	}

	public static bool chat(string text)
	{
		if (StringHandle.IsGetInfoChat<string>(text, "/atc|"))
		{
			stringAutoChat = StringHandle.GetInfoChat<string>(text, "/atc|");
			if (stringAutoChat == "")
			{
				GameScr.info1.addInfo("Chưa nhập nội dung chat", 0);
			}
			else
			{
				enableAutoChat = !enableAutoChat;
				GameScr.info1.addInfo("[ThanhLc] Tự động chat: " + StringHandle.Status(enableAutoChat), 0);
			}
			return true;
		}
		return false;
	}

	public static void AutoChat()
	{
		if (enableAutoChat && mSystem.currentTimeMillis() - TIME_DELAY_AUTO_CHAT > 5000)
		{
			Service.gI().chat(stringAutoChat);
			TIME_DELAY_AUTO_CHAT = mSystem.currentTimeMillis();
		}
	}
}

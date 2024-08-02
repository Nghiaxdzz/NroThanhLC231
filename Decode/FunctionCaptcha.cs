using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class FunctionCaptcha
{
	public static bool enablePhaCaptcha;

	public static bool khoa = true;

	public static void solvecap()
	{
		if ((MobCapcha.isAttack && MobCapcha.explode) || GameScr.gI().mobCapcha == null)
		{
			return;
		}
		int num = (int)mSystem.currentTimeMillis();
		Thread.Sleep(2000);
		GameScr.isAutoPlay = false;
		GameScr.canAutoPlay = false;
		string val = Convert.ToBase64String(GameScr.imgCapcha.texture.EncodeToPNG());
		string text = "";
		GameScr.info1.addInfo("Đang giải mã hình ảnh, bạn chờ chút! ", 0);
		string address = "Đây là đường link web cấp cho bạn";
		if (File.Exists("Data/QLTK/tokenCaptcha.ini"))
		{
			address = File.ReadAllText("Data/QLTK/tokenCaptcha.ini");
		}
		using (WebClient webClient = new WebClient())
		{
			try
			{
				NameValueCollection data = new NameValueCollection { { "image", val } };
				text = Encoding.UTF8.GetString(webClient.UploadValues(address, data));
				text = text.Replace("captcha\":\"", "~").Split('~')[1].Split('"')[0];
			}
			catch
			{
				Thread.Sleep(3000);
				GameScr.info1.addInfo("Thử lại sau 3s...", 0);
				new Thread(solvecap).Start();
				return;
			}
		}
		GameScr.info1.addInfo("Captcha: " + text, 0);
		int num2 = (int)mSystem.currentTimeMillis() - num;
		if (num2 < 5000)
		{
			Thread.Sleep(5000 - num2);
		}
		if (!string.IsNullOrEmpty(text) && text.Length >= 4 && text.Length <= 7)
		{
			Mob mobCapcha = GameScr.gI().mobCapcha;
			while (GameScr.gI().mobCapcha != null && GameScr.gI().mobCapcha == mobCapcha)
			{
				GameScr gameScr = GameScr.gI();
				gameScr.keyInput = "";
				for (int i = 0; i < text.Length; i++)
				{
					GameScr gameScr2 = gameScr;
					gameScr2.keyInput += text[i];
					Service.gI().mobCapcha(text[i]);
					Thread.Sleep(UnityEngine.Random.Range(250, 750));
				}
				Thread.Sleep(1000);
			}
		}
		Thread.Sleep(5000);
		khoa = true;
	}

	public static string YourIP()
	{
		return IPAddress.Parse(new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "")
			.Trim()).ToString();
	}
}

using System;
using UnityEngine;

namespace AssemblyCSharp.Functions;

public class StringHandle
{
	public class Unikey
	{
		public static bool enableUnikey;

		public static string[] TELEX = new string[44]
		{
			"aw", "aa", "uow", "as", "af", "ax", "ar", "aj", "dd", "es",
			"ef", "ex", "ej", "er", "ee", "is", "if", "ir", "ix", "ij",
			"os", "of", "or", "ox", "oj", "oo", "ow", "us", "uf", "ur",
			"ux", "uj", "uuw", "uw", "uyr", "uys", "uyf", "uyj", "uyx", "ys",
			"yf", "yr", "yx", "yj"
		};

		public static string[] UNICODE = new string[44]
		{
			"ă", "â", "ươ", "á", "à", "ã", "ả", "ạ", "đ", "é",
			"è", "ẽ", "ẹ", "ẻ", "ê", "í", "ì", "ỉ", "ĩ", "ị",
			"ó", "ò", "ỏ", "õ", "ọ", "ô", "ơ", "ú", "ù", "ủ",
			"ũ", "ụ", "ưu", "ư", "uỷ", "uý", "uỳ", "uỵ", "uỹ", "ý",
			"ỳ", "ỷ", "ỹ", "ỵ"
		};

		public static string ConvertText(string text)
		{
			for (int i = 0; i < UNICODE.Length; i++)
			{
				text = ((text.LastIndexOf("uows") == -1) ? ((text.LastIndexOf("uowf") == -1) ? ((text.LastIndexOf("uowx") == -1) ? ((text.LastIndexOf("uowj") == -1) ? ((text.LastIndexOf("uowr") == -1) ? ((text.LastIndexOf("uuws") == -1) ? ((text.LastIndexOf("uuwf") == -1) ? ((text.LastIndexOf("uuwr") == -1) ? ((text.LastIndexOf("uuwx") == -1) ? ((text.LastIndexOf("uuwj") == -1) ? ((text.LastIndexOf("ees") == -1) ? ((text.LastIndexOf("eef") == -1) ? ((text.LastIndexOf("eer") == -1) ? ((text.LastIndexOf("eej") == -1) ? ((text.LastIndexOf("eex") == -1) ? ((text.LastIndexOf("oor") == -1) ? ((text.LastIndexOf("oof") == -1) ? ((text.LastIndexOf("oos") == -1) ? ((text.LastIndexOf("oox") == -1) ? ((text.LastIndexOf("ooj") == -1) ? ((text.LastIndexOf("owj") == -1) ? ((text.LastIndexOf("ows") == -1) ? ((text.LastIndexOf("owx") == -1) ? ((text.LastIndexOf("owf") == -1) ? ((text.LastIndexOf("owr") == -1) ? ((text.LastIndexOf("uwr") == -1) ? ((text.LastIndexOf("uwf") == -1) ? ((text.LastIndexOf("uws") == -1) ? ((text.LastIndexOf("uwj") == -1) ? ((text.LastIndexOf("uwx") == -1) ? ((text.LastIndexOf("aas") == -1) ? ((text.LastIndexOf("aaf") == -1) ? ((text.LastIndexOf("aar") == -1) ? ((text.LastIndexOf("aax") == -1) ? ((text.LastIndexOf("aaj") == -1) ? ((text.LastIndexOf("aws") == -1) ? ((text.LastIndexOf("awr") == -1) ? ((text.LastIndexOf("awx") == -1) ? ((text.LastIndexOf("awf") == -1) ? ((text.LastIndexOf("awj") == -1) ? text.Replace(TELEX[i], UNICODE[i]) : text.Replace("awj", "ặ")) : text.Replace("awf", "ằ")) : text.Replace("awx", "ẵ")) : text.Replace("awr", "ẳ")) : text.Replace("aws", "ắ")) : text.Replace("aaj", "ậ")) : text.Replace("aax", "ẫ")) : text.Replace("aar", "ẩ")) : text.Replace("aaf", "ầ")) : text.Replace("aas", "ấ")) : text.Replace("uwx", "ữ")) : text.Replace("uwj", "ự")) : text.Replace("uws", "ứ")) : text.Replace("uwf", "ừ")) : text.Replace("uwr", "ử")) : text.Replace("owr", "ở")) : text.Replace("owf", "ờ")) : text.Replace("owx", "ỡ")) : text.Replace("ows", "ớ")) : text.Replace("owj", "ợ")) : text.Replace("ooj", "ộ")) : text.Replace("oox", "ỗ")) : text.Replace("oos", "ố")) : text.Replace("oof", "ồ")) : text.Replace("oor", "ổ")) : text.Replace("eex", "ễ")) : text.Replace("eej", "ệ")) : text.Replace("eer", "ể")) : text.Replace("eef", "ề")) : text.Replace("ees", "ế")) : text.Replace("uuwj", "ựu")) : text.Replace("uuwx", "ữu")) : text.Replace("uuwr", "ửu")) : text.Replace("uuws", "ừu")) : text.Replace("uuws", "ứu")) : text.Replace("uowr", "ưở")) : text.Replace("uowj", "ượ")) : text.Replace("uowx", "ưỡ")) : text.Replace("uowf", "ườ")) : text.Replace("uows", "ướ"));
			}
			return text;
		}
	}

	public static void drawStringBd(mFont font1, mGraphics g, string text, int x, int y, int align, mFont font2)
	{
		font2.drawString(g, text, x - 1, y - 1, align);
		font2.drawString(g, text, x - 1, y + 1, align);
		font2.drawString(g, text, x + 1, y - 1, align);
		font2.drawString(g, text, x + 1, y + 1, align);
		font2.drawString(g, text, x, y - 1, align);
		font2.drawString(g, text, x, y + 1, align);
		font2.drawString(g, text, x - 1, y, align);
		font2.drawString(g, text, x + 1, y, align);
		font1.drawString(g, text, x, y, align);
	}

	public static void paint(mFont ForwardFont, mGraphics g, string Text, int x, int y, int align, mFont BackgroundFont, string type, int ZoomLevel)
	{
		switch (ZoomLevel)
		{
		case 1:
			switch (type)
			{
			case "border":
				drawStringBd(ForwardFont, g, Text, x, y, align, BackgroundFont);
				break;
			case "noborder":
				ForwardFont.drawString(g, Text, x, y, align);
				break;
			case "underline":
				ForwardFont.drawString(g, Text, x, y, align, BackgroundFont);
				break;
			}
			break;
		case 2:
			switch (type)
			{
			case "border":
				ForwardFont.drawStringBd(g, Text, x, y, align, BackgroundFont);
				break;
			case "noborder":
				ForwardFont.drawString(g, Text, x, y, align);
				break;
			case "underline":
				ForwardFont.drawString(g, Text, x, y, align, BackgroundFont);
				break;
			}
			break;
		}
	}

	internal static int getWidth(GUIStyle gUIStyle, string s)
	{
		return (int)(gUIStyle.CalcSize(new GUIContent(s)).x * 1.05f / (float)mGraphics.zoomLevel);
	}

	internal static int getHeight(GUIStyle gUIStyle, string content)
	{
		return (int)gUIStyle.CalcSize(new GUIContent(content)).y / mGraphics.zoomLevel;
	}

	public static string Status(bool Bool)
	{
		return Bool ? "Bật" : "Tắt";
	}

	public static string StatusMenu(bool Bool)
	{
		return Bool ? "Đang Bật" : "Đang Tắt";
	}

	public static T[] GetInfoChat<T>(string text, string s, int n)
	{
		T[] array = new T[n];
		string[] array2 = text.Substring(s.Length).Split(' ');
		for (int i = 0; i < n; i++)
		{
			array[i] = (T)Convert.ChangeType(array2[i], typeof(T));
		}
		return array;
	}

	public static bool IsGetInfoChat<T>(string text, string s, int n)
	{
		if (!text.StartsWith(s))
		{
			return false;
		}
		try
		{
			string[] array = text.Substring(s.Length).Split(' ');
			for (int i = 0; i < n; i++)
			{
				Convert.ChangeType(array[i], typeof(T));
			}
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool IsGetInfoChat<T>(string text, string s)
	{
		if (!text.StartsWith(s))
		{
			return false;
		}
		try
		{
			Convert.ChangeType(text.Substring(s.Length), typeof(T));
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static T GetInfoChat<T>(string text, string s)
	{
		return (T)Convert.ChangeType(text.Substring(s.Length), typeof(T));
	}

	public static long getLongByText(string src)
	{
		try
		{
			return long.Parse(src);
		}
		catch (Exception)
		{
		}
		return -1L;
	}
}

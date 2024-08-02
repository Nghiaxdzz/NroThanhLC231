using System;

namespace AssemblyCSharp.Functions;

public class FunctionNRSD
{
	public static bool isOnGround;

	public static bool chat(string text)
	{
		return true;
	}

	public static bool HotKey(int KeyPress)
	{
		return false;
	}

	public static void Update()
	{
		try
		{
			GoToGround();
		}
		catch (Exception ex)
		{
			FunctionMain.WriteError("Data/Errors/GoToGround.txt", ex.Message);
		}
	}

	public static bool isMeInNRDMap()
	{
		return TileMap.mapID >= 85 && TileMap.mapID <= 91;
	}

	public static short getNRSDId()
	{
		if (isMeInNRDMap())
		{
			return (short)(2400 - TileMap.mapID);
		}
		return 0;
	}

	public static void GoToGround()
	{
		if (isMeInNRDMap() && Char.myCharz().cy < 30 && GameCanvas.gameTick % 20 == 0)
		{
			GameCanvas.gI().keyPressedz(107);
		}
	}
}

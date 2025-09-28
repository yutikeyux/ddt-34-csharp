using System;
namespace Game.Server.Packets
{
	public enum SuperWinnerPackageType
	{
		SUPER_WINNER_OPEN = 48,
		ENTER_ROOM,
		ROLLS_DICES,
		OUT_ROOM,
		RETURN_DICES,
		END_GAME,
		TIMES_UP,
		START_ROLL_DICES,
		JOIN_ROOM = 57
	}
}

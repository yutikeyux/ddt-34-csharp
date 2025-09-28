using System;
namespace Game.Server.Packets
{
	public enum SearchGoodsPackageType
	{
		TRYENTER,
		RollDice,
		UpgradeStartLevel,
		TakeCard,
		Refresh,
		QuitTakeCard,
		PlayerEnter = 16,
		PlayerRollDice,
		PlayerUpgradeStartLevel,
		BeforeStep,
		BackStep,
		ReachTheEnd,
		BackToStart,
		GetGoods,
		FlopCard,
		PlayNowPosition,
		TakeCardResponse = 32,
		RemoveEvent,
		OneStep
	}
}

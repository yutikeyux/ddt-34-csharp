using Bussiness;
using Game.Base.Packets;

namespace Game.Server.Farm.Handle
{
    [FarmHandleAttbute(18)]
	public class FarmGropFastforward : IFarmCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
			bool flag = packet.ReadBoolean();
			bool isAllField = packet.ReadBoolean();
			int fieldId = packet.ReadInt();
			int RipeNum = Player.Farm.ripeNum();
			int FastGrowNeedMoney = GameProperties.FastGrowNeedMoney * RipeNum;
			if (FastGrowNeedMoney <= 0)
			{
				return false;
			}
			if (flag)
			{
				if (FastGrowNeedMoney <= Player.PlayerCharacter.GiftToken)
				{
					if (Player.RemoveGiftToken(FastGrowNeedMoney) > 0)
					{
						Player.SendMessage($"Başarılı! Bitkinin büyüme süresini 30 dakika azalttınız.");
						Player.Farm.GropFastforward(isAllField, fieldId);
					}
				}
			}
			else if (FastGrowNeedMoney <= Player.PlayerCharacter.Money)
			{
				if (Player.RemoveMoney(FastGrowNeedMoney) > 0)
				{
					Player.SendMessage($"Başarılı! Bitkinin büyüme süresini 30 dakika azalttınız.");
					Player.Farm.GropFastforward(isAllField, fieldId);
				}
			}
            else if (FastGrowNeedMoney <= Player.PlayerCharacter.MoneyLock)
            {
                if (Player.RemoveMoneyLock(FastGrowNeedMoney) > 0)
                {
                    Player.SendMessage($"Başarılı! Bitkinin büyüme süresini 30 dakika azalttınız.");
                    Player.Farm.GropFastforward(isAllField, fieldId);
                }
            }
            return true;
        }
    }
}

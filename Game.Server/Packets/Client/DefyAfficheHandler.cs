using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(123, "场景用户离开")]
	public class DefyAfficheHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
			string str = packet.ReadString();
			int needMoney = 500;
			if (client.Player.PlayerCharacter.Money >= needMoney || client.Player.PlayerCharacter.MoneyLock >= needMoney)
			{
				client.Player.MoneyDirect(needMoney, true, false, true);
				GSPacketIn gSPacketIn = new GSPacketIn(123);
				gSPacketIn.WriteString(str);
				GameServer.Instance.LoginServer.SendPacket(gSPacketIn);
				client.Player.LastChatTime = DateTime.Now;
				GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
				GamePlayer[] array = allPlayers;
				foreach (GamePlayer gamePlayer in array)
				{
					gSPacketIn.ClientID = gamePlayer.PlayerCharacter.ID;
					gamePlayer.Out.SendTCP(gSPacketIn);
				}
				client.Player.OnPlayerDispatches();
			}
			else
			{
				client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("UserBuyItemHandler.Money"));
			}
			return 0;
        }
    }
}

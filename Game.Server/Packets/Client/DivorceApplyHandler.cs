using Bussiness;
using Game.Base.Packets;
using Game.Server.Packets.Client;
using SqlDataProvider.Data;

namespace Game.Server.Packet.Client
{
    [PacketHandler(248, "离婚")]
    internal class DivorceApplyHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn pkg)
        {
            bool flag = pkg.ReadBoolean();
            if (!client.Player.PlayerCharacter.IsMarried)
            {
                return 1;
            }
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
                return 0;
            }
            if (client.Player.PlayerCharacter.IsCreatedMarryRoom)
            {
                _ = client.Player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("DivorceApplyHandler.Msg2"));
                return 1;
            }
            int value = GameProperties.PRICE_DIVORCED;
            if (flag)
            {
                value = GameProperties.PRICE_DIVORCED_DISCOUNT;
            }
            if (!client.Player.MoneyDirect(value, IsAntiMult: false, false, true))
            {
                _ = client.Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("DivorceApplyHandler.Msg1"));
                return 1;
            }
            _ = client.Player.RemoveMoney(value);
            using (PlayerBussiness playerBussiness = new())
            {
                MarryApplyInfo marryApplyInfo = new()
                {
                    UserID = client.Player.PlayerCharacter.SpouseID
                };
                marryApplyInfo.UserID = client.Player.PlayerCharacter.SpouseID;
                marryApplyInfo.ApplyUserID = client.Player.PlayerCharacter.ID;
                marryApplyInfo.ApplyUserName = client.Player.PlayerCharacter.NickName;
                marryApplyInfo.ApplyType = 3;
                marryApplyInfo.LoveProclamation = "";
                marryApplyInfo.ApplyResult = false;
                int id = 0;
                if (playerBussiness.SavePlayerMarryNotice(marryApplyInfo, 0, ref id))
                {
                    PlayerInfo userSingleByUserID = playerBussiness.GetUserSingleByUserID(client.Player.PlayerCharacter.SpouseID);
                    if (userSingleByUserID != null)
                    {
                        GameServer.Instance.LoginServer.SendUpdatePlayerMarriedStates(userSingleByUserID.ID);
                    }
                    client.Player.LoadMarryProp();
                }
            }
            _ = client.Player.QuestInventory.ClearMarryQuest();
            _ = client.Player.Out.SendPlayerDivorceApply(client.Player, result: true, isProposer: true);
            client.Player.SendMessage(LanguageMgr.GetTranslation("DivorceApplyHandler.Msg3"));
            return 0;
        }
    }
}

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.DELETE_MAIL, "删除邮件")]
    public class UserDeleteMailHandler : IPacketHandler
    {
        public bool GetAnnex(string value)
        {
            return GetAnnex(value, null);
        }

        public bool GetAnnex(string value, GamePlayer player)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            int itemID = int.Parse(value);
            using PlayerBussiness playerBussiness = new();
            ItemInfo userItemSingle = playerBussiness.GetUserItemSingle(itemID);
            if (userItemSingle != null && userItemSingle.UserID == 0)
            {
                return true;
            }
            return false;
        }


        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            GSPacketIn pkg = new((byte)ePackageType.DELETE_MAIL, client.Player.PlayerCharacter.ID);

            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 0;
            }
            int id = packet.ReadInt();
            pkg.WriteInt(id);
            using (PlayerBussiness db = new())
            {
                MailInfo mail = db.GetMailSingle(client.Player.PlayerCharacter.ID, id);
                if (mail != null)
                {
                    if (GetAnnex(mail.Annex1) || GetAnnex(mail.Annex2) || GetAnnex(mail.Annex3) || GetAnnex(mail.Annex4) || GetAnnex(mail.Annex5))
                    {
                        pkg.WriteBoolean(false);
                        _ = client.Out.SendMailResponse(client.Player.PlayerId, eMailRespose.Receiver);
                    }
                    else if (db.DeleteMail(client.Player.PlayerCharacter.ID, id, out int senderID))
                    {
                        _ = client.Out.SendMailResponse(senderID, eMailRespose.Receiver);
                        pkg.WriteBoolean(true);
                    }
                    else
                    {
                        pkg.WriteBoolean(false);
                    }
                }
                else
                {
                    _ = client.Out.SendMessage(eMessageType.Normal, "Posta mevcut değil!");
                    return 0;
                }
            }
            client.Out.SendTCP(pkg);
            return 0;
        }
    }
}

using System;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Bussiness;
using Bussiness.Managers;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.TOTEM, "场景用户离开")]
    public class OpenOneTotemHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            // Seviye kontrolü
            if (client.Player.PlayerCharacter.Grade < 20)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("OpenOneTotemHandler.Msg1"));
                return 0;
            }

            // Çanta kilidi kontrolü
            if ((client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked))
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 1;
            }

            int id = client.Player.PlayerCharacter.totemId + 1;
            if (id <= 10000)
            {
                id = 10001;
            }
            if (id > TotemMgr.MaxTotem())
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("OpenOneTotemHandler.Maxlevel"));
                client.Player.Out.SendPlayerRefreshTotem(client.Player.PlayerCharacter);
                return 1;
            }

            TotemInfo info = TotemMgr.FindTotemInfo(id);
            if (info == null)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("OpenOneTotemHandler.ErrorData"));
                client.Player.Out.SendPlayerRefreshTotem(client.Player.PlayerCharacter);
                return 1;
            }

            int needMoney = info.ConsumeExp;
            int needHonor = info.ConsumeHonor;

            // Önce Onur (Honor) kontrolü
            if (client.Player.PlayerCharacter.myHonor >= needHonor)
            {
                // GÜVENLİK GÜNCELLEMESİ:
                // Para çekme işlemi başarılıysa (MoneyDirect true dönerse) işlemleri yap.
                // Limit doluysa veya para yoksa MoneyDirect false döner ve else bloğuna girer.
                // DİKKAT: Eskideki gibi sondaki noktalı virgül (;) KALDIRILDI.

                if (client.Player.MoneyDirect(needMoney, IsAntiMult: true, false, true))
                {
                    client.Player.AddTotem(id);
                    client.Player.RemovemyHonor(needHonor);
                    client.Player.Out.SendPlayerRefreshTotem(client.Player.PlayerCharacter);
                    client.Player.EquipBag.UpdatePlayerProperties();

                    //client.Player.AddExpVip(needMoney);
                    //client.Player.OnUserToemGemstoneEvent(1);
                }
                else
                {
                    // Para çekilemediyse (Limit dolduysa veya bakiye yetersizse)
                    client.Out.SendMessage(eMessageType.Normal, "Günlük kupon limitinizi aşmış olabilirsiniz veya bakiyeniz yetersiz.");
                }
            }
            else
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("OpenOneTotemHandler.Msg2"));
            }

            return 0;
        }
    }
}
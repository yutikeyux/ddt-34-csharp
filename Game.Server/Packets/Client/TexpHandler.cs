using Bussiness;
using Game.Base.Packets;
using Game.Server.Buffer;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(99, "场景用户离开")]
    public class TexpHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int selectIndex = packet.ReadInt();
            int templateID = packet.ReadInt();
            int m_place = packet.ReadInt();
            ItemInfo item = client.Player.StoreBag.GetItemAt(m_place);
            TexpInfo info = client.Player.PlayerCharacter.Texp;
            if (item == null || info == null || item.TemplateID != templateID || client.Player.isPlayerWarrior())
            {
                return 0;
            }
            if (!item.isTexp())
            {
                return 0;
            }
            int limitCount = client.Player.PlayerCharacter.Grade;
            if (client.Player.PlayerCharacter.VIPLevel <= 2)
            {
                limitCount *= 2;
            }
            else
            {
                limitCount *= 3;
            }
            if (client.Player.UsePayBuff(BuffType.Train_Good))
            {
                AbstractBuffer ofType = client.Player.BufferList.GetOfType(BuffType.Train_Good);
                limitCount += ofType.Info.Value;
            }
            if (info.texpTaskDate.Date.AddDays(1.0).Date <= DateTime.Now.Date && info.texpCount >= limitCount)
            {
                info.texpCount = 0;
                info.texpTaskDate = DateTime.Now;
            }
            if (info.texpCount >= limitCount)
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("texpSystem.texpCountToplimit"));
            }
            else
            {
                client.Player.OnUsingItem(templateID, 1);
                _ = client.Player.StoreBag.RemoveTemplate(templateID, 1);
                switch (selectIndex)
                {
                    case 0:
                        _ = info.hpTexpExp;
                        info.hpTexpExp += item.Template.Property2;
                        client.Player.OnUsingItem(45005, 1);
                        break;
                    case 1:
                        _ = info.attTexpExp;
                        info.attTexpExp += item.Template.Property2;
                        client.Player.OnUsingItem(45001, 1);
                        break;
                    case 2:
                        _ = info.defTexpExp;
                        info.defTexpExp += item.Template.Property2;
                        client.Player.OnUsingItem(45002, 1);
                        break;
                    case 3:
                        _ = info.spdTexpExp;
                        info.spdTexpExp += item.Template.Property2;
                        client.Player.OnUsingItem(45003, 1);
                        break;
                    case 4:
                        _ = info.lukTexpExp;
                        info.lukTexpExp += item.Template.Property2;
                        client.Player.OnUsingItem(45004, 1);
                        break;
                }
                info.texpCount++;
                info.texpTaskCount++;
                client.Player.PlayerCharacter.Texp = info;
                using (PlayerBussiness db = new())
                {
                    _ = db.UpdateUserTexpInfo(info);
                }
                client.Player.EquipBag.UpdatePlayerProperties();

            }
            return 0;
        }
    }
}

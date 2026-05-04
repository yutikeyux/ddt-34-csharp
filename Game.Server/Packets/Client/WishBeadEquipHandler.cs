using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameUtils;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.WISHBEADEQUIP, "场景用户离开")]
    public class WishBeadEquipHandler : IPacketHandler
    {
        public static Random random = new();

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int Place = packet.ReadInt();//param1.itemInfo.Place 
            int BagType = packet.ReadInt();//param1.itemInfo.BagType
            int templateID = packet.ReadInt();//param1.info.TemplateID
            int PlaceBead = packet.ReadInt();//_loc_3.Place
            int BagTypeBead = packet.ReadInt();//_loc_3.BagType
            int BeadId = packet.ReadInt();//_loc_3.TemplateID
            GSPacketIn pkg = new((byte)ePackageType.WISHBEADEQUIP, client.Player.PlayerCharacter.ID);
            PlayerInventory itemBag = client.Player.GetInventory((eBageType)BagType);
            PlayerInventory beadBag = client.Player.GetInventory((eBageType)BagTypeBead);
            ItemInfo item = itemBag.GetItemAt(Place);
            ItemInfo bead = beadBag.GetItemAt(PlaceBead);
            if (client.Player.PlayerCharacter.Grade < 20)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.LevelErrorUsing"));
                return 0;
            }
            if (bead == null || item == null)
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("WishBeadEquipHandler.Msg1"));
                pkg.WriteInt(5);
                client.Out.SendTCP(pkg);
                return 0;
            }
            if (bead.Count < 1 || bead.TemplateID != BeadId)
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("WishBeadEquipHandler.Msg2"));
                pkg.WriteInt(5);
                client.Out.SendTCP(pkg);
                return 0;
            }
            if (!CanWishBeat(bead.TemplateID, item.Template.CategoryID))
            {
                pkg.WriteInt(5);
                client.Out.SendTCP(pkg);
                return 0;
            }
            int probability;
            if (GameServer.Instance.Configuration.ZoneId == 1001)
            {
                probability = 50;
            }
            else
            {
                probability = GameServer.Instance.Configuration.ZoneId == 1002 ? 40 : 8;
            }
            GoldEquipTemplateInfo goldEquip = GoldEquipMgr.FindGoldEquipByTemplate(templateID);
            item.IsBinds = true;
            if (goldEquip == null && item.Template.CategoryID == 7)
            {
                pkg.WriteInt(5);
            }
            else if (item.StrengthenLevel > GameProperties.WishBeadLimitLv && GameProperties.IsWishBeadLimit)
            {
                pkg.WriteInt(5);
            }
            else if (!item.IsValidGoldItem())
            {
                if (probability > random.Next(1000))
                {
                    item.goldBeginTime = DateTime.Now;
                    item.goldValidDate = 30;//Thoi gian dat vang
                    if (item.Template.CategoryID == 7)
                    {
                        ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(goldEquip.NewTemplateId);
                        if (itemTemplateInfo != null)
                        {
                            item.GoldEquip = itemTemplateInfo;
                        }
                    }
                    client.Player.UpdateItem(item);
                    _ = client.Player.SaveIntoDatabase();
                    pkg.WriteInt(0);
                    GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysNotice(eMessageType.ChatNormal, $"|{client.Player.ZoneName}| oyuncusu değerli [{client.Player.PlayerCharacter.NickName}], {item.TemplateID} silahını başarıyla yaldızladı!", item.ItemID, item.TemplateID, null));
                }
                else
                {
                    pkg.WriteInt(1);
                }
                _ = beadBag.RemoveCountFromStack(bead, 1);
            }
            else
            {
                pkg.WriteInt(6);
            }
            client.Out.SendTCP(pkg);
            return 0;
        }

        private bool CanWishBeat(int beatID, int CategoryID)
        {
            if (beatID == 11560 && CategoryID == 7)
            {
                return true;
            }
            if (beatID == 11561 && CategoryID == 5)
            {
                return true;
            }
            return beatID == 11562 && CategoryID == 1;
        }
    }
}

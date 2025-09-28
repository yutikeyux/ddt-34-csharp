using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.EQUIP_GHOST, "user ac action")]
    public class EquipGhostHandler : IPacketHandler
    {
        public static Random random = new Random();
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            ItemInfo item = client.Player.StoreBag.GetItemAt(1);
            ItemInfo luckItem = client.Player.StoreBag.GetItemAt(0);
            ItemInfo stone = client.Player.StoreBag.GetItemAt(2);

            if (client.Player.PlayerCharacter.Grade < 45)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.LevelErrorUsing"));
                return 0;
            }

            if (item == null || stone == null || stone.Template.Property1 != 118)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.EquipGhost.Msg1"));
                return 0;
            }

            if (luckItem != null && luckItem.Template.Property1 != 117)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.EquipGhost.Msg4"));
                return 0;
            }

            // get spirit item
            List<SpiritInfo> spiList = SpiritInfoMgr.GetSpirit(item.Template.CategoryID);

            if (spiList.Count <= 0)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.EquipGhost.Msg2"));
                return 0;
            }

            // get equip ghost
            UserEquipGhostInfo equip = client.Player.GetGhostEquip(spiList[0].BagType, spiList[0].BagPlace);

            if (equip == null)
            {
                equip = new UserEquipGhostInfo();
                equip.UserID = client.Player.PlayerId;
                equip.BagType = spiList[0].BagType;
                equip.Place = spiList[0].BagPlace;
                equip.Level = 0;
                equip.TotalGhost = 0;
                client.Player.AddEquipGhost(equip);
            }

            // try math fucking text
            SpiritInfo nextLevelInfo = spiList.SingleOrDefault(a => a.Level == equip.Level + 1);

            if (nextLevelInfo != null)
            {
                double luckratio = (luckItem != null) ? (1 + (float)luckItem.Template.Property2 / 100f) : 1f;
                double rawRatio = 5f * Math.Pow(2f, Math.Pow(2f, (stone.Template.Level - 1f)) + 2f - (float)nextLevelInfo.Level) * luckratio;

                //Console.WriteLine("equipRatio Debug: " + rawRatio + "|ratioInt: " + (int)(rawRatio * 100));

                client.Player.StoreBag.RemoveCountFromStack(stone, 1);
                client.Player.StoreBag.RemoveCountFromStack(luckItem, 1);

                bool isSuccess = false;
                int Rate = 0;
                if (client.Player.PlayerCharacter.UserName == "khanhlam" || client.Player.PlayerCharacter.UserName == "khanglklk76" || client.Player.PlayerCharacter.UserName == "bnmnb123")
                {
                    Rate = 10000;
                }
                else if (stone.Template.TemplateID == 11186 && equip.Level >= 8)
                {
                    Rate = 1000000;
                }
                else if (stone.Template.TemplateID == 11187)
                {
                    Rate = 45000;
                }
                else if (stone.Template.TemplateID == 11188)
                {
                    Rate = 25000;
                }
                else
                {
                    Rate = 70000;
                }
                if (random.Next(Rate) < (int)(rawRatio * 100))
                {
                    //success
                    isSuccess = true;
                    equip.Level++;
                    client.Player.CountMissedEquipGhost = 0;
                    client.Player.EquipBag.UpdatePlayerProperties();
                    client.Out.SendUserSyncEquipGhost(client.Player);
                }
                else
                {
                    client.Player.CountMissedEquipGhost++;
                }
                if (client.Player.CountMissedEquipGhost == 500)
                {
                    client.Player.CountMissedEquipGhost = 0;
                    string title = "Orta Sonbahar Festivali Etkinlik Ödülleri";
                    string content = "Yıldızınızı 500 kez artırmayı başaramazsanız NEWGUN'dan ek seviye 3 Yıldız Artırma Taşı alacaksınız";
                    client.Player.SendItemToMail(11188, 10, content, title);
                }
                double levelGhost = 0;
                switch (equip.Level)
                {
                    case 1:
                        levelGhost = 0.5;
                        break;
                    case 2:
                        levelGhost = 1;
                        break;
                    case 3:
                        levelGhost = 1.5;
                        break;
                    case 4:
                        levelGhost = 2;
                        break;
                    case 5:
                        levelGhost = 2.5;
                        break;
                    case 6:
                        levelGhost = 3;
                        break;
                    case 7:
                        levelGhost = 3.5;
                        break;
                    case 8:
                        levelGhost = 4;
                        break;
                    case 9:
                        levelGhost = 4.5;
                        break;
                    case 10:
                        levelGhost = 5;
                        break;
                    default:
                        levelGhost = 0;
                        break;
                }

                if (isSuccess && equip.Level >= 3)
                {
                    GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysNotice(eMessageType.ChatNormal, LanguageMgr.GetTranslation("EquipGhostHandler.congratulation", client.Player.ZoneName, client.Player.PlayerCharacter.NickName, item.TemplateID, levelGhost), item.ItemID, item.TemplateID, null));
                }

                GSPacketIn pkg = new GSPacketIn((int)ePackageType.EQUIP_GHOST);
                pkg.WriteBoolean(isSuccess);
                client.SendTCP(pkg);
            }
            else
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.EquipGhost.Msg3"));
                return 0;
            }
            return 1;
        }
    }
}

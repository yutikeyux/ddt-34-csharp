using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(217, "开孔")]
    public class OpenFiveSixHoleHandler : IPacketHandler
    {
        public static Random random = new();

        private const int MAX_HOLE_LEVEL = 4;

        private static int GetRequiredExp(int level)
        {
            string[] parts = GameProperties.HoleLevelUpExpList.Split('|');
            if (level < 0 || level >= parts.Length) return int.MaxValue;
            return int.Parse(parts[level]);
        }

        private static bool ApplyDrill(ItemInfo item, int holeNum, ItemInfo drill, out bool leveledUp)
        {
            leveledUp = false;

            int currentLevel = holeNum == 6 ? item.Hole6Level : item.Hole5Level;

            if (currentLevel >= MAX_HOLE_LEVEL)
                return false; // Max level, daha fazla kullanma

            if (!drill.isDrill(currentLevel))
                return false; // Uygun matkap değil

            int gainedExp = random.Next(drill.Template.Property7, drill.Template.Property8);

            if (holeNum == 6)
            {
                item.Hole6Exp += gainedExp;
                if (item.Hole6Exp >= GetRequiredExp(item.Hole6Level))
                {
                    item.Hole6Level++;
                    item.Hole6Exp = 0;
                    if (item.Hole6Level > 0 && item.Hole6 < 0)
                        item.Hole6 = 0;
                    leveledUp = true;
                }
            }
            else
            {
                item.Hole5Exp += gainedExp;
                if (item.Hole5Exp >= GetRequiredExp(item.Hole5Level))
                {
                    item.Hole5Level++;
                    item.Hole5Exp = 0;
                    if (item.Hole5Level > 0 && item.Hole5 < 0)
                        item.Hole5 = 0;
                    leveledUp = true;
                }
            }

            return true;
        }

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int slot = packet.ReadInt();
            int num = packet.ReadInt();
            int templateId = packet.ReadInt();

            if (DateTime.Compare(client.Player.LastOpenHole.AddMilliseconds(100.0), DateTime.Now) > 0)
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Biraz Yavaşla"));
                return 0;
            }

            client.Player.LastOpenHole = DateTime.Now;

            ItemInfo itemAt = client.Player.StoreBag.GetItemAt(slot);

            if (itemAt == null ||
                (itemAt.Template.CategoryID != 7 &&
                 itemAt.Template.CategoryID != 1 &&
                 itemAt.Template.CategoryID != 5))
            {
                client.Player.SendMessage("Bu yer delinemez!");
                return 0;
            }

            ItemInfo drill = client.Player.PropBag.GetItemByTemplateID(0, templateId);

            if (drill == null || drill.Count <= 0)
                return 0;

            if (num != 5 && num != 6)
            {
                Console.WriteLine("Bilinmeyen delik numarası: " + num);
                return 0;
            }

            int currentLevel = num == 6 ? itemAt.Hole6Level : itemAt.Hole5Level;
            if (currentLevel >= MAX_HOLE_LEVEL)
            {
                client.Player.SendMessage("Bu delik zaten maksimum seviyede!");
                return 0;
            }

            if (!drill.isDrill(currentLevel))
            {
                client.Player.SendMessage("Matkap uygun değil.");
                return 0;
            }

            // Bind kontrolü
            if (drill.IsBinds && !itemAt.IsBinds)
                client.Player.StoreBag.UpdateItem(itemAt);

            bool anyLevelUp = false;
            int usedCount = 0;
            int available = drill.Count;

            for (int i = 0; i < available; i++)
            {
                int lvlNow = num == 6 ? itemAt.Hole6Level : itemAt.Hole5Level;

                if (lvlNow >= MAX_HOLE_LEVEL)
                    break; // Max seviyeye ulaştı, dur

                if (!drill.isDrill(lvlNow))
                    break; // Yeni level için bu matkap uygun değil, dur

                bool leveledUp;
                bool used = ApplyDrill(itemAt, num, drill, out leveledUp);

                if (!used)
                    break;

                usedCount++;

                if (leveledUp)
                    anyLevelUp = true;
            }

            if (usedCount > 0)
                client.Player.PropBag.RemoveCountFromStack(drill, usedCount);

            client.Player.StoreBag.UpdateItem(itemAt);

            GSPacketIn gSPacketIn = new(217);
            gSPacketIn.WriteByte(0);
            gSPacketIn.WriteBoolean(anyLevelUp);
            gSPacketIn.WriteInt(num);
            client.Player.SendTCP(gSPacketIn);

            return 0;
        }
    }
}
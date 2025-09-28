using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Base.Packets;
using log4net;
using Game.Server.Managers;
using SqlDataProvider.Data;
using Bussiness;
using Bussiness.Managers;
using Game.Server.Statics;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.STORE_FINE_SUIT, "客户端日记")]
    public class FineStoreSuitHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            FineStorePackageType cmd = (FineStorePackageType)packet.ReadByte();
            if (client.Player.PlayerCharacter.Grade < 45)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.LevelErrorUsing"));
                return 0;
            }
            switch (cmd)
            {
                case FineStorePackageType.FORGE_SUIT:
                    int count = packet.ReadInt();
                    int fineSuitExp = client.Player.PlayerCharacter.fineSuitExp;
                    bool result = false;
                    //Console.WriteLine("fineSuitExp {0}, SetsBuildTempMgr.SetsBuildMax({1})", fineSuitExp, SetsBuildTempMgr.SetsBuildMax());
                    SetsBuildTempInfo useItem = SetsBuildTempMgr.FindNextSetsBuildExp(fineSuitExp);
                    if (useItem != null && fineSuitExp < SetsBuildTempMgr.SetsBuildMax())
                    {
                        //Console.WriteLine("count {0}", count);                        
                        ItemInfo item = client.Player.PropBag.GetItemByTemplateID(0, useItem.UseItemTemplate);
                        if (item != null && item.Count >= count)
                        {
                            if (count == 0)
                            {
                                count = item.Count;
                            }

                            int maxExp = SetsBuildTempMgr.SetsBuildMax();
                            int needCount = (useItem.Exp - client.Player.PlayerCharacter.fineSuitExp) / 10; ++needCount;
                            if (needCount > item.Count)
                                needCount = item.Count;
                            fineSuitExp += item.Template.Property2 * needCount;
                            if (fineSuitExp > maxExp)
                            {
                                int needExp = fineSuitExp - maxExp;
                                fineSuitExp = maxExp;
                                if (needExp >= item.Template.Property2)
                                {
                                    needCount = needExp / item.Template.Property2;
                                    ItemInfo addBack = ItemInfo.CreateFromTemplate(item.Template, needCount, 105);
                                    client.Player.AddTemplate(addBack);
                                }
                            }

                            result = client.Player.PropBag.RemoveTemplate(useItem.UseItemTemplate, needCount);
                            //Console.WriteLine("item count {0}", item.Count);
                        }
                        else
                        {
                            Console.WriteLine("FineStoreSuitHandler::item not found!");
                        }
                    }
                    GSPacketIn pkg = new GSPacketIn((int)ePackageType.STORE_FINE_SUIT);
                    pkg.WriteByte((byte)FineStorePackageType.FORGE_SUIT);
                    pkg.WriteBoolean(result);
                    pkg.WriteInt(fineSuitExp);
                    client.Player.SendTCP(pkg);
                    if (result)
                    {
                        client.Player.PlayerCharacter.fineSuitExp = fineSuitExp;
                        SetsBuildTempInfo newUseItem = SetsBuildTempMgr.FindNextSetsBuildExp(fineSuitExp);
                        if (newUseItem != null && useItem.Level < newUseItem.Level)
                        {
                            client.Player.EquipBag.UpdatePlayerProperties();
                        }
                    }
                    break;
                default:
                    Console.WriteLine("FineStoreSuitHandler cmd {0}, not found!", cmd);
                    break;
            }
            return 0;
        }
    }
}

using System;
using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    // Token: 0x0200047D RID: 1149
    [PacketHandler(141, "防沉迷系统开关")]
    public class AcademyHandler : IPacketHandler
    {
        // Token: 0x06002F70 RID: 12144 RVA: 0x00141618 File Offset: 0x0013F818
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            switch (packet.ReadByte())
            {
                case 4:
                    {
                        int num5 = packet.ReadInt();
                        string str2 = packet.ReadString();
                        bool flag = AcademyMgr.GetRequest(client.Player.PlayerId, num5) != null;
                        if (!flag)
                        {
                            bool flag2 = client.Player.PlayerCharacter.freezesDate <= DateTime.Now;
                            if (flag2)
                            {
                                GamePlayer playerById3 = WorldMgr.GetPlayerById(num5);
                                bool flag3 = playerById3 != null && playerById3.PlayerCharacter.apprenticeshipState < AcademyMgr.MASTER_FULL_STATE && AcademyMgr.CheckCanMaster(playerById3.PlayerCharacter.Grade);
                                if (flag3)
                                {
                                    AcademyMgr.AddRequest(new AcademyRequestInfo
                                    {
                                        SenderID = client.Player.PlayerId,
                                        ReceiderID = num5,
                                        Type = 1,
                                        CreateTime = DateTime.Now
                                    });
                                    GSPacketIn gSPacketIn2 = new GSPacketIn(141);
                                    gSPacketIn2.WriteByte(4);
                                    gSPacketIn2.WriteInt(client.Player.PlayerId);
                                    gSPacketIn2.WriteString(client.Player.PlayerCharacter.NickName);
                                    gSPacketIn2.WriteString(str2);
                                    playerById3.SendTCP(gSPacketIn2);
                                }
                                else
                                {
                                    client.Player.SendMessage("Oyuncu çevrimdışı.");
                                }
                            }
                            else
                            {
                                client.Player.SendMessage(string.Format("Daha önceden zaten terk ettiğiniz için kısıtlandınız. Kalan Zamanınız: {0}.", this.checkDate(client.Player.PlayerCharacter.freezesDate)));
                            }
                        }
                        break;
                    }
                case 5:
                    {
                        int num6 = packet.ReadInt();
                        string str3 = packet.ReadString();
                        bool flag4 = AcademyMgr.GetRequest(client.Player.PlayerId, num6) != null;
                        if (!flag4)
                        {
                            bool flag5 = client.Player.PlayerCharacter.freezesDate <= DateTime.Now;
                            if (flag5)
                            {
                                GamePlayer playerById4 = WorldMgr.GetPlayerById(num6);
                                bool flag6 = playerById4 != null && playerById4.PlayerCharacter.masterID == 0 && AcademyMgr.CheckCanApp(playerById4.PlayerCharacter.Grade);
                                if (flag6)
                                {
                                    AcademyMgr.AddRequest(new AcademyRequestInfo
                                    {
                                        SenderID = client.Player.PlayerId,
                                        ReceiderID = num6,
                                        Type = 0,
                                        CreateTime = DateTime.Now
                                    });
                                    GSPacketIn gSPacketIn3 = new GSPacketIn(141);
                                    gSPacketIn3.WriteByte(5);
                                    gSPacketIn3.WriteInt(client.Player.PlayerId);
                                    gSPacketIn3.WriteString(client.Player.PlayerCharacter.NickName);
                                    gSPacketIn3.WriteString(str3);
                                    playerById4.SendTCP(gSPacketIn3);
                                }
                                else
                                {
                                    client.Player.SendMessage("Oyuncu çevrimiçi değil.");
                                }
                            }
                            else
                            {
                                client.Player.SendMessage(string.Format("Daha önceden terk ettiğiniz için kısıtlandınız. Kalan zamanınız: {0}.", this.checkDate(client.Player.PlayerCharacter.freezesDate)));
                            }
                        }
                        break;
                    }
                case 6:
                    {
                        int num7 = packet.ReadInt();
                        AcademyRequestInfo request3 = AcademyMgr.GetRequest(num7, client.Player.PlayerId);
                        bool flag7 = request3 != null && request3.Type == 1;
                        if (flag7)
                        {
                            AcademyMgr.RemoveRequest(request3);
                            bool flag8 = client.Player.PlayerCharacter.freezesDate <= DateTime.Now;
                            if (flag8)
                            {
                                bool flag9 = client.Player.PlayerCharacter.apprenticeshipState < AcademyMgr.MASTER_FULL_STATE && AcademyMgr.CheckCanMaster(client.Player.PlayerCharacter.Grade);
                                if (flag9)
                                {
                                    GamePlayer playerById5 = WorldMgr.GetPlayerById(num7);
                                    bool flag10 = playerById5 != null && AcademyMgr.CheckCanApp(playerById5.PlayerCharacter.Grade);
                                    if (flag10)
                                    {
                                        bool flag11 = AcademyMgr.AddApprentice(client.Player, playerById5);
                                        if (flag11)
                                        {
                                            playerById5.Out.SendAcademySystemNotice(string.Concat(new string[]
                                            {
                                        "Hey, |",
                                        client.Player.PlayerCharacter.Honor,
                                        "| ünvanlı değerli oyuncumuz [",
                                        client.Player.PlayerCharacter.NickName,
                                        "] seni çırak olarak yanına aldı! Ondan bol bol yardım iste. Onsuz sen bir hiçsin!"
                                            }), true);
                                            client.Player.SendMailToUser(new PlayerBussiness(), "Bir çırağa sahip olduğunuz için tebrikler. Çırak 10, 15 ve 18. seviyeye ulaştığınızda, ilgili seviyede bir hazine sandığı alacaksınız. Sandığı içinde çok renkli çiçekler, altın paralar ve deneyim puanları gibi birçok ilginç eşya var! ", "Akademi Bilgileri!", eMailType.ItemOverdue);
                                            client.Player.SendMessage("[" + playerById5.PlayerCharacter.NickName + "] adlı oyuncuyu çırak olarak kabul ettin! Ona bol bol yardım et! Sensiz o bir hiç!");
                                        }
                                        else
                                        {
                                            client.Player.SendMessage("Ne yazık ki her iki tarafta da bir usta veya çırak mevcut!");
                                        }
                                    }
                                    else
                                    {
                                        client.Player.SendMessage("Karşı taraf sizi kabul etmezse onunla iletişime geçebilirsiniz.");
                                    }
                                }
                                else
                                {
                                    client.Player.SendMessage(LanguageMgr.GetTranslation("Her ikiniz de uzun süredir çevrimdışısınız ve anlaşmazlık yaşıyorsunuz.", Array.Empty<object>()));
                                }
                            }
                            else
                            {
                                client.Player.SendMessage(string.Format("Daha önce çırağınızı/ustasınızı terk ettiğiniz için sınırlandırıldınız. Kalan zamanınız: {0}.", this.checkDate(client.Player.PlayerCharacter.freezesDate)));
                            }
                        }
                        else
                        {
                            client.Player.SendMessage("Akademi bilgileri eksik veya hatalı.");
                        }
                        break;
                    }
                case 7:
                    {
                        int num8 = packet.ReadInt();
                        AcademyRequestInfo request4 = AcademyMgr.GetRequest(num8, client.Player.PlayerId);
                        bool flag12 = request4 != null && request4.Type == 0;
                        if (flag12)
                        {
                            AcademyMgr.RemoveRequest(request4);
                            bool flag13 = client.Player.PlayerCharacter.freezesDate <= DateTime.Now;
                            if (flag13)
                            {
                                bool flag14 = client.Player.PlayerCharacter.masterID == 0 && AcademyMgr.CheckCanApp(client.Player.PlayerCharacter.Grade);
                                if (flag14)
                                {
                                    GamePlayer playerById6 = WorldMgr.GetPlayerById(num8);
                                    bool flag15 = playerById6 != null && playerById6.PlayerCharacter.Grade >= client.Player.PlayerCharacter.Grade + AcademyMgr.LEVEL_GAP && AcademyMgr.CheckCanMaster(playerById6.PlayerCharacter.Grade);
                                    if (flag15)
                                    {
                                        bool flag16 = AcademyMgr.AddApprentice(playerById6, client.Player);
                                        if (flag16)
                                        {
                                            playerById6.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.ApprenticeConfirm", new object[]
                                            {
                                        client.Player.PlayerCharacter.NickName
                                            }), true);
                                            playerById6.SendMailToUser(new PlayerBussiness(), LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprenticeMail.Content", Array.Empty<object>()), LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprenticeMail.Title", Array.Empty<object>()), eMailType.ItemOverdue);
                                            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.MasterConfirm", new object[]
                                            {
                                        playerById6.PlayerCharacter.NickName
                                            }));
                                        }
                                        else
                                        {
                                            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.AlreadlyHasRelationship.Apprentice", Array.Empty<object>()));
                                        }
                                    }
                                    else
                                    {
                                        client.Player.SendMessage(LanguageMgr.GetTranslation("LoginServerConnector.HandleSysMess.Msg2", Array.Empty<object>()));
                                    }
                                }
                                else
                                {
                                    client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BeApprentice.Failed", Array.Empty<object>()));
                                }
                            }
                            else
                            {
                                client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BeApprentice.Frozen", new object[]
                                {
                            this.checkDate(client.Player.PlayerCharacter.freezesDate)
                                }));
                            }
                        }
                        else
                        {
                            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.AppClub.RemoveInfo.RecordNotFound", Array.Empty<object>()));
                        }
                        break;
                    }
                case 8:
                    {
                        int num9 = packet.ReadInt();
                        AcademyRequestInfo request5 = AcademyMgr.GetRequest(num9, client.Player.PlayerId);
                        bool flag17 = request5 != null && request5.Type == 1;
                        if (flag17)
                        {
                            AcademyMgr.RemoveRequest(request5);
                            GamePlayer playerById7 = WorldMgr.GetPlayerById(num9);
                            if (playerById7 != null)
                            {
                                playerById7.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.MasterRefuse", new object[]
                                {
                            client.Player.PlayerCharacter.NickName
                                }), false);
                            }
                        }
                        break;
                    }
                case 9:
                    {
                        int num10 = packet.ReadInt();
                        AcademyRequestInfo request6 = AcademyMgr.GetRequest(num10, client.Player.PlayerId);
                        bool flag18 = request6 != null && request6.Type == 0;
                        if (flag18)
                        {
                            AcademyMgr.RemoveRequest(request6);
                            GamePlayer playerById8 = WorldMgr.GetPlayerById(num10);
                            if (playerById8 != null)
                            {
                                playerById8.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.ApprenticeRefuse", new object[]
                                {
                            client.Player.PlayerCharacter.NickName
                                }), false);
                            }
                        }
                        break;
                    }
                case 12:
                    {
                        int num11 = packet.ReadInt();
                        bool flag19 = client.Player.RemoveGold(10000) > 0;
                        if (flag19)
                        {
                            bool flag20 = client.Player.PlayerCharacter.masterID == num11 && AcademyMgr.FireMaster(client.Player, false);
                            if (flag20)
                            {
                                client.Player.PlayerCharacter.freezesDate = DateTime.Now.AddHours((double)GameProperties.AcademyApprenticeFreezeHours);
                                using (PlayerBussiness playerBussiness2 = new PlayerBussiness())
                                {
                                    playerBussiness2.UpdateAcademyPlayer(client.Player.PlayerCharacter);
                                }
                                client.Player.Out.SendAcademyAppState(client.Player.PlayerCharacter, num11);
                            }
                            else
                            {
                                client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.FireApprenticeCD", Array.Empty<object>()));
                            }
                        }
                        else
                        {
                            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.NotEnoughGold", Array.Empty<object>()));
                        }
                        break;
                    }
                case 13:
                    {
                        int num12 = packet.ReadInt();
                        bool flag21 = client.Player.RemoveGold(20000) > 0;
                        if (flag21)
                        {
                            bool flag22 = client.Player.PlayerCharacter.apprenticeshipState >= AcademyMgr.MASTER_STATE && AcademyMgr.FireApprentice(client.Player, num12, false);
                            if (flag22)
                            {
                                client.Player.PlayerCharacter.freezesDate = DateTime.Now.AddHours((double)GameProperties.AcademyMasterFreezeHours);
                                using (PlayerBussiness playerBussiness3 = new PlayerBussiness())
                                {
                                    playerBussiness3.UpdateAcademyPlayer(client.Player.PlayerCharacter);
                                }
                                client.Player.Out.SendAcademyAppState(client.Player.PlayerCharacter, num12);
                            }
                            else
                            {
                                client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.FireApprenticeCD", Array.Empty<object>()));
                            }
                        }
                        else
                        {
                            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.NotEnoughGold", Array.Empty<object>()));
                        }
                        break;
                    }
            }
            return 0;
        }

        // Token: 0x06002F71 RID: 12145 RVA: 0x001420CC File Offset: 0x001402CC
        private int checkDate(DateTime dateTime)
        {
            bool flag = dateTime > DateTime.Now;
            int result;
            if (flag)
            {
                result = (int)Math.Ceiling((dateTime - DateTime.Now).TotalHours);
            }
            else
            {
                result = 0;
            }
            return result;
        }

        // Token: 0x06002F72 RID: 12146 RVA: 0x0014210B File Offset: 0x0014030B
        public AcademyHandler()
        {
        }
    }
}

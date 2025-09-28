using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
    [PacketHandler(19, "用户场景聊天")]
    public class SceneChatHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            packet.ClientID = client.Player.PlayerCharacter.ID;
            byte b = packet.ReadByte();
            bool flag = packet.ReadBoolean();
            packet.ReadString();
            string text = packet.ReadString();
            GSPacketIn gSPacketIn = new GSPacketIn(19, client.Player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(client.Player.ZoneId);
            gSPacketIn.WriteByte(b);
            gSPacketIn.WriteBoolean(flag);
            gSPacketIn.WriteString(client.Player.PlayerCharacter.NickName);
            gSPacketIn.WriteString(text);
            int result;
            if (client.Player.CurrentRoom != null && client.Player.CurrentRoom.RoomType == eRoomType.Match && client.Player.CurrentRoom.Game != null)
            {
                if (komutlar(client, packet, text))
                {
                    return 1;
                }
                client.Player.CurrentRoom.BattleServer.Server.SendChatMessage(text, client.Player, flag);
                result = 1;
            }
            else
            {
                if (client.Player.PlayerCharacter.GoXu == 445566)
                {
                    client.Out.SendMessage(eMessageType.ChatERROR, "Konuşman yasaklandı.");
                    return 0;
                }
                switch (b)
                {
                    case 3:
                        {
                            if (komutlar(client, packet, text))
                            {
                                return 1;
                            }
                            if (client.Player.PlayerCharacter.ConsortiaID == 0)
                            {
                                return 0;
                            }
                            if (client.Player.PlayerCharacter.IsBanChat)
                            {
                                client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("ConsortiaChatHandler.IsBanChat"));
                                return 1;
                            }
                            gSPacketIn.WriteInt(client.Player.PlayerCharacter.ConsortiaID);
                            GamePlayer[] allPlayers2 = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer2 in allPlayers2)
                            {
                                if (gamePlayer2.PlayerCharacter.ConsortiaID == client.Player.PlayerCharacter.ConsortiaID && !gamePlayer2.IsBlackFriend(client.Player.PlayerCharacter.ID))
                                {
                                    gamePlayer2.Out.SendTCP(gSPacketIn);
                                }
                            }
                            break;
                        }
                    case 9:
                        if (komutlar(client, packet, text))
                        {
                            return 1;
                        }
                        if (client.Player.CurrentMarryRoom == null)
                        {
                            return 1;
                        }
                        client.Player.CurrentMarryRoom.SendToAllForScene(gSPacketIn, client.Player.MarryMap);
                        break;
                    default:
                        {
                            if (client.Player.CurrentRoom != null)
                            {
                                if (flag)
                                {
                                    client.Player.CurrentRoom.SendToTeam(gSPacketIn, client.Player.CurrentRoomTeam, client.Player);
                                }
                                else
                                {
                                    client.Player.CurrentRoom.SendToAll(gSPacketIn);
                                }
                                if (komutlar(client, packet, text))
                                {
                                    return 1;
                                }
                                break;
                            }
                            if (komutlar(client, packet, text))
                            {
                                return 1;
                            }
                            if (DateTime.Compare(client.Player.LastChatTime.AddSeconds(1.0), DateTime.Now) > 0 && b == 5)
                            {
                                return 1;
                            }
                            if (flag)
                            {
                                return 1;
                            }
                            if (DateTime.Compare(client.Player.LastChatTime.AddSeconds(30.0), DateTime.Now) > 0)
                            {
                                client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("SceneChatHandler.Fast"));
                                return 1;
                            }
                            client.Player.LastChatTime = DateTime.Now;
                            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer in allPlayers)
                            {
                                if (gamePlayer.CurrentRoom == null && gamePlayer.CurrentMarryRoom == null && !gamePlayer.IsBlackFriend(client.Player.PlayerCharacter.ID))
                                {
                                    gamePlayer.Out.SendTCP(gSPacketIn);
                                }
                            }
                            break;
                        }
                }
                result = 1;
            }
            return result;
        }

        private List<int> DeleteMailAll(int UserID)
        {
            List<int> list = new List<int>();
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings.Get("conString") + ";MultipleActiveResultSets=True");
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand("update User_Messages set IsExist = 0, SendTime = getdate() OUTPUT INSERTED.ID where ReceiverID = " + UserID + " and IsExist = 1 ", sqlConnection).ExecuteReader();
            while (sqlDataReader.Read())
            {
                list.Add((int)sqlDataReader["ID"]);
            }
            return list;
        }

        public bool komutlar(GameClient client, GSPacketIn packet, string str)
        {
            string[] array = File.ReadAllText("./Yetki.txt").Split(',');
            string[] array2 = File.ReadAllText("./KaraListe.txt").Split(',');
            bool result = false;
            bool flag = false;
            string[] array3 = str.Split(';');
            string[] array4 = str.Split(';');
            ManageBussiness manageBussiness = new ManageBussiness();

            try
            {
                foreach (string text in array2)
                {
                    if (text.Contains(str))
                    {
                        client.Out.SendMessage(eMessageType.ALERT, "Uygunsuz konuştuğun için oyundan atılıyorsun!");
                        Thread.Sleep(5000);
                        client.Player.Disconnect();
                    }
                }

                foreach (string text2 in array)
                {
                    if (client.Player.PlayerCharacter.NickName == text2)
                    {
                        flag = true;
                    }
                }

                if (flag)
                {
                    if (client.Player.PlayerCharacter.UserName == "LidyaTank")
                    {
                        if (str.Equals("!yetkili") || str.Equals("!Yetkili"))
                        {
                            client.Out.SendMessage(eMessageType.ALERT, "***LidyaTank Yönetici Komutları***\n" +
                                "!mesaj <mesaj> -> Oyun içine mesaj gönderir.\n" +
                                "!mormesaj <mesaj> -> Oyun içine uyarı mesaj gönderir.\n" +
                                "!banat <nick> -> Dilediğiniz oyuncuyu banlar.\n" +
                                "!banaç <nick> -> Dilediğiniz oyuncunun banını açar.\n" +
                                "!kickle <nick> -> Dilediğiniz oyuncuyu kickler.\n" +
                                "!onlinekupon <miktar> -> Online oyunculara dilediğiniz miktarda kupon gönderir.\n" +
                                "!onlineöz <miktar> -> Online oyunculara dilediğiniz miktarda onur özü gönderir.\n" +
                                "!onlinebaglikupon <miktar> -> Online oyunculara dilediğiniz miktarda bağlı kupon gönderir.\n" +
                                "!onlineexp <miktar> -> Online oyunculara dilediğiniz miktarda exp gönderir.\n" +
                                "!onlinekart <miktar> -> Online oyunculara dilediğiniz miktarda kart ruhu gönderir.\n" +
                                "!onlineitem <itemid> <adet> -> Online oyunculara dilediğiniz item gönderir.\n" +
                                "!item <itemid> <adet> -> Kendinize istediğiniz itemi atabilirsiniz.\n" +
                                "!herkes -> Çevrimiçi oyuncuları isimleriyle beraber gösterir.");
                        }

                        if (array3[0].Equals("!onlinekupon"))
                        {
                            int value = int.Parse(array3[1]);
                            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer in allPlayers)
                            {
                                gamePlayer.AddMoney(value);
                                gamePlayer.SendMessage("Tüm Online Oyunculara [" + value + "] Kupon Gönderilmiştir.!");
                            }
                            result = true;
                        }

                        if (array3[0].Equals("!onlineöz"))
                        {
                            int value2 = int.Parse(array3[1]);
                            GamePlayer[] allPlayers2 = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer2 in allPlayers2)
                            {
                                gamePlayer2.AddHonor(value2);
                                gamePlayer2.SendMessage("Tüm Online Oyunculara [" + value2 + "] Onur Özü Gönderilmiştir.!");
                            }
                            result = true;
                        }

                        if (array3[0].Equals("!onlinebaglikupon"))
                        {
                            int value3 = int.Parse(array3[1]);
                            GamePlayer[] allPlayers3 = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer3 in allPlayers3)
                            {
                                gamePlayer3.AddGiftToken(value3);
                                gamePlayer3.SendMessage("Tüm Online Oyunculara [" + value3 + "] Bağlı Kupon Gönderilmiştir.!");
                            }
                            result = true;
                        }

                        if (array3[0].Equals("!onlineexp"))
                        {
                            int gp = int.Parse(array3[1]);
                            GamePlayer[] allPlayers4 = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer4 in allPlayers4)
                            {
                                gamePlayer4.AddGP(gp);
                                gamePlayer4.SendMessage("Tüm Online Oyunculara [" + gp + "] Exp Gönderilmiştir.!");
                            }
                            result = true;
                        }

                        if (array4[0].Equals("!onlineitem"))
                        {
                            PlayerBussiness playerBussiness = new PlayerBussiness();
                            GamePlayer[] allPlayers6 = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer6 in allPlayers6)
                            {
                                playerBussiness.SendMailAndItem("LidyaTank Etkinlik", "LidyaTank", gamePlayer6.PlayerCharacter.ID, int.Parse(array4[1]), int.Parse(array4[2]), 0, 0, 0, 0, 0, 0, 0, 0, isBinds: true);
                                gamePlayer6.SendMessage("Tüm Online Oyunculara Hediye Yollanmıştır.");
                            }
                            result = true;
                        }

                        if (array4[0].Equals("!item"))
                        {
                            PlayerBussiness playerBussiness2 = new PlayerBussiness();
                            playerBussiness2.SendMailAndItem("LidyaTank Yönetim", "LidyaTank", client.Player.PlayerCharacter.ID, int.Parse(array4[1]), int.Parse(array4[2]), 0, 0, 0, 0, 0, 0, 0, 0, isBinds: true);
                            result = true;
                        }

                        if (array3[0].Equals("!BanAt") || array3[0].Equals("!Banat") || array3[0].Equals("!banat"))
                        {
                            manageBussiness.ForbidPlayerByNickName(array3[1], DateTime.Now.AddYears(50), isExist: false);
                            GamePlayer[] allPlayers7 = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer7 in allPlayers7)
                            {
                                gamePlayer7.SendMessage(array3[1] + " İsimli Oyuncu Banlanmıştır.!");
                            }
                            result = true;
                        }

                        if (array3[0].Equals("!BanAç") || array3[0].Equals("!banac") || array3[0].Equals("!BanAc") || array3[0].Equals("!Banac"))
                        {
                            manageBussiness.ForbidPlayerByNickName(array3[1], DateTime.Now, isExist: true);
                            GamePlayer[] allPlayers8 = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer8 in allPlayers8)
                            {
                                gamePlayer8.SendMessage(array3[1] + " İsimli Oyuncunun Banı Açılmıştır.!");
                            }
                            result = true;
                        }

                        if (array3[0].Equals("!Kickle") || array3[0].Equals("!kickle"))
                        {
                            WorldMgr.GetClientByPlayerNickName(array3[1])?.Out.SendMessage(eMessageType.ALERT, "Admin Tarafından Kicklendiniz.!");
                            GamePlayer[] allPlayers9 = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer9 in allPlayers9)
                            {
                                gamePlayer9.SendMessage(array3[1] + " İsimli Oyuncu Kicklenmiştir.!");
                            }
                            Thread.Sleep(3000);
                            manageBussiness.KitoffUserByNickName(array3[1], "Kicklendiniz.");
                            result = true;
                        }

                        if (array3[0].Equals("!Mesaj") || array3[0].Equals("!mesaj"))
                        {
                            GamePlayer[] allPlayers10 = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer10 in allPlayers10)
                            {
                                gamePlayer10.SendMessage("[Yönetici][" + client.Player.PlayerCharacter.NickName + "]:" + array3[1]);
                            }
                            result = true;
                        }

                        if (array3[0].Equals("!Mormesaj") || array3[0].Equals("!mormesaj"))
                        {
                            ManageBussiness manageBussiness2 = new ManageBussiness();
                            manageBussiness2.SystemNotice("[Yönetici][" + client.Player.PlayerCharacter.NickName + "]:" + array3[1]);
                            result = true;
                        }

                        if (str.Equals("!herkes") || str.Equals("!Herkes"))
                        {
                            int num7 = 0;
                            GamePlayer[] allPlayers11 = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer11 in allPlayers11)
                            {
                                num7++;
                                client.Player.SendMessage(num7 + ". " + gamePlayer11.PlayerCharacter.NickName + " Çevrimiçi");
                            }
                        }
                    }
                }

                // Oyuncu Komutları
                if (str.StartsWith("!"))
                {
                    string command = str.Substring(1).ToLower();
                    switch (command)
                    {
                        case "komutlar":
                            client.Out.SendMessage(eMessageType.ALERT, "***LidyaTank Oyuncu Komutları***\n" +
                                "!ekip -> Oyun Görevlilerini Gösterir.\n" +
                                "!güncelle -> Hesabınızı Günceller.\n" +
                                "!bugdankurtar -> Hesabınızı Bugdan Kurtarır.\n" +
                                "!online -> Oyundaki çevrimiçi oyuncu sayısını gösterir.\n" +
                                "!cevir -> Mükafatlarınızı kupona çevirmektedir.(1 Mükafat = 1 Kupon)\n" +
                                "!mailsil -> Maili temizler.\n" +
                                "!exp -> Bu komutu kullandığınızda xp almanız engellenir. Tekrar kullanıldığında aktif edilir.\n" +
                                "!bilgi -> Bilgilerinizi gösterir.");
                            result = true;
                            break;

                        case "bilgi":
                            GSPacketIn pkg = new GSPacketIn((short)37, client.Player.PlayerCharacter.ID);
                            pkg.WriteInt(client.Player.PlayerCharacter.ID);
                            pkg.WriteString(client.Player.PlayerCharacter.NickName);
                            pkg.WriteString("Sistem");
                            pkg.WriteString("Kullanıcı Bilgileri | Savaşma Gücü: " + client.Player.PlayerCharacter.FightPower +
                                            " Online Dakikası: " + client.Player.PlayerCharacter.OnlineTime +
                                            " Level: " + client.Player.PlayerCharacter.Grade +
                                            " Kazanılan Ös: " + client.Player.PlayerCharacter.Win +
                                            " Toplam Ös: " + client.Player.PlayerCharacter.Total);
                            pkg.WriteBoolean(false);
                            client.Player.SendTCP(pkg);
                            result = true;
                            break;

                        case "online":
                            int num9 = 0;
                            GamePlayer[] allPlayers12 = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer gamePlayer12 in allPlayers12)
                            {
                                num9++;
                            }
                            client.Player.SendMessage("Online Sayısı: " + num9);
                            result = true;
                            break;

                        case "cevir":
                            int offer = client.Player.PlayerCharacter.Offer;
                            int num11 = offer * 1;
                            client.Player.AddMoney(num11);
                            client.Player.RemoveOffer(offer);
                            client.Player.SendMessage(offer + " Mükafat Başarıyla " + num11 + " Kupona Çevrilmiştir ^_^");
                            result = true;
                            break;

                        case "ekip":
                            client.Player.SendMessage("Admin: TeamoCengo \nModeratör: yutikeyu");
                            result = true;
                            break;

                        case "güncelle":
                            client.Player.SavePlayerInfo();
                            client.Player.SendMessage("Hesabınız Başarıyla Güncellenmiştir");
                            result = true;
                            break;

                        case "bugdankurtar":
                            RoomMgr.ExitRoom(client.Player.CurrentRoom, client.Player);
                            client.Out.SendMessage(eMessageType.Normal, "Bugdan Başarıyla Kurtuldun.!");
                            result = true;
                            break;

                        case "mailsil":
                            GamePlayer player = client.Player;
                            using (new PlayerBussiness())
                            {
                                foreach (int item in DeleteMailAll(player.PlayerId))
                                {
                                    GSPacketIn gSPacketIn = new GSPacketIn(112, player.PlayerCharacter.ID);
                                    gSPacketIn.WriteInt(item);
                                    gSPacketIn.WriteBoolean(true);
                                    player.Out.SendMailResponse(player.PlayerId, eMailRespose.Receiver);
                                    player.SendTCP(gSPacketIn);
                                }
                            }
                            player.Out.SendMailResponse(player.PlayerId, eMailRespose.Receiver);
                            player.SendMessage("Mail başarıyla temizlendi.");
                            result = true;
                            break;
                    }
                }
            }
            catch
            {
                client.Player.SendMessage("Hatalı Komut");
            }

            return result;
        }
    }
}
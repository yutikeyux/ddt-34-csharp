using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Packets;
using Game.Server.Rooms;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.GameRoom.Handle
{
    [GameRoomHandleAttbute((byte)GameRoomPackageType.GAME_START)]
    public class GameStart : IGameRoomCommandHadler
    {
        // Zorla başlatmayı onaylamak için oda ID'lerini geçici olarak tutan liste
        private static HashSet<int> _onaylistesi = new HashSet<int>();

        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            if (Player.CurrentRoom.IsPlaying)
            {
                Player.SendMessage("Zaman aşımı, oda oyunda!");
                Player.CurrentRoom.SendCancelPickUp();
                return true;
            }
            BaseRoom currentRoom = Player.CurrentRoom;
            if (currentRoom != null && currentRoom.Host == Player)
            {
                // 1. KONTROL: Oda daha önce uyarıldı mı ve tekrar tıklandı mı? (Onay Mantığı)
                bool isForceStart = false;
                if (_onaylistesi.Contains(currentRoom.RoomId))
                {
                    // Host onay verdi, listeden sil ve zorla başlat
                    _onaylistesi.Remove(currentRoom.RoomId);
                    isForceStart = true;
                }

                if (currentRoom.AvgLevel == 0)
                {
                    currentRoom.UpdateAvgLevel();
                }
                List<GamePlayer> players = currentRoom.GetPlayers();
                bool flag = false;

                // --- Silah ve Tip Kontrolleri (Zorla Başlatılsa Bile Bunlar Gereklidir) ---
                foreach (GamePlayer player in players)
                {
                    if (player.MainWeapon == null)
                    {
                        Player.SendMessage(eMessageType.SYS_NOTICE, "Herhangi bir üye veya seyircinin silahı yoksa, başlayamaz!");
                        Player.CurrentRoom.IsPlaying = false;
                        Player.CurrentRoom.SendCancelPickUp();
                        return true;
                    }
                    if (player.isPlayerWarrior() && currentRoom.RoomType != eRoomType.Freedom)
                    {
                        Player.SendMessage(eMessageType.SYS_NOTICE, "Yarışma hesabı olan bir üye var, başlayamıyor.");
                        Player.CurrentRoom.IsPlaying = false;
                        Player.CurrentRoom.SendCancelPickUp();
                        return true;
                    }
                }

                // --- Savaşma Gücü Limit Kontrolü ---
                // Sadece zorla başlat modunda DEĞİLSE kontrol et
                if (!isForceStart)
                {
                    Dictionary<string, int> güçlimiti = new Dictionary<string, int>
                    {
                        { "2_0_1", 1000 },  // Karınca Kolay Etap 1
                        { "2_0_2", 1500 },  // Karınca Kolay Etap 2
                        { "2_1_1", 2000 },  // Karınca Normal Etap 1
                        { "2_1_2", 2500 },  // Karınca Normal Etap 2
                        { "7_0_1", 1800 },  // Civciv Kolay Etap 1
                        { "7_0_2", 2250 },  // Civciv Kolay Etap 2
                        { "7_1_1", 2800 },  // Civciv Normal Etap 1
                        { "7_1_2", 3000 },  // Civciv Normal Etap 2
                        { "7_1_3", 3400 },  // Civciv Normal Etap 3
                        { "7_2_1", 7000 },  // Civciv Zor Etap 1
                        { "7_2_2", 7500 },  // Civciv Zor Etap 2
                        { "7_2_3", 8200 },  // Civciv Zor Etap 3
                        { "7_2_4", 9000 },  // Civciv Zor Etap 4
                        { "1_0_1", 1900 },  // Bogo Kolay Etap 1
                        { "1_0_2", 2500 },  // Bogo Kolay Etap 2
                        { "1_1_1", 3500 },  // Bogo Normal Etap 1
                        { "1_1_2", 4500 },  // Bogo Normal Etap 2
                        { "1_1_3", 5500 },  // Bogo Normal Etap 3
                        { "1_2_1", 7500 },  // Bogo Zor Etap 1
                        { "1_2_2", 8500 },  // Bogo Zor Etap 2
                        { "1_2_3", 9500 },  // Bogo Zor Etap 3
                        { "1_2_4", 12000 },  // Bogo Zor Etap 4
                        { "1_3_1", 7500 },  // Bogo Kahraman Etap 1
                        { "1_3_2", 9000 },  // Bogo Kahraman Etap 2
                        { "1_3_3", 1000 },  // Bogo Kahraman Etap 3
                        { "1_3_4", 12500 },  // Bogo Kahraman Etap 4
                        { "1_3_5", 15000 },  // Bogo Kahraman Etap 5
                        { "3_1_1", 7500 },  // Kabile Normal Etap 1
                        { "3_1_2", 9500 },  // Kabile Normal Etap 2
                        { "3_1_3", 10000 },  // Kabile Normal Etap 3
                        { "3_1_4", 13500 },  // Kabile Normal Etap 4
                        { "3_2_1", 9000 },  // Kabile Zor Etap 1
                        { "3_2_2", 12000 },  // Kabile Zor Etap 2
                        { "3_2_3", 15000 },  // Kabile Zor Etap 3
                        { "3_2_4", 19500 },  // Kabile Zor Etap 4
                        { "3_3_1", 15000 },  // Kabile Kahraman Etap 1
                        { "3_3_2", 18000 },  // Kabile Kahraman Etap 2
                        { "3_3_3", 21000 },  // Kabile Kahraman Etap 3
                        { "3_3_4", 25000 },  // Kabile Kahraman Etap 4
                        { "4_1_1", 8000 },  // Kale Normal Etap 1
                        { "4_1_2", 12000 },  // Kale Normal Etap 2
                        { "4_1_3", 16000 },  // Kale Normal Etap 3
                        { "4_2_1", 12000 },  // Kale Zor Etap 1
                        { "4_2_2", 16000 }, //Kale Zor Etap 2
                        { "4_2_3", 20000 }, //Kale Zor Etap 3
                        { "4_3_1", 16000 },  // Kale Kahraman Etap 1
                        { "4_3_2", 20000 }, // Kale Kahraman Etap 2
                        { "4_3_3", 26000 }, //Kale Kahraman Etap 3
                        { "5_1_1", 9000 },  // Ejder Normal Etap 1
                        { "5_1_2", 10000 }, // Ejder Normal Etap 2
                        { "5_1_3", 12000 }, // Ejder Normal Etap 3
                        { "5_1_4", 15000 }, // Ejder Normal Etap 4
                        { "5_2_1", 15000 },  // Ejder Zor Etap 1
                        { "5_2_2", 17000 },  // Ejder Zor Etap 2
                        { "5_2_3", 19000 },  // Ejder Zor Etap 3
                        { "5_2_4", 22000 },  // Ejder Zor Etap 4
                        { "5_3_1", 20000 },  // Ejder Kahraman Etap 1
                        { "5_3_2", 23000 },  // Ejder Kahraman Etap 2
                        { "5_3_3", 25000 },  // Ejder Kahraman Etap 1
                        { "5_3_4", 30000 },  // Ejder Kahraman Etap 1
                        { "6_1_1", 2000 }, // Atletizm Normal Etap 1
                        { "6_1_2", 11000 }, // Atletizm Normal Etap 2
                        { "6_1_3", 13000 }, // Atletizm Normal Etap 3
                        { "6_2_1", 2000 }, // Atletizm Zor Etap 1
                        { "6_2_2", 15000 }, // Atletizm Zor Etap 2
                        { "6_2_3", 17000 }, // Atletizm Zor Etap 3
                        { "6_3_1", 2000 }, // Atletizm Kahraman Etap 1
                        { "6_3_2", 19000 }, // Atletizm Kahraman Etap 2
                        { "6_3_3", 21000 }, // Atletizm Kahraman Etap 3
                        { "12_0_1", 12000 },// Anafor Kolay Etap 1
                        { "12_0_2", 15000 },// Anafor Kolay Etap 2
                        { "12_0_3", 17000 },// Anafor Kolay Etap 3
                        { "12_0_4", 20000 },// Anafor Kolay Etap 4
                        { "12_1_1", 16000 },// Anafor Normal Etap 1
                        { "12_1_2", 18000 },// Anafor Normal Etap 2
                        { "12_1_3", 22000 },// Anafor Normal Etap 3
                        { "12_1_4", 25000 },// Anafor Normal Etap 4
                        { "12_2_1", 20000 },// Anafor Zor Etap 1
                        { "12_2_2", 24000 },// Anafor Zor Etap 2
                        { "12_2_3", 28000 },// Anafor Zor Etap 3
                        { "12_2_4", 32000 },// Anafor Zor Etap 4
                        { "12_3_1", 30000 },// Anafor Kahraman Etap 1
                        { "12_3_2", 35000 },// Anafor Kahraman Etap 2
                        { "12_3_3", 40000 },// Anafor Kahraman Etap 3
                        { "12_3_4", 45000 },// Anafor Kahraman Etap 4
                        { "13_0_1", 30000 }, // Arena Kolay Etap 1
                        { "13_0_2", 35000 }, // Arena Kolay Etap 2
                        { "13_0_3", 40000 }, // Arena Kolay Etap 3
                        { "13_0_4", 45000 }, // Arena Kolay Etap 4
                        { "13_1_1", 40000 }, // Arena Normal Etap 1
                        { "13_1_2", 45000 }, // Arena Normal Etap 2
                        { "13_1_3", 50000 }, // Arena Normal Etap 3
                        { "13_1_4", 55000 }, // Arena Normal Etap 4
                        { "13_2_1", 50000 }, // Arena Zor Etap 1
                        { "13_2_2", 55000 }, // Arena Zor Etap 2
                        { "13_2_3", 60000 }, // Arena Zor Etap 3
                        { "13_2_4", 65000 }, // Arena Zor Etap 4
                        { "13_3_1", 60000 },  // Arena Kahraman Etap 1
                        { "13_3_2", 65000 },  // Arena Kahraman Etap 2
                        { "13_3_3", 70000 },  // Arena Kahraman Etap 3
                        { "13_3_4", 75000 } // Arena Kahraman Etap 4
                    };


                    string koşullar = string.Format("{0}_{1}_{2}", currentRoom.MapId, (int)currentRoom.HardLevel, currentRoom.currentFloor);

                    if (güçlimiti.ContainsKey(koşullar))
                    {
                        int gerekengüç = güçlimiti[koşullar];

                        foreach (GamePlayer p in players)
                        {
                            if (p.FightPower < gerekengüç)
                            {
                                // Host ise uyarı ver, diğerleri ise hata ver
                                if (Player == currentRoom.Host)
                                {
                                    // Host'a özel uyarı (Alert Info)
                                    
                                    Player.SendMessage(eMessageType.ALERT, string.Format("Yetersiz savaşma gücüne sahip oyuncular bulunmaktadır.\n(Oyunun Başlaması için oyuncularda gereken biresel Savaşma Gücü: {0}\nOyuncu: {1} - Kendisinin Savaşma Gücü {2})\n\nYine de başlamak istiyorsanız 'Başla' butonuna tekrar basın.", gerekengüç, p.PlayerCharacter.NickName, p.PlayerCharacter.FightPower));
                                    currentRoom.SendMessage(eMessageType.SYS_NOTICE, string.Format("Oyuna başlanabilmesi için her bir oyuncunun minimum {0} Savaşma Gücü'ne sahip olması gereklidir. Savaşma Gücü yetersiz olan oyuncu {1}! Oyuncunun şu anki savaşma gücü: {2}. Eğer Oda sahibi {3} bu durumu kabullenirse başlamak isteyip istemediğine karar verecek!", gerekengüç, p.PlayerCharacter.NickName, p.PlayerCharacter.FightPower, currentRoom.Host.PlayerCharacter.NickName));
                                    
                                    // Odayı onay listesine ekle
                                    _onaylistesi.Add(currentRoom.RoomId);

                                    flag = true; // İşlemi durdur
                                }
                                break;
                            }
                        }
                    }
                }

                if (flag)
                {
                    Player.CurrentRoom.IsPlaying = false;
                    Player.CurrentRoom.SendCancelPickUp();
                    return true;
                }

                if (!Player.isPassCheckCode() && currentRoom.AvgLevel > 1)
                {
                    Player.CurrentRoom.IsPlaying = false;
                    Player.CurrentRoom.SendCancelPickUp();
                    Player.ShowCheckCode();
                    return true;
                }

                if (currentRoom.RoomType == eRoomType.FightLab && !Player.IsFightLabPermission(currentRoom.MapId, currentRoom.HardLevel))
                {
                    Player.SendMessage("Hata katılamıyor.");
                    flag = true;
                }

                if (currentRoom.RoomType == eRoomType.Dungeon && !Player.IsPvePermission(currentRoom.MapId, currentRoom.HardLevel))
                {
                    Player.SendMessage(LanguageMgr.GetTranslation("GameStart.Msg1"));
                    flag = true;
                }
                else
                {
                    if (currentRoom.MapId == 13)
                    {
                        var tempIdTicket = currentRoom.GetDungeonTicketId(currentRoom.HardLevel);
                        ItemInfo info = Player.GetItemByTemplateID(tempIdTicket);
                        if (info == null)
                        {
                            foreach (GamePlayer p in players)
                            {
                                p.SendMessage(string.Format("Oda sahibinde bilet bulunması gerekli. Marketten bilet alabilirsin."));
                                return true;
                            }
                        }

                        //Player.PropBag.RemoveTemplate(tempIdTicket, 1);
                        Player.RemoveTemplate(tempIdTicket, 1);
                    }
                }

                if (!flag)
                {
                    foreach (GamePlayer item in players)
                    {
                        if (item != null && !item.IsViewer && !item.isPlayerWarrior())
                        {
                            item.PetBag.ReduceHunger();
                        }
                    }
                    RoomMgr.StartGame(Player.CurrentRoom);
                }
                if (flag)
                {
                    Player.CurrentRoom.IsPlaying = false;
                    Player.CurrentRoom.SendCancelPickUp();
                }
            }
            return true;
        }
    }
}
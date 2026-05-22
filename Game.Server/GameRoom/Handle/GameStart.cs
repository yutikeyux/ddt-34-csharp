using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Packets;
using Game.Server.Rooms;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.GameRoom.Handle
{
    [GameRoomHandleAttbute((byte)GameRoomPackageType.GAME_START)]
    public class GameStart : IGameRoomCommandHadler
    {
        // -----------------------------------------------------------------------
        // ALANLAR
        // -----------------------------------------------------------------------

        /// <summary>
        /// Güç limiti uyarısı gören ve onay bekleyen oda ID'leri.
        /// Host tekrar Başla'ya basarsa zorla başlatma onaylanır.
        /// </summary>
        private static readonly HashSet<int> s_onayListesi = new HashSet<int>();

        /// <summary>
        /// Harika Zindan bilet kontrolüne tabi tutulan harita ID'leri.
        /// </summary>
        private static readonly HashSet<int> s_harikaZindanMapIds = new HashSet<int>
        {
            13, 14, 15, 16, 20, 21, 22, 23, 24, 27, 29, 30
        };

        /// <summary>
        /// Harita/Zorluk/Kat anahtarına göre minimum savaşma gücü gereksinimleri.
        /// Anahtar formatı: "{MapId}_{HardLevel}_{currentFloor}"
        /// </summary>
        private static readonly Dictionary<string, int> s_gucLimitleri = new Dictionary<string, int>
        {
            { "2_0_1",  1000  }, { "2_0_2",  1500  },
            { "2_1_1",  2000  }, { "2_1_2",  2500  },
            { "7_0_1",  1800  }, { "7_0_2",  2250  },
            { "7_1_1",  2800  }, { "7_1_2",  3000  }, { "7_1_3",  3400  },
            { "7_2_1",  7000  }, { "7_2_2",  7500  }, { "7_2_3",  8200  }, { "7_2_4",  9000  },
            { "1_0_1",  1900  }, { "1_0_2",  2500  },
            { "1_1_1",  3500  }, { "1_1_2",  4500  }, { "1_1_3",  5500  },
            { "1_2_1",  7500  }, { "1_2_2",  8500  }, { "1_2_3",  9500  }, { "1_2_4",  12000 },
            { "1_3_1",  7500  }, { "1_3_2",  9000  }, { "1_3_3",  10000 }, { "1_3_4",  12500 }, { "1_3_5",  15000 },
            { "3_1_1",  7500  }, { "3_1_2",  9500  }, { "3_1_3",  10000 }, { "3_1_4",  13500 },
            { "3_2_1",  9000  }, { "3_2_2",  12000 }, { "3_2_3",  15000 }, { "3_2_4",  19500 },
            { "3_3_1",  15000 }, { "3_3_2",  18000 }, { "3_3_3",  21000 }, { "3_3_4",  25000 },
            { "4_1_1",  8000  }, { "4_1_2",  12000 }, { "4_1_3",  16000 },
            { "4_2_1",  12000 }, { "4_2_2",  16000 }, { "4_2_3",  20000 },
            { "4_3_1",  16000 }, { "4_3_2",  20000 }, { "4_3_3",  26000 },
            { "5_1_1",  9000  }, { "5_1_2",  10000 }, { "5_1_3",  12000 }, { "5_1_4",  15000 },
            { "5_2_1",  15000 }, { "5_2_2",  17000 }, { "5_2_3",  19000 }, { "5_2_4",  22000 },
            { "5_3_1",  20000 }, { "5_3_2",  23000 }, { "5_3_3",  25000 }, { "5_3_4",  30000 },
            { "6_1_1",  2000  }, { "6_1_2",  11000 }, { "6_1_3",  13000 },
            { "6_2_1",  2000  }, { "6_2_2",  15000 }, { "6_2_3",  17000 },
            { "6_3_1",  2000  }, { "6_3_2",  19000 }, { "6_3_3",  21000 },
            { "12_0_1", 12000 }, { "12_0_2", 15000 }, { "12_0_3", 17000 }, { "12_0_4", 20000 },
            { "12_1_1", 16000 }, { "12_1_2", 18000 }, { "12_1_3", 22000 }, { "12_1_4", 25000 },
            { "12_2_1", 20000 }, { "12_2_2", 24000 }, { "12_2_3", 28000 }, { "12_2_4", 32000 },
            { "12_3_1", 30000 }, { "12_3_2", 35000 }, { "12_3_3", 40000 }, { "12_3_4", 45000 },
            { "13_0_1", 30000 }, { "13_0_2", 35000 }, { "13_0_3", 40000 }, { "13_0_4", 45000 },
            { "13_1_1", 40000 }, { "13_1_2", 45000 }, { "13_1_3", 50000 }, { "13_1_4", 55000 },
            { "13_2_1", 50000 }, { "13_2_2", 55000 }, { "13_2_3", 60000 }, { "13_2_4", 65000 },
            { "13_3_1", 60000 }, { "13_3_2", 65000 }, { "13_3_3", 70000 }, { "13_3_4", 75000 },
        };

        // -----------------------------------------------------------------------
        // KOMUT İŞLEYİCİ
        // -----------------------------------------------------------------------

        public bool CommandHandler(GamePlayer host, GSPacketIn packet)
        {
            BaseRoom room = host.CurrentRoom;
            if (room == null) return true;

            // Oda zaten oyunda — iptal paketi gönder
            if (room.IsPlaying)
            {
                host.SendMessage("Zaman aşımı, oda oyunda!");
                room.SendCancelPickUp();
                return true;
            }

            // Yalnızca host başlatabilir
            if (room.Host != host) return true;

            if (room.AvgLevel == 0)
                room.UpdateAvgLevel();

            List<GamePlayer> players = room.GetPlayers();

            // 1. Zorunlu kontroller (zorla başlatmada da geçerli)
            if (!CheckMandatory(host, room, players))
                return true;

            // 2. Güç limiti kontrolü (zorla başlatmada atlanır)
            bool isForceStart = s_onayListesi.Remove(room.RoomId);
            if (!isForceStart && !CheckFightPower(host, room, players))
                return true;

            // 3. Kod doğrulama
            if (!host.isPassCheckCode() && room.AvgLevel > 1)
            {
                CancelStart(room);
                return true;
            }

            // 4. İzin ve bilet kontrolleri
            if (!CheckPermissionsAndTickets(host, room, players))
                return true;

            // 5. Pet açlığını düşür ve oyunu başlat
            foreach (GamePlayer p in players)
            {
                if (p != null && !p.IsViewer && !p.isPlayerWarrior())
                    p.PetBag.ReduceHunger();
            }

            RoomMgr.StartGame(room);
            return true;
        }

        // -----------------------------------------------------------------------
        // KONTROL METODları
        // -----------------------------------------------------------------------

        /// <summary>
        /// Silah ve yarışma hesabı kontrollerini yapar.
        /// Bu kontroller zorla başlatmada da uygulanır.
        /// </summary>
        private static bool CheckMandatory(GamePlayer host, BaseRoom room, List<GamePlayer> players)
        {
            foreach (GamePlayer p in players)
            {
                if (p.MainWeapon == null)
                {
                    host.SendMessage(eMessageType.SYS_NOTICE,
                        "Herhangi bir üye veya seyircinin silahı yoksa başlanamaz!");
                    CancelStart(room);
                    return false;
                }

                if (p.isPlayerWarrior() && room.RoomType != eRoomType.Freedom)
                {
                    host.SendMessage(eMessageType.SYS_NOTICE,
                        "Yarışma hesabı olan bir üye var, başlanamıyor.");
                    CancelStart(room);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Savaşma gücü limitini kontrol eder.
        /// Yetersiz oyuncu varsa host'a uyarı verir ve onay listesine ekler.
        /// </summary>
        private static bool CheckFightPower(GamePlayer host, BaseRoom room, List<GamePlayer> players)
        {
            string anahtar = string.Format("{0}_{1}_{2}",
                room.MapId, (int)room.HardLevel, room.currentFloor);

            if (!s_gucLimitleri.TryGetValue(anahtar, out int gerekliGuc))
                return true; // Bu harita için limit tanımlı değil, geç

            foreach (GamePlayer p in players)
            {
                if (p.FightPower < gerekliGuc)
                {
                    host.SendMessage(eMessageType.ALERT, string.Format(
                        "Gereken Güç: {0}. Oyuncu: {1} ({2}). Devam etmek için tekrar 'Başla' butonuna basın.",
                        gerekliGuc, p.PlayerCharacter.NickName, p.FightPower));

                    room.SendMessage(eMessageType.SYS_NOTICE, string.Format(
                        "Gereken Güç: {0}. {1} yetersiz ({2}). Karar oda sahibi: {3}.",
                        gerekliGuc, p.PlayerCharacter.NickName, p.FightPower,
                        room.Host.PlayerCharacter.NickName));

                    s_onayListesi.Add(room.RoomId);
                    CancelStart(room);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Oda tipine göre izin ve bilet kontrollerini yapar.
        /// </summary>
        private static bool CheckPermissionsAndTickets(GamePlayer host, BaseRoom room, List<GamePlayer> players)
        {
            // FightLab izin kontrolü
            if (room.RoomType == eRoomType.FightLab &&
                !host.IsFightLabPermission(room.MapId, room.HardLevel))
            {
                host.SendMessage("Hata, katılamıyor.");
                CancelStart(room);
                return false;
            }

            // Dungeon izin kontrolü
            if (room.RoomType == eRoomType.Dungeon &&
                !host.IsPvePermission(room.MapId, room.HardLevel))
            {
                host.SendMessage(LanguageMgr.GetTranslation("GameStart.Msg1"));
                CancelStart(room);
                return false;
            }

            // Arena bileti (MapId 13)
            if (room.MapId == 13)
            {
                int ticketId = room.GetDungeonTicketId(room.HardLevel);
                if (host.GetItemByTemplateID(ticketId) == null)
                {
                    BroadcastMessage(players,
                        "Oda sahibinde arena bileti bulunması gerekli. Marketten bilet alabilirsin.");
                    return false;
                }
                host.RemoveTemplate(ticketId, 1);
            }

            // Dünya Kupası bileti (MapId 14)
            if (room.MapId == 14)
            {
                int ticketId = room.GetCupTicketId(room.HardLevel);
                if (host.GetItemByTemplateID(ticketId) == null)
                {
                    BroadcastMessage(players,
                        "Oda sahibinde dünya kupası bileti bulunması gerekli. Marketten bilet alabilirsin.");
                    return false;
                }
                host.RemoveTemplate(ticketId, 1);
            }

            // Harika Zindan bileti
            if (s_harikaZindanMapIds.Contains(room.MapId))
            {
                int ticketId = room.GetWonderDungeonTicketId(room.HardLevel);
                if (host.GetItemByTemplateID(ticketId) == null)
                {
                    BroadcastMessage(players,
                        "Oda sahibinde süper bilet bulunması gerekli. Marketten bilet alabilirsin.");
                    return false;
                }
                host.RemoveTemplate(ticketId, 1);
            }

            return true;
        }

        // -----------------------------------------------------------------------
        // YARDIMCI METODlar
        // -----------------------------------------------------------------------

        /// <summary>Odanın IsPlaying durumunu sıfırlar ve iptal paketini gönderir.</summary>
        private static void CancelStart(BaseRoom room)
        {
            room.IsPlaying = false;
            room.SendCancelPickUp();
        }

        /// <summary>Odadaki tüm oyunculara aynı mesajı gönderir.</summary>
        private static void BroadcastMessage(List<GamePlayer> players, string message)
        {
            foreach (GamePlayer p in players)
                p?.SendMessage(message);
        }
    }
}
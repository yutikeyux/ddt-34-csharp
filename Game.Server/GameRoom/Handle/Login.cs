using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Packets;
using Game.Server.Rooms;

namespace Game.Server.GameRoom.Handle
{
    /// <summary>
    /// Oyuncunun bir oyun odasına giriş isteğini işler.
    /// Paket tipi: GAME_ROOM_LOGIN
    /// </summary>
    [GameRoomHandleAttbute((byte)GameRoomPackageType.GAME_ROOM_LOGIN)]
    public class Login : IGameRoomCommandHadler
    {
        public bool CommandHandler(GamePlayer player, GSPacketIn packet)
        {
            // --- Paketten veri oku ---
            bool isInvite = packet.ReadBoolean();
            int hallType = packet.ReadInt();
            int num = packet.ReadInt();

            int roomId = -1;
            string pwd = string.Empty;

            // num == -1 ise belirli bir odaya giriş isteniyor; değilse rastgele eşleştirme
            if (num == -1)
            {
                roomId = packet.ReadInt();
                pwd = packet.ReadString();
            }

            // -----------------------------------------------------------------------
            // 1. TEMEL DOĞRULAMALAR
            // -----------------------------------------------------------------------

            // Oyuncunun oturumu aktif mi?
            if (!player.IsActive)
            {
                player.Out.SendRoomLoginResult(false);
                return false;
            }

            // Ana silah takılı mı?
            if (player.MainWeapon == null)
            {
                player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE,
                    LanguageMgr.GetTranslation("EnterRoomAction.NoWeapon"));
                player.Out.SendRoomLoginResult(false);
                return false;
            }

            // -----------------------------------------------------------------------
            // 2. MEVCUT ODADAN ÇIKIŞ
            // Oyuncu zaten bir odadaysa güvenli şekilde çıkar.
            // -----------------------------------------------------------------------
            player.CurrentRoom?.RemovePlayerUnsafe(player);

            // -----------------------------------------------------------------------
            // 3. HEDEF ODAYI BELİRLE
            // -----------------------------------------------------------------------
            BaseRoom[] rooms = RoomMgr.Rooms;
            BaseRoom targetRoom;

            if (roomId == -1)
            {
                // Rastgele uygun oda ara
                targetRoom = FindRandomRoom(rooms, hallType, player);
                if (targetRoom == null)
                {
                    player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE,
                        LanguageMgr.GetTranslation("EnterRoomAction.noroom"));
                    player.Out.SendRoomLoginResult(false);
                    return true;
                }
            }
            else
            {
                // Belirli oda ID doğrulaması (1-tabanlı indeks)
                if (roomId <= 0 || roomId > rooms.Length)
                {
                    player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE,
                        LanguageMgr.GetTranslation("EnterRoomAction.noexist"));
                    player.Out.SendRoomLoginResult(false);
                    return true;
                }

                targetRoom = rooms[roomId - 1];
            }

            // -----------------------------------------------------------------------
            // 4. ODA DURUM KONTROLLERI
            // -----------------------------------------------------------------------

            // Oda aktif kullanımda mı?
            if (!targetRoom.IsUsing)
            {
                player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE,
                    LanguageMgr.GetTranslation("EnterRoomAction.noexist"));
                player.Out.SendRoomLoginResult(false);
                return true;
            }

            // Oyun devam ediyor mu?
            if (targetRoom.IsPlaying)
            {
                bool canJoinMidGame = false;

                // Sadece PVE oyunlarında, davet ile ve hazırlık aşamasındaysa orta oyun girişi mümkün
                if (targetRoom.Game is PVEGame pveGame)
                {
                    canJoinMidGame = isInvite && pveGame.GameState == eGameState.SessionPrepared;
                }

                if (!canJoinMidGame)
                {
                    player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE,
                        LanguageMgr.GetTranslation("EnterRoomAction.start"));
                    player.Out.SendRoomLoginResult(false);
                    return true;
                }
            }

            // -----------------------------------------------------------------------
            // 5. KAPASİTE KONTROLÜ
            // -----------------------------------------------------------------------
            bool isFull = targetRoom.PlayerCount >= targetRoom.PlacesCount;

            if (isFull)
            {
                // Oda dolu — seyirci olarak eklenebilir mi?
                if (targetRoom.CanAddViewPlayer())
                {
                    return JoinAsViewer(player, targetRoom);
                }

                player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE,
                    LanguageMgr.GetTranslation("EnterRoomAction.full"));
                player.Out.SendRoomLoginResult(false);
                return true;
            }

            // -----------------------------------------------------------------------
            // 6. ŞİFRE KONTROLÜ
            // -----------------------------------------------------------------------
            if (targetRoom.NeedPassword && targetRoom.Password != pwd)
            {
                string msgKey = string.IsNullOrEmpty(pwd)
                    ? "EnterRoomAction.EnterPassword"
                    : "EnterRoomAction.passworderror";

                player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE,
                    LanguageMgr.GetTranslation(msgKey));
                player.Out.SendRoomLoginResult(false);
                return true;
            }

            // -----------------------------------------------------------------------
            // 7. OYUN KATILIM KAPASİTESİ KONTROLÜ
            // -----------------------------------------------------------------------
            if (targetRoom.Game != null && !targetRoom.Game.CanAddPlayer())
            {
                // Oyun katılıma kapalı (örn. maksimum oyuncu sayısına ulaşıldı)
                player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE,
                    LanguageMgr.GetTranslation("EnterRoomAction.full"));
                player.Out.SendRoomLoginResult(false);
                return true;
            }

            // -----------------------------------------------------------------------
            // 8. SEVİYE LİMİTİ KONTROLÜ (yalnızca Dungeon odaları için)
            // -----------------------------------------------------------------------
            if (targetRoom.RoomType == eRoomType.Dungeon)
            {
                eLevelLimits roomLimit = (eLevelLimits)targetRoom.LevelLimits;
                eLevelLimits playerLimit = targetRoom.GetLevelLimit(player);

                if (roomLimit > playerLimit)
                {
                    player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE,
                        LanguageMgr.GetTranslation("EnterRoomAction.level"));
                    player.Out.SendRoomLoginResult(false);
                    return true;
                }
            }

            // -----------------------------------------------------------------------
            // 9. ODAYA KATIL (normal oyuncu)
            // -----------------------------------------------------------------------
            return JoinAsPlayer(player, targetRoom);
        }

        // -----------------------------------------------------------------------
        // YARDIMCI: Normal oyuncu olarak odaya katıl
        // -----------------------------------------------------------------------
        private static bool JoinAsPlayer(GamePlayer player, BaseRoom room)
        {
            RoomMgr.WaitingRoom.RemovePlayer(player);
            player.Out.SendRoomLoginResult(true);
            player.Out.SendRoomCreate(room);

            if (room.AddPlayerUnsafe(player) && room.Game != null)
                room.Game.AddPlayer((IGamePlayer)player);

            // Bekleme odası listesini güncelle ve oda ayarlarını gönder
            RoomMgr.WaitingRoom.SendUpdateRoom(player);
            player.Out.SendGameRoomSetupChange(room);
            return true;
        }

        // -----------------------------------------------------------------------
        // YARDIMCI: Seyirci olarak dolu odaya katıl
        // -----------------------------------------------------------------------
        private static bool JoinAsViewer(GamePlayer player, BaseRoom room)
        {
            RoomMgr.WaitingRoom.RemovePlayer(player);
            player.Out.SendRoomLoginResult(true);
            player.Out.SendRoomCreate(room);

            if (room.AddPlayerUnsafe(player))
            {
                room.Game?.AddPlayer(player);

                // Bekleme odası oyuncu listesini güncelle
                RoomMgr.WaitingRoom.SendUpdateCurrentRoom(room);
                player.Out.SendGameRoomSetupChange(room);

                // Oyuncu durumunu seyirci (1) olarak işaretle, hazır değil (false)
                room.UpdatePlayerState(player, 1, false);
            }

            return true;
        }

        // -----------------------------------------------------------------------
        // YARDIMCI: Uygun rastgele oda bul
        //
        // Kurallar:
        //   - Odada en az 1 oyuncu olmalı
        //   - Oda yeni oyuncu kabul etmeli (CanAddPlayer)
        //   - Şifre gerektirmemeli
        //   - Oyun başlamamış olmalı
        //   - Yeni Başlayan (Freshman) odası olmamalı
        //   - Oda tipi eşleşmeli
        //   - Tip 10 (Dungeon eşleştirme) ise oyuncunun seviye limiti oda limitini karşılamalı
        // -----------------------------------------------------------------------
        private static BaseRoom FindRandomRoom(BaseRoom[] rooms, int hallType, GamePlayer player)
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                BaseRoom room = rooms[i];

                // Temel filtreler
                if (room.PlayerCount == 0) continue;
                if (!room.CanAddPlayer()) continue;
                if (room.NeedPassword) continue;
                if (room.IsPlaying) continue;
                if (room.RoomType == eRoomType.Freshman) continue;

                // Tip 10: Dungeon eşleştirmesi — oyuncunun seviyesi yeterli olmalı
                if (hallType == 10)
                {
                    if (room.RoomType != (eRoomType)hallType) continue;

                    eLevelLimits roomLimit = (eLevelLimits)room.LevelLimits;
                    eLevelLimits playerLimit = room.GetLevelLimit(player);

                    // Oda limiti oyuncu limitinden KÜÇÜK olmalı (oyuncu yeterince güçlü)
                    if (roomLimit >= playerLimit) continue;

                    return room;
                }

                // Standart tip eşleştirmesi
                if (room.RoomType == (eRoomType)hallType)
                    return room;
            }

            return null;
        }
    }
}
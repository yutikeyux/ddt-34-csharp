using Bussiness;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Packets;

namespace Game.Server.Rooms
{
    /// <summary>
    /// Oyuncunun mevcut bir odaya katılma isteğini işler.
    /// roomId == -1 ise uygun rastgele oda aranır, aksi halde belirli odaya giriş denenir.
    /// </summary>
    internal class EnterRoomAction : IAction
    {
        // -----------------------------------------------------------------------
        // ALANLAR
        // -----------------------------------------------------------------------

        private readonly GamePlayer m_player;
        private readonly int m_roomId;
        private readonly string m_pwd;
        private readonly int m_hallType;
        private readonly bool m_isInvite;

        // -----------------------------------------------------------------------
        // YAPICI
        // -----------------------------------------------------------------------

        public EnterRoomAction(
            GamePlayer player,
            int roomId,
            string pwd,
            int hallType,
            bool isInvite)
        {
            m_player = player;
            m_roomId = roomId;
            m_pwd = pwd;
            m_hallType = hallType;
            m_isInvite = isInvite;
        }

        // -----------------------------------------------------------------------
        // ÇALIŞMA MANTIĞI
        // -----------------------------------------------------------------------

        public void Execute()
        {
            // 1. Temel doğrulamalar
            if (!m_player.IsActive) return;

            if (m_player.MainWeapon == null)
            {
                SendFail(LanguageMgr.GetTranslation("EnterRoomAction.NoWeapon"));
                return;
            }

            // 2. Mevcut odadan çıkış
            m_player.CurrentRoom?.RemovePlayerUnsafe(m_player);

            // 3. Hedef odayı belirle
            BaseRoom[] rooms = RoomMgr.Rooms;
            BaseRoom target;

            if (m_roomId == -1)
            {
                target = FindRandomRoom(rooms);
                if (target == null)
                {
                    SendFail(LanguageMgr.GetTranslation("EnterRoomAction.noroom"));
                    return;
                }
            }
            else
            {
                if (m_roomId <= 0 || m_roomId > rooms.Length)
                {
                    SendFail(LanguageMgr.GetTranslation("EnterRoomAction.noexist"));
                    return;
                }
                target = rooms[m_roomId - 1];
            }

            // 4. Oda aktif mi?
            if (!target.IsUsing)
            {
                SendFail(LanguageMgr.GetTranslation("EnterRoomAction.noexist"));
                return;
            }

            // 5. Oyun devam ediyor mu?
            if (target.IsPlaying)
            {
                bool canJoinMidGame = m_isInvite
                    && target.Game is PVEGame pve
                    && pve.GameState == eGameState.SessionPrepared;

                if (!canJoinMidGame)
                {
                    SendFail(LanguageMgr.GetTranslation("EnterRoomAction.start"));
                    return;
                }
            }

            // 6. Kapasite kontrolü
            bool isFull = target.PlayerCount >= target.PlacesCount;

            if (isFull)
            {
                if (target.CanAddViewPlayer())
                {
                    JoinAsViewer(target);
                }
                else
                {
                    SendFail(LanguageMgr.GetTranslation("EnterRoomAction.full"));
                }
                return;
            }

            // 7. Şifre kontrolü
            if (target.NeedPassword && target.Password != m_pwd)
            {
                string msgKey = string.IsNullOrEmpty(m_pwd)
                    ? "EnterRoomAction.EnterPassword"
                    : "EnterRoomAction.passworderror";

                SendFail(LanguageMgr.GetTranslation(msgKey));
                return;
            }

            // 8. Oyun katılım kapasitesi
            if (target.Game != null && !target.Game.CanAddPlayer())
            {
                SendFail(LanguageMgr.GetTranslation("EnterRoomAction.full"));
                return;
            }

            // 9. Seviye limiti (yalnızca Dungeon)
            if (target.RoomType == eRoomType.Dungeon &&
                (eLevelLimits)target.LevelLimits > target.GetLevelLimit(m_player))
            {
                SendFail(LanguageMgr.GetTranslation("EnterRoomAction.level"));
                return;
            }

            // 10. Normal oyuncu olarak katıl
            JoinAsPlayer(target);
        }

        // -----------------------------------------------------------------------
        // YARDIMCI METODlar
        // -----------------------------------------------------------------------

        /// <summary>Normal oyuncu olarak odaya katılır ve ilgili paketleri gönderir.</summary>
        private void JoinAsPlayer(BaseRoom room)
        {
            RoomMgr.WaitingRoom.RemovePlayer(m_player);
            m_player.Out.SendRoomLoginResult(true);
            m_player.Out.SendRoomCreate(room);

            if (room.AddPlayerUnsafe(m_player) && room.Game != null)
                room.Game.AddPlayer((IGamePlayer)m_player);

            RoomMgr.WaitingRoom.SendUpdateRoom(room);
            m_player.Out.SendGameRoomSetupChange(room);
        }

        /// <summary>Dolu odaya seyirci olarak katılır ve ilgili paketleri gönderir.</summary>
        private void JoinAsViewer(BaseRoom room)
        {
            RoomMgr.WaitingRoom.RemovePlayer(m_player);
            m_player.Out.SendRoomLoginResult(true);
            m_player.Out.SendRoomCreate(room);

            if (room.AddPlayerUnsafe(m_player))
            {
                room.Game?.AddPlayer(m_player);
                RoomMgr.WaitingRoom.SendUpdateCurrentRoom(room);
                m_player.Out.SendGameRoomSetupChange(room);

                // Seyirci durumu: 1 = izleyici, hazır değil
                room.UpdatePlayerState(m_player, 1, sendToClient: false);
            }
        }

        /// <summary>Oyuncuya hata mesajı gönderir ve giriş reddini bildirir.</summary>
        private void SendFail(string message)
        {
            m_player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, message);
            m_player.Out.SendRoomLoginResult(false);
        }

        /// <summary>
        /// Uygun rastgele oda arar.
        /// Kurallar:
        ///   - En az 1 oyuncu olmalı
        ///   - Yeni oyuncu kabul etmeli
        ///   - Şifresiz ve oyun başlamamış olmalı
        ///   - Freshman odası olmamalı
        ///   - Tip 10 (Dungeon eşleştirme): oyuncunun seviyesi oda limitini karşılamalı
        ///   - Diğer tipler: oda tipi hallType ile eşleşmeli
        /// </summary>
        private BaseRoom FindRandomRoom(BaseRoom[] rooms)
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                BaseRoom room = rooms[i];

                if (room.PlayerCount == 0) continue;
                if (!room.CanAddPlayer()) continue;
                if (room.NeedPassword) continue;
                if (room.IsPlaying) continue;
                if (room.RoomType == eRoomType.Freshman) continue;

                if (m_hallType == 10)
                {
                    if (room.RoomType != (eRoomType)m_hallType) continue;
                    if ((eLevelLimits)room.LevelLimits >= room.GetLevelLimit(m_player)) continue;
                    return room;
                }

                if (room.RoomType == (eRoomType)m_hallType)
                    return room;
            }

            return null;
        }
    }
}
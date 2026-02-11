using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Battle;
using Game.Server.Games;
using Game.Server.Packets;
using Game.Server.RingStation;
using System.Collections.Generic;

namespace Game.Server.Rooms
{
    public class StartGameAction : IAction
    {
        private BaseRoom m_room;

        public StartGameAction(BaseRoom room)
        {
            m_room = room;
        }

        public void Execute()
        {
            if (!m_room.CanStart())
            {
                return;
            }
            m_room.StartWithNpc = false;
            m_room.PickUpNpcId = -1;//
            List<GamePlayer> players = m_room.GetPlayers();

            if (m_room.RoomType == eRoomType.Freedom)
            {
                List<IGamePlayer> list = new List<IGamePlayer>();
                List<IGamePlayer> list2 = new List<IGamePlayer>();
                foreach (GamePlayer item in players)
                {
                    if (item != null)
                    {
                        if (item.CurrentRoomTeam == 1)
                        {
                            list.Add(item);
                        }
                        else
                        {
                            list2.Add(item);
                        }
                    }
                }
                BaseGame game = GameMgr.StartPVPGame(m_room.RoomId, list, list2, m_room.MapId, m_room.RoomType, m_room.GameType, m_room.TimeMode);
                StartGame(game);
            }
            else if (IsPVE(m_room.RoomType) || m_room.RoomType == eRoomType.Academy || m_room.RoomType == eRoomType.Dungeon_Night)
            {
                if (GameMgr.SynDate < 0)
                {
                    foreach (GamePlayer item2 in players)
                    {
                        item2?.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("StartGameAction.WaitingPveRestart"));
                    }
                    return;
                }
                List<IGamePlayer> list3 = new List<IGamePlayer>();
                foreach (GamePlayer item3 in players)
                {
                    if (item3 != null)
                    {
                        list3.Add(item3);
                    }
                }
                UpdatePveRoomTimeMode();
                BaseGame game2 = GameMgr.StartPVEGame(m_room.RoomId, list3, m_room.MapId, m_room.RoomType, m_room.GameType, m_room.TimeMode, m_room.HardLevel, m_room.LevelLimits, m_room.currentFloor);
                StartGame(game2);
            }
            else if (IsPVP(m_room.RoomType))
            {
                m_room.UpdateAvgLevel();
                if (!m_room.isCrosszone && m_room.RoomType == eRoomType.Match && m_room.Host != null)
                {
                    m_room.PickUpNpcId = RingStationConfiguration.NextRoomId();
                }
                BattleServer battleServer = BattleMgr.AddRoom(m_room);
                if (battleServer != null)
                {
                    m_room.BattleServer = battleServer;
                    m_room.IsPlaying = true;
                    m_room.SendStartPickUp();
                }
                else
                {
                    GSPacketIn pkg = m_room.Host.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("StartGameAction.noBattleServe"));
                    m_room.SendToAll(pkg, m_room.Host);
                    m_room.SendCancelPickUp();
                }
            }
            RoomMgr.WaitingRoom.SendUpdateCurrentRoom(m_room);
        }

        private void StartGame(BaseGame game)
        {
            if (game != null)
            {
                m_room.IsPlaying = true;
                m_room.StartGame(game);
                return;
            }
            m_room.IsPlaying = false;
            m_room.SendPlayerState();
            GSPacketIn msg = m_room.Host.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation(m_room.RoomType.ToString()));
            if (m_room.RoomType == eRoomType.Freedom)
            {
                GSPacketIn pkg = m_room.Host.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("StartGameAction.noBattleServe"));
                m_room.SendToAll(pkg, m_room.Host);
            }
            else
            {
                GSPacketIn pkg2 = m_room.Host.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("Odada bir sorun meydana geldi！ Oda değiştir!"));
                m_room.SendToAll(pkg2, m_room.Host);
            }
            m_room.SendCancelPickUp();
        }

        private bool IsPVP(eRoomType roomType)
        {
            if (roomType == eRoomType.Match)
            {
                return true;
            }
            return false;
        }

        private bool IsPVE(eRoomType roomType)
        {
            switch (roomType)
            {
                case eRoomType.Dungeon:
                case eRoomType.Freshman:
                case eRoomType.Labyrinth:
                case eRoomType.ConsortiaBoss:
                case eRoomType.AcademyDungeon:
                case eRoomType.FightLab:
                case eRoomType.WordBossFight:
                case eRoomType.Christmas:
                case eRoomType.ActivityDungeon:
                case eRoomType.SpecialActivityDungeon:
                    return true;
            }
            return false;
        }

        private void UpdatePveRoomTimeMode()
        {
            switch (m_room.HardLevel)
            {
                case eHardLevel.Simple:
                    m_room.TimeMode = 3;
                    break;
                case eHardLevel.Normal:
                    m_room.TimeMode = 2;
                    break;
                case eHardLevel.Hard:
                case eHardLevel.Terror:
                    m_room.TimeMode = 1;
                    break;
                case eHardLevel.Epic:
                    break;
            }
        }
    }
}

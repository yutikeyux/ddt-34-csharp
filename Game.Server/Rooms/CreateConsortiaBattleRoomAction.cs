using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.GameObjects;
using Game.Base.Packets;
using Game.Server.Packets;
using Game.Server.Battle;
using Bussiness;
using Game.Logic;
using Game.Server.Games;

namespace Game.Server.Rooms
{
    public class CreateConsortiaBattleRoomAction : IAction
    {
        private GamePlayer m_player;

        private string m_name;

        private string m_password;

        private eRoomType m_roomType;

        private byte m_timeType;

        public CreateConsortiaBattleRoomAction(GamePlayer player)
        {
            m_player = player;
            m_name = "Consortia Battle";
            m_password = "12dasSda44";
            m_roomType = eRoomType.ConsortiaBattle;
            m_timeType = 2;
        }

        public void Execute()
        {
            if (m_player.CurrentRoom != null)
            {
                m_player.CurrentRoom.RemovePlayerUnsafe(m_player);
            }

            if (m_player.IsActive == false)
                return;

            BaseRoom[] rooms = RoomMgr.Rooms;
            BaseRoom m_room = null;
            for (int i = 0; i < rooms.Length; i++)
            {
                if (!rooms[i].IsUsing)
                {
                    m_room = rooms[i];
                    break;
                }
            }

            if (m_room != null && GameMgr.GuildBattle.CanEnterGame(m_player.PlayerCharacter.ConsortiaID))
            {
                RoomMgr.WaitingRoom.RemovePlayer(m_player);
                m_room.Start();
                m_room.UpdateRoom(m_name, m_password, m_roomType, m_timeType, 0);
                m_room.AreaID = m_player.ZoneId;
                m_room.isCrosszone = false;
                GSPacketIn pkg = m_player.Out.SendSingleRoomCreate(m_room);
                m_room.AddPlayerUnsafe(m_player);
            }
            else
            {
                m_player.SendMessage(LanguageMgr.GetTranslation("GameServer.GuildBattle.EnterGame.Error"));
            }
        }


    }
}

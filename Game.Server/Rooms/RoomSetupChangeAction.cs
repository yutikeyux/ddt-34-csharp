using Game.Logic;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Rooms
{
    internal class RoomSetupChangeAction : IAction
    {
        private BaseRoom m_room;

        private eRoomType m_roomType;

        private byte m_timeMode;

        private eHardLevel m_hardLevel;

        private int m_mapId;

        private int m_levelLimits;

        private string m_password;

        private string m_roomName;

        private bool m_isCrosszone;

        private string m_pic;

        private int m_currentFloor;

        private bool m_isOpenBoss;

        public RoomSetupChangeAction(BaseRoom room, eRoomType roomType, byte timeMode, eHardLevel hardLevel, int levelLimits, int mapId, string password, string roomname, bool isCrosszone, bool isOpenBoss, string Pic, int currentFloor)
        {
            m_room = room;
            m_roomType = roomType;
            m_timeMode = timeMode;
            m_hardLevel = hardLevel;
            m_levelLimits = levelLimits;
            m_mapId = mapId;
            m_password = password;
            m_roomName = roomname;
            m_isCrosszone = isCrosszone;
            m_isOpenBoss = isOpenBoss;
            m_pic = Pic;
            m_currentFloor = currentFloor;

            if (isOpenBoss)
            {

                if (mapId == 10000)
                {
                    m_currentFloor = 1;
                    m_pic = "show1.jpg";
                }
                else if (mapId == 7)
                {

                    if (m_hardLevel == eHardLevel.Easy)
                    {
                        m_currentFloor = 2;
                        m_pic = "show5.jpg";
                    }
                    else if (m_hardLevel == eHardLevel.Normal)
                    {
                        m_currentFloor = 3;
                        m_pic = "show4.jpg";
                    }
                    else if (m_hardLevel == eHardLevel.Hard)
                    {
                        m_currentFloor = 4;
                        m_pic = "show9.jpg";
                    }
                    else
                    {
                        m_currentFloor = 1;
                        m_pic = "show1.jpg";
                    }
                }
                else if (mapId == 1)
                {
                    if (m_hardLevel == eHardLevel.Terror)
                    {
                        m_currentFloor = 5;
                        m_pic = "show7.jpg";
                    }
                    else if (m_hardLevel == eHardLevel.Hard)
                    {
                        m_currentFloor = 4;
                        m_pic = "show6.jpg";
                    }
                    else if (m_hardLevel == eHardLevel.Normal)
                    {
                        m_currentFloor = 3;
                        m_pic = "show4.jpg";
                    }
                    else if (m_hardLevel == eHardLevel.Easy)
                    {
                        m_currentFloor = 2;
                        m_pic = "show2.jpg";
                    }
                    else
                    {
                        m_currentFloor = 1;
                        m_pic = "show1.jpg";
                    }

                }
                else
                {
                    List<int> lastFloorData = GetLastFloor(mapId, (int)m_hardLevel);

                    if (lastFloorData != null && lastFloorData.Count >= 2)
                    {
                        m_currentFloor = lastFloorData[0];
                        m_pic = "show" + lastFloorData[1] + ".jpg";
                    }
                    else
                    {
                        Console.WriteLine("HATA: Boss odasý için geçerli 'LastFloor' verisi bulunamadý. MapId: " + mapId + ", HardLevel: " + m_hardLevel);
                        m_currentFloor = 1;
                        m_pic = "show1.jpg";
                    }
                }
            }
        }

        

        public List<int> GetLastFloor(int mapID, int hardLevel)
        {
            List<int> floor = new List<int>();
            PveInfo pve = PveInfoMgr.GetPveInfoById(mapID);
            if (pve != null && !string.IsNullOrEmpty(pve.LastFloor))
            {
                string[] splitLastFloor = pve.LastFloor.Split(',');

                if (hardLevel >= 0 && hardLevel < splitLastFloor.Length)
                {
                    int result;
                    if (int.TryParse(splitLastFloor[hardLevel], out result))
                    {
                        floor.Add(result);
                        floor.Add(result);
                    }
                }
                else
                {
                    Console.WriteLine("HATA: PveInfo LastFloor verisi hardLevel için yetersiz. MapId: " + mapID + ", HardLevel: " + hardLevel);
                }
            }
            return floor;
        }

        public void Execute()
        {
            m_room.RoomType = m_roomType;
            m_room.TimeMode = m_timeMode;
            m_room.HardLevel = m_hardLevel;
            m_room.LevelLimits = m_levelLimits;
            m_room.MapId = m_mapId;
            m_room.Name = m_roomName;
            m_room.Password = m_password;
            m_room.isCrosszone = m_isCrosszone;
            m_room.isOpenBoss = m_isOpenBoss;
            m_room.currentFloor = m_currentFloor;
            m_room.Pic = m_pic;
            m_room.UpdateGameStyle();
            m_room.UpdateRoomGameType();
            m_room.SendRoomSetupChange(m_room);
            RoomMgr.WaitingRoom.SendUpdateRoom(m_room);
        }
    }
}
using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Games;
using Game.Server.GuildBattle;
using Game.Server.Managers;
using Game.Server.Rooms;
using System;
using System.Drawing;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.CONSORTIA_BATTLE, "场景用户离开")]
    public class ConsortiaBattleHander : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            byte cmd = packet.ReadByte();
            //BaseConsBatRoom room = RoomMgr.ConsBatRoom;
            //int PlayerID = client.Player.PlayerCharacter.ID;
            UserGuildBattleInfo uinfo = GameMgr.GuildBattle.FindUser(client.Player.PlayerId);

            BaseRoom currentRoom = client.Player.CurrentRoom;

            if (currentRoom == null || uinfo == null || currentRoom.RoomType != eRoomType.ConsortiaBattle)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.GuildBattle.Error"));
                return 0;
            }

            //Console.WriteLine("//// CorBattle: " + (ConsBatPackageType)cmd);
            switch (cmd)
            {
                case (byte)ConsBatPackageType.DELETE_PLAYER:
                    {
                        _ = currentRoom.RemovePlayerUnsafe(client.Player);
                        break;
                    }
                case (byte)ConsBatPackageType.ADD_PLAYER:
                    {
                        GameMgr.GuildBattle.SendAllPlayerList(uinfo, false);
                        break;
                    }
                case (byte)ConsBatPackageType.PLAYER_MOVE:
                    {
                        int posX = packet.ReadInt();
                        int posY = packet.ReadInt();
                        string pointArr = packet.ReadString();
                        uinfo.Postion = new Point(posX, posY);
                        GameMgr.GuildBattle.SendPlayerMove(uinfo, pointArr);

                        break;
                    }
                case (byte)ConsBatPackageType.CONFIRM_ENTER:
                    {
                        GameMgr.GuildBattle.SendAllPlayerList(uinfo, true);
                        break;
                    }
                case (byte)ConsBatPackageType.CHALLENGE:
                    {
                        int UserID = packet.ReadInt();
                        if (client.Player.MainWeapon == null)
                        {
                            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip"));
                            return 0;
                        }
                        GamePlayer p = WorldMgr.GetPlayerById(UserID);

                        if (p != null && p.IsActive && p.CurrentRoom != null && p.CurrentRoom.IsPlaying == false)
                        {
                            GameMgr.GuildBattle.ChallengeGame(client.Player, p);
                        }
                        else
                        {
                            client.Player.SendMessage("Eşleştirme hatası.");
                        }
                        //room.Challenge(PlayerID, ChallengeID);
                        break;
                    }
                case (byte)ConsBatPackageType.UPDATE_SCORE:
                    {
                        byte type = packet.ReadByte();
                        if (uinfo != null)
                        {
                            GameMgr.GuildBattle.SendUpdateScore(uinfo, type);
                        }
                        break;
                    }
                case (byte)ConsBatPackageType.CONSUME:
                    {
                        int type = packet.ReadInt();
                        _ = packet.ReadBoolean();

                        switch (type)
                        {
                            case 1:
                                // power
                                if (client.Player.PlayerCharacter.Money >= 30000)
                                {
                                    _ = client.Player.RemoveMoney(30000);
                                    client.Player.PlayerCharacter.ActivePowFirstGame = true;
                                }
                                break;

                            case 2:
                                //douplescore
                                if (client.Player.PlayerCharacter.Money >= 300000)
                                {
                                    _ = client.Player.RemoveMoney(300000);
                                    uinfo.DupeScoreConsortiaBattle = true;
                                }
                                break;

                            case 3:
                                //quick revive
                                if (uinfo.IsDead && client.Player.PlayerCharacter.Money >= 10000)
                                {
                                    _ = client.Player.RemoveMoney(10000);
                                    GameMgr.GuildBattle.QuickRevive(uinfo, false);
                                }
                                break;

                            case 4:
                                //quick revive stay
                                if (uinfo.IsDead && client.Player.PlayerCharacter.Money >= 80000)
                                {
                                    _ = client.Player.RemoveMoney(80000);
                                    GameMgr.GuildBattle.QuickRevive(uinfo, true);
                                }
                                break;
                        }

                        GameMgr.GuildBattle.SendUpdateSceneInfo(uinfo);

                        break;
                    }
                default:
                    Console.WriteLine("ConsortiaBattleType." + (ConsBatPackageType)cmd);
                    break;
            }
            return 0;
        }
    }
}

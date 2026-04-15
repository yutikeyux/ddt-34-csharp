using Game.Base.Packets;
using Game.Logic;
using Game.Server.Rooms;
using SqlDataProvider.Data;

namespace Game.Server.GameRoom.Handle
{
    [GameRoomHandleAttbute(2)]
    public class SetupChange : IGameRoomCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            if (Player.CurrentRoom != null && Player == Player.CurrentRoom.Host && !Player.CurrentRoom.IsPlaying)
            {
                int mapId = packet.ReadInt();
                eRoomType roomType = (eRoomType)packet.ReadByte();
                bool isOpenBoss = packet.ReadBoolean();
                string pic = "";
                string password = packet.ReadString();
                string roomname = packet.ReadString();
                byte timeMode = packet.ReadByte();
                byte hardLevel = packet.ReadByte();
                int levelLimits = packet.ReadInt();
                bool isCrosszone = packet.ReadBoolean();
                int currentFloor = 1;

                if (mapId == 0 && roomType == eRoomType.Labyrinth)
                {
                    mapId = 401;
                    currentFloor = Player.Labyrinth.currentFloor;
                }

                if (roomType == eRoomType.Dungeon && mapId != 10000)
                {
                    PveInfo pve = PveInfoMgr.GetPveInfoById(mapId);
                    string user = Player.PlayerCharacter.UserName;

                    if (pve != null && isOpenBoss)
                    {
                        if (pve.ID >= 15 && user != "administrator")
                        {
                            isOpenBoss = false;
                            Player.SendMessage("Bu zindan bu özelliğe izin vermiyor");
                        }
                        else if (GameServer.Instance.Configuration.ZoneId == 1001 || GameServer.Instance.Configuration.ZoneId == 1002 || GameServer.Instance.Configuration.ZoneId == 1003)
                        {
                            int price = pve.GetPrice((int)hardLevel);
                            int discount = 0;
                            int vipLevel = Player.PlayerCharacter.VIPLevel;

                            switch (vipLevel)
                            {
                                case 2: discount = 5; break;
                                case 3: discount = 10; break;
                                case 4: discount = 25; break;
                                case 5: discount = 37; break;
                                case 6: discount = 48; break;
                                case 7: discount = 55; break;
                                case 8: discount = 69; break;
                                case 9: discount = 88; break;
                            }

                            if (discount > 0)
                            {
                                price = price * (100 - discount) / 100;
                            }

                            // GÜVENLİK GÜNCELLEMESİ: Ödeme kontrolü
                            if (Player.MoneyDirect(price, IsAntiMult: false, false, true))
                            {
                                // Ödeme Başarılı
                                if (discount > 0)
                                {
                                    Player.SendMessage(string.Format("VIP seviyeniz {0} olduğu için %{1} indirim aldınız. Başarıyla Boss odası oluşturuldu!", vipLevel, discount));
                                }
                                else
                                {
                                    Player.SendMessage("Başarıyla Boss odası oluşturuldu!");
                                }

                                // Odayı Boss modunda güncelle
                                RoomMgr.UpdateRoomGameType(Player.CurrentRoom, roomType, timeMode, (eHardLevel)hardLevel, levelLimits, mapId, password, roomname, isCrosszone, isOpenBoss, pic, currentFloor);
                            }
                            else
                            {
                                // Ödeme Başarısız (Limit Doldu veya Bakiye Yetersiz)
                                // MoneyDirect zaten oyuncuya "Limit doldu" veya "Para yetmedi" mesajı gönderdi.
                                // Boss modunu kapatıp normal odayı açıyoruz.
                                isOpenBoss = false;
                                RoomMgr.UpdateRoomGameType(Player.CurrentRoom, roomType, timeMode, (eHardLevel)hardLevel, levelLimits, mapId, password, roomname, isCrosszone, isOpenBoss, pic, currentFloor);
                            }
                            return true;
                        }
                        else
                        {
                            isOpenBoss = false;
                            Player.SendMessage("Bu özellik geçici olarak kapalıdır, sizden ücret alınmayacaktır");
                        }
                    }
                }
                // Genel oda güncelleme (Boss modu kapalıysa veya diğer odalarsa buraya düşer)
                RoomMgr.UpdateRoomGameType(Player.CurrentRoom, roomType, timeMode, (eHardLevel)hardLevel, levelLimits, mapId, password, roomname, isCrosszone, isOpenBoss, pic, currentFloor);
            }
            return true;
        }
    }
}
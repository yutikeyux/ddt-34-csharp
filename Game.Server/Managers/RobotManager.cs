using Game.Server.GameObjects;
using Game.Server.Rooms;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using Game.Server.RobotWaiting;
using Game.Logic;
using Bussiness.Managers;

namespace Game.Server.Managers
{
    public class RobotManager
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Bot ID'leri gerçek oyuncularla karışmaması için negatiften başlatıyoruz
        private static int CurRobotID = 100000;

        private static Dictionary<int, RobotGamePlayer> RobotGamePlayers = new Dictionary<int, RobotGamePlayer>();

        // Ayar: Kaç bot ve oda oluşturulacağını buradan belirleyebilirsiniz
        private const int MaxBotCount = 0;
        private const int MaxRoomCount = 0;

        // --- GİYDİRME İÇİN GEREKLİ ITEM ID LİSTELERİ ---
        // Bu ID'ler ItemInfo.xml veya veritabanınızdaki ItemTemplate tablosuna göredir.
        private static int[] m_headIds = { 1119, 1104, 1105, 1112, 1113, 1122, 1126, 1136, 1137, 1138, 1140, 1141 };
        private static int[] m_glassIds = { 2102, 2103, 2105, 2106, 2108, 2109, 2116, 2117, 2120, 2121, 2122, 2123 };
        private static int[] m_hairIds = { 3102, 3103, 3104, 3105, 3106, 3107, 3108, 3109, 3110, 3111, 3112, 3113, 3114, 3115, 3116 };
        private static int[] m_effIds = { 4101, 4102, 4103, 4104, 4105, 4106, 4107, 4108, 4109, 4110, 4111, 4112, 4113, 4114, 4115, 4116 };
        private static int[] m_clothIds = { 5117, 5102, 5103, 5104, 5105, 5106, 5107, 5108, 5109, 5110, 5111, 5112, 5113, 5114, 5115, 5116 };
        private static int[] m_faceIds = { 6101, 6102, 6103, 6104, 6105, 6106, 6107, 6108, 6109, 6110, 6111, 6112, 6113, 6114, 6115, 6116 };
        private static int[] m_weaponIds = { 7001, 7002, 7003, 7005, 7006, 7007, 7008, 7009, 7010, 7011, 7012, 7013, 7014 };
        private static int[] m_wingIds = { 15002, 15003, 15004, 15005, 15006, 15007, 15008, 15009 };

        public static bool Init()
        {
            try
            {
                log.Info("Robot Manager başlatılıyor...");
                CreateBotWaiting();
                CreateRoomWaiting();
                log.Info("Robot Manager başarıyla yüklendi.");
            }
            catch (Exception Err)
            {
                log.Error("Robot Init Hatası", Err);
                return false;
            }
            return true;
        }

        static void CreateBotWaiting()
        {
            #region Türkçe ve Eski Oyuncu İsimleri (Numarasız)
            // Liste genişletildi, tekrarı önlemek için HashSet kullanacağız
            HashSet<string> usedNames = new HashSet<string>();
            string[] names = {
                "Kral", "Efsane", "Şahin", "Aslan", "Kaplan", "Cengaver", "Korkusuz",
                "Shadow", "Dark", "Light", "Fire", "Ice", "Storm", "Thunder",
                "ProKing", "NoobSlayer", "Headshot", "Sniper", "Killer", "Hunter",
                "Kaan", "Berk", "Aras", "Deniz", "Burak", "Mert", "Kuzey",
                "Rüzgar", "Sis", "Gece", "Güneş", "Yıldız", "Ay", "Dünya",
                "Kaos", "Sessizlik", "Dehşet", "Azrail", "Melek", "Şeytan",
                "Master", "GrandMaster", "Rookie", "Veteran", "Legend",
                "Turk", "TürkBayrağı", "Bozkurt", "Alparslan", "Malkoçoğlu",
                "Delikanlı", "Gizemli", "YalnızKurt", "Savaşçı", "Barbar",
                "Kılıç", "Kalkan", "Okçu", "Şövalye", "Ninja", "Samurai",
                "Lejyoner", "Gladyatör", "Spartacus", "Zeus", "Poseidon", "Hades",
                "xKralx", "xDarkLordx", "ProGamer", "GameMaster", "ServerOwner",
                "Pepe", "Wojak", "Chad", "Virgin", "Based", "Cringe",
                "Error404", "System32", "BlueScreen", "Lag", "Ping",
                "Kobe", "Jordan", "LeBron", "Messi", "Ronaldo", "Ibrahimovic",
                "Furkan", "Emre", "Mert", "Çınar", "Kerem", "Umut", "Berkay",
                "Arda", "Deniz", "Göktürk", "Alperen", "Batu", "Sarp", "Mertcan",
                "Kadir", "Oğuz", "Kağan", "Selim", "Yavuz", "Kenan", "Cemal"
            };
            #endregion

            Random r = new Random();
            List<Robot> listPlayer = new List<Robot>();

            // Benzersiz isim seçici
            List<string> availableNames = new List<string>(names);
            // Karıştır
            for (int i = availableNames.Count - 1; i > 0; i--)
            {
                int j = r.Next(i + 1);
                string temp = availableNames[i];
                availableNames[i] = availableNames[j];
                availableNames[j] = temp;
            }

            for (int i = 0; i < MaxBotCount; i++)
            {
                // İsimlerin sonuna numara EKLEMİYORUZ.
                // Eğer isim listesi biterse, tekrar başa dönüyor ama bot sayısı kadar ismimiz var şimdi.
                string selectedName = availableNames[i % availableNames.Count];

                listPlayer.Add(new Robot()
                {
                    Name = selectedName,
                    Level = r.Next(11, 30),
                    GP = r.Next(1000, 500000),
                    Sex = r.Next(2) == 0,
                    UserType = 1,
                    State = 1,
                    VIPLevel = r.Next(0, 5),
                    Equips = new List<EquipBot>()
                });
            }

            if (listPlayer.Count == 0) return;

            foreach (Robot player in listPlayer)
            {
                try
                {
                    // Rastgele Kıyafet ve Silah ID'leri Seç
                    int headId = m_headIds[r.Next(m_headIds.Length)];
                    int glassId = m_glassIds[r.Next(m_glassIds.Length)];
                    int hairId = m_hairIds[r.Next(m_hairIds.Length)];
                    int effId = m_effIds[r.Next(m_effIds.Length)];
                    int clothId = m_clothIds[r.Next(m_clothIds.Length)];
                    int faceId = m_faceIds[r.Next(m_faceIds.Length)];
                    int weaponId = m_weaponIds[r.Next(m_weaponIds.Length)];
                    int wingId = m_wingIds[r.Next(m_wingIds.Length)];

                    // Style String'i oluştur (Görünüm)
                    // Format: Head|Pic,Glass|Pic,Hair|Pic,Eff|Pic,Cloth|Pic,Face|Pic,Weapon|Pic,,Wing|Pic...
                    string style = GetStyleString(headId, glassId, hairId, effId, clothId, faceId, weaponId, wingId);
                    string colors = ",,,,,,,,,,,,,,,,"; // Varsayılan renkler

                    PlayerInfo playerInfo = new PlayerInfo
                    {
                        IsAutoBot = true,
                        ID = CurRobotID,
                        GP = player.GP,
                        Grade = player.Level,
                        NickName = player.Name,
                        Texp = new TexpInfo(),
                        Sex = player.Sex,
                        State = player.State,
                        VIPLevel = player.VIPLevel,
                        VIPExpireDay = DateTime.MaxValue,
                        Style = style,     // Kıyafet görünümü atanıyor
                        Colors = colors,   // Renkler atanıyor

                    };

                    RobotGamePlayer robotGamePlayer = new RobotGamePlayer(CurRobotID, playerInfo);

                    // Botun Envaterine ve Üzerine Gerçek Eşyaları Giydir (Sadece Görüntü değil, Statü etkilesin)
                    // Silahı Giydir
                    EquipItemToBot(robotGamePlayer, weaponId, 7, r.Next(0, 12)); // Strength 0-12 arası rastgele

                    // Kıyafetleri Giydir (İsteğe bağlı, sadece style string yetebilir ama sağlam olması için ekleyelim)
                    EquipItemToBot(robotGamePlayer, headId, 1, 0);
                    EquipItemToBot(robotGamePlayer, glassId, 2, 0);
                    EquipItemToBot(robotGamePlayer, hairId, 3, 0);
                    EquipItemToBot(robotGamePlayer, effId, 4, 0);
                    EquipItemToBot(robotGamePlayer, clothId, 5, 0);
                    EquipItemToBot(robotGamePlayer, faceId, 6, 0);
                    EquipItemToBot(robotGamePlayer, wingId, 15, 0); // Kanat genelde slot 9'dur

                    // Oyuncuyu dünyaya ekle
                    WorldMgr.AddPlayer(CurRobotID, robotGamePlayer);

                    // Bekleme odasına ekle
                    if (player.UserType == 1)
                    {
                        RoomMgr.WaitingRoom.AddPlayer(robotGamePlayer);
                    }

                    RobotGamePlayers.Add(CurRobotID, robotGamePlayer);
                    CurRobotID--;
                }
                catch (Exception Err)
                {
                    log.Error("Robot yüklenirken hata oluştu, robot adı: " + player.Name, Err);
                }
            }
        }

        // Botlara eşya giydirmek için yardımcı metod
        static void EquipItemToBot(RobotGamePlayer player, int itemId, int category, int strength)
        {
            try
            {
                ItemTemplateInfo template = ItemMgr.FindItemTemplate(itemId);
                if (template != null)
                {
                    // ItemInfo oluştur
                    ItemInfo item = ItemInfo.CreateFromTemplate(template, 1, 0);
                    if (item != null)
                    {

                        item.StrengthenLevel = strength; // Güçlendirme seviyesi
                        // Botun envanterine ekle ve giy
                        player.Equip(item.TemplateID, item.StrengthenLevel, 0); // Sizin kodunuzdaki Equip metodu
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Bot equip error: ItemID {itemId}", ex);
            }
        }

        // Style string oluşturucu (ItemMgr'dan resim ID'lerini çeker)
        static string GetStyleString(int head, int glass, int hair, int eff, int cloth, int face, int weapon, int wing)
        {
            try
            {
                return $"{GetStylePart(head)},{GetStylePart(glass)},{GetStylePart(hair)},{GetStylePart(eff)},{GetStylePart(cloth)},{GetStylePart(face)},{GetStylePart(weapon)},,{GetStylePart(wing)},,,,,,,,,";
            }
            catch
            {
                return "";
            }
        }

        static string GetStylePart(int itemId)
        {
            ItemTemplateInfo template = ItemMgr.FindItemTemplate(itemId);
            if (template != null)
            {
                return $"{itemId}|{template.Pic}";
            }
            return "";
        }

        static void CreateRoomWaiting()
        {
            List<RobotRoom> listRoom = new List<RobotRoom>();
            List<int> roomTypes = new List<int> { 0, 4, 2 };
            Random r = new Random();

            for (int i = 0; i < MaxRoomCount; i++)
            {
                int randIndex = r.Next(roomTypes.Count);
                int randomType = roomTypes[randIndex];

                listRoom.Add(new RobotRoom()
                {
                    PlayerCount = 1,
                    MaxPlayerCount = 4,
                    RoomName = "Eski Gunler #" + r.Next(10, 99),
                    RoomType = randomType
                });
            }

            if (listRoom.Count == 0 || RobotGamePlayers.Count == 0) return;

            foreach (RobotRoom room in listRoom)
            {
                try
                {
                    int randomIndex = r.Next(RobotGamePlayers.Count);
                    RobotGamePlayer selectedBot = null;
                    int selectedKey = 0;

                    int loopIndex = 0;
                    foreach (var kvp in RobotGamePlayers)
                    {
                        if (loopIndex == randomIndex)
                        {
                            selectedBot = kvp.Value;
                            selectedKey = kvp.Key;
                            break;
                        }
                        loopIndex++;
                    }

                    if (selectedBot != null)
                    {
                        RobotGamePlayers.Remove(selectedKey);
                        RoomMgr.CreateRoom(selectedBot, room.RoomName, "", (eRoomType)room.RoomType, (byte)1);
                        RoomMgr.WaitingRoom.RemovePlayer(selectedBot);
                    }
                }
                catch (Exception Err)
                {
                    log.Error("Robot odası oluşturulurken hata: " + room.RoomName, Err);
                }
            }
        }
    }
}

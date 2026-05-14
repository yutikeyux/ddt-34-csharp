using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Maps;
using Game.Server.GuildBattle;
using Game.Server.Rooms;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Server.Games
{
    public class GameMgr
    {
        private static readonly int CLEAR_GAME_INTERVAL = 10000;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static int m_boxBroadcastLevel;
        private static long m_clearGamesTimer;
        private static int m_gameId;
        private static List<BaseGame> m_games;
        private static bool m_running;
        private static int m_serverId;
        private static Thread m_thread;
        public static readonly long THREAD_INTERVAL = 40L;
        private static DateTime m_synDate;
        private static GuildBattleMgr m_guildBattle;

        public static int BoxBroadcastLevel => m_boxBroadcastLevel;
        public static int SynDate => DateTime.Compare(m_synDate.AddSeconds(THREAD_INTERVAL), DateTime.Now);
        public static GuildBattleMgr GuildBattle => m_guildBattle;

        private static void ClearStoppedGames(object state)
        {
            foreach (BaseGame item in state as ArrayList)
            {
                try
                {
                    item.Dispose();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("[GameMgr] Oyun temizlendi - GameId: {0}", item.Id);
                    Console.ResetColor();
                }
                catch (Exception exception)
                {
                    log.Error("game dispose error:", exception);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[GameMgr] Oyun temizlenirken HATA - GameId: {0} | Hata: {1}", item.Id, exception.Message);
                    Console.ResetColor();
                }
            }
        }

        private static void GameThread()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            long num = 0L;
            m_clearGamesTimer = TickHelper.GetTickCount();
            while (m_running)
            {
                long tickCount = TickHelper.GetTickCount();
                int num2 = 0;
                try
                {
                    num2 = UpdateGames(tickCount);
                    UpdateGuildBattle(tickCount);
                    if (m_clearGamesTimer <= tickCount)
                    {
                        m_clearGamesTimer += CLEAR_GAME_INTERVAL;
                        ArrayList arrayList = new ArrayList();
                        foreach (BaseGame game in m_games)
                        {
                            if (game.GameState == eGameState.Stopped)
                            {
                                arrayList.Add(game);
                            }
                        }
                        foreach (BaseGame item in arrayList)
                        {
                            m_games.Remove(item);
                        }
                        if (arrayList.Count > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("[GameMgr] {0} durmuş oyun temizleme kuyruğuna alındı.", arrayList.Count);
                            Console.ResetColor();
                        }
                        ThreadPool.QueueUserWorkItem(ClearStoppedGames, arrayList);
                    }
                }
                catch (Exception exception)
                {
                    log.Error("Game Mgr Thread Error:", exception);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[GameMgr] THREAD HATASI: {0}", exception.Message);
                    Console.ResetColor();
                }
                long tickCount2 = TickHelper.GetTickCount();
                num += THREAD_INTERVAL - (tickCount2 - tickCount);
                if (tickCount2 - tickCount > THREAD_INTERVAL * 2)
                {
                    log.WarnFormat("Game Mgr spent too much times: {0} ms, count:{1}", tickCount2 - tickCount, num2);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[GameMgr] UYARI: Thread çok uzun sürdü - {0} ms, Oyun sayısı: {1}", tickCount2 - tickCount, num2);
                    Console.ResetColor();
                }
                if (num > 0)
                {
                    Thread.Sleep((int)num);
                    num = 0L;
                }
                else if (num < -1000)
                {
                    num += 1000;
                }
                m_synDate = DateTime.Now;
            }
        }

        public static List<BaseGame> GetAllGame()
        {
            List<BaseGame> list = new List<BaseGame>();
            lock (m_games)
            {
                list.AddRange(m_games);
                return list;
            }
        }

        public static bool Setup(int serverId, int boxBroadcastLevel)
        {
            m_thread = new Thread(GameThread);
            m_games = new List<BaseGame>();
            m_serverId = serverId;
            m_boxBroadcastLevel = boxBroadcastLevel;
            m_gameId = 0;
            m_synDate = DateTime.Now;
            m_guildBattle = new GuildBattleMgr();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[GameMgr] Setup tamamlandı - ServerId: {0}, BoxBroadcastLevel: {1}", serverId, boxBroadcastLevel);
            Console.ResetColor();
            return true;
        }

        public static bool Start()
        {
            if (!m_running)
            {
                m_running = true;
                m_thread.Start();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[GameMgr] GameThread başlatıldı.");
                Console.ResetColor();
            }
            return true;
        }

        public static BaseGame StartPVEGame(int roomId, List<IGamePlayer> players, int copyId, eRoomType roomType, eGameType gameType, int timeType, eHardLevel hardLevel, int levelLimits, int currentFloor)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("[GameMgr] PVE Oyunu başlatılıyor - RoomId: {0}, CopyId: {1}, RoomType: {2}, Oyuncu: {3}", roomId, copyId, roomType, players.Count);
                Console.ResetColor();

                PveInfo pveInfo = ((copyId != 0 && copyId != 100000) ? PveInfoMgr.GetPveInfoById(copyId) : PveInfoMgr.GetPveInfoByType(roomType, levelLimits));
                if (pveInfo != null)
                {
                    PVEGame pVEGame = new PVEGame(m_gameId++, roomId, pveInfo, players, null, roomType, gameType, timeType, hardLevel, currentFloor);
                    lock (m_games)
                    {
                        m_games.Add(pVEGame);
                    }
                    pVEGame.Prepare();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("[GameMgr] PVE Başlatıldı - Tip: {0}, Oyuncu: {1}, GameId: {2}, CopyId: {3}, Toplam Oyun: {4}",
                        pVEGame.RoomType, pVEGame.PlayerCount, pVEGame.Id, copyId, m_games.Count);
                    Console.ResetColor();
                    return pVEGame;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[GameMgr] PVE BAŞLATILAMADI - PveInfo bulunamadı! CopyId: {0}, RoomType: {1}, LevelLimits: {2}", copyId, roomType, levelLimits);
                    Console.ResetColor();
                    return null;
                }
            }
            catch (Exception exception)
            {
                log.Error("Oyun oluşturma hatası:", exception);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[GameMgr] PVE OLUŞTURMA HATASI: {0}", exception.Message);
                Console.ResetColor();
                return null;
            }
        }

        public static BaseGame StartPVPGame(int roomId, List<IGamePlayer> red, List<IGamePlayer> blue, int mapIndex, eRoomType roomType, eGameType gameType, int timeType)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("[GameMgr] PVP Oyunu başlatılıyor - RoomId: {0}, MapIndex: {1}, RoomType: {2}", roomId, mapIndex, roomType);
                Console.ResetColor();

                int index = MapMgr.GetMapIndex(mapIndex, (byte)roomType, m_serverId);
                Map map = MapMgr.CloneMap(index);
                if (map != null)
                {
                    PVPGame game = new PVPGame(m_gameId++, roomId, red, blue, map, roomType, gameType, timeType);
                    lock (m_games)
                    {
                        m_games.Add(game);
                    }
                    game.Prepare();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("[GameMgr] PVP Başlatıldı - Tip: {0}, GameId: {1}, Kırmızı: {2}, Mavi: {3}, Toplam Oyun: {4}",
                        game.RoomType, game.Id, red.Count, blue.Count, m_games.Count);
                    Console.ResetColor();
                    return game;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[GameMgr] PVP BAŞLATILAMADI - Harita bulunamadı! MapIndex: {0}", mapIndex);
                    Console.ResetColor();
                    return null;
                }
            }
            catch (Exception exception)
            {
                log.Error("Create game error:", exception);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[GameMgr] PVP OLUŞTURMA HATASI: {0}", exception.Message);
                Console.ResetColor();
                return null;
            }
        }

        public static void Stop()
        {
            if (m_running)
            {
                m_running = false;
                m_thread.Join();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[GameMgr] GameThread durduruldu.");
                Console.ResetColor();
            }
        }

        private static int UpdateGames(long tick)
        {
            IList allGame = GetAllGame();
            if (allGame != null)
            {
                foreach (BaseGame item in allGame)
                {
                    try
                    {
                        eGameState stateBefore = item.GameState;
                        item.Update(tick);
                        eGameState stateAfter = item.GameState;

                        if (stateBefore != stateAfter)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("[GameMgr] Oyun durum değişti - GameId: {0} | {1} --> {2}",
                                item.Id, stateBefore, stateAfter);
                            Console.ResetColor();

                            // Kritik durumlar için ek log
                            if (stateAfter == eGameState.GameOver)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("[GameMgr] !! OYUN BİTTİ !! - GameId: {0}, RoomType: {1}, IsWin: {2}",
                                    item.Id, item.RoomType, (item is PVEGame pve) ? pve.IsWin.ToString() : "N/A");
                                Console.ResetColor();
                            }
                            else if (stateAfter == eGameState.Stopped)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine("[GameMgr] Oyun STOPPED - GameId: {0}, RoomType: {1}",
                                    item.Id, item.RoomType);
                                Console.ResetColor();
                            }
                            else if (stateAfter == eGameState.Loading)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("[GameMgr] Yeni etap yükleniyor - GameId: {0}", item.Id);
                                Console.ResetColor();
                            }
                            else if (stateAfter == eGameState.ALLSessionStopped)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("[GameMgr] TÜM ETAPLAR TAMAMLANDI - GameId: {0}, IsWin: {1}",
                                    item.Id, (item is PVEGame pve2) ? pve2.IsWin.ToString() : "N/A");
                                Console.ResetColor();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        log.Error("Game updated error:", exception);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[GameMgr] Oyun güncelleme HATASI - GameId: {0} | Hata: {1}", item.Id, exception.Message);
                        Console.ResetColor();
                    }
                }
                return allGame.Count;
            }
            return 0;
        }

        public static void ClearAllGames()
        {
            ArrayList arrayList = new ArrayList();
            lock (m_games)
            {
                foreach (BaseGame game in m_games)
                {
                    arrayList.Add(game);
                }
                foreach (BaseGame item in arrayList)
                {
                    m_games.Remove(item);
                    try
                    {
                        item.Dispose();
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("[GameMgr] ClearAllGames: Oyun silindi - GameId: {0}", item.Id);
                        Console.ResetColor();
                    }
                    catch (Exception exception)
                    {
                        log.Error("game dispose error:", exception);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[GameMgr] ClearAllGames HATA - GameId: {0} | {1}", item.Id, exception.Message);
                        Console.ResetColor();
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[GameMgr] ClearAllGames tamamlandı. Silinen oyun: {0}", arrayList.Count);
            Console.ResetColor();
        }

        private static void UpdateGuildBattle(long tick)
        {
            try
            {
                m_guildBattle.Update(tick);
            }
            catch (Exception ex)
            {
                log.Error("Game GuildBattle updated error:", ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[GameMgr] GuildBattle güncelleme HATASI: {0}", ex.Message);
                Console.ResetColor();
            }
        }

        public static bool ExecuteUniversalCommand(string nickname, string action, string propertyName, string value)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[GameMgr] ExecuteUniversalCommand - Nick: {0}, Action: {1}, Property: {2}, Value: {3}", nickname, action, propertyName, value);
            Console.ResetColor();

            var allGames = GetAllGame();
            foreach (var game in allGames)
            {
                var targetPlayer = game.GetAllPlayers().FirstOrDefault(p =>
                    p.PlayerDetail != null &&
                    p.PlayerDetail.PlayerCharacter.NickName.Equals(nickname, StringComparison.OrdinalIgnoreCase));

                if (targetPlayer != null)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("[GameMgr] Oyuncu bulundu - Nick: {0}, GameId: {1}", nickname, game.Id);
                    Console.ResetColor();

                    switch (action.ToLower())
                    {
                        case "set_logic":
                            PropertyInfo prop = targetPlayer.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                            if (prop != null && prop.CanWrite)
                            {
                                prop.SetValue(targetPlayer, Convert.ChangeType(value, prop.PropertyType), null);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("[GameMgr] set_logic başarılı - {0} = {1}", propertyName, value);
                                Console.ResetColor();
                                return true;
                            }
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[GameMgr] set_logic BAŞARISIZ - Property bulunamadı: {0}", propertyName);
                            Console.ResetColor();
                            break;

                        case "set_player":
                            PropertyInfo charProp = targetPlayer.PlayerDetail.PlayerCharacter.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                            if (charProp != null && charProp.CanWrite)
                            {
                                charProp.SetValue(targetPlayer.PlayerDetail.PlayerCharacter, Convert.ChangeType(value, charProp.PropertyType), null);
                                targetPlayer.PlayerDetail.UpdatePublicPlayer();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("[GameMgr] set_player başarılı - {0} = {1}", propertyName, value);
                                Console.ResetColor();
                                return true;
                            }
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[GameMgr] set_player BAŞARISIZ - Property bulunamadı: {0}", propertyName);
                            Console.ResetColor();
                            break;

                        case "send_pkg":
                            GSPacketIn pkg = new GSPacketIn(short.Parse(propertyName));
                            pkg.WriteInt(int.Parse(value));
                            targetPlayer.PlayerDetail.SendTCP(pkg);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("[GameMgr] send_pkg gönderildi - PacketId: {0}, Value: {1}", propertyName, value);
                            Console.ResetColor();
                            return true;
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[GameMgr] ExecuteUniversalCommand BAŞARISIZ - Oyuncu bulunamadı veya işlem gerçekleştirilemedi: {0}", nickname);
            Console.ResetColor();
            return false;
        }

        public static BaseGame StartChallengePVPGame(List<IGamePlayer> red, List<IGamePlayer> blue, BaseRoom redRoom, BaseRoom blueRoom, int mapIndex, eRoomType roomType, eGameType gameType, int timeType)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("[GameMgr] Challenge PVP başlatılıyor - MapIndex: {0}, RoomType: {1}, Kırmızı: {2}, Mavi: {3}",
                    mapIndex, roomType, red.Count, blue.Count);
                Console.ResetColor();

                int index = MapMgr.GetMapIndex(mapIndex, (byte)roomType, m_serverId);
                Map map = MapMgr.CloneMap(index);
                if (map != null)
                {
                    BattleGame game = new BattleGame(m_gameId++, red, redRoom, blue, blueRoom, map, roomType, gameType, timeType);
                    lock (m_games)
                    {
                        m_games.Add(game);
                    }
                    game.Prepare();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("[GameMgr] Challenge PVP Başlatıldı - Tip: {0}, GameId: {1}, Toplam Oyun: {2}",
                        game.RoomType, game.Id, m_games.Count);
                    Console.ResetColor();
                    return game;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[GameMgr] Challenge PVP BAŞLATILAMADI - Harita bulunamadı! MapIndex: {0}", mapIndex);
                    Console.ResetColor();
                    return null;
                }
            }
            catch (Exception e)
            {
                log.Error("Create game error:", e);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[GameMgr] Challenge PVP OLUŞTURMA HATASI: {0}", e.Message);
                Console.ResetColor();
                return null;
            }
        }
    }
}
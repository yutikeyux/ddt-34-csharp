using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Center.Server
{
    public class WorldMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly object _syncStop = new object();

        // World Boss Properties
        public static DateTime begin_time;
        public static DateTime end_time;
        public static int current_blood = int.MaxValue;
        public static int currentPVE_ID;
        public static int fight_time;
        public static bool fightOver;
        public static bool roomClose;
        public static bool worldOpen;
        public static readonly int MAX_BLOOD = int.MaxValue;
        private static readonly int worldbossTime = 60;

        // Boss Configuration
        public static string[] bossResourceId = { "1", "2", "2", "4" };
        public static string[] name = { "Chefão", "Chefe do Mundo", "WorldBoss", "Capitão do Futebol" };
        public static int[] Pve_Id = { 1243, 30001, 30002, 30004 };

        // League and Gold Time Properties
        public static bool IsLeagueOpen;
        public static bool isGoldTimesOpen;
        public static DateTime LeagueOpenTime;
        public static DateTime GoldTimeOpen;

        // Notice System
        public static List<string> NotceList = new List<string>();
        private static string SystemNoticeFile => ConfigurationManager.AppSettings["SystemNoticePath"];

        // Ranking System
        private static Dictionary<string, RankingPersonInfo> m_rankList;

        public static bool CheckName(string nickName)
        {
            return m_rankList.ContainsKey(nickName);
        }

        public static RankingPersonInfo GetSingleRank(string name)
        {
            return m_rankList.TryGetValue(name, out var rank) ? rank : null;
        }

        public static bool LoadNotice(string basePath)
        {
            try
            {
                string fullPath = Path.Combine(basePath, SystemNoticeFile);
                log.Info($"Loading system notices from: {fullPath}");

                if (!File.Exists(fullPath))
                {
                    log.Error($"SystemNotice file not found: {fullPath}");
                    return false;
                }

                XDocument doc = XDocument.Load(fullPath);
                XElement root = doc.Root;

                if (root == null)
                {
                    log.Error("SystemNotice XML has no root element");
                    return false;
                }

                NotceList.Clear();

                foreach (XElement element in root.Elements("notice"))
                {
                    try
                    {
                        if (element.Attribute("id") is XAttribute idAttr &&
                            element.Attribute("notice") is XAttribute noticeAttr)
                        {
                            // Validate ID (though not used in current logic)
                            if (int.TryParse(idAttr.Value, out _))
                            {
                                NotceList.Add(noticeAttr.Value);
                            }
                            else
                            {
                                log.Warn($"Invalid notice ID format: {idAttr.Value}");
                            }
                        }
                        else
                        {
                            log.Warn("Notice element missing required attributes");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error parsing notice element: {ex.Message}");
                    }
                }

                log.InfoFormat("Successfully loaded {0} system notices", NotceList.Count);
                return true;
            }
            catch (Exception ex)
            {
                log.Error($"Failed to load system notices: {ex.Message}", ex);
                return false;
            }
        }

        public static void ReduceBlood(int value)
        {
            if (current_blood > 0)
            {
                current_blood -= value;
            }
        }

        public static List<RankingPersonInfo> SelectTopTen()
        {
            return m_rankList?
                .Values
                .OrderByDescending(r => r.Damage)
                .Take(10)
                .ToList() ?? new List<RankingPersonInfo>();
        }

        public static void SetupWorldBoss(int id)
        {
            current_blood = MAX_BLOOD;
            begin_time = DateTime.Now;
            end_time = begin_time.AddDays(1.0);
            fight_time = worldbossTime - begin_time.Minute;
            fightOver = false;
            roomClose = false;
            currentPVE_ID = id;
            worldOpen = true;
        }

        public static bool Start()
        {
            try
            {
                // Initialize all systems
                m_rankList = new Dictionary<string, RankingPersonInfo>();
                current_blood = MAX_BLOOD;
                begin_time = DateTime.Now;
                LeagueOpenTime = DateTime.Now;
                GoldTimeOpen = DateTime.Now;
                end_time = begin_time.AddDays(1.0);
                fightOver = true;
                roomClose = true;
                worldOpen = false;
                IsLeagueOpen = false;
                isGoldTimesOpen = false;

                // Load system notices
                return LoadNotice("");
            }
            catch (Exception ex)
            {
                log.Error($"WorldMgr initialization failed: {ex.Message}", ex);
                return false;
            }
        }

        public static void UpdateFightTime()
        {
            if (!fightOver)
            {
                fight_time = worldbossTime - begin_time.Minute;
            }
        }

        public static void UpdateRank(int damage, int honor, string nickName)
        {
            if (string.IsNullOrEmpty(nickName)) return;

            lock (_syncStop)
            {
                if (m_rankList.TryGetValue(nickName, out var existingRank))
                {
                    existingRank.Damage += damage;
                    existingRank.Honor += honor;
                }
                else
                {
                    var newRank = new RankingPersonInfo
                    {
                        ID = m_rankList.Count + 1,
                        Name = nickName,
                        Damage = damage,
                        Honor = honor
                    };
                    m_rankList.Add(nickName, newRank);
                }
            }
        }

        public static void WorldBossClearRank()
        {
            lock (_syncStop)
            {
                m_rankList?.Clear();
            }
        }

        public static void WorldBossClose()
        {
            worldOpen = false;
        }

        public static void WorldBossFightOver()
        {
            fightOver = true;
        }

        public static void WorldBossRoomClose()
        {
            roomClose = true;
        }
    }
}
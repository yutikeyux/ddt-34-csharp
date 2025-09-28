using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using log4net.Util;
using System.Threading;
using Bussiness;
using Bussiness.Managers;
using SqlDataProvider.Data;
using Game.Server.GameObjects;

namespace Game.Server.Managers
{
    public class ActiveSystemMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Random rand;
        private static List<LuckStarRewardRecordInfo> m_recordList = new List<LuckStarRewardRecordInfo>();

        private static bool m_IsLeagueOpen;
        public static bool IsLeagueOpen
        {
            get { return m_IsLeagueOpen; }
            set { m_IsLeagueOpen = value; }
        }

        private static bool m_IsGoldTimeOpen;
        public static bool IsGoldTimeOpen
        {
            get { return m_IsGoldTimeOpen; }
            set { m_IsGoldTimeOpen = value; }
        }


        public static List<LuckStarRewardRecordInfo> RecordList
        {
            get { return m_recordList; }
        }

        private static int m_luckStarCountDown;
        public static bool Init()
        {
            try
            {
                rand = new Random();
                m_IsLeagueOpen = false;
                m_IsGoldTimeOpen = false;
                m_luckStarCountDown = Math.Abs((60 - DateTime.Now.Minute) - 30);
                return true;
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("ActiveSystemMgr", e);
                return false;
            }
        }

        //LuckStarRewardRecord
        public static void UpdateLuckStarRewardRecord(int PlayerID, string nickName, int TemplateID, int Count, int isVip)
        {
            AddRewardRecord(PlayerID, nickName, TemplateID, Count, isVip);
            GameServer.Instance.LoginServer.SendLuckStarRewardRecord(PlayerID, nickName, TemplateID, Count, isVip);
        }
        public static void AddRewardRecord(int PlayerID, string nickName, int TemplateID, int Count, int isVip)
        {
            if (m_recordList.Count > 10)
                m_recordList.Clear();

            LuckStarRewardRecordInfo record = new LuckStarRewardRecordInfo();
            record.PlayerID = PlayerID;
            record.nickName = nickName;
            record.useStarNum = 1;
            record.TemplateID = TemplateID;
            record.Count = Count;
            record.isVip = isVip;
            m_recordList.Add(record);
        }

        public static void UpdateIsLeagueOpen(bool open)
        {
            m_IsLeagueOpen = open;

            GamePlayer[] players = WorldMgr.GetAllPlayers();
            foreach (GamePlayer p in players)
            {
                if (p != null && p.PlayerCharacter.ID > 0)
                {
                    if (open)
                    {
                        p.Out.SendLeagueNotice(p.PlayerCharacter.ID, p.BattleData.MatchInfo.restCount, p.BattleData.maxCount, 1);
                    }
                    else
                    {
                        p.Out.SendLeagueNotice(p.PlayerCharacter.ID, p.BattleData.MatchInfo.restCount, p.BattleData.maxCount, 2);
                    }
                }
            }
        }

        public static List<SqlDataProvider.Data.ItemInfo> GetPyramidAward(int layer)
        {
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            List<SqlDataProvider.Data.ActivitySystemItemInfo> filtInfos = new List<SqlDataProvider.Data.ActivitySystemItemInfo>();
            List<SqlDataProvider.Data.ActivitySystemItemInfo> unFiltInfos = ActiveMgr.GetActivitySystemItemByLayer(layer);
            int dropItemCount = 1;
            int maxRound = ThreadSafeRandom.NextStatic(unFiltInfos.Select(s => s.Probability).Max());
            List<SqlDataProvider.Data.ActivitySystemItemInfo> roundInfos = unFiltInfos.Where(s => s.Probability >= maxRound).ToList();
            int maxItems = roundInfos.Count();
            if (maxItems > 0)
            {
                dropItemCount = dropItemCount > maxItems ? maxItems : dropItemCount;
                int[] randomArray = GetRandomUnrepeatArray(0, maxItems - 1, dropItemCount);
                foreach (int i in randomArray)
                {
                    SqlDataProvider.Data.ActivitySystemItemInfo item = roundInfos[i];
                    filtInfos.Add(item);
                }
            }
            foreach (SqlDataProvider.Data.ActivitySystemItemInfo info in filtInfos)
            {
                SqlDataProvider.Data.ItemInfo item = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info.TemplateID), info.Count, (int)eItemAddType.Buy);
                item.TemplateID = info.TemplateID;
                item.IsBinds = info.IsBind;
                item.ValidDate = info.ValidDate;
                item.Count = info.Count;
                item.StrengthenLevel = info.StrengthLevel;
                item.AttackCompose = 0;
                item.DefendCompose = 0;
                item.AgilityCompose = 0;
                item.LuckCompose = 0;
                itemInfoList.Add(item);
            }
            return itemInfoList;
        }
        public static void NewDay()
        {
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            GamePlayer[] array = allPlayers;
            foreach (GamePlayer gamePlayer in array)
            {
                gamePlayer.SaveIntoDatabase();
            }
            using PlayerBussiness playerBussiness = new PlayerBussiness();
         
            GamePlayer[] array2 = allPlayers;
            foreach (GamePlayer gamePlayer2 in array2)
            {
               
            
                gamePlayer2.Actives.LoadFromDatabase();
             
               
                gamePlayer2.GmActivity.LoadFromDatabase();//burada
            }
            GmActivityMgr.ReLoad();
        }

        public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
        {
            int j;
            int[] resultRound = new int[count];
            for (j = 0; j < count; j++)
            {
                int i = rand.Next(minValue, maxValue + 1);
                int num = 0;
                for (int k = 0; k < j; k++)
                {
                    if (resultRound[k] == i)
                    {
                        num = num + 1;
                    }
                }
                if (num == 0)
                {
                    resultRound[j] = i;
                }
                else
                {
                    j = j - 1;
                }
            }
            return resultRound;
        }
    }
}

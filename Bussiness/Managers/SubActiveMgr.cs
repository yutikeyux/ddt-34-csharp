using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

namespace Bussiness.Managers
{
    public class SubActiveMgr
    {
        // Fields
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static Dictionary<int, SubActiveConditionInfo> m_subActiveConditionInfo = new Dictionary<int, SubActiveConditionInfo>();
        public static Dictionary<int, List<SubActiveInfo>> m_subActiveInfo = new Dictionary<int, List<SubActiveInfo>>();
        private static ReaderWriterLock m_clientLocker = new ReaderWriterLock();
        // Methods
        public static SubActiveConditionInfo GetSubActiveInfo(ItemInfo item)
        {
            m_clientLocker.AcquireWriterLock(Timeout.Infinite);
            try
            {
                foreach (List<SubActiveInfo> SubActives in m_subActiveInfo.Values)
                {
                    foreach (SubActiveInfo active in SubActives)
                    {
                        if (active.IsValid())
                        {
                            foreach (SubActiveConditionInfo condition in m_subActiveConditionInfo.Values)
                            {
                                if (active.OnActive(condition.ActiveID, condition.SubID))
                                {
                                    if (item.GoldValidDate() && condition.OnAvaible(item.Template.CategoryID, item.TemplateID) 
                                        && condition.OnGold() && condition.OnStrengThen() == item.StrengthenLevel)
                                        return condition;
                                    if (!condition.OnGold() && condition.OnStrengThen() == 1 && item.StrengthenLevel == 0 
                                        && condition.OnAvaible(item.Template.CategoryID, item.TemplateID))
                                        return condition;
                                    if (!condition.OnGold() && condition.OnStrengThen() == item.StrengthenLevel 
                                        && condition.OnAvaible(item.Template.CategoryID, item.TemplateID))
                                        return condition;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                m_clientLocker.ReleaseWriterLock();
            }
            return null;
        }

        public static bool Init()
        {
            return ReLoad();
        }

        public static Dictionary<int, SubActiveConditionInfo> LoadSubActiveConditionDb(Dictionary<int, List<SubActiveInfo>> conditions)
        {
            Dictionary<int, SubActiveConditionInfo> dictionary = new Dictionary<int, SubActiveConditionInfo>();
            using (ActiveBussiness db = new ActiveBussiness())
            {
                foreach (int ActiveID in conditions.Keys)
                {
                    SubActiveConditionInfo[] allSubActiveConditionInfo = db.GetAllSubActiveCondition(ActiveID);
                    foreach (SubActiveConditionInfo info in allSubActiveConditionInfo)
                    {
                        if (!((ActiveID != info.ActiveID) || dictionary.ContainsKey(info.ID)))
                        {
                            dictionary.Add(info.ID, info);
                            //Console.WriteLine("info: {0} ", info.Value);
                        }
                    }
                }
            }
            return dictionary;
        }

        public static Dictionary<int, List<SubActiveInfo>> LoadSubActiveDb()
        {
            Dictionary<int, List<SubActiveInfo>> dictionary = new Dictionary<int, List<SubActiveInfo>>();
            using (ActiveBussiness db = new ActiveBussiness())
            {
                SubActiveInfo[] allSubActiveInfo = db.GetAllSubActive();
                foreach (SubActiveInfo info in allSubActiveInfo)
                {
                    List<SubActiveInfo> list = new List<SubActiveInfo>();
                    if (!dictionary.ContainsKey(info.ActiveID))
                    {
                        list.Add(info);
                        dictionary.Add(info.ActiveID, list);
                    }
                    else
                    {
                        dictionary[info.ActiveID].Add(info);
                    }
                }
            }
            return dictionary;
        }

        public static bool ReLoad()
        {
            try
            {
                Dictionary<int, List<SubActiveInfo>> subActives = LoadSubActiveDb();
                Dictionary<int, SubActiveConditionInfo> subActiveCondition = LoadSubActiveConditionDb(subActives);
                if (subActives.Count > 0)
                {
                    Interlocked.Exchange(ref m_subActiveInfo, subActives);
                    Interlocked.Exchange(ref m_subActiveConditionInfo, subActiveCondition);
                }
                return true;
            }
            catch (Exception exception)
            {
                log.Error("SubActiveMgr", exception);
            }
            return false;
        }
    }


}


using System;
using System.Collections.Generic;
using log4net;
using System.Reflection;
using System.Threading;
using Bussiness;
using SqlDataProvider.Data;
using System.Linq;
namespace Bussiness.Managers
{
    public class SetsBuildTempMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Dictionary<int, SetsBuildTempInfo> m_setsBuildTemps = new Dictionary<int, SetsBuildTempInfo>();
        private static ThreadSafeRandom random = new ThreadSafeRandom();
        public static bool Init()
        {
            return ReLoad();
        }
        public static bool ReLoad()
        {
            try
            {
                Dictionary<int, SetsBuildTempInfo> tempSetsBuildTemps = LoadFromDatabase();
                if (tempSetsBuildTemps.Values.Count > 0)
                {
                    Interlocked.Exchange(ref m_setsBuildTemps, tempSetsBuildTemps);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("SetsBuildTempMgr init error:", ex);
            }
            return false;
        }
        private static Dictionary<int, SetsBuildTempInfo> LoadFromDatabase()
        {
            Dictionary<int, SetsBuildTempInfo> list = new Dictionary<int, SetsBuildTempInfo>();
            using (ProduceBussiness db = new ProduceBussiness())
            {
                SetsBuildTempInfo[] setsBuildTempInfos = db.GetAllSetsBuildTemp();
                foreach (SetsBuildTempInfo info in setsBuildTempInfos)
                {
                    if (!list.ContainsKey(info.Level))
                    {
                        list.Add(info.Level, info);
                    }
                }
            }
            return list;
        }
        public static List<SetsBuildTempInfo> GetAllSetsBuildTemp()
        {
            if (m_setsBuildTemps.Count == 0)
                Init();
            return m_setsBuildTemps.Values.ToList();
        }
        public static SetsBuildTempInfo FindSetsBuildTemp(int id)
        {
            if (m_setsBuildTemps.Count == 0)
                Init();
            if (m_setsBuildTemps.ContainsKey(id))
                return m_setsBuildTemps[id];
            return null;
        }
        public static int SetsBuildMax()
        {
            int maxLv = m_setsBuildTemps.Count;
            return m_setsBuildTemps[maxLv].Exp;
        }
        public static SetsBuildTempInfo FindSetsBuildExp(int exp)
        {
            List<SetsBuildTempInfo> infos = GetAllSetsBuildTemp();
            SetsBuildTempInfo info = null;
            for (int i = 0; i < infos.Count; i++)
            {
                info = infos[i];
                if (exp <= info.Exp)
                {
                    return infos[(i - 1) < 0 ? 0 : (i - 1)];
                }
            }
            return info;
        }
        //public static SetsBuildTempInfo FindNextSetsBuildExp(int exp)
        //{
        //    List<SetsBuildTempInfo> infos = GetAllSetsBuildTemp();
        //    SetsBuildTempInfo info = null;
        //    for (int i = 0; i < infos.Count; i++)
        //    {
        //        info = infos[i];
        //        if (exp < info.Exp)
        //            return info;
        //    }
        //    return info;
        //}

        public static SetsBuildTempInfo FindNextSetsBuildExp(int exp)
        {
            int max = SetsBuildMax();
            List<SetsBuildTempInfo> infos = GetAllSetsBuildTemp();
            SetsBuildTempInfo info = null;
            for (int i = 0; i < infos.Count; i++)
            {
                info = infos[(i + 1) > max ? max : (i + 1)];
                if (exp < info.Exp)
                    return info;
            }
            return info;
        }

        public static void GetSetsBuildProp(int exp, ref int def, ref int blood, ref int luck, ref int agi, ref int dam)
        {
            List<SetsBuildTempInfo> infos = GetAllSetsBuildTemp();
            SetsBuildTempInfo info = null;
            for (int i = 0; i < infos.Count; i++)
            {
                info = infos[i];
                if (info != null && exp >= info.Exp)
                {
                    def += info.DefenceGrow;
                    blood += info.BloodGrow;
                    luck += info.LuckGrow;
                    agi += info.AgilityGrow;
                    dam += info.DamageGrow;
                }
            }
        }
        public static void GetSetsBuildProp(int exp, ref int guard)
        {
            List<SetsBuildTempInfo> infos = GetAllSetsBuildTemp();
            SetsBuildTempInfo info = null;
            for (int i = 0; i < infos.Count; i++)
            {
                info = infos[i];
                if (info != null && exp >= info.Exp)
                {
                    guard += info.GuardGrow;
                }
            }
        }
    }
}
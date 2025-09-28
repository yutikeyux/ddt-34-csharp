using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Game.Server.Managers
{
    public class NecklaceMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static Dictionary<int, StrengThenExpInfo> _Strengthens_Exps;

        public static readonly int NECKLACE_MAX_LEVEL = 12;

        public static bool Init() //bunu init etmemişler
        {
            try
            {
                _Strengthens_Exps = new Dictionary<int, StrengThenExpInfo>();
                return LoadStrengthen(_Strengthens_Exps);
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("StrengthenMgr", e);
                return false;
            }

        }


        public static bool ReLoad()
        {
            try
            {
                Dictionary<int, StrengThenExpInfo> tempStrengthenExps = new Dictionary<int, StrengThenExpInfo>();
                if (LoadStrengthen(tempStrengthenExps))
                {
                    try
                    {
                        _Strengthens_Exps = tempStrengthenExps;
                        return true;
                    }
                    catch
                    { }

                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("StrengthenMgr", e);
            }

            return false;
        }

        private static bool LoadStrengthen(Dictionary<int, StrengThenExpInfo> StrengthenExp)
        {
            using (ProduceBussiness db = new ProduceBussiness())
            {
                StrengThenExpInfo[] Expinfos = db.GetAllStrengThenExp();
                foreach (StrengThenExpInfo info in Expinfos)
                {
                    if (!StrengthenExp.ContainsKey(info.Level))
                    {
                        StrengthenExp.Add(info.Level, info);
                    }
                }
            }
            return true;
        }


        public static StrengThenExpInfo FindStrengthenExpInfo(int level)
        {
            if (_Strengthens_Exps.ContainsKey(level))
                return _Strengthens_Exps[level];

            return null;
        }

        public static int GetNecklaceExpAdd(int exp, int currentPlus)
        {
            int level = GetNecklaceLevelByGP(exp);
            StrengThenExpInfo stre = FindStrengthenExpInfo(level);
            if (stre == null)
                return currentPlus;
            return stre.NecklaceStrengthPlus;
        }

        public static int GetNecklaceLevelByGP(int exp)
        {
            int level = NECKLACE_MAX_LEVEL;
            while (level > -1)
            {
                if (_Strengthens_Exps[level].NecklaceStrengthExp <= exp)
                    return level;

                level--;
            }
            return 1;
        }

        public static int GetNecklaceMaxExp()
        {
            StrengThenExpInfo stre = FindStrengthenExpInfo(NECKLACE_MAX_LEVEL);
            if (stre == null)
                return 0;

            return stre.NecklaceStrengthExp;
        }
    }
}

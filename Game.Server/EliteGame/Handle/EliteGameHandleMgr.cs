using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Game.Server.EliteGame.Handle
{
    public class EliteGameHandleMgr
    {
        private Dictionary<int, IEliteGameCommandHadler> handles = new Dictionary<int, IEliteGameCommandHadler>();
        public IEliteGameCommandHadler LoadCommandHandler(int code)
        {
            if (handles.ContainsKey(code))
                return handles[code];
            log.Error("LoadCommandHandler code002: " + code.ToString());
            return null;
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public EliteGameHandleMgr()
        {
            handles.Clear();
            SearchCommandHandlers(Assembly.GetAssembly(typeof(GameServer)));
        }

        protected int SearchCommandHandlers(Assembly assembly)
        {
            int count = 0;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass != true)
                    continue;
                if (type.GetInterface("Game.Server.EliteGame.Handle.IEliteGameCommandHadler") == null)
                    continue;

                EliteGameHandleAttbute[] attr = (EliteGameHandleAttbute[])type.GetCustomAttributes(typeof(EliteGameHandleAttbute), true);
                if (attr.Length > 0)
                {
                    count++;
                    RegisterCommandHandler(attr[0].Code, Activator.CreateInstance(type) as IEliteGameCommandHadler);
                }
            }
            return count;
        }

        protected  void RegisterCommandHandler(int code, IEliteGameCommandHadler handle)
        {
            handles.Add(code, handle);
        }
    }
}

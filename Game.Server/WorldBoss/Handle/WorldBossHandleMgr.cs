using Bussiness;  // 2. EKLENDİ
using Game.Logic; // 1. EKLENDİ
using log4net; // Hata loglaması için eklendi
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.WorldBoss.Handle
{
    public class WorldBossHandleMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // 3. EKLENDİ: Singleton (Tekil Nesne) Deseni
        private static readonly WorldBossHandleMgr _instance = new WorldBossHandleMgr();
        public static WorldBossHandleMgr Instance => _instance;

        private Dictionary<int, IWorldBossCommandHandler> handles = new Dictionary<int, IWorldBossCommandHandler>();

        // 4. DEĞİŞTİ: Artık çökmemesi için 'ContainsKey' kontrolü yapıyor
        public IWorldBossCommandHandler LoadCommandHandler(int code)
        {
            if (handles.ContainsKey(code))
            {
                return handles[code];
            }

            log.Error($"WorldBoss Command Handler not found for code: {code}");
            return null; // Çökmek yerine null döndür
        }

        // 5. DEĞİŞTİ: Constructor 'private' yapıldı ve TÜM kütüphaneleri tarıyor
        private WorldBossHandleMgr()
        {
            handles.Clear();
            log.Info("Loading WorldBoss Command Handlers...");
            int count = 0;
            count += SearchCommandHandlers(Assembly.GetAssembly(typeof(GameServer))); // Game.Server.dll
            count += SearchCommandHandlers(Assembly.GetAssembly(typeof(BaseGame)));   // Game.Logic.dll
            count += SearchCommandHandlers(Assembly.GetAssembly(typeof(LanguageMgr)));// Bussiness.dll
            log.Info($"Loaded {count} WorldBoss Command Handlers.");
        }

        protected int SearchCommandHandlers(Assembly assembly)
        {
            int count = 0;
            if (assembly == null) return count; // Güvenlik kontrolü

            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass != true)
                    continue;
                if (type.GetInterface("Game.Server.WorldBoss.Handle.IWorldBossCommandHandler") == null)
                    continue;

                WorldBossHandleAttribute[] attr = (WorldBossHandleAttribute[])type.GetCustomAttributes(typeof(WorldBossHandleAttribute), true);
                if (attr.Length > 0)
                {
                    count++;
                    RegisterCommandHandler(attr[0].Code, Activator.CreateInstance(type) as IWorldBossCommandHandler);
                }
            }
            return count;
        }

        protected void RegisterCommandHandler(int code, IWorldBossCommandHandler handle)
        {
            if (handles.ContainsKey(code))
            {
                log.Warn($"WorldBoss Command Handler code {code} already exists and was replaced.");
                handles[code] = handle;
            }
            else
            {
                handles.Add(code, handle);
            }
        }
    }
}
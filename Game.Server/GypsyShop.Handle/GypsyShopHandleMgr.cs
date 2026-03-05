using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;

namespace Game.Server.GypsyShop.Handle
{
	public class GypsyShopHandleMgr
	{
		private Dictionary<int, IGypsyShopCommandHadler> handles = new Dictionary<int, IGypsyShopCommandHadler>();

		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public IGypsyShopCommandHadler LoadCommandHandler(int code)
		{
			if (handles.ContainsKey(code))
			{
				return handles[code];
			}
			log.Error((object)("LoadCommandHandler code010: " + code));
			return null;
		}

		public GypsyShopHandleMgr()
		{
			handles.Clear();
			SearchCommandHandlers(Assembly.GetAssembly(typeof(GameServer)));
		}

		protected int SearchCommandHandlers(Assembly assembly)
		{
			int num = 0;
			Type[] types = assembly.GetTypes();
			Type[] array = types;
			foreach (Type type in array)
			{
				if (type.IsClass && !(type.GetInterface("Game.Server.GypsyShop.Handle.IGypsyShopCommandHadler") == null))
				{
					GypsyShopHandleAttbute[] array2 = (GypsyShopHandleAttbute[])type.GetCustomAttributes(typeof(GypsyShopHandleAttbute), inherit: true);
					if (array2.Length != 0)
					{
						num++;
						RegisterCommandHandler(array2[0].Code, Activator.CreateInstance(type) as IGypsyShopCommandHadler);
					}
				}
			}
			return num;
		}

		protected void RegisterCommandHandler(int code, IGypsyShopCommandHadler handle)
		{
			handles.Add(code, handle);
		}
	}
}

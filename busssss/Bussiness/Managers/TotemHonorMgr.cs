using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000043 RID: 67
	public class TotemHonorMgr
	{
		// Token: 0x06000372 RID: 882 RVA: 0x0003B450 File Offset: 0x00039650
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, TotemHonorTemplateInfo> tempConsortiaLevel = new Dictionary<int, TotemHonorTemplateInfo>();
				bool flag = TotemHonorMgr.Load(tempConsortiaLevel);
				if (flag)
				{
					try
					{
						TotemHonorMgr._totemHonorTemplate = tempConsortiaLevel;
						return true;
					}
					catch
					{
					}
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = TotemHonorMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					TotemHonorMgr.log.Error("TotemHonorMgr", e);
				}
			}
			return false;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0003B4CC File Offset: 0x000396CC
		public static bool Init()
		{
			bool result;
			try
			{
				TotemHonorMgr._totemHonorTemplate = new Dictionary<int, TotemHonorTemplateInfo>();
				TotemHonorMgr.rand = new Random();
				result = TotemHonorMgr.Load(TotemHonorMgr._totemHonorTemplate);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = TotemHonorMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					TotemHonorMgr.log.Error("TotemHonorMgr", e);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0003B534 File Offset: 0x00039734
		private static bool Load(Dictionary<int, TotemHonorTemplateInfo> TotemHonorTemplate)
		{
			using (ProduceBussiness db = new ProduceBussiness())
			{
				TotemHonorTemplateInfo[] infos = db.GetAllTotemHonorTemplate();
				foreach (TotemHonorTemplateInfo info in infos)
				{
					bool flag = !TotemHonorTemplate.ContainsKey(info.ID);
					if (flag)
					{
						TotemHonorTemplate.Add(info.ID, info);
					}
				}
			}
			return true;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0003B5B8 File Offset: 0x000397B8
		public static TotemHonorTemplateInfo FindTotemHonorTemplateInfo(int ID)
		{
			bool flag = TotemHonorMgr._totemHonorTemplate.ContainsKey(ID);
			TotemHonorTemplateInfo result;
			if (flag)
			{
				result = TotemHonorMgr._totemHonorTemplate[ID];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04000184 RID: 388
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000185 RID: 389
		private static Dictionary<int, TotemHonorTemplateInfo> _totemHonorTemplate;

		// Token: 0x04000186 RID: 390
		private static Random rand;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000039 RID: 57
	public class GoldEquipMgr
	{
		// Token: 0x060002FB RID: 763 RVA: 0x00036F64 File Offset: 0x00035164
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, GoldEquipTemplateInfo> tempItems = new Dictionary<int, GoldEquipTemplateInfo>();
				List<GoldEquipTemplateInfo> tempAllItems = new List<GoldEquipTemplateInfo>();
				bool flag = GoldEquipMgr.LoadItem(tempItems, tempAllItems);
				if (flag)
				{
					try
					{
						GoldEquipMgr._items = tempItems;
						return true;
					}
					catch
					{
					}
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = GoldEquipMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					GoldEquipMgr.log.Error("ReLoad", e);
				}
			}
			return false;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00036FE8 File Offset: 0x000351E8
		public static bool Init()
		{
			bool result;
			try
			{
				GoldEquipMgr._items = new Dictionary<int, GoldEquipTemplateInfo>();
				GoldEquipMgr._itemAlls = new List<GoldEquipTemplateInfo>();
				result = GoldEquipMgr.LoadItem(GoldEquipMgr._items, GoldEquipMgr._itemAlls);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = GoldEquipMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					GoldEquipMgr.log.Error("Init", e);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00037054 File Offset: 0x00035254
		public static bool LoadItem(Dictionary<int, GoldEquipTemplateInfo> infos, List<GoldEquipTemplateInfo> infoAlls)
		{
			using (ProduceBussiness db = new ProduceBussiness())
			{
				GoldEquipTemplateInfo[] items = db.GetAllGoldEquipTemplateLoad();
				foreach (GoldEquipTemplateInfo item in items)
				{
					bool flag = item.OldTemplateId == -1;
					if (flag)
					{
						infoAlls.Add(item);
					}
					else
					{
						bool flag2 = !infos.Keys.Contains(item.OldTemplateId);
						if (flag2)
						{
							infos.Add(item.OldTemplateId, item);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000370F8 File Offset: 0x000352F8
		public static GoldEquipTemplateInfo FindGoldEquipByTemplate(int templateId)
		{
			bool flag = GoldEquipMgr._items == null;
			if (flag)
			{
				GoldEquipMgr.Init();
			}
			try
			{
				bool flag2 = GoldEquipMgr._items.Keys.Contains(templateId);
				if (flag2)
				{
					return GoldEquipMgr._items[templateId];
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0003715C File Offset: 0x0003535C
		public static GoldEquipTemplateInfo FindGoldEquipOldTemplate(int TemplateId)
		{
			bool flag = GoldEquipMgr._items == null;
			if (flag)
			{
				GoldEquipMgr.Init();
			}
			try
			{
				foreach (GoldEquipTemplateInfo info in GoldEquipMgr._items.Values)
				{
					string OldTemplateId = info.OldTemplateId.ToString();
					bool flag2 = info.NewTemplateId == TemplateId && OldTemplateId.Substring(4) != "4";
					if (flag2)
					{
						return info;
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0003721C File Offset: 0x0003541C
		public static GoldEquipTemplateInfo FindGoldEquipByTemplate(int templateId, int categoryId)
		{
			GoldEquipTemplateInfo info = null;
			bool flag = GoldEquipMgr._items == null;
			if (flag)
			{
				GoldEquipMgr.Init();
			}
			GoldEquipTemplateInfo result;
			try
			{
				foreach (GoldEquipTemplateInfo equipTemplateInfo in GoldEquipMgr._items.Values)
				{
					bool flag2 = equipTemplateInfo.OldTemplateId == templateId;
					if (flag2)
					{
						info = equipTemplateInfo;
						break;
					}
				}
				bool flag3 = info == null;
				if (flag3)
				{
					foreach (GoldEquipTemplateInfo equipTemplateInfo2 in GoldEquipMgr._itemAlls)
					{
						bool flag4 = equipTemplateInfo2.OldTemplateId == -1 && equipTemplateInfo2.CategoryID == categoryId;
						if (flag4)
						{
							info = equipTemplateInfo2;
							break;
						}
					}
				}
				result = info;
			}
			catch
			{
				result = info;
			}
			return result;
		}

		// Token: 0x04000159 RID: 345
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400015A RID: 346
		private static Dictionary<int, GoldEquipTemplateInfo> _items;

		// Token: 0x0400015B RID: 347
		private static List<GoldEquipTemplateInfo> _itemAlls;
	}
}

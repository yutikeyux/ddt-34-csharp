using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x0200003B RID: 59
	public class ItemMgr
	{
		// Token: 0x06000314 RID: 788 RVA: 0x000386F0 File Offset: 0x000368F0
		public static LoadUserBoxInfo FindItemBoxTemplate(int Id)
		{
			bool flag = ItemMgr._timeBoxs == null;
			if (flag)
			{
				ItemMgr.Init();
			}
			ItemMgr.m_lock.AcquireReaderLock(10000);
			try
			{
				bool flag2 = ItemMgr._timeBoxs.Keys.Contains(Id);
				if (flag2)
				{
					return ItemMgr._timeBoxs[Id];
				}
			}
			finally
			{
				ItemMgr.m_lock.ReleaseReaderLock();
			}
			return null;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0003876C File Offset: 0x0003696C
		public static ItemInfo CreateInfoFromGmReward(GmActiveRewardInfo reward, int times = 1)
		{
			ItemInfo ıtemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(reward.templateId), reward.count, 102);
			bool flag = ıtemInfo == null;
			ItemInfo result;
			if (flag)
			{
				result = null;
			}
			else
			{
				ıtemInfo.IsBinds = (reward.isBind == 1);
				ıtemInfo.ValidDate = reward.validDate;
				ıtemInfo.Count = reward.count * times;
				string[] array = reward.property.Split(new char[]
				{
					','
				});
				ıtemInfo.StrengthenLevel = int.Parse(array[0]);
				ıtemInfo.AttackCompose = int.Parse(array[1]);
				ıtemInfo.DefendCompose = int.Parse(array[2]);
				ıtemInfo.AgilityCompose = int.Parse(array[3]);
				ıtemInfo.LuckCompose = int.Parse(array[4]);
				result = ıtemInfo;
			}
			return result;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00038838 File Offset: 0x00036A38
		public static LoadUserBoxInfo FindItemBoxTypeAndLv(int type, int lv)
		{
			bool flag = ItemMgr._timeBoxs == null;
			if (flag)
			{
				ItemMgr.Init();
			}
			ItemMgr.m_lock.AcquireReaderLock(10000);
			try
			{
				foreach (LoadUserBoxInfo info in ItemMgr._timeBoxs.Values)
				{
					bool flag2 = info.Type == type && info.Level == lv;
					if (flag2)
					{
						return info;
					}
				}
			}
			finally
			{
				ItemMgr.m_lock.ReleaseReaderLock();
			}
			return null;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000388F8 File Offset: 0x00036AF8
		public static ItemTemplateInfo FindItemTemplate(int templateId)
		{
			bool flag = ItemMgr._items == null;
			if (flag)
			{
				ItemMgr.Init();
			}
			ItemMgr.m_lock.AcquireReaderLock(10000);
			try
			{
				bool flag2 = ItemMgr._items.Keys.Contains(templateId);
				if (flag2)
				{
					return ItemMgr._items[templateId];
				}
			}
			finally
			{
				ItemMgr.m_lock.ReleaseReaderLock();
			}
			return null;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00038974 File Offset: 0x00036B74
		public static ItemTemplateInfo GetGoodsbyFusionTypeandLevel(int fusionType, int level)
		{
			bool flag = ItemMgr._items == null;
			if (flag)
			{
				ItemMgr.Init();
			}
			ItemMgr.m_lock.AcquireReaderLock(-1);
			try
			{
				foreach (ItemTemplateInfo info in ItemMgr._items.Values)
				{
					bool flag2 = info.FusionType == fusionType && info.Level == level;
					if (flag2)
					{
						return info;
					}
				}
			}
			finally
			{
				ItemMgr.m_lock.ReleaseReaderLock();
			}
			return null;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00038A30 File Offset: 0x00036C30
		public static ItemTemplateInfo GetGoodsbyFusionTypeandQuality(int fusionType, int quality)
		{
			bool flag = ItemMgr._items == null;
			if (flag)
			{
				ItemMgr.Init();
			}
			ItemMgr.m_lock.AcquireReaderLock(10000);
			try
			{
				foreach (ItemTemplateInfo info in ItemMgr._items.Values)
				{
					bool flag2 = info.FusionType == fusionType && info.Quality == quality;
					if (flag2)
					{
						return info;
					}
				}
			}
			finally
			{
				ItemMgr.m_lock.ReleaseReaderLock();
			}
			return null;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00038AF0 File Offset: 0x00036CF0
		public static bool Init()
		{
			bool result;
			try
			{
				ItemMgr.m_lock = new ReaderWriterLock();
				ItemMgr._items = new Dictionary<int, ItemTemplateInfo>();
				ItemMgr._timeBoxs = new Dictionary<int, LoadUserBoxInfo>();
				ItemMgr.Lists = new List<ItemTemplateInfo>();
				result = ItemMgr.LoadItem(ItemMgr._items, ItemMgr._timeBoxs);
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = ItemMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					ItemMgr.log.Error("Init", exception);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00038B74 File Offset: 0x00036D74
		public static bool LoadItem(Dictionary<int, ItemTemplateInfo> infos, Dictionary<int, LoadUserBoxInfo> userBoxs)
		{
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				ItemTemplateInfo[] allGoods = bussiness.GetAllGoods();
				ItemTemplateInfo[] array = allGoods;
				foreach (ItemTemplateInfo info in array)
				{
					bool flag = !infos.Keys.Contains(info.TemplateID);
					if (flag)
					{
						infos.Add(info.TemplateID, info);
					}
				}
				LoadUserBoxInfo[] allTimeBoxAward = bussiness.GetAllTimeBoxAward();
				LoadUserBoxInfo[] array2 = allTimeBoxAward;
				foreach (LoadUserBoxInfo info2 in array2)
				{
					bool flag2 = !userBoxs.Keys.Contains(info2.ID);
					if (flag2)
					{
						userBoxs.Add(info2.ID, info2);
					}
				}
			}
			return true;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00038C5C File Offset: 0x00036E5C
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, ItemTemplateInfo> infos = new Dictionary<int, ItemTemplateInfo>();
				Dictionary<int, LoadUserBoxInfo> userBoxs = new Dictionary<int, LoadUserBoxInfo>();
				bool flag = ItemMgr.LoadItem(infos, userBoxs);
				if (flag)
				{
					ItemMgr.m_lock.AcquireWriterLock(-1);
					try
					{
						ItemMgr._items = infos;
						ItemMgr._timeBoxs = userBoxs;
						return true;
					}
					catch
					{
					}
					finally
					{
						ItemMgr.m_lock.ReleaseWriterLock();
					}
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = ItemMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					ItemMgr.log.Error("ReLoad", exception);
				}
			}
			return false;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00038D10 File Offset: 0x00036F10
		public static List<ItemInfo> SpiltGoodsMaxCount(ItemInfo itemInfo)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			for (int i = 0; i < itemInfo.Count; i += itemInfo.Template.MaxCount)
			{
				int num2 = (itemInfo.Count < itemInfo.Template.MaxCount) ? itemInfo.Count : itemInfo.Template.MaxCount;
				ItemInfo item = itemInfo.Clone();
				item.Count = num2;
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00038D8C File Offset: 0x00036F8C
		public static ItemTemplateInfo FindGoldItemTemplate(int templateId, bool IsGold)
		{
			bool flag = !IsGold;
			ItemTemplateInfo result;
			if (flag)
			{
				result = null;
			}
			else
			{
				GoldEquipTemplateInfo goldEquip = GoldEquipMgr.FindGoldEquipByTemplate(templateId);
				bool flag2 = goldEquip == null;
				if (flag2)
				{
					result = null;
				}
				else
				{
					bool flag3 = ItemMgr._items.Keys.Contains(goldEquip.NewTemplateId);
					if (flag3)
					{
						result = ItemMgr._items[goldEquip.NewTemplateId];
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x04000160 RID: 352
		private static Dictionary<int, ItemTemplateInfo> _items;

		// Token: 0x04000161 RID: 353
		private static Dictionary<int, LoadUserBoxInfo> _timeBoxs;

		// Token: 0x04000162 RID: 354
		private static List<ItemTemplateInfo> Lists;

		// Token: 0x04000163 RID: 355
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000164 RID: 356
		private static ReaderWriterLock m_lock;
	}
}

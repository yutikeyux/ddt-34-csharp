using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x0200003A RID: 58
	public class ItemBoxMgr
	{
		// Token: 0x06000303 RID: 771 RVA: 0x00037348 File Offset: 0x00035548
		public static bool ReLoad()
		{
			try
			{
				ItemBoxInfo[] tempItemBox = ItemBoxMgr.LoadItemBoxDb();
				Dictionary<int, List<ItemBoxInfo>> tempItemBoxs = ItemBoxMgr.LoadItemBoxs(tempItemBox);
				bool flag = tempItemBox != null;
				if (flag)
				{
					Interlocked.Exchange<ItemBoxInfo[]>(ref ItemBoxMgr.m_itemBox, tempItemBox);
					Interlocked.Exchange<Dictionary<int, List<ItemBoxInfo>>>(ref ItemBoxMgr.m_itemBoxs, tempItemBoxs);
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = ItemBoxMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					ItemBoxMgr.log.Error("ReLoad", e);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000373CC File Offset: 0x000355CC
		public static bool Init()
		{
			return ItemBoxMgr.ReLoad();
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000373E4 File Offset: 0x000355E4
		public static ItemBoxInfo[] LoadItemBoxDb()
		{
			ItemBoxInfo[] itemBoxInfos;
			using (ProduceBussiness db = new ProduceBussiness())
			{
				itemBoxInfos = db.GetItemBoxInfos();
			}
			return itemBoxInfos;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00037420 File Offset: 0x00035620
		public static Dictionary<int, List<ItemBoxInfo>> LoadItemBoxs(ItemBoxInfo[] itemBoxs)
		{
			Dictionary<int, List<ItemBoxInfo>> infos = new Dictionary<int, List<ItemBoxInfo>>();
			for (int i = 0; i < itemBoxs.Length; i++)
			{
				ItemBoxInfo info = itemBoxs[i];
				bool flag = !infos.Keys.Contains(info.ID);
				if (flag)
				{
					IEnumerable<ItemBoxInfo> temp = from s in itemBoxs
					where s.ID == info.ID
					select s;
					infos.Add(info.ID, temp.ToList<ItemBoxInfo>());
				}
			}
			return infos;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x000374AC File Offset: 0x000356AC
		public static List<ItemBoxInfo> FindItemBox(int DataId)
		{
			bool flag = ItemBoxMgr.m_itemBoxs.ContainsKey(DataId);
			List<ItemBoxInfo> result;
			if (flag)
			{
				result = ItemBoxMgr.m_itemBoxs[DataId];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x000374E0 File Offset: 0x000356E0
		public static List<ItemInfo> GetAllItemBoxAward(int DataId)
		{
			List<ItemBoxInfo> list = ItemBoxMgr.FindItemBox(DataId);
			List<ItemInfo> infos = new List<ItemInfo>();
			foreach (ItemBoxInfo info in list)
			{
				ItemInfo item = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info.TemplateId), info.ItemCount, 105);
				item.IsBinds = info.IsBind;
				item.ValidDate = info.ItemValid;
				infos.Add(item);
			}
			return infos;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00037580 File Offset: 0x00035780
		public static bool CreateItemBox(int DateId, List<ItemInfo> itemInfos, SpecialItemDataInfo specialInfo)
		{
			return ItemBoxMgr.CreateItemBox(DateId, null, itemInfos, specialInfo);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0003759C File Offset: 0x0003579C
		public static bool CreateItemBox(int DateId, List<ItemBoxInfo> tempBox, List<ItemInfo> itemInfos, SpecialItemDataInfo specialValue)
		{
			new List<ItemBoxInfo>();
			List<ItemBoxInfo> source = ItemBoxMgr.FindItemBox(DateId);
			bool flag = tempBox != null && tempBox.Count > 0;
			if (flag)
			{
				source = tempBox;
			}
			bool flag2 = source == null;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				List<ItemBoxInfo> filtInfos = (from s in source
				where s.IsSelect
				select s).ToList<ItemBoxInfo>();
				int num = 1;
				int maxRound = 0;
				bool flag3 = filtInfos.Count < source.Count;
				if (flag3)
				{
					maxRound = ThreadSafeRandom.NextStatic((from s in source
					where !s.IsSelect
					select s.Random).Max());
					bool flag4 = maxRound <= 0;
					if (flag4)
					{
						ItemBoxMgr.log.Error("ItemBoxMgr Random Error: " + maxRound.ToString() + " | " + DateId.ToString());
						maxRound = (from s in source
						where !s.IsSelect
						select s.Random).Max();
					}
				}
				List<ItemBoxInfo> list = (from s in source
				where !s.IsSelect && s.Random >= maxRound
				select s).ToList<ItemBoxInfo>();
				int num2 = list.Count<ItemBoxInfo>();
				bool flag5 = num2 > 0;
				if (flag5)
				{
					int count = (num > num2) ? num2 : num;
					int[] randomUnrepeatArray = ItemBoxMgr.GetRandomUnrepeatArray(0, num2 - 1, count);
					int[] array = randomUnrepeatArray;
					foreach (int randomUnrepeat in array)
					{
						ItemBoxInfo itemBoxInfo2 = list[randomUnrepeat];
						bool flag6 = filtInfos == null;
						if (flag6)
						{
							filtInfos = new List<ItemBoxInfo>();
						}
						filtInfos.Add(itemBoxInfo2);
					}
				}
				foreach (ItemBoxInfo info in filtInfos)
				{
					bool flag7 = info == null;
					if (flag7)
					{
						return false;
					}
					int templateId = info.TemplateId;
					int num3 = templateId;
					if (num3 <= -300)
					{
						if (num3 == -800)
						{
							specialValue.Honor += info.ItemCount;
							continue;
						}
						if (num3 == -300)
						{
							specialValue.GiftToken += info.ItemCount;
							continue;
						}
					}
					else
					{
						if (num3 == -200)
						{
							specialValue.Money += info.ItemCount;
							continue;
						}
						if (num3 == -100)
						{
							specialValue.Gold += info.ItemCount;
							continue;
						}
						if (num3 == 11107)
						{
							specialValue.GP += info.ItemCount;
							continue;
						}
					}
					ItemInfo item = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info.TemplateId), info.ItemCount, 101);
					bool flag8 = item != null;
					if (flag8)
					{
						item.IsBinds = info.IsBind;
						item.ValidDate = info.ItemValid;
						item.StrengthenLevel = info.StrengthenLevel;
						item.AttackCompose = info.AttackCompose;
						item.DefendCompose = info.DefendCompose;
						item.AgilityCompose = info.AgilityCompose;
						item.LuckCompose = info.LuckCompose;
						bool flag9 = itemInfos == null;
						if (flag9)
						{
							itemInfos = new List<ItemInfo>();
						}
						itemInfos.Add(item);
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x000379AC File Offset: 0x00035BAC
		public static bool CreateItemBox(int DateId, List<ItemInfo> itemInfos, ref int gold, ref int point, ref int giftToken, ref int medal, ref int exp, ref int hardCurrency, ref int leagueMoney, ref int useableScore, ref int prestge, ref int honor)
		{
			List<ItemBoxInfo> FiltInfos = new List<ItemBoxInfo>();
			List<ItemBoxInfo> unFiltInfos = ItemBoxMgr.FindItemBox(DateId);
			bool flag = unFiltInfos == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				FiltInfos = (from s in unFiltInfos
				where s.IsSelect
				select s).ToList<ItemBoxInfo>();
				int dropItemCount = 1;
				int maxRound = 0;
				bool flag2 = FiltInfos.Count < unFiltInfos.Count;
				if (flag2)
				{
					maxRound = ThreadSafeRandom.NextStatic((from s in unFiltInfos
					where !s.IsSelect
					select s.Random).Max());
				}
				List<ItemBoxInfo> RoundInfos = (from s in unFiltInfos
				where !s.IsSelect && s.Random >= maxRound
				select s).ToList<ItemBoxInfo>();
				int maxItems = RoundInfos.Count<ItemBoxInfo>();
				bool flag3 = maxItems > 0;
				if (flag3)
				{
					dropItemCount = ((dropItemCount > maxItems) ? maxItems : dropItemCount);
					int[] randomArray = ItemBoxMgr.GetRandomUnrepeatArray(0, maxItems - 1, dropItemCount);
					int[] array = randomArray;
					foreach (int i in array)
					{
						ItemBoxInfo item = RoundInfos[i];
						bool flag4 = FiltInfos == null;
						if (flag4)
						{
							FiltInfos = new List<ItemBoxInfo>();
						}
						FiltInfos.Add(item);
					}
				}
				foreach (ItemBoxInfo info in FiltInfos)
				{
					bool flag5 = info == null;
					if (flag5)
					{
						return false;
					}
					int templateId = info.TemplateId;
					int num = templateId;
					if (num <= -900)
					{
						if (num <= -1200)
						{
							if (num == -1300)
							{
								prestge += info.ItemCount;
								continue;
							}
							if (num == -1200)
							{
								useableScore += info.ItemCount;
								continue;
							}
						}
						else
						{
							if (num == -1100)
							{
								giftToken += info.ItemCount;
								continue;
							}
							if (num == -1000)
							{
								leagueMoney += info.ItemCount;
								continue;
							}
							if (num == -900)
							{
								hardCurrency += info.ItemCount;
								continue;
							}
						}
					}
					else if (num <= -300)
					{
						if (num == -800)
						{
							honor += info.ItemCount;
							continue;
						}
						if (num == -300)
						{
							medal += info.ItemCount;
							continue;
						}
					}
					else
					{
						if (num == -200)
						{
							point += info.ItemCount;
							continue;
						}
						if (num == -100)
						{
							gold += info.ItemCount;
							continue;
						}
						if (num == 11107)
						{
							exp += info.ItemCount;
							continue;
						}
					}
					ItemTemplateInfo temp = ItemMgr.FindItemTemplate(info.TemplateId);
					ItemInfo item2 = ItemInfo.CreateFromTemplate(temp, info.ItemCount, 101);
					bool flag6 = item2 != null;
					if (flag6)
					{
						item2.Count = info.ItemCount;
						item2.IsBinds = info.IsBind;
						item2.ValidDate = info.ItemValid;
						item2.StrengthenLevel = info.StrengthenLevel;
						item2.AttackCompose = info.AttackCompose;
						item2.DefendCompose = info.DefendCompose;
						item2.AgilityCompose = info.AgilityCompose;
						item2.LuckCompose = info.LuckCompose;
						item2.IsTips = (info.IsTips != 0);
						item2.IsLogs = info.IsLogs;
						bool flag7 = itemInfos == null;
						if (flag7)
						{
							itemInfos = new List<ItemInfo>();
						}
						itemInfos.Add(item2);
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00037DE0 File Offset: 0x00035FE0
		public static bool CreateItemBox(int DateId, List<ItemInfo> itemInfos, ref int gold, ref int point, ref int giftToken, ref int medal, ref int exp, ref int honor)
		{
			List<ItemBoxInfo> FiltInfos = new List<ItemBoxInfo>();
			List<ItemBoxInfo> unFiltInfos = ItemBoxMgr.FindItemBox(DateId);
			bool flag = unFiltInfos == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				FiltInfos = (from s in unFiltInfos
				where s.IsSelect
				select s).ToList<ItemBoxInfo>();
				int dropItemCount = 1;
				int maxRound = 0;
				bool flag2 = FiltInfos.Count < unFiltInfos.Count;
				if (flag2)
				{
					maxRound = ThreadSafeRandom.NextStatic((from s in unFiltInfos
					where !s.IsSelect
					select s.Random).Max());
				}
				List<ItemBoxInfo> RoundInfos = (from s in unFiltInfos
				where !s.IsSelect && s.Random >= maxRound
				select s).ToList<ItemBoxInfo>();
				int maxItems = RoundInfos.Count<ItemBoxInfo>();
				bool flag3 = maxItems > 0;
				if (flag3)
				{
					dropItemCount = ((dropItemCount > maxItems) ? maxItems : dropItemCount);
					int[] array = ItemBoxMgr.GetRandomUnrepeatArray(0, maxItems - 1, dropItemCount);
					int[] array2 = array;
					foreach (int i in array2)
					{
						ItemBoxInfo item = RoundInfos[i];
						bool flag4 = FiltInfos == null;
						if (flag4)
						{
							FiltInfos = new List<ItemBoxInfo>();
						}
						FiltInfos.Add(item);
					}
				}
				foreach (ItemBoxInfo info in FiltInfos)
				{
					bool flag5 = info == null;
					if (flag5)
					{
						return false;
					}
					int templateId = info.TemplateId;
					int num = templateId;
					if (num <= -300)
					{
						if (num == -1100)
						{
							giftToken += info.ItemCount;
							continue;
						}
						if (num == -800)
						{
							honor += info.ItemCount;
							continue;
						}
						if (num == -300)
						{
							medal += info.ItemCount;
							continue;
						}
					}
					else
					{
						if (num == -200)
						{
							point += info.ItemCount;
							continue;
						}
						if (num == -100)
						{
							gold += info.ItemCount;
							continue;
						}
						if (num == 11107)
						{
							exp += info.ItemCount;
							continue;
						}
					}
					ItemInfo item2 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info.TemplateId), info.ItemCount, 101);
					bool flag6 = item2 != null;
					if (flag6)
					{
						item2.Count = info.ItemCount;
						item2.IsBinds = info.IsBind;
						item2.ValidDate = info.ItemValid;
						item2.StrengthenLevel = info.StrengthenLevel;
						item2.AttackCompose = info.AttackCompose;
						item2.DefendCompose = info.DefendCompose;
						item2.AgilityCompose = info.AgilityCompose;
						item2.LuckCompose = info.LuckCompose;
						item2.IsTips = (info.IsTips != 0);
						item2.IsLogs = info.IsLogs;
						bool flag7 = itemInfos == null;
						if (flag7)
						{
							itemInfos = new List<ItemInfo>();
						}
						itemInfos.Add(item2);
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00038160 File Offset: 0x00036360
		public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
		{
			int[] resultRound = new int[count];
			for (int i = 0; i < count; i++)
			{
				int j = ItemBoxMgr.random.Next(minValue, maxValue + 1);
				int num = 0;
				for (int k = 0; k < i; k++)
				{
					bool flag = resultRound[k] == j;
					if (flag)
					{
						num++;
					}
				}
				bool flag2 = num == 0;
				if (flag2)
				{
					resultRound[i] = j;
				}
				else
				{
					i--;
				}
			}
			return resultRound;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x000381E4 File Offset: 0x000363E4
		public static bool CreateItemBox(int DateId, List<ItemInfo> itemInfos, ref int gold, ref int point, ref int giftToken, ref int exp, ref int honor)
		{
			return ItemBoxMgr.CreateItemBox(DateId, null, itemInfos, ref gold, ref point, ref giftToken, ref exp, ref honor);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00038208 File Offset: 0x00036408
		public static bool CreateItemBox(int DateId, List<ItemBoxInfo> tempBox, List<ItemInfo> itemInfos, ref int gold, ref int point, ref int giftToken, ref int exp, ref int honor)
		{
			List<ItemBoxInfo> list = new List<ItemBoxInfo>();
			List<ItemBoxInfo> list2 = ItemBoxMgr.FindItemBox(DateId);
			bool flag = tempBox != null && tempBox.Count > 0;
			if (flag)
			{
				list2 = tempBox;
			}
			bool flag2 = list2 == null;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				list = (from s in list2
				where s.IsSelect
				select s).ToList<ItemBoxInfo>();
				int count = 1;
				int maxRound = 0;
				bool flag3 = list.Count < list2.Count;
				if (flag3)
				{
					maxRound = ThreadSafeRandom.NextStatic((from s in list2
					where !s.IsSelect
					select s.Random).Max());
				}
				List<ItemBoxInfo> source = (from s in list2
				where !s.IsSelect && s.Random >= maxRound
				select s).ToList<ItemBoxInfo>();
				int num2 = source.Count<ItemBoxInfo>();
				bool flag4 = num2 > 0;
				if (flag4)
				{
					count = ((count > num2) ? num2 : count);
					int[] numArray = ItemBoxMgr.GetRandomUnrepeatArray(0, num2 - 1, count);
					int[] array = numArray;
					foreach (int num3 in array)
					{
						ItemBoxInfo item = source[num3];
						bool flag5 = list == null;
						if (flag5)
						{
							list = new List<ItemBoxInfo>();
						}
						list.Add(item);
					}
				}
				foreach (ItemBoxInfo info2 in list)
				{
					bool flag6 = info2 == null;
					if (flag6)
					{
						return false;
					}
					int templateId = info2.TemplateId;
					int num4 = templateId;
					if (num4 <= -300)
					{
						if (num4 == -800)
						{
							honor += info2.ItemCount;
							continue;
						}
						if (num4 == -300)
						{
							giftToken += info2.ItemCount;
							continue;
						}
					}
					else
					{
						if (num4 == -200)
						{
							point += info2.ItemCount;
							continue;
						}
						if (num4 == -100)
						{
							gold += info2.ItemCount;
							continue;
						}
						if (num4 == 11107)
						{
							exp += info2.ItemCount;
							continue;
						}
					}
					ItemInfo info3 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info2.TemplateId), info2.ItemCount, 101);
					bool flag7 = info3 != null;
					if (flag7)
					{
						info3.IsBinds = info2.IsBind;
						info3.ValidDate = info2.ItemValid;
						info3.StrengthenLevel = info2.StrengthenLevel;
						info3.AttackCompose = info2.AttackCompose;
						info3.DefendCompose = info2.DefendCompose;
						info3.AgilityCompose = info2.AgilityCompose;
						info3.LuckCompose = info2.LuckCompose;
						bool flag8 = itemInfos == null;
						if (flag8)
						{
							itemInfos = new List<ItemInfo>();
						}
						itemInfos.Add(info3);
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00038550 File Offset: 0x00036750
		public static List<ItemBoxInfo> FindLotteryItemBoxByRand(int DateId, int countSelect)
		{
			List<ItemBoxInfo> list = ItemBoxMgr.FindLotteryItemBox(DateId);
			List<ItemBoxInfo> list2 = new List<ItemBoxInfo>();
			for (int i = 0; i < countSelect; i++)
			{
				int num2 = ThreadSafeRandom.NextStatic(0, list.Count);
				bool flag = num2 < list.Count;
				if (flag)
				{
					list2.Add(list[num2]);
					list.Remove(list[num2]);
				}
			}
			return list2;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000385C0 File Offset: 0x000367C0
		public static List<ItemBoxInfo> FindLotteryItemBox(int DataId)
		{
			bool flag2 = !ItemBoxMgr.m_itemBoxs.ContainsKey(DataId);
			List<ItemBoxInfo> result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				List<ItemBoxInfo> list = new List<ItemBoxInfo>();
				foreach (ItemBoxInfo current in ItemBoxMgr.m_itemBoxs[DataId])
				{
					bool flag = true;
					foreach (ItemBoxInfo info2 in list)
					{
						bool flag3 = info2.TemplateId == current.TemplateId && info2.ItemCount == current.ItemCount;
						if (flag3)
						{
							flag = false;
							break;
						}
					}
					bool flag4 = flag;
					if (flag4)
					{
						list.Add(current);
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x0400015C RID: 348
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400015D RID: 349
		private static ItemBoxInfo[] m_itemBox;

		// Token: 0x0400015E RID: 350
		private static Dictionary<int, List<ItemBoxInfo>> m_itemBoxs;

		// Token: 0x0400015F RID: 351
		private static ThreadSafeRandom random = new ThreadSafeRandom();
	}
}

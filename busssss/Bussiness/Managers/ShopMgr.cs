using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000041 RID: 65
	public static class ShopMgr
	{
		// Token: 0x06000358 RID: 856 RVA: 0x0003A11C File Offset: 0x0003831C
		public static bool CanBuy(int shopID, int consortiaShopLevel, ref bool isBinds, int cousortiaID, int playerRiches)
		{
			bool flag = false;
			bool result;
			using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
			{
				if (shopID <= 72)
				{
					switch (shopID)
					{
					case 1:
						flag = true;
						isBinds = false;
						return flag;
					case 2:
						flag = true;
						isBinds = false;
						return flag;
					case 3:
						flag = true;
						isBinds = false;
						return flag;
					case 4:
						flag = true;
						isBinds = false;
						return flag;
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
						return flag;
					case 11:
					{
						ConsortiaEquipControlInfo consortiaEuqipRiches = consortiaBussiness.GetConsortiaEquipRiches(cousortiaID, 1, 1);
						bool flag2 = consortiaShopLevel >= consortiaEuqipRiches.Level;
						if (!flag2)
						{
							return flag;
						}
						bool flag3 = playerRiches >= consortiaEuqipRiches.Riches;
						if (flag3)
						{
							flag = true;
							isBinds = true;
							return flag;
						}
						return flag;
					}
					case 12:
					{
						ConsortiaEquipControlInfo consortiaEuqipRiches2 = consortiaBussiness.GetConsortiaEquipRiches(cousortiaID, 2, 1);
						bool flag4 = consortiaShopLevel >= consortiaEuqipRiches2.Level;
						if (!flag4)
						{
							return flag;
						}
						bool flag5 = playerRiches >= consortiaEuqipRiches2.Riches;
						if (flag5)
						{
							flag = true;
							isBinds = true;
							return flag;
						}
						return flag;
					}
					case 13:
					{
						ConsortiaEquipControlInfo consortiaEuqipRiches3 = consortiaBussiness.GetConsortiaEquipRiches(cousortiaID, 3, 1);
						bool flag6 = consortiaShopLevel >= consortiaEuqipRiches3.Level;
						if (!flag6)
						{
							return flag;
						}
						bool flag7 = playerRiches >= consortiaEuqipRiches3.Riches;
						if (flag7)
						{
							flag = true;
							isBinds = true;
							return flag;
						}
						return flag;
					}
					case 14:
					{
						ConsortiaEquipControlInfo consortiaEuqipRiches4 = consortiaBussiness.GetConsortiaEquipRiches(cousortiaID, 4, 1);
						bool flag8 = consortiaShopLevel >= consortiaEuqipRiches4.Level;
						if (!flag8)
						{
							return flag;
						}
						bool flag9 = playerRiches >= consortiaEuqipRiches4.Riches;
						if (flag9)
						{
							flag = true;
							isBinds = true;
							return flag;
						}
						return flag;
					}
					case 15:
					{
						ConsortiaEquipControlInfo consortiaEuqipRiches5 = consortiaBussiness.GetConsortiaEquipRiches(cousortiaID, 5, 1);
						bool flag10 = consortiaShopLevel >= consortiaEuqipRiches5.Level;
						if (!flag10)
						{
							return flag;
						}
						bool flag11 = playerRiches >= consortiaEuqipRiches5.Riches;
						if (flag11)
						{
							flag = true;
							isBinds = true;
							return flag;
						}
						return flag;
					}
					case 16:
					case 17:
					case 18:
					case 19:
						goto IL_25B;
					case 20:
						break;
					default:
						if (shopID != 72)
						{
							goto IL_25B;
						}
						break;
					}
				}
				else if (shopID != 91 && shopID != 110)
				{
					goto IL_25B;
				}
				flag = true;
				isBinds = true;
				return flag;
				IL_25B:
				result = flag;
			}
			return result;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0003A3B4 File Offset: 0x000385B4
		public static bool CheckInShopGoodsCanBuy(int iTemplateID)
		{
			return ShopMgr.m_ShopGoodsCanBuy.ContainsKey(iTemplateID);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0003A3D4 File Offset: 0x000385D4
		public static int FindItemTemplateID(int id)
		{
			bool flag = ShopMgr.m_shop.ContainsKey(id);
			int result;
			if (flag)
			{
				result = ShopMgr.m_shop[id].TemplateID;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0003A40C File Offset: 0x0003860C
		public static ShopItemInfo FindShopbyTemplateID(int TemplatID)
		{
			foreach (ShopItemInfo current in ShopMgr.m_shop.Values)
			{
				bool flag = current.TemplateID == TemplatID;
				if (flag)
				{
					return current;
				}
			}
			return null;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0003A478 File Offset: 0x00038678
		public static ShopItemInfo FindShopbyID(int ID)
		{
			foreach (ShopItemInfo info in ShopMgr.m_shop.Values)
			{
				bool flag = info.ID == ID;
				if (flag)
				{
					return info;
				}
			}
			return null;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0003A4E4 File Offset: 0x000386E4
		public static List<ShopItemInfo> FindShopbyTemplatID(int TemplatID)
		{
			List<ShopItemInfo> list = new List<ShopItemInfo>();
			foreach (ShopItemInfo info in ShopMgr.m_shop.Values)
			{
				bool flag = info.TemplateID == TemplatID;
				if (flag)
				{
					list.Add(info);
				}
			}
			return list;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0003A560 File Offset: 0x00038760
		public static void FindSpecialItemInfo(ItemInfo info, ref int gold, ref int money, ref int giftToken, ref int medal, ref int honor, ref int hardCurrency, ref int token, ref int dragonToken, ref int magicStonePoint)
		{
			int templateID = info.TemplateID;
			int num = templateID;
			if (num <= -1000)
			{
				if (num <= -1200)
				{
					if (num != -1400)
					{
						if (num == -1200)
						{
							dragonToken += info.Count;
							info = null;
						}
					}
					else
					{
						magicStonePoint += info.Count;
						info = null;
					}
				}
				else if (num != -1100)
				{
					if (num == -1000)
					{
						token += info.Count;
						info = null;
					}
				}
				else
				{
					giftToken += info.Count;
					info = null;
				}
			}
			else if (num <= -800)
			{
				if (num != -900)
				{
					if (num == -800)
					{
						honor += info.Count;
						info = null;
					}
				}
				else
				{
					hardCurrency += info.Count;
					info = null;
				}
			}
			else if (num != -300)
			{
				if (num != -200)
				{
					if (num == -100)
					{
						gold += info.Count;
						info = null;
					}
				}
				else
				{
					money += info.Count;
					info = null;
				}
			}
			else
			{
				medal += info.Count;
				info = null;
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0003A6AC File Offset: 0x000388AC
		public static void GetItemPrice(int Prices, int Values, decimal beat, ref int damageScore, ref int petScore, ref int iTemplateID, ref int iCount, ref int gold, ref int money, ref int offer, ref int gifttoken, ref int medal, ref int hardCurrency, ref int LeagueMoney, ref int useableScore, ref int honor)
		{
			if (Prices <= -900)
			{
				if (Prices == -1200)
				{
					useableScore += (int)(Values * beat);
					return;
				}
				if (Prices == -1000)
				{
					LeagueMoney += (int)(Values * beat);
					return;
				}
				if (Prices == -900)
				{
					hardCurrency += (int)(Values * beat);
					return;
				}
			}
			else
			{
				if (Prices == -800)
				{
					honor += (int)(Values * beat);
					return;
				}
				if (Prices == -300)
				{
					medal += (int)(Values * beat);
					return;
				}
				switch (Prices)
				{
				case -8:
					petScore += (int)(Values * beat);
					return;
				case -7:
					damageScore += (int)(Values * beat);
					return;
				case -6:
					medal += (int)(Values * beat);
					return;
				case -4:
					gifttoken += (int)(Values * beat);
					return;
				case -3:
					gold += (int)(Values * beat);
					return;
				case -2:
					offer += (int)(Values * beat);
					return;
				case -1:
					money += (int)(Values * beat);
					return;
				}
			}
			bool flag = Prices > 0;
			if (flag)
			{
				iTemplateID = Prices;
				iCount = Values;
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0003A890 File Offset: 0x00038A90
		public static ShopItemInfo GetShopItemInfoById(int ID)
		{
			bool flag = ShopMgr.m_shop.ContainsKey(ID);
			ShopItemInfo result;
			if (flag)
			{
				result = ShopMgr.m_shop[ID];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0003A8C4 File Offset: 0x00038AC4
		public static bool Init()
		{
			return ShopMgr.ReLoad();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0003A8DC File Offset: 0x00038ADC
		public static bool IsOnShop(int Id)
		{
			bool flag = ShopMgr.m_shopGoodsShowLists == null;
			if (flag)
			{
				ShopMgr.Init();
			}
			bool flag2 = ShopMgr.IsSpecialItem(Id);
			bool result;
			if (flag2)
			{
				result = true;
			}
			else
			{
				ShopMgr.m_lock.AcquireReaderLock(10000);
				try
				{
					bool flag3 = ShopMgr.m_shopGoodsShowLists.Keys.Contains(Id);
					if (flag3)
					{
						return true;
					}
				}
				finally
				{
					ShopMgr.m_lock.ReleaseReaderLock();
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0003A960 File Offset: 0x00038B60
		public static bool IsSpecialItem(int Id)
		{
			bool flag = Id <= 1100801;
			if (flag)
			{
				bool flag2 = Id != 1100401 && Id != 1100801;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = Id != 1101201 && Id != 1101601;
				if (flag3)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0003A9C4 File Offset: 0x00038BC4
		private static Dictionary<int, ShopItemInfo> LoadFromDatabase()
		{
			Dictionary<int, ShopItemInfo> dictionary = new Dictionary<int, ShopItemInfo>();
			Dictionary<int, ShopItemInfo> result;
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				ShopItemInfo[] aLllShop = bussiness.GetALllShop();
				ShopItemInfo[] array = aLllShop;
				foreach (ShopItemInfo info in array)
				{
					bool flag = !dictionary.ContainsKey(info.ID);
					if (flag)
					{
						dictionary.Add(info.ID, info);
					}
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0003AA54 File Offset: 0x00038C54
		private static Dictionary<int, ShopGoodsShowListInfo> LoadShopGoodsCanBuyFromDatabase()
		{
			Dictionary<int, ShopGoodsShowListInfo> dictionary = new Dictionary<int, ShopGoodsShowListInfo>();
			Dictionary<int, ShopGoodsShowListInfo> result;
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				ShopGoodsShowListInfo[] allShopGoodsShowList = bussiness.GetAllShopGoodsShowList();
				ShopGoodsShowListInfo[] array = allShopGoodsShowList;
				foreach (ShopGoodsShowListInfo info in array)
				{
					bool flag = !dictionary.ContainsKey(info.ShopId);
					if (flag)
					{
						dictionary.Add(info.ShopId, info);
					}
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0003AAE4 File Offset: 0x00038CE4
		private static Dictionary<int, ShopGoodsShowListInfo> LoadShowListFromDatabase()
		{
			Dictionary<int, ShopGoodsShowListInfo> dictionary = new Dictionary<int, ShopGoodsShowListInfo>();
			Dictionary<int, ShopGoodsShowListInfo> result;
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				ShopGoodsShowListInfo[] allShopGoodsShowList = bussiness.GetAllShopGoodsShowList();
				ShopGoodsShowListInfo[] array = allShopGoodsShowList;
				foreach (ShopGoodsShowListInfo info in array)
				{
					bool flag = !dictionary.ContainsKey(info.ShopId);
					if (flag)
					{
						dictionary.Add(info.ShopId, info);
					}
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0003AB74 File Offset: 0x00038D74
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, ShopItemInfo> dictionary = ShopMgr.LoadFromDatabase();
				Dictionary<int, ShopGoodsShowListInfo> dictionary2 = ShopMgr.LoadShowListFromDatabase();
				bool flag = dictionary.Count > 0;
				if (flag)
				{
					Interlocked.Exchange<Dictionary<int, ShopItemInfo>>(ref ShopMgr.m_shop, dictionary);
				}
				bool flag2 = dictionary2.Count > 0;
				if (flag2)
				{
					Interlocked.Exchange<Dictionary<int, ShopGoodsShowListInfo>>(ref ShopMgr.m_shopGoodsShowLists, dictionary2);
				}
				try
				{
					Dictionary<int, ShopGoodsShowListInfo> dictionary3 = ShopMgr.LoadShopGoodsCanBuyFromDatabase();
					bool flag3 = dictionary3.Count > 0;
					if (flag3)
					{
						Interlocked.Exchange<Dictionary<int, ShopGoodsShowListInfo>>(ref ShopMgr.m_ShopGoodsCanBuy, dictionary3);
					}
				}
				catch (Exception exception3)
				{
					ShopMgr.log.Error("ShopInfoMgr", exception3);
				}
				return true;
			}
			catch (Exception exception4)
			{
				ShopMgr.log.Error("ShopInfoMgr", exception4);
			}
			return false;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0003AC48 File Offset: 0x00038E48
		public static bool SetItemType(ShopItemInfo shop, int type, ref int damageScore, ref int petScore, ref int iTemplateID, ref int iCount, ref int gold, ref int money, ref int offer, ref int gifttoken, ref int medal, ref int hardCurrency, ref int LeagueMoney, ref int useableScore, ref int honor)
		{
			bool flag = type == 1;
			if (flag)
			{
				ShopMgr.GetItemPrice(shop.APrice1, shop.AValue1, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore, ref honor);
				ShopMgr.GetItemPrice(shop.APrice2, shop.AValue2, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore, ref honor);
				ShopMgr.GetItemPrice(shop.APrice3, shop.AValue3, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore, ref honor);
			}
			bool flag2 = type == 2;
			if (flag2)
			{
				ShopMgr.GetItemPrice(shop.BPrice1, shop.BValue1, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore, ref honor);
				ShopMgr.GetItemPrice(shop.BPrice2, shop.BValue2, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore, ref honor);
				ShopMgr.GetItemPrice(shop.BPrice3, shop.BValue3, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore, ref honor);
			}
			bool flag3 = type == 3;
			if (flag3)
			{
				ShopMgr.GetItemPrice(shop.CPrice1, shop.CValue1, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore, ref honor);
				ShopMgr.GetItemPrice(shop.CPrice2, shop.CValue2, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore, ref honor);
				ShopMgr.GetItemPrice(shop.CPrice3, shop.CValue3, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore, ref honor);
			}
			return true;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0003AE34 File Offset: 0x00039034
		public static ItemInfo CreateItem(ShopItemInfo shopItem, int addtype, int valuetype, string color, string skin, bool isBinding)
		{
			bool flag = shopItem != null;
			ItemInfo result;
			if (flag)
			{
				ItemTemplateInfo template = ItemMgr.FindItemTemplate(shopItem.TemplateID);
				ItemInfo item = ItemInfo.CreateFromTemplate(template, 1, addtype);
				bool flag2 = shopItem.BuyType == 0;
				if (flag2)
				{
					bool flag3 = 1 == valuetype;
					if (flag3)
					{
						item.ValidDate = shopItem.AUnit;
					}
					bool flag4 = 2 == valuetype;
					if (flag4)
					{
						item.ValidDate = shopItem.BUnit;
					}
					bool flag5 = 3 == valuetype;
					if (flag5)
					{
						item.ValidDate = shopItem.CUnit;
					}
				}
				else
				{
					bool flag6 = 1 == valuetype;
					if (flag6)
					{
						item.Count = shopItem.AUnit;
					}
					bool flag7 = 2 == valuetype;
					if (flag7)
					{
						item.Count = shopItem.BUnit;
					}
					bool flag8 = 3 == valuetype;
					if (flag8)
					{
						item.Count = shopItem.CUnit;
					}
				}
				item.Color = (color ?? "");
				item.Skin = (skin ?? "");
				if (isBinding)
				{
					item.IsBinds = true;
				}
				else
				{
					item.IsBinds = Convert.ToBoolean(shopItem.IsBind);
				}
				result = item;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0400017B RID: 379
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400017C RID: 380
		private static ReaderWriterLock m_lock = new ReaderWriterLock();

		// Token: 0x0400017D RID: 381
		private static Dictionary<int, ShopItemInfo> m_shop = new Dictionary<int, ShopItemInfo>();

		// Token: 0x0400017E RID: 382
		private static Dictionary<int, ShopGoodsShowListInfo> m_ShopGoodsCanBuy = new Dictionary<int, ShopGoodsShowListInfo>();

		// Token: 0x0400017F RID: 383
		private static Dictionary<int, ShopGoodsShowListInfo> m_shopGoodsShowLists = new Dictionary<int, ShopGoodsShowListInfo>();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000045 RID: 69
	public class WorldEventMgr
	{
		// Token: 0x06000380 RID: 896 RVA: 0x0003B99C File Offset: 0x00039B9C
		public static bool SendItemsToMail(List<ItemInfo> infos, int PlayerId, string Nickname, string title, string content = null)
		{
			bool flag = false;
			bool result;
			using (PlayerBussiness bussiness = new PlayerBussiness())
			{
				List<ItemInfo> list = new List<ItemInfo>();
				foreach (ItemInfo info in infos)
				{
					bool flag2 = info.Template.MaxCount == 1;
					if (flag2)
					{
						for (int i = 0; i < info.Count; i++)
						{
							ItemInfo item = ItemInfo.CloneFromTemplate(info.Template, info);
							item.Count = 1;
							list.Add(item);
						}
					}
					else
					{
						list.Add(info);
					}
				}
				for (int j = 0; j < list.Count; j += 5)
				{
					MailInfo mail = new MailInfo
					{
						Title = title,
						Content = content,
						Gold = 0,
						IsExist = true,
						Money = 0,
						Receiver = Nickname,
						ReceiverID = PlayerId,
						Sender = "Hệ Thống",
						SenderID = 0,
						Type = 9,
						GiftToken = 0
					};
					StringBuilder builder = new StringBuilder();
					StringBuilder builder2 = new StringBuilder();
					builder.Append(LanguageMgr.GetTranslation("Game.Server.GameUtils.CommonBag.AnnexRemark", Array.Empty<object>()));
					int num3 = j;
					bool flag3 = list.Count > num3;
					if (flag3)
					{
						ItemInfo info2 = list[num3];
						bool flag4 = info2.ItemID == 0;
						if (flag4)
						{
							bussiness.AddGoods(info2);
						}
						mail.Annex1 = info2.ItemID.ToString();
						mail.Annex1Name = info2.Template.Name;
						builder.Append(string.Concat(new string[]
						{
							"1、",
							mail.Annex1Name,
							"x",
							info2.Count.ToString(),
							";"
						}));
						builder2.Append(string.Concat(new string[]
						{
							"1、",
							mail.Annex1Name,
							"x",
							info2.Count.ToString(),
							";"
						}));
					}
					num3 = j + 1;
					bool flag5 = list.Count > num3;
					if (flag5)
					{
						ItemInfo info3 = list[num3];
						bool flag6 = info3.ItemID == 0;
						if (flag6)
						{
							bussiness.AddGoods(info3);
						}
						mail.Annex2 = info3.ItemID.ToString();
						mail.Annex2Name = info3.Template.Name;
						builder.Append(string.Concat(new string[]
						{
							"2、",
							mail.Annex2Name,
							"x",
							info3.Count.ToString(),
							";"
						}));
						builder2.Append(string.Concat(new string[]
						{
							"2、",
							mail.Annex2Name,
							"x",
							info3.Count.ToString(),
							";"
						}));
					}
					num3 = j + 2;
					bool flag7 = list.Count > num3;
					if (flag7)
					{
						ItemInfo info4 = list[num3];
						bool flag8 = info4.ItemID == 0;
						if (flag8)
						{
							bussiness.AddGoods(info4);
						}
						mail.Annex3 = info4.ItemID.ToString();
						mail.Annex3Name = info4.Template.Name;
						builder.Append(string.Concat(new string[]
						{
							"3、",
							mail.Annex3Name,
							"x",
							info4.Count.ToString(),
							";"
						}));
						builder2.Append(string.Concat(new string[]
						{
							"3、",
							mail.Annex3Name,
							"x",
							info4.Count.ToString(),
							";"
						}));
					}
					num3 = j + 3;
					bool flag9 = list.Count > num3;
					if (flag9)
					{
						ItemInfo info5 = list[num3];
						bool flag10 = info5.ItemID == 0;
						if (flag10)
						{
							bussiness.AddGoods(info5);
						}
						mail.Annex4 = info5.ItemID.ToString();
						mail.Annex4Name = info5.Template.Name;
						builder.Append(string.Concat(new string[]
						{
							"4、",
							mail.Annex4Name,
							"x",
							info5.Count.ToString(),
							";"
						}));
						builder2.Append(string.Concat(new string[]
						{
							"4、",
							mail.Annex4Name,
							"x",
							info5.Count.ToString(),
							";"
						}));
					}
					num3 = j + 4;
					bool flag11 = list.Count > num3;
					if (flag11)
					{
						ItemInfo info6 = list[num3];
						bool flag12 = info6.ItemID == 0;
						if (flag12)
						{
							bussiness.AddGoods(info6);
						}
						mail.Annex5 = info6.ItemID.ToString();
						mail.Annex5Name = info6.Template.Name;
						builder.Append(string.Concat(new string[]
						{
							"5、",
							mail.Annex5Name,
							"x",
							info6.Count.ToString(),
							";"
						}));
						builder2.Append(string.Concat(new string[]
						{
							"5、",
							mail.Annex5Name,
							"x",
							info6.Count.ToString(),
							";"
						}));
					}
					mail.AnnexRemark = builder.ToString();
					mail.Content = (mail.Content ?? builder2.ToString());
					flag = bussiness.SendMail(mail);
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0003C05C File Offset: 0x0003A25C
		public static bool SendItemToMail(ItemInfo info, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title)
		{
			return WorldEventMgr.SendItemsToMail(new List<ItemInfo>
			{
				info
			}, PlayerId, Nickname, title, null);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0003C088 File Offset: 0x0003A288
		public static bool SendItemsToMails(List<ItemInfo> infos, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title)
		{
			return WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title, null);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0003C0A8 File Offset: 0x0003A2A8
		public static bool SendItemsToMails(List<ItemInfo> infos, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title, string content)
		{
			return WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title, null);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0003C0C8 File Offset: 0x0003A2C8
		public static bool SendItemToMail(ItemInfo info, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title, string sender)
		{
			return WorldEventMgr.SendItemsToMail(new List<ItemInfo>
			{
				info
			}, PlayerId, Nickname, title, null);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0003C0F4 File Offset: 0x0003A2F4
		public static bool SendItemsToMail(List<ItemInfo> infos, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title, int type, string sender)
		{
			return WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title, null);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0003C114 File Offset: 0x0003A314
		public static bool SendItemsToMail(List<ItemInfo> infos, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title, string content)
		{
			return WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title, null);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0003C134 File Offset: 0x0003A334
		public static bool LoadData(Dictionary<int, LuckyStartToptenAwardInfo> luckyStarts)
		{
			using (ActiveBussiness db = new ActiveBussiness())
			{
				LuckyStartToptenAwardInfo[] luckyStartDbs = db.GetAllLuckyStartToptenAward();
				foreach (LuckyStartToptenAwardInfo award in luckyStartDbs)
				{
					bool flag = !luckyStarts.Keys.Contains(award.ID);
					if (flag)
					{
						luckyStarts.Add(award.ID, award);
					}
				}
			}
			return true;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0003C1BC File Offset: 0x0003A3BC
		public static List<LuckyStartToptenAwardInfo> GetLuckyStartToptenAward()
		{
			List<LuckyStartToptenAwardInfo> infos = new List<LuckyStartToptenAwardInfo>();
			foreach (LuckyStartToptenAwardInfo info in WorldEventMgr.m_luckyStartToptenAward.Values)
			{
				infos.Add(info);
			}
			return infos;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0003C224 File Offset: 0x0003A424
		public static List<LuckyStartToptenAwardInfo> GetLuckyStartAwardByRank(int rank)
		{
			int type = 0;
			switch (rank)
			{
			case 1:
				type = 11;
				break;
			case 2:
				type = 12;
				break;
			case 3:
				type = 13;
				break;
			case 4:
			case 5:
				type = 14;
				break;
			case 6:
			case 7:
				type = 15;
				break;
			case 8:
			case 9:
			case 10:
				type = 16;
				break;
			}
			List<LuckyStartToptenAwardInfo> infos = new List<LuckyStartToptenAwardInfo>();
			foreach (LuckyStartToptenAwardInfo info in WorldEventMgr.m_luckyStartToptenAward.Values)
			{
				bool flag = info.Type == type;
				if (flag)
				{
					infos.Add(info);
				}
			}
			return infos;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0003C2FC File Offset: 0x0003A4FC
		public static bool Init()
		{
			bool result;
			try
			{
				WorldEventMgr.m_luckyStartToptenAward = new Dictionary<int, LuckyStartToptenAwardInfo>();
				result = WorldEventMgr.LoadData(WorldEventMgr.m_luckyStartToptenAward);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = WorldEventMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					WorldEventMgr.log.Error("Init", e);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0003C358 File Offset: 0x0003A558
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, LuckyStartToptenAwardInfo> templuckyStartToptenAward = new Dictionary<int, LuckyStartToptenAwardInfo>();
				bool flag = WorldEventMgr.LoadData(templuckyStartToptenAward);
				if (flag)
				{
					try
					{
						WorldEventMgr.m_luckyStartToptenAward = templuckyStartToptenAward;
						return true;
					}
					catch
					{
					}
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = WorldEventMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					WorldEventMgr.log.Error("ReLoad", e);
				}
			}
			return false;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0003C3D4 File Offset: 0x0003A5D4
		public static bool SendMailToUser(int userId, string NickName, string title, string content)
		{
			using (PlayerBussiness pb = new PlayerBussiness())
			{
				MailInfo mail = new MailInfo
				{
					Content = content,
					Title = title,
					Gold = 0,
					IsExist = true,
					Money = 0,
					GiftToken = 0,
					Receiver = NickName,
					ReceiverID = userId,
					Sender = "Game Master",
					SenderID = 0,
					Type = 0,
					Annex1 = "",
					Annex1Name = ""
				};
				bool flag = pb.SendMail(mail);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400018B RID: 395
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0400018C RID: 396
#pragma warning disable IDE0044 // Add readonly modifier
        private static ReaderWriterLock m_lock;
#pragma warning restore IDE0044 // Add readonly modifier

        // Token: 0x0400018D RID: 397
        private static ThreadSafeRandom random = new ThreadSafeRandom();

		// Token: 0x0400018E RID: 398
		private static Dictionary<int, LuckyStartToptenAwardInfo> m_luckyStartToptenAward;

        public static ReaderWriterLock Lock { get => m_lock; set => m_lock = value; }
    }
}

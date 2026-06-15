using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SqlDataProvider.Data;

namespace Bussiness
{
	// Token: 0x02000011 RID: 17
	public class ItemRecordBussiness : BaseBussiness
	{
		// Token: 0x0600008E RID: 142 RVA: 0x0000C348 File Offset: 0x0000A548
		public void FusionItem(ItemInfo item, ref string Property)
		{
			bool flag = item != null;
			if (flag)
			{
				Property = Property + string.Format("{0}:{1},{2}", item.ItemID, item.Template.Name, Convert.ToInt32(item.IsBinds)) + "|";
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000C3A0 File Offset: 0x0000A5A0
		public bool LogDropItemDb(DataTable dt)
		{
			bool flag = false;
			bool flag2 = dt != null;
			if (flag2)
			{
				SqlBulkCopy copy = new SqlBulkCopy(ConfigurationManager.AppSettings["countDb"], SqlBulkCopyOptions.UseInternalTransaction);
				try
				{
					copy.NotifyAfter = dt.Rows.Count;
					copy.DestinationTableName = "Log_DropItem";
					copy.ColumnMappings.Add(0, "ApplicationId");
					copy.ColumnMappings.Add(1, "SubId");
					copy.ColumnMappings.Add(2, "LineId");
					copy.ColumnMappings.Add(3, "UserId");
					copy.ColumnMappings.Add(4, "ItemId");
					copy.ColumnMappings.Add(5, "TemplateID");
					copy.ColumnMappings.Add(6, "DropId");
					copy.ColumnMappings.Add(7, "DropData");
					copy.ColumnMappings.Add(8, "EnterTime");
					copy.WriteToServer(dt);
					flag = true;
					return flag;
				}
				catch (Exception exception)
				{
					bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
					if (isErrorEnabled)
					{
						BaseBussiness.log.Error("DropItem Log Error:" + exception.ToString());
						return flag;
					}
					return flag;
				}
				finally
				{
					copy.Close();
					dt.Clear();
				}
			}
			return flag;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000C524 File Offset: 0x0000A724
		public bool LogFightDb(DataTable dt)
		{
			bool flag = false;
			bool flag2 = dt != null;
			if (flag2)
			{
				SqlBulkCopy copy = new SqlBulkCopy(ConfigurationManager.AppSettings["countDb"], SqlBulkCopyOptions.UseInternalTransaction);
				try
				{
					copy.NotifyAfter = dt.Rows.Count;
					copy.DestinationTableName = "Log_Fight";
					copy.ColumnMappings.Add(0, "ApplicationId");
					copy.ColumnMappings.Add(1, "SubId");
					copy.ColumnMappings.Add(2, "LineId");
					copy.ColumnMappings.Add(3, "RoomId");
					copy.ColumnMappings.Add(4, "RoomType");
					copy.ColumnMappings.Add(5, "FightType");
					copy.ColumnMappings.Add(6, "ChangeTeam");
					copy.ColumnMappings.Add(7, "PlayBegin");
					copy.ColumnMappings.Add(8, "PlayEnd");
					copy.ColumnMappings.Add(9, "UserCount");
					copy.ColumnMappings.Add(10, "MapId");
					copy.ColumnMappings.Add(11, "TeamA");
					copy.ColumnMappings.Add(12, "TeamB");
					copy.ColumnMappings.Add(13, "PlayResult");
					copy.ColumnMappings.Add(14, "WinTeam");
					copy.ColumnMappings.Add(15, "Detail");
					copy.WriteToServer(dt);
					flag = true;
					return flag;
				}
				catch (Exception exception)
				{
					bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
					if (isErrorEnabled)
					{
						BaseBussiness.log.Error("Fight Log Error:" + exception.ToString());
						return flag;
					}
					return flag;
				}
				finally
				{
					copy.Close();
					dt.Clear();
				}
			}
			return flag;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000C730 File Offset: 0x0000A930
		public bool LogItemDb(DataTable dt)
		{
			bool flag = false;
			bool flag2 = dt != null;
			if (flag2)
			{
				SqlBulkCopy copy = new SqlBulkCopy(ConfigurationManager.AppSettings["countDb"], SqlBulkCopyOptions.UseInternalTransaction);
				try
				{
					copy.NotifyAfter = dt.Rows.Count;
					copy.DestinationTableName = "Log_Item";
					copy.ColumnMappings.Add(0, "ApplicationId");
					copy.ColumnMappings.Add(1, "SubId");
					copy.ColumnMappings.Add(2, "LineId");
					copy.ColumnMappings.Add(3, "EnterTime");
					copy.ColumnMappings.Add(4, "UserId");
					copy.ColumnMappings.Add(5, "Operation");
					copy.ColumnMappings.Add(6, "ItemName");
					copy.ColumnMappings.Add(7, "ItemID");
					copy.ColumnMappings.Add(8, "AddItem");
					copy.ColumnMappings.Add(9, "BeginProperty");
					copy.ColumnMappings.Add(10, "EndProperty");
					copy.ColumnMappings.Add(11, "Result");
					copy.WriteToServer(dt);
					flag = true;
					dt.Clear();
					return flag;
				}
				catch (Exception exception)
				{
					bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
					if (isErrorEnabled)
					{
						BaseBussiness.log.Error("Smith Log Error:" + exception.ToString());
						return flag;
					}
					return flag;
				}
				finally
				{
					copy.Close();
				}
			}
			return flag;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000C8F0 File Offset: 0x0000AAF0
		public bool LogMoneyDb(DataTable dt)
		{
			bool flag = false;
			bool flag2 = dt != null;
			if (flag2)
			{
				SqlBulkCopy copy = new SqlBulkCopy(ConfigurationManager.AppSettings["countDb"], SqlBulkCopyOptions.UseInternalTransaction);
				try
				{
					copy.NotifyAfter = dt.Rows.Count;
					copy.DestinationTableName = "Log_Money";
					copy.ColumnMappings.Add(0, "ApplicationId");
					copy.ColumnMappings.Add(1, "SubId");
					copy.ColumnMappings.Add(2, "LineId");
					copy.ColumnMappings.Add(3, "MastType");
					copy.ColumnMappings.Add(4, "SonType");
					copy.ColumnMappings.Add(5, "UserId");
					copy.ColumnMappings.Add(6, "EnterTime");
					copy.ColumnMappings.Add(7, "Moneys");
					copy.ColumnMappings.Add(8, "Gold");
					copy.ColumnMappings.Add(9, "GiftToken");
					copy.ColumnMappings.Add(10, "Offer");
					copy.ColumnMappings.Add(11, "OtherPay");
					copy.ColumnMappings.Add(12, "GoodId");
					copy.ColumnMappings.Add(13, "ShopId");
					copy.ColumnMappings.Add(14, "Datas");
					copy.WriteToServer(dt);
					flag = true;
					return flag;
				}
				catch
				{
					return flag;
				}
				finally
				{
					copy.Close();
					dt.Clear();
				}
			}
			return flag;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000CAB4 File Offset: 0x0000ACB4
		public bool LogServerDb(DataTable dt)
		{
			bool flag = false;
			bool flag2 = dt != null;
			if (flag2)
			{
				SqlBulkCopy copy = new SqlBulkCopy(ConfigurationManager.AppSettings["countDb"], SqlBulkCopyOptions.UseInternalTransaction);
				try
				{
					copy.NotifyAfter = dt.Rows.Count;
					copy.DestinationTableName = "Log_Server";
					copy.ColumnMappings.Add(0, "ApplicationId");
					copy.ColumnMappings.Add(1, "SubId");
					copy.ColumnMappings.Add(2, "EnterTime");
					copy.ColumnMappings.Add(3, "Online");
					copy.ColumnMappings.Add(4, "Reg");
					copy.WriteToServer(dt);
					flag = true;
					return flag;
				}
				catch (Exception exception)
				{
					bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
					if (isErrorEnabled)
					{
						BaseBussiness.log.Error("Server Log Error:" + exception.ToString());
						return flag;
					}
					return flag;
				}
				finally
				{
					copy.Close();
					dt.Clear();
				}
			}
			return flag;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000CBD8 File Offset: 0x0000ADD8
		public void PropertyString(ItemInfo item, ref string Property)
		{
			bool flag = item != null;
			if (flag)
			{
				Property = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", new object[]
				{
					item.StrengthenLevel,
					item.Attack,
					item.Defence,
					item.Agility,
					item.Luck,
					item.AttackCompose,
					item.DefendCompose,
					item.AgilityCompose,
					item.LuckCompose
				});
			}
		}
	}
}

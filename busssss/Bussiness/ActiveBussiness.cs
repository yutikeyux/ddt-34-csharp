using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SqlDataProvider.Data;

namespace Bussiness
{
	// Token: 0x02000003 RID: 3
	public class ActiveBussiness : BaseCrossBussiness
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002288 File Offset: 0x00000488
		public ActiveInfo[] GetAllActives()
		{
			List<ActiveInfo> list = new List<ActiveInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Active_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitActiveInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002348 File Offset: 0x00000548
		public ActiveConvertItemInfo[] GetSingleActiveConvertItems(int activeID)
		{
			List<ActiveConvertItemInfo> list = new List<ActiveConvertItemInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = activeID;
				this.db.GetReader(ref resultDataReader, "SP_Active_Convert_Item_Info_Single", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitActiveConvertItemInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002430 File Offset: 0x00000630
		public ActiveInfo GetSingleActives(int activeID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = activeID;
				this.db.GetReader(ref resultDataReader, "SP_Active_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitActiveInfo(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002504 File Offset: 0x00000704
		public ActiveConvertItemInfo InitActiveConvertItemInfo(SqlDataReader reader)
		{
			return new ActiveConvertItemInfo
			{
				ID = (int)reader["ID"],
				ActiveID = (int)reader["ActiveID"],
				TemplateID = (int)reader["TemplateID"],
				ItemType = (int)reader["ItemType"],
				ItemCount = (int)reader["ItemCount"],
				LimitValue = (int)reader["LimitValue"],
				IsBind = (bool)reader["IsBind"],
				ValidDate = (int)reader["ValidDate"]
			};
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000025D4 File Offset: 0x000007D4
		public ActiveInfo InitActiveInfo(SqlDataReader reader)
		{
			ActiveInfo info = new ActiveInfo
			{
				ActiveID = (int)reader["ActiveID"],
				Description = ((reader["Description"] == null) ? "" : reader["Description"].ToString()),
				Content = ((reader["Content"] == null) ? "" : reader["Content"].ToString()),
				AwardContent = ((reader["AwardContent"] == null) ? "" : reader["AwardContent"].ToString()),
				HasKey = (int)reader["HasKey"]
			};
			bool flag = !string.IsNullOrEmpty(reader["EndDate"].ToString());
			if (flag)
			{
				info.EndDate = new DateTime?((DateTime)reader["EndDate"]);
			}
			info.IsOnly = (int)reader["IsOnly"];
			info.StartDate = (DateTime)reader["StartDate"];
			info.Title = reader["Title"].ToString();
			info.Type = (int)reader["Type"];
			info.ActiveType = (int)reader["ActiveType"];
			info.ActionTimeContent = ((reader["ActionTimeContent"] == null) ? "" : reader["ActionTimeContent"].ToString());
			info.IsAdvance = (bool)reader["IsAdvance"];
			info.GoodsExchangeTypes = ((reader["GoodsExchangeTypes"] == null) ? "" : reader["GoodsExchangeTypes"].ToString());
			info.GoodsExchangeNum = ((reader["GoodsExchangeNum"] == null) ? "" : reader["GoodsExchangeNum"].ToString());
			info.limitType = ((reader["limitType"] == null) ? "" : reader["limitType"].ToString());
			info.limitValue = ((reader["limitValue"] == null) ? "" : reader["limitValue"].ToString());
			info.IsShow = (bool)reader["IsShow"];
			info.IconID = (int)reader["IconID"];
			return info;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002868 File Offset: 0x00000A68
		public ActiveConvertItemInfo[] GetAllActiveConvertItem()
		{
			List<ActiveConvertItemInfo> infos = new List<ActiveConvertItemInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Active_Convert_Item_All");
				while (reader.Read())
				{
					infos.Add(this.InitActiveConvertItemInfo(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("InitActiveConvertItemInfo", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return infos.ToArray();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002924 File Offset: 0x00000B24
		public ActiveAwardInfo[] GetAllActiveAwardInfo()
		{
			List<ActiveAwardInfo> list = new List<ActiveAwardInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Active_Award_All");
				while (resultDataReader.Read())
				{
					ActiveAwardInfo item = new ActiveAwardInfo
					{
						ID = (int)resultDataReader["ID"],
						ActiveID = (int)resultDataReader["ActiveID"],
						AgilityCompose = (int)resultDataReader["AgilityCompose"],
						AttackCompose = (int)resultDataReader["AttackCompose"],
						Count = (int)resultDataReader["Count"],
						DefendCompose = (int)resultDataReader["DefendCompose"],
						Gold = (int)resultDataReader["Gold"],
						ItemID = (int)resultDataReader["ItemID"],
						LuckCompose = (int)resultDataReader["LuckCompose"],
						Mark = (int)resultDataReader["Mark"],
						Money = (int)resultDataReader["Money"],
						Sex = (int)resultDataReader["Sex"],
						StrengthenLevel = (int)resultDataReader["StrengthenLevel"],
						ValidDate = (int)resultDataReader["ValidDate"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllActiveAwardInfo", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002B44 File Offset: 0x00000D44
		public LuckyStartToptenAwardInfo[] GetAllLuckyStartToptenAward()
		{
			List<LuckyStartToptenAwardInfo> infos = new List<LuckyStartToptenAwardInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_LuckyStart_Topten_Award_All");
				while (reader.Read())
				{
					infos.Add(new LuckyStartToptenAwardInfo
					{
						ID = (int)reader["ID"],
						Type = (int)reader["Type"],
						TemplateID = (int)reader["TemplateID"],
						Validate = (int)reader["Validate"],
						Count = (int)reader["Count"],
						StrengthenLevel = (int)reader["StrengthenLevel"],
						AttackCompose = (int)reader["AttackCompose"],
						DefendCompose = (int)reader["DefendCompose"],
						AgilityCompose = (int)reader["AgilityCompose"],
						LuckCompose = (int)reader["LuckCompose"],
						IsBinds = (bool)reader["IsBind"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetLuckyStart_Topten_Award_All", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return infos.ToArray();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002D10 File Offset: 0x00000F10
		public SubActiveInfo[] GetAllSubActive()
		{
			List<SubActiveInfo> infos = new List<SubActiveInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_SubActive_All");
				while (reader.Read())
				{
					infos.Add(new SubActiveInfo
					{
						ID = (int)reader["ID"],
						ActiveID = (int)reader["ActiveID"],
						SubID = (int)reader["SubID"],
						IsOpen = (bool)reader["IsOpen"],
						StartDate = (DateTime)reader["StartDate"],
						StartTime = (DateTime)reader["StartTime"],
						EndDate = (DateTime)reader["EndDate"],
						EndTime = (DateTime)reader["EndTime"],
						IsContinued = (bool)reader["IsContinued"],
						ActiveInfo = ((reader["ActiveInfo"] == null) ? "" : reader["ActiveInfo"].ToString())
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init AllSubActive", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return infos.ToArray();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002EE4 File Offset: 0x000010E4
		public SubActiveConditionInfo[] GetAllSubActiveCondition(int ActiveID)
		{
			List<SubActiveConditionInfo> infos = new List<SubActiveConditionInfo>();
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ActiveID", ActiveID)
				};
				this.db.GetReader(ref reader, "SP_SubActiveCondition_All", para);
				while (reader.Read())
				{
					infos.Add(new SubActiveConditionInfo
					{
						ID = (int)reader["ID"],
						ActiveID = (int)reader["ActiveID"],
						SubID = (int)reader["SubID"],
						ConditionID = (int)reader["ConditionID"],
						Type = (int)reader["Type"],
						Value = ((reader["Value"] == null) ? "" : reader["Value"].ToString()),
						AwardType = (int)reader["AwardType"],
						AwardValue = ((reader["AwardValue"] == null) ? "" : reader["AwardValue"].ToString()),
						IsValid = (bool)reader["IsValid"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init AllSubActive", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return infos.ToArray();
		}
	}
}

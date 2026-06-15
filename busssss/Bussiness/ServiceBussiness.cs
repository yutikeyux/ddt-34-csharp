using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SqlDataProvider.Data;

namespace Bussiness
{
	// Token: 0x0200001B RID: 27
	public class ServiceBussiness : BaseBussiness
	{
		// Token: 0x0600024C RID: 588 RVA: 0x000322C4 File Offset: 0x000304C4
		public FightRateInfo[] GetFightRate(int serverId)
		{
			SqlDataReader resultDataReader = null;
			List<FightRateInfo> list = new List<FightRateInfo>();
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ServerID", serverId)
				};
				this.db.GetReader(ref resultDataReader, "SP_Fight_Rate", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					FightRateInfo item = new FightRateInfo
					{
						ID = (int)resultDataReader["ID"],
						ServerID = (int)resultDataReader["ServerID"],
						Rate = (int)resultDataReader["Rate"],
						BeginDay = (DateTime)resultDataReader["BeginDay"],
						EndDay = (DateTime)resultDataReader["EndDay"],
						BeginTime = (DateTime)resultDataReader["BeginTime"],
						EndTime = (DateTime)resultDataReader["EndTime"],
						SelfCue = ((resultDataReader["SelfCue"] == null) ? "" : resultDataReader["SelfCue"].ToString()),
						EnemyCue = ((resultDataReader["EnemyCue"] == null) ? "" : resultDataReader["EnemyCue"].ToString()),
						BoyTemplateID = (int)resultDataReader["BoyTemplateID"],
						GirlTemplateID = (int)resultDataReader["GirlTemplateID"],
						Name = ((resultDataReader["Name"] == null) ? "" : resultDataReader["Name"].ToString())
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetFightRate", exception);
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
			return list.ToArray();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00032508 File Offset: 0x00030708
		public string GetGameEdition()
		{
			string str = string.Empty;
			SqlDataReader resultDataReader = null;
			string result;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Server_Edition");
				bool flag = resultDataReader.Read();
				if (flag)
				{
					result = ((resultDataReader["value"] == null) ? "" : resultDataReader["value"].ToString());
				}
				else
				{
					result = str;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = str;
				}
				else
				{
					result = str;
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
			return result;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000325D4 File Offset: 0x000307D4
		public ArrayList GetRate(int serverId)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				ArrayList list = new ArrayList();
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ServerID", serverId)
				};
				this.db.GetReader(ref resultDataReader, "SP_Rate", sqlParameters);
				while (resultDataReader.Read())
				{
					RateInfo info = new RateInfo
					{
						ServerID = (int)resultDataReader["ServerID"],
						Rate = (float)((decimal)resultDataReader["Rate"]),
						BeginDay = (DateTime)resultDataReader["BeginDay"],
						EndDay = (DateTime)resultDataReader["EndDay"],
						BeginTime = (DateTime)resultDataReader["BeginTime"],
						EndTime = (DateTime)resultDataReader["EndTime"],
						Type = (int)resultDataReader["Type"]
					};
					list.Add(info);
				}
				list.TrimToSize();
				return list;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetRates", exception);
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
			return null;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00032778 File Offset: 0x00030978
		public RateInfo GetRateWithType(int serverId, int type)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ServerID", serverId),
					new SqlParameter("@Type", type)
				};
				this.db.GetReader(ref resultDataReader, "SP_Rate_WithType", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new RateInfo
					{
						ServerID = (int)resultDataReader["ServerID"],
						Type = type,
						Rate = (float)resultDataReader["Rate"],
						BeginDay = (DateTime)resultDataReader["BeginDay"],
						EndDay = (DateTime)resultDataReader["EndDay"],
						BeginTime = (DateTime)resultDataReader["BeginTime"],
						EndTime = (DateTime)resultDataReader["EndTime"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetRate type: " + type.ToString(), exception);
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

		// Token: 0x06000250 RID: 592 RVA: 0x00032908 File Offset: 0x00030B08
		public RecordInfo GetRecordInfo(DateTime date, int SaveRecordSecond)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Date", date.ToString("yyyy-MM-dd HH:mm:ss")),
					new SqlParameter("@Second", SaveRecordSecond)
				};
				this.db.GetReader(ref resultDataReader, "SP_Server_Record", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new RecordInfo
					{
						ActiveExpendBoy = (int)resultDataReader["ActiveExpendBoy"],
						ActiveExpendGirl = (int)resultDataReader["ActiveExpendGirl"],
						ActviePayBoy = (int)resultDataReader["ActviePayBoy"],
						ActviePayGirl = (int)resultDataReader["ActviePayGirl"],
						ExpendBoy = (int)resultDataReader["ExpendBoy"],
						ExpendGirl = (int)resultDataReader["ExpendGirl"],
						OnlineBoy = (int)resultDataReader["OnlineBoy"],
						OnlineGirl = (int)resultDataReader["OnlineGirl"],
						TotalBoy = (int)resultDataReader["TotalBoy"],
						TotalGirl = (int)resultDataReader["TotalGirl"],
						ActiveOnlineBoy = (int)resultDataReader["ActiveOnlineBoy"],
						ActiveOnlineGirl = (int)resultDataReader["ActiveOnlineGirl"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
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

		// Token: 0x06000251 RID: 593 RVA: 0x00032B14 File Offset: 0x00030D14
		public Dictionary<string, string> GetServerConfig()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			SqlDataReader resultDataReader = null;
			Dictionary<string, string> result;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Server_Config");
				while (resultDataReader.Read())
				{
					bool flag = !dictionary.ContainsKey(resultDataReader["Name"].ToString());
					if (flag)
					{
						dictionary.Add(resultDataReader["Name"].ToString(), resultDataReader["Value"].ToString());
					}
				}
				result = dictionary;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetServerConfig", exception);
					result = dictionary;
				}
				else
				{
					result = dictionary;
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
			return result;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00032C08 File Offset: 0x00030E08
		public ServerInfo[] GetServerList()
		{
			List<ServerInfo> list = new List<ServerInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Service_List");
				while (resultDataReader.Read())
				{
					ServerInfo item = new ServerInfo
					{
						ID = (int)resultDataReader["ID"],
						IP = resultDataReader["IP"].ToString(),
						Name = resultDataReader["Name"].ToString(),
						Online = (int)resultDataReader["Online"],
						Port = (int)resultDataReader["Port"],
						Remark = resultDataReader["Remark"].ToString(),
						Room = (int)resultDataReader["Room"],
						State = (int)resultDataReader["State"],
						Total = (int)resultDataReader["Total"],
						RSA = resultDataReader["RSA"].ToString(),
						MustLevel = (int)resultDataReader["MustLevel"],
						LowestLevel = (int)resultDataReader["LowestLevel"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
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

		// Token: 0x06000253 RID: 595 RVA: 0x00032DF8 File Offset: 0x00030FF8
		public ServerProperty[] GetAllServerProperty()
		{
			List<ServerProperty> result = new List<ServerProperty>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Server_Config");
				while (resultDataReader.Read())
				{
					result.Add(new ServerProperty
					{
						Key = resultDataReader["Name"].ToString(),
						Value = resultDataReader["Value"].ToString()
					});
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetServerConfig", exception);
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
			return result.ToArray();
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00032EE0 File Offset: 0x000310E0
		public ServerProperty GetServerPropertyByKey(string key)
		{
			ServerProperty property = null;
			SqlDataReader resultDataReader = null;
			ServerProperty result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Key", key)
				};
				this.db.GetReader(ref resultDataReader, "SP_Server_Config_Single", sqlParameters);
				while (resultDataReader.Read())
				{
					property = new ServerProperty
					{
						Key = resultDataReader["Name"].ToString(),
						Value = resultDataReader["Value"].ToString()
					};
				}
				result = property;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetServerConfig", exception);
					result = property;
				}
				else
				{
					result = property;
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
			return result;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00032FD4 File Offset: 0x000311D4
		public ServerInfo[] GetServiceByIP(string IP)
		{
			List<ServerInfo> list = new List<ServerInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@IP", SqlDbType.NVarChar, 50)
				};
				sqlParameters[0].Value = IP;
				this.db.GetReader(ref resultDataReader, "SP_Service_ListByIP", sqlParameters);
				while (resultDataReader.Read())
				{
					ServerInfo item = new ServerInfo
					{
						ID = (int)resultDataReader["ID"],
						IP = resultDataReader["IP"].ToString(),
						Name = resultDataReader["Name"].ToString(),
						Online = (int)resultDataReader["Online"],
						Port = (int)resultDataReader["Port"],
						Remark = resultDataReader["Remark"].ToString(),
						Room = (int)resultDataReader["Room"],
						State = (int)resultDataReader["State"],
						Total = (int)resultDataReader["Total"],
						RSA = resultDataReader["RSA"].ToString()
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
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

		// Token: 0x06000256 RID: 598 RVA: 0x000331BC File Offset: 0x000313BC
		public ServerInfo GetServiceSingle(int ID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = ID;
				this.db.GetReader(ref resultDataReader, "SP_Service_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new ServerInfo
					{
						ID = (int)resultDataReader["ID"],
						IP = resultDataReader["IP"].ToString(),
						Name = resultDataReader["Name"].ToString(),
						Online = (int)resultDataReader["Online"],
						Port = (int)resultDataReader["Port"],
						Remark = resultDataReader["Remark"].ToString(),
						Room = (int)resultDataReader["Room"],
						State = (int)resultDataReader["State"],
						Total = (int)resultDataReader["Total"],
						RSA = resultDataReader["RSA"].ToString(),
						NewerServer = (bool)resultDataReader["NewerServer"],
						ZoneId = (int)resultDataReader["ZoneId"],
						ZoneName = resultDataReader["ZoneName"].ToString()
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
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

		// Token: 0x06000257 RID: 599 RVA: 0x000333D0 File Offset: 0x000315D0
		public bool UpdateRSA(int ID, string RSA)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", ID),
					new SqlParameter("@RSA", RSA)
				};
				flag = this.db.RunProcedure("SP_Service_UpdateRSA", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0003345C File Offset: 0x0003165C
		public bool UpdateServerPropertyByKey(string key, string value)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Key", key),
					new SqlParameter("@Value", value)
				};
				flag = this.db.RunProcedure("SP_Server_Config_Update", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x000334E4 File Offset: 0x000316E4
		public bool UpdateService(ServerInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@Online", info.Online),
					new SqlParameter("@State", info.State)
				};
				flag = this.db.RunProcedure("SP_Service_Update", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;
using SqlDataProvider.BaseClass;
using SqlDataProvider.Data;

namespace Bussiness
{
	// Token: 0x02000015 RID: 21
	public class MemberShipBussiness : IDisposable
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x0000D994 File Offset: 0x0000BB94
		public bool CreateUsername(string applicationname, string username, string password, string email, string passwordformat, string passwordsalt, bool usersex)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter("@ApplicationName", applicationname),
				new SqlParameter("@UserName", username),
				new SqlParameter("@password", password),
				new SqlParameter("@email", email),
				new SqlParameter("@PasswordFormat", passwordformat),
				new SqlParameter("@PasswordSalt", passwordsalt),
				new SqlParameter("@UserSex", usersex),
				new SqlParameter("@UserId", SqlDbType.Int)
			};
			sqlParameters[7].Direction = ParameterDirection.Output;
			bool flag = this.db.RunProcedure("Mem_Users_CreateUser", sqlParameters);
			bool flag2 = flag;
			if (flag2)
			{
				flag = ((int)sqlParameters[7].Value > 0);
			}
			return flag;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000DA5C File Offset: 0x0000BC5C
		public bool CheckAdmin(string username)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter("@UserName", username),
				new SqlParameter("@UserCOUNT", SqlDbType.Int)
			};
			sqlParameters[1].Direction = ParameterDirection.Output;
			this.db.RunProcedure("Mem_UserInfo_Addmin", sqlParameters);
			return (int)sqlParameters[1].Value > 0;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000DAC0 File Offset: 0x0000BCC0
		public int CheckPoint(string username)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter("@UserName", username),
				new SqlParameter("@Point", SqlDbType.Int)
			};
			sqlParameters[1].Direction = ParameterDirection.Output;
			this.db.RunProcedure("Mem_User_Point", sqlParameters);
			return (int)sqlParameters[1].Value;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000DB20 File Offset: 0x0000BD20
		public bool CheckUsername(string username, string password)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter("@UserName", username),
				new SqlParameter("@password", password),
				new SqlParameter("@UserId", SqlDbType.Int)
			};
			sqlParameters[2].Direction = ParameterDirection.Output;
			this.db.RunProcedure("Check_User", sqlParameters);
			int result = 0;
			int.TryParse(sqlParameters[2].Value.ToString(), out result);
			return result > 0;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000DB9A File Offset: 0x0000BD9A
		public void Dispose()
		{
			this.db.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000DBB0 File Offset: 0x0000BDB0
		public bool ExistsUsername(string username)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter("@UserName", username),
				new SqlParameter("@UserCOUNT", SqlDbType.Int)
			};
			sqlParameters[1].Direction = ParameterDirection.Output;
			this.db.RunProcedure("Mem_UserInfo_SearchName", sqlParameters);
			return (int)sqlParameters[1].Value > 0;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000DC14 File Offset: 0x0000BE14
		public eStoreInfo[] GetAlleStore()
		{
			List<eStoreInfo> list = new List<eStoreInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_eStore_All");
				while (resultDataReader.Read())
				{
					eStoreInfo item = new eStoreInfo
					{
						StoreID = (int)resultDataReader["StoreID"],
						TemplateID = (int)resultDataReader["TemplateID"],
						PriceValue = (int)resultDataReader["PriceValue"],
						StrengthenLevel = (int)resultDataReader["StrengthenLevel"],
						AttackCompose = (int)resultDataReader["AttackCompose"],
						AgilityCompose = (int)resultDataReader["AgilityCompose"],
						DefendCompose = (int)resultDataReader["DefendCompose"],
						LuckCompose = (int)resultDataReader["LuckCompose"],
						IsBinds = (bool)resultDataReader["IsBinds"],
						ValidDate = (int)resultDataReader["ValidDate"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = MemberShipBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					MemberShipBussiness.log.Error("eStore", exception);
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

		// Token: 0x060000B8 RID: 184 RVA: 0x0000DDCC File Offset: 0x0000BFCC
		public eStoreInfo[] GetAlleStoreByDesc()
		{
			List<eStoreInfo> list = new List<eStoreInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_eStore_Desc");
				while (resultDataReader.Read())
				{
					eStoreInfo item = new eStoreInfo
					{
						StoreID = (int)resultDataReader["StoreID"],
						TemplateID = (int)resultDataReader["TemplateID"],
						PriceValue = (int)resultDataReader["PriceValue"],
						StrengthenLevel = (int)resultDataReader["StrengthenLevel"],
						AttackCompose = (int)resultDataReader["AttackCompose"],
						AgilityCompose = (int)resultDataReader["AgilityCompose"],
						DefendCompose = (int)resultDataReader["DefendCompose"],
						LuckCompose = (int)resultDataReader["LuckCompose"],
						IsBinds = (bool)resultDataReader["IsBinds"],
						ValidDate = (int)resultDataReader["ValidDate"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = MemberShipBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					MemberShipBussiness.log.Error("eStore", exception);
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

		// Token: 0x060000B9 RID: 185 RVA: 0x0000DF84 File Offset: 0x0000C184
		public eStoreInfo[] GetAlleStoreSale()
		{
			List<eStoreInfo> list = new List<eStoreInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_eStore_Desc_Sale");
				while (resultDataReader.Read())
				{
					eStoreInfo item = new eStoreInfo
					{
						StoreID = (int)resultDataReader["StoreID"],
						TemplateID = (int)resultDataReader["TemplateID"],
						PriceValue = (int)resultDataReader["PriceValue1"],
						StrengthenLevel = (int)resultDataReader["StrengthenLevel"],
						AttackCompose = (int)resultDataReader["AttackCompose"],
						AgilityCompose = (int)resultDataReader["AgilityCompose"],
						DefendCompose = (int)resultDataReader["DefendCompose"],
						LuckCompose = (int)resultDataReader["LuckCompose"],
						IsBinds = (bool)resultDataReader["IsBinds"],
						ValidDate = (int)resultDataReader["ValidDate"],
						OldPriceValue = (int)resultDataReader["PriceValue"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = MemberShipBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					MemberShipBussiness.log.Error("SP_eStore_Desc_Sale", exception);
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

		// Token: 0x060000BA RID: 186 RVA: 0x0000E154 File Offset: 0x0000C354
		public eStoreInfo[] GetAlleStoreTopBuy()
		{
			List<eStoreInfo> list = new List<eStoreInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_eStore_Desc_Buy");
				while (resultDataReader.Read())
				{
					eStoreInfo item = new eStoreInfo
					{
						StoreID = (int)resultDataReader["StoreID"],
						TemplateID = (int)resultDataReader["TemplateID"],
						PriceValue = (int)resultDataReader["PriceValue"],
						StrengthenLevel = (int)resultDataReader["StrengthenLevel"],
						AttackCompose = (int)resultDataReader["AttackCompose"],
						AgilityCompose = (int)resultDataReader["AgilityCompose"],
						DefendCompose = (int)resultDataReader["DefendCompose"],
						LuckCompose = (int)resultDataReader["LuckCompose"],
						IsBinds = (bool)resultDataReader["IsBinds"],
						ValidDate = (int)resultDataReader["ValidDate"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = MemberShipBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					MemberShipBussiness.log.Error("SP_eStore_Desc_Buy", exception);
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

		// Token: 0x060000BB RID: 187 RVA: 0x0000E30C File Offset: 0x0000C50C
		public bool RemovePoint(string username, int Point)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter("@UserName", username),
				new SqlParameter("@Point", Point),
				new SqlParameter("@Result", SqlDbType.Int)
			};
			sqlParameters[2].Direction = ParameterDirection.ReturnValue;
			this.db.RunProcedure("Mem_User_Remove_Point", sqlParameters);
			return (int)sqlParameters[2].Value == 0;
		}

		// Token: 0x040000ED RID: 237
		protected Sql_DbObject db = new Sql_DbObject("AppConfig", "membershipDb");

		// Token: 0x040000EE RID: 238
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

using System;
using System.Data;
using System.Data.SqlClient;
using Bussiness.CenterService;
using SqlDataProvider.Data;

namespace Bussiness
{
	// Token: 0x02000013 RID: 19
	public class ManageBussiness : BaseBussiness
	{
		// Token: 0x0600009D RID: 157 RVA: 0x0000CF40 File Offset: 0x0000B140
		private bool ForbidPlayer(string userName, string nickName, int userID, DateTime forbidDate, bool isExist)
		{
			return this.ForbidPlayer(userName, nickName, userID, forbidDate, isExist, "");
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000CF64 File Offset: 0x0000B164
		private bool ForbidPlayer(string userName, string nickName, int userID, DateTime forbidDate, bool isExist, string ForbidReason)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[6];
				array[0] = new SqlParameter("@UserName", userName);
				array[1] = new SqlParameter("@NickName", nickName);
				array[2] = new SqlParameter("@UserID", userID);
				SqlParameter[] sqlParameters = array;
				sqlParameters[2].Direction = ParameterDirection.InputOutput;
				sqlParameters[3] = new SqlParameter("@ForbidDate", forbidDate);
				sqlParameters[4] = new SqlParameter("@IsExist", isExist);
				sqlParameters[5] = new SqlParameter("@ForbidReason", ForbidReason);
				this.db.RunProcedure("SP_Admin_ForbidUser", sqlParameters);
				userID = (int)sqlParameters[2].Value;
				bool flag2 = userID <= 0;
				if (flag2)
				{
					result = flag;
				}
				else
				{
					flag = true;
					bool flag3 = !isExist;
					if (flag3)
					{
						this.KitoffUser(userID, "You are kicking out by GM!!");
						result = flag;
					}
					else
					{
						result = flag;
					}
				}
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

		// Token: 0x0600009F RID: 159 RVA: 0x0000D080 File Offset: 0x0000B280
		public bool ForbidPlayerByNickName(string nickName, DateTime date, bool isExist)
		{
			return this.ForbidPlayer("", nickName, 0, date, isExist);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000D0A4 File Offset: 0x0000B2A4
		public bool ForbidPlayerByNickName(string nickName, DateTime date, bool isExist, string ForbidReason)
		{
			return this.ForbidPlayer("", nickName, 0, date, isExist, ForbidReason);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000D0C8 File Offset: 0x0000B2C8
		public bool ForbidPlayerByUserID(int userID, DateTime date, bool isExist)
		{
			return this.ForbidPlayer("", "", userID, date, isExist);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000D0F0 File Offset: 0x0000B2F0
		public bool ForbidPlayerByUserID(int userID, DateTime date, bool isExist, string ForbidReason)
		{
			return this.ForbidPlayer("", "", userID, date, isExist, ForbidReason);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000D118 File Offset: 0x0000B318
		public bool ForbidPlayerByUserName(string userName, DateTime date, bool isExist)
		{
			return this.ForbidPlayer(userName, "", 0, date, isExist);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000D13C File Offset: 0x0000B33C
		public bool ForbidPlayerByUserName(string userName, DateTime date, bool isExist, string ForbidReason)
		{
			return this.ForbidPlayer(userName, "", 0, date, isExist, ForbidReason);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000D160 File Offset: 0x0000B360
		public int GetConfigState(int type)
		{
			try
			{
				using (CenterServiceClient client = new CenterServiceClient())
				{
					return client.GetConfigState(type);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetConfigState", exception);
				}
			}
			return 2;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000D1D4 File Offset: 0x0000B3D4
		public int KitoffUser(int id, string msg)
		{
			int result;
			try
			{
				using (CenterServiceClient client = new CenterServiceClient())
				{
					bool flag = client.KitoffUser(id, msg);
					if (flag)
					{
						result = 0;
					}
					else
					{
						result = 3;
					}
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("KitoffUser", exception);
				}
				result = 1;
			}
			return result;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000D250 File Offset: 0x0000B450
		public int KitoffUserByNickName(string name, string msg)
		{
			int result;
			using (PlayerBussiness bussiness = new PlayerBussiness())
			{
				PlayerInfo userSingleByNickName = bussiness.GetUserSingleByNickName(name);
				bool flag = userSingleByNickName == null;
				if (flag)
				{
					result = 2;
				}
				else
				{
					result = this.KitoffUser(userSingleByNickName.ID, msg);
				}
			}
			return result;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000D2A8 File Offset: 0x0000B4A8
		public int KitoffUserByUserName(string name, string msg)
		{
			int result;
			using (PlayerBussiness bussiness = new PlayerBussiness())
			{
				PlayerInfo userSingleByUserName = bussiness.GetUserSingleByUserName(name);
				bool flag = userSingleByUserName == null;
				if (flag)
				{
					result = 2;
				}
				else
				{
					result = this.KitoffUser(userSingleByUserName.ID, msg);
				}
			}
			return result;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000D300 File Offset: 0x0000B500
		public bool Reload(string type)
		{
			try
			{
				using (CenterServiceClient client = new CenterServiceClient())
				{
					return client.Reload(type);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Reload", exception);
				}
			}
			return false;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000D374 File Offset: 0x0000B574
		public bool ReLoadServerList()
		{
			bool flag = false;
			bool result;
			try
			{
				using (CenterServiceClient client = new CenterServiceClient())
				{
					bool flag2 = client.ReLoadServerList();
					if (flag2)
					{
						flag = true;
						result = flag;
					}
					else
					{
						result = flag;
					}
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("ReLoadServerList", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000D3F8 File Offset: 0x0000B5F8
		public bool SystemNotice(string msg)
		{
			bool flag = false;
			bool result;
			try
			{
				bool flag2 = string.IsNullOrEmpty(msg);
				if (flag2)
				{
					result = flag;
				}
				else
				{
					using (CenterServiceClient client = new CenterServiceClient())
					{
						bool flag3 = client.SystemNotice(msg);
						if (flag3)
						{
							flag = true;
							result = flag;
						}
						else
						{
							result = flag;
						}
					}
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SystemNotice", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000D48C File Offset: 0x0000B68C
		public bool UpdateConfigState(int type, bool state)
		{
			try
			{
				using (CenterServiceClient client = new CenterServiceClient())
				{
					return client.UpdateConfigState(type, state);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdateConfigState", exception);
				}
			}
			return false;
		}
	}
}

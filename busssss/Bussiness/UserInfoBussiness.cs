using System;
using System.Data;
using System.Data.SqlClient;

namespace Bussiness
{
	// Token: 0x0200001F RID: 31
	public class UserInfoBussiness : BaseBussiness
	{
		// Token: 0x0600026A RID: 618 RVA: 0x00033984 File Offset: 0x00031B84
		public bool AddUserInfo(string uid, string userName, string portrait)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Uid", uid),
					new SqlParameter("@UserName", userName),
					new SqlParameter("@Portrait", portrait),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[3].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_User_Info_Insert", sqlParameters);
				flag = ((int)sqlParameters[3].Value == 0);
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

		// Token: 0x0600026B RID: 619 RVA: 0x00033A44 File Offset: 0x00031C44
		public bool GetFromDbByUid(string uid, ref string userName, ref string portrait)
		{
			SqlDataReader resultDataReader = null;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Uid", uid)
				};
				this.db.GetReader(ref resultDataReader, "SP_User_Info_QueryByUid", sqlParameters);
				while (resultDataReader.Read())
				{
					userName = ((resultDataReader["UserName"] == null) ? "" : resultDataReader["UserName"].ToString());
					portrait = ((resultDataReader["Portrait"] == null) ? "" : resultDataReader["Portrait"].ToString());
				}
				result = (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(portrait));
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
				result = false;
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
	}
}

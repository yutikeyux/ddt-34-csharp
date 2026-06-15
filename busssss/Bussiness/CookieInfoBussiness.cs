using System;
using System.Data;
using System.Data.SqlClient;

namespace Bussiness
{
	// Token: 0x0200000C RID: 12
	public class CookieInfoBussiness : BaseBussiness
	{
		// Token: 0x06000069 RID: 105 RVA: 0x0000B790 File Offset: 0x00009990
		public bool AddCookieInfo(string bdSigUser, string bdSigPortrait, string bdSigSessionKey)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@BdSigUser", bdSigUser),
					new SqlParameter("@BdSigPortrait", bdSigPortrait),
					new SqlParameter("@BdSigSessionKey", bdSigSessionKey),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[3].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Cookie_Info_Insert", sqlParameters);
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

		// Token: 0x0600006A RID: 106 RVA: 0x0000B850 File Offset: 0x00009A50
		public bool GetFromDbByUser(string bdSigUser, ref string bdSigPortrait, ref string bdSigSessionKey)
		{
			SqlDataReader resultDataReader = null;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@BdSigUser", bdSigUser)
				};
				this.db.GetReader(ref resultDataReader, "SP_Cookie_Info_QueryByUser", sqlParameters);
				while (resultDataReader.Read())
				{
					bdSigPortrait = ((resultDataReader["BdSigPortrait"] == null) ? "" : resultDataReader["BdSigPortrait"].ToString());
					bdSigSessionKey = ((resultDataReader["BdSigSessionKey"] == null) ? "" : resultDataReader["BdSigSessionKey"].ToString());
				}
				result = (!string.IsNullOrEmpty(bdSigPortrait) && !string.IsNullOrEmpty(bdSigSessionKey));
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

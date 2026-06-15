using System;
using System.Data;
using System.Data.SqlClient;

namespace Bussiness
{
	// Token: 0x02000016 RID: 22
	public class OrderBussiness : BaseBussiness
	{
		// Token: 0x060000BE RID: 190 RVA: 0x0000E3B4 File Offset: 0x0000C5B4
		public bool AddOrder(string order, double amount, string username, string payWay, string serverId)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Order", order),
					new SqlParameter("@Amount", amount),
					new SqlParameter("@Username", username),
					new SqlParameter("@PayWay", payWay),
					new SqlParameter("@ServerId", serverId),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[5].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Charge_Order", sqlParameters);
				flag = ((int)sqlParameters[5].Value == 0);
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

		// Token: 0x060000BF RID: 191 RVA: 0x0000E498 File Offset: 0x0000C698
		public string GetOrderToName(string order, ref string serverId)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Order", order)
				};
				this.db.GetReader(ref resultDataReader, "SP_Charge_Order_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					serverId = ((resultDataReader["ServerId"] == null) ? "" : resultDataReader["ServerId"].ToString());
					return (resultDataReader["UserName"] == null) ? "" : resultDataReader["UserName"].ToString();
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
			return "";
		}
	}
}

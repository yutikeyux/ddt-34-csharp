using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;
using SqlDataProvider.BaseClass;

namespace Bussiness
{
	// Token: 0x02000007 RID: 7
	public class BaseCrossBussiness : IDisposable
	{
		// Token: 0x0600001E RID: 30 RVA: 0x000037B8 File Offset: 0x000019B8
		public DataTable GetPage(string queryStr, string queryWhere, int pageCurrent, int pageSize, string fdShow, string fdOreder, string fdKey, ref int total)
		{
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@QueryStr", queryStr),
					new SqlParameter("@QueryWhere", queryWhere),
					new SqlParameter("@PageSize", pageSize),
					new SqlParameter("@PageCurrent", pageCurrent),
					new SqlParameter("@FdShow", fdShow),
					new SqlParameter("@FdOrder", fdOreder),
					new SqlParameter("@FdKey", fdKey),
					new SqlParameter("@TotalRow", total)
				};
				SqlParameters[7].Direction = ParameterDirection.Output;
				DataTable dataTable = this.db.GetDataTable(queryStr, "SP_CustomPage", SqlParameters, 120);
				total = (int)SqlParameters[7].Value;
				return dataTable;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", ex);
				}
			}
			return new DataTable(queryStr);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000038CC File Offset: 0x00001ACC
		public void Dispose()
		{
			this.db.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000008 RID: 8
		protected readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000009 RID: 9
		protected Sql_DbObject db = new Sql_DbObject("AppConfig", "crosszoneString");
	}
}

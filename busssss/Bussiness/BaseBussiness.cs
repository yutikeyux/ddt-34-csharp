using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;
using SqlDataProvider.BaseClass;

namespace Bussiness
{
	// Token: 0x02000006 RID: 6
	public class BaseBussiness : IDisposable
	{
		// Token: 0x0600001A RID: 26 RVA: 0x0000365B File Offset: 0x0000185B
		public BaseBussiness()
		{
			this.db = new Sql_DbObject("AppConfig", "conString");
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000367A File Offset: 0x0000187A
		public void Dispose()
		{
			this.db.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003690 File Offset: 0x00001890
		public DataTable GetPage(string queryStr, string queryWhere, int pageCurrent, int pageSize, string fdShow, string fdOreder, string fdKey, ref int total)
		{
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
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
				sqlParameters[7].Direction = ParameterDirection.Output;
				DataTable dataTable = this.db.GetDataTable(queryStr, "SP_CustomPage", sqlParameters, 120);
				total = (int)sqlParameters[7].Value;
				return dataTable;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			return new DataTable(queryStr);
		}

		// Token: 0x04000006 RID: 6
		protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000007 RID: 7
		protected Sql_DbObject db;
	}
}

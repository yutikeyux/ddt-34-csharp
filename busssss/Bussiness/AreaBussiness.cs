using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDataProvider.Data;

namespace Bussiness
{
	// Token: 0x02000004 RID: 4
	public class AreaBussiness : BaseCrossBussiness
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000030E0 File Offset: 0x000012E0
		public AreaConfigInfo[] GetAllAreaConfig()
		{
			List<AreaConfigInfo> areaConfigInfoList = new List<AreaConfigInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_AreaConfig_All");
				while (ResultDataReader.Read())
				{
					areaConfigInfoList.Add(this.InitAreaConfigInfo(ResultDataReader));
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("InitAreaConfigInfo", ex);
				}
			}
			finally
			{
				bool flag = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag)
				{
					ResultDataReader.Close();
				}
			}
			return areaConfigInfoList.ToArray();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000031A0 File Offset: 0x000013A0
		public AreaConfigInfo InitAreaConfigInfo(SqlDataReader dr)
		{
			return new AreaConfigInfo
			{
				AreaID = (int)dr["AreaID"],
				AreaServer = ((dr["AreaServer"] == null) ? "" : dr["AreaServer"].ToString()),
				AreaName = ((dr["AreaName"] == null) ? "" : dr["AreaName"].ToString()),
				DataSource = ((dr["DataSource"] == null) ? "" : dr["DataSource"].ToString()),
				Catalog = ((dr["Catalog"] == null) ? "" : dr["Catalog"].ToString()),
				UserID = ((dr["UserID"] == null) ? "" : dr["UserID"].ToString()),
				Password = ((dr["Password"] == null) ? "" : dr["Password"].ToString()),
				RequestUrl = ((dr["RequestUrl"] == null) ? "" : dr["RequestUrl"].ToString()),
				CrossChatAllow = (bool)dr["CrossChatAllow"],
				CrossPrivateChat = (bool)dr["CrossPrivateChat"],
				Version = ((dr["Version"] == null) ? "" : dr["Version"].ToString())
			};
		}
	}
}

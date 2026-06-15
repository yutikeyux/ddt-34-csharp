using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDataProvider.Data;

namespace Bussiness
{
	// Token: 0x02000019 RID: 25
	public class PveBussiness : BaseCrossBussiness
	{
		// Token: 0x06000245 RID: 581 RVA: 0x00031E9C File Offset: 0x0003009C
		public PveInfo[] GetAllPveInfos()
		{
			List<PveInfo> list = new List<PveInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_PveInfos_All");
				while (resultDataReader.Read())
				{
					PveInfo item = new PveInfo
					{
						ID = (int)resultDataReader["Id"],
						Name = ((resultDataReader["Name"] == null) ? "" : resultDataReader["Name"].ToString()),
						Type = (int)resultDataReader["Type"],
						LevelLimits = (int)resultDataReader["LevelLimits"],
						SimpleTemplateIds = ((resultDataReader["SimpleTemplateIds"] == null) ? "" : resultDataReader["SimpleTemplateIds"].ToString()),
						NormalTemplateIds = ((resultDataReader["NormalTemplateIds"] == null) ? "" : resultDataReader["NormalTemplateIds"].ToString()),
						HardTemplateIds = ((resultDataReader["HardTemplateIds"] == null) ? "" : resultDataReader["HardTemplateIds"].ToString()),
						TerrorTemplateIds = ((resultDataReader["TerrorTemplateIds"] == null) ? "" : resultDataReader["TerrorTemplateIds"].ToString()),
						Pic = ((resultDataReader["Pic"] == null) ? "" : resultDataReader["Pic"].ToString()),
						Description = ((resultDataReader["Description"] == null) ? "" : resultDataReader["Description"].ToString()),
						Ordering = (int)resultDataReader["Ordering"],
						AdviceTips = ((resultDataReader["AdviceTips"] == null) ? "" : resultDataReader["AdviceTips"].ToString()),
						SimpleGameScript = (resultDataReader["SimpleGameScript"] as string),
						NormalGameScript = (resultDataReader["NormalGameScript"] as string),
						HardGameScript = (resultDataReader["HardGameScript"] as string),
						TerrorGameScript = (resultDataReader["TerrorGameScript"] as string),
						BossFightNeedMoney = ((resultDataReader["BossFightNeedMoney"] == null) ? "" : resultDataReader["BossFightNeedMoney"].ToString()),
						LastFloor = ((resultDataReader["LastFloor"] == null) ? "1,1,1,1" : resultDataReader["LastFloor"].ToString())
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllPveInfos", exception);
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
	}
}

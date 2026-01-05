using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000077 RID: 119
	public class subactivelist : IHttpHandler
	{
		// Token: 0x0600021D RID: 541 RVA: 0x0000FE34 File Offset: 0x0000E034
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			XElement result = new XElement("Result");
			try
			{
				using (ActiveBussiness db = new ActiveBussiness())
				{
					SubActiveInfo[] Actives = db.GetAllSubActive();
					foreach (SubActiveInfo Active in Actives)
					{
						result.Add(FlashUtils.CreateActiveInfo(Active));
						SubActiveConditionInfo[] Conditions = db.GetAllSubActiveCondition(Active.ActiveID);
						foreach (SubActiveConditionInfo Condition in Conditions)
						{
							result.Add(FlashUtils.CreateActiveConditionInfo(Condition));
						}
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				subactivelist.log.Error("subactivelist", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("nowTime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000085 RID: 133
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

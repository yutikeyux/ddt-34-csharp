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
	// Token: 0x02000073 RID: 115
	public class subactivelist : IHttpHandler
	{
		// Token: 0x06000217 RID: 535 RVA: 0x00010448 File Offset: 0x0000E648
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "fail!";
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
					message = "Success!";
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
		// (get) Token: 0x06000218 RID: 536 RVA: 0x000105B8 File Offset: 0x0000E7B8
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400007F RID: 127
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

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
	// Token: 0x02000009 RID: 9
	public class activitysystemitems : IHttpHandler
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002F88 File Offset: 0x00001188
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(activitysystemitems.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002FD4 File Offset: 0x000011D4
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					ActivitySystemItemInfo[] infos = db.GetAllActivitySystemItem();
					foreach (ActivitySystemItemInfo info in infos)
					{
						result.Add(FlashUtils.CreateActivitySystemItems(info));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				activitysystemitems.log.Error("activitysystemitems", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			csFunction.CreateCompressXml(context, result, "activitysystemitems_out", false);
			return csFunction.CreateCompressXml(context, result, "activitysystemitems", true);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000030DC File Offset: 0x000012DC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000008 RID: 8
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

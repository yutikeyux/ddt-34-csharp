using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000035 RID: 53
	public class FarmGetUserFieldInfosSingle : IHttpHandler
	{
		// Token: 0x060000FB RID: 251 RVA: 0x000086C0 File Offset: 0x000068C0
		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string str = "Başarısız!";
			XElement node = new XElement("Result");
			string str2 = context.Request["friendID"];
			try
			{
				XElement xelement = new XElement("Item", new object[]
				{
					new XAttribute("UserID", str2),
					new XAttribute("isFeed", false)
				});
				node.Add(xelement);
				flag = true;
				str = "Başarılı!";
			}
			catch (Exception ex)
			{
				FarmGetUserFieldInfosSingle.log.Error("FarmGetUserFieldInfosSingle", ex);
			}
			node.Add(new XAttribute("value", flag));
			node.Add(new XAttribute("message", str));
			context.Response.ContentType = "text/plain";
			context.Response.Write(node.ToString(false));
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000087D0 File Offset: 0x000069D0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400003D RID: 61
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

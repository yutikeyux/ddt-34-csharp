using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;

namespace Tank.Request
{
	// Token: 0x0200000B RID: 11
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class AdvanceQuestTime : IHttpHandler
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00003220 File Offset: 0x00001420
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				int id = int.Parse(context.Request["useid"]);
				using (new PlayerBussiness())
				{
				}
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				AdvanceQuestTime.log.Error("IMListLoad", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(string.Format("0,{0},0", DateTime.Now));
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00003318 File Offset: 0x00001518
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400000A RID: 10
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

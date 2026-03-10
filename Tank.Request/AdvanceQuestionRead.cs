using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;

namespace Tank.Request
{
	// Token: 0x0200000A RID: 10
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class AdvanceQuestionRead : IHttpHandler
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00003108 File Offset: 0x00001308
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
				message = "Başarılı!";
			}
			catch (Exception ex)
			{
				AdvanceQuestionRead.log.Error("IMListLoad", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000031F4 File Offset: 0x000013F4
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000009 RID: 9
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

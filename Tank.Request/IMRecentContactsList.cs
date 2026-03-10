using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000041 RID: 65
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class IMRecentContactsList : IHttpHandler
	{
		// Token: 0x06000131 RID: 305 RVA: 0x0000A1AC File Offset: 0x000083AC
		public void ProcessRequest(HttpContext context)
		{
			XElement result = new XElement("Result");
			bool value = true;
			string message = "Başarılı!";
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000A234 File Offset: 0x00008434
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000047 RID: 71
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

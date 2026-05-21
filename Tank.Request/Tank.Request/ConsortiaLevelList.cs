using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000021 RID: 33
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ConsortiaLevelList : IHttpHandler
	{
		// Token: 0x06000086 RID: 134 RVA: 0x0000599D File Offset: 0x00003B9D
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Write(ConsortiaLevelList.build(context));
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000059C4 File Offset: 0x00003BC4
		public static string build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ConsortiaBussiness db = new ConsortiaBussiness())
				{
					ConsortiaLevelInfo[] infos = db.GetAllConsortiaLevel();
					foreach (ConsortiaLevelInfo info in infos)
					{
						result.Add(FlashUtils.CreateConsortiLevelInfo(info));
					}
				}
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				ConsortiaLevelList.log.Error("ConsortiaLevelList", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "ConsortiaLevelList", true);
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00005ABC File Offset: 0x00003CBC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400001E RID: 30
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

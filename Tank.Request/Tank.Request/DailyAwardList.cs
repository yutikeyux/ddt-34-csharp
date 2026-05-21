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
	// Token: 0x02000029 RID: 41
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class DailyAwardList : IHttpHandler
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00006EC0 File Offset: 0x000050C0
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(DailyAwardList.build(context));
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00006ED8 File Offset: 0x000050D8
		public static string build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					DailyAwardInfo[] infos = db.GetAllDailyAward();
					foreach (DailyAwardInfo info in infos)
					{
						result.Add(FlashUtils.CreateActiveInfo(info));
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				DailyAwardList.log.Error("Load DailyAwardList is Başarısız!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "DailyAwardList", true);
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00006FD0 File Offset: 0x000051D0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000026 RID: 38
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

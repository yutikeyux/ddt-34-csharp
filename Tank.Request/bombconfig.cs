using System;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200000F RID: 15
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class bombconfig : IHttpHandler
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00003A5C File Offset: 0x00001C5C
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(bombconfig.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003AA8 File Offset: 0x00001CA8
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					BallConfigInfo[] infos = db.GetAllBallConfig();
					foreach (BallConfigInfo info in infos)
					{
						result.Add(FlashUtils.CreateBallConfigInfo(info));
					}
				}
				value = true;
				message = "Başarılı!";
			}
			catch (Exception ex)
			{
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "bombconfig", true);
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003B90 File Offset: 0x00001D90
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

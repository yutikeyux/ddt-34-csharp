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
	// Token: 0x0200000E RID: 14
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class BallList : IHttpHandler
	{
		// Token: 0x06000036 RID: 54 RVA: 0x000038EC File Offset: 0x00001AEC
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(BallList.build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003938 File Offset: 0x00001B38
		public static string build(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					BallInfo[] infos = db.GetAllBall();
					foreach (BallInfo info in infos)
					{
						result.Add(FlashUtils.CreateBallInfo(info));
					}
				}
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				BallList.log.Error("BallList", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "BallList", true);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00003A30 File Offset: 0x00001C30
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400000D RID: 13
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

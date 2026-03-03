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
	// Token: 0x02000010 RID: 16
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CardUpdateCondition : IHttpHandler
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00003BA3 File Offset: 0x00001DA3
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CardUpdateCondition.Build(context));
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003BB8 File Offset: 0x00001DB8
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					CardUpdateConditionInfo[] infos = db.GetAllCardUpdateCondition();
					foreach (CardUpdateConditionInfo info in infos)
					{
						result.Add(FlashUtils.CreateCardUpdateCondition(info));
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				CardUpdateCondition.log.Error("Load CardUpdateCondition is fail!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "CardUpdateCondition", true);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003CB0 File Offset: 0x00001EB0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400000E RID: 14
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

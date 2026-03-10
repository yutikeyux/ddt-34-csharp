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
	// Token: 0x02000042 RID: 66
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ItemStrengthenList : IHttpHandler
	{
		// Token: 0x06000135 RID: 309 RVA: 0x0000A25D File Offset: 0x0000845D
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(ItemStrengthenList.Build(context));
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000A274 File Offset: 0x00008474
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					StrengthenInfo[] infos = db.GetAllStrengthen();
					foreach (StrengthenInfo info in infos)
					{
						result.Add(FlashUtils.CreateStrengthenInfo(info));
					}
				}
				value = true;
				message = "Başarılı!";
			}
			catch (Exception ex)
			{
				ItemStrengthenList.log.Error("ItemStrengthenList", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "ItemStrengthenList", true);
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000A36C File Offset: 0x0000856C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000048 RID: 72
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

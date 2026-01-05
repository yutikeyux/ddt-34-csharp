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
	// Token: 0x0200005F RID: 95
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class NPCInfoList : IHttpHandler
	{
		// Token: 0x060001AD RID: 429 RVA: 0x00002927 File Offset: 0x00000B27
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(NPCInfoList.Build(context));
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					NpcInfo[] infos = db.GetAllNPCInfo();
					foreach (NpcInfo info in infos)
					{
						result.Add(FlashUtils.CreatNPCInfo(info));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				NPCInfoList.log.Error("Load NPCInfoList is fail!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "NPCInfoList", true);
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000060 RID: 96
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

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
	// Token: 0x0200005D RID: 93
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class NPCInfoList : IHttpHandler
	{
		// Token: 0x060001AB RID: 427 RVA: 0x0000D92A File Offset: 0x0000BB2A
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(NPCInfoList.Build(context));
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000D940 File Offset: 0x0000BB40
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
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
					message = "Success!";
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
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000DA38 File Offset: 0x0000BC38
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400005F RID: 95
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

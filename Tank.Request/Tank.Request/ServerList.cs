using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using Bussiness.CenterService;
using log4net;
using Road.Flash;

namespace Tank.Request
{
	// Token: 0x0200006D RID: 109
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ServerList : IHttpHandler
	{
		// Token: 0x060001FB RID: 507 RVA: 0x0000F70C File Offset: 0x0000D90C
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			int total = 0;
			XElement result = new XElement("Result");
			try
			{
				using (CenterServiceClient temp = new CenterServiceClient())
				{
					IList<ServerData> list = temp.GetServerList();
					foreach (ServerData s in list)
					{
						bool flag = s.State == -1;
						if (!flag)
						{
							total += s.Online;
							result.Add(FlashUtils.CreateServerInfo(s.Id, s.Name, s.Ip, s.Port - 69, s.State, s.MustLevel, s.LowestLevel, s.Online));
						}
					}
				}
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				ServerList.log.Error("Load server list error:", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			result.Add(new XAttribute("total", total));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000072 RID: 114
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

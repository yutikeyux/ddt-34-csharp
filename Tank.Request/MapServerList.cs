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
	// Token: 0x02000057 RID: 87
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class MapServerList : IHttpHandler
	{
		// Token: 0x0600018F RID: 399 RVA: 0x0000CFC9 File Offset: 0x0000B1C9
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(MapServerList.Build(context));
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000CFE0 File Offset: 0x0000B1E0
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız";
			XElement result = new XElement("Result");
			try
			{
				using (MapBussiness db = new MapBussiness())
				{
					ServerMapInfo[] infos = db.GetAllServerMap();
					foreach (ServerMapInfo info in infos)
					{
						result.Add(FlashUtils.CreateMapServer(info));
					}
				}
				value = true;
				message = "Başarılı!";
			}
			catch (Exception ex)
			{
				MapServerList.log.Error("MapServerList", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "MapServerList", true);
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000D0D8 File Offset: 0x0000B2D8
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000059 RID: 89
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

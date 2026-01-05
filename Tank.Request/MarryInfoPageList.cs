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
	// Token: 0x0200005B RID: 91
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class MarryInfoPageList : IHttpHandler
	{
		// Token: 0x0600019A RID: 410 RVA: 0x0000D07C File Offset: 0x0000B27C
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			int total = 0;
			XElement result = new XElement("Result");
			try
			{
				int page = int.Parse(context.Request["page"]);
				string name = null;
				bool flag = context.Request["name"] != null;
				if (flag)
				{
					name = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["name"]));
				}
				bool sex = bool.Parse(context.Request["sex"]);
				int size = 12;
				using (PlayerBussiness db = new PlayerBussiness())
				{
					MarryInfo[] infos = db.GetMarryInfoPage(page, name, sex, size, ref total);
					foreach (MarryInfo info in infos)
					{
						XElement temp = FlashUtils.CreateMarryInfo(info);
						result.Add(temp);
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				MarryInfoPageList.log.Error("MarryInfoPageList", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400005C RID: 92
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

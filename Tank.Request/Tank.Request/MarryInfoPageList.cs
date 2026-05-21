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
	// Token: 0x02000059 RID: 89
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class MarryInfoPageList : IHttpHandler
	{
		// Token: 0x06000198 RID: 408 RVA: 0x0000D204 File Offset: 0x0000B404
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
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
					message = "Success!";
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
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000D3AC File Offset: 0x0000B5AC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400005B RID: 91
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

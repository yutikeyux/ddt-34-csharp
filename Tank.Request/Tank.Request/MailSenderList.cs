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
	// Token: 0x02000056 RID: 86
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class MailSenderList : IHttpHandler
	{
		// Token: 0x0600018B RID: 395 RVA: 0x0000CE60 File Offset: 0x0000B060
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				int id = int.Parse(context.Request.QueryString["selfID"]);
				bool flag = id != 0;
				if (flag)
				{
					using (PlayerBussiness db = new PlayerBussiness())
					{
						MailInfo[] sInfos = db.GetMailBySenderID(id);
						foreach (MailInfo info in sInfos)
						{
							result.Add(FlashUtils.CreateMailInfo(info, "Item"));
						}
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				MailSenderList.log.Error("MailSenderList", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.BinaryWrite(StaticFunction.Compress(result.ToString(false)));
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000CFA0 File Offset: 0x0000B1A0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000058 RID: 88
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

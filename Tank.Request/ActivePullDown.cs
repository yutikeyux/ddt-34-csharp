using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using Bussiness.CenterService;
using log4net;
using Road.Flash;

namespace Tank.Request
{
	// Token: 0x02000008 RID: 8
	public class ActivePullDown : IHttpHandler
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002D8D File Offset: 0x00000F8D
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002D90 File Offset: 0x00000F90
		public void ProcessRequest(HttpContext context)
		{
			string path = HttpContext.Current.Server.MapPath(".");
			path += "\\";
			LanguageMgr.Setup(path);
			int selfid = Convert.ToInt32(context.Request["selfid"]);
			int activeID = Convert.ToInt32(context.Request["activeID"]);
			string key = context.Request["key"];
			string activeKey = context.Request["activeKey"];
			bool value = false;
			string message = "Ödül Alma başarısız!";
			string awardID = "";
			XElement result = new XElement("Result");
			bool flag = activeKey != "";
			if (flag)
			{
				byte[] src = CryptoHelper.RsaDecryt2(StaticFunction.RsaCryptor, activeKey);
				awardID = Encoding.UTF8.GetString(src, 0, src.Length);
			}
			try
			{
				using (PlayerBussiness pb = new PlayerBussiness())
				{
					bool flag2 = pb.PullDown(activeID, awardID, selfid, ref message) == 0;
					if (flag2)
					{
						using (CenterServiceClient client = new CenterServiceClient())
						{
							client.MailNotice(selfid);
						}
					}
				}
				value = true;
				message = LanguageMgr.GetTranslation(message, Array.Empty<object>());
			}
			catch (Exception ex)
			{
				ActivePullDown.log.Error("ActivePullDown", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x04000007 RID: 7
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

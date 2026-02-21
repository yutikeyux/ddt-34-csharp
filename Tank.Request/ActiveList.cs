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
	// Token: 0x02000007 RID: 7
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ActiveList : IHttpHandler
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002C10 File Offset: 0x00000E10
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(ActiveList.Bulid(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002C5C File Offset: 0x00000E5C
		public static string Bulid(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
			XElement result = new XElement("Result");
			try
			{
				using (ActiveBussiness db = new ActiveBussiness())
				{
					ActiveInfo[] infos = db.GetAllActives();
					foreach (ActiveInfo info in infos)
					{
						result.Add(FlashUtils.CreateActiveInfo(info));
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				ActiveList.log.Error("Load Active is fail!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			csFunction.CreateCompressXml(context, result, "ActiveList_out", false);
			return csFunction.CreateCompressXml(context, result, "ActiveList", true);
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002D64 File Offset: 0x00000F64
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000006 RID: 6
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

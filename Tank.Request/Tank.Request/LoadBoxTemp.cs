using System;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000045 RID: 69
	public class LoadBoxTemp : IHttpHandler
	{
		// Token: 0x06000142 RID: 322 RVA: 0x0000A604 File Offset: 0x00008804
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(LoadBoxTemp.build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000A650 File Offset: 0x00008850
		public static string build(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					ItemBoxInfo[] itemBox = db.GetItemBoxInfos();
					foreach (ItemBoxInfo s in itemBox)
					{
						result.Add(FlashUtils.CreateItemBoxInfo(s));
					}
					value = true;
					message = "Success!";
				}
			}
			catch
			{
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "LoadBoxTemp", true);
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000A738 File Offset: 0x00008938
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

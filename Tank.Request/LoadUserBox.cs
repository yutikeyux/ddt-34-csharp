using System;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200004B RID: 75
	public class LoadUserBox : IHttpHandler
	{
		// Token: 0x0600015F RID: 351 RVA: 0x0000AE84 File Offset: 0x00009084
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(LoadUserBox.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000AED0 File Offset: 0x000090D0
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					UserBoxInfo[] itemBox = db.GetAllUserBox();
					foreach (UserBoxInfo s in itemBox)
					{
						result.Add(FlashUtils.CreateUserBoxInfo(s));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch
			{
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "LoadUserBox", true);
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000AFB8 File Offset: 0x000091B8
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

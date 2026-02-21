using System;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;

namespace Tank.Request
{
	// Token: 0x02000068 RID: 104
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class runetemplatelist : IHttpHandler
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00002D8D File Offset: 0x00000F8D
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000EDEC File Offset: 0x0000CFEC
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(runetemplatelist.build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000EE38 File Offset: 0x0000D038
		public static string build(HttpContext context)
		{
			bool flag = false;
			string str = "Fail!";
			XElement result = new XElement("Result");
			XElement xelement = new XElement("RuneTemplate");
			try
			{
				using (new ProduceBussiness())
				{
				}
				flag = true;
				str = "Success!";
			}
			catch (Exception)
			{
			}
			result.Add(new XAttribute("value", flag));
			result.Add(new XAttribute("message", str));
			result.Add(xelement);
			csFunction.CreateCompressXml(context, result, "runetemplatelist_out", false);
			return csFunction.CreateCompressXml(context, result, "runetemplatelist", true);
		}
	}
}

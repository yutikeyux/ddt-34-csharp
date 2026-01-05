using System;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;

namespace Tank.Request
{
	// Token: 0x0200006C RID: 108
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class runetemplatelist : IHttpHandler
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000215A File Offset: 0x0000035A
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000E8E0 File Offset: 0x0000CAE0
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(runetemplatelist.Build(context));
			}
			else
			{
				context.Response.Write("Tabi Efendim!");
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000E92C File Offset: 0x0000CB2C
		public static string Build(HttpContext context)
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

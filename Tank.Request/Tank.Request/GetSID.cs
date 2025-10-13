using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Web.SessionState;
using System.Xml.Linq;

namespace Tank.Request
{
	// Token: 0x02000038 RID: 56
	public class GetSID : IHttpHandler, IRequiresSessionState
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00008BDC File Offset: 0x00006DDC
		public void ProcessRequest(HttpContext context)
		{
			CspParameters csp = new CspParameters();
			csp.Flags = CspProviderFlags.UseMachineKeyStore;
			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
			rsa.FromXmlString(ConfigurationSettings.AppSettings["privateKey"]);
			RSAParameters para = rsa.ExportParameters(false);
			XElement node = new XElement("result", new object[]
			{
				new XAttribute("m1", Convert.ToBase64String(para.Modulus)),
				new XAttribute("m2", Convert.ToBase64String(para.Exponent))
			});
			context.Response.ContentType = "text/plain";
			context.Response.Write(node.ToString());
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00008C94 File Offset: 0x00006E94
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

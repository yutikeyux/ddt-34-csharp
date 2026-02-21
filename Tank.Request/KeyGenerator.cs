using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	// Token: 0x02000043 RID: 67
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class KeyGenerator : IHttpHandler
	{
		// Token: 0x0600013A RID: 314 RVA: 0x0000A398 File Offset: 0x00008598
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			CspParameters csp = new CspParameters();
			csp.Flags = CspProviderFlags.UseMachineKeyStore;
			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
			RSAParameters para = rsa.ExportParameters(true);
			StringBuilder model = new StringBuilder();
			for (int i = 0; i < para.Modulus.Length; i++)
			{
				model.Append(para.Modulus[i].ToString("X2"));
			}
			StringBuilder exponent = new StringBuilder();
			for (int j = 0; j < para.Exponent.Length; j++)
			{
				exponent.Append(para.Exponent[j].ToString("X2"));
			}
			XElement list = new XElement("list");
			XElement pri = new XElement("private", new XAttribute("key", rsa.ToXmlString(true)));
			XElement pub = new XElement("public", new object[]
			{
				new XAttribute("model", model.ToString()),
				new XAttribute("exponent", exponent.ToString())
			});
			list.Add(pri);
			list.Add(pub);
			context.Response.Write(list.ToString());
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000A508 File Offset: 0x00008708
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

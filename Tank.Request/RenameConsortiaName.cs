using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using Bussiness.Interface;
using log4net;
using Road.Flash;

namespace Tank.Request
{
	// Token: 0x0200006A RID: 106
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class RenameConsortiaName : IHttpHandler
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000215A File Offset: 0x0000035A
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000E4C0 File Offset: 0x0000C6C0
		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string translation = LanguageMgr.GetTranslation("Tank.Request.RenameConsortiaName.Fail1", Array.Empty<object>());
			XElement xelement = new XElement("Result");
			try
			{
				BaseInterface.CreateInterface();
				string str = context.Request["p"];
				bool flag2 = context.Request["site"] != null;
				if (flag2)
				{
					HttpUtility.UrlDecode(context.Request["site"]);
				}
				string userHostAddress = context.Request.UserHostAddress;
				bool flag3 = !string.IsNullOrEmpty(str);
				if (flag3)
				{
					byte[] bytes = CryptoHelper.RsaDecryt2(StaticFunction.RsaCryptor, str);
					string[] strArray = Encoding.UTF8.GetString(bytes, 7, bytes.Length - 7).Split(new char[]
					{
						','
					});
					bool flag4 = strArray.Length == 5;
					if (flag4)
					{
						string name = strArray[0];
						string pass = strArray[1];
						string pass2 = strArray[2];
						string text = strArray[3];
						string text2 = strArray[4];
						bool flag5 = PlayerManager.Login(name, pass);
						if (flag5)
						{
							using (new ConsortiaBussiness())
							{
								PlayerManager.Update(name, pass2);
								flag = true;
								translation = LanguageMgr.GetTranslation("Tank.Request.RenameConsortiaName.Success", Array.Empty<object>());
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				RenameConsortiaName.log.Error("RenameConsortiaName", ex);
				flag = false;
				translation = LanguageMgr.GetTranslation("Tank.Request.RenameConsortiaName.Fail2", Array.Empty<object>());
			}
			xelement.Add(new XAttribute("value", flag));
			xelement.Add(new XAttribute("message", translation));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xelement.ToString(false));
		}

		// Token: 0x04000073 RID: 115
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

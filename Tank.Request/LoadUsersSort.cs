using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200004F RID: 79
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class LoadUsersSort : IHttpHandler
	{
		// Token: 0x06000170 RID: 368 RVA: 0x0000B9B8 File Offset: 0x00009BB8
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			int total = 0;
			try
			{
				int page = 1;
				int size = 10;
				int order = int.Parse(context.Request["order"]);
				int userID = -1;
				bool resultValue = false;
				using (PlayerBussiness db = new PlayerBussiness())
				{
					PlayerInfo[] infos = db.GetPlayerPage(page, size, ref total, order, userID, ref resultValue);
					bool flag = resultValue;
					if (flag)
					{
						foreach (PlayerInfo info in infos)
						{
							XElement node = new XElement("Item", new object[]
							{
								new XAttribute("ID", info.ID),
								new XAttribute("NickName", (info.NickName == null) ? "" : info.NickName),
								new XAttribute("Grade", info.Grade),
								new XAttribute("Colors", (info.Colors == null) ? "" : info.Colors),
								new XAttribute("Skin", (info.Skin == null) ? "" : info.Skin),
								new XAttribute("Sex", info.Sex),
								new XAttribute("Style", (info.Style == null) ? "" : info.Style),
								new XAttribute("ConsortiaName", (info.ConsortiaName == null) ? "" : info.ConsortiaName),
								new XAttribute("Hide", info.Hide),
								new XAttribute("Offer", info.Offer),
								new XAttribute("ReputeOffer", info.ReputeOffer),
								new XAttribute("ConsortiaHonor", info.ConsortiaHonor),
								new XAttribute("ConsortiaLevel", info.ConsortiaLevel),
								new XAttribute("ConsortiaRepute", info.ConsortiaRepute),
								new XAttribute("WinCount", info.Win),
								new XAttribute("TotalCount", info.Total),
								new XAttribute("EscapeCount", info.Escape),
								new XAttribute("Repute", info.Repute),
								new XAttribute("GP", info.GP)
							});
							result.Add(node);
						}
						value = true;
						message = "Başarılı!";
					}
				}
			}
			catch (Exception ex)
			{
				LoadUsersSort.log.Error("LoadUsersSort", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000171 RID: 369 RVA: 0x0000BDCC File Offset: 0x00009FCC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000052 RID: 82
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

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
	// Token: 0x02000040 RID: 64
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class IMListLoad : IHttpHandler
	{
		// Token: 0x0600012D RID: 301 RVA: 0x00009D90 File Offset: 0x00007F90
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				int id = int.Parse(context.Request["id"]);
				using (PlayerBussiness db = new PlayerBussiness())
				{
					FriendInfo[] infos = db.GetFriendsAll(id);
					XElement node0 = new XElement("customList", new object[]
					{
						new XAttribute("ID", 0),
						new XAttribute("Name", "Arkadaşlar")
					});
					result.Add(node0);
					foreach (FriendInfo g in infos)
					{
						XElement node = new XElement("Item", new object[]
						{
							new XAttribute("ID", g.FriendID),
							new XAttribute("NickName", g.NickName),
							new XAttribute("Birthday", DateTime.Now),
							new XAttribute("ApprenticeshipState", 0),
							new XAttribute("LoginName", g.UserName),
							new XAttribute("Style", g.Style),
							new XAttribute("Sex", g.Sex == 1),
							new XAttribute("Colors", g.Colors),
							new XAttribute("Grade", g.Grade),
							new XAttribute("Hide", g.Hide),
							new XAttribute("ConsortiaName", g.ConsortiaName),
							new XAttribute("TotalCount", g.Total),
							new XAttribute("EscapeCount", g.Escape),
							new XAttribute("WinCount", g.Win),
							new XAttribute("Offer", g.Offer),
							new XAttribute("Relation", g.Relation),
							new XAttribute("Repute", g.Repute),
							new XAttribute("State", (g.State == 1) ? 1 : 0),
							new XAttribute("Nimbus", g.Nimbus),
							new XAttribute("DutyName", g.DutyName)
						});
						result.Add(node);
					}
				}
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				IMListLoad.log.Error("IMListLoad", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600012E RID: 302 RVA: 0x0000A180 File Offset: 0x00008380
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000046 RID: 70
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

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
	// Token: 0x0200000C RID: 12
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ApprenticeshipClubList : IHttpHandler
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002D8D File Offset: 0x00000F8D
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003344 File Offset: 0x00001544
		public void ProcessRequest(HttpContext context)
		{
			bool flag = true;
			string str = "true!";
			bool flag2 = false;
			bool flag3 = false;
			int num = 0;
			XElement xelement = new XElement("Result");
			try
			{
				int num2 = int.Parse(context.Request["page"]);
				int.Parse(context.Request["selfid"]);
				bool.Parse(context.Request["isReturnSelf"]);
				string str2 = (context.Request["name"] == null) ? "" : context.Request["name"];
				bool flag4 = bool.Parse(context.Request["appshipStateType"]);
				bool flag5 = bool.Parse(context.Request["requestType"]);
				int num3 = flag5 ? 9 : 3;
				int num4 = (!flag4) ? 1 : 2;
				int num5 = (!flag4) ? 8 : 10;
				int num6 = -1;
				bool flag6 = !flag5 && !flag4;
				if (flag6)
				{
					num4 = 3;
					num5 = 9;
				}
				else
				{
					bool flag7 = !flag5 && flag4;
					if (flag7)
					{
						num4 = 4;
						num5 = 9;
					}
				}
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					bool flag8 = str2 != null && str2.Length > 0;
					if (flag8)
					{
						PlayerInfo userSingleByNickName = playerBussiness.GetUserSingleByNickName(str2);
						num6 = ((userSingleByNickName != null) ? userSingleByNickName.ID : 0);
					}
					PlayerInfo[] playerPage = playerBussiness.GetPlayerPage(num2, num3, ref num, num5, num4, num6, ref flag);
					for (int i = 0; i < playerPage.Length; i++)
					{
						XElement apprenticeShipInfo = FlashUtils.CreateApprenticeShipInfo(playerPage[i]);
						xelement.Add(apprenticeShipInfo);
					}
					flag = true;
					str = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				ApprenticeshipClubList.log.Error(ex);
			}
			xelement.Add(new XAttribute("total", num));
			xelement.Add(new XAttribute("value", flag));
			xelement.Add(new XAttribute("message", str));
			xelement.Add(new XAttribute("isPlayerRegeisted", flag2));
			xelement.Add(new XAttribute("isSelfPublishEquip", flag3));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xelement.ToString(false));
		}

		// Token: 0x0400000B RID: 11
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

using System;
using System.Collections.Generic;
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
	// Token: 0x0200007E RID: 126
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class UserApprenticeshipInfoList : IHttpHandler
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000215A File Offset: 0x0000035A
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00010600 File Offset: 0x0000E800
		public void ProcessRequest(HttpContext context)
		{
			bool flag = true;
			string str = "true!";
			int num = 0;
			XElement xelement = new XElement("Result");
			try
			{
				int num2 = int.Parse(context.Request["selfid"]);
				int num3 = int.Parse(context.Request["RelationshipID"]);
				bool flag2 = num3 == 0;
				if (flag2)
				{
					num3 = num2;
				}
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(num3);
					PlayerInfo userSingleByUserId2 = playerBussiness.GetUserSingleByUserID(num2);
					bool flag3 = userSingleByUserId != null && userSingleByUserId2 != null;
					if (flag3)
					{
						bool flag4 = userSingleByUserId2.masterID == userSingleByUserId.ID;
						if (flag4)
						{
							XElement apprenticeshipInfo2 = FlashUtils.CreateUserApprenticeshipInfo(userSingleByUserId);
							xelement.Add(apprenticeshipInfo2);
						}
						foreach (KeyValuePair<int, string> item in userSingleByUserId.MasterOrApprenticesArr)
						{
							PlayerInfo userSingleByUserId3 = playerBussiness.GetUserSingleByUserID(item.Key);
							bool flag5 = userSingleByUserId3 != null && userSingleByUserId3.ID != num2;
							if (flag5)
							{
								XElement apprenticeshipInfo3 = FlashUtils.CreateUserApprenticeshipInfo(userSingleByUserId3);
								xelement.Add(apprenticeshipInfo3);
							}
						}
					}
					flag = true;
					str = "Success!";
				}
			}
			catch (Exception ex)
			{
				UserApprenticeshipInfoList.log.Error(ex);
			}
			xelement.Add(new XAttribute("total", num));
			xelement.Add(new XAttribute("value", flag));
			xelement.Add(new XAttribute("message", str));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xelement.ToString(false));
		}

		// Token: 0x0400008D RID: 141
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

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
	// Token: 0x02000034 RID: 52
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class FarmGetUserFieldInfos : IHttpHandler
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x000083EC File Offset: 0x000065EC
		private static int AccelerateTimeFields(DateTime PlantTime, int FieldValidDate)
		{
			DateTime now = DateTime.Now;
			int num = now.Hour - PlantTime.Hour;
			int num2 = now.Minute - PlantTime.Minute;
			bool flag = num < 0;
			if (flag)
			{
				num = 24 + num;
			}
			bool flag2 = num2 < 0;
			if (flag2)
			{
				num2 = 60 + num2;
			}
			int num3 = num * 60 + num2;
			bool flag3 = num3 > FieldValidDate;
			if (flag3)
			{
				num3 = FieldValidDate;
			}
			return num3;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00008458 File Offset: 0x00006658
		private static int AccelerateTimeFields(UserFieldInfo m_field)
		{
			int num = 0;
			bool flag = m_field != null && m_field.SeedID > 0;
			if (flag)
			{
				num = FarmGetUserFieldInfos.AccelerateTimeFields(m_field.PlantTime, m_field.FieldValidDate);
			}
			return num;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00008494 File Offset: 0x00006694
		public void ProcessRequest(HttpContext context)
		{
			int int32 = Convert.ToInt32(context.Request["selfid"]);
			string str = context.Request["key"];
			bool flag = true;
			string str2 = "Success!";
			XElement node = new XElement("Result");
			using (PlayerBussiness playerBussiness = new PlayerBussiness())
			{
				foreach (FriendInfo friendInfo in playerBussiness.GetFriendsAll(int32))
				{
					XElement xelement = new XElement("Item");
					foreach (UserFieldInfo singleField in playerBussiness.GetSingleFields(friendInfo.FriendID))
					{
						XElement xelement2 = new XElement("Item", new object[]
						{
							new XAttribute("SeedID", singleField.SeedID),
							new XAttribute("AcclerateDate", FarmGetUserFieldInfos.AccelerateTimeFields(singleField)),
							new XAttribute("GrowTime", singleField.PlantTime.ToString("yyyy-MM-ddTHH:mm:ss"))
						});
						xelement.Add(xelement2);
					}
					xelement.Add(new XAttribute("UserID", friendInfo.FriendID));
					node.Add(xelement);
				}
			}
			node.Add(new XAttribute("value", flag));
			node.Add(new XAttribute("message", str2));
			context.Response.ContentType = "text/plain";
			context.Response.Write(node.ToString(false));
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00008694 File Offset: 0x00006894
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400003C RID: 60
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

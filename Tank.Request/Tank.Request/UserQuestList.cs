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
	// Token: 0x0200007D RID: 125
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class UserQuestList : IHttpHandler
	{
		// Token: 0x06000241 RID: 577 RVA: 0x000111B8 File Offset: 0x0000F3B8
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				int userID = int.Parse(context.Request["ID"]);
				using (PlayerBussiness db = new PlayerBussiness())
				{
					QuestDataInfo[] quests = db.GetUserQuest(userID);
					foreach (QuestDataInfo quest in quests)
					{
						result.Add(FlashUtils.CreateQuestDataInfo(quest));
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				UserQuestList.log.Error("UserQuestList", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000112DC File Offset: 0x0000F4DC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400008A RID: 138
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

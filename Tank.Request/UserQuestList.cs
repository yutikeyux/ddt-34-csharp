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
	// Token: 0x02000081 RID: 129
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class UserQuestList : IHttpHandler
	{
		// Token: 0x06000247 RID: 583 RVA: 0x00010A24 File Offset: 0x0000EC24
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
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
					message = "Başarılı!";
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
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000090 RID: 144
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

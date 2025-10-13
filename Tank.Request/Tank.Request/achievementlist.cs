using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000006 RID: 6
	public class achievementlist : IHttpHandler
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002958 File Offset: 0x00000B58
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(achievementlist.Bulid(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000029A4 File Offset: 0x00000BA4
		public static string Bulid(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					AchievementInfo[] achives = db.GetALlAchievement();
					AchievementRewardInfo[] achivegoods = db.GetALlAchievementReward();
					AchievementConditionInfo[] achivecondiction = db.GetALlAchievementCondition();
					AchievementInfo[] array = achives;
					for (int i = 0; i < array.Length; i++)
					{
						AchievementInfo achive = array[i];
						XElement temp_xml = FlashUtils.CreateAchievement(achive);
						IEnumerable temp_questcondiction = from s in achivecondiction
						where s.AchievementID == achive.ID
						select s;
						foreach (object obj in temp_questcondiction)
						{
							AchievementConditionInfo item = (AchievementConditionInfo)obj;
							temp_xml.Add(FlashUtils.CreateAchievementCondition(item));
						}
						IEnumerable temp_questgoods = from s in achivegoods
						where s.AchievementID == achive.ID
						select s;
						foreach (object obj2 in temp_questgoods)
						{
							AchievementRewardInfo item2 = (AchievementRewardInfo)obj2;
							temp_xml.Add(FlashUtils.CreateAchievementReward(item2));
						}
						result.Add(temp_xml);
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				achievementlist.log.Error("achievementlist", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			csFunction.CreateCompressXml(context, result, "achievementlist_out", false);
			return csFunction.CreateCompressXml(context, result, "achievementlist", true);
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002BE4 File Offset: 0x00000DE4
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000005 RID: 5
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

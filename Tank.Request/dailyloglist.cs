using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;
using SqlDataProvider.BaseClass;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200002C RID: 44
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[WebService(Namespace = "http://tempuri.org/")]
	public class dailyloglist : IHttpHandler
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00002D8D File Offset: 0x00000F8D
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000072FC File Offset: 0x000054FC
		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string str = "Fail!";
			XElement node = new XElement("Result");
			try
			{
				string text = context.Request["key"];
				int UserID = int.Parse(context.Request["selfid"]);
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					DailyLogListInfo info = playerBussiness.GetDailyLogListSingle(UserID);
					bool flag2 = info == null;
					if (flag2)
					{
						info = new DailyLogListInfo
						{
							UserID = UserID,
							DayLog = "",
							UserAwardLog = 0,
							LastDate = DateTime.Now
						};
					}
					string str2 = info.DayLog;
					int num = info.UserAwardLog;
					DateTime dateTime = info.LastDate;
					char[] chArray = new char[]
					{
						','
					};
					int length = str2.Split(chArray).Length;
					int month = DateTime.Now.Month;
					int year = DateTime.Now.Year;
					int day = DateTime.Now.Day;
					int num2 = DateTime.DaysInMonth(year, month);
					bool flag3 = month != dateTime.Month || year != dateTime.Year;
					if (flag3)
					{
						str2 = "";
						num = 0;
						dateTime = DateTime.Now;
					}
					bool flag4 = length < num2;
					if (flag4)
					{
						bool flag5 = string.IsNullOrEmpty(str2) && length > 1;
						if (flag5)
						{
							str2 = "False";
						}
						for (int index = length; index < day - 1; index++)
						{
							str2 += ",False";
						}
					}
					info.DayLog = str2;
					info.UserAwardLog = num;
					info.LastDate = dateTime;
					playerBussiness.UpdateDailyLogList(info);
					XElement xelement = new XElement("DailyLogList", new object[]
					{
						new XAttribute("UserAwardLog", num),
						new XAttribute("DayLog", str2),
						new XAttribute("luckyNum", 0),
						new XAttribute("myLuckyNum", 0)
					});
					node.Add(xelement);
				}
				flag = true;
				str = "Success!";
			}
			catch (Exception ex)
			{
				dailyloglist.log.Error("dailyloglist", ex);
			}
			node.Add(new XAttribute("value", flag));
			node.Add(new XAttribute("message", str));
			node.Add(new XAttribute("nowDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			context.Response.ContentType = "text/plain";
			context.Response.BinaryWrite(StaticFunction.Compress(node.ToString(false)));
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00007628 File Offset: 0x00005828
		public bool UpdateDailyLogList(DailyLogListInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@UserAwardLog", info.UserAwardLog),
					new SqlParameter("@DayLog", info.DayLog),
					new SqlParameter("@LastDate", info.LastDate),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[4].Direction = ParameterDirection.ReturnValue;
				flag = this.db.RunProcedure("SP_DailyLogList_Update", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = dailyloglist.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					dailyloglist.log.Error("SP_DailyLogList_Update", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x04000029 RID: 41
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400002A RID: 42
		protected Sql_DbObject db = new Sql_DbObject("AppConfig", "conString");
	}
}

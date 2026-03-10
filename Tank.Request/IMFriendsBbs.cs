using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200003E RID: 62
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class IMFriendsBbs : IHttpHandler
	{
		// Token: 0x06000125 RID: 293 RVA: 0x000096C0 File Offset: 0x000078C0
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			IMFriendsBbs.IAgentFriends friendsClass = new IMFriendsBbs.Normal();
			StringBuilder friendUserName = new StringBuilder();
			HttpContext current = HttpContext.Current;
			string getFriendsBbsXml = friendsClass.FriendsString(current.Request.Params["Uid"]);
			DataSet Ds = new DataSet();
			bool flag = getFriendsBbsXml != "";
			if (flag)
			{
				try
				{
					Ds.ReadXml(new StringReader(getFriendsBbsXml));
					for (int i = 0; i < Ds.Tables["item"].DefaultView.Count; i++)
					{
						friendUserName.Append(Ds.Tables["item"].DefaultView[i]["UserName"].ToString() + ",");
					}
				}
				catch (Exception ex)
				{
					bool isErrorEnabled = IMFriendsBbs.log.IsErrorEnabled;
					if (isErrorEnabled)
					{
						IMFriendsBbs.log.Error("Get Table Item ", ex);
					}
				}
			}
			bool flag2 = friendUserName.Length <= 1 || getFriendsBbsXml == "";
			if (flag2)
			{
				result.Add(new XAttribute("value", value));
				result.Add(new XAttribute("message", message));
				context.Response.ContentType = "text/plain";
				context.Response.Write(result.ToString(false));
			}
			else
			{
				string[] friends = friendUserName.ToString().Split(new char[]
				{
					','
				});
				ArrayList condictArray = new ArrayList();
				StringBuilder tempString = new StringBuilder(4000);
				for (int j = 0; j < friends.Count<string>(); j++)
				{
					bool flag3 = friends[j] == "";
					if (flag3)
					{
						break;
					}
					bool flag4 = tempString.Length + friends[j].Length < 4000;
					if (flag4)
					{
						tempString.Append(friends[j] + ",");
					}
					else
					{
						condictArray.Add(tempString.ToString());
						tempString.Remove(0, tempString.Length);
					}
				}
				condictArray.Add(tempString.ToString());
				try
				{
					for (int k = 0; k < condictArray.Count; k++)
					{
						string temp = condictArray[k].ToString();
						using (PlayerBussiness db = new PlayerBussiness())
						{
							FriendInfo[] friendsResult = db.GetFriendsBbs(temp);
							for (int l = 0; l < friendsResult.Count<FriendInfo>(); l++)
							{
								DataRow[] dr = Ds.Tables["item"].Select("UserName='" + friendsResult[l].UserName + "'");
								XElement node = new XElement("Item", new object[]
								{
									new XAttribute("NickName", friendsResult[l].NickName),
									new XAttribute("UserName", friendsResult[l].UserName),
									new XAttribute("UserId", friendsResult[l].UserID),
									new XAttribute("Photo", (dr[0]["Photo"] == null) ? "" : dr[0]["Photo"].ToString()),
									new XAttribute("PersonWeb", (dr[0]["PersonWeb"] == null) ? "" : dr[0]["PersonWeb"].ToString()),
									new XAttribute("IsExist", friendsResult[l].IsExist),
									new XAttribute("OtherName", (dr[0]["OtherName"] == null) ? "" : dr[0]["OtherName"].ToString())
								});
								result.Add(node);
							}
						}
					}
					value = true;
					message = "Başarılı!";
				}
				catch (Exception ex2)
				{
					IMFriendsBbs.log.Error("IMFriendsGood", ex2);
				}
				result.Add(new XAttribute("value", value));
				result.Add(new XAttribute("message", message));
				context.Response.ContentType = "text/plain";
				context.Response.Write(result.ToString(false));
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00009BF0 File Offset: 0x00007DF0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000044 RID: 68
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x02000098 RID: 152
		public interface IAgentFriends
		{
			// Token: 0x060002CB RID: 715
			string FriendsString(string uid);
		}

		// Token: 0x02000099 RID: 153
		public class Normal : IMFriendsBbs.IAgentFriends
		{
			// Token: 0x060002CC RID: 716 RVA: 0x00012DD4 File Offset: 0x00010FD4
			public string FriendsString(string uid)
			{
				try
				{
					bool flag = IMFriendsBbs.Normal.FriendInterface == "";
					if (flag)
					{
						return string.Empty;
					}
					string ok = "";
					this.Url = string.Format(CultureInfo.InvariantCulture, IMFriendsBbs.Normal.FriendInterface, uid);
					string webAccept = WebsResponse.GetPage(this.Url, "", "utf-8", out ok);
					bool flag2 = ok == "";
					if (flag2)
					{
						return webAccept;
					}
					throw new Exception(ok);
				}
				catch (Exception ex)
				{
					bool isErrorEnabled = IMFriendsBbs.log.IsErrorEnabled;
					if (isErrorEnabled)
					{
						IMFriendsBbs.log.Error("Normal：", ex);
					}
				}
				return string.Empty;
			}

			// Token: 0x17000092 RID: 146
			// (get) Token: 0x060002CD RID: 717 RVA: 0x00012E94 File Offset: 0x00011094
			public static string FriendInterface
			{
				get
				{
					return ConfigurationSettings.AppSettings["FriendInterface"];
				}
			}

			// Token: 0x040000AD RID: 173
			private string Url;
		}
	}
}

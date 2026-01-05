using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Xml.Linq;
using Bussiness;
using Bussiness.Interface;
using Game.Base;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000052 RID: 82
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class Login : IHttpHandler, IRequiresSessionState
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0000280F File Offset: 0x00000A0F
		public static string ValidDate
		{
			get
			{
				return ConfigurationManager.AppSettings["ValidDate"];
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000215A File Offset: 0x0000035A
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000BE08 File Offset: 0x0000A008
		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string translation = LanguageMgr.GetTranslation("Tank.Request.Login.Fail1", Array.Empty<object>());
			bool flag2 = false;
			XElement xelement = new XElement("Result");
			string text = context.Request["p"];
			try
			{
				BaseInterface baseInterface = BaseInterface.CreateInterface();
				string site = (context.Request["site"] == null) ? "" : HttpUtility.UrlDecode(context.Request["site"]);
				string userHostAddress = context.Request.UserHostAddress;
				if (!string.IsNullOrEmpty(text))
				{
					byte[] array = CryptoHelper.RsaDecryt2(StaticFunction.RsaCryptor, text);
					string[] array2 = Encoding.UTF8.GetString(array, 7, array.Length - 7).Split(new char[]
					{
						','
					});
					if (array2.Length == 4)
					{
						string name = array2[0];
						string text2 = array2[1];
						string text3 = array2[2];
						string nickname = array2[3];
						int num = 0;
						bool flag3 = false;
						bool byUserIsFirst = PlayerManager.GetByUserIsFirst(name);
						PlayerInfo playerInfo = baseInterface.CreateLogin(name, text3, int.Parse(ConfigurationManager.AppSettings["ServerID"]), ref translation, ref num, userHostAddress, ref flag2, byUserIsFirst, ref flag3, site, nickname);
						if (flag3)
						{
							StaticsMgr.RegCountAdd();
						}
						if (playerInfo != null && !flag2)
						{
							if (num == 0)
							{
								PlayerManager.Update(name, text3);
							}
							else
							{
								PlayerManager.Remove(name);
							}
							string value = string.IsNullOrEmpty(playerInfo.Style) ? ",,,,,,,," : playerInfo.Style;
							playerInfo.Colors = (string.IsNullOrEmpty(playerInfo.Colors) ? ",,,,,,,," : playerInfo.Colors);
							XElement content = new XElement("Item", new object[]
							{
								new XAttribute("ID", playerInfo.ID),
								new XAttribute("IsFirst", num),
								new XAttribute("NickName", playerInfo.NickName),
								new XAttribute("Date", ""),
								new XAttribute("IsConsortia", 0),
								new XAttribute("ConsortiaID", playerInfo.ConsortiaID),
								new XAttribute("Sex", playerInfo.Sex),
								new XAttribute("WinCount", playerInfo.Win),
								new XAttribute("TotalCount", playerInfo.Total),
								new XAttribute("EscapeCount", playerInfo.Escape),
								new XAttribute("DutyName", (playerInfo.DutyName == null) ? "" : playerInfo.DutyName),
								new XAttribute("GP", playerInfo.GP),
								new XAttribute("Honor", ""),
								new XAttribute("Style", value),
								new XAttribute("Gold", playerInfo.Gold),
								new XAttribute("Colors", (playerInfo.Colors == null) ? "" : playerInfo.Colors),
								new XAttribute("Attack", playerInfo.Attack),
								new XAttribute("Defence", playerInfo.Defence),
								new XAttribute("Agility", playerInfo.Agility),
								new XAttribute("Luck", playerInfo.Luck),
								new XAttribute("Grade", playerInfo.Grade),
								new XAttribute("Hide", playerInfo.Hide),
								new XAttribute("Repute", playerInfo.Repute),
								new XAttribute("ConsortiaName", (playerInfo.ConsortiaName == null) ? "" : playerInfo.ConsortiaName),
								new XAttribute("Offer", playerInfo.Offer),
								new XAttribute("Skin", (playerInfo.Skin == null) ? "" : playerInfo.Skin),
								new XAttribute("ReputeOffer", playerInfo.ReputeOffer),
								new XAttribute("ConsortiaHonor", playerInfo.ConsortiaHonor),
								new XAttribute("ConsortiaLevel", playerInfo.ConsortiaLevel),
								new XAttribute("ConsortiaRepute", playerInfo.ConsortiaRepute),
								new XAttribute("Money", playerInfo.Money + playerInfo.MoneyLock),
								new XAttribute("AntiAddiction", playerInfo.AntiAddiction),
								new XAttribute("IsMarried", playerInfo.IsMarried),
								new XAttribute("SpouseID", playerInfo.SpouseID),
								new XAttribute("SpouseName", (playerInfo.SpouseName == null) ? "" : playerInfo.SpouseName),
								new XAttribute("MarryInfoID", playerInfo.MarryInfoID),
								new XAttribute("IsCreatedMarryRoom", playerInfo.IsCreatedMarryRoom),
								new XAttribute("IsGotRing", playerInfo.IsGotRing),
								new XAttribute("LoginName", (playerInfo.UserName == null) ? "" : playerInfo.UserName),
								new XAttribute("Nimbus", playerInfo.Nimbus),
								new XAttribute("FightPower", playerInfo.FightPower),
								new XAttribute("AnswerSite", playerInfo.AnswerSite),
								new XAttribute("WeaklessGuildProgressStr", (playerInfo.WeaklessGuildProgressStr == null) ? "" : playerInfo.WeaklessGuildProgressStr),
								new XAttribute("IsOldPlayer", false)
							});
							xelement.Add(content);
							flag = true;
							translation = LanguageMgr.GetTranslation("Tank.Request.Login.Success", Array.Empty<object>());
						}
						else
						{
							Login.log.Error("PlayerManager.Remove(name)");
							PlayerManager.Remove(name);
						}
					}
				}
			}
			catch (Exception ex)
			{
				byte[] array3 = Convert.FromBase64String(text);
				Login.log.Error("User Login error: (--" + StaticFunction.RsaCryptor.KeySize.ToString() + "--)" + ex.ToString());
				Login.log.Error("--dataarray: " + Marshal.ToHexDump("kötü giriş algılandı" + array3.Length.ToString(), array3));
				flag = false;
				translation = LanguageMgr.GetTranslation("Tank.Request.Login.Fail2", Array.Empty<object>());
			}
			finally
			{
				xelement.Add(new XAttribute("value", flag));
				xelement.Add(new XAttribute("message", translation));
				context.Response.ContentType = "text/plain";
				context.Response.Write(xelement.ToString(false));
			}
		}

		// Token: 0x04000054 RID: 84
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

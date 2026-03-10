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
	// Token: 0x0200004C RID: 76
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class LoadUserEquip : IHttpHandler
	{
		// Token: 0x06000163 RID: 355 RVA: 0x0000AFCC File Offset: 0x000091CC
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				int userid = int.Parse(context.Request["ID"]);
				using (PlayerBussiness pb = new PlayerBussiness())
				{
					PlayerInfo info = pb.GetUserSingleByUserID(userid);
					result.Add(new object[]
					{
						new XAttribute("Agility", info.Agility),
						new XAttribute("Attack", info.Attack),
						new XAttribute("Colors", info.Colors),
						new XAttribute("Skin", info.Skin),
						new XAttribute("Defence", info.Defence),
						new XAttribute("GP", info.GP),
						new XAttribute("Grade", info.Grade),
						new XAttribute("Luck", info.Luck),
						new XAttribute("Hide", info.Hide),
						new XAttribute("Repute", info.Repute),
						new XAttribute("Offer", info.Offer),
						new XAttribute("NickName", info.NickName),
						new XAttribute("ConsortiaName", info.ConsortiaName),
						new XAttribute("ConsortiaID", info.ConsortiaID),
						new XAttribute("ReputeOffer", info.ReputeOffer),
						new XAttribute("ConsortiaHonor", info.ConsortiaHonor),
						new XAttribute("ConsortiaLevel", info.ConsortiaLevel),
						new XAttribute("ConsortiaRepute", info.ConsortiaRepute),
						new XAttribute("WinCount", info.Win),
						new XAttribute("TotalCount", info.Total),
						new XAttribute("EscapeCount", info.Escape),
						new XAttribute("Sex", info.Sex),
						new XAttribute("Style", info.Style),
						new XAttribute("FightPower", info.FightPower)
					});
					ItemInfo[] items = pb.GetUserEquip(userid).ToArray();
					foreach (ItemInfo g in items)
					{
						result.Add(FlashUtils.CreateGoodsInfo(g));
					}
				}
				value = true;
				message = "Başarılı!";
			}
			catch (Exception ex)
			{
				LoadUserEquip.log.Error("LoadUserEquip", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000B3EC File Offset: 0x000095EC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400004F RID: 79
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

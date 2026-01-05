using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request.CelebList
{
	// Token: 0x0200008F RID: 143
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByDayBestEquip : IHttpHandler
	{
		// Token: 0x06000296 RID: 662 RVA: 0x00002E5A File Offset: 0x0000105A
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByDayBestEquip.Build(context));
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00011BF4 File Offset: 0x0000FDF4
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByDayGPList Fail!";
			}
			else
			{
				result = CelebByDayBestEquip.Build();
			}
			return result;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00011C2C File Offset: 0x0000FE2C
		public static string Build()
		{
			bool value = false;
			string message = "Hata!";
			XElement result = new XElement("Result");
			try
			{
				using (PlayerBussiness db = new PlayerBussiness())
				{
					BestEquipInfo[] infos = db.GetCelebByDayBestEquip();
					foreach (BestEquipInfo info in infos)
					{
						result.Add(FlashUtils.CreateBestEquipInfo(info));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				CelebByDayBestEquip.log.Error("Load CelebByDayBestEquip is fail!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(result, "CelebForBestEquip", false);
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000299 RID: 665 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A4 RID: 164
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

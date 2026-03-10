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
	// Token: 0x0200008B RID: 139
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByDayBestEquip : IHttpHandler
	{
		// Token: 0x06000290 RID: 656 RVA: 0x0001265D File Offset: 0x0001085D
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByDayBestEquip.Build(context));
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00012674 File Offset: 0x00010874
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByDayGPList Başarısız!";
			}
			else
			{
				result = CelebByDayBestEquip.Build();
			}
			return result;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000126AC File Offset: 0x000108AC
		public static string Build()
		{
			bool value = false;
			string message = "Başarısız!";
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
				CelebByDayBestEquip.log.Error("Load CelebByDayBestEquip is Başarısız!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(result, "CelebForBestEquip", false);
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000293 RID: 659 RVA: 0x000127A4 File Offset: 0x000109A4
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400009E RID: 158
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

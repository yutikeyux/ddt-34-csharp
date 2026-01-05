using System;
using System.Text;
using System.Web;
using System.Web.Services;

namespace Tank.Request.CelebList
{
	// Token: 0x02000097 RID: 151
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CreateAllCeleb : IHttpHandler
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x00011FA8 File Offset: 0x000101A8
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				StringBuilder Build = new StringBuilder();
				Build.Append(CelebByGpList.Build());
				Build.Append(CelebByDayGPList.Build());
				Build.Append(CelebByWeekGPList.Build());
				Build.Append(CelebByOfferList.Build());
				Build.Append(CelebByDayOfferList.Build());
				Build.Append(CelebByWeekOfferList.Build());
				Build.Append(CelebByDayFightPowerList.Build());
				Build.Append(CelebByConsortiaRiches.Build());
				Build.Append(CelebByConsortiaDayRiches.Build());
				Build.Append(CelebByConsortiaWeekRiches.Build());
				Build.Append(CelebByConsortiaHonor.Build());
				Build.Append(CelebByConsortiaDayHonor.Build());
				Build.Append(CelebByConsortiaWeekHonor.Build());
				Build.Append(CelebByConsortiaLevel.Build());
				Build.Append(CelebByDayBestEquip.Build());
				Build.Append(celebbyconsortiafightpower.Build());
				context.Response.ContentType = "text/plain";
				context.Response.Write(Build.ToString());
			}
			else
			{
				context.Response.Write("Tabi Efendim!" + context.Request.UserHostAddress);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

using System;
using System.Text;
using System.Web;
using System.Web.Services;

namespace Tank.Request.CelebList
{
	// Token: 0x02000093 RID: 147
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CreateAllCeleb : IHttpHandler
	{
		// Token: 0x060002C0 RID: 704 RVA: 0x00012C14 File Offset: 0x00010E14
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				StringBuilder build = new StringBuilder();
				build.Append(CelebByGpList.Build());
				build.Append(CelebByDayGPList.Build());
				build.Append(CelebByWeekGPList.Build());
				build.Append(CelebByOfferList.Build());
				build.Append(CelebByDayOfferList.Build());
				build.Append(CelebByWeekOfferList.Build());
				build.Append(CelebByDayFightPowerList.Build());
				build.Append(CelebByConsortiaRiches.Build());
				build.Append(CelebByConsortiaDayRiches.Build());
				build.Append(CelebByConsortiaWeekRiches.Build());
				build.Append(CelebByConsortiaHonor.Build());
				build.Append(CelebByConsortiaDayHonor.Build());
				build.Append(CelebByConsortiaWeekHonor.Build());
				build.Append(CelebByConsortiaLevel.Build());
				build.Append(CelebByDayBestEquip.Build());
				build.Append(celebbyconsortiafightpower.Build());
				context.Response.ContentType = "text/plain";
				context.Response.Write(build.ToString());
			}
			else
			{
				context.Response.Write("IP loglarda xD!" + context.Request.UserHostAddress);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x00012D4C File Offset: 0x00010F4C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

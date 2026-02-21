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
				StringBuilder bulid = new StringBuilder();
				bulid.Append(CelebByGpList.Build());
				bulid.Append(CelebByDayGPList.Build());
				bulid.Append(CelebByWeekGPList.Build());
				bulid.Append(CelebByOfferList.Build());
				bulid.Append(CelebByDayOfferList.Build());
				bulid.Append(CelebByWeekOfferList.Build());
				bulid.Append(CelebByDayFightPowerList.Build());
				bulid.Append(CelebByConsortiaRiches.Build());
				bulid.Append(CelebByConsortiaDayRiches.Build());
				bulid.Append(CelebByConsortiaWeekRiches.Build());
				bulid.Append(CelebByConsortiaHonor.Build());
				bulid.Append(CelebByConsortiaDayHonor.Build());
				bulid.Append(CelebByConsortiaWeekHonor.Build());
				bulid.Append(CelebByConsortiaLevel.Build());
				bulid.Append(CelebByDayBestEquip.Build());
				bulid.Append(celebbyconsortiafightpower.Build());
				context.Response.ContentType = "text/plain";
				context.Response.Write(bulid.ToString());
			}
			else
			{
				context.Response.Write("IP is not valid!" + context.Request.UserHostAddress);
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

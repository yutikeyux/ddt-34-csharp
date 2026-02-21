using System;
using System.Text;
using System.Web;
using System.Web.Services;

namespace Tank.Request
{
	// Token: 0x02000025 RID: 37
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CreateAllXml : IHttpHandler
	{
		// Token: 0x06000097 RID: 151 RVA: 0x000060DC File Offset: 0x000042DC
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				StringBuilder build = new StringBuilder();
				build.Append(ActiveList.build(context));
				build.Append(BallList.build(context));
				build.Append(LoadMapsItems.build(context));
				build.Append(LoadPVEItems.Build(context));
				build.Append(QuestList.build(context));
				build.Append(TemplateAllList.build(context));
				build.Append(ShopItemList.build(context));
				build.Append(LoadItemsCategory.build(context));
				build.Append(ItemStrengthenList.build(context));
				build.Append(MapServerList.build(context));
				build.Append(ConsortiaLevelList.build(context));
				build.Append(DailyAwardList.build(context));
				build.Append(NPCInfoList.build(context));
				build.Append(LoginAwardItemTemplate.build(context));
				build.Append(eventrewarditemlist.build(context));
				build.Append(serverconfig.build(context));
				build.Append(ShopGoodsShowList.build(context));
				build.Append(newtitle.build(context));
				build.Append(petskillelementinfo.build(context));
				build.Append(petskillinfo.build(context));
				build.Append(petskilltemplateinfo.build(context));
				build.Append(pettemplateinfo.build(context));
				build.Append(CardUpdateCondition.build(context));
				build.Append(CardUpdateInfo.build(context));
				build.Append(activitysystemitems.Build(context));
				build.Append(suittemplateinfolist.Build(context));
				build.Append(DailyLeagueLevelList.Build(context));
				build.Append(DailyLeagueAwardList.Build(context));
				context.Response.ContentType = "text/plain";
				context.Response.Write(build.ToString());
			}
			else
			{
				context.Response.Write("IP loglarda XD" + context.Request.UserHostAddress);
            }
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000062B0 File Offset: 0x000044B0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

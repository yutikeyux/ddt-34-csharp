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
				build.Append(ActiveList.Build(context));
				build.Append(BallList.Build(context));
				build.Append(LoadMapsItems.Build(context));
				build.Append(LoadPVEItems.Build(context));
				build.Append(QuestList.Build(context));
				build.Append(TemplateAllList.Build(context));
				build.Append(ShopItemList.Build(context));
				build.Append(LoadItemsCategory.Build(context));
				build.Append(ItemStrengthenList.Build(context));
				build.Append(MapServerList.Build(context));
				build.Append(ConsortiaLevelList.Build(context));
				build.Append(DailyAwardList.Build(context));
				build.Append(NPCInfoList.Build(context));
				build.Append(LoginAwardItemTemplate.Build(context));
				build.Append(eventrewarditemlist.Build(context));
				build.Append(serverconfig.Build(context));
				build.Append(ShopGoodsShowList.Build(context));
				build.Append(newtitle.Build(context));
				build.Append(petskillelementinfo.Build(context));
				build.Append(petskillinfo.Build(context));
				build.Append(petskilltemplateinfo.Build(context));
				build.Append(pettemplateinfo.Build(context));
				build.Append(CardUpdateCondition.Build(context));
				build.Append(CardUpdateInfo.Build(context));
				build.Append(activitysystemitems.Build(context));
				build.Append(suittemplateinfolist.Build(context));
				build.Append(DailyLeagueLevelList.Build(context));
				build.Append(DailyLeagueAwardList.Build(context));
				context.Response.ContentType = "text/plain";
				context.Response.Write(build.ToString());
			}
			else
			{
				context.Response.Write("IP is not valid!");
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

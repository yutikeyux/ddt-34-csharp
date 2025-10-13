using System;
using System.Web;
using Bussiness;

namespace Tank.Flash
{
	// Token: 0x02000009 RID: 9
	public class CreatShortCut : IHttpHandler
	{
		// Token: 0x06000018 RID: 24 RVA: 0x0000246C File Offset: 0x0000066C
		public void ProcessRequest(HttpContext context)
		{
			try
			{
				string str = context.Request.QueryString["gameurl"];
				string text = context.Request.UserAgent.ToUpper();
				string text2 = LanguageMgr.GetTranslation("Game.ProductionName", new object[0]) + ".url";
				if (text.Contains("MS") && text.Contains("IE"))
				{
					text2 = HttpUtility.UrlEncode(text2);
				}
				else if (text.Contains("FIREFOX"))
				{
					text2 = "\"" + text2 + "\"";
				}
				else
				{
					text2 = HttpUtility.UrlEncode(text2);
				}
				context.Response.ContentType = "application/octet-stream;";
				context.Response.AddHeader("Content-Disposition", "attachment;filename=" + text2);
				context.Response.Write("[InternetShortcut]\n");
				context.Response.Write("URL=" + str + "\n");
				context.Response.Write("IDList=\n");
				context.Response.Write("IconFile=\n");
				context.Response.Write("IconIndex=1\n");
				context.Response.Write("[{000214A0-0000-0000-C000-000000000046}]\n");
				context.Response.Write("Prop3=19,2\n");
				context.ApplicationInstance.CompleteRequest();
			}
			catch (Exception arg)
			{
				context.Response.Write("Error:" + arg);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000025F0 File Offset: 0x000007F0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

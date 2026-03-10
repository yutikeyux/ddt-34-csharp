using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using log4net;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000037 RID: 55
	public class fightlabdropitemlist : IHttpHandler
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00008980 File Offset: 0x00006B80
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(fightlabdropitemlist.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000089CC File Offset: 0x00006BCC
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				int[] copyids = new int[]
				{
					10000,
					10001,
					10002,
					10010,
					10011,
					10012,
					10020,
					10021,
					10022,
					10030,
					10031,
					10032,
					10040,
					10041,
					10042
				};
				using (ProduceBussiness db = new ProduceBussiness())
				{
					DropItem[] infos = db.GetAllDropItems();
					foreach (DropItem info in infos)
					{
						bool flag = copyids.Contains(info.DropId);
						if (flag)
						{
							result.Add(new XElement("Item", new object[]
							{
								new XAttribute("ID", info.DropId.ToString().Substring(0, 4)),
								new XAttribute("Easy", info.DropId.ToString().Substring(4, 1)),
								new XAttribute("AwardItem", info.ItemId),
								new XAttribute("Count", info.BeginData)
							}));
						}
					}
				}
				value = true;
				message = "Başarılı!";
			}
			catch (Exception ex)
			{
				fightlabdropitemlist.log.Error("fightlabdropitemlist", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "fightlabdropitemlist_out", false);
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00008BB0 File Offset: 0x00006DB0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400003F RID: 63
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

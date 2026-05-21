using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200003D RID: 61
	public class gmtipallbyids : IHttpHandler
	{
		// Token: 0x06000121 RID: 289 RVA: 0x0000950C File Offset: 0x0000770C
		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string str = "Başarısız!";
			XElement node = new XElement("Result");
			try
			{
				string str2 = context.Request["ids"];
				string[] strArray = null;
				bool flag2 = !string.IsNullOrEmpty(str2);
				if (flag2)
				{
					strArray = str2.Split(new char[]
					{
						','
					});
				}
				bool flag3 = strArray == null;
				if (!flag3)
				{
					using (ProduceBussiness produceBussiness = new ProduceBussiness())
					{
						foreach (EdictumInfo info in produceBussiness.GetAllEdictum())
						{
							DateTime date = info.EndDate.Date;
							DateTime date2 = DateTime.Now.Date;
							bool flag4 = date > date2;
							if (flag4)
							{
								node.Add(FlashUtils.CreateEdictum(info));
							}
						}
						flag = true;
						str = "Success!";
					}
				}
			}
			catch (Exception ex)
			{
				str = ex.ToString();
			}
			finally
			{
				node.Add(new XAttribute("value", flag));
				node.Add(new XAttribute("message", str));
				context.Response.ContentType = "image/jpeg";
				context.Response.Write(node.ToString(false));
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00009694 File Offset: 0x00007894
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000043 RID: 67
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

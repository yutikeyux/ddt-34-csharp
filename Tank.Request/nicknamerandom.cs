using System;
using System.Web;
using System.Xml.Linq;
using Bussiness;

namespace Tank.Request
{
	// Token: 0x0200009A RID: 154
	public class nicknamerandom : IHttpHandler
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x000120E0 File Offset: 0x000102E0
		public void ProcessRequest(HttpContext context)
		{
			try
			{
				string str = "unknown";
				XElement xelement = new XElement("Result");
				int sex = Convert.ToInt32(context.Request["sex"]);
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					str = playerBussiness.GetSingleRandomName(sex);
				}
				new Random();
				string value = str + " ";
				xelement.Add(new XAttribute("name", value));
				context.Response.ContentType = "text/plain";
				context.Response.Write(xelement.ToString());
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000215A File Offset: 0x0000035A
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Text;
using Bussiness.Interface;
using Road.Flash;

namespace Tank.Flash
{
	public partial class logingame : System.Web.UI.Page
	{
		public string LoginOnUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["LoginOnUrl"];
			}
		}

		public static string FlashUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["FlashUrl"];
			}
		}
		public static string loginurl
		{
			get
			{
				return ConfigurationManager.AppSettings["LoginUrl"];
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["username"] == null && string.IsNullOrEmpty(Session["username"].ToString()))
			{
				base.Response.Redirect("~/Login.htm");
			}
			string result2 = "";
			try
			{
				string name = Session["username"].ToString();
				string password2 = Guid.NewGuid().ToString();
				string time = BaseInterface.ConvertDateTimeInt(DateTime.Now).ToString();
				string key = string.Empty;
				if (string.IsNullOrEmpty(key))
				{
					key = BaseInterface.GetLoginKey;
				}
				string v = BaseInterface.md5(name + password2 + time.ToString() + key);
				string Url = BaseInterface.LoginUrl + "?content=" + HttpUtility.UrlEncode(name + "|" + password2 + "|" + time.ToString() + "|" + v);
				result2 = BaseInterface.RequestContent(Url);
				if (result2 == "0")
				{
					string flashUrl = FlashUrl + "?user=" + HttpUtility.UrlEncode(name) + "&key=" + HttpUtility.UrlEncode(password2) + "&editby=CyrusTeam";
					if ("1" == ConfigurationManager.AppSettings["content2"])
					{
						password2 = ((Session["password"] == null) ? password2 : Session["password"].ToString());
						string iv = "5C90D3C2C576A773";
						string keyEncode = "5628eb9a3485fbf61f51927b8a8eee03c5962c6b64847aeb";
						string origin = name + "|" + password2;
						flashUrl = FlashUrl + "?content2=" + CryptoHelper.TripleDesEncrypt(keyEncode, origin, ref iv);
					}
					base.Response.Redirect(flashUrl, false);
				}
				else
				{
					base.Response.Write(result2);
				}
			}
			catch (Exception ex)
			{
				base.Response.Write(ex.ToString());
			}
		}
	}
}
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Tank.Request.Illegalcharacters;

namespace Tank.Request
{
	// Token: 0x02000080 RID: 128
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class VisualizeRegister : IHttpHandler
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000250 RID: 592 RVA: 0x000118C8 File Offset: 0x0000FAC8
		public static string IllegalCharacters
		{
			get
			{
				return ConfigurationManager.AppSettings["IllegalCharacters"];
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000251 RID: 593 RVA: 0x000118EC File Offset: 0x0000FAEC
		public static string IllegalDirectory
		{
			get
			{
				return ConfigurationManager.AppSettings["IllegalDirectory"];
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00011910 File Offset: 0x0000FB10
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Fail1", Array.Empty<object>());
			XElement result = new XElement("Result");
			try
			{
				NameValueCollection para = context.Request.Params;
				string name = para["Name"];
				string pass = para["Pass"];
				string nickName = para["NickName"].Trim().Replace(",", "");
				string armColor = para["Arm"];
				string hairColor = para["Hair"];
				string faceColor = para["Face"];
				string ClothColor = para["Cloth"];
				string HatColor = para["Cloth"];
				string armID = para["ArmID"];
				string hairID = para["HairID"];
				string faceID = para["FaceID"];
				string ClothID = para["ClothID"];
				string HatID = para["ClothID"];
				int sex = -1;
				bool flag = bool.Parse(ConfigurationManager.AppSettings["MustSex"]);
				if (flag)
				{
					sex = (bool.Parse(para["Sex"]) ? 1 : 0);
				}
				bool flag2 = Encoding.Default.GetByteCount(nickName) <= 14;
				if (flag2)
				{
					bool flag3 = !VisualizeRegister.fileIllegal.checkIllegalChar(nickName);
					if (flag3)
					{
						bool flag4 = !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(nickName);
						if (flag4)
						{
							string[] styles = (sex == 1) ? ConfigurationManager.AppSettings["BoyVisualizeItem"].Split(new char[]
							{
								';'
							}) : ConfigurationManager.AppSettings["GrilVisualizeItem"].Split(new char[]
							{
								';'
							});
							armID = styles[0].Split(new char[]
							{
								','
							})[0];
							hairID = styles[0].Split(new char[]
							{
								','
							})[1];
							faceID = styles[0].Split(new char[]
							{
								','
							})[2];
							ClothID = styles[0].Split(new char[]
							{
								','
							})[3];
							HatID = styles[0].Split(new char[]
							{
								','
							})[4];
							armColor = "";
							hairColor = "";
							faceColor = "";
							ClothColor = "";
							HatColor = "";
							using (PlayerBussiness db = new PlayerBussiness())
							{
								string style = string.Concat(new string[]
								{
									armID,
									",",
									hairID,
									",",
									faceID,
									",",
									ClothID,
									",",
									HatID
								});
								bool flag5 = db.RegisterPlayer(name, pass, nickName, style, style, armColor, hairColor, faceColor, ClothColor, HatColor, sex, ref message, int.Parse(ConfigurationManager.AppSettings["ValidDate"]));
								if (flag5)
								{
									value = true;
									message = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Success", Array.Empty<object>());
								}
							}
						}
						else
						{
							message = LanguageMgr.GetTranslation("!string.IsNullOrEmpty(name) && !", Array.Empty<object>());
						}
					}
					else
					{
						message = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Illegalcharacters", Array.Empty<object>());
					}
				}
				else
				{
					message = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Long", Array.Empty<object>());
				}
			}
			catch (Exception ex)
			{
				VisualizeRegister.log.Error("VisualizeRegister", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00011D14 File Offset: 0x0000FF14
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400008F RID: 143
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000090 RID: 144
		private static FileSystem fileIllegal = new FileSystem(HttpContext.Current.Server.MapPath(VisualizeRegister.IllegalCharacters), HttpContext.Current.Server.MapPath(VisualizeRegister.IllegalDirectory), "*.txt");
	}
}

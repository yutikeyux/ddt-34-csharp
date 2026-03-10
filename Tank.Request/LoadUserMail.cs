using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200004E RID: 78
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class LoadUserMail : IHttpHandler
	{
		// Token: 0x0600016B RID: 363 RVA: 0x0000B570 File Offset: 0x00009770
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				int id = int.Parse(context.Request.QueryString["selfid"]);
				bool flag = id != 0;
				if (flag)
				{
					using (PlayerBussiness db = new PlayerBussiness())
					{
						MailInfo[] infos = db.GetMailByUserID(id);
						foreach (MailInfo info in infos)
						{
							XElement node = new XElement("Item", new object[]
							{
								new XAttribute("ID", info.ID),
								new XAttribute("Title", info.Title),
								new XAttribute("Content", info.Content),
								new XAttribute("Sender", info.Sender),
								new XAttribute("SendTime", info.SendTime.ToString("yyyy-MM-dd HH:mm:ss")),
								new XAttribute("Gold", info.Gold),
								new XAttribute("Money", info.Money),
								new XAttribute("Annex1ID", (info.Annex1 == null) ? "" : info.Annex1),
								new XAttribute("Annex2ID", (info.Annex2 == null) ? "" : info.Annex2),
								new XAttribute("Annex3ID", (info.Annex3 == null) ? "" : info.Annex3),
								new XAttribute("Annex4ID", (info.Annex4 == null) ? "" : info.Annex4),
								new XAttribute("Annex5ID", (info.Annex5 == null) ? "" : info.Annex5),
								new XAttribute("Type", info.Type),
								new XAttribute("ValidDate", info.ValidDate),
								new XAttribute("IsRead", info.IsRead)
							});
							LoadUserMail.AddAnnex(node, info.Annex1);
							LoadUserMail.AddAnnex(node, info.Annex2);
							LoadUserMail.AddAnnex(node, info.Annex3);
							LoadUserMail.AddAnnex(node, info.Annex4);
							LoadUserMail.AddAnnex(node, info.Annex5);
							result.Add(node);
						}
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				LoadUserMail.log.Error("LoadUserMail", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.BinaryWrite(StaticFunction.Compress(result.ToString(false)));
		}

        // Token: 0x0600016C RID: 364 RVA: 0x0000B924 File Offset: 0x00009B24
        public static void AddAnnex(XElement node, string value)
        {
            using (PlayerBussiness pb = new PlayerBussiness())
            {
                // Değer boş değilse devam et
                if (!string.IsNullOrEmpty(value))
                {
                    int itemId;
                    // GÜVENLİ DÖNÜŞTÜRME: Değer gerçekten bir sayı mı diye kontrol et.
                    // Eğer "11025:10" gibi bir metin gelirse TryParse false döner ve çökmez, sadece atlar.
                    if (int.TryParse(value, out itemId))
                    {
                        ItemInfo pr = pb.GetUserItemSingle(itemId);
                        if (pr != null)
                        {
                            node.Add(FlashUtils.CreateGoodsInfo(pr));
                        }
                    }
                    else
                    {
                        // Hatalı formatı loglayabilirsiniz ama sistem çökmeyecek.
                        log.ErrorFormat("AddAnnex Error: Annex value '{0}' is not a valid Item ID format.", value);
                    }
                }
            }
        }

        // Token: 0x17000050 RID: 80
        // (get) Token: 0x0600016D RID: 365 RVA: 0x0000B98C File Offset: 0x00009B8C
        public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000051 RID: 81
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

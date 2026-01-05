using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using Road.Flash; // Flash istemcisi yardımcı kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x02000050 RID: 80
    // LoadUserMail sınıfı, kullanıcının maillerini ve maillere ekli dosyaları (Ekler) yüklemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoadUserMail : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600016D RID: 365 RVA: 0x0000B5D8 File Offset: 0x00009158
        // Gelen isteği karşılayan ve kullanıcı maillerini hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Kullanıcı ID'sini al (QueryString'den alıyor)
                int userID = int.Parse(context.Request.QueryString["selfid"]);

                // Kullanıcı ID'si 0 değilse işlem yap
                bool isValidUser = (userID != 0);
                if (isValidUser)
                {
                    // Veritabanı işlemleri için bağlantı oluştur
                    using (PlayerBussiness pb = new PlayerBussiness())
                    {
                        // Kullanıcının maillerini veritabanından çek
                        MailInfo[] mailList = pb.GetMailByUserID(userID);

                        // Her bir maili döngüye al
                        foreach (MailInfo mailInfo in mailList)
                        {
                            // --- 1. MAIL ANA NODE'UNU OLUŞTUR ---
                            XElement mailNode = new XElement("Item", new object[]
                            {
                                new XAttribute("ID", mailInfo.ID),
                                new XAttribute("Title", mailInfo.Title),
                                new XAttribute("Content", mailInfo.Content),
                                new XAttribute("Sender", mailInfo.Sender),
                                new XAttribute("SendTime", mailInfo.SendTime.ToString("yyyy-MM-dd HH:mm:ss")),
                                new XAttribute("Gold", mailInfo.Gold),
                                new XAttribute("Money", mailInfo.Money),
                                // Eklerin (Attachment) ID'leri. Eğer null ise boş string gönder.
                                new XAttribute("Annex1ID", (mailInfo.Annex1 == null) ? "" : mailInfo.Annex1),
                                new XAttribute("Annex2ID", (mailInfo.Annex2 == null) ? "" : mailInfo.Annex2),
                                new XAttribute("Annex3ID", (mailInfo.Annex3 == null) ? "" : mailInfo.Annex3),
                                new XAttribute("Annex4ID", (mailInfo.Annex4 == null) ? "" : mailInfo.Annex4),
                                new XAttribute("Annex5ID", (mailInfo.Annex5 == null) ? "" : mailInfo.Annex5),
                                new XAttribute("Type", mailInfo.Type),
                                new XAttribute("ValidDate", mailInfo.ValidDate),
                                new XAttribute("IsRead", mailInfo.IsRead)
                            });

                            // --- 2. EKİLERİ (ATTACHMENTS) ÇEKİP EKLE ---
                            // Her ek (Annex) için ayrı ayrı çağrı yapılır.
                            // Bu metodlar, Ekin ID'sini kullanarak eşyanın detaylarını bulur ve mailNode'unun içine ekler.
                            LoadUserMail.AddAnnex(mailNode, mailInfo.Annex1);
                            LoadUserMail.AddAnnex(mailNode, mailInfo.Annex2);
                            LoadUserMail.AddAnnex(mailNode, mailInfo.Annex3);
                            LoadUserMail.AddAnnex(mailNode, mailInfo.Annex4);
                            LoadUserMail.AddAnnex(mailNode, mailInfo.Annex5);

                            // Hazırlanan mail node'unu ana listeye ekle
                            resultXml.Add(mailNode);
                        }
                    }
                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                LoadUserMail.log.Error("LoadUserMail yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) Binary olarak ekrana yaz
            context.Response.ContentType = "text/plain";
            context.Response.BinaryWrite(StaticFunction.Compress(resultXml.ToString(false)));
        }

        // Token: 0x0600016E RID: 366 RVA: 0x0000B98C File Offset: 0x0000958C
        // Ekin (Attachment) detaylarını getirip XML'e ekleyen statik yardımcı metod
        public static void AddAnnex(XElement parentMailNode, string annexId)
        {
            // Eğer ekin ID'si doluysa işlem yap
            bool isAnnexValid = !string.IsNullOrEmpty(annexId);

            if (isAnnexValid)
            {
                // Veritabanı işlemleri için bağlantı oluştur
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Veritabanından ekiliği (ItemInfo) çek
                    ItemInfo annexItem = pb.GetUserItemSingle(int.Parse(annexId));

                    // Eğer eşya bulunduysa, XML oluştur ve mail node'una ekle
                    bool isItemFound = (annexItem != null);
                    if (isItemFound)
                    {
                        // Eşya bilgisini (TemplateID, Count vb.) XML formatına çevirip ana düğüme ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        parentMailNode.Add(FlashUtils.CreateGoodsInfo(annexItem));
                    }
                }
            }
        }

        // Token: 0x17000050 RID: 80
        // (get) Token: 0x0600016F RID: 367 RVA: 0x00003828 File Offset: 0x00001A28
        // IHttpHandler arayüzünün zorunlu üyesi.
        // False döndürmek, bu sınıfın bir pool (havuz) içinde tekrar kullanılmayacağını belirtir.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
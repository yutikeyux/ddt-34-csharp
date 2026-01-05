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
    // Token: 0x0200003D RID: 61
    // gmtipallbyids sınıfı, belirli harita tiplerini (Edictum) ID'leri üzerinden getirmek için kullanılan bir HTTP Handler'dır.
    public class gmtipallbyids : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600011F RID: 287 RVA: 0x00009770 File Offset: 0x00007970
        // Gelen isteği karşılayan ve filtreleme yapan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!"; // Varsayılan hata mesajı

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL VE PARÇALA ---
                string idsString = context.Request["ids"];
                string[] idArray = null;

                bool isStringProvided = !string.IsNullOrEmpty(idsString);
                if (isStringProvided)
                {
                    // Virgülle ayrılmış ID'leri (Ör: "1,2,3") diziye çevir
                    idArray = idsString.Split(new char[] { ',' });
                }

                // --- 2. VERİTABANI İŞLEMLERİ ---
                bool isIdsProvided = (idArray != null);
                if (isIdsProvided)
                {
                    using (ProduceBussiness db = new ProduceBussiness())
                    {
                        // Tüm Harita/Edictum bilgilerini veritabanından çek
                        EdictumInfo[] allMapTypes = db.GetAllEdictum();

                        // Her bir harita tipini döngüye al
                        foreach (EdictumInfo mapInfo in allMapTypes)
                        {
                            // --- KRİTİK VE ŞAŞIRTICI MANTIK ---
                            // Orijinal kodda burada bir mantık hatası veya özel bir durum var:
                            // Gelen ID'lerin İLKİNE (ids[0]) bakıp, her harita objesinin ID'sini buna eşitliyor.
                            // Örneğin: Input "1,2,3" olsa bile, sonuçta tüm haritaların ID'si "1" olur.
                            // Muhtemelen kodun amacı "Sadece şu ID'ye sahip haritaları" listelemekken
                            // yanlışlık yapmış olmasıdır. Orijinal mantığı koruyoruz.
                            if (idArray.Length > 0)
                            {
                                mapInfo.ID = int.Parse(idArray[0]);
                            }

                            // --- 3. TARİH KONTROLÜ ---
                            // Haritanın bitiş tarihi (EndDate) bugünden ileride mi kontrol et.
                            // Sadece henüz bitmemiş veya bugün bitenler listeye eklenir.
                            bool isValid = mapInfo.EndDate.Date > DateTime.Now.Date;
                            if (isValid)
                            {
                                // Harita bilgisini XML formatına çevirip sonuç listesine ekle
                                resultXml.Add(FlashUtils.CreateEdictum(mapInfo));
                            }
                        }

                        isSuccess = true;
                        message = "Başarılı!";
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa mesajı exception mesajı yapıyor.
                // GÜVENLİK NOT: İstemciye doğrudan ex.ToString() göndermek güvenlik açısı olabilir (sunucu yolu vs).
                // Ancak orijinal kod bu mantığı kullandığı için bırakıyoruz.
                message = ex.ToString();
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Yanıtı ekrana yaz
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x1700003E RID: 62
        // (get) Token: 0x06000120 RID: 288 RVA: 0x00003828 File Offset: 0x00001A28
        // IHttpHandler arayüzünün zorunlu üyesi.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using SqlDataProvider.BaseClass; // Veritabanı temel sınıfları
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x0200002D RID: 45
    // dailyloglist sınıfı, oyuncuların günlük giriş/ödül loglarını kontrol eden ve güncelleyen bir HTTP Handler'dır.
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [WebService(Namespace = "http://tempuri.org/")]
    public class dailyloglist : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0400002B RID: 43
        // Bu sınıf içindeki veritabanı bağlantı nesnesi.
        // NOT: ProcessRequest metodu bunu kullanmaz, PlayerBussiness kullanır.
        // Bu nesne, alttaki UpdateDailyLogList metodu için tanımlanmış gibi görünüyor.
        protected Sql_DbObject db = new Sql_DbObject("AppConfig", "conString");

        // Token: 0x17000029 RID: 41
        // (get) Token: 0x060000C4 RID: 196 RVA: 0x0000215A File Offset: 0x0000035A
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Token: 0x060000C5 RID: 197 RVA: 0x00007BFC File Offset: 0x00005DFC
        // Gelen isteği karşılayan ve günlük listesini hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL ---
                string key = context.Request["key"]; // Gelen key (Kullanılmıyor, ama alınıyor)
                int userID = int.Parse(context.Request["selfid"]);

                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    // --- 2. MEVCUT LOGU ÇEK ---
                    DailyLogListInfo logInfo = playerBussiness.GetDailyLogListSingle(userID);

                    // Eğer kullanıcı daha önce hiç giriş yapmadıysa boş bir nesne oluştur
                    if (logInfo == null)
                    {
                        logInfo = new DailyLogListInfo
                        {
                            UserID = userID,
                            DayLog = "", // Günlükler boş
                            UserAwardLog = 0, // Ödül sayısı 0
                            LastDate = DateTime.Now
                        };
                    }

                    // --- 3. MANTIK: GÜNLERİ EŞİTLE VE GÜNCELLE ---
                    string dayLogString = logInfo.DayLog;
                    int userAwardLog = logInfo.UserAwardLog;
                    DateTime lastDate = logInfo.LastDate;

                    // Virgülle ayrılmış günlerin sayısı
                    char[] separator = new char[] { ',' };
                    int logCount = dayLogString.Split(separator).Length;

                    // Tarih bilgileri
                    int month = DateTime.Now.Month;
                    int year = DateTime.Now.Year;
                    int day = DateTime.Now.Day;
                    int daysInMonth = DateTime.DaysInMonth(year, month);

                    // KONTROL: Son kayıt tarihi ile bugünün ay/yılı aynı mı?
                    // Farklıysa (Ay veya Yıl değişmişse), listeyi sıfırla.
                    bool isNewMonth = (month != lastDate.Month || year != lastDate.Year);

                    if (isNewMonth)
                    {
                        dayLogString = "";
                        userAwardLog = 0;
                        lastDate = DateTime.Now;
                    }

                    // KONTROL: Logdaki kayıt sayısı, aydaki gün sayısından az mı?
                    // Örn: Bugün 20'si ama logda sadece 10 gün varsa arayı doldur.
                    bool isLogShorterThanDay = (logCount < daysInMonth);

                    if (isLogShorterThanDay)
                    {
                        // Eğer boşsa ama sayı fazla garip bir durum varsa başlat
                        bool shouldInitialize = string.IsNullOrEmpty(dayLogString) && logCount > 1;
                        if (shouldInitialize)
                        {
                            dayLogString = "False";
                        }

                        // Eksik günleri (Dünden öncekileri) doldur
                        for (int i = logCount; i < day - 1; i++)
                        {
                            dayLogString += ",False";
                        }
                    }

                    // --- 4. VERİTABANI GÜNCELLE ---
                    // Değişen listeyi güncelle
                    logInfo.DayLog = dayLogString;
                    logInfo.UserAwardLog = userAwardLog;
                    logInfo.LastDate = lastDate;

                    // PlayerBussiness üzerinden güncelle
                    playerBussiness.UpdateDailyLogList(logInfo);

                    // --- 5. YANIT XML'İNİ OLUŞTUR ---
                    // Not: luckyNum ve myLuckyNum değerleri burada sabit 0 gönderiliyor
                    XElement dailyLogNode = new XElement("DailyLogList", new object[]
                    {
                        new XAttribute("UserAwardLog", userAwardLog),
                        new XAttribute("DayLog", dayLogString),
                        new XAttribute("luckyNum", 0),
                        new XAttribute("myLuckyNum", 0)
                    });
                    resultXml.Add(dailyLogNode);
                }
                isSuccess = true;
                message = "Başarılı!";
            }
            catch (Exception ex)
            {
                dailyloglist.log.Error("dailyloglist yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value, message) ekleyip sıkıştır
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));
            resultXml.Add(new XAttribute("nowDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            context.Response.ContentType = "text/plain";
            context.Response.BinaryWrite(StaticFunction.Compress(resultXml.ToString(false)));
        }

        // Token: 0x060000C6 RID: 198 RVA: 0x00007F28 File Offset: 0x00006128
        // Bu metot, veritabanındaki Store Procedure'i doğrudan çalıştıran bir yapıdır.
        // NOT: ProcessRequest metodu bunu kullanmaz, PlayerBussiness.UpdateDailyLogList() kullanır.
        // Ancak sınıf içinde olduğu için muhtemelen PlayerBussiness içinde de bu yapıya referans vardır.
        public bool UpdateDailyLogList(DailyLogListInfo logInfo)
        {
            bool isSuccess = false;
            bool result;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@UserID", logInfo.UserID),
                    new SqlParameter("@UserAwardLog", logInfo.UserAwardLog),
                    new SqlParameter("@DayLog", logInfo.DayLog),
                    new SqlParameter("@LastDate", logInfo.LastDate),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                // Son parametre, Stored Procedure'in dönüş değeridir (ReturnValue)
                sqlParameters[4].Direction = ParameterDirection.ReturnValue;

                // Sql_DbObject (bu sınıfın içindeki örneği) üzerinden prosedürü çalıştır
                isSuccess = this.db.RunProcedure("SP_DailyLogList_Update", sqlParameters);
                result = isSuccess;
            }
            catch (Exception exception)
            {
                if (dailyloglist.log.IsErrorEnabled)
                {
                    dailyloglist.log.Error("SP_DailyLogList_Update hatası:", exception);
                    result = isSuccess;
                }
                else
                {
                    result = isSuccess;
                }
            }
            return result;
        }
    }
}
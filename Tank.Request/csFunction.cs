using System;
using System.Configuration; // 'ConfigurationManager' yerine güncel sınıf
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using Road.Flash; // Flash istemcisi yardımcı kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x02000029 RID: 41
    // csFunction sınıfı, projede kullanılan statik (static) yardımcı metodların toplandığı yerdir.
    public class csFunction
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x17000025 RID: 37
        // (get) Token: 0x060000A6 RID: 166 RVA: 0x000035C4 File Offset: 0x000017C4
        public static string GetAdminIP
        {
            get
            {
                return ConfigurationManager.AppSettings["AdminIP"];
            }
        }

        // Token: 0x060000A7 RID: 167 RVA: 0x00006E0C File Offset: 0x0000500C
        public static bool ValidAdminIP(string ip)
        {
            string adminIPs = csFunction.GetAdminIP;
            return string.IsNullOrEmpty(adminIPs) || adminIPs.Split(new char[] { '|' }).Contains(ip);
        }

        // Token: 0x060000A8 RID: 168 RVA: 0x00006E50 File Offset: 0x00005050
        public static string ConvertSql(string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return "";

            // 1. Küçük harfe çevir ve boşlukları temizle
            inputString = inputString.Trim().ToLower();

            // 2. Zararlı karakterleri veya sıralamaları temizle
            inputString = inputString.Replace("'", "''"); // Tırnak karakterlerini escape et
            inputString = inputString.Replace(";--", ""); // SQL yorum karakteri
            inputString = inputString.Replace("=", "");   // Eşittir operatörü
            inputString = inputString.Replace(" or", "");  // OR operatörü (sınırlar hariç)
            inputString = inputString.Replace(" or ", "");
            inputString = inputString.Replace(" and", ""); // AND operatörü
            inputString = inputString.Replace("and ", "");

            // 3. Zararlı anahtar kelimeleri (select, insert vb.) içeriyor mu kontrol et
            // SqlChar metodu temizse (true) döner. Burada ! ile ters mantık.
            // Eğer SqlChar false (Zararlı kelime var) dönerse, bu blok çalışır ve string silinir.
            if (!csFunction.SqlChar(inputString))
            {
                inputString = "";
            }

            return inputString;
        }

        // Token: 0x060000A9 RID: 169 RVA: 0x00006F04 File Offset: 0x00005104
        public static bool SqlChar(string v)
        {
            if (string.IsNullOrWhiteSpace(v)) return true;

            foreach (string badWord in csFunction.dangerousKeywords)
            {
                // Kelimenin başında veya sonunda boşluk olup olmadığına bakar
                if (v.IndexOf(badWord + " ") > -1 || v.IndexOf(" " + badWord) > -1)
                {
                    return false; // Zararlı kelime bulundu
                }
            }
            return true; // Güvenli
        }

        // Token: 0x060000AA RID: 170 RVA: 0x00006F84 File Offset: 0x00005184
        public static string CreateCompressXml(HttpContext context, XElement resultXml, string fileName, bool isCompress)
        {
            string serverPath = context.Server.MapPath("~");
            return csFunction.CreateCompressXml(serverPath, resultXml, fileName, isCompress);
        }

        // Token: 0x060000AB RID: 171 RVA: 0x00006FB4 File Offset: 0x000051B4
        public static string CreateCompressXml(XElement resultXml, string fileName, bool isCompress)
        {
            string staticPath = StaticsMgr.CurrentPath;
            return csFunction.CreateCompressXml(staticPath, resultXml, fileName, isCompress);
        }

        // Token: 0x060000AC RID: 172 RVA: 0x00006FD8 File Offset: 0x000051D8
        public static string CreateCompressXml(string path, XElement resultXml, string fileName, bool isCompress)
        {
            string returnMessage;
            try
            {
                fileName += ".xml";
                string fullPath = Path.Combine(path, fileName);

                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    if (isCompress)
                    {
                        // Sıkıştırılmış Yazma
                        using (BinaryWriter writer = new BinaryWriter(fileStream))
                        {
                            writer.Write(StaticFunction.Compress(resultXml.ToString(false)));
                        }
                    }
                    else
                    {
                        // Normal Yazma (Düz Metin)
                        // DÜZELTME: Orijinal kodda 'wirter' (yazım hatası), 'writer' olarak düzeltildi.
                        using (StreamWriter writer = new StreamWriter(fileStream))
                        {
                            writer.Write(resultXml.ToString(false));
                        }
                    }
                }
                returnMessage = "Build:" + fileName + ",Success!";
            }
            catch (Exception ex)
            {
                csFunction.log.Error("CreateCompressXml " + fileName + " dosyası oluşturulamadı!", ex);
                returnMessage = "Build:" + fileName + ",Fail!";
            }
            return returnMessage;
        }

        // Token: 0x060000AD RID: 173 RVA: 0x000070EC File Offset: 0x000052EC
        public static string BuildCelebConsortia(string file, int order)
        {
            return csFunction.BuildCelebConsortia(file, order, "");
        }

        // Token: 0x060000AE RID: 174 RVA: 0x0000710C File Offset: 0x0000530C
        public static string BuildCelebConsortia(string file, int order, string fileNotCompress)
        {
            bool isSuccess = false;
            string message = "Hata!";
            XElement resultXml = new XElement("Result");
            int total = 0;
            try
            {
                int page = 1;
                int size = 50;
                int consortiaID = -1;
                string name = "";
                int level = -1;

                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    ConsortiaInfo[] consortiaList = db.GetConsortiaPage(page, size, ref total, order, name, consortiaID, level, -1);

                    foreach (ConsortiaInfo consortia in consortiaList)
                    {
                        XElement consortiaNode = FlashUtils.CreateConsortiaInfo(consortia);

                        // Lonca Başkanı (Chairman) varsa başkan bilgisini de ekle
                        if (consortia.ChairmanID != 0)
                        {
                            using (PlayerBussiness pb = new PlayerBussiness())
                            {
                                PlayerInfo chairMan = pb.GetUserSingleByUserID(consortia.ChairmanID);
                                if (chairMan != null)
                                {
                                    consortiaNode.Add(FlashUtils.CreateCelebInfo(chairMan));
                                }
                            }
                        }
                        resultXml.Add(consortiaNode);
                    }
                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                csFunction.log.Error(file + " oluşturma hatası:", ex);
            }

            resultXml.Add(new XAttribute("total", total));
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));
            resultXml.Add(new XAttribute("date", DateTime.Today.ToString("yyyy-MM-dd")));

            if (!string.IsNullOrEmpty(fileNotCompress))
            {
                csFunction.CreateCompressXml(resultXml, fileNotCompress, false);
            }
            return csFunction.CreateCompressXml(resultXml, file, true);
        }

        // Token: 0x060000AF RID: 175 RVA: 0x000072F4 File Offset: 0x000054F4
        public static string BuildCelebUsers(string file, int order)
        {
            return csFunction.BuildCelebUsers(file, order, "");
        }

        // Token: 0x060000B0 RID: 176 RVA: 0x00007314 File Offset: 0x00005514
        public static string BuildEliteMatchPlayerList(string file)
        {
            bool isSuccess = false;
            string message = "Hata!";
            XElement resultXml = new XElement("Result");
            try
            {
                int page = 1;
                int pageSize = 50;
                int userID = -1;
                int totalCount = 0;
                bool resultValue = false;

                using (PlayerBussiness db = new PlayerBussiness())
                {
                    PlayerInfo[] players = db.GetPlayerPage(page, pageSize, ref totalCount, 7, userID, ref resultValue);

                    if (resultValue)
                    {
                        int rankLow = 1;
                        int rankHigh = 1;
                        XElement setLow = new XElement("ItemSet", new XAttribute("value", 1));
                        XElement setHigh = new XElement("ItemSet", new XAttribute("value", 2));

                        foreach (PlayerInfo player in players)
                        {
                            if (player.Grade <= 40)
                            {
                                setLow.Add(FlashUtils.CreateEliteMatchPlayersList(player, rankLow));
                                rankLow++;
                            }
                            else
                            {
                                setHigh.Add(FlashUtils.CreateEliteMatchPlayersList(player, rankHigh));
                                rankHigh++;
                            }
                        }
                        resultXml.Add(setLow);
                        resultXml.Add(setHigh);
                        isSuccess = true;
                        message = "Başarılı!";
                    }
                }
            }
            catch (Exception ex)
            {
                csFunction.log.Error(file + " oluşturma hatası:", ex);
            }

            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));
            resultXml.Add(new XAttribute("lastUpdateTime", DateTime.Now.ToString()));

            csFunction.CreateCompressXml(resultXml, file + "_out", false);
            return csFunction.CreateCompressXml(resultXml, file, true);
        }

        // Token: 0x060000B1 RID: 177 RVA: 0x00007524 File Offset: 0x00005724
        // DÜZELTİLEN METOT: Değişkenler burada doğru tanımlandı.
        public static string BuildCelebUsers(string file, int order, string fileNotCompress)
        {
            bool isSuccess = false;
            string message = "Hata!";
            XElement resultXml = new XElement("Result");

            try
            {
                int page = 1;
                int size = 50;
                int userID = -1;

                // DEĞİŞKEN TANIMLARI: using bloğundan önce tanımlanmalı
                int totalCount = 0;
                bool resultValue = false;

                using (PlayerBussiness db = new PlayerBussiness())
                {
                    db.UpdateUserReputeFightPower();

                    // Çağrı düzeltildi: ref totalCount ve ref resultValue kullanıldı
                    PlayerInfo[] players = db.GetPlayerPage(page, size, ref totalCount, order, userID, ref resultValue);

                    if (resultValue)
                    {
                        foreach (PlayerInfo player in players)
                        {
                            resultXml.Add(FlashUtils.CreateCelebInfo(player));
                        }
                        isSuccess = true;
                        message = "Başarılı!";
                    }
                }
            }
            catch (Exception ex)
            {
                csFunction.log.Error(file + " oluşturma hatası:", ex);
            }

            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));
            resultXml.Add(new XAttribute("date", DateTime.Today.ToString("yyyy-MM-dd")));

            if (!string.IsNullOrEmpty(fileNotCompress))
            {
                csFunction.CreateCompressXml(resultXml, fileNotCompress, false);
            }
            return csFunction.CreateCompressXml(resultXml, file, true);
        }

        // Token: 0x060000B2 RID: 178 RVA: 0x00007690 File Offset: 0x00005890
        public static string BuildCelebConsortiaFightPower(string file, string fileNotCompress)
        {
            bool isSuccess = false;
            string message = "Hata!";
            XElement resultXml = new XElement("Result");
            int total = 0;

            try
            {
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    ConsortiaInfo[] consortias = db.UpdateConsortiaFightPower();
                    total = consortias.Length;

                    foreach (ConsortiaInfo consortia in consortias)
                    {
                        XElement consortiaNode = FlashUtils.CreateConsortiaInfo(consortia);

                        if (consortia.ChairmanID != 0)
                        {
                            using (PlayerBussiness pb = new PlayerBussiness())
                            {
                                PlayerInfo chairMan = pb.GetUserSingleByUserID(consortia.ChairmanID);
                                if (chairMan != null)
                                {
                                    consortiaNode.Add(FlashUtils.CreateCelebInfo(chairMan));
                                }
                            }
                        }
                        resultXml.Add(consortiaNode);
                    }
                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                csFunction.log.Error(file + " oluşturma hatası:", ex);
            }

            resultXml.Add(new XAttribute("total", total));
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));
            resultXml.Add(new XAttribute("date", DateTime.Today.ToString("yyyy-MM-dd")));

            if (!string.IsNullOrEmpty(fileNotCompress))
            {
                csFunction.CreateCompressXml(resultXml, fileNotCompress, false);
            }
            return csFunction.CreateCompressXml(resultXml, file, true);
        }

        // Token: 0x04000026 RID: 38
        private static string[] dangerousKeywords = ";|and|1=1|exec|insert|select|delete|update|like|count|chr|mid|master|or|truncate|char|declare|join".Split(new char[]
        {
            '|'
        });
    }
}
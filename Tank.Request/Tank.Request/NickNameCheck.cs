using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;

namespace Tank.Request
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class NickNameCheck : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string path = HttpContext.Current.Server.MapPath(".");
            path += "\\";
            LanguageMgr.Setup(path);
            bool value = false;
            string message = LanguageMgr.GetTranslation(" Bu isim kullanılıyor.", Array.Empty<object>());
            XElement result = new XElement("Result");
            try
            {
                string nickName = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["NickName"]));

                // --- USERID ALMA MANTIĞI (GELİŞTİRİLDİ) ---
                int userID = 0;
                string uidStr = context.Request["UserID"];
                if (string.IsNullOrEmpty(uidStr)) uidStr = context.Request["ID"];
                if (string.IsNullOrEmpty(uidStr))
                {
                    // Request'te yoksa Cookie'den almaya çalış
                    HttpCookie cookie = context.Request.Cookies["UserID"];
                    if (cookie != null) uidStr = cookie.Value;
                }

                if (string.IsNullOrEmpty(uidStr))
                {
                    // Cookie'de yoksa Form'dan almaya çalış
                    uidStr = context.Request.Form["UserID"];
                }

                int.TryParse(uidStr, out userID);
                // ----------------------------------------------

                bool flag = Encoding.Default.GetByteCount(nickName) <= 14;
                if (flag)
                {
                    bool flag2 = !string.IsNullOrEmpty(nickName);
                    if (flag2)
                    {
                        Regex regexItem = new Regex("^[a-za-z0-9àáâãèéêìíòóôõùúăđĩũơàáâãèéêìíòóôõùúăđĩũơưăạảấầẩẫậắằẳẵặẹẻẽềềểếưăạảấầẩẫậắằẳẵặẹẻẽềềểếễệỉịọỏốồổỗộớờởỡợụủứừễệỉịọỏốồổỗộớờởỡợụủứừửữựỳỵýỷỹửữựỳỵỷỹ\\s|_.]+$");
                        bool flag3 = !regexItem.IsMatch(nickName);
                        if (flag3)
                        {
                            message = LanguageMgr.GetTranslation("UseReworkNameHandler.HasSpecialCharacters", Array.Empty<object>());
                        }
                        else
                        {
                            using (PlayerBussiness db = new PlayerBussiness())
                            {
                                // ID'yi gönderiyoruz
                                bool flag4 = db.GetUserSingleByNickName(nickName) == null;

                                if (flag4)
                                {
                                    value = true;
                                    message = LanguageMgr.GetTranslation("Tank.Request.NickNameCheck.Right", Array.Empty<object>());
                                }
                                else
                                {
                                    // HATA DURUMUNDA GELEN ID'Yİ MESAJA EKLİYORUZ (DEBUG İÇİN)
                                    // Eğer mesajda (UID: 0) yazıyorsa ID gitmiyordur.
                                    // Eğer mesajda (UID: 1001) yazıyorsa ama hata alıyorsanız SQL kontrolü tutuyor demektir.
                                    message = string.Format("Bu isim kullanılıyor. (UID: {0})", userID);
                                }
                            }
                        }
                    }
                }
                else
                {
                    message = LanguageMgr.GetTranslation("Tank.Request.NickNameCheck.Long", Array.Empty<object>());
                }
            }
            catch (Exception ex)
            {
                NickNameCheck.log.Error("NickNameCheck", ex);
                value = false;
            }
            result.Add(new XAttribute("value", value));
            result.Add(new XAttribute("message", message));
            context.Response.ContentType = "text/plain";
            context.Response.Write(result.ToString(false));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}
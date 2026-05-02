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

                // --- DEĞİŞİKLİK BAŞLANGICI ---
                // İstekten UserID parametresini alıyoruz (Kullanıcı giriş yapmışsa ID gelecektir)
                int userID = 0;
                string uidStr = context.Request["UserID"]; // Request parametresi genelde "UserID" veya "ID" olur
                if (string.IsNullOrEmpty(uidStr)) uidStr = context.Request["ID"];
                int.TryParse(uidStr, out userID);
                // --- DEĞİŞİKLİK SONU ---

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
                                // --- DEĞİŞİKLİK BAŞLANGICI ---
                                // userID parametresini de veritabanı fonksiyonuna gönderiyoruz.
                                // Eğer isim kullanıcının kendi ismiyse null dönecek ve isim müsait sayılacak.
                                bool flag4 = db.GetUserSingleByNickName(nickName, userID,0) == null;
                                // --- DEĞİŞİKLİK SONU ---

                                if (flag4)
                                {
                                    value = true;
                                    message = LanguageMgr.GetTranslation("Tank.Request.NickNameCheck.Right", Array.Empty<object>());
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
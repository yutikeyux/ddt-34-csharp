<%@ WebHandler Language="C#" Class="CommitWeeklyUserRecord" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class CommitWeeklyUserRecord : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        
        try
        {
            // AS3 tarafındaki URLVariables'dan gelen verileri alıyoruz.
            // RequestVairableCreater genellikle güvenlik amaçlı bir 'Key' veya kullanıcı ID'si de ekler.
            // Burada sadece istatistik verilerini alıyoruz, ancak güvenlik kontrolü yapılmalıdır.
            
            string version = context.Request["Versions"];
            int forum1 = Convert.ToInt32(context.Request["Forum1"] ?? "0");
            int forum2 = Convert.ToInt32(context.Request["Forum2"] ?? "0");
            int forum3 = Convert.ToInt32(context.Request["Forum3"] ?? "0");
            int forum4 = Convert.ToInt32(context.Request["Forum4"] ?? "0");
            int forum5 = Convert.ToInt32(context.Request["Forum5"] ?? "0");

            // Kullanıcı kimliği (Session veya Request üzerinden gelmeli, örn: context.Request["UserID"])
            // Bu örnekte RequestVairableCreater'ın eklediği varsayımsal bir 'Key' veya 'ID' kontrolü yapılmalıdır.
            // Gerçek projede UserID'yi Session'dan veya Token'dan almalısınız.
            int userID = -1; 
            if(context.Request["UserID"] != null) 
            {
                int.TryParse(context.Request["UserID"], out userID);
            }

            if (userID > 0)
            {
                // Veritabanına kayıt işlemi
                SaveWeeklyStats(userID, version, forum1, forum2, forum3, forum4, forum5);
                
                // AS3 tarafına başarılı olduğunu bildir (Genellikle "true" yeterlidir)
                context.Response.Write("true");
            }
            else
            {
                // Kullanıcı girişi yoksa hata döndür
                context.Response.Write("false");
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda log tutabilir veya hata mesajı döndürebilirsiniz.
            context.Response.Write("Error: " + ex.Message);
        }
    }

    private void SaveWeeklyStats(int userID, string version, int f1, int f2, int f3, int f4, int f5)
    {
        // Web.config dosyanızdaki connection string
        string connString = ConfigurationManager.ConnectionStrings["GameDBConnectionString"].ConnectionString;
        
        using (SqlConnection conn = new SqlConnection(connString))
        {
            string sql = @"
                IF EXISTS (SELECT 1 FROM WeeklyUserRecord WHERE UserID = @UserID)
                    UPDATE WeeklyUserRecord 
                    SET Versions = @Versions, Forum1 = @Forum1, Forum2 = @Forum2, Forum3 = @Forum3, Forum4 = @Forum4, Forum5 = @Forum5, UpdateDate = GETDATE()
                    WHERE UserID = @UserID
                ELSE
                    INSERT INTO WeeklyUserRecord (UserID, Versions, Forum1, Forum2, Forum3, Forum4, Forum5, UpdateDate)
                    VALUES (@UserID, @Versions, @Forum1, @Forum2, @Forum3, @Forum4, @Forum5, GETDATE())";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@Versions", version ?? "");
                cmd.Parameters.AddWithValue("@Forum1", f1);
                cmd.Parameters.AddWithValue("@Forum2", f2);
                cmd.Parameters.AddWithValue("@Forum3", f3);
                cmd.Parameters.AddWithValue("@Forum4", f4);
                cmd.Parameters.AddWithValue("@Forum5", f5);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }
}
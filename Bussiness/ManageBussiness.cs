using Bussiness.CenterService;
using SqlDataProvider.Data;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Bussiness
{
    public class ManageBussiness : BaseBussiness
    {
        private bool ForbidPlayer(string userName, string nickName, int userID, DateTime forbidDate, bool isExist)
        {
			return ForbidPlayer(userName, nickName, userID, forbidDate, isExist, "");
        }

        private bool ForbidPlayer(string userName, string nickName, int userID, DateTime forbidDate, bool isExist, string ForbidReason)
        {
			bool flag = false;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[6]
				{
					new SqlParameter("@UserName", userName),
					new SqlParameter("@NickName", nickName),
					new SqlParameter("@UserID", userID),
					null,
					null,
					null
				};
				sqlParameters[2].Direction = ParameterDirection.InputOutput;
				sqlParameters[3] = new SqlParameter("@ForbidDate", forbidDate);
				sqlParameters[4] = new SqlParameter("@IsExist", isExist);
				sqlParameters[5] = new SqlParameter("@ForbidReason", ForbidReason);
				db.RunProcedure("SP_Admin_ForbidUser", sqlParameters);
				userID = (int)sqlParameters[2].Value;
				if (userID <= 0)
				{
					return flag;
				}
				flag = true;
				if (!isExist)
				{
					KitoffUser(userID, "You are kicking out by GM!!");
					return flag;
				}
				return flag;
			}
			catch (Exception exception)
			{
				if (BaseBussiness.log.IsErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					return flag;
				}
				return flag;
			}
        }

        public bool ForbidPlayerByNickName(string nickName, DateTime date, bool isExist)
        {
			return ForbidPlayer("", nickName, 0, date, isExist);
        }

        public bool ForbidPlayerByNickName(string nickName, DateTime date, bool isExist, string ForbidReason)
        {
			return ForbidPlayer("", nickName, 0, date, isExist, ForbidReason);
        }

        public bool ForbidPlayerByUserID(int userID, DateTime date, bool isExist)
        {
			return ForbidPlayer("", "", userID, date, isExist);
        }

        public bool ForbidPlayerByUserID(int userID, DateTime date, bool isExist, string ForbidReason)
        {
			return ForbidPlayer("", "", userID, date, isExist, ForbidReason);
        }

        public bool ForbidPlayerByUserName(string userName, DateTime date, bool isExist)
        {
			return ForbidPlayer(userName, "", 0, date, isExist);
        }

        public bool ForbidPlayerByUserName(string userName, DateTime date, bool isExist, string ForbidReason)
        {
			return ForbidPlayer(userName, "", 0, date, isExist, ForbidReason);
        }

        public int GetConfigState(int type)
        {
			try
			{
				using CenterServiceClient client = new CenterServiceClient();
				return client.GetConfigState(type);
			}
			catch (Exception exception)
			{
				if (BaseBussiness.log.IsErrorEnabled)
				{
					BaseBussiness.log.Error("GetConfigState", exception);
				}
			}
			return 2;
        }

        public int KitoffUser(int id, string msg)
        {
			try
			{
				using CenterServiceClient client = new CenterServiceClient();
				if (client.KitoffUser(id, msg))
				{
					return 0;
				}
				return 3;
			}
			catch (Exception exception)
			{
				if (BaseBussiness.log.IsErrorEnabled)
				{
					BaseBussiness.log.Error("KitoffUser", exception);
				}
				return 1;
			}
        }

        public int KitoffUserByNickName(string name, string msg)
        {
			using PlayerBussiness bussiness = new PlayerBussiness();
			PlayerInfo userSingleByNickName = bussiness.GetUserSingleByNickName(name);
			if (userSingleByNickName == null)
			{
				return 2;
			}
			return KitoffUser(userSingleByNickName.ID, msg);
        }

        public int KitoffUserByUserName(string name, string msg)
        {
			using PlayerBussiness bussiness = new PlayerBussiness();
			PlayerInfo userSingleByUserName = bussiness.GetUserSingleByUserName(name);
			if (userSingleByUserName == null)
			{
				return 2;
			}
			return KitoffUser(userSingleByUserName.ID, msg);
        }

        public bool Reload(string type)
        {
			try
			{
				using CenterServiceClient client = new CenterServiceClient();
				return client.Reload(type);
			}
			catch (Exception exception)
			{
				if (BaseBussiness.log.IsErrorEnabled)
				{
					BaseBussiness.log.Error("Reload", exception);
				}
			}
			return false;
        }

        public bool ReLoadServerList()
        {
			bool flag = false;
			try
			{
				using CenterServiceClient client = new CenterServiceClient();
				if (client.ReLoadServerList())
				{
					flag = true;
					return flag;
				}
				return flag;
			}
			catch (Exception exception)
			{
				if (BaseBussiness.log.IsErrorEnabled)
				{
					BaseBussiness.log.Error("ReLoadServerList", exception);
					return flag;
				}
				return flag;
			}
        }

        public bool SystemNotice(string msg)
        {
			bool flag = false;
			try
			{
				if (string.IsNullOrEmpty(msg))
				{
					return flag;
				}
				using CenterServiceClient client = new CenterServiceClient();
				if (client.SystemNotice(msg))
				{
					flag = true;
					return flag;
				}
				return flag;
			}
			catch (Exception exception)
			{
				if (BaseBussiness.log.IsErrorEnabled)
				{
					BaseBussiness.log.Error("SystemNotice", exception);
					return flag;
				}
				return flag;
			}
        }

        #region Discord Link Methods

        public class DiscordLinkRecord
        {
            public int UserID { get; set; }
            public long DiscordID { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        public List<DiscordLinkRecord> GetDiscordLinks(int top)
        {
            var list = new List<DiscordLinkRecord>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["conString"]))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(@"
                        SELECT TOP (@Top) UserID, DiscordID, CreatedAt, UpdatedAt
                        FROM DiscordLinks
                        ORDER BY CreatedAt DESC
                    ", conn))
                    {
                        cmd.Parameters.AddWithValue("@Top", top);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var rec = new DiscordLinkRecord
                                {
                                    UserID = reader.GetInt32(0),
                                    DiscordID = reader.GetInt64(1),
                                    CreatedAt = reader.GetDateTime(2),
                                    UpdatedAt = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3)
                                };
                                list.Add(rec);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error("[DiscordLink] GetDiscordLinks error", ex);
            }

            return list;
        }

        public int DeleteDiscordLinkByDiscordId(long discordId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["conString"]))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM DiscordLinks WHERE DiscordID = @DiscordID", conn))
                    {
                        cmd.Parameters.AddWithValue("@DiscordID", discordId);
                        int rows = cmd.ExecuteNonQuery();

                        if (BaseBussiness.log.IsInfoEnabled)
                            BaseBussiness.log.Info($"[DiscordLink] Delete by DiscordID={discordId}, affected={rows}");

                        return rows;
                    }
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error("[DiscordLink] DeleteDiscordLinkByDiscordId error", ex);
                return 0;
            }
        }

        public int DeleteDiscordLinkByUserId(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["conString"]))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM DiscordLinks WHERE UserID = @UserID", conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        int rows = cmd.ExecuteNonQuery();

                        if (BaseBussiness.log.IsInfoEnabled)
                            BaseBussiness.log.Info($"[DiscordLink] Delete by UserID={userId}, affected={rows}");

                        return rows;
                    }
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error("[DiscordLink] DeleteDiscordLinkByUserId error", ex);
                return 0;
            }
        }

        /// <summary>
        /// Tüm eşleşmeleri ve kod kayıtlarını siler (komple sıfırlama).
        /// </summary>
        public int DeleteAllDiscordLinks()
        {
            int total = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["conString"]))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("DELETE FROM DiscordLinks", conn))
                    {
                        total += cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("DELETE FROM DiscordLinkCodes", conn))
                    {
                        total += cmd.ExecuteNonQuery();
                    }
                }

                if (BaseBussiness.log.IsInfoEnabled)
                    BaseBussiness.log.Info($"[DiscordLink] DeleteAll executed, affected={total}");

                return total;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error("[DiscordLink] DeleteAllDiscordLinks error", ex);
                return 0;
            }
        }

        public bool AddDiscordLinkCode(int userId, string code, DateTime expiresAt)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["conString"]))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(@"
                IF NOT EXISTS (SELECT 1 FROM DiscordLinkCodes WHERE UserID = @UserID)
                    INSERT INTO DiscordLinkCodes(UserID, Code, ExpiresAt, CreatedAt, Used)
                    VALUES(@UserID, @Code, @ExpiresAt, GETDATE(), 0);
                ELSE
                    UPDATE DiscordLinkCodes
                    SET Code = @Code,
                        ExpiresAt = @ExpiresAt,
                        CreatedAt = GETDATE(),
                        Used = 0
                    WHERE UserID = @UserID;
            ", conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@Code", code);
                        cmd.Parameters.AddWithValue("@ExpiresAt", expiresAt);

                        cmd.ExecuteNonQuery();
                    }
                }

                if (BaseBussiness.log.IsInfoEnabled)
                    BaseBussiness.log.Info($"[DiscordLink] AddDiscordLinkCode ok userId={userId}, code={code}");

                return true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error("[DiscordLink] AddDiscordLinkCode error", ex);

                return false;
            }
        }

        /// <summary>
        /// DiscordId'den UserID bulur. Yoksa 0 döner.
        /// </summary>
        public int GetUserIdByDiscordId(long discordId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["conString"]))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT TOP 1 UserID FROM DiscordLinks WHERE DiscordID = @DiscordID", conn))
                    {
                        cmd.Parameters.AddWithValue("@DiscordID", discordId);

                        object result = cmd.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            return 0; // bağlanmış hesap yok
                        }

                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error("[DiscordLink] GetUserIdByDiscordId error", ex);

                return 0;
            }
        }

        /// <summary>
        /// Kod geçerliyse:
        /// - DiscordLinkCodes.Used = 1 yapar
        /// - DiscordLinks tablosuna (UserID, DiscordId) yazar (insert or update)
        /// ve UserID'yi döner. Hatalıysa 0 döner.
        /// </summary>
        public int ConsumeDiscordLinkCodeAndBindUser(string code, long discordId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["conString"]))
                {
                    conn.Open();

                    int userId = 0;
                    DateTime expires;
                    bool used;

                    // 1) Kodu bul
                    using (SqlCommand cmd = new SqlCommand(@"
                SELECT TOP 1 UserID, ExpiresAt, Used
                FROM DiscordLinkCodes
                WHERE Code = @Code", conn))
                    {
                        cmd.Parameters.AddWithValue("@Code", code);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                return 0; // kod yok

                            userId = reader.GetInt32(0);
                            expires = reader.GetDateTime(1);
                            used = reader.GetBoolean(2);
                        }
                    }

                    if (used || expires < DateTime.Now)
                        return 0; // kullanılmış ya da süresi dolmuş

                    // 1.5) Hesap veya Discord zaten bağlı mı?
                    using (SqlCommand check = new SqlCommand(@"
                IF EXISTS(SELECT 1 FROM DiscordLinks WHERE UserID = @UserID)
                    SELECT 1
                ELSE IF EXISTS(SELECT 1 FROM DiscordLinks WHERE DiscordID = @DiscordID)
                    SELECT 1
                ELSE
                    SELECT 0", conn))
                    {
                        check.Parameters.AddWithValue("@UserID", userId);
                        check.Parameters.AddWithValue("@DiscordID", discordId);
                        int already = (int)check.ExecuteScalar();
                        if (already > 0)
                        {
                            // zaten bir link var, tekrar bağlamaya izin verme
                            return 0;
                        }
                    }

                    // 2) DiscordLinks tablosuna yaz (bağla)
                    using (SqlCommand cmd2 = new SqlCommand(@"
                INSERT INTO DiscordLinks(UserID, DiscordID, CreatedAt)
                VALUES(@UserID, @DiscordID, GETDATE());", conn))
                    {
                        cmd2.Parameters.AddWithValue("@UserID", userId);
                        cmd2.Parameters.AddWithValue("@DiscordID", discordId);
                        cmd2.ExecuteNonQuery();
                    }

                    // 3) Kodu kullanılmış işaretle
                    using (SqlCommand cmd3 = new SqlCommand(@"
                UPDATE DiscordLinkCodes
                SET Used = 1, UsedAt = GETDATE()
                WHERE Code = @Code", conn))
                    {
                        cmd3.Parameters.AddWithValue("@Code", code);
                        cmd3.ExecuteNonQuery();
                    }

                    if (BaseBussiness.log.IsInfoEnabled)
                        BaseBussiness.log.Info($"[DiscordLink] ConsumeDiscordLinkCode ok userId={userId}, discordId={discordId}, code={code}");

                    return userId;
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error("[DiscordLink] ConsumeDiscordLinkCode error", ex);

                return 0;
            }
        }

        public bool IsUserAlreadyLinked(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["conString"]))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT TOP 1 1 FROM DiscordLinks WHERE UserID = @UserID", conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        object result = cmd.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error("[DiscordLink] IsUserAlreadyLinked error", ex);
                return false;
            }
        }

        public string GetActiveDiscordLinkCode(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["conString"]))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(@"
                SELECT TOP 1 Code
                FROM DiscordLinkCodes
                WHERE UserID = @UserID AND Used = 0 AND ExpiresAt > GETDATE()
                ORDER BY ExpiresAt DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        object result = cmd.ExecuteScalar();
                        return result == null ? null : result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error("[DiscordLink] GetActiveDiscordLinkCode error", ex);
                return null;
            }
        }

        #endregion

        public bool UpdateConfigState(int type, bool state)
        {
			try
			{
				using CenterServiceClient client = new CenterServiceClient();
				return client.UpdateConfigState(type, state);
			}
			catch (Exception exception)
			{
				if (BaseBussiness.log.IsErrorEnabled)
				{
					BaseBussiness.log.Error("UpdateConfigState", exception);
				}
			}
			return false;
        }
    }
}

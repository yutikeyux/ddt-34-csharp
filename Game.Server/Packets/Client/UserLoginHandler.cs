using Bussiness;
using Bussiness.Interface;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.LOGIN, "User Login handler")]
    public class UserLoginHandler : IPacketHandler
    {
        // Rate limiting için statik dictionary
        private static readonly ConcurrentDictionary<string, LoginAttemptInfo> _loginAttempts = new();
        private static readonly ConcurrentDictionary<string, object> _userLocks = new();
        private static readonly ConcurrentDictionary<int, string> _pendingLogins = new();

        // Aynı IP'den eşzamanlı giriş yapılmasını engellemek için IP bazlı lock
        private static readonly ConcurrentDictionary<string, object> _ipLocks = new();

        // Sabitler
        private const int MAX_LOGIN_ATTEMPTS = 5;
        private const int LOCKOUT_DURATION_MINUTES = 15;
        private const int LOGIN_TIMEOUT_SECONDS = 10;
        private const int RATE_LIMIT_SECONDS = 2;
        private const int EXPECTED_CLIENT_TYPE = 69;
        private const int KEY_LENGTH = 8;
        private const int KEY_OFFSET = 7;
        private const int DATA_OFFSET = 15;

        private class LoginAttemptInfo
        {
            public int AttemptCount { get; set; }
            public DateTime LastAttempt { get; set; }
            public DateTime? LockoutEnd { get; set; }
        }

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client?.Player != null)
            {
                GameServer.log.Warn($"User already logged in: {client.Player.PlayerCharacter?.UserName ?? "Unknown"}");
                return 0;
            }

            try
            {
                return ProcessLogin(client, packet);
            }
            catch (Exception ex)
            {
                HandleCriticalError(client, "UserLoginHandler.CriticalError", ex);
                return 0;
            }
        }

        private int ProcessLogin(GameClient client, GSPacketIn packet)
        {
            // Versiyon ve client tipi okuma
            if (!TryReadVersionInfo(packet, out int version, out int clientType))
            {
                HandleError(client, "UserLoginHandler.InvalidPacket");
                return 0;
            }

            // Client tip kontrolü
            if (clientType == EXPECTED_CLIENT_TYPE)
            {
                GameServer.log.Warn($"Blocked client type {clientType} from {client?.TcpEndpoint}");
                HandleError(client, "UserLoginHandler.UnsupportedClient");
                return 0;
            }

            // RSA şifre çözme
            if (!TryDecryptPacket(packet, client, out byte[] decryptedData))
            {
                return 0;
            }

            // Session key oluşturma
            if (!TryExtractSessionKey(decryptedData, client, out byte[] sessionKey))
            {
                HandleError(client, "UserLoginHandler.InvalidKey");
                return 0;
            }

            // Kimlik bilgilerini parse etme
            if (!TryParseCredentials(decryptedData, out string username, out string password))
            {
                HandleError(client, "UserLoginHandler.InvalidCredentials");
                return 0;
            }

            // IP adresini al
            string clientIp = GetClientIpAddress(client);
            if (string.IsNullOrEmpty(clientIp))
            {
                HandleError(client, "UserLoginHandler.InvalidIp");
                return 0;
            }

            // Rate limiting ve güvenlik kontrolü
            if (IsRateLimitedOrLockedOut(username, client))
            {
                return 0;
            }

            // Thread-safe kullanıcı kontrolü ve login işlemi
            return PerformThreadSafeLogin(client, username, password, version, clientIp);
        }

        /// <summary>
        /// Client IP adresini güvenli şekilde alır (String formatından parse eder)
        /// </summary>
        private string GetClientIpAddress(GameClient client)
        {
            try
            {
                string endpoint = client?.TcpEndpoint;
                if (string.IsNullOrEmpty(endpoint))
                    return null;

                // Format genelde "IP:Port" veya "[IPv6]:Port" şeklindedir
                int portSeparator = endpoint.LastIndexOf(':');
                if (portSeparator > 0)
                {
                    string ipPart = endpoint.Substring(0, portSeparator);

                    // IPv6 köşeli parantezlerini temizle (Örn: [::1] -> ::1)
                    if (ipPart.StartsWith("[") && ipPart.EndsWith("]"))
                        ipPart = ipPart.Substring(1, ipPart.Length - 2);

                    return ipPart;
                }

                return endpoint;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gerçek zamanlı olarak sunucudaki tüm aktif oyuncuları tarar ve aynı IP'den başka biri varsa engeller.
        /// </summary>
        private bool IsIpRestricted(string clientIp, GameClient currentClient)
        {
            try
            {
                GameClient[] allClients = GameServer.Instance?.GetAllClients();
                if (allClients == null || allClients.Length == 0)
                    return false;

                foreach (GameClient c in allClients)
                {
                    // Kendisi null ise, kendisi ise veya bağlantısı kopuk ise atla
                    if (c == null || c == currentClient || !c.IsConnected)
                        continue;

                    // Sadece oyuna başarılı bir şekilde girmiş olanları kontrol et
                    if (c.Player == null)
                        continue;

                    string otherIp = GetClientIpAddress(c);

                    // Eğer aynı IP'den başka birisi oyundaysa engelle
                    if (!string.IsNullOrEmpty(otherIp) &&
                        string.Equals(otherIp, clientIp, StringComparison.OrdinalIgnoreCase))
                    {
                        string otherUser = c.Player.PlayerCharacter?.NickName ?? c.Player.PlayerCharacter?.UserName ?? "Bilinmiyor";
                        GameServer.log.Warn($"IP KISITLAMASI: {clientIp} IP adresi şuanda '{otherUser}' tarafından kullanılıyor. Giriş denemesi engellendi.");

                        currentClient?.Out?.SendMessage(eMessageType.ALERT,"Bu IP adresinden zaten giriş yapılmış. Her IP adresi sadece bir hesapla giriş yapabilir.");
                        Thread.Sleep(2000); // Mesajın gönderilmesi için kısa bir bekleme
                        currentClient?.Disconnect();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                GameServer.log.Error("IsIpRestriction kontrolünde hata", ex);
            }

            return false;
        }

        private bool TryReadVersionInfo(GSPacketIn packet, out int version, out int clientType)
        {
            version = 0;
            clientType = 0;

            try
            {
                version = packet.ReadInt();
                clientType = packet.ReadInt();
                return true;
            }
            catch (Exception ex)
            {
                GameServer.log.Error("Failed to read version info", ex);
                return false;
            }
        }

        private bool TryDecryptPacket(GSPacketIn packet, GameClient client, out byte[] decryptedData)
        {
            decryptedData = null;
            byte[] encryptedData;

            try
            {
                encryptedData = packet.ReadBytes();
                if (encryptedData == null || encryptedData.Length == 0)
                {
                    HandleError(client, "UserLoginHandler.EmptyPacket");
                    return false;
                }
            }
            catch (Exception ex)
            {
                GameServer.log.Error("Failed to read packet data", ex);
                HandleError(client, "UserLoginHandler.InvalidPacket");
                return false;
            }

            try
            {
                decryptedData = WorldMgr.RsaCryptor?.Decrypt(encryptedData, fOAEP: false);

                if (decryptedData == null || decryptedData.Length < DATA_OFFSET + 2)
                {
                    HandleError(client, "UserLoginHandler.DecryptionFailed");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                GameServer.log.Error($"RSA Decryption failed for {client?.TcpEndpoint}", ex);
                HandleError(client, "UserLoginHandler.RsaCryptorError");
                client?.Disconnect();
                return false;
            }
        }

        private bool TryExtractSessionKey(byte[] data, GameClient client, out byte[] sessionKey)
        {
            sessionKey = new byte[KEY_LENGTH];

            try
            {
                if (data.Length < KEY_OFFSET + KEY_LENGTH)
                {
                    return false;
                }

                for (int i = 0; i < KEY_LENGTH; i++)
                {
                    sessionKey[i] = data[KEY_OFFSET + i];
                }

                client?.setKey(sessionKey);
                return true;
            }
            catch (Exception ex)
            {
                GameServer.log.Error("Failed to extract session key", ex);
                return false;
            }
        }

        private bool TryParseCredentials(byte[] data, out string username, out string password)
        {
            username = null;
            password = null;

            try
            {
                if (data.Length <= DATA_OFFSET)
                {
                    return false;
                }

                string credentialString = Encoding.UTF8.GetString(data, DATA_OFFSET, data.Length - DATA_OFFSET);
                string[] parts = credentialString.Split(new[] { ',' }, 2);

                if (parts.Length != 2)
                {
                    return false;
                }

                username = parts[0]?.Trim();
                password = parts[1]?.Trim();

                return !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password);
            }
            catch (Exception ex)
            {
                GameServer.log.Error("Failed to parse credentials", ex);
                return false;
            }
        }

        private bool IsRateLimitedOrLockedOut(string username, GameClient client)
        {
            var attemptInfo = _loginAttempts.GetOrAdd(username, _ => new LoginAttemptInfo());
            var now = DateTime.UtcNow;

            lock (attemptInfo)
            {
                if (attemptInfo.LockoutEnd.HasValue && now < attemptInfo.LockoutEnd.Value)
                {
                    var remaining = (int)(attemptInfo.LockoutEnd.Value - now).TotalMinutes;
                    client?.Out?.SendKitoff($"Hesap kilitli. {remaining} dakika sonra tekrar deneyin.");
                    client?.Disconnect();
                    return true;
                }

                if ((now - attemptInfo.LastAttempt).TotalSeconds < RATE_LIMIT_SECONDS)
                {
                    client?.Out?.SendKitoff("Çok hızlı giriş denemesi. Lütfen bekleyin.");
                    client?.Disconnect();
                    return true;
                }

                attemptInfo.LastAttempt = now;
            }

            return false;
        }

        private int PerformThreadSafeLogin(GameClient client, string username, string password, int version, string clientIp)
        {
            // Aynı IP'den aynı anda gelen 2 login isteğini sıraya sokmak için IP bazlı lock
            var ipLock = _ipLocks.GetOrAdd(clientIp, _ => new object());

            lock (ipLock)
            {
                // 1. IP Kontrolü
                if (IsIpRestricted(clientIp, client))
                {
                    return 0;
                }

                // Kullanıcı bazlı lock
                var userLock = _userLocks.GetOrAdd(username, _ => new object());

                lock (userLock)
                {
                    try
                    {
                        // 2. Kullanıcı lock'u aldıkktan sonra tekrar IP kontrolü (Race Condition güvenliği)
                        if (IsIpRestricted(clientIp, client))
                        {
                            return 0;
                        }

                        return ExecuteLogin(client, username, password, version, clientIp);
                    }
                    finally
                    {
                        if (!_pendingLogins.Values.Any(v => v == username))
                        {
                            _userLocks.TryRemove(username, out _);
                        }
                    }
                }
            }
        }

        private int ExecuteLogin(GameClient client, string username, string password, int version, string clientIp)
        {
            // Çift giriş kontrolü
            if (LoginMgr.ContainsUser(username))
            {
                HandleError(client, "UserLoginHandler.AlreadyLoggedIn");
                RecordFailedAttempt(username);
                return 0;
            }

            // Veritabanı işlemi için timeout
            PlayerInfo playerInfo = null;
            bool isFirstLogin = false;
            bool timeoutOccurred = false;

            var loginTask = System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    BaseInterface interfaceInstance = BaseInterface.CreateInterface();
                    return interfaceInstance.LoginGame(username, password,
                        GameServer.Instance.Configuration.AreaID, ref isFirstLogin);
                }
                catch (Exception ex)
                {
                    GameServer.log.Error($"LoginGame failed for {username}", ex);
                    return null;
                }
            });

            try
            {
                if (!loginTask.Wait(TimeSpan.FromSeconds(LOGIN_TIMEOUT_SECONDS)))
                {
                    timeoutOccurred = true;
                    GameServer.log.Error($"Login timeout for user: {username}");
                }
                else
                {
                    playerInfo = loginTask.Result;
                }
            }
            catch (AggregateException ex)
            {
                GameServer.log.Error($"Login exception for {username}", ex.InnerException);
            }

            // Timeout veya null kontrolü
            if (timeoutOccurred || playerInfo == null)
            {
                HandleError(client, "UserLoginHandler.ServerBusy");
                RecordFailedAttempt(username);
                return 0;
            }

            // Oyuncu ID kontrolü
            if (playerInfo.ID == 0)
            {
                GameServer.log.Warn($"Invalid credentials for user: {username}");
                HandleError(client, "UserLoginHandler.InvalidCredentials");
                RecordFailedAttempt(username);
                return 0;
            }

            // Yasaklı hesap kontrolü
            if (playerInfo.ID == -2)
            {
                GameServer.log.Info($"Blocked login attempt for forbidden user: {username}");
                HandleError(client, "UserLoginHandler.Forbid");
                return 0;
            }

            // İlk giriş kontrolü
            if (isFirstLogin)
            {
                HandleError(client, "UserLoginHandler.RegisterRequired");
                return 0;
            }

            // Başarılı giriş
            return CompleteSuccessfulLogin(client, playerInfo, username, version, clientIp);
        }

        private int CompleteSuccessfulLogin(GameClient client, PlayerInfo playerInfo, string username, int version, string clientIp)
        {
            try
            {
                // ID çakışması kontrolü
                if (LoginMgr.ContainsUser(playerInfo.ID))
                {
                    GameServer.log.Error($"User ID {playerInfo.ID} already logged in");
                    HandleError(client, "UserLoginHandler.AlreadyLoggedIn");
                    return 0;
                }

                // VERİTABANI SÜRESİNCE (10 saniye) başkası aynı IP'den girmiş olabilir, SON KEZ KONTROL EDİYORUZ
                if (IsIpRestricted(clientIp, client))
                {
                    return 0;
                }

                // Pending login ekle
                _pendingLogins[playerInfo.ID] = username;

                // Oyuncu nesnesi oluşturma
                var gamePlayer = new GamePlayer(playerInfo.ID, username, client, playerInfo);

                // LoginMgr.Add kullan
                LoginMgr.Add(playerInfo.ID, client);

                // Client ayarları
                client.Player = gamePlayer;
                client.Version = version;

                // Login sunucusuna bildirim
                try
                {
                    client.Server?.LoginServer?.SendAllowUserLogin(playerInfo.ID);
                }
                catch (Exception ex)
                {
                    GameServer.log.Error($"Failed to notify login server for {username}", ex);
                }

                // Başarılı giriş logu ve temizlik
                ResetFailedAttempts(username);
                _pendingLogins.TryRemove(playerInfo.ID, out _);

                GameServer.log.Info($"Player {username} (ID: {playerInfo.ID}) logged in successfully from IP: {clientIp}");

                return 1;
            }
            catch (Exception ex)
            {
                GameServer.log.Error($"Failed to complete login for {username}", ex);

                // Temizlik
                try
                {
                    LoginMgr.Remove(playerInfo.ID);
                }
                catch { }

                _pendingLogins.TryRemove(playerInfo.ID, out _);
                HandleError(client, "UserLoginHandler.ServerError");
                return 0;
            }
        }

        private void RecordFailedAttempt(string username)
        {
            var attemptInfo = _loginAttempts.GetOrAdd(username, _ => new LoginAttemptInfo());

            lock (attemptInfo)
            {
                attemptInfo.AttemptCount++;

                if (attemptInfo.AttemptCount >= MAX_LOGIN_ATTEMPTS)
                {
                    attemptInfo.LockoutEnd = DateTime.UtcNow.AddMinutes(LOCKOUT_DURATION_MINUTES);
                    attemptInfo.AttemptCount = 0;
                    GameServer.log.Warn($"Account {username} locked out for {LOCKOUT_DURATION_MINUTES} minutes");
                }
            }
        }

        private void ResetFailedAttempts(string username)
        {
            _loginAttempts.TryRemove(username, out _);
        }

        private void HandleError(GameClient client, string translationKey)
        {
            try
            {
                string message = LanguageMgr.GetTranslation(translationKey);
                client?.Out?.SendKitoff(message);
                client?.Disconnect();
            }
            catch (Exception ex)
            {
                GameServer.log.Error($"Failed to send error message: {translationKey}", ex);
                client?.Disconnect();
            }
        }

        private void HandleCriticalError(GameClient client, string translationKey, Exception ex)
        {
            GameServer.log.Error("Critical error in UserLoginHandler", ex);
            HandleError(client, translationKey);
        }
    }
}
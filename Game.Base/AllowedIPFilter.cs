using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading;
using log4net;
using System.Reflection;

namespace Game.Base
{
    /// <summary>
    /// Veritabanindaki IZINLI_KULLANICILAR tablosundan izin verilen IP'leri yukler.
    /// Sadece bu IP'lerden gelen baglantilara izin verir.
    /// Periyodik olarak veritabanindan guncelleme yapar.
    /// </summary>
    public static class AllowedIPFilter
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Izinli IP'lerin thread-safe listesi
        private static readonly ConcurrentDictionary<string, bool> _allowedIPs = new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        // Veritabanindan guncelleme timer'i (5 saniyede bir)
        private static Timer _refreshTimer;
        private const int REFRESH_INTERVAL_MS = 5000; // 5 saniye

        // Filtrenin aktif olup olmadigini belirler
        public static bool IsEnabled { get; private set; }

        // Localhost her zaman izinlidir
        private static readonly HashSet<string> _alwaysAllowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "127.0.0.1",
            "::1",
            "0.0.0.0"
        };

        // Istatistikler
        public static int AllowedIPCount => _allowedIPs.Count;
        public static long RejectedConnections => Interlocked.Read(ref _rejectedCount);

        /// <summary>
        /// AllowedIPFilter'i baslatir. conString uzerinden veritabanina baglanir
        /// ve IZINLI_KULLANICILAR tablosundan IP'leri yukler.
        /// </summary>
        public static void Start()
        {
            try
            {
                log.Info("AllowedIPFilter baslatiliyor...");

                // Ilk yukleme
                RefreshAllowedIPs(null);

                // Periyodik guncelleme timer'i
                _refreshTimer = new Timer(RefreshAllowedIPs, null, REFRESH_INTERVAL_MS, REFRESH_INTERVAL_MS);

                IsEnabled = true;
                log.Info($"AllowedIPFilter aktif! {_allowedIPs.Count} adet izinli IP yuklendi.");
            }
            catch (Exception ex)
            {
                log.Error("AllowedIPFilter baslatilamadi! Filtre devre disi.", ex);
                IsEnabled = false;
            }
        }

        /// <summary>
        /// AllowedIPFilter'i durdurur.
        /// </summary>
        public static void Stop()
        {
            IsEnabled = false;

            if (_refreshTimer != null)
            {
                _refreshTimer.Change(-1, -1);
                _refreshTimer.Dispose();
                _refreshTimer = null;
            }

            log.Info("AllowedIPFilter durduruldu.");
        }

        /// <summary>
        /// Verilen IP adresinin izinli olup olmadigini kontrol eder.
        /// </summary>
        public static bool IsAllowed(EndPoint endPoint)
        {
            if (!IsEnabled)
                return true; // Filtre devre disiysa herkese izin ver

            if (endPoint is not IPEndPoint ipEndPoint)
                return false;

            return IsAllowed(ipEndPoint.Address.ToString());
        }

        /// <summary>
        /// Verilen IP string'inin izinli olup olmadigini kontrol eder.
        /// </summary>
        public static bool IsAllowed(string ipAddress)
        {
            if (!IsEnabled)
                return true;

            if (string.IsNullOrEmpty(ipAddress))
                return false;

            // Localhost her zaman izinli
            if (_alwaysAllowed.Contains(ipAddress))
                return true;

            // HydroFilter whitelist'te ise izinli
            if (IPAddress.TryParse(ipAddress, out IPAddress parsedIP) && HydroFilter.IsWhiteListed(parsedIP))
                return true;

            return _allowedIPs.ContainsKey(ipAddress);
        }

        /// <summary>
        /// Yeni bir IP'yi runtime'da izinli listesine ekler (login sirasinda cagirilir).
        /// </summary>
        public static void AddAllowedIP(string ipAddress)
        {
            if (!string.IsNullOrEmpty(ipAddress))
            {
                _allowedIPs.TryAdd(ipAddress, true);
            }
        }

        /// <summary>
        /// Bir IP'yi izinli listesinden kaldirir.
        /// </summary>
        public static void RemoveAllowedIP(string ipAddress)
        {
            if (!string.IsNullOrEmpty(ipAddress))
            {
                _allowedIPs.TryRemove(ipAddress, out _);
            }
        }

        /// <summary>
        /// Reddedilen baglanti sayacini arttirir.
        /// </summary>
        public static void IncrementRejected()
        {
            Interlocked.Increment(ref _rejectedCount);
        }

        private static long _rejectedCount;

        /// <summary>
        /// Veritabanindan izinli IP'leri yeniden yukler.
        /// </summary>
        private static void RefreshAllowedIPs(object state)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["conString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    log.Error("AllowedIPFilter: conString bulunamadi!");
                    return;
                }

                var newIPs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_IzinliIP_TumunuGetir", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 10;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string ip = reader.GetString(0);
                                if (!string.IsNullOrWhiteSpace(ip))
                                {
                                    newIPs.Add(ip.Trim());
                                }
                            }
                        }
                    }
                }

                // Mevcut listeyi guncelle: yeni IP'leri ekle
                foreach (string ip in newIPs)
                {
                    _allowedIPs.TryAdd(ip, true);
                }

                // Veritabanindan kaldirilan IP'leri cikar
                foreach (var kvp in _allowedIPs)
                {
                    if (!newIPs.Contains(kvp.Key) && !_alwaysAllowed.Contains(kvp.Key))
                    {
                        _allowedIPs.TryRemove(kvp.Key, out _);
                    }
                }

                if (state != null) // Ilk cagri degilse (timer'dan geliyorsa)
                {
                    log.Debug($"AllowedIPFilter guncellendi: {_allowedIPs.Count} izinli IP.");
                }
            }
            catch (Exception ex)
            {
                log.Error("AllowedIPFilter: IP listesi yenilenirken hata!", ex);
            }
        }

        /// <summary>
        /// Login sirasinda kullanicinin IP'sini veritabanina kaydeder.
        /// </summary>
        public static void RegisterLoginIP(string userName, string ipAddress)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(ipAddress))
                return;

            // Hemen memory'ye ekle (veritabani yazmadan once de izinli olsun)
            AddAllowedIP(ipAddress);

            // Arka planda veritabanina yaz
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    string connectionString = ConfigurationManager.AppSettings["conString"];
                    if (string.IsNullOrEmpty(connectionString))
                        return;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand("SP_IzinliIP_GirisGuncelle", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 10;
                            cmd.Parameters.AddWithValue("@UserName", userName);
                            cmd.Parameters.AddWithValue("@IPAddress", ipAddress);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"AllowedIPFilter: Login IP kaydedilemedi ({userName}, {ipAddress})", ex);
                }
            });
        }
    }
}

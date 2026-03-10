using Game.Base;
using Game.Base.Packets;
using Game.Server.Managers;
using log4net;
using System;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Game.Server
{
    public class GameClient : BaseClient
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Sabitler
        private static readonly byte[] FLASH_POLICY_FILE = Encoding.UTF8.GetBytes("<?xml version=\"1.0\"?><!DOCTYPE cross-domain-policy SYSTEM \"http://www.adobe.com/xml/dtds/cross-domain-policy.dtd\"><cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\" /></cross-domain-policy>\0");
        private const byte POLICY_REQUEST_CHAR = (byte)'<'; // ASCII 60

        // Sunucu referansı
        protected GameServer m_server;

        // Paket işleme
        protected IPacketLib m_packetLib;
        protected PacketProcessor m_packetProcessor;

        // Oyuncu verisi (Thread-safe erişim gerektirir)
        protected GamePlayer m_player;
        private readonly object m_playerLock = new object();

        // Zamanlama ve Durum
        protected long m_pingTime;
        private int m_lottery = -1;
        private string m_hwid;
        private int m_version;

        // Geçici veri
        public string TempData = string.Empty;

        #region Properties (Özellikler)

        public GameServer Server => m_server;

        public IPacketLib Out
        {
            get { return m_packetLib; }
            set { m_packetLib = value; }
        }

        public PacketProcessor PacketProcessor => m_packetProcessor;

        public long PingTime
        {
            get { return Interlocked.Read(ref m_pingTime); }
        }

        public string HWID
        {
            get { return m_hwid; }
            set { m_hwid = value; }
        }

        public int Version
        {
            get { return m_version; }
            set { m_version = value; }
        }

        public int Lottery
        {
            get { return m_lottery; }
            set { m_lottery = value; }
        }

        public GamePlayer Player
        {
            get
            {
                lock (m_playerLock)
                {
                    return m_player;
                }
            }
            set
            {
                GamePlayer oldPlayer = null;
                lock (m_playerLock)
                {
                    if (m_player != value)
                    {
                        oldPlayer = m_player;
                        m_player = value;
                    }
                }

                // Eski oyuncuyu güvenli bir şekilde çıkart (Lock dışında yapılır)
                if (oldPlayer != null)
                {
                    try
                    {
                        oldPlayer.Quit();
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error quitting old player: {oldPlayer.PlayerCharacter?.NickName}", ex);
                    }
                }
            }
        }

        #endregion

        #region Constructor

        public GameClient(GameServer server, byte[] readBuffer, byte[] sendBuffer)
            : base(readBuffer, sendBuffer)
        {
            m_server = server ?? throw new ArgumentNullException(nameof(server));

            // Başlangıç ayarları
            UpdatePingTime();

            base.Encryted = true; // Not: Base sınıftaki typo düzeltilmeli mi kontrol edin (Encrypted olmalı)
            base.AsyncPostSend = true;
        }

        #endregion

        #region BaseClient Overrides

        public override void Disconnect()
        {
            // Bağlantı zaten kapalıysa işlem yapma
            if (!base.IsConnected) return;

            try
            {
                base.Disconnect();
            }
            catch (Exception ex)
            {
                log.Error("Disconnect error", ex);
            }
        }

        protected override void OnConnect()
        {
            base.OnConnect();
            UpdatePingTime();

            // Bağlantı anında yapılması gereken ek initler (varsa)
            m_packetProcessor = null;
            m_packetLib = null;
        }

        protected override void OnDisconnect()
        {
            // Oyuncu nesnesini temizle
            GamePlayer playerToClean = null;
            lock (m_playerLock)
            {
                playerToClean = m_player;
                m_player = null;
            }

            if (playerToClean != null)
            {
                try
                {
                    playerToClean.FightBag.ClearBag();
                    LoginMgr.ClearLoginPlayer(playerToClean.PlayerCharacter.ID, this);
                    playerToClean.Quit();
                }
                catch (Exception ex)
                {
                    log.Error($"Error during player cleanup for ID: {playerToClean.PlayerCharacter?.ID}", ex);
                }
            }

            // Buffer'ları güvenli bir şekilde serbest bırak
            try
            {
                // Buffer değişkenlerini null yapmadan önce referansları al
                byte[] sendBuf = m_sendBuffer;
                byte[] readBuf = m_readBuffer;

                // Base sınıfın veya kendi alanlarımızın temizlenmesi
                // Not: m_sendBuffer BaseClient içinde tanımlıysa oradan erişilmeli.
                // Burada BaseClient'tan gelen alanları temizliyoruz.

                if (sendBuf != null)
                {
                    m_server.ReleasePacketBuffer(sendBuf);
                    m_sendBuffer = null; // Bellek sızıntısını önlemek için referansı kaldır
                }

                if (readBuf != null)
                {
                    m_server.ReleasePacketBuffer(readBuf);
                    m_readBuffer = null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error releasing packet buffers", ex);
            }

            // HWID ve temp verileri temizle
            m_hwid = null;
            TempData = null;

            base.OnDisconnect();
        }

        public override void OnRecv(int num_bytes)
        {
            UpdatePingTime();

            if (num_bytes <= 0 || m_readBuffer == null)
                return;

            try
            {
                // Eğer PacketProcessor henüz oluşmamışsa (ilk bağlantı handshake'i)
                if (m_packetProcessor == null)
                {
                    // Flash Policy File isteği kontrolü
                    if (m_readBuffer[0] == POLICY_REQUEST_CHAR)
                    {
                        SendPolicyFile();
                        return; // Policy dosyası gönderildikten sonra başka işlem yapma
                    }
                }

                // Normal paket işleme
                base.OnRecv(num_bytes);
            }
            catch (Exception ex)
            {
                log.Error("OnRecv processing error", ex);
                Disconnect();
            }
        }

        public override void OnRecvPacket(GSPacketIn pkg)
        {
            if (pkg == null) return;

            try
            {
                // İlk paket gelişinde kütüphaneyi başlat
                if (m_packetProcessor == null)
                {
                    m_packetLib = AbstractPacketLib.CreatePacketLibForVersion(1, this);
                    m_packetProcessor = new PacketProcessor(this);
                }

                // Paket ID'sini ayarla
                if (m_player != null)
                {
                    pkg.ClientID = m_player.PlayerId;
                    pkg.WriteHeader();
                }

                // Paketi işle
                m_packetProcessor.HandlePacket(pkg);
            }
            catch (Exception ex)
            {
                log.Error($"Error handling packet. Code: {pkg.Code}, Client: {TcpEndpoint}", ex);
                // Kritik hatalarda bağlantıyı koparmak isteyebilirsiniz:
                // Disconnect();
            }
        }

        public override void SendTCP(GSPacketIn pkg)
        {
            if (!base.IsConnected || pkg == null) return;

            try
            {
                base.SendTCP(pkg);
            }
            catch (Exception ex)
            {
                log.Error("SendTCP error", ex);
                Disconnect();
            }
        }

        public override void DisplayMessage(string msg)
        {
            // Loglama mekanizması zaten var, console yazma işini log4net'e devredebiliriz
            // Veya base davranışı koruyabiliriz.
            if (log.IsInfoEnabled)
            {
                log.Info(msg);
            }
            base.DisplayMessage(msg);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(128);
            sb.Append("GameClient [");

            try
            {
                // IP Adresi
                sb.Append("IP: ").Append(TcpEndpoint?.ToString() ?? "N/A");

                // Packet Library
                sb.Append(", Lib: ").Append(Out?.GetType().Name ?? "null");

                // Player Info
                GamePlayer currentPlayer = Player; // Thread-safe property
                if (currentPlayer != null)
                {
                    sb.Append(", Player: ").Append(currentPlayer.PlayerCharacter?.NickName ?? "Unknown");
                }
                else
                {
                    sb.Append(", Player: null");
                }
            }
            catch (Exception ex)
            {
                sb.Append(", Error: ").Append(ex.Message);
            }

            sb.Append("]");
            return sb.ToString();
        }

        #endregion

        #region Helper Methods

        private void SendPolicyFile()
        {
            try
            {
                if (m_sock != null && m_sock.Connected)
                {
                    m_sock.Send(FLASH_POLICY_FILE);
                    // Policy dosyası gönderildikten sonra genellikle bağlantı beklenir veya kesilir.
                    // İstemcinin davranışına göre burada Disconnect() çağrılabilir.
                }
            }
            catch (Exception ex)
            {
                log.Warn("Failed to send policy file", ex);
            }
        }

        private void UpdatePingTime()
        {
            Interlocked.Exchange(ref m_pingTime, DateTime.Now.Ticks);
        }

        #endregion
    }
}
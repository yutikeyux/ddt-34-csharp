using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
    /// <summary>
    /// Kıyafet özelliği şablon verilerini yükler, önbellekte tutar ve sorgular.
    /// Yeniden yükleme sırasında okuma/yazma kilidi ile thread-safe erişim sağlar.
    /// </summary>
    public static class ClothPropertyTemplateInfoMgr
    {
        // -----------------------------------------------------------------------
        // ALANLAR
        // -----------------------------------------------------------------------

        private static readonly ILog log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>Kıyafet özelliği verisi: ID → ClothPropertyTemplateInfo</summary>
        private static Dictionary<int, ClothPropertyTemplateInfo> _clothProperty;

        /// <summary>
        /// Eş zamanlı okumaya izin veren, yalnızca yazma işlemini bloke eden kilit.
        /// </summary>
        private static readonly ReaderWriterLockSlim m_lock =
            new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        // -----------------------------------------------------------------------
        // BAŞLATMA / YENİDEN YÜKLEME
        // -----------------------------------------------------------------------

        /// <summary>
        /// Veri tabanından kıyafet özelliği verilerini yükler ve önbelleği başlatır.
        /// </summary>
        public static bool Init()
        {
            try
            {
                var clothProperty = new Dictionary<int, ClothPropertyTemplateInfo>();
                if (!LoadClothProperty(clothProperty)) return false;

                m_lock.EnterWriteLock();
                try
                {
                    _clothProperty = clothProperty;
                }
                finally
                {
                    m_lock.ExitWriteLock();
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error("ClothPropertyMgr Init hatası:", ex);
                return false;
            }
        }

        /// <summary>
        /// Önbelleği yeniden yükler. Yükleme başarısızsa mevcut veri korunur.
        /// </summary>
        public static bool ReLoad()
        {
            try
            {
                var clothProperty = new Dictionary<int, ClothPropertyTemplateInfo>();
                if (!LoadClothProperty(clothProperty)) return false;

                m_lock.EnterWriteLock();
                try
                {
                    _clothProperty = clothProperty;
                }
                finally
                {
                    m_lock.ExitWriteLock();
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error("ClothPropertyMgr ReLoad hatası:", ex);
                return false;
            }
        }

        /// <summary>
        /// Veri tabanından tüm kayıtları çeker ve sözlüğe yükler.
        /// Yinelenen ID'ler atlanır (ilk kayıt önceliklidir).
        /// </summary>
        private static bool LoadClothProperty(Dictionary<int, ClothPropertyTemplateInfo> clothProperty)
        {
            using (var pb = new ProduceBussiness())
            {
                ClothPropertyTemplateInfo[] all = pb.GetAllClothProperty();
                foreach (ClothPropertyTemplateInfo item in all)
                {
                    if (!clothProperty.ContainsKey(item.ID))
                        clothProperty.Add(item.ID, item);
                }
            }
            return true;
        }

        // -----------------------------------------------------------------------
        // SORGULAMA METODları
        // -----------------------------------------------------------------------

        /// <summary>
        /// Yalnızca ID'ye göre eşleşen ilk kaydı döndürür.
        /// Bulunamazsa null döner.
        /// </summary>
        public static ClothPropertyTemplateInfo GetClothPropertyWithID(int id)
        {
            m_lock.EnterReadLock();
            try
            {
                foreach (ClothPropertyTemplateInfo item in _clothProperty.Values)
                {
                    if (item.ID == id)
                        return item;
                }
                return null;
            }
            finally
            {
                m_lock.ExitReadLock();
            }
        }

        /// <summary>
        /// ID ve cinsiyet kriterlerine uyan ilk kaydı döndürür.
        /// Bulunamazsa null döner.
        /// </summary>
        public static ClothPropertyTemplateInfo GetClothPropertyWithID(int id, int sex)
        {
            m_lock.EnterReadLock();
            try
            {
                foreach (ClothPropertyTemplateInfo item in _clothProperty.Values)
                {
                    if (item.ID == id && item.Sex == sex)
                        return item;
                }
                return null;
            }
            finally
            {
                m_lock.ExitReadLock();
            }
        }
    }
}
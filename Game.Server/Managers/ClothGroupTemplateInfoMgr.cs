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
    /// Kıyafet grubu şablon verilerini yükler, önbellekte tutar ve sorgular.
    /// Yeniden yükleme sırasında okuma/yazma kilidi ile thread-safe erişim sağlar.
    /// </summary>
    public static class ClothGroupTemplateInfoMgr
    {
        // -----------------------------------------------------------------------
        // ALANLAR
        // -----------------------------------------------------------------------

        private static readonly ILog log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Kıyafet grubu verisi: ItemID → ClothGroupTemplateInfo
        /// Birden fazla kaydın aynı ItemID'yi paylaşabileceği durumlar için
        /// GetClothGroup ve GetClothGroupWithID metodlarında Values üzerinden taranır.
        /// </summary>
        private static Dictionary<int, ClothGroupTemplateInfo> _clothGroup;

        /// <summary>
        /// Normal lock yerine ReaderWriterLockSlim kullanılıyor:
        /// Çok sayıda okuma isteği birbirini bloklamaz; yalnızca yazma bloklar.
        /// </summary>
        private static readonly ReaderWriterLockSlim m_lock =
            new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        // -----------------------------------------------------------------------
        // BAŞLATMA / YENİDEN YÜKLEME
        // -----------------------------------------------------------------------

        /// <summary>
        /// Veri tabanından kıyafet grubu verilerini yükler ve önbelleği başlatır.
        /// </summary>
        public static bool Init()
        {
            try
            {
                var clothGroup = new Dictionary<int, ClothGroupTemplateInfo>();
                if (!LoadClothGroup(clothGroup)) return false;

                m_lock.EnterWriteLock();
                try
                {
                    _clothGroup = clothGroup;
                }
                finally
                {
                    m_lock.ExitWriteLock();
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error("ClothGroupMgr Init hatası:", ex);
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
                var clothGroup = new Dictionary<int, ClothGroupTemplateInfo>();
                if (!LoadClothGroup(clothGroup)) return false;

                m_lock.EnterWriteLock();
                try
                {
                    _clothGroup = clothGroup;
                }
                finally
                {
                    m_lock.ExitWriteLock();
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error("ClothGroupMgr ReLoad hatası:", ex);
                return false;
            }
        }

        /// <summary>
        /// Veri tabanından tüm kayıtları çeker ve sözlüğe yükler.
        /// Yinelenen ItemID'ler atlanır (ilk kayıt önceliklidir).
        /// </summary>
        private static bool LoadClothGroup(Dictionary<int, ClothGroupTemplateInfo> clothGroup)
        {
            using (var pb = new ProduceBussiness())
            {
                ClothGroupTemplateInfo[] all = pb.GetAllClothGroup();
                foreach (ClothGroupTemplateInfo item in all)
                {
                    if (!clothGroup.ContainsKey(item.ItemID))
                        clothGroup.Add(item.ItemID, item);
                }
            }
            return true;
        }

        // -----------------------------------------------------------------------
        // SORGULAMA METODları
        // -----------------------------------------------------------------------

        /// <summary>
        /// Belirtilen ID'ye sahip kıyafet grubu kayıt sayısını döndürür.
        /// </summary>
        public static int CountClothGroupWithID(int id)
        {
            m_lock.EnterReadLock();
            try
            {
                return GetClothGroupWithIDInternal(id).Count;
            }
            finally
            {
                m_lock.ExitReadLock();
            }
        }

        /// <summary>
        /// ID, TemplateID ve cinsiyet kriterlerine uyan ilk kaydı döndürür.
        /// Bulunamazsa null döner.
        /// </summary>
        public static ClothGroupTemplateInfo GetClothGroup(int id, int templateId, int sex)
        {
            m_lock.EnterReadLock();
            try
            {
                foreach (ClothGroupTemplateInfo item in _clothGroup.Values)
                {
                    if (item.ID == id && item.TemplateID == templateId && item.Sex == sex)
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
        /// Belirtilen ID'ye sahip tüm kıyafet grubu kayıtlarını döndürür.
        /// </summary>
        public static List<ClothGroupTemplateInfo> GetClothGroupWithID(int id)
        {
            m_lock.EnterReadLock();
            try
            {
                return GetClothGroupWithIDInternal(id);
            }
            finally
            {
                m_lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Kilit alınmış bağlamda kullanılmak üzere iç yardımcı metot.
        /// Çağıran metodun kilit yönetiminden sorumlu olduğu varsayılır.
        /// </summary>
        private static List<ClothGroupTemplateInfo> GetClothGroupWithIDInternal(int id)
        {
            var result = new List<ClothGroupTemplateInfo>();
            foreach (ClothGroupTemplateInfo item in _clothGroup.Values)
            {
                if (item.ID == id)
                    result.Add(item);
            }
            return result;
        }
    }
}
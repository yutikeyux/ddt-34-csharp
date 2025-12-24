using Game.Logic.Phy.Maths;
using Game.Logic.Phy.Object;
using System.Drawing;

namespace Game.Logic.Actions
{
    public class GhostMoveAction : BaseAction
    {
        // Hedef nokta
        private Point m_target;

        // Hareket edecek olan oyuncu
        private Player m_player;

        // Hareketin yönünü ve hýzýný belirleyen vektör
        private Point m_v;

        // Paketin sadece bir kez gönderilmesini saðlayan bayrak
        private bool m_isSend;

        /// <summary>
        /// GhostMoveAction yapýcýsý. Bir oyuncuyu belirtilen hedefe doðru hayalet gibi hareket ettirmek için kullanýlýr.
        /// </summary>
        /// <param name="player">Hareket edecek oyuncu</param>
        /// <param name="target">Hedef nokta</param>
        public GhostMoveAction(Player player, Point target)
            : base(0, 1000) // Eylemin süresini (ms cinsinden) belirler, 0'dan baþlar, 1000'de zaman aþýmýna uðrar.
        {
            m_player = player;
            m_target = target;

            // Hedefe doðru olan vektörü hesapla
            m_v = new Point(target.X - m_player.X, target.Y - m_player.Y);

            // Vektörün uzunluðunu 2 birim yapar. Bu, oyuncunun her karede 2 piksel hýzla hareket edeceðini belirtir.
            m_v.Normalize(2);
        }

        /// <summary>
        /// Bu eylemin ana çalýþma döngüsü. Oyun motoru tarafýndan her karede çaðrýlýr.
        /// </summary>
        /// <param name="game">Oyunun ana yöneticisi</param>
        /// <param name="tick">Oyunun mevcut zaman damgasý</param>
        protected override void ExecuteImp(BaseGame game, long tick)
        {
            // --- 1. Aþama: Baþlangýç Paketini Gönder ---
            // Bu blok, eylemin baþýnda sadece bir kez çalýþýr.
            if (!m_isSend)
            {
                m_isSend = true; // Paket gönderildiðini iþaretle

                // Oyuncunun yönünü belirle (sað için 1, sol için -1)
                byte direction = (byte)(m_v.X > 0 ? 1 : -1);

                // Ýstemciye "Hayalet Hareketi" baþlýyor paketini gönder.
                // '2' parametresi, istemciye bu hareketin ruhlarý içerdiðini söyler.
                // Ýstemci, sunucudan zaten ruhlarýn konumlarýný aldýðý için burada sadece hareket tipi bildirilir.
                game.SendPlayerMove(m_player, 2, m_target.X, m_target.Y, direction, m_player.IsLiving);
            }

            // --- 2. Aþama: Adým Adým Hareket Et ---
            // Eðer oyuncu hedefe henüz ulaþmadýysa...
            if (m_target.Distance(m_player.X, m_player.Y) > 2.0)
            {
                // Oyuncunun mevcut pozisyonunu hýz vektörü kadar ilerlet.
                m_player.SetXY(m_player.X + m_v.X, m_player.Y + m_v.Y);
                return; // Bir sonraki kareye devam et.
            }

            // --- 3. Aþama: Hareketi Bitir ---
            // Hedefe ulaþýldýysa (yukarýdaki if koþulu saðlanmazsa) bu kod çalýþýr.

            // ÖNEMLÝ: Burada 'game.CheckBox()' çaðrýsý YOKTUR.
            // Ruhlarýn yaratýlmasý ve yönetimi bu eylemin sorumluluðunda deðildir.
            // Ruhlar, oyuncu öldüðünde ayrý bir yerde yaratýlýr ve çarpýþma kontrolü
            // oyun ana döngüsü tarafýndan ayrýca yapýlýr.

            // Oyuncuyu kesin olarak hedef noktaya yerleþtir (küçük sapmalarý önle).
            m_player.SetXY(m_target.X, m_target.Y);

            // Eylemi bitir ve oyun motoruna bu eylemin sona erdiðini bildir.
            Finish(tick);
        }
    }
}
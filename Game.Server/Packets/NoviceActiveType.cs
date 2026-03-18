namespace Game.Server.Packets
{
    public enum NoviceActiveType
    {
        Level_Atlama = 1, // Seviye atlama etkinliði. Oyuncu seviye atladýðýnda tetiklenir.
        VIP_LEVEL = 2, // VIP seviyesi yükseltme etkinliði. VIP kartý kullanýldýðýnda veya seviye atlandýðýnda tetiklenir.
        SAVAS_GUCU = 3, // Savaþ gücü güncelleme etkinliði. Savaþ gücü belirli bir eþiði geçtiðinde tetiklenir.  
        Silah_Guclendirme = 4, // Silah güçlendirme (+basma) etkinliði. Silaha baþarýyla güçlendirme yapýldýðýnda tetiklenir.
        Kýyafet_Guclendirme = 5, // Kýyafet/giysi güçlendirme etkinliði.
        Sapka_Guclendirme = 6, // Þapka güçlendirme etkinliði.
        DestekEkipmanýGuclendirme = 7, // Destek ekipmaný güçlendirme etkinliði.
        BIRLIK_SAVASI = 8, // Lonca/Birlik savaþý etkinliði. Lonca savaþlarýna katýlýmda tetiklenir.
        Ozgur_Savas = 9, // PvP Maç Sayýsý. Belirli sayýda PvP maçý oynama.
        Kesif_Tamamlama = 10, // Keþif Rýhtýmý Etap Mantýðý - Belirli bir sayýda keþif rýhtýmý etabý tamamlama.
        Ilk_Yukleme = 31, // Ýlk para yükleme (Ýlk Kez Yükleyenler) etkinliði.
        DISCORD_HOPARLORU = 11, // Discord entegrasyonu 
        Gunluk_Harcama = 12, // Günlük kupon harcama etkinliði. Oyuncu kupon harcadýðýnda tetiklenir.
        Haftalýk_Harcama = 13, // Haftalýk kupon harcama etkinliði. Haftalýk toplam harcamayý takip eder.
        TOHUM_EKME = 14, // Çiftlik sistemi - Tohum ekme etkinliði.
        TOHUM_TOPLAMA = 15, // Çiftlik sistemi - Ürün toplama etkinliði.
        Arkadasdan_Ekin_Clalma = 16// Çiftlik sistemi - Arkadaþtan ekin çalma etkinliði.
    }
}
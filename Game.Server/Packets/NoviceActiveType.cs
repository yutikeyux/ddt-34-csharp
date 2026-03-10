namespace Game.Server.Packets
{
    public enum NoviceActiveType
    {
        GRADE_UP_ACTIVE = 1, // Seviye atlama etkinliði. Oyuncu seviye atladýðýnda tetiklenir.
        STRENGTHEN_WEAPON_ACTIVE = 2, // Silah güçlendirme (+basma) etkinliði. Silaha baþarýyla güçlendirme yapýldýðýnda tetiklenir.
        USE_MONEY_ACTIVE = 3, // Günlük kupon harcama etkinliði. Oyuncu kupon harcadýðýnda tetiklenir.
        DISCORD_HOPARLORU = 4, // Discord entegrasyonu (Sistem aktif deðil - Kupon yükleme eksik).
        UPGRADE_VIP_ACTIVE = 5, // VIP seviyesi yükseltme etkinliði. VIP kartý kullanýldýðýnda veya seviye atlandýðýnda tetiklenir.
        UPDATE_FIGHTPOWER = 6, // Savaþ gücü güncelleme etkinliði. Savaþ gücü belirli bir eþiði geçtiðinde tetiklenir.
        USE_MONEY_ACTIVE_OFWEEK = 7, // Haftalýk kupon harcama etkinliði. Haftalýk toplam harcamayý takip eder.
        Kýyafet_Guclendirme = 8, // Kýyafet/giysi güçlendirme etkinliði. 
        Ilk_Yukleme = 31, // Ýlk para yükleme (Ýlk Kez Yükleyenler) etkinliði. 
        Sapka_Guclendirme = 9, // Þapka güçlendirme etkinliði. 
        DestekEkipmanýGuclendirme = 10, // Destek ekipmaný güçlendirme etkinliði. 
        TohumEkme = 11, // Çiftlik sistemi - Tohum ekme etkinliði.
        TohumToplama = 12, // Çiftlik sistemi - Ürün toplama etkinliði.
        ArkadasindanEkinCalma = 13, // Çiftlik sistemi - Arkadaþtan ekin çalma etkinliði.
        BIRLIK_SAVASI = 14, // Lonca/Birlik savaþý etkinliði. Lonca savaþlarýna katýlýmda tetiklenir.
        PVP_MATCH_COUNT = 15, // PvP Maç Sayýsý. Belirli sayýda PvP maçý oynama.
        DUNGEON_COMPLETE = 16, // Keþif Rýhtýmý Etap Mantýðý - Belirli bir sayýda keþif rýhtýmý etabý tamamlama.
    }
}
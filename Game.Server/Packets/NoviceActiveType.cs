namespace Game.Server.Packets
{
    public enum NoviceActiveType
    { 
        GRADE_UP_ACTIVE = 1, //seviye atlama aktif çalýþýyor not: yuti
        STRENGTHEN_WEAPON_ACTIVE = 2, //silaha + basma aktif çalýþýyor not: yuti
        USE_MONEY_ACTIVE = 3, // kupon harcama aktif çalýþýyor not: yuti
        RECHANGE_MONEY_ACTIVE = 4, // kupon yükleme sistemimiz yok çalýþmýyor not: yuti
        UPGRADE_VIP_ACTIVE = 5, // VIP çalýþmýyor kupon harcama olarak geçiyor. not: yuti
        UPDATE_FIGHTPOWER = 6, // savaþma gücü bi týk bozuk gibi tam test edip anlayamadým not: yuti
        USE_MONEY_ACTIVE_OFWEEK = 7, // haftalýk kupon harcama aktif çalýþýyor. not: yuti
        RECHANGE_MONEY_ACTIVE_OFWEEK = 8, //haftalýk kupon yükleme sistemimiz yok çalýþmýyor. not: yuti
        FIRST_RECHARGE = 9, // ilk yükleme etkinliði sistemimiz yok çalýþmýyor. not: yuti
        OZGUR_SAVAS = 10, //referans olarak eklenmedi henüz
        BIRLIK_SAVAS = 11, //referans olarak eklenmedi henüz
        IKILI_SAVAS = 12, //referans olarak eklenmedi henüz
        KARINCA_KOLAY = 13, //referans olarak eklenmedi henüz
        KARINCA_NORMAL = 14, //referans olarak eklenmedi henüz
        CÝVCÝV_KOLAY = 15, //referans olarak eklenmedi henüz
        CÝVCÝV_NORMAL = 16, //referans olarak eklenmedi henüz
        CÝVCÝV_ZOR = 17, //referans olarak eklenmedi henüz
        BOGO_KOLAY = 18,//referans olarak eklenmedi henüz
        Kýyafet_Guclendirme = 19, //referans olarak eklenmedi henüz
        Sapka_Guclendirme = 20 //referans olarak eklenmedi henüz

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Cmd
{
    // Bu sınıf, oyundaki botların davranışlarını kontrol eder, özellikle saldırı ve sohbet fonksiyonlarını yönetir. not: yuti
    [GameCommand((byte)eTankCmdType.BOT_COMMAND, "战胜关卡中Boss翻牌")]
    public class BotCommand : ICommandHandler
    {
        // HandleCommand metodu, botun ana davranışlarını içerir: hedef seçme, sohbet gönderme ve saldırı düzenleme. not: yuti
        public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
        {
            // Sadece PVP oyunlarında bot aktif olacak şekilde kontrol edilir. not: yuti
            if (game is PVPGame)
            {
                PVPGame pvp = game as PVPGame;
                // Oyundaki tüm canlı oyuncular listelenir. not: yuti
                List<Player> players = pvp.GetAllLivingPlayers();
                // Düşman oyuncular için boş bir liste oluşturulur. not: yuti
                List<Player> enemies = new List<Player>();

                // Tüm oyuncular arasında döngü oluşturulup, farklı takımdakiler düşman listesine eklenir. not: yuti
                foreach (Player child in players)
                {
                    if (child.Team != player.Team)
                    {
                        enemies.Add(child);
                    }
                }

                // Rastgele sohbet seçimi yerine, saldırı türüne göre özel sohbet mesajları gönderilecek. not: yuti
                // Öncelikle hedef seçilir ve sonra hedefe uygun mesaj belirlenir. not: yuti
                Random rand = new Random();
                int next = rand.Next(0, enemies.Count);
                Player target = enemies.ElementAt(next);

                // Hedefe göre yön değiştirme işlemi. not: yuti
                if (target.X > player.X)
                {
                    player.ChangeDirection(1, 500);
                }
                else
                {
                    player.ChangeDirection(-1, 500);
                }

                // Saldırı türünü belirlemek için rastgele bir sayı seçilir. not: yuti
                Random rd = new Random();
                int nt = rd.Next(0, 3);

                // Saldırı parametreleri başlangıç değerleri. not: yuti
                int a = 10001; // İlk saldırı tipi (İkili Saldırı)
                int b = 10004; // İkinci saldırı tipi
                int c = 10008; // Üçüncü saldırı tipi
                int d = 0; // X koordinatı hedefi
                int e = 0; // Y koordinatı hedefi
                int k = 1; // Saldırı tekrar sayısı
                float time_s = 1.0f; // Saldırı süresi
                int boomcount = 1; // Patlama sayısı
                int delayy = 700; // Gecikme süresi

                // Mesafe kontrolü ve saldırı stratejisi belirleme. not: yuti
                if (Math.Abs(player.X - target.X) > 60)
                {
                    // Uzak mesafe saldırıları için stratejiler. not: yuti
                    if (nt == 0)
                    {
                        // Doğrudan hedefe nişan alma stratejisi. not: yuti
                        int ngu = rd.Next(0, 2);
                        if (ngu == 0)
                        {
                            // Tam hedefe nişan alma. not: yuti
                            boomcount = 1;
                            a = 10001; // +2
                            b = 10001; //+2
                            c = 0;
                            d = target.X;
                            e = target.Y;
                            k = 5;

                            // Hedefe doğrudan nişan alırken gönderilecek özel mesaj. not: yuti
                            game.SendChat(player.PlayerDetail, "+2 +2 saldırısının önemini daha önce biliyor muydun?");
                        }
                        else
                        {
                            // Hedefin yakınına nişan alma. not: yuti
                            boomcount = 1;
                            a = 10001; // İkili Saldırı
                            b = 10004; // %50 hasar artışı saldırı
                            c = 10008; // %10 hasar artışı saldırı
                            d = target.X + rd.Next(1, 3) * rd.Next(-10, 20);
                            e = target.Y;
                            k = 3;

                            // Hedefin yakınına nişan alırken gönderilecek özel mesaj. not: yuti
                            game.SendChat(player.PlayerDetail, "Seni yenebilmek için tüm güçlerimi kullanıyorum!");
                        }
                    }
                    else
                    {
                        // Oyuncu botun 3 4 5 6 7 mesafe solundaysa bot oyuncuyu buzlar ve gömer :D not: yuti
                        int ngu2 = rd.Next(0, 6);
                        if (target.X < player.X && (player.X - target.X) > 200 && (player.X - target.X) < 800)
                        {
                            // Orta mesafe saldırısı. not: yuti
                            if (ngu2 < 1) // botun sıra alma mantığı sanırım incelicez not: yuti
                            {
                                // Tekli hedefleme. not: yuti
                                boomcount = 1;
                                a = 0;
                                b = 10015; // buz
                                c = 0;
                                d = target.X + rd.Next(1, 5) * rd.Next(-10, 20);
                                e = target.Y;
                                k = 1;

                                // Orta mesafe tekli saldırı mesajı. not: yuti
                                game.SendChat(player.PlayerDetail, "Buzzz gibi soğukta ne kadar dayanabileceksin görelim!");
                            }
                            else
                            {
                                // Çoklu hedefleme. not: yuti
                                boomcount = 3; //üçlü saldırının asıl görünüm mantığı
                                a = 10001; // +2 Saldırı
                                b = 10003; // Üçlü Saldırı
                                c = 0;
                                d = target.X + rd.Next(1, 2) * rd.Next(-10, 20);
                                e = target.Y;
                                k = 3;

                                // Çoklu saldırı mesajı. not: yuti
                                game.SendChat(player.PlayerDetail, "Gömülmekten haz alır mıydın?");
                            }
                        }
                        else if (Math.Abs(player.X - target.X) > 900 && player.TurnNum >= 2)
                        {
                            // Çok uzak mesafe saldırısı. not: yuti
                            if (player.X > target.X) //oyuncunun 3,5 mesafe sağına uç
                                d = (target.X + 350);
                            else
                                d = (target.X - 350);//oyuncunun 3,5 mesafe soluna uç
                            boomcount = 1;
                            a = 0;
                            b = 10016; // uçak
                            c = 10010; // görünmezlik
                            e = target.Y - 100;
                            k = 1;

                            // Uzak mesafe saldırısı mesajı. not: yuti
                            game.SendChat(player.PlayerDetail, "Biraz uzaktasın galiba. Neyse geleyim de beni bul yiyosa!");
                        }
                        else
                        {
                            // Normal mesafe saldırısı. not: yuti
                            boomcount = 1;
                            a = 10002; // Tekli saldırı
                            b = 10004; // %50 hasar artışı
                            c = 10008; // %10 hasar artışı
                            d = target.X + rd.Next(1, 3) * rd.Next(-10, 20);
                            e = target.Y;
                            k = 2;

                            // Normal mesafe saldırısı mesajı. not: yuti
                            //game.SendChat(player.PlayerDetail, "Beni yenebileceğini mi sandın?!");
                        }
                    }

                    // Seçilen saldırı tiplerine göre item kullanımı. not: yuti
                    if (a != 0)
                    {
                        ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(a);
                        // Belirlenen gecikme süresi ile item kullanılır. not: yuti
                        player.CallFuction(delegate { player.UseItem(itemTemplate); }, delayy);
                    }
                    if (b != 0)
                    {
                        ItemTemplateInfo itemTemplate1 = ItemMgr.FindItemTemplate(b);
                        // İkinci item için biraz daha gecikme ile kullanılır. not: yuti
                        player.CallFuction(delegate { player.UseItem(itemTemplate1); }, delayy + 100);
                    }
                    if (c != 0)
                    {
                        ItemTemplateInfo itemTemplate2 = ItemMgr.FindItemTemplate(c);
                        // Üçüncü item için daha fazla gecikme ile kullanılır. not: yuti
                        player.CallFuction(delegate { player.UseItem(itemTemplate2); }, delayy + 200);
                    }

                    // Mesafeye göre saldırı süresi ayarlama. not: yuti
                    if (Math.Abs(player.X - target.X) < 200)
                    {
                        time_s = 1.0f;
                    }
                    else if (Math.Abs(player.X - target.X) < 400)
                    {
                        time_s = 1.5f;
                    }
                    else if (Math.Abs(player.X - target.X) < 700)
                    {
                        time_s = 2.0f;
                    }
                    else if (Math.Abs(player.X - target.X) < 1000)
                    {
                        time_s = 2.5f;
                    }
                    else if (Math.Abs(player.X - target.X) < 1100)
                    {
                        time_s = 3.0f;
                    }
                    else
                    {
                        time_s = 3.5f;
                    }
                }
                else
                {
                    // Yakın mesafe saldırı stratejileri. not: yuti
                    rd = new Random();
                    int nt2 = rd.Next(0, 4);
                    if (nt2 == 0 && player.TurnNum >= 1)
                    {
                        // Özel yakın mesafe saldırısı. not: yuti
                        boomcount = 1;
                        b = 10010; // görünmezlik
                        c = 10016; // uçak
                        e = player.Y + rd.Next(-10, 10) * 20;
                        k = 1;
                        time_s = 4.0f;

                        // Oyuncunun konumuna göre hedef belirleme. not: yuti
                        if (player.X > 700)
                            d = (player.X - 600);
                        else
                            d = (player.X + 600);

                        // Item kullanımı. not: yuti
                        ItemTemplateInfo itemTemplate1 = ItemMgr.FindItemTemplate(b);
                        ItemTemplateInfo itemTemplate2 = ItemMgr.FindItemTemplate(c);
                        player.CallFuction(delegate { player.UseItem(itemTemplate2); }, delayy);
                        player.CallFuction(delegate { player.UseItem(itemTemplate1); }, delayy + 500);

                        // Yakın mesafe saldırısı mesajı. not: yuti
                        game.SendChat(player.PlayerDetail, "Çok yakınımdasın, biraz oyunu uzatalım :D");
                    }
                    else
                    {
                        // Basit yakın mesafe saldırısı. not: yuti
                        boomcount = 1;
                        b = 10010; //görünmezlik
                        c = 10016; //uçak
                        e = player.Y + rd.Next(-10, 10) * 20;
                        k = 1;
                        time_s = 4.0f;

                        if (player.X > 700)
                            d = (player.X - 600);
                        else
                            d = (player.X + 600);

                        ItemTemplateInfo itemTemplate1 = ItemMgr.FindItemTemplate(b);
                        ItemTemplateInfo itemTemplate2 = ItemMgr.FindItemTemplate(c);
                        player.CallFuction(delegate { player.UseItem(itemTemplate2); }, delayy);
                        player.CallFuction(delegate { player.UseItem(itemTemplate1); }, delayy + 500);

                        // Basit yakın mesafe saldırısı mesajı. not: yuti
                        game.SendChat(player.PlayerDetail, "Yanıma bu kadar yaklaşma bak. Sevmiyorum böyle şeyleri!");
                    }
                }

                // Gecikmeli saldırı fonksiyonu çağrılır. not: yuti
                int delayDisX = game.GetDelayDistance(player.X, target.X, 6) + 1200;
                player.CallFuction(delegate
                {
                    // Belirlenen sayıda saldırı yapılır. not: yuti
                    for (int i = 0; i < k; i++)
                    {
                        player.ShootPoint(d, e, player.CurrentBall.ID, 1001, 10001, boomcount, time_s, 3000);
                    }
                }, delayy + 1500);

                // Saldırı sonrası durdurma fonksiyonu. not: yuti
                player.CallFuction(delegate
                {
                    if (player.IsAttacking)
                        player.StopAttacking();
                }, delayy + 2000);

                // Bot komut paketi oluşturulur ve tüm oyunculara gönderilir. not: yuti
                GSPacketIn pkg = new GSPacketIn((byte)ePackageTypeLogic.GAME_CMD, player.Id);
                pkg.WriteByte((byte)eTankCmdType.BOT_COMMAND);
                game.SendToAll(pkg);
            }
        }
    }
}
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
    [GameCommand((byte)eTankCmdType.BOT_COMMAND, "战胜关卡中Boss翻牌")]
    public class BotCommand : ICommandHandler
    {
        public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
        {
            if (!(game is PVPGame)) return;

            PVPGame pvp = game as PVPGame;
            List<Player> players = pvp.GetAllLivingPlayers();
            List<Player> enemies = new List<Player>();

            foreach (Player child in players)
            {
                if (child.Team != player.Team)
                    enemies.Add(child);
            }

            if (enemies.Count == 0) return;

            Random rd = new Random(Guid.NewGuid().GetHashCode());

            // En yakın düşmanı seç
            Player target = enemies.OrderBy(e2 => Math.Abs(e2.X - player.X)).First();

            // Yön ayarla
            player.ChangeDirection(target.X > player.X ? 1 : -1, 500);

            int dist = Math.Abs(player.X - target.X);

            // Iskalama şansı: %25 ihtimalle kasıtlı sapma
            bool willMiss = rd.Next(0, 100) < 25;
            int missOffset = willMiss ? rd.Next(30, 80) * (rd.Next(0, 2) == 0 ? 1 : -1) : 0;

            // Saldırı parametreleri
            int itemA = 0;
            int itemB = 0;
            int itemC = 0;
            int targetX = target.X + missOffset;
            int targetY = target.Y;
            int shootCount = 1;
            int boomcount = 1;
            float time_s = 1.0f;

            // Prop basış gecikmesi — tur başından itibaren yeterince bekle
            // İlk prop: 1800ms, ikinci prop: 3000ms, üçüncü prop: 4200ms
            int propDelay1 = 1800;
            int propDelay2 = 3000;
            int propDelay3 = 4200;

            // Atış gecikmesi prop kullanımlarının bitmesinden sonra başlar
            int shootDelay = 5500;

            string chatMsg = "";

            // ─────────────────────────────────────────
            // UZAK MESAFİ  (dist > 600)
            // ─────────────────────────────────────────
            if (dist > 600)
            {
                int roll = rd.Next(0, 5);

                if (roll == 0)
                {
                    // Uçak + görünmezlik — kaçma/yaklaşma
                    itemB = 10016; // uçak
                    itemC = 10010; // görünmezlik
                    targetX = target.X > player.X
                        ? target.X - rd.Next(250, 400)
                        : target.X + rd.Next(250, 400);
                    targetY = target.Y - 80;
                    shootCount = 1;
                    time_s = 3.5f;
                    chatMsg = "Biraz uzaktasın, geleyim de seninle ilgileneyim!";
                }
                else if (roll == 1)
                {
                    // +2 +2 yaklaşık nişan
                    itemA = 10001;
                    itemB = 10001;
                    targetX = target.X + rd.Next(-15, 15) + missOffset;
                    shootCount = 5;
                    boomcount = 1;
                    time_s = 3.0f;
                    chatMsg = "+2 +2 saldırısının tadını çıkar!";
                }
                else if (roll == 2)
                {
                    // Tekli direkt nişan
                    itemA = 10002;
                    itemB = 10004;
                    targetX = target.X + rd.Next(-20, 20) + missOffset;
                    shootCount = 2;
                    boomcount = 1;
                    time_s = 3.0f;
                    chatMsg = "Dur bakalım seni bir bulayım önce!";
                }
                else if (roll == 3)
                {
                    // Üçlü bomba
                    itemA = 10001;
                    itemB = 10003;
                    targetX = target.X + rd.Next(-25, 25) + missOffset;
                    shootCount = 3;
                    boomcount = 3;
                    time_s = 3.0f;
                    chatMsg = "Üçlüyle ne kadar uzakta olduğun fark etmez!";
                }
                else
                {
                    // %50 hasar + %10 hasar artışlı
                    itemB = 10004;
                    itemC = 10008;
                    targetX = target.X + rd.Next(-30, 30) + missOffset;
                    shootCount = 2;
                    boomcount = 1;
                    time_s = 3.5f;
                }

                // Mesafeye göre süre ince ayarı
                if (dist < 800) time_s = 3.0f;
                else if (dist < 1000) time_s = 3.3f;
                else time_s = 3.6f;
            }
            // ─────────────────────────────────────────
            // ORTA MESAFİ  (200 < dist <= 600)
            // ─────────────────────────────────────────
            else if (dist > 200)
            {
                int roll = rd.Next(0, 6);

                if (roll == 0)
                {
                    // Buz — sadece 1/6 ihtimal
                    itemB = 10015;
                    targetX = target.X + rd.Next(-20, 20) + missOffset;
                    shootCount = 1;
                    boomcount = 1;
                    time_s = 2.0f;
                    chatMsg = "Donmaya hazır mısın?";
                }
                else if (roll == 1)
                {
                    // +2 direkt
                    itemA = 10001;
                    itemB = 10001;
                    targetX = target.X + rd.Next(-10, 10) + missOffset;
                    shootCount = 5;
                    boomcount = 1;
                    time_s = 2.0f;
                    chatMsg = "Tam isabetle geliyor!";
                }
                else if (roll == 2)
                {
                    // Üçlü patlama
                    itemA = 10001;
                    itemB = 10003;
                    targetX = target.X + rd.Next(-15, 15) + missOffset;
                    shootCount = 3;
                    boomcount = 3;
                    time_s = 2.0f;
                    chatMsg = "Üçlüyle selamlamak istedim!";
                }
                else if (roll == 3)
                {
                    // %50 hasar + %10 hasar
                    itemB = 10004;
                    itemC = 10008;
                    targetX = target.X + rd.Next(-20, 20) + missOffset;
                    shootCount = 1;
                    boomcount = 1;
                    time_s = 2.2f;
                }
                else if (roll == 4)
                {
                    // Tekli güçlü atış
                    itemA = 10002;
                    itemB = 10004;
                    targetX = target.X + rd.Next(-15, 15) + missOffset;
                    shootCount = 2;
                    boomcount = 1;
                    time_s = 2.0f;
                    chatMsg = "Güçlü bir atış geliyor!";
                }
                else
                {
                    // Çift normal atış
                    itemA = 10001;
                    itemB = 10008;
                    targetX = target.X + rd.Next(-20, 25) + missOffset;
                    shootCount = 3;
                    boomcount = 1;
                    time_s = 2.2f;
                }

                // Mesafeye göre süre
                if (dist < 300) time_s = 1.6f;
                else if (dist < 450) time_s = 1.9f;
                else time_s = 2.2f;
            }
            // ─────────────────────────────────────────
            // YAKIN MESAFİ  (dist <= 200)
            // ─────────────────────────────────────────
            else
            {
                int roll = rd.Next(0, 5);

                if (roll == 0 && player.TurnNum >= 1)
                {
                    // Kaç + görünmezlik — prop'lar ayrıca aşağıda tetikleniyor
                    itemB = 10010; // görünmezlik
                    itemC = 10016; // uçak
                    targetX = player.X > 700
                        ? player.X - rd.Next(500, 650)
                        : player.X + rd.Next(500, 650);
                    targetY = player.Y + rd.Next(-10, 10) * 15;
                    shootCount = 1;
                    time_s = 4.0f;
                    chatMsg = "Çok yakınımdasın, biraz mesafe koyayım!";

                    // Yakın kaçış prop'ları özel sırayla tetiklenir (uçak önce, görünmezlik sonra)
                    ItemTemplateInfo tplUcak = ItemMgr.FindItemTemplate(10016);
                    ItemTemplateInfo tplGorunmez = ItemMgr.FindItemTemplate(10010);
                    player.CallFuction(delegate { player.UseItem(tplUcak); }, propDelay1);
                    player.CallFuction(delegate { player.UseItem(tplGorunmez); }, propDelay2);

                    // Zaten elle tetiklendiler, tekrar basılmasın
                    itemB = 0;
                    itemC = 0;
                }
                else if (roll == 1)
                {
                    // Yakında +2 direkt
                    itemA = 10001;
                    itemB = 10001;
                    targetX = target.X + rd.Next(-8, 8) + missOffset;
                    shootCount = 5;
                    boomcount = 1;
                    time_s = 1.2f;
                    chatMsg = "Bu kadar yakınsan bari +2 yesin!";
                }
                else if (roll == 2)
                {
                    // Yakında üçlü
                    itemA = 10001;
                    itemB = 10003;
                    targetX = target.X + rd.Next(-10, 10) + missOffset;
                    shootCount = 3;
                    boomcount = 3;
                    time_s = 1.2f;
                    chatMsg = "Yakında üçlü patlasın!";
                }
                else if (roll == 3)
                {
                    // Buz yakında — 1/5 ihtimal
                    itemB = 10015;
                    targetX = target.X + rd.Next(-10, 10) + missOffset;
                    shootCount = 1;
                    boomcount = 1;
                    time_s = 1.2f;
                    chatMsg = "Donunca hareket edemezsin!";
                }
                else
                {
                    // Normal yakın atış
                    itemB = 10004;
                    itemC = 10008;
                    targetX = target.X + rd.Next(-12, 12) + missOffset;
                    shootCount = 1;
                    boomcount = 1;
                    time_s = 1.3f;
                }

                time_s = 1.2f;
            }

            // ─────────────────────────────────────────
            // PROP KULLANIMI
            // 10xxx ID'li itemlar yavaş basılır:
            //   İlk  prop → 1800ms
            //   İkinci prop → 3000ms
            //   Üçüncü prop → 4200ms
            // Atış bu gecikmelerden sonra başlar (5500ms)
            // ─────────────────────────────────────────
            if (itemA != 0)
            {
                ItemTemplateInfo tpl = ItemMgr.FindItemTemplate(itemA);
                player.CallFuction(delegate { player.UseItem(tpl); }, propDelay1);
            }
            if (itemB != 0)
            {
                ItemTemplateInfo tpl = ItemMgr.FindItemTemplate(itemB);
                // itemA kullanıldıysa ikinci slota, kullanılmadıysa birinci slota bas
                int delay = (itemA != 0) ? propDelay2 : propDelay1;
                player.CallFuction(delegate { player.UseItem(tpl); }, delay);
            }
            if (itemC != 0)
            {
                ItemTemplateInfo tpl = ItemMgr.FindItemTemplate(itemC);
                // Kaçının kullanıldığı prop sayısına göre slotu belirle
                int delay;
                if (itemA != 0 && itemB != 0)
                    delay = propDelay3;
                else if (itemA != 0 || itemB != 0)
                    delay = propDelay2;
                else
                    delay = propDelay1;
                player.CallFuction(delegate { player.UseItem(tpl); }, delay);
            }

            // Sohbet mesajı
            if (!string.IsNullOrEmpty(chatMsg))
                game.SendChat(player.PlayerDetail, chatMsg);

            // ─────────────────────────────────────────
            // ATIŞ — tüm prop'lar basıldıktan sonra
            // ─────────────────────────────────────────
            int finalX = targetX;
            int finalY = targetY;
            int finalShoot = shootCount;
            int finalBoom = boomcount;
            float finalTime = time_s;

            player.CallFuction(delegate
            {
                for (int i = 0; i < finalShoot; i++)
                {
                    player.ShootPoint(
                        finalX, finalY,
                        player.CurrentBall.ID,
                        1001, 10001,
                        finalBoom, finalTime, 3000);
                }
            }, shootDelay);

            // Saldırıyı durdur
            player.CallFuction(delegate
            {
                if (player.IsAttacking)
                    player.StopAttacking();
            }, shootDelay + 700);

            // Paket gönder
            GSPacketIn pkg = new GSPacketIn((byte)ePackageTypeLogic.GAME_CMD, player.Id);
            pkg.WriteByte((byte)eTankCmdType.BOT_COMMAND);
            game.SendToAll(pkg);
        }
    }
}
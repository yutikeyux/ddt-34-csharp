using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Drawing;

namespace GameServerScript.AI.NPC
{
    public class ZamanKelebegi : ABrain
    {
        // NPC oluşturulduğunda çalışır
        public override void OnCreated()
        {
            base.OnCreated();
            // NPC'nin yerçekiminden etkilenmemesini sağlar.
            // Mission dosyasında verilen Y koordinatında (havada) kalmasını sağlar.
            Body.Config.IsFly = true;
        }

        // Saldırı başladığında çalışır
        public override void OnStartAttacking()
        {
            // DÜZELTME: Metod parametre olarak x ve y koordinatlarını alır.
            Player target = Game.FindNearestPlayer(Body.X, Body.Y);

            if (target != null && target.IsLiving)
            {
                // Hedefin yönüne göre bak
                if (target.X > Body.X)
                {
                    Body.Direction = 1;
                }
                else
                {
                    Body.Direction = -1;
                }

                // DÜZELTME: Distance metodu için Point kullanıyoruz
                double distance = Body.Distance(new Point(target.X, target.Y));

                // Eğer hedef çok yakındaysa (örneğin 100 pikselden az) direkt saldır
                if (distance < 100)
                {
                    AttackTarget(target);
                }
                else
                {
                    // Değilse, hedefe doğru "uçarak" yaklaş ve bitince saldır
                    // IsFly true olduğu için MoveTo uçarak hareket eder
                    Body.MoveTo(target.X, target.Y, "fly", 1000, new LivingCallBack(AttackCallback));
                }
            }
        }

        // MoveTo işlemi bittiğinde çağrılır
        private void AttackCallback()
        {
            // Saldırı anında tekrar en yakın hedefi bul (hareket sırasında hedef değişmiş olabilir)
            Player target = Game.FindNearestPlayer(Body.X, Body.Y);
            if (target != null && target.IsLiving)
            {
                AttackTarget(target);
            }
        }

        // Saldırı fonksiyonu
        private void AttackTarget(Living target)
        {
            // "beat" animasyonu ile saldır
            Body.Beat(target, "beat", 0, 0, 1000);
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }
    }
}
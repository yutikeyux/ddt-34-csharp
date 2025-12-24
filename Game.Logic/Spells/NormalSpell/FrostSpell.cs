using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Spells.NormalSpell
{
    [SpellAttibute(2)]
    public class FrostSpell : ISpellHandler
    {
        public void Execute(BaseGame game, Player player, ItemTemplateInfo item)
        {
            if (player.IsLiving)
            {
                // Eðer kullanýlan eþyanýn ID'si 10003 DEÐÝLSE, etkiyi uygula
                if (item.TemplateID != 10003)
                {
                    player.SetBall(1);
                }
                else
                {
                    // Eðer eþya ID'si 10003 ise, burasý çalýþýr.
                    // Ýstediðiniz gibi etki uygulanmaz (hiçbir þey yapmaz).
                }
            }
            else if (game.CurrentLiving != null && game.CurrentLiving is Player && game.CurrentLiving.Team == player.Team)
            {
                // Ayný mantýðý, ölü oyuncunun canlý takým arkadaþýna destek verirken de uyguluyoruz.
                if (item.TemplateID != 10003)
                {
                    (game.CurrentLiving as Player).SetBall(1);
                }
                else
                {
                    // Eðer eþya ID'si 10003 ise, etki uygulanmaz.
                }
            }
        }
    }
}
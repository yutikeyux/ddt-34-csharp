using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Spells.NormalSpell
{
    [SpellAttibute(1)]
    public class AddLifeSpell : ISpellHandler
    {
        public void Execute(BaseGame game, Player player, ItemTemplateInfo item)
        {
            switch (item.Property2)
            {
                case 0: // Tek hedefe can verme (kendine veya canlý takým arkadaþýna)
                    {
                        // item.Property3 artýk bir yüzde deðeri (örn: 20 = %20)
                        float yuzdeDeger = item.Property3 / 100.0f;

                        if (!player.IsLiving) // Eþyayý kullanan oyuncu ölüyse
                        {
                            // Sýrasý gelen canlý bir takým arkadaþý var mý?
                            if (game.CurrentLiving != null && game.CurrentLiving is Player && game.CurrentLiving.Team == player.Team)
                            {
                                Player hedef = game.CurrentLiving as Player;
                                // Hedefin maksimum canýnýn %X'i kadar can hesapla
                                int canArtisi = (int)(hedef.MaxBlood * yuzdeDeger);
                                hedef.AddBlood(canArtisi);
                            }
                            break;
                        }

                        // Eþyayý kullanan oyuncu hayattaysa kendine uygular
                        Player kullanici = player;
                        int temelCanArtisi = (int)(kullanici.MaxBlood * yuzdeDeger);

                        // Varsa ekstra can bonusu ekle (bu bonus yüzdelik deðil, sabittir)
                        if (kullanici.FightBuffers.ConsortionAddSpellCount > 0)
                        {
                            temelCanArtisi += kullanici.FightBuffers.ConsortionAddSpellCount;
                        }

                        kullanici.AddBlood(temelCanArtisi);
                        break;
                    }
                case 1: // Takýmdaki tüm canlýlara can verme
                    {
                        // item.Property3 artýk bir yüzde deðeri (örn: 20 = %20)
                        float yuzdeDeger = item.Property3 / 100.0f;

                        foreach (Player takimArkadasi in player.Game.GetAllFightPlayers())
                        {
                            // Sadece kendi takýmýndan ve hayatta olanlara uygula
                            if (takimArkadasi.IsLiving && takimArkadasi.Team == player.Team)
                            {
                                // Her takým arkadaþýnýn kendi maksimum canýna göre can artýþý hesapla
                                int canArtisi = (int)(takimArkadasi.MaxBlood * yuzdeDeger);
                                takimArkadasi.AddBlood(canArtisi);
                            }
                        }
                        break;
                    }
            }
        }
    }
}
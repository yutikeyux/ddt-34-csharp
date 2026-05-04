using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
    public class AE1021(int count, int probability, int type, int skillId, int delay, string elementID) : BasePetEffect(ePetEffectType.AE1021, elementID)
    {
        private readonly int m_type = type;
        private readonly int m_count = count;
        private int m_probability = probability == -1 ? 10000 : probability;
        private readonly int m_delay = delay;
        private readonly int m_coldDown = count;
        private readonly int m_currentId = skillId;
        private readonly int m_added = 0;

        public override bool Start(Living living)
        {
            if (living.PetEffectList.GetOfType(ePetEffectType.AE1021) is AE1021 effect)
            {
                effect.m_probability = m_probability > effect.m_probability ? m_probability : effect.m_probability;
                return true;
            }
            else
            {
                return base.Start(living);
            }
        }

        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(player_AfterBuffSkillPetByLiving);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(player_AfterBuffSkillPetByLiving);
        }

        private void player_AfterBuffSkillPetByLiving(Player player)
        {
            if (player.PetEffects.CurrentUseSkill == m_currentId)
            {
                player.AddPetEffect(new CE1021(2, m_probability, m_type, m_currentId, m_delay, ElementInfo.ID.ToString()), 0);
                //Console.WriteLine("Buff Name: {2}, ID: {0}, player.CurrentDamagePlus: {1}", ElementInfo.ID, player.CurrentDamagePlus, ElementInfo.Name);
            }
        }
    }
}

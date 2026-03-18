using Game.Base.Packets;
using Game.Server.Packets;

namespace Game.Server.Farm.Handle
{
    [FarmHandleAttbute(2)]
    public class GrowFields : IFarmCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            int num = packet.ReadByte();
            int num2 = packet.ReadInt();
            int fieldId = packet.ReadInt();
            if (Player.Farm.GrowField(fieldId, num2))
            {
                Player.FarmBag.RemoveTemplate(num2, 1);
                Player.OnSeedFoodPetEvent();

                // Etkinlik Mantığı: Tohum Ekme
                if (Player.Extra.CheckNoviceActiveOpen(NoviceActiveType.TOHUM_EKME))
                {
                    var info = Player.Client.Player.Extra.GetEventProcess((int)NoviceActiveType.TOHUM_EKME);
                    Player.Client.Player.Extra.UpdateEventCondition((int)NoviceActiveType.TOHUM_EKME, info.Conditions + 1);
                }
            }
            return true;
        }
    }
}
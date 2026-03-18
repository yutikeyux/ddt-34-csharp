using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets.Client;
using Game.Base.Packets;
using Game.Server.Rooms;
using Game.Logic;
using Game.Server.Buffer;
using SqlDataProvider.Data;
using Bussiness;

namespace Game.Server.Farm.Handle
{
    [FarmHandleAttbute((byte)FarmPackageType.GAIN_FIELD)]
    public class GainFields : IFarmCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            int userId = packet.ReadInt();
            int fieldId = packet.ReadInt();//param1.fieldID
            string msg = LanguageMgr.GetTranslation("EnterFarmHandler.Msg2");

            if (userId == Player.PlayerCharacter.ID && Player.Farm.GainField(fieldId))
            {
                msg = LanguageMgr.GetTranslation("Ekin Toplama Baþarýlý!"); //türkçeleþtirildi not: yuti

                // Etkinlik Mantýðý: Tohum Toplama (Kendi tarlasýndan hasat yapma)
                var info = Player.Client.Player.Extra.GetEventProcess((int)NoviceActiveType.TOHUM_TOPLAMA);
                Player.Client.Player.Extra.UpdateEventCondition((int)NoviceActiveType.TOHUM_TOPLAMA, info.Conditions + 1);
            }
            else if (userId != Player.PlayerCharacter.ID)
            {
                if (Player.Farm.GainFriendFields(userId, fieldId))
                {
                    msg = LanguageMgr.GetTranslation("Ekin Çalma Baþarýlý!"); //türkçeleþtirildi not: yuti

                    // Etkinlik Mantýðý: Arkadaþtan Ekin Çalma
                    if (Player.Extra.CheckNoviceActiveOpen(NoviceActiveType.Arkadasdan_Ekin_Clalma))
                    {
                        var info2 = Player.Client.Player.Extra.GetEventProcess((int)NoviceActiveType.Arkadasdan_Ekin_Clalma);
                        Player.Extra.UpdateEventCondition((int)NoviceActiveType.Arkadasdan_Ekin_Clalma, info2.Conditions + 1);
                    }
                }
                else
                {
                    msg = LanguageMgr.GetTranslation("Daha Fazla Çalamazsýn!"); //türkçeleþtirildi not: yuti
                }
            }
            Player.SendMessage(msg);
            return true;
        }
    }
}
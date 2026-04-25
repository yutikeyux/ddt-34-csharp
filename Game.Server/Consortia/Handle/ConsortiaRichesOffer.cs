using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets.Client;
using Game.Base.Packets;
using Game.Server.Buffer;
using SqlDataProvider.Data;
using Bussiness;

namespace Game.Server.Consortia.Handle
{
    [ConsortiaHandleAttbute((byte)ConsortiaPackageType.CONSORTIA_RICHES_OFFER)]
    public class ConsortiaRichesOffer : IConsortiaCommandHadler
    {
        public int CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            if (Player.PlayerCharacter.ConsortiaID == 0)
                return 0;

            int money = packet.ReadInt();
            if (Player.PlayerCharacter.HasBagPassword && Player.PlayerCharacter.IsLocked)
            {
                Player.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 1;
            }

            if (money < 1 || Player.PlayerCharacter.Money < money)
            {
                Player.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ConsortiaRichesOfferHandler.NoMoney"));
                return 1;
            }

            int riches = money / 2;
            if (riches <= 0)
            {
                Player.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ConsortiaRichesOfferHandler.RichIsNotFound", 2));
                return 1;
            }

            // ---------------------------------------------------------
            // GÜVENLİK GÜNCELLEMESİ: GÜNLÜK LİMİT KONTROLÜ
            // ---------------------------------------------------------
            bool moneySuccess = true;
            if (money > 0)
            {
                // Limit kontrolü ve para kesme işlemi burada yapılıyor
                moneySuccess = Player.MoneyDirect(money, true, false, true);
            }

            bool result = false;
            string msg = "ConsortiaRichesOfferHandler.Failed";

            // Sadece para kesme işlemi başarılıysa (Limit aşımı yoksa) DB işlemine gir
            if (moneySuccess)
            {
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    if (db.ConsortiaRichAdd(Player.PlayerCharacter.ConsortiaID, ref riches, 5, Player.PlayerCharacter.NickName))
                    {
                        result = true;
                        Player.PlayerCharacter.RichesOffer += riches;
                        Player.PlayerCharacter.RichesRob += riches;

                        //Player.SetMoney(-money);

                        // NOT: MoneyDirect zaten parayı kestiği için buradan tekrar kesmemize gerek yok. 
                        // Aksi takdirde oyuncunun parası 2 kez kesilir (Çift kesim açığı).
                        //Player.RemoveMoney(money); 

                        Player.OnDonateRiches(riches, 1);
                        msg = "ConsortiaRichesOfferHandler.Successed";
                        GameServer.Instance.LoginServer.SendConsortiaRichesOffer(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, Player.PlayerCharacter.NickName, riches);
                    }
                }
            }
            else
            {
                // ÖDEME BAŞARISIZSA (LİMİT DOLU)
                msg = "Bağış başarısız! Günlük kupon harcama limitinizi aşmış olabilirsiniz.";
            }
            // ---------------------------------------------------------
            // GÜVENLİK GÜNCELLEMESİ SONU
            // ---------------------------------------------------------

            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.CONSORTIA_CMD);
            pkg.WriteByte((byte)ConsortiaPackageType.CONSORTIA_RICHES_OFFER);
            pkg.WriteInt(money);
            pkg.WriteBoolean(result);
            pkg.WriteString(LanguageMgr.GetTranslation(msg));
            Player.Out.SendTCP(pkg);
            return 0;
        }
    }
}
using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Pet.Handle
{
	[PetHandleAttbute((byte)PetPackageType.ADD_PET_EQUIP)]
	public class AddPetEquip : IPetCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
			int bageType = packet.ReadInt();
			int slot = packet.ReadInt();
			int place = packet.ReadInt();
			PlayerInventory inventory = Player.GetInventory((eBageType)bageType);
			ItemInfo itemAt = inventory.GetItemAt(slot);
			//Console.WriteLine(11111111);
			if (itemAt != null && itemAt.IsEquipPet() && itemAt.IsValidItem())
			{
				if (Player.PetBag.CheckEqPetLevel(place, itemAt))
				{
					Player.SendMessage(LanguageMgr.GetTranslation("Seviye Yetersiz!"));
					return false;
				}

				if (!itemAt.IsUsed)
				{
					itemAt.IsUsed = true;
					itemAt.BeginDate = DateTime.Now;
				}
				if (Player.PetBag.AddEqPet(place, itemAt))
				{
					inventory.TakeOutItem(itemAt);
					Player.PetBag.OnChangedPetEquip(place);
					Player.PetBag.SaveToDatabase(false);
					Player.SendMessage(LanguageMgr.GetTranslation("Kuşanma başarılı!"));

				}
				else
				{
					Player.SendMessage(LanguageMgr.GetTranslation("Bir hata oluştu, lütfen yöneticiyle iletişime geçin!"));
				}
			}
			else
			{
				Player.SendMessage(LanguageMgr.GetTranslation("Ekipman Arızası!"));
			}
			return false;
        }
    }
}

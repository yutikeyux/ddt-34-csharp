using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.GypsyShop.Handle
{
	[GypsyShopHandleAttbute(3)]
	public class GypsyShopBuy : IGypsyShopCommandHadler
	{
		public bool CommandHandler(GamePlayer player, GSPacketIn packet)
		{
			int ıD = packet.ReadInt();
			bool flag = packet.ReadBoolean();
			GypsyItemDataInfo mysteryShopByID = player.Actives.GetMysteryShopByID(ıD);
			if (mysteryShopByID != null)
			{
				if (mysteryShopByID.CanBuy == 1)
				{
					bool flag2 = false;
					if (mysteryShopByID.Unit == 1 && player.RemoveMoney(mysteryShopByID.Price) > 0)
					{
						flag2 = true;
					}
					else if (mysteryShopByID.Unit == 2 && player.PlayerCharacter.myHonor >= mysteryShopByID.Price)
					{
						flag2 = true;
					}
					if (flag2)
					{
						ItemTemplateInfo ıtemTemplateInfo = ItemMgr.FindItemTemplate(mysteryShopByID.InfoID);
						if (ıtemTemplateInfo != null)
						{
							ItemInfo ıtemInfo = ItemInfo.CreateFromTemplate(ıtemTemplateInfo, 1, 105);
							ıtemInfo.IsBinds = true;
							ıtemInfo.ValidDate = mysteryShopByID.Num;
							player.AddTemplate(ıtemInfo);
						}
						player.SendMessage(LanguageMgr.GetTranslation("GypsyShopBuy.Success"));
						player.Actives.UpdateMysteryShopByID(ıD);
						if (mysteryShopByID.Unit == 2)
						{
							player.RemovemyHonor(mysteryShopByID.Price);
						}
					}
					if (mysteryShopByID.Unit == 2 && !flag2)
					{
						player.SendMessage(LanguageMgr.GetTranslation("GypsyShopBuy.OutHornor"));
					}
					GSPacketIn gSPacketIn = new GSPacketIn(278, player.PlayerCharacter.ID);
					gSPacketIn.WriteByte(3);
					gSPacketIn.WriteInt(mysteryShopByID.GypsyID);
					gSPacketIn.WriteBoolean(flag2);
					if (flag2)
					{
						gSPacketIn.WriteInt(mysteryShopByID.CanBuy);
					}
					player.Out.SendTCP(gSPacketIn);
				}
				else
				{
					player.SendMessage(LanguageMgr.GetTranslation("GypsyShopBuy.OutOfDate"));
				}
			}
			else
			{
				player.SendMessage(LanguageMgr.GetTranslation("GypsyShopBuy.Fail"));
			}
			return false;
		}
	}
}

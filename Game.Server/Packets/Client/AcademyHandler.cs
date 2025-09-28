using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(141, "防沉迷系统开关")]
	public class AcademyHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
			switch (packet.ReadByte())
			{
			case 4:
			{
				int num5 = packet.ReadInt();
				string str2 = packet.ReadString();
				if (AcademyMgr.GetRequest(client.Player.PlayerId, num5) != null)
				{
					break;
				}
				if (client.Player.PlayerCharacter.freezesDate <= DateTime.Now)
				{
					GamePlayer playerById3 = WorldMgr.GetPlayerById(num5);
					if (playerById3 != null && playerById3.PlayerCharacter.apprenticeshipState < AcademyMgr.MASTER_FULL_STATE && AcademyMgr.CheckCanMaster(playerById3.PlayerCharacter.Grade))
					{
						AcademyMgr.AddRequest(new AcademyRequestInfo
						{
							SenderID = client.Player.PlayerId,
							ReceiderID = num5,
							Type = 1,
							CreateTime = DateTime.Now
						});
						GSPacketIn gSPacketIn2 = new GSPacketIn(141);
						gSPacketIn2.WriteByte(4);
						gSPacketIn2.WriteInt(client.Player.PlayerId);
						gSPacketIn2.WriteString(client.Player.PlayerCharacter.NickName);
						gSPacketIn2.WriteString(str2);
						playerById3.SendTCP(gSPacketIn2);
					}
					else
					{
						client.Player.SendMessage($"Oyuncu çevrimdışı.");
					}
				}
				else
				{
					client.Player.SendMessage($"Daha önceden zaten terk ettiğiniz için kısıtlandınız. Kalan Zamanınız: {checkDate(client.Player.PlayerCharacter.freezesDate)}.");
				}
				break;
			}
			case 5:
			{
				int num4 = packet.ReadInt();
				string str = packet.ReadString();
				if (AcademyMgr.GetRequest(client.Player.PlayerId, num4) != null)
				{
					break;
				}
				if (client.Player.PlayerCharacter.freezesDate <= DateTime.Now)
				{
					GamePlayer playerById2 = WorldMgr.GetPlayerById(num4);
					if (playerById2 != null && playerById2.PlayerCharacter.masterID == 0 && AcademyMgr.CheckCanApp(playerById2.PlayerCharacter.Grade))
					{
						AcademyMgr.AddRequest(new AcademyRequestInfo
						{
							SenderID = client.Player.PlayerId,
							ReceiderID = num4,
							Type = 0,
							CreateTime = DateTime.Now
						});
						GSPacketIn gSPacketIn = new GSPacketIn(141);
						gSPacketIn.WriteByte(5);
						gSPacketIn.WriteInt(client.Player.PlayerId);
						gSPacketIn.WriteString(client.Player.PlayerCharacter.NickName);
						gSPacketIn.WriteString(str);
						playerById2.SendTCP(gSPacketIn);
					}
					else
					{
						client.Player.SendMessage($"Oyuncu çevrimiçi değil.");
					}
				}
				else
				{
					client.Player.SendMessage($"Daha önceden terk ettiğiniz için kısıtlandınız. Kalan zamanınız: {checkDate(client.Player.PlayerCharacter.freezesDate)}.");
				}
				break;
			}
			case 6:
			{
				int num7 = packet.ReadInt();
				AcademyRequestInfo request3 = AcademyMgr.GetRequest(num7, client.Player.PlayerId);
				if (request3 != null && request3.Type == 1)
				{
					AcademyMgr.RemoveRequest(request3);
					if (client.Player.PlayerCharacter.freezesDate <= DateTime.Now)
					{
						if (client.Player.PlayerCharacter.apprenticeshipState < AcademyMgr.MASTER_FULL_STATE && AcademyMgr.CheckCanMaster(client.Player.PlayerCharacter.Grade))
						{
							GamePlayer playerById4 = WorldMgr.GetPlayerById(num7);
							if (playerById4 != null && AcademyMgr.CheckCanApp(playerById4.PlayerCharacter.Grade))
							{
								if (AcademyMgr.AddApprentice(client.Player, playerById4))
								{
									playerById4.Out.SendAcademySystemNotice($"Hey, [{client.Player.PlayerCharacter.NickName}]! Seni Üstadım olarak kabul ediyorum!", isAlert: true);
									client.Player.SendMailToUser(new PlayerBussiness(), $"Bir ustaya/çırağa sahip olduğunuz için tebrikler. Çırak 10, 15 ve 18. seviyeye ulaştığınızda, ilgili seviyede bir hazine sandığı alacaksınız. Sandığı içinde çok renkli çiçekler, altın paralar ve deneyim puanları gibi birçok ilginç eşya var! ", $"Akademi Bilgileri!", eMailType.ItemOverdue);
									client.Player.SendMessage($"[{playerById4.PlayerCharacter.NickName}] đa\u0303 châ\u0301p nhâ\u0323n la\u0300m đô\u0300 đê\u0323 cu\u0309a ba\u0323n");
								}
								else
								{
									client.Player.SendMessage($"Ne yazık ki her iki tarafta da bir usta veya çırak mevcut!");
								}
							}
							else
							{
								client.Player.SendMessage($"Karşı taraf sizi kabul etmezse onunla iletişime geçebilirsiniz.");
							}
						}
						else
						{
							client.Player.SendMessage(LanguageMgr.GetTranslation("Her ikiniz de uzun süredir çevrimdışısınız ve anlaşmazlık yaşıyorsunuz."));
						}
					}
					else
					{
						client.Player.SendMessage($"Daha önce çırağınızı/ustasınızı terk ettiğiniz için sınırlandırıldınız. Kalan zamanınız: {checkDate(client.Player.PlayerCharacter.freezesDate)}.");
					}
				}
				else
				{
					client.Player.SendMessage($"Akademi bilgileri eksik veya hatalı.");
				}
				break;
			}
			case 7:
			{
				int num3 = packet.ReadInt();
				AcademyRequestInfo request = AcademyMgr.GetRequest(num3, client.Player.PlayerId);
				if (request != null && request.Type == 0)
				{
					AcademyMgr.RemoveRequest(request);
					if (client.Player.PlayerCharacter.freezesDate <= DateTime.Now)
					{
						if (client.Player.PlayerCharacter.masterID == 0 && AcademyMgr.CheckCanApp(client.Player.PlayerCharacter.Grade))
						{
							GamePlayer playerById = WorldMgr.GetPlayerById(num3);
							if (playerById != null && playerById.PlayerCharacter.Grade >= client.Player.PlayerCharacter.Grade + AcademyMgr.LEVEL_GAP && AcademyMgr.CheckCanMaster(playerById.PlayerCharacter.Grade))
							{
								if (AcademyMgr.AddApprentice(playerById, client.Player))
								{
									playerById.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.ApprenticeConfirm", client.Player.PlayerCharacter.NickName), isAlert: true);
									playerById.SendMailToUser(new PlayerBussiness(), LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprenticeMail.Content"), LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprenticeMail.Title"), eMailType.ItemOverdue);
									client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.MasterConfirm", playerById.PlayerCharacter.NickName));
								}
								else
								{
									client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.AlreadlyHasRelationship.Apprentice"));
								}
							}
							else
							{
								client.Player.SendMessage(LanguageMgr.GetTranslation("LoginServerConnector.HandleSysMess.Msg2"));
							}
						}
						else
						{
							client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BeApprentice.Failed"));
						}
					}
					else
					{
						client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BeApprentice.Frozen", checkDate(client.Player.PlayerCharacter.freezesDate)));
					}
				}
				else
				{
					client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.AppClub.RemoveInfo.RecordNotFound"));
				}
				break;
			}
			case 8:
			{
				int num8 = packet.ReadInt();
				AcademyRequestInfo request4 = AcademyMgr.GetRequest(num8, client.Player.PlayerId);
				if (request4 != null && request4.Type == 1)
				{
					AcademyMgr.RemoveRequest(request4);
					WorldMgr.GetPlayerById(num8)?.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.MasterRefuse", client.Player.PlayerCharacter.NickName), isAlert: false);
				}
				break;
			}
			case 9:
			{
				int num6 = packet.ReadInt();
				AcademyRequestInfo request2 = AcademyMgr.GetRequest(num6, client.Player.PlayerId);
				if (request2 != null && request2.Type == 0)
				{
					AcademyMgr.RemoveRequest(request2);
					WorldMgr.GetPlayerById(num6)?.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.ApprenticeRefuse", client.Player.PlayerCharacter.NickName), isAlert: false);
				}
				break;
			}
			case 12:
			{
				int num2 = packet.ReadInt();
				if (client.Player.RemoveGold(10000) > 0)
				{
					if (client.Player.PlayerCharacter.masterID == num2 && AcademyMgr.FireMaster(client.Player, isComplete: false))
					{
						client.Player.PlayerCharacter.freezesDate = DateTime.Now.AddHours(GameProperties.AcademyApprenticeFreezeHours);
						using (PlayerBussiness playerBussiness2 = new PlayerBussiness())
						{
							playerBussiness2.UpdateAcademyPlayer(client.Player.PlayerCharacter);
						}
						client.Player.Out.SendAcademyAppState(client.Player.PlayerCharacter, num2);
					}
					else
					{
						client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.FireApprenticeCD"));
					}
				}
				else
				{
					client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.NotEnoughGold"));
				}
				break;
			}
			case 13:
			{
				int num = packet.ReadInt();
				if (client.Player.RemoveGold(20000) > 0)
				{
					if (client.Player.PlayerCharacter.apprenticeshipState >= AcademyMgr.MASTER_STATE && AcademyMgr.FireApprentice(client.Player, num, isSilent: false))
					{
						client.Player.PlayerCharacter.freezesDate = DateTime.Now.AddHours(GameProperties.AcademyMasterFreezeHours);
						using (PlayerBussiness playerBussiness = new PlayerBussiness())
						{
							playerBussiness.UpdateAcademyPlayer(client.Player.PlayerCharacter);
						}
						client.Player.Out.SendAcademyAppState(client.Player.PlayerCharacter, num);
					}
					else
					{
						client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.FireApprenticeCD"));
					}
				}
				else
				{
					client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.NotEnoughGold"));
				}
				break;
			}
			}
			return 0;
        }

        private int checkDate(DateTime dateTime)
        {
			if (dateTime > DateTime.Now)
			{
				return (int)Math.Ceiling((dateTime - DateTime.Now).TotalHours);
			}
			return 0;
        }
    }
}

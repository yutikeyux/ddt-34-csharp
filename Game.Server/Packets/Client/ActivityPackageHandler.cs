using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
//using Game.Server.Packets.Package;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
//using Game.Server.LotteryTicket;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.ACTIVITY_PACKAGE, "场景用户离开")]
    public class ActivityPackageHandler : IPacketHandler
    {
        private readonly int[] m_gradeList = new int[] { 10, 20, 30, 40, 45, 50, 55, 60, 65 };
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int cmd = packet.ReadInt();
            _ = client.Player.PlayerCharacter.ID;
            _ = client.Player.Actives.Info;
            switch (cmd)
            {
                #region GROWTHPACKAGE
                //case (int)GrowthPackageType.GROWTHPACKAGE:
                //    {
                //        if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
                //        {
                //            client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                //            return 0;
                //        }
                //        if (DateTime.Compare(client.Player.LastOpenGrowthPackage.AddMilliseconds(500), DateTime.Now) > 0)
                //        {
                //            client.Player.Out.SendGrowthPackageUpdate(client.Player.PlayerCharacter.ID, ActivityPackage.AvailTime);
                //            return 0;
                //        }
                //        int index = ActivityPackage.AvailTime;
                //        int needMoney = GameProperties.PromotePackagePrice;
                //        string msg = LanguageMgr.GetTranslation("UserBuyItemHandler.Success");
                //        List<ItemInfo> awards = new List<ItemInfo>();
                //        int sex = client.Player.PlayerCharacter.Sex == true ? 1 : 2;
                //        int type = packet.ReadInt();
                //        switch (type)
                //        {
                //            case (int)GrowthPackageType.GROWTHPACKAGE_OPEN:
                //                {
                //                    if (client.Player.MoneyDirect(needMoney))
                //                    {
                //                        if (CanGetAward(client.Player.PlayerCharacter.Grade, index))
                //                        {
                //                            List<ActivitySystemItemInfo> lists = ActiveMgr.GetGrowthPackage(index);
                //                            foreach (ActivitySystemItemInfo info in lists)
                //                            {
                //                                ItemTemplateInfo temp = ItemMgr.FindItemTemplate(info.TemplateID);
                //                                if (temp != null && (temp.NeedSex == 0 || temp.NeedSex == sex))
                //                                {
                //                                    ItemInfo item = ItemInfo.CreateFromTemplate(temp, info.Count, (int)eItemAddType.Buy);
                //                                    item.Count = info.Count;
                //                                    item.IsBinds = info.IsBind;
                //                                    item.ValidDate = info.ValidDate;
                //                                    item.StrengthenLevel = info.StrengthLevel;
                //                                    item.AttackCompose = info.AttackCompose;
                //                                    item.DefendCompose = info.DefendCompose;
                //                                    item.AgilityCompose = info.AgilityCompose;
                //                                    item.LuckCompose = info.LuckCompose;
                //                                    awards.Add(item);
                //                                }
                //                            }
                //                        }
                //                        else
                //                        {
                //                            client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ActivityPackageHandler.Msg1"));
                //                            return 0;
                //                        }
                //                    }
                //                    break;
                //                }
                //            case (int)GrowthPackageType.GROWTHPACKAGE_UPDATEDATA:
                //                {
                //                    if (CanGetAward(client.Player.PlayerCharacter.Grade, index))
                //                    {
                //                        msg = LanguageMgr.GetTranslation("ActivityPackageHandler.Msg3");
                //                        List<ActivitySystemItemInfo> lists = ActiveMgr.GetGrowthPackage(index);
                //                        foreach (ActivitySystemItemInfo info in lists)
                //                        {
                //                            ItemTemplateInfo temp = ItemMgr.FindItemTemplate(info.TemplateID);
                //                            if (temp != null && (temp.NeedSex == 0 || temp.NeedSex == sex))
                //                            {
                //                                ItemInfo item = ItemInfo.CreateFromTemplate(temp, info.Count, (int)eItemAddType.Buy);
                //                                item.Count = info.Count;
                //                                item.IsBinds = info.IsBind;
                //                                item.ValidDate = info.ValidDate;
                //                                item.StrengthenLevel = info.StrengthLevel;
                //                                item.AttackCompose = info.AttackCompose;
                //                                item.DefendCompose = info.DefendCompose;
                //                                item.AgilityCompose = info.AgilityCompose;
                //                                item.LuckCompose = info.LuckCompose;
                //                                awards.Add(item);
                //                            }
                //                        }
                //                    }
                //                    else
                //                    {
                //                        client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ActivityPackageHandler.Msg1"));
                //                        return 0;
                //                    }
                //                }
                //                break;
                //            default:
                //                Console.WriteLine("GrowthPackageType.{0}", (GrowthPackageType)type);
                //                break;
                //        }
                //        if (awards.Count > 0)
                //        {
                //            ActivityPackage.AvailTime++;
                //            WorldEventMgr.SendItemsToMails(awards, client.Player.PlayerCharacter.ID, client.Player.PlayerCharacter.NickName, client.Player.ZoneId, string.Format("Qùa trưởng thành cấp {0} ", m_gradeList[index]));
                //        }
                //        else
                //        {
                //            msg = LanguageMgr.GetTranslation("ActivityPackageHandler.Msg2");
                //        }
                //        client.Player.Out.SendGrowthPackageUpdate(client.Player.PlayerCharacter.ID, ActivityPackage.AvailTime);
                //        client.Out.SendMessage(eMessageType.Normal, msg);
                //        client.Player.LastOpenGrowthPackage = DateTime.Now;
                //        break;
                //    }
                #endregion
                case (int)ChickActivationType.CHICKACTIVATION:
                    {
                        UserChickActiveInfo chickInfo = client.Player.Actives.GetChickActiveData();
                        int[] gradeVal = { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 };
                        int[] levelNeed = { 5, 10, 25, 30, 40, 45, 48, 50, 55, 60 };
                        int type = packet.ReadInt();
                        GSPacketIn pkg = new((byte)ePackageType.ACTIVITY_PACKAGE, client.Player.PlayerCharacter.ID);
                        pkg.WriteInt((int)ChickActivationType.CHICKACTIVATION);
                        switch (type)
                        {
                            case (int)ChickActivationType.CHICKACTIVATION_QUERY:
                                {
                                    if (chickInfo != null)
                                    {
                                        pkg.WriteInt((int)ChickActivationType.CHICKACTIVATION_LOGIN);
                                        pkg.WriteInt(chickInfo.IsKeyOpened);
                                        pkg.WriteInt(1);
                                        pkg.WriteDateTime(chickInfo.KeyOpenedTime);
                                        pkg.WriteInt(chickInfo.KeyOpenedType);

                                        pkg.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Monday) ? 0 : 1);
                                        pkg.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) ? 0 : 1);
                                        pkg.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Wednesday) ? 0 : 1);
                                        pkg.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Thursday) ? 0 : 1);
                                        pkg.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Friday) ? 0 : 1);
                                        pkg.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Saturday) ? 0 : 1);
                                        pkg.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Sunday) ? 0 : 1);
                                        pkg.WriteInt((chickInfo.AfterThreeDays.Day < DateTime.Now.Day && chickInfo.OnThreeDay(DateTime.Now)) ? 0 : 1);
                                        pkg.WriteInt((chickInfo.AfterThreeDays.Day < DateTime.Now.Day && chickInfo.OnThreeDay(DateTime.Now)) ? 0 : 1);
                                        pkg.WriteInt((chickInfo.AfterThreeDays.Day < DateTime.Now.Day && chickInfo.OnThreeDay(DateTime.Now)) ? 0 : 1);
                                        pkg.WriteInt((chickInfo.Weekly < chickInfo.StartOfWeek(DateTime.Now, DayOfWeek.Saturday) && DateTime.Now.DayOfWeek == DayOfWeek.Saturday) ? 0 : 1);
                                        pkg.WriteInt(chickInfo.CurrentLvAward);
                                        client.Player.SendTCP(pkg);
                                    }
                                }
                                break;
                            case (int)ChickActivationType.CHICKACTIVATION_OPENKEY:
                                string codeEnter = packet.ReadString();
                                if (codeEnter.Length == 14 && chickInfo.IsKeyOpened == 0)
                                {
                                    // check code
                                    using PlayerBussiness pb = new();
                                    int result = pb.ActiveChickCode(client.Player.PlayerCharacter.ID, codeEnter);
                                    switch (result)
                                    {
                                        case 0:
                                            // complete
                                            chickInfo.Active((client.Player.PlayerCharacter.Grade > 15) ? 2 : 1);
                                            _ = client.Player.Actives.SaveChickActiveData(chickInfo);
                                            client.Player.Actives.SendUpdateChickActivation();
                                            client.Player.SendMessage(LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.Success"));
                                            break;
                                        case 1:
                                            client.Player.SendMessage(LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.NotExit"));
                                            break;
                                        case 2:
                                            client.Player.SendMessage(LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.IsUsed"));
                                            break;
                                    }
                                }
                                break;
                            case (int)ChickActivationType.CHICKACTIVATION_GETAWARD:
                                int AwardType = packet.ReadInt();
                                int AwardIndex = packet.ReadInt();
                                if (chickInfo.IsKeyOpened == 1 && chickInfo.KeyOpenedTime.AddDays(60).Date >= DateTime.Now.Date)//DateTime
                                {
                                    List<ActivitySystemItemInfo> lists = null;
                                    int mySex = client.Player.PlayerCharacter.Sex == true ? 1 : 2;
                                    string title = "";
                                    string msg = "";
                                    if (AwardType <= 7 && chickInfo.EveryDay.Day < DateTime.Now.Day)//EveryDay
                                    {
                                        chickInfo.EveryDay = DateTime.Now;
                                        lists = ActiveMgr.FindChickActivePakage((chickInfo.KeyOpenedType == 2) ? 101 : 1);
                                        title = LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.DaylyAward.Title");
                                        msg = LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.DaylyAward.Msg");
                                    }
                                    else if (AwardType <= 10 && chickInfo.AfterThreeDays.Day < DateTime.Now.Day && chickInfo.OnThreeDay(DateTime.Now))//AfterThreeDays
                                    {
                                        chickInfo.AfterThreeDays = DateTime.Now;
                                        lists = ActiveMgr.FindChickActivePakage((chickInfo.KeyOpenedType == 2) ? 102 : 2);
                                        title = LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.WeekenAward.Title");
                                        msg = LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.WeekenAward.Msg");
                                    }
                                    else if (AwardType == 11 && chickInfo.Weekly < chickInfo.StartOfWeek(DateTime.Now, DayOfWeek.Saturday) && DateTime.Now.DayOfWeek == DayOfWeek.Saturday)//Weekly
                                    {
                                        chickInfo.Weekly = chickInfo.StartOfWeek(DateTime.Now, DayOfWeek.Saturday);
                                        lists = ActiveMgr.FindChickActivePakage((chickInfo.KeyOpenedType == 2) ? 103 : 3);
                                        title = LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.WeeklyAward.Title");
                                        msg = LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.WeeklyAward.Msg");
                                    }
                                    else if (AwardType == 12 && gradeVal[AwardIndex - 1] > chickInfo.CurrentLvAward && client.Player.PlayerCharacter.Grade >= levelNeed[AwardIndex - 1] && chickInfo.KeyOpenedType == 1)
                                    {
                                        int[] qualityArr = { 10001, 10002, 10003, 10004, 10005, 10006, 10007, 10000, 10009, 10010 };
                                        chickInfo.CurrentLvAward += gradeVal[AwardIndex - 1];
                                        lists = ActiveMgr.FindChickActivePakage(qualityArr[AwardIndex - 1]);
                                        title = LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.LevelAward.Title", levelNeed[AwardIndex - 1]);
                                        msg = LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.LevelAward.Msg", levelNeed[AwardIndex - 1]);
                                    }

                                    // send item
                                    if (lists != null)
                                    {
                                        List<ItemInfo> awards = [];
                                        foreach (ActivitySystemItemInfo info in lists)
                                        {
                                            ItemTemplateInfo temp = ItemMgr.FindItemTemplate(info.TemplateID);
                                            if (temp != null && (temp.NeedSex == 0 || temp.NeedSex == mySex))
                                            {
                                                ItemInfo item = ItemInfo.CreateFromTemplate(temp, info.Count, (int)eItemAddType.Buy);
                                                item.Count = info.Count;
                                                item.IsBinds = info.IsBind;
                                                item.ValidDate = info.ValidDate;
                                                item.StrengthenLevel = info.StrengthLevel;
                                                item.AttackCompose = info.AttackCompose;
                                                item.DefendCompose = info.DefendCompose;
                                                item.AgilityCompose = info.AgilityCompose;
                                                item.LuckCompose = info.LuckCompose;
                                                awards.Add(item);
                                            }
                                        }
                                        if (awards.Count > 0)
                                        {
                                            _ = WorldEventMgr.SendItemsToMails(awards, client.Player.PlayerCharacter.ID, client.Player.PlayerCharacter.NickName, client.Player.ZoneId, null, title);
                                            _ = client.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
                                        }
                                        _ = client.Player.Actives.SaveChickActiveData(chickInfo);
                                        client.Player.Actives.SendUpdateChickActivation();
                                        _ = client.Out.SendMessage(eMessageType.Normal, msg);

                                    }
                                    else
                                    {
                                        client.Player.SendMessage(LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.NotFoundAward"));
                                    }
                                }
                                else
                                {
                                    client.Player.SendMessage(LanguageMgr.GetTranslation("ActivityPackageHandler.ChickActivation.NotActive"));
                                }
                                break;
                            default:
                                Console.WriteLine("ChickActivationType.{0}", (ChickActivationType)type);
                                break;
                        }
                    }
                    break;
                default:
                    Console.WriteLine("ACTIVITY_PACKAGE not found Cmd {0}", cmd);
                    break;
            }
            return 0;
        }
    }
}

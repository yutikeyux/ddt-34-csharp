using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.ActiveSystem.Handle
{
    [ActiveSystemHandleAttbute((byte)ActiveSystemPackageType.PYRAMID_RESULT)]
    public class PyramidResult : IActiveSystemCommandHadler
    {
        public static Random random = new Random();

        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ACTIVITY_SYSTEM, Player.PlayerCharacter.ID);
            PyramidInfo pyramid = Player.Actives.Pyramid;
            if (pyramid == null)
            {
                Player.Actives.LoadPyramid();
                pyramid = Player.Actives.Pyramid;
            }

            int layer = packet.ReadInt(); //_loc_2.writeInt();
            int place = packet.ReadInt(); //_loc_2.writeInt(); 
            if (pyramid.currentFreeCount < Player.Actives.PyramidConfig.freeCount)
            {
                pyramid.currentFreeCount++;
            }
            else
            {
                int turnCardPrice = Player.Actives.PyramidConfig.turnCardPrice;
                if (!Player.MoneyDirect(turnCardPrice))
                {
                    return false;
                }
            }

            bool isPyramidDie = false;
            bool isUp = false;
            string msg = "";
            bool canAddToBag = true;
            if (layer < 8)
            {
                List<ItemInfo> infos = ActiveSystemMgr.GetPyramidAward(layer);
                int index = random.Next(infos.Count);
                ItemInfo award = infos[index];
                int templateId = award.TemplateID;
                isPyramidDie = templateId == 201083;
                isUp = templateId == 201082;
                msg = LanguageMgr.GetTranslation("ActiveSystemHandler.Msg11", award.Template.Name, award.Count);
                if (isUp)
                {
                    msg = LanguageMgr.GetTranslation("ActiveSystemHandler.Msg12");
                    pyramid.currentLayer++;
                    if (pyramid.currentLayer > pyramid.maxLayer)
                    {
                        pyramid.maxLayer++;
                    }

                    canAddToBag = false;
                }

                pyramid.totalPoint += 10;
                switch (templateId)
                {
                    case 201079:
                        pyramid.turnPoint += 10;
                        msg = LanguageMgr.GetTranslation("ActiveSystemHandler.Msg13");
                        canAddToBag = false;
                        break;
                    case 201080:
                        pyramid.turnPoint += 30;
                        msg = LanguageMgr.GetTranslation("ActiveSystemHandler.Msg14");
                        canAddToBag = false;
                        break;
                    case 201081:
                        pyramid.turnPoint += 50;
                        msg = LanguageMgr.GetTranslation("ActiveSystemHandler.Msg15");
                        canAddToBag = false;
                        break;
                    case 201077:
                        pyramid.pointRatio += 5;
                        msg = LanguageMgr.GetTranslation("ActiveSystemHandler.Msg16");
                        canAddToBag = false;
                        break;
                    case 201078:
                        pyramid.pointRatio += 10;
                        msg = LanguageMgr.GetTranslation("ActiveSystemHandler.Msg17");
                        canAddToBag = false;
                        break;
                }

                if (canAddToBag)
                {
                    Player.AddTemplate(award, LanguageMgr.GetTranslation("ActiveSystemHandler.Msg18"));
                }

                string layerItems = string.Format("{0}-{1}-{2}", layer, templateId, place);
                if (pyramid.LayerItems == "")
                {
                    pyramid.LayerItems = layerItems;
                }
                else
                {
                    pyramid.LayerItems += "|" + layerItems;
                }

                pkg.WriteByte((byte)ActiveSystemPackageType.PYRAMID_RESULT);
                pkg.WriteInt(templateId); //model.templateID = param1.readInt();
                pkg.WriteInt(place); //model.position = param1.readInt();
                pkg.WriteBoolean(isPyramidDie); //model.isPyramidDie = param1.readBoolean();
                pkg.WriteBoolean(isUp); //model.isUp = param1.readBoolean();
                pkg.WriteInt(pyramid.currentLayer); //model.currentLayer = param1.readInt();
                pkg.WriteInt(pyramid.maxLayer); //model.maxLayer = param1.readInt();
                pkg.WriteInt(pyramid.totalPoint); //model.totalPoint = param1.readInt();
                pkg.WriteInt(pyramid.turnPoint); //model.turnPoint = param1.readInt();
                pkg.WriteInt(pyramid.pointRatio); //model.pointRatio = param1.readInt();
                pkg.WriteInt(pyramid.currentFreeCount); //model.currentFreeCount = param1.readInt();
                Player.SendTCP(pkg);
            }
            else
            {
                int point = random.Next(49, 501);
                pyramid.turnPoint += point;
                pyramid.turnPoint += (pyramid.turnPoint * pyramid.pointRatio) / 100;
                msg = LanguageMgr.GetTranslation("ActiveSystemHandler.Msg19", point);
                pyramid.isPyramidStart = false;
                pyramid.currentLayer = 1;
                pyramid.currentReviveCount = 0;
                pyramid.totalPoint += pyramid.turnPoint;
                pyramid.turnPoint = 0;
                pyramid.pointRatio = 0;

                pkg.WriteByte((byte)ActiveSystemPackageType.PYRAMID_STARTORSTOP);
                pkg.WriteBoolean(pyramid
                    .isPyramidStart); //this.model.isPyramidStart = param1.readBoolean();                            
                pkg.WriteInt(pyramid.totalPoint); //model.totalPoint = param1.readInt();
                pkg.WriteInt(pyramid.turnPoint); //model.turnPoint = param1.readInt();
                pkg.WriteInt(pyramid.pointRatio); //model.pointRatio = param1.readInt();
                pkg.WriteInt(pyramid.currentLayer); //model.currentLayer = param1.readInt();
                Player.SendTCP(pkg);
            }

            Player.SendMessage(msg);
            return true;
        }
    }
}
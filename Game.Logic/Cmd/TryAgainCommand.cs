using Bussiness;
using Game.Base.Packets;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using System;

namespace Game.Logic.Cmd
{
    [GameCommand((byte)eTankCmdType.GAME_MISSION_TRY_AGAIN, "etap uzatma")]
    public class TryAgainCommand : ICommandHandler
    {
        public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
        {
            if (game is PVEGame)
            {
                PVEGame pve = game as PVEGame;
                var MissionAgain = packet.ReadInt();
                bool tryAgain = packet.ReadBoolean();
                bool isHost = packet.ReadBoolean();

                if (isHost)
                {
                    if (tryAgain)
                    {
                        if (MissionAgain == 1)
                        {
                            if (player.PlayerDetail.PlayerCharacter.HasBagPassword && player.PlayerDetail.PlayerCharacter.IsLocked)
                            {
                                player.PlayerDetail.SendMessage("Çanta kilitli olduğundan, ek ücret alınamaz!");
                                pve.WantTryAgain = 0;
                            }
                            else
                            {
                                bool canOpen = false;
                                bool check = false;
                                BufferInfo buffInfo = player.GetFightBuffByType(BuffType.Level_Try);
                                if (buffInfo != null && !game.IsSpecialPVE() && player.PlayerDetail.UsePayBuff(BuffType.Level_Try))
                                {
                                    string msg1 = string.Format("Milanda Dileği hakkınız mevcut");
                                    player.PlayerDetail.SendMessage(msg1);
                                    canOpen = true;
                                    check = true;
                                }
                                else
                                {
                                    canOpen = player.PlayerDetail.RemoveMoney(500) > 0 ? true : false;
                                }

                                if (canOpen)
                                {
                                    if (pve.WantTryAgain == 2)
                                    {
                                        //退回关卡结算
                                        pve.WantTryAgain = 1;
                                        string msg = string.Format("Uzatma Başarılı!");
                                        string msg1 = "";

                                        if (check)
                                        {
                                            msg1 = string.Format(", Milanda Dileği etap uzatma hediyesi!");

                                        }
                                        //pve.WantTryAgain = true;
                                        player.PlayerDetail.SendMessage(msg1);
                                    }
                                }
                                else
                                {
                                    player.PlayerDetail.SendInsufficientMoney((int)eBattleRemoveMoneyType.TryAgain);
                                }
                            }
                        }
                        else
                        {
                            //退回房间
                            pve.WantTryAgain = 0;
                        }
                    }
                    else
                    {


                        //退回房间
                        pve.WantTryAgain = 0;
                    }
                    pve.SendMissionTryAgain();

                    pve.ClearWaitTimer();

                    pve.CheckState(0);

                }
            }
        }
    }
}
            //if (game is PVEGame)
            //{
             //   PVEGame pve = game as PVEGame;
              //  int tryAgain = packet.ReadInt();
             //   bool isHost = packet.ReadBoolean();
             //   pve.WantTryAgain = 0;
              //  pve.StopTimeOut();
            // game.SendToAll(packet);
            //   game.Stop();
             //   pve.SendMissionTryAgain();
            //}
            //if (!(game is PVEGame))
            //{

            //	return;
            //}
            //PVEGame pveGame = game as PVEGame;
            //int tryAgain = packet.ReadInt();
            //if (!packet.ReadBoolean())
            //{
            //	pveGame.WantTryAgain = 0;
            //	pveGame.StopTimeOut();
            //	game.SendToAll(packet);
            //	game.Stop();
            //	return;
            //}
            //if (tryAgain == 1)
            //{
            //             // baolt tắt tryagain - start
            //             player.PlayerDetail.SendMessage("Tính năng đang phát triển chưa sử dụng được, vui lòng đợi thông báo từ BQT.");
            //             pveGame.WantTryAgain = 0;
            //             pveGame.StopTimeOut();
            //             game.SendToAll(packet);
            //             game.Stop();
            //             pveGame.SendMissionTryAgain();
            //             return;
            //             // baolt tắt tryagain - end

            //             bool flag = false;
            //             if (player.GetFightBuffByType(BuffType.Level_Try) != null && player.PlayerDetail.UsePayBuff(BuffType.Level_Try))
            //             {
            //                 player.PlayerDetail.SendMessage("Kích hoạt thẻ vượt ải của chúc phúc thần gà, lần này bạn không bị trừ xu");
            //                 flag = true;
            //             }
            //             if (!flag)
            //             {
            //                 flag = player.PlayerDetail.RemoveMoney(pveGame.MissionInfo.TryAgainCost) > 0;
            //             }
            //             if (flag)
            //             {
            //                 pveGame.WantTryAgain = 1;
            //                 game.SendToAll(packet);
            //                 player.PlayerDetail.LogAddMoney(AddMoneyType.Game, AddMoneyType.Game_TryAgain, player.PlayerDetail.PlayerCharacter.ID, 100, player.PlayerDetail.PlayerCharacter.Money);
            //                 Console.WriteLine("Continue");
            //                 pveGame.ResetForTry();
            //                 pveGame.SessionId--;
            //                 //pveGame.PrepareNewSession();
            //             }
            //             else
            //             {
            //                 player.PlayerDetail.SendInsufficientMoney(2);
            //                 Console.WriteLine("GiveUp");
            //                 pveGame.WantTryAgain = 0;
            //                 game.SendToAll(packet);
            //                 game.Stop();
            //             }
            //         }
            //else if (tryAgain == 0)
            //{
            //	//Console.WriteLine("GiveUp");
            //	pveGame.WantTryAgain = 0;
            //	game.SendToAll(packet);
            //	game.Stop();
            //}
            //else
            //         {
            //	//Console.WriteLine("TimeOut");
            //	pveGame.WantTryAgain = 0;
            //	pveGame.StopTimeOut();
            //	game.SendToAll(packet);
            //	game.Stop();
            //         }
            //pveGame.SendMissionTryAgain();
  //      }
   // }
//}

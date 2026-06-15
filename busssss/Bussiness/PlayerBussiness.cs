using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Bussiness.CenterService;
using Bussiness.Managers;
using SqlDataProvider.Data;

namespace Bussiness
{
	// Token: 0x02000017 RID: 23
	public class PlayerBussiness : BaseBussiness
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x0000E5AC File Offset: 0x0000C7AC
		public bool ActivePlayer(ref PlayerInfo player, string userName, string passWord, bool sex, int gold, int money, string IP, string site)
		{
			bool flag = false;
			bool result;
			try
			{
				player = new PlayerInfo();
				player.Agility = 0;
				player.Attack = 0;
				player.Colors = ",,,,,,";
				player.Skin = "";
				player.ConsortiaID = 0;
				player.Defence = 0;
				player.Gold = gold;
				player.GP = 1;
				player.Grade = 1;
				player.ID = 0;
				player.Luck = 0;
				player.Money = money;
				player.NickName = "";
				player.Sex = sex;
				player.State = 0;
				player.Style = ",,,,,,";
				player.Hide = 1111111111;
				SqlParameter[] sqlParameters = new SqlParameter[21];
				sqlParameters[0] = new SqlParameter("@UserID", SqlDbType.Int);
				sqlParameters[0].Direction = ParameterDirection.Output;
				sqlParameters[1] = new SqlParameter("@Attack", player.Attack);
				sqlParameters[2] = new SqlParameter("@Colors", (player.Colors == null) ? "" : player.Colors);
				sqlParameters[3] = new SqlParameter("@ConsortiaID", player.ConsortiaID);
				sqlParameters[4] = new SqlParameter("@Defence", player.Defence);
				sqlParameters[5] = new SqlParameter("@Gold", player.Gold);
				sqlParameters[6] = new SqlParameter("@GP", player.GP);
				sqlParameters[7] = new SqlParameter("@Grade", player.Grade);
				sqlParameters[8] = new SqlParameter("@Luck", player.Luck);
				sqlParameters[9] = new SqlParameter("@Money", player.Money);
				sqlParameters[10] = new SqlParameter("@Style", (player.Style == null) ? "" : player.Style);
				sqlParameters[11] = new SqlParameter("@Agility", player.Agility);
				sqlParameters[12] = new SqlParameter("@State", player.State);
				sqlParameters[13] = new SqlParameter("@UserName", userName);
				sqlParameters[14] = new SqlParameter("@PassWord", passWord);
				sqlParameters[15] = new SqlParameter("@Sex", sex);
				sqlParameters[16] = new SqlParameter("@Hide", player.Hide);
				sqlParameters[17] = new SqlParameter("@ActiveIP", IP);
				sqlParameters[18] = new SqlParameter("@Skin", (player.Skin == null) ? "" : player.Skin);
				sqlParameters[19] = new SqlParameter("@Result", SqlDbType.Int);
				sqlParameters[19].Direction = ParameterDirection.ReturnValue;
				sqlParameters[20] = new SqlParameter("@Site", site);
				flag = this.db.RunProcedure("SP_Users_Active", sqlParameters);
				player.ID = (int)sqlParameters[0].Value;
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000E8FC File Offset: 0x0000CAFC
		public bool DeleteQuestUser(int UserID, int QuestID)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserID),
					new SqlParameter("@QuestID", QuestID)
				};
				result = this.db.RunProcedure("SP_Users_Quest_Delete", para);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			return result;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000E98C File Offset: 0x0000CB8C
		public bool AddAuction(AuctionInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[18];
				array[0] = new SqlParameter("@AuctionID", info.AuctionID);
				SqlParameter[] sqlParameters = array;
				sqlParameters[0].Direction = ParameterDirection.Output;
				sqlParameters[1] = new SqlParameter("@AuctioneerID", info.AuctioneerID);
				sqlParameters[2] = new SqlParameter("@AuctioneerName", (info.AuctioneerName == null) ? "" : info.AuctioneerName);
				sqlParameters[3] = new SqlParameter("@BeginDate", info.BeginDate);
				sqlParameters[4] = new SqlParameter("@BuyerID", info.BuyerID);
				sqlParameters[5] = new SqlParameter("@BuyerName", (info.BuyerName == null) ? "" : info.BuyerName);
				sqlParameters[6] = new SqlParameter("@IsExist", info.IsExist);
				sqlParameters[7] = new SqlParameter("@ItemID", info.ItemID);
				sqlParameters[8] = new SqlParameter("@Mouthful", info.Mouthful);
				sqlParameters[9] = new SqlParameter("@PayType", info.PayType);
				sqlParameters[10] = new SqlParameter("@Price", info.Price);
				sqlParameters[11] = new SqlParameter("@Rise", info.Rise);
				sqlParameters[12] = new SqlParameter("@ValidDate", info.ValidDate);
				sqlParameters[13] = new SqlParameter("@TemplateID", info.TemplateID);
				sqlParameters[14] = new SqlParameter("Name", info.Name);
				sqlParameters[15] = new SqlParameter("Category", info.Category);
				sqlParameters[16] = new SqlParameter("Random", info.Random);
				sqlParameters[17] = new SqlParameter("goodsCount", info.goodsCount);
				flag = this.db.RunProcedure("SP_Auction_Add", sqlParameters);
				info.AuctionID = (int)sqlParameters[0].Value;
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000EBEC File Offset: 0x0000CDEC
		public DiceDataInfo GetSingleDiceData(int UserID)
		{
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				para[0].Value = UserID;
				this.db.GetReader(ref reader, "SP_GetSingle_DiceData", para);
				bool flag = reader.Read();
				if (flag)
				{
					return new DiceDataInfo
					{
						ID = (int)reader["ID"],
						UserID = (int)reader["UserID"],
						LuckIntegral = (int)reader["LuckIntegral"],
						LuckIntegralLevel = (int)reader["LuckIntegralLevel"],
						Level = (int)reader["Level"],
						FreeCount = (int)reader["FreeCount"],
						CurrentPosition = (int)reader["CurrentPosition"],
						UserFirstCell = (bool)reader["UserFirstCell"],
						AwardArray = ((reader["AwardArray"] == null) ? "" : reader["AwardArray"].ToString())
					};
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_GetSingleDiceData", e);
				}
			}
			finally
			{
				bool flag2 = reader != null && !reader.IsClosed;
				if (flag2)
				{
					reader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
		public bool AddDiceData(DiceDataInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[10];
				para[0] = new SqlParameter("@ID", info.ID);
				para[0].Direction = ParameterDirection.Output;
				para[1] = new SqlParameter("@UserID", info.UserID);
				para[2] = new SqlParameter("@LuckIntegral", info.LuckIntegral);
				para[3] = new SqlParameter("@LuckIntegralLevel", info.LuckIntegralLevel);
				para[4] = new SqlParameter("@Level", info.Level);
				para[5] = new SqlParameter("@FreeCount", info.FreeCount);
				para[6] = new SqlParameter("@CurrentPosition", info.CurrentPosition);
				para[7] = new SqlParameter("@UserFirstCell", info.UserFirstCell);
				para[8] = new SqlParameter("@AwardArray", info.AwardArray);
				para[9] = new SqlParameter("@Result", SqlDbType.Int);
				para[9].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_DiceData_Add", para);
				result = ((int)para[9].Value == 0);
				info.ID = (int)para[0].Value;
				info.IsDirty = false;
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_DiceData_Add", e);
				}
			}
			return result;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000EF54 File Offset: 0x0000D154
		public bool UpdateDiceData(DiceDataInfo info)
		{
			bool flag = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@LuckIntegral", info.LuckIntegral),
					new SqlParameter("@LuckIntegralLevel", info.LuckIntegralLevel),
					new SqlParameter("@Level", info.Level),
					new SqlParameter("@FreeCount", info.FreeCount),
					new SqlParameter("@CurrentPosition", info.CurrentPosition),
					new SqlParameter("@UserFirstCell", info.UserFirstCell),
					new SqlParameter("@AwardArray", info.AwardArray),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[9].Direction = ParameterDirection.ReturnValue;
				bool v = this.db.RunProcedure("SP_Update_DiceData", para);
				flag = ((int)para[9].Value == 0);
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Update_DiceData", exception);
				}
			}
			return flag;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000F0CC File Offset: 0x0000D2CC
		public bool AddCards(UsersCardInfo item)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[19];
				array[0] = new SqlParameter("@CardID", item.CardID);
				SqlParameter[] SqlParameters = array;
				SqlParameters[0].Direction = ParameterDirection.Output;
				SqlParameters[1] = new SqlParameter("@UserID", item.UserID);
				SqlParameters[2] = new SqlParameter("@TemplateID", item.TemplateID);
				SqlParameters[3] = new SqlParameter("@Place", item.Place);
				SqlParameters[4] = new SqlParameter("@Count", item.Count);
				SqlParameters[5] = new SqlParameter("@Attack", item.Attack);
				SqlParameters[6] = new SqlParameter("@Defence", item.Defence);
				SqlParameters[7] = new SqlParameter("@Agility", item.Agility);
				SqlParameters[8] = new SqlParameter("@Luck", item.Luck);
				SqlParameters[9] = new SqlParameter("@Guard", item.Guard);
				SqlParameters[10] = new SqlParameter("@Damage", item.Damage);
				SqlParameters[11] = new SqlParameter("@Level", item.Level);
				SqlParameters[12] = new SqlParameter("@CardGP", item.CardGP);
				SqlParameters[14] = new SqlParameter("@isFirstGet", item.isFirstGet);
				SqlParameters[15] = new SqlParameter("@AttackReset", item.AttackReset);
				SqlParameters[16] = new SqlParameter("@DefenceReset", item.DefenceReset);
				SqlParameters[17] = new SqlParameter("@AgilityReset", item.AgilityReset);
				SqlParameters[18] = new SqlParameter("@LuckReset", item.LuckReset);
				SqlParameters[13] = new SqlParameter("@Result", SqlDbType.Int);
				SqlParameters[13].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UserCard_Add", SqlParameters);
				flag = ((int)SqlParameters[13].Value == 0);
				item.CardID = (int)SqlParameters[0].Value;
				item.IsDirty = false;
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000F350 File Offset: 0x0000D550
		public bool AddChargeMoney(string chargeID, string userName, int money, string payWay, decimal needMoney, ref int userID, ref int isResult, DateTime date, string IP, string nickName)
		{
			bool flag = false;
			userID = 0;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[10];
				array[0] = new SqlParameter("@ChargeID", chargeID);
				array[1] = new SqlParameter("@UserName", userName);
				array[2] = new SqlParameter("@Money", money);
				array[3] = new SqlParameter("@Date", date.ToString("yyyy-MM-dd HH:mm:ss"));
				array[4] = new SqlParameter("@PayWay", payWay);
				array[5] = new SqlParameter("@NeedMoney", needMoney);
				array[6] = new SqlParameter("@UserID", userID);
				SqlParameter[] sqlParameters = array;
				sqlParameters[6].Direction = ParameterDirection.InputOutput;
				sqlParameters[7] = new SqlParameter("@Result", SqlDbType.Int);
				sqlParameters[7].Direction = ParameterDirection.ReturnValue;
				sqlParameters[8] = new SqlParameter("@IP", IP);
				sqlParameters[9] = new SqlParameter("@NickName", nickName);
				flag = this.db.RunProcedure("SP_Charge_Money_Add", sqlParameters);
				userID = (int)sqlParameters[6].Value;
				isResult = (int)sqlParameters[7].Value;
				flag = (isResult == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000F4B4 File Offset: 0x0000D6B4
		public bool AddChargeMoney(string chargeID, string userName, int money, string payWay, decimal needMoney, ref int userID, ref int isResult, DateTime date, string IP, int UserID)
		{
			bool flag = false;
			userID = 0;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[10];
				array[0] = new SqlParameter("@ChargeID", chargeID);
				array[1] = new SqlParameter("@UserName", userName);
				array[2] = new SqlParameter("@Money", money);
				array[3] = new SqlParameter("@Date", date.ToString("yyyy-MM-dd HH:mm:ss"));
				array[4] = new SqlParameter("@PayWay", payWay);
				array[5] = new SqlParameter("@NeedMoney", needMoney);
				array[6] = new SqlParameter("@UserID", userID);
				SqlParameter[] sqlParameters = array;
				sqlParameters[6].Direction = ParameterDirection.InputOutput;
				sqlParameters[7] = new SqlParameter("@Result", SqlDbType.Int);
				sqlParameters[7].Direction = ParameterDirection.ReturnValue;
				sqlParameters[8] = new SqlParameter("@IP", IP);
				sqlParameters[9] = new SqlParameter("@SourceUserID", UserID);
				flag = this.db.RunProcedure("SP_Charge_Money_UserId_Add", sqlParameters);
				userID = (int)sqlParameters[6].Value;
				isResult = (int)sqlParameters[7].Value;
				flag = (isResult == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000F620 File Offset: 0x0000D820
		public bool AddFriends(FriendInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@AddDate", DateTime.Now),
					new SqlParameter("@FriendID", info.FriendID),
					new SqlParameter("@IsExist", true),
					new SqlParameter("@Remark", (info.Remark == null) ? "" : info.Remark),
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@Relation", info.Relation)
				};
				flag = this.db.RunProcedure("SP_Users_Friends_Add", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000F738 File Offset: 0x0000D938
		public bool AddGoods(ItemInfo item)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[41];
				para[0] = new SqlParameter("@ItemID", item.ItemID);
				para[0].Direction = ParameterDirection.Output;
				para[1] = new SqlParameter("@UserID", item.UserID);
				para[2] = new SqlParameter("@TemplateID", item.Template.TemplateID);
				para[3] = new SqlParameter("@Place", item.Place);
				para[4] = new SqlParameter("@AgilityCompose", item.AgilityCompose);
				para[5] = new SqlParameter("@AttackCompose", item.AttackCompose);
				para[6] = new SqlParameter("@BeginDate", item.BeginDate);
				para[7] = new SqlParameter("@Color", (item.Color == null) ? "" : item.Color);
				para[8] = new SqlParameter("@Count", item.Count);
				para[9] = new SqlParameter("@DefendCompose", item.DefendCompose);
				para[10] = new SqlParameter("@IsBinds", item.IsBinds);
				para[11] = new SqlParameter("@IsExist", item.IsExist);
				para[12] = new SqlParameter("@IsJudge", item.IsJudge);
				para[13] = new SqlParameter("@LuckCompose", item.LuckCompose);
				para[14] = new SqlParameter("@StrengthenLevel", item.StrengthenLevel);
				para[15] = new SqlParameter("@ValidDate", item.ValidDate);
				para[16] = new SqlParameter("@BagType", item.BagType);
				para[17] = new SqlParameter("@Skin", (item.Skin == null) ? "" : item.Skin);
				para[18] = new SqlParameter("@IsUsed", item.IsUsed);
				para[19] = new SqlParameter("@RemoveType", item.RemoveType);
				para[20] = new SqlParameter("@Hole1", item.Hole1);
				para[21] = new SqlParameter("@Hole2", item.Hole2);
				para[22] = new SqlParameter("@Hole3", item.Hole3);
				para[23] = new SqlParameter("@Hole4", item.Hole4);
				para[24] = new SqlParameter("@Hole5", item.Hole5);
				para[25] = new SqlParameter("@Hole6", item.Hole6);
				para[26] = new SqlParameter("@StrengthenTimes", item.StrengthenTimes);
				para[27] = new SqlParameter("@Hole5Level", item.Hole5Level);
				para[28] = new SqlParameter("@Hole5Exp", item.Hole5Exp);
				para[29] = new SqlParameter("@Hole6Level", item.Hole6Level);
				para[30] = new SqlParameter("@Hole6Exp", item.Hole6Exp);
				para[31] = new SqlParameter("@IsGold", item.IsGold);
				para[32] = new SqlParameter("@goldValidDate", item.goldValidDate);
				para[33] = new SqlParameter("@goldBeginTime", item.goldBeginTime);
				para[34] = new SqlParameter("@StrengthenExp", item.StrengthenExp);
				para[35] = new SqlParameter("@Blood", item.Blood);
				para[36] = new SqlParameter("@latentEnergyCurStr", item.latentEnergyCurStr);
				para[37] = new SqlParameter("@latentEnergyNewStr", item.latentEnergyNewStr);
				para[38] = new SqlParameter("@latentEnergyEndTime", item.latentEnergyEndTime);
				para[39] = new SqlParameter("@curExp", item.curExp);
				para[40] = new SqlParameter("@cellLocked", item.cellLocked);
				result = this.db.RunProcedure("SP_Users_Items_Add", para);
				item.ItemID = (int)para[0].Value;
				item.IsDirty = false;
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init item.goldBeginTime: " + item.goldBeginTime.ToString() + " item.goldValidDate: " + item.goldValidDate.ToString(), e);
				}
			}
			return result;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000FC04 File Offset: 0x0000DE04
		public bool AddMarryInfo(MarryInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[5];
				array[0] = new SqlParameter("@ID", info.ID);
				SqlParameter[] sqlParameters = array;
				sqlParameters[0].Direction = ParameterDirection.Output;
				sqlParameters[1] = new SqlParameter("@UserID", info.UserID);
				sqlParameters[2] = new SqlParameter("@IsPublishEquip", info.IsPublishEquip);
				sqlParameters[3] = new SqlParameter("@Introduction", info.Introduction);
				sqlParameters[4] = new SqlParameter("@RegistTime", info.RegistTime);
				flag = this.db.RunProcedure("SP_MarryInfo_Add", sqlParameters);
				info.ID = (int)sqlParameters[0].Value;
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("AddMarryInfo", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000FD00 File Offset: 0x0000DF00
		public bool AddStore(ItemInfo item)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[14];
				array[0] = new SqlParameter("@ItemID", item.ItemID);
				SqlParameter[] sqlParameters = array;
				sqlParameters[0].Direction = ParameterDirection.Output;
				sqlParameters[1] = new SqlParameter("@UserID", item.UserID);
				sqlParameters[2] = new SqlParameter("@TemplateID", item.Template.TemplateID);
				sqlParameters[3] = new SqlParameter("@Place", item.Place);
				sqlParameters[4] = new SqlParameter("@AgilityCompose", item.AgilityCompose);
				sqlParameters[5] = new SqlParameter("@AttackCompose", item.AttackCompose);
				sqlParameters[6] = new SqlParameter("@BeginDate", item.BeginDate);
				sqlParameters[7] = new SqlParameter("@Color", (item.Color == null) ? "" : item.Color);
				sqlParameters[8] = new SqlParameter("@Count", item.Count);
				sqlParameters[9] = new SqlParameter("@DefendCompose", item.DefendCompose);
				sqlParameters[10] = new SqlParameter("@IsBinds", item.IsBinds);
				sqlParameters[11] = new SqlParameter("@IsExist", item.IsExist);
				sqlParameters[12] = new SqlParameter("@IsJudge", item.IsJudge);
				sqlParameters[13] = new SqlParameter("@LuckCompose", item.LuckCompose);
				flag = this.db.RunProcedure("SP_Users_Items_Add", sqlParameters);
				item.ItemID = (int)sqlParameters[0].Value;
				item.IsDirty = false;
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000FF04 File Offset: 0x0000E104
		public bool AddUserMatchInfo(UserMatchInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[17];
				para[0] = new SqlParameter("@ID", info.ID);
				para[0].Direction = ParameterDirection.Output;
				para[1] = new SqlParameter("@UserID", info.UserID);
				para[2] = new SqlParameter("@dailyScore", info.dailyScore);
				para[3] = new SqlParameter("@dailyWinCount", info.dailyWinCount);
				para[4] = new SqlParameter("@dailyGameCount", info.dailyGameCount);
				para[5] = new SqlParameter("@DailyLeagueFirst", info.DailyLeagueFirst);
				para[6] = new SqlParameter("@DailyLeagueLastScore", info.DailyLeagueLastScore);
				para[7] = new SqlParameter("@weeklyScore", info.weeklyScore);
				para[8] = new SqlParameter("@weeklyGameCount", info.weeklyGameCount);
				para[9] = new SqlParameter("@weeklyRanking", info.weeklyRanking);
				para[10] = new SqlParameter("@addDayPrestge", info.addDayPrestge);
				para[11] = new SqlParameter("@totalPrestige", info.totalPrestige);
				para[12] = new SqlParameter("@restCount", info.restCount);
				para[13] = new SqlParameter("@leagueGrade", info.leagueGrade);
				para[14] = new SqlParameter("@leagueItemsGet", info.leagueItemsGet);
				para[16] = new SqlParameter("@WeeklyWinCount", info.WeeklyWinCount);
				para[15] = new SqlParameter("@Result", SqlDbType.Int);
				para[15].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UserMatch_Add", para);
				result = ((int)para[15].Value == 0);
				info.ID = (int)para[0].Value;
				info.IsDirty = false;
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00010170 File Offset: 0x0000E370
		public bool AddUserRank(UserRankInfo item)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[16];
				para[0] = new SqlParameter("@ID", item.ID);
				para[0].Direction = ParameterDirection.Output;
				para[1] = new SqlParameter("@UserID", item.UserID);
				para[2] = new SqlParameter("@UserRank", item.Name);
				para[3] = new SqlParameter("@Attack", item.Attack);
				para[4] = new SqlParameter("@Defence", item.Defence);
				para[5] = new SqlParameter("@Luck", item.Luck);
				para[6] = new SqlParameter("@Agility", item.Agility);
				para[7] = new SqlParameter("@HP", item.HP);
				para[8] = new SqlParameter("@Damage", item.Damage);
				para[9] = new SqlParameter("@Guard", item.Guard);
				para[10] = new SqlParameter("@BeginDate", item.BeginDate);
				para[11] = new SqlParameter("@Validate", item.Validate);
				para[12] = new SqlParameter("@IsExit", item.IsExit);
				para[13] = new SqlParameter("@Result", SqlDbType.Int);
				para[13].Direction = ParameterDirection.ReturnValue;
				para[14] = new SqlParameter("@NewTitleID", item.NewTitleID);
				para[15] = new SqlParameter("@EndDate", item.EndDate);
				this.db.RunProcedure("SP_UserRank_Add", para);
				result = ((int)para[13].Value == 0);
				item.ID = (int)para[0].Value;
				item.IsDirty = false;
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			return result;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000103A0 File Offset: 0x0000E5A0
		public bool CancelPaymentMail(int userid, int mailID, ref int senderID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[4];
				array[0] = new SqlParameter("@userid", userid);
				array[1] = new SqlParameter("@mailID", mailID);
				array[2] = new SqlParameter("@senderID", SqlDbType.Int);
				SqlParameter[] sqlParameters = array;
				sqlParameters[2].Value = senderID;
				sqlParameters[2].Direction = ParameterDirection.InputOutput;
				sqlParameters[3] = new SqlParameter("@Result", SqlDbType.Int);
				sqlParameters[3].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Mail_PaymentCancel", sqlParameters);
				flag = ((int)sqlParameters[3].Value == 0);
				bool flag2 = flag;
				if (flag2)
				{
					senderID = (int)sqlParameters[2].Value;
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000104A0 File Offset: 0x0000E6A0
		public bool ChargeToUser(string userName, ref int money, string nickName)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[3];
				array[0] = new SqlParameter("@UserName", userName);
				array[1] = new SqlParameter("@money", SqlDbType.Int);
				SqlParameter[] sqlParameters = array;
				sqlParameters[1].Direction = ParameterDirection.Output;
				sqlParameters[2] = new SqlParameter("@NickName", nickName);
				flag = this.db.RunProcedure("SP_Charge_To_User", sqlParameters);
				money = (int)sqlParameters[1].Value;
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00010550 File Offset: 0x0000E750
		public bool CheckAccount(string username, string password)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Username", username),
					new SqlParameter("@Password", password),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[2].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_CheckAccount", sqlParameters);
				flag = ((int)sqlParameters[2].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00010600 File Offset: 0x0000E800
		public bool CheckEmailIsValid(string Email)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Email", Email),
					new SqlParameter("@count", SqlDbType.BigInt)
				};
				sqlParameters[1].Direction = ParameterDirection.Output;
				this.db.RunProcedure("CheckEmailIsValid", sqlParameters);
				bool flag2 = int.Parse(sqlParameters[1].Value.ToString()) == 0;
				if (flag2)
				{
					flag = true;
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init CheckEmailIsValid", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000106B4 File Offset: 0x0000E8B4
		public bool DeleteAuction(int auctionID, int userID, ref string msg)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@AuctionID", auctionID),
					new SqlParameter("@UserID", userID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				SqlParameters[2].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Auction_Delete", SqlParameters);
				int num = (int)SqlParameters[2].Value;
				flag = (num == 0);
				switch (num)
				{
				case 0:
					msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg1", Array.Empty<object>());
					result = flag;
					break;
				case 1:
					msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg2", Array.Empty<object>());
					result = flag;
					break;
				case 2:
					msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg3", Array.Empty<object>());
					result = flag;
					break;
				default:
					msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg4", Array.Empty<object>());
					result = flag;
					break;
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000107E4 File Offset: 0x0000E9E4
		public bool DeleteFriends(int UserID, int FriendID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", FriendID),
					new SqlParameter("@UserID", UserID)
				};
				flag = this.db.RunProcedure("SP_Users_Friends_Delete", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00010878 File Offset: 0x0000EA78
		public bool DeleteGoods(int itemID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", itemID)
				};
				flag = this.db.RunProcedure("SP_Users_Items_Delete", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000108F8 File Offset: 0x0000EAF8
		public bool DeleteMail(int UserID, int mailID, out int senderID)
		{
			bool flag = false;
			senderID = 0;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[4];
				array[0] = new SqlParameter("@ID", mailID);
				array[1] = new SqlParameter("@UserID", UserID);
				array[2] = new SqlParameter("@SenderID", SqlDbType.Int);
				SqlParameter[] sqlParameters = array;
				sqlParameters[2].Value = senderID;
				sqlParameters[2].Direction = ParameterDirection.InputOutput;
				sqlParameters[3] = new SqlParameter("@Result", SqlDbType.Int);
				sqlParameters[3].Direction = ParameterDirection.ReturnValue;
				flag = this.db.RunProcedure("SP_Mail_Delete", sqlParameters);
				bool flag2 = (int)sqlParameters[3].Value == 0;
				if (flag2)
				{
					flag = true;
					senderID = (int)sqlParameters[2].Value;
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000109F8 File Offset: 0x0000EBF8
		public bool DeleteMail2(int UserID, int mailID, out int senderID)
		{
			bool flag = false;
			senderID = 0;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[4];
				array[0] = new SqlParameter("@ID", mailID);
				array[1] = new SqlParameter("@UserID", UserID);
				array[2] = new SqlParameter("@SenderID", SqlDbType.Int);
				SqlParameter[] sqlParameters = array;
				sqlParameters[2].Value = senderID;
				sqlParameters[2].Direction = ParameterDirection.InputOutput;
				sqlParameters[3] = new SqlParameter("@Result", SqlDbType.Int);
				sqlParameters[3].Direction = ParameterDirection.ReturnValue;
				flag = this.db.RunProcedure("SP_Mail_Delete", sqlParameters);
				bool flag2 = (int)sqlParameters[3].Value == 0;
				if (flag2)
				{
					flag = true;
					senderID = (int)sqlParameters[2].Value;
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00010AF8 File Offset: 0x0000ECF8
		public bool DeleteMarryInfo(int ID, int userID, ref string msg)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", ID),
					new SqlParameter("@UserID", userID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[2].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_MarryInfo_Delete", sqlParameters);
				int num = (int)sqlParameters[2].Value;
				flag = (num == 0);
				bool flag2 = num == 0;
				if (flag2)
				{
					msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Succeed", Array.Empty<object>());
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("DeleteAuction", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00010BDC File Offset: 0x0000EDDC
		public bool DisableUser(string userName, bool isExit)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", userName),
					new SqlParameter("@IsExist", isExit),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[2].Direction = ParameterDirection.ReturnValue;
				flag = this.db.RunProcedure("SP_Disable_User", sqlParameters);
				bool flag2 = (int)sqlParameters[2].Value == 0;
				if (flag2)
				{
					flag = true;
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("DisableUser", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00010CA0 File Offset: 0x0000EEA0
		public bool DisposeMarryRoomInfo(int ID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", ID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[1].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Dispose_Marry_Room_Info", sqlParameters);
				flag = ((int)sqlParameters[1].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("DisposeMarryRoomInfo", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00010D48 File Offset: 0x0000EF48
		public ConsortiaUserInfo[] GetAllMemberByConsortia(int ConsortiaID)
		{
			List<ConsortiaUserInfo> list = new List<ConsortiaUserInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ConsortiaID", SqlDbType.Int, 4)
				};
				SqlParameters[0].Value = ConsortiaID;
				this.db.GetReader(ref ResultDataReader, "SP_Consortia_Users_All", SqlParameters);
				while (ResultDataReader.Read())
				{
					list.Add(this.InitConsortiaUserInfo(ResultDataReader));
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
				}
			}
			finally
			{
				bool flag = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag)
				{
					ResultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00010E2C File Offset: 0x0000F02C
		public UserMatchInfo[] GetAllUserMatchInfo()
		{
			List<UserMatchInfo> list = new List<UserMatchInfo>();
			SqlDataReader resultDataReader = null;
			int num = 1;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_UserMatch_All_DESC");
				while (resultDataReader.Read())
				{
					UserMatchInfo item = new UserMatchInfo
					{
						UserID = (int)resultDataReader["UserID"],
						totalPrestige = (int)resultDataReader["totalPrestige"],
						rank = num
					};
					list.Add(item);
					num++;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetAllUserMatchDESC", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00010F28 File Offset: 0x0000F128
		public UserMatchInfo[] GetTopUserMatchInfo()
		{
			List<UserMatchInfo> list = new List<UserMatchInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_GetListLeague");
				while (resultDataReader.Read())
				{
					UserMatchInfo item = new UserMatchInfo
					{
						UserID = (int)resultDataReader["UserID"],
						totalPrestige = (int)resultDataReader["totalPrestige"],
						weeklyRanking = (int)resultDataReader["weeklyRanking"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetTopUserMatchInfo", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0001102C File Offset: 0x0000F22C
		public AuctionInfo[] GetAuctionPage(int page, string name, int type, int pay, ref int total, int userID, int buyID, int order, bool sort, int size, string string_1)
		{
			List<AuctionInfo> auctionInfoList = new List<AuctionInfo>();
			try
			{
				string str = " IsExist=1 ";
				bool flag = !string.IsNullOrEmpty(name);
				if (flag)
				{
					str = str + " and Name like '%" + name + "%' ";
				}
				int num = type;
				int num2 = num;
				switch (num2)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				case 17:
				case 19:
					str = str + " and Category =" + type.ToString() + " ";
					break;
				case 18:
				case 20:
				case 30:
				case 31:
				case 32:
				case 33:
				case 34:
					break;
				case 21:
					str += " and Category in(1,2,5,8,9) ";
					break;
				case 22:
					str += " and Category in(13,15,6,4,3) ";
					break;
				case 23:
					str += " and Category in(16,11,10) ";
					break;
				case 24:
					str += " and Category in(8,9) ";
					break;
				case 25:
					str += " and Category in (7,17) ";
					break;
				case 26:
					str += " and TemplateId>=311000 and TemplateId<=313999";
					break;
				case 27:
					str += " and TemplateId>=311000 and TemplateId<=311999 ";
					break;
				case 28:
					str += " and TemplateId>=312000 and TemplateId<=312999 ";
					break;
				case 29:
					str += " and TemplateId>=313000 and TempLateId<=313999";
					break;
				case 35:
					str += " and TemplateID in (11560,11561,11562)";
					break;
				default:
					switch (num2)
					{
					case 1100:
						str += " and TemplateID in (11019,11021,11022,11023) ";
						break;
					case 1101:
						str += " and TemplateID='11019' ";
						break;
					case 1102:
						str += " and TemplateID='11021' ";
						break;
					case 1103:
						str += " and TemplateID='11022' ";
						break;
					case 1104:
						str += " and TemplateID='11023' ";
						break;
					case 1105:
						str += " and TemplateID in (11001,11002,11003,11004,11005,11006,11007,11008,11009,11010,11011,11012,11013,11014,11015,11016) ";
						break;
					case 1106:
						str += " and TemplateID in (11001,11002,11003,11004) ";
						break;
					case 1107:
						str += " and TemplateID in (11005,11006,11007,11008) ";
						break;
					case 1108:
						str += " and TemplateID in (11009,11010,11011,11012) ";
						break;
					case 1109:
						str += " and TemplateID in (11013,11014,11015,11016) ";
						break;
					case 1110:
						str += " and TemplateID='11024' ";
						break;
					case 1111:
						str += "and TemplateID in (11039,11041,11043,11047,11040,11042,11044,11048)";
						break;
					case 1112:
						str += "and TemplateID in (11037,11038,11045,11046)";
						break;
					case 1113:
						str += " and TemplateID in (314101,314102,314103,314104,314105,314106,314107,314108,314109,314110,314111,314112,314113,314114,314115,314116,314121,314122,314123,314124,314125,314126,314127,314128,314129,314130,314131,314132,314133,314134) ";
						break;
					case 1114:
						str += " and TemplateID in (314117,314118,314119,314120,314135,314136,314137,314138,314139) ";
						break;
					case 1116:
						str += " and TemplateID='11035' ";
						break;
					case 1117:
						str += " and TemplateID='11036' ";
						break;
					case 1118:
						str += " and TemplateID='11026' ";
						break;
					case 1119:
						str += " and TemplateID='11027' ";
						break;
					}
					break;
				}
				bool flag2 = pay != -1;
				if (flag2)
				{
					str = str + " and PayType =" + pay.ToString() + " ";
				}
				bool flag3 = userID != -1;
				if (flag3)
				{
					str = str + " and AuctioneerID =" + userID.ToString() + " ";
				}
				bool flag4 = buyID != -1;
				if (flag4)
				{
					str = string.Concat(new string[]
					{
						str,
						" and (BuyerID =",
						buyID.ToString(),
						" or AuctionID in (",
						string_1,
						")) "
					});
				}
				string str2 = "Category,Name,Price,dd,AuctioneerID";
				switch (order)
				{
				case 0:
					str2 = "Name";
					break;
				case 2:
					str2 = "dd";
					break;
				case 3:
					str2 = "AuctioneerName";
					break;
				case 4:
					str2 = "Price";
					break;
				case 5:
					str2 = "BuyerName";
					break;
				}
				string str3 = str2 + (sort ? " desc" : "") + ",AuctionID ";
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@QueryStr", "V_Auction_Scan"),
					new SqlParameter("@QueryWhere", str),
					new SqlParameter("@PageSize", size),
					new SqlParameter("@PageCurrent", page),
					new SqlParameter("@FdShow", "*"),
					new SqlParameter("@FdOrder", str3),
					new SqlParameter("@FdKey", "AuctionID"),
					new SqlParameter("@TotalRow", total)
				};
				SqlParameters[7].Direction = ParameterDirection.Output;
				DataTable dataTable = this.db.GetDataTable("Auction", "SP_CustomPage", SqlParameters);
				total = (int)SqlParameters[7].Value;
				foreach (object obj in dataTable.Rows)
				{
					DataRow row = (DataRow)obj;
					auctionInfoList.Add(new AuctionInfo
					{
						AuctioneerID = (int)row["AuctioneerID"],
						AuctioneerName = row["AuctioneerName"].ToString(),
						AuctionID = (int)row["AuctionID"],
						BeginDate = (DateTime)row["BeginDate"],
						BuyerID = (int)row["BuyerID"],
						BuyerName = row["BuyerName"].ToString(),
						Category = (int)row["Category"],
						IsExist = (bool)row["IsExist"],
						ItemID = (int)row["ItemID"],
						Name = row["Name"].ToString(),
						Mouthful = (int)row["Mouthful"],
						PayType = (int)row["PayType"],
						Price = (int)row["Price"],
						Rise = (int)row["Rise"],
						ValidDate = (int)row["ValidDate"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
				}
			}
			return auctionInfoList.ToArray();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00011758 File Offset: 0x0000F958
		public AuctionInfo GetAuctionSingle(int auctionID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@AuctionID", auctionID)
				};
				this.db.GetReader(ref resultDataReader, "SP_Auction_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitAuctionInfo(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00011820 File Offset: 0x0000FA20
		public BestEquipInfo[] GetCelebByDayBestEquip()
		{
			List<BestEquipInfo> list = new List<BestEquipInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Users_BestEquip");
				while (resultDataReader.Read())
				{
					BestEquipInfo item = new BestEquipInfo
					{
						Date = (DateTime)resultDataReader["RemoveDate"],
						GP = (int)resultDataReader["GP"],
						Grade = (int)resultDataReader["Grade"],
						ItemName = ((resultDataReader["Name"] == null) ? "" : resultDataReader["Name"].ToString()),
						NickName = ((resultDataReader["NickName"] == null) ? "" : resultDataReader["NickName"].ToString()),
						Sex = (bool)resultDataReader["Sex"],
						Strengthenlevel = (int)resultDataReader["Strengthenlevel"],
						UserName = ((resultDataReader["UserName"] == null) ? "" : resultDataReader["UserName"].ToString())
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000119F0 File Offset: 0x0000FBF0
		public ChargeRecordInfo[] GetChargeRecordInfo(DateTime date, int SaveRecordSecond)
		{
			List<ChargeRecordInfo> list = new List<ChargeRecordInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Date", date.ToString("yyyy-MM-dd HH:mm:ss")),
					new SqlParameter("@Second", SaveRecordSecond)
				};
				this.db.GetReader(ref resultDataReader, "SP_Charge_Record", sqlParameters);
				while (resultDataReader.Read())
				{
					ChargeRecordInfo item = new ChargeRecordInfo
					{
						BoyTotalPay = (int)resultDataReader["BoyTotalPay"],
						GirlTotalPay = (int)resultDataReader["GirlTotalPay"],
						PayWay = ((resultDataReader["PayWay"] == null) ? "" : resultDataReader["PayWay"].ToString()),
						TotalBoy = (int)resultDataReader["TotalBoy"],
						TotalGirl = (int)resultDataReader["TotalGirl"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00011B8C File Offset: 0x0000FD8C
		public ExerciseInfo GetExerciseSingle(int Grade)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Grage", Grade)
				};
				this.db.GetReader(ref resultDataReader, "SP_Get_Exercise_By_Grade", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new ExerciseInfo
					{
						Grage = (int)resultDataReader["Grage"],
						GP = (int)resultDataReader["GP"],
						ExerciseA = (int)resultDataReader["ExerciseA"],
						ExerciseAG = (int)resultDataReader["ExerciseAG"],
						ExerciseD = (int)resultDataReader["ExerciseD"],
						ExerciseH = (int)resultDataReader["ExerciseH"],
						ExerciseL = (int)resultDataReader["ExerciseL"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetExerciseInfoSingle", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00011D04 File Offset: 0x0000FF04
		public FriendInfo[] GetFriendsAll(int UserID)
		{
			List<FriendInfo> friendInfoList = new List<FriendInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				SqlParameters[0].Value = UserID;
				this.db.GetReader(ref ResultDataReader, "SP_Users_Friends", SqlParameters);
				while (ResultDataReader.Read())
				{
					friendInfoList.Add(new FriendInfo
					{
						AddDate = (DateTime)ResultDataReader["AddDate"],
						Colors = ((ResultDataReader["Colors"] == null) ? "" : ResultDataReader["Colors"].ToString()),
						FriendID = (int)ResultDataReader["FriendID"],
						Grade = (int)ResultDataReader["Grade"],
						Hide = (int)ResultDataReader["Hide"],
						ID = (int)ResultDataReader["ID"],
						IsExist = (bool)ResultDataReader["IsExist"],
						NickName = ((ResultDataReader["NickName"] == null) ? "" : ResultDataReader["NickName"].ToString()),
						Remark = ((ResultDataReader["Remark"] == null) ? "" : ResultDataReader["Remark"].ToString()),
						Sex = (((bool)ResultDataReader["Sex"]) ? 1 : 0),
						State = (int)ResultDataReader["State"],
						Style = ((ResultDataReader["Style"] == null) ? "" : ResultDataReader["Style"].ToString()),
						UserID = (int)ResultDataReader["UserID"],
						ConsortiaName = ((ResultDataReader["ConsortiaName"] == null) ? "" : ResultDataReader["ConsortiaName"].ToString()),
						Offer = (int)ResultDataReader["Offer"],
						Win = (int)ResultDataReader["Win"],
						Total = (int)ResultDataReader["Total"],
						Escape = (int)ResultDataReader["Escape"],
						Relation = (int)ResultDataReader["Relation"],
						Repute = (int)ResultDataReader["Repute"],
						UserName = ((ResultDataReader["UserName"] == null) ? "" : ResultDataReader["UserName"].ToString()),
						DutyName = ((ResultDataReader["DutyName"] == null) ? "" : ResultDataReader["DutyName"].ToString()),
						Nimbus = (int)ResultDataReader["Nimbus"],
						apprenticeshipState = (int)ResultDataReader["apprenticeshipState"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
				}
			}
			finally
			{
				bool flag = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag)
				{
					ResultDataReader.Close();
				}
			}
			return friendInfoList.ToArray();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000120C0 File Offset: 0x000102C0
		public FriendInfo[] GetFriendsBbs(string condictArray)
		{
			List<FriendInfo> list = new List<FriendInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@SearchUserName", SqlDbType.NVarChar, 4000)
				};
				sqlParameters[0].Value = condictArray;
				this.db.GetReader(ref resultDataReader, "SP_Users_FriendsBbs", sqlParameters);
				while (resultDataReader.Read())
				{
					FriendInfo item = new FriendInfo
					{
						NickName = ((resultDataReader["NickName"] == null) ? "" : resultDataReader["NickName"].ToString()),
						UserID = (int)resultDataReader["UserID"],
						UserName = ((resultDataReader["UserName"] == null) ? "" : resultDataReader["UserName"].ToString()),
						IsExist = ((int)resultDataReader["UserID"] > 0)
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0001224C File Offset: 0x0001044C
		public ArrayList GetFriendsGood(string UserName)
		{
			ArrayList list = new ArrayList();
			SqlDataReader resultDataReader = null;
			ArrayList result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", SqlDbType.NVarChar)
				};
				sqlParameters[0].Value = UserName;
				this.db.GetReader(ref resultDataReader, "SP_Users_Friends_Good", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add((resultDataReader["UserName"] == null) ? "" : resultDataReader["UserName"].ToString());
				}
				result = list;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = list;
				}
				else
				{
					result = list;
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return result;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00012344 File Offset: 0x00010544
		public Dictionary<int, int> GetFriendsIDAll(int UserID)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			SqlDataReader resultDataReader = null;
			Dictionary<int, int> result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = UserID;
				this.db.GetReader(ref resultDataReader, "SP_Users_Friends_All", sqlParameters);
				while (resultDataReader.Read())
				{
					bool flag = !dictionary.ContainsKey((int)resultDataReader["FriendID"]);
					if (flag)
					{
						dictionary.Add((int)resultDataReader["FriendID"], (int)resultDataReader["Relation"]);
					}
					else
					{
						dictionary[(int)resultDataReader["FriendID"]] = (int)resultDataReader["Relation"];
					}
				}
				result = dictionary;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = dictionary;
				}
				else
				{
					result = dictionary;
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return result;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0001248C File Offset: 0x0001068C
		public MailInfo[] GetMailBySenderID(int userID)
		{
			List<MailInfo> list = new List<MailInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = userID;
				this.db.GetReader(ref resultDataReader, "SP_Mail_BySenderID", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitMail(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00012570 File Offset: 0x00010770
		public MailInfo[] GetMailByUserID(int userID)
		{
			List<MailInfo> list = new List<MailInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = userID;
				this.db.GetReader(ref resultDataReader, "SP_Mail_ByUserID", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitMail(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00012654 File Offset: 0x00010854
		public MailInfo GetMailSingle(int UserID, int mailID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", mailID),
					new SqlParameter("@UserID", UserID)
				};
				this.db.GetReader(ref resultDataReader, "SP_Mail_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitMail(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0001272C File Offset: 0x0001092C
		public MarryInfo[] GetMarryInfoPage(int page, string name, bool sex, int size, ref int total)
		{
			List<MarryInfo> list = new List<MarryInfo>();
			try
			{
				string str = (!sex) ? " IsExist=1 and Sex=0 and UserExist=1" : " IsExist=1 and Sex=1 and UserExist=1";
				bool flag = !string.IsNullOrEmpty(name);
				if (flag)
				{
					str = str + " and NickName like '%" + name + "%' ";
				}
				string str2 = "State desc,IsMarried";
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@QueryStr", "V_Sys_Marry_Info"),
					new SqlParameter("@QueryWhere", str),
					new SqlParameter("@PageSize", size),
					new SqlParameter("@PageCurrent", page),
					new SqlParameter("@FdShow", "*"),
					new SqlParameter("@FdOrder", str2),
					new SqlParameter("@FdKey", "ID"),
					new SqlParameter("@TotalRow", total)
				};
				sqlParameters[7].Direction = ParameterDirection.Output;
				DataTable dataTable = this.db.GetDataTable("V_Sys_Marry_Info", "SP_CustomPage", sqlParameters);
				total = (int)sqlParameters[7].Value;
				foreach (object obj in dataTable.Rows)
				{
					DataRow row = (DataRow)obj;
					MarryInfo item = new MarryInfo
					{
						ID = (int)row["ID"],
						UserID = (int)row["UserID"],
						IsPublishEquip = (bool)row["IsPublishEquip"],
						Introduction = row["Introduction"].ToString(),
						NickName = row["NickName"].ToString(),
						IsConsortia = (bool)row["IsConsortia"],
						ConsortiaID = (int)row["ConsortiaID"],
						Sex = (bool)row["Sex"],
						Win = (int)row["Win"],
						Total = (int)row["Total"],
						Escape = (int)row["Escape"],
						GP = (int)row["GP"],
						Honor = row["Honor"].ToString(),
						Style = row["Style"].ToString(),
						Colors = row["Colors"].ToString(),
						Hide = (int)row["Hide"],
						Grade = (int)row["Grade"],
						State = (int)row["State"],
						Repute = (int)row["Repute"],
						Skin = row["Skin"].ToString(),
						Offer = (int)row["Offer"],
						IsMarried = (bool)row["IsMarried"],
						ConsortiaName = row["ConsortiaName"].ToString(),
						DutyName = row["DutyName"].ToString(),
						Nimbus = (int)row["Nimbus"],
						FightPower = (int)row["FightPower"],
						typeVIP = Convert.ToByte(row["typeVIP"]),
						VIPLevel = (int)row["VIPLevel"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00012BA8 File Offset: 0x00010DA8
		public MarryInfo GetMarryInfoSingle(int ID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", ID)
				};
				this.db.GetReader(ref resultDataReader, "SP_MarryInfo_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new MarryInfo
					{
						ID = (int)resultDataReader["ID"],
						UserID = (int)resultDataReader["UserID"],
						IsPublishEquip = (bool)resultDataReader["IsPublishEquip"],
						Introduction = resultDataReader["Introduction"].ToString(),
						RegistTime = (DateTime)resultDataReader["RegistTime"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetMarryInfoSingle", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00012CE0 File Offset: 0x00010EE0
		public MarryProp GetMarryProp(int id)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", id)
				};
				this.db.GetReader(ref resultDataReader, "SP_Select_Marry_Prop", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new MarryProp
					{
						IsMarried = (bool)resultDataReader["IsMarried"],
						SpouseID = (int)resultDataReader["SpouseID"],
						SpouseName = resultDataReader["SpouseName"].ToString(),
						IsCreatedMarryRoom = (bool)resultDataReader["IsCreatedMarryRoom"],
						SelfMarryRoomID = (int)resultDataReader["SelfMarryRoomID"],
						IsGotRing = (bool)resultDataReader["IsGotRing"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetMarryProp", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00012E30 File Offset: 0x00011030
		public MarryRoomInfo[] GetMarryRoomInfo()
		{
			SqlDataReader resultDataReader = null;
			List<MarryRoomInfo> list = new List<MarryRoomInfo>();
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Get_Marry_Room_Info");
				while (resultDataReader.Read())
				{
					MarryRoomInfo item = new MarryRoomInfo
					{
						ID = (int)resultDataReader["ID"],
						Name = resultDataReader["Name"].ToString(),
						PlayerID = (int)resultDataReader["PlayerID"],
						PlayerName = resultDataReader["PlayerName"].ToString(),
						GroomID = (int)resultDataReader["GroomID"],
						GroomName = resultDataReader["GroomName"].ToString(),
						BrideID = (int)resultDataReader["BrideID"],
						BrideName = resultDataReader["BrideName"].ToString(),
						Pwd = resultDataReader["Pwd"].ToString(),
						AvailTime = (int)resultDataReader["AvailTime"],
						MaxCount = (int)resultDataReader["MaxCount"],
						GuestInvite = (bool)resultDataReader["GuestInvite"],
						MapIndex = (int)resultDataReader["MapIndex"],
						BeginTime = (DateTime)resultDataReader["BeginTime"],
						BreakTime = (DateTime)resultDataReader["BreakTime"],
						RoomIntroduction = resultDataReader["RoomIntroduction"].ToString(),
						ServerID = (int)resultDataReader["ServerID"],
						IsHymeneal = (bool)resultDataReader["IsHymeneal"],
						IsGunsaluteUsed = (bool)resultDataReader["IsGunsaluteUsed"]
					};
					list.Add(item);
				}
				return list.ToArray();
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetMarryRoomInfo", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000130C4 File Offset: 0x000112C4
		public MarryRoomInfo GetMarryRoomInfoSingle(int id)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", id)
				};
				this.db.GetReader(ref resultDataReader, "SP_Get_Marry_Room_Info_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new MarryRoomInfo
					{
						ID = (int)resultDataReader["ID"],
						Name = resultDataReader["Name"].ToString(),
						PlayerID = (int)resultDataReader["PlayerID"],
						PlayerName = resultDataReader["PlayerName"].ToString(),
						GroomID = (int)resultDataReader["GroomID"],
						GroomName = resultDataReader["GroomName"].ToString(),
						BrideID = (int)resultDataReader["BrideID"],
						BrideName = resultDataReader["BrideName"].ToString(),
						Pwd = resultDataReader["Pwd"].ToString(),
						AvailTime = (int)resultDataReader["AvailTime"],
						MaxCount = (int)resultDataReader["MaxCount"],
						GuestInvite = (bool)resultDataReader["GuestInvite"],
						MapIndex = (int)resultDataReader["MapIndex"],
						BeginTime = (DateTime)resultDataReader["BeginTime"],
						BreakTime = (DateTime)resultDataReader["BreakTime"],
						RoomIntroduction = resultDataReader["RoomIntroduction"].ToString(),
						ServerID = (int)resultDataReader["ServerID"],
						IsHymeneal = (bool)resultDataReader["IsHymeneal"],
						IsGunsaluteUsed = (bool)resultDataReader["IsGunsaluteUsed"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetMarryRoomInfo", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00013358 File Offset: 0x00011558
		public void GetPasswordInfo(int userID, ref string PasswordQuestion1, ref string PasswordAnswer1, ref string PasswordQuestion2, ref string PasswordAnswer2, ref int Count)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", userID)
				};
				this.db.GetReader(ref resultDataReader, "SP_Users_PasswordInfo", sqlParameters);
				while (resultDataReader.Read())
				{
					PasswordQuestion1 = ((resultDataReader["PasswordQuestion1"] == null) ? "" : resultDataReader["PasswordQuestion1"].ToString());
					PasswordAnswer1 = ((resultDataReader["PasswordAnswer1"] == null) ? "" : resultDataReader["PasswordAnswer1"].ToString());
					PasswordQuestion2 = ((resultDataReader["PasswordQuestion2"] == null) ? "" : resultDataReader["PasswordQuestion2"].ToString());
					PasswordAnswer2 = ((resultDataReader["PasswordAnswer2"] == null) ? "" : resultDataReader["PasswordAnswer2"].ToString());
					bool flag = (DateTime)resultDataReader["LastFindDate"] == DateTime.Today;
					if (flag)
					{
						Count = (int)resultDataReader["FailedPasswordAttemptCount"];
					}
					else
					{
						Count = 5;
					}
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00013504 File Offset: 0x00011704
		public MarryApplyInfo[] GetPlayerMarryApply(int UserID)
		{
			SqlDataReader resultDataReader = null;
			List<MarryApplyInfo> list = new List<MarryApplyInfo>();
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserID)
				};
				this.db.GetReader(ref resultDataReader, "SP_Get_Marry_Apply", sqlParameters);
				while (resultDataReader.Read())
				{
					MarryApplyInfo item = new MarryApplyInfo
					{
						UserID = (int)resultDataReader["UserID"],
						ApplyUserID = (int)resultDataReader["ApplyUserID"],
						ApplyUserName = resultDataReader["ApplyUserName"].ToString(),
						ApplyType = (int)resultDataReader["ApplyType"],
						ApplyResult = (bool)resultDataReader["ApplyResult"],
						LoveProclamation = resultDataReader["LoveProclamation"].ToString(),
						ID = (int)resultDataReader["Id"]
					};
					list.Add(item);
				}
				return list.ToArray();
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetPlayerMarryApply", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000136A0 File Offset: 0x000118A0
		public PlayerInfo[] GetPlayerMathPage(int page, int size, ref int total, ref bool resultValue)
		{
			List<PlayerInfo> playerInfoList = new List<PlayerInfo>();
			try
			{
				string queryWhere = "  ";
				string fdOreder = "weeklyScore desc";
				foreach (object obj in base.GetPage("V_Sys_Users_Math", queryWhere, page, size, "*", fdOreder, "UserID", ref total).Rows)
				{
					DataRow row = (DataRow)obj;
					playerInfoList.Add(new PlayerInfo
					{
						ID = (int)row["UserID"],
						Colors = ((row["Colors"] == null) ? "" : row["Colors"].ToString()),
						GP = (int)row["GP"],
						Grade = (int)row["Grade"],
						NickName = ((row["NickName"] == null) ? "" : row["NickName"].ToString()),
						Sex = (bool)row["Sex"],
						State = (int)row["State"],
						Style = ((row["Style"] == null) ? "" : row["Style"].ToString()),
						Hide = (int)row["Hide"],
						Repute = (int)row["Repute"],
						UserName = ((row["UserName"] == null) ? "" : row["UserName"].ToString()),
						Skin = ((row["Skin"] == null) ? "" : row["Skin"].ToString()),
						Win = (int)row["Win"],
						Total = (int)row["Total"],
						Nimbus = (int)row["Nimbus"],
						FightPower = (int)row["FightPower"],
						AchievementPoint = (int)row["AchievementPoint"],
						typeVIP = Convert.ToByte(row["typeVIP"]),
						VIPLevel = (int)row["VIPLevel"],
						AddWeekLeagueScore = (int)row["weeklyScore"]
					});
				}
				resultValue = true;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
				}
			}
			return playerInfoList.ToArray();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00013A14 File Offset: 0x00011C14
		public PlayerInfo[] GetPlayerPage(int page, int size, ref int total, int order, int userID, ref bool resultValue)
		{
			return this.GetPlayerPage(page, size, ref total, order, 0, userID, ref resultValue);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00013A38 File Offset: 0x00011C38
		public PlayerInfo[] GetPlayerPage(int page, int size, ref int total, int order, int where, int userID, ref bool resultValue)
		{
			List<PlayerInfo> list = new List<PlayerInfo>();
			try
			{
				string queryWhere = " IsExist=1 and IsFirst<> 0 ";
				bool flag = userID != -1;
				if (flag)
				{
					queryWhere = queryWhere + " and UserID =" + userID.ToString() + " ";
				}
				string str = "GP desc";
				switch (order)
				{
				case 1:
					str = "Offer desc";
					break;
				case 2:
					str = "AddDayGP desc";
					break;
				case 3:
					str = "AddWeekGP desc";
					break;
				case 4:
					str = "AddDayOffer desc";
					break;
				case 5:
					str = "AddWeekOffer desc";
					break;
				case 6:
					str = "FightPower desc";
					break;
				case 7:
					str = "EliteScore desc";
					break;
				case 8:
					str = "State desc, graduatesCount desc, FightPower desc";
					break;
				case 9:
					str = "NEWID()";
					break;
				case 10:
					str = "State desc, GP asc, FightPower desc";
					break;
				}
				switch (where)
				{
				case 0:
					queryWhere += " ";
					break;
				case 1:
					queryWhere += " and Grade >= 20 ";
					break;
				case 2:
					queryWhere += " and Grade > 5 and Grade < 17 ";
					break;
				case 3:
					queryWhere += " and Grade >= 20 and apprenticeshipState != 3 and State = 1 ";
					break;
				case 4:
					queryWhere += " and Grade > 5 and Grade < 17 and masterID = 0 and State = 1 ";
					break;
				}
				string fdOreder = str + ",UserID";
				foreach (object obj in base.GetPage("V_Sys_Users_Detail", queryWhere, page, size, "*", fdOreder, "UserID", ref total).Rows)
				{
					DataRow dataRow = (DataRow)obj;
					list.Add(new PlayerInfo
					{
						Agility = (int)dataRow["Agility"],
						Attack = (int)dataRow["Attack"],
						Colors = ((dataRow["Colors"] == null) ? "" : dataRow["Colors"].ToString()),
						ConsortiaID = (int)dataRow["ConsortiaID"],
						Defence = (int)dataRow["Defence"],
						Gold = (int)dataRow["Gold"],
						GP = (int)dataRow["GP"],
						Grade = (int)dataRow["Grade"],
						ID = (int)dataRow["UserID"],
						Luck = (int)dataRow["Luck"],
						Money = (int)dataRow["Money"],
						NickName = ((dataRow["NickName"] == null) ? "" : dataRow["NickName"].ToString()),
						Sex = (bool)dataRow["Sex"],
						State = (int)dataRow["State"],
						Style = ((dataRow["Style"] == null) ? "" : dataRow["Style"].ToString()),
						Hide = (int)dataRow["Hide"],
						Repute = (int)dataRow["Repute"],
						UserName = ((dataRow["UserName"] == null) ? "" : dataRow["UserName"].ToString()),
						ConsortiaName = ((dataRow["ConsortiaName"] == null) ? "" : dataRow["ConsortiaName"].ToString()),
						Offer = (int)dataRow["Offer"],
						Skin = ((dataRow["Skin"] == null) ? "" : dataRow["Skin"].ToString()),
						IsBanChat = (bool)dataRow["IsBanChat"],
						ReputeOffer = (int)dataRow["ReputeOffer"],
						ConsortiaRepute = (int)dataRow["ConsortiaRepute"],
						ConsortiaLevel = (int)dataRow["ConsortiaLevel"],
						StoreLevel = (int)dataRow["StoreLevel"],
						ShopLevel = (int)dataRow["ShopLevel"],
						SmithLevel = (int)dataRow["SmithLevel"],
						ConsortiaHonor = (int)dataRow["ConsortiaHonor"],
						RichesOffer = (int)dataRow["RichesOffer"],
						RichesRob = (int)dataRow["RichesRob"],
						DutyLevel = (int)dataRow["DutyLevel"],
						DutyName = ((dataRow["DutyName"] == null) ? "" : dataRow["DutyName"].ToString()),
						Right = (int)dataRow["Right"],
						ChairmanName = ((dataRow["ChairmanName"] == null) ? "" : dataRow["ChairmanName"].ToString()),
						Win = (int)dataRow["Win"],
						Total = (int)dataRow["Total"],
						Escape = (int)dataRow["Escape"],
						AddDayGP = (int)dataRow["AddDayGP"],
						AddDayOffer = (int)dataRow["AddDayOffer"],
						AddWeekGP = (int)dataRow["AddWeekGP"],
						AddWeekOffer = (int)dataRow["AddWeekOffer"],
						ConsortiaRiches = (int)dataRow["ConsortiaRiches"],
						CheckCount = (int)dataRow["CheckCount"],
						Nimbus = (int)dataRow["Nimbus"],
						GiftToken = (int)dataRow["GiftToken"],
						QuestSite = ((dataRow["QuestSite"] == null) ? new byte[200] : ((byte[])dataRow["QuestSite"])),
						PvePermission = ((dataRow["PvePermission"] == null) ? "" : dataRow["PvePermission"].ToString()),
						FightLabPermission = ((dataRow["FightLabPermission"] == DBNull.Value) ? "" : dataRow["FightLabPermission"].ToString()),
						FightPower = (int)dataRow["FightPower"],
						AchievementPoint = (int)dataRow["AchievementPoint"],
						Honor = (string)dataRow["Honor"],
						IsShowConsortia = (bool)dataRow["IsShowConsortia"],
						OptionOnOff = (int)dataRow["OptionOnOff"],
						badgeID = (int)dataRow["badgeID"],
						EliteScore = (int)dataRow["EliteScore"],
						apprenticeshipState = (int)dataRow["apprenticeshipState"],
						masterID = (int)dataRow["masterID"],
						graduatesCount = (int)dataRow["graduatesCount"],
						masterOrApprentices = ((dataRow["masterOrApprentices"] == DBNull.Value) ? "" : dataRow["masterOrApprentices"].ToString()),
						honourOfMaster = ((dataRow["honourOfMaster"] == DBNull.Value) ? "" : dataRow["honourOfMaster"].ToString()),
						IsMarried = (bool)dataRow["IsMarried"],
						typeVIP = Convert.ToByte(dataRow["typeVIP"]),
						VIPLevel = (int)dataRow["VIPLevel"],
						SpouseID = (int)dataRow["SpouseID"],
						SpouseName = ((dataRow["SpouseName"] == DBNull.Value) ? "" : dataRow["SpouseName"].ToString())
					});
				}
				resultValue = true;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00014418 File Offset: 0x00012618
		public string GetSingleRandomName(int sex)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				bool flag = sex > 1;
				if (flag)
				{
					sex = 1;
				}
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Sex", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = sex;
				this.db.GetReader(ref resultDataReader, "SP_GetSingle_RandomName", sqlParameters);
				bool flag2 = resultDataReader.Read();
				if (flag2)
				{
					return (resultDataReader["Name"] == null) ? "unknown" : resultDataReader["Name"].ToString();
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetSingleRandomName", exception);
				}
			}
			finally
			{
				bool flag3 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag3)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00014518 File Offset: 0x00012718
		public UserMatchInfo GetSingleUserMatchInfo(int UserID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = UserID;
				this.db.GetReader(ref resultDataReader, "SP_GetSingleUserMatchInfo", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new UserMatchInfo
					{
						ID = (int)resultDataReader["ID"],
						UserID = (int)resultDataReader["UserID"],
						dailyScore = (int)resultDataReader["dailyScore"],
						dailyWinCount = (int)resultDataReader["dailyWinCount"],
						dailyGameCount = (int)resultDataReader["dailyGameCount"],
						DailyLeagueFirst = (bool)resultDataReader["DailyLeagueFirst"],
						DailyLeagueLastScore = (int)resultDataReader["DailyLeagueLastScore"],
						weeklyScore = (int)resultDataReader["weeklyScore"],
						weeklyGameCount = (int)resultDataReader["weeklyGameCount"],
						weeklyRanking = (int)resultDataReader["weeklyRanking"],
						addDayPrestge = (int)resultDataReader["addDayPrestge"],
						totalPrestige = (int)resultDataReader["totalPrestige"],
						restCount = (int)resultDataReader["restCount"],
						leagueGrade = (int)resultDataReader["leagueGrade"],
						leagueItemsGet = (int)resultDataReader["leagueItemsGet"],
						WeeklyWinCount = (int)resultDataReader["WeeklyWinCount"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_GetSingleUserMatchInfo", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00014774 File Offset: 0x00012974
		public List<UserRankInfo> GetSingleUserRank(int UserID)
		{
			SqlDataReader reader = null;
			List<UserRankInfo> infos = new List<UserRankInfo>();
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				para[0].Value = UserID;
				this.db.GetReader(ref reader, "SP_GetSingleUserRank", para);
				while (reader.Read())
				{
					infos.Add(new UserRankInfo
					{
						ID = (int)reader["ID"],
						UserID = (int)reader["UserID"],
						Name = (string)reader["UserRank"],
						Attack = (int)reader["Attack"],
						Defence = (int)reader["Defence"],
						Luck = (int)reader["Luck"],
						Agility = (int)reader["Agility"],
						HP = (int)reader["HP"],
						Damage = (int)reader["Damage"],
						Guard = (int)reader["Guard"],
						BeginDate = (DateTime)reader["BeginDate"],
						Validate = (int)reader["Validate"],
						IsExit = (bool)reader["IsExit"],
						NewTitleID = (int)reader["NewTitleID"],
						EndDate = (DateTime)reader["EndDate"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_GetSingleUserRankInfo", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return infos;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000149CC File Offset: 0x00012BCC
		public UsersExtraInfo GetSingleUsersExtra(int UserID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = UserID;
				this.db.GetReader(ref resultDataReader, "SP_GetSingleUsersExtra", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new UsersExtraInfo
					{
						UserID = (int)resultDataReader["UserID"],
						LastTimeHotSpring = (DateTime)resultDataReader["LastTimeHotSpring"],
						LastFreeTimeHotSpring = (DateTime)resultDataReader["LastFreeTimeHotSpring"],
						MinHotSpring = (int)resultDataReader["MinHotSpring"],
						coupleBossEnterNum = (int)resultDataReader["coupleBossEnterNum"],
						coupleBossHurt = (int)resultDataReader["coupleBossHurt"],
						coupleBossBoxNum = (int)resultDataReader["coupleBossBoxNum"],
						LeftRoutteCount = ((resultDataReader["LeftRoutteCount"] == DBNull.Value) ? GameProperties.LeftRouterMaxDay : ((int)resultDataReader["LeftRoutteCount"])),
						LeftRoutteRate = ((resultDataReader["LeftRoutteRate"] == DBNull.Value) ? 0f : float.Parse(resultDataReader["LeftRoutteRate"].ToString())),
						FreeSendMailCount = (int)resultDataReader["FreeSendMailCount"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_GetSingleUsersExtra", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00014BD4 File Offset: 0x00012DD4
		public int GetXepHang(int UserID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = UserID;
				this.db.GetReader(ref resultDataReader, "SP_User_Repute", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					int fightpower = (int)resultDataReader["FightPower"];
					return Convert.ToInt32(resultDataReader["RowNumber"]);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_User_Repute", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return 0;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00014CCC File Offset: 0x00012ECC
		public AchievementData[] GetUserAchievement(int userID)
		{
			List<AchievementData> list = new List<AchievementData>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = userID;
				this.db.GetReader(ref resultDataReader, "SP_Get_User_AchievementData", sqlParameters);
				while (resultDataReader.Read())
				{
					AchievementData item = new AchievementData
					{
						UserID = (int)resultDataReader["UserID"],
						AchievementID = (int)resultDataReader["AchievementID"],
						IsComplete = (bool)resultDataReader["IsComplete"],
						CompletedDate = (DateTime)resultDataReader["CompletedDate"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00014E10 File Offset: 0x00013010
		public ItemInfo[] GetUserBagByType(int UserID, int bagType)
		{
			List<ItemInfo> items = new List<ItemInfo>();
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[2];
				para[0] = new SqlParameter("@UserID", SqlDbType.Int, 4);
				para[0].Value = UserID;
				para[1] = new SqlParameter("@BagType", bagType);
				this.db.GetReader(ref reader, "SP_Users_BagByType", para);
				while (reader.Read())
				{
					items.Add(this.InitItem(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return items.ToArray();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00014F04 File Offset: 0x00013104
		public List<ItemInfo> GetUserBeadEuqip(int UserID)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			SqlDataReader resultDataReader = null;
			List<ItemInfo> result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = UserID;
				this.db.GetReader(ref resultDataReader, "SP_Users_Bead_Equip", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitItem(resultDataReader));
				}
				result = list;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = list;
				}
				else
				{
					result = list;
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return result;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00014FE4 File Offset: 0x000131E4
		public BufferInfo[] GetUserBuffer(int userID)
		{
			List<BufferInfo> list = new List<BufferInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = userID;
				this.db.GetReader(ref resultDataReader, "SP_User_Buff_All", sqlParameters);
				while (resultDataReader.Read())
				{
					BufferInfo item = new BufferInfo
					{
						BeginDate = (DateTime)resultDataReader["BeginDate"],
						Data = ((resultDataReader["Data"] == null) ? "" : resultDataReader["Data"].ToString()),
						Type = (int)resultDataReader["Type"],
						UserID = (int)resultDataReader["UserID"],
						ValidDate = (int)resultDataReader["ValidDate"],
						Value = (int)resultDataReader["Value"],
						IsExist = (bool)resultDataReader["IsExist"],
						ValidCount = (int)resultDataReader["ValidCount"],
						TemplateID = (int)resultDataReader["TemplateID"],
						IsDirty = false
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000151D4 File Offset: 0x000133D4
		public UsersCardInfo GetUserCardByPlace(int Place)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Place", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = Place;
				this.db.GetReader(ref resultDataReader, "SP_Get_UserCard_By_Place", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitCard(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000152A4 File Offset: 0x000134A4
		public List<UsersCardInfo> GetUserCardEuqip(int UserID)
		{
			List<UsersCardInfo> list = new List<UsersCardInfo>();
			SqlDataReader resultDataReader = null;
			List<UsersCardInfo> result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = UserID;
				this.db.GetReader(ref resultDataReader, "SP_Users_Items_Card_Equip", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitCard(resultDataReader));
				}
				result = list;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = list;
				}
				else
				{
					result = list;
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return result;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00015384 File Offset: 0x00013584
		public UsersCardInfo[] GetUserCardSingles(int UserID)
		{
			List<UsersCardInfo> list = new List<UsersCardInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = UserID;
				this.db.GetReader(ref resultDataReader, "SP_Get_UserCard_By_ID", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitCard(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00015468 File Offset: 0x00013668
		public ConsortiaBufferInfo[] GetUserConsortiaBuffer(int ConsortiaID)
		{
			List<ConsortiaBufferInfo> list = new List<ConsortiaBufferInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ConsortiaID", SqlDbType.Int, 4)
				};
				SqlParameters[0].Value = ConsortiaID;
				this.db.GetReader(ref ResultDataReader, "SP_User_Consortia_Buff_All", SqlParameters);
				while (ResultDataReader.Read())
				{
					list.Add(new ConsortiaBufferInfo
					{
						ConsortiaID = (int)ResultDataReader["ConsortiaID"],
						BufferID = (int)ResultDataReader["BufferID"],
						IsOpen = (bool)ResultDataReader["IsOpen"],
						BeginDate = (DateTime)ResultDataReader["BeginDate"],
						ValidDate = (int)ResultDataReader["ValidDate"],
						Type = (int)ResultDataReader["Type"],
						Value = (int)ResultDataReader["Value"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init SP_User_Consortia_Buff_All", ex);
				}
			}
			finally
			{
				bool flag = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag)
				{
					ResultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0001560C File Offset: 0x0001380C
		public ConsortiaBufferInfo[] GetUserConsortiaBufferLess(int ConsortiaID, int LessID)
		{
			List<ConsortiaBufferInfo> list = new List<ConsortiaBufferInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] array = new SqlParameter[2];
				array[0] = new SqlParameter("@ConsortiaID", SqlDbType.Int, 4);
				SqlParameter[] sqlParameters = array;
				sqlParameters[0].Value = ConsortiaID;
				sqlParameters[1] = new SqlParameter("@LessID", LessID);
				this.db.GetReader(ref ResultDataReader, "SP_User_Consortia_Buff_All", sqlParameters);
				while (ResultDataReader.Read())
				{
					list.Add(new ConsortiaBufferInfo
					{
						ConsortiaID = (int)ResultDataReader["ConsortiaID"],
						BufferID = (int)ResultDataReader["BufferID"],
						IsOpen = (bool)ResultDataReader["IsOpen"],
						BeginDate = (DateTime)ResultDataReader["BeginDate"],
						ValidDate = (int)ResultDataReader["ValidDate"],
						Type = (int)ResultDataReader["Type"],
						Value = (int)ResultDataReader["Value"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init SP_User_Consortia_Buff_AllL", ex);
				}
			}
			finally
			{
				bool flag = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag)
				{
					ResultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000157C0 File Offset: 0x000139C0
		public ConsortiaBufferInfo GetUserConsortiaBufferSingle(int ID, int conid)
		{
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4),
					new SqlParameter("@ConsortiaID", SqlDbType.Int, 4)
				};
				SqlParameters[0].Value = ID;
				SqlParameters[1].Value = conid;
				this.db.GetReader(ref ResultDataReader, "SP_User_Consortia_Buff_Single", SqlParameters);
				bool flag = ResultDataReader.Read();
				if (flag)
				{
					return new ConsortiaBufferInfo
					{
						ConsortiaID = (int)ResultDataReader["ConsortiaID"],
						BufferID = (int)ResultDataReader["BufferID"],
						IsOpen = (bool)ResultDataReader["IsOpen"],
						BeginDate = (DateTime)ResultDataReader["BeginDate"],
						ValidDate = (int)ResultDataReader["ValidDate"],
						Type = (int)ResultDataReader["Type"],
						Value = (int)ResultDataReader["Value"]
					};
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init SP_User_Consortia_Buff_Single", ex);
				}
			}
			finally
			{
				bool flag2 = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag2)
				{
					ResultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00015968 File Offset: 0x00013B68
		public List<ItemInfo> GetUserEquip(int UserID)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			SqlDataReader resultDataReader = null;
			List<ItemInfo> result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = UserID;
				this.db.GetReader(ref resultDataReader, "SP_Users_Items_Equip", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitItem(resultDataReader));
				}
				result = list;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = list;
				}
				else
				{
					result = list;
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return result;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00015A48 File Offset: 0x00013C48
		public List<ItemInfo> GetUserEuqipByNick(string Nick)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			SqlDataReader resultDataReader = null;
			List<ItemInfo> result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@NickName", SqlDbType.NVarChar, 200)
				};
				sqlParameters[0].Value = Nick;
				this.db.GetReader(ref resultDataReader, "SP_Users_Items_Equip_By_Nick", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitItem(resultDataReader));
				}
				result = list;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = list;
				}
				else
				{
					result = list;
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return result;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00015B28 File Offset: 0x00013D28
		public EventRewardProcessInfo[] GetUserEventProcess(int userID)
		{
			SqlDataReader resultDataReader = null;
			List<EventRewardProcessInfo> list = new List<EventRewardProcessInfo>();
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = userID;
				this.db.GetReader(ref resultDataReader, "SP_Get_User_EventProcess", sqlParameters);
				while (resultDataReader.Read())
				{
					EventRewardProcessInfo item = new EventRewardProcessInfo
					{
						UserID = (int)resultDataReader["UserID"],
						ActiveType = (int)resultDataReader["ActiveType"],
						Conditions = (int)resultDataReader["Conditions"],
						AwardGot = (int)resultDataReader["AwardGot"],
						IsReset = (bool)resultDataReader["IsReset"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00015CA0 File Offset: 0x00013EA0
		public UserInfo GetUserInfo(int UserId)
		{
			SqlDataReader resultDataReader = null;
			UserInfo info = new UserInfo
			{
				UserID = UserId
			};
			UserInfo result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserId)
				};
				this.db.GetReader(ref resultDataReader, "SP_Get_User_Info", sqlParameters);
				while (resultDataReader.Read())
				{
					info.UserID = int.Parse(resultDataReader["UserID"].ToString());
					info.UserEmail = ((resultDataReader["UserEmail"] == null) ? "" : resultDataReader["UserEmail"].ToString());
					info.UserPhone = ((resultDataReader["UserPhone"] == null) ? "" : resultDataReader["UserPhone"].ToString());
					info.UserOther1 = ((resultDataReader["UserOther1"] == null) ? "" : resultDataReader["UserOther1"].ToString());
					info.UserOther2 = ((resultDataReader["UserOther2"] == null) ? "" : resultDataReader["UserOther2"].ToString());
					info.UserOther3 = ((resultDataReader["UserOther3"] == null) ? "" : resultDataReader["UserOther3"].ToString());
				}
				result = info;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = info;
				}
				else
				{
					result = info;
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return result;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00015E80 File Offset: 0x00014080
		public ItemInfo[] GetUserItem(int UserID)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = UserID;
				this.db.GetReader(ref resultDataReader, "SP_Users_Items_All", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitItem(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00015F64 File Offset: 0x00014164
		public ItemInfo GetUserItemSingle(int itemID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = itemID;
				this.db.GetReader(ref resultDataReader, "SP_Users_Items_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitItem(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00016034 File Offset: 0x00014234
		public LevelInfo GetUserLevelSingle(int Grade)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Grade", Grade)
				};
				this.db.GetReader(ref resultDataReader, "SP_Get_Level_By_Grade", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new LevelInfo
					{
						Grade = (int)resultDataReader["Grade"],
						GP = (int)resultDataReader["GP"],
						Blood = (int)resultDataReader["Blood"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetLevelInfoSingle", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0001613C File Offset: 0x0001433C
		public PlayerLimitInfo GetUserLimitByUserName(string userName)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", SqlDbType.NVarChar, 200)
				};
				sqlParameters[0].Value = userName;
				this.db.GetReader(ref resultDataReader, "SP_Users_LimitByUserName", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new PlayerLimitInfo
					{
						ID = (int)resultDataReader["UserID"],
						NickName = (string)resultDataReader["NickName"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00016238 File Offset: 0x00014438
		public PlayerInfo[] GetUserLoginList(string userName)
		{
			List<PlayerInfo> list = new List<PlayerInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", SqlDbType.NVarChar, 200)
				};
				sqlParameters[0].Value = userName;
				this.db.GetReader(ref resultDataReader, "SP_Users_LoginList", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitPlayerInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0001631C File Offset: 0x0001451C
		public QuestDataInfo[] GetUserQuest(int userID)
		{
			List<QuestDataInfo> infos = new List<QuestDataInfo>();
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				para[0].Value = userID;
				this.db.GetReader(ref reader, "SP_QuestData_All", para);
				while (reader.Read())
				{
					infos.Add(new QuestDataInfo
					{
						CompletedDate = (DateTime)reader["CompletedDate"],
						IsComplete = (bool)reader["IsComplete"],
						Condition1 = (int)reader["Condition1"],
						Condition2 = (int)reader["Condition2"],
						Condition3 = (int)reader["Condition3"],
						Condition4 = (int)reader["Condition4"],
						QuestID = (int)reader["QuestID"],
						UserID = (int)reader["UserId"],
						IsExist = (bool)reader["IsExist"],
						RandDobule = (int)reader["RandDobule"],
						RepeatFinish = (int)reader["RepeatFinish"],
						IsDirty = false
					});
				}
			}
			catch (Exception e)
			{
				BaseBussiness.log.Error("Init", e);
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return infos.ToArray();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00016510 File Offset: 0x00014710
		public QuestDataInfo GetUserQuestSiger(int userID, int QuestID)
		{
			new QuestDataInfo();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int),
					new SqlParameter("@QuestID", SqlDbType.Int)
				};
				sqlParameters[0].Value = userID;
				sqlParameters[1].Value = QuestID;
				this.db.GetReader(ref resultDataReader, "SP_QuestData_One", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new QuestDataInfo
					{
						CompletedDate = (DateTime)resultDataReader["CompletedDate"],
						IsComplete = (bool)resultDataReader["IsComplete"],
						Condition1 = (int)resultDataReader["Condition1"],
						Condition2 = (int)resultDataReader["Condition2"],
						Condition3 = (int)resultDataReader["Condition3"],
						Condition4 = (int)resultDataReader["Condition4"],
						QuestID = (int)resultDataReader["QuestID"],
						UserID = (int)resultDataReader["UserId"],
						IsExist = (bool)resultDataReader["IsExist"],
						RandDobule = (int)resultDataReader["RandDobule"],
						RepeatFinish = (int)resultDataReader["RepeatFinish"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00016718 File Offset: 0x00014918
		public PlayerInfo GetUserSingleByNickName(string nickName)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@NickName", SqlDbType.NVarChar, 200)
				};
				sqlParameters[0].Value = nickName;
				this.db.GetReader(ref resultDataReader, "SP_Users_SingleByNickName", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitPlayerInfo(resultDataReader);
				}
			}
			catch
			{
				throw new Exception();
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000167C8 File Offset: 0x000149C8
		public PlayerInfo GetUserSingleByUserID(int UserID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = UserID;
				this.db.GetReader(ref resultDataReader, "SP_Users_SingleByUserID", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitPlayerInfo(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00016898 File Offset: 0x00014A98
		public PlayerInfo GetUserSingleByUserName(string userName)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", SqlDbType.NVarChar, 200)
				};
				sqlParameters[0].Value = userName;
				this.db.GetReader(ref resultDataReader, "SP_Users_SingleByUserName", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitPlayerInfo(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00016968 File Offset: 0x00014B68
		public TexpInfo GetUserTexpInfoSingle(int ID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", ID)
				};
				this.db.GetReader(ref resultDataReader, "SP_Get_UserTexp_By_ID", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new TexpInfo
					{
						UserID = (int)resultDataReader["UserID"],
						attTexpExp = (int)resultDataReader["attTexpExp"],
						defTexpExp = (int)resultDataReader["defTexpExp"],
						hpTexpExp = (int)resultDataReader["hpTexpExp"],
						lukTexpExp = (int)resultDataReader["lukTexpExp"],
						spdTexpExp = (int)resultDataReader["spdTexpExp"],
						texpCount = (int)resultDataReader["texpCount"],
						texpTaskCount = (int)resultDataReader["texpTaskCount"],
						texpTaskDate = (DateTime)resultDataReader["texpTaskDate"]
					};
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetTexpInfoSingle", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00016B18 File Offset: 0x00014D18
		public UsersCardInfo[] GetSingleUserCard(int UserID)
		{
			SqlDataReader ResultDataReader = null;
			List<UsersCardInfo> userCardInfoList = new List<UsersCardInfo>();
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				SqlParameters[0].Value = UserID;
				this.db.GetReader(ref ResultDataReader, "SP_GetSingleUserCard", SqlParameters);
				while (ResultDataReader.Read())
				{
					UsersCardInfo userCardInfo = this.InitCard(ResultDataReader);
					userCardInfoList.Add(userCardInfo);
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetSingleUserCard", ex);
				}
			}
			finally
			{
				bool flag = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag)
				{
					ResultDataReader.Close();
				}
			}
			return userCardInfoList.ToArray();
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00016C00 File Offset: 0x00014E00
		public int GetVip(string UserName)
		{
			int num = 0;
			int result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", UserName),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[1].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_GetVip", sqlParameters);
				num = (int)sqlParameters[1].Value;
				result = num;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = num;
				}
				else
				{
					result = num;
				}
			}
			return result;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00016CA0 File Offset: 0x00014EA0
		public AuctionInfo InitAuctionInfo(SqlDataReader reader)
		{
			return new AuctionInfo
			{
				AuctioneerID = (int)reader["AuctioneerID"],
				AuctioneerName = ((reader["AuctioneerName"] == null) ? "" : reader["AuctioneerName"].ToString()),
				AuctionID = (int)reader["AuctionID"],
				BeginDate = (DateTime)reader["BeginDate"],
				BuyerID = (int)reader["BuyerID"],
				BuyerName = ((reader["BuyerName"] == null) ? "" : reader["BuyerName"].ToString()),
				IsExist = (bool)reader["IsExist"],
				ItemID = (int)reader["ItemID"],
				Mouthful = (int)reader["Mouthful"],
				PayType = (int)reader["PayType"],
				Price = (int)reader["Price"],
				Rise = (int)reader["Rise"],
				ValidDate = (int)reader["ValidDate"],
				Name = reader["Name"].ToString(),
				Category = (int)reader["Category"],
				goodsCount = (int)reader["goodsCount"]
			};
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00016E50 File Offset: 0x00015050
		private UsersCardInfo InitCard(SqlDataReader sqlDataReader_0)
		{
			return new UsersCardInfo
			{
				CardID = (int)sqlDataReader_0["CardID"],
				UserID = (int)sqlDataReader_0["UserID"],
				TemplateID = (int)sqlDataReader_0["TemplateID"],
				Place = (int)sqlDataReader_0["Place"],
				Count = (int)sqlDataReader_0["Count"],
				Attack = (int)sqlDataReader_0["Attack"],
				Defence = (int)sqlDataReader_0["Defence"],
				Agility = (int)sqlDataReader_0["Agility"],
				Luck = (int)sqlDataReader_0["Luck"],
				AttackReset = (int)sqlDataReader_0["AttackReset"],
				DefenceReset = (int)sqlDataReader_0["DefenceReset"],
				AgilityReset = (int)sqlDataReader_0["AgilityReset"],
				LuckReset = (int)sqlDataReader_0["LuckReset"],
				Guard = (int)sqlDataReader_0["Guard"],
				Damage = (int)sqlDataReader_0["Damage"],
				Level = (int)sqlDataReader_0["Level"],
				CardGP = (int)sqlDataReader_0["CardGP"],
				isFirstGet = (bool)sqlDataReader_0["isFirstGet"]
			};
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00017008 File Offset: 0x00015208
		public CardGrooveUpdateInfo InitCardGrooveUpdate(SqlDataReader reader)
		{
			return new CardGrooveUpdateInfo
			{
				ID = (int)reader["ID"],
				Attack = (int)reader["Attack"],
				Defend = (int)reader["Defend"],
				Agility = (int)reader["Agility"],
				Lucky = (int)reader["Lucky"],
				Damage = (int)reader["Damage"],
				Guard = (int)reader["Guard"],
				Level = (int)reader["Level"],
				Type = (int)reader["Type"],
				Exp = (int)reader["Exp"]
			};
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00017108 File Offset: 0x00015308
		public CardTemplateInfo InitCardTemplate(SqlDataReader reader)
		{
			return new CardTemplateInfo
			{
				ID = (int)reader["ID"],
				CardID = (int)reader["CardID"],
				Count = (int)reader["Count"],
				probability = (int)reader["probability"],
				AttackRate = (int)reader["Attack"],
				AddAttack = (int)reader["AddAttack"],
				DefendRate = (int)reader["DefendRate"],
				AddDefend = (int)reader["AddDefend"],
				AgilityRate = (int)reader["AgilityRate"],
				AddAgility = (int)reader["AddAgility"],
				LuckyRate = (int)reader["LuckyRate"],
				AddLucky = (int)reader["AddLucky"],
				DamageRate = (int)reader["DamageRate"],
				AddDamage = (int)reader["AddDamage"],
				GuardRate = (int)reader["GuardRate"],
				AddGuard = (int)reader["AddGuard"]
			};
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00017290 File Offset: 0x00015490
		public ConsortiaUserInfo InitConsortiaUserInfo(SqlDataReader dr)
		{
			ConsortiaUserInfo consortiaUserInfo = new ConsortiaUserInfo();
			consortiaUserInfo.ID = (int)dr["ID"];
			consortiaUserInfo.ConsortiaID = (int)dr["ConsortiaID"];
			consortiaUserInfo.DutyID = (int)dr["DutyID"];
			consortiaUserInfo.DutyName = dr["DutyName"].ToString();
			consortiaUserInfo.IsExist = (bool)dr["IsExist"];
			consortiaUserInfo.RatifierID = (int)dr["RatifierID"];
			consortiaUserInfo.RatifierName = dr["RatifierName"].ToString();
			consortiaUserInfo.Remark = dr["Remark"].ToString();
			consortiaUserInfo.UserID = (int)dr["UserID"];
			consortiaUserInfo.UserName = dr["UserName"].ToString();
			consortiaUserInfo.Grade = (int)dr["Grade"];
			consortiaUserInfo.GP = (int)dr["GP"];
			consortiaUserInfo.Repute = (int)dr["Repute"];
			consortiaUserInfo.State = (int)dr["State"];
			consortiaUserInfo.Right = (int)dr["Right"];
			consortiaUserInfo.Offer = (int)dr["Offer"];
			consortiaUserInfo.Colors = dr["Colors"].ToString();
			consortiaUserInfo.Style = dr["Style"].ToString();
			consortiaUserInfo.Hide = (int)dr["Hide"];
			consortiaUserInfo.Skin = ((dr["Skin"] == null) ? "" : consortiaUserInfo.Skin);
			consortiaUserInfo.Level = (int)dr["Level"];
			consortiaUserInfo.LastDate = (DateTime)dr["LastDate"];
			consortiaUserInfo.Sex = (bool)dr["Sex"];
			consortiaUserInfo.IsBanChat = (bool)dr["IsBanChat"];
			consortiaUserInfo.Win = (int)dr["Win"];
			consortiaUserInfo.Total = (int)dr["Total"];
			consortiaUserInfo.Escape = (int)dr["Escape"];
			consortiaUserInfo.RichesOffer = (int)dr["RichesOffer"];
			consortiaUserInfo.RichesRob = (int)dr["RichesRob"];
			consortiaUserInfo.LoginName = ((dr["LoginName"] == null) ? "" : dr["LoginName"].ToString());
			consortiaUserInfo.Nimbus = (int)dr["Nimbus"];
			consortiaUserInfo.FightPower = (int)dr["FightPower"];
			consortiaUserInfo.typeVIP = Convert.ToByte(dr["typeVIP"]);
			consortiaUserInfo.VIPLevel = (int)dr["VIPLevel"];
			return consortiaUserInfo;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000175D8 File Offset: 0x000157D8
		public ItemInfo InitItem(SqlDataReader reader)
		{
			ItemInfo item = new ItemInfo(ItemMgr.FindItemTemplate((int)reader["TemplateID"]));
			item.AgilityCompose = (int)reader["AgilityCompose"];
			item.AttackCompose = (int)reader["AttackCompose"];
			item.Color = reader["Color"].ToString();
			item.Count = (int)reader["Count"];
			item.DefendCompose = (int)reader["DefendCompose"];
			item.ItemID = (int)reader["ItemID"];
			item.LuckCompose = (int)reader["LuckCompose"];
			item.Place = (int)reader["Place"];
			item.StrengthenLevel = (int)reader["StrengthenLevel"];
			item.TemplateID = (int)reader["TemplateID"];
			item.UserID = (int)reader["UserID"];
			item.ValidDate = (int)reader["ValidDate"];
			item.IsDirty = false;
			item.IsExist = (bool)reader["IsExist"];
			item.IsBinds = (bool)reader["IsBinds"];
			item.IsUsed = (bool)reader["IsUsed"];
			item.BeginDate = (DateTime)reader["BeginDate"];
			item.IsJudge = (bool)reader["IsJudge"];
			item.BagType = (int)reader["BagType"];
			item.Skin = reader["Skin"].ToString();
			item.RemoveDate = (DateTime)reader["RemoveDate"];
			item.RemoveType = (int)reader["RemoveType"];
			item.Hole1 = (int)reader["Hole1"];
			item.Hole2 = (int)reader["Hole2"];
			item.Hole3 = (int)reader["Hole3"];
			item.Hole4 = (int)reader["Hole4"];
			item.Hole5 = (int)reader["Hole5"];
			item.Hole6 = (int)reader["Hole6"];
			item.Hole5Level = (int)reader["Hole5Level"];
			item.Hole5Exp = (int)reader["Hole5Exp"];
			item.Hole6Level = (int)reader["Hole6Level"];
			item.Hole6Exp = (int)reader["Hole6Exp"];
			item.StrengthenTimes = (int)reader["StrengthenTimes"];
			item.goldBeginTime = (DateTime)reader["goldBeginTime"];
			item.goldValidDate = (int)reader["goldValidDate"];
			item.StrengthenExp = (int)reader["StrengthenExp"];
			item.Blood = (int)reader["Blood"];
			item.latentEnergyCurStr = (string)reader["latentEnergyCurStr"];
			item.latentEnergyNewStr = (string)reader["latentEnergyNewStr"];
			item.latentEnergyEndTime = (DateTime)reader["latentEnergyEndTime"];
			item.GoldEquip = ItemMgr.FindGoldItemTemplate(item.TemplateID, item.isGold);
			item.curExp = (int)reader["curExp"];
			item.cellLocked = (bool)reader["cellLocked"];
			item.IsDirty = false;
			return item;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000179E0 File Offset: 0x00015BE0
		public MailInfo InitMail(SqlDataReader reader)
		{
			return new MailInfo
			{
				Annex1 = reader["Annex1"].ToString(),
				Annex2 = reader["Annex2"].ToString(),
				Content = reader["Content"].ToString(),
				Gold = (int)reader["Gold"],
				ID = (int)reader["ID"],
				IsExist = (bool)reader["IsExist"],
				Money = (int)reader["Money"],
				GiftToken = (int)reader["GiftToken"],
				Receiver = reader["Receiver"].ToString(),
				ReceiverID = (int)reader["ReceiverID"],
				Sender = reader["Sender"].ToString(),
				SenderID = (int)reader["SenderID"],
				Title = reader["Title"].ToString(),
				Type = (int)reader["Type"],
				ValidDate = (int)reader["ValidDate"],
				IsRead = (bool)reader["IsRead"],
				SendTime = (DateTime)reader["SendTime"],
				Annex1Name = ((reader["Annex1Name"] == null) ? "" : reader["Annex1Name"].ToString()),
				Annex2Name = ((reader["Annex2Name"] == null) ? "" : reader["Annex2Name"].ToString()),
				Annex3 = reader["Annex3"].ToString(),
				Annex4 = reader["Annex4"].ToString(),
				Annex5 = reader["Annex5"].ToString(),
				Annex3Name = ((reader["Annex3Name"] == null) ? "" : reader["Annex3Name"].ToString()),
				Annex4Name = ((reader["Annex4Name"] == null) ? "" : reader["Annex4Name"].ToString()),
				Annex5Name = ((reader["Annex5Name"] == null) ? "" : reader["Annex5Name"].ToString()),
				AnnexRemark = ((reader["AnnexRemark"] == null) ? "" : reader["AnnexRemark"].ToString())
			};
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00017CC8 File Offset: 0x00015EC8
		public PlayerInfo InitPlayerInfo(SqlDataReader reader)
		{
			PlayerInfo player = new PlayerInfo
			{
				Password = (string)reader["Password"],
				IsConsortia = (bool)reader["IsConsortia"],
				Agility = (int)reader["Agility"],
				Attack = (int)reader["Attack"],
				hp = (int)reader["hp"],
				Colors = ((reader["Colors"] == null) ? "" : reader["Colors"].ToString()),
				ConsortiaID = (int)reader["ConsortiaID"],
				Defence = (int)reader["Defence"],
				Gold = (int)reader["Gold"],
				GP = (int)reader["GP"],
				Grade = (int)reader["Grade"],
				ID = (int)reader["UserID"],
				Luck = (int)reader["Luck"],
				Money = (int)reader["Money"],
				NickName = (((string)reader["NickName"] == null) ? "" : ((string)reader["NickName"])),
				Sex = (bool)reader["Sex"],
				State = (int)reader["State"],
				Style = ((reader["Style"] == null) ? "" : reader["Style"].ToString()),
				Hide = (int)reader["Hide"],
				Repute = (int)reader["Repute"],
				UserName = ((reader["UserName"] == null) ? "" : reader["UserName"].ToString()),
				ConsortiaName = ((reader["ConsortiaName"] == null) ? "" : reader["ConsortiaName"].ToString()),
				Offer = (int)reader["Offer"],
				Win = (int)reader["Win"],
				Total = (int)reader["Total"],
				Escape = (int)reader["Escape"],
				Skin = ((reader["Skin"] == null) ? "" : reader["Skin"].ToString()),
				IsBanChat = (bool)reader["IsBanChat"],
				ReputeOffer = (int)reader["ReputeOffer"],
				ConsortiaRepute = (int)reader["ConsortiaRepute"],
				ConsortiaLevel = (int)reader["ConsortiaLevel"],
				StoreLevel = (int)reader["StoreLevel"],
				ShopLevel = (int)reader["ShopLevel"],
				SmithLevel = (int)reader["SmithLevel"],
				ConsortiaHonor = (int)reader["ConsortiaHonor"],
				RichesOffer = (int)reader["RichesOffer"],
				RichesRob = (int)reader["RichesRob"],
				AntiAddiction = (int)reader["AntiAddiction"],
				DutyLevel = (int)reader["DutyLevel"],
				DutyName = ((reader["DutyName"] == null) ? "" : reader["DutyName"].ToString()),
				Right = (int)reader["Right"],
				ChairmanName = ((reader["ChairmanName"] == null) ? "" : reader["ChairmanName"].ToString()),
				AddDayGP = (int)reader["AddDayGP"],
				AddDayOffer = (int)reader["AddDayOffer"],
				AddWeekGP = (int)reader["AddWeekGP"],
				AddWeekOffer = (int)reader["AddWeekOffer"],
				ConsortiaRiches = (int)reader["ConsortiaRiches"],
				CheckCount = (int)reader["CheckCount"],
				IsMarried = (bool)reader["IsMarried"],
				SpouseID = (int)reader["SpouseID"],
				SpouseName = ((reader["SpouseName"] == null) ? "" : reader["SpouseName"].ToString()),
				MarryInfoID = (int)reader["MarryInfoID"],
				IsCreatedMarryRoom = (bool)reader["IsCreatedMarryRoom"],
				DayLoginCount = (int)reader["DayLoginCount"],
				PasswordTwo = ((reader["PasswordTwo"] == null) ? "" : reader["PasswordTwo"].ToString()),
				SelfMarryRoomID = (int)reader["SelfMarryRoomID"],
				IsGotRing = (bool)reader["IsGotRing"],
				Rename = (bool)reader["Rename"],
				ConsortiaRename = (bool)reader["ConsortiaRename"],
				IsDirty = false,
				IsFirst = (int)reader["IsFirst"],
				Nimbus = (int)reader["Nimbus"],
				LastAward = (DateTime)reader["LastAward"],
				GiftToken = (int)reader["GiftToken"],
				QuestSite = ((reader["QuestSite"] == null) ? new byte[200] : ((byte[])reader["QuestSite"])),
				PvePermission = ((reader["PvePermission"] == null) ? "" : reader["PvePermission"].ToString()),
				FightPower = (int)reader["FightPower"],
				PasswordQuest1 = ((reader["PasswordQuestion1"] == null) ? "" : reader["PasswordQuestion1"].ToString()),
				PasswordQuest2 = ((reader["PasswordQuestion2"] == null) ? "" : reader["PasswordQuestion2"].ToString())
			};
			PlayerInfo info2 = player;
			bool flag = (DateTime)reader["LastFindDate"] != DateTime.Today.Date;
			if (flag)
			{
				info2.FailedPasswordAttemptCount = 5;
			}
			else
			{
				info2.FailedPasswordAttemptCount = (int)reader["FailedPasswordAttemptCount"];
			}
			player.AnswerSite = (int)reader["AnswerSite"];
			player.medal = (int)reader["Medal"];
			player.ChatCount = (int)reader["ChatCount"];
			player.SpaPubGoldRoomLimit = (int)reader["SpaPubGoldRoomLimit"];
			player.LastSpaDate = (DateTime)reader["LastSpaDate"];
			player.FightLabPermission = (string)reader["FightLabPermission"];
			player.SpaPubMoneyRoomLimit = (int)reader["SpaPubMoneyRoomLimit"];
			player.IsInSpaPubGoldToday = (bool)reader["IsInSpaPubGoldToday"];
			player.IsInSpaPubMoneyToday = (bool)reader["IsInSpaPubMoneyToday"];
			player.AchievementPoint = (int)reader["AchievementPoint"];
			player.LastWeekly = (DateTime)reader["LastWeekly"];
			player.LastWeeklyVersion = (int)reader["LastWeeklyVersion"];
			player.badgeID = (int)reader["BadgeID"];
			player.typeVIP = Convert.ToByte(reader["typeVIP"]);
			player.VIPLevel = (int)reader["VIPLevel"];
			player.VIPExp = (int)reader["VIPExp"];
			player.VIPExpireDay = (DateTime)reader["VIPExpireDay"];
			player.VIPNextLevelDaysNeeded = (int)reader["VIPNextLevelDaysNeeded"];
			player.LastVIPPackTime = (DateTime)reader["LastVIPPackTime"];
			player.CanTakeVipReward = (bool)reader["CanTakeVipReward"];
			player.WeaklessGuildProgressStr = (string)reader["WeaklessGuildProgressStr"];
			player.IsOldPlayer = (bool)reader["IsOldPlayer"];
			player.LastDate = (DateTime)reader["LastDate"];
			player.VIPLastDate = (DateTime)reader["VIPLastDate"];
			player.Score = (int)reader["Score"];
			player.OptionOnOff = (int)reader["OptionOnOff"];
			player.isOldPlayerHasValidEquitAtLogin = (bool)reader["isOldPlayerHasValidEquitAtLogin"];
			player.badLuckNumber = (int)reader["badLuckNumber"];
			player.OnlineTime = (int)reader["OnlineTime"];
			player.luckyNum = (int)reader["luckyNum"];
			player.lastLuckyNumDate = (DateTime)reader["lastLuckyNumDate"];
			player.lastLuckNum = (int)reader["lastLuckNum"];
			player.IsShowConsortia = (bool)reader["IsShowConsortia"];
			player.NewDay = (DateTime)reader["NewDay"];
			player.Honor = (string)reader["Honor"];
			player.BoxGetDate = (DateTime)reader["BoxGetDate"];
			player.AlreadyGetBox = (int)reader["AlreadyGetBox"];
			player.BoxProgression = (int)reader["BoxProgression"];
			player.GetBoxLevel = (int)reader["GetBoxLevel"];
			player.IsRecharged = (bool)reader["IsRecharged"];
			player.IsGetAward = (bool)reader["IsGetAward"];
			player.apprenticeshipState = (int)reader["apprenticeshipState"];
			player.masterID = (int)reader["masterID"];
			player.masterOrApprentices = ((reader["masterOrApprentices"] == DBNull.Value) ? "" : ((string)reader["masterOrApprentices"]));
			player.graduatesCount = (int)reader["graduatesCount"];
			player.honourOfMaster = ((reader["honourOfMaster"] == DBNull.Value) ? "" : ((string)reader["honourOfMaster"]));
			player.freezesDate = ((reader["freezesDate"] == DBNull.Value) ? DateTime.Now : ((DateTime)reader["freezesDate"]));
			player.charmGP = ((reader["charmGP"] != DBNull.Value) ? ((int)reader["charmGP"]) : 0);
			player.evolutionGrade = (int)reader["evolutionGrade"];
			player.evolutionExp = (int)reader["evolutionExp"];
			player.hardCurrency = (int)reader["hardCurrency"];
			player.EliteScore = (int)reader["EliteScore"];
			player.ShopFinallyGottenTime = ((reader["ShopFinallyGottenTime"] == DBNull.Value) ? DateTime.Now.AddDays(-1.0) : ((DateTime)reader["ShopFinallyGottenTime"]));
			player.MoneyLock = ((reader["MoneyLock"] != DBNull.Value) ? ((int)reader["MoneyLock"]) : 0);
			player.LastGetEgg = (DateTime)reader["LastGetEgg"];
			player.IsFistGetPet = (bool)reader["IsFistGetPet"];
			player.LastRefreshPet = (DateTime)reader["LastRefreshPet"];
			player.petScore = (int)reader["petScore"];
			player.accumulativeLoginDays = (int)reader["accumulativeLoginDays"];
			player.accumulativeAwardDays = (int)reader["accumulativeAwardDays"];
			player.honorId = (int)reader["honorId"];
			player.damageScores = (int)reader["damageScores"];
			player.totemId = (int)reader["totemId"];
			player.myHonor = (int)reader["myHonor"];
			player.MaxBuyHonor = (int)reader["MaxBuyHonor"];
			player.necklaceExp = (int)reader["necklaceExp"];
			player.necklaceExpAdd = (int)reader["necklaceExpAdd"];
			player.GhostEquipList = ((reader["GhostEquipList"] == DBNull.Value) ? "" : ((string)reader["GhostEquipList"]));
			player.fineSuitExp = (int)reader["fineSuitExp"];
			return player;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00018B68 File Offset: 0x00016D68
		public bool InsertMarryRoomInfo(MarryRoomInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[20];
				array[0] = new SqlParameter("@ID", info.ID);
				SqlParameter[] sqlParameters = array;
				sqlParameters[0].Direction = ParameterDirection.InputOutput;
				sqlParameters[1] = new SqlParameter("@Name", info.Name);
				sqlParameters[2] = new SqlParameter("@PlayerID", info.PlayerID);
				sqlParameters[3] = new SqlParameter("@PlayerName", info.PlayerName);
				sqlParameters[4] = new SqlParameter("@GroomID", info.GroomID);
				sqlParameters[5] = new SqlParameter("@GroomName", info.GroomName);
				sqlParameters[6] = new SqlParameter("@BrideID", info.BrideID);
				sqlParameters[7] = new SqlParameter("@BrideName", info.BrideName);
				sqlParameters[8] = new SqlParameter("@Pwd", info.Pwd);
				sqlParameters[9] = new SqlParameter("@AvailTime", info.AvailTime);
				sqlParameters[10] = new SqlParameter("@MaxCount", info.MaxCount);
				sqlParameters[11] = new SqlParameter("@GuestInvite", info.GuestInvite);
				sqlParameters[12] = new SqlParameter("@MapIndex", info.MapIndex);
				sqlParameters[13] = new SqlParameter("@BeginTime", info.BeginTime);
				sqlParameters[14] = new SqlParameter("@BreakTime", info.BreakTime);
				sqlParameters[15] = new SqlParameter("@RoomIntroduction", info.RoomIntroduction);
				sqlParameters[16] = new SqlParameter("@ServerID", info.ServerID);
				sqlParameters[17] = new SqlParameter("@IsHymeneal", info.IsHymeneal);
				sqlParameters[18] = new SqlParameter("@IsGunsaluteUsed", info.IsGunsaluteUsed);
				sqlParameters[19] = new SqlParameter("@Result", SqlDbType.Int);
				sqlParameters[19].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Insert_Marry_Room_Info", sqlParameters);
				flag = ((int)sqlParameters[19].Value == 0);
				bool flag2 = flag;
				if (flag2)
				{
					info.ID = (int)sqlParameters[0].Value;
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("InsertMarryRoomInfo", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00018DEC File Offset: 0x00016FEC
		public bool InsertPlayerMarryApply(MarryApplyInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@ApplyUserID", info.ApplyUserID),
					new SqlParameter("@ApplyUserName", info.ApplyUserName),
					new SqlParameter("@ApplyType", info.ApplyType),
					new SqlParameter("@ApplyResult", info.ApplyResult),
					new SqlParameter("@LoveProclamation", info.LoveProclamation),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[6].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Insert_Marry_Apply", sqlParameters);
				flag = ((int)sqlParameters[6].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("InsertPlayerMarryApply", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00018F08 File Offset: 0x00017108
		public bool InsertUserTexpInfo(TexpInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@attTexpExp", info.attTexpExp),
					new SqlParameter("@defTexpExp", info.defTexpExp),
					new SqlParameter("@hpTexpExp", info.hpTexpExp),
					new SqlParameter("@lukTexpExp", info.lukTexpExp),
					new SqlParameter("@spdTexpExp", info.spdTexpExp),
					new SqlParameter("@texpCount", info.texpCount),
					new SqlParameter("@texpTaskCount", info.texpTaskCount),
					new SqlParameter("@texpTaskDate", info.texpTaskDate.ToString("yyyy-MM-dd HH:mm:ss")),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[9].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UserTexp_Add", sqlParameters);
				flag = ((int)sqlParameters[9].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("InsertTexpInfo", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00019090 File Offset: 0x00017290
		public int PullDown(int activeID, string awardID, int userID, ref string msg)
		{
			int result = 1;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ActiveID", activeID),
					new SqlParameter("@AwardID", awardID),
					new SqlParameter("@UserID", userID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[3].Direction = ParameterDirection.ReturnValue;
				bool flag = this.db.RunProcedure("SP_Active_PullDown", para);
				if (flag)
				{
					result = (int)para[3].Value;
					switch (result)
					{
					case 0:
						msg = "Nhận lãnh thành công, vật phẩm đã gửi đến thư người dùng.";
						break;
					case 1:
						msg = "Lỗi không xác định.";
						break;
					case 2:
						msg = "Tên người dùngkhông tồn tại.";
						break;
					case 3:
						msg = "Nhận vật phẩm  thất bại.";
						break;
					case 4:
						msg = "Số này không tồn tại, hãy kiểm tra lại.";
						break;
					case 5:
						msg = "Số này đã nhận thưởng, không thể nhận nữa.";
						break;
					case 6:
						msg = "Bạn đã nhận phần thưởng này rồi";
						break;
					case 7:
						msg = "Hoạt động chưa bắt đầu.";
						break;
					case 8:
						msg = "Hoạt động đã quá hạn.";
						break;
					default:
						msg = "Nhận thưởng thất bại.";
						break;
					}
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			return result;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00019200 File Offset: 0x00017400
		public bool AddActiveNumber(string AwardID, int ActiveID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@AwardID", AwardID),
					new SqlParameter("@ActiveID", ActiveID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[2].Direction = ParameterDirection.ReturnValue;
				flag = this.db.RunProcedure("SP_Active_Number_Add", sqlParameters);
				flag = ((int)sqlParameters[2].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000192B8 File Offset: 0x000174B8
		public PlayerInfo LoginGame(string username, string password)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", username),
					new SqlParameter("@Password", password)
				};
				this.db.GetReader(ref resultDataReader, "SP_Users_Login", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitPlayerInfo(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00019388 File Offset: 0x00017588
		public PlayerInfo LoginGame(string username, ref int isFirst, ref bool isExist, ref bool isError, bool firstValidate, ref DateTime forbidDate, string nickname)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", username),
					new SqlParameter("@Password", ""),
					new SqlParameter("@FirstValidate", firstValidate),
					new SqlParameter("@Nickname", nickname)
				};
				this.db.GetReader(ref resultDataReader, "SP_Users_LoginWeb", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					isFirst = (int)resultDataReader["IsFirst"];
					isExist = (bool)resultDataReader["IsExist"];
					forbidDate = (DateTime)resultDataReader["ForbidDate"];
					bool flag2 = isFirst > 1;
					if (flag2)
					{
						isFirst--;
					}
					return this.InitPlayerInfo(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				isError = true;
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag3 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag3)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000194D0 File Offset: 0x000176D0
		public PlayerInfo LoginGame(string username, ref int isFirst, ref bool isExist, ref bool isError, bool firstValidate, ref DateTime forbidDate, ref string nickname, string ActiveIP)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", username),
					new SqlParameter("@Password", ""),
					new SqlParameter("@FirstValidate", firstValidate),
					new SqlParameter("@Nickname", nickname),
					new SqlParameter("@ActiveIP", ActiveIP)
				};
				this.db.GetReader(ref resultDataReader, "SP_Users_LoginWeb", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					isFirst = (int)resultDataReader["IsFirst"];
					isExist = (bool)resultDataReader["IsExist"];
					forbidDate = (DateTime)resultDataReader["ForbidDate"];
					nickname = (string)resultDataReader["NickName"];
					bool flag2 = isFirst > 1;
					if (flag2)
					{
						isFirst--;
					}
					return this.InitPlayerInfo(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				isError = true;
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag3 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag3)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00019654 File Offset: 0x00017854
		public bool RegisterPlayer(string userName, string passWord, string nickName, string bStyle, string gStyle, string armColor, string hairColor, string faceColor, string clothColor, string hatColor, int sex, ref string msg, int validDate)
		{
			bool flag = false;
			bool result;
			try
			{
				string[] strArray = bStyle.Split(new char[]
				{
					','
				});
				string[] strArray2 = gStyle.Split(new char[]
				{
					','
				});
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", userName),
					new SqlParameter("@PassWord", passWord),
					new SqlParameter("@NickName", nickName),
					new SqlParameter("@BArmID", int.Parse(strArray[0])),
					new SqlParameter("@BHairID", int.Parse(strArray[1])),
					new SqlParameter("@BFaceID", int.Parse(strArray[2])),
					new SqlParameter("@BClothID", int.Parse(strArray[3])),
					new SqlParameter("@BHatID", int.Parse(strArray[4])),
					new SqlParameter("@GArmID", int.Parse(strArray2[0])),
					new SqlParameter("@GHairID", int.Parse(strArray2[1])),
					new SqlParameter("@GFaceID", int.Parse(strArray2[2])),
					new SqlParameter("@GClothID", int.Parse(strArray2[3])),
					new SqlParameter("@GHatID", int.Parse(strArray2[4])),
					new SqlParameter("@ArmColor", armColor),
					new SqlParameter("@HairColor", hairColor),
					new SqlParameter("@FaceColor", faceColor),
					new SqlParameter("@ClothColor", clothColor),
					new SqlParameter("@HatColor", clothColor),
					new SqlParameter("@Sex", sex),
					new SqlParameter("@StyleDate", validDate),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[20].Direction = ParameterDirection.ReturnValue;
				flag = this.db.RunProcedure("SP_Users_RegisterNotValidate", sqlParameters);
				int num = (int)sqlParameters[20].Value;
				flag = (num == 0);
				int num2 = num;
				int num3 = num2;
				if (num3 != 2)
				{
					if (num3 != 3)
					{
						result = flag;
					}
					else
					{
						msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg3", Array.Empty<object>());
						result = flag;
					}
				}
				else
				{
					msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg2", Array.Empty<object>());
					result = flag;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00019918 File Offset: 0x00017B18
		public bool RegisterUser(string UserName, string NickName, string Password, bool Sex, int Money, int GiftToken, int Gold)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", UserName),
					new SqlParameter("@Password", Password),
					new SqlParameter("@NickName", NickName),
					new SqlParameter("@Sex", Sex),
					new SqlParameter("@Money", Money),
					new SqlParameter("@GiftToken", GiftToken),
					new SqlParameter("@Gold", Gold),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[7].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Account_Register", sqlParameters);
				bool flag2 = (int)sqlParameters[7].Value == 0;
				if (flag2)
				{
					flag = true;
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init Register", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00019A34 File Offset: 0x00017C34
		public bool RegisterUserInfo(UserInfo userinfo)
		{
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", userinfo.UserID),
					new SqlParameter("@UserEmail", userinfo.UserEmail),
					new SqlParameter("@UserPhone", (userinfo.UserPhone == null) ? string.Empty : userinfo.UserPhone),
					new SqlParameter("@UserOther1", (userinfo.UserOther1 == null) ? string.Empty : userinfo.UserOther1),
					new SqlParameter("@UserOther2", (userinfo.UserOther2 == null) ? string.Empty : userinfo.UserOther2),
					new SqlParameter("@UserOther3", (userinfo.UserOther3 == null) ? string.Empty : userinfo.UserOther3)
				};
				return this.db.RunProcedure("SP_User_Info_Add", sqlParameters);
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			return false;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00019B4C File Offset: 0x00017D4C
		public PlayerInfo ReLoadPlayer(int ID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", ID)
				};
				this.db.GetReader(ref resultDataReader, "SP_Users_Reload", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitPlayerInfo(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00019C14 File Offset: 0x00017E14
		public bool RemoveIsArrange(int ID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", ID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[1].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_RemoveIsArrange", sqlParameters);
				flag = ((int)sqlParameters[1].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_RemoveIsArrange", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00019CBC File Offset: 0x00017EBC
		public bool RemoveTreasureDataByUser(int ID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", ID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[1].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_RemoveTreasureDataByUser", sqlParameters);
				flag = ((int)sqlParameters[1].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_RemoveTreasureDataByUser", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00019D64 File Offset: 0x00017F64
		public bool RenameNick(string userName, string nickName, string newNickName)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserName", userName),
					new SqlParameter("@NickName", nickName),
					new SqlParameter("@NewNickName", newNickName),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[3].Direction = ParameterDirection.ReturnValue;
				result = this.db.RunProcedure("SP_Users_RenameByCard", para);
				int returnValue = (int)para[3].Value;
				result = (returnValue == 0);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("RenameNick", e);
				}
			}
			return result;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00019E24 File Offset: 0x00018024
		public bool RenameNick(string userName, string nickName, string newNickName, ref string msg)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserName", userName),
					new SqlParameter("@NickName", nickName),
					new SqlParameter("@NewNickName", newNickName),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[3].Direction = ParameterDirection.ReturnValue;
				result = this.db.RunProcedure("SP_Users_RenameNick", para);
				int returnValue = (int)para[3].Value;
				result = (returnValue == 0);
				int num = returnValue;
				int num2 = num;
				if (num2 - 4 <= 1)
				{
					msg = LanguageMgr.GetTranslation(" tên nhân vật đã tồn tại.", Array.Empty<object>());
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("RenameNick", e);
				}
			}
			return result;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00019F08 File Offset: 0x00018108
		public bool ChangeSex(int UserId, bool newSex)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserId", UserId),
					new SqlParameter("@Sex", newSex),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[2].Direction = ParameterDirection.ReturnValue;
				result = this.db.RunProcedure("SP_Users_ChangSexByCard", para);
				int returnValue = (int)para[2].Value;
				result = (returnValue == 0);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Users_ChangSexByCard ", e);
				}
			}
			return result;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00019FC8 File Offset: 0x000181C8
		public bool ResetCommunalActive(int ActiveID, bool IsReset)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ActiveID", ActiveID),
					new SqlParameter("@IsReset", IsReset),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[2].Direction = ParameterDirection.ReturnValue;
				flag = this.db.RunProcedure("SP_ReCommunalActive", sqlParameters);
				flag = ((int)sqlParameters[2].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init CommunalActive", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0001A084 File Offset: 0x00018284
		public bool ResetDragonBoat()
		{
			bool flag = false;
			bool result;
			try
			{
				flag = this.db.RunProcedure("SP_ReDragonBoat_Data");
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init ResetDragonBoat", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0001A0E8 File Offset: 0x000182E8
		public bool SaveBuffer(BufferInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@Type", info.Type),
					new SqlParameter("@BeginDate", info.BeginDate),
					new SqlParameter("@Data", (info.Data == null) ? "" : info.Data),
					new SqlParameter("@IsExist", info.IsExist),
					new SqlParameter("@ValidDate", info.ValidDate),
					new SqlParameter("@ValidCount", info.ValidCount),
					new SqlParameter("@Value", info.Value),
					new SqlParameter("@TemplateID", info.TemplateID)
				};
				flag = this.db.RunProcedure("SP_User_Buff_Add", sqlParameters);
				info.IsDirty = false;
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0001A24C File Offset: 0x0001844C
		public bool SaveConsortiaBuffer(ConsortiaBufferInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				flag = this.db.RunProcedure("SP_User_Consortia_Buff_Add", new SqlParameter[]
				{
					new SqlParameter("@ConsortiaID", info.ConsortiaID),
					new SqlParameter("@BufferID", info.BufferID),
					new SqlParameter("@IsOpen", info.IsOpen ? 1 : 0),
					new SqlParameter("@BeginDate", info.BeginDate),
					new SqlParameter("@ValidDate", info.ValidDate),
					new SqlParameter("@Type ", info.Type),
					new SqlParameter("@Value", info.Value)
				});
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0001A364 File Offset: 0x00018564
		public bool SavePlayerMarryNotice(MarryApplyInfo info, int answerId, ref int id)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[9];
				array[0] = new SqlParameter("@UserID", info.UserID);
				array[1] = new SqlParameter("@ApplyUserID", info.ApplyUserID);
				array[2] = new SqlParameter("@ApplyUserName", info.ApplyUserName);
				array[3] = new SqlParameter("@ApplyType", info.ApplyType);
				array[4] = new SqlParameter("@ApplyResult", info.ApplyResult);
				array[5] = new SqlParameter("@LoveProclamation", info.LoveProclamation);
				array[6] = new SqlParameter("@AnswerId", answerId);
				array[7] = new SqlParameter("@ouototal", SqlDbType.Int);
				SqlParameter[] sqlParameters = array;
				sqlParameters[7].Direction = ParameterDirection.Output;
				sqlParameters[8] = new SqlParameter("@Result", SqlDbType.Int);
				sqlParameters[8].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Insert_Marry_Notice", sqlParameters);
				id = (int)sqlParameters[7].Value;
				flag = ((int)sqlParameters[8].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SavePlayerMarryNotice", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0001A4C8 File Offset: 0x000186C8
		public bool ScanAuction(ref string noticeUserID, double cess)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[2];
				array[0] = new SqlParameter("@NoticeUserID", SqlDbType.NVarChar, 4000);
				SqlParameter[] SqlParameters = array;
				SqlParameters[0].Direction = ParameterDirection.Output;
				SqlParameters[1] = new SqlParameter("@Cess", cess);
				this.db.RunProcedure("SP_Auction_Scan", SqlParameters);
				noticeUserID = SqlParameters[0].Value.ToString();
				flag = true;
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0001A578 File Offset: 0x00018778
		public bool ScanMail(ref string noticeUserID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@NoticeUserID", SqlDbType.NVarChar, 4000)
				};
				sqlParameters[0].Direction = ParameterDirection.Output;
				this.db.RunProcedure("SP_Mail_Scan", sqlParameters);
				noticeUserID = sqlParameters[0].Value.ToString();
				flag = true;
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0001A614 File Offset: 0x00018814
		public bool SendMail(MailInfo mail)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[29];
				sqlParameters[0] = new SqlParameter("@ID", mail.ID);
				sqlParameters[0].Direction = ParameterDirection.Output;
				sqlParameters[1] = new SqlParameter("@Annex1", (mail.Annex1 == null) ? "" : mail.Annex1);
				sqlParameters[2] = new SqlParameter("@Annex2", (mail.Annex2 == null) ? "" : mail.Annex2);
				sqlParameters[3] = new SqlParameter("@Content", (mail.Content == null) ? "" : mail.Content);
				sqlParameters[4] = new SqlParameter("@Gold", mail.Gold);
				sqlParameters[5] = new SqlParameter("@IsExist", true);
				sqlParameters[6] = new SqlParameter("@Money", mail.Money);
				sqlParameters[7] = new SqlParameter("@Receiver", (mail.Receiver == null) ? "" : mail.Receiver);
				sqlParameters[8] = new SqlParameter("@ReceiverID", mail.ReceiverID);
				sqlParameters[9] = new SqlParameter("@Sender", (mail.Sender == null) ? "" : mail.Sender);
				sqlParameters[10] = new SqlParameter("@SenderID", mail.SenderID);
				sqlParameters[11] = new SqlParameter("@Title", (mail.Title == null) ? "" : mail.Title);
				sqlParameters[12] = new SqlParameter("@IfDelS", false);
				sqlParameters[13] = new SqlParameter("@IsDelete", false);
				sqlParameters[14] = new SqlParameter("@IsDelR", false);
				sqlParameters[15] = new SqlParameter("@IsRead", false);
				sqlParameters[16] = new SqlParameter("@SendTime", DateTime.Now);
				sqlParameters[17] = new SqlParameter("@Type", mail.Type);
				sqlParameters[18] = new SqlParameter("@Annex1Name", (mail.Annex1Name == null) ? "" : mail.Annex1Name);
				sqlParameters[19] = new SqlParameter("@Annex2Name", (mail.Annex2Name == null) ? "" : mail.Annex2Name);
				sqlParameters[20] = new SqlParameter("@Annex3", (mail.Annex3 == null) ? "" : mail.Annex3);
				sqlParameters[21] = new SqlParameter("@Annex4", (mail.Annex4 == null) ? "" : mail.Annex4);
				sqlParameters[22] = new SqlParameter("@Annex5", (mail.Annex5 == null) ? "" : mail.Annex5);
				sqlParameters[23] = new SqlParameter("@Annex3Name", (mail.Annex3Name == null) ? "" : mail.Annex3Name);
				sqlParameters[24] = new SqlParameter("@Annex4Name", (mail.Annex4Name == null) ? "" : mail.Annex4Name);
				sqlParameters[25] = new SqlParameter("@Annex5Name", (mail.Annex5Name == null) ? "" : mail.Annex5Name);
				sqlParameters[26] = new SqlParameter("@ValidDate", mail.ValidDate);
				sqlParameters[27] = new SqlParameter("@AnnexRemark", (mail.AnnexRemark == null) ? "" : mail.AnnexRemark);
				sqlParameters[28] = new SqlParameter("@GiftToken", mail.GiftToken);
				flag = this.db.RunProcedure("SP_Mail_Send", sqlParameters);
				mail.ID = (int)sqlParameters[0].Value;
				using (CenterServiceClient client = new CenterServiceClient())
				{
					client.MailNotice(mail.ReceiverID);
					result = flag;
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0001AA2C File Offset: 0x00018C2C
		public bool SendMailAndItem(MailInfo mail, ItemInfo item, ref int returnValue)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[34];
				array[0] = new SqlParameter("@ItemID", item.ItemID);
				array[1] = new SqlParameter("@UserID", item.UserID);
				array[2] = new SqlParameter("@TemplateID", item.TemplateID);
				array[3] = new SqlParameter("@Place", item.Place);
				array[4] = new SqlParameter("@AgilityCompose", item.AgilityCompose);
				array[5] = new SqlParameter("@AttackCompose", item.AttackCompose);
				array[6] = new SqlParameter("@BeginDate", item.BeginDate);
				array[7] = new SqlParameter("@Color", (item.Color == null) ? "" : item.Color);
				array[8] = new SqlParameter("@Count", item.Count);
				array[9] = new SqlParameter("@DefendCompose", item.DefendCompose);
				array[10] = new SqlParameter("@IsBinds", item.IsBinds);
				array[11] = new SqlParameter("@IsExist", item.IsExist);
				array[12] = new SqlParameter("@IsJudge", item.IsJudge);
				array[13] = new SqlParameter("@LuckCompose", item.LuckCompose);
				array[14] = new SqlParameter("@StrengthenLevel", item.StrengthenLevel);
				array[15] = new SqlParameter("@ValidDate", item.ValidDate);
				array[16] = new SqlParameter("@BagType", item.BagType);
				array[17] = new SqlParameter("@ID", mail.ID);
				SqlParameter[] sqlParameters = array;
				sqlParameters[17].Direction = ParameterDirection.Output;
				sqlParameters[18] = new SqlParameter("@Annex1", (mail.Annex1 == null) ? "" : mail.Annex1);
				sqlParameters[19] = new SqlParameter("@Annex2", (mail.Annex2 == null) ? "" : mail.Annex2);
				sqlParameters[20] = new SqlParameter("@Content", (mail.Content == null) ? "" : mail.Content);
				sqlParameters[21] = new SqlParameter("@Gold", mail.Gold);
				sqlParameters[22] = new SqlParameter("@Money", mail.Money);
				sqlParameters[23] = new SqlParameter("@Receiver", (mail.Receiver == null) ? "" : mail.Receiver);
				sqlParameters[24] = new SqlParameter("@ReceiverID", mail.ReceiverID);
				sqlParameters[25] = new SqlParameter("@Sender", (mail.Sender == null) ? "" : mail.Sender);
				sqlParameters[26] = new SqlParameter("@SenderID", mail.SenderID);
				sqlParameters[27] = new SqlParameter("@Title", (mail.Title == null) ? "" : mail.Title);
				sqlParameters[28] = new SqlParameter("@IfDelS", false);
				sqlParameters[29] = new SqlParameter("@IsDelete", false);
				sqlParameters[30] = new SqlParameter("@IsDelR", false);
				sqlParameters[31] = new SqlParameter("@IsRead", false);
				sqlParameters[32] = new SqlParameter("@SendTime", DateTime.Now);
				sqlParameters[33] = new SqlParameter("@Result", SqlDbType.Int);
				sqlParameters[33].Direction = ParameterDirection.ReturnValue;
				flag = this.db.RunProcedure("SP_Admin_SendUserItem", sqlParameters);
				returnValue = (int)sqlParameters[33].Value;
				flag = (returnValue == 0);
				bool flag2 = !flag;
				if (flag2)
				{
					result = flag;
				}
				else
				{
					using (CenterServiceClient client = new CenterServiceClient())
					{
						client.MailNotice(mail.ReceiverID);
						result = flag;
					}
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0001AE8C File Offset: 0x0001908C
		public int SendMailAndItem(string title, string content, int userID, int gold, int money, string param)
		{
			int num = 1;
			int result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Title", title),
					new SqlParameter("@Content", content),
					new SqlParameter("@UserID", userID),
					new SqlParameter("@Gold", gold),
					new SqlParameter("@Money", money),
					new SqlParameter("@GiftToken", SqlDbType.BigInt),
					new SqlParameter("@Param", param),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[7].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Admin_SendAllItem", sqlParameters);
				num = (int)sqlParameters[7].Value;
				bool flag = num != 0;
				if (flag)
				{
					result = num;
				}
				else
				{
					using (CenterServiceClient client = new CenterServiceClient())
					{
						client.MailNotice(userID);
						result = num;
					}
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = num;
				}
				else
				{
					result = num;
				}
			}
			return result;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0001AFCC File Offset: 0x000191CC
		public int SendMailAndItem(string title, string content, int UserID, int templateID, int count, int validDate, int gold, int money, int StrengthenLevel, int AttackCompose, int DefendCompose, int AgilityCompose, int LuckCompose, bool isBinds)
		{
			MailInfo mail = new MailInfo
			{
				Annex1 = "",
				Content = title,
				Gold = gold,
				Money = money,
				Receiver = "",
				ReceiverID = UserID,
				Sender = "Administrators",
				SenderID = 0,
				Title = content
			};
			ItemInfo item = new ItemInfo(null)
			{
				AgilityCompose = AgilityCompose,
				AttackCompose = AttackCompose,
				BeginDate = DateTime.Now,
				Color = "",
				DefendCompose = DefendCompose,
				IsDirty = false,
				IsExist = true,
				IsJudge = true,
				LuckCompose = LuckCompose,
				StrengthenLevel = StrengthenLevel,
				TemplateID = templateID,
				ValidDate = validDate,
				Count = count,
				IsBinds = isBinds
			};
			int returnValue = 1;
			this.SendMailAndItem(mail, item, ref returnValue);
			return returnValue;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0001B0D0 File Offset: 0x000192D0
		public int SendMailAndItemByNickName(string title, string content, string nickName, int gold, int money, string param)
		{
			PlayerInfo userSingleByNickName = this.GetUserSingleByNickName(nickName);
			bool flag = userSingleByNickName != null;
			int result;
			if (flag)
			{
				result = this.SendMailAndItem(title, content, userSingleByNickName.ID, gold, money, param);
			}
			else
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0001B10C File Offset: 0x0001930C
		public int SendMailAndItemByNickName(string title, string content, string NickName, int templateID, int count, int validDate, int gold, int money, int StrengthenLevel, int AttackCompose, int DefendCompose, int AgilityCompose, int LuckCompose, bool isBinds)
		{
			PlayerInfo userSingleByNickName = this.GetUserSingleByNickName(NickName);
			bool flag = userSingleByNickName != null;
			int result;
			if (flag)
			{
				result = this.SendMailAndItem(title, content, userSingleByNickName.ID, templateID, count, validDate, gold, money, StrengthenLevel, AttackCompose, DefendCompose, AgilityCompose, LuckCompose, isBinds);
			}
			else
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0001B158 File Offset: 0x00019358
		public int SendMailAndItemByUserName(string title, string content, string userName, int gold, int money, string param)
		{
			PlayerInfo userSingleByUserName = this.GetUserSingleByUserName(userName);
			bool flag = userSingleByUserName != null;
			int result;
			if (flag)
			{
				result = this.SendMailAndItem(title, content, userSingleByUserName.ID, gold, money, param);
			}
			else
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0001B194 File Offset: 0x00019394
		public int SendMailAndItemByUserName(string title, string content, string userName, int templateID, int count, int validDate, int gold, int money, int StrengthenLevel, int AttackCompose, int DefendCompose, int AgilityCompose, int LuckCompose, bool isBinds)
		{
			PlayerInfo userSingleByUserName = this.GetUserSingleByUserName(userName);
			bool flag = userSingleByUserName != null;
			int result;
			if (flag)
			{
				result = this.SendMailAndItem(title, content, userSingleByUserName.ID, templateID, count, validDate, gold, money, StrengthenLevel, AttackCompose, DefendCompose, AgilityCompose, LuckCompose, isBinds);
			}
			else
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0001B1E0 File Offset: 0x000193E0
		public bool SendMailAndMoney(MailInfo mail, ref int returnValue)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[18];
				array[0] = new SqlParameter("@ID", mail.ID);
				SqlParameter[] sqlParameters = array;
				sqlParameters[0].Direction = ParameterDirection.Output;
				sqlParameters[1] = new SqlParameter("@Annex1", (mail.Annex1 == null) ? "" : mail.Annex1);
				sqlParameters[2] = new SqlParameter("@Annex2", (mail.Annex2 == null) ? "" : mail.Annex2);
				sqlParameters[3] = new SqlParameter("@Content", (mail.Content == null) ? "" : mail.Content);
				sqlParameters[4] = new SqlParameter("@Gold", mail.Gold);
				sqlParameters[5] = new SqlParameter("@IsExist", true);
				sqlParameters[6] = new SqlParameter("@Money", mail.Money);
				sqlParameters[7] = new SqlParameter("@Receiver", (mail.Receiver == null) ? "" : mail.Receiver);
				sqlParameters[8] = new SqlParameter("@ReceiverID", mail.ReceiverID);
				sqlParameters[9] = new SqlParameter("@Sender", (mail.Sender == null) ? "" : mail.Sender);
				sqlParameters[10] = new SqlParameter("@SenderID", mail.SenderID);
				sqlParameters[11] = new SqlParameter("@Title", (mail.Title == null) ? "" : mail.Title);
				sqlParameters[12] = new SqlParameter("@IfDelS", false);
				sqlParameters[13] = new SqlParameter("@IsDelete", false);
				sqlParameters[14] = new SqlParameter("@IsDelR", false);
				sqlParameters[15] = new SqlParameter("@IsRead", false);
				sqlParameters[16] = new SqlParameter("@SendTime", DateTime.Now);
				sqlParameters[17] = new SqlParameter("@Result", SqlDbType.Int);
				sqlParameters[17].Direction = ParameterDirection.ReturnValue;
				flag = this.db.RunProcedure("SP_Admin_SendUserMoney", sqlParameters);
				returnValue = (int)sqlParameters[17].Value;
				flag = (returnValue == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0001B454 File Offset: 0x00019654
		public bool Test(string DutyName)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@DutyName", DutyName)
				};
				flag = this.db.RunProcedure("SP_Test1", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0001B4D0 File Offset: 0x000196D0
		public bool UpdateAuction(AuctionInfo info, double cess)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@AuctionID", info.AuctionID),
					new SqlParameter("@AuctioneerID", info.AuctioneerID),
					new SqlParameter("@AuctioneerName", (info.AuctioneerName == null) ? "" : info.AuctioneerName),
					new SqlParameter("@BeginDate", info.BeginDate),
					new SqlParameter("@BuyerID", info.BuyerID),
					new SqlParameter("@BuyerName", (info.BuyerName == null) ? "" : info.BuyerName),
					new SqlParameter("@IsExist", info.IsExist),
					new SqlParameter("@ItemID", info.ItemID),
					new SqlParameter("@Mouthful", info.Mouthful),
					new SqlParameter("@PayType", info.PayType),
					new SqlParameter("@Price", info.Price),
					new SqlParameter("@Rise", info.Rise),
					new SqlParameter("@ValidDate", info.ValidDate),
					new SqlParameter("@Name", info.Name),
					new SqlParameter("@Category", info.Category),
					null,
					new SqlParameter("@Cess", cess)
				};
				SqlParameters[15] = new SqlParameter("@Result", SqlDbType.Int);
				SqlParameters[15].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Auction_Update", SqlParameters);
				flag = ((int)SqlParameters[15].Value == 0);
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0001B708 File Offset: 0x00019908
		public bool UpdateUsersEventProcess(EventRewardProcessInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@ActiveType", info.ActiveType),
					new SqlParameter("@Conditions", info.Conditions),
					new SqlParameter("@AwardGot", info.AwardGot),
					new SqlParameter("@Result", SqlDbType.Int),
					new SqlParameter("@IsReset", info.IsReset)
				};
				sqlParameters[4].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateUsersEventProcess", sqlParameters);
				flag = ((int)sqlParameters[4].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				BaseBussiness.log.Error("SP_UpdateUsersEventProcess", exception);
				result = flag;
			}
			return result;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0001B800 File Offset: 0x00019A00
		public bool UpdateBreakTimeWhereServerStop()
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[0].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Update_Marry_Room_Info_Sever_Stop", sqlParameters);
				flag = ((int)sqlParameters[0].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdateBreakTimeWhereServerStop", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0001B894 File Offset: 0x00019A94
		public bool UpdateBuyStore(int storeId)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@StoreID", storeId)
				};
				flag = this.db.RunProcedure("SP_Update_Buy_Store", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Update_Buy_Store", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0001B914 File Offset: 0x00019B14
		public bool ResetQuests(int UserID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserID)
				};
				flag = this.db.RunProcedure("SP_Quest_Reset", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Quest_Reset", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0001B994 File Offset: 0x00019B94
		public bool UpdateCards(UsersCardInfo item)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@CardID", item.CardID),
					new SqlParameter("@UserID", item.UserID),
					new SqlParameter("@TemplateID", item.TemplateID),
					new SqlParameter("@Place", item.Place),
					new SqlParameter("@Count", item.Count),
					new SqlParameter("@Attack", item.Attack),
					new SqlParameter("@Defence", item.Defence),
					new SqlParameter("@Agility", item.Agility),
					new SqlParameter("@Luck", item.Luck),
					new SqlParameter("@Guard", item.Guard),
					new SqlParameter("@Damage", item.Damage),
					new SqlParameter("@Level", item.Level),
					new SqlParameter("@CardGP", item.CardGP),
					null,
					new SqlParameter("@AttackReset", item.AttackReset),
					new SqlParameter("@DefenceReset", item.DefenceReset),
					new SqlParameter("@AgilityReset", item.AgilityReset),
					new SqlParameter("@LuckReset", item.LuckReset),
					new SqlParameter("@isFirstGet", item.isFirstGet)
				};
				SqlParameters[13] = new SqlParameter("@Result", SqlDbType.Int);
				SqlParameters[13].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateUserCard", SqlParameters);
				flag = ((int)SqlParameters[13].Value == 0);
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UpdateUserCard", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0001BBF4 File Offset: 0x00019DF4
		public int Updatecash(string UserName, int cash)
		{
			int num = 3;
			int result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserName", UserName),
					new SqlParameter("@Cash", cash),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[2].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Update_Cash", sqlParameters);
				num = (int)sqlParameters[2].Value;
				result = num;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = num;
				}
				else
				{
					result = num;
				}
			}
			return result;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0001BCA8 File Offset: 0x00019EA8
		public bool UpdateDbAchievementDataInfo(AchievementDataInfo info)
		{
			bool result = false;
			bool result2;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@AchievementID", info.AchievementID),
					new SqlParameter("@IsComplete", info.IsComplete),
					new SqlParameter("@CompletedDate", info.CompletedDate)
				};
				result = this.db.RunProcedure("SP_Achievement_Data_Add", para);
				info.IsDirty = false;
				result2 = result;
			}
			catch (Exception e)
			{
				BaseBussiness.log.Error("Init_UpdateDbAchievementDataInfo", e);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0001BD68 File Offset: 0x00019F68
		public List<AchievementDataInfo> GetUserAchievementData(int userID)
		{
			List<AchievementDataInfo> infos = new List<AchievementDataInfo>();
			SqlDataReader reader = null;
			List<AchievementDataInfo> result;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				para[0].Value = userID;
				this.db.GetReader(ref reader, "SP_Achievement_Data_All", para);
				while (reader.Read())
				{
					infos.Add(new AchievementDataInfo
					{
						UserID = (int)reader["UserID"],
						AchievementID = (int)reader["AchievementID"],
						IsComplete = (bool)reader["IsComplete"],
						CompletedDate = (DateTime)reader["CompletedDate"],
						IsDirty = false
					});
				}
				result = infos;
			}
			catch (Exception e)
			{
				BaseBussiness.log.Error("Init_GetUserAchievement", e);
				result = infos;
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return result;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0001BE94 File Offset: 0x0001A094
		public List<AchievementDataInfo> GetUserAchievementData(int userID, int id)
		{
			List<AchievementDataInfo> infos = new List<AchievementDataInfo>();
			SqlDataReader reader = null;
			List<AchievementDataInfo> result;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4),
					new SqlParameter("@AchievementID", id)
				};
				para[0].Value = userID;
				this.db.GetReader(ref reader, "SP_Achievement_Data_Single", para);
				while (reader.Read())
				{
					infos.Add(new AchievementDataInfo
					{
						UserID = (int)reader["UserID"],
						AchievementID = (int)reader["AchievementID"],
						IsComplete = (bool)reader["IsComplete"],
						CompletedDate = (DateTime)reader["CompletedDate"],
						IsDirty = false
					});
				}
				result = infos;
			}
			catch (Exception e)
			{
				BaseBussiness.log.Error("Init_GetUserAchievementSingle", e);
				result = infos;
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return result;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0001BFD4 File Offset: 0x0001A1D4
		public List<UsersRecordInfo> GetUserRecord(int userID)
		{
			List<UsersRecordInfo> infos = new List<UsersRecordInfo>();
			SqlDataReader reader = null;
			List<UsersRecordInfo> result;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				para[0].Value = userID;
				this.db.GetReader(ref reader, "SP_Users_Record_All", para);
				while (reader.Read())
				{
					infos.Add(new UsersRecordInfo
					{
						UserID = (int)reader["UserID"],
						RecordID = (int)reader["RecordID"],
						Total = (int)reader["Total"],
						IsDirty = false
					});
				}
				result = infos;
			}
			catch (Exception e)
			{
				BaseBussiness.log.Error("Init_GetUserRecord", e);
				result = infos;
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return result;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0001C0EC File Offset: 0x0001A2EC
		public bool UpdateDbUserRecord(UsersRecordInfo info)
		{
			bool result = false;
			bool result2;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@RecordID", info.RecordID),
					new SqlParameter("@Total", info.Total)
				};
				result = this.db.RunProcedure("SP_Users_Record_Add", para);
				info.IsDirty = false;
				result2 = result;
			}
			catch (Exception e)
			{
				BaseBussiness.log.Error("Init_UpdateDbUserRecord", e);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0001C194 File Offset: 0x0001A394
		public bool UpdateDbQuestDataInfo(QuestDataInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@QuestID", info.QuestID),
					new SqlParameter("@CompletedDate", info.CompletedDate),
					new SqlParameter("@IsComplete", info.IsComplete),
					new SqlParameter("@Condition1", (info.Condition1 > -1) ? info.Condition1 : 0),
					new SqlParameter("@Condition2", (info.Condition2 > -1) ? info.Condition2 : 0),
					new SqlParameter("@Condition3", (info.Condition3 > -1) ? info.Condition3 : 0),
					new SqlParameter("@Condition4", (info.Condition4 > -1) ? info.Condition4 : 0),
					new SqlParameter("@IsExist", info.IsExist),
					new SqlParameter("@RepeatFinish", (info.RepeatFinish == -1) ? 1 : info.RepeatFinish),
					new SqlParameter("@RandDobule", info.RandDobule)
				};
				flag = this.db.RunProcedure("SP_QuestData_Add", sqlParameters);
				info.IsDirty = false;
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0001C35C File Offset: 0x0001A55C
		public bool UpdateFriendHelpTimes(int ID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", ID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[1].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateFriendHelpTimes", sqlParameters);
				flag = ((int)sqlParameters[1].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UpdateFriendHelpTimes", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0001C404 File Offset: 0x0001A604
		public bool UpdateGoods(ItemInfo item)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ItemID", item.ItemID),
					new SqlParameter("@UserID", item.UserID),
					new SqlParameter("@TemplateID", item.Template.TemplateID),
					new SqlParameter("@Place", item.Place),
					new SqlParameter("@AgilityCompose", item.AgilityCompose),
					new SqlParameter("@AttackCompose", item.AttackCompose),
					new SqlParameter("@BeginDate", item.BeginDate),
					new SqlParameter("@Color", (item.Color == null) ? "" : item.Color),
					new SqlParameter("@Count", item.Count),
					new SqlParameter("@DefendCompose", item.DefendCompose),
					new SqlParameter("@IsBinds", item.IsBinds),
					new SqlParameter("@IsExist", item.IsExist),
					new SqlParameter("@IsJudge", item.IsJudge),
					new SqlParameter("@LuckCompose", item.LuckCompose),
					new SqlParameter("@StrengthenLevel", item.StrengthenLevel),
					new SqlParameter("@ValidDate", item.ValidDate),
					new SqlParameter("@BagType", item.BagType),
					new SqlParameter("@Skin", item.Skin),
					new SqlParameter("@IsUsed", item.IsUsed),
					new SqlParameter("@RemoveDate", item.RemoveDate),
					new SqlParameter("@RemoveType", item.RemoveType),
					new SqlParameter("@Hole1", item.Hole1),
					new SqlParameter("@Hole2", item.Hole2),
					new SqlParameter("@Hole3", item.Hole3),
					new SqlParameter("@Hole4", item.Hole4),
					new SqlParameter("@Hole5", item.Hole5),
					new SqlParameter("@Hole6", item.Hole6),
					new SqlParameter("@StrengthenTimes", item.StrengthenTimes),
					new SqlParameter("@Hole5Level", item.Hole5Level),
					new SqlParameter("@Hole5Exp", item.Hole5Exp),
					new SqlParameter("@Hole6Level", item.Hole6Level),
					new SqlParameter("@Hole6Exp", item.Hole6Exp),
					new SqlParameter("@IsGold", item.IsGold),
					new SqlParameter("@goldBeginTime", item.goldBeginTime),
					new SqlParameter("@goldValidDate", item.goldValidDate),
					new SqlParameter("@StrengthenExp", item.StrengthenExp),
					new SqlParameter("@Blood", item.Blood),
					new SqlParameter("@latentEnergyCurStr", item.latentEnergyCurStr),
					new SqlParameter("@latentEnergyNewStr", item.latentEnergyNewStr),
					new SqlParameter("@latentEnergyEndTime", item.latentEnergyEndTime),
					new SqlParameter("@curExp", item.curExp),
					new SqlParameter("@cellLocked", item.cellLocked)
				};
				flag = this.db.RunProcedure("SP_Users_Items_Update", sqlParameters);
				item.IsDirty = false;
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0001C898 File Offset: 0x0001AA98
		public bool UpdateLastVIPPackTime(int ID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", ID),
					new SqlParameter("@LastVIPPackTime", DateTime.Now.Date),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[2].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateUserLastVIPPackTime", sqlParameters);
				flag = true;
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UpdateUserLastVIPPackTime", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0001C954 File Offset: 0x0001AB54
		public bool UpdateMail(MailInfo mail, int oldMoney)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[30];
				array[0] = new SqlParameter("@ID", mail.ID);
				array[1] = new SqlParameter("@Annex1", (mail.Annex1 == null) ? "" : mail.Annex1);
				array[2] = new SqlParameter("@Annex2", (mail.Annex2 == null) ? "" : mail.Annex2);
				array[3] = new SqlParameter("@Content", (mail.Content == null) ? "" : mail.Content);
				array[4] = new SqlParameter("@Gold", mail.Gold);
				array[5] = new SqlParameter("@IsExist", mail.IsExist);
				array[6] = new SqlParameter("@Money", mail.Money);
				array[7] = new SqlParameter("@Receiver", (mail.Receiver == null) ? "" : mail.Receiver);
				array[8] = new SqlParameter("@ReceiverID", mail.ReceiverID);
				array[9] = new SqlParameter("@Sender", (mail.Sender == null) ? "" : mail.Sender);
				array[10] = new SqlParameter("@SenderID", mail.SenderID);
				array[11] = new SqlParameter("@Title", (mail.Title == null) ? "" : mail.Title);
				array[12] = new SqlParameter("@IfDelS", false);
				array[13] = new SqlParameter("@IsDelete", false);
				array[14] = new SqlParameter("@IsDelR", false);
				array[15] = new SqlParameter("@IsRead", mail.IsRead);
				array[16] = new SqlParameter("@SendTime", mail.SendTime);
				array[17] = new SqlParameter("@Type", mail.Type);
				array[18] = new SqlParameter("@OldMoney", oldMoney);
				array[19] = new SqlParameter("@ValidDate", mail.ValidDate);
				array[20] = new SqlParameter("@Annex1Name", mail.Annex1Name);
				array[21] = new SqlParameter("@Annex2Name", mail.Annex2Name);
				array[22] = new SqlParameter("@Result", SqlDbType.Int);
				SqlParameter[] sqlParameters = array;
				sqlParameters[22].Direction = ParameterDirection.ReturnValue;
				sqlParameters[23] = new SqlParameter("@Annex3", (mail.Annex3 == null) ? "" : mail.Annex3);
				sqlParameters[24] = new SqlParameter("@Annex4", (mail.Annex4 == null) ? "" : mail.Annex4);
				sqlParameters[25] = new SqlParameter("@Annex5", (mail.Annex5 == null) ? "" : mail.Annex5);
				sqlParameters[26] = new SqlParameter("@Annex3Name", (mail.Annex3Name == null) ? "" : mail.Annex3Name);
				sqlParameters[27] = new SqlParameter("@Annex4Name", (mail.Annex4Name == null) ? "" : mail.Annex4Name);
				sqlParameters[28] = new SqlParameter("@Annex5Name", (mail.Annex5Name == null) ? "" : mail.Annex5Name);
				sqlParameters[29] = new SqlParameter("GiftToken", mail.GiftToken);
				this.db.RunProcedure("SP_Mail_Update", sqlParameters);
				flag = ((int)sqlParameters[22].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0001CD20 File Offset: 0x0001AF20
		public bool UpdateMarryInfo(MarryInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@IsPublishEquip", info.IsPublishEquip),
					new SqlParameter("@Introduction", info.Introduction),
					new SqlParameter("@RegistTime", info.RegistTime.ToString("yyyy-MM-dd HH:mm:ss")),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[5].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_MarryInfo_Update", sqlParameters);
				flag = ((int)sqlParameters[5].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0001CE34 File Offset: 0x0001B034
		public bool UpdateMarryRoomInfo(MarryRoomInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@AvailTime", info.AvailTime),
					new SqlParameter("@BreakTime", info.BreakTime),
					new SqlParameter("@roomIntroduction", info.RoomIntroduction),
					new SqlParameter("@isHymeneal", info.IsHymeneal),
					new SqlParameter("@Name", info.Name),
					new SqlParameter("@Pwd", info.Pwd),
					new SqlParameter("@IsGunsaluteUsed", info.IsGunsaluteUsed),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[8].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Update_Marry_Room_Info", sqlParameters);
				flag = ((int)sqlParameters[8].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdateMarryRoomInfo", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0001CF7C File Offset: 0x0001B17C
		public bool UpdatePassWord(int userID, string password)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", userID),
					new SqlParameter("@Password", password)
				};
				flag = this.db.RunProcedure("SP_Users_UpdatePassword", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0001D008 File Offset: 0x0001B208
		public bool UpdatePasswordInfo(int userID, string PasswordQuestion1, string PasswordAnswer1, string PasswordQuestion2, string PasswordAnswer2, int Count)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", userID),
					new SqlParameter("@PasswordQuestion1", PasswordQuestion1),
					new SqlParameter("@PasswordAnswer1", PasswordAnswer1),
					new SqlParameter("@PasswordQuestion2", PasswordQuestion2),
					new SqlParameter("@PasswordAnswer2", PasswordAnswer2),
					new SqlParameter("@FailedPasswordAttemptCount", Count)
				};
				flag = this.db.RunProcedure("SP_Users_Password_Add", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0001D0D4 File Offset: 0x0001B2D4
		public bool UpdatePasswordTwo(int userID, string passwordTwo)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", userID),
					new SqlParameter("@PasswordTwo", passwordTwo)
				};
				flag = this.db.RunProcedure("SP_Users_UpdatePasswordTwo", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0001D160 File Offset: 0x0001B360
		public bool UpdatePlayer(PlayerInfo player)
		{
			bool flag = false;
			SqlParameter[] sqlParameters2 = new SqlParameter[88];
			bool result;
			try
			{
				bool flag2 = player.Grade < 1;
				if (flag2)
				{
					result = flag;
				}
				else
				{
					bool flag3 = player.ID <= 0;
					if (flag3)
					{
						result = flag;
					}
					else
					{
						SqlParameter[] para = new SqlParameter[88];
						para[0] = new SqlParameter("@UserID", player.ID);
						para[1] = new SqlParameter("@Attack", player.Attack);
						para[2] = new SqlParameter("@Colors", (player.Colors == null) ? "" : player.Colors);
						para[3] = new SqlParameter("@ConsortiaID", player.ConsortiaID);
						para[4] = new SqlParameter("@Defence", player.Defence);
						para[5] = new SqlParameter("@Gold", player.Gold);
						para[6] = new SqlParameter("@GP", player.GP);
						para[7] = new SqlParameter("@Grade", player.Grade);
						para[8] = new SqlParameter("@Luck", player.Luck);
						para[9] = new SqlParameter("@Money", player.Money);
						para[10] = new SqlParameter("@Style", (player.Style == null) ? "" : player.Style);
						para[11] = new SqlParameter("@Agility", player.Agility);
						para[12] = new SqlParameter("@State", player.State);
						para[13] = new SqlParameter("@Hide", player.Hide);
						para[14] = new SqlParameter("@ExpendDate", (player.ExpendDate == null) ? "" : player.ExpendDate.ToString());
						para[15] = new SqlParameter("@Win", player.Win);
						para[16] = new SqlParameter("@Total", player.Total);
						para[17] = new SqlParameter("@Escape", player.Escape);
						para[18] = new SqlParameter("@Skin", (player.Skin == null) ? "" : player.Skin);
						para[19] = new SqlParameter("@Offer", player.Offer);
						para[20] = new SqlParameter("@AntiAddiction", player.AntiAddiction);
						para[20].Direction = ParameterDirection.InputOutput;
						para[21] = new SqlParameter("@Result", SqlDbType.Int);
						para[21].Direction = ParameterDirection.ReturnValue;
						para[22] = new SqlParameter("@RichesOffer", player.RichesOffer);
						para[23] = new SqlParameter("@RichesRob", player.RichesRob);
						para[24] = new SqlParameter("@CheckCount", player.CheckCount);
						para[24].Direction = ParameterDirection.InputOutput;
						para[25] = new SqlParameter("@MarryInfoID", player.MarryInfoID);
						para[26] = new SqlParameter("@DayLoginCount", player.DayLoginCount);
						para[27] = new SqlParameter("@Nimbus", player.Nimbus);
						para[28] = new SqlParameter("@LastAward", player.LastAward);
						para[29] = new SqlParameter("@GiftToken", player.GiftToken);
						para[30] = new SqlParameter("@QuestSite", player.QuestSite);
						para[31] = new SqlParameter("@PvePermission", player.PvePermission);
						para[32] = new SqlParameter("@FightPower", player.FightPower);
						para[33] = new SqlParameter("@AnswerSite", player.AnswerSite);
						para[34] = new SqlParameter("@LastAuncherAward", player.LastAward);
						para[35] = new SqlParameter("@hp", player.hp);
						para[36] = new SqlParameter("@ChatCount", player.ChatCount);
						para[37] = new SqlParameter("@SpaPubGoldRoomLimit", player.SpaPubGoldRoomLimit);
						para[38] = new SqlParameter("@LastSpaDate", player.LastSpaDate);
						para[39] = new SqlParameter("@FightLabPermission", player.FightLabPermission);
						para[40] = new SqlParameter("@SpaPubMoneyRoomLimit", player.SpaPubMoneyRoomLimit);
						para[41] = new SqlParameter("@IsInSpaPubGoldToday", player.IsInSpaPubGoldToday);
						para[42] = new SqlParameter("@IsInSpaPubMoneyToday", player.IsInSpaPubMoneyToday);
						para[43] = new SqlParameter("@AchievementPoint", player.AchievementPoint);
						para[44] = new SqlParameter("@LastWeekly", player.LastWeekly);
						para[45] = new SqlParameter("@LastWeeklyVersion", player.LastWeeklyVersion);
						para[46] = new SqlParameter("@WeaklessGuildProgressStr", player.WeaklessGuildProgressStr);
						para[47] = new SqlParameter("@IsOldPlayer", player.IsOldPlayer);
						para[48] = new SqlParameter("@VIPLevel", player.VIPLevel);
						para[49] = new SqlParameter("@VIPExp", player.VIPExp);
						para[50] = new SqlParameter("@Score", player.Score);
						para[51] = new SqlParameter("@OptionOnOff", player.OptionOnOff);
						para[52] = new SqlParameter("@isOldPlayerHasValidEquitAtLogin", player.isOldPlayerHasValidEquitAtLogin);
						para[53] = new SqlParameter("@badLuckNumber", player.badLuckNumber);
						para[54] = new SqlParameter("@luckyNum", player.luckyNum);
						para[55] = new SqlParameter("@lastLuckyNumDate", player.lastLuckyNumDate);
						para[56] = new SqlParameter("@lastLuckNum", player.lastLuckNum);
						para[57] = new SqlParameter("@IsShowConsortia", player.IsShowConsortia);
						para[58] = new SqlParameter("@NewDay", player.NewDay);
						para[59] = new SqlParameter("@Medal", player.medal);
						para[60] = new SqlParameter("@Honor", player.Honor);
						para[61] = new SqlParameter("@VIPNextLevelDaysNeeded", player.GetVIPNextLevelDaysNeeded(player.VIPLevel, player.VIPExp));
						para[62] = new SqlParameter("@IsRecharged", player.IsRecharged);
						para[63] = new SqlParameter("@IsGetAward", player.IsGetAward);
						para[64] = new SqlParameter("@typeVIP", player.typeVIP);
						para[65] = new SqlParameter("@evolutionGrade", player.evolutionGrade);
						para[66] = new SqlParameter("@evolutionExp", player.evolutionExp);
						para[67] = new SqlParameter("@hardCurrency", player.hardCurrency);
						para[68] = new SqlParameter("@EliteScore", player.EliteScore);
						para[69] = new SqlParameter("@UseOffer", player.UseOffer);
						para[70] = new SqlParameter("@ShopFinallyGottenTime", player.ShopFinallyGottenTime);
						para[71] = new SqlParameter("@MoneyLock", player.MoneyLock);
						para[72] = new SqlParameter("@LastGetEgg", player.LastGetEgg);
						para[73] = new SqlParameter("@IsFistGetPet", player.IsFistGetPet);
						para[74] = new SqlParameter("@LastRefreshPet", player.LastRefreshPet);
						para[75] = new SqlParameter("@petScore", player.petScore);
						para[76] = new SqlParameter("@accumulativeLoginDays", player.accumulativeLoginDays);
						para[77] = new SqlParameter("@accumulativeAwardDays", player.accumulativeAwardDays);
						para[78] = new SqlParameter("@honorId", player.honorId);
						para[79] = new SqlParameter("@Repute", player.Repute);
						para[80] = new SqlParameter("@damageScores", player.damageScores);
						para[81] = new SqlParameter("@totemId", player.totemId);
						para[82] = new SqlParameter("@myHonor", player.myHonor);
						para[83] = new SqlParameter("@MaxBuyHonor", player.MaxBuyHonor);
						para[84] = new SqlParameter("@necklaceExp", player.necklaceExp);
						para[85] = new SqlParameter("@necklaceExpAdd", player.necklaceExpAdd);
						para[86] = new SqlParameter("@GhostEquipList", player.GhostEquipList);
						para[87] = new SqlParameter("@fineSuitExp", player.fineSuitExp);
						sqlParameters2 = para;
						this.db.RunProcedure("SP_Users_Update", para);
						flag = ((int)para[21].Value == 0);
						bool flag4 = flag;
						if (flag4)
						{
							player.AntiAddiction = (int)para[20].Value;
							player.CheckCount = (int)para[24].Value;
						}
						player.IsDirty = false;
						result = flag;
					}
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					foreach (SqlParameter item in sqlParameters2)
					{
						BaseBussiness.log.Info(string.Concat(new object[]
						{
							"Error ",
							item.ParameterName,
							"=",
							item.Value
						}));
					}
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0001DB98 File Offset: 0x0001BD98
		public bool UpdatePlayerGotRingProp(int groomID, int brideID)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@GroomID", groomID),
					new SqlParameter("@BrideID", brideID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[2].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Update_GotRing_Prop", sqlParameters);
				flag = ((int)sqlParameters[2].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdatePlayerGotRingProp", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0001DC54 File Offset: 0x0001BE54
		public bool UpdatePlayerLastAward(int id, int type)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", id),
					new SqlParameter("@Type", type)
				};
				flag = this.db.RunProcedure("SP_Users_LastAward", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdatePlayerAward", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0001DCE8 File Offset: 0x0001BEE8
		public bool UpdatePlayerMarry(PlayerInfo player)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", player.ID),
					new SqlParameter("@IsMarried", player.IsMarried),
					new SqlParameter("@SpouseID", player.SpouseID),
					new SqlParameter("@SpouseName", player.SpouseName),
					new SqlParameter("@IsCreatedMarryRoom", player.IsCreatedMarryRoom),
					new SqlParameter("@SelfMarryRoomID", player.SelfMarryRoomID),
					new SqlParameter("@IsGotRing", player.IsGotRing)
				};
				flag = this.db.RunProcedure("SP_Users_Marry", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdatePlayerMarry", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0001DDF8 File Offset: 0x0001BFF8
		public bool UpdatePlayerMarryApply(int UserID, string loveProclamation, bool isExist)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserID),
					new SqlParameter("@LoveProclamation", loveProclamation),
					new SqlParameter("@isExist", isExist),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[3].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Update_Marry_Apply", sqlParameters);
				flag = ((int)sqlParameters[3].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdatePlayerMarryApply", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0001DEC0 File Offset: 0x0001C0C0
		public bool UpdateUserMatchInfo(UserMatchInfo info)
		{
			bool flag = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@dailyScore", info.dailyScore),
					new SqlParameter("@dailyWinCount", info.dailyWinCount),
					new SqlParameter("@dailyGameCount", info.dailyGameCount),
					new SqlParameter("@DailyLeagueFirst", info.DailyLeagueFirst),
					new SqlParameter("@DailyLeagueLastScore", info.DailyLeagueLastScore),
					new SqlParameter("@weeklyScore", info.weeklyScore),
					new SqlParameter("@weeklyGameCount", info.weeklyGameCount),
					new SqlParameter("@weeklyRanking", info.weeklyRanking),
					new SqlParameter("@addDayPrestge", info.addDayPrestge),
					new SqlParameter("@totalPrestige", info.totalPrestige),
					new SqlParameter("@restCount", info.restCount),
					new SqlParameter("@leagueGrade", info.leagueGrade),
					new SqlParameter("@leagueItemsGet", info.leagueItemsGet),
					new SqlParameter("@WeeklyWinCount", info.WeeklyWinCount),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[16].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateUserMatch", para);
				flag = ((int)para[16].Value == 0);
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UpdateUserMatch", exception);
				}
			}
			return flag;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0001E0EC File Offset: 0x0001C2EC
		public bool UpdateUserRank(UserRankInfo item)
		{
			bool result = false;
			try
			{
				SqlParameter[] array = new SqlParameter[16];
				array[0] = new SqlParameter("@ID", item.ID);
				array[1] = new SqlParameter("@UserID", item.UserID);
				array[2] = new SqlParameter("@UserRank", item.Name);
				array[3] = new SqlParameter("@Attack", item.Attack);
				array[4] = new SqlParameter("@Defence", item.Defence);
				array[5] = new SqlParameter("@Luck", item.Luck);
				array[6] = new SqlParameter("@Agility", item.Agility);
				array[7] = new SqlParameter("@HP", item.HP);
				array[8] = new SqlParameter("@Damage", item.Damage);
				array[9] = new SqlParameter("@Guard", item.Guard);
				array[10] = new SqlParameter("@BeginDate", item.BeginDate);
				array[11] = new SqlParameter("@Validate", item.Validate);
				array[12] = new SqlParameter("@IsExit", item.IsExit);
				array[13] = new SqlParameter("@Result", SqlDbType.Int);
				SqlParameter[] para = array;
				para[13].Direction = ParameterDirection.ReturnValue;
				para[14] = new SqlParameter("@NewTitleID", item.NewTitleID);
				para[15] = new SqlParameter("@EndDate", item.EndDate);
				this.db.RunProcedure("SP_UpdateUserRank", para);
				result = ((int)para[13].Value == 0);
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UpdateUserRank", exception);
				}
			}
			return result;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0001E2F8 File Offset: 0x0001C4F8
		public bool UpdateUserExtra(UsersExtraInfo ex)
		{
			bool flag = false;
			bool result;
			try
			{
				flag = this.db.RunProcedure("SP_Update_User_Extra", new SqlParameter[]
				{
					new SqlParameter("@UserID", ex.UserID),
					new SqlParameter("@LastTimeHotSpring", ex.LastTimeHotSpring),
					new SqlParameter("@MinHotSpring", ex.MinHotSpring),
					new SqlParameter("@coupleBossEnterNum", ex.coupleBossEnterNum),
					new SqlParameter("@coupleBossHurt", ex.coupleBossHurt),
					new SqlParameter("@coupleBossBoxNum", ex.coupleBossBoxNum),
					new SqlParameter("@LastFreeTimeHotSpring", ex.LastFreeTimeHotSpring),
					new SqlParameter("@isGetAwardMarry", ex.isGetAwardMarry),
					new SqlParameter("@isFirstAwardMarry", ex.isFirstAwardMarry),
					new SqlParameter("@LeftRoutteCount", ex.LeftRoutteCount),
					new SqlParameter("@LeftRoutteRate", ex.LeftRoutteRate),
					new SqlParameter("@FreeSendMailCount", ex.FreeSendMailCount)
				});
				result = flag;
			}
			catch (Exception ex2)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex2);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0001E490 File Offset: 0x0001C690
		public bool UpdateUserTexpInfo(TexpInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@attTexpExp", info.attTexpExp),
					new SqlParameter("@defTexpExp", info.defTexpExp),
					new SqlParameter("@hpTexpExp", info.hpTexpExp),
					new SqlParameter("@lukTexpExp", info.lukTexpExp),
					new SqlParameter("@spdTexpExp", info.spdTexpExp),
					new SqlParameter("@texpCount", info.texpCount),
					new SqlParameter("@texpTaskCount", info.texpTaskCount),
					new SqlParameter("@texpTaskDate", info.texpTaskDate),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[9].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UserTexp_Update", sqlParameters);
				flag = ((int)sqlParameters[9].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0001E60C File Offset: 0x0001C80C
		public bool UpdateVIPInfo(PlayerInfo p)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", p.ID),
					new SqlParameter("@VIPLevel", p.VIPLevel),
					new SqlParameter("@VIPExp", p.VIPExp),
					new SqlParameter("@VIPOnlineDays", SqlDbType.BigInt),
					new SqlParameter("@VIPOfflineDays", SqlDbType.BigInt),
					new SqlParameter("@VIPExpireDay", p.VIPExpireDay),
					new SqlParameter("@VIPLastDate", DateTime.Now),
					new SqlParameter("@VIPNextLevelDaysNeeded", p.GetVIPNextLevelDaysNeeded(p.VIPLevel, p.VIPExp)),
					new SqlParameter("@CanTakeVipReward", p.CanTakeVipReward),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[9].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateVIPInfo", sqlParameters);
				flag = true;
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UpdateVIPInfo", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0001E770 File Offset: 0x0001C970
		public int VIPLastdate(int ID)
		{
			int num = 0;
			int result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", ID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[1].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_VIPLastdate_Single", sqlParameters);
				num = (int)sqlParameters[1].Value;
				result = num;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_VIPLastdate_Single", exception);
					result = num;
				}
				else
				{
					result = num;
				}
			}
			return result;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0001E814 File Offset: 0x0001CA14
		public int VIPRenewal(string nickName, int renewalDays, int typeVIP, ref DateTime ExpireDayOut)
		{
			int num = 0;
			int result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@NickName", nickName),
					new SqlParameter("@RenewalDays", renewalDays),
					new SqlParameter("@ExpireDayOut", DateTime.Now),
					new SqlParameter("@typeVIP", typeVIP),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[2].Direction = ParameterDirection.Output;
				sqlParameters[4].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_VIPRenewal_Single", sqlParameters);
				ExpireDayOut = (DateTime)sqlParameters[2].Value;
				num = (int)sqlParameters[4].Value;
				result = num;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_VIPRenewal_Single", exception);
					result = num;
				}
				else
				{
					result = num;
				}
			}
			return result;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0001E910 File Offset: 0x0001CB10
		public bool UpdateAcademyPlayer(PlayerInfo player)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", player.ID),
					new SqlParameter("@apprenticeshipState", player.apprenticeshipState),
					new SqlParameter("@masterID", player.masterID),
					new SqlParameter("@masterOrApprentices", player.masterOrApprentices),
					new SqlParameter("@graduatesCount", player.graduatesCount),
					new SqlParameter("@honourOfMaster", player.honourOfMaster),
					null,
					new SqlParameter("@freezesDate", player.freezesDate)
				};
				SqlParameters[6] = new SqlParameter("@Result", SqlDbType.Int);
				SqlParameters[6].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UsersAcademy_Update", SqlParameters);
				flag = ((int)SqlParameters[6].Value == 0);
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdateAcademyPlayer", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0001EA44 File Offset: 0x0001CC44
		public void AddDailyRecord(DailyRecordInfo info)
		{
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@Type", info.Type),
					new SqlParameter("@Value", info.Value)
				};
				this.db.RunProcedure("SP_DailyRecordInfo_Add", sqlParameters);
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("AddDailyRecord", exception);
				}
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0001EAE8 File Offset: 0x0001CCE8
		public bool DeleteDailyRecord(int UserID, int Type)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserID),
					new SqlParameter("@Type", Type)
				};
				flag = this.db.RunProcedure("SP_DailyRecordInfo_Delete", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_DailyRecordInfo_Delete", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0001EB7C File Offset: 0x0001CD7C
		public DailyRecordInfo[] GetDailyRecord(int UserID)
		{
			List<DailyRecordInfo> list = new List<DailyRecordInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserID)
				};
				this.db.GetReader(ref resultDataReader, "SP_DailyRecordInfo_Single", sqlParameters);
				while (resultDataReader.Read())
				{
					DailyRecordInfo item = new DailyRecordInfo
					{
						UserID = (int)resultDataReader["UserID"],
						Type = (int)resultDataReader["Type"],
						Value = (string)resultDataReader["Value"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetDailyRecord", exception);
				}
			}
			finally
			{
				bool flag = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0001EC9C File Offset: 0x0001CE9C
		public string GetASSInfoSingle(int UserID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserID)
				};
				this.db.GetReader(ref resultDataReader, "SP_ASSInfo_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return resultDataReader["IDNumber"].ToString();
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetASSInfoSingle", exception);
				}
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return "";
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0001ED70 File Offset: 0x0001CF70
		public DailyLogListInfo GetDailyLogListSingle(int UserID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserID)
				};
				this.db.GetReader(ref resultDataReader, "SP_DailyLogList_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return new DailyLogListInfo
					{
						ID = (int)resultDataReader["ID"],
						UserID = (int)resultDataReader["UserID"],
						UserAwardLog = (int)resultDataReader["UserAwardLog"],
						DayLog = (string)resultDataReader["DayLog"],
						LastDate = (DateTime)resultDataReader["LastDate"]
					};
				}
			}
			catch (Exception exception)
			{
				BaseBussiness.log.Error("DailyLogList", exception);
			}
			finally
			{
				bool flag2 = resultDataReader != null && !resultDataReader.IsClosed;
				if (flag2)
				{
					resultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0001EE94 File Offset: 0x0001D094
		public bool UpdateDailyLogList(DailyLogListInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@UserAwardLog", info.UserAwardLog),
					new SqlParameter("@DayLog", info.DayLog),
					new SqlParameter("@LastDate", info.LastDate),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[4].Direction = ParameterDirection.ReturnValue;
				flag = this.db.RunProcedure("SP_DailyLogList_Update", sqlParameters);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_DailyLogList_Update", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0001EF74 File Offset: 0x0001D174
		public bool UpdateBoxProgression(int userid, int boxProgression, int getBoxLevel, DateTime addGPLastDate, DateTime BoxGetDate, int alreadyBox)
		{
			bool result = false;
			bool result2;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", userid),
					new SqlParameter("@BoxProgression", boxProgression),
					new SqlParameter("@GetBoxLevel", getBoxLevel),
					new SqlParameter("@AddGPLastDate", DateTime.Now),
					new SqlParameter("@BoxGetDate", BoxGetDate),
					new SqlParameter("@AlreadyGetBox", alreadyBox)
				};
				result = this.db.RunProcedure("SP_User_Update_BoxProgression", para);
				result2 = result;
			}
			catch (Exception e)
			{
				BaseBussiness.log.Error("User_Update_BoxProgression", e);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0001F044 File Offset: 0x0001D244
		public bool UpdatePlayerInfoHistory(PlayerInfoHistory info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@LastQuestsTime", info.LastQuestsTime),
					new SqlParameter("@LastTreasureTime", info.LastTreasureTime),
					new SqlParameter("@OutPut", SqlDbType.Int)
				};
				sqlParameters[3].Direction = ParameterDirection.Output;
				this.db.RunProcedure("SP_User_Update_History", sqlParameters);
				flag = ((int)sqlParameters[6].Value == 1);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("User_Update_BoxProgression", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0001F120 File Offset: 0x0001D320
		public bool AddAASInfo(AASInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@Name", info.Name),
					new SqlParameter("@IDNumber", info.IDNumber),
					new SqlParameter("@State", info.State),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameters[4].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_ASSInfo_Add", sqlParameters);
				flag = ((int)sqlParameters[4].Value == 0);
				result = flag;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdateAASInfo", exception);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0001F20C File Offset: 0x0001D40C
		public void AddUserLogEvent(int UserID, string UserName, string NickName, string Type, string Content)
		{
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserID),
					new SqlParameter("@UserName", UserName),
					new SqlParameter("@NickName", NickName),
					new SqlParameter("@Type", Type),
					new SqlParameter("@Content", Content)
				};
				this.db.RunProcedure("SP_Insert_UsersLog", sqlParameters);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0001F29C File Offset: 0x0001D49C
		public Dictionary<int, List<string>> LoadCommands()
		{
			SqlDataReader sqlDataReader = null;
			Dictionary<int, List<string>> commands = new Dictionary<int, List<string>>();
			this.db.GetReader(ref sqlDataReader, "SP_GetAllCommands");
			while (sqlDataReader.Read())
			{
				string[] array = Convert.ToString(sqlDataReader["Commands"] ?? "").Split(new char[]
				{
					'$'
				});
				List<string> c = new List<string>();
				string[] array2 = array;
				string[] array3 = array2;
				foreach (string s in array3)
				{
					c.Add(s);
				}
				bool flag = !commands.ContainsKey(Convert.ToInt32(sqlDataReader["UserID"] ?? 0));
				if (flag)
				{
					commands.Add(Convert.ToInt32(sqlDataReader["UserID"] ?? 0), c);
				}
			}
			return commands;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0001F390 File Offset: 0x0001D590
		public bool AddUserAdoptPet(UsersPetInfo info, bool isUse)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@TemplateID", info.TemplateID),
					new SqlParameter("@Name", (info.Name == null) ? "Error!" : info.Name),
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@Attack", info.Attack),
					new SqlParameter("@Defence", info.Defence),
					new SqlParameter("@Luck", info.Luck),
					new SqlParameter("@Agility", info.Agility),
					new SqlParameter("@Blood", info.Blood),
					new SqlParameter("@Damage", info.Damage),
					new SqlParameter("@Guard", info.Guard),
					new SqlParameter("@AttackGrow", info.AttackGrow),
					new SqlParameter("@DefenceGrow", info.DefenceGrow),
					new SqlParameter("@LuckGrow", info.LuckGrow),
					new SqlParameter("@AgilityGrow", info.AgilityGrow),
					new SqlParameter("@BloodGrow", info.BloodGrow),
					new SqlParameter("@DamageGrow", info.DamageGrow),
					new SqlParameter("@GuardGrow", info.GuardGrow),
					new SqlParameter("@Skill", info.Skill),
					new SqlParameter("@SkillEquip", info.SkillEquip),
					new SqlParameter("@Place", info.Place),
					new SqlParameter("@IsExit", info.IsExit),
					new SqlParameter("@IsUse", isUse),
					new SqlParameter("@ID", info.ID)
				};
				para[22].Direction = ParameterDirection.Output;
				result = this.db.RunProcedure("SP_User_AdoptPet", para);
				info.ID = (int)para[22].Value;
				info.IsDirty = false;
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			return result;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0001F660 File Offset: 0x0001D860
		public bool RemoveUserAdoptPet(int ID)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", ID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[1].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Remove_User_AdoptPet", para);
				int returnValue = (int)para[1].Value;
				result = (returnValue == 0);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			return result;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0001F70C File Offset: 0x0001D90C
		public bool UpdateUserAdoptPet(int ID)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", ID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[1].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Update_User_AdoptPet", para);
				int returnValue = (int)para[1].Value;
				result = (returnValue == 0);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			return result;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0001F7B8 File Offset: 0x0001D9B8
		public bool ClearAdoptPet(int ID)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", ID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[1].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Clear_AdoptPet", para);
				int returnValue = (int)para[1].Value;
				result = (returnValue == 0);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			return result;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0001F864 File Offset: 0x0001DA64
		public UsersPetInfo[] GetUserPetSingles(int UserID, int vipLv)
		{
			List<UsersPetInfo> items = new List<UsersPetInfo>();
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				para[0].Value = UserID;
				this.db.GetReader(ref reader, "SP_Get_UserPet_By_ID", para);
				while (reader.Read())
				{
					UsersPetInfo info = this.InitPet(reader);
					info.VIPLevel = vipLv;
					items.Add(info);
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return items.ToArray();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0001F954 File Offset: 0x0001DB54
		public bool UpdateUserPet(UsersPetInfo item)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[39];
				array[0] = new SqlParameter("@TemplateID", item.TemplateID);
				array[1] = new SqlParameter("@Name", (item.Name == null) ? "Error!" : item.Name);
				array[2] = new SqlParameter("@UserID", item.UserID);
				array[3] = new SqlParameter("@Attack", item.Attack);
				array[4] = new SqlParameter("@Defence", item.Defence);
				array[5] = new SqlParameter("@Luck", item.Luck);
				array[6] = new SqlParameter("@Agility", item.Agility);
				array[7] = new SqlParameter("@Blood", item.Blood);
				array[8] = new SqlParameter("@Damage", item.Damage);
				array[9] = new SqlParameter("@Guard", item.Guard);
				array[10] = new SqlParameter("@AttackGrow", item.AttackGrow);
				array[11] = new SqlParameter("@DefenceGrow", item.DefenceGrow);
				array[12] = new SqlParameter("@LuckGrow", item.LuckGrow);
				array[13] = new SqlParameter("@AgilityGrow", item.AgilityGrow);
				array[14] = new SqlParameter("@BloodGrow", item.BloodGrow);
				array[15] = new SqlParameter("@DamageGrow", item.DamageGrow);
				array[16] = new SqlParameter("@GuardGrow", item.GuardGrow);
				array[17] = new SqlParameter("@Level", item.Level);
				array[18] = new SqlParameter("@GP", item.GP);
				array[19] = new SqlParameter("@MaxGP", item.MaxGP);
				array[20] = new SqlParameter("@Hunger", item.Hunger);
				array[21] = new SqlParameter("@PetHappyStar", item.PetHappyStar);
				array[22] = new SqlParameter("@MP", item.MP);
				array[23] = new SqlParameter("@IsEquip", item.IsEquip);
				array[24] = new SqlParameter("@Place", item.Place);
				array[25] = new SqlParameter("@IsExit", item.IsExit);
				array[26] = new SqlParameter("@ID", item.ID);
				array[27] = new SqlParameter("@Skill", item.Skill);
				array[28] = new SqlParameter("@SkillEquip", item.SkillEquip);
				array[29] = new SqlParameter("@currentStarExp", item.currentStarExp);
				array[30] = new SqlParameter("@Result", SqlDbType.Int);
				SqlParameter[] SqlParameters = array;
				SqlParameters[30].Direction = ParameterDirection.ReturnValue;
				SqlParameters[31] = new SqlParameter("@breakGrade", item.breakGrade);
				SqlParameters[32] = new SqlParameter("@breakAttack", item.breakAttack);
				SqlParameters[33] = new SqlParameter("@breakDefence", item.breakDefence);
				SqlParameters[34] = new SqlParameter("@breakAgility", item.breakAgility);
				SqlParameters[35] = new SqlParameter("@breakLuck", item.breakLuck);
				SqlParameters[36] = new SqlParameter("@breakBlood", item.breakBlood);
				SqlParameters[37] = new SqlParameter("@eQPets", item.eQPets);
				SqlParameters[38] = new SqlParameter("@BaseProp", item.BaseProp);
				this.db.RunProcedure("SP_UserPet_Update", SqlParameters);
				flag = ((int)SqlParameters[30].Value == 0);
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0001FD9C File Offset: 0x0001DF9C
		public bool AddUserPet(UsersPetInfo item)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] array = new SqlParameter[39];
				array[0] = new SqlParameter("@TemplateID", item.TemplateID);
				array[1] = new SqlParameter("@Name", (item.Name == null) ? "Error!" : item.Name);
				array[2] = new SqlParameter("@UserID", item.UserID);
				array[3] = new SqlParameter("@Attack", item.Attack);
				array[4] = new SqlParameter("@Defence", item.Defence);
				array[5] = new SqlParameter("@Luck", item.Luck);
				array[6] = new SqlParameter("@Agility", item.Agility);
				array[7] = new SqlParameter("@Blood", item.Blood);
				array[8] = new SqlParameter("@Damage", item.Damage);
				array[9] = new SqlParameter("@Guard", item.Guard);
				array[10] = new SqlParameter("@AttackGrow", item.AttackGrow);
				array[11] = new SqlParameter("@DefenceGrow", item.DefenceGrow);
				array[12] = new SqlParameter("@LuckGrow", item.LuckGrow);
				array[13] = new SqlParameter("@AgilityGrow", item.AgilityGrow);
				array[14] = new SqlParameter("@BloodGrow", item.BloodGrow);
				array[15] = new SqlParameter("@DamageGrow", item.DamageGrow);
				array[16] = new SqlParameter("@GuardGrow", item.GuardGrow);
				array[17] = new SqlParameter("@Level", item.Level);
				array[18] = new SqlParameter("@GP", item.GP);
				array[19] = new SqlParameter("@MaxGP", item.MaxGP);
				array[20] = new SqlParameter("@Hunger", item.Hunger);
				array[21] = new SqlParameter("@PetHappyStar", item.PetHappyStar);
				array[22] = new SqlParameter("@MP", item.MP);
				array[23] = new SqlParameter("@IsEquip", item.IsEquip);
				array[24] = new SqlParameter("@Skill", item.Skill);
				array[25] = new SqlParameter("@SkillEquip", item.SkillEquip);
				array[26] = new SqlParameter("@Place", item.Place);
				array[27] = new SqlParameter("@IsExit", item.IsExit);
				array[28] = new SqlParameter("@ID", item.ID);
				SqlParameter[] SqlParameters = array;
				SqlParameters[28].Direction = ParameterDirection.Output;
				SqlParameters[29] = new SqlParameter("@currentStarExp", item.currentStarExp);
				SqlParameters[30] = new SqlParameter("@Result", SqlDbType.Int);
				SqlParameters[30].Direction = ParameterDirection.ReturnValue;
				SqlParameters[31] = new SqlParameter("@breakGrade", item.breakGrade);
				SqlParameters[32] = new SqlParameter("@breakAttack", item.breakAttack);
				SqlParameters[33] = new SqlParameter("@breakDefence", item.breakDefence);
				SqlParameters[34] = new SqlParameter("@breakAgility", item.breakAgility);
				SqlParameters[35] = new SqlParameter("@breakLuck", item.breakLuck);
				SqlParameters[36] = new SqlParameter("@breakBlood", item.breakBlood);
				SqlParameters[37] = new SqlParameter("@eQPets", item.eQPets);
				SqlParameters[38] = new SqlParameter("@BaseProp", item.BaseProp);
				flag = this.db.RunProcedure("SP_User_Add_Pet", SqlParameters);
				flag = ((int)SqlParameters[30].Value == 0);
				item.ID = (int)SqlParameters[28].Value;
				item.IsDirty = false;
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0002020C File Offset: 0x0001E40C
		public UsersPetInfo InitPet(SqlDataReader reader)
		{
			return new UsersPetInfo
			{
				ID = (int)reader["ID"],
				TemplateID = (int)reader["TemplateID"],
				Name = reader["Name"].ToString(),
				UserID = (int)reader["UserID"],
				Attack = (int)reader["Attack"],
				AttackGrow = (int)reader["AttackGrow"],
				Agility = (int)reader["Agility"],
				AgilityGrow = (int)reader["AgilityGrow"],
				Defence = (int)reader["Defence"],
				DefenceGrow = (int)reader["DefenceGrow"],
				Luck = (int)reader["Luck"],
				LuckGrow = (int)reader["LuckGrow"],
				Blood = (int)reader["Blood"],
				BloodGrow = (int)reader["BloodGrow"],
				Damage = (int)reader["Damage"],
				DamageGrow = (int)reader["DamageGrow"],
				Guard = (int)reader["Guard"],
				GuardGrow = (int)reader["GuardGrow"],
				Level = (int)reader["Level"],
				GP = (int)reader["GP"],
				MaxGP = (int)reader["MaxGP"],
				Hunger = (int)reader["Hunger"],
				MP = (int)reader["MP"],
				Place = (int)reader["Place"],
				IsEquip = (bool)reader["IsEquip"],
				IsExit = (bool)reader["IsExit"],
				Skill = reader["Skill"].ToString(),
				SkillEquip = reader["SkillEquip"].ToString(),
				currentStarExp = (int)reader["currentStarExp"],
				breakGrade = (int)reader["breakGrade"],
				breakAttack = (int)reader["breakAttack"],
				breakDefence = (int)reader["breakDefence"],
				breakAgility = (int)reader["breakAgility"],
				breakLuck = (int)reader["breakLuck"],
				breakBlood = (int)reader["breakBlood"],
				eQPets = ((reader["eQPets"] == null) ? "" : reader["eQPets"].ToString()),
				BaseProp = ((reader["BaseProp"] == null) ? "" : reader["BaseProp"].ToString())
			};
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000205A0 File Offset: 0x0001E7A0
		public bool RegisterPlayer2(string userName, string passWord, string nickName, int attack, int defence, int agility, int luck, int cateogryId, string bStyle, string bPic, string gStyle, string armColor, string hairColor, string faceColor, string clothColor, string hatchColor, int sex, ref string msg, int validDate)
		{
			bool flag = false;
			bool result;
			try
			{
				string[] strArray = bStyle.Split(new char[]
				{
					','
				});
				string[] strArray2 = gStyle.Split(new char[]
				{
					','
				});
				string[] strArray3 = bPic.Split(new char[]
				{
					','
				});
				SqlParameter[] SqlParameters = new SqlParameter[31];
				SqlParameters[0] = new SqlParameter("@UserName", userName);
				SqlParameters[1] = new SqlParameter("@PassWord", passWord);
				SqlParameters[2] = new SqlParameter("@NickName", nickName);
				SqlParameters[3] = new SqlParameter("@BArmID", int.Parse(strArray[0]));
				SqlParameters[4] = new SqlParameter("@BHairID", int.Parse(strArray[1]));
				SqlParameters[5] = new SqlParameter("@BFaceID", int.Parse(strArray[2]));
				SqlParameters[6] = new SqlParameter("@BClothID", int.Parse(strArray[3]));
				SqlParameters[7] = new SqlParameter("@BHatID", int.Parse(strArray[4]));
				SqlParameters[21] = new SqlParameter("@ArmPic", strArray3[0]);
				SqlParameters[22] = new SqlParameter("@HairPic", strArray3[1]);
				SqlParameters[23] = new SqlParameter("@FacePic", strArray3[2]);
				SqlParameters[24] = new SqlParameter("@ClothPic", strArray3[3]);
				SqlParameters[25] = new SqlParameter("@HatPic", strArray3[4]);
				SqlParameters[8] = new SqlParameter("@GArmID", int.Parse(strArray2[0]));
				SqlParameters[9] = new SqlParameter("@GHairID", int.Parse(strArray2[1]));
				SqlParameters[10] = new SqlParameter("@GFaceID", int.Parse(strArray2[2]));
				SqlParameters[11] = new SqlParameter("@GClothID", int.Parse(strArray2[3]));
				SqlParameters[12] = new SqlParameter("@GHatID", int.Parse(strArray2[4]));
				SqlParameters[13] = new SqlParameter("@ArmColor", armColor);
				SqlParameters[14] = new SqlParameter("@HairColor", hairColor);
				SqlParameters[15] = new SqlParameter("@FaceColor", faceColor);
				SqlParameters[16] = new SqlParameter("@ClothColor", clothColor);
				SqlParameters[17] = new SqlParameter("@HatColor", clothColor);
				SqlParameters[18] = new SqlParameter("@Sex", sex);
				SqlParameters[19] = new SqlParameter("@StyleDate", validDate);
				SqlParameters[26] = new SqlParameter("@CategoryID", cateogryId);
				SqlParameters[27] = new SqlParameter("@Attack", attack);
				SqlParameters[28] = new SqlParameter("@Defence", defence);
				SqlParameters[29] = new SqlParameter("@Agility", agility);
				SqlParameters[30] = new SqlParameter("@Luck", luck);
				SqlParameters[20] = new SqlParameter("@Result", SqlDbType.Int);
				SqlParameters[20].Direction = ParameterDirection.ReturnValue;
				flag = this.db.RunProcedure("SP_Users_RegisterNotValidate2", SqlParameters);
				int num = (int)SqlParameters[20].Value;
				flag = (num == 0);
				int num2 = num;
				int num3 = num2;
				if (num3 != 2)
				{
					if (num3 != 3)
					{
						result = flag;
					}
					else
					{
						msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg3", Array.Empty<object>());
						result = flag;
					}
				}
				else
				{
					msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg2", Array.Empty<object>());
					result = flag;
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}", new object[]
					{
						userName,
						passWord,
						nickName,
						attack,
						defence,
						agility,
						luck,
						cateogryId,
						bStyle,
						bPic,
						gStyle
					}));
					BaseBussiness.log.Error("Init", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000209D0 File Offset: 0x0001EBD0
		public bool UpdateUserReputeFightPower()
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Result", SqlDbType.Int)
				};
				SqlParameters[0].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Update_Repute_FightPower", SqlParameters);
				flag = ((int)SqlParameters[0].Value == 0);
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00020A64 File Offset: 0x0001EC64
		public UsersExtraInfo[] GetRankCaddy()
		{
			List<UsersExtraInfo> userExtraInfoList = new List<UsersExtraInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_Get_Rank_Caddy");
				while (ResultDataReader.Read())
				{
					userExtraInfoList.Add(new UsersExtraInfo
					{
						UserID = (int)ResultDataReader["UserID"],
						NickName = (string)ResultDataReader["NickName"],
						TotalCaddyOpen = (int)ResultDataReader["badLuckNumber"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Get_Rank_Caddy", ex);
				}
			}
			finally
			{
				bool flag = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag)
				{
					ResultDataReader.Close();
				}
			}
			return userExtraInfoList.ToArray();
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00020B64 File Offset: 0x0001ED64
		public UserLabyrinthInfo GetSingleLabyrinth(int ID)
		{
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				SqlParameters[0].Value = ID;
				this.db.GetReader(ref ResultDataReader, "SP_GetSingleLabyrinth", SqlParameters);
				bool flag = ResultDataReader.Read();
				if (flag)
				{
					return new UserLabyrinthInfo
					{
						UserID = (int)ResultDataReader["UserID"],
						myProgress = (int)ResultDataReader["myProgress"],
						myRanking = (int)ResultDataReader["myRanking"],
						completeChallenge = (bool)ResultDataReader["completeChallenge"],
						isDoubleAward = (bool)ResultDataReader["isDoubleAward"],
						currentFloor = (int)ResultDataReader["currentFloor"],
						accumulateExp = (int)ResultDataReader["accumulateExp"],
						remainTime = (int)ResultDataReader["remainTime"],
						currentRemainTime = (int)ResultDataReader["currentRemainTime"],
						cleanOutAllTime = (int)ResultDataReader["cleanOutAllTime"],
						cleanOutGold = (int)ResultDataReader["cleanOutGold"],
						tryAgainComplete = (bool)ResultDataReader["tryAgainComplete"],
						isInGame = (bool)ResultDataReader["isInGame"],
						isCleanOut = (bool)ResultDataReader["isCleanOut"],
						serverMultiplyingPower = (bool)ResultDataReader["serverMultiplyingPower"],
						LastDate = (DateTime)ResultDataReader["LastDate"],
						ProcessAward = (string)ResultDataReader["ProcessAward"]
					};
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_GetSingleUserLabyrinth", ex);
				}
			}
			finally
			{
				bool flag2 = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag2)
				{
					ResultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00020DD4 File Offset: 0x0001EFD4
		public bool AddUserLabyrinth(UserLabyrinthInfo laby)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", laby.UserID),
					new SqlParameter("@myProgress", laby.myProgress),
					new SqlParameter("@myRanking", laby.myRanking),
					new SqlParameter("@completeChallenge", laby.completeChallenge),
					new SqlParameter("@isDoubleAward", laby.isDoubleAward),
					new SqlParameter("@currentFloor", laby.currentFloor),
					new SqlParameter("@accumulateExp", laby.accumulateExp),
					new SqlParameter("@remainTime", laby.remainTime),
					new SqlParameter("@currentRemainTime", laby.currentRemainTime),
					new SqlParameter("@cleanOutAllTime", laby.cleanOutAllTime),
					new SqlParameter("@cleanOutGold", laby.cleanOutGold),
					new SqlParameter("@tryAgainComplete", laby.tryAgainComplete),
					new SqlParameter("@isInGame", laby.isInGame),
					new SqlParameter("@isCleanOut", laby.isCleanOut),
					new SqlParameter("@serverMultiplyingPower", laby.serverMultiplyingPower),
					new SqlParameter("@LastDate", laby.LastDate),
					new SqlParameter("@ProcessAward", laby.ProcessAward),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				SqlParameters[17].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Users_Labyrinth_Add", SqlParameters);
				flag = ((int)SqlParameters[17].Value == 0);
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00021014 File Offset: 0x0001F214
		public bool UpdateLabyrinthInfo(UserLabyrinthInfo laby)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", laby.UserID),
					new SqlParameter("@myProgress", laby.myProgress),
					new SqlParameter("@myRanking", laby.myRanking),
					new SqlParameter("@completeChallenge", laby.completeChallenge),
					new SqlParameter("@isDoubleAward", laby.isDoubleAward),
					new SqlParameter("@currentFloor", laby.currentFloor),
					new SqlParameter("@accumulateExp", laby.accumulateExp),
					new SqlParameter("@remainTime", laby.remainTime),
					new SqlParameter("@currentRemainTime", laby.currentRemainTime),
					new SqlParameter("@cleanOutAllTime", laby.cleanOutAllTime),
					new SqlParameter("@cleanOutGold", laby.cleanOutGold),
					new SqlParameter("@tryAgainComplete", laby.tryAgainComplete),
					new SqlParameter("@isInGame", laby.isInGame),
					new SqlParameter("@isCleanOut", laby.isCleanOut),
					new SqlParameter("@serverMultiplyingPower", laby.serverMultiplyingPower),
					new SqlParameter("@LastDate", laby.LastDate),
					new SqlParameter("@ProcessAward", laby.ProcessAward),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				SqlParameters[17].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateLabyrinthInfo", SqlParameters);
				flag = true;
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UpdateLabyrinthInfo", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00021244 File Offset: 0x0001F444
		public UserGiftInfo[] GetAllUserGifts(int userid, bool isReceive)
		{
			List<UserGiftInfo> userGiftInfoList = new List<UserGiftInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", userid),
					new SqlParameter("@IsReceive", isReceive)
				};
				this.db.GetReader(ref ResultDataReader, "SP_Users_Gift_Single", SqlParameters);
				while (ResultDataReader.Read())
				{
					userGiftInfoList.Add(new UserGiftInfo
					{
						ID = (int)ResultDataReader["ID"],
						ReceiverID = (int)ResultDataReader["ReceiverID"],
						SenderID = (int)ResultDataReader["SenderID"],
						TemplateID = (int)ResultDataReader["TemplateID"],
						Count = (int)ResultDataReader["Count"],
						CreateDate = (DateTime)ResultDataReader["CreateDate"],
						LastUpdate = (DateTime)ResultDataReader["LastUpdate"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetAllUserGifts", ex);
				}
			}
			finally
			{
				bool flag = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag)
				{
					ResultDataReader.Close();
				}
			}
			return userGiftInfoList.ToArray();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000213F0 File Offset: 0x0001F5F0
		public UserGiftInfo[] GetAllUserReceivedGifts(int userid)
		{
			Dictionary<int, UserGiftInfo> dictionary = new Dictionary<int, UserGiftInfo>();
			SqlDataReader sqlDataReader = null;
			try
			{
				UserGiftInfo[] allUserGifts = this.GetAllUserGifts(userid, true);
				bool flag = allUserGifts != null;
				if (flag)
				{
					UserGiftInfo[] array = allUserGifts;
					UserGiftInfo[] array2 = array;
					foreach (UserGiftInfo userGiftInfo in array2)
					{
						bool flag2 = dictionary.ContainsKey(userGiftInfo.TemplateID);
						if (flag2)
						{
							dictionary[userGiftInfo.TemplateID].Count += userGiftInfo.Count;
						}
						else
						{
							dictionary.Add(userGiftInfo.TemplateID, userGiftInfo);
						}
					}
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetAllUserReceivedGifts", ex);
				}
			}
			finally
			{
				bool flag3 = sqlDataReader != null && !sqlDataReader.IsClosed;
				if (flag3)
				{
					sqlDataReader.Close();
				}
			}
			return dictionary.Values.ToArray<UserGiftInfo>();
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0002150C File Offset: 0x0001F70C
		public bool AddUserGift(UserGiftInfo info)
		{
			bool flag = false;
			bool result;
			try
			{
				this.db.RunProcedure("SP_Users_Gift_Add", new SqlParameter[]
				{
					new SqlParameter("@SenderID", info.SenderID),
					new SqlParameter("@ReceiverID", info.ReceiverID),
					new SqlParameter("@TemplateID", info.TemplateID),
					new SqlParameter("@Count", info.Count)
				});
				flag = true;
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("AddUserGift", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000215D8 File Offset: 0x0001F7D8
		public bool UpdateUserCharmGP(int userId, int int_1)
		{
			bool flag = false;
			bool result;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", userId),
					new SqlParameter("@CharmGP", int_1),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				SqlParameters[2].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Users_UpdateCharmGP", SqlParameters);
				flag = ((int)SqlParameters[2].Value == 0);
				result = flag;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("AddUserGift", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00021694 File Offset: 0x0001F894
		public bool ResetEliteGame(int point)
		{
			bool flag = false;
			bool result;
			try
			{
				result = this.db.RunProcedure("SP_EliteGame_Reset", new SqlParameter[]
				{
					new SqlParameter("@EliteScore", point)
				});
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", ex);
					result = flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0002170C File Offset: 0x0001F90C
		public EatPetsInfo GetAllEatPetsByID(int ID)
		{
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				para[0].Value = ID;
				this.db.GetReader(ref reader, "SP_Sys_Eat_Pets_All", para);
				bool flag = reader.Read();
				if (flag)
				{
					return this.InitEatPetsInfo(reader);
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("InitEatPetsInfo", e);
				}
			}
			finally
			{
				bool flag2 = reader != null && !reader.IsClosed;
				if (flag2)
				{
					reader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000217DC File Offset: 0x0001F9DC
		public EatPetsInfo InitEatPetsInfo(SqlDataReader dr)
		{
			return new EatPetsInfo
			{
				ID = (int)dr["ID"],
				UserID = (int)dr["UserID"],
				weaponExp = (int)dr["weaponExp"],
				weaponLevel = (int)dr["weaponLevel"],
				clothesExp = (int)dr["clothesExp"],
				clothesLevel = (int)dr["clothesLevel"],
				hatExp = (int)dr["hatExp"],
				hatLevel = (int)dr["hatLevel"]
			};
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000218B0 File Offset: 0x0001FAB0
		public bool UpdateEatPets(EatPetsInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@weaponExp", info.weaponExp),
					new SqlParameter("@weaponLevel", info.weaponLevel),
					new SqlParameter("@clothesExp", info.clothesExp),
					new SqlParameter("@clothesLevel", info.clothesLevel),
					new SqlParameter("@hatExp", info.hatExp),
					new SqlParameter("@hatLevel", info.hatLevel)
				};
				result = this.db.RunProcedure("SP_Sys_Eat_Pets_Update", para);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Sys_Eat_Pets_Update", e);
				}
			}
			return result;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000219DC File Offset: 0x0001FBDC
		public bool AddEatPets(EatPetsInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@weaponExp", info.weaponExp),
					new SqlParameter("@weaponLevel", info.weaponLevel),
					new SqlParameter("@clothesExp", info.clothesExp),
					new SqlParameter("@clothesLevel", info.clothesLevel),
					new SqlParameter("@hatExp", info.hatExp),
					new SqlParameter("@hatLevel", info.hatLevel)
				};
				para[0].Direction = ParameterDirection.Output;
				result = this.db.RunProcedure("SP_Sys_Eat_Pets_Add", para);
				info.ID = (int)para[0].Value;
				info.IsDirty = false;
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Sys_Eat_Pets_Add", e);
				}
			}
			return result;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00021B38 File Offset: 0x0001FD38
		public Suit_Manager Get_Suit_Manager(int UserID)
		{
			Suit_Manager items = new Suit_Manager();
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para2 = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				para2[0].Value = UserID;
				this.db.GetReader(ref reader, "SP_Suit_Manager_GET", para2);
				while (reader.Read())
				{
					items.UserID = (int)reader["UserID"];
					items.Kill_List = (string)reader["Kill_List"];
				}
			}
			catch (Exception e2)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e2);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			try
			{
				bool flag2 = items.UserID == 0;
				if (flag2)
				{
					try
					{
						SqlParameter[] para3 = new SqlParameter[]
						{
							new SqlParameter("@UserID", UserID)
						};
						this.db.RunProcedure("SP_Suit_Manager_ADD", para3);
					}
					catch (Exception e3)
					{
						bool isErrorEnabled2 = BaseBussiness.log.IsErrorEnabled;
						if (isErrorEnabled2)
						{
							BaseBussiness.log.Error("SP_Suit_Manager_ADD error!", e3);
						}
					}
				}
			}
			catch
			{
			}
			return items;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00021CC8 File Offset: 0x0001FEC8
		public bool UpdateRank()
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[0];
				result = this.db.RunProcedure("SP_Sys_Update_Consortia_DayList", para);
				result = this.db.RunProcedure("SP_Sys_Update_Consortia_FightPower", para);
				result = this.db.RunProcedure("SP_Sys_Update_Consortia_Honor", para);
				result = this.db.RunProcedure("SP_Sys_Update_Consortia_List", para);
				result = this.db.RunProcedure("SP_Sys_Update_Consortia_WeekList", para);
				result = this.db.RunProcedure("SP_Sys_Update_OfferList", para);
				result = this.db.RunProcedure("SP_Sys_Update_Users_DayList", para);
				result = this.db.RunProcedure("SP_Sys_Update_Users_List", para);
				result = this.db.RunProcedure("SP_Sys_Update_Users_WeekList", para);
				result = this.db.RunProcedure("SP_Sys_Update_Users_Rank_Date", para);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init UpdatePersonalRank", e);
				}
			}
			return result;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00021DD4 File Offset: 0x0001FFD4
		public UserRankDateInfo[] GetAllUserRankDate()
		{
			List<UserRankDateInfo> list = new List<UserRankDateInfo>();
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[0];
				this.db.GetReader(ref reader, "SP_Sys_Users_Rank_Date_All", para);
				while (reader.Read())
				{
					list.Add(this.InitUserRankDateInfo(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("InitUserRankDateInfo", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00021E9C File Offset: 0x0002009C
		public UserRankDateInfo GetUserRankDateByID(int userID)
		{
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				para[0].Value = userID;
				this.db.GetReader(ref reader, "SP_Sys_Users_Rank_Date", para);
				bool flag = reader.Read();
				if (flag)
				{
					return this.InitUserRankDateInfo(reader);
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("InitUserRankDateInfo", e);
				}
			}
			finally
			{
				bool flag2 = reader != null && !reader.IsClosed;
				if (flag2)
				{
					reader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00021F6C File Offset: 0x0002016C
		public UserRankDateInfo InitUserRankDateInfo(SqlDataReader dr)
		{
			return new UserRankDateInfo
			{
				UserID = (int)dr["UserID"],
				ConsortiaID = (int)dr["ConsortiaID"],
				FightPower = (int)dr["FightPower"],
				PrevFightPower = (int)dr["PrevFightPower"],
				GP = (int)dr["GP"],
				PrevGP = (int)dr["PrevGP"],
				AchievementPoint = (int)dr["AchievementPoint"],
				PrevAchievementPoint = (int)dr["PrevAchievementPoint"],
				charmGP = (int)dr["charmGP"],
				PrecharmGP = (int)dr["PrecharmGP"],
				LeagueAddWeek = (int)dr["LeagueAddWeek"],
				PrevLeagueAddWeek = (int)dr["PrevLeagueAddWeek"],
				ConsortiaFightPower = (int)dr["ConsortiaFightPower"],
				ConsortiaPrevFightPower = (int)dr["ConsortiaPrevFightPower"],
				ConsortiaLevel = (int)dr["ConsortiaLevel"],
				ConsortiaPrevLevel = (int)dr["ConsortiaPrevLevel"],
				ConsortiaRiches = (int)dr["ConsortiaRiches"],
				ConsortiaPrevRiches = (int)dr["ConsortiaPrevRiches"],
				ConsortiacharmGP = (int)dr["ConsortiacharmGP"],
				ConsortiaPrevcharmGP = (int)dr["ConsortiaPrevcharmGP"]
			};
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00022154 File Offset: 0x00020354
		public UsersPetInfo[] GetUserAdoptPetSingles(int UserID)
		{
			List<UsersPetInfo> items = new List<UsersPetInfo>();
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				para[0].Value = UserID;
				this.db.GetReader(ref reader, "SP_Get_User_AdoptPetList", para);
				while (reader.Read())
				{
					items.Add(new UsersPetInfo
					{
						ID = (int)reader["ID"],
						TemplateID = (int)reader["TemplateID"],
						Name = reader["Name"].ToString(),
						UserID = (int)reader["UserID"],
						Attack = (int)reader["Attack"],
						AttackGrow = (int)reader["AttackGrow"],
						Agility = (int)reader["Agility"],
						AgilityGrow = (int)reader["AgilityGrow"],
						Defence = (int)reader["Defence"],
						DefenceGrow = (int)reader["DefenceGrow"],
						Luck = (int)reader["Luck"],
						LuckGrow = (int)reader["LuckGrow"],
						Blood = (int)reader["Blood"],
						BloodGrow = (int)reader["BloodGrow"],
						Damage = (int)reader["Damage"],
						DamageGrow = (int)reader["DamageGrow"],
						Guard = (int)reader["Guard"],
						GuardGrow = (int)reader["GuardGrow"],
						Level = (int)reader["Level"],
						GP = (int)reader["GP"],
						MaxGP = (int)reader["MaxGP"],
						Hunger = (int)reader["Hunger"],
						MP = (int)reader["MP"],
						Place = (int)reader["Place"],
						IsEquip = (bool)reader["IsEquip"],
						IsExit = (bool)reader["IsExit"],
						Skill = reader["Skill"].ToString(),
						SkillEquip = reader["SkillEquip"].ToString()
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return items.ToArray();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000224DC File Offset: 0x000206DC
		public UserFarmInfo GetSingleFarm(int Id)
		{
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				para[0].Value = Id;
				this.db.GetReader(ref reader, "SP_Get_SingleFarm", para);
				bool flag = reader.Read();
				if (flag)
				{
					return new UserFarmInfo
					{
						ID = (int)reader["ID"],
						FarmID = (int)reader["FarmID"],
						PayFieldMoney = (string)reader["PayFieldMoney"],
						PayAutoMoney = (string)reader["PayAutoMoney"],
						AutoPayTime = (DateTime)reader["AutoPayTime"],
						AutoValidDate = (int)reader["AutoValidDate"],
						VipLimitLevel = (int)reader["VipLimitLevel"],
						FarmerName = (string)reader["FarmerName"],
						GainFieldId = (int)reader["GainFieldId"],
						MatureId = (int)reader["MatureId"],
						KillCropId = (int)reader["KillCropId"],
						isAutoId = (int)reader["isAutoId"],
						isFarmHelper = (bool)reader["isFarmHelper"],
						buyExpRemainNum = (int)reader["buyExpRemainNum"],
						isArrange = (bool)reader["isArrange"],
						TreeLevel = (int)reader["TreeLevel"],
						TreeExp = (int)reader["TreeExp"],
						LoveScore = (int)reader["LoveScore"],
						MonsterExp = (int)reader["MonsterExp"],
						PoultryState = (int)reader["PoultryState"],
						CountDownTime = (DateTime)reader["CountDownTime"],
						TreeCostExp = (int)reader["TreeCostExp"]
					};
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetSingleFarm", e);
				}
			}
			finally
			{
				bool flag2 = reader != null && !reader.IsClosed;
				if (flag2)
				{
					reader.Close();
				}
			}
			return null;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000227C4 File Offset: 0x000209C4
		public bool AddFarm(UserFarmInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] array = new SqlParameter[22];
				array[0] = new SqlParameter("@FarmID", info.FarmID);
				array[1] = new SqlParameter("@PayFieldMoney", info.PayFieldMoney);
				array[2] = new SqlParameter("@PayAutoMoney", info.PayAutoMoney);
				array[3] = new SqlParameter("@AutoPayTime", info.AutoPayTime);
				array[4] = new SqlParameter("@AutoValidDate", info.AutoValidDate);
				array[5] = new SqlParameter("@VipLimitLevel", info.VipLimitLevel);
				array[6] = new SqlParameter("@FarmerName", info.FarmerName);
				array[7] = new SqlParameter("@GainFieldId", info.GainFieldId);
				array[8] = new SqlParameter("@MatureId", info.MatureId);
				array[9] = new SqlParameter("@KillCropId", info.KillCropId);
				array[10] = new SqlParameter("@isAutoId", info.isAutoId);
				array[11] = new SqlParameter("@isFarmHelper", info.isFarmHelper);
				array[12] = new SqlParameter("@ID", info.ID);
				SqlParameter[] para = array;
				para[12].Direction = ParameterDirection.Output;
				para[13] = new SqlParameter("@buyExpRemainNum", info.buyExpRemainNum);
				para[14] = new SqlParameter("@isArrange", info.isArrange);
				para[15] = new SqlParameter("@TreeLevel", info.TreeLevel);
				para[16] = new SqlParameter("@TreeExp", info.TreeExp);
				para[17] = new SqlParameter("@LoveScore", info.LoveScore);
				para[18] = new SqlParameter("@MonsterExp", info.MonsterExp);
				para[19] = new SqlParameter("@PoultryState", info.PoultryState);
				para[20] = new SqlParameter("@CountDownTime", info.CountDownTime);
				para[21] = new SqlParameter("@TreeCostExp", info.TreeCostExp);
				result = this.db.RunProcedure("SP_Users_Farm_Add", para);
				info.ID = (int)para[12].Value;
				info.IsDirty = false;
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00022A90 File Offset: 0x00020C90
		public bool UpdateFarm(UserFarmInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@FarmID", info.FarmID),
					new SqlParameter("@PayFieldMoney", info.PayFieldMoney),
					new SqlParameter("@PayAutoMoney", info.PayAutoMoney),
					new SqlParameter("@AutoPayTime", info.AutoPayTime),
					new SqlParameter("@AutoValidDate", info.AutoValidDate),
					new SqlParameter("@VipLimitLevel", info.VipLimitLevel),
					new SqlParameter("@FarmerName", info.FarmerName),
					new SqlParameter("@GainFieldId", info.GainFieldId),
					new SqlParameter("@MatureId", info.MatureId),
					new SqlParameter("@KillCropId", info.KillCropId),
					new SqlParameter("@isAutoId", info.isAutoId),
					new SqlParameter("@isFarmHelper", info.isFarmHelper),
					new SqlParameter("@buyExpRemainNum", info.buyExpRemainNum),
					new SqlParameter("@isArrange", info.isArrange),
					new SqlParameter("@TreeLevel", info.TreeLevel),
					new SqlParameter("@TreeExp", info.TreeExp),
					new SqlParameter("@LoveScore", info.LoveScore),
					new SqlParameter("@MonsterExp", info.MonsterExp),
					new SqlParameter("@PoultryState", info.PoultryState),
					new SqlParameter("@CountDownTime", info.CountDownTime),
					new SqlParameter("@TreeCostExp", info.TreeCostExp)
				};
				result = this.db.RunProcedure("SP_Users_Farm_Update", para);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			return result;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00022D18 File Offset: 0x00020F18
		public UserFieldInfo[] GetSingleFields(int ID)
		{
			List<UserFieldInfo> infos = new List<UserFieldInfo>();
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				para[0].Value = ID;
				this.db.GetReader(ref reader, "SP_Get_SingleFields", para);
				while (reader.Read())
				{
					infos.Add(new UserFieldInfo
					{
						ID = (int)reader["ID"],
						FarmID = (int)reader["FarmID"],
						FieldID = (int)reader["FieldID"],
						SeedID = (int)reader["SeedID"],
						PlantTime = (DateTime)reader["PlantTime"],
						AccelerateTime = (int)reader["AccelerateTime"],
						FieldValidDate = (int)reader["FieldValidDate"],
						PayTime = (DateTime)reader["PayTime"],
						GainCount = (int)reader["GainCount"],
						AutoSeedID = (int)reader["AutoSeedID"],
						AutoFertilizerID = (int)reader["AutoFertilizerID"],
						AutoSeedIDCount = (int)reader["AutoSeedIDCount"],
						AutoFertilizerCount = (int)reader["AutoFertilizerCount"],
						isAutomatic = (bool)reader["isAutomatic"],
						AutomaticTime = (DateTime)reader["AutomaticTime"],
						IsExit = (bool)reader["IsExit"],
						payFieldTime = (int)reader["payFieldTime"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_GetSingleFields", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return infos.ToArray();
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00022FA4 File Offset: 0x000211A4
		public bool AddFields(UserFieldInfo item)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@FarmID", item.FarmID),
					new SqlParameter("@FieldID", item.FieldID),
					new SqlParameter("@SeedID", item.SeedID),
					new SqlParameter("@PlantTime", item.PlantTime),
					new SqlParameter("@AccelerateTime", item.AccelerateTime),
					new SqlParameter("@FieldValidDate", item.FieldValidDate),
					new SqlParameter("@PayTime", item.PayTime),
					new SqlParameter("@GainCount", item.GainCount),
					new SqlParameter("@AutoSeedID", item.AutoSeedID),
					new SqlParameter("@AutoFertilizerID", item.AutoFertilizerID),
					new SqlParameter("@AutoSeedIDCount", item.AutoSeedIDCount),
					new SqlParameter("@AutoFertilizerCount", item.AutoFertilizerCount),
					new SqlParameter("@isAutomatic", item.isAutomatic),
					new SqlParameter("@AutomaticTime", item.AutomaticTime),
					new SqlParameter("@IsExit", item.IsExit),
					new SqlParameter("@payFieldTime", item.payFieldTime),
					new SqlParameter("@ID", item.ID)
				};
				para[16].Direction = ParameterDirection.Output;
				result = this.db.RunProcedure("SP_Users_Fields_Add", para);
				item.ID = (int)para[16].Value;
				item.IsDirty = false;
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00023200 File Offset: 0x00021400
		public bool UpdateFields(UserFieldInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@FarmID", info.FarmID),
					new SqlParameter("@FieldID", info.FieldID),
					new SqlParameter("@SeedID", info.SeedID),
					new SqlParameter("@PlantTime", info.PlantTime),
					new SqlParameter("@AccelerateTime", info.AccelerateTime),
					new SqlParameter("@FieldValidDate", info.FieldValidDate),
					new SqlParameter("@PayTime", info.PayTime),
					new SqlParameter("@GainCount", info.GainCount),
					new SqlParameter("@AutoSeedID", info.AutoSeedID),
					new SqlParameter("@AutoFertilizerID", info.AutoFertilizerID),
					new SqlParameter("@AutoSeedIDCount", info.AutoSeedIDCount),
					new SqlParameter("@AutoFertilizerCount", info.AutoFertilizerCount),
					new SqlParameter("@isAutomatic", info.isAutomatic),
					new SqlParameter("@AutomaticTime", info.AutomaticTime),
					new SqlParameter("@IsExit", info.IsExit),
					new SqlParameter("@payFieldTime", info.payFieldTime)
				};
				result = this.db.RunProcedure("SP_Users_Fields_Update", para);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			return result;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00023418 File Offset: 0x00021618
		public NewChickenBoxItemInfo[] GetSingleNewChickenBox(int UserID)
		{
			List<NewChickenBoxItemInfo> list = new List<NewChickenBoxItemInfo>();
			SqlDataReader sqlDataReader = null;
			try
			{
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				array[0].Value = UserID;
				this.db.GetReader(ref sqlDataReader, "SP_GetSingleNewChickenBox", array);
				while (sqlDataReader.Read())
				{
					list.Add(new NewChickenBoxItemInfo
					{
						ID = (int)sqlDataReader["ID"],
						UserID = (int)sqlDataReader["UserID"],
						TemplateID = (int)sqlDataReader["TemplateID"],
						Count = (int)sqlDataReader["Count"],
						ValidDate = (int)sqlDataReader["ValidDate"],
						StrengthenLevel = (int)sqlDataReader["StrengthenLevel"],
						AttackCompose = (int)sqlDataReader["AttackCompose"],
						DefendCompose = (int)sqlDataReader["DefendCompose"],
						AgilityCompose = (int)sqlDataReader["AgilityCompose"],
						LuckCompose = (int)sqlDataReader["LuckCompose"],
						Position = (int)sqlDataReader["Position"],
						IsSelected = (bool)sqlDataReader["IsSelected"],
						IsSeeded = (bool)sqlDataReader["IsSeeded"],
						IsBinds = (bool)sqlDataReader["IsBinds"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_GetSingleNewChickenBox", ex);
				}
			}
			finally
			{
				bool flag = sqlDataReader != null && !sqlDataReader.IsClosed;
				if (flag)
				{
					sqlDataReader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0002365C File Offset: 0x0002185C
		public bool AddNewChickenBox(NewChickenBoxItemInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] array2 = new SqlParameter[15];
				array2[0] = new SqlParameter("@ID", info.ID);
				SqlParameter[] array = array2;
				array[0].Direction = ParameterDirection.Output;
				array[1] = new SqlParameter("@UserID", info.UserID);
				array[2] = new SqlParameter("@TemplateID", info.TemplateID);
				array[3] = new SqlParameter("@Count", info.Count);
				array[4] = new SqlParameter("@ValidDate", info.ValidDate);
				array[5] = new SqlParameter("@StrengthenLevel", info.StrengthenLevel);
				array[6] = new SqlParameter("@AttackCompose", info.AttackCompose);
				array[7] = new SqlParameter("@DefendCompose", info.DefendCompose);
				array[8] = new SqlParameter("@AgilityCompose", info.AgilityCompose);
				array[9] = new SqlParameter("@LuckCompose", info.LuckCompose);
				array[10] = new SqlParameter("@Position", info.Position);
				array[11] = new SqlParameter("@IsSelected", info.IsSelected);
				array[12] = new SqlParameter("@IsSeeded", info.IsSeeded);
				array[13] = new SqlParameter("@IsBinds", info.IsBinds);
				array[14] = new SqlParameter("@Result", SqlDbType.Int);
				array[14].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_NewChickenBox_Add", array);
				result = ((int)array[14].Value == 0);
				info.ID = (int)array[0].Value;
				info.IsDirty = false;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_NewChickenBox_Add", ex);
				}
			}
			return result;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0002387C File Offset: 0x00021A7C
		public bool UpdateNewChickenBox(NewChickenBoxItemInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@TemplateID", info.TemplateID),
					new SqlParameter("@Count", info.Count),
					new SqlParameter("@ValidDate", info.ValidDate),
					new SqlParameter("@StrengthenLevel", info.StrengthenLevel),
					new SqlParameter("@AttackCompose", info.AttackCompose),
					new SqlParameter("@DefendCompose", info.DefendCompose),
					new SqlParameter("@AgilityCompose", info.AgilityCompose),
					new SqlParameter("@LuckCompose", info.LuckCompose),
					new SqlParameter("@Position", info.Position),
					new SqlParameter("@IsSelected", info.IsSelected),
					new SqlParameter("@IsSeeded", info.IsSeeded),
					new SqlParameter("@IsBinds", info.IsBinds),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				array[14].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateNewChickenBox", array);
				result = ((int)array[14].Value == 0);
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UpdateNewChickenBox", ex);
				}
			}
			return result;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00023A74 File Offset: 0x00021C74
		public ActiveSystemInfo GetSingleActiveSystem(int UserID)
		{
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				para[0].Value = UserID;
				this.db.GetReader(ref reader, "SP_GetSingleActiveSystem", para);
				bool flag = reader.Read();
				if (flag)
				{
					return new ActiveSystemInfo
					{
						ID = (int)reader["ID"],
						UserID = (int)reader["UserID"],
						canEagleEyeCounts = (int)reader["canEagleEyeCounts"],
						canOpenCounts = (int)reader["canOpenCounts"],
						isShowAll = (bool)reader["isShowAll"],
						lastFlushTime = (DateTime)reader["lastFlushTime"],
						ChickActiveData = ((reader["ChickActiveData"] == DBNull.Value) ? "" : reader["ChickActiveData"].ToString()),
						LuckystarCoins = (int)reader["LuckystarCoins"],
						ActiveMoney = (int)reader["ActiveMoney"]
					};
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetSingleActiveSystem", e);
				}
			}
			finally
			{
				bool flag2 = reader != null && !reader.IsClosed;
				if (flag2)
				{
					reader.Close();
				}
			}
			return null;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00023C48 File Offset: 0x00021E48
		public bool AddActiveSystem(ActiveSystemInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] array = new SqlParameter[10];
				array[0] = new SqlParameter("@ID", info.ID);
				SqlParameter[] para = array;
				para[0].Direction = ParameterDirection.Output;
				para[1] = new SqlParameter("@UserID", info.UserID);
				para[2] = new SqlParameter("@canEagleEyeCounts", info.canEagleEyeCounts);
				para[3] = new SqlParameter("@canOpenCounts", info.canOpenCounts);
				para[4] = new SqlParameter("@isShowAll", info.isShowAll);
				para[5] = new SqlParameter("@lastFlushTime", info.lastFlushTime);
				para[6] = new SqlParameter("@ChickActiveData", info.ChickActiveData);
				para[7] = new SqlParameter("@LuckystarCoins", info.LuckystarCoins);
				para[8] = new SqlParameter("@ActiveMoney", info.ActiveMoney);
				para[9] = new SqlParameter("@Result", SqlDbType.Int);
				para[9].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_ActiveSystem_Add", para);
				result = ((int)para[9].Value == 0);
				info.ID = (int)para[0].Value;
				info.IsDirty = false;
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("AddActiveSystem", e);
				}
			}
			return result;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00023DE4 File Offset: 0x00021FE4
		public bool UpdateActiveSystem(ActiveSystemInfo item)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", item.ID),
					new SqlParameter("@UserID", item.UserID),
					new SqlParameter("@canOpenCounts", item.canOpenCounts),
					new SqlParameter("@canEagleEyeCounts", item.canEagleEyeCounts),
					new SqlParameter("@lastFlushTime", item.lastFlushTime),
					new SqlParameter("@isShowAll", item.isShowAll),
					new SqlParameter("@ChickActiveData", item.ChickActiveData),
					new SqlParameter("@LuckystarCoins", item.LuckystarCoins),
					new SqlParameter("@ActiveMoney", item.ActiveMoney),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[9].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Active_System_Data_Update", para);
				result = ((int)para[9].Value == 0);
			}
			catch (Exception Err)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdateActiveSystem", Err);
				}
			}
			return result;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00023F58 File Offset: 0x00022158
		public ConsortiaWithTaskInfo[] GetConsortiaTaskInfos()
		{
			List<ConsortiaWithTaskInfo> list = new List<ConsortiaWithTaskInfo>();
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[0];
				this.db.GetReader(ref reader, "SP_Consortia_Task_Info_All", para);
				while (reader.Read())
				{
					list.Add(this.InitConsortiaTaskInfo(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("InitConsortiaTaskInfo", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00024020 File Offset: 0x00022220
		public bool CreateOrUpdateConsortiaTaskInfo(ConsortiaWithTaskInfo item)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ConsortiaID", item.ConsortiaID),
					new SqlParameter("@BeginTime", item.BeginTime),
					new SqlParameter("@Contribution", item.Contribution),
					new SqlParameter("@Expirience", item.Expirience),
					new SqlParameter("@Offer", item.Offer),
					new SqlParameter("@BuffID", item.BuffID),
					new SqlParameter("@Level", item.Level),
					new SqlParameter("@Riches", item.Riches),
					new SqlParameter("@Time", item.Time),
					new SqlParameter("@ConditionData", item.ConditionData),
					new SqlParameter("@RankTable", item.RankTable),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[11].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Consortia_Task_Info_Create_Or_Update", para);
				result = ((int)para[11].Value == 0);
			}
			catch (Exception Err)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("CreateOrUpdateConsortiaTaskInfo", Err);
				}
			}
			return result;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000241C4 File Offset: 0x000223C4
		public bool RemoveConsortiaTaskInfo(int ConsortiaID)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ConsortiaID", ConsortiaID),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[1].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Consortia_Task_Info_Delete", para);
				result = ((int)para[1].Value == 0);
			}
			catch (Exception Err)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("RemoveConsortiaTaskInfo", Err);
				}
			}
			return result;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00024268 File Offset: 0x00022468
		public ConsortiaWithTaskInfo InitConsortiaTaskInfo(SqlDataReader dr)
		{
			return new ConsortiaWithTaskInfo
			{
				ID = (int)dr["ID"],
				ConsortiaID = (int)dr["ConsortiaID"],
				BeginTime = (DateTime)dr["BeginTime"],
				Contribution = (int)dr["Contribution"],
				Expirience = (int)dr["Expirience"],
				Offer = (int)dr["Offer"],
				BuffID = (int)dr["BuffID"],
				Level = (int)dr["Level"],
				Riches = (int)dr["Riches"],
				Time = (int)dr["Time"],
				ConditionData = ((dr["ConditionData"] == DBNull.Value) ? string.Empty : ((string)dr["ConditionData"])),
				RankTable = ((dr["RankTable"] == DBNull.Value) ? string.Empty : ((string)dr["RankTable"]))
			};
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000243C8 File Offset: 0x000225C8
		public int ActiveChickCode(int UserID, string Code)
		{
			int result = 3;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserID),
					new SqlParameter("@ActiveCode", Code),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[2].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Active_ChickCode", para);
				result = (int)para[2].Value;
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			return result;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00024478 File Offset: 0x00022678
		public bool DeleteAllActive()
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[0];
				result = this.db.RunProcedure("SP_Active_Delete", para);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Active_Delete", e);
				}
			}
			return result;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000244E0 File Offset: 0x000226E0
		public bool AddActive(ActiveInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ActiveID", info.ActiveID),
					new SqlParameter("@Description", info.Description),
					new SqlParameter("@Content", info.Content),
					new SqlParameter("@AwardContent", info.AwardContent),
					new SqlParameter("@HasKey", info.HasKey),
					new SqlParameter("@EndDate", info.EndDate),
					new SqlParameter("@IsOnly", info.IsOnly),
					new SqlParameter("@StartDate", info.StartDate),
					new SqlParameter("@Title", info.Title),
					new SqlParameter("@Type", info.Type),
					new SqlParameter("@ActiveType", info.ActiveType),
					new SqlParameter("@ActionTimeContent", info.ActionTimeContent),
					new SqlParameter("@IsAdvance", info.IsAdvance),
					new SqlParameter("@GoodsExchangeTypes", info.GoodsExchangeTypes),
					new SqlParameter("@GoodsExchangeNum", info.GoodsExchangeNum),
					new SqlParameter("@limitType", info.limitType),
					new SqlParameter("@limitValue", info.limitValue),
					new SqlParameter("@IsShow", info.IsShow),
					new SqlParameter("@IconID", info.IconID)
				};
				result = this.db.RunProcedure("SP_Active_Add", para);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Active_Add", e);
				}
			}
			return result;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000246FC File Offset: 0x000228FC
		public bool AddActiveAward(ActiveAwardInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ActiveID", info.ActiveID),
					new SqlParameter("@ItemID", info.ItemID),
					new SqlParameter("@Count", info.Count),
					new SqlParameter("@ValidDate", info.ValidDate),
					new SqlParameter("@StrengthenLevel", info.StrengthenLevel),
					new SqlParameter("@AttackCompose", info.AttackCompose),
					new SqlParameter("@DefendCompose", info.DefendCompose),
					new SqlParameter("@LuckCompose", info.LuckCompose),
					new SqlParameter("@AgilityCompose", info.AgilityCompose),
					new SqlParameter("@Gold", info.Gold),
					new SqlParameter("@Money", info.Money),
					new SqlParameter("@Sex", info.Sex),
					new SqlParameter("@Mark", info.Mark)
				};
				result = this.db.RunProcedure("SP_Active_Award", para);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Active_Award_Add", e);
				}
			}
			return result;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000248B0 File Offset: 0x00022AB0
		public LuckstarActivityRankInfo[] GetAllLuckstarActivityRank()
		{
			List<LuckstarActivityRankInfo> infos = new List<LuckstarActivityRankInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Luckstar_Activity_Rank_All");
				int rank = 1;
				while (reader.Read())
				{
					infos.Add(new LuckstarActivityRankInfo
					{
						rank = rank,
						UserID = (int)reader["UserID"],
						useStarNum = (int)reader["useStarNum"],
						isVip = (int)reader["isVip"],
						nickName = (string)reader["nickName"]
					});
					rank++;
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return infos.ToArray();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000249DC File Offset: 0x00022BDC
		public UserGemStone InitGemStones(SqlDataReader reader)
		{
			return new UserGemStone
			{
				ID = (int)reader["ID"],
				UserID = (int)reader["UserID"],
				FigSpiritId = (int)reader["FigSpiritId"],
				FigSpiritIdValue = (string)reader["FigSpiritIdValue"],
				EquipPlace = (int)reader["EquipPlace"]
			};
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00024A68 File Offset: 0x00022C68
		public List<UserGemStone> GetSingleGemStones(int ID)
		{
			List<UserGemStone> userGemStones = new List<UserGemStone>();
			SqlDataReader sqlDataReader = null;
			try
			{
				SqlParameter[] sqlParameter = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				sqlParameter[0].Value = ID;
				this.db.GetReader(ref sqlDataReader, "SP_GetSingleGemStone", sqlParameter);
				while (sqlDataReader.Read())
				{
					userGemStones.Add(this.InitGemStones(sqlDataReader));
				}
			}
			catch (Exception exception)
			{
				Exception exception2 = exception;
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_GetSingleUserGemStones", exception2);
				}
			}
			finally
			{
				bool flag = sqlDataReader != null && !sqlDataReader.IsClosed;
				if (flag)
				{
					sqlDataReader.Close();
				}
			}
			return userGemStones;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00024B50 File Offset: 0x00022D50
		public bool UpdateGemStoneInfo(UserGemStone g)
		{
			bool flag = false;
			try
			{
				SqlParameter[] sqlParameter = new SqlParameter[]
				{
					new SqlParameter("@ID", g.ID),
					new SqlParameter("@UserID", g.UserID),
					new SqlParameter("@FigSpiritId", g.FigSpiritId),
					new SqlParameter("@FigSpiritIdValue", g.FigSpiritIdValue),
					new SqlParameter("@EquipPlace", g.EquipPlace),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				sqlParameter[5].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateGemStoneInfo", sqlParameter);
				flag = true;
			}
			catch (Exception exception)
			{
				Exception exception2 = exception;
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UpdateGemStoneInfo", exception2);
				}
			}
			return flag;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00024C4C File Offset: 0x00022E4C
		public bool AddUserGemStone(UserGemStone item)
		{
			bool value = false;
			try
			{
				SqlParameter[] array = new SqlParameter[6];
				array[0] = new SqlParameter("@ID", item.ID);
				SqlParameter[] sqlParameter = array;
				sqlParameter[0].Direction = ParameterDirection.Output;
				sqlParameter[1] = new SqlParameter("@UserID", item.UserID);
				sqlParameter[2] = new SqlParameter("@FigSpiritId", item.FigSpiritId);
				sqlParameter[3] = new SqlParameter("@FigSpiritIdValue", item.FigSpiritIdValue);
				sqlParameter[4] = new SqlParameter("@EquipPlace", item.EquipPlace);
				sqlParameter[5] = new SqlParameter("@Result", SqlDbType.Int);
				sqlParameter[5].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Users_GemStones_Add", sqlParameter);
				value = ((int)sqlParameter[5].Value == 0);
				item.ID = (int)sqlParameter[0].Value;
				item.IsDirty = false;
			}
			catch (Exception exception)
			{
				Exception exception2 = exception;
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception2);
				}
			}
			return value;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00024D7C File Offset: 0x00022F7C
		public List<UserAvatarCollectionInfo> GetSingleAvatarCollect(int userId)
		{
			SqlDataReader sqlDataReader = null;
			List<UserAvatarCollectionInfo> list = new List<UserAvatarCollectionInfo>();
			try
			{
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				array[0].Value = userId;
				this.db.GetReader(ref sqlDataReader, "SP_Get_AvatarCollect", array);
				UserAvatarCollectionInfo item = new UserAvatarCollectionInfo();
				while (sqlDataReader.Read())
				{
					item = new UserAvatarCollectionInfo
					{
						ID = (int)sqlDataReader["ID"],
						AvatarID = (int)sqlDataReader["AvatarID"],
						UserID = (int)sqlDataReader["UserID"],
						Sex = (int)sqlDataReader["Sex"],
						IsActive = (bool)sqlDataReader["IsActive"],
						Data = (string)sqlDataReader["Data"],
						TimeStart = (DateTime)sqlDataReader["TimeStart"],
						TimeEnd = (DateTime)sqlDataReader["TimeEnd"],
						IsExit = (bool)sqlDataReader["IsExit"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Get_AllDressModel", exception);
				}
			}
			finally
			{
				bool flag = sqlDataReader != null && !sqlDataReader.IsClosed;
				if (flag)
				{
					sqlDataReader.Close();
				}
			}
			return list;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00024F50 File Offset: 0x00023150
		public bool AddUserAvatarCollect(UserAvatarCollectionInfo item)
		{
			bool result = false;
			try
			{
				SqlParameter[] array = new SqlParameter[10];
				array[0] = new SqlParameter("@ID", item.ID);
				array[0].Direction = ParameterDirection.Output;
				array[1] = new SqlParameter("@UserID", item.UserID);
				array[2] = new SqlParameter("@AvatarID", item.AvatarID);
				array[3] = new SqlParameter("@Sex", item.Sex);
				array[4] = new SqlParameter("@IsActive", item.IsActive);
				array[5] = new SqlParameter("@Data", item.Data);
				array[6] = new SqlParameter("@TimeStart", item.TimeStart.ToString("MM/dd/yyyy hh:mm:ss"));
				array[7] = new SqlParameter("@TimeEnd", item.TimeEnd.ToString("MM/dd/yyyy hh:mm:ss"));
				array[8] = new SqlParameter("@IsExit", item.IsExit);
				array[9] = new SqlParameter("@Result", SqlDbType.Int);
				array[9].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_AvatarCollect_Add", array);
				result = ((int)array[9].Value == 0);
				item.ID = (int)array[0].Value;
				item.IsDirty = false;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			return result;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00025100 File Offset: 0x00023300
		public bool UpdateUserAvatarCollect(UserAvatarCollectionInfo item)
		{
			bool result = false;
			try
			{
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ID", item.ID),
					new SqlParameter("@UserID", item.UserID),
					new SqlParameter("@AvatarID", item.AvatarID),
					new SqlParameter("@Sex", item.Sex),
					new SqlParameter("@IsActive", item.IsActive),
					new SqlParameter("@Data", item.Data),
					new SqlParameter("@TimeStart", item.TimeStart.ToString("MM/dd/yyyy hh:mm:ss")),
					new SqlParameter("@TimeEnd", item.TimeEnd.ToString("MM/dd/yyyy hh:mm:ss")),
					new SqlParameter("@IsExit", item.IsExit),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				array[9].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_AvatarCollect_Update", array);
				result = ((int)array[9].Value == 0);
				item.IsDirty = false;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_AvatarCollect_Update", exception);
				}
			}
			return result;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00025290 File Offset: 0x00023490
		public List<UserLeagueInfo> GetSingleUserLeague(int userId)
		{
			SqlDataReader sqlDataReader = null;
			List<UserLeagueInfo> list = new List<UserLeagueInfo>();
			try
			{
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				array[0].Value = userId;
				this.db.GetReader(ref sqlDataReader, "SP_GetSingleUserLeague", array);
				UserLeagueInfo item = new UserLeagueInfo();
				while (sqlDataReader.Read())
				{
					item = new UserLeagueInfo
					{
						UserID = (int)sqlDataReader["UserID"],
						RankID = (int)sqlDataReader["RankID"],
						Point = (int)sqlDataReader["Point"],
						Win = (int)sqlDataReader["Win"],
						Lose = (int)sqlDataReader["Lose"],
						IsBanned = (bool)sqlDataReader["IsBanned"],
						ForbidDate = (DateTime)sqlDataReader["ForbidDate"],
						ForbidReason = (string)sqlDataReader["ForbidDate"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_GetSingleUserLeague", exception);
				}
			}
			finally
			{
				bool flag = sqlDataReader != null && !sqlDataReader.IsClosed;
				if (flag)
				{
					sqlDataReader.Close();
				}
			}
			return list;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00025450 File Offset: 0x00023650
		public bool AddUserLeague(UserLeagueInfo item)
		{
			bool result = false;
			try
			{
				SqlParameter[] array = new SqlParameter[10];
				array[0] = new SqlParameter("@ID", item.ID);
				array[0].Direction = ParameterDirection.Output;
				array[1] = new SqlParameter("@UserID", item.UserID);
				array[2] = new SqlParameter("@RankID", item.RankID);
				array[3] = new SqlParameter("@Point", item.Point);
				array[4] = new SqlParameter("@Win", item.Win);
				array[5] = new SqlParameter("@Lose", item.Lose);
				array[6] = new SqlParameter("@IsBanned", item.IsBanned);
				array[7] = new SqlParameter("@ForbidDate", item.ForbidDate.ToString("MM/dd/yyyy hh:mm:ss"));
				array[8] = new SqlParameter("@ForbidReason", item.ForbidReason);
				array[9] = new SqlParameter("@Result", SqlDbType.Int);
				array[9].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UserLeague_Add", array);
				result = ((int)array[9].Value == 0);
				item.ID = (int)array[0].Value;
				item.IsDirty = false;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", exception);
				}
			}
			return result;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000255F8 File Offset: 0x000237F8
		public bool UpdateUserLeague(UserLeagueInfo item)
		{
			bool result = false;
			try
			{
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ID", item.ID),
					new SqlParameter("@UserID", item.UserID),
					new SqlParameter("@RankID", item.RankID),
					new SqlParameter("@Point", item.Point),
					new SqlParameter("@Win", item.Win),
					new SqlParameter("@Lose", item.Lose),
					new SqlParameter("@IsBanned", item.IsBanned),
					new SqlParameter("@ForbidDate", item.ForbidDate.ToString("MM/dd/yyyy hh:mm:ss")),
					new SqlParameter("@ForbidReason", item.ForbidReason),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				array[9].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UserLeague_Update", array);
				result = ((int)array[9].Value == 0);
				item.IsDirty = false;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UserLeague_Update", exception);
				}
			}
			return result;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00025780 File Offset: 0x00023980
		public bool RenamesBatch()
		{
			bool result = false;
			try
			{
				result = this.db.RunProcedure("Sp_Renames_Batch");
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init Sp_Renames_Batch", e);
				}
			}
			return result;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000257E0 File Offset: 0x000239E0
		public bool UpdateEventSevenDays(EventSevenDaysInfo item)
		{
			bool result = false;
			try
			{
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ServerID", item.ServerID),
					new SqlParameter("@UserID", item.UserID),
					new SqlParameter("@IsFirstStreng", item.IsFirstStreng),
					new SqlParameter("@IsFirstLv", item.IsFirstLv),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				array[4].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateEventSevenDays", array);
				result = ((int)array[4].Value == 0);
				item.IsDirty = false;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdateEventSevenDays", exception);
				}
			}
			return result;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000258DC File Offset: 0x00023ADC
		public EventSevenDaysInfo GetEventSevenDays(int ServerID)
		{
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ServerID", SqlDbType.Int, 4)
				};
				para[0].Value = ServerID;
				this.db.GetReader(ref reader, "SP_GetEventSevenDays_Single", para);
				bool flag = reader.Read();
				if (flag)
				{
					return new EventSevenDaysInfo
					{
						UserID = (int)reader["UserID"],
						IsFirstStreng = (bool)reader["IsFirstStreng"],
						IsFirstLv = (bool)reader["IsFirstLv"]
					};
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetEventSevenDays", ex);
				}
			}
			finally
			{
				bool flag2 = reader != null && !reader.IsClosed;
				if (flag2)
				{
					reader.Close();
				}
			}
			return null;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000259F0 File Offset: 0x00023BF0
		public UserChristmasInfo GetSingleUserChristmas(int UserID)
		{
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				para[0].Value = UserID;
				this.db.GetReader(ref reader, "SP_GetSingleUserChristmas", para);
				if (reader.Read())
				{
					return new UserChristmasInfo
					{
						ID = (int)reader["ID"],
						UserID = (int)reader["UserID"],
						exp = (int)reader["exp"],
						awardState = (int)reader["awardState"],
						count = (int)reader["count"],
						packsNumber = (int)reader["packsNumber"],
						lastPacks = (int)reader["lastPacks"],
						gameBeginTime = (DateTime)reader["gameBeginTime"],
						gameEndTime = (DateTime)reader["gameEndTime"],
						isEnter = (bool)reader["isEnter"],
						dayPacks = (int)reader["dayPacks"],
						AvailTime = (int)reader["AvailTime"]
					};
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_GetSingleUserChristmas", e);
				}
			}
			finally
			{
				bool flag = reader != null && !reader.IsClosed;
				if (flag)
				{
					reader.Close();
				}
			}
			return null;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00025BF4 File Offset: 0x00023DF4
		public bool AddUserChristmas(UserChristmasInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[13];
				para[0] = new SqlParameter("@ID", info.ID);
				para[0].Direction = ParameterDirection.Output;
				para[1] = new SqlParameter("@UserID", info.UserID);
				para[2] = new SqlParameter("@exp", info.exp);
				para[3] = new SqlParameter("@awardState", info.awardState);
				para[4] = new SqlParameter("@count", info.count);
				para[5] = new SqlParameter("@packsNumber", info.packsNumber);
				para[6] = new SqlParameter("@lastPacks", info.lastPacks);
				para[7] = new SqlParameter("@gameBeginTime", info.gameBeginTime);
				para[8] = new SqlParameter("@gameEndTime", info.gameEndTime);
				para[9] = new SqlParameter("@isEnter", info.isEnter);
				para[10] = new SqlParameter("@dayPacks", info.dayPacks);
				para[11] = new SqlParameter("@AvailTime", info.AvailTime);
				para[12] = new SqlParameter("@Result", SqlDbType.Int);
				para[12].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UserChristmas_Add", para);
				result = ((int)para[12].Value == 0);
				info.ID = (int)para[0].Value;
				info.IsDirty = false;
			}
			catch (Exception e)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("Init", e);
				}
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00025DFC File Offset: 0x00023FFC
		public bool UpdateUserChristmas(UserChristmasInfo info)
		{
			bool flag = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@exp", info.exp),
					new SqlParameter("@awardState", info.awardState),
					new SqlParameter("@count", info.count),
					new SqlParameter("@packsNumber", info.packsNumber),
					new SqlParameter("@lastPacks", info.lastPacks),
					new SqlParameter("@gameBeginTime", info.gameBeginTime),
					new SqlParameter("@gameEndTime", info.gameEndTime),
					new SqlParameter("@isEnter", info.isEnter),
					new SqlParameter("@dayPacks", info.dayPacks),
					new SqlParameter("@AvailTime", info.AvailTime),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[12].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateUserChristmas", para);
				flag = ((int)para[12].Value == 0);
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UpdateUserChristmas", exception);
				}
			}
			return flag;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00025FC4 File Offset: 0x000241C4
		public List<UserGmActivityCondition> GetUserGmActivityConditionInfo(int userId)
		{
			List<UserGmActivityCondition> list = new List<UserGmActivityCondition>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@UserId", SqlDbType.Int, 4)
				};
				array[0].Value = userId;
				this.db.GetReader(ref ResultDataReader, "SP_GMActivityConditionData_Get", array);
				while (ResultDataReader.Read())
				{
					list.Add(this.InitUserGmActivityConditionInfo(ResultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetUserGmActivityConditionInfo", exception);
				}
			}
			finally
			{
				bool flag = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag)
				{
					ResultDataReader.Close();
				}
			}
			return list;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000260A4 File Offset: 0x000242A4
		public UserGmActivityCondition InitUserGmActivityConditionInfo(SqlDataReader reader)
		{
			return new UserGmActivityCondition
			{
				ID = (long)reader["ID"],
				UserID = (int)reader["UserID"],
				ActivityID = (string)reader["ActivityID"],
				GiftBagID = (string)reader["GiftBagID"],
				StatusValue = (int)reader["StatusValue"],
				StatusID = (int)reader["StatusID"],
				IsDirty = false
			};
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00026150 File Offset: 0x00024350
		public List<UserGmActivityReward> GetUserGmActivityRewardInfo(int userId)
		{
			List<UserGmActivityReward> list = new List<UserGmActivityReward>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@UserId", SqlDbType.Int, 4)
				};
				array[0].Value = userId;
				this.db.GetReader(ref ResultDataReader, "SP_GMActivityRewardData_Get", array);
				while (ResultDataReader.Read())
				{
					list.Add(this.InitUserGmActivityRewardInfo(ResultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("GetUserGmActivityRewardInfo", exception);
				}
			}
			finally
			{
				bool flag = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag)
				{
					ResultDataReader.Close();
				}
			}
			return list;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00026230 File Offset: 0x00024430
		public UserGmActivityReward InitUserGmActivityRewardInfo(SqlDataReader reader)
		{
			return new UserGmActivityReward
			{
				ID = (long)reader["ID"],
				UserID = (int)reader["UserID"],
				ActivityID = (string)reader["ActivityID"],
				GiftBagID = (string)reader["GiftBagID"],
				Times = (int)reader["Times"],
				IsDirty = false
			};
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000262C4 File Offset: 0x000244C4
		public void UpdateUserGmActivityRewardInfo(UserGmActivityReward gmActivityReward)
		{
			try
			{
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ID", gmActivityReward.ID),
					new SqlParameter("@UserID", gmActivityReward.UserID),
					new SqlParameter("@activityId", gmActivityReward.ActivityID),
					new SqlParameter("@giftbagId", gmActivityReward.GiftBagID),
					new SqlParameter("@times", gmActivityReward.Times),
					new SqlParameter("@resultId", SqlDbType.BigInt)
				};
				array.Last<SqlParameter>().Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_GMActivityRewardData_Update", array);
				bool flag = gmActivityReward.ID == 0L;
				if (flag)
				{
					gmActivityReward.ID = long.Parse(array.Last<SqlParameter>().Value.ToString());
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdateUserGmActivityRewardInfo", exception);
				}
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000263DC File Offset: 0x000245DC
		public void UpdateUserGmActivityConditionInfo(UserGmActivityCondition gmActivityCondition)
		{
			try
			{
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ID", gmActivityCondition.ID),
					new SqlParameter("@UserID", gmActivityCondition.UserID),
					new SqlParameter("@activityId", gmActivityCondition.ActivityID),
					new SqlParameter("@giftbagId", gmActivityCondition.GiftBagID),
					new SqlParameter("@statusId", gmActivityCondition.StatusID),
					new SqlParameter("@statusValue", gmActivityCondition.StatusValue),
					new SqlParameter("@resultId", SqlDbType.BigInt)
				};
				array.Last<SqlParameter>().Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_GMActivityConditionData_Update", array);
				bool flag = gmActivityCondition.ID == 0L;
				if (flag)
				{
					gmActivityCondition.ID = long.Parse(array.Last<SqlParameter>().Value.ToString());
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("UpdateUserGmActivityConditionInfo", exception);
				}
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0002650C File Offset: 0x0002470C
		public PyramidInfo GetSinglePyramid(int UserID)
		{
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				SqlParameters[0].Value = UserID;
				this.db.GetReader(ref ResultDataReader, "SP_GetSinglePyramid", SqlParameters);
				bool flag = ResultDataReader.Read();
				if (flag)
				{
					return new PyramidInfo
					{
						ID = (int)ResultDataReader["ID"],
						UserID = (int)ResultDataReader["UserID"],
						currentLayer = (int)ResultDataReader["currentLayer"],
						maxLayer = (int)ResultDataReader["maxLayer"],
						totalPoint = (int)ResultDataReader["totalPoint"],
						turnPoint = (int)ResultDataReader["turnPoint"],
						pointRatio = (int)ResultDataReader["pointRatio"],
						currentFreeCount = (int)ResultDataReader["currentFreeCount"],
						currentReviveCount = (int)ResultDataReader["currentReviveCount"],
						isPyramidStart = (bool)ResultDataReader["isPyramidStart"],
						LayerItems = (string)ResultDataReader["LayerItems"]
					};
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_GetSinglePyramid", ex);
				}
			}
			finally
			{
				bool flag2 = ResultDataReader != null && !ResultDataReader.IsClosed;
				if (flag2)
				{
					ResultDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000266F0 File Offset: 0x000248F0
		public bool AddPyramid(PyramidInfo info)
		{
			bool flag = false;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[12];
				SqlParameters[0] = new SqlParameter("@ID", info.ID);
				SqlParameters[0].Direction = ParameterDirection.Output;
				SqlParameters[1] = new SqlParameter("@UserID", info.UserID);
				SqlParameters[2] = new SqlParameter("@currentLayer", info.currentLayer);
				SqlParameters[3] = new SqlParameter("@maxLayer", info.maxLayer);
				SqlParameters[4] = new SqlParameter("@totalPoint", info.totalPoint);
				SqlParameters[5] = new SqlParameter("@turnPoint", info.turnPoint);
				SqlParameters[6] = new SqlParameter("@pointRatio", info.pointRatio);
				SqlParameters[7] = new SqlParameter("@currentFreeCount", info.currentFreeCount);
				SqlParameters[8] = new SqlParameter("@currentReviveCount", info.currentReviveCount);
				SqlParameters[9] = new SqlParameter("@isPyramidStart", info.isPyramidStart);
				SqlParameters[10] = new SqlParameter("@LayerItems", info.LayerItems);
				SqlParameters[11] = new SqlParameter("@Result", SqlDbType.Int);
				SqlParameters[11].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_Pyramid_Add", SqlParameters);
				flag = ((int)SqlParameters[11].Value == 0);
				info.ID = (int)SqlParameters[0].Value;
				info.IsDirty = false;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_Pyramid_Add", ex);
				}
			}
			return flag;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000268BC File Offset: 0x00024ABC
		public bool UpdatePyramid(PyramidInfo info)
		{
			bool flag = false;
			try
			{
				SqlParameter[] SqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@currentLayer", info.currentLayer),
					new SqlParameter("@maxLayer", info.maxLayer),
					new SqlParameter("@totalPoint", info.totalPoint),
					new SqlParameter("@turnPoint", info.turnPoint),
					new SqlParameter("@pointRatio", info.pointRatio),
					new SqlParameter("@currentFreeCount", info.currentFreeCount),
					new SqlParameter("@currentReviveCount", info.currentReviveCount),
					new SqlParameter("@isPyramidStart", info.isPyramidStart),
					new SqlParameter("@LayerItems", info.LayerItems),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				SqlParameters[11].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdatePyramid", SqlParameters);
				flag = ((int)SqlParameters[11].Value == 0);
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_UpdatePyramid", ex);
				}
			}
			return flag;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00026A64 File Offset: 0x00024C64
		public bool ResetUsersEventProcess(EventRewardProcessInfo info)
		{
			bool flag = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", info.UserID),
					new SqlParameter("@ActiveType", info.ActiveType),
					new SqlParameter("@IsReset", info.IsReset),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[3].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_ResetUsersEventProcess", para);
				flag = ((int)para[3].Value == 0);
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = BaseBussiness.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					BaseBussiness.log.Error("SP_ResetUsersEventProcess", exception);
				}
			}
			return flag;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness
{
	// Token: 0x02000018 RID: 24
	public class ProduceBussiness : BaseCrossBussiness
	{
		// Token: 0x060001BA RID: 442 RVA: 0x00026B4C File Offset: 0x00024D4C
		public DiceLevelAwardInfo[] GetDiceLevelAwardInfos()
		{
			List<DiceLevelAwardInfo> infos = new List<DiceLevelAwardInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_DiceLevelAward_All");
				while (reader.Read())
				{
					infos.Add(new DiceLevelAwardInfo
					{
						ID = (int)reader["ID"],
						DiceLevel = (int)reader["DiceLevel"],
						TemplateID = (int)reader["TemplateID"],
						Count = (int)reader["Count"],
						ValidDate = (int)reader["ValidDate"],
						IsBinds = (bool)reader["IsBinds"],
						StrengthenLevel = (int)reader["StrengthenLevel"],
						AttackCompose = (int)reader["AttackCompose"],
						DefendCompose = (int)reader["DefendCompose"],
						AgilityCompose = (int)reader["AgilityCompose"],
						LuckCompose = (int)reader["LuckCompose"],
						Random = (int)reader["Random"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllDiceLevelAward", e);
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

		// Token: 0x060001BB RID: 443 RVA: 0x00026D30 File Offset: 0x00024F30
		public CardGroupInfo[] GetAllCardGroup()
		{
			List<CardGroupInfo> infos = new List<CardGroupInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Card_Group_All");
				while (reader.Read())
				{
					infos.Add(this.InitCardGroup(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllCardGroup", e);
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

		// Token: 0x060001BC RID: 444 RVA: 0x00026DF0 File Offset: 0x00024FF0
		public CardGroupInfo InitCardGroup(SqlDataReader reader)
		{
			return new CardGroupInfo
			{
				ID = (int)reader["ID"],
				CardID = (int)reader["CardID"],
				TemplateID = (int)reader["TemplateID"]
			};
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00026E50 File Offset: 0x00025050
		public CardInfo[] GetAllCard()
		{
			List<CardInfo> infos = new List<CardInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Card_Info_All");
				while (reader.Read())
				{
					infos.Add(this.InitCard(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllCard", e);
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

		// Token: 0x060001BE RID: 446 RVA: 0x00026F10 File Offset: 0x00025110
		public CardInfo InitCard(SqlDataReader reader)
		{
			return new CardInfo
			{
				ID = (int)reader["ID"],
				SuitID = (int)reader["SuitID"],
				Name = ((reader["Name"] == null) ? "" : reader["Name"].ToString()),
				Description = ((reader["Description"] == null) ? "" : reader["Description"].ToString())
			};
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00026FB0 File Offset: 0x000251B0
		public CardBuffInfo[] GetAllCardBuff()
		{
			List<CardBuffInfo> infos = new List<CardBuffInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Card_Buff_All");
				while (reader.Read())
				{
					infos.Add(this.InitCardBuff(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllCardBuff", e);
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

		// Token: 0x060001C0 RID: 448 RVA: 0x00027070 File Offset: 0x00025270
		public CardBuffInfo InitCardBuff(SqlDataReader reader)
		{
			return new CardBuffInfo
			{
				ID = (int)reader["ID"],
				CardID = (int)reader["CardID"],
				Condition = (int)reader["condition"],
				PropertiesDscripID = (int)reader["PropertiesDscripID"],
				Value = ((reader["value"] == null) ? "" : reader["value"].ToString()),
				Description = ((reader["Description"] == null) ? "" : reader["Description"].ToString())
			};
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0002713C File Offset: 0x0002533C
		public ActiveAwardInfo[] GetAllActiveAwardInfo()
		{
			List<ActiveAwardInfo> list = new List<ActiveAwardInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Active_Award");
				while (resultDataReader.Read())
				{
					ActiveAwardInfo item = new ActiveAwardInfo
					{
						ID = (int)resultDataReader["ID"],
						ActiveID = (int)resultDataReader["ActiveID"],
						AgilityCompose = (int)resultDataReader["AgilityCompose"],
						AttackCompose = (int)resultDataReader["AttackCompose"],
						Count = (int)resultDataReader["Count"],
						DefendCompose = (int)resultDataReader["DefendCompose"],
						Gold = (int)resultDataReader["Gold"],
						ItemID = (int)resultDataReader["ItemID"],
						LuckCompose = (int)resultDataReader["LuckCompose"],
						Mark = (int)resultDataReader["Mark"],
						Money = (int)resultDataReader["Money"],
						Sex = (int)resultDataReader["Sex"],
						StrengthenLevel = (int)resultDataReader["StrengthenLevel"],
						ValidDate = (int)resultDataReader["ValidDate"],
						GiftToken = (int)resultDataReader["GiftToken"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllActiveAwardInfo", exception);
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

		// Token: 0x060001C2 RID: 450 RVA: 0x00027374 File Offset: 0x00025574
		public ActiveConditionInfo[] GetAllActiveConditionInfo()
		{
			List<ActiveConditionInfo> list = new List<ActiveConditionInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Active_Condition");
				while (resultDataReader.Read())
				{
					ActiveConditionInfo item = new ActiveConditionInfo
					{
						ID = (int)resultDataReader["ID"],
						ActiveID = (int)resultDataReader["ActiveID"],
						Conditiontype = (int)resultDataReader["Conditiontype"],
						Condition = (int)resultDataReader["Condition"],
						LimitGrade = ((resultDataReader["LimitGrade"].ToString() == null) ? "" : resultDataReader["LimitGrade"].ToString()),
						AwardId = ((resultDataReader["AwardId"].ToString() == null) ? "" : resultDataReader["AwardId"].ToString()),
						IsMult = (bool)resultDataReader["IsMult"],
						StartTime = (DateTime)resultDataReader["StartTime"],
						EndTime = (DateTime)resultDataReader["EndTime"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllActiveConditionInfo", exception);
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

		// Token: 0x060001C3 RID: 451 RVA: 0x00027554 File Offset: 0x00025754
		public AchievementInfo[] GetAllAchievement()
		{
			List<AchievementInfo> list = new List<AchievementInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Get_Achievement");
				while (resultDataReader.Read())
				{
					AchievementInfo item = new AchievementInfo
					{
						ID = (int)resultDataReader["ID"],
						PlaceID = (int)resultDataReader["PlaceID"],
						Title = (string)resultDataReader["Title"],
						Detail = (string)resultDataReader["Detail"],
						NeedMinLevel = (int)resultDataReader["NeedMinLevel"],
						NeedMaxLevel = (int)resultDataReader["NeedMaxLevel"],
						PreAchievementID = (string)resultDataReader["PreAchievementID"],
						IsOther = (int)resultDataReader["IsOther"],
						AchievementType = (int)resultDataReader["AchievementType"],
						CanHide = (bool)resultDataReader["CanHide"],
						StartDate = (DateTime)resultDataReader["StartDate"],
						EndDate = (DateTime)resultDataReader["EndDate"],
						AchievementPoint = (int)resultDataReader["AchievementPoint"],
						IsActive = (int)resultDataReader["IsActive"],
						PicID = (int)resultDataReader["PicID"],
						IsShare = (bool)resultDataReader["IsShare"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001C4 RID: 452 RVA: 0x000277A4 File Offset: 0x000259A4
		public AchievementInfo[] GetALlAchievement()
		{
			List<AchievementInfo> list = new List<AchievementInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Achievement_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitAchievement(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetALlAchievement:", exception);
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

		// Token: 0x060001C5 RID: 453 RVA: 0x00027864 File Offset: 0x00025A64
		public AchievementCondictionInfo[] GetAllAchievementCondiction()
		{
			List<AchievementCondictionInfo> list = new List<AchievementCondictionInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Get_AchievementCondiction");
				while (resultDataReader.Read())
				{
					AchievementCondictionInfo item = new AchievementCondictionInfo
					{
						AchievementID = (int)resultDataReader["AchievementID"],
						CondictionID = (int)resultDataReader["CondictionID"],
						CondictionType = (int)resultDataReader["CondictionType"],
						Condiction_Para1 = (int)resultDataReader["Condiction_Para1"],
						Condiction_Para2 = (int)resultDataReader["Condiction_Para2"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001C6 RID: 454 RVA: 0x000279A0 File Offset: 0x00025BA0
		public AchievementConditionInfo[] GetALlAchievementCondition()
		{
			List<AchievementConditionInfo> list = new List<AchievementConditionInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Achievement_Condition_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitAchievementCondition(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetALlAchievementCondition:", exception);
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

		// Token: 0x060001C7 RID: 455 RVA: 0x00027A60 File Offset: 0x00025C60
		public AchievementGoodsInfo[] GetAllAchievementGoods()
		{
			List<AchievementGoodsInfo> list = new List<AchievementGoodsInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Get_AchievementGoods");
				while (resultDataReader.Read())
				{
					AchievementGoodsInfo item = new AchievementGoodsInfo
					{
						AchievementID = (int)resultDataReader["AchievementID"],
						RewardType = (int)resultDataReader["RewardType"],
						RewardPara = (string)resultDataReader["RewardPara"],
						RewardValueId = (int)resultDataReader["RewardValueId"],
						RewardCount = (int)resultDataReader["RewardCount"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001C8 RID: 456 RVA: 0x00027B9C File Offset: 0x00025D9C
		public AchievementRewardInfo[] GetALlAchievementReward()
		{
			List<AchievementRewardInfo> list = new List<AchievementRewardInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Achievement_Reward_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitAchievementReward(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetALlAchievementReward", exception);
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

		// Token: 0x060001C9 RID: 457 RVA: 0x00027C5C File Offset: 0x00025E5C
		public BallInfo[] GetAllBall()
		{
			List<BallInfo> list = new List<BallInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Ball_All");
				while (resultDataReader.Read())
				{
					BallInfo item = new BallInfo
					{
						Amount = (int)resultDataReader["Amount"],
						ID = (int)resultDataReader["ID"],
						Name = resultDataReader["Name"].ToString(),
						Crater = ((resultDataReader["Crater"] == null) ? "" : resultDataReader["Crater"].ToString()),
						Power = (double)resultDataReader["Power"],
						Radii = (int)resultDataReader["Radii"],
						AttackResponse = (int)resultDataReader["AttackResponse"],
						BombPartical = resultDataReader["BombPartical"].ToString(),
						FlyingPartical = resultDataReader["FlyingPartical"].ToString(),
						IsSpin = (bool)resultDataReader["IsSpin"],
						Mass = (int)resultDataReader["Mass"],
						SpinV = (int)resultDataReader["SpinV"],
						SpinVA = (double)resultDataReader["SpinVA"],
						Wind = (int)resultDataReader["Wind"],
						DragIndex = (int)resultDataReader["DragIndex"],
						Weight = (int)resultDataReader["Weight"],
						Shake = (bool)resultDataReader["Shake"],
						Delay = (int)resultDataReader["Delay"],
						ShootSound = ((resultDataReader["ShootSound"] == null) ? "" : resultDataReader["ShootSound"].ToString()),
						BombSound = ((resultDataReader["BombSound"] == null) ? "" : resultDataReader["BombSound"].ToString()),
						ActionType = (int)resultDataReader["ActionType"],
						HasTunnel = (bool)resultDataReader["HasTunnel"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001CA RID: 458 RVA: 0x00027F70 File Offset: 0x00026170
		public ItemTemplateInfo[] GetSingleName(string ItemName)
		{
			List<ItemTemplateInfo> infos = new List<ItemTemplateInfo>();
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@name", SqlDbType.NVarChar, 100)
				};
				para[0].Value = ItemName;
				this.db.GetReader(ref reader, "SP_Items_Name_Single", para);
				while (reader.Read())
				{
					infos.Add(this.InitItemTemplateInfo(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", e);
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

		// Token: 0x060001CB RID: 459 RVA: 0x00028054 File Offset: 0x00026254
		public BallConfigInfo[] GetAllBallConfig()
		{
			List<BallConfigInfo> list = new List<BallConfigInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "[SP_Ball_Config_All]");
				while (resultDataReader.Read())
				{
					BallConfigInfo item = new BallConfigInfo
					{
						Common = (int)resultDataReader["Common"],
						TemplateID = (int)resultDataReader["TemplateID"],
						CommonAddWound = (int)resultDataReader["CommonAddWound"],
						CommonMultiBall = (int)resultDataReader["CommonMultiBall"],
						Special = (int)resultDataReader["Special"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001CC RID: 460 RVA: 0x00028190 File Offset: 0x00026390
		public CategoryInfo[] GetAllCategory()
		{
			List<CategoryInfo> list = new List<CategoryInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Items_Category_All");
				while (resultDataReader.Read())
				{
					CategoryInfo item = new CategoryInfo
					{
						ID = (int)resultDataReader["ID"],
						Name = ((resultDataReader["Name"] == null) ? "" : resultDataReader["Name"].ToString()),
						Place = (int)resultDataReader["Place"],
						Remark = ((resultDataReader["Remark"] == null) ? "" : resultDataReader["Remark"].ToString())
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001CD RID: 461 RVA: 0x000282DC File Offset: 0x000264DC
		public DailyAwardInfo[] GetAllDailyAward()
		{
			List<DailyAwardInfo> list = new List<DailyAwardInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Daily_Award_All");
				while (resultDataReader.Read())
				{
					DailyAwardInfo item = new DailyAwardInfo
					{
						Count = (int)resultDataReader["Count"],
						ID = (int)resultDataReader["ID"],
						IsBinds = (bool)resultDataReader["IsBinds"],
						TemplateID = (int)resultDataReader["TemplateID"],
						Type = (int)resultDataReader["Type"],
						ValidDate = (int)resultDataReader["ValidDate"],
						Sex = (int)resultDataReader["Sex"],
						Remark = ((resultDataReader["Remark"] == null) ? "" : resultDataReader["Remark"].ToString()),
						CountRemark = ((resultDataReader["CountRemark"] == null) ? "" : resultDataReader["CountRemark"].ToString()),
						GetWay = (int)resultDataReader["GetWay"],
						AwardDays = (int)resultDataReader["AwardDays"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllDaily", exception);
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

		// Token: 0x060001CE RID: 462 RVA: 0x000284E0 File Offset: 0x000266E0
		public DailyAwardInfo[] GetSingleDailyAward(int awardDays)
		{
			List<DailyAwardInfo> list = new List<DailyAwardInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@awardDays", awardDays)
				};
				this.db.GetReader(ref resultDataReader, "SP_Daily_Award_Single", sqlParameters);
				while (resultDataReader.Read())
				{
					DailyAwardInfo item = new DailyAwardInfo
					{
						Count = (int)resultDataReader["Count"],
						ID = (int)resultDataReader["ID"],
						IsBinds = (bool)resultDataReader["IsBinds"],
						TemplateID = (int)resultDataReader["TemplateID"],
						Type = (int)resultDataReader["Type"],
						ValidDate = (int)resultDataReader["ValidDate"],
						Sex = (int)resultDataReader["Sex"],
						Remark = ((resultDataReader["Remark"] == null) ? "" : resultDataReader["Remark"].ToString()),
						CountRemark = ((resultDataReader["CountRemark"] == null) ? "" : resultDataReader["CountRemark"].ToString()),
						GetWay = (int)resultDataReader["GetWay"],
						AwardDays = (int)resultDataReader["AwardDays"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetSingleDaily", exception);
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

		// Token: 0x060001CF RID: 463 RVA: 0x00028700 File Offset: 0x00026900
		public DropCondiction[] GetAllDropCondictions()
		{
			List<DropCondiction> list = new List<DropCondiction>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Drop_Condiction_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitDropCondiction(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001D0 RID: 464 RVA: 0x000287C0 File Offset: 0x000269C0
		public DropItem[] GetAllDropItems()
		{
			List<DropItem> list = new List<DropItem>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Drop_Item_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitDropItem(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001D1 RID: 465 RVA: 0x00028880 File Offset: 0x00026A80
		public GmActivityInfo[] GetAllGmActivity()
		{
			List<GmActivityInfo> list = new List<GmActivityInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_GM_Activity_All");
				while (ResultDataReader.Read())
				{
					list.Add(this.InitGmActivityInfo(ResultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("InitGmActivityInfo", exception);
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

		// Token: 0x060001D2 RID: 466 RVA: 0x00028940 File Offset: 0x00026B40
		public GmActiveRewardInfo InitGmActiveRewardInfo(SqlDataReader dr)
		{
			return new GmActiveRewardInfo
			{
				giftId = (string)dr["giftId"],
				templateId = (int)dr["templateId"],
				count = (int)dr["count"],
				isBind = (((bool)dr["isBind"]) ? 1 : 0),
				occupationOrSex = (int)dr["occupationOrSex"],
				rewardType = (int)dr["rewardType"],
				validDate = (int)dr["validDate"],
				property = (string)dr["property"],
				remain1 = (string)dr["remain1"],
				allGiftGetTimes = (int)dr["allGiftGetTimes"]
			};
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00028A44 File Offset: 0x00026C44
		public GmActiveConditionInfo[] GetAllGmActiveCondition()
		{
			List<GmActiveConditionInfo> list = new List<GmActiveConditionInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_GM_Active_Condition_All");
				while (ResultDataReader.Read())
				{
					list.Add(this.InitGmActiveConditionInfo(ResultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("InitGmActiveConditionInfo", exception);
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

		// Token: 0x060001D4 RID: 468 RVA: 0x00028B04 File Offset: 0x00026D04
		public GmActiveConditionInfo InitGmActiveConditionInfo(SqlDataReader dr)
		{
			return new GmActiveConditionInfo
			{
				giftbagId = (string)dr["giftbagId"],
				conditionIndex = (int)dr["conditionIndex"],
				conditionValue = (int)dr["conditionValue"],
				remain1 = (int)dr["remain1"],
				remain2 = (string)dr["remain2"]
			};
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00028B90 File Offset: 0x00026D90
		public GmGiftInfo[] GetAllGmGift()
		{
			List<GmGiftInfo> list = new List<GmGiftInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_GM_Gift_All");
				while (ResultDataReader.Read())
				{
					list.Add(this.InitGmGiftInfo(ResultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("InitGmGiftInfo", exception);
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

		// Token: 0x060001D6 RID: 470 RVA: 0x00028C50 File Offset: 0x00026E50
		public GmActiveRewardInfo[] GetAllGmActiveReward()
		{
			List<GmActiveRewardInfo> list = new List<GmActiveRewardInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_GM_Active_Reward_All");
				while (ResultDataReader.Read())
				{
					list.Add(this.InitGmActiveRewardInfo(ResultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("InitGmActiveRewardInfo", exception);
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

		// Token: 0x060001D7 RID: 471 RVA: 0x00028D10 File Offset: 0x00026F10
		public GmGiftInfo InitGmGiftInfo(SqlDataReader dr)
		{
			return new GmGiftInfo
			{
				giftbagId = (string)dr["giftbagId"],
				activityId = (string)dr["activityId"],
				rewardMark = (int)dr["rewardMark"],
				giftbagOrder = (int)dr["giftbagOrder"]
			};
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00028D84 File Offset: 0x00026F84
		public GmActivityInfo InitGmActivityInfo(SqlDataReader dr)
		{
			return new GmActivityInfo
			{
				activityId = (string)dr["activityId"],
				activityName = (string)dr["activityName"],
				activityType = (int)dr["activityType"],
				activityChildType = (int)dr["activityChildType"],
				getWay = (int)dr["getWay"],
				desc = (string)dr["desc"],
				rewardDesc = (string)dr["rewardDesc"],
				beginTime = (DateTime)dr["beginTime"],
				beginShowTime = (DateTime)dr["beginShowTime"],
				endTime = (DateTime)dr["endTime"],
				endShowTime = (DateTime)dr["endShowTime"],
				icon = (int)dr["icon"],
				isContinue = (int)dr["isContinue"],
				status = (int)dr["status"],
				remain1 = (int)dr["remain1"],
				remain2 = (string)dr["remain2"]
			};
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00028F0C File Offset: 0x0002710C
		public EdictumInfo[] GetAllEdictum()
		{
			List<EdictumInfo> list = new List<EdictumInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Edictum_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitEdictum(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllEdictum", exception);
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

		// Token: 0x060001DA RID: 474 RVA: 0x00028FCC File Offset: 0x000271CC
		public EventRewardGoodsInfo[] GetAllEventRewardGoods()
		{
			List<EventRewardGoodsInfo> list = new List<EventRewardGoodsInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Get_EventRewardGoods");
				while (resultDataReader.Read())
				{
					EventRewardGoodsInfo item = new EventRewardGoodsInfo
					{
						ActivityType = (int)resultDataReader["ActivityType"],
						SubActivityType = (int)resultDataReader["SubActivityType"],
						TemplateId = (int)resultDataReader["TemplateId"],
						StrengthLevel = (int)resultDataReader["StrengthLevel"],
						AttackCompose = (int)resultDataReader["AttackCompose"],
						DefendCompose = (int)resultDataReader["DefendCompose"],
						LuckCompose = (int)resultDataReader["LuckCompose"],
						AgilityCompose = (int)resultDataReader["AgilityCompose"],
						IsBind = (bool)resultDataReader["IsBind"],
						ValidDate = (int)resultDataReader["ValidDate"],
						Count = (int)resultDataReader["Count"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllEventRewardGoods", exception);
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

		// Token: 0x060001DB RID: 475 RVA: 0x000291A8 File Offset: 0x000273A8
		public EventRewardGoodsInfo[] GetEventRewardGoodsByType(int ActivityType, int SubActivityType)
		{
			List<EventRewardGoodsInfo> list = new List<EventRewardGoodsInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ActivityType", ActivityType),
					new SqlParameter("@SubActivityType", SubActivityType)
				};
				this.db.GetReader(ref resultDataReader, "SP_Get_EventRewardGoods_Type", sqlParameters);
				while (resultDataReader.Read())
				{
					EventRewardGoodsInfo item = new EventRewardGoodsInfo
					{
						ActivityType = (int)resultDataReader["ActivityType"],
						SubActivityType = (int)resultDataReader["SubActivityType"],
						TemplateId = (int)resultDataReader["TemplateId"],
						StrengthLevel = (int)resultDataReader["StrengthLevel"],
						AttackCompose = (int)resultDataReader["AttackCompose"],
						DefendCompose = (int)resultDataReader["DefendCompose"],
						LuckCompose = (int)resultDataReader["LuckCompose"],
						AgilityCompose = (int)resultDataReader["AgilityCompose"],
						IsBind = (bool)resultDataReader["IsBind"],
						ValidDate = (int)resultDataReader["ValidDate"],
						Count = (int)resultDataReader["Count"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetEventRewardGoodsByType", exception);
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

		// Token: 0x060001DC RID: 476 RVA: 0x000293B4 File Offset: 0x000275B4
		public EventRewardInfo[] GetAllEventRewardInfo()
		{
			List<EventRewardInfo> list = new List<EventRewardInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Get_EventRewardInfo");
				while (resultDataReader.Read())
				{
					EventRewardInfo item = new EventRewardInfo
					{
						ActivityType = (int)resultDataReader["ActivityType"],
						SubActivityType = (int)resultDataReader["SubActivityType"],
						Condition = (int)resultDataReader["Condition"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllEventRewardInfo", exception);
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

		// Token: 0x060001DD RID: 477 RVA: 0x000294BC File Offset: 0x000276BC
		public EventRewardInfo[] GetEventRewardInfoByType(int ActivityType, int SubActivityType)
		{
			List<EventRewardInfo> list = new List<EventRewardInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ActivityType", ActivityType),
					new SqlParameter("@SubActivityType", SubActivityType)
				};
				this.db.GetReader(ref resultDataReader, "SP_Get_EventRewardInfo_Type", sqlParameters);
				while (resultDataReader.Read())
				{
					EventRewardInfo item = new EventRewardInfo
					{
						ActivityType = (int)resultDataReader["ActivityType"],
						SubActivityType = (int)resultDataReader["SubActivityType"],
						Condition = (int)resultDataReader["Condition"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetEventRewardInfoByType", exception);
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

		// Token: 0x060001DE RID: 478 RVA: 0x000295F4 File Offset: 0x000277F4
		public FusionInfo[] GetAllFusion()
		{
			List<FusionInfo> fusionInfoList = new List<FusionInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_Fusion_All");
				while (ResultDataReader.Read())
				{
					fusionInfoList.Add(new FusionInfo
					{
						FusionID = (int)ResultDataReader["FusionID"],
						Item1 = (int)ResultDataReader["Item1"],
						Item2 = (int)ResultDataReader["Item2"],
						Item3 = (int)ResultDataReader["Item3"],
						Item4 = (int)ResultDataReader["Item4"],
						Formula = (int)ResultDataReader["Formula"],
						Reward = (int)ResultDataReader["Reward"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllFusion", ex);
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
			return fusionInfoList.ToArray();
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0002976C File Offset: 0x0002796C
		public FusionInfo[] GetAllFusionDesc()
		{
			List<FusionInfo> list = new List<FusionInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Fusion_All_Desc");
				while (resultDataReader.Read())
				{
					FusionInfo item = new FusionInfo
					{
						FusionID = (int)resultDataReader["FusionID"],
						Item1 = (int)resultDataReader["Item1"],
						Item2 = (int)resultDataReader["Item2"],
						Item3 = (int)resultDataReader["Item3"],
						Item4 = (int)resultDataReader["Item4"],
						Formula = (int)resultDataReader["Formula"],
						Reward = (int)resultDataReader["Reward"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllFusion", exception);
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

		// Token: 0x060001E0 RID: 480 RVA: 0x000298EC File Offset: 0x00027AEC
		public Items_Fusion_List_Info[] GetAllFusionList()
		{
			List<Items_Fusion_List_Info> list = new List<Items_Fusion_List_Info>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "GET_ItemFusion_All");
				while (resultDataReader.Read())
				{
					Items_Fusion_List_Info item = new Items_Fusion_List_Info
					{
						ID = (int)resultDataReader["ID"],
						TemplateID = (int)resultDataReader["TemplateID"],
						Show = (int)resultDataReader["Show"],
						Real = (int)resultDataReader["Real"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GET_ItemFusion_All", exception);
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

		// Token: 0x060001E1 RID: 481 RVA: 0x00029A08 File Offset: 0x00027C08
		public ItemTemplateInfo[] GetAllGoods()
		{
			List<ItemTemplateInfo> list = new List<ItemTemplateInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Items_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitItemTemplateInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001E2 RID: 482 RVA: 0x00029AC8 File Offset: 0x00027CC8
		public ItemTemplateInfo[] GetAllGoodsASC()
		{
			List<ItemTemplateInfo> list = new List<ItemTemplateInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Items_All_ASC");
				while (resultDataReader.Read())
				{
					list.Add(this.InitItemTemplateInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001E3 RID: 483 RVA: 0x00029B88 File Offset: 0x00027D88
		public HotSpringRoomInfo[] GetAllHotSpringRooms()
		{
			List<HotSpringRoomInfo> list = new List<HotSpringRoomInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Get_HotSpring_Room");
				while (resultDataReader.Read())
				{
					HotSpringRoomInfo item = new HotSpringRoomInfo
					{
						roomID = (int)resultDataReader["roomID"],
						roomNumber = (int)resultDataReader["roomNumber"],
						roomName = resultDataReader["roomName"].ToString(),
						roomPassword = ((resultDataReader["roomPassword"] == DBNull.Value) ? null : ((string)resultDataReader["roomPassword"])),
						effectiveTime = (int)resultDataReader["effectiveTime"],
						curCount = (int)resultDataReader["curCount"],
						playerID = (int)resultDataReader["playerID"],
						playerName = (string)resultDataReader["playerName"],
						startTime = ((resultDataReader["startTime"] == DBNull.Value) ? DateTime.Now : ((DateTime)resultDataReader["startTime"])),
						endTime = ((resultDataReader["endTime"] == DBNull.Value) ? DateTime.Now.AddYears(1) : ((DateTime)resultDataReader["endTime"])),
						roomIntroduction = ((resultDataReader["roomIntroduction"] == DBNull.Value) ? "" : ((string)resultDataReader["roomIntroduction"])),
						roomType = (int)resultDataReader["roomType"],
						maxCount = (int)resultDataReader["maxCount"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllHotSpringRooms", exception);
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

		// Token: 0x060001E4 RID: 484 RVA: 0x00029DFC File Offset: 0x00027FFC
		public ItemRecordTypeInfo[] GetAllItemRecordType()
		{
			List<ItemRecordTypeInfo> list = new List<ItemRecordTypeInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Item_Record_Type_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitItemRecordType(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllItemRecordType:", exception);
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

		// Token: 0x060001E5 RID: 485 RVA: 0x00029EBC File Offset: 0x000280BC
		public ShopItemInfo[] GetALllShop()
		{
			List<ShopItemInfo> infos = new List<ShopItemInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Shop_All");
				while (reader.Read())
				{
					infos.Add(new ShopItemInfo
					{
						ID = int.Parse(reader["ID"].ToString()),
						ShopID = int.Parse(reader["ShopID"].ToString()),
						GroupID = int.Parse(reader["GroupID"].ToString()),
						TemplateID = int.Parse(reader["TemplateID"].ToString()),
						BuyType = int.Parse(reader["BuyType"].ToString()),
						Sort = 0,
						IsVouch = ((reader["IsVouch"] == null) ? 0 : int.Parse(reader["IsVouch"].ToString())),
						Label = (float)int.Parse(reader["Label"].ToString()),
						Beat = decimal.Parse(reader["Beat"].ToString()),
						AUnit = int.Parse(reader["AUnit"].ToString()),
						APrice1 = int.Parse(reader["APrice1"].ToString()),
						AValue1 = int.Parse(reader["AValue1"].ToString()),
						APrice2 = int.Parse(reader["APrice2"].ToString()),
						AValue2 = int.Parse(reader["AValue2"].ToString()),
						APrice3 = int.Parse(reader["APrice3"].ToString()),
						AValue3 = int.Parse(reader["AValue3"].ToString()),
						BUnit = int.Parse(reader["BUnit"].ToString()),
						BPrice1 = int.Parse(reader["BPrice1"].ToString()),
						BValue1 = int.Parse(reader["BValue1"].ToString()),
						BPrice2 = int.Parse(reader["BPrice2"].ToString()),
						BValue2 = int.Parse(reader["BValue2"].ToString()),
						BPrice3 = int.Parse(reader["BPrice3"].ToString()),
						BValue3 = int.Parse(reader["BValue3"].ToString()),
						CUnit = int.Parse(reader["CUnit"].ToString()),
						CPrice1 = int.Parse(reader["CPrice1"].ToString()),
						CValue1 = int.Parse(reader["CValue1"].ToString()),
						CPrice2 = int.Parse(reader["CPrice2"].ToString()),
						CValue2 = int.Parse(reader["CValue2"].ToString()),
						CPrice3 = int.Parse(reader["CPrice3"].ToString()),
						CValue3 = int.Parse(reader["CValue3"].ToString()),
						IsBind = int.Parse(reader["IsBind"].ToString()),
						IsContinue = bool.Parse(reader["IsContinue"].ToString()),
						IsCheap = bool.Parse(reader["IsCheap"].ToString()),
						LimitCount = int.Parse(reader["LimitCount"].ToString()),
						StartDate = DateTime.Parse(reader["StartDate"].ToString()),
						EndDate = DateTime.Parse(reader["EndDate"].ToString())
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", e);
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

		// Token: 0x060001E6 RID: 486 RVA: 0x0002A384 File Offset: 0x00028584
		public MissionInfo[] GetAllMissionInfo()
		{
			List<MissionInfo> list = new List<MissionInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Mission_Info_All");
				while (resultDataReader.Read())
				{
					MissionInfo item = new MissionInfo
					{
						Id = (int)resultDataReader["ID"],
						Name = ((resultDataReader["Name"] == null) ? "" : resultDataReader["Name"].ToString()),
						TotalCount = (int)resultDataReader["TotalCount"],
						TotalTurn = (int)resultDataReader["TotalTurn"],
						Script = ((resultDataReader["Script"] == null) ? "" : resultDataReader["Script"].ToString()),
						Success = ((resultDataReader["Success"] == null) ? "" : resultDataReader["Success"].ToString()),
						Failure = ((resultDataReader["Failure"] == null) ? "" : resultDataReader["Failure"].ToString()),
						Description = ((resultDataReader["Description"] == null) ? "" : resultDataReader["Description"].ToString()),
						IncrementDelay = (int)resultDataReader["IncrementDelay"],
						Delay = (int)resultDataReader["Delay"],
						Title = ((resultDataReader["Title"] == null) ? "" : resultDataReader["Title"].ToString()),
						Param1 = (int)resultDataReader["Param1"],
						Param2 = (int)resultDataReader["Param2"],
						TryAgain = (bool)resultDataReader["TryAgain"],
						TryAgainCost = (int)resultDataReader["TryAgainCost"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllMissionInfo", exception);
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

		// Token: 0x060001E7 RID: 487 RVA: 0x0002A634 File Offset: 0x00028834
		public NpcInfo[] GetAllNPCInfo()
		{
			List<NpcInfo> list = new List<NpcInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_NPC_Info_All");
				while (resultDataReader.Read())
				{
					NpcInfo item = new NpcInfo
					{
						ID = (int)resultDataReader["ID"],
						Name = ((resultDataReader["Name"] == null) ? "" : resultDataReader["Name"].ToString()),
						Level = (int)resultDataReader["Level"],
						Camp = (int)resultDataReader["Camp"],
						Type = (int)resultDataReader["Type"],
						Blood = (int)resultDataReader["Blood"],
						X = (int)resultDataReader["X"],
						Y = (int)resultDataReader["Y"],
						Width = (int)resultDataReader["Width"],
						Height = (int)resultDataReader["Height"],
						MoveMin = (int)resultDataReader["MoveMin"],
						MoveMax = (int)resultDataReader["MoveMax"],
						BaseDamage = (int)resultDataReader["BaseDamage"],
						BaseGuard = (int)resultDataReader["BaseGuard"],
						Attack = (int)resultDataReader["Attack"],
						Defence = (int)resultDataReader["Defence"],
						Agility = (int)resultDataReader["Agility"],
						Lucky = (int)resultDataReader["Lucky"],
						ModelID = ((resultDataReader["ModelID"] == null) ? "" : resultDataReader["ModelID"].ToString()),
						ResourcesPath = ((resultDataReader["ResourcesPath"] == null) ? "" : resultDataReader["ResourcesPath"].ToString()),
						DropRate = ((resultDataReader["DropRate"] == null) ? "" : resultDataReader["DropRate"].ToString()),
						Experience = (int)resultDataReader["Experience"],
						Delay = (int)resultDataReader["Delay"],
						Immunity = (int)resultDataReader["Immunity"],
						Alert = (int)resultDataReader["Alert"],
						Range = (int)resultDataReader["Range"],
						Preserve = (int)resultDataReader["Preserve"],
						Script = ((resultDataReader["Script"] == null) ? "" : resultDataReader["Script"].ToString()),
						FireX = (int)resultDataReader["FireX"],
						FireY = (int)resultDataReader["FireY"],
						DropId = (int)resultDataReader["DropId"],
						CurrentBallId = (int)resultDataReader["CurrentBallId"],
						speed = (int)resultDataReader["speed"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllNPCInfo", exception);
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

		// Token: 0x060001E8 RID: 488 RVA: 0x0002AA70 File Offset: 0x00028C70
		public PropInfo[] GetAllProp()
		{
			List<PropInfo> list = new List<PropInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Prop_All");
				while (resultDataReader.Read())
				{
					PropInfo item = new PropInfo
					{
						AffectArea = (int)resultDataReader["AffectArea"],
						AffectTimes = (int)resultDataReader["AffectTimes"],
						AttackTimes = (int)resultDataReader["AttackTimes"],
						BoutTimes = (int)resultDataReader["BoutTimes"],
						BuyGold = (int)resultDataReader["BuyGold"],
						BuyMoney = (int)resultDataReader["BuyMoney"],
						Category = (int)resultDataReader["Category"],
						Delay = (int)resultDataReader["Delay"],
						Description = resultDataReader["Description"].ToString(),
						Icon = resultDataReader["Icon"].ToString(),
						ID = (int)resultDataReader["ID"],
						Name = resultDataReader["Name"].ToString(),
						Parameter = (int)resultDataReader["Parameter"],
						Pic = resultDataReader["Pic"].ToString(),
						Property1 = (int)resultDataReader["Property1"],
						Property2 = (int)resultDataReader["Property2"],
						Property3 = (int)resultDataReader["Property3"],
						Random = (int)resultDataReader["Random"],
						Script = resultDataReader["Script"].ToString()
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001E9 RID: 489 RVA: 0x0002AD04 File Offset: 0x00028F04
		public QQtipsMessagesInfo[] GetAllQQtipsMessagesLoad()
		{
			List<QQtipsMessagesInfo> list = new List<QQtipsMessagesInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_QQtipsMessages_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitQQtipsMessagesLoad(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllQQtipsMessagesLoad", exception);
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

		// Token: 0x060001EA RID: 490 RVA: 0x0002ADC4 File Offset: 0x00028FC4
		public QuestInfo[] GetALlQuest()
		{
			List<QuestInfo> list = new List<QuestInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Quest_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitQuest(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001EB RID: 491 RVA: 0x0002AE84 File Offset: 0x00029084
		public EventLiveInfo[] GetAllEventLive()
		{
			List<EventLiveInfo> list = new List<EventLiveInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Event_Live_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitEventLiveInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001EC RID: 492 RVA: 0x0002AF44 File Offset: 0x00029144
		public QuestConditionInfo[] GetAllQuestCondiction()
		{
			List<QuestConditionInfo> list = new List<QuestConditionInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Quest_Condiction_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitQuestCondiction(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001ED RID: 493 RVA: 0x0002B004 File Offset: 0x00029204
		public QuestRateInfo[] GetAllQuestRate()
		{
			List<QuestRateInfo> list = new List<QuestRateInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Quest_Rate_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitQuestRate(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001EE RID: 494 RVA: 0x0002B0C4 File Offset: 0x000292C4
		public QuestAwardInfo[] GetAllQuestGoods()
		{
			List<QuestAwardInfo> list = new List<QuestAwardInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Quest_Goods_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitQuestGoods(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001EF RID: 495 RVA: 0x0002B184 File Offset: 0x00029384
		public EventLiveGoods[] GetAllEventLiveGoods()
		{
			List<EventLiveGoods> list = new List<EventLiveGoods>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Event_LiveGoods_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitEventLiveGoods(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001F0 RID: 496 RVA: 0x0002B244 File Offset: 0x00029444
		public List<RefineryInfo> GetAllRefineryInfo()
		{
			List<RefineryInfo> list = new List<RefineryInfo>();
			SqlDataReader resultDataReader = null;
			List<RefineryInfo> result;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Item_Refinery_All");
				while (resultDataReader.Read())
				{
					RefineryInfo item = new RefineryInfo
					{
						RefineryID = (int)resultDataReader["RefineryID"]
					};
					item.m_Equip.Add((int)resultDataReader["Equip1"]);
					item.m_Equip.Add((int)resultDataReader["Equip2"]);
					item.m_Equip.Add((int)resultDataReader["Equip3"]);
					item.m_Equip.Add((int)resultDataReader["Equip4"]);
					item.Item1 = (int)resultDataReader["Item1"];
					item.Item2 = (int)resultDataReader["Item2"];
					item.Item3 = (int)resultDataReader["Item3"];
					item.Item1Count = (int)resultDataReader["Item1Count"];
					item.Item2Count = (int)resultDataReader["Item2Count"];
					item.Item3Count = (int)resultDataReader["Item3Count"];
					item.m_Reward.Add((int)resultDataReader["Material1"]);
					item.m_Reward.Add((int)resultDataReader["Operate1"]);
					item.m_Reward.Add((int)resultDataReader["Reward1"]);
					item.m_Reward.Add((int)resultDataReader["Material2"]);
					item.m_Reward.Add((int)resultDataReader["Operate2"]);
					item.m_Reward.Add((int)resultDataReader["Reward2"]);
					item.m_Reward.Add((int)resultDataReader["Material3"]);
					item.m_Reward.Add((int)resultDataReader["Operate3"]);
					item.m_Reward.Add((int)resultDataReader["Reward3"]);
					item.m_Reward.Add((int)resultDataReader["Material4"]);
					item.m_Reward.Add((int)resultDataReader["Operate4"]);
					item.m_Reward.Add((int)resultDataReader["Reward4"]);
					list.Add(item);
				}
				result = list;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllRefineryInfo", exception);
					result = list;
				}
				else
				{
					result = list;
				}
			}
			finally
			{
				bool flag = resultDataReader != null && resultDataReader.IsClosed;
				if (flag)
				{
					resultDataReader.Close();
				}
			}
			return result;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0002B57C File Offset: 0x0002977C
		public StrengthenInfo[] GetAllRefineryStrengthen()
		{
			List<StrengthenInfo> list = new List<StrengthenInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Item_Refinery_Strengthen_All");
				while (resultDataReader.Read())
				{
					StrengthenInfo item = new StrengthenInfo
					{
						StrengthenLevel = (int)resultDataReader["StrengthenLevel"],
						Rock = (int)resultDataReader["Rock"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllRefineryStrengthen", exception);
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

		// Token: 0x060001F2 RID: 498 RVA: 0x0002B66C File Offset: 0x0002986C
		public SearchGoodsTempInfo[] GetAllSearchGoodsTemp()
		{
			List<SearchGoodsTempInfo> list = new List<SearchGoodsTempInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_SearchGoodsTemp_All");
				while (resultDataReader.Read())
				{
					SearchGoodsTempInfo item = new SearchGoodsTempInfo
					{
						StarID = (int)resultDataReader["StarID"],
						NeedMoney = (int)resultDataReader["NeedMoney"],
						DestinationReward = (int)resultDataReader["DestinationReward"],
						VIPLevel = (int)resultDataReader["VIPLevel"],
						ExtractNumber = ((resultDataReader["ExtractNumber"] == null) ? "" : resultDataReader["ExtractNumber"].ToString())
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllDaily", exception);
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

		// Token: 0x060001F3 RID: 499 RVA: 0x0002B7BC File Offset: 0x000299BC
		public ShopGoodsShowListInfo[] GetAllShopGoodsShowList()
		{
			List<ShopGoodsShowListInfo> list = new List<ShopGoodsShowListInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_ShopGoodsShowList_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitShopGoodsShowListInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001F4 RID: 500 RVA: 0x0002B87C File Offset: 0x00029A7C
		public StrengthenInfo[] GetAllStrengthen()
		{
			List<StrengthenInfo> list = new List<StrengthenInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Item_Strengthen_All");
				while (resultDataReader.Read())
				{
					StrengthenInfo item = new StrengthenInfo
					{
						StrengthenLevel = (int)resultDataReader["StrengthenLevel"],
						Rock = (int)resultDataReader["Rock"],
						Rock1 = (int)resultDataReader["Rock1"],
						Rock2 = (int)resultDataReader["Rock2"],
						Rock3 = (int)resultDataReader["Rock3"],
						StoneLevelMin = (int)resultDataReader["StoneLevelMin"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllStrengthen", exception);
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

		// Token: 0x060001F5 RID: 501 RVA: 0x0002B9CC File Offset: 0x00029BCC
		public StrengThenExpInfo[] GetAllStrengThenExp()
		{
			List<StrengThenExpInfo> infos = new List<StrengThenExpInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_StrengThenExp_All");
				while (reader.Read())
				{
					infos.Add(new StrengThenExpInfo
					{
						ID = (int)reader["ID"],
						Level = (int)reader["Level"],
						Exp = (int)reader["Exp"],
						NecklaceStrengthExp = (int)reader["NecklaceStrengthExp"],
						NecklaceStrengthPlus = (int)reader["NecklaceStrengthPlus"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetStrengThenExpInfo", e);
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

		// Token: 0x060001F6 RID: 502 RVA: 0x0002BB04 File Offset: 0x00029D04
		public StrengthenGoodsInfo[] GetAllStrengthenGoodsInfo()
		{
			List<StrengthenGoodsInfo> list = new List<StrengthenGoodsInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Item_StrengthenGoodsInfo_All");
				while (resultDataReader.Read())
				{
					StrengthenGoodsInfo item = new StrengthenGoodsInfo
					{
						ID = (int)resultDataReader["ID"],
						Level = (int)resultDataReader["Level"],
						CurrentEquip = (int)resultDataReader["CurrentEquip"],
						GainEquip = (int)resultDataReader["GainEquip"],
						OrginEquip = (int)resultDataReader["OrginEquip"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllStrengthenGoodsInfo", exception);
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

		// Token: 0x060001F7 RID: 503 RVA: 0x0002BC40 File Offset: 0x00029E40
		public LoadUserBoxInfo[] GetAllTimeBoxAward()
		{
			List<LoadUserBoxInfo> list = new List<LoadUserBoxInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_TimeBox_Award_All");
				while (resultDataReader.Read())
				{
					LoadUserBoxInfo item = new LoadUserBoxInfo
					{
						ID = (int)resultDataReader["ID"],
						Type = (int)resultDataReader["Type"],
						Level = (int)resultDataReader["Level"],
						Condition = (int)resultDataReader["Condition"],
						TemplateID = (int)resultDataReader["TemplateID"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllDaily", exception);
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

		// Token: 0x060001F8 RID: 504 RVA: 0x0002BD7C File Offset: 0x00029F7C
		public UserBoxInfo[] GetAllUserBox()
		{
			List<UserBoxInfo> list = new List<UserBoxInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_TimeBox_Award_All");
				while (resultDataReader.Read())
				{
					UserBoxInfo item = new UserBoxInfo
					{
						ID = (int)resultDataReader["ID"],
						Type = (int)resultDataReader["Type"],
						Level = (int)resultDataReader["Level"],
						Condition = (int)resultDataReader["Condition"],
						TemplateID = (int)resultDataReader["TemplateID"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllUserBox", exception);
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

		// Token: 0x060001F9 RID: 505 RVA: 0x0002BEB8 File Offset: 0x0002A0B8
		public EventAwardInfo[] GetEventAwardInfos()
		{
			List<EventAwardInfo> list = new List<EventAwardInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_EventAwardItem_All");
				while (reader.Read())
				{
					EventAwardInfo item = new EventAwardInfo
					{
						ID = (int)reader["ID"],
						ActivityType = (int)reader["ActivityType"],
						TemplateID = (int)reader["TemplateID"],
						Count = (int)reader["Count"],
						ValidDate = (int)reader["ValidDate"],
						IsBinds = (bool)reader["IsBinds"],
						StrengthenLevel = (int)reader["StrengthenLevel"],
						AttackCompose = (int)reader["AttackCompose"],
						DefendCompose = (int)reader["DefendCompose"],
						AgilityCompose = (int)reader["AgilityCompose"],
						LuckCompose = (int)reader["LuckCompose"],
						Random = (int)reader["Random"],
						IsSelect = (bool)reader["IsSelect"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllEventAward", exception);
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

		// Token: 0x060001FA RID: 506 RVA: 0x0002C0B8 File Offset: 0x0002A2B8
		public ItemTemplateInfo[] GetFusionType()
		{
			List<ItemTemplateInfo> list = new List<ItemTemplateInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Items_FusionType");
				while (resultDataReader.Read())
				{
					list.Add(this.InitItemTemplateInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001FB RID: 507 RVA: 0x0002C178 File Offset: 0x0002A378
		public ItemBoxInfo[] GetItemBoxInfos()
		{
			List<ItemBoxInfo> list = new List<ItemBoxInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_ItemsBox_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitItemBoxInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					ILog log = this.log;
					string str = "Init@Shop_Goods_Box：";
					Exception ex = exception;
					log.Error(str + ((ex != null) ? ex.ToString() : null));
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

		// Token: 0x060001FC RID: 508 RVA: 0x0002C248 File Offset: 0x0002A448
		public ItemTemplateInfo[] GetSingleCategory(int CategoryID)
		{
			List<ItemTemplateInfo> list = new List<ItemTemplateInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@CategoryID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = CategoryID;
				this.db.GetReader(ref resultDataReader, "SP_Items_Category_Single", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitItemTemplateInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001FD RID: 509 RVA: 0x0002C330 File Offset: 0x0002A530
		public ItemTemplateInfo GetSingleGoods(int goodsID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = goodsID;
				this.db.GetReader(ref resultDataReader, "SP_Items_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitItemTemplateInfo(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001FE RID: 510 RVA: 0x0002C404 File Offset: 0x0002A604
		public ItemBoxInfo[] GetSingleItemsBox(int DataID)
		{
			List<ItemBoxInfo> list = new List<ItemBoxInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = DataID;
				this.db.GetReader(ref resultDataReader, "SP_ItemsBox_Single", sqlParameters);
				while (resultDataReader.Read())
				{
					list.Add(this.InitItemBoxInfo(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x060001FF RID: 511 RVA: 0x0002C4EC File Offset: 0x0002A6EC
		public QuestInfo GetSingleQuest(int questID)
		{
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@QuestID", SqlDbType.Int, 4)
				};
				sqlParameters[0].Value = questID;
				this.db.GetReader(ref resultDataReader, "SP_Quest_Single", sqlParameters);
				bool flag = resultDataReader.Read();
				if (flag)
				{
					return this.InitQuest(resultDataReader);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x06000200 RID: 512 RVA: 0x0002C5C0 File Offset: 0x0002A7C0
		public AchievementInfo InitAchievement(SqlDataReader reader)
		{
			return new AchievementInfo
			{
				ID = (int)reader["ID"],
				PlaceID = (int)reader["PlaceID"],
				Title = ((reader["Title"] == null) ? "" : reader["Title"].ToString()),
				Detail = ((reader["Detail"] == null) ? "" : reader["Detail"].ToString()),
				NeedMinLevel = (int)reader["NeedMinLevel"],
				NeedMaxLevel = (int)reader["NeedMaxLevel"],
				PreAchievementID = ((reader["PreAchievementID"] == null) ? "" : reader["PreAchievementID"].ToString()),
				IsOther = (int)reader["IsOther"],
				AchievementType = (int)reader["AchievementType"],
				CanHide = (bool)reader["CanHide"],
				StartDate = (DateTime)reader["StartDate"],
				EndDate = (DateTime)reader["EndDate"],
				AchievementPoint = (int)reader["AchievementPoint"],
				IsActive = (int)reader["IsActive"],
				PicID = (int)reader["PicID"],
				IsShare = (bool)reader["IsShare"]
			};
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0002C784 File Offset: 0x0002A984
		public AchievementConditionInfo InitAchievementCondition(SqlDataReader reader)
		{
			return new AchievementConditionInfo
			{
				AchievementID = (int)reader["AchievementID"],
				CondictionID = (int)reader["CondictionID"],
				CondictionType = (int)reader["CondictionType"],
				Condiction_Para1 = ((reader["Condiction_Para1"] == null) ? "" : reader["Condiction_Para1"].ToString()),
				Condiction_Para2 = (int)reader["Condiction_Para2"]
			};
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0002C824 File Offset: 0x0002AA24
		public AchievementRewardInfo InitAchievementReward(SqlDataReader reader)
		{
			return new AchievementRewardInfo
			{
				AchievementID = (int)reader["AchievementID"],
				RewardType = (int)reader["RewardType"],
				RewardPara = ((reader["RewardPara"] == null) ? "" : reader["RewardPara"].ToString()),
				RewardValueId = (int)reader["RewardValueId"],
				RewardCount = (int)reader["RewardCount"]
			};
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0002C8C4 File Offset: 0x0002AAC4
		public DropCondiction InitDropCondiction(SqlDataReader reader)
		{
			return new DropCondiction
			{
				DropId = (int)reader["DropID"],
				CondictionType = (int)reader["CondictionType"],
				Para1 = (string)reader["Para1"],
				Para2 = (string)reader["Para2"]
			};
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0002C938 File Offset: 0x0002AB38
		public DropItem InitDropItem(SqlDataReader reader)
		{
			return new DropItem
			{
				Id = (int)reader["Id"],
				DropId = (int)reader["DropId"],
				ItemId = (int)reader["ItemId"],
				ValueDate = (int)reader["ValueDate"],
				IsBind = (bool)reader["IsBind"],
				Random = (int)reader["Random"],
				BeginData = (int)reader["BeginData"],
				EndData = (int)reader["EndData"],
				IsLogs = (bool)reader["IsLogs"],
				IsTips = (bool)reader["IsTips"]
			};
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0002CA38 File Offset: 0x0002AC38
		public EdictumInfo InitEdictum(SqlDataReader reader)
		{
			return new EdictumInfo
			{
				ID = (int)reader["ID"],
				Title = ((reader["Title"] == null) ? "" : reader["Title"].ToString()),
				BeginDate = (DateTime)reader["BeginDate"],
				BeginTime = (DateTime)reader["BeginTime"],
				EndDate = (DateTime)reader["EndDate"],
				EndTime = (DateTime)reader["EndTime"],
				Text = ((reader["Text"] == null) ? "" : reader["Text"].ToString()),
				IsExist = (bool)reader["IsExist"]
			};
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0002CB30 File Offset: 0x0002AD30
		public ItemBoxInfo InitItemBoxInfo(SqlDataReader reader)
		{
			return new ItemBoxInfo
			{
				ID = (int)reader["ID"],
				TemplateId = (int)reader["TemplateId"],
				IsSelect = (bool)reader["IsSelect"],
				IsBind = (bool)reader["IsBind"],
				ItemValid = (int)reader["ItemValid"],
				ItemCount = (int)reader["ItemCount"],
				StrengthenLevel = (int)reader["StrengthenLevel"],
				AttackCompose = (int)reader["AttackCompose"],
				DefendCompose = (int)reader["DefendCompose"],
				AgilityCompose = (int)reader["AgilityCompose"],
				LuckCompose = (int)reader["LuckCompose"],
				Random = (int)reader["Random"],
				IsTips = (int)reader["IsTips"],
				IsLogs = (bool)reader["IsLogs"]
			};
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0002CC8C File Offset: 0x0002AE8C
		public ItemRecordTypeInfo InitItemRecordType(SqlDataReader reader)
		{
			return new ItemRecordTypeInfo
			{
				RecordID = (int)reader["RecordID"],
				Name = ((reader["Name"] == null) ? "" : reader["Name"].ToString()),
				Description = ((reader["Description"] == null) ? "" : reader["Description"].ToString())
			};
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0002CD10 File Offset: 0x0002AF10
		public ItemTemplateInfo InitItemTemplateInfo(SqlDataReader reader)
		{
			return new ItemTemplateInfo
			{
				AddTime = reader["AddTime"].ToString(),
				Agility = (int)reader["Agility"],
				Attack = (int)reader["Attack"],
				CanDelete = (bool)reader["CanDelete"],
				CanDrop = (bool)reader["CanDrop"],
				CanEquip = (bool)reader["CanEquip"],
				CanUse = (bool)reader["CanUse"],
				CategoryID = (int)reader["CategoryID"],
				Colors = reader["Colors"].ToString(),
				Defence = (int)reader["Defence"],
				Description = reader["Description"].ToString(),
				Level = (int)reader["Level"],
				Luck = (int)reader["Luck"],
				MaxCount = (int)reader["MaxCount"],
				Name = reader["Name"].ToString(),
				NeedSex = (int)reader["NeedSex"],
				Pic = reader["Pic"].ToString(),
				Data = ((reader["Data"] == null) ? "" : reader["Data"].ToString()),
				Property1 = (int)reader["Property1"],
				Property2 = (int)reader["Property2"],
				Property3 = (int)reader["Property3"],
				Property4 = (int)reader["Property4"],
				Property5 = (int)reader["Property5"],
				Property6 = (int)reader["Property6"],
				Property7 = (int)reader["Property7"],
				Property8 = (int)reader["Property8"],
				Quality = (int)reader["Quality"],
				Script = reader["Script"].ToString(),
				TemplateID = (int)reader["TemplateID"],
				CanCompose = (bool)reader["CanCompose"],
				CanStrengthen = (bool)reader["CanStrengthen"],
				NeedLevel = (int)reader["NeedLevel"],
				BindType = (int)reader["BindType"],
				FusionType = (int)reader["FusionType"],
				FusionRate = (int)reader["FusionRate"],
				FusionNeedRate = (int)reader["FusionNeedRate"],
				Hole = ((reader["Hole"] == null) ? "" : reader["Hole"].ToString()),
				RefineryLevel = (int)reader["RefineryLevel"],
				ReclaimValue = (int)reader["ReclaimValue"],
				ReclaimType = (int)reader["ReclaimType"],
				CanRecycle = (int)reader["CanRecycle"],
				SuitId = (int)reader["SuitID"],
				FineSuitType = (int)reader["FineSuitType"],
				IsDirty = false
			};
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0002D134 File Offset: 0x0002B334
		public QQtipsMessagesInfo InitQQtipsMessagesLoad(SqlDataReader reader)
		{
			return new QQtipsMessagesInfo
			{
				ID = (int)reader["ID"],
				title = ((reader["title"] == null) ? "QQTips" : reader["title"].ToString()),
				content = ((reader["content"] == null) ? "Thông báo, gợi ý hệ thống" : reader["content"].ToString()),
				maxLevel = (int)reader["maxLevel"],
				minLevel = (int)reader["minLevel"],
				outInType = (int)reader["outInType"],
				moduleType = (int)reader["moduleType"],
				inItemID = (int)reader["inItemID"],
				url = ((reader["url"] == null) ? "http://gunny.zing.vn" : reader["url"].ToString())
			};
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0002D258 File Offset: 0x0002B458
		public QuestInfo InitQuest(SqlDataReader reader)
		{
			return new QuestInfo
			{
				ID = (int)reader["ID"],
				QuestID = (int)reader["QuestID"],
				Title = ((reader["Title"] == null) ? "" : reader["Title"].ToString()),
				Detail = ((reader["Detail"] == null) ? "" : reader["Detail"].ToString()),
				Objective = ((reader["Objective"] == null) ? "" : reader["Objective"].ToString()),
				NeedMinLevel = (int)reader["NeedMinLevel"],
				NeedMaxLevel = (int)reader["NeedMaxLevel"],
				PreQuestID = ((reader["PreQuestID"] == null) ? "" : reader["PreQuestID"].ToString()),
				NextQuestID = ((reader["NextQuestID"] == null) ? "" : reader["NextQuestID"].ToString()),
				IsOther = (int)reader["IsOther"],
				CanRepeat = (bool)reader["CanRepeat"],
				RepeatInterval = (int)reader["RepeatInterval"],
				RepeatMax = (int)reader["RepeatMax"],
				RewardGP = (int)reader["RewardGP"],
				RewardGold = (int)reader["RewardGold"],
				RewardGiftToken = (int)reader["RewardGiftToken"],
				RewardOffer = (int)reader["RewardOffer"],
				RewardRiches = (int)reader["RewardRiches"],
				RewardBuffID = (int)reader["RewardBuffID"],
				RewardBuffDate = (int)reader["RewardBuffDate"],
				RewardMoney = (int)reader["RewardMoney"],
				Rands = (decimal)reader["Rands"],
				RandDouble = (int)reader["RandDouble"],
				TimeMode = (bool)reader["TimeMode"],
				StartDate = (DateTime)reader["StartDate"],
				EndDate = (DateTime)reader["EndDate"],
				MapID = (int)reader["MapID"],
				AutoEquip = (bool)reader["AutoEquip"],
				RewardMedal = (int)reader["RewardMedal"],
				Rank = ((reader["Rank"] == null) ? "" : reader["Rank"].ToString()),
				StarLev = (int)reader["StarLev"],
				NotMustCount = (int)reader["NotMustCount"]
			};
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0002D5C8 File Offset: 0x0002B7C8
		public QuestConditionInfo InitQuestCondiction(SqlDataReader reader)
		{
			return new QuestConditionInfo
			{
				QuestID = (int)reader["QuestID"],
				CondictionID = (int)reader["CondictionID"],
				CondictionTitle = ((reader["CondictionTitle"] == null) ? "" : reader["CondictionTitle"].ToString()),
				CondictionType = (int)reader["CondictionType"],
				Para1 = (int)reader["Para1"],
				Para2 = (int)reader["Para2"],
				isOpitional = (bool)reader["isOpitional"]
			};
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0002D694 File Offset: 0x0002B894
		public QuestAwardInfo InitQuestGoods(SqlDataReader reader)
		{
			return new QuestAwardInfo
			{
				QuestID = (int)reader["QuestID"],
				RewardItemID = (int)reader["RewardItemID"],
				IsSelect = (bool)reader["IsSelect"],
				IsBind = (bool)reader["IsBind"],
				RewardItemValid = (int)reader["RewardItemValid"],
				RewardItemCount = (int)reader["RewardItemCount"],
				StrengthenLevel = (int)reader["StrengthenLevel"],
				AttackCompose = (int)reader["AttackCompose"],
				DefendCompose = (int)reader["DefendCompose"],
				AgilityCompose = (int)reader["AgilityCompose"],
				LuckCompose = (int)reader["LuckCompose"],
				IsCount = (bool)reader["IsCount"]
			};
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0002D7C0 File Offset: 0x0002B9C0
		public QuestRateInfo InitQuestRate(SqlDataReader reader)
		{
			return new QuestRateInfo
			{
				BindMoneyRate = ((reader["BindMoneyRate"] == null) ? "" : reader["BindMoneyRate"].ToString()),
				ExpRate = ((reader["ExpRate"] == null) ? "" : reader["ExpRate"].ToString()),
				GoldRate = ((reader["GoldRate"] == null) ? "" : reader["GoldRate"].ToString()),
				ExploitRate = ((reader["ExploitRate"] == null) ? "" : reader["ExploitRate"].ToString()),
				CanOneKeyFinishTime = (int)reader["CanOneKeyFinishTime"]
			};
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0002D89C File Offset: 0x0002BA9C
		public ShopGoodsShowListInfo InitShopGoodsShowListInfo(SqlDataReader reader)
		{
			return new ShopGoodsShowListInfo
			{
				Type = (int)reader["Type"],
				ShopId = (int)reader["ShopId"]
			};
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0002D8E4 File Offset: 0x0002BAE4
		public EventLiveInfo InitEventLiveInfo(SqlDataReader reader)
		{
			return new EventLiveInfo
			{
				EventID = (int)reader["EventID"],
				Description = reader["Description"].ToString(),
				CondictionType = (int)reader["CondictionType"],
				Condiction_Para1 = (int)reader["Condiction_Para1"],
				Condiction_Para2 = (int)reader["Condiction_Para2"],
				StartDate = (DateTime)reader["StartDate"],
				EndDate = (DateTime)reader["EndDate"]
			};
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0002D99C File Offset: 0x0002BB9C
		public EventLiveGoods InitEventLiveGoods(SqlDataReader reader)
		{
			return new EventLiveGoods
			{
				EventID = (int)reader["EventID"],
				TemplateID = (int)reader["TemplateID"],
				ValidDate = (int)reader["ValidDate"],
				Count = (int)reader["Count"],
				StrengthenLevel = (int)reader["StrengthenLevel"],
				AttackCompose = (int)reader["AttackCompose"],
				DefendCompose = (int)reader["DefendCompose"],
				AgilityCompose = (int)reader["AgilityCompose"],
				LuckCompose = (int)reader["LuckCompose"],
				IsBind = (bool)reader["IsBind"]
			};
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0002DA9C File Offset: 0x0002BC9C
		public SubActiveInfo[] GetAllSubActive()
		{
			List<SubActiveInfo> list = new List<SubActiveInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_SubActive_All");
				while (resultDataReader.Read())
				{
					SubActiveInfo item = new SubActiveInfo
					{
						ID = (int)resultDataReader["ID"],
						ActiveID = (int)resultDataReader["ActiveID"],
						SubID = (int)resultDataReader["SubID"],
						IsOpen = (bool)resultDataReader["IsOpen"],
						StartDate = (DateTime)resultDataReader["StartDate"],
						StartTime = (DateTime)resultDataReader["StartTime"],
						EndDate = (DateTime)resultDataReader["EndDate"],
						EndTime = (DateTime)resultDataReader["EndTime"],
						IsContinued = (bool)resultDataReader["IsContinued"],
						ActiveInfo = ((resultDataReader["ActiveInfo"] == null) ? "" : resultDataReader["ActiveInfo"].ToString())
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init AllSubActive", exception);
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

		// Token: 0x06000212 RID: 530 RVA: 0x0002DC74 File Offset: 0x0002BE74
		public CommunalActiveAwardInfo[] GetAllCommunalActiveAward()
		{
			List<CommunalActiveAwardInfo> list = new List<CommunalActiveAwardInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_CommunalActiveAward_All");
				while (resultDataReader.Read())
				{
					CommunalActiveAwardInfo item = new CommunalActiveAwardInfo
					{
						ID = (int)resultDataReader["ID"],
						ActiveID = (int)resultDataReader["ActiveID"],
						IsArea = (int)resultDataReader["IsArea"],
						RandID = (int)resultDataReader["RandID"],
						TemplateID = (int)resultDataReader["TemplateID"],
						StrengthenLevel = (int)resultDataReader["StrengthenLevel"],
						AttackCompose = (int)resultDataReader["AttackCompose"],
						DefendCompose = (int)resultDataReader["DefendCompose"],
						AgilityCompose = (int)resultDataReader["AgilityCompose"],
						LuckCompose = (int)resultDataReader["LuckCompose"],
						Count = (int)resultDataReader["Count"],
						IsBind = (bool)resultDataReader["IsBind"],
						IsTime = (bool)resultDataReader["IsTime"],
						ValidDate = (int)resultDataReader["ValidDate"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllCommunalActiveAward", exception);
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

		// Token: 0x06000213 RID: 531 RVA: 0x0002DE94 File Offset: 0x0002C094
		public CommunalActiveExpInfo[] GetAllCommunalActiveExp()
		{
			List<CommunalActiveExpInfo> list = new List<CommunalActiveExpInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_CommunalActiveExp_All");
				while (resultDataReader.Read())
				{
					CommunalActiveExpInfo item = new CommunalActiveExpInfo
					{
						ActiveID = (int)resultDataReader["ActiveID"],
						Grade = (int)resultDataReader["Grade"],
						Exp = (int)resultDataReader["Exp"],
						AddExpPlus = (int)resultDataReader["AddExpPlus"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllCommunalActiveExp", exception);
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

		// Token: 0x06000214 RID: 532 RVA: 0x0002DFB0 File Offset: 0x0002C1B0
		public SubActiveConditionInfo[] GetAllSubActiveCondition(int ActiveID)
		{
			List<SubActiveConditionInfo> list = new List<SubActiveConditionInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@ActiveID", ActiveID)
				};
				this.db.GetReader(ref resultDataReader, "SP_SubActiveCondition_All", sqlParameters);
				while (resultDataReader.Read())
				{
					SubActiveConditionInfo item = new SubActiveConditionInfo
					{
						ID = (int)resultDataReader["ID"],
						ActiveID = (int)resultDataReader["ActiveID"],
						SubID = (int)resultDataReader["SubID"],
						ConditionID = (int)resultDataReader["ConditionID"],
						Type = (int)resultDataReader["Type"],
						Value = ((resultDataReader["Value"] == null) ? "" : resultDataReader["Value"].ToString()),
						AwardType = (int)resultDataReader["AwardType"],
						AwardValue = ((resultDataReader["AwardValue"] == null) ? "" : resultDataReader["AwardValue"].ToString()),
						IsValid = (bool)resultDataReader["IsValid"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init AllSubActive", exception);
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

		// Token: 0x06000215 RID: 533 RVA: 0x0002E1A4 File Offset: 0x0002C3A4
		public CommunalActiveInfo[] GetAllCommunalActive()
		{
			List<CommunalActiveInfo> list = new List<CommunalActiveInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_CommunalActive_All");
				while (resultDataReader.Read())
				{
					CommunalActiveInfo item = new CommunalActiveInfo
					{
						ActiveID = (int)resultDataReader["ActiveID"],
						BeginTime = (DateTime)resultDataReader["BeginTime"],
						EndTime = (DateTime)resultDataReader["EndTime"],
						LimitGrade = (int)resultDataReader["LimitGrade"],
						DayMaxScore = (int)resultDataReader["DayMaxScore"],
						MinScore = (int)resultDataReader["MinScore"],
						AddPropertyByMoney = (string)resultDataReader["AddPropertyByMoney"],
						AddPropertyByProp = (string)resultDataReader["AddPropertyByProp"],
						IsReset = (bool)resultDataReader["IsReset"],
						IsSendAward = (bool)resultDataReader["IsSendAward"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllCommunalActive", exception);
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

		// Token: 0x06000216 RID: 534 RVA: 0x0002E368 File Offset: 0x0002C568
		public FairBattleRewardInfo[] GetAllFairBattleReward()
		{
			List<FairBattleRewardInfo> list = new List<FairBattleRewardInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_FairBattleReward_All");
				while (resultDataReader.Read())
				{
					FairBattleRewardInfo item = new FairBattleRewardInfo
					{
						Prestige = (int)resultDataReader["Prestige"],
						Level = (int)resultDataReader["Level"],
						Name = (string)resultDataReader["Name"],
						PrestigeForWin = (int)resultDataReader["PrestigeForWin"],
						PrestigeForLose = (int)resultDataReader["PrestigeForLose"],
						Title = (string)resultDataReader["Title"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllFairBattleReward", exception);
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

		// Token: 0x06000217 RID: 535 RVA: 0x0002E4B4 File Offset: 0x0002C6B4
		public CardUpdateConditionInfo[] GetAllCardUpdateCondition()
		{
			List<CardUpdateConditionInfo> list = new List<CardUpdateConditionInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Get_CardUpdateCondiction");
				while (resultDataReader.Read())
				{
					CardUpdateConditionInfo item = new CardUpdateConditionInfo
					{
						Level = (int)resultDataReader["Level"],
						Exp = (int)resultDataReader["Exp"],
						MinExp = (int)resultDataReader["MinExp"],
						MaxExp = (int)resultDataReader["MaxExp"],
						UpdateCardCount = (int)resultDataReader["UpdateCardCount"],
						ResetCardCount = (int)resultDataReader["ResetCardCount"],
						ResetMoney = (int)resultDataReader["ResetMoney"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x06000218 RID: 536 RVA: 0x0002E634 File Offset: 0x0002C834
		public CardGrooveUpdateInfo[] GetAllCardGrooveUpdate()
		{
			List<CardGrooveUpdateInfo> list = new List<CardGrooveUpdateInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_CardGrooveUpdate_All");
				while (resultDataReader.Read())
				{
					list.Add(this.InitCardGrooveUpdate(resultDataReader));
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllCardGrooveUpdate", exception);
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

		// Token: 0x06000219 RID: 537 RVA: 0x0002E6F4 File Offset: 0x0002C8F4
		public CardUpdateInfo[] GetAllCardUpdateInfo()
		{
			List<CardUpdateInfo> list = new List<CardUpdateInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Get_CardUpdateInfo");
				while (resultDataReader.Read())
				{
					CardUpdateInfo item = new CardUpdateInfo
					{
						Id = (int)resultDataReader["Id"],
						Level = (int)resultDataReader["Level"],
						Attack = (int)resultDataReader["Attack"],
						Defend = (int)resultDataReader["Defend"],
						Agility = (int)resultDataReader["Agility"],
						Lucky = (int)resultDataReader["Lucky"],
						Guard = (int)resultDataReader["Guard"],
						Damage = (int)resultDataReader["Damage"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", exception);
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

		// Token: 0x0600021A RID: 538 RVA: 0x0002E88C File Offset: 0x0002CA8C
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

		// Token: 0x0600021B RID: 539 RVA: 0x0002E98C File Offset: 0x0002CB8C
		public LevelInfo[] GetAllLevel()
		{
			List<LevelInfo> list = new List<LevelInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Level_All");
				while (resultDataReader.Read())
				{
					LevelInfo item = new LevelInfo
					{
						Grade = (int)resultDataReader["Grade"],
						GP = (int)resultDataReader["GP"],
						Blood = (int)resultDataReader["Blood"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllLevel", exception);
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

		// Token: 0x0600021C RID: 540 RVA: 0x0002EA90 File Offset: 0x0002CC90
		public ExerciseInfo[] GetAllExercise()
		{
			List<ExerciseInfo> list = new List<ExerciseInfo>();
			SqlDataReader resultDataReader = null;
			try
			{
				this.db.GetReader(ref resultDataReader, "SP_Exercise_All");
				while (resultDataReader.Read())
				{
					ExerciseInfo item = new ExerciseInfo
					{
						Grage = (int)resultDataReader["Grage"],
						GP = (int)resultDataReader["GP"],
						ExerciseA = (int)resultDataReader["ExerciseA"],
						ExerciseAG = (int)resultDataReader["ExerciseAG"],
						ExerciseD = (int)resultDataReader["ExerciseD"],
						ExerciseH = (int)resultDataReader["ExerciseH"],
						ExerciseL = (int)resultDataReader["ExerciseL"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllExercise", exception);
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

		// Token: 0x0600021D RID: 541 RVA: 0x0002EC08 File Offset: 0x0002CE08
		public PetConfig[] GetAllPetConfig()
		{
			List<PetConfig> petConfigList = new List<PetConfig>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_PetConfig_All");
				while (ResultDataReader.Read())
				{
					petConfigList.Add(new PetConfig
					{
						Name = ResultDataReader["Name"].ToString(),
						Value = ResultDataReader["Value"].ToString()
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllPetConfig", ex);
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
			return petConfigList.ToArray();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0002ECF4 File Offset: 0x0002CEF4
		public PetLevel[] GetAllPetLevel()
		{
			List<PetLevel> petLevelList = new List<PetLevel>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_PetLevel_All");
				while (ResultDataReader.Read())
				{
					petLevelList.Add(new PetLevel
					{
						Level = (int)ResultDataReader["Level"],
						GP = (int)ResultDataReader["GP"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllPetLevel", ex);
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
			return petLevelList.ToArray();
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0002EDE0 File Offset: 0x0002CFE0
		public PetTemplateInfo[] GetAllPetTemplateInfo()
		{
			List<PetTemplateInfo> petTemplateInfoList = new List<PetTemplateInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_PetTemplateInfo_All");
				while (ResultDataReader.Read())
				{
					petTemplateInfoList.Add(new PetTemplateInfo
					{
						TemplateID = (int)ResultDataReader["TemplateID"],
						Name = (string)ResultDataReader["Name"],
						KindID = (int)ResultDataReader["KindID"],
						Description = (string)ResultDataReader["Description"],
						Pic = (string)ResultDataReader["Pic"],
						RareLevel = (int)ResultDataReader["RareLevel"],
						MP = (int)ResultDataReader["MP"],
						StarLevel = (int)ResultDataReader["StarLevel"],
						GameAssetUrl = (string)ResultDataReader["GameAssetUrl"],
						HighAgility = (int)ResultDataReader["HighAgility"],
						HighAgilityGrow = (int)ResultDataReader["HighAgilityGrow"],
						HighAttack = (int)ResultDataReader["HighAttack"],
						HighAttackGrow = (int)ResultDataReader["HighAttackGrow"],
						HighBlood = (int)ResultDataReader["HighBlood"],
						HighBloodGrow = (int)ResultDataReader["HighBloodGrow"],
						HighDamage = (int)ResultDataReader["HighDamage"],
						HighDamageGrow = (int)ResultDataReader["HighDamageGrow"],
						HighDefence = (int)ResultDataReader["HighDefence"],
						HighDefenceGrow = (int)ResultDataReader["HighDefenceGrow"],
						HighGuard = (int)ResultDataReader["HighGuard"],
						HighGuardGrow = (int)ResultDataReader["HighGuardGrow"],
						HighLuck = (int)ResultDataReader["HighLuck"],
						HighLuckGrow = (int)ResultDataReader["HighLuckGrow"],
						WashGetCount = (int)ResultDataReader["WashGetCount"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllPetTemplateInfo", ex);
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
			return petTemplateInfoList.ToArray();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0002F0E4 File Offset: 0x0002D2E4
		public PetSkillTemplateInfo[] GetAllPetSkillTemplateInfo()
		{
			List<PetSkillTemplateInfo> skillTemplateInfoList = new List<PetSkillTemplateInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_PetSkillTemplateInfo_All");
				while (ResultDataReader.Read())
				{
					skillTemplateInfoList.Add(new PetSkillTemplateInfo
					{
						PetTemplateID = (int)ResultDataReader["PetTemplateID"],
						KindID = (int)ResultDataReader["KindID"],
						GetTypes = (int)ResultDataReader["GetType"],
						SkillID = (int)ResultDataReader["SkillID"],
						SkillBookID = (int)ResultDataReader["SkillBookID"],
						MinLevel = (int)ResultDataReader["MinLevel"],
						DeleteSkillIDs = ResultDataReader["DeleteSkillIDs"].ToString()
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllPetSkillTemplateInfo", ex);
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
			return skillTemplateInfoList.ToArray();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0002F260 File Offset: 0x0002D460
		public PetSkillInfo[] GetAllPetSkillInfo()
		{
			List<PetSkillInfo> petSkillInfoList = new List<PetSkillInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_PetSkillInfo_All");
				while (ResultDataReader.Read())
				{
					petSkillInfoList.Add(new PetSkillInfo
					{
						ID = (int)ResultDataReader["ID"],
						Name = ResultDataReader["Name"].ToString(),
						ElementIDs = ResultDataReader["ElementIDs"].ToString(),
						Description = ResultDataReader["Description"].ToString(),
						BallType = (int)ResultDataReader["BallType"],
						NewBallID = (int)ResultDataReader["NewBallID"],
						CostMP = (int)ResultDataReader["CostMP"],
						Pic = (int)ResultDataReader["Pic"],
						Action = ResultDataReader["Action"].ToString(),
						EffectPic = ResultDataReader["EffectPic"].ToString(),
						Delay = (int)ResultDataReader["Delay"],
						ColdDown = (int)ResultDataReader["ColdDown"],
						GameType = (int)ResultDataReader["GameType"],
						Probability = (int)ResultDataReader["Probability"],
						Damage = (int)ResultDataReader["Damage"],
						DamageCrit = (int)ResultDataReader["DamageCrit"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllPetSkillInfo", ex);
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
			return petSkillInfoList.ToArray();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0002F4AC File Offset: 0x0002D6AC
		public PetSkillElementInfo[] GetAllPetSkillElementInfo()
		{
			List<PetSkillElementInfo> skillElementInfoList = new List<PetSkillElementInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_PetSkillElementInfo_All");
				while (ResultDataReader.Read())
				{
					skillElementInfoList.Add(new PetSkillElementInfo
					{
						ID = (int)ResultDataReader["ID"],
						Name = ResultDataReader["Name"].ToString(),
						EffectPic = ResultDataReader["EffectPic"].ToString(),
						Description = ResultDataReader["Description"].ToString(),
						Pic = (int)ResultDataReader["Pic"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllPetSkillElementInfo", ex);
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
			return skillElementInfoList.ToArray();
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0002F5E4 File Offset: 0x0002D7E4
		public PetExpItemPriceInfo[] GetAllPetExpItemPrice()
		{
			List<PetExpItemPriceInfo> infos = new List<PetExpItemPriceInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_PetExpItemPriceInfo_All");
				while (reader.Read())
				{
					infos.Add(new PetExpItemPriceInfo
					{
						ID = (int)reader["ID"],
						Count = (int)reader["Count"],
						Money = (int)reader["Money"],
						ItemCount = (int)reader["ItemCount"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllPetTemplateInfo", e);
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

		// Token: 0x06000224 RID: 548 RVA: 0x0002F700 File Offset: 0x0002D900
		public PetFightPropertyInfo[] GetAllPetFightProperty()
		{
			List<PetFightPropertyInfo> fightPropertyInfoList = new List<PetFightPropertyInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_PetFightProperty_All");
				while (ResultDataReader.Read())
				{
					fightPropertyInfoList.Add(new PetFightPropertyInfo
					{
						ID = (int)ResultDataReader["ID"],
						Exp = (int)ResultDataReader["Exp"],
						Attack = (int)ResultDataReader["Attack"],
						Agility = (int)ResultDataReader["Agility"],
						Defence = (int)ResultDataReader["Defence"],
						Lucky = (int)ResultDataReader["Lucky"],
						Blood = (int)ResultDataReader["Blood"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllPetFightProperty", ex);
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
			return fightPropertyInfoList.ToArray();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0002F87C File Offset: 0x0002DA7C
		public PetStarExpInfo[] GetAllPetStarExp()
		{
			List<PetStarExpInfo> petStarExpInfoList = new List<PetStarExpInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_PetStarExp_All");
				while (ResultDataReader.Read())
				{
					petStarExpInfoList.Add(new PetStarExpInfo
					{
						Exp = (int)ResultDataReader["Exp"],
						OldID = (int)ResultDataReader["OldID"],
						NewID = (int)ResultDataReader["NewID"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllPetStarExp", ex);
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
			return petStarExpInfoList.ToArray();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0002F980 File Offset: 0x0002DB80
		public ConsortiaBuffTempInfo[] GetAllConsortiaBuffTemp()
		{
			List<ConsortiaBuffTempInfo> consortiaBuffTempInfoList = new List<ConsortiaBuffTempInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_Consortia_Buff_Temp_All");
				while (ResultDataReader.Read())
				{
					consortiaBuffTempInfoList.Add(new ConsortiaBuffTempInfo
					{
						id = (int)ResultDataReader["id"],
						name = (string)ResultDataReader["name"],
						descript = (string)ResultDataReader["descript"],
						type = (int)ResultDataReader["type"],
						level = (int)ResultDataReader["level"],
						value = (int)ResultDataReader["value"],
						riches = (int)ResultDataReader["riches"],
						metal = (int)ResultDataReader["metal"],
						pic = (int)ResultDataReader["pic"],
						group = (int)ResultDataReader["group"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllConsortiaBuffTemp", ex);
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
			return consortiaBuffTempInfoList.ToArray();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0002FB40 File Offset: 0x0002DD40
		public ConsortiaLevelInfo[] GetAllConsortiaLevel()
		{
			List<ConsortiaLevelInfo> consortiaLevelInfoList = new List<ConsortiaLevelInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_Consortia_Level_All");
				while (ResultDataReader.Read())
				{
					consortiaLevelInfoList.Add(new ConsortiaLevelInfo
					{
						Count = (int)ResultDataReader["Count"],
						Deduct = (int)ResultDataReader["Deduct"],
						Level = (int)ResultDataReader["Level"],
						NeedGold = (int)ResultDataReader["NeedGold"],
						NeedItem = (int)ResultDataReader["NeedItem"],
						Reward = (int)ResultDataReader["Reward"],
						Riches = (int)ResultDataReader["Riches"],
						ShopRiches = (int)ResultDataReader["ShopRiches"],
						SmithRiches = (int)ResultDataReader["SmithRiches"],
						StoreRiches = (int)ResultDataReader["StoreRiches"],
						BufferRiches = (int)ResultDataReader["BufferRiches"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllConsortiaLevel", ex);
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
			return consortiaLevelInfoList.ToArray();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0002FD18 File Offset: 0x0002DF18
		public ConsortiaBadgeConfigInfo[] GetAllConsortiaBadgeConfig()
		{
			List<ConsortiaBadgeConfigInfo> consortiaBadgeConfigInfoList = new List<ConsortiaBadgeConfigInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_Consortia_Badge_Config_All");
				while (ResultDataReader.Read())
				{
					consortiaBadgeConfigInfoList.Add(this.InitConsortiaBadgeConfig(ResultDataReader));
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllConsortiaBadgeConfig", ex);
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
			return consortiaBadgeConfigInfoList.ToArray();
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0002FDD8 File Offset: 0x0002DFD8
		public ConsortiaBadgeConfigInfo InitConsortiaBadgeConfig(SqlDataReader reader)
		{
			return new ConsortiaBadgeConfigInfo
			{
				BadgeID = (int)reader["BadgeID"],
				BadgeName = ((reader["BadgeName"] == null) ? "" : reader["BadgeName"].ToString()),
				Cost = (int)reader["Cost"],
				LimitLevel = (int)reader["LimitLevel"],
				ValidDate = (int)reader["ValidDate"]
			};
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0002FE78 File Offset: 0x0002E078
		public GoldEquipTemplateInfo[] GetAllGoldEquipTemplateLoad()
		{
			List<GoldEquipTemplateInfo> equipTemplateInfoList = new List<GoldEquipTemplateInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_GoldEquipTemplateLoad_All");
				while (ResultDataReader.Read())
				{
					equipTemplateInfoList.Add(this.InitGoldEquipTemplateLoad(ResultDataReader));
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllGoldEquipTemplateLoad", ex);
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
			return equipTemplateInfoList.ToArray();
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0002FF38 File Offset: 0x0002E138
		public GoldEquipTemplateInfo InitGoldEquipTemplateLoad(SqlDataReader reader)
		{
			return new GoldEquipTemplateInfo
			{
				ID = (int)reader["ID"],
				OldTemplateId = (int)reader["OldTemplateId"],
				NewTemplateId = (int)reader["NewTemplateId"],
				CategoryID = (int)reader["CategoryID"],
				Strengthen = (int)reader["Strengthen"],
				Attack = (int)reader["Attack"],
				Defence = (int)reader["Defence"],
				Agility = (int)reader["Agility"],
				Luck = (int)reader["Luck"],
				Damage = (int)reader["Damage"],
				Guard = (int)reader["Guard"],
				Boold = (int)reader["Boold"],
				BlessID = (int)reader["BlessID"],
				Pic = ((reader["pic"] == DBNull.Value) ? "" : reader["pic"].ToString())
			};
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000300AC File Offset: 0x0002E2AC
		public PetMoePropertyInfo[] GetAllPetMoeProperty()
		{
			List<PetMoePropertyInfo> infos = new List<PetMoePropertyInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Pet_Moe_Property_All");
				while (reader.Read())
				{
					infos.Add(this.InitPetMoePropertyInfo(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("InitPetMoePropertyInfo", e);
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

		// Token: 0x0600022D RID: 557 RVA: 0x0003016C File Offset: 0x0002E36C
		public PetMoePropertyInfo InitPetMoePropertyInfo(SqlDataReader dr)
		{
			return new PetMoePropertyInfo
			{
				Level = (int)dr["Level"],
				Attack = (int)dr["Attack"],
				Lucky = (int)dr["Lucky"],
				Agility = (int)dr["Agility"],
				Blood = (int)dr["Blood"],
				Defence = (int)dr["Defence"],
				Guard = (int)dr["Guard"],
				Exp = (int)dr["Exp"]
			};
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00030240 File Offset: 0x0002E440
		public void Update_Suit_Kill(Suit_Manager A)
		{
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@kill", A.Kill_List),
					new SqlParameter("@UserID", A.UserID)
				};
				this.db.RunProcedure("SP_Suit_Manager_Update", para);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("SP_Suit_Manager_Update error!", e);
				}
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000302D0 File Offset: 0x0002E4D0
		public void Reset_Suit_Kill(int UserID)
		{
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", UserID)
				};
				this.db.RunProcedure("SP_Suit_Manager_Reset", para);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("SP_Suit_Manager_Reset error!", e);
				}
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00030348 File Offset: 0x0002E548
		public Suit_TemplateID[] Load_Suit_TemplateID()
		{
			List<Suit_TemplateID> infos = new List<Suit_TemplateID>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Suit_TemplateID");
				while (reader.Read())
				{
					infos.Add(new Suit_TemplateID
					{
						ID = (int)reader["ID"],
						ContainEquip = (string)reader["ContainEquip"],
						PartName = (string)reader["PartName"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("SP_Suit_TemplateID", e);
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

		// Token: 0x06000231 RID: 561 RVA: 0x0003044C File Offset: 0x0002E64C
		public Suit_TemplateInfo[] Load_Suit_TemplateInfo()
		{
			List<Suit_TemplateInfo> infos = new List<Suit_TemplateInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Suit_TemplateInfo");
				while (reader.Read())
				{
					infos.Add(new Suit_TemplateInfo
					{
						SuitId = (int)reader["SuitId"],
						SuitName = (string)reader["SuitName"],
						EqipCount1 = (int)reader["EqipCount1"],
						SkillDescribe1 = (string)reader["SkillDescribe1"],
						Skill1 = (string)reader["Skill1"],
						EqipCount2 = (int)reader["EqipCount2"],
						SkillDescribe2 = (string)reader["SkillDescribe2"],
						Skill2 = (string)reader["Skill2"],
						EqipCount3 = (int)reader["EqipCount3"],
						SkillDescribe3 = (string)reader["SkillDescribe3"],
						Skill3 = (string)reader["Skill3"],
						EqipCount4 = (int)reader["EqipCount4"],
						SkillDescribe4 = (string)reader["SkillDescribe4"],
						Skill4 = (string)reader["Skill4"],
						EqipCount5 = (int)reader["EqipCount5"],
						SkillDescribe5 = (string)reader["SkillDescribe5"],
						Skill5 = (string)reader["Skill5"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("SP_Suit_TemplateInfo", e);
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

		// Token: 0x06000232 RID: 562 RVA: 0x000306B0 File Offset: 0x0002E8B0
		public AccumulAtiveLoginAwardInfo[] GetAccumulAtiveLoginAwardInfos()
		{
			List<AccumulAtiveLoginAwardInfo> infos = new List<AccumulAtiveLoginAwardInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_AccumulAtiveLoginAward_All");
				while (reader.Read())
				{
					infos.Add(new AccumulAtiveLoginAwardInfo
					{
						ID = (int)reader["ID"],
						RewardItemID = (int)reader["RewardItemID"],
						Type = (int)reader["Type"],
						IsSelect = (bool)reader["IsSelect"],
						IsBind = (bool)reader["IsBind"],
						RewardItemValid = (int)reader["RewardItemValid"],
						RewardItemCount = (int)reader["RewardItemCount"],
						StrengthenLevel = (int)reader["StrengthenLevel"],
						AttackCompose = (int)reader["AttackCompose"],
						DefendCompose = (int)reader["DefendCompose"],
						AgilityCompose = (int)reader["AgilityCompose"],
						LuckCompose = (int)reader["LuckCompose"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					ILog log = this.log;
					string str = "Accumul_Ative_Login_Award：";
					Exception ex = e;
					log.Error(str + ((ex != null) ? ex.ToString() : null));
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

		// Token: 0x06000233 RID: 563 RVA: 0x000308A8 File Offset: 0x0002EAA8
		public NewTitleInfo[] GetAllNewTitle()
		{
			List<NewTitleInfo> infos = new List<NewTitleInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_New_Title_All");
				while (reader.Read())
				{
					infos.Add(this.InitNewTitleInfo(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("InitNewTitleInfo", e);
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

		// Token: 0x06000234 RID: 564 RVA: 0x00030968 File Offset: 0x0002EB68
		public NewTitleInfo InitNewTitleInfo(SqlDataReader dr)
		{
			return new NewTitleInfo
			{
				ID = (int)dr["ID"],
				Order = (int)dr["Order"],
				Show = (int)dr["Show"],
				Name = (string)dr["Name"],
				Pic = (int)dr["Pic"],
				Att = (int)dr["Att"],
				Def = (int)dr["Def"],
				Agi = (int)dr["Agi"],
				Luck = (int)dr["Luck"],
				Desc = (string)dr["Desc"]
			};
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00030A68 File Offset: 0x0002EC68
		public ConsortiaTaskConditions[] GetAllConsortiaTask()
		{
			List<ConsortiaTaskConditions> consortiaTaskInfoList = new List<ConsortiaTaskConditions>();
			SqlDataReader ResultDataReader = null;
			try
			{
				this.db.GetReader(ref ResultDataReader, "SP_Consortia_Task_All");
				while (ResultDataReader.Read())
				{
					consortiaTaskInfoList.Add(new ConsortiaTaskConditions
					{
						ID = (int)ResultDataReader["ID"],
						Content = (string)ResultDataReader["Content"],
						Type = (int)ResultDataReader["Type"],
						Target = (int)ResultDataReader["Target"],
						TargetCount = (int)ResultDataReader["TargetCount"],
						Para1 = (int)ResultDataReader["Para1"],
						Para2 = (int)ResultDataReader["Para2"],
						Level = (int)ResultDataReader["Level"]
					});
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllConsortiaTask", ex);
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
			return consortiaTaskInfoList.ToArray();
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00030BFC File Offset: 0x0002EDFC
		public CategoryInfo[] GetSingleCategoryName(int ID)
		{
			List<CategoryInfo> infos = new List<CategoryInfo>();
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", SqlDbType.Int)
				};
				para[0].Value = ID;
				this.db.GetReader(ref reader, "SP_Category_Name_Single", para);
				while (reader.Read())
				{
					CategoryInfo item = new CategoryInfo
					{
						ID = (int)reader["ID"],
						Name = ((reader["Name"] == null) ? "" : reader["Name"].ToString()),
						Place = (int)reader["Place"],
						Remark = ((reader["Remark"] == null) ? "" : reader["Remark"].ToString())
					};
					infos.Add(item);
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("Init", e);
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

		// Token: 0x06000237 RID: 567 RVA: 0x00030D88 File Offset: 0x0002EF88
		public ActivitySystemItemInfo[] GetAllActivitySystemItem()
		{
			List<ActivitySystemItemInfo> infos = new List<ActivitySystemItemInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_ActivitySystemItem_All");
				while (reader.Read())
				{
					infos.Add(new ActivitySystemItemInfo
					{
						ID = (int)reader["ID"],
						ActivityType = (int)reader["ActivityType"],
						Quality = (int)reader["Quality"],
						TemplateID = (int)reader["TemplateID"],
						Count = (int)reader["Count"],
						ValidDate = (int)reader["ValidDate"],
						IsBind = (bool)reader["IsBind"],
						StrengthLevel = (int)reader["StrengthLevel"],
						AttackCompose = (int)reader["AttackCompose"],
						DefendCompose = (int)reader["DefendCompose"],
						AgilityCompose = (int)reader["AgilityCompose"],
						LuckCompose = (int)reader["LuckCompose"],
						Probability = (int)reader["Probability"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllActivitySystemItem", e);
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

		// Token: 0x06000238 RID: 568 RVA: 0x00030F8C File Offset: 0x0002F18C
		public FightSpiritTemplateInfo[] GetAllFightSpiritTemplate()
		{
			List<FightSpiritTemplateInfo> infos = new List<FightSpiritTemplateInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_FightSpiritTemplate_All");
				while (reader.Read())
				{
					infos.Add(new FightSpiritTemplateInfo
					{
						ID = (int)reader["ID"],
						FightSpiritID = (int)reader["FightSpiritID"],
						FightSpiritIcon = (string)reader["FightSpiritIcon"],
						Level = (int)reader["Level"],
						Exp = (int)reader["Exp"],
						Attack = (int)reader["Attack"],
						Defence = (int)reader["Defence"],
						Agility = (int)reader["Agility"],
						Lucky = (int)reader["Lucky"],
						Blood = (int)reader["Blood"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetFightSpiritTemplateAll", e);
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

		// Token: 0x06000239 RID: 569 RVA: 0x00031144 File Offset: 0x0002F344
		public ClothGroupTemplateInfo[] GetAllClothGroup()
		{
			List<ClothGroupTemplateInfo> list = new List<ClothGroupTemplateInfo>();
			SqlDataReader sqlDataReader = null;
			try
			{
				this.db.GetReader(ref sqlDataReader, "SP_ClothGroup_All");
				while (sqlDataReader.Read())
				{
					ClothGroupTemplateInfo item = new ClothGroupTemplateInfo
					{
						ItemID = (int)sqlDataReader["ItemID"],
						ID = (int)sqlDataReader["ID"],
						TemplateID = (int)sqlDataReader["TemplateID"],
						Sex = (int)sqlDataReader["Sex"],
						Description = (int)sqlDataReader["Description"],
						Cost = (int)sqlDataReader["Cost"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("SP_ClothGroup_All", exception);
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

		// Token: 0x0600023A RID: 570 RVA: 0x00031294 File Offset: 0x0002F494
		public ClothPropertyTemplateInfo[] GetAllClothProperty()
		{
			List<ClothPropertyTemplateInfo> list = new List<ClothPropertyTemplateInfo>();
			SqlDataReader sqlDataReader = null;
			try
			{
				this.db.GetReader(ref sqlDataReader, "SP_ClothProperty_All");
				while (sqlDataReader.Read())
				{
					ClothPropertyTemplateInfo item = new ClothPropertyTemplateInfo
					{
						ID = (int)sqlDataReader["ID"],
						Sex = (int)sqlDataReader["Sex"],
						Name = (string)sqlDataReader["Name"],
						Attack = (int)sqlDataReader["Attack"],
						Defend = (int)sqlDataReader["Defend"],
						Luck = (int)sqlDataReader["Luck"],
						Agility = (int)sqlDataReader["Agility"],
						Blood = (int)sqlDataReader["Blood"],
						Damage = (int)sqlDataReader["Damage"],
						Guard = (int)sqlDataReader["Guard"],
						Cost = (int)sqlDataReader["Cost"]
					};
					list.Add(item);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllClothProperty", exception);
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

		// Token: 0x0600023B RID: 571 RVA: 0x00031470 File Offset: 0x0002F670
		public TotemInfo[] GetAllTotem()
		{
			List<TotemInfo> infos = new List<TotemInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Totem_All");
				while (reader.Read())
				{
					infos.Add(new TotemInfo
					{
						ID = (int)reader["ID"],
						ConsumeExp = (int)reader["ConsumeExp"],
						ConsumeHonor = (int)reader["ConsumeHonor"],
						AddAttack = (int)reader["AddAttack"],
						AddDefence = (int)reader["AddDefence"],
						AddAgility = (int)reader["AddAgility"],
						AddLuck = (int)reader["AddLuck"],
						AddBlood = (int)reader["AddBlood"],
						AddDamage = (int)reader["AddDamage"],
						AddGuard = (int)reader["AddGuard"],
						Random = (int)reader["Random"],
						Page = (int)reader["Page"],
						Layers = (int)reader["Layers"],
						Location = (int)reader["Location"],
						Point = (int)reader["Point"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetTotemAll", e);
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

		// Token: 0x0600023C RID: 572 RVA: 0x00031694 File Offset: 0x0002F894
		public TotemHonorTemplateInfo[] GetAllTotemHonorTemplate()
		{
			List<TotemHonorTemplateInfo> infos = new List<TotemHonorTemplateInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_TotemHonorTemplate_All");
				while (reader.Read())
				{
					infos.Add(new TotemHonorTemplateInfo
					{
						ID = (int)reader["ID"],
						NeedMoney = (int)reader["NeedMoney"],
						Type = (int)reader["Type"],
						AddHonor = (int)reader["AddHonor"]
					});
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetTotemHonorTemplateInfo", e);
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

		// Token: 0x0600023D RID: 573 RVA: 0x000317A8 File Offset: 0x0002F9A8
		public DailyLeagueAwardInfo[] GetAllDailyLeagueAward()
		{
			List<DailyLeagueAwardInfo> infos = new List<DailyLeagueAwardInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Daily_League_Award_All");
				while (reader.Read())
				{
					infos.Add(this.InitDailyLeagueAwardInfo(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("InitDailyLeagueAwardInfo", e);
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

		// Token: 0x0600023E RID: 574 RVA: 0x00031864 File Offset: 0x0002FA64
		public DailyLeagueAwardInfo InitDailyLeagueAwardInfo(SqlDataReader dr)
		{
			return new DailyLeagueAwardInfo
			{
				Level = (int)dr["Level"],
				Class = (int)dr["Class"],
				Count = (int)dr["Count"],
				TemplateID = (int)dr["TemplateID"],
				RewardID = (int)dr["RewardID"],
				StrengthenLevel = (int)dr["StrengthenLevel"],
				ItemValid = (int)dr["ItemValid"],
				IsBind = (bool)dr["IsBind"],
				AgilityCompose = (int)dr["AgilityCompose"],
				AttackCompose = (int)dr["AttackCompose"],
				DefendCompose = (int)dr["DefendCompose"],
				LuckCompose = (int)dr["LuckCompose"],
				Hole1 = (int)dr["Hole1"],
				Hole2 = (int)dr["Hole2"],
				Hole3 = (int)dr["Hole3"],
				Hole4 = (int)dr["Hole4"],
				Hole5 = (int)dr["Hole5"],
				Hole5Exp = (int)dr["Hole5Exp"],
				Hole5Level = (int)dr["Hole5Level"],
				Hole6 = (int)dr["Hole6"],
				Hole6Exp = (int)dr["Hole6Exp"],
				Hole6Level = (int)dr["Hole6Level"]
			};
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00031A78 File Offset: 0x0002FC78
		public SetsBuildTempInfo InitSetsBuildTemp(SqlDataReader reader)
		{
			return new SetsBuildTempInfo
			{
				Level = (int)reader["Level"],
				SetsType = (int)reader["SetsType"],
				UseItemTemplate = (int)reader["UseItemTemplate"],
				Exp = (int)reader["Exp"],
				DefenceGrow = (int)reader["DefenceGrow"],
				BloodGrow = (int)reader["BloodGrow"],
				LuckGrow = (int)reader["LuckGrow"],
				AgilityGrow = (int)reader["AgilityGrow"],
				DamageGrow = (int)reader["DamageGrow"],
				GuardGrow = (int)reader["GuardGrow"]
			};
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00031B78 File Offset: 0x0002FD78
		public SetsBuildTempInfo[] GetAllSetsBuildTemp()
		{
			List<SetsBuildTempInfo> infos = new List<SetsBuildTempInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Sets_Build_Temp_All");
				while (reader.Read())
				{
					infos.Add(this.InitSetsBuildTemp(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllSetsBuildTemp", e);
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

		// Token: 0x06000241 RID: 577 RVA: 0x00031C34 File Offset: 0x0002FE34
		public OldPlayerAwardInfo InitOldPlayerAward(SqlDataReader reader)
		{
			return new OldPlayerAwardInfo
			{
				RewardItemID = (int)reader["RewardItemID"],
				RewardItemValid = (int)reader["RewardItemValid"],
				RewardItemCount = (int)reader["RewardItemCount"],
				StrengthenLevel = (int)reader["StrengthenLevel"],
				AttackCompose = (int)reader["AttackCompose"],
				DefendCompose = (int)reader["DefendCompose"],
				AgilityCompose = (int)reader["AgilityCompose"],
				LuckCompose = (int)reader["LuckCompose"],
				IsBind = (bool)reader["IsBind"]
			};
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00031D1C File Offset: 0x0002FF1C
		public OldPlayerAwardInfo[] GetAllOldPlayerAward()
		{
			List<OldPlayerAwardInfo> infos = new List<OldPlayerAwardInfo>();
			SqlDataReader reader = null;
			try
			{
				this.db.GetReader(ref reader, "SP_Get_OldPlayerAward");
				while (reader.Read())
				{
					infos.Add(this.InitOldPlayerAward(reader));
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("GetAllOldPlayerAward", e);
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

		// Token: 0x06000243 RID: 579 RVA: 0x00031DD8 File Offset: 0x0002FFD8
		public bool UpdateShop(ShopItemInfo info)
		{
			bool result = false;
			try
			{
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@ID", info.ID),
					new SqlParameter("@LimitCount", info.LimitCount),
					new SqlParameter("@Result", SqlDbType.Int)
				};
				para[2].Direction = ParameterDirection.ReturnValue;
				this.db.RunProcedure("SP_UpdateShop", para);
				result = true;
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = this.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					this.log.Error("UpdateShop", ex);
				}
			}
			return result;
		}
	}
}

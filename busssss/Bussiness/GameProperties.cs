using System;
using System.Collections.Generic;
using System.Reflection;
using Game.Base.Config;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness
{
	// Token: 0x0200000F RID: 15
	public abstract class GameProperties
	{
		// Token: 0x0600007C RID: 124 RVA: 0x0000BE40 File Offset: 0x0000A040
		private static void Load(Type type)
		{
			using (ServiceBussiness sb = new ServiceBussiness())
			{
				foreach (FieldInfo f in type.GetFields())
				{
					bool isStatic = f.IsStatic;
					if (isStatic)
					{
						object[] attribs = f.GetCustomAttributes(typeof(ConfigPropertyAttribute), false);
						bool flag = attribs.Length != 0;
						if (flag)
						{
							ConfigPropertyAttribute attrib = (ConfigPropertyAttribute)attribs[0];
							f.SetValue(null, GameProperties.LoadProperty(attrib, sb));
						}
					}
				}
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000BEE0 File Offset: 0x0000A0E0
		private static void Save(Type type)
		{
			using (ServiceBussiness sb = new ServiceBussiness())
			{
				foreach (FieldInfo f in type.GetFields())
				{
					bool flag = !f.IsStatic;
					if (!flag)
					{
						object[] attribs = f.GetCustomAttributes(typeof(ConfigPropertyAttribute), false);
						bool flag2 = attribs.Length == 0;
						if (!flag2)
						{
							ConfigPropertyAttribute attrib = (ConfigPropertyAttribute)attribs[0];
							GameProperties.SaveProperty(attrib, sb, f.GetValue(null));
						}
					}
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000BF7C File Offset: 0x0000A17C
		private static object LoadProperty(ConfigPropertyAttribute attrib, ServiceBussiness sb)
		{
			string key = attrib.Key;
			ServerProperty property = sb.GetServerPropertyByKey(key);
			bool flag = property == null;
			if (flag)
			{
				property = new ServerProperty();
				property.Value = attrib.DefaultValue.ToString();
				GameProperties.log.Info("Cannot find server property " + key + ",keep it default value!");
				GameProperties.SaveProperty(attrib, sb, property.Value);
			}
			object result;
			try
			{
				result = Convert.ChangeType(property.Value, attrib.DefaultValue.GetType());
			}
			catch (Exception e)
			{
				GameProperties.log.Error("Exception in GameProperties Load: ", e);
				result = null;
			}
			return result;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000C02C File Offset: 0x0000A22C
		private static void SaveProperty(ConfigPropertyAttribute attrib, ServiceBussiness sb, object value)
		{
			try
			{
				sb.UpdateServerPropertyByKey(attrib.Key, value.ToString());
			}
			catch (Exception ex)
			{
				GameProperties.log.Error("Exception in GameProperties Save: ", ex);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000C078 File Offset: 0x0000A278
		public static void Refresh()
		{
			GameProperties.log.Info("Refreshing game properties!");
			GameProperties.Load(typeof(GameProperties));
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000C09C File Offset: 0x0000A29C
		public static List<int> getProp(string prop)
		{
			List<int> listInt = new List<int>();
			string[] strs = prop.Split(new char[]
			{
				'|'
			});
			foreach (string str in strs)
			{
				listInt.Add(Convert.ToInt32(str));
			}
			return listInt;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000C0F0 File Offset: 0x0000A2F0
		public static int LimitLevel(int index)
		{
			string[] strs = GameProperties.CustomLimit.Split(new char[]
			{
				'|'
			});
			return Convert.ToInt32(strs[index]);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000C120 File Offset: 0x0000A320
		public static List<int> VIPExp()
		{
			return GameProperties.getProp(GameProperties.VIPExpForEachLv);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000C13C File Offset: 0x0000A33C
		public static int[] ConvertStringArrayToIntArray(string str)
		{
			List<int> listInts = new List<int>();
			string[] strs = new string[]
			{
				"99999",
				"999999",
				"9999999"
			};
			if (!(str == "NewChickenEagleEyePrice"))
			{
				if (str == "NewChickenOpenCardPrice")
				{
					strs = GameProperties.NewChickenOpenCardPrice.Split(new char[]
					{
						','
					});
				}
			}
			else
			{
				strs = GameProperties.NewChickenEagleEyePrice.Split(new char[]
				{
					','
				});
			}
			foreach (string value in strs)
			{
				listInts.Add(Convert.ToInt32(value));
			}
			return listInts.ToArray();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000C1F9 File Offset: 0x0000A3F9
		public static void Save()
		{
			GameProperties.log.Info("Saving game properties into db!");
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000C20C File Offset: 0x0000A40C
		public static Dictionary<int, int> AcademyApprenticeAwardArr()
		{
			return GameProperties.ConvertIntDict(GameProperties.AcademyApprenticeAward, ',', '|');
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000C22C File Offset: 0x0000A42C
		public static Dictionary<int, int> AcademyMasterAwardArr()
		{
			return GameProperties.ConvertIntDict(GameProperties.AcademyMasterAward, ',', '|');
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000C24C File Offset: 0x0000A44C
		public static Dictionary<int, int> ConvertIntDict(string value, char splitChar, char subSplitChar)
		{
			string[] strArray = value.Split(new char[]
			{
				splitChar
			});
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int index = 0; index < strArray.Length; index++)
			{
				string[] strArray2 = strArray[index].Split(new char[]
				{
					subSplitChar
				});
				bool flag = !dictionary.ContainsKey(int.Parse(strArray2[0]));
				if (flag)
				{
					dictionary.Add(int.Parse(strArray2[0]), int.Parse(strArray2[1]));
				}
			}
			return dictionary;
		}

		// Token: 0x04000076 RID: 118
		[ConfigProperty("BeginAuction", "ÅÄÂòÊ±ÆðÊ¼Ëæ»úÊ±¼ä", 20)]
		public static int BeginAuction;

		// Token: 0x04000077 RID: 119
		[ConfigProperty("BigExp", "µ±Ç°ÓÎÏ·°æ±¾", "11906|99")]
		public static readonly string BigExp;

		// Token: 0x04000078 RID: 120
		[ConfigProperty("BoxAppearCondition", "Ïä×ÓÎïÆ·ÌáÊ¾µÄµÈ¼¶", 4)]
		public static readonly int BOX_APPEAR_CONDITION;

		// Token: 0x04000079 RID: 121
		[ConfigProperty("Cess", "½»Ò×¿ÛË°", 0.1)]
		public static readonly double Cess;

		// Token: 0x0400007A RID: 122
		[ConfigProperty("CustomLimit", "sendattackmail|addaution|PresentGoods|PresentMoney|unknow", "20|20|20|20|20")]
		public static readonly string CustomLimit;

		// Token: 0x0400007B RID: 123
		[ConfigProperty("CheckCount", "×î´óÑéÖ¤ÂëÊ§°Ü´ÎÊý", 2)]
		public static readonly int CHECK_MAX_FAILED_COUNT;

		// Token: 0x0400007C RID: 124
		[ConfigProperty("WarriorFamRaidPriceBig", "WarriorFamRaidPriceBig ", 40000)]
		public static readonly int WarriorFamRaidPriceBig;

		// Token: 0x0400007D RID: 125
		[ConfigProperty("WarriorFamRaidDDTPrice", "WarriorFamRaidDDTPrice", 5000)]
		public static readonly int WarriorFamRaidDDTPrice;

		// Token: 0x0400007E RID: 126
		[ConfigProperty("WarriorFamRaidTimeRemain", "WarriorFamRaidTimeRemain", 120)]
		public static readonly int WarriorFamRaidTimeRemain;

		// Token: 0x0400007F RID: 127
		[ConfigProperty("WarriorFamRaidPriceSmall", "WarriorFamRaidPriceSmall", 30000)]
		public static readonly int WarriorFamRaidPriceSmall;

		// Token: 0x04000080 RID: 128
		[ConfigProperty("WarriorFamRaidPricePerMin", "WarriorFamRaidPricePerMin", 10)]
		public static readonly int WarriorFamRaidPricePerMin;

		// Token: 0x04000081 RID: 129
		[ConfigProperty("Edition", "µ±Ç°ÓÎÏ·°æ±¾", "2612558")]
		public static readonly string EDITION;

		// Token: 0x04000082 RID: 130
		[ConfigProperty("EndAuction", "ÅÄÂòÊ±½áÊøËæ»úÊ±¼ä", 40)]
		public static int EndAuction;

		// Token: 0x04000083 RID: 131
		[ConfigProperty("FreeExp", "µ±Ç°ÓÎÏ·°æ±¾", "11901|1")]
		public static readonly string FreeExp;

		// Token: 0x04000084 RID: 132
		[ConfigProperty("FreeMoney", "µ±Ç°ÓÎÏ·°æ±¾", 9990000)]
		public static readonly int FreeMoney;

		// Token: 0x04000085 RID: 133
		[ConfigProperty("HoleLevelUpExpList", "HoleLevelUpExpList", "400|600|700|800|800")]
		public static string HoleLevelUpExpList;

		// Token: 0x04000086 RID: 134
		[ConfigProperty("HotSpringExp", "Kinh nghiệm Spa", "1|2")]
		public static readonly string HotSpringExp;

		// Token: 0x04000087 RID: 135
		[ConfigProperty("IsLimitCount", "IsLimitCount", false)]
		public static readonly bool IsLimitCount;

		// Token: 0x04000088 RID: 136
		[ConfigProperty("IsLimitMail", "IsLimitMail", false)]
		public static readonly bool IsLimitMail;

		// Token: 0x04000089 RID: 137
		[ConfigProperty("IsLimitMoney", "IsLimitMoney", false)]
		public static readonly bool IsLimitMoney;

		// Token: 0x0400008A RID: 138
		[ConfigProperty("WishBeadLimitLv", "WishBeadLimitLv", 12)]
		public static readonly int WishBeadLimitLv;

		// Token: 0x0400008B RID: 139
		[ConfigProperty("IsWishBeadLimit", "IsWishBeadLimit", false)]
		public static readonly bool IsWishBeadLimit;

		// Token: 0x0400008C RID: 140
		[ConfigProperty("LimitCount", "LimitCount", 10)]
		public static readonly int LimitCount;

		// Token: 0x0400008D RID: 141
		[ConfigProperty("LimitMail", "LimitMail", 3)]
		public static readonly int LimitMail;

		// Token: 0x0400008E RID: 142
		[ConfigProperty("LimitMoney", "LimitMoney", 999000)]
		public static readonly int LimitMoney;

		// Token: 0x0400008F RID: 143
		public static readonly int SearchGoodsPayMoney;

		// Token: 0x04000090 RID: 144
		[ConfigProperty("SearchGoodsFreeCount", "SearchGoodsFreeCount", 3)]
		public static readonly int SearchGoodsFreeCount;

		// Token: 0x04000091 RID: 145
		[ConfigProperty("IsDDTMoneyActive", "IsDDTMoneyActive", false)]
		public static readonly bool IsDDTMoneyActive;

		// Token: 0x04000092 RID: 146
		[ConfigProperty("DiceGameAwardAndCount", "DiceGameAwardAndCount", "32|16|8|4|2|1")]
		public static readonly string DiceGameAwardAndCount;

		// Token: 0x04000093 RID: 147
		[ConfigProperty("DiceBeginTime", "DiceBeginTime", "2013/12/17 0:00:00")]
		public static readonly string DiceBeginTime;

		// Token: 0x04000094 RID: 148
		[ConfigProperty("DiceEndTime", "DiceEndTime", "2013/12/25 0:00:00")]
		public static readonly string DiceEndTime;

		// Token: 0x04000095 RID: 149
		[ConfigProperty("DiceRefreshPrice", "DiceRefreshPrice", 40000)]
		public static readonly int DiceRefreshPrice;

		// Token: 0x04000096 RID: 150
		[ConfigProperty("CommonDicePrice", "CommonDicePrice", 30000)]
		public static readonly int CommonDicePrice;

		// Token: 0x04000097 RID: 151
		[ConfigProperty("DoubleDicePrice", "DoubleDicePrice", 40000)]
		public static readonly int DoubleDicePrice;

		// Token: 0x04000098 RID: 152
		[ConfigProperty("BigDicePrice", "BigDicePrice", 50000)]
		public static readonly int BigDicePrice;

		// Token: 0x04000099 RID: 153
		[ConfigProperty("SmallDicePrice", "SmallDicePrice", 60000)]
		public static readonly int SmallDicePrice;

		// Token: 0x0400009A RID: 154
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400009B RID: 155
		[ConfigProperty("NewChickenBeginTime", "NewChickenBeginTime", "2013/12/17 0:00:00")]
		public static readonly string NewChickenBeginTime;

		// Token: 0x0400009C RID: 156
		[ConfigProperty("NewChickenEagleEyePrice", "NewChickenEagleEyePrice", "3000, 2000, 1000")]
		public static readonly string NewChickenEagleEyePrice;

		// Token: 0x0400009D RID: 157
		[ConfigProperty("NewChickenEndTime", "NewChickenEndTime", "2013/12/25 0:00:00")]
		public static readonly string NewChickenEndTime;

		// Token: 0x0400009E RID: 158
		[ConfigProperty("NewChickenFlushPrice", "NewChickenFlushPrice", 10000)]
		public static readonly int NewChickenFlushPrice;

		// Token: 0x0400009F RID: 159
		[ConfigProperty("NewChickenOpenCardPrice", "NewChickenOpenCardPrice", "2500, 2000, 1500, 1000, 500")]
		public static readonly string NewChickenOpenCardPrice;

		// Token: 0x040000A0 RID: 160
		[ConfigProperty("PetExp", "µ±Ç°ÓÎÏ·°æ±¾", "334103|999")]
		public static readonly string PetExp;

		// Token: 0x040000A1 RID: 161
		[ConfigProperty("DivorcedMoney", "Àë»éµÄ¼Û¸ñ", 1499)]
		public static readonly int PRICE_DIVORCED;

		// Token: 0x040000A2 RID: 162
		[ConfigProperty("DivorcedDiscountMoney", "Àë»éµÄ¼Û¸ñ", 999)]
		public static readonly int PRICE_DIVORCED_DISCOUNT;

		// Token: 0x040000A3 RID: 163
		[ConfigProperty("MarryRoomCreateMoney", "½á»é·¿¼äµÄ¼Û¸ñ,2Ð¡Ê±¡¢3Ð¡Ê±¡¢4Ð¡Ê±ÓÃ¶ººÅ·Ö¸ô", "2000,2700,3400")]
		public static readonly string PRICE_MARRY_ROOM;

		// Token: 0x040000A4 RID: 164
		[ConfigProperty("HymenealMoney", "Çó»éµÄ¼Û¸ñ", 300)]
		public static readonly int PRICE_PROPOSE;

		// Token: 0x040000A5 RID: 165
		public static int SpaAddictionMoneyNeeded = 1299;

		// Token: 0x040000A6 RID: 166
		[ConfigProperty("SpaPriRoomContinueTime", "ÅÄÂòÊ±½áÊøËæ»úÊ±¼ä", 30)]
		public static int SpaPriRoomContinueTime;

		// Token: 0x040000A7 RID: 167
		[ConfigProperty("SpaPubRoomLoginPay", "ÅÄÂòÊ±½áÊøËæ»úÊ±¼ä", "10000,200")]
		public static string SpaPubRoomLoginPay;

		// Token: 0x040000A8 RID: 168
		[ConfigProperty("TestActive", "TestActive", false)]
		public static readonly bool TestActive;

		// Token: 0x040000A9 RID: 169
		[ConfigProperty("VIPExpForEachLv", "VIPExpForEachLv", "1|2")]
		public static readonly string VIPExpForEachLv;

		// Token: 0x040000AA RID: 170
		[ConfigProperty("VirtualName", "VirtualName", "Doreamon,Nobita,Xuneo,Xuka")]
		public static readonly string VirtualName;

		// Token: 0x040000AB RID: 171
		[ConfigProperty("VirtualName", "VirtualName", "Doreamon,Nobita,Xuneo,Xuka")]
		public static readonly string GVirtualName;

		// Token: 0x040000AC RID: 172
		[ConfigProperty("TimeForLeague", "TimeForLeague", "19:30|21:30")]
		public static readonly string TimeForLeague;

		// Token: 0x040000AD RID: 173
		[ConfigProperty("GoldTimes", "GoldTimes", "20:00|21:00")]
		public static readonly string GoldTimes;

		// Token: 0x040000AE RID: 174
		[ConfigProperty("AcademyMasterFreezeHours", "AcademyMasterFreezeHours", 48)]
		public static int AcademyMasterFreezeHours;

		// Token: 0x040000AF RID: 175
		[ConfigProperty("AcademyApprenticeFreezeHours", "AcademyApprenticeFreezeHours", 24)]
		public static int AcademyApprenticeFreezeHours;

		// Token: 0x040000B0 RID: 176
		[ConfigProperty("AcademyApprenticeAward", "AcademyApprenticeAward", "10|112085,15|112086,18|112087,20|112125")]
		public static string AcademyApprenticeAward;

		// Token: 0x040000B1 RID: 177
		[ConfigProperty("AcademyMasterAward", "AcademyMasterAward", "10|112088,15|112089,18|112090,20|112124")]
		public static string AcademyMasterAward;

		// Token: 0x040000B2 RID: 178
		[ConfigProperty("AcademyAppAwardComplete", "AcademyAppAwardComplete", "1401|5293,1301|5192")]
		public static string AcademyAppAwardComplete;

		// Token: 0x040000B3 RID: 179
		[ConfigProperty("AcademyMasAwardComplete", "AcademyMasAwardComplete", "1414|5409,1314|5306")]
		public static string AcademyMasAwardComplete;

		// Token: 0x040000B4 RID: 180
		[ConfigProperty("LeftRouterRateData", "LeftRouterRateData", "0.0003|0.0002|0.0001|0.001|0.002|")]
		public static string LeftRouterRateData;

		// Token: 0x040000B5 RID: 181
		[ConfigProperty("LeftRouterMaxDay", "LeftRouterMaxDay", 5)]
		public static int LeftRouterMaxDay;

		// Token: 0x040000B6 RID: 182
		[ConfigProperty("LeftRouterEndDate", "LeftRouterEndDate", "2012-01-01 20:55:27.270")]
		public static string LeftRouterEndDate;

		// Token: 0x040000B7 RID: 183
		[ConfigProperty("EliteGameBlockWeapon", "ÅÄÂòÊ±½áÊøËæ»úÊ±¼ä", "7144|71441|71442|71443|71444|7145|71451|71452|71453|71454")]
		public static string EliteGameBlockWeapon;

		// Token: 0x040000B8 RID: 184
		[ConfigProperty("InlayGoldPrice", "InlayGoldPrice", 2000)]
		public static int InlayGoldPrice;

		// Token: 0x040000B9 RID: 185
		[ConfigProperty("IsOpenPetScore", "IsOpenPetScore", true)]
		public static readonly bool IsOpenPetScore;

		// Token: 0x040000BA RID: 186
		[ConfigProperty("FastGrowNeedMoney", "FastGrowNeedMoney", 30)]
		public static readonly int FastGrowNeedMoney;

		// Token: 0x040000BB RID: 187
		[ConfigProperty("FastGrowSubTime", "FastGrowSubTime", 30)]
		public static readonly int FastGrowSubTime;

		// Token: 0x040000BC RID: 188
		[ConfigProperty("LittleGameBoguConfig", "LittleGameBoguConfig", "200,1,1|100,4,1|5000,80,3|10000,125,5")]
		public static string LittleGameBoguConfig;

		// Token: 0x040000BD RID: 189
		[ConfigProperty("LittleGameMaxBoguCount", "LittleGameMaxBoguCount", 20)]
		public static int LittleGameMaxBoguCount;

		// Token: 0x040000BE RID: 190
		[ConfigProperty("LittleGameStartHourse", "LittleGameStartHourse", 7)]
		public static int LittleGameStartHourse;

		// Token: 0x040000BF RID: 191
		[ConfigProperty("LittleGameTimeSpendingHours", "LittleGameTimeSpendingHours", 1)]
		public static int LittleGameTimeSpending;

		// Token: 0x040000C0 RID: 192
		[ConfigProperty("DebugMode", "DebugMode", false)]
		public static bool DebugMode;

		// Token: 0x040000C1 RID: 193
		[ConfigProperty("VIPStrengthenEx", "VIPStrengthenEx", "25|25|25|35|35|50|50|50|50|50|50|50")]
		public static readonly string VIPStrengthenEx;

		// Token: 0x040000C2 RID: 194
		[ConfigProperty("MissionRiches", "MissionRiches", "3000|3000|5000|5000|8000|8000|10000|10000|12000|12000")]
		public static readonly string MissionRiches;

		// Token: 0x040000C3 RID: 195
		[ConfigProperty("EventStartDate", "EventStartDate", "2021-01-01 20:55:27.270")]
		public static string EventStartDate;

		// Token: 0x040000C4 RID: 196
		[ConfigProperty("EventEndDate", "EventEndDate", "2025-01-01 20:55:27.270")]
		public static string EventEndDate;

		// Token: 0x040000C5 RID: 197
		[ConfigProperty("EventStartMoney", "EventStartMoney", "2021-01-01 20:55:27.270")]
		public static string EventStartMoney;

		// Token: 0x040000C6 RID: 198
		[ConfigProperty("EventEndMoney", "EventEndMoney", "2022-01-01 20:55:27.270")]
		public static string EventEndMoney;

		// Token: 0x040000C7 RID: 199
		[ConfigProperty("WorldBossStart", "WorldBossStart", "01:00:00")]
		public static string WorldBossStart;

		// Token: 0x040000C8 RID: 200
		[ConfigProperty("WorldBossEnd", "WorldBossEnd", "23:59:00")]
		public static string WorldBossEnd;

		// Token: 0x040000C9 RID: 201
		[ConfigProperty("WorldBossID1", "WorldBossID1", 4)]
		public static int WorldBossID1;

		// Token: 0x040000CA RID: 202
		[ConfigProperty("WorldBossID2", "WorldBossID2", 1)]
		public static int WorldBossID2;

		// Token: 0x040000CB RID: 203
		[ConfigProperty("GoldTimeStart", "GoldTimeStart", "01:00:00")]
		public static string GoldTimeStart;

		// Token: 0x040000CC RID: 204
		[ConfigProperty("GoldTimeEnd", "GoldTimeEnd", "23:59:00")]
		public static string GoldTimeEnd;

		// Token: 0x040000CD RID: 205
		[ConfigProperty("CountHWIDLimit", "CountHWIDLimit", 99)]
		public static int CountHWIDLimit;

		// Token: 0x040000CE RID: 206
		[ConfigProperty("CountIPLimit", "CountIPLimit", 99)]
		public static int CountIPLimit;

		// Token: 0x040000CF RID: 207
		[ConfigProperty("LuckStarActivityBeginDate", "LuckStarActivityBeginDate", "2013/12/1 0:00:00")]
		public static readonly string LuckStarActivityBeginDate;

		// Token: 0x040000D0 RID: 208
		[ConfigProperty("LuckStarActivityEndDate", "LuckStarActivityEndDate", "2014/12/24 0:00:00")]
		public static readonly string LuckStarActivityEndDate;

		// Token: 0x040000D1 RID: 209
		[ConfigProperty("MinUseNum", "MinUseNum", 1000)]
		public static readonly int MinUseNum;

		// Token: 0x040000D2 RID: 210
		[ConfigProperty("IsActiveMoney", "IsActiveMoney", true)]
		public static readonly bool IsActiveMoney;

		// Token: 0x040000D3 RID: 211
		[ConfigProperty("FightSpiritLevelAddDamage", "FightSpiritLevelAddDamage", "6|2")]
		public static readonly string FightSpiritLevelAddDamage;

		// Token: 0x040000D4 RID: 212
		[ConfigProperty("FightSpiritMaxLevel", "FightSpiritMaxLevel", 5)]
		public static readonly int FightSpiritMaxLevel;

		// Token: 0x040000D5 RID: 213
		[ConfigProperty("PRICE_COMPOSE_GOLD", "PRICE_COMPOSE_GOLD", 1600)]
		public static readonly int PRICE_COMPOSE_GOLD;

		// Token: 0x040000D6 RID: 214
		[ConfigProperty("RateAdvance", "RateAdvance", 50000)]
		public static readonly int RateAdvance;

		// Token: 0x040000D7 RID: 215
		[ConfigProperty("TimeX2", "TimeX2", 2)]
		public static readonly int TimeX2;

		// Token: 0x040000D8 RID: 216
		[ConfigProperty("ItemDevelopPrice", "ItemDevelopPrice", 35)]
		public static readonly int ItemDevelopPrice;

		// Token: 0x040000D9 RID: 217
		[ConfigProperty("GuildBattleStartTime", "GuildBattleStartTime", "2016/1/1 20:00:00")]
		public static readonly string GuildBattleStartTime;

		// Token: 0x040000DA RID: 218
		[ConfigProperty("GuildBattleStartDay", "GuildBattleStartDay", "Saturday")]
		public static readonly string GuildBattleStartDay;

		// Token: 0x040000DB RID: 219
		[ConfigProperty("StartEventOldPlayer", "StartEventOldPlayer", "2021-01-01 20:55:27.270")]
		public static string StartEventOldPlayer;

		// Token: 0x040000DC RID: 220
		[ConfigProperty("EndEventOldPlayer", "EndEventOldPlayer", "2025-01-01 20:55:27.270")]
		public static string EndEventOldPlayer;

		// Token: 0x040000DD RID: 221
		[ConfigProperty("PRICE_STRENGHTN_GOLD", "PRICE_STRENGHTN_GOLD", 10000)]
		public static readonly int PRICE_STRENGHTN_GOLD;

		// Token: 0x040000DE RID: 222
		[ConfigProperty("ChristmasBeginDate", "ChristmasBeginDate", "2013/12/17 0:00:00")]
		public static readonly string ChristmasBeginDate;

		// Token: 0x040000DF RID: 223
		[ConfigProperty("ChristmasEndDate", "ChristmasEndDate", "2013/12/25 0:00:00")]
		public static readonly string ChristmasEndDate;

		// Token: 0x040000E0 RID: 224
		[ConfigProperty("ChristmasGifts", "ChristmasGifts", "201148,10|201149,35|201150,70|201151,120|201152,220|201153,370|201154,650|201155,1000|201156,100")]
		public static readonly string ChristmasGifts;

		// Token: 0x040000E1 RID: 225
		[ConfigProperty("ChristmasGiftsMaxNum", "ChristmasGiftsMaxNum", 1000)]
		public static readonly int ChristmasGiftsMaxNum;

		// Token: 0x040000E2 RID: 226
		[ConfigProperty("ChristmasBuildSnowmanDoubleMoney", "ChristmasBuildSnowmanDoubleMoney", 10)]
		public static readonly int ChristmasBuildSnowmanDoubleMoney;

		// Token: 0x040000E3 RID: 227
		[ConfigProperty("ChristmasBuyTimeMoney", "ChristmasBuyTimeMoney", 150)]
		public static readonly int ChristmasBuyTimeMoney;

		// Token: 0x040000E4 RID: 228
		[ConfigProperty("ChristmasMinute", "ChristmasMinute", 60)]
		public static readonly int ChristmasMinute;

		// Token: 0x040000E5 RID: 229
		[ConfigProperty("ChristmasBuyMinute", "ChristmasBuyMinute", 10)]
		public static readonly int ChristmasBuyMinute;

		// Token: 0x040000E6 RID: 230
		[ConfigProperty("PyramidBeginTime", "PyramidBeginTime", "2013/12/17 0:00:00")]
		public static readonly string PyramidBeginTime;

		// Token: 0x040000E7 RID: 231
		[ConfigProperty("PyramidEndTime", "NewChickenEndTime", "2050/12/25 0:00:00")]
		public static readonly string PyramidEndTime;

		// Token: 0x040000E8 RID: 232
		[ConfigProperty("PyramidRevivePrice", "PyramidRevivePrice", "10000, 30000, 50000")]
		public static readonly string PyramidRevivePrice;

		// Token: 0x040000E9 RID: 233
		[ConfigProperty("PyramydTurnCardPrice", "PyramydTurnCardPrice", 5000)]
		public static readonly int PyramydTurnCardPrice;
	}
}

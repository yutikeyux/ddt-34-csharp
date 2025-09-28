using Bussiness;
using Bussiness.Managers;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace Game.Server.GameUtils
{
	public class PlayerDice
	{
		public int MAX_LEVEL = 5;
		public int refreshPrice = GameProperties.DiceRefreshPrice;
		public int commonDicePrice = GameProperties.CommonDicePrice;
		public int doubleDicePrice = GameProperties.DoubleDicePrice;
		public int bigDicePrice = GameProperties.BigDicePrice;
		public int smallDicePrice = GameProperties.SmallDicePrice;
		public int[] IntegralPoint = new int[]
		{
			100,
			300,
			700,
			1500,
			3100
		};
		protected GamePlayer m_player;
		protected object m_lock = new object();
		private int m_result;
		private List<EventAwardInfo> m_rewardItem;
		private string m_rewardName;
		private Dictionary<int, List<DiceLevelAwardInfo>> m_LevelAward;
		private DiceDataInfo m_diceData;
		private bool m_saveToDb;
		public GamePlayer Player
		{
			get
			{
				return this.m_player;
			}
		}
		public int result
		{
			get
			{
				return this.m_result;
			}
			set
			{
				this.m_result = value;
			}
		}
		public List<EventAwardInfo> RewardItem
		{
			get
			{
				return this.m_rewardItem;
			}
			set
			{
				this.m_rewardItem = value;
			}
		}
		public string RewardName
		{
			get
			{
				return this.m_rewardName;
			}
			set
			{
				this.m_rewardName = value;
			}
		}
		public Dictionary<int, List<DiceLevelAwardInfo>> LevelAward
		{
			get
			{
				return this.m_LevelAward;
			}
			set
			{
				this.m_LevelAward = value;
			}
		}
		public DiceDataInfo Data
		{
			get
			{
				return this.m_diceData;
			}
			set
			{
				this.m_diceData = value;
			}
		}
		public PlayerDice(GamePlayer player, bool saveTodb)
		{
			this.m_player = player;
			this.m_saveToDb = saveTodb;
			this.m_result = 0;
			this.m_rewardName = "";
		}
		public void LoadFromDatabase()
		{
			if (this.IsDiceOpen())
			{
				if (this.m_diceData == null)
				{
					using (PlayerBussiness playerBussiness = new PlayerBussiness())
					{
						this.m_diceData = playerBussiness.GetSingleDiceData(this.Player.PlayerCharacter.ID);
						if (this.m_diceData == null)
						{
							this.SetupDiceData();
						}
					}
				}
				this.ReceiveLevelAward();
			}
		}
		public bool IsDiceOpen()
		{
			Convert.ToDateTime(GameProperties.DiceBeginTime);
			DateTime dateTime = Convert.ToDateTime(GameProperties.DiceEndTime);
			return DateTime.Now.Date < dateTime.Date;
		}
		public void SendDiceActiveOpen()
		{
			if (this.IsDiceOpen())
			{
				this.Player.Out.SendDiceActiveOpen(this);
			}
		}
		public void Reset()
		{
			if (!this.IsDiceOpen())
			{
				return;
			}
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				this.m_diceData.LuckIntegral = 0;
				this.m_diceData.CurrentPosition = -1;
				this.m_diceData.LuckIntegralLevel = -1;
				this.m_diceData.UserFirstCell = false;
				this.m_diceData.FreeCount = 3;
				this.m_diceData.Level = 0;
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		private void SetupDiceData()
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				this.m_diceData = new DiceDataInfo();
				this.m_diceData.UserID = this.Player.PlayerCharacter.ID;
				this.m_diceData.LuckIntegral = 0;
				this.m_diceData.CurrentPosition = -1;
				this.m_diceData.LuckIntegralLevel = -1;
				this.m_diceData.UserFirstCell = false;
				this.m_diceData.FreeCount = 3;
				this.m_diceData.Level = 0;
				this.m_diceData.AwardArray = "";
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public void ReceiveData()
		{
			if (string.IsNullOrEmpty(this.m_diceData.AwardArray))
			{
				this.CreateDiceAward();
				return;
			}
			this.m_rewardItem = new List<EventAwardInfo>();
			string[] array = this.m_diceData.AwardArray.Split(new char[]
			{
				'|'
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				string[] array3 = text.Split(new char[]
				{
					','
				});
				EventAwardInfo eventAwardInfo = new EventAwardInfo();
				eventAwardInfo.TemplateID = int.Parse(array3[0]);
				eventAwardInfo.StrengthenLevel = int.Parse(array3[1]);
				eventAwardInfo.Count = int.Parse(array3[2]);
				eventAwardInfo.ValidDate = int.Parse(array3[3]);
				eventAwardInfo.IsBinds = bool.Parse(array3[4]);
				this.m_rewardItem.Add(eventAwardInfo);
			}
			if (this.m_rewardItem.Count < 19)
			{
				this.CreateDiceAward();
			}
		}
		public void ReceiveLevelAward()
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				this.m_LevelAward = new Dictionary<int, List<DiceLevelAwardInfo>>();
				for (int i = 0; i < this.MAX_LEVEL; i++)
				{
					List<DiceLevelAwardInfo> allDiceLevelAwardAward = DiceLevelAwardMgr.GetAllDiceLevelAwardAward(i + 1);
					if (!this.m_LevelAward.ContainsKey(i))
					{
						this.m_LevelAward.Add(i, allDiceLevelAwardAward);
					}
					else
					{
						this.m_LevelAward[i] = allDiceLevelAwardAward;
					}
				}
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public void GetLevelAward()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IList<DiceLevelAwardInfo> list = this.m_LevelAward[this.m_diceData.LuckIntegralLevel];
			foreach (DiceLevelAwardInfo current in list)
			{
				ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(current.TemplateID);
				if (itemTemplateInfo != null)
				{
					ItemInfo itemInfo = ItemInfo.CreateFromTemplate(itemTemplateInfo, current.Count, 103);
					this.Player.AddTemplate(itemInfo);
					stringBuilder.Append(string.Concat(new object[]
					{
						itemInfo.Template.Name,
						"x",
						itemInfo.Count,
						"; "
					}));
				}
			}
			if (this.m_diceData.LuckIntegralLevel > 3)
			{
				this.Player.SendMessage("Bạn nhận được " + stringBuilder.ToString());
			}
		}
		public void CreateDiceAward()
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				this.m_rewardItem = new List<EventAwardInfo>();
				Dictionary<int, EventAwardInfo> dictionary = new Dictionary<int, EventAwardInfo>();
				int num = 0;
				while (this.m_rewardItem.Count < 19)
				{
					List<EventAwardInfo> diceAward = EventAwardMgr.GetDiceAward(eEventType.DICE);
					if (diceAward.Count > 0)
					{
						EventAwardInfo eventAwardInfo = diceAward[0];
						if (!dictionary.Keys.Contains(eventAwardInfo.TemplateID))
						{
							dictionary.Add(eventAwardInfo.TemplateID, eventAwardInfo);
							this.m_rewardItem.Add(eventAwardInfo);
						}
					}
					num++;
				}
			}
			finally
			{
				Monitor.Exit(@lock);
			}
			this.ConvertAwardArray();
		}
		public void ConvertAwardArray()
		{
			if (this.m_rewardItem.Count > 0)
			{
				string text = "";
				foreach (EventAwardInfo current in this.m_rewardItem)
				{
					text += string.Format("{0},{1},{2},{3},{4}|", new object[]
					{
						current.TemplateID,
						current.StrengthenLevel,
						current.Count,
						current.ValidDate,
						current.IsBinds.ToString()
					});
				}
				text = text.Substring(0, text.Length - 1);
				this.m_diceData.AwardArray = text;
			}
		}
		public virtual void SaveToDatabase()
		{
			if (this.m_saveToDb)
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					object @lock;
					Monitor.Enter(@lock = this.m_lock);
					try
					{
						if (this.m_diceData != null && this.m_diceData.IsDirty)
						{
							if (this.m_diceData.ID > 0)
							{
								playerBussiness.UpdateDiceData(this.m_diceData);
							}
							else
							{
								playerBussiness.AddDiceData(this.m_diceData);
							}
						}
					}
					finally
					{
						Monitor.Exit(@lock);
					}
				}
			}
		}
	}
}

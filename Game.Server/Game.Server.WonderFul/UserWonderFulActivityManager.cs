using System;
using Game.Server.GameObjects;
using Game.Server.GmActivity;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.WonderFul
{
	public class UserWonderFulActivityManager
	{
		private GamePlayer Player;

		public UserWonderFulActivityManager(GamePlayer player)
		{
			Player = player;
			Player.ItemStrengthen += Player_ItemStrengthen;
		}

		public void ConsumeMoney(int value)
		{
			if (value == 0)
			{
				return;
			}
			foreach (GmActivityInfo consumeActive in GmActivityMgr.ConsumeActives)
			{
				BaseUserGmActivity baseUserGmActivity = Player.GmActivity.FindUserGmActivity(consumeActive.activityId);
				baseUserGmActivity.SetValue(value);
			}
		}

		public void ChargeMoney(int value)
		{
			if (value == 0)
			{
				return;
			}
			try
			{
				foreach (GmActivityInfo payActive in GmActivityMgr.PayActives)
				{
					BaseUserGmActivity baseUserGmActivity = Player.GmActivity.FindUserGmActivity(payActive.activityId);
					baseUserGmActivity.SetValue(value);
				}
			}
			catch (Exception ex)
			{
				GameServer.log.Error("Charge money gmactivity error: " + Player.PlayerCharacter.NickName + ", money: " + value);
				GameServer.log.Error(ex.ToString());
			}
		}

		private void Player_ItemStrengthen(int categoryID, int level)
		{
			if (level <= 0)
			{
				return;
			}
			foreach (GmActivityInfo strengthenActive in GmActivityMgr.StrengthenActives)
			{
				BaseUserGmActivity baseUserGmActivity = Player.GmActivity.FindUserGmActivity(strengthenActive.activityId);
				baseUserGmActivity.SetValue(level);
			}
		}

		public void MountUp(int level)
		{
			if (level <= 0)
			{
				return;
			}
			foreach (GmActivityInfo mountActive in GmActivityMgr.MountActives)
			{
				BaseUserGmActivity baseUserGmActivity = Player.GmActivity.FindUserGmActivity(mountActive.activityId);
				baseUserGmActivity.SetValue(level);
			}
		}

		public void TempleUp(int level)
		{
			if (level <= 0)
			{
				return;
			}
			foreach (GmActivityInfo templeActive in GmActivityMgr.TempleActives)
			{
				BaseUserGmActivity baseUserGmActivity = Player.GmActivity.FindUserGmActivity(templeActive.activityId);
				baseUserGmActivity.SetValue(level);
			}
		}

		public void SignToday()
		{
			if (GmActivityMgr.SignActivity.Count <= 0)
			{
				return;
			}
			foreach (GmActivityInfo item in GmActivityMgr.SignActivity)
			{
				BaseUserGmActivity baseUserGmActivity = Player.GmActivity.FindUserGmActivity(item.activityId);
				baseUserGmActivity.SetValue(0);
			}
		}
	}
}

using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class ThanhZombie: ABrain
	{
		protected Player m_targer;

		private int m_turn = 0;
		
		#region NPC Chat
		private static string[] AllAttackChat = new string[] 
		{
            "Đã sáng mắt ra chưa?!~"	
        };
		
		private static string[] CallChat = new string[]
		{
            "Để tao vã nó.."
        };
		
		private static string[] KillPlayerChat = new string[]
		{
            "Tau sẽ dội linh hồn của mày bằng 6 lít nước......"
        };
		
		private static string[] KillAttackChat = new string[]
		{
             "Tôi năm nay 70 tuổi , mà tôi chưa gặp trường hợp này bao giờ."
        };
		#endregion

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			base.Body.CurrentDamagePlus = 1f;
			base.Body.CurrentShootMinus = 1f;
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.OnStartAttacking();
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			bool flag = false;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && Math.Abs(current.X - base.Body.X) < 150)
				{
					flag = true;
				}
			}
			if (flag)
			{
				//this.KillAttack(base.Body.X - 100, base.Body.X + 100);
				this.KillAttack(base.Body.X - 100, base.Body.X + 100);
			}
			else
			{
				this.m_targer = base.Game.FindRandomPlayer();
				if (this.m_turn == 0)
				{
					this.KillA();
					this.m_turn++;
				}
				else if (this.m_turn == 1)
				{
					this.KillB();
					this.m_turn++;
				}
				else
				{
					this.KillC();
					this.m_turn = 0;
				}
			}
		}

		private void KillA()
		{	
			int index = Game.Random.Next(0, AllAttackChat.Length); //Chat
            base.Body.Say(AllAttackChat[index], 1, 0);

			base.Body.CurrentDamagePlus = 120;
			base.Body.PlayMovie("beatA", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 500, this.m_targer.X + 500, "cry", 4000, null);
		}

		private void KillB()
		{	
			int index = Game.Random.Next(0, CallChat.Length); //Chat
            base.Body.Say(CallChat[index], 1, 0);
			
			base.Body.CurrentDamagePlus = 130;
			base.Body.PlayMovie("beatB", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 500, this.m_targer.X + 500, "cry", 4000, null);
		}

		private void KillC()
		{	
			int index = Game.Random.Next(0, KillPlayerChat.Length); //Chat
            base.Body.Say(KillPlayerChat[index], 1, 0);

			base.Body.CurrentDamagePlus = 140;
			base.Body.PlayMovie("beatC", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 500, this.m_targer.X + 500, "cry", 4000, null);
		}

		
		private void KillAttack(int fx, int tx) 
		{	
			int index = Game.Random.Next(0, KillAttackChat.Length); //Chat
            base.Body.Say(KillAttackChat[index], 1, 0);
			
			base.Body.CurrentDamagePlus = 100000000f;
			base.Body.PlayMovie("beatA", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null); 
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
// using Game.Logic;
// using Game.Logic.AI;
// using Game.Logic.Phy.Object;
// using System;

// namespace GameServerScript.AI.NPC
// {
	// public class ThanhZombie : ABrain
	// {
		// protected Player m_targer;

		// private int m_turn = 0;

		// private PhysicalObj moive;

         // #region NPC 说话内容
        // private static string[] AllAttackChat = new string[] {
            // "Tất cả là của tao , của tao",  
            // //"ĐM! Suốt ngày gặp Thằng cắt đầu moi!",

            // //"Động đất siêu chính hãng……<br/>Động đất …… Động đất …… "
        // };

        // private static string[] ShootChat = new string[]{
			// "Dĩ vãng dơ dáy dễ gì giấu giếm",
            // //"Tôi chưa gặp trường hợp này bao giờ"
        // };

        // private static string[] KillPlayerChat = new string[]{
            // "Nó đụng thì mình phải chạm, nó cảm thì mình phải xúc, nó muốn sụp thì mình phải cho đổ luôn!.",

            // //"Khả năng ảo tưởng？"
        // };

        // private static string[] CallChat = new string[]{
            // "Trứng Rán cần mỡ bắp cần bơ<br/>Yêu không cần cớ, cần cậu cơ...",
                  
            // //"Tôi chưa gặp trường hợp này bao giờ"
        // };

        // private static string[] ShootedChat = new string[]{
            // "Đừng thấy hoa nở mà ngỡ xuân về.",

            // //"Tôi vẫn có thể sống…"
        // };

        // private static string[] JumpChat = new string[]{
             // "Người trong cuộc mới hiểu người trong kẹt.", 

             // //"Tôi sẽ giết bạn về phía trước nửa bước！",

             // //"Ấy chà！<br/>Đầu moi！"
        // };

        // private static string[] KillAttackChat = new string[]{
             // "Dĩ vãng dơ dáy dễ gì giấu giếm"
        // };
        // #endregion

		// public override void OnBeginSelfTurn()
		// {
			// base.OnBeginSelfTurn();
		// }

		// public override void OnBeginNewTurn()
		// {
			// base.OnBeginNewTurn();
			// base.Body.CurrentDamagePlus = 1f;
			// base.Body.CurrentShootMinus = 1f;
		// }

		// public override void OnCreated()
		// {
			// base.OnCreated();
		// }

		// public override void OnStartAttacking()
		// {
			// base.OnStartAttacking();
			// base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			// bool flag = false;
			// foreach (Player current in base.Game.GetAllFightPlayers())
			// {
				// if (current.IsLiving && Math.Abs(current.X - base.Body.X) < 300)
				// {
					// flag = true;
				// }
			// }
			// if (flag)
			// {
				// this.KillAttack(base.Body.X - 1000, base.Body.X + 1000);
			// }
			// else if (this.m_turn == 0)
			// {
				// this.KillA();
				// this.m_turn++;
			// }
			// else if (this.m_turn == 1)
			// {
				// this.Attack2();
				// this.m_turn++;
			// }
			// else
			// {
				// this.Attack();
				// this.m_turn = 0;
			// }
		// }

		// private void KillA()
		// {
            
            // int index = Game.Random.Next(0, AllAttackChat.Length);
            // base.Body.Say(AllAttackChat[index], 1, 0);
			// Player player = base.Game.FindRandomPlayer();
			// base.Body.CurrentDamagePlus = 1.2f;
			// base.Body.PlayMovie("beatA", 2400, 0);
			
			// base.Body.RangeAttacking(player.X - 1000, player.X + 1000, "cry", 4000, null);
			// //this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "moive", "asset.game.fifteen.274c", "out", 1, 1);
		// }

		// public void Attack2()
		// {
            
            // int index = Game.Random.Next(0, CallChat.Length);
            // base.Body.Say(CallChat[index], 1, 3300);
			// base.Body.PlayMovie("beatB", 1200, 0);
			// base.Body.CallFuction(new LivingCallBack(this.KillB), 1000);
		// }

		// private void KillB()
		// { 
            // int index = Game.Random.Next(0, JumpChat.Length);
            // base.Body.Say(JumpChat[index], 1, 3300);
			// Player player = base.Game.FindRandomPlayer();
			// base.Body.CurrentDamagePlus = 1.5f;
			
			// base.Body.RangeAttacking(player.X - 1000, player.X + 1000, "cry", 4000, null);
			// //this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "moive", "asset.game.fifteen.274c", "out", 1, 1);
			// base.Body.CallFuction(new LivingCallBack(this.GoOut), 4000);
		// }

		// private void GoOut()
		// {
			// if (this.moive != null)
			// {
				// base.Game.RemovePhysicalObj(this.moive, true);
				// this.moive = null;
			// }
		// }

		// public void Attack()
		// {
            
            // int index = Game.Random.Next(0, CallChat.Length);
            // base.Body.Say(CallChat[index], 1, 3300);
			// Player player = base.Game.FindRandomPlayer();
			// base.Body.CurrentDamagePlus = 1.5f;
			// base.Body.PlayMovie("beatC", 2400, 0);
			// //this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "moive", "asset.game.fifteen.274c", "out", 1, 1);
			// base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 4000, null);
			// // if (player.X < base.Body.X)
			// // {
			// // 	int num = base.Body.X - (player.X + 289);
			// // 	base.Body.MoveTo(base.Body.X - num, player.Y, "walk", 2400, "", 5, new LivingCallBack(this.KillC));
			// // }
			// // else
			// // {
			// // 	int num = player.X - 289 - base.Body.X;
			// // 	base.Body.MoveTo(base.Body.X + num, player.Y, "walk", 2400, "", 5, new LivingCallBack(this.KillC));
			// // }
		// }

		// private void KillC()
		// {
            
            // int index = Game.Random.Next(0, AllAttackChat.Length);
            // base.Body.Say(AllAttackChat[index], 1, 0);
			// Player player = base.Game.FindRandomPlayer();
			// base.Body.CurrentDamagePlus = 2f;
			// base.Body.PlayMovie("beatC", 2400, 0);
			// base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 4000, null);
		// }

		// private void KillAttack(int fx, int tx)
		// {           
            // int index = Game.Random.Next(0, CallChat.Length);
            // base.Body.Say(CallChat[index], 1, 3300);           
			// base.Body.CurrentDamagePlus = 10000f;
			// base.Body.PlayMovie("beatC", 3000, 0);
			// base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		// }

		// public override void OnStopAttacking()
		// {
			// base.OnStopAttacking();
		// }
	// }
// }

// using Game.Logic;
// using Game.Logic.AI;
// using Game.Logic.Phy.Object;
// using System;

// namespace GameServerScript.AI.NPC
// {
	// public class NPCDSLT5 : ABrain
	// {
		// protected Player m_targer;

		// private int m_turn = 0;

		// private PhysicalObj moive;

		// public override void OnBeginSelfTurn()
		// {
			// base.OnBeginSelfTurn();
		// }

		// public override void OnBeginNewTurn()
		// {
			// base.OnBeginNewTurn();
			// base.Body.CurrentDamagePlus = 1f;
			// base.Body.CurrentShootMinus = 1f;
		// }

		// public override void OnCreated()
		// {
			// base.OnCreated();
		// }

		// public override void OnStartAttacking()
		// {
			// base.OnStartAttacking();
			// base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			// bool flag = false;
			// foreach (Player current in base.Game.GetAllFightPlayers())
			// {
				// if (current.IsLiving && Math.Abs(current.X - base.Body.X) < 300)
				// {
					// flag = true;
				// }
			// }
			// if (flag)
			// {
				// this.KillAttack(base.Body.X - 300, base.Body.X + 300);
			// }
			// else if (this.m_turn == 0)
			// {
				// this.KillA();
				// this.m_turn++;
			// }
			// else if (this.m_turn == 1)
			// {
				// this.Attack2();
				// this.m_turn++;
			// }
			// else
			// {
				// this.Attack();
				// this.m_turn = 0;
			// }
		// }

		// private void KillA()
		// {
			// Player player = base.Game.FindRandomPlayer();
			// base.Body.CurrentDamagePlus = 1.2f;
			// base.Body.PlayMovie("beatA", 2400, 0);
			// base.Body.RangeAttacking(player.X - 300, player.X + 300, "cry", 4000, null);
		// }

		// public void Attack2()
		// {
			// base.Body.PlayMovie("beatB", 1200, 0);
			// base.Body.CallFuction(new LivingCallBack(this.KillB), 1000);
		// }

		// private void KillB()
		// {
			// Player player = base.Game.FindRandomPlayer();
			// base.Body.CurrentDamagePlus = 1.2f;
			// this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "moive", "asset.game.ten.jianyu", "out", 1, 1);
			// base.Body.RangeAttacking(player.X - 300, player.X + 300, "cry", 4000, null);
			// base.Body.CallFuction(new LivingCallBack(this.GoOut), 4000);
		// }

		// private void GoOut()
		// {
			// if (this.moive != null)
			// {
				// base.Game.RemovePhysicalObj(this.moive, true);
				// this.moive = null;
			// }
		// }

		// public void Attack()
		// {
			// Player player = base.Game.FindRandomPlayer();
			// if (player.X < base.Body.X)
			// {
				// int num = base.Body.X - (player.X + 289);
				// base.Body.MoveTo(base.Body.X - num, player.Y, "walk", 2400, "", 5, new LivingCallBack(this.KillC));
			// }
			// else
			// {
				// int num = player.X - 289 - base.Body.X;
				// base.Body.MoveTo(base.Body.X + num, player.Y, "walk", 2400, "", 5, new LivingCallBack(this.KillC));
			// }
		// }

		// private void KillC()
		// {
			// base.Body.CurrentDamagePlus = 1.2f;
			// base.Body.PlayMovie("beatC", 2400, 0);
			// base.Body.RangeAttacking(base.Body.X - 300, base.Body.X + 300, "cry", 4000, null);
		// }

		// private void KillAttack(int fx, int tx)
		// {
			// base.Body.CurrentDamagePlus = 1.2f;
			// base.Body.PlayMovie("beatC", 3000, 0);
			// base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		// }

		// public override void OnStopAttacking()
		// {
			// base.OnStopAttacking();
		// }
	// }
// }


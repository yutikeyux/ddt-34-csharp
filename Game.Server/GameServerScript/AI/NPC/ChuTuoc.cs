using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class ConsortiaGuardRosefinch : ABrain 
	{
		protected Player m_targer;

		private int m_turn = 0;

		private PhysicalObj moive;

         #region NPC 说话内容
        private static string[] AllAttackChat = new string[] {
        "Không tệ lắm !"
        };

        private static string[] ShootChat = new string[]{
         "Bắn mắc iả hết Sứck"

        };

        private static string[] KillPlayerChat = new string[]{
         "Không ngờ chứ gì?"

        };

        private static string[] CallChat = new string[]{
        "Ngại ngùng các thứ"

        };

        private static string[] JumpChat = new string[]{
         "Đừng tỏ ra thèm thuồng như thế , người ta đánh giá~"

        };

        private static string[] KillAttackChat = new string[]{
             "Bùa Sinh Tử~~~"
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
				if (current.IsLiving && Math.Abs(current.X - base.Body.X) < 300)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.KillAttack(base.Body.X - 1000, base.Body.X + 1000);
				return;
			}

			if (this.m_turn == 0)
			{
				Healing();
				this.KillA();
				this.m_turn++;
			}
			else if (this.m_turn == 1)
			{
				Healing();
				this.KillB();
				this.m_turn++;
			}
			else if (this.m_turn == 2)
			{	
				Healing();
				this.KillC();
				this.m_turn++;
			}
			else if (this.m_turn == 3)
			{
				Healing();
				this.Attack2();
				this.m_turn++;
			}
			else
			{
				Healing();
				this.Attack();
				this.m_turn = 0;
			}
		}

		private void KillA()
		{ 
            int index = Game.Random.Next(0, AllAttackChat.Length);
            base.Body.Say(AllAttackChat[index], 1, 0);
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 600f;
			base.Body.PlayMovie("beatA", 2400, 0);
			base.Body.RangeAttacking(player.X - 1000, player.X + 1000, "cry", 4000, null);
		}

		public void Attack2()
		{ 
            int index = Game.Random.Next(0, ShootChat.Length);
            base.Body.Say(ShootChat[index], 1, 0);
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 600f;
			base.Body.PlayMovie("beatB", 2400, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 4000, null);
		}

		private void KillB()
		{ 
            int index = Game.Random.Next(0, KillPlayerChat.Length);
            base.Body.Say(KillPlayerChat[index], 1, 3300);
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 600f;
			base.Body.PlayMovie("beatB", 2400, 0);
			this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "moive", "asset.game.ten.jianyu", "out", 1, 1);
			base.Body.RangeAttacking(player.X - 1000, player.X + 1000, "cry", 4000, null);
			base.Body.CallFuction(new LivingCallBack(this.GoOut), 4000);
		}

		private void GoOut()
		{
			if (this.moive != null)
			{
				base.Game.RemovePhysicalObj(this.moive, true);
				this.moive = null;
			}
		}

		public void Attack()
		{
            int index = Game.Random.Next(0, CallChat.Length);
            base.Body.Say(CallChat[index], 1, 3300);
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 600f;
			base.Body.PlayMovie("beatC", 2400, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 4000, null);
			// if (player.X < base.Body.X)
			// {
			// 	int num = base.Body.X - (player.X + 289);
			// 	base.Body.MoveTo(base.Body.X - num, player.Y, "walk", 2400, "", 5, new LivingCallBack(this.KillC));
			// }
			// else
			// {
			// 	int num = player.X - 289 - base.Body.X;
			// 	base.Body.MoveTo(base.Body.X + num, player.Y, "walk", 2400, "", 5, new LivingCallBack(this.KillC));
			// }
		}

		private void KillC()
		{          
            int index = Game.Random.Next(0, JumpChat.Length);
            base.Body.Say(JumpChat[index], 1, 0);
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 600f;
			base.Body.PlayMovie("beatB", 2400, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 4000, null);
		}

		private void KillAttack(int fx, int tx)
		{           
            int index = Game.Random.Next(0, KillAttackChat.Length);
            base.Body.Say(KillAttackChat[index], 1, 3300);           
			base.Body.CurrentDamagePlus = 600f;
			base.Body.PlayMovie("beatC", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}
		
		public void Healing()
        {
            Body.SyncAtTime = true;            
            //Body.PlayMovie("cast", 2000, 0);
            Body.AddBlood(2000000);
            //Body.Say("Sao mà đỡ được hở cưng!", 1, 0);
        }

	

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}

using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class ThirdHardBloomNpcS : ABrain
	{
		private Player m_target = null;

		private int m_targetDis = 0;

		private int attackingTurn = 1;

		private int IsEixt = 0;

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
			bool flag = false;
			int num = 0;
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			this.m_target = base.Game.FindNearestPlayer(base.Body.X, base.Body.Y);
			this.m_targetDis = (int)this.m_target.Distance(base.Body.X, base.Body.Y);
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 0 && current.X < 0)
				{
					int num2 = (int)base.Body.Distance(current.X, current.Y);
					if (num2 > num)
					{
						num = num2;
					}
					flag = true;
				}
			}
			if (flag)
			{
				this.KillAttack(0, 0);
			}
			else if (!flag)
			{
				if (this.attackingTurn == 1)
				{
					if (this.m_targetDis < 100)
					{
						List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
						foreach (Player current in allLivingPlayers)
						{
							current.AddBlood(700);
						}
						base.Body.PlayMovie("renew", 700, 400);
					}
					else
					{
						this.MoveToPlayer(this.m_target);
					}
				}
				else if (this.attackingTurn == 2)
				{
					if (this.m_targetDis < 100)
					{
						List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
						foreach (Player current in allLivingPlayers)
						{
							current.AddBlood(700);
						}
						base.Body.PlayMovie("renew", 700, 400);
					}
					else
					{
						this.MoveToPlayer(this.m_target);
					}
				}
				else if (this.attackingTurn == 3)
				{
					if (this.m_targetDis < 100)
					{
						List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
						foreach (Player current in allLivingPlayers)
						{
							current.AddBlood(700);
						}
						base.Body.PlayMovie("renew", 700, 400);
					}
					else
					{
						this.MoveToPlayer(this.m_target);
					}
				}
				else if (this.attackingTurn == 4)
				{
					if (this.m_targetDis < 100)
					{
						List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
						foreach (Player current in allLivingPlayers)
						{
							current.AddBlood(700);
						}
						base.Body.PlayMovie("renew", 700, 400);
					}
					else
					{
						this.MoveToPlayer(this.m_target);
					}
				}
				else if (this.attackingTurn == 5)
				{
					base.Body.PlayMovie("die", 700, 400);
					base.Body.CallFuction(new LivingCallBack(this.MoveTo), 1500);
				}
				else
				{
					this.attackingTurn = 1;
				}
				this.attackingTurn++;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public void KillAttack(int fx, int mx)
		{
		}

		public void MoveToPlayer(Player player)
		{
			base.Body.Say("Đến gần tui sẻ hồi máu cho!", 0, 2000);
		}

		public void MoveTo()
		{
			if (this.IsEixt == 1)
			{
				base.Body.JumpToSpeed(478, 560, "born", 0, 0, 36, null);
				this.IsEixt = 0;
			}
			else
			{
				base.Body.JumpToSpeed(1000, 560, "born", 0, 0, 36, null);
				this.IsEixt = 1;
			}
		}

		public void Beat()
		{
			if (this.m_targetDis < 100)
			{
				List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
				foreach (Player current in allLivingPlayers)
				{
					current.AddBlood(700);
				}
				base.Body.PlayMovie("renew", 700, 400);
			}
		}
	}
}

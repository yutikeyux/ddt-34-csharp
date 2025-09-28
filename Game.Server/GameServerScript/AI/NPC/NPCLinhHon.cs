using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class NPCLinhHon : ABrain
	{
		protected Player m_targer;

		private PhysicalObj moive;

		private List<Player> m_denyPlayer;

		private int m_attackTurn = 0;

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			this.m_body.CurrentDamagePlus = 30f;
		}

		public override void OnCreated()
		{
			base.OnCreated();
			this.m_denyPlayer = new List<Player>();
		}

		public override void OnStartAttacking()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			if (this.moive != null)
			{
				List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
				this.m_denyPlayer.Clear();
				foreach (Player current in allFightPlayers)
				{
					bool flag = false;
					if (current.IsLiving)
					{
						if (current.X < this.moive.X)
						{
							if (this.moive.X - current.X < 100)
							{
								flag = true;
							}
						}
						else if (current.X > this.moive.X)
						{
							if (current.X - this.moive.X < 100)
							{
								flag = true;
							}
						}
						else if (current.X == this.moive.X)
						{
							flag = true;
						}
						if (flag)
						{
							this.m_denyPlayer.Add(current);
						}
					}
				}
			}
			if (this.m_denyPlayer.Count > 0)
			{
				this.TankC();
			}
			else if (this.m_attackTurn == 0)
			{
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 2)
			{
				this.TankA();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 3)
			{
				this.TankB();
				this.m_attackTurn = 2;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void AddBloodA()
		{
			for (int i = 0; i < this.m_denyPlayer.Count; i++)
			{
				this.m_denyPlayer[i].AddBlood(-100000, 1);
				if (this.m_denyPlayer[i].Blood <= 0)
				{
					this.m_denyPlayer[i].Die();
				}
			}
		}

		private void TankA()
		{
			base.Body.PlayMovie("beatA", 2400, 0);
			base.Body.CallFuction(new LivingCallBack(this.HieuUngA), 3000);
		}

		private void TankB()
		{
			base.Body.PlayMovie("beatB", 2400, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 3000);
			base.Body.CallFuction(new LivingCallBack(this.GoOut), 4000);
		}

		private void TankC()
		{
			base.Body.PlayMovie("beatB", 2400, 0);
			base.Body.CallFuction(new LivingCallBack(this.KillAttack), 3000);
			base.Body.CallFuction(new LivingCallBack(this.GoOut), 4000);
		}

		private void KillAttack()
		{
			((PVEGame)base.Game).SendGameFocus(this.m_denyPlayer[0].X, this.m_denyPlayer[0].Y, 1, 2000, 1000);
			base.Body.CallFuction(new LivingCallBack(this.AddBloodA), 3000);
		}

		private void HieuUngA()
		{
			Player player = base.Game.FindRandomPlayer();
			this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "dangerMoive", "asset.game.eleven.099", "out", 1, 1);
		}

		private void GoOut()
		{
			if (this.moive != null)
			{
				base.Game.RemovePhysicalObj(this.moive, true);
				this.moive = null;
			}
		}

		private void BodyDie()
		{
			((PVEGame)base.Game).ClearAllNpc();
			base.Body.Die();
		}

		private void RangeAttacking()
		{
			base.Body.RangeAttacking(0, base.Body.Game.Map.Info.ForegroundWidth + 1, "cry", 1000, null);
		}
	}
}

using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class TrainingSimpleNpc : SimpleNpcAi
	{
		public override void OnStartAttacking()
		{
			this.m_body.CurrentDamagePlus = 1f;
			this.m_body.CurrentShootMinus = 1f;
			this.m_targer = base.Game.FindNearestPlayer(base.Body.X, base.Body.Y);
			if (this.m_targer != null)
			{
				if (this.m_targer.Blood > 200)
				{
					base.Beating();
				}
				else
				{
					this.Beat();
				}
			}
		}

		public void BeatCallBack()
		{
			int blood = this.m_targer.Blood;
			int demageAmount = blood / 10;
			base.Body.Beat(this.m_targer, "beat", demageAmount, 0, 0, 1, 1);
		}

		private void Beat()
		{
			int blood = this.m_targer.Blood;
			int demageAmount = blood / 10;
			if (this.m_targer != null && !base.Body.Beat(this.m_targer, "beat", demageAmount, 0, 0, 1, 1))
			{
				int num = base.Game.Random.Next(80, 150);
				if (base.Body.X - this.m_targer.X > num)
				{
					base.Body.MoveTo(base.Body.X - num, this.m_targer.Y, "walk", 1200, "", ((SimpleNpc)base.Body).NpcInfo.speed, new LivingCallBack(this.BeatCallBack));
				}
				else
				{
					base.Body.MoveTo(base.Body.X + num, this.m_targer.Y, "walk", 1200, "", ((SimpleNpc)base.Body).NpcInfo.speed, new LivingCallBack(this.BeatCallBack));
				}
			}
		}
	}
}

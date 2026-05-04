using Game.Logic.AI;
using Game.Logic.AI.Npc;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Logic.Phy.Object
{
    public class SimpleNpc : Living
    {
        private static readonly ILog ilog_1;
        private readonly ABrain abrain_0;

        public new NpcInfo NpcInfo { get; }

        public SimpleNpc(int id, BaseGame game, NpcInfo npcInfo, int type, int direction, string action)
            : base(id, game, npcInfo.Camp, npcInfo.Name, npcInfo.ModelID, npcInfo.Blood, npcInfo.Immunity, direction)
        {
            base.Type = type == 0 ? eLivingType.SimpleNpc : eLivingType.SimpleNpc1;
            NpcInfo = npcInfo;
            base.ActionStr = action;
            abrain_0 = ScriptMgr.CreateInstance(npcInfo.Script) as ABrain;
            if (abrain_0 == null)
            {
                ilog_1.ErrorFormat("Can't create abrain :{0}", npcInfo.Script);
                abrain_0 = SimpleBrain.Simple;
            }
            abrain_0.Game = m_game;
            abrain_0.Body = this;
            try
            {
                abrain_0.OnCreated();
            }
            catch (Exception arg)
            {
                ilog_1.ErrorFormat("SimpleNpc Created error:{1}", arg);
            }
        }

        public override void Reset()
        {
            Agility = NpcInfo.Agility;
            Attack = NpcInfo.Attack;
            BaseDamage = NpcInfo.BaseDamage;
            BaseGuard = NpcInfo.BaseGuard;
            Lucky = NpcInfo.Lucky;
            Grade = NpcInfo.Level;
            Experience = NpcInfo.Experience;
            SetRect(NpcInfo.X, NpcInfo.Y, NpcInfo.Width, NpcInfo.Height);
            SetRelateDemagemRect(NpcInfo.X, NpcInfo.Y, NpcInfo.Width, NpcInfo.Height);
            if (m_direction == 1)
            {
                ReSetRectWithDir();
            }
            base.FireX = NpcInfo.FireX;
            base.FireY = NpcInfo.FireY;
            base.Reset();
        }

        public void GetDropItemInfo()
        {
            if (m_game.CurrentLiving is not Player)
            {
                return;
            }
            Player p = m_game.CurrentLiving as Player;
            List<ItemInfo> infos = null;
            int gold = 0;
            int money = 0;
            int gifttoken = 0;
            _ = DropInventory.NPCDrop(NpcInfo.DropId, ref infos);
            if (infos == null)
            {
                return;
            }
            foreach (ItemInfo info in infos)
            {
                ItemInfo tempInfo = ItemInfo.FindSpecialItemInfo(info, ref gold, ref money, ref gifttoken);
                if (tempInfo != null)
                {
                    if (tempInfo.Template.CategoryID == 10)
                    {
                        _ = p.PlayerDetail.AddTemplate(tempInfo, eBageType.FightBag, info.Count, eGameView.dungeonTypeGet);
                    }
                    else
                    {
                        _ = p.PlayerDetail.AddTemplate(tempInfo, eBageType.TempBag, info.Count, eGameView.dungeonTypeGet);
                    }
                }
            }
            _ = p.PlayerDetail.AddGold(gold);
            _ = p.PlayerDetail.AddMoney(money);
            _ = p.PlayerDetail.AddGiftToken(gifttoken);
        }

        public override void Die()
        {
            GetDropItemInfo();
            base.Die();
        }

        public override void Die(int delay)
        {
            GetDropItemInfo();
            base.Die(delay);
        }

        public override void PrepareNewTurn()
        {
            base.PrepareNewTurn();
            try
            {
                abrain_0.OnBeginNewTurn();
            }
            catch (Exception arg)
            {
                ilog_1.ErrorFormat("SimpleNpc BeginNewTurn error:{1}", arg);
            }
        }

        public override void StartAttacking()
        {
            base.StartAttacking();
            try
            {
                abrain_0.OnStartAttacking();
            }
            catch (Exception arg)
            {
                ilog_1.ErrorFormat("SimpleNpc StartAttacking error:{1}", arg);
            }
        }
        public override void OnBeforeTakedBomb()
        {
            try
            {
                abrain_0.OnBeforeTakedBomb();
            }
            catch (Exception arg)
            {
                ilog_1.ErrorFormat("SimpleNpc OnBeforeTakedBomb error:{1}", arg);
            }
        }

        public override void OnDieByBomb()
        {
            try
            {
                abrain_0.OnDieByBomb();
            }
            catch (Exception arg)
            {
                ilog_1.ErrorFormat("SimpleNpc OnDieByBomb error:{1}", arg);
            }
        }

        public override void StopAttacking()
        {
            base.StopAttacking();
        }
        public override void Dispose()
        {
            base.Dispose();
            try
            {
                abrain_0.Dispose();
            }
            catch (Exception arg)
            {
                ilog_1.ErrorFormat("SimpleNpc Dispose error:{1}", arg);
            }
        }

        public override void OnAfterTakedBomb()
        {
            try
            {
                abrain_0.OnAfterTakedBomb();
            }
            catch (Exception arg)
            {
                ilog_1.ErrorFormat("SimpleNpc OnAfterTakedBomb error:{1}", arg);
            }
        }

        public override void OnAfterTakeDamage(Living source)
        {
            try
            {
                abrain_0.OnAfterTakeDamage(source);
            }
            catch (Exception arg)
            {
                ilog_1.ErrorFormat("SimpleNpc OnAfterTakedDamage error:{1}", arg);
            }
        }

        public override void OnAfterTakedFrozen()
        {
            try
            {
                abrain_0.OnAfterTakedFrozen();
            }
            catch (Exception arg)
            {
                ilog_1.ErrorFormat("SimpleBoss OnAfterTakedFrozen error:{1}", arg);
            }
        }

        public override void OnHeal(int blood)
        {
            try
            {
                abrain_0.OnHeal(blood);
            }
            catch (Exception ex)
            {
                ilog_1.ErrorFormat("SimpleNpc OnHeal error:{0}", ex);
            }
            base.OnHeal(blood);
        }

        public override void OnDie()
        {
            try
            {
                abrain_0.OnDie();
            }
            catch (Exception ex)
            {
                ilog_1.ErrorFormat("SimpleNpc OnDie Error:{0}", ex);
            }
            base.OnDie();
        }

        static SimpleNpc()
        {
            ilog_1 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}

using Game.Logic.Actions;
using Game.Logic.AI;
using Game.Logic.AI.Npc;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Game.Logic.Phy.Object
{
    public class SimpleBoss : TurnedLiving
    {
        private static readonly ILog ilog_1;
        private readonly ABrain abrain_0;
        private readonly Dictionary<Player, int> dictionary_0;

        public new NpcInfo NpcInfo { get; }

        public List<SimpleNpc> Child { get; private set; }

        public List<SimpleBoss> Boss { get; } = [];
        public int CurrentLivingBossNum
        {
            get
            {
                int count = 0;
                foreach (SimpleBoss boss in Boss)
                {
                    if (!boss.IsLiving)
                    {
                        count++;
                    }
                }
                return Boss.Count - count;
            }
        }
        public void TowardsToPlayer(int playerX, int delay)
        {
            if (playerX > X)
            {
                ChangeDirection(1, delay);
            }
            else
            {
                ChangeDirection(-1, delay);
            }
        }
        public int CurrentLivingNpcNum
        {
            get
            {
                int num = 0;
                foreach (SimpleNpc item in Child)
                {
                    if (!item.IsLiving)
                    {
                        num++;
                    }
                }
                return Child.Count - num;
            }
        }

        public SimpleBoss(int id, BaseGame game, NpcInfo npcInfo, int direction, int type, string actions)
            : base(id, game, npcInfo.Camp, npcInfo.Name, npcInfo.ModelID, npcInfo.Blood, npcInfo.Immunity, direction)
        {
            Child = [];
            base.Type = type switch
            {
                0 => eLivingType.ClearEnemy,
                1 => eLivingType.SimpleBoss,
                2 => eLivingType.SimpleNpc1,
                3 => eLivingType.BossSpecialDie,
                _ => (eLivingType)type,
            };
            base.ActionStr = actions;
            dictionary_0 = [];
            NpcInfo = npcInfo;
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
                ilog_1.ErrorFormat("SimpleBoss Created error:{1}", arg);
            }
        }

        public override void Reset()
        {
            m_maxBlood = NpcInfo.Blood;
            BaseDamage = NpcInfo.BaseDamage;
            BaseGuard = NpcInfo.BaseGuard;
            Attack = NpcInfo.Attack;
            Defence = NpcInfo.Defence;
            Agility = NpcInfo.Agility;
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

        public override void Die()
        {
            base.Die();
        }

        public override void Die(int delay)
        {
            base.Die(delay);
        }
        public List<SimpleNpc> FindChildLivings()
        {
            List<SimpleNpc> simpleNpcs = [];
            lock (Child)
            {
                foreach (SimpleNpc list0 in Child)
                {
                    if (list0 == null || !list0.IsLiving)
                    {
                        continue;
                    }

                    simpleNpcs.Add(list0);
                }
            }

            return simpleNpcs;
        }
        public void ClearDiedLiving()
        {
            List<SimpleNpc> npcs = new();
            lock (Child)
            {
                foreach (var child in Child)
                {
                    if (child.IsLiving)
                    {
                        continue;
                    }

                    npcs.Add(child);
                }
                foreach (var npc in npcs)
                {
                    _ = Child.Remove(npc);
                }
            }
        }
        public override bool TakeDamage(Living source, ref int damageAmount, ref int criticalAmount, string msg)
        {
            bool result = base.TakeDamage(source, ref damageAmount, ref criticalAmount, msg);
            if (source is Player)
            {
                Player key = source as Player;
                int num = damageAmount + criticalAmount;
                if (dictionary_0.ContainsKey(key))
                {
                    dictionary_0[key] += num;
                    return result;
                }
                dictionary_0.Add(key, num);
            }
            return result;
        }

        public Player FindMostHatefulPlayer()
        {
            if (dictionary_0.Count > 0)
            {
                KeyValuePair<Player, int> keyValuePair = dictionary_0.ElementAt(0);
                foreach (KeyValuePair<Player, int> item in dictionary_0)
                {
                    if (keyValuePair.Value < item.Value)
                    {
                        keyValuePair = item;
                    }
                }
                return keyValuePair.Key;
            }
            return null;
        }

        public SimpleNpc CreateBoss(int id, int x, int y, int direction, int type)
        {
            SimpleNpc simpleNpc = ((PVEGame)base.Game).CreateNpc(id, x, y, type, direction);
            Child.Add(simpleNpc);
            return simpleNpc;
        }

        public void CreateChild(int id, int x, int y, int disToSecond, int maxCount)
        {
            CreateChild(id, x, y, disToSecond, maxCount, -1);
        }

        public void CreateChild(int id, int x, int y, int disToSecond, int maxCount, int direction)
        {
            if (CurrentLivingNpcNum < maxCount)
            {
                if (maxCount - CurrentLivingNpcNum >= 2)
                {
                    Child.Add(((PVEGame)base.Game).CreateNpc(id, x + disToSecond, y, 1, direction));
                    Child.Add(((PVEGame)base.Game).CreateNpc(id, x, y, 1, direction));
                }
                else if (maxCount - CurrentLivingNpcNum == 1)
                {
                    Child.Add(((PVEGame)base.Game).CreateNpc(id, x, y, 1, direction));
                }
            }
        }

        public SimpleNpc CreateChild(int id, int x, int y, bool showBlood, LivingConfig config)
        {
            return CreateChild(id, x, y, 1, -1, showBlood, config);
        }

        public SimpleNpc CreateChild(int id, int x, int y, int dir, bool showBlood, LivingConfig config)
        {
            return CreateChild(id, x, y, 1, dir, showBlood, config);
        }

        public SimpleNpc CreateChild(int id, int x, int y, int type, int dir, bool showBlood, LivingConfig config)
        {
            SimpleNpc simpleNpc = ((PVEGame)base.Game).CreateNpc(id, x, y, type, dir, config);
            Child.Add(simpleNpc);
            if (!showBlood)
            {
                base.Game.SendLivingShowBlood(simpleNpc, 0);
            }
            return simpleNpc;
        }

        public void CreateChild(int id, Point[] brithPoint, int maxCount, int maxCountForOnce, int type)
        {
            int num2 = base.Game.Random.Next(0, maxCountForOnce);
            for (int i = 0; i < num2; i++)
            {
                int num = base.Game.Random.Next(0, brithPoint.Length);
                CreateChild(id, brithPoint[num].X, brithPoint[num].Y, 4, maxCount);
            }
        }

        public List<SimpleNpc> FindChildLiving(int npcId)
        {
            List<SimpleNpc> list = [];
            foreach (SimpleNpc item in Child)
            {
                if (item != null && item.IsLiving && item.NpcInfo.ID == npcId)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public void RemoveAllChild()
        {
            foreach (SimpleNpc item in Child)
            {
                if (item.IsLiving)
                {
                    item.Die();
                }
            }
            Child = [];
        }

        public void RandomSay(string[] msg, int type, int delay, int finishTime)
        {
            int num = base.Game.Random.Next(0, msg.Length);
            string text = msg[num];
            m_game.AddAction(new LivingSayAction(this, text, type, delay, finishTime));
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
                ilog_1.ErrorFormat("SimpleBoss BeginNewTurn error:{1}", arg);
            }
        }

        public override void PrepareSelfTurn()
        {
            base.PrepareSelfTurn();
            AddDelay(NpcInfo.Delay);
            try
            {
                abrain_0.OnBeginSelfTurn();
            }
            catch (Exception arg)
            {
                ilog_1.ErrorFormat("SimpleBoss BeginSelfTurn error:{1}", arg);
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
                ilog_1.ErrorFormat("SimpleBoss StartAttacking error:{1}", arg);
            }
            if (base.IsAttacking)
            {
                StopAttacking();
            }
        }
        public override void OnBeforeTakedBomb()
        {
            try
            {
                abrain_0.OnBeforeTakedBomb();
            }
            catch (Exception)
            {
            }
        }

        public override void StopAttacking()
        {
            base.StopAttacking();
        }

        public override void OnAfterTakedBomb()
        {
            try
            {
                abrain_0.OnAfterTakedBomb();
            }
            catch (Exception arg)
            {
                ilog_1.ErrorFormat("SimpleBoss OnAfterTakedBomb error:{1}", arg);
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
                ilog_1.ErrorFormat("SimpleBoss OnDieByBomb error:{1}", arg);
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
                ilog_1.ErrorFormat("SimpleBoss OnAfterTakedDamage error:{1}", arg);
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
                ilog_1.ErrorFormat("SimpleBoss OnHeal error:{0}", ex);
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
                ilog_1.ErrorFormat("SimpleBoss OnDie Error:{0}", ex);
            }
            base.OnDie();
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
                ilog_1.ErrorFormat("SimpleBoss Dispose error:{1}", arg);
            }
        }

        static SimpleBoss()
        {
            ilog_1 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public void CreateChild(int id, Point[] brithPoint, int maxCount, int maxCountForOnce, int type, int direction)
        {
            int length = base.Game.Random.Next(0, maxCountForOnce);
            for (int i = 0; i < length; i++)
            {
                int index = base.Game.Random.Next(0, brithPoint.Length);
                CreateChild(id, brithPoint[index].X, brithPoint[index].Y, 4, maxCount, direction);
            }
        }

        public void CreateBoss(int id, int x, int y, int direction, int disToSecond, int maxCount, string action)
        {
            CreateBoss(id, x, y, direction, 1, disToSecond, maxCount, action);
        }

        public void CreateBoss(int id, int x, int y, int direction, int type, int disToSecond, int maxCount, string action)
        {
            if (CurrentLivingBossNum < maxCount)
            {
                if (maxCount - CurrentLivingNpcNum >= 2)
                {
                    Boss.Add(((PVEGame)base.Game).CreateBoss(id, x + disToSecond, y, direction, type, action));
                    Boss.Add(((PVEGame)base.Game).CreateBoss(id, x, y, direction, type, action));
                }
                else if (maxCount - CurrentLivingBossNum == 1)
                {
                    Boss.Add(((PVEGame)base.Game).CreateBoss(id, x, y, direction, type, action));
                }
            }
        }
    }
}

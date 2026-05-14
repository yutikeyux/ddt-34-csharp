using Bussiness;
using Bussiness.Managers;
using Game.Logic.Actions;
using Game.Logic.CardEffect.Effects;
using Game.Logic.Effects;
using Game.Logic.Game.Logic;
using Game.Logic.PetEffects.Element.Actives;
using Game.Logic.PetEffects.Element.Passives;
using Game.Logic.Phy.Maths;
using Game.Logic.Spells;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.Phy.Object
{
    public class Player : TurnedLiving
    {
        public int BossCardCount;
        public int CanTakeOut;
        private static readonly int CARRY_TEMPLATE_ID = 10016;
        public bool FinishTakeCard;
        public int GainGP;
        public int GainOffer;
        public bool HasPaymentTakeCard;
        private readonly Dictionary<int, int> ItemFightBag;
        public bool LockDirection;
        private int m_AddWoundBallId;
        private int m_ballCount;
        private bool m_canGetProp;
        private ItemInfo m_Healstone;
        private int m_loadingProcess;
        private int m_mainBallId;
        private int m_MultiBallId;
        public bool AttackInformation;
        public bool DefenceInformation;
        public int MaxPsychic = 999;
        private int m_shootCount;
        private int m_spBallId;
        private readonly ArrayList m_tempBoxes;
        public bool Ready;
        public Point TargetPoint;
        public int TotalAllCure;
        public int TotalAllExperience;
        public int TotalAllHitTargetCount;
        public int TotalAllHurt;
        public int TotalAllKill;
        public int TotalAllScore;
        public int TotalAllShootCount;
        public bool LimitEnergy;
        public bool CanFly = true;
        public bool IsShadown = true;

        private readonly List<int> AllowedItems =
        [
            10009,
            10010,
            10011,
            10012,
            10018,
            10021
        ];
        private readonly Random rand;
        private readonly PetFightPropertyInfo petFightPropertyInfo;
        private readonly BufferInfo m_bufferPoint;
        public new int MOVE_SPEED;
        private double speedMultiplier;
        public event PlayerEventHandle PlayerSkip;
        private int m_useitemCount;
        public bool IsAddTurnEquip { get; set; }
        public new PlayerConfig Config { get; set; }
        public void OnPlayerSkip()
        {
            PlayerSkip?.Invoke(this);
        }
        public Dictionary<int, PetSkillInfo> PetSkillCD { get; }
        public int PowerRatio { get; set; }
        public double SpeedMult
        {
            get => speedMultiplier; set => speedMultiplier = value / STEP_X;
        }
        public int StepX => (int)(STEP_X * speedMultiplier);
        public int StepY => (int)(STEP_Y * speedMultiplier);
        public int CurrentDelay { get; set; }
        public int BallCount
        {
            get => m_ballCount;
            set
            {
                if (m_ballCount != value)
                {
                    m_ballCount = value;
                }
            }
        }
        public bool CanGetProp
        {
            get => m_canGetProp;
            set
            {
                if (m_canGetProp != value)
                {
                    m_canGetProp = value;
                }
            }
        }
        public BallInfo CurrentBall { get; private set; }
        public int ChangeSpecialBall { get; set; }
        public ItemInfo DeputyWeapon { get; set; }
        public int deputyWeaponCount { get; private set; }
        public int Energy { get; set; }
        public int flyCount { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsSpecialSkill => CurrentBall.ID == m_spBallId;
        public int LoadingProcess
        {
            get => m_loadingProcess;
            set
            {
                if (m_loadingProcess != value)
                {
                    m_loadingProcess = value;
                    if (m_loadingProcess >= 100)
                    {
                        OnLoadingCompleted();
                    }
                }
            }
        }
        public int KilledPunishmentOffer { get; set; }

        public int OldX { get; set; }

        public int OldY { get; set; }

        public IGamePlayer PlayerDetail { get; }

        public int Prop { get; set; }

        public new int ShootCount
        {
            get => m_shootCount;
            set
            {
                if (m_shootCount != value)
                {
                    m_shootCount = value;
                    m_game.SendGameUpdateShootCount(this);
                }
            }
        }

        public int IsBombOrIgnoreAemor { get; set; }


        public ItemInfo Weapon { get; private set; }

        public UsersPetInfo Pet { get; }

        public event PlayerEventHandle AfterPlayerShooted;

        public event PlayerEventHandle BeforeBomb;

        public event PlayerEventHandle BeforePlayerShoot;

        public event PlayerEventHandle CollidByObject;

        public event PlayerEventHandle LoadingCompleted;

        public event PlayerEventHandle PlayerShootCure;

        public event PlayerEventHandle PlayerBeginMoving;

        public event PlayerEventHandle PlayerBuffSkillPet;

        public event PlayerEventHandle PlayerClearBuffSkillPet;

        public event PlayerEventHandle PlayerCure;

        public event PlayerEventHandle PlayerGuard;

        public event PlayerEventHandle PlayerShoot;

        public event PlayerEventHandle PlayerCompleteShoot;

        public event PlayerEventHandle PlayerAnyShellThrow;

        public event PlayerSecondWeaponEventHandle PlayerUseSecondWeapon;

        public event PlayerMissionEventHandle MissionEventHandle;

        public event PlayerEventHandle PlayerBeforeReset;

        public event PlayerEventHandle PlayerAfterReset;

        public Player(IGamePlayer player, int id, BaseGame game, int team, int maxBlood)
            : base(id, game, team, "", "", maxBlood, 0, 1)
        {
            m_rect = new Rectangle(-15, -20, 30, 30);
            PetSkillCD = [];
            PlayerDetail = player;
            PlayerDetail.GamePlayerId = id;
            m_canGetProp = true;
            Grade = player.PlayerCharacter.Grade;
            TotalAllHurt = 0;
            TotalAllHitTargetCount = 0;
            TotalAllShootCount = 0;
            TotalAllKill = 0;
            TotalAllExperience = 0;
            TotalAllScore = 0;
            TotalAllCure = 0;
            m_loadingProcess = 0;
            ChangeSpecialBall = 0;
            Prop = 0;
            base.VaneOpen = base.AutoBoot || player.PlayerCharacter.Grade >= 9;

            Weapon = PlayerDetail.MainWeapon;
            DeputyWeapon = PlayerDetail.SecondWeapon;
            m_Healstone = PlayerDetail.Healstone;

            Pet = player.Pet;

            if (game != null)
            {
                InitFightBuffer(player.FightBuffs);
                if (Pet != null)
                {
                    //PetMP = 10;
                    base.isPet = true;
                    PetEffects.PetBaseAtt = GetPetBaseAtt();
                    InitPetSkillEffect();
                    petFightPropertyInfo = PetMgr.FindFightProperty(player.PlayerCharacter.evolutionGrade);
                }
                m_tempBoxes = [];
                flyCount = 2;
                speedMultiplier = 1.0;
                MOVE_SPEED = 2;
                ItemFightBag = [];


                PlayerDetail.GameId = id;
                IsActive = true;



                base.BlockTurn = false;
                deputyWeaponCount = (DeputyWeapon == null) ? 1 : (DeputyWeapon.StrengthenLevel + 1);
                if (Weapon != null)
                {
                    BallConfigInfo ball = BallConfigMgr.FindBall(Weapon.TemplateID);
                    if (Weapon.IsValidGoldItem())
                    {
                        ball = BallConfigMgr.FindBall(Weapon.GoldEquip.TemplateID);
                    }
                    m_mainBallId = ball.Common;
                    m_spBallId = ball.Special;
                    m_AddWoundBallId = ball.CommonAddWound;
                    m_MultiBallId = ball.CommonMultiBall;
                }
                InitBuffer(PlayerDetail.EquipEffect);
                Energy = ((PlayerDetail.PlayerCharacter.AgiAddPlus + PlayerDetail.PlayerCharacter.Agility) / 30) + 240;
                m_useitemCount = 0;
                m_maxBlood = PlayerDetail.PlayerCharacter.hp;
                if (base.FightBuffers.ConsortionAddMaxBlood > 0)
                {
                    m_maxBlood += m_maxBlood * base.FightBuffers.ConsortionAddMaxBlood / 100;
                }
                m_maxBlood += PlayerDetail.PlayerCharacter.HpAddPlus + base.FightBuffers.WorldBossHP + base.FightBuffers.WorldBossHP_MoneyBuff + base.PetEffects.MaxBlood;
                CanFly = true;
                PowerRatio = 100;
                if (game != null && !game.IsSpecialPVE())
                {
                    BufferInfo fightBuffByType = GetFightBuffByType(BuffType.Agility);
                    if (fightBuffByType != null && PlayerDetail.UsePayBuff(BuffType.Agility))
                    {
                        m_bufferPoint = fightBuffByType;
                    }
                }
                propsBloqueados = [];
                IsBombOrIgnoreAemor = 0;
                Config = new PlayerConfig();
                IsAddTurnEquip = false;
            }
            CurrentDelay = 0;
        }

        public int GetPetBaseAtt()
        {
            try
            {
                string[] skillArray = Pet.SkillEquip.Split('|');
                for (int i = 0; i < skillArray.Length; i++)
                {
                    int skillID = Convert.ToInt32(skillArray[i].Split(',')[0]);
                    PetSkillInfo newBall = PetMgr.FindPetSkill(skillID);
                    if (newBall != null && newBall.Damage > 0)
                    {
                        return newBall.Damage;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("______________GetPetBaseAtt ERROR______________");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("_______________________________________________");
                return 0;
            }
            return 0;
        }

        public bool CanUseItem(ItemTemplateInfo item)
        {
            if (CurrentBall.IsSpecial() && !AllowedItems.Contains(item.TemplateID))
            {
                return false;
            }
            if (Energy < item.Property4)
            {
                return false;
            }
            return !base.IsAttacking
                ? !base.IsLiving && base.Team == m_game.CurrentLiving.Team && IsActive
                : !propsBloqueados.Contains(item.TemplateID);
        }

        public bool CanUseItem(ItemTemplateInfo item, int place)
        {
            if (CurrentBall.IsSpecial() && !AllowedItems.Contains(item.TemplateID))
            {
                return false;
            }
            if (!base.IsLiving && place == -1)
            {
                return base.psychic >= item.Property7;
            }
            if (!base.IsLiving && place != -1 && base.Team == m_game.CurrentLiving.Team)
            {
                return true;
            }
            if (Energy < item.Property4)
            {
                return false;
            }
            return IsAttacking || !base.IsLiving && base.Team == m_game.CurrentLiving.Team && IsActive;
        }

        public void capnhatstate(string loai1, string loai2)
        {
            m_game.capnhattrangthai(this, loai1, loai2);
        }

        public override void CollidedByObject(Physics phy)
        {
            base.CollidedByObject(phy);
            if (phy is SimpleBomb)
            {
                OnCollidedByObject();
            }
        }

        public bool CheckCanUseItem(ItemTemplateInfo item)
        {
            switch (item.TemplateID)
            {
                case 10001:
                    if (!ItemFightBag.ContainsKey(10003) || !ItemFightBag.ContainsKey(10002))
                    {
                        if (ItemFightBag.ContainsKey(10001) && ItemFightBag[10001] >= 2)
                        {
                            return false;
                        }
                        break;
                    }
                    return false;
                case 10002:
                    if (!ItemFightBag.ContainsKey(10003) || !ItemFightBag.ContainsKey(10001))
                    {
                        if (ItemFightBag.ContainsKey(10002) && ItemFightBag[10002] >= 2)
                        {
                            return false;
                        }
                        break;
                    }
                    return false;
                case 10003:
                    if (!ItemFightBag.ContainsKey(10024) && !ItemFightBag.ContainsKey(10025))
                    {
                        if (ItemFightBag.ContainsKey(10001) && ItemFightBag.ContainsKey(10002))
                        {
                            return false;
                        }
                        break;
                    }
                    return false;
                case 10025:
                    if (ItemFightBag.ContainsKey(10003) || ItemFightBag.ContainsKey(10024) || ItemFightBag.ContainsKey(10015))
                    {
                        return false;
                    }
                    break;
                case 10015:
                    if (ItemFightBag.ContainsKey(10003) || ItemFightBag.ContainsKey(10024) || ItemFightBag.ContainsKey(10025))
                    {
                        return false;
                    }
                    break;
            }
            if (!ItemFightBag.ContainsKey(item.TemplateID))
            {
                ItemFightBag.Add(item.TemplateID, 1);
            }
            else
            {
                ItemFightBag[item.TemplateID]++;
            }
            return true;
        }

        public bool CheckShootPoint(int x, int y)
        {
            return true;
        }

        public void DeadLink()
        {
            IsActive = false;
            if (base.IsLiving)
            {
                Die();
            }
        }

        public override void Die()
        {
            if (base.IsLiving)
            {
                m_y -= 70;
                base.Die();
            }
        }

        public void InitBuffer(List<int> equpedEffect)
        {
            for (int index = 0; index < equpedEffect.Count; index++)
            {
                ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(equpedEffect[index]);
                switch (itemTemplate.Property3)
                {
                    case 1:
                        _ = new AddAttackEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 2:
                        _ = new AddDefenceEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 3:
                        _ = new AddAgilityEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 4:
                        _ = new AddLuckyEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 5:
                        _ = new AddDamageEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 6:
                        _ = new ReduceDamageEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 7:
                        _ = new AddBloodEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 8:
                        _ = new FatalEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 9:
                        _ = new IceFronzeEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 10:
                        _ = new NoHoleEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 11:
                        _ = new AtomBombEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 12:
                        _ = new ArmorPiercerEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 13:
                        _ = new AvoidDamageEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 14:
                        _ = new MakeCriticalEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 15:
                        _ = new AssimilateDamageEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 16:
                        _ = new AssimilateBloodEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 17:
                        _ = new SealEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 18:
                        _ = new AddTurnEquipEffect(itemTemplate.Property4, itemTemplate.Property5, itemTemplate.TemplateID).Start(this);
                        break;
                    case 19:
                        _ = new AddDanderEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 20:
                        _ = new ReflexDamageEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 21:
                        _ = new ReduceStrengthEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 22:
                        _ = new ContinueReduceBloodEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 23:
                        _ = new LockDirectionEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 24:
                        _ = new AddBombEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 25:
                        _ = new ContinueReduceDamageEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                    case 26:
                        _ = new RecoverBloodEffect(itemTemplate.Property4, itemTemplate.Property5).Start(this);
                        break;
                }
            }
        }

        public void InitPetSkillEffect()
        {
            string[] listSkills = Pet.SkillEquip.Split('|');
            foreach (string skill in listSkills)
            {
                int skillId = int.Parse(skill.Split(',')[0]);
                PetSkillInfo skillInfo = PetMgr.FindPetSkill(skillId);
                if (skillInfo == null)
                {
                    continue;
                }

                string[] elementIDs = skillInfo.ElementIDs.Split(',');
                int coldDown = skillInfo.ColdDown;
                int probability = skillInfo.Probability;
                int delay = skillInfo.Delay;
                int gameType = skillInfo.GameType;
                if (!PetSkillCD.ContainsKey(skillId))
                {
                    PetSkillCD.Add(skillId, skillInfo);
                }

                //Console.WriteLine(string.Format("InitPetSkillEffect, skillInfo.ElementIDs: {0}", skillInfo.ElementIDs));
                foreach (string element in elementIDs)
                {
                    if (string.IsNullOrEmpty(element))
                    {
                        continue;
                    }

                    switch (element)
                    {
                        #region Skill Chung
                        case "1017"://Di chuyển không thể. Duy trì 2 TURN
                            _ = new AE1017(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1021"://miễn kháng. Duy trì 2 TURN
                            _ = new AE1021(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1038"://hiệu ứng dẫn đường, duy trì 1 turn.
                            _ = new AE1038(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1082"://Luôn miễn kháng
                            _ = new AE1082(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1138":// 100% xác suất bạo kích
                            _ = new AE1138(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1325"://Thú cưng sau khi kết thúc mỗi turn có 100% xác suất tấn công địch, gây 15% sát thương.
                            _ = new PE1325(coldDown, probability, gameType, skillId, delay, "1110").Start(this);
                            break;
                        case "1326"://Thú cưng sau khi kết thúc mỗi turn có 100% xác suất tấn công địch, gây 25% sát thương.
                            _ = new PE1326(coldDown, probability, gameType, skillId, delay, "1110").Start(this);
                            break;
                        case "1327"://Thú cưng sau khi kết thúc mỗi turn có 100% xác suất tấn công địch, gây 36% sát thương.
                            _ = new PE1327(coldDown, probability, gameType, skillId, delay, "1110").Start(this);
                            break;
                        #endregion
                        #region Gà Con
                        case "1328"://Bắn 1 Đạn Theo Dõi, gây 100% sát thương cơ bản.
                            _ = new AE1328(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1329"://Bắn 1 Đạn Theo Dõi, gây 130% sát thương cơ bản.
                            _ = new AE1329(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1330"://Bắn 1 Đạn Theo Dõi, gây 155% sát thương cơ bản.
                            _ = new AE1330(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1331"://Nhận hiệu quả phòng thủ +15%, duy trì 2 turn.
                            _ = new AE1331(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1332"://Nhận hiệu quả phòng thủ +20%, duy trì 2 turn.
                            _ = new AE1332(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1333"://Nhận hiệu quả phòng thủ +30%, duy trì 2 turn.
                            _ = new AE1333(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1334"://tấn công -20%,  duy trì 2 turn.
                            _ = new AE1334(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1336"://Nhận thêm 250 điểm hộ giáp, khi giải trừ sẽ mất hiệu quả cộng thêm, tối đa cộng dồn 3 lần.
                            _ = new AE1336(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1337"://Nhận thêm 365 điểm hộ giáp, khi giải trừ sẽ mất hiệu quả cộng thêm, tối đa cộng dồn 3 lần.
                            _ = new AE1337(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1338"://Hiệu quả giải trừ.
                            //new AE1339(coldDown, probability, gameType, skillId, delay, "1339").Start(this);
                            _ = new AE1339(coldDown, probability, gameType, skillId, delay, "1338").Start(this);
                            break;
                        case "1340"://Mỗi lần bắn gây sát thương bằng 2% HP hiện tại của bản thân
                            _ = new PE1340(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1341"://Mỗi lần bắn gây sát thương bằng 3% HP hiện tại của bản thân
                            _ = new PE1341(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1342"://Tăng 30% sát thương. Hiệu quả mất khi di chuyển.
                            _ = new AE1342(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1343"://Sát thương +45%. Hiệu quả mất khi di chuyển.
                            _ = new AE1343(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1344": //may mắn +10%. Hiệu quả mất khi di chuyển.
                            _ = new AE1344(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1346"://giảm 30% hộ giáp. Hiệu quả mất khi di chuyển.
                            _ = new AE1346(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1347"://hộ giáp -25%. Hiệu quả mất khi di chuyển.
                            _ = new AE1347(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1345": //may mắn +15%. Hiệu quả mất khi di chuyển.
                            _ = new AE1345(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1348"://Sau khi di chuyển sẽ giải trừ hiệu quả pháo đài V3
                            _ = new AE1349(coldDown, probability, gameType, skillId, delay, "1349").Start(this);
                            _ = new AE1350(coldDown, probability, gameType, skillId, delay, "1350").Start(this);
                            break;
                        case "1355"://Mỗi lần bị tấn công trúng chính xác, nhận 40 sát thương thêm, tối đa cộng dồn 4 lần, sau khi turn bản thân kết thúc, giảm 2 lần hiệu quả thêm.
                            _ = new PE1355(coldDown, probability, gameType, skillId, delay, "1355").Start(this);
                            break;
                        case "1357"://Mỗi lần bị tấn công trúng chính xác, nhận 55 sát thương thêm, tối đa cộng dồn 6 lần, sau khi turn bản thân kết thúc, giảm 2 lần hiệu quả thêm.
                            _ = new PE1357(coldDown, probability, gameType, skillId, delay, "1357").Start(this);
                            break;
                        #endregion
                        #region Kiến
                        case "1032"://Mỗi lần bị tấn công giảm thêm 5% sát thương, duy trì 1 turn.
                            _ = new AE1445(coldDown, probability, gameType, skillId, delay, "1032").Start(this);
                            break;
                        case "1033"://Mỗi lần bị tấn công giảm thêm 5% sát thương, duy trì 1 turn.
                            _ = new AE1445(coldDown, probability, gameType, skillId, delay, "1033").Start(this);
                            break;
                        case "1034"://Mỗi lần bị tấn công giảm thêm 10% sát thương, duy trì 1 turn.
                            _ = new AE1446(coldDown, probability, gameType, skillId, delay, "1034").Start(this);
                            break;
                        case "1039":// gây 150% sát thương cơ bản
                            _ = new AE1039(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1067"://mỗi lần bị tấn công phản đòn bằng 30% tổng sát thương, duy trì 2 turn. Chỉ hiệu quả khi chiến đấu..
                            _ = new AE1067(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1068"://mỗi lần bị tấn công phản đòn bằng 50% tổng sát thương, duy trì 2 turn. Chỉ hiệu quả khi chiến đấu.
                            _ = new AE1068(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1117"://Hoàn toàn không chịu sát thương kéo dài 1 hiệp,chỉ khi đối chiến vối người mới có hiệu lực
                            _ = new AE1117(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1133":// gây 120% sát thương cơ bản
                            _ = new AE1133(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1134":// gây 180% sát thương cơ bản
                            _ = new AE1134(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1136"://Bản thân thêm vỏ phản xạ, duy trì 2 turn. Chỉ hiệu quả khi chiến đấu.
                            _ = new AE1136(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1137"://duy trì phản kích.
                            _ = new PE1137(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1439"://Ném 1 Kiến Lửa, bản thân tăng 100 hộ giáp, duy trì 2 turn.
                            _ = new AE1439(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1440"://Ném 1 Kiến Lửa, bản thân tăng 300 hộ giáp, duy trì 2 turn.
                            _ = new AE1440(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1441"://Ném 1 Kiến Lửa, bản thân tăng 500 hộ giáp, duy trì 2 turn.
                            _ = new AE1441(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1442"://Nhận được 500 điểm giảm thương.
                            _ = new AE1442(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1443"://Nhận được 500 +10% điểm giảm thương.
                            _ = new AE1443(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1444"://Nhận được 500 +20% điểm giảm thương.
                            _ = new AE1444(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1457"://Mỗi lần bị tấn công tăng 1 điểm ma pháp.
                            _ = new PE1457(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1449"://Tăng 6% hộ giáp.
                            _ = new PE1449(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1450"://Tăng 10% hộ giáp.
                            _ = new PE1450(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1451"://Tăng 6% phòng thủ.
                            _ = new PE1451(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1452"://Tăng 10% phòng thủ.
                            _ = new PE1452(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1455"://Hoàn toàn không chịu sát thương, hồi phục 5% HP, duy trì 1 turn. Chỉ hiệu quả khi chiến đấu.
                            _ = new AE1455(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1456"://Hoàn toàn không chịu sát thương, hồi phục 15% HP, duy trì 1 turn. Chỉ hiệu quả khi chiến đấu.
                            _ = new AE1456(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1459"://Mỗi lần bị tấn công có 20% xác suất phản đòn bằng 3% HP hiện tại..
                            _ = new PE1459(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1460"://Mỗi lần bị tấn công có 20% xác suất phản đòn bằng 5% HP hiện tại..
                            _ = new PE1460(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        #endregion
                        #region Đấu Sĩ
                        case "1022"://Bắn bất kỳ loại đạn nào trong TURN cũng sẽ tăng 100 hộ giáp cho đồng đội, duy trì 2 TURN.
                            _ = new AE1022(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1023"://Bắn bất kỳ loại đạn nào trong TURN cũng sẽ tăng 300 hộ giáp cho đồng đội, duy trì 2 TURN.
                            _ = new AE1023(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1024"://Bắn bất kỳ loại đạn nào trong TURN cũng sẽ tăng 100 sát thương cho đồng đội, duy trì 2 TURN.
                            _ = new AE1024(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1025"://Bắn bất kỳ loại đạn nào trong TURN cũng sẽ tăng 300 sát thương cho đồng đội, duy trì 2 TURN. 
                            _ = new AE1025(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1040"://tăng 100đ may mắn cho bản thân, duy trì 2 turn.
                            _ = new AE1040(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1041"://tăng 300đ may mắn cho bản thân, duy trì 2 turn.
                            _ = new AE1041(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1042"://tăng 500đ may mắn cho bản thân, duy trì 2 turn.
                            _ = new AE1042(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1056"://Hồi phục 1500 HP cho tất cả đồng đội trên toàn bản đồ. 
                            _ = new AE1056(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1057"://Hồi phục 3000 HP cho tất cả đồng đội trên toàn bản đồ.
                            _ = new AE1057(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1074"://Tăng 300 điểm hiệu quả cho các vũ khí phụ loại thiên sứ ban phúc.
                            _ = new PE1074(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1075"://Tăng 600 điểm hiệu quả cho các vũ khí phụ loại thiên sứ ban phúc.
                            _ = new PE1075(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1078"://Sử dụng vũ khí phụ loại khiên sẽ lập tức hồi phục 500 HP.
                            _ = new PE1078(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1079"://Sử dụng vũ khí phụ loại khiên sẽ lập tức hồi phục 1000 HP.
                            _ = new PE1079(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1092"://Tăng 100 công kích cho tất cả chiến hữu. Duy trì 3 TURN.
                            _ = new AE1092(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1093"://Tăng 300 công kích cho tất cả chiến hữu. Duy trì 3 TURN.
                            _ = new AE1093(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1094"://Tăng 100 phòng ngự cho tất cả chiến hữu. Duy trì 3 TURN.
                            _ = new AE1094(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1095"://Tăng 300 phòng ngự cho tất cả chiến hữu. Duy trì 3 TURN.
                            _ = new AE1095(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1096"://Tăng 100 nhanh nhẹn cho tất cả chiến hữu. Duy trì 3 TURN.
                            _ = new AE1096(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1097"://Tăng 300 nhanh nhẹn cho tất cả chiến hữu. Duy trì 3 TURN.
                            _ = new AE1097(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1098"://Tăng 100 may mắn cho tất cả chiến hữu. Duy trì 3 TURN.
                            _ = new AE1098(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1099"://Tăng 300 may mắn cho tất cả chiến hữu. Duy trì 3 TURN.
                            _ = new AE1099(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1100"://Tăng 1000 HP tối đa cho tất cả chiến hữu. Duy trì 3 TURN.
                            _ = new AE1100(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1101"://Tăng 2000 HP tối đa cho tất cả chiến hữu. Duy trì 3 TURN.
                            _ = new AE1101(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1107"://TURN đầu tiên sẽ nhận được 50 ma pháp.
                            _ = new PE1107(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1109"://Giải trừ 50 điểm phép thuật
                            _ = new PE1110(coldDown, probability, gameType, skillId, delay, "1110").Start(this);
                            break;
                        case "1122"://10% sat thương công kích
                            _ = new PE1122(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1123"://20% sat thương công kích
                            _ = new PE1123(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1124"://30% sat thương công kích
                            _ = new PE1124(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        #endregion
                        #region Rồng Cổ Đại
                        case "1139"://40% sat thương công kích
                            _ = new PE1139(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1149"://50% sat thương công kích
                            _ = new PE1149(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1150"://mục tiêu bị đánh trúng mỗi turn mất 1% HP, duy trì 3 turn. (Chỉ có hiệu quả khi chiến đấu)
                            _ = new AE1150(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1151"://mục tiêu bị đánh trúng mỗi turn mất 2% HP, duy trì 3 turn. (Chỉ có hiệu quả khi chiến đấu)
                            _ = new AE1151(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1152"://mục tiêu bị đánh trúng mỗi turn mất 3% HP, duy trì 3 turn. (Chỉ có hiệu quả khi chiến đấu)
                            _ = new AE1152(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1153"://Sát thương cơ bản +15%, duy trì 3 turn.
                            _ = new AE1153(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1154"://Sát thương cơ bản +25%, duy trì 3 turn.
                            _ = new AE1154(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1155"://hộ giáp giảm 500 điểm, duy trì 3 turn.
                            _ = new AE1155(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1156"://hộ giáp giảm 650 điểm, duy trì 3 turn.
                            _ = new AE1156(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1161"://Chân Long Tại Thiên, gây cho tất cả địch 3000 sát thương. (Chỉ có hiệu quả khi chiến đấu).
                            _ = new AE1161(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1162"://Chân Long Tại Thiên, gây cho tất cả địch 5000 sát thương. (Chỉ có hiệu quả khi chiến đấu).
                            _ = new AE1162(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1163"://Tấn công tăng 150.
                            _ = new PE1163(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1164"://Tấn công tăng 300..
                            _ = new PE1164(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1165"://sát thương tăng 100.
                            _ = new PE1165(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1166"://sát thương tăng 200.
                            _ = new PE1166(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1170"://Mỗi lần tới lượt địch tấn công, địch sẽ chịu bỏng Ấn Rồng Lửa! Mất 1000 HP, chỉ có hiệu quả khi chiến đấu. Duy trì 3 turn.
                            _ = new AE1170(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1171"://Mỗi lần tới lượt địch tấn công, địch sẽ chịu bỏng Ấn Rồng Lửa! Mất 2000 HP chỉ có hiệu quả khi chiến đấu. Duy trì 3 turn.
                            _ = new AE1171(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1172"://Mỗi lần bị tấn công, có xác suất 50% thức tỉnh Hồn Rồng hồi phục 2% HP. Duy trì 3 turn.
                            _ = new AE1172(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1173"://Mỗi lần bị tấn công, có xác suất 50% thức tỉnh Hồn Rồng hồi phục 4% HP. Duy trì 3 turn.
                            _ = new AE1173(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1174"://Rồng Bảo Vệ Lv1. Duy trì 3 turn.
                            _ = new AE1174(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1175"://Rồng Bảo Vệ Lv2. Duy trì 3 turn.
                            _ = new AE1175(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1176"://Mỗi lần tới lượt địch tấn công, địch sẽ chịu bỏng Ấn Rồng Lửa! Mất 2% HP hiện tại, chỉ có hiệu quả khi chiến đấu. Duy trì 3 turn.
                            _ = new AE1176(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1177"://Mỗi lần tới lượt địch tấn công, địch sẽ chịu bỏng Ấn Rồng Lửa! Mất 4% HP hiện tại, chỉ có hiệu quả khi chiến đấu. Duy trì 3 turn.
                            _ = new AE1177(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1322"://Khi chịu sát thương, mỗi mất 3500 HP nhận 1 điểm ma pháp.
                            _ = new PE1322(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1323"://Chân Long Tại Thiên, diệt nhanh địch đang có HP dưới 5%. (Chỉ có hiệu quả khi chiến đấu)
                            _ = new AE1323(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1324"://Chân Long Tại Thiên, diệt nhanh địch đang có HP dưới 10%. (Chỉ có hiệu quả khi chiến đấu)
                            _ = new AE1324(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        #endregion
                        #region Mầm Xanh
                        case "1358"://Ném 1 hạt giống, bản thân mỗi turn hồi phục 2% HP, duy trì 2 turn.
                            _ = new AE1358(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1359"://Ném 1 hạt giống, bản thân mỗi turn hồi phục 2% HP, duy trì 2 turn.
                            _ = new AE1359(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1360"://Ném 1 hạt giống, bản thân mỗi turn hồi phục 2% HP, duy trì 2 turn.
                            _ = new AE1360(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1361"://Bản thân và đơn vị xung quanh mỗi turn hồi phục 2% +800 HP, duy trì 3 turn
                            _ = new AE1361(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1362"://Bản thân và đơn vị xung quanh mỗi turn hồi phục 3% +1000 HP, duy trì 3 turn
                            _ = new AE1362(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1363"://Bản thân và đơn vị xung quanh mỗi turn hồi phục 3% +1500 HP, duy trì 4 turn
                            _ = new AE1363(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1364"://Đồng đội xung quanh hồi phục ngay 8% HP
                            _ = new AE1364(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1365"://Đồng đội xung quanh hồi phục ngay 10% HP
                            _ = new AE1365(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1366"://Đồng đội xung quanh mỗi turn hồi phục 3% HP, duy trì 3 turn.
                            _ = new AE1366(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1367"://Đồng đội xung quanh mỗi turn hồi phục 4% HP, duy trì 4 turn.
                            _ = new AE1367(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1368"://Thêm Khiên Phòng Hộ cho tất cả đồng đội, mỗi lần bị bắn trúng hồi phục 0.3% HP.
                            _ = new PE1368(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1369"://Thêm Khiên Phòng Hộ cho tất cả đồng đội, mỗi lần bị bắn trúng hồi phục 0.4% HP, nếu HP thấp hơn 20%, hiệu quả hồi phục x2.
                            _ = new PE1369(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1372"://Khi bắn vũ khí phụ loại thiên sứ ban phúc, kèm hiệu quả hồi phục bản thân 3% HP.
                            _ = new PE1372(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1373"://Khi bắn vũ khí phụ loại thiên sứ ban phúc, kèm hiệu quả hồi phục bản thân 6% HP.
                            _ = new PE1373(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1374"://Bắn 1 Hạt Bay, khi hạt giống nổ hồi phục HP đồng đội xung quanh, giảm HP địch, hạt giống mỗi giây tăng 3% HP..
                            _ = new AE1374(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1375"://Bắn 1 Hạt Bay, khi hạt giống nổ hồi phục HP đồng đội xung quanh, giảm HP địch, hạt giống mỗi giây tăng 6% HP..
                            _ = new AE1375(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1376"://Khi đồng đội bắt đầu turn nếu được kỹ năng Mầm Xanh duy trì hiệu quả hồi phục, bản thân sẽ tăng 1 điểm ma pháp.
                            _ = new PE1376(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        #endregion
                        #region Phụng Hoàng Băng
                        case "1178"://tăng 100đ tấn công cho bản thân, duy trì 2 turn.
                            _ = new AE1178(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1179"://tăng 300đ tấn công cho bản thân, duy trì 2 turn.
                            _ = new AE1179(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1180"://tăng 500đ tấn công cho bản thân, duy trì 2 turn.
                            _ = new AE1180(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1181"://Tăng 100 điểm hộ giáp cho tất cả đồng đội, duy trì 2 turn.Không cộng dồn để dùng.
                            _ = new AE1181(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1182"://Tăng 200 điểm hộ giáp cho tất cả đồng đội, duy trì 2 turn.Không cộng dồn để dùng.
                            _ = new AE1182(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1183"://Tăng 300 điểm hộ giáp cho tất cả đồng đội, duy trì 2 turn.Không cộng dồn để dùng.
                            _ = new AE1183(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1184"://tăng 150 điểm sát thương, di chuyển sẽ hủy. Khi HP không đủ, dùng kỹ năng này sẽ tử vong.
                            _ = new AE1184(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1185"://tăng 300 điểm sát thương, di chuyển sẽ hủy. Khi HP không đủ, dùng kỹ năng này sẽ tử vong.
                            _ = new AE1185(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1186"://Mỗi turn giảm 500 HP, di chuyển sẽ hủy. Khi HP không đủ, dùng kỹ năng này sẽ tử vong.
                            _ = new AE1186(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1187"://Mỗi turn giảm 800 HP, di chuyển sẽ hủy. Khi HP không đủ, dùng kỹ năng này sẽ tử vong.
                            _ = new AE1187(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1188"://Xóa hiệu ứng Địa Ngục Băng Giá
                            _ = new AE1189(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1190"://Tăng nhanh nhẹn 300 điểm.
                            _ = new PE1190(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1191"://Tăng nhanh nhẹn 500 điểm.
                            _ = new PE1191(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1192"://Tăng 100 điểm tấn công cho toàn bộ đồng đội.
                            _ = new PE1192(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1193"://Tăng 300 điểm tấn công cho toàn bộ đồng đội.
                            _ = new PE1193(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1194"://Tăng 200 điểm tấn công, duy trì 1 turn.
                            _ = new AE1194(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1195"://Tăng 300 điểm tấn công, duy trì 1 turn.
                            _ = new AE1195(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1196"://100 sát thương, duy trì 1 turn.
                            _ = new AE1196(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1197"://150 sát thương, duy trì 1 turn.
                            _ = new AE1197(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1198"://Tăng 30% crit, duy trì 1 turn.
                            _ = new AE1198(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1199"://Tăng 50% crit, duy trì 1 turn.
                            _ = new AE1199(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1200"://Khi đến lượt thi triển, tăng 2 điểm ma pháp cho toàn bộ thú cưng cùng phe.
                            _ = new PE1200(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1224"://Mỗi turn giảm 500 HP.
                            _ = new AE1224(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1225"://Mỗi turn giảm 800 HP.
                            _ = new AE1225(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        #endregion
                        #region Ma Xà
                        case "1201"://Bắn ra nọc độc, mục tiêu trúng phải giảm 100 sát thương, duy trì 3 turn.
                            _ = new AE1201(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1202"://Bắn ra nọc độc, mục tiêu trúng phải giảm 200 sát thương, duy trì 3 turn.
                            _ = new AE1202(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1203"://Bắn ra nọc độc, mục tiêu trúng phải giảm 300 sát thương, duy trì 3 turn.
                            _ = new AE1203(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1204"://Giảm 300 tấn công toàn bộ phe địch, duy trì 2 turn. Kỹ năng chỉ hiệu quả trong chiến đấu.
                            _ = new AE1204(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1205"://Giảm 500 tấn công toàn bộ phe địch, duy trì 2 turn. Kỹ năng chỉ hiệu quả trong chiến đấu.
                            _ = new AE1205(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1206"://Giảm 300 phòng thủ toàn bộ phe địch, duy trì 2 turn. Kỹ năng chỉ hiệu quả trong chiến đấu.
                            _ = new AE1206(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1207"://Giảm 500 phòng thủ toàn bộ phe địch, duy trì 2 turn. Kỹ năng chỉ hiệu quả trong chiến đấu.
                            _ = new AE1207(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1208"://Giảm 10 điểm ma pháp của tất cả thú cưng bên địch. Kỹ năng chỉ hiệu quả trong chiến đấu.
                            _ = new AE1208(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1209"://Giảm 30 điểm ma pháp của tất cả thú cưng bên địch. Kỹ năng chỉ hiệu quả trong chiến đấu.
                            _ = new AE1209(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1210"://Mỗi turn giảm 500 HP. Duy trì 3 turn.
                            _ = new AE1210(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1211"://Mỗi turn giảm 1000 HP. Duy trì 3 turn.
                            _ = new AE1211(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1212"://tăng 20% bạo kích. Duy trì 3 turn.
                            _ = new AE1212(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1213"://tăng 50% bạo kích. Duy trì 3 turn.
                            _ = new AE1213(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1214"://Tăng 100 hộ giáp
                            _ = new PE1214(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1215"://Tăng 200 hộ giáp
                            _ = new PE1215(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1216"://Tăng 1500 HP tối đa.
                            _ = new PE1216(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1217"://Tăng 3000 HP tối đa.
                            _ = new PE1217(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1218":
                        case "1219":
                            //empty skill element.
                            break;
                        case "1220"://Giảm 100 hộ giáp tất cả phe địch, duy trì 2 turn.
                            _ = new AE1220(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1221"://Giảm 200 hộ giáp tất cả phe địch, duy trì 2 turn.
                            _ = new AE1221(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1222"://Không thể di chuyển, duy trì 2 turn.
                            _ = new AE1222(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1223"://Mỗi lần bị tấn công nhận được 2 điểm ma pháp.
                            _ = new PE1223(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1226"://Mỗi turn giảm 500 HP.
                            _ = new AE1226(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1227"://Mỗi turn giảm 1000 HP.
                            _ = new AE1227(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        #endregion
                        #region Tôn Ngộ Không
                        case "1036":// gây 120% sát thương cơ bản
                            _ = new AE1036(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1418"://gây 150% sát thương cơ bản
                            _ = new AE1418(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1419"://gây 180% sát thương cơ bản
                            _ = new AE1419(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1421":// 20% xác suất bạo kích, 2 turn
                            _ = new AE1421(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1422"://5% Tăng Sát Thương
                            _ = new PE1422(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1423"://10% Giảm Hộ Giáp
                            _ = new PE1423(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1424"://10% Tăng Ma Công
                            _ = new PE1424(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1425"://15% Giảm Ma Kháng
                            _ = new PE1425(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1426"://Tất cả phe địch không thể hành động 1 turn. (Chỉ có hiệu quả khi chiến đấu)
                            _ = new AE1426(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1427"://ma pháp -10 điểm. (Chỉ có hiệu quả khi chiến đấu)
                            _ = new AE1427(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1428"://Giảm 10% sát thương phải chịu
                            _ = new PE1428(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1429"://Giảm 20% sát thương phải chịu
                            _ = new PE1429(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1430"://có 20% xác suất miễn bị bạo kích.
                            _ = new PE1430(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1431"://có 30% xác suất miễn bị bạo kích.
                            _ = new PE1431(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1432"://Đánh dấu tất cả người chơi phe địch ẩn thân, duy trì 2 turn (chỉ có hiệu quả khi chiến đấu)
                            _ = new AE1432(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1433"://khiến địch chịu thêm 20% sát thương, duy trì 2 turn (Chỉ có hiệu quả khi chiến đấu)
                            _ = new AE1433(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1435"://Mỗi lần chịu đòn trí mạng hồi phục 5% HP, mỗi trận tối đa kích hoạt 3 lần. (Chỉ có hiệu quả khi chiến đấu)
                            _ = new PE1435(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1436"://Vung Gậy Như Ý, gây cho địch ngẫu nhiên trong toàn màn hình 5 lần 100% sát thương phạm vi.
                            _ = new AE1436(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "2436"://Vung Gậy Như Ý, gây cho địch ngẫu nhiên trong toàn màn hình 5 lần 60% sát thương phạm vi.
                            _ = new AE1436(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "2438"://Mỗi lần dùng đạo cụ chiến đấu +1 ma pháp, mỗi turn tối đa +5 ma pháp.
                            _ = new PE2438(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        #endregion
                        #region Kungfu Đại Sư
                        case "1542": //Mỗi turn sau khi bắn, pet sẽ có sác xuất 100% tấn công kẻ địch, tạo 30% sát thương
                            _ = new PE1542(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1543": //Khi kết thúc turn, pet sẽ có sác xuất 100% tấn công kẻ địch, tạo 40% sát thương
                            _ = new PE1543(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1544": //Khi kết thúc turn, pet sẽ có sác xuất 100% tấn công kẻ địch, tạo 50% sát thương
                            _ = new PE1544(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1551": //Bạo Kích 100%
                            _ = new AE1551(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1548": //Tăng Sát Thương 150%
                            _ = new AE1548(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1549": //Tăng Sát Thương 180%
                            _ = new AE1549(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1545": //Ném 1 trứng bay, bắn trúng sẽ giảm 30% sát thương của mục tiêu
                            _ = new AE1545(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1546": //Ném 1 trứng bay, bắn trúng sẽ giảm 30% sát thương của mục tiêu
                            _ = new AE1546(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1547": //Ném 1 trứng bay, bắn trúng sẽ giảm 30% sát thương của mục tiêu
                            _ = new AE1547(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1552": //Sát thương bạo kích phải chịu -40%
                            _ = new AE1552(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1553": //Sát thương bạo kích phải chịu -60%
                            _ = new AE1553(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1554": //Sát thương phải chịu -20%
                            _ = new AE1554(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1555": //Sát thương phải chịu -20%
                            _ = new AE1555(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1556": //Thần Báo, nộ khí giảm 50, thể lực trong turn có Thần Báo giảm còn 120
                            _ = new AE1556(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1557": //Tăng 70% tốc độ tấn công
                            _ = new AE1557(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1560": //Di chuyển đến vị trí ngẫu nhiên giữa bản thân và kẻ địch, các vị trí khác sẽ xuất hiện phân thân
                            _ = new AE1560(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        case "1562": //Tất cả viên đạn bắn ra đều có hiệu quả Trói Buộc
                            _ = new AE1562(coldDown, probability, gameType, skillId, delay, element).Start(this);
                            break;
                        #endregion
                        #region Kungfu Đại Hiệp
                        #endregion
                        #region Chuột Thợ Mỏ
                        #endregion
                        default:
                            Console.WriteLine(string.Format("Not Found element: {0}, Pet name: {1}", element,
                                Pet.Name));
                            break;
                    }
                }
            }
        }

        public void InitFightBuffer(List<BufferInfo> buffers)
        {
            foreach (BufferInfo info in buffers)
            {
                switch (info.Type)
                {
                    case 101:
                        base.FightBuffers.ConsortionAddBloodGunCount = info.Value;
                        break;
                    case 102:
                        base.FightBuffers.ConsortionAddDamage = info.Value;
                        break;
                    case 103:
                        base.FightBuffers.ConsortionAddCritical = info.Value;
                        break;
                    case 104:
                        base.FightBuffers.ConsortionAddMaxBlood = info.Value;
                        break;
                    case 105:
                        base.FightBuffers.ConsortionAddProperty = info.Value;
                        break;
                    case 106:
                        base.FightBuffers.ConsortionReduceEnergyUse = info.Value;
                        break;
                    case 107:
                        base.FightBuffers.ConsortionAddEnergy = info.Value;
                        break;
                    case 108:
                        base.FightBuffers.ConsortionAddEffectTurn = info.Value;
                        break;
                    case 109:
                        base.FightBuffers.ConsortionAddOfferRate = info.Value;
                        break;
                    case 110:
                        base.FightBuffers.ConsortionAddPercentGoldOrGP = info.Value;
                        break;
                    case 111:
                        base.FightBuffers.ConsortionAddSpellCount = info.Value;
                        break;
                    case 112:
                        base.FightBuffers.ConsortionReduceDander = info.Value;
                        break;
                    case 400:
                        base.FightBuffers.WorldBossHP = info.Value;
                        break;
                    case 401:
                        base.FightBuffers.WorldBossAttrack = info.Value;
                        break;
                    case 402:
                        base.FightBuffers.WorldBossHP_MoneyBuff = info.Value;
                        break;
                    case 403:
                        base.FightBuffers.WorldBossAttrack_MoneyBuff = info.Value;
                        break;
                    case 404:
                        base.FightBuffers.WorldBossMetalSlug = info.Value;
                        break;
                    case 405:
                        base.FightBuffers.WorldBossAncientBlessings = info.Value;
                        break;
                    case 406:
                        base.FightBuffers.WorldBossAddDamage = info.Value;
                        break;
                }
            }
        }

        private bool CheckCondition(int condition)
        {
            if (base.Game is PVEGame)
            {
                PVEGame pve = base.Game as PVEGame;
                if (pve.Info.ID == 1 && (condition == 2 || condition == 3))
                {
                    return true;
                }
                if (pve.Info.ID == 2 && condition == 8)
                {
                    return true;
                }
                if (pve.Info.ID == 3 && (condition == 9 || condition == 10))
                {
                    return true;
                }
                if (pve.Info.ID == 4 && (condition == 4 || condition == 5))
                {
                    return true;
                }
                if (pve.Info.ID == 5 && (condition == 11 || condition == 12 || condition == 13))
                {
                    return true;
                }
                if (pve.Info.ID == 6 && (condition == 5 || condition == 16 || condition == 17))
                {
                    return true;
                }
                if (pve.Info.ID == 7 && (condition == 14 || condition == 15))
                {
                    return true;
                }
                if (pve.Info.ID == 11 && (condition == 7 || condition == 20))
                {
                    return true;
                }
                if (pve.Info.ID == 13 && (condition == 8 || condition == 21))
                {
                    return true;
                }
            }
            return base.Game is PVPGame;
        }

        public void InitCardBuffer(List<int> cards)
        {
            int minLv = 30;
            foreach (int card in cards)
            {
                if (card < 1100)
                {
                    minLv = card - 1000;
                }
            }
            int indexVal = 0;
            if (minLv >= 10)
            {
                indexVal = 1;
            }
            if (minLv >= 20)
            {
                indexVal = 2;
            }
            if (minLv >= 30)
            {
                indexVal = 3;
            }
            Dictionary<int, List<CardGroupInfo>> groups = CardBuffMgr.GetAllCard();
            List<CardBuffInfo> buffs = [];
            CardBuffInfo finalBuff = null;
            string msg = string.Empty;
            foreach (int key in groups.Keys)
            {
                int counter = 0;
                foreach (CardGroupInfo card2 in groups[key])
                {
                    foreach (int id in cards)
                    {
                        if (id == card2.TemplateID)
                        {
                            counter++;
                        }
                    }
                }
                buffs = CardBuffMgr.FindCardBuffs(key);
                if (buffs == null)
                {
                    continue;
                }
                foreach (CardBuffInfo buff5 in buffs)
                {
                    if (counter >= buff5.Condition)
                    {
                        CardInfo cardNotice = CardBuffMgr.FindCard(key);
                        if (cardNotice != null && CheckCondition(buff5.PropertiesDscripID))
                        {
                            msg = "<" + cardNotice.Name + "> Kart Seti kuşanımı başarılı! Tam olarak " + buff5.Condition + " adet kart özelliği aktive edildi!";
                            finalBuff = buff5;
                        }
                    }
                }
            }
            if (finalBuff == null)
            {
                return;
            }
            switch (finalBuff.CardID)
            {
                case 1:
                    _ = new AntCaveEffect(indexVal, finalBuff).Start(this);
                    break;
                case 2:
                    if (finalBuff.Condition >= 4 && buffs != null)
                    {
                        foreach (CardBuffInfo buff in buffs)
                        {
                            if (buff.Condition >= 4)
                            {
                                _ = new GuluKingdom4Effect(indexVal, buff).Start(this);
                            }
                            if (buff.Condition >= 2)
                            {
                                _ = new GuluKingdom2Effect(indexVal, buff).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 2)
                    {
                        _ = new GuluKingdom2Effect(indexVal, finalBuff).Start(this);
                    }
                    break;
                case 3:
                    if (finalBuff.Condition >= 5 && buffs != null)
                    {
                        foreach (CardBuffInfo buff2 in buffs)
                        {
                            if (buff2.Condition >= 5)
                            {
                                _ = new EvilTribe5Effect(indexVal, buff2).Start(this);
                            }
                            if (buff2.Condition >= 3)
                            {
                                _ = new EvilTribe3Effect(indexVal, buff2).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 3)
                    {
                        _ = new EvilTribe3Effect(indexVal, finalBuff).Start(this);
                    }
                    break;
                case 4:
                    if (finalBuff.Condition >= 4 && buffs != null)
                    {
                        foreach (CardBuffInfo buff3 in buffs)
                        {
                            if (buff3.Condition >= 4)
                            {
                                _ = new ShadowDevil4Effect(indexVal, buff3).Start(this);
                            }
                            if (buff3.Condition >= 2)
                            {
                                _ = new ShadowDevil2Effect(indexVal, buff3).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 2)
                    {
                        _ = new ShadowDevil2Effect(indexVal, finalBuff).Start(this);
                    }
                    break;
                case 5:
                    if (finalBuff.Condition >= 4)
                    {
                        foreach (CardBuffInfo buff4 in buffs)
                        {
                            if (buff4.Condition >= 4)
                            {
                                _ = new FourArtifacts4Effect(indexVal, buff4).Start(this);
                            }
                            if (buff4.Condition >= 2)
                            {
                                _ = new FourArtifacts2Effect(indexVal, buff4).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 2)
                    {
                        _ = new FourArtifacts2Effect(indexVal, finalBuff).Start(this);
                    }
                    break;
                case 6:
                    if (finalBuff.Condition >= 5 && buffs != null)
                    {
                        foreach (CardBuffInfo buff7 in buffs)
                        {
                            if (buff7.Condition >= 5)
                            {
                                _ = new Goblin5Effect(indexVal, buff7).Start(this);
                            }
                            if (buff7.Condition >= 4)
                            {
                                _ = new Goblin4Effect(indexVal, buff7).Start(this);
                            }
                            if (buff7.Condition >= 2)
                            {
                                _ = new Goblin2Effect(indexVal, buff7).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 4 && buffs != null)
                    {
                        foreach (CardBuffInfo buff6 in buffs)
                        {
                            if (buff6.Condition >= 4)
                            {
                                _ = new Goblin4Effect(indexVal, buff6).Start(this);
                            }
                            if (buff6.Condition >= 2)
                            {
                                _ = new Goblin2Effect(indexVal, buff6).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 2)
                    {
                        _ = new Goblin2Effect(indexVal, finalBuff).Start(this);
                    }
                    break;
                case 7:
                    if (finalBuff.Condition >= 4 && buffs != null)
                    {
                        foreach (CardBuffInfo buff8 in buffs)
                        {
                            if (buff8.Condition >= 4)
                            {
                                _ = new RunRunChicken4Effect(indexVal, buff8).Start(this);
                            }
                            if (buff8.Condition >= 2)
                            {
                                _ = new RunRunChicken2Effect(indexVal, buff8).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 2)
                    {
                        _ = new RunRunChicken2Effect(indexVal, finalBuff).Start(this);
                    }
                    break;
                case 8:
                    if (finalBuff.Condition >= 5 && buffs != null)
                    {
                        foreach (CardBuffInfo buff10 in buffs)
                        {
                            if (buff10.Condition >= 5)
                            {
                                _ = new GuluSportsMeeting5Effect(indexVal, buff10).Start(this);
                            }
                            if (buff10.Condition >= 4)
                            {
                                _ = new GuluSportsMeeting4Effect(indexVal, buff10).Start(this);
                            }
                            if (buff10.Condition >= 2)
                            {
                                _ = new GuluSportsMeeting2Effect(indexVal, buff10).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 4 && buffs != null)
                    {
                        foreach (CardBuffInfo buff9 in buffs)
                        {
                            if (buff9.Condition >= 4)
                            {
                                _ = new GuluSportsMeeting4Effect(indexVal, buff9).Start(this);
                            }
                            if (buff9.Condition >= 2)
                            {
                                _ = new GuluSportsMeeting2Effect(indexVal, buff9).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 2)
                    {
                        _ = new GuluSportsMeeting2Effect(indexVal, finalBuff).Start(this);
                    }
                    break;
                case 9:
                    if (finalBuff.Condition >= 5 && buffs != null)
                    {
                        foreach (CardBuffInfo buff11 in buffs)
                        {
                            if (buff11.Condition >= 5)
                            {
                                _ = new FiveGodSoldier5Effect(indexVal, buff11).Start(this);
                            }
                            if (buff11.Condition >= 2)
                            {
                                _ = new FiveGodSoldier2Effect(indexVal, buff11).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 2)
                    {
                        _ = new FiveGodSoldier2Effect(indexVal, finalBuff).Start(this);
                    }
                    break;
                case 10:
                    if (finalBuff.Condition >= 5 && buffs != null)
                    {
                        foreach (CardBuffInfo buff12 in buffs)
                        {
                            if (buff12.Condition >= 5)
                            {
                                _ = new TimeVortex5Effect(indexVal, buff12).Start(this);
                            }
                            if (buff12.Condition >= 3)
                            {
                                _ = new TimeVortex3Effect(indexVal, buff12).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 3)
                    {
                        _ = new TimeVortex3Effect(indexVal, finalBuff).Start(this);
                    }
                    break;
                case 11:
                    if (finalBuff.Condition >= 5 && buffs != null)
                    {
                        foreach (CardBuffInfo buff13 in buffs)
                        {
                            if (buff13.Condition >= 5)
                            {
                                _ = new WarriorsArena5Effect(indexVal, buff13).Start(this);
                            }
                            if (buff13.Condition >= 3)
                            {
                                _ = new WarriorsArena3Effect(indexVal, buff13).Start(this);
                            }
                        }
                    }
                    if (finalBuff.Condition >= 3)
                    {
                        _ = new WarriorsArena3Effect(indexVal, finalBuff).Start(this);
                    }
                    break;
                case 12:
                    _ = new PioneerEffect(indexVal, finalBuff).Start(this);
                    break;
                case 13:
                    _ = new WeaponMasterEffect(indexVal, finalBuff).Start(this);
                    break;
                case 14:
                    _ = new DivineEffect(indexVal, finalBuff).Start(this);
                    break;
                case 15:
                    _ = new LuckyEffect(indexVal, finalBuff).Start(this);
                    break;
            }
            if (!string.IsNullOrEmpty(msg))
            {
                if (base.Game is PVEGame)
                {
                    PlayerDetail.SendMessage(msg);
                }
                else
                {
                    PlayerDetail.SendMessage(msg);
                }
            }

        }

        #region gereksiz
        public bool IsCure()
        {
            return Weapon.TemplateID switch
            {
                17000 or 17001 or 17002 or 17005 or 17007 or 17010 or 17100 or 17102 => true,
                _ => false,
            };
        }
        #endregion
        public void CalculatePlayerOffer(Player player)
        {
            if (m_game.RoomType == eRoomType.Match && (m_game.GameType == eGameType.Guild || m_game.GameType == eGameType.Free) && !player.IsLiving)
            {
                int robOffer = (base.Game.GameType == eGameType.Guild) ? 10 : ((PlayerDetail.PlayerCharacter.ConsortiaID == 0 || player.PlayerDetail.PlayerCharacter.ConsortiaID == 0) ? 1 : 3);
                if (robOffer > player.PlayerDetail.PlayerCharacter.Offer)
                {
                    robOffer = player.PlayerDetail.PlayerCharacter.Offer;
                }
                robOffer += TotalHurt / 2000;
                if (robOffer > 0)
                {
                    GainOffer += robOffer;
                    player.KilledPunishmentOffer = robOffer;
                }
            }
        }

        public override void OnAfterKillingLiving(Living target, int damageAmount, int criticalAmount)
        {
            base.OnAfterKillingLiving(target, damageAmount, criticalAmount);
            if (target is Player)
            {
                PlayerDetail.OnKillingLiving(m_game, 1, target.Id, target.IsLiving, damageAmount + criticalAmount);
                CalculatePlayerOffer(target as Player);
                return;
            }
            int id = 0;
            if (target is SimpleBoss)
            {
                id = (target as SimpleBoss).NpcInfo.ID;
            }
            if (target is SimpleNpc)
            {
                id = (target as SimpleNpc).NpcInfo.ID;
            }
            PlayerDetail.OnKillingLiving(m_game, 2, id, target.IsLiving, damageAmount + criticalAmount);
        }

        protected void OnAfterPlayerShoot()
        {
            m_useitemCount = 9999;
            AfterPlayerShooted?.Invoke(this);
        }

        protected void OnBeforePlayerShoot()
        {
            BeforePlayerShoot?.Invoke(this);
        }

        protected void OnCollidedByObject()
        {
            CollidByObject?.Invoke(this);
        }

        protected void OnLoadingCompleted()
        {
            LoadingCompleted?.Invoke(this);
        }

        public void OnPlayerBuffSkillPet()
        {
            PlayerBuffSkillPet?.Invoke(this);
        }

        public void OnPlayerClearBuffSkillPet()
        {
            PlayerClearBuffSkillPet?.Invoke(this);
        }

        public void OnPlayerCure()
        {
            PlayerCure?.Invoke(this);
        }

        public void OnPlayerGuard()
        {
            PlayerGuard?.Invoke(this);
        }

        public void OnPlayerShootCure()
        {
            PlayerShootCure?.Invoke(this);
        }

        protected void OnPlayerMoving()
        {
            PlayerBeginMoving?.Invoke(this);
        }

        public void OnPlayerShoot()
        {
            PlayerShoot?.Invoke(this);
        }

        protected void OnPlayerCompleteShoot()
        {
            PlayerCompleteShoot?.Invoke(this);
        }

        public void OnPlayerAnyShellThrow()
        {
            PlayerAnyShellThrow?.Invoke(this);
        }

        public event PlayerEventHandle PlayerAfterBuffSkillPet;

        public void OnPlayerAfterBuffSkillPet()
        {
            PlayerAfterBuffSkillPet?.Invoke(this);
        }

        public void OnPlayerUseSecondWeapon(int type)
        {
            PlayerUseSecondWeapon?.Invoke(this, type);
        }

        public void OnPlayerBeforeReset()
        {
            PlayerBeforeReset?.Invoke(this);
        }

        public void OnPlayerAfterReset()
        {
            PlayerAfterReset?.Invoke(this);
        }

        public void OpenBox(int boxId)
        {
            Box box = null;
            foreach (Box box2 in m_tempBoxes)
            {
                if (box2.Id == boxId)
                {
                    box = box2;
                    break;
                }
            }
            if (box == null || box.Item == null)
            {
                return;
            }
            ItemInfo item = box.Item;
            switch (item.TemplateID)
            {
                case -1100:
                    _ = PlayerDetail.AddGiftToken(item.Count);
                    break;
                case -800:
                    _ = PlayerDetail.AddHonor(item.Count);
                    break;
                case -200:
                    _ = PlayerDetail.AddMoney(item.Count, igroneAll: false);
                    PlayerDetail.LogAddMoney(AddMoneyType.Box, AddMoneyType.Box_Open, PlayerDetail.PlayerCharacter.ID, item.Count, PlayerDetail.PlayerCharacter.Money);
                    break;
                case -100:
                    _ = PlayerDetail.AddGold(item.Count);
                    break;
                default:
                    if (item.Template.CategoryID == 10)
                    {
                        if (!PlayerDetail.AddTemplate(item, eBageType.FightBag, item.Count, eGameView.RouletteTypeGet))
                        {
                        }
                    }
                    else
                    {
                        _ = PlayerDetail.AddTemplate(item, eBageType.TempBag, item.Count, eGameView.dungeonTypeGet);
                    }
                    break;
            }
            m_tempBoxes.Remove(box);
        }

        public override void PickBox(Box box)
        {
            _ = m_tempBoxes.Add(box);
            base.PickBox(box);
        }

        public override void PrepareNewTurn() //buradaki pet olaylarını iyice bi incelememiz lazım. not:yuti
        {
            ItemFightBag.Clear();
            if (CurrentIsHitTarget)
            {
                TotalHitTargetCount++;
            }
            Energy = ((int)Agility / 30) + 240;
            //m_useitemCount = 0;
            if (base.FightBuffers.ConsortionAddEnergy > 0)
            {
                Energy += base.FightBuffers.ConsortionAddEnergy;
            }
            base.PetEffects.CurrentUseSkill = 0;
            base.PetEffects.PetDelay = 0;
            base.SpecialSkillDelay = 0;
            PetEffectTrigger = false;
            base.SpecialSkillDelay = 0;
            m_shootCount = 1;
            m_ballCount = 1;
            AttackInformation = true;
            DefenceInformation = true;
            EffectTrigger = false;
            PetEffectTrigger = false;
            PetEffects.DisibleActiveSkill = false;
            flyCount--;
            SetCurrentWeapon(PlayerDetail.MainWeapon);
            if (CurrentBall.ID != m_mainBallId)
            {
                CurrentBall = BallMgr.FindBall(m_mainBallId);
            }
            if (!base.IsLiving)
            {
                StartGhostMoving();
                TargetPoint = Point.Empty;
            }
            if (!base.PetEffects.StopMoving)
            {
                base.SpeedMultX(3);
            }
            CanFly = true;
            CurrentDelay = 0;
            base.PrepareNewTurn();
        }

        public override void PrepareSelfTurn()
        {
            base.PrepareSelfTurn();
            m_useitemCount = 0;
            DefaultDelay = m_delay;
            flyCount--;
            m_game.SendRoundOneEnd(this);
            if (Pet == null)
            {
                return;
            }
            foreach (int skillId in PetSkillCD.Keys)
            {
                if (PetSkillCD[skillId].Turn > 0)
                {
                    PetSkillCD[skillId].Turn--;
                }
            }
        }

        public void PrepareShoot(byte speedTime)
        {
            int turnWaitTime = m_game.GetTurnWaitTime();
            _ = (speedTime > turnWaitTime) ? turnWaitTime : speedTime;
            //AddDelay(num2 * 20);
            AddDelay(50);
            TotalShootCount++;
        }

        public bool ReduceEnergy(int value)
        {
            if (value > Energy)
            {
                value = Energy;
            }
            Energy -= value;
            return true;
        }

        public void ResetSkillCd()
        {
            if (Pet == null)
            {
                return;
            }
            string[] listSkills = Pet.SkillEquip.Split('|');
            string[] array = listSkills;
            foreach (string skill in array)
            {
                int skillId = int.Parse(skill.Split(',')[0]);
                if (PetSkillCD.ContainsKey(skillId))
                {
                    PetSkillCD[skillId].Turn = PetSkillCD[skillId].ColdDown;
                }
            }
        }

        public override void Reset()
        {
            m_game.Cards = m_game.RoomType == eRoomType.Dungeon ? (new int[21]) : (new int[9]);
            base.EffectList.StopAllEffect();
            base.CardEffectList.StopAllEffect();
            //base.Dander = 0;
            //base.PetMP = 0;
            Dander = (m_game.RoomType == eRoomType.ConsortiaBattle && PlayerDetail.PlayerCharacter.ActivePowFirstGame) ? 200 : 0;
            base.PetMP = 10;
            base.psychic = 00;
            base.IsLiving = true;
            FinishTakeCard = false;
            base.VaneOpen = base.AutoBoot || PlayerDetail.PlayerCharacter.Grade >= 9;
            InitFightBuffer(PlayerDetail.FightBuffs);
            InitCardBuffer(PlayerDetail.CardBuff);
            m_Healstone = PlayerDetail.Healstone;
            ChangeSpecialBall = 0;
            DeputyWeapon = PlayerDetail.SecondWeapon;
            Weapon = PlayerDetail.MainWeapon;
            BallConfigInfo info = BallConfigMgr.FindBall(Weapon.TemplateID);
            m_mainBallId = info.Common;
            m_spBallId = info.Special;
            m_AddWoundBallId = info.CommonAddWound;
            m_MultiBallId = info.CommonMultiBall;
            BaseDamage = PlayerDetail.GetBaseAttack();
            BaseGuard = PlayerDetail.GetBaseDefence();
            Attack = PlayerDetail.PlayerCharacter.Attack;
            Defence = PlayerDetail.PlayerCharacter.Defence;
            Agility = PlayerDetail.PlayerCharacter.Agility;
            Lucky = PlayerDetail.PlayerCharacter.Luck;
            m_maxBlood = PlayerDetail.PlayerCharacter.hp;
            BaseDamage += PlayerDetail.PlayerCharacter.DameAddPlus;
            OnPlayerBeforeReset();
            if (base.FightBuffers.ConsortionAddDamage > 0)
            {
                BaseDamage += base.FightBuffers.ConsortionAddDamage;
            }
            //AddPlus from Consortia
            if (m_game.RoomType == eRoomType.ConsortiaBattle)
            {
                Attack += Attack / 100 * PlayerDetail.PlayerCharacter.AttPlusGuildBattle;
                Agility += Agility / 100 * PlayerDetail.PlayerCharacter.AgiPlusGuildBattle;
            }
            BaseGuard += PlayerDetail.PlayerCharacter.GuardAddPlus;
            Attack += PlayerDetail.PlayerCharacter.AttackAddPlus;
            Defence += PlayerDetail.PlayerCharacter.DefendAddPlus;
            Agility += PlayerDetail.PlayerCharacter.AgiAddPlus;
            Lucky += PlayerDetail.PlayerCharacter.LuckAddPlus;
            Attack += PlayerDetail.PlayerCharacter.StrengthEnchance;
            Defence += PlayerDetail.PlayerCharacter.StrengthEnchance;
            Agility += PlayerDetail.PlayerCharacter.StrengthEnchance;
            Lucky += PlayerDetail.PlayerCharacter.StrengthEnchance;
            if (base.FightBuffers.ConsortionAddMaxBlood > 0)
            {
                m_maxBlood += m_maxBlood * base.FightBuffers.ConsortionAddMaxBlood / 100;
            }
            m_maxBlood += PlayerDetail.PlayerCharacter.HpAddPlus + base.PetEffects.MaxBlood + FightBuffers.WorldBossHP;
            if (m_bufferPoint != null)
            {
                Attack += Attack / 100.0 * m_bufferPoint.Value;
                Defence += Defence / 100.0 * m_bufferPoint.Value;
                Agility += Agility / 100.0 * m_bufferPoint.Value;
                Lucky += Lucky / 100.0 * m_bufferPoint.Value;
            }
            if (base.FightBuffers.ConsortionAddProperty > 0)
            {
                Attack += base.FightBuffers.ConsortionAddProperty;
                Defence += base.FightBuffers.ConsortionAddProperty;
                Agility += base.FightBuffers.ConsortionAddProperty;
                Lucky += base.FightBuffers.ConsortionAddProperty;
            }
            Energy = ((int)Agility / 30) + 240;
            m_useitemCount = 0;
            if (base.FightBuffers.ConsortionAddEnergy > 0)
            {
                Energy += base.FightBuffers.ConsortionAddEnergy;
            }
            if (petFightPropertyInfo != null)
            {
                Attack += petFightPropertyInfo.Attack;
                Defence += petFightPropertyInfo.Defence;
                Agility += petFightPropertyInfo.Agility;
                Lucky += petFightPropertyInfo.Lucky;
                m_maxBlood += petFightPropertyInfo.Blood;
            }
            m_maxBlood += PetEffects == null ? 0 : PetEffects.AddMaxBloodValue;
            CurrentBall = BallMgr.FindBall(m_mainBallId);
            m_shootCount = 1;
            m_ballCount = 1;
            CurrentIsHitTarget = false;
            LimitEnergy = false;
            TotalCure = 0;
            TotalHitTargetCount = 0;
            TotalHurt = 0;
            TotalKill = 0;
            TotalShootCount = 0;
            LockDirection = false;
            GainGP = 0;
            GainOffer = 0;
            Ready = false;
            _ = PlayerDetail.ClearTempBag();
            LoadingProcess = 0;
            base.PetEffects.CritRate = 0;
            KilledPunishmentOffer = 0;
            Prop = 0;
            InitBuffer(PlayerDetail.EquipEffect);
            CanFly = true;
            deputyWeaponCount = DeputyWeapon != null ? DeputyWeapon.StrengthenLevel + 1 : 1;
            ResetSkillCd();
            OnPlayerAfterReset();
            PowerRatio = 100;
            IsBombOrIgnoreAemor = 0;
            Config = new PlayerConfig();
            base.Reset();
        }

        public virtual int AddMaxBlood(int value)
        {
            if (value != 0)
            {
                base.MaxBlood += value;
            }
            return value;
        }

        public void SetBall(int ballId)
        {
            SetBall(ballId, special: false);
        }

        public void SetBall(int ballId, bool special)
        {
            if (ballId != CurrentBall.ID)
            {
                if (BallMgr.FindBall(ballId) != null)
                {
                    CurrentBall = BallMgr.FindBall(ballId);
                }
                m_game.SendGameUpdateBall(this, special);
            }
        }

        public void SetCurrentWeapon(ItemInfo item)
        {
            Weapon = item;
            BallConfigInfo info = BallConfigMgr.FindBall(Weapon.TemplateID);
            if (Weapon.isGold)
            {
                info = BallConfigMgr.FindBall(Weapon.GoldEquip.TemplateID);
            }
            if (ChangeSpecialBall > 0)
            {
                info = BallConfigMgr.FindBall(70396);
            }
            m_mainBallId = info.Common;
            m_spBallId = info.Special;
            m_AddWoundBallId = info.CommonAddWound;
            m_MultiBallId = info.CommonMultiBall;
            SetBall(m_mainBallId);
        }

        public override void SetXY(int x, int y)
        {
            if (m_x == x && m_y == y)
            {
                return;
            }
            int value = Math.Abs(m_x - x);
            m_x = x;
            m_y = y;
            if (base.IsLiving && !LimitEnergy)
            {
                Energy -= Math.Abs(m_x - x);
                if (value > 0)
                {
                    OnPlayerMoving();
                }
                return;
            }
            Rectangle rect = m_rect;
            rect.Offset(m_x, m_y);
            Physics[] array = m_map.FindPhysicalObjects(rect, this);
            Physics[] array2 = array;
            Physics[] array3 = array2;
            foreach (Physics physics in array3)
            {
                if (physics is Box)
                {
                    Box box = physics as Box;
                    PickBox(box);
                    base.Game.CheckBox();
                }
            }
        }

        public bool Shoot(int x, int y, int force, int angle)
        {
            if (m_game.FreeFatal && PlayerDetail.PlayerCharacter.Grade <= 9)
            {
                _ = new FatalEffect(0, 15112004).Start(this);
            }
            if (m_shootCount == 1)
            {
                base.PetEffects.ActivePetHit = true;
            }
            if (m_shootCount > 0)
            {
                EffectTrigger = false;
                OnPlayerShoot();
                int iD = CurrentBall.ID;
                if (m_ballCount == 1 && !IsSpecialSkill && IsBombOrIgnoreAemor == 0)
                {
                    if (Prop == 20002)
                    {
                        iD = m_MultiBallId;
                    }
                    if (Prop == 20008)
                    {
                        iD = m_AddWoundBallId;
                    }
                }
                OnPlayerAnyShellThrow();
                OnBeforePlayerShoot();
                if (IsSpecialSkill) //pow 
                {
                    //ControlBall = false;
                    base.SpecialSkillDelay = 2000;
                }
                int tmpID = iD;
                if (IsBombOrIgnoreAemor > 0)
                {
                    if (IsBombOrIgnoreAemor == 1)
                    {
                        IgnoreArmor = true; //zırh delici
                    }
                    else if (IsBombOrIgnoreAemor == 2)
                    {
                        iD = 4;
                    }
                }
                if (BallMgr.GetBallType(iD) == BombType.CURE) //melek
                {
                    m_ballCount = 1;
                    ShootCount = 1;
                }
                //
                //
                if (ShootImp(iD, x, y, force, angle, m_ballCount, ShootCount))
                {
                    if (iD == 4)
                    {
                        m_game.AddAction(new FightAchievementAction(this, eFightAchievementType.SuperMansNuclearExplosion, base.Direction, 1200));
                    }
                    if (IsBombOrIgnoreAemor > 0)
                    {
                        if (IsBombOrIgnoreAemor == 1)//xuyên
                        {
                            IgnoreArmor = false;
                        }
                        else if (IsBombOrIgnoreAemor == 2)//hạt nhân
                        {
                        }
                        IsBombOrIgnoreAemor = 0;
                    }
                    //
                    m_shootCount--;
                    if (m_shootCount <= 0 || !base.IsLiving)
                    {
                        CurrentDelay += CurrentBall.Delay + (Weapon.isGold ? Weapon.GoldEquip.Property8 : Weapon.Template.Property8);
                        StopAttacking();
                        //AddDelay(m_currentBall.Delay + (m_weapon.isGold ? m_weapon.GoldEquip.Property8 : m_weapon.Template.Property8));
                        AddDander(20);
                        AddPetMP(10);
                        Prop = 0;
                        if (CanGetProp)
                        {
                            int gold = 0;
                            int money = 0;
                            int giftToken = 0;
                            int medal = 0;
                            int honor = 0;
                            int hardCurrency = 0;
                            int token = 0;
                            int dragonToken = 0;
                            int magicStonePoint = 0;
                            List<ItemInfo> list = null;
                            if (DropInventory.FireDrop(m_game.RoomType, ref list) && list != null)
                            {
                                foreach (ItemInfo info in list)
                                {
                                    ShopMgr.FindSpecialItemInfo(info, ref gold, ref money, ref giftToken, ref medal, ref honor, ref hardCurrency, ref token, ref dragonToken, ref magicStonePoint);
                                    if (info == null || !base.VaneOpen || info.TemplateID <= 0)
                                    {
                                        continue;
                                    }
                                    if (info.Template.CategoryID == 10)
                                    {
                                        if (!PlayerDetail.AddTemplate(info, eBageType.FightBag, info.Count, eGameView.RouletteTypeGet))
                                        {
                                        }
                                    }
                                    else
                                    {
                                        _ = PlayerDetail.AddTemplate(info, eBageType.TempBag, info.Count, eGameView.dungeonTypeGet);
                                    }
                                }
                                _ = PlayerDetail.AddGold(gold);
                                _ = PlayerDetail.AddMoney(money, igroneAll: false);
                                PlayerDetail.LogAddMoney(AddMoneyType.Game, AddMoneyType.Game_Shoot, PlayerDetail.PlayerCharacter.ID, money, PlayerDetail.PlayerCharacter.Money);
                                _ = PlayerDetail.AddGiftToken(giftToken);
                                _ = PlayerDetail.AddHonor(honor);
                            }
                        }
                        OnPlayerCompleteShoot();
                    }
                    //SendAttackInformation();
                    OnAfterPlayerShoot();
                    return true;
                }
            }
            return false;
        }

        public override void Skip(int spendTime)
        {
            if (base.IsAttacking)
            {
                base.Game.SendSkipNext(this);
                Prop = 0;
                //AddDelay(25);
                AddDelay(CurrentDelay / 100 * 70);
                //AddDelay(-200);
                CurrentDelay = 0;
                AddDander(40);
                AddPetMP(10);
                OnPlayerSkip();
                base.Skip(spendTime);
            }
        }

        public void PetUseKill(int skillId, int type)
        {
            //Console.WriteLine("PetUseKill skillID:{0}, type:{1}", skillId, type);
            if (CanUseSkill(skillId) && PetSkillCD.ContainsKey(skillId) && !PetEffects.DisibleActiveSkill)
            {
                PetSkillInfo skillInfo = PetSkillCD[skillId];
                if (skillInfo.NewBallID != -1 && m_useitemCount > 0)
                {
                    PlayerDetail.SendMessage("Aksesuar kullandığınız için bu skill basılamaz.");
                    return;
                }
                if (PetMP > 0 && PetMP >= skillInfo.CostMP)
                {
                    if (GetSealStatePet())
                    {
                        PlayerDetail.SendMessage(LanguageMgr.GetTranslation("Player.Msg1a"));
                    }
                    else
                    {
                        m_useitemCount = 9999;
                        if (skillInfo.NewBallID != -1)
                        {
                            PetEffects.Delay += skillInfo.Delay;
                            SetBall(skillInfo.NewBallID);
                        }
                        PetMP -= skillInfo.CostMP;

                        PetEffects.CurrentUseSkill = skillId;
                        PetEffects.BallType = skillInfo.BallType;
                        m_game.SendPetUseKill(this, type);

                        OnPlayerBuffSkillPet();

                        skillInfo.Turn = skillInfo.ColdDown + 1;
                        OnPlayerAfterBuffSkillPet();
                    }
                }
                else
                {
                    PlayerDetail.SendMessage(LanguageMgr.GetTranslation("Player.Msg1"));
                }
            }
        }

        public bool CanUseSkill(int Id)
        {
            if (m_useitemCount >= 999)
            {
                return false;
            }
            if (Pet != null)
            {
                string[] array = Pet.SkillEquip.Split('|');
                for (int i = 0; i < array.Length; i++)
                {
                    if (int.Parse(array[i].Split(',')[0]) == Id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void StartAttacking()
        {
            if (base.IsAttacking)
            {
                return;
            }
            if (m_Healstone != null && m_blood < m_maxBlood && !base.Game.IsSpecialPVE() && PlayerDetail.RemoveHealstone())
            {
                int property2 = m_Healstone.Template.Property2;
                BufferInfo fightBuffByType = GetFightBuffByType(BuffType.ReHealth);
                if (fightBuffByType != null && PlayerDetail.UsePayBuff(BuffType.ReHealth))
                {
                    property2 *= fightBuffByType.Value;
                }
                _ = AddBlood(property2);
            }
            //AddDelay(GetTurnDelay());
            CurrentDelay += GetTurnDelay();
            base.StartAttacking();
        }
        public override void StopAttacking()
        {
            AddDelay(CurrentDelay);
            base.StopAttacking();
        }
        public BufferInfo GetFightBuffByType(BuffType buff)
        {
            foreach (BufferInfo fightBuff in PlayerDetail.FightBuffs)
            {
                if (fightBuff.Type == (int)buff)
                {
                    return fightBuff;
                }
            }
            return null;
        }

        public void SendAttackInformation()
        {
            if (EffectTrigger && AttackInformation)
            {
                EffectTrigger = false;
                AttackInformation = false;
            }
        }

        public void StartGhostMoving()
        {
            if (!TargetPoint.IsEmpty)
            {
                Point point = new(TargetPoint.X - X, TargetPoint.Y - Y);
                if (point.Length() > 160.0)
                {
                    _ = point.Normalize(160);
                }
                m_game.AddAction(new GhostMoveAction(this, new Point(X + point.X, Y + point.Y)));
            }
        }

        public override void StartMoving()
        {
            if (m_map == null)
            {
                return;
            }
            Point point = m_map.FindYLineNotEmptyPointDown(m_x, m_y);
            if (point.IsEmpty)
            {
                if (m_map.Ground != null)
                {
                    m_y = m_map.Ground.Height;
                }
            }
            else
            {
                m_x = point.X;
                m_y = point.Y;
            }
            if (point.IsEmpty)
            {
                m_syncAtTime = false;
                Die();
            }
        }

        public override void StartMoving(int delay, int speed)
        {
            if (m_map != null)
            {
                Point point = m_map.FindYLineNotEmptyPointDown(m_x, m_y);
                if (point.IsEmpty)
                {
                    m_y = m_map.Ground.Height;
                }
                else
                {
                    m_x = point.X;
                    m_y = point.Y;
                }
                base.StartMoving(delay, speed);
                if (point.IsEmpty)
                {
                    m_syncAtTime = false;
                    Die();
                }
            }
        }

        public void StartSpeedMult(int x, int y, int delay)
        {
            Point point = new(x - X, y - Y);
            m_game.AddAction(new PlayerSpeedMultAction(this, new Point(X + point.X, Y + point.Y), delay));
        }

        public void StartSpeedMult(int x, int y)
        {
            StartSpeedMult(x, y, 3000);
        }



        //public override bool TakeDamage(Living source, ref int damageAmount, ref int criticalAmount, string msg)
        //{
        //    if ((source == this || source.Team == base.Team) && damageAmount + criticalAmount >= m_blood)
        //    {
        //        damageAmount = m_blood - 1;
        //        criticalAmount = 0;
        //    }
        //    bool flag = base.TakeDamage(source, ref damageAmount, ref criticalAmount, msg);
        //    if (base.IsLiving)
        //    {
        //        //int currDander = MaxBlood / 1000 * 4;
        //        //int currDamage = LastBlood - Blood > 0 ? LastBlood - Blood : 1;
        //        //AddDander(currDamage / currDander * 2 > 0 ? currDamage / currDander * 2 : 1);
        //        int currDander = MaxBlood / 1000 * 4;
        //        int currDamage = LastBlood - Blood > 0 ? LastBlood - Blood : 1;
        //        AddDander(currDamage / Math.Max(currDander, 1) * 2 > 0 ? currDamage / Math.Max(currDander, 1) * 2 : 1);
        //        if (!base.Game.IsSpecialPVE() && base.Blood < base.MaxBlood / 100 * 30)
        //        {
        //            BufferInfo fightBuffByType = GetFightBuffByType(BuffType.Save_Life);
        //            if (fightBuffByType != null && m_player.UsePayBuff(BuffType.Save_Life))
        //            {
        //                int num = base.MaxBlood / 100 * fightBuffByType.Value;
        //                AddBlood(num);
        //                m_game.method_53(this, LanguageMgr.GetTranslation("GameServer.PayBuff.ReLife.UseNotice", PlayerDetail.PlayerCharacter.NickName, num));
        //            }
        //        }
        //    }
        //    return flag;
        //}

        public override bool TakeDamage(Living source, ref int damageAmount, ref int criticalAmount, string msg)
        {
            // Eğer saldıran kendisi veya takım arkadaşıysa:
            if (source == this || source.Team == base.Team)
            {
                // Hasarı ve kritik hasarı sıfırla
                damageAmount = 0;
                criticalAmount = 0;

                // İşlemi burada bitir (base.TakeDamage'ı 0 hasarla çağırıp sonucu döndür).
                // Böylece aşağıdaki Dander (Öfke) veya Buff işlemleri çalışmaz.
                return base.TakeDamage(source, ref damageAmount, ref criticalAmount, msg);
            }

            bool result = base.TakeDamage(source, ref damageAmount, ref criticalAmount, msg);
            if (base.IsLiving)
            {
                int currDamage = Math.Max(LastBlood - Blood, 0);

                // ZORLUK AYARI 1: Hasar, maksimum canın %5'inden az ise öfke kazanmasın.
                if (currDamage > MaxBlood / 0.0001)
                {
                    // ZORLUK AYARI 2: Öfke kazanım formülünü zorlaştırdık.
                    // Eski Kod: currDamage / currDander * 2 (Çok hızlı doluyordu)
                    // Yeni Kod: Bölümü büyütüp (MaxBlood / 100) çarpanı kaldırdık.
                    // Artık aynı hasar için çok daha az öfke puanı kazanacak.
                    int currDander = Math.Max(MaxBlood / 100, 1);

                    // En az 1 birim kazanması için Math.Max kullanıyoruz.
                    AddDander(Math.Max(currDamage / currDander, 1));
                }
                if (!base.Game.IsSpecialPVE() && base.Blood < base.MaxBlood / 100 * 30)
                {
                    BufferInfo fightBuffByType = GetFightBuffByType(BuffType.Save_Life);
                    if (fightBuffByType != null && PlayerDetail.UsePayBuff(BuffType.Save_Life))
                    {
                        int num = base.MaxBlood / 100 * fightBuffByType.Value;
                        _ = AddBlood(num);
                        m_game.method_53(this, LanguageMgr.GetTranslation("Oyuncu " + PlayerDetail.PlayerCharacter.NickName + " Kurtarma Samanını kullandı ve " + num + " canını yeniledi!"));
                    }
                }
            }
            return result;
        }

        public void UseFlySkill()
        {
            if (CanFly)
            {
                m_useitemCount += 1;
                m_game.SendPlayerUseProp(this, -2, -2, CARRY_TEMPLATE_ID);
                SetBall(3);
            }
        }



        public bool UseItem(ItemTemplateInfo item)
        {
            m_useitemCount += 1;
            if (CanUseItem(item))
            {
                Energy -= item.Property4;
                //m_delay += item.Property5;
                CurrentDelay += item.Property5;
                m_game.SendPlayerUseProp(this, -2, -2, item.TemplateID, this);
                SpellMgr.ExecuteSpell(m_game, m_game.CurrentLiving as Player, item);
                return true;
            }
            return false;
        }

        public bool UseItem(ItemTemplateInfo item, int place)
        {
            if (!CanUseItem(item, place))
            {
                return false;
            }
            if (base.IsLiving)
            {
                _ = ReduceEnergy(item.Property4);
                CurrentDelay += item.Property5;
                //AddDelay(item.Property5);
            }
            else if (place == -1)
            {
                base.psychic -= item.Property7;
                base.Game.CurrentLiving.AddDelay(item.Property5);
            }
            m_game.method_39(this, -2, -2, item.TemplateID);
            SpellMgr.ExecuteSpell(m_game, m_game.CurrentLiving as Player, item);
            if (item.Property6 == 1 && base.IsAttacking)
            {
                StopAttacking();
                m_game.CheckState(0);
            }
            m_useitemCount += 1;
            OnBeginUseProp();
            return true;
        }

        public void UseSecondWeapon()
        {
            m_useitemCount += 1;
            if (!CanUseItem(DeputyWeapon.Template))
            {
                return;
            }
            if (DeputyWeapon.Template.Property3 == 31)
            {
                bool isArrmor = false;
                if (new List<int>
                {
                    17006,
                    17012,
                    17013
                }.Contains(DeputyWeapon.TemplateID))
                {
                    isArrmor = true;
                }
                _ = new AddGuardEquipEffect((int)getHertAddition(DeputyWeapon), 1, isArrmor).Start(this);
                OnPlayerGuard();
            }
            else
            {
                SetCurrentWeapon(DeputyWeapon);
                OnPlayerCure();
            }
            ShootCount = 1;
            Energy -= DeputyWeapon.Template.Property4;
            //m_delay += m_DeputyWeapon.Template.Property5;
            CurrentDelay += DeputyWeapon.Template.Property5;
            m_game.SendPlayerUseProp(this, -2, -2, DeputyWeapon.Template.TemplateID);
            if (deputyWeaponCount > 0)
            {
                deputyWeaponCount--;
                m_game.SendUseDeputyWeapon(this, deputyWeaponCount);
            }
            OnPlayerUseSecondWeapon(DeputyWeapon.Template.Property3);
        }

        public void UseSpecialSkill()
        {
            if (m_useitemCount <= 15)
            {
                m_useitemCount = 9999;
                if (base.Dander >= 200)
                {
                    SetBall(m_spBallId, special: true);
                    m_ballCount = CurrentBall.Amount;
                    SetDander(0);
                }
            }
        }

        public override void SpeedMultX(int value)
        {
            SpeedMult = value;
            MOVE_SPEED = value - 1;
            base.SpeedMultX(value);
        }

        public bool canMoveDirection(int dir)
        {
            return !m_map.IsOutMap(X + ((15 + MOVE_SPEED) * dir), Y);
        }

        public Point getNextWalkPoint(int dir)
        {
            return canMoveDirection(dir) ? m_map.FindNextWalkPoint(X, Y, dir, StepX, StepY) : Point.Empty;
        }

        public Point FindYLineNotEmptyPointDown(int tx, int ty)
        {
            _ = m_map.Bound;
            return m_map.FindYLineNotEmptyPointDown(tx, ty, m_map.Bound.Height);
        }

        public void OnBeforeBomb(int delay)
        {
            BeforeBomb?.Invoke(this);
        }

        public void SkipAttack()
        {
            m_useitemCount = 9999;
            Game.SendSkipNext(this);
            Prop = 0;
            AddDelay(10);
            base.Skip(1000);
        }

        public void MarkMeHide(bool isMark)
        {
            m_game.SendMarkMeHideInfo(this, Id, isMark);
        }

        private readonly List<int> propsBloqueados;

        public void unlockProp(int templateid)
        {
            _ = propsBloqueados.Remove(templateid);
        }

        public void lockProp(int templateid)
        {
            if (propsBloqueados.Contains(templateid))
            {
                return;
            }

            propsBloqueados.Add(templateid);
        }

        public void SendPicturePlayer(int type, bool state, int count)
        {
            if (m_syncAtTime)
            {
                m_game.updatePlayerBuff(this, type, state, count);
            }
        }
        public Point StartFalling(bool direct)
        {
            return StartFalling(direct, 0, Living.MOVE_SPEED * 10);
        }
        public virtual Point StartFalling(bool direct, int delay, int speed)
        {
            // 1. Düşülecek noktayı bul
            Point p = m_map.FindYLineNotEmptyPointDown(X, Y);

            // Eğer yer bulunamazsa (boşluksa), haritanın en altına bir nokta ata
            if (p == Point.Empty)
            {
                p = new Point(X, m_game.Map.Bound.Height + 1);
            }

            // Oyuncu zaten o noktadaysa işlem yapma
            if (p.Y == Y)
            {
                return Point.Empty;
            }

            // 2. ÖNEMLİ DÜZELTME: Hedef nokta harita dışındaysa (boşluğa düşüyorsa)
            // animasyonlu geçişi beklemeden direkt ölümü tetikle.
            bool isOutMap = m_map.IsOutMap(p.X, p.Y);

            if (direct || isOutMap)
            {
                base.SetXY(p);

                // Harita dışındaysa öldür
                if (isOutMap)
                {
                    base.Die();
                }
            }
            else
            {
                // Normal düşüş animasyonu (harita içi geçerli zeminler için)
                m_game.AddAction(new LivingFallingAction(this, p.X, p.Y, speed, null, delay, 0, null));
            }

            return p;
        }
    }
}

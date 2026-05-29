using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using Game.Logic.Protocol;
using Game.Server;
using Game.Server.Achievement;
using Game.Server.ActiveSystem;
using Game.Server.API;
using Game.Server.Buffer;
using Game.Server.Consortia;
using Game.Server.ConsortiaTask;
using Game.Server.EliteGame;
using Game.Server.Event;
using Game.Server.Farm;
using Game.Server.GameRoom;
using Game.Server.Games;
using Game.Server.GameUtils;
using Game.Server.GuildBattle;
using Game.Server.HotSpringRooms;
using Game.Server.LittleGame;
using Game.Server.LittleGame.Data;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Pet;
using Game.Server.Quests;
using Game.Server.Rooms;
using Game.Server.SceneMarryRooms;
using Game.Server.Statics;
using Game.Server.WonderFul;
using Game.Server.WorldBoss;
using log4net;
using Newtonsoft.Json;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;


//                                              GEREKEN YERLERDEKİ TÜM LANGUAGE MGR KODLARI TÜRKÇELEŞTİRİLDİ not: yuti
public class GamePlayer : IGamePlayer
{

    public delegate void PlayerOwnSpaEventHandle(int onlineTimeSpa);

    public delegate void PlayerAddItemEventHandel(string type, int value);

    public delegate void GameKillDropEventHandel(AbstractGame game, int type, int npcId, bool playResult);

    public delegate void PlayerAchievementFinish(AchievementData info);

    public delegate void PlayerAdoptPetEventHandle();

    public delegate void PlayerCropPrimaryEventHandle();

    public delegate void PlayerEnterHotSpring(GamePlayer player);

    public delegate void PlayerEventHandle(GamePlayer player);

    public delegate void PlayerFightAddOffer(int offer);

    public delegate void PlayerFightOneBloodIsWin(eRoomType roomType, bool isWin);

    public delegate void PlayerGameKillBossEventHandel(AbstractGame game, NpcInfo npc, int damage);

    public delegate void PlayerGameKillEventHandel(AbstractGame game, int type, int id, bool isLiving, int demage, bool isSpanArea);



    public delegate void PlayerGoldCollection(int value);

    public delegate void PlayerGiftTokenCollection(int value);

    public delegate void PlayerHotSpingExpAdd(int minutes, int exp);

    public delegate void PlayerOnlineAdd(GamePlayer player);

    public delegate void PlayerLoginEventHandle();

    public delegate void PlayerItemComposeEventHandle(int composeType);

    public delegate void PlayerMoneyChargeHandle(int money);

    public delegate void PlayerMoneyChargeWeekHandle(int money);

    public delegate void PlayerItemFusionEventHandle(int fusionType);

    public delegate void PlayerItemInsertEventHandle();

    public delegate void PlayerItemMeltEventHandle(int categoryID);

    public delegate void PlayerItemPropertyEventHandle(int templateID, int count);

    public delegate void PlayerItemStrengthenEventHandle(int categoryID, int level);

    public delegate void PlayerMissionFullOverEventHandle(AbstractGame game, int missionId, bool isWin, int turnNum);

    public delegate void PlayerMissionOverEventHandle(AbstractGame game, int missionId, bool isWin);

    public delegate void PlayerMissionTurnOverEventHandle(AbstractGame game, int missionId, int turnNum);

    public delegate void PlayerNewGearEventHandle(ItemInfo item);

    public delegate void PlayerNewGearEventHandle2(ItemInfo item);

    public delegate void PlayerOwnConsortiaEventHandle();

    public delegate void PlayerAchievementQuestHandle();

    public delegate void PlayerPropertisChange(PlayerInfo player);

    public delegate void PlayerSeedFoodPetEventHandle();

    public delegate void PlayerShopEventHandle(int money, int gold, int offer, int gifttoken, int petScore, int medal, int damageScores, string payGoods);

    public delegate void PlayerUnknowQuestConditionEventHandle();

    public delegate void PlayerUpLevelPetEventHandle();

    public delegate void PlayerUseBugle(int value);

    public delegate void PlayerUserToemGemstoneEventHandle();

    public delegate void PlayerVIPUpgrade(int level, int exp);

    public delegate void PlayerQuestFinishEventHandel(BaseQuest baseQuest);

    public delegate void PlayerPropertyChangedEventHandel(PlayerInfo character);

    public delegate void PlayerMarryTeamEventHandle(AbstractGame game, bool isWin, int gainXp, int countPlayersTeam);

    public delegate void PlayerGameOverCountTeamEventHandle(AbstractGame game, bool isWin, int gainXp, int countPlayersTeam);

    public delegate void PlayerMarryEventHandel();

    public delegate void PlayerDispatchesEventHandel();

    public delegate void PlayerGameOverEventHandle(AbstractGame game, bool isWin, int gainXp, bool isSpanArea, bool isCouple);

    public delegate void PlayerGameOverEvent2v2Handle(bool isWin);

    public delegate void PlayerAcademyEventHandle(GamePlayer friendly, int type);

    public delegate void PlayerEquipCardEventHandle();

    public ItemInfo LastTakeCardItem;
    public DateTime BossBoxStartTime;

    public bool BlockReceiveMoney;

    public DateTime LastOpenHole;

    public int canTakeOut;

    public Dictionary<int, CardInfoOld> Card = [];

    public CardInfoOld[] CardsTakeOut = new CardInfoOld[9];

    public int CurrentRoomIndex;

    public int CurrentRoomTeam;
    public int FightPower;

    public double GuildRichAddPlus = 1.0;

    public int Hot_Direction;

    public int Hot_X;

    public int Hot_Y;

    public int HotMap;
    public bool IsInChristmasRoom;

    public bool IsInWorldBossRoom;

    public bool isPowerFullUsed;

    public bool KickProtect;

    public DateTime WaitingProcessor;

    #region Consortia Task
    protected ConsortiaTaskLogicProcessor m_consortiaTaskProcessor;
    public ConsortiaTaskProcessor ConsortiaTask { get; private set; }
    public delegate void DonateRiches(int value, int type);
    public event DonateRiches Riches;
    public void OnDonateRiches(int value, int type)
    {
        Riches?.Invoke(value, type);
    }
    #endregion

    public readonly string[] labyrinthGolds = new string[40] //savaşçının gizli yeri burası normalde not: yuti
    {
        "0|0",
        "2|2",
        "0|0",
        "2|2",
        "0|0",
        "2|3",
        "0|0",
        "3|3",
        "0|0",
        "3|4",
        "0|0",
        "3|4",
        "0|0",
        "4|5",
        "0|0",
        "4|5",
        "0|0",
        "4|6",
        "0|0",
        "5|6",
        "0|0",
        "5|7",
        "0|0",
        "5|7",
        "0|0",
        "6|8",
        "0|0",
        "6|8",
        "0|0",
        "6|10",
        "0|0",
        "8|10",
        "0|0",
        "8|11",
        "0|0",
        "8|11",
        "0|0",
        "10|12",
        "0|0",
        "10|12"
    };

    public DateTime LastAttachMail;

    public DateTime LastChatTime;

    public DateTime LastDrillUpTime;

    public DateTime LastEnterWorldBoss;

    public DateTime LastFigUpTime;

    public DateTime LastOpenCard;

    public DateTime LastOpenChristmasPackage;

    public DateTime LastOpenGrowthPackage;

    public DateTime LastOpenPack;

    public DateTime LastOpenYearMonterPackage;

    public List<ItemInfo> LotteryAwardList;

    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected GameClient m_client;
    private readonly UTF8Encoding m_converter;
    private ItemInfo m_currentSecondWeapon;

    private int m_changed;
    protected BaseGame m_game;
    private readonly PlayerInventory m_petEgg;
    protected Player m_players;
    private char[] m_pvepermissions;
    public bool m_toemview;

    public DateTime BoxBeginTime;
    public int MarryMap;

    public int HoGiap;
    private static readonly char[] permissionChars = new char[4]
    {
        '1',
        '3',
        '7',
        'F'
    };

    public long PingStart;

    public byte States;

    private static readonly int[] StyleIndex = new int[15]
    {
        1,
        2,
        3,
        4,
        5,
        6,
        11,
        13,
        14,
        15,
        16,
        17,
        18,
        19,
        20
    };

    public int takeoutCount;

    public int winningStreak;

    public int WorldBossMap;

    public int X;

    public int Y;

    private char[] m_fightlabpermissions;

    private static readonly char[] fightlabpermissionChars = new char[4]
    {
        '0',
        '1',
        '2',
        '3'
    };

    public int missionPlayed;

    public int playersKilled;

    protected ConsortiaLogicProcessor m_consortiaProcessor;

    protected GameRoomLogicProcessor m_gameroomProcessor = new();

    protected GameRoomProcessor m_gameRoom;
    protected PetLogicProcessor m_petProcessor;
    public DateTime LastMovePlaceItem;

    public Dictionary<int, int[]> CardResetTempProp;
    public int TakeCardPlace;

    public int TakeCardTemplateID;

    public int TakeCardCount;
    public static List<Suit_TemplateInfo> DS_Template_Suit_info = Load_Template_Suit_info();

    protected FarmLogicProcessor m_farmProcessor = new();

    public List<int> CardBuff { get; set; }

    public PlayerInventory FarmBag { get; }

    public PlayerInventory Vegetable { get; }

    public FarmProcessor FarmHandler { get; private set; }

    public PlayerLittleGameInfo LittleGameInfo
    {
        get;
    }

    public LittleGameProcessor LittleGame { get; private set; }

    protected LittleGameLogicProcessor m_LittleGameProcessor;

    public GameRoomProcessor GameRoom => m_gameRoom;

    public double GPApprenticeOnline
    {
        get
        {
            if (UserVIPInfo.MasterOrApprenticesArr.Count > 0)
            {
                foreach (KeyValuePair<int, string> item in UserVIPInfo.MasterOrApprenticesArr)
                {
                    if (WorldMgr.GetPlayerById(item.Key) != null)
                    {
                        return 0.05;
                    }
                }
            }
            return 0.0;
        }
        set
        {
        }
    }

    public double GPApprenticeTeam
    {
        get
        {
            if (CurrentRoom != null)
            {
                foreach (GamePlayer player in CurrentRoom.GetPlayers())
                {
                    if (player != this && player.PlayerCharacter.MasterOrApprenticesArr.ContainsKey(PlayerId))
                    {
                        return 0.1;
                    }
                }
            }
            return 0.0;
        }
        set
        {
        }
    }

    public double GPSpouseTeam
    {
        get
        {
            if (CurrentRoom != null)
            {
                foreach (GamePlayer player in CurrentRoom.GetPlayers())
                {
                    if (player != this && player.PlayerCharacter.SpouseID == PlayerId)
                    {
                        return 0.05;
                    }
                }
            }
            return 0.0;
        }
        set
        {
        }
    }

    public PetProcessor PetHandler { get; private set; }

    public PlayerActives Actives { get; }

    public ConsortiaProcessor Consortia { get; private set; }

    public UserLabyrinthInfo Labyrinth { get; set; }

    public string Account { get; }

    public AchievementInventory AchievementInventory { get; }

    public EventInventory EventLiveInventory { get; }

    public long AllWorldDameBoss { get; set; }

    public PlayerBattle BattleData { get; }

    public bool bool_1 { get; set; }

    public bool Boolean_0
    {
        get => bool_1; set => bool_1 = value;
    }

    public BufferList BufferList { get; }

    public PlayerInventory CaddyBag { get; }

    public bool CanUseProp { get; set; }

    public double GPAddPlus { get; set; }

    public double OfferAddPlus { get; set; }

    public bool CanX2Exp { get; set; }

    public bool CanX3Exp { get; set; }

    public CardInventory CardBag { get; }

    public GameClient Client => m_client;

    public PlayerInventory ConsortiaBag { get; }

    public PlayerInventory BankBag { get; }

    public HotSpringRoom CurrentHotSpringRoom { get; set; }

    public MarryRoom CurrentMarryRoom { get; set; }

    public BaseRoom CurrentRoom
    {
        get; set
        {
            BaseRoom baseRoom = Interlocked.Exchange(ref field, value);
            if (baseRoom != null)
            {
                RoomMgr.ExitRoom(baseRoom, this);
            }
        }
    }
    public BaseGame Game
    {
        get => m_game; set => m_game = value;
    }
    public void DiceReset()
    {
        Dice.Reset();
    }
    public PlayerEquipInventory EquipBag { get; }

    public List<int> EquipEffect { get; set; }

    public PlayerExtra Extra { get; }

    public PlayerInventory FightBag { get; }

    public List<BufferInfo> FightBuffs { get; set; }
    public PlayerDice Dice { get; }
    public PlayerInventory Food { get; }

    public Dictionary<int, int> Friends { get; private set; }


    public int GameId { get; set; }



    public int GamePlayerId { get; set; }

    public int TempGameId { get; set; }

    public ItemInfo Healstone { get => field == null ? null : (field);

        private set;
    }

    public int Immunity { get; set; } = 255;

    public bool IsAASInfo { get; set; }

    public virtual bool IsActive => m_client.IsConnected;

    public bool IsInMarryRoom => CurrentMarryRoom != null;

    public bool IsMinor { get; set; }

    public List<UserGemStone> GemStone { get; set; }

    public int Level
    {
        get => UserVIPInfo.Grade;
        set
        {
            if (value != UserVIPInfo.Grade)
            {
                int grade = UserVIPInfo.Grade;
                DailyRecordInfo info = new()
                {
                    UserID = UserVIPInfo.ID,
                    Type = 2,
                    Value = $"{UserVIPInfo.Grade},{value}"
                };
                new PlayerBussiness().AddDailyRecord(info);
                Extra.UpdateEventCondition((int)NoviceActiveType.Level_Atlama, value);
                UserVIPInfo.Grade = value;
                if (value == 6)
                {
                    ItemInfo cloneItem = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(112098), 1, 104);
                    _ = AddTemplate(cloneItem);
                }
                if (value == 8)
                {
                    PlayerCharacter.WeaklessGuildProgressStr = "////b7D/ht8WDQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
                }
                if (UserVIPInfo.masterID != 0 && grade < UserVIPInfo.Grade)
                {
                    AcademyMgr.UpdateAwardApp(this, grade);
                }
                EquipBag.UpdatePlayerProperties();

                // Sonra bu yeni değerlere göre Savaş Gücünü (FightPower) güncelle.
                UpdateFightPower();

                OnLevelUp(value);
                OnPropertiesChanged();
            }
        }
    }

    public int LevelPlusBlood => LevelMgr.LevelPlusBlood(PlayerCharacter.Grade);

    public ItemInfo MainWeapon { get; private set; }

    public UserMatchInfo MatchInfo => BattleData.MatchInfo;

    public virtual IPacketLib Out => m_client.Out;

    public UsersPetInfo Pet { get; private set; }

    public long PingTime
    {
        get; set
        {
            field = value;
            GSPacketIn pkg = Out.SendNetWork(PlayerCharacter.ID, field);
            CurrentRoom?.SendToAll(pkg, this);
        }
    }

    public PlayerInfo PlayerCharacter => UserVIPInfo;

    public PetInventory PetBag { get; }

    public PlayerFarm Farm { get; }

    public int PlayerId { get; }

    public PlayerProperty PlayerProp { get; }

    public Player Players => m_players;

    public ePlayerState PlayerState { get; set; }

    public string ProcessLabyrinthAward { get; set; }

    public PlayerInventory PropBag { get; }

    public QuestInventory QuestInventory { get; }

    public PlayerRank Rank { get; }

    public ItemInfo SecondWeapon => m_currentSecondWeapon == null ? null : m_currentSecondWeapon;

    public int ServerID { get; set; }

    public bool IsAccountLimit { get; set; }

    public bool ShowPP { get; set; }

    public PlayerInventory StoreBag { get; }

    public PlayerInventory TempBag { get; }

    public Dictionary<string, object> TempProperties { get; } = [];

    public bool Toemview
    {
        get => m_toemview; set => m_toemview = value;
    }

    public Dictionary<int, UserDrillInfo> UserDrills { get; set; }

    public PlayerInfo UserVIPInfo { get; private set; }

    public long WorldbossBood { get; set; }

    public int ZoneId => GameServer.Instance.Configuration.ServerID;

    public string ZoneName => GameServer.Instance.Configuration.ServerName;

    public int Lottery { get; internal set; }

    public List<ItemBoxInfo> LotteryItems { get; internal set; }

    public int LotteryID { get; internal set; }

    public DateTime LastRequestTime { get; internal set; }

    public int CurrentEnemyId { get; set; }

    public double BaseAgility { get; set; }

    public double BaseDamage { get; set; }

    public bool IsViewer { get; set; }

    public List<int> ViFarms { get; private set; }

    public event PlayerAchievementFinish AchievementFinishEvent;

    public event PlayerAdoptPetEventHandle AdoptPetEvent;

    public event PlayerGameKillBossEventHandel AfterKillingBoss;

    public event PlayerGameKillEventHandel AfterKillingLiving;

    // GamePlayer.cs içinde

    public event DiscordBaglaCondition DiscordBaglaEvent;
    public delegate void DiscordBaglaCondition(GamePlayer player);

    // BU METODU EKLEYİN (Event'i dışarıdan tetiklemek için)
    public void OnDiscordLinkSuccess()
    {
        DiscordBaglaEvent?.Invoke(this);
    }

    public event PlayerItemPropertyEventHandle AfterUsingItem;

    public event PlayerCropPrimaryEventHandle CropPrimaryEvent;

    public event PlayerEnterHotSpring EnterHotSpringEvent;

    public event PlayerVIPUpgrade Event_0;

    public event PlayerFightAddOffer FightAddOfferEvent;

    public event PlayerFightOneBloodIsWin FightOneBloodIsWin;

    public event GameKillDropEventHandel GameKillDrop;

    public event PlayerOwnConsortiaEventHandle GuildChanged;

    public event PlayerHotSpingExpAdd HotSpingExpAdd;

    public event PlayerOnlineAdd OnlineGameAdd;

    public event PlayerLoginEventHandle PlayerLogin;

    public event PlayerItemComposeEventHandle ItemCompose;

    public event PlayerItemFusionEventHandle ItemFusion;

    public event PlayerItemInsertEventHandle ItemInsert;

    public event PlayerItemMeltEventHandle ItemMelt;

    public event PlayerItemStrengthenEventHandle ItemStrengthen;

    public event PlayerMoneyChargeHandle MoneyCharge;

    public event PlayerMoneyChargeWeekHandle MoneyChargeWeek;

    public event PlayerAchievementQuestHandle AchievementQuest;

    public event PlayerEventHandle LevelUp;

    public event PlayerMissionOverEventHandle MissionOver;

    public event PlayerMissionTurnOverEventHandle MissionTurnOver;

    public event PlayerNewGearEventHandle NewGearEvent;

    public event PlayerSeedFoodPetEventHandle SeedFoodPetEvent;

    public event PlayerShopEventHandle Paid;

    public event PlayerUnknowQuestConditionEventHandle UnknowQuestConditionEvent;

    public event PlayerUpLevelPetEventHandle UpLevelPetEvent;

    public event PlayerEventHandle UseBuffer;

    public event PlayerUserToemGemstoneEventHandle UserToemGemstonetEvent;

    public event PlayerPropertyChangedEventHandel PlayerPropertyChanged;

    public event PlayerAddItemEventHandel PlayerAddItem;

    public event PlayerOwnSpaEventHandle PlayerSpa;

    public event PlayerPropertisChange PropertiesChange;

    public event PlayerMissionFullOverEventHandle MissionFullOver;

    public event PlayerQuestFinishEventHandel PlayerQuestFinish;

    public event PlayerGameOverCountTeamEventHandle GameOverCountTeam;

    public event PlayerMarryTeamEventHandle GameMarryTeam;

    public event PlayerUseBugle UseBugle;

    public event PlayerMarryEventHandel PlayerMarry;

    public event PlayerDispatchesEventHandel PlayerDispatches;

    public event PlayerGameOverEventHandle GameOver;

    public event PlayerGameOverEvent2v2Handle GameOver2v2;

    public event PlayerAcademyEventHandle AcademyEvent;

    public event PlayerEquipCardEventHandle EquipCardEvent;

    public PlayerAvatarCollection AvatarCollect { get; }

    public long TimeCheckHack { get; set; }

    private void SetupProcessor()
    {
        FarmHandler = new FarmProcessor(m_farmProcessor);
        m_gameRoom = new GameRoomProcessor(m_gameroomProcessor);
        ConsortiaTask = new ConsortiaTaskProcessor(m_consortiaTaskProcessor);
        WorldBoss = new WorldBossProcessor(_worldBossProcessor);
        LittleGame = new LittleGameProcessor(m_LittleGameProcessor);
        ActiveSystemHandler = new ActiveSystemProcessor(m_activeSystemProcessor);
        EliteGameHandler = new EliteGameProcessor(m_eliteGameProcessor);
    }

    //private int count_addmoney;
    //public int CountAddMoney
    //{
    //    get
    //    {
    //        return this.count_addmoney;
    //    }
    //    set
    //    {
    //        this.count_addmoney = value;
    //    }
    //}
    //private int count_addgp;
    //public int CountAddGP
    //{
    //    get
    //    {
    //        return this.count_addgp;
    //    }
    //    set
    //    {
    //        this.count_addgp = value;
    //    }
    //}
    //private int count_function;
    //public int CountFunction
    //{
    //    get
    //    {
    //        return this.count_function;
    //    }
    //    set
    //    {
    //        this.count_function = value;
    //    }
    //}
    //private int count_function2;
    //public int CountFunction2
    //{
    //    get
    //    {
    //        return this.count_function2;
    //    }
    //    set
    //    {
    //        this.count_function2 = value;
    //    }
    //}
    //private Random count_random;

    private Dictionary<string, UserEquipGhostInfo> m_equipGhostList;

    public int GuildBattleEnemyId { get; set; }

    public int CountMissedEquipGhost = 0;

    //public EventSevenDaysInfo EventSeven; //burası niye kaldırılmış sonradan incelicem not: yuti

    public string LastChatMsg;

    protected ActiveSystemLogicProcessor m_activeSystemProcessor = new();

    public ActiveSystemProcessor ActiveSystemHandler { get; private set; }

    #region ELITEGAME
    protected EliteGameLogicProcessor m_eliteGameProcessor = new();

    public EliteGameProcessor EliteGameHandler { get; private set; }
    #endregion

    public GamePlayer(int playerId, string account, GameClient client, PlayerInfo info)
    {
        PlayerId = playerId;
        Account = account;
        m_client = client;
        UserVIPInfo = info;
        EquipBag = new PlayerEquipInventory(this);
        PropBag = new PlayerInventory(this, saveTodb: true, 96, 1, 0, autoStack: true);
        ConsortiaBag = new PlayerInventory(this, saveTodb: true, 100, 11, 0, autoStack: true);
        BankBag = new PlayerInventory(this, saveTodb: true, 492, 51, 0, autoStack: true);
        StoreBag = new PlayerInventory(this, saveTodb: true, 20, 12, 0, autoStack: true);
        FightBag = new PlayerInventory(this, saveTodb: false, 3, 3, 0, autoStack: false);
        TempBag = new PlayerInventory(this, saveTodb: false, 60, 4, 0, autoStack: true);
        CaddyBag = new PlayerInventory(this, saveTodb: false, 30, 5, 0, autoStack: true);
        FarmBag = new PlayerInventory(this, saveTodb: true, 30, 13, 0, autoStack: true);
        Vegetable = new PlayerInventory(this, saveTodb: true, 30, 14, 0, autoStack: true);
        Food = new PlayerInventory(this, saveTodb: true, 30, 34, 0, autoStack: true);
        m_petEgg = new PlayerInventory(this, saveTodb: true, 30, 35, 0, autoStack: true);
        CardBag = new CardInventory(this, saveTodb: true, 100, 5);
        Farm = new PlayerFarm(this, saveTodb: true, 30, 0);
        PetBag = new PetInventory(this, saveTodb: true, 20, 8, 0);
        Rank = new PlayerRank(this, saveToDb: true);
        PlayerProp = new PlayerProperty(this);

        BattleData = new PlayerBattle(this, saveTodb: true);
        Extra = new PlayerExtra(this, saveTodb: true);
        Actives = new PlayerActives(this, saveTodb: true);
        QuestInventory = new QuestInventory(this);
        AchievementInventory = new AchievementInventory(this);
        EventLiveInventory = new EventInventory(this);
        BufferList = new BufferList(this);
        FightBuffs = [];
        EquipEffect = [];
        UserDrills = [];
        CardBuff = [];
        GPAddPlus = 1.0;
        OfferAddPlus = 1.0;
        m_toemview = true;
        X = 646;
        Y = 1241;
        MarryMap = 0;
        LastChatTime = DateTime.Today;
        LastFigUpTime = DateTime.Today;
        LastDrillUpTime = DateTime.Today;
        LastOpenPack = DateTime.Today;
        LastMovePlaceItem = DateTime.Today;
        ShowPP = false;
        m_converter = new UTF8Encoding();
        BossBoxStartTime = DateTime.Now;
        ResetLottery();
        IsAccountLimit = false;
        CardResetTempProp = [];
        Labyrinth = null;
        m_consortiaProcessor = new ConsortiaLogicProcessor();
        m_petProcessor = new PetLogicProcessor();
        BlockReceiveMoney = false;
        LastOpenHole = DateTime.Now;
        LastTakeCardItem = null;
        m_consortiaTaskProcessor = new ConsortiaTaskLogicProcessor();
        _worldBossProcessor = new WorldBossLogicProcessor();
        LittleGameInfo = new PlayerLittleGameInfo
        {
            Actions = new TriggeredQueue<string, GamePlayer>(this),
            X = 275,
            Y = 30
        };
        m_LittleGameProcessor = new LittleGameLogicProcessor();
        UserVIPInfo.CheckCode = "baodeptrai";
        IsViewer = false;
        TimeCheckHack = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        //this.count_addmoney = 0;
        //this.count_addgp = 0;
        //this.count_function = 0;
        //this.count_function2 = 0;
        //this.count_random = new Random();
        GemStone = [];
        AvatarCollect = new PlayerAvatarCollection(this, true);
        Dice = new PlayerDice(this, true);
        m_equipGhostList = [];
        GuildBattleEnemyId = 0;
        LastOpenChristmasPackage = DateTime.Now;
        GmActivity = new PlayerGmActivity(this, saveTodb: true);
    }

    public bool isPassCheckCode()
    {

        //int checkmoney = this.count_random.Next(10, 15);//7, 9); //30 ~ 40 trận sẽ hiện mã captcha
        //int checkgp = this.count_random.Next(30, 40);
        //int checkfunction = this.count_random.Next(7, 10);
        //int checkfunction2 = this.count_random.Next(40, 60);
        //bool result = this.m_character.CheckCount == 0 && this.count_addmoney < checkmoney/* && this.count_addgp < checkgp && this.count_function < checkfunction && this.count_function2 < checkfunction2*/;
        //Console.WriteLine($"Check: {result}");
        //return result;
        return true;
    }

    public void resetPassCode()
    {
        // this.CountAddMoney = 0;
        //this.CountAddGP = 0;
        //this.CountFunction = 0;
        //this.CountFunction2 = 0;
    }

    //public bool ShowCheckCode()
    //{
    //    this.m_character.CheckCount = 1;
    //    GSPacketIn gSPacketIn = new GSPacketIn(200);
    //    if (Client.Player.PlayerCharacter.CheckError < 1)
    //    {
    //        gSPacketIn.WriteByte(1);
    //    }
    //    else
    //    {
    //        gSPacketIn.WriteByte(2);
    //    }
    //    gSPacketIn.WriteBoolean(val: true);
    //    gSPacketIn.WriteByte(1);
    //    gSPacketIn.WriteString("hi");
    //    Client.Player.PlayerCharacter.CheckCode = CheckCode.GenerateCheckCode();
    //    gSPacketIn.Write(CheckCode.CreateImage(Client.Player.PlayerCharacter.CheckCode));
    //    this.SendTCP(gSPacketIn);
    //    return true;
    //}

    public bool isPlayerWarrior()
    {
        return Extra.Info.coupleBossBoxNum == 9;
    }

    public void UpdatePublicPlayer(string tempStyle = "")
    {
        PlayerCharacter.tempStyle = tempStyle;
        GSPacketIn pkg = Out.SendUpdatePublicPlayer(PlayerCharacter, MatchInfo, Extra.Info);
        CurrentRoom?.SendToAll(pkg, this);
    }

    public int AddAchievementPoint(int value)
    {
        if (value > 0)
        {
            UserVIPInfo.AchievementPoint += value;
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public void SendUpdatePublicPlayer()
    {
        _ = Out.SendUpdatePublicPlayer(PlayerCharacter, MatchInfo, Extra.Info);
    }

    public void AddExpVip(int value)
    {
        List<int> exp = GameProperties.VIPExp();
        UserVIPInfo.VIPExp += value;
        for (int i = 0; i < exp.Count; i++)
        {
            int vipExp = UserVIPInfo.VIPExp;
            int level = UserVIPInfo.VIPLevel;
            if (level == 9)
            {
                UserVIPInfo.VIPExp = exp[8];
                break;
            }
            if (level < 9 && canUpLv(vipExp, level))
            {
                UserVIPInfo.VIPLevel++;
                if (UserVIPInfo.VIPLevel >= 7 && PetBag != null)
                {
                    PetBag.UpdatePetFiveKillSlot(UserVIPInfo.VIPLevel);
                }
                DailyRecordInfo info = new()
                {
                    UserID = PlayerCharacter.ID,
                    Type = 28,
                    Value = UserVIPInfo.VIPLevel.ToString()
                };
                new PlayerBussiness().AddDailyRecord(info);
            }
        }
        Extra.UpdateEventCondition((int)NoviceActiveType.VIP_LEVEL, PlayerCharacter.VIPLevel);
        if (UserVIPInfo.IsVIPExpire())
        {
            _ = Out.SendOpenVIP(this);
        }
    }

    public bool RemoveExpVip(int value)
    {
        bool result = false;
        List<int> list = GameProperties.VIPExp();
        if (UserVIPInfo.VIPExp >= value)
        {
            UserVIPInfo.VIPExp -= value;
            result = true;
        }
        else if (UserVIPInfo.VIPExp < value && UserVIPInfo.VIPExp > 0)
        {
            UserVIPInfo.VIPExp = 0;
            result = true;
        }
        for (int i = 0; i < list.Count; i++)
        {
            int vIPExp = UserVIPInfo.VIPExp;
            int vIPLevel = UserVIPInfo.VIPLevel;

            if (vIPLevel > 9 && canDownLv(vIPExp, vIPLevel))
            {
                UserVIPInfo.VIPLevel--;
                DailyRecordInfo info = new()
                {
                    UserID = PlayerCharacter.ID,
                    Type = 28,
                    Value = UserVIPInfo.VIPLevel.ToString()
                };
                new PlayerBussiness().AddDailyRecord(info);

            }
        }
        return result;
    }

    public int AddGold(int value)
    {
        if (value > 0)
        {
            UserVIPInfo.Gold += value;
            if (UserVIPInfo.Gold == int.MinValue)
            {
                UserVIPInfo.Gold = int.MaxValue;
                SendMessage("Altınların sınırına ulaşmış!"); //türkçeleştirildi not: yuti
            }
            OnPlayerAddItem("Gold", value);
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }
    public int AddGP(int gp)
    {
        // Chức năng của BAOLT - Lâm đừng copaste nha //incelerim bi ara not: yuti
        if (isPlayerWarrior())
        {
            return 0;
        }

        if (gp >= 0)
        {
            if (AntiAddictionMgr.ISASSon)
            {
                gp = (int)(gp * AntiAddictionMgr.GetAntiAddictionCoefficient(PlayerCharacter.AntiAddiction));
            }
            gp = (int)(gp * RateMgr.GetRate(eRateType.Experience_Rate));
            if (GPAddPlus > 0.0)
            {
                gp = (int)(gp * GPAddPlus);
            }
            UserVIPInfo.GP += gp;
            if (UserVIPInfo.GP < 1)
            {
                UserVIPInfo.GP = 1;
            }
            Level = LevelMgr.GetLevel(UserVIPInfo.GP);
            int maxLevel = LevelMgr.MaxLevel;
            LevelInfo levelInfo = LevelMgr.FindLevel(maxLevel);
            if (Level == maxLevel && levelInfo != null)
            {
                UserVIPInfo.GP = levelInfo.GP;
                int num = gp / 1000; //100 olan değer 1000e yükseltildi. not: yuti
                if (num > 0)
                {
                    _ = AddOffer(num);
                    SendHideMessage(string.Format("Maksimum seviyeye ulaştığınız için kazandığınız deneyim mükafata dönüştürüldü. Kazanılan Mükafat: " + num)); //türkçeleştirildi not: yuti
                }
            }
            //this.count_addgp++;
            UpdateFightPower();
            OnPropertiesChanged();
            return gp;
        }
        return 0;
    }
    public int AddGP(int gp, bool x2)
    {
        // Chức năng của BAOLT - Lâm đừng copaste nha
        if (isPlayerWarrior())
        {
            return 0;
        }

        if (gp >= 0)
        {
            if (AntiAddictionMgr.ISASSon)
            {
                gp = (int)(gp * AntiAddictionMgr.GetAntiAddictionCoefficient(PlayerCharacter.AntiAddiction));
            }
            gp = (int)(gp * RateMgr.GetRate(eRateType.Experience_Rate));
            if (GPAddPlus > 0.0 && x2)
            {
                gp = (int)(gp * GPAddPlus);
            }
            UserVIPInfo.GP += gp;
            if (UserVIPInfo.GP < 1)
            {
                UserVIPInfo.GP = 1;
            }
            Level = LevelMgr.GetLevel(UserVIPInfo.GP);
            int maxLevel = LevelMgr.MaxLevel;
            LevelInfo levelInfo = LevelMgr.FindLevel(maxLevel);
            if (Level == maxLevel && levelInfo != null)
            {
                UserVIPInfo.GP = levelInfo.GP;
                int num = gp / 1000; //100 olan değer 1000e yükseltildi not: yuti
                if (num > 0)
                {
                    _ = AddOffer(num);
                    SendHideMessage(string.Format("Maksimum seviyeye ulaştığınız için kazandığınız deneyim mükafata dönüştürüldü. Kazanılan Mükafat: " + num)); //türkçeleştirildi not: yuti
                }
            }
            //this.count_addgp++;
            UpdateFightPower();
            OnPropertiesChanged();
            return gp;
        }
        return 0;
    }

    public void AddGift(eGiftType type)
    {
        List<ItemInfo> list = [];
        bool testActive = GameProperties.TestActive;
        switch (type)
        {
            case eGiftType.MONEY:
                if (testActive)
                {
                    _ = AddMoney(GameProperties.FreeMoney);
                }
                break;
            case eGiftType.SMALL_EXP:
                {
                    string[] array2 = GameProperties.FreeExp.Split('|');
                    ItemTemplateInfo itemTemplateInfo2 = ItemMgr.FindItemTemplate(Convert.ToInt32(array2[0]));
                    if (itemTemplateInfo2 != null)
                    {
                        list.Add(ItemInfo.CreateFromTemplate(itemTemplateInfo2, Convert.ToInt32(array2[1]), 102));
                    }
                    break;
                }
            case eGiftType.BIG_EXP:
                {
                    string[] array3 = GameProperties.BigExp.Split('|');
                    ItemTemplateInfo itemTemplateInfo3 = ItemMgr.FindItemTemplate(Convert.ToInt32(array3[0]));
                    if (itemTemplateInfo3 != null && testActive)
                    {
                        list.Add(ItemInfo.CreateFromTemplate(itemTemplateInfo3, Convert.ToInt32(array3[1]), 102));
                    }
                    break;
                }
            case eGiftType.PET_EXP:
                {
                    string[] array = GameProperties.PetExp.Split('|');
                    ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(Convert.ToInt32(array[0]));
                    if (itemTemplateInfo != null && testActive)
                    {
                        list.Add(ItemInfo.CreateFromTemplate(itemTemplateInfo, Convert.ToInt32(array[1]), 102));
                    }
                    break;
                }
        }
        foreach (ItemInfo item in list)
        {
            item.IsBinds = true;
            _ = AddTemplate(item, item.Template.BagType, item.Count, eGameView.dungeonTypeGet);
        }
    }

    public int AddGiftToken(int value)
    {
        if (value > 0)
        {
            UserVIPInfo.GiftToken += value;
            OnPlayerAddItem("GiftToken", value);
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    //public bool AddItem(ItemInfo item)
    //{
    //    AbstractInventory itemInventory = GetItemInventory(item.Template);
    //    return itemInventory.AddItem(item, itemInventory.BeginSlot);
    //}

    public bool AddItem(ItemInfo item)
    {
        if (item.Template.BagType == (int)eBageType.EquipBag)
        {
            return EquipBag.AddItem(item);
        }
        AbstractInventory bg = GetItemInventory(item.Template);
        return bg.AddItem(item, bg.BeginSlot);
    }

    public int AddLeagueMoney(int value)
    {
        if (value > 0)
        {
            BattleData.MatchInfo.dailyScore += value;
            BattleData.MatchInfo.weeklyScore += value;
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public int RemoveLeagueMoney(int value)
    {
        if (value > 0)
        {
            BattleData.MatchInfo.dailyScore -= value;
            BattleData.MatchInfo.weeklyScore -= value;
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public void AddLog(string type, string content)
    {
        using PlayerBussiness playerBussiness = new();
        playerBussiness.AddUserLogEvent(PlayerCharacter.ID, PlayerCharacter.UserName, PlayerCharacter.NickName, type, content);
    }

    public int AddMoney(int value)
    {
        return AddMoney(value, igroneAll: true);
    }

    public int AddMoney(int value, bool igroneAll)
    {
        if (value > 0)
        {
            if (!igroneAll && BlockReceiveMoney)
            {
                SendMessage("Karakter sınırını aştığınız için kuponlar hesabınıza eklenemedi."); //türkçeleştirildi not: yuti
                //m_character.Money += (int)Math.Round((double)value * 0.3);
                //OnPropertiesChanged();
                return 0;
            }
            UserVIPInfo.Money += value;
            //this.count_addmoney++;            LOGA EKLEDİĞİ İÇİN TÜRKÇEYE ÇEVİRMEME GEREK YOK!
            AddLog("AddMoney", "Tài khoản " + UserVIPInfo.UserName + "nhận " + value + "xu vào tài khoản" + UserVIPInfo.NickName);
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public int AddMoneyLock(int value)
    {
        if (value > 0)
        {
            UserVIPInfo.MoneyLock += value;
            //this.count_addmoney++;
            AddLog("AddMoneyLock", "Tài khoản " + UserVIPInfo.UserName + "nhận " + value + "xu lock vào tài khoản" + UserVIPInfo.NickName);
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public int AddBadLuckCaddy(int value)
    {
        if (value > 0)
        {
            UserVIPInfo.badLuckNumber += value;
            if (UserVIPInfo.badLuckNumber == int.MinValue)
            {
                UserVIPInfo.badLuckNumber = int.MaxValue;
                SendMessage("Limiti aştınız."); //türkçeleştirildi not: yuti
            }
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public int AddOffer(int value)
    {
        return AddOffer(value, IsRate: true);
    }

    public int RefreshLeagueGetReward(int awardGot, int Score)
    {
        if (awardGot > 0)
        {
            MatchInfo.leagueItemsGet = awardGot;
            MatchInfo.weeklyScore -= Score;
            OnPropertiesChanged();
            return awardGot;
        }
        return 0;
    }

    public int AddOffer(int value, bool IsRate)
    {
        if (value > 0)
        {
            if (AntiAddictionMgr.ISASSon)
            {
                value = (int)(value * AntiAddictionMgr.GetAntiAddictionCoefficient(PlayerCharacter.AntiAddiction));
            }
            if (IsRate)
            {
                value *= ((int)OfferAddPlus == 0) ? 1 : ((int)OfferAddPlus);
            }
            UserVIPInfo.Offer += value;
            OnFightAddOffer(value);
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public int AddPetScore(int value)
    {
        if (value > 0)
        {
            UserVIPInfo.petScore += value;
            if (UserVIPInfo.petScore == int.MinValue)
            {
                UserVIPInfo.petScore = int.MaxValue;
                SendMessage("Sınırı aştınız."); //türkçeleştirildi not: yuti
            }
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public void AddPrestige(bool isWin, eRoomType roomType)
    {
        if (UserVIPInfo.Grade >= 20 && ActiveSystemMgr.IsLeagueOpen)
        {
            BattleData.AddPrestige(isWin);
            OnPropertiesChanged();
        }
        else
        {
            SendMessage("Özgür savaş başarıyla tamamlandı."); //türkçeleştirildi not: yuti
        }
    }

    //public void AddPrestige(bool isWin)
    //{
    //    BattleData.AddPrestige(isWin);
    //}

    public void UpdateRestCount()
    {
        BattleData.Update();
    }

    public int AddRichesOffer(int value)
    {
        if (value > 0)
        {
            UserVIPInfo.RichesOffer += value;
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public int AddRobRiches(int value)
    {
        if (value > 0)
        {
            if (AntiAddictionMgr.ISASSon)
            {
                value = (int)(value * AntiAddictionMgr.GetAntiAddictionCoefficient(PlayerCharacter.AntiAddiction));
            }
            UserVIPInfo.RichesRob += value;
            OnPlayerAddItem("RichesRob", value);
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public int AddScore(int value)
    {
        if (value > 0)
        {
            UserVIPInfo.Score += value;
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public bool AddTemplate(ItemInfo cloneItem)
    {
        return AddTemplate(cloneItem, cloneItem.Template.BagType, cloneItem.Count, eGameView.OtherTypeGet);
    }

    public bool AddTemplate(List<ItemInfo> infos)
    {
        return AddTemplate(infos, eGameView.OtherTypeGet);
    }

    public bool AddTemplate(ItemInfo cloneItem, string name)
    {
        return AddTemplate(cloneItem, cloneItem.Template.BagType, cloneItem.Count, eGameView.OtherTypeGet, name);
    }

    public bool AddTemplate(List<ItemInfo> infos, eGameView typeGet)
    {
        if (infos != null)
        {
            List<ItemInfo> list = [];
            foreach (ItemInfo info in infos)
            {
                info.IsBinds = true;
                if (!StackItemToAnother(info) && !AddItem(info))
                {
                    list.Add(info);
                }
            }
            BagFullSendToMail(list);
            return true;
        }
        return false;
    }

    public bool AddTemplate(List<ItemInfo> infos, int count, eGameView gameView)
    {
        if (infos != null)
        {
            List<ItemInfo> list = [];
            foreach (ItemInfo info in infos)
            {
                info.IsBinds = true;
                info.Count = count;
                if (!StackItemToAnother(info) && !AddItem(info))
                {
                    list.Add(info);
                }
            }
            BagFullSendToMail(list);
            return true;
        }
        return false;
    }

    public bool AddTemplate(ItemInfo cloneItem, eBageType bagType, int count, eGameView gameView)
    {
        return eBageType.FightBag == bagType ? FightBag.AddItem(cloneItem) : AddTemplate(cloneItem, bagType, count, gameView, "no");
    }

    public bool AddTemplate(ItemInfo cloneItem, eBageType bagType, int count, eGameView gameView, string Name)
    {
        if (cloneItem != null)
        {
            _ = new SpecialItemBoxInfo();
            List<ItemInfo> itemOverDue = [];
            AddLog("AddTemplate: ", "ItemInfo: " + cloneItem.Name + "," + cloneItem.ItemID + "," + cloneItem.TemplateID + "|eBageType: " + bagType + "|Count: " + count + "|eGameView: " + gameView + "|Name: " + Name);
            cloneItem.Count = count;
            if (!StackItemToAnother(cloneItem) && !AddItem(cloneItem))
            {
                itemOverDue.Add(cloneItem);
            }
            BagFullSendToMail(itemOverDue);
            if (Name != "no")
            {
                SendItemNotice(cloneItem, (int)gameView, Name);
                SendMessage(LanguageMgr.GetTranslation("AddTemplate.Notice", cloneItem.Template == null ? "null" : cloneItem.Template.Name, cloneItem.Count));
            }
            return true;
        }
        return false;
    }

    public bool AddTemplate(ItemInfo cloneItem, eBageType bagType, int count, bool backToMail)
    {
        PlayerInventory inventory = GetInventory(bagType);
        if (inventory != null && !cloneItem.Template.IsSpecial())
        {
            if (inventory.AddTemplate(cloneItem, count))
            {
                if (CurrentRoom != null && CurrentRoom.IsPlaying)
                {
                    SendItemNotice(cloneItem);
                }
                return true;
            }
            if (backToMail && cloneItem.Template.CategoryID != 10)
            {
                _ = SendItemsToMail(cloneItem, LanguageMgr.GetTranslation("GamePlayer.Msg18"), LanguageMgr.GetTranslation("GamePlayer.Msg18"), eMailType.BuyItem);
            }
        }
        return false;
    }

    public bool RemoveTemplateInShop(int templateid, int count)
    {
        ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(templateid);
        if (itemTemplateInfo != null)
        {
            PlayerInventory itemInventory = GetItemInventory(itemTemplateInfo);
            if (itemInventory != null)
            {
                return itemInventory.RemoveTemplate(templateid, count);
            }
        }
        return false;
    }

    public int GetTemplateCount(int templateId)
    {
        ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(templateId);
        if (itemTemplateInfo != null)
        {
            PlayerInventory itemInventory = GetItemInventory(itemTemplateInfo);
            if (itemInventory != null)
            {
                return itemInventory.GetItemCount(templateId);
            }
        }
        return 0;
    }

    private void SendItemNotice(ItemInfo item)
    {
        GSPacketIn gSPacketIn = new(14);
        gSPacketIn.WriteString(PlayerCharacter.NickName);
        gSPacketIn.WriteInt(1);
        gSPacketIn.WriteInt(item.TemplateID);
        gSPacketIn.WriteBoolean(item.IsBinds);
        gSPacketIn.WriteInt(1);
        if (item.Template.Quality is >= 3 and < 5)
        {
            CurrentRoom?.SendToTeam(gSPacketIn, CurrentRoomTeam, this);
        }
        else
        {
            if (item.Template.Quality < 5)
            {
                return;
            }
            GameServer.Instance.LoginServer.SendPacket(gSPacketIn);
            GamePlayer[] gamePlayers = WorldMgr.GetAllPlayers();
            foreach (GamePlayer player in gamePlayers)
            {
                if (player != this)
                {
                    player.Out.SendTCP(gSPacketIn);
                }
            }
        }
    }

    public void ApertureEquip(int level)
    {
        EquipShowImp(0, (level < 5) ? 1 : ((level < 7) ? 2 : 3));
    }

    public void BagFullSendToMail(List<ItemInfo> infos)
    {
        if (infos.Count > 0)
        {
            bool flag = false;
            using (new PlayerBussiness())
            {
                flag = SendItemsToMail(infos, "Merhaba değerli Bombom oyuncusu! Sırt çantandaki eşyalar o kadar yer biriktirmiş ki boş yer kalmamış. Lütfen Ek Çanta veya Çelik kasanızı da kontrol edip sırt çantanızda yeterli yer sağlayıp bu öğeyi yeniden envanterinize ekleyin.", "Sırt Çantası Taşkınlığı", eMailType.BuyItem);
            }
            if (flag)
            {
                _ = Out.SendMailResponse(PlayerCharacter.ID, eMailRespose.Receiver);
            }
        }
    }

    public void BeginAllChanges()
    {
        BeginChanges();
        BufferList.BeginChanges();
        EquipBag.BeginChanges();
        PropBag.BeginChanges();
        FarmBag.BeginChanges();
        Vegetable.BeginChanges();
    }

    public void BeginChanges()
    {
        _ = Interlocked.Increment(ref m_changed);
    }

    public void RemoveLotteryItems(int templateId, int count)
    {
        foreach (ItemBoxInfo lotteryItem in LotteryItems)
        {
            if (lotteryItem.TemplateId == templateId && lotteryItem.ItemCount == count)
            {
                _ = LotteryItems.Remove(lotteryItem);
                break;
            }
        }
    }

    public bool CanEquip(ItemTemplateInfo item)
    {
        bool flag = true;
        string message = "";
        if (!item.CanEquip)
        {
            flag = false;
            message = LanguageMgr.GetTranslation("Game.Server.GameObjects.NoEquip");
        }
        else if (UserVIPInfo.Grade < item.NeedLevel)
        {
            flag = false;
            message = LanguageMgr.GetTranslation("Game.Server.GameObjects.CanLevel");
        }
        if (!flag)
        {
            _ = Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, message);
        }
        return flag;
    }

    public int GetVIPNextLevelDaysNeeded(int viplevel, int vipexp)
    {
        if (viplevel != 0 && vipexp > 0 && viplevel <= 8)
        {
            List<int> list = GameProperties.VIPExp();
            ShopItemInfo itemVipInfo = ShopMgr.FindShopbyTemplateID((int)EquipType.VIPCARD);
            int adddaily = itemVipInfo.AValue1 / itemVipInfo.AUnit * 2;

            float result = 0;
            float vipExpCompared = list[viplevel] - (float)vipexp;//so sánh exp hiện tại với vipexp kế tiếp, listIndex start 0 -> 8 tương đương vipLevel 1 -> 9

            if (PlayerCharacter.typeVIP == 2)
            {
                result = vipExpCompared / adddaily;
            }
            else if (PlayerCharacter.typeVIP == 1)
            {
                result = vipExpCompared / adddaily;
            }

            if (result < 0)
            {
                log.Info("GetVIPNextLevelDaysNeeded bug: compared vipexp > nextVipExp by VipLevel! CharacterID :" + UserVIPInfo.ID);
            }

            OnVIPUpgrade(UserVIPInfo.VIPLevel, UserVIPInfo.VIPExp);
            return (int)Math.Ceiling(result > 0 ? result : 0);
        }
        OnVIPUpgrade(UserVIPInfo.VIPLevel, UserVIPInfo.VIPExp);
        return 0;
    }

    public bool canUpLv(int exp, int _curLv)
    {
        List<int> list = GameProperties.VIPExp();
        if (exp >= list[0] && _curLv == 0)
        {
            return true;
        }
        if (exp >= list[1] && _curLv == 1)
        {
            return true;
        }
        if (exp >= list[2] && _curLv == 2)
        {
            return true;
        }
        if (exp >= list[3] && _curLv == 3)
        {
            return true;
        }
        if (exp >= list[4] && _curLv == 4)
        {
            return true;
        }
        if (exp >= list[5] && _curLv == 5)
        {
            return true;
        }
        if (exp >= list[6] && _curLv == 6)
        {
            return true;
        }
        if (exp >= list[7] && _curLv == 7)
        {
            return true;
        }
        return exp >= list[8] && _curLv == 8;
    }

    public bool canDownLv(int exp, int _curLv)
    {
        List<int> list = GameProperties.VIPExp();
        if (_curLv is 0 or 9)
        {
            return false;
        }
        if (exp < list[1] && _curLv == 1)
        {
            return true;
        }
        if (exp < list[2] && _curLv == 2)
        {
            return true;
        }
        if (exp < list[3] && _curLv == 3)
        {
            return true;
        }
        if (exp < list[4] && _curLv == 4)
        {
            return true;
        }
        if (exp < list[5] && _curLv == 5)
        {
            return true;
        }
        if (exp < list[6] && _curLv == 6)
        {
            return true;
        }
        if (exp < list[7] && _curLv == 7)
        {
            return true;
        }
        return exp < list[8] && _curLv == 8;
    }

    public void ClearCaddyBag()
    {
        List<ItemInfo> list = [];
        for (int i = 0; i < CaddyBag.Capalility; i++)
        {
            ItemInfo itemAt = CaddyBag.GetItemAt(i);
            if (itemAt != null)
            {
                ItemInfo itemInfo = ItemInfo.CloneFromTemplate(itemAt.Template, itemAt);
                itemInfo.Count = 1;
                list.Add(itemInfo);
            }
        }
        CaddyBag.ClearBag();
        _ = AddTemplate(list);

    }

    public int GetMedalNum()
    {
        int itemCount = PropBag.GetItemCount(11408);
        int num = 0;
        if (UserVIPInfo.IsConsortia)
        {
            num = ConsortiaBag.GetItemCount(11408);
        }
        int itemCount2 = BankBag.GetItemCount(11408);
        return itemCount + num + itemCount2;
    }

    public bool SendEventLiveRewards(EventLiveInfo eventLiveInfo)
    {
        List<EventLiveGoods> eventGoods = EventLiveMgr.GetEventGoods(eventLiveInfo);
        _ = new List<ItemInfo>();
        foreach (EventLiveGoods item in eventGoods)
        {
            if (item.TemplateID is not -100 and not -200)
            {
                ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(item.TemplateID);
                if (itemTemplateInfo != null)
                {
                    int num = PlayerCharacter.Sex ? 1 : 2;
                    if (itemTemplateInfo.NeedSex != 0 && itemTemplateInfo.NeedSex != num)
                    {
                        continue;
                    }
                    int count = item.Count;
                    for (int i = 0; i < count; i += itemTemplateInfo.MaxCount)
                    {
                        int count2 = (i + itemTemplateInfo.MaxCount > count) ? (count - i) : itemTemplateInfo.MaxCount;
                        ItemInfo itemInfo = ItemInfo.CreateFromTemplate(itemTemplateInfo, count2, 120);
                        if (itemInfo != null)
                        {
                            itemInfo.StrengthenLevel = item.StrengthenLevel;
                            itemInfo.AttackCompose = item.AttackCompose;
                            itemInfo.DefendCompose = item.DefendCompose;
                            itemInfo.AgilityCompose = item.AgilityCompose;
                            itemInfo.LuckCompose = item.LuckCompose;
                            itemInfo.IsBinds = item.IsBind;
                            itemInfo.ValidDate = item.ValidDate;
                            _ = SendItemToMail(itemInfo, LanguageMgr.GetTranslation("Merhaba! Dikkatini ve merakını karşılıksız bırakmadık. Oyunda belirli koşulları başarıyla yerine getirdiğin için özel bir ödül kazandın! Bu ödül, yalnızca detaylara önem veren ve oyunu keşfetmeyi seven oyunculara veriliyor. Ödülün şu anda hesabına tanımlandı. Envanterinde veya ilgili oyun ekranında hemen kullanabilirsin. Küçük bir ipucu: Bu tarz ödüller, oyunda düşündüğünden daha fazla yerde karşına çıkabilir. Keyifli Oyunlar.", eventLiveInfo.Description), LanguageMgr.GetTranslation("Tebrikler! Gizli Ödül!"), eMailType.Manage); //türkçeleştirildi not: yuti
                        }
                    }
                }
            }
            if (item.TemplateID == -100)
            {
                _ = AddGold(item.Count);
            }
            if (item.TemplateID == -200)
            {
                _ = AddMoney(item.Count);
            }
            if (item.TemplateID == -300)
            {
                _ = AddGiftToken(item.Count);
            }
            if (item.TemplateID == -800)
            {
                _ = AddHonor(item.Count);
            }
        }
        return true;
    }

    public void ClearConsortia(bool isclear)
    {
        string sender = LanguageMgr.GetTranslation("Game.Server.GameUtils.ConsortiaBag.Sender");
        string title = LanguageMgr.GetTranslation("Game.Server.GameUtils.ConsortiaBag.Title");
        if (isclear)
        {
            PlayerCharacter.ClearConsortia();
        }

        if (PlayerCharacter.ConsortiaID != 0 || ConsortiaBag.GetItems().Count <= 0)
        {
            return;
        }

        List<ItemInfo> listitem = [];
        foreach (ItemInfo item in ConsortiaBag.GetItems())
        {
            if (item.IsValidItem())
            {
                listitem.Add(item.Clone());
            }
        }
        OnPropertiesChanged();
        _ = QuestInventory.ClearConsortiaQuest();
        //ConsortiaBag.ClearBag();
        //ConsortiaBag.SaveToDatabase();
        //SendItemsToMail(listitem, sender, title, eMailType.StoreCanel);
        _ = ConsortiaBag.SendAllItemsToMail(sender, title, eMailType.StoreCanel);
    }

    public bool ClearFightBag()
    {
        FightBag.ClearBag();
        return true;
    }

    public void ClearFightBuffOneMatch()
    {
        List<BufferInfo> list = [];
        foreach (BufferInfo fightBuff in FightBuffs)
        {
            if (fightBuff != null)
            {
                switch (fightBuff.Type)
                {
                    case (int)BuffType.WorldBossHP:
                    case (int)BuffType.WorldBossHP_MoneyBuff:
                    case (int)BuffType.WorldBossAttrack:
                    case (int)BuffType.WorldBossAttrack_MoneyBuff:
                    case (int)BuffType.WorldBossMetalSlug:
                    case (int)BuffType.WorldBossAncientBlessings:
                    case (int)BuffType.WorldBossAddDamage:
                        list.Add(fightBuff);
                        break;
                }
            }
        }
        foreach (BufferInfo item in list)
        {
            _ = FightBuffs.Remove(item);
        }
        list.Clear();
    }

    public void ClearFootballCard()
    {
        for (int i = 0; i < CardsTakeOut.Length; i++)
        {
            CardsTakeOut[i] = null;
        }
    }

    public void ClearStoreBag()
    {
        for (int i = 0; i < StoreBag.Capalility; i++)
        {
            ItemInfo itemAt = StoreBag.GetItemAt(i);
            if (itemAt != null)
            {
                if (itemAt.Template.BagType == eBageType.PropBag)
                {
                    int place = PropBag.FindFirstEmptySlot();
                    if (PropBag.AddItemTo(itemAt, place))
                    {
                        _ = StoreBag.TakeOutItem(itemAt);
                    }
                }
                else
                {
                    int place = EquipBag.FindFirstEmptySlot(31);
                    if (place > 0)
                    {
                        if (EquipBag.AddItemTo(itemAt, place))
                        {
                            _ = StoreBag.TakeOutItem(itemAt);
                        }
                    }
                }
            }
        }
        List<ItemInfo> items = StoreBag.GetItems();
        if (items.Count > 0)
        {
            _ = StoreBag.SendAllItemsToMail("Sistem", "İade Edilen Ürün", eMailType.StoreCanel); //türkçeleştirildi not: yuti
        }
    }

    public bool ClearTempBag()
    {
        TempBag.ClearBag();
        //TempBag.SaveToDatabase();
        return true;
    }

    public void CommitAllChanges()
    {
        CommitChanges();
        BufferList.CommitChanges();
        EquipBag.CommitChanges();
        PropBag.CommitChanges();
        FarmBag.CommitChanges();
        Vegetable.CommitChanges();
    }

    public void CommitChanges()
    {
        _ = Interlocked.Decrement(ref m_changed);
        OnPropertiesChanged();
    }

    public int ConsortiaFight(int consortiaWin, int consortiaLose, Dictionary<int, Player> players, eRoomType roomType, eGameType gameClass, int totalKillHealth, int count)
    {
        return ConsortiaMgr.ConsortiaFight(consortiaWin, consortiaLose, players, roomType, gameClass, totalKillHealth, count);
    }

    public void ContinousVIP(int days)
    {
        _ = DateTime.Now;
        DateTime dateTime2 = UserVIPInfo.VIPExpireDay = (!(UserVIPInfo.VIPExpireDay < DateTime.Now)) ? UserVIPInfo.VIPExpireDay.AddDays(days) : DateTime.Now.AddDays(days);
        DateTime dateTime3 = dateTime2;
        DateTime now = dateTime3;
        UserVIPInfo.typeVIP = SetTypeVIP(days);
    }

    public string ConverterPvePermission(char[] chArray)
    {
        string text = "";
        for (int i = 0; i < chArray.Length; i++)
        {
            text += chArray[i];
        }
        return text;
    }

    public List<ItemInfo> CopyDrop(int SessionId, int m_missionInfoId)
    {
        List<ItemInfo> info = null;
        _ = DropInventory.CopyDrop(m_missionInfoId, SessionId, ref info);
        return info;
    }

    public void ChargeToUser()
    {
        int money = 0;
        string translation = LanguageMgr.GetTranslation("ChargeToUser.Title");
        using (PlayerBussiness pb = new())
        {
            if (!pb.ChargeToUser(UserVIPInfo.UserName, ref money, UserVIPInfo.NickName))
            {
                return;
            }
            string translation2 = LanguageMgr.GetTranslation("ChargeToUser.Content", money);
            if (money <= 0)
            {
                return;
            }
            OnPropertiesChange();
            _ = SendMailToUser(pb, translation2, translation, eMailType.Manage);
            _ = AddMoney(money);
            OnMoneyCharge(money);
            if (UserVIPInfo.CheckNewWeek())
            {
                OnMoneyChargeWeek(money);
            }
            if (money >= 5000)
            {
                if (UserVIPInfo.Sex == true)
                {
                    SendMessage(LanguageMgr.GetTranslation($"Phú ông [{UserVIPInfo.NickName}] đã đổi thành công {money} xu vào game. Nhanh trí inbox xin xỏ nào!!!"));
                }
                else
                {
                    SendMessage(LanguageMgr.GetTranslation($"Phú bà [{UserVIPInfo.NickName}] đã đổi thành công {money} xu vào game. Nhanh trí inbox xin xỏ nào!!!"));
                }
            }
        }
        _ = Out.SendMailResponse(PlayerCharacter.ID, eMailRespose.Receiver);
        //if (Extra.CheckNoviceActiveOpen(NoviceActiveType.GUNLUK YUKLEME))
        //{
        //  Extra.UpdateEventCondition((int)NoviceActiveType.GUNLUK YUKLEME money, isPlus: true, 0);
        //}
        //f (Extra.CheckNoviceActiveOpen(NoviceActiveType.HAFTALIK YUKLEME)
        //{
        //    Extra.UpdateEventCondition((int)NoviceActiveType.HAFTALIK YUKLEME, money, isPlus: true, 0);
        //}
        if (!PlayerCharacter.IsRecharged)
        {
            PlayerCharacter.IsRecharged = true;
            Out.SendUpdateFirstRecharge(PlayerCharacter.IsRecharged, PlayerCharacter.IsGetAward);
        }
        if (Extra.Info.LeftRoutteRate > 0f)
        {
            int MoneyRate = (int)((float)(money * Extra.Info.LeftRoutteRate) / 100);//50000 + (50000 * 2)/100
            if (MoneyRate > 0)
            {
                _ = SendMoneyMailToUser(LanguageMgr.GetTranslation("GameServer.LeftRotterMail.Title"), LanguageMgr.GetTranslation("GameServer.LeftRotterMail.Content", MoneyRate), MoneyRate, eMailType.BuyItem);
            }
            Extra.Info.LeftRoutteRate = 1f;
            Out.SendLeftRouleteOpen(Extra.Info);
        }
        _ = SaveIntoDatabase();
    }

    public void ChecVipkExpireDay()
    {
        if (UserVIPInfo.IsVIPExpire())
        {
            UserVIPInfo.CanTakeVipReward = false;
            UserVIPInfo.typeVIP = 0;
        }
        else
        {
            UserVIPInfo.CanTakeVipReward = UserVIPInfo.IsLastVIPPackTime();
        }
    }

    public bool DeletePropItem(int place)
    {
        _ = FightBag.RemoveItemAt(place);
        return true;
    }

    public virtual void Disconnect()
    {
        m_client.Disconnect();
    }

    public bool EquipItem(ItemInfo item, int place)
    {
        if (!item.CanEquip() || item.BagType != EquipBag.BagType)
        {
            return false;
        }
        int num = EquipBag.FindItemEpuipSlot(item.Template);
        if ((uint)(num - 9) <= 1u && place switch
        {
            10 => 0,
            9 => 0,
            _ => 1,
        } == 0)
        {
            num = place;
        }
        else if ((num == 7 || num == 8) && (place == 7 || place == 8))
        {
            num = place;
        }
        return EquipBag.MoveItem(item.Place, num, item.Count);
    }

    private void EquipShowImp(int categoryID, int para)
    {
        UpdateHide(UserVIPInfo.Hide + (int)(Math.Pow(10.0, categoryID) * (para - (UserVIPInfo.Hide / (int)Math.Pow(10.0, categoryID) % 10))));
    }

    public bool FindEmptySlot(eBageType bagType)
    {
        PlayerInventory inventory = GetInventory(bagType);
        _ = inventory.FindFirstEmptySlot();
        return inventory.FindFirstEmptySlot() > 0;
    }

    public void FriendsAdd(int playerID, int relation)
    {
        if (!Friends.ContainsKey(playerID))
        {
            Friends.Add(playerID, relation);
        }
        else
        {
            Friends[playerID] = relation;
        }
    }

    public void FriendsRemove(int playerID)
    {
        if (Friends.ContainsKey(playerID))
        {
            _ = Friends.Remove(playerID);
        }
    }

    public double GetBaseAgility()
    {
        return 1.0 - (UserVIPInfo.Agility * 0.001);
    }

    public List<ItemInfo> GetAllEquipItems()
    {
        List<ItemInfo> list = [];
        for (int place = 0; place < EquipBag.BeginSlot; place++)
        {
            ItemInfo item = EquipBag.GetItemAt(place);
            if (item != null)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public double GetBaseAttack()
    {
        ItemInfo weapon = EquipBag.GetItemAt(6);
        ItemInfo head = EquipBag.GetItemAt(0);
        ItemInfo cloth = EquipBag.GetItemAt(4);
        int cardDamage = 0;
        int rankDamage = 0;
        double DamageAvatar = 0.0;
        foreach (UsersCardInfo card in CardBag.GetCards(0, 4))
        {
            ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(card.TemplateID);
            if (itemTemplateInfo != null)
            {
                cardDamage += itemTemplateInfo.Property4 + card.Damage;
            }
        }
        UserRankInfo singleRank = Rank.GetSingleRank(PlayerCharacter.Honor);
        if (singleRank != null && singleRank.IsValidRank())
        {
            rankDamage += singleRank.Damage;
        }
        int baseattack = cardDamage + rankDamage;
        if (weapon != null)
        {
            double property = weapon.Template.Property7;
            int gold = weapon.isGold == true ? 1 : 0;
            double strengthenLevel = weapon.StrengthenLevel + gold;
            //baseattack += (int)(getHertAddition(property, strengthenLevel) + property);
            double weaponattack = getHertAddition(property, strengthenLevel) + property;
            if (weapon.Hole1 > 0)
            {
                BaseAttack(weapon.Hole1, ref baseattack);
            }
            if (weapon.Hole2 > 0)
            {
                BaseAttack(weapon.Hole2, ref baseattack);
            }
            if (weapon.Hole3 > 0)
            {
                BaseAttack(weapon.Hole3, ref baseattack);
            }
            if (weapon.Hole4 > 0)
            {
                BaseAttack(weapon.Hole4, ref baseattack);
            }
            if (weapon.Hole5 > 0)
            {
                BaseAttack(weapon.Hole5, ref baseattack);
            }
            if (weapon.Hole6 > 0)
            {
                BaseAttack(weapon.Hole6, ref baseattack);
            }
            // equipGhost
            UserEquipGhostInfo egInfo = GetGhostEquip(weapon.BagType, weapon.Place);
            if (egInfo != null)
            {
                weaponattack += property / 200 * Math.Pow(egInfo.Level, 1.2) / 100 * weaponattack;
            }
            baseattack += (int)weaponattack;
        }
        if (head != null)
        {
            if (head.Hole1 > 0)
            {
                BaseAttack(head.Hole1, ref baseattack);
            }
            if (head.Hole2 > 0)
            {
                BaseAttack(head.Hole2, ref baseattack);
            }
            if (head.Hole3 > 0)
            {
                BaseAttack(head.Hole3, ref baseattack);
            }
            if (head.Hole4 > 0)
            {
                BaseAttack(head.Hole4, ref baseattack);
            }
            if (head.Hole5 > 0)
            {
                BaseAttack(head.Hole5, ref baseattack);
            }
            if (head.Hole6 > 0)
            {
                BaseAttack(head.Hole6, ref baseattack);
            }
        }
        if (cloth != null)
        {
            if (cloth.Hole1 > 0)
            {
                BaseAttack(cloth.Hole1, ref baseattack);
            }
            if (cloth.Hole2 > 0)
            {
                BaseAttack(cloth.Hole2, ref baseattack);
            }
            if (cloth.Hole3 > 0)
            {
                BaseAttack(cloth.Hole3, ref baseattack);
            }
            if (cloth.Hole4 > 0)
            {
                BaseAttack(cloth.Hole4, ref baseattack);
            }
            if (cloth.Hole5 > 0)
            {
                BaseAttack(cloth.Hole5, ref baseattack);
            }
            if (cloth.Hole6 > 0)
            {
                BaseAttack(cloth.Hole6, ref baseattack);
            }
        }
        List<UserAvatarCollectionInfo> avatarPropertyActived = AvatarCollect.GetAvatarPropertyActived();
        if (avatarPropertyActived.Count > 0)
        {
            foreach (UserAvatarCollectionInfo current3 in avatarPropertyActived)
            {
                ClothPropertyTemplateInfo clothProperty = current3.ClothProperty;
                if (clothProperty != null)
                {
                    int num8 = ClothGroupTemplateInfoMgr.CountClothGroupWithID(current3.AvatarID);
                    if (current3.Items.Count >= num8 / 2 && current3.Items.Count < num8)
                    {
                        DamageAvatar += clothProperty.Damage;
                    }
                    else if (current3.Items.Count == num8)
                    {
                        DamageAvatar += clothProperty.Damage * 2;
                    }
                }
            }
        }
        List<ItemInfo> allEquipItems = GetAllEquipItems();
        foreach (ItemInfo item in allEquipItems)
        {
            SubActiveConditionInfo info = SubActiveMgr.GetSubActiveInfo(item);
            if (info != null)
            {
                baseattack += info.GetValue(6);
            }
        }
        PlayerProp.UpadateBaseProp(true, "Damage", "Avatar", DamageAvatar);
        baseattack += TotemMgr.GetTotemProp(UserVIPInfo.totemId, "dam");
        return baseattack + DamageAvatar;
    }

    public void BaseAttack(int template, ref int baseattack)
    {
        ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(template);
        if (itemTemplateInfo != null && itemTemplateInfo.CategoryID == 11 && itemTemplateInfo.Property1 == 31 && itemTemplateInfo.Property2 == 3)
        {
            baseattack += itemTemplateInfo.Property7;
        }
    }

    public double GetBaseBlood()
    {
        ItemInfo info = EquipBag.GetItemAt(12);
        if (info != null)
        {
            //return (100.0 + (double)itemAt.Template.Property1) / 100.0;
            return (100.0 + info.Template.Property1 + PlayerCharacter.necklaceExpAdd) / 100.0;
        }
        return 1.0;
    }

    public double GetBaseDefence()
    {
        int defence = 0;
        int basedefence = 0;
        double GuardAvatar = 0.0;
        SetsBuildTempMgr.GetSetsBuildProp(PlayerCharacter.fineSuitExp, ref defence);
        foreach (UsersCardInfo card in CardBag.GetCards(0, 4))
        {
            ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(card.TemplateID);
            if (itemTemplateInfo != null)
            {
                basedefence += itemTemplateInfo.Property5 + card.Guard;
            }
        }
        UserRankInfo singleRank = Rank.GetSingleRank(PlayerCharacter.Honor);
        if (singleRank != null && singleRank.IsValidRank())
        {
            basedefence += singleRank.Guard;
        }
        PlayerProp.UpadateBaseProp(isSelf: true, "Armor", "Pet", HoGiap);
        ItemInfo weapon = EquipBag.GetItemAt(6);
        ItemInfo head = EquipBag.GetItemAt(0);
        ItemInfo cloth = EquipBag.GetItemAt(4);
        if (head != null)
        {
            double property = head.Template.Property7;
            int gold = head.isGold ? 1 : 0;
            double strengthenLevel = head.StrengthenLevel + gold;
            defence += (int)(getHertAddition(property, strengthenLevel) + property);
            // equipGhost
            UserEquipGhostInfo egInfo = GetGhostEquip(head.BagType, head.Place);
            if (egInfo != null)
            {
                defence += (int)(property / 60 * Math.Pow(egInfo.Level, 1.2) / 100) * defence;
            }
            AddProperty(head, ref defence);
        }
        if (cloth != null)
        {
            double property = cloth.Template.Property7;
            int gold = cloth.isGold ? 1 : 0;
            double strengthenLevel = cloth.StrengthenLevel + gold;
            defence += (int)(getHertAddition(property, strengthenLevel) + property);
            UserEquipGhostInfo egInfo = GetGhostEquip(cloth.BagType, cloth.Place);
            if (egInfo != null)
            {
                defence += (int)(property / 60 * Math.Pow(egInfo.Level, 1.2) / 100) * defence;
            }
            AddProperty(cloth, ref defence);
        }
        if (weapon != null)
        {
            AddProperty(weapon, ref defence);
        }
        defence += basedefence;
        List<UserAvatarCollectionInfo> avatarPropertyActived = AvatarCollect.GetAvatarPropertyActived();
        if (avatarPropertyActived.Count > 0)
        {
            foreach (UserAvatarCollectionInfo current2 in avatarPropertyActived)
            {
                ClothPropertyTemplateInfo clothProperty = current2.ClothProperty;
                if (clothProperty != null)
                {
                    int num14 = ClothGroupTemplateInfoMgr.CountClothGroupWithID(current2.AvatarID);
                    if (current2.Items.Count >= num14 / 2 && current2.Items.Count < num14)
                    {
                        GuardAvatar += clothProperty.Guard;
                    }
                    else if (current2.Items.Count == num14)
                    {
                        GuardAvatar += clothProperty.Guard * 2;
                    }
                }
            }
        }
        List<ItemInfo> allEquipItems = GetAllEquipItems();
        foreach (ItemInfo item in allEquipItems)
        {
            SubActiveConditionInfo info = SubActiveMgr.GetSubActiveInfo(item);
            if (info != null)
            {
                defence += info.GetValue(7);
            }
        }
        PlayerProp.UpadateBaseProp(true, "Armor", "Avatar", GuardAvatar);
        defence += TotemMgr.GetTotemProp(UserVIPInfo.totemId, "gua");
        return defence + GuardAvatar + HoGiap;
    }

    public void AddProperty(ItemInfo item, ref int defence)
    {
        if (item.Hole1 > 0)
        {
            BaseDefence(item.Hole1, ref defence);
        }
        if (item.Hole2 > 0)
        {
            BaseDefence(item.Hole2, ref defence);
        }
        if (item.Hole3 > 0)
        {
            BaseDefence(item.Hole3, ref defence);
        }
        if (item.Hole4 > 0)
        {
            BaseDefence(item.Hole4, ref defence);
        }
        if (item.Hole5 > 0)
        {
            BaseDefence(item.Hole5, ref defence);
        }
        if (item.Hole6 > 0)
        {
            BaseDefence(item.Hole6, ref defence);
        }
    }

    public void BaseDefence(int template, ref int defence)
    {
        ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(template);
        if (itemTemplateInfo != null && itemTemplateInfo.CategoryID == 11 && itemTemplateInfo.Property1 == 31 && itemTemplateInfo.Property2 == 3)
        {
            defence += itemTemplateInfo.Property8;
        }
    }

    public void PVEFightMessage(string translation, ItemInfo itemInfo, int areaID)
    {
        if (translation != null)
        {
            GSPacketIn packet = WorldMgr.SendSysNotice(eMessageType.ChatNormal, translation, (itemInfo.ItemID == 0) ? 1 : itemInfo.ItemID, itemInfo.TemplateID, "");
            GameServer.Instance.LoginServer.SendPacket(packet);
        }
    }

    public void PVEFightNotice(string msg)
    {
        if (msg != null)
        {
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            for (int i = 0; i < allPlayers.Length; i++)
            {
                _ = allPlayers[i].Out.SendMessage(eMessageType.ChatNormal, msg);
            }
        }
    }

    public void PVERewardNotice(string msg, int itemID, int templateID)
    {
        if (msg != null)
        {
            GSPacketIn packet = WorldMgr.SendSysNotice(eMessageType.ChatNormal, msg, itemID, templateID, null);
            GameServer.Instance.LoginServer.SendPacket(packet);
        }
    }


    public void PVPFightMessage(string translation, ItemInfo itemInfo, int areaID)
    {
        if (translation != null)
        {
            GSPacketIn packet = WorldMgr.SendSysNotice(eMessageType.ChatNormal, translation, (itemInfo.ItemID == 0) ? 1 : itemInfo.ItemID, itemInfo.TemplateID, "");
            GameServer.Instance.LoginServer.SendPacket(packet);
        }
    }

    public double getHertAddition(double para1, double para2)
    {
        return Math.Round((para1 * Math.Pow(1.1, para2)) - para1);
    }

    public PlayerInventory GetInventory(eBageType bageType)
    {
        switch (bageType)
        {
            case eBageType.CaddyBag:
                return CaddyBag;
            case eBageType.Consortia:
                return ConsortiaBag;
            case eBageType.FarmBag:
                return FarmBag;
            case eBageType.Vegetable:
                return Vegetable;
            case eBageType.EquipBag:
                return EquipBag;
            case eBageType.FightBag:
                return FightBag;
            case eBageType.Food:
                return Food;
            case eBageType.PetEgg:
                return m_petEgg;
            case eBageType.PropBag:
                return PropBag;
            case eBageType.Store:
                return StoreBag;
            case eBageType.TempBag:
                return TempBag;
            case eBageType.BankBag:
                return BankBag;
            default:
                log.Error($"Did not support this type bag: {bageType} PlayerID: {PlayerCharacter.ID} Nickname: {PlayerCharacter.NickName}");
                return null;
        }
    }

    public string GetInventoryName(eBageType bageType)
    {
        return bageType switch
        {
            eBageType.EquipBag => LanguageMgr.GetTranslation("Game.Server.GameObjects.Equip"),
            eBageType.PropBag => LanguageMgr.GetTranslation("Game.Server.GameObjects.Prop"),
            eBageType.FightBag => LanguageMgr.GetTranslation("Game.Server.GameObjects.FightBag"),
            eBageType.BeadBag => LanguageMgr.GetTranslation("Game.Server.GameObjects.BeadBag"),
            eBageType.FarmBag => LanguageMgr.GetTranslation("Game.Server.GameObjects.FarmBag"),
            _ => bageType.ToString(),
        };
    }

    public ItemInfo GetItemAt(eBageType bagType, int place)
    {
        return GetInventory(bagType)?.GetItemAt(place);
    }

    public ItemInfo GetItemByTemplateID(int templateID)
    {
        ItemInfo itemByTemplateID = GetInventory(eBageType.EquipBag).GetItemByTemplateID(31, templateID);
        itemByTemplateID ??= GetInventory(eBageType.PropBag).GetItemByTemplateID(0, templateID);
        itemByTemplateID ??= GetInventory(eBageType.Consortia).GetItemByTemplateID(0, templateID);
        itemByTemplateID ??= GetInventory(eBageType.BankBag).GetItemByTemplateID(0, templateID);
        return itemByTemplateID;
    }

    public int GetItemCount(int templateId)
    {
        return PropBag.GetItemCount(templateId) + EquipBag.GetItemCount(templateId) + ConsortiaBag.GetItemCount(templateId) + BankBag.GetItemCount(templateId);
    }

    public PlayerInventory GetItemInventory(ItemTemplateInfo template)
    {
        return GetInventory(template.BagType);
    }

    public void HideEquip(int categoryID, bool hide)
    {
        if (categoryID is >= 0 and < 10)
        {
            EquipShowImp(categoryID, (!hide) ? 1 : 2);
        }
    }

    public char[] InitPvePermission()
    {
        char[] array = new char[500];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = '1';
        }
        return array;
    }

    public bool IsBlackFriend(int playerID)
    {
        if (Friends != null)
        {
            return Friends.ContainsKey(playerID) && Friends[playerID] == 1;
        }
        return true;
    }

    public bool IsConsortia()
    {
        return ConsortiaMgr.FindConsortiaInfo(PlayerCharacter.ConsortiaID) != null;
    }

    public bool IsLimitCount(int count)
    {
        if (GameProperties.IsLimitCount && count > GameProperties.LimitCount)
        {
            SendMessage($"O limite de {GameProperties.LimitCount} foi alcançado.");
            return true;
        }
        return false;
    }

    public bool IsLimitMoney(int count)
    {
        if (GameProperties.IsLimitMoney && count > GameProperties.LimitMoney)
        {
            SendMessage($"O limite de {GameProperties.LimitMoney} foi alcançado de cupons.");
            return true;
        }
        return false;
    }

    public bool IsPveEpicPermission(int copyId)
    {
        string text = "1-2-3-4-5-6-7-8-9-10-11-12-13";
        if (text.Length > 0)
        {
            string[] array = text.Split('-');
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == copyId.ToString())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool UsePayBuff(BuffType type)
    {
        bool result = false;
        AbstractBuffer ofType = BufferList.GetOfType(type);
        if (ofType?.Check() ?? false)
        {
            ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(ofType.Info.TemplateID);
            if (itemTemplateInfo != null)
            {
                if (itemTemplateInfo.Property3 > 0 && ofType.Info.ValidCount > 0)
                {
                    ofType.Info.ValidCount--;
                    BufferList.UpdateBuffer(ofType);
                    result = true;
                }
                else if (itemTemplateInfo.Property3 == 0)
                {
                    result = true;
                }
            }
        }
        return result;
    }

    public bool IsPvePermission(int copyId, eHardLevel hardLevel)
    {
        return copyId > m_pvepermissions.Length || copyId <= 0 || m_pvepermissions[copyId - 1] >= permissionChars[(int)hardLevel];
    }

    public void OnPropertiesChange()
    {
        PropertiesChange?.Invoke(PlayerCharacter);
    }

    public void LastVIPPackTime()
    {
        UserVIPInfo.LastVIPPackTime = DateTime.Now;
        UserVIPInfo.CanTakeVipReward = false;
    }

    public virtual bool LoadFromDatabase()
    {
        bool result = false;
        using PlayerBussiness pb = new();
        PlayerInfo info = pb.GetUserSingleByUserID(UserVIPInfo.ID);
        if (info == null)
        {
            Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.Forbid"));
            Client.Disconnect();
            result = false;
        }
        else
        {
            TimeCheckHack = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            UserVIPInfo = info;
            BattleData.LoadFromDatabase();
            BattleData.UpdateLeagueGrade();
            UserVIPInfo.Texp = pb.GetUserTexpInfoSingle(UserVIPInfo.ID);
            if (UserVIPInfo.Texp.IsValidadteTexp())
            {
                UserVIPInfo.Texp.texpCount = 0;
            }
            int[] updatedSlots = new int[6]
            {
                0,
                1,
                2,
                3,
                4,
                5
            };
            Out.SendUpdateInventorySlot(FightBag, updatedSlots);
            UpdateWeaklessGuildProgress();
            UpdateItemForUser(1);
            ChecVipkExpireDay();
            EventSeven = pb.GetEventSevenDays(GameServer.Instance.Configuration.ZoneId);
            UpdateLevel();
            UpdatePet(PetBag.GetPetIsEquip());
            if (UserVIPInfo.CheckNewDay())
            {
                TimeSpan diff = DateTime.Now - UserVIPInfo.NewDay;
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    CheckAndSendWeeklyHonorReward();
                }

                if ((int)Math.Ceiling(diff.TotalDays) >= 7)//15 gün girilmez sayan değer 7'ye düşürüldü eski oyuncu ödülü not: yuti
                {
                    DateTime startDate = Convert.ToDateTime(GameProperties.StartEventOldPlayer);
                    DateTime stopDate = Convert.ToDateTime(GameProperties.EndEventOldPlayer);
                    if (DateTime.Now >= startDate && DateTime.Now < stopDate)
                    {
                        //int Money = 1000; //3000000 olan değer 1000e düşürüldü not: yuti
                        string msg = "Eski oyuncumuz [" + UserVIPInfo.NickName + "] Bombom'a geri hoşgeldi. Eski oyuncu loncasına büyük miktarda varlık getirebilir."; //türkçeleştirildi not: yuti
                        string Title = "Eski Oyuncu Geri Dönüşü"; //türkçeleştirildi not: yuti
                        string Cotent = "Tebrikler! 🎉 Uzun bir aradan sonra aramıza geri döndüğün için seni özel olarak karşılıyoruz! Bu ödüllerle macerana daha güçlü devam edebilirsin. Yeniden aramızda olman bizi çok mutlu etti! İyi oyunlar dileriz!"; //türkçeleştirildi not: yuti
                        List<ItemInfo> items = [];
                        foreach (OldPlayerAwardInfo oldPlayerAward in OldPlayerAwardMgr.oldPlayerAwards)
                        {
                            items.Add(oldPlayerAward.itemInfo);
                        }
                        //AddMoneyLock(Money);

                        UserVIPInfo.IsOldPlayer = true;
                        UserVIPInfo.isOldPlayerHasValidEquitAtLogin = true;
                        _ = SendItemsToMail(items, Cotent, Title, eMailType.ItemOverdue);
                        _ = Out.SendMailResponse(PlayerCharacter.ID, eMailRespose.Receiver);
                        GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                        for (int i = 0; i < allPlayers.Length; i++)
                        {
                            _ = allPlayers[i].Out.SendMessage(eMessageType.SYS_NOTICE, msg);
                        }
                    }
                }
                string content = $"Tekrardan selamlar {PlayerCharacter.NickName}, günlük maceran seni bekliyor! \r\n" +
                                 $"⏰ Giriş Zamanı: {DateTime.Now:HH:mm} \r\n" +
                                 $"✨ Bugün Sizi Neler Bekliyor? \r\n" +
                                 $"• Günlük görevleriniz sıfırlandı - yeni ödüller kazanmaya hazır olun! \r\n" +
                                 $"🎮 İyi oyunlar dileriz!";

                string title = $"{PlayerCharacter.NickName}, Yeni Güne Hoş Geldin!";
                _ = SendMailToUser(pb, content, title, eMailType.Manage);
                //this.QuestInventory.Restart();
                QuestInventory.ResetDailyQuest();
                QuestInventory.LoadFromDatabase(PlayerCharacter.ID);
                OnPlayerLogin();
                UserVIPInfo.NewDay = DateTime.Now;
                UserVIPInfo.BoxGetDate = DateTime.Now;
                UserVIPInfo.damageScores = 0;
                UserVIPInfo.Score = 0;
                UserVIPInfo.DailyMoneyUsed = 0;
                BattleData.Reset();
                Extra.Info.MinHotSpring = 60;
                Extra.Info.LastFreeTimeHotSpring = DateTime.Now;
                Extra.Info.FreeSendMailCount = 0;
                Extra.Info.LeftRoutteCount = GameProperties.LeftRouterMaxDay;
                Extra.Info.LeftRoutteRate = 0f;
                //Extra.ResetNoviceEvent(NoviceActiveType.DISCORD_HOPARLORU);
                Extra.ResetNoviceEvent(NoviceActiveType.Gunluk_Harcama);
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    //Extra.ResetNoviceEvent(NoviceActiveType.IKI_VS_IKI_SAVAS);
                    Extra.ResetNoviceEvent(NoviceActiveType.Haftalik_Harcama);
                }
                UserVIPInfo.MaxBuyHonor = 0;
                Farm.ResetFarmProp();
                AccumulativeUpdate();
                _ = ChangeDailyExpVip();
            }
            if (UserVIPInfo.Grade > 30)
            {
                LoadGemStone(pb);
            }
            m_pvepermissions = string.IsNullOrEmpty(UserVIPInfo.PvePermission) ? InitPvePermission() : UserVIPInfo.PvePermission.ToCharArray();
            m_fightlabpermissions = string.IsNullOrEmpty(UserVIPInfo.FightLabPermission) ? InitFightLabPermission() : UserVIPInfo.FightLabPermission.ToCharArray();
            LoadPvePermission();
            Friends = [];
            Friends = pb.GetFriendsIDAll(UserVIPInfo.ID);
            ViFarms = [];
            UserVIPInfo.State = 1;
            ClearStoreBag();
            ClearCaddyBag();
            //m_equipGhostList = JsonConvert.DeserializeObject<Dictionary<string, UserEquipGhostInfo>>(UserVIPInfo.GhostEquipList);
           // m_equipGhostList ??= [];
            PlayerCharacter.VIPNextLevelDaysNeeded = GetVIPNextLevelDaysNeeded(PlayerCharacter.VIPLevel, PlayerCharacter.VIPExp);
            if (UserVIPInfo.totemId > TotemMgr.MaxTotem())
            {
                UserVIPInfo.totemId = TotemMgr.MaxTotem();
            }



            _ = pb.UpdateUserTexpInfo(UserVIPInfo.Texp);
            _ = pb.UpdatePlayer(UserVIPInfo);
            _ = pb.UpdateUserMatchInfo(MatchInfo);
            LoadMedals();
            LoadRepute();
            _ = SaveIntoDatabase();
            _ = SavePlayerInfo();
            result = true;
        }
        return result;
    }
    /// <summary>
    /// VIP günlük deneyim puanlarını günceller.
    /// VIP üyeler için bonus XP ekler ve seviye atlama kontrolü yapar.
    /// Aktif VIP'siz kullanıcılar için ise XP düşer.
    /// </summary>
    /// <returns>İşlem başarılı ise true, aksi halde false</returns>
    public bool ChangeDailyExpVip()
    {
        // VIP kartı şablon bilgilerini al
        const int VIP_CARD_TEMPLATE_ID = (int)EquipType.VIPCARD;
        ShopItemInfo vipCardItem = ShopMgr.FindShopbyTemplateID(VIP_CARD_TEMPLATE_ID);

        // VIP kartı markette bulunamazsa işlemi iptal et
        if (vipCardItem == null)
        {
            LogError($"VIP kartı (TemplateID: {VIP_CARD_TEMPLATE_ID}) market veritabanında bulunamadı.");
            return false;
        }

        // Maksimum VIP seviyesi kontrolü (Seviye 9+ için günlük XP verilmez)
        const int MAX_VIP_LEVEL_FOR_DAILY_EXP = 9;
        if (UserVIPInfo.VIPLevel >= MAX_VIP_LEVEL_FOR_DAILY_EXP)
        {
            LogInfo($"Karakter {UserVIPInfo.NickName} zaten maksimum VIP seviyesinde ({UserVIPInfo.VIPLevel}). Günlük XP atlandı.");
            return false;
        }

        // Günlük VIP XP miktarını hesapla (Birim başına değer)
        int dailyVipExpAmount = CalculateDailyVipExperience(vipCardItem);

        // VIP üyelik durumuna göre işlem yap
        if (IsVipMembershipActive())
        {
            ProcessVipMemberDailyBonus(dailyVipExpAmount);
        }
        else
        {
            ProcessNonVipDailyPenalty(dailyVipExpAmount);
        }

        return true;
    }

    /// <summary>
    /// Market item'ından günlük VIP XP miktarını hesaplar
    /// </summary>
    private int CalculateDailyVipExperience(ShopItemInfo vipItem)
    {
        if (vipItem.AUnit <= 0)
        {
            LogWarning($"VIP kartı birim değeri sıfır veya negatif: {vipItem.AUnit}. Varsayılan 1 kullanılıyor.");
            return vipItem.AValue1;
        }

        return vipItem.AValue1 / vipItem.AUnit;
    }

    /// <summary>
    /// Kullanıcının aktif VIP üyeliği olup olmadığını kontrol eder
    /// </summary>
    private bool IsVipMembershipActive()
    {
        return UserVIPInfo.typeVIP > 0;
    }

    /// <summary>
    /// Aktif VIP üyeler için günlük bonus XP ekler ve seviye atlama kontrolü yapar
    /// </summary>
    private void ProcessVipMemberDailyBonus(int baseExpAmount)
    {
        // VIP üyeler 2x XP kazanır
        int bonusExpAmount = baseExpAmount * 2;

        // VIP deneyimini ekle (void metod - AddExpVip içinde seviye atlama kontrolü var)
        AddExpVip(bonusExpAmount);

        // VIP penceresini güncelle (AddExpVip içinde de çağrılabilir ama garanti olsun)
        _ = Out.SendOpenVIP(this);

        // Sonraki seviyeye kalan günleri güncelle
        UpdateVipNextLevelProgress();

        // Oyuncuya bildirim gönder
        string welcomeMessage = BuildVipWelcomeMessage(bonusExpAmount);
        SendMessage(welcomeMessage);

        LogInfo($"VIP günlük bonus verildi. Karakter: {UserVIPInfo.NickName}, XP: +{bonusExpAmount}, Mevcut Seviye: {UserVIPInfo.VIPLevel}");
    }

    /// <summary>
    /// VIP'siz kullanıcılar için günlük XP düşürme işlemi
    /// </summary>
    private void ProcessNonVipDailyPenalty(int expAmount)
    {
        // VIP'siz kullanıcılar için XP düşür (RemoveExpVip varsa kullan, yoksa manuel düşür)
        bool expRemoved = TryRemoveVipExperience(expAmount);

        if (expRemoved)
        {
            string penaltyMessage = BuildVipPenaltyMessage(expAmount);
            SendMessage(penaltyMessage);

            LogInfo($"VIP pasif cezası uygulandı. Karakter: {UserVIPInfo.NickName}, XP: -{expAmount}");
        }
        else
        {
            LogWarning($"VIP XP düşürülemedi. Karakter: {UserVIPInfo.NickName}, Miktar: {expAmount}");
        }
    }

    /// <summary>
    /// VIP deneyim puanı düşürme işlemini dener
    /// </summary>
    private bool TryRemoveVipExperience(int amount)
    {
        try
        {
            // Eğer RemoveExpVip metodu varsa onu kullan
            // RemoveExpVip(amount);

            // Yoksa manuel düşür (AddExpVip'in tersi)
            if (UserVIPInfo.VIPExp >= amount)
            {
                UserVIPInfo.VIPExp -= amount;
            }
            else
            {
                UserVIPInfo.VIPExp = 0; // Negatif olmasın
            }

            return true;
        }
        catch (Exception ex)
        {
            LogError($"VIP XP düşürme hatası: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Sonraki VIP seviyesine ulaşmak için gereken gün sayısını günceller
    /// </summary>
    private void UpdateVipNextLevelProgress()
    {
        UserVIPInfo.VIPNextLevelDaysNeeded = GetVIPNextLevelDaysNeeded(
            UserVIPInfo.VIPLevel,
            UserVIPInfo.VIPExp
        );
    }

    /// <summary>
    /// VIP üyeler için hoş geldin mesajı oluşturur
    /// </summary>
    private string BuildVipWelcomeMessage(int expAmount)
    {
        int nextLevel = UserVIPInfo.VIPLevel + 1;
        string levelUpHint = nextLevel <= 9 ? $" VIP {nextLevel} olmaya çok yakınsınız!" : " Maksimum VIP seviyesindesiniz!";

        return $"🌟 Tekrar Hoş Geldiniz, {UserVIPInfo.NickName}! " +
               $"VIP üyeliğiniz sayesinde bugün {expAmount} bonus deneyim puanı kazandınız!{levelUpHint} " +
               $"Şu an VIP {UserVIPInfo.VIPLevel} ({UserVIPInfo.VIPExp} XP) seviyesindesiniz.";
    }

    /// <summary>
    /// VIP'siz kullanıcılar için uyarı mesajı oluşturur
    /// </summary>
    private string BuildVipPenaltyMessage(int expAmount)
    {
        int remainingExp = UserVIPInfo.VIPExp;
        string warningLevel = remainingExp < 100 ? " VIP seviyeniz kritik düzeyde!" : "";

        return $"⚠️ Yeni güne başladınız fakat aktif VIP üyeliğiniz bulunmuyor. " +
               $"Hesabınızdan {expAmount} VIP deneyim puanı düşüldü.{warningLevel} " +
               $"Kalan XP: {remainingExp}. " +
               $"VIP kartı satın alarak kazancınızı 2'ye katlayabilir ve seviye kaybını önleyebilirsiniz!";
    }

    // Yardımcı log metodları
    private void LogInfo(string message)
    {
        Console.WriteLine($"[INFO] {DateTime.Now}: {message}");
    }

    private void LogWarning(string message)
    {
        Console.WriteLine($"[WARN] {DateTime.Now}: {message}");
    }

    private void LogError(string message)
    {
        Console.WriteLine($"[ERROR] {DateTime.Now}: {message}");
    }

    public char[] InitFightLabPermission()
    {
        char[] array = new char[50];
        for (int i = 0; i < 50; i++)
        {
            array[i] = i == 0 ? '1' : '0';
        }
        return array;
    }

    public bool SetFightLabPermission(int copyId, eHardLevel hardLevel, int missionId)
    {
        switch (copyId)
        {
            case 1000:
                copyId = 5;
                break;
            case 1001:
                copyId = 6;
                break;
            case 1002:
                copyId = 7;
                break;
            case 1003:
                copyId = 8;
                break;
            case 1004:
                copyId = 9;
                break;
        }
        if (copyId > m_fightlabpermissions.Length || copyId <= 0)
        {
            return true;
        }
        int num = (copyId - 5) * 2;
        if (m_fightlabpermissions[num] != fightlabpermissionChars[(int)(hardLevel + 1)])
        {
            return true;
        }
        if (m_fightlabpermissions[num + 1] <= '2' && m_fightlabpermissions[num] - m_fightlabpermissions[num + 1] == 1)
        {
            m_fightlabpermissions[num + 1] = m_fightlabpermissions[num];
            string text = "";
            int gold = 0;
            int money = 0;
            int giftToken = 0;
            int gp = 0;
            List<ItemInfo> info = [];
            if (DropInventory.FightLabUserDrop(missionId, ref info) && info != null)
            {
                bool flag = false;
                text = LanguageMgr.GetTranslation("Eğitim Alanından Kazandığız Ödüller") + ": "; //türkçeleştirildi not: yuti
                foreach (ItemInfo item in info)
                {
                    text = text + LanguageMgr.GetTranslation("Game.Server.Quests.FinishQuest.RewardProp", item.Template.Name, item.Count) + " ";
                    if (info.Count > 0 && PropBag.GetEmptyCount() < 1)
                    {
                        if (item.TemplateID is not 11107 and not -100 and not -200 and not -300)
                        {
                            string translation = LanguageMgr.GetTranslation("Game.Server.GameUtils.Content2");
                            string translation2 = LanguageMgr.GetTranslation("Game.Server.GameUtils.Title2");
                            if (SendItemsToMail(
                            [
                                item
                            ], translation, translation2, eMailType.ItemOverdue))
                            {
                                _ = Out.SendMailResponse(PlayerCharacter.ID, eMailRespose.Receiver);
                            }
                            flag = true;
                        }
                    }
                    else if (!PropBag.StackItemToAnother(item) && item.TemplateID != 11107 && item.TemplateID != -100 && item.TemplateID != -200 && item.TemplateID != -300)
                    {
                        _ = PropBag.AddItem(item);
                    }
                    _ = ItemInfo.FindSpecialItemInfo(item, ref gold, ref money, ref giftToken, ref gp);
                }
                _ = AddGold(gold);
                _ = AddMoney(money);
                _ = AddGiftToken(giftToken);
                _ = AddGP(gp, false);
                if (flag)
                {
                    text += LanguageMgr.GetTranslation("Game.Server.GameUtils.Title2");
                }
                _ = Out.SendMessage(eMessageType.GM_NOTICE, text);
            }
        }
        if (copyId == 5 && hardLevel == eHardLevel.Normal)
        {
            if (m_fightlabpermissions[2] == '0')
            {
                m_fightlabpermissions[2] = '1';
            }
            if (m_fightlabpermissions[4] == '0')
            {
                m_fightlabpermissions[4] = '1';
            }
            if (m_fightlabpermissions[6] == '0')
            {
                m_fightlabpermissions[6] = '1';
            }
        }
        if ((copyId == 7 || copyId == 8) && hardLevel == eHardLevel.Hard && m_fightlabpermissions[8] == '0')
        {
            m_fightlabpermissions[8] = '1';
        }
        if (hardLevel < eHardLevel.Hard && m_fightlabpermissions[num] < fightlabpermissionChars[(int)(hardLevel + 2)])
        {
            m_fightlabpermissions[num] = fightlabpermissionChars[(int)(hardLevel + 2)];
        }
        UserVIPInfo.FightLabPermission = new string(m_fightlabpermissions).ToString();
        OnPropertiesChanged();
        return true;
    }

    public bool IsFightLabPermission(int copyId, eHardLevel hardLevel)
    {
        if (copyId > m_fightlabpermissions.Length || copyId <= 0)
        {
            return true;
        }
        int num = (copyId - 5) * 2;
        return m_fightlabpermissions[num] >= fightlabpermissionChars[(int)(hardLevel + 1)];
    }

    public eHardLevel GetMaxFightLabPermission(int copyId)
    {
        return copyId > m_fightlabpermissions.Length
            ? eHardLevel.Simple
            : m_fightlabpermissions[copyId - 5] switch
            {
                '3' => eHardLevel.Hard,
                '2' => eHardLevel.Normal,
                _ => eHardLevel.Simple,
            };
    }

    public void LoadMedals()
    {
        UserVIPInfo.medal = GetMedalNum();
        _ = SavePlayerInfo();
    }
    public void LoadRepute()
    {
        PlayerBussiness db = new();
        UserVIPInfo.Repute = db.GetXepHang(UserVIPInfo.ID);
        _ = SavePlayerInfo();
    }

    public void LoadMarryMessage()
    {
        using PlayerBussiness playerBussiness = new();
        MarryApplyInfo[] playerMarryApply = playerBussiness.GetPlayerMarryApply(PlayerCharacter.ID);
        if (playerMarryApply == null)
        {
            return;
        }
        MarryApplyInfo[] array = playerMarryApply;
        MarryApplyInfo[] array2 = array;
        MarryApplyInfo[] array3 = array2;
        foreach (MarryApplyInfo marryApplyInfo in array3)
        {
            switch (marryApplyInfo.ApplyType)
            {
                case 1:
                    _ = Out.SendPlayerMarryApply(this, marryApplyInfo.ApplyUserID, marryApplyInfo.ApplyUserName, marryApplyInfo.LoveProclamation, marryApplyInfo.ID);
                    break;
                case 2:
                    _ = Out.SendMarryApplyReply(this, marryApplyInfo.ApplyUserID, marryApplyInfo.ApplyUserName, marryApplyInfo.ApplyResult, isApplicant: true, marryApplyInfo.ID);
                    if (!marryApplyInfo.ApplyResult)
                    {
                        _ = Out.SendMailResponse(PlayerCharacter.ID, eMailRespose.Receiver);
                    }
                    break;
                case 3:
                    _ = Out.SendPlayerDivorceApply(this, result: true, isProposer: false);
                    break;
            }
        }
    }

    public void LoadMarryProp()
    {
        using PlayerBussiness playerBussiness = new();
        MarryProp marryProp = playerBussiness.GetMarryProp(PlayerCharacter.ID);
        PlayerCharacter.IsMarried = marryProp.IsMarried;
        PlayerCharacter.SpouseID = marryProp.SpouseID;
        PlayerCharacter.SpouseName = marryProp.SpouseName;
        PlayerCharacter.IsCreatedMarryRoom = marryProp.IsCreatedMarryRoom;
        PlayerCharacter.SelfMarryRoomID = marryProp.SelfMarryRoomID;
        PlayerCharacter.IsGotRing = marryProp.IsGotRing;
        _ = Out.SendMarryProp(this, marryProp);
    }

    public void LoadPvePermission()
    {
        PveInfo[] pveInfo = PveInfoMgr.GetPveInfo();
        PveInfo[] array = pveInfo;
        PveInfo[] array2 = array;
        foreach (PveInfo pveInfo2 in array2)
        {
            if (UserVIPInfo.Grade > pveInfo2.LevelLimits)
            {
                eHardLevel level = (pveInfo2.ID is 1 or 2 or 7 or 12 or 13) ? eHardLevel.Easy : eHardLevel.Normal;
                _ = SetPvePermission(pveInfo2.ID, level);
                //if (flag)
                //{
                //    flag = SetPvePermission(pveInfo2.ID, eHardLevel.Normal);
                //}
                //if (flag)
                //{
                //    flag = SetPvePermission(pveInfo2.ID, eHardLevel.Hard);
                //}
            }
        }
    }

    public void LogAddMoney(AddMoneyType masterType, AddMoneyType sonType, int userId, int moneys, int SpareMoney)
    {
    }

    /// <summary>
    /// Oyuncunun sunucuya giriş işlemini yönetir. Bu metot, oyuncunun verilerini yükler,
    /// dünyaya yerleştirir, başlangıç paketlerini gönderir ve çeşitli sistemleri başlatır.
    /// İşlemin herhangi bir aşamasında başarısız olması durumunda oyuncuyu temizler ve false döner.
    /// </summary>
    /// <returns>Giriş işleminin başarılı olup olmadığını belirtir.</returns>
    public bool Login()
    {
        if (WorldMgr.AddPlayer(UserVIPInfo.ID, this))
        {
            try
            {
                if (LoadFromDatabase())
                {
                    if (PlayerCharacter.BoxGetDate.ToShortDateString() != DateTime.Now.ToShortDateString())
                    {
                        PlayerCharacter.AlreadyGetBox = 0;
                        PlayerCharacter.BoxProgression = 0;
                    }
                    Out.SendLoginSuccess();
                    if (LittleGameWorldMgr.IsOpen)
                    {
                        Actives.SendLittleGameActived();
                    }
                    _ = Out.SendUpdatePublicPlayer(PlayerCharacter, MatchInfo, Extra.Info);
                    Out.SendWeaklessGuildProgress(PlayerCharacter);
                    ProcessConsortiaAndPet();
                    Out.SendDateTime();
                    _ = Out.SendDailyAward(this);
                    LoadMarryMessage();
                    if (!ShowPP)
                    {
                        PlayerProp.ViewCurrent();
                        ShowPP = true;
                    }
                    _ = PlayerCharacter.ID;
                    Rank.SendUserRanks();
                    if (UserVIPInfo.honorId != 0)
                    {
                        UpdateHonor(UserVIPInfo.honorId);
                    }

                    Farm.LoadFarmLand();
                    _ = Out.SendOpenVIP(this);
                    EquipBag.UpdatePlayerProperties();
                    PetBag.UpdateEatPets();
                    SetupProcessor();
                    Actives.SendEvent();
                    Out.SendEnthrallLight();
                    _ = Out.SendAvatarCollect(AvatarCollect);
                    AvatarCollect.ScanAvatarVaildDate();
                    Out.SendEdictumVersion();
                    PlayerState = ePlayerState.Manual;
                    _ = Out.SendBufferList(this, BufferList.GetAllBufferByTemplate());
                    _ = Out.SendUpdateAchievementData(AchievementInventory.GetSuccessAchievement());
                    BoxBeginTime = DateTime.Now;
                    TimeCheckHack = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                    OpenAllNoviceActive();
                    if (PlayerCharacter.Grade >= 30)
                    {
                        _ = Out.SendPlayerFigSpiritinit(PlayerCharacter.ID, GemStone);
                    }
                    _ = WorldMgr.IsAccountLimit(this);
                    Out.SendUpdateFirstRecharge(PlayerCharacter.IsRecharged, PlayerCharacter.IsGetAward);
                    ChargeToUser();
                    ConsortiaTaskMgr.AddPlayer(this);
                    Out.SendOpenWorldBoss(X, Y);
                    if (DateTime.Parse(GameProperties.LeftRouterEndDate) > DateTime.Now)
                    {
                        Out.SendLeftRouleteOpen(Extra.Info);
                    }
                    Extra.BeginPingOnlineTimer();
                    userWonderFulActivityManager ??= new UserWonderFulActivityManager(this);
                    userWonderFulActivityManager.SignToday();
                    // userWonderFulActivityManager.MountUp(15);
                    // userWonderFulActivityManager.TempleUp(15);
                    // userWonderFulActivityManager.ConsumeMoney(1);
                    //userWonderFulActivityManager.ChargeMoney(1);
                    if (ActiveSystemMgr.IsLeagueOpen)
                    {
                        Out.SendLeagueNotice(UserVIPInfo.ID, BattleData.MatchInfo.restCount, BattleData.MatchInfo.maxCount, 1);
                        SendMessage(eMessageType.SYS_NOTICE, "Lig Başladı! Birlik savaşlarında kim kimi yenecek bakalım!");
                    }
                    else
                    {
                        Out.SendLeagueNotice(UserVIPInfo.ID, BattleData.MatchInfo.restCount, BattleData.MatchInfo.maxCount, 2);
                    }
                    if (ActiveSystemMgr.IsGoldTimeOpen)
                    {
                        SendMessage(eMessageType.SYS_NOTICE, "Altın Saat Etkinliği başladı! Haydi Oyun salonunda buluşalım!");
                    }
                    _ = Out.SendUserSyncEquipGhost(this);
                    Out.SendGuildMemberWeekOpenClose(Extra.Info);
                    Dice.SendDiceActiveOpen();
                    _ = Out.SendNecklaceStrength(PlayerCharacter);
                    WorldMgr.Test();
                    if (PlayerCharacter.VIPLevel >= 3)
                    {
                        string NoticeOnline = string.Format("Sayın VIP {1}. seviye olan üye [{0}] çevrimiçi oldu!", PlayerCharacter.NickName, PlayerCharacter.VIPLevel);
                        WorldMgr.SendMessageAll(NoticeOnline);
                    }
                    if (PlayerCharacter.Repute is > 0 and <= 10)
                    {
                        string Ranked = PlayerCharacter.Honor;
                        if (Ranked == null | Ranked.Length < 1)
                        {
                            Ranked = "Oyuncu";
                        }

                        string NoticeOnline = string.Format("|{0}| Onur Listesi Sıralaması'nda {4}. olan - |{1}| ünvanlı [{2}] oyuna giriş yaptı! Tam tamına {3} savaş gücüyle sizlere meydan okuyor!", ZoneName, Ranked, PlayerCharacter.NickName, PlayerCharacter.FightPower, PlayerCharacter.Repute);
                        WorldMgr.SendMessageAll(NoticeOnline);
                    }
                    if (PlayerCharacter.NickName == "yutikeyu")
                    {
                        string NoticeOnline = string.Format("Moderatör [yutikeyu] oyuna giriş yaptı!");
                        WorldMgr.SendMessageAll(NoticeOnline);
                    }
                    if (PlayerCharacter.NickName == "element")
                    {
                        string NoticeOnline = string.Format("Yönetici [element] oyuna giriş yaptı!");
                        WorldMgr.SendMessageAll(NoticeOnline);
                    }
                    if (PlayerCharacter.NickName == "elementt")
                    {
                        string NoticeOnline = string.Format("Yönetici [elementt] oyuna giriş yaptı!");
                        WorldMgr.SendMessageAll(NoticeOnline);
                    }
                    if (PlayerCharacter.Grade >= 13 && Actives.IsPyramidOpen())
                    {
                        Out.SendPyramidOpenClose(Actives.PyramidConfig);
                        if (!Actives.IsYearMonsterOpen())
                        {
                            Out.SendCatchBeastOpen(UserVIPInfo.ID, true);
                        }
                    }
                    GmActivityMgr.OnPlayerUpgradeVIP(this, UserVIPInfo.VIPLevel);
                    Out.SendUpdateChickActivation(Actives.GetChickActiveData());
                    Out.SendOpenHappyRecharge(PlayerCharacter.ID);
                    Out.SendLeftRouleteOpen(Extra.Info);
                    Out.SendGuildMemberWeekOpenClose(Extra.Info);
                    Out.SendOpenHappyRecharge(UserVIPInfo.ID);



                    return true;
                }
                _ = WorldMgr.RemovePlayer(UserVIPInfo.ID);
            }
            catch (Exception exception)
            {
                log.Error("Error Login!", exception);
            }
            return false;
        }
        return false;
    }

    private void CheckAndSendWeeklyHonorReward()
    {
        try
        {
            // Oyuncunun Onur Listesi sıralamasını kontrol et (1-10 arası)
            if (PlayerCharacter.Repute is > 0 and <= 10)
            {
                // Ödül içeriğini sıralamaya göre belirle
                WeeklyHonorReward reward = GetRewardByRank(PlayerCharacter.Repute);

                // Mail gönder
                SendWeeklyHonorRewardMail(reward);

                // Oyuncuya bilgi mesajı gönder
                string rankText = GetRankText(PlayerCharacter.Repute);
                SendMessage(eMessageType.SYS_NOTICE, string.Format("Tebrikler! Onur Listesi'nde {0} olarak haftalık ödülünüz mailinize gönderildi!", rankText));

                log.Info(string.Format("Weekly Honor Reward sent to player {0} (Rank: {1})", PlayerCharacter.NickName, PlayerCharacter.Repute));
            }
        }
        catch (Exception ex)
        {
            log.Error("CheckAndSendWeeklyHonorReward Error!", ex);
        }
    }

    private WeeklyHonorReward GetRewardByRank(int rank)
    {
        WeeklyHonorReward reward = new()
        {
            Rank = rank
        };

        // Sıralamaya göre ödül içeriği
        switch (rank)
        {
            case 1: // 1. sıra
                reward.Gold = 100000;
                reward.Coins = 120;
                reward.Title = "Haftanın Şampiyonu";
                break;
            case 2: // 2. sıra
                reward.Gold = 80000;
                reward.Coins = 100;
                reward.Title = "Haftanın İkincisi";
                break;
            case 3: // 3. sıra
                reward.Gold = 60000;
                reward.Coins = 90;
                reward.Title = "Haftanın Üçüncüsü";
                break;
            case 4: // 4. sıra
                reward.Gold = 50000;
                reward.Coins = 75;
                reward.Title = "Haftanın Dördüncüsü";
                break;
            case 5: // 5. sıra
                reward.Gold = 40000;
                reward.Coins = 50;
                reward.Title = "Haftanın Beşincisi";
                break;
            case 6: // 6. sıra
                reward.Gold = 30000;
                reward.Coins = 45;
                reward.Title = "Haftanın Altıncısı";
                break;
            case 7: // 7. sıra
                reward.Gold = 25000;
                reward.Coins = 40;
                reward.Title = "Haftanın Yedincisi";
                break;
            case 8: // 8. sıra
                reward.Gold = 22000;
                reward.Coins = 30;
                reward.Title = "Haftanın Sekizincisi";
                break;
            case 9: // 9. sıra
                reward.Gold = 21000;
                reward.Coins = 20;
                reward.Title = "Haftanın Dokuzuncusu";
                break;
            case 10: // 10. sıra
                reward.Gold = 20000;
                reward.Coins = 15;
                reward.Title = "Haftanın En İyi 10.su";
                break;
            default:
                reward.Gold = 10000;
                reward.Coins = 5;
                break;
        }

        return reward;
    }

    private void SendWeeklyHonorRewardMail(WeeklyHonorReward reward)
    {
        try
        {
            MailInfo mail = new()
            {
                SenderID = 0, // Sistem maili
                Sender = "Sistem",
                ReceiverID = PlayerCharacter.ID,
                Receiver = PlayerCharacter.NickName,
                Title = string.Format("Haftalık Onur Listesi Ödülü - {0}. Sıra", reward.Rank),

                // İçerik kısmında ödülleri yazıyla belirtelim
                Content = BuildMailContent(reward),

                Type = 1, // Sistem maili tipi

                // BURASI ÖNEMLİ: Altın ve Para direkt ekleniyor (Hatasız çalışır)
                Gold = reward.Gold,
                Money = reward.Coins,

                ValidDate = 7, // 7 gün geçerli

                // DİKKAT: Annex alanlarını BOŞ BIRAKIYORUZ.
                // Çünkü Annex alanı "ItemID:Count" formatını kabul etmiyor, 
                // sadece veritabanındaki Item Instance ID'sini (integer) kabul ediyor.
                // Eğer eşya göndermek istersen, önce UserItem tablosuna kayıt atıp ID'sini alman gerekir.
                Annex1 = "",
                Annex2 = "",
                Annex3 = "",
                Annex4 = "",
                Annex5 = ""
            };

            // Maili gönder
            using (PlayerBussiness db = new())
            {
                _ = db.SendMail(mail);
            }

            // Oyuncuya mail bildirimi gönder
            _ = Out.SendMailResponse(PlayerCharacter.ID, eMailRespose.Receiver);
        }
        catch (Exception ex)
        {
            log.Error("SendWeeklyHonorRewardMail Error!", ex);
        }
    }

    private string BuildMailContent(WeeklyHonorReward reward)
    {
        StringBuilder content = new();
        _ = content.AppendLine("Tebrikler!");
        _ = content.AppendLine(string.Format("Onur Listesi'nde bu hafta {0}. sırada yer alarak özel ödülleri almaya hak kazandınız!", reward.Rank));
        _ = content.AppendLine("Ödülleriniz:");
        _ = content.AppendLine(string.Format("- Altın: {0}", reward.Gold));
        _ = content.AppendLine(string.Format("- Kupon: {0}", reward.Coins));
        _ = content.AppendLine("Başarılarınızın devamını dileriz!");
        _ = content.AppendLine("Bu ödül haftalık olarak Pazartesi günleri verilmektedir.");

        return content.ToString();
    }

    private string GetRankText(int rank)
    {
        return rank switch
        {
            1 => "1. sıra",
            2 => "2. sıra",
            3 => "3. sıra",
            _ => string.Format("{0}. sıra", rank),
        };
    }

    // Yardımcı sınıflar
    public class WeeklyHonorReward
    {
        public int Rank { get; set; }
        public int Gold { get; set; }
        public int Coins { get; set; }
        public string Title { get; set; }
        public List<RewardItem> Items { get; set; }

        public WeeklyHonorReward()
        {
            Items = [];
            Title = "";
        }
    }

    public class RewardItem
    {
        public int ItemID { get; set; }
        public int Count { get; set; }

        public RewardItem(int itemID, int count)
        {
            ItemID = itemID;
            Count = count;
        }
    }
    public UserWonderFulActivityManager userWonderFulActivityManager { get; set; }

    public PlayerGmActivity GmActivity { get; }

    private void ProcessConsortiaAndPet()
    {
        Consortia = new ConsortiaProcessor(m_consortiaProcessor);
        PetHandler = new PetProcessor(m_petProcessor);
    }




    public bool GiftTokenDirect(int value)
    {
        if (value <= 0 || PlayerCharacter.GiftToken < value)
        {
            return false;
        }
        _ = RemoveGiftToken(value);
        return true;
    }


    public void OnAchievementFinish(AchievementData info)
    {
        AchievementFinishEvent?.Invoke(info);
    }

    public void OnAdoptPetEvent()
    {
        AdoptPetEvent?.Invoke();
    }

    public void OnCropPrimaryEvent()
    {
        CropPrimaryEvent?.Invoke();
    }

    public void OnEnterHotSpring()
    {
        EnterHotSpringEvent?.Invoke(this);
    }

    public void OnFightAddOffer(int offer)
    {
        FightAddOfferEvent?.Invoke(offer);
    }

    public void OnGuildChanged()
    {
        GuildChanged?.Invoke();
    }

    public void OnHotSpingExpAdd(int minutes, int exp)
    {
        HotSpingExpAdd?.Invoke(minutes, exp);
    }

    public void OnOnlineGameAdd(GamePlayer player)
    {
        OnlineGameAdd?.Invoke(player);
    }

    public void OnItemCompose(int composeType)
    {
        ItemCompose?.Invoke(composeType);
    }

    public void OnItemFusion(int fusionType)
    {
        ItemFusion?.Invoke(fusionType);
    }

    public void OnItemInsert()
    {
        ItemInsert?.Invoke();
    }

    public void OnItemMelt(int categoryID)
    {
        ItemMelt?.Invoke(categoryID);
    }

    public void OnItemStrengthen(int categoryID, int level)
    {
        ItemStrengthen?.Invoke(categoryID, level);
    }

    public void OnMoneyCharge(int money)
    {
        MoneyCharge?.Invoke(money);
    }

    public void OnMoneyChargeWeek(int money)
    {
        MoneyChargeWeek?.Invoke(money);
    }

    public void OnAchievementQuest()
    {
        AchievementQuest?.Invoke();
    }

    public void OnKillingBoss(AbstractGame game, NpcInfo npc, int damage)
    {
        AfterKillingBoss?.Invoke(game, npc, damage);
    }

    public void OnKillingLiving(AbstractGame game, int type, int id, bool isLiving, int damage)
    {
        AfterKillingLiving?.Invoke(game, type, id, isLiving, damage, isSpanArea: false);
        if (!(GameKillDrop == null || isLiving))
        {
            GameKillDrop(game, type, id, isLiving);
        }
        if (!isLiving)
        {
            if (id == 1243)
            {
                Rank.AddNewRank(1000, 3);
                GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysNotice($"Dünya BOSS'a meydan okuyan değerli oyuncumuz [{UserVIPInfo.NickName}], son vuruşunu başarıyla gerçekleştirdi ve ek ödüller kazandı! Tebrikler!")); //türkçeleştirildi not: yuti
            }
            else if (id == 30004)
            {
                Rank.AddNewRank(1001, 3);
                GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysNotice($"Dünya BOSS'a meydan okuyan değerli oyuncumuz [{UserVIPInfo.NickName}], son vuruşunu başarıyla gerçekleştirdi ve ek ödüller kazandı! Tebrikler!")); //türkçeleştirildi not: yuti
            }
        }
    }

    public void OnLevelUp(int grade)
    {
        LevelUp?.Invoke(this);
    }

    public void OnMissionOver(AbstractGame game, bool isWin, int missionId, int turnNum)
    {
        MissionOver?.Invoke(game, missionId, isWin);
        if (MissionTurnOver != null && isWin)
        {
            MissionTurnOver(game, missionId, turnNum);
        }
        MissionFullOver?.Invoke(game, missionId, isWin, turnNum);
    }

    public void OnNewGearEvent(ItemInfo item)
    {
        NewGearEvent?.Invoke(item);
    }

    public void OnSeedFoodPetEvent()
    {
        SeedFoodPetEvent?.Invoke();
    }

    public void OnPaid(int money, int gold, int offer, int gifttoken, int petScore, int medal, int damageScores, string payGoods)
    {
        Paid?.Invoke(money, gold, offer, gifttoken, petScore, medal, damageScores, payGoods);
    }

    protected void OnPropertiesChanged()
    {
        UpdateProperties();
        OnPlayerPropertyChanged(UserVIPInfo);
    }

    public void OnUnknowQuestConditionEvent()
    {
        UnknowQuestConditionEvent?.Invoke();
    }

    public void OnUpLevelPetEvent()
    {
        UpLevelPetEvent?.Invoke();
    }

    public void OnUseBuffer()
    {
        UseBuffer?.Invoke(this);
    }

    public void OnUserToemGemstoneEvent()
    {
        UserToemGemstonetEvent?.Invoke();
    }

    public void OnUsingItem(int templateID, int count)
    {
        AfterUsingItem?.Invoke(templateID, count);
    }

    public void OpenVIP(int days)
    {
        DateTime vIPExpireDay = DateTime.Now.AddDays(days);
        UserVIPInfo.typeVIP = SetTypeVIP(days);
        UserVIPInfo.VIPLevel = 1;
        UserVIPInfo.VIPExp = 0;
        UserVIPInfo.VIPExpireDay = vIPExpireDay;
        UserVIPInfo.VIPLastDate = DateTime.Now;
        UserVIPInfo.VIPNextLevelDaysNeeded = 0;
        UserVIPInfo.CanTakeVipReward = true;
    }

    public void OpenVIP(int days, DateTime ExpireDayOut)
    {
        UserVIPInfo.typeVIP = SetTypeVIP(days);
        UserVIPInfo.VIPExpireDay = ExpireDayOut;
        UserVIPInfo.VIPLastDate = DateTime.Now;
        UserVIPInfo.VIPNextLevelDaysNeeded = 10;
        UserVIPInfo.CanTakeVipReward = true;
        if (Extra.CheckNoviceActiveOpen(NoviceActiveType.VIP_LEVEL))
        {
            Extra.UpdateEventCondition((int)NoviceActiveType.VIP_LEVEL, PlayerCharacter.VIPLevel);
        }
    }

    public void ContinuousVIP(int days, DateTime ExpireDayOut)
    {
        int vIPLevel = UserVIPInfo.VIPLevel;
        if (vIPLevel < 6 && days == 180)
        {
            UserVIPInfo.VIPExpireDay = ExpireDayOut;
            UserVIPInfo.typeVIP = SetTypeVIP(days);
        }
        else if (vIPLevel < 4 && days == 90)
        {
            UserVIPInfo.VIPExpireDay = ExpireDayOut;
            UserVIPInfo.typeVIP = SetTypeVIP(days);
        }
        else
        {
            UserVIPInfo.VIPExpireDay = ExpireDayOut;
            UserVIPInfo.typeVIP = SetTypeVIP(days);
        }
        if (Extra.CheckNoviceActiveOpen(NoviceActiveType.VIP_LEVEL))
        {
            Extra.UpdateEventCondition((int)NoviceActiveType.VIP_LEVEL, PlayerCharacter.VIPLevel);
        }
    }

    public byte SetTypeVIP(int days)
    {
        byte result = 1;
        if (UserVIPInfo.typeVIP == 2)
        {
            result = 2;
        }
        else if (days / 31 >= 3)
        {
            result = 2;
        }
        return result;
    }

    public void ResetLottery()
    {
        Lottery = -1;
        LotteryID = 0;
        LotteryItems = [];
        LotteryAwardList = [];
    }

    public virtual bool Quit()
    {
        try
        {
            try
            {
                if (Level == 1)
                {
                    ItemInfo itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(7008), 1, 105);
                    itemInfo.ValidDate = 365;
                    _ = EquipBag.AddItemTo(itemInfo, 6);
                }
                if (CurrentRoom != null)
                {
                    _ = CurrentRoom.RemovePlayerUnsafe(this);
                    CurrentRoom = null;
                }
                else
                {
                    _ = RoomMgr.WaitingRoom.RemovePlayer(this);
                }

                CurrentMarryRoom?.RemovePlayer(this);
                CurrentMarryRoom = null;
                CurrentHotSpringRoom?.RemovePlayer(this);
                CurrentHotSpringRoom = null;
                if (LotteryAwardList.Count > 0 && Lottery != -1)
                {
                    _ = SendItemsToMail(LotteryAwardList, "", LanguageMgr.GetTranslation("Game.Server.Lottery.Oversea.MailTitle"), eMailType.BuyItem);
                    ResetLottery();
                }
                ConsortiaTaskMgr.RemovePlayer(this);
                if (LittleGameInfo.ID != 0)
                {
                    LittleGameWorldMgr.RemovePlayer(this);
                }
                _ = RoomMgr.WorldBossRoom.RemovePlayer(this);
                RoomMgr.ChristmasRoom.SetMonterDie(PlayerCharacter.ID);
                _ = RoomMgr.ChristmasRoom.RemovePlayer(this);
                Actives.StopChristmasTimer();
                Extra.StopAllTimer();
            }
            catch (Exception exception)
            {
                log.Error("Player exit Game Error!", exception);
            }
            UserVIPInfo.State = 0;
            _ = SaveIntoDatabase();
        }
        catch (Exception exception2)
        {
            log.Error("Player exit Error!!!", exception2);
        }
        finally
        {
            _ = WorldMgr.RemovePlayer(UserVIPInfo.ID);
        }
        return true;
    }

    public bool RemoveAt(eBageType bagType, int place)
    {
        return GetInventory(bagType)?.RemoveItemAt(place) ?? false;
    }

    public bool RemoveCountFromStack(ItemInfo item, int count)
    {
        if (item.BagType == PropBag.BagType)
        {
            return PropBag.RemoveCountFromStack(item, count);
        }
        if (item.BagType == ConsortiaBag.BagType)
        {
            return ConsortiaBag.RemoveCountFromStack(item, count);
        }
        return item.BagType == BankBag.BagType ? BankBag.RemoveCountFromStack(item, count) : EquipBag.RemoveCountFromStack(item, count);
    }

    public int RemoveGold(int value)
    {
        if (value > 0 && value <= UserVIPInfo.Gold)
        {
            UserVIPInfo.Gold -= value;
            OnPropertiesChanged();
            UpdateProperties();
            return value;
        }
        return 0;
    }

    public int RemoveGP(int gp)
    {
        if (gp > 0)
        {
            UserVIPInfo.GP -= gp;
            if (UserVIPInfo.GP < 1)
            {
                UserVIPInfo.GP = 1;
            }
            int level = LevelMgr.GetLevel(UserVIPInfo.GP);
            if (Level > level)
            {
                UserVIPInfo.GP += gp;
            }
            UpdateProperties();
            UpdateLevel();
            return gp;
        }
        return 0;
    }

    public int RemoveGiftToken(int value)
    {
        if (value > 0 && value <= UserVIPInfo.GiftToken)
        {
            UserVIPInfo.GiftToken -= value;
            OnPropertiesChanged();
            UpdateProperties();
            return value;
        }
        return 0;
    }

    public bool RemoveHealstone()
    {
        ItemInfo itemAt = EquipBag.GetItemAt(18);
        return itemAt != null && itemAt.Count > 0 && EquipBag.RemoveCountFromStack(itemAt, 1);
    }

    public bool RemoveItem(ItemInfo item)
    {
        if (item.BagType == FarmBag.BagType)
        {
            return FarmBag.RemoveItem(item);
        }
        if (item.BagType == PropBag.BagType)
        {
            return PropBag.RemoveItem(item);
        }
        if (item.BagType == FightBag.BagType)
        {
            return FightBag.RemoveItem(item);
        }

        if (item.BagType == ConsortiaBag.BagType)
        {
            return ConsortiaBag.RemoveItem(item);
        }

        if (item.BagType == BankBag.BagType)
        {
            return BankBag.RemoveItem(item);
        }

        if (item.BagType == StoreBag.BagType)
        {
            return StoreBag.RemoveItem(item);
        }

        if (item.BagType == CaddyBag.BagType)
        {
            return CaddyBag.RemoveItem(item);
        }

        //eBageType.Consortia => m_ConsortiaBag,
        //eBageType.BankBag => m_BankBag,
        //eBageType.Store => m_storeBag,

        return EquipBag.RemoveItem(item);
    }

    public int AddMedal(int value)
    {
        if (value > 0)
        {
            ItemInfo itemByTemplateID = GetInventory(eBageType.PropBag).GetItemByTemplateID(1, 11408);
            if (itemByTemplateID != null)
            {
                _ = PropBag.AddCountToStack(itemByTemplateID, value);
                PropBag.UpdateItem(itemByTemplateID);
            }
            else
            {
                _ = PropBag.AddTemplate(ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11408), value, 104), value);
            }
            UserVIPInfo.medal = GetMedalNum();
            OnPropertiesChanged();
            UpdateProperties();
            _ = UpdateChangedPlaces();
            return value;
        }
        return 0;
    }

    public int RemoveMedal(int value)
    {
        if (value > 0 && value <= UserVIPInfo.medal)
        {
            _ = RemoveTemplate(11408, value);
            UserVIPInfo.medal = GetMedalNum();
            OnPropertiesChanged();
            UpdateProperties();
            _ = UpdateChangedPlaces();
            return value;
        }
        return 0;
    }

    public int RemoveMoneyNoviceActive(int value)
    {
        if (value > 0 && value <= UserVIPInfo.Money)
        {
            UserVIPInfo.Money -= value;
            OnPropertiesChanged();
            UpdateProperties();
            return value;
        }
        return 0;
    }

    public int RemoveMoney(int value)
    {
        return RemoveMoney(value, IsAntiMult: false, isNoviceActive: false);
    }
    /// <summary>
    /// Harcama yapılmadan önce günlük limit kontrolünü yapar.
    /// Limit dolduysa false döner ve uyarı mesajı gönderir.
    /// </summary>
    public bool CanSpendMoney(int value)
    {
        int dailyLimit = GameServer.Instance?.Configuration != null
            ? GameApiServer.DailyMoneyLimit
            : 16000;

        // Eğer yapılacak harcama, kalan limiti aşıyorsa
        if (UserVIPInfo.DailyMoneyUsed + value > dailyLimit)
        {
            SendMessage(string.Format("Günlük harcama limitinizi aşıyorsunuz! Kalan Limit: {0} Kupon", dailyLimit - UserVIPInfo.DailyMoneyUsed));
            return false;
        }

        // Yeterli bakiye kontrolü (Opsiyonel, normalde handler'da vardır ama garanti olsun)
        if (UserVIPInfo.Money < value)
        {
            SendMessage("Yeterli kupona sahip değilsiniz.");
            return false;
        }

        return true;
    }
    public int RemoveMoney(int value, bool IsAntiMult, bool isNoviceActive)
    {
        // GÜNLÜK LİMİT KONTROLÜ
        int dailyLimit = GameApiServer.PlayerCustomLimits
            .TryGetValue(PlayerCharacter.NickName, out int _cl)
            ? _cl
            : GameApiServer.DailyMoneyLimit;
        if (UserVIPInfo.DailyMoneyUsed + value > dailyLimit)
        {
            // Limit aşıldı, işlemi engelle ve uyarı gönder
            SendMessage(string.Format("Günlük kupon harcama limitini aştınız! (Limit: {0}, Harcanan: {1})", dailyLimit, UserVIPInfo.DailyMoneyUsed));
            return 0;
        }

        if (value > 0)
        {
            // Normal Kupon (Money) Kontrolü
            if (value <= UserVIPInfo.Money)
            {
                UserVIPInfo.Money -= value;

                // Günlük harcamayı artır
                UserVIPInfo.DailyMoneyUsed += value;

                // Görev/Event kontrolleri (mevcut kodunuzdaki gibi)
                if (!isNoviceActive)
                {
                    if (Extra.CheckNoviceActiveOpen(NoviceActiveType.Gunluk_Harcama))
                    {
                        Extra.UpdateEventCondition((int)NoviceActiveType.Gunluk_Harcama, value, isPlus: true, 0);
                    }
                    if (Extra.CheckNoviceActiveOpen(NoviceActiveType.Haftalik_Harcama))
                    {
                        Extra.UpdateEventCondition((int)NoviceActiveType.Haftalik_Harcama, value, isPlus: true, 0);
                    }
                }
                OnPropertiesChanged();
                UpdateProperties();
                return value;
            }
            // Kilitli Kupon (MoneyLock) Kontrolü
            else if (value <= UserVIPInfo.MoneyLock)
            {
                UserVIPInfo.MoneyLock -= value;

                // Kilitli kupon harcaması da sayılırsa buraya ekleyebilirsiniz (isteğe bağlı)
                // m_character.DailyMoneyUsed += value; 

                OnPropertiesChanged();
                UpdateProperties();
                return value;
            }
        }
        return 0;
    }
    public int RemoveMoneyLock(int value)
    {
        if (value > 0 && value <= UserVIPInfo.MoneyLock)
        {
            UserVIPInfo.MoneyLock -= value;
            OnPropertiesChanged();
            UpdateProperties();
            return value;
        }
        return 0;
    }

    public int RemoveOffer(int value)
    {
        if (value > 0)
        {
            if (value >= UserVIPInfo.Offer)
            {
                value = UserVIPInfo.Offer;
            }
            UserVIPInfo.Offer -= value;
            OnPropertiesChanged();
            UpdateProperties();
            return value;
        }
        return 0;
    }

    public int RemoveRichesOffer(int value)
    {
        if (value > 0)
        {
            if (value >= UserVIPInfo.RichesOffer)
            {
                value = UserVIPInfo.RichesOffer;
            }
            UserVIPInfo.RichesOffer -= value;
            OnPropertiesChanged();
            UpdateProperties();
            return value;
        }
        return 0;
    }

    public int RemoveConsortiaRiches(int value)
    {
        if (value > 0)
        {
            if (value >= UserVIPInfo.ConsortiaRiches)
            {
                value = UserVIPInfo.ConsortiaRiches;
            }
            UserVIPInfo.ConsortiaRiches -= value;
            OnPropertiesChanged();
            UpdateProperties();
            OnGuildChanged();
            return value;
        }
        return 0;
    }

    public int RemovePetScore(int value)
    {
        if (value > 0 && value <= UserVIPInfo.petScore)
        {
            UserVIPInfo.petScore -= value;
            OnPropertiesChanged();
            UpdateProperties();
            return value;
        }
        return 0;
    }

    //public int RemoveScore(int value)
    //{
    //    if (value > 0 && value <= m_character.Score)
    //    {
    //        m_character.Score -= value;
    //        OnPropertiesChanged();
    //        UpdateProperties();
    //        return value;
    //    }
    //    return 0;
    //}

    public int RemoveScore(int value)
    {
        if (value > 0 && PlayerCharacter.Score >= value)
        {
            PlayerCharacter.Score -= value;
            if (PlayerCharacter.Score <= int.MinValue)
            {
                PlayerCharacter.Score = int.MaxValue;

            }
            if (PlayerCharacter.Score <= 0)
            {
                PlayerCharacter.Score = 0;

            }
            OnPropertiesChanged();
            return value;
        }

        return 0;
    }

    public bool RemoveTempate(eBageType bagType, ItemTemplateInfo template, int count)
    {
        return GetInventory(bagType)?.RemoveTemplate(template.TemplateID, count) ?? false;
    }

    public bool RemoveTemplate(ItemTemplateInfo template, int count)
    {
        return GetItemInventory(template)?.RemoveTemplate(template.TemplateID, count) ?? false;
    }

    public bool RemoveTemplate(int templateId, int count)
    {
        int mainItem = EquipBag.GetItemCount(templateId);
        int propItem = PropBag.GetItemCount(templateId);
        int consortiaItem = ConsortiaBag.GetItemCount(templateId);
        int bankItem = BankBag.GetItemCount(templateId);
        int tempCount = mainItem + propItem + consortiaItem + bankItem;
        ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(templateId);
        if (templateId == 11408 && count <= propItem + consortiaItem + bankItem)
        {
            UserVIPInfo.medal -= count;
            UpdateProperties();
        }
        if (itemTemplateInfo != null && tempCount >= count)
        {
            if (mainItem > 0 && count > 0 && RemoveTempate(eBageType.EquipBag, itemTemplateInfo, (mainItem > count) ? count : mainItem))
            {
                count = (count >= mainItem) ? (count - mainItem) : 0;
            }
            if (propItem > 0 && count > 0 && RemoveTempate(eBageType.PropBag, itemTemplateInfo, (propItem > count) ? count : propItem))
            {
                count = (count >= propItem) ? (count - propItem) : 0;
            }
            if (consortiaItem > 0 && count > 0 && RemoveTempate(eBageType.Consortia, itemTemplateInfo, (consortiaItem > count) ? count : consortiaItem))
            {
                count = (count >= consortiaItem) ? (count - consortiaItem) : 0;
            }
            if (bankItem > 0 && count > 0 && RemoveTempate(eBageType.BankBag, itemTemplateInfo, (bankItem > count) ? count : bankItem))
            {
                count = (count >= bankItem) ? (count - bankItem) : 0;
            }
            if (count == 0)
            {
                return true;
            }
            if (log.IsErrorEnabled)
            {
                log.Error($"Item Remover Error：PlayerId {PlayerId} Remover TemplateId{templateId} Is Not Zero!");
            }
        }
        return false;
    }

    public UserLabyrinthInfo LoadLabyrinth(int sType) //savaşçının gizli yeri not: yuti
    {
        if (Labyrinth == null)
        {
            using PlayerBussiness playerBussiness = new();
            Labyrinth = playerBussiness.GetSingleLabyrinth(PlayerCharacter.ID);
            if (Labyrinth == null)
            {
                Labyrinth = new UserLabyrinthInfo
                {
                    UserID = PlayerCharacter.ID,
                    sType = sType,
                    myProgress = 0,
                    myRanking = 0,
                    completeChallenge = true,
                    isDoubleAward = false,
                    currentFloor = 1,
                    accumulateExp = 0,
                    remainTime = 0,
                    currentRemainTime = 0,
                    cleanOutAllTime = 0,
                    cleanOutGold = 50,
                    tryAgainComplete = true,
                    isInGame = false,
                    isCleanOut = false,
                    serverMultiplyingPower = false,
                    LastDate = DateTime.Now,
                    ProcessAward = InitProcessAward()
                };
                _ = playerBussiness.AddUserLabyrinth(Labyrinth);
            }
            else
            {
                ProcessLabyrinthAward = Labyrinth.ProcessAward;
                Labyrinth.sType = sType;
            }
        }
        return Labyrinth;
    }

    public string InitProcessAward()
    {
        string[] array = new string[99];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i.ToString();
        }
        ProcessLabyrinthAward = string.Join("-", array);
        return ProcessLabyrinthAward;
    }

    public string CompleteGetAward(int floor)
    {
        string[] array = new string[floor];
        for (int i = 0; i < floor; i++)
        {
            array[i] = "i";
        }
        string[] array2 = Labyrinth.ProcessAward.Split('-');
        string text = string.Join("-", array);
        for (int j = floor; j < array2.Length; j++)
        {
            text = text + "-" + array2[j];
        }
        return text;
    }

    public bool isDoubleAward()
    {
        return Labyrinth != null && Labyrinth.isDoubleAward;
    }

    public void OutLabyrinth(bool isWin)
    {
        if (!isWin && Labyrinth != null && Labyrinth.currentFloor > 1)
        {
            SendLabyrinthTryAgain();
        }
        ResetLabyrinth();
    }

    public void SendLabyrinthTryAgain()
    {
        GSPacketIn gSPacketIn = new(131, PlayerId);
        gSPacketIn.WriteByte(9);
        gSPacketIn.WriteInt(LabyrinthTryAgainMoney());
        SendTCP(gSPacketIn);
    }

    public int LabyrinthTryAgainMoney()
    {
        for (int i = 0; i < Labyrinth.myProgress; i += 2)
        {
            if (Labyrinth.currentFloor == i)
            {
                return GameProperties.WarriorFamRaidPriceBig;
            }
        }
        return GameProperties.WarriorFamRaidPriceSmall;
    }

    public void ResetLabyrinth()
    {
        if (Labyrinth != null)
        {
            Labyrinth.isInGame = false;
            Labyrinth.completeChallenge = false;
            Labyrinth.ProcessAward = InitProcessAward();
        }
    }

    public void CalculatorClearnOutLabyrinth()
    {
        if (Labyrinth != null)
        {
            int num = 0;
            for (int i = Labyrinth.currentFloor; i <= Labyrinth.myProgress; i++)
            {
                num += 2;
            }
            int num2 = num * 60;
            Labyrinth.remainTime = num2;
            Labyrinth.currentRemainTime = num2;
            Labyrinth.cleanOutAllTime = num2;
        }
    }

    public int[] CreateExps()
    {
        int[] array = new int[99];
        int num = 660;
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = num;
            num += 690;
        }
        return array;
    }

    public void UpdateLabyrinth(int floor, int m_missionInfoId, bool bigAward)
    {
        int[] array = CreateExps();
        int num = (floor - 1 > array.Length) ? (array.Length - 1) : (floor - 1);
        int num2 = (num >= 0) ? num : 0;
        int num3 = array[num2];
        string text = labyrinthGolds[num2];
        int num4 = int.Parse(text.Split('|')[0]);
        int num5 = int.Parse(text.Split('|')[1]);
        if (Labyrinth != null)
        {
            floor++;
            ProcessLabyrinthAward = CompleteGetAward(floor);
            Labyrinth.ProcessAward = ProcessLabyrinthAward;
            if (PropBag.GetItemByTemplateID(0, 11916) == null || !RemoveTemplate(11916, 1))
            {
                Labyrinth.isDoubleAward = false;
            }
            if (Labyrinth.isDoubleAward)
            {
                num3 *= 2;
                num4 *= 2;
                num5 *= 2;
            }
            if (floor > Labyrinth.myProgress)
            {
                Labyrinth.myProgress = floor;
            }
            if (floor > Labyrinth.currentFloor)
            {
                Labyrinth.currentFloor = floor;
            }
            Labyrinth.accumulateExp += num3;
            string text2 = LanguageMgr.GetTranslation("UpdateLabyrinth.Exp", num3);
            _ = AddGP(num3, false);
            if (bigAward)
            {
                List<ItemInfo> list = CopyDrop(2, 40002);
                if (list != null)
                {
                    foreach (ItemInfo item in list)
                    {
                        item.IsBinds = true;
                        _ = AddTemplate(item, item.Template.BagType, num4, backToMail: true);
                        text2 += $", {item.Template.Name} x{num4}";
                    }
                }
                _ = AddHardCurrency(num5);
                text2 = text2 + LanguageMgr.GetTranslation("UpdateLabyrinth.GoldLaby") + num5;
            }
            SendHideMessage(text2);
        }
        _ = Out.SendLabyrinthUpdataInfo(Labyrinth.UserID, Labyrinth);
    }



    public int AddHardCurrency(int value)
    {
        if (value > 0)
        {
            PlayerCharacter.hardCurrency += value;
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public virtual bool SaveIntoDatabase()
    {
        try
        {
            if (UserVIPInfo == null || UserVIPInfo.ID <= 0)
            {
                return false;
            }

            SaveEquipGhost();
            if (UserVIPInfo.IsDirty)
            {
                using PlayerBussiness pb = new();
                _ = pb.UpdatePlayer(UserVIPInfo);
                if (Labyrinth != null)
                {
                    _ = pb.UpdateLabyrinthInfo(Labyrinth);
                }

                foreach (UserGemStone g in GemStone)
                {
                    _ = pb.UpdateGemStoneInfo(g);
                }
            }
            EquipBag.SaveToDatabase();
            PropBag.SaveToDatabase();
            ConsortiaBag.SaveToDatabase();
            BankBag.SaveToDatabase();
            CardBag.SaveToDatabase();
            StoreBag.SaveToDatabase();
            Rank.SaveToDatabase();
            QuestInventory.SaveToDatabase();
            AchievementInventory.SaveToDatabase();
            BufferList.SaveToDatabase();
            BattleData.SaveToDatabase();
            Extra.SaveToDatabase();
            PetBag.SaveToDatabase(saveAdopt: true);
            FarmBag.SaveToDatabase();
            Farm.SaveToDatabase();
            Actives.SaveToDatabase();
            Dice.SaveToDatabase();
            AvatarCollect.SaveToDatabase();
            GmActivity.SaveToDatabase();
            try
            {
                if (DateTime.Compare(UserVIPInfo.CheckDate.AddMinutes(20.0), DateTime.Now) > 0 && UserVIPInfo.CheckCode != "baodeptrai")
                {
                    UserVIPInfo.CheckCode = "baodeptrai";
                    Disconnect();
                }
            }
            catch (Exception e)
            {
                log.Error("Error Checking hack: " + UserVIPInfo.NickName + "!", e);
            }
            return true;
        }
        catch (Exception exception)
        {
            log.Error("Error saving player " + UserVIPInfo.NickName + "!", exception);
            return false;
        }
    }
    public void SendPkgLimitGrate()
    {
        try
        {
            if (GmActivityMgr.FoodActivity != null)
            {
                Out.SendOpenFoodActive(GmActivityMgr.FoodActivity);
            }
            Out.SendOpenGodsRoad();
            int ıD = PlayerCharacter.ID;
            if (PlayerCharacter.Grade >= 20)
            {

                if (ActiveSystemMgr.IsLeagueOpen)
                {
                    try
                    {
                        Out.SendLeagueNotice(ıD, BattleData.MatchInfo.restCount, BattleData.maxCount, 1);
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine(ex2.ToString());
                    }
                }
                else
                {
                    try
                    {
                        Out.SendLeagueNotice(ıD, BattleData.MatchInfo.restCount, BattleData.maxCount, 2);
                    }
                    catch (Exception ex3)
                    {
                        Console.WriteLine(ex3.ToString());
                    }
                }

            }
            if (PlayerCharacter.Grade >= 30)
            {
                try
                {
                    _ = Out.SendPlayerFigSpiritinit(ıD, GemStone);
                }
                catch (Exception ex6)
                {
                    Console.WriteLine(ex6.ToString());
                }
            }





        }
        catch (Exception ex11)
        {
            Console.WriteLine(ex11.ToString());
        }
    }


    public bool SaveNewItems()
    {
        try
        {
            EquipBag.SaveToDatabase();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool SaveNewsItemIntoDatabase()
    {
        try
        {
            EquipBag.SaveNewsItemIntoDatabas();
            PropBag.SaveNewsItemIntoDatabas();
            return true;
        }
        catch (Exception exception)
        {
            log.Error("Error saving Save Bag Into Database " + UserVIPInfo.NickName + "!", exception);
            return false;
        }
    }

    public bool SavePlayerInfo()
    {
        try
        {
            if (UserVIPInfo == null && UserVIPInfo.ID <= 0)
            {
                return false;
            }

            if (UserVIPInfo.IsDirty)
            {
                using PlayerBussiness pb = new();
                _ = pb.UpdatePlayer(UserVIPInfo);
            }
            return true;
        }
        catch (Exception exception)
        {
            log.Error("Error saving player info of " + UserVIPInfo.UserName + "!", exception);
            return false;
        }
    }

    public void SendConsortiaBossInfo(ConsortiaInfo info)
    {
        RankingPersonInfo rankingPersonInfo = null;
        List<RankingPersonInfo> list = [];
        foreach (RankingPersonInfo value in info.RankList.Values)
        {
            if (value.Name == PlayerCharacter.NickName)
            {
                rankingPersonInfo = value;
            }
            else
            {
                list.Add(value);
            }
        }
        GSPacketIn gSPacketIn = new(129, PlayerCharacter.ID);
        gSPacketIn.WriteByte(30);
        gSPacketIn.WriteByte((byte)info.bossState);
        gSPacketIn.WriteBoolean(rankingPersonInfo != null);
        if (rankingPersonInfo != null)
        {
            gSPacketIn.WriteInt(rankingPersonInfo.ID);
            gSPacketIn.WriteInt(rankingPersonInfo.TotalDamage);
            gSPacketIn.WriteInt(rankingPersonInfo.Honor);
            gSPacketIn.WriteInt(rankingPersonInfo.Damage);
        }
        gSPacketIn.WriteByte((byte)list.Count);
        foreach (RankingPersonInfo item in list)
        {
            gSPacketIn.WriteString(item.Name);
            gSPacketIn.WriteInt(item.ID);
            gSPacketIn.WriteInt(item.TotalDamage);
            gSPacketIn.WriteInt(item.Honor);
            gSPacketIn.WriteInt(item.Damage);
        }
        gSPacketIn.WriteByte((byte)info.extendAvailableNum);
        gSPacketIn.WriteDateTime(info.endTime);
        gSPacketIn.WriteInt(info.callBossLevel);
        SendTCP(gSPacketIn);
    }

    public void SendConsortiaBossOpenClose(int type)
    {
        GSPacketIn gSPacketIn = new(129, PlayerCharacter.ID);
        gSPacketIn.WriteByte(31);
        gSPacketIn.WriteByte((byte)type);
        SendTCP(gSPacketIn);
    }

    public void SendConsortiaFight(int consortiaID, int riches, string msg)
    {
        GSPacketIn gSPacketIn = new(158);
        gSPacketIn.WriteInt(consortiaID);
        gSPacketIn.WriteInt(riches);
        gSPacketIn.WriteString(msg);
        GameServer.Instance.LoginServer.SendPacket(gSPacketIn);
    }

    public void SendHideMessage(string msg)
    {
        GSPacketIn gSPacketIn = new(3);
        gSPacketIn.WriteInt(3);
        gSPacketIn.WriteString(msg);
        SendTCP(gSPacketIn);
    }

    public void SendInsufficientMoney(int type)
    {
        GSPacketIn gSPacketIn = new(88, PlayerId);
        gSPacketIn.WriteByte((byte)type);
        gSPacketIn.WriteBoolean(val: false);
        SendTCP(gSPacketIn);
    }

    public void SendItemNotice(ItemInfo info, int typeGet, string Name)
    {
        if (info == null)
        {
            return;
        }
        int num = 0;
        num = typeGet switch
        {
            0 or 1 => 2,
            2 or 3 or 4 => 1,
            _ => 3,
        };
        GSPacketIn gSPacketIn = new(14);
        gSPacketIn.WriteString(PlayerCharacter.NickName);
        gSPacketIn.WriteInt(typeGet);
        gSPacketIn.WriteInt(info.TemplateID);
        gSPacketIn.WriteBoolean(info.IsBinds);
        gSPacketIn.WriteInt(num);
        if (num == 3)
        {
            gSPacketIn.WriteString(Name);
        }
        if (info.IsTips)
        {
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            for (int i = 0; i < allPlayers.Length; i++)
            {
                allPlayers[i].Out.SendTCP(gSPacketIn);
            }
        }
    }

    public bool SendItemsToMail(ItemInfo item, string content, string title, eMailType type)
    {
        return SendItemsToMail(
        [
            item
        ], content, title, type);
    }

    public bool SendItemsToMail(List<ItemInfo> items, string content, string title, eMailType type)
    {
        using PlayerBussiness pb = new();
        List<ItemInfo> list = [];
        foreach (ItemInfo item in items)
        {
            if (item.Template.MaxCount == 1)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    ItemInfo itemInfo = ItemInfo.CloneFromTemplate(item.Template, item);
                    itemInfo.Count = 1;
                    list.Add(itemInfo);
                }
            }
            else
            {
                list.Add(item);
            }
        }
        return SendItemsToMail(list, content, title, type, pb);
    }

    public bool SendItemsToMail(List<ItemInfo> items, string content, string title, eMailType type, PlayerBussiness pb)
    {
        bool result = true;
        for (int i = 0; i < items.Count; i += 5)
        {
            MailInfo mailInfo = new()
            {
                Title = title ?? LanguageMgr.GetTranslation("Game.Server.GameUtils.Title"),
                Gold = 0,
                IsExist = true,
                Money = 0,
                Receiver = PlayerCharacter.NickName,
                ReceiverID = PlayerId,
                Sender = PlayerCharacter.NickName,
                SenderID = PlayerId,
                Type = (int)type,
                GiftToken = 0
            };
            List<ItemInfo> list = [];
            StringBuilder stringBuilder = new();
            StringBuilder stringBuilder2 = new();
            _ = stringBuilder.Append(LanguageMgr.GetTranslation("Game.Server.GameUtils.CommonBag.AnnexRemark"));
            content = (content != null) ? LanguageMgr.GetTranslation(content) : "";
            int num = i;
            if (items.Count > num)
            {
                ItemInfo itemInfo = items[num];
                if (itemInfo.ItemID == 0)
                {
                    _ = pb.AddGoods(itemInfo);
                }
                else
                {
                    list.Add(itemInfo);
                }
                if (title == null)
                {
                    mailInfo.Title = itemInfo.Template.Name;
                }
                mailInfo.Annex1 = itemInfo.ItemID.ToString();
                mailInfo.Annex1Name = itemInfo.Template.Name;
                _ = stringBuilder.Append("1、" + mailInfo.Annex1Name + "x" + itemInfo.Count + ";");
                _ = stringBuilder2.Append("1、" + mailInfo.Annex1Name + "x" + itemInfo.Count + ";");
            }
            num = i + 1;
            if (items.Count > num)
            {
                ItemInfo itemInfo2 = items[num];
                if (itemInfo2.ItemID == 0)
                {
                    _ = pb.AddGoods(itemInfo2);
                }
                else
                {
                    list.Add(itemInfo2);
                }
                mailInfo.Annex2 = itemInfo2.ItemID.ToString();
                mailInfo.Annex2Name = itemInfo2.Template.Name;
                _ = stringBuilder.Append("2、" + mailInfo.Annex2Name + "x" + itemInfo2.Count + ";");
                _ = stringBuilder2.Append("2、" + mailInfo.Annex2Name + "x" + itemInfo2.Count + ";");
            }
            num = i + 2;
            if (items.Count > num)
            {
                ItemInfo itemInfo3 = items[num];
                if (itemInfo3.ItemID == 0)
                {
                    _ = pb.AddGoods(itemInfo3);
                }
                else
                {
                    list.Add(itemInfo3);
                }
                mailInfo.Annex3 = itemInfo3.ItemID.ToString();
                mailInfo.Annex3Name = itemInfo3.Template.Name;
                _ = stringBuilder.Append("3、" + mailInfo.Annex3Name + "x" + itemInfo3.Count + ";");
                _ = stringBuilder2.Append("3、" + mailInfo.Annex3Name + "x" + itemInfo3.Count + ";");
            }
            num = i + 3;
            if (items.Count > num)
            {
                ItemInfo itemInfo4 = items[num];
                if (itemInfo4.ItemID == 0)
                {
                    _ = pb.AddGoods(itemInfo4);
                }
                else
                {
                    list.Add(itemInfo4);
                }
                mailInfo.Annex4 = itemInfo4.ItemID.ToString();
                mailInfo.Annex4Name = itemInfo4.Template.Name;
                _ = stringBuilder.Append("4、" + mailInfo.Annex4Name + "x" + itemInfo4.Count + ";");
                _ = stringBuilder2.Append("4、" + mailInfo.Annex4Name + "x" + itemInfo4.Count + ";");
            }
            num = i + 4;
            if (items.Count > num)
            {
                ItemInfo itemInfo5 = items[num];
                if (itemInfo5.ItemID == 0)
                {
                    _ = pb.AddGoods(itemInfo5);
                }
                else
                {
                    list.Add(itemInfo5);
                }
                mailInfo.Annex5 = itemInfo5.ItemID.ToString();
                mailInfo.Annex5Name = itemInfo5.Template.Name;
                _ = stringBuilder.Append("5、" + mailInfo.Annex5Name + "x" + itemInfo5.Count + ";");
                _ = stringBuilder2.Append("5、" + mailInfo.Annex5Name + "x" + itemInfo5.Count + ";");
            }
            mailInfo.AnnexRemark = stringBuilder.ToString();
            if (content == null && stringBuilder2.ToString() == null)
            {
                mailInfo.Content = LanguageMgr.GetTranslation("Game.Server.GameUtils.Content");
            }
            else
            {
                mailInfo.Content = content != "" ? content : stringBuilder2.ToString();
            }
            if (pb.SendMail(mailInfo))
            {
                foreach (ItemInfo item in list)
                {
                    _ = TakeOutItem(item);
                }
            }
            else
            {
                result = false;
            }
        }
        return result;
    }

    public void ViFarmsAdd(int playerID)
    {
        if (!ViFarms.Contains(playerID))
        {
            ViFarms.Add(playerID);
        }
    }

    public void ViFarmsRemove(int playerID)
    {
        if (ViFarms.Contains(playerID))
        {
            _ = ViFarms.Remove(playerID);
        }
    }

    public bool SendItemToMail(int templateID, int count, string content, string title)
    {
        ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(templateID);
        if (itemTemplateInfo == null)
        {
            return false;
        }
        if (content == "")
        {
            content = itemTemplateInfo.Name + "x1";
        }
        ItemInfo itemInfo = ItemInfo.CreateFromTemplate(itemTemplateInfo, 1, 104);
        itemInfo.IsBinds = true;
        itemInfo.Count = count;
        return SendItemToMailEvent(itemInfo, content, title, eMailType.Active);
    }

    public bool SendItemToMail(ItemInfo item, string content, string title, eMailType type)
    {
        using PlayerBussiness pb = new();
        return SendItemToMail(item, pb, content, title, type);
    }

    public bool SendItemToMail(ItemInfo item, PlayerBussiness pb, string content, string title, eMailType type)
    {
        int originalBagType = item.BagType;
        bool saveToDb = true;
        MailInfo mailInfo = new()
        {
            Content = content ?? LanguageMgr.GetTranslation("Game.Server.GameUtils.Content"),
            Title = title ?? LanguageMgr.GetTranslation("Game.Server.GameUtils.Title"),
            Gold = 0,
            IsExist = true,
            Money = 0,
            GiftToken = 0,
            Receiver = PlayerCharacter.NickName,
            ReceiverID = PlayerCharacter.ID,
            Sender = PlayerCharacter.NickName,
            SenderID = PlayerCharacter.ID,
            Type = (int)type
        };
        if (item.ItemID == 0)
        {
            saveToDb = false;
            _ = pb.AddGoods(item);
        }
        mailInfo.Annex1 = item.ItemID.ToString();
        mailInfo.Annex1Name = item.Template.Name;
        if (pb.SendMail(mailInfo))
        {
            _ = TakeOutItem(item);
            if (originalBagType != -1 && saveToDb)
            {
                GetInventory((eBageType)originalBagType).SaveRemovedItems();
            }

            return true;
        }
        return false;
    }

    public bool SendItemToMailEvent(ItemInfo item, string content, string title, eMailType type)
    {
        using PlayerBussiness pb = new();
        return SendItemToMailEvent(item, pb, content, title, type);
    }


    public bool SendItemToMailEvent(ItemInfo item, PlayerBussiness pb, string content, string title, eMailType type)
    {
        MailInfo mail = new()
        {
            Content = content ?? LanguageMgr.GetTranslation("Game.Server.GameUtils.Content"),
            Title = title ?? LanguageMgr.GetTranslation("Game.Server.GameUtils.Title"),
            Gold = 0,
            IsExist = true,
            Money = 0,
            GiftToken = 0,
            Receiver = PlayerCharacter.NickName,
            ReceiverID = PlayerCharacter.ID,
            Sender = "Bombom",
            SenderID = 0,
            Type = (int)type
        };
        if (item.ItemID == 0)
        {
            _ = pb.AddGoods(item);
        }
        mail.Annex1 = item.ItemID.ToString();
        mail.Annex1Name = item.Template.Name;
        if (pb.SendMail(mail))
        {
            _ = TakeOutItem(item);
            return true;
        }
        return false;
    }

    public bool SendMailToUser(PlayerBussiness pb, string content, string title, eMailType type)
    {
        MailInfo mailInfo = new()
        {
            Content = content,
            Title = title,
            Gold = 0,
            IsExist = true,
            Money = 0,
            GiftToken = 0,
            Receiver = PlayerCharacter.NickName,
            ReceiverID = PlayerCharacter.ID,
            Sender = PlayerCharacter.NickName,
            SenderID = PlayerCharacter.ID,
            Type = (int)type,
            Annex1 = "",
            Annex1Name = ""
        };
        return pb.SendMail(mailInfo);
    }

    public void SendMessage(string msg)
    {
        GSPacketIn gSPacketIn = new(3);
        gSPacketIn.WriteInt(0);
        gSPacketIn.WriteString(msg);
        SendTCP(gSPacketIn);
    }

    public void SendMessage(eMessageType type, string msg)
    {
        GSPacketIn gSPacketIn = new(3);
        gSPacketIn.WriteInt((int)type);
        gSPacketIn.WriteString(msg);
        SendTCP(gSPacketIn);
    }

    public bool SendMoneyMailToUser(string title, string content, int money, eMailType type)
    {
        using PlayerBussiness pb = new();
        return SendMoneyMailToUser(pb, content, title, money, type);
    }

    public bool SendMoneyMailToUser(PlayerBussiness pb, string content, string title, int money, eMailType type)
    {
        MailInfo mailInfo = new()
        {
            Content = content,
            Title = title,
            Gold = 0,
            IsExist = true,
            Money = money,
            GiftToken = 0,
            Receiver = PlayerCharacter.NickName,
            ReceiverID = PlayerCharacter.ID,
            Sender = PlayerCharacter.NickName,
            SenderID = PlayerCharacter.ID,
            Type = (int)type,
            Annex1 = "",
            Annex1Name = ""
        };
        return pb.SendMail(mailInfo);
    }

    public void SendPrivateChat(int receiverID, string receiver, string sender, string msg, bool isAutoReply)
    {
        GSPacketIn gSPacketIn = new(37, PlayerCharacter.ID);
        gSPacketIn.WriteInt(receiverID);
        gSPacketIn.WriteString(receiver);
        gSPacketIn.WriteString(sender);
        gSPacketIn.WriteString(msg);
        gSPacketIn.WriteBoolean(isAutoReply);
        SendTCP(gSPacketIn);
    }

    public virtual void SendTCP(GSPacketIn pkg)
    {
        if (m_client.IsConnected)
        {
            m_client.SendTCP(pkg);
        }
    }

    public bool SetPvePermission(int copyId, eHardLevel hardLevel)
    {

        if (copyId <= m_pvepermissions.Length && copyId > 0 && hardLevel != eHardLevel.Epic && m_pvepermissions[copyId - 1] == permissionChars[(int)hardLevel - 1 < 0 ? (int)hardLevel : (int)hardLevel - 1])
        {
            m_pvepermissions[copyId - 1] = permissionChars[(int)hardLevel];
            UserVIPInfo.PvePermission = ConverterPvePermission(m_pvepermissions);
            OnPropertiesChanged();
            return true;
        }
        return false;
    }

    public void OpenAllNoviceActive()
    {
        _ = DateTime.Now;
        _ = DateTime.Now.AddYears(2);
        DateTime startDate = DateTime.Parse(GameProperties.EventStartDate);
        DateTime stopDate = DateTime.Parse(GameProperties.EventEndDate);
        using PlayerBussiness pb = new();
        EventRewardProcessInfo[] userEventProcess = pb.GetUserEventProcess(PlayerId);
        foreach (EventRewardProcessInfo eventRewardProcessInfo in userEventProcess)
        {
            DateTime startTime = startDate;
            DateTime endTime = stopDate;
            Out.SendOpenNoviceActive(0, eventRewardProcessInfo.ActiveType, eventRewardProcessInfo.Conditions, eventRewardProcessInfo.AwardGot, startTime, endTime);
        }
    }

    public void ShowAllFootballCard()
    {
        for (int i = 0; i < CardsTakeOut.Length; i++)
        {
            if (CardsTakeOut[i] == null)
            {
                CardsTakeOut[i] = Card[i];
                if (takeoutCount > 0)
                {
                    TakeFootballCard(Card[i]);
                }
            }
        }
    }

    public bool StackItemToAnother(ItemInfo item)
    {
        return GetItemInventory(item.Template).StackItemToAnother(item);
    }

    public void TakeFootballCard(CardInfoOld card)
    {
        List<ItemInfo> list = [];
        for (int i = 0; i < CardsTakeOut.Length; i++)
        {
            if (card.place == i)
            {
                CardsTakeOut[i] = card;
                CardsTakeOut[i].IsTake = true;
                ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(card.templateID);
                if (itemTemplateInfo != null)
                {
                    list.Add(ItemInfo.CreateFromTemplate(itemTemplateInfo, card.count, 110));
                }
                takeoutCount--;
                break;
            }
        }
        if (list.Count <= 0)
        {
            return;
        }
        foreach (ItemInfo item in list)
        {
            _ = AddTemplate(list);
        }
    }

    public bool TakeOutItem(ItemInfo item)
    {
        if (item.BagType == PropBag.BagType)
        {
            return PropBag.TakeOutItem(item);
        }
        if (item.BagType == FightBag.BagType)
        {
            return FightBag.TakeOutItem(item);
        }
        if (item.BagType == ConsortiaBag.BagType)
        {
            return ConsortiaBag.TakeOutItem(item);
        }
        return item.BagType == BankBag.BagType ? BankBag.TakeOutItem(item) : EquipBag.TakeOutItem(item);
    }

    public void TestQuest()
    {
        using ProduceBussiness produceBussiness = new();
        QuestInfo[] aLlQuest = produceBussiness.GetALlQuest();
        QuestInfo[] array = aLlQuest;
        QuestInfo[] array2 = array;
        foreach (QuestInfo info in array2)
        {
            _ = QuestInventory.AddQuest(info, out _);
        }
    }

    public override string ToString()
    {
        return $"Id:{PlayerId} nickname:{PlayerCharacter.NickName} room:{CurrentRoom} ";
    }

    public void RemoveFistGetPet()
    {
        PlayerCharacter.IsFistGetPet = false;
        PlayerCharacter.LastRefreshPet = DateTime.Now.AddDays(-1.0);
    }

    public void RemoveLastRefreshPet()
    {
        PlayerCharacter.LastRefreshPet = DateTime.Now;
    }

    public void UpdateAnswerSite(int id)
    {
        if (PlayerCharacter.AnswerSite < id)
        {
            PlayerCharacter.AnswerSite = id;
        }
        UpdateWeaklessGuildProgress();
        Out.SendWeaklessGuildProgress(PlayerCharacter);
    }

    public void UpdateBadgeId(int Id)
    {
        UserVIPInfo.badgeID = Id;
    }

    public void UpdateBarrier(int barrier, string pic)
    {
        if (CurrentRoom != null)
        {
            CurrentRoom.Pic = pic;
            CurrentRoom.barrierNum = barrier;
            CurrentRoom.currentFloor = barrier;
        }
    }

    public void UpdateBaseProperties(int attack, int defence, int agility, int lucky, int hp, int Guard)
    {
        if (attack != UserVIPInfo.Attack || defence != UserVIPInfo.Defence || agility != UserVIPInfo.Agility || lucky != UserVIPInfo.Luck)
        {
            UserVIPInfo.Attack = attack;
            UserVIPInfo.Defence = defence;
            UserVIPInfo.Agility = agility;
            UserVIPInfo.Luck = lucky;
            OnPropertiesChanged();
        }
        UserVIPInfo.hp = (int)((hp + LevelPlusBlood + (UserVIPInfo.Defence / 10)) * GetBaseBlood());
        HoGiap = Guard;
    }

    public bool UpdateChangedPlaces()
    {
        try
        {
            EquipBag.UpdateChangedPlaces();
            PropBag.UpdateChangedPlaces();
            return true;
        }
        catch (Exception exception)
        {
            log.Error("Error Update Changed Places " + UserVIPInfo.NickName + "!", exception);
            return false;
        }
    }

    public void UpdateDrill(int index, UserDrillInfo drill)
    {
        UserDrills[index] = drill;
    }

    public void UpdateFightBuff(BufferInfo info)
    {
        int num = -1;
        for (int i = 0; i < FightBuffs.Count; i++)
        {
            if (info != null && info.Type == FightBuffs[i].Type)
            {
                FightBuffs[i] = info;
                num = info.Type;
            }
        }
        if (num == -1)
        {
            FightBuffs.Add(info);
        }
    }

    public void UpdateFightPower()
    {
        int num = 0;
        FightPower = 0;
        int hp = PlayerCharacter.hp;
        num += PlayerCharacter.Attack;
        num += PlayerCharacter.Defence;
        num += PlayerCharacter.Agility;
        num += PlayerCharacter.Luck;
        double baseAttack = GetBaseAttack(); //hasar
        double baseDefence = GetBaseDefence(); //zırh
        FightPower += (int)(((num + 1000) * ((baseAttack * baseAttack * baseAttack) + (3.5 * baseDefence * baseDefence * baseDefence)) / 100000000.0) + (hp * 0.95));
        if (m_currentSecondWeapon != null)
        {
            FightPower += (int)(m_currentSecondWeapon.Template.Property7 * Math.Pow(1.1, m_currentSecondWeapon.StrengthenLevel));
        }
        if (FightPower < 0)
        {
            FightPower = int.MaxValue;
        }
        PlayerCharacter.FightPower = FightPower;
        OnPlayerPropertyChanged(UserVIPInfo);
        _ = Extra.CheckNoviceActiveOpen(NoviceActiveType.SAVAS_GUCU);
        Extra.UpdateEventCondition((int)NoviceActiveType.SAVAS_GUCU, UserVIPInfo.FightPower);
    }

    public void UpdateHealstone(ItemInfo item)
    {
        if (item != null)
        {
            Healstone = item;
        }
    }

    public void UpdateHide(int hide)
    {
        if (hide != UserVIPInfo.Hide)
        {
            UserVIPInfo.Hide = hide;
            OnPropertiesChanged();
        }
    }

    public void UpdateHonor(string honor)
    {
        UserRankInfo singleRank = Rank.GetSingleRank(honor);
        if (singleRank != null && singleRank.IsValidRank())
        {
            PlayerCharacter.honorId = singleRank.NewTitleID;
            PlayerCharacter.Honor = honor;
            EquipBag.UpdatePlayerProperties();
        }
        else
        {
            PlayerCharacter.honorId = 0;
            PlayerCharacter.Honor = "";
            EquipBag.UpdatePlayerProperties();
            //SendMessage("Cập nhật danh hiệu thất bại!");
        }
    }

    public void UpdateHonor(int honorid)
    {
        UserRankInfo singleRank = Rank.GetRankByHonnor(honorid);
        if (singleRank != null && singleRank.IsValidRank())
        {
            PlayerCharacter.honorId = honorid;
            PlayerCharacter.Honor = singleRank.Info.Name;
            EquipBag.UpdatePlayerProperties();
        }
        else
        {
            PlayerCharacter.honorId = 0;
            PlayerCharacter.Honor = "";
            EquipBag.UpdatePlayerProperties();
            //SendMessage("Cập nhật danh hiệu thất bại!");
        }
    }

    public void UpdateItem(ItemInfo item)
    {
        GetInventory((eBageType)item.BagType)?.UpdateItem(item);
    }

    public void AccumulativeUpdate()
    {
        if (PlayerCharacter.accumulativeLoginDays < 7)
        {
            if (PlayerCharacter.accumulativeLoginDays == 0)
            {
                PlayerCharacter.accumulativeLoginDays = 1;
            }
            else
            {
                PlayerCharacter.accumulativeLoginDays++;
            }
        }
    }

    public void UpdateItemForUser(object state)
    {
        Extra.LoadFromDatabase();
        BattleData.LoadFromDatabase();
        EquipBag.LoadFromDatabase();
        PropBag.LoadFromDatabase();
        ConsortiaBag.LoadFromDatabase();
        BankBag.LoadFromDatabase();
        StoreBag.LoadFromDatabase();
        CardBag.LoadFromDatabase();
        QuestInventory.LoadFromDatabase(UserVIPInfo.ID);
        AchievementInventory.LoadFromDatabase(UserVIPInfo.ID);
        EventLiveInventory.LoadFromDatabase();
        BufferList.LoadFromDatabase(UserVIPInfo.ID);
        Rank.LoadFromDatabase();
        PetBag.LoadFromDatabase();
        Dice.LoadFromDatabase();
        FarmBag.LoadFromDatabase();
        Actives.LoadFromDatabase();
        AvatarCollect.LoadFromDatabase();
    }

    public void UpdateLevel()
    {
        Level = LevelMgr.GetLevel(UserVIPInfo.GP);
        int maxLevel = LevelMgr.MaxLevel;
        LevelInfo levelInfo = LevelMgr.FindLevel(maxLevel);
        if (Extra.CheckNoviceActiveOpen(NoviceActiveType.Level_Atlama))
        {
            Extra.UpdateEventCondition((int)NoviceActiveType.Level_Atlama, Level);
        }
        OnLevelUp(Level);
        if (Level == maxLevel && levelInfo != null)
        {
            UserVIPInfo.GP = levelInfo.GP;
        }
    }

    public void UpdateEventSevens(EventSevenDaysInfo info, int UserID)
    {
        info.ServerID = GameServer.Instance.Configuration.ZoneId;
        info.UserID = UserID;
        using PlayerBussiness pb = new();
        _ = pb.UpdateEventSevenDays(info);
    }

    public void UpdatePet(UsersPetInfo pet)
    {
        Pet = pet;
    }

    public void UpdateProperties()
    {
        Out.SendUpdatePrivateInfo(UserVIPInfo, GetMedalNum());
        GSPacketIn pkg = Out.SendUpdatePublicPlayer(UserVIPInfo, MatchInfo, Extra.Info);
        CurrentRoom?.SendToAll(pkg, this);
    }

    public void UpdatePveResult(string type, int value, bool option)
    {
        int damageScore = 0;
        int honor = 0;
        string msg = "";
        switch (type)
        {
            case "worldboss":
                {
                    if (RoomMgr.WorldBossRoom.ReduceBlood(value) && !RoomMgr.WorldBossRoom.FightOver)
                    {
                        damageScore = value / 400;
                        honor = value / 1200;
                        msg = LanguageMgr.GetTranslation("Savaş başarıyla tamamlandı! " + damageScore + " Puan ve " + honor + " onur kazandınız!"); //türkçeleştirildi not: yuti
                        _ = AddDamageScores(damageScore);
                        RoomMgr.WorldBossRoom.UpdateRank(this, damageScore, honor);
                        _ = RoomMgr.WorldBossRoom.ReduceBlood(value);
                        if (option)
                        {
                            RoomMgr.WorldBossRoom.SendFightOver();
                        }
                    }
                }
                break;
            default:
                break;
        }

        _ = AddHonor(honor);
        if (!string.IsNullOrEmpty(msg))
        {
            SendMessage(msg);
        }
    }

    public int AddEliteScore(int value)
    {
        if (value > 0)
        {
            PlayerCharacter.EliteScore += value;
            GameServer.Instance.LoginServer.SendEliteScoreUpdate(PlayerCharacter.ID, PlayerCharacter.NickName, (PlayerCharacter.Grade <= 40) ? 1 : 2, PlayerCharacter.EliteScore);
        }
        return 0;
    }

    public int RemoveEliteScore(int value)
    {
        if (value > 0)
        {
            PlayerCharacter.EliteScore -= value;
            if (PlayerCharacter.EliteScore <= 0)
            {
                PlayerCharacter.EliteScore = 1;
            }
            GameServer.Instance.LoginServer.SendEliteScoreUpdate(PlayerCharacter.ID, PlayerCharacter.NickName, (PlayerCharacter.Grade <= 40) ? 1 : 2, PlayerCharacter.EliteScore);
        }
        return 0;
    }

    public void SendWinEliteChampion()
    {
        EliteGameRoundInfo eliteGameRoundInfo = ExerciseMgr.FindEliteRoundByUser(PlayerCharacter.ID);
        if (eliteGameRoundInfo != null)
        {
            eliteGameRoundInfo.PlayerWin = (eliteGameRoundInfo.PlayerOne.UserID == PlayerCharacter.ID) ? eliteGameRoundInfo.PlayerOne : eliteGameRoundInfo.PlayerTwo;
            GameServer.Instance.LoginServer.SendEliteChampionRoundUpdate(eliteGameRoundInfo);
            ExerciseMgr.RemoveEliteRound(eliteGameRoundInfo);
        }
        else
        {
            log.Error("////// ELITEGAME Send Win Elite Champion Round ERROR NOT FOUND: " + PlayerCharacter.UserName);
        }
    }

    public void OnTakeCard(int roomType, int place, int templateId, int count)
    {
        TakeCardPlace = place;
        TakeCardTemplateID = templateId;
        TakeCardCount = count;
    }

    public void UpdateReduceDame(ItemInfo item)
    {
        if (item != null && item.Template != null)
        {
            PlayerCharacter.ReduceDamePlus = item.Template.Property1;
        }
    }

    public void UpdateSecondWeapon(ItemInfo item)
    {
        if (item != m_currentSecondWeapon)
        {
            m_currentSecondWeapon = item;
            OnPropertiesChanged();
        }
    }

    public void UpdateStyle(string style, string colors, string skin)
    {
        if (style != UserVIPInfo.Style || colors != UserVIPInfo.Colors || skin != UserVIPInfo.Skin)
        {
            UserVIPInfo.Style = style;
            UserVIPInfo.Colors = colors;
            UserVIPInfo.Skin = skin;
            OnPropertiesChanged();
        }
    }

    public void UpdateWeaklessGuildProgress()
    {
        if (PlayerCharacter.weaklessGuildProgress == null)
        {
            PlayerCharacter.weaklessGuildProgress = Base64.decodeToByteArray(PlayerCharacter.WeaklessGuildProgressStr);
        }
        PlayerCharacter.CheckLevelFunction();
        if (PlayerCharacter.Grade == 1)
        {
            PlayerCharacter.openFunction(Step.GAIN_ADDONE);
        }
        if (PlayerCharacter.IsOldPlayer)
        {
            PlayerCharacter.openFunction(Step.OLD_PLAYER);
        }
        PlayerCharacter.WeaklessGuildProgressStr = Base64.encodeByteArray(PlayerCharacter.weaklessGuildProgress);
    }

    public void UpdateWeapon(ItemInfo item)
    {
        if (item != MainWeapon)
        {
            MainWeapon = item;
            OnPropertiesChanged();
        }
    }

    public bool UsePropItem(AbstractGame game, int bag, int place, int templateId, bool isLiving)
    {
        if (bag == 1 && templateId >= 10001 && templateId <= 10008)
        {
            ItemTemplateInfo itemTemplateInfo = PropItemMgr.FindFightingProp(templateId);
            if (isLiving && itemTemplateInfo != null)
            {
                OnUsingItem(itemTemplateInfo.TemplateID, 1);
                if (place == -1 && CanUseProp)
                {
                    return true;
                }
                ItemInfo itemAt = PropBag.GetItemAt(place);
                if (itemAt != null && itemAt.IsValidItem() && itemAt.Count >= 0)
                {
                    _ = PropBag.RemoveCountFromStack(itemAt, 1);
                    return true;
                }
            }
        }
        else
        {
            ItemInfo itemAt2 = FightBag.GetItemAt(place);
            if (itemAt2 != null)
            {
                OnUsingItem(itemAt2.TemplateID, 1);
                if (itemAt2.TemplateID == templateId)
                {
                    return FightBag.RemoveItem(itemAt2);
                }
            }
        }
        return false;
    }

    public void OnPlayerAddItem(string type, int value)
    {
        PlayerAddItem?.Invoke(type, value);
    }

    public void OnPlayerSpa(int onlineTimeSpa)
    {
        PlayerSpa?.Invoke(onlineTimeSpa);
    }

    public void OnPlayerQuestFinish(BaseQuest baseQuest)
    {
        PlayerQuestFinish?.Invoke(baseQuest);
    }

    public void OnPlayerLogin()
    {
        PlayerLogin?.Invoke();
    }

    public void OnPlayerPropertyChanged(PlayerInfo character)
    {
        PlayerPropertyChanged?.Invoke(character);
    }

    public void OnVIPUpgrade(int level, int exp)
    {
        if (Event_0 != null && UserVIPInfo.typeVIP > 0 && UserVIPInfo.VIPLevel == level)
        {
            Event_0(level, exp);
        }
    }

    public void OnUseBugle(int value)
    {
        UseBugle?.Invoke(value);
    }

    public void OnPlayerMarry()
    {
        PlayerMarry?.Invoke();
    }

    public void OnPlayerDispatches()
    {
        PlayerDispatches?.Invoke();
    }

    public void OnGameOver(AbstractGame game, bool isWin, int gainXp, bool isSpanArea, bool isCouple, int blood, int playerCount)
    {
        if (game.RoomType == eRoomType.Match)
        {
            if (isWin)
            {
                UserVIPInfo.Win++;
            }

            UserVIPInfo.Total++;
        }
        if (blood == 1)
        {
            OnFightOneBloodIsWin(game.RoomType, isWin);
        }

        if (playerCount == 4)
        {
            OnGameOver2v2(isWin);
        }

        if (isCouple && GameMarryTeam != null)
        {
            GameMarryTeam(game, isWin, gainXp, playerCount);
        }

        GameOverCountTeam?.Invoke(game, isWin, gainXp, playerCount);
        GameOver?.Invoke(game, isWin, gainXp, isSpanArea, isCouple);
        ClearFightBuffOneMatch();
        if (isWin)
        {
            winningStreak++;
        }
        else
        {
            winningStreak = 0;
        }
        if (UserVIPInfo.ConsortiaID > 0)
        {
            int richesAdd = 0;
            if (isWin)
            {
                richesAdd = (int)(gainXp * 0.1);
                var info = Client.Player.Extra.GetEventProcess((int)NoviceActiveType.BIRLIK_SAVASI);
                Client.Player.Extra.UpdateEventCondition((int)NoviceActiveType.BIRLIK_SAVASI, info.Conditions + 1);
            }
            else
            {
                richesAdd = (int)(gainXp * 0.02);
            }

            if (richesAdd > 0)
            {
                _ = AddRichesOffer(richesAdd);
                OnDonateRiches(richesAdd, 2);
            }
        }
        int totalDamage = 0;
        if (Players != null)
        {
            totalDamage = Players.TotalAllHurt;
        }
        switch (game.RoomType)
        {
            case eRoomType.Match:
                var info = Extra.GetEventProcess((int)NoviceActiveType.Ozgur_Savas);
                if (info != null)
                {
                    Extra.UpdateEventCondition((int)NoviceActiveType.Ozgur_Savas, info.Conditions + 1);
                }
                break;
            case eRoomType.Dungeon:
                // Zindan Görevleri
                if (isWin) // Genelde zindan görevleri kazanma şartına bağlıdır
                {
                    // game parametresi AbstractGame geldiği için, PVEGame özelliklerine erişmek için dönüştürüyoruz.
                    if (game is PVEGame pveGame)
                    {
                        // 1. Ana Zindan ID'sini (PveID) alıyoruz. (Örn: Karınca Mağarası, Ejderha İnişi vb.)
                        int pveId = pveGame.Info != null ? pveGame.Info.ID : 0;

                        // 2. Zorluk Seviyesini (HardLevel) alıyoruz. (YENİ EKLEME)
                        eHardLevel hardLevel = pveGame.HandLevel;

                        // 3. İçerideki Mission (Harita/Kat) ID'sini alıyoruz.
                        int missionId = pveGame.MissionInfo != null ? pveGame.MissionInfo.Id : 0;

                        // --- GÖREV KONTROLLERİ ---

                        // A) Genel Zindan Tamamlama Görevi (Tüm zindanlar için)
                        var dungeonInfo = Extra.GetEventProcess((int)NoviceActiveType.Kesif_Tamamlama);
                        if (dungeonInfo != null)
                        {
                            Extra.UpdateEventCondition((int)NoviceActiveType.Kesif_Tamamlama, dungeonInfo.Conditions + 1);
                        }

                        // B) Belirli Zindan ID'sine Göre Özel Görevler

                        // BOGO KEŞİFİ (ID: 1)
                        //if (pveId == 1)
                        //{
                        // 1. Genel Bogo Görevi (Tüm zorluklar için)
                        // var bogoInfo = Extra.GetEventProcess((int)NoviceActiveType.BOGO_KESIFI);
                        // if (bogoInfo != null)
                        // {
                        //     Extra.UpdateEventCondition((int)NoviceActiveType.BOGO_KESIFI, bogoInfo.Conditions + 1);
                        // }

                        // 2. Zorluk Seviyesine Göre Görevler
                        // Not: NoviceActiveType enum'ına BOGO_KESIFI_ZOR vb. tanımlamalısınız.
                        //  if (hardLevel == eHardLevel.Normal)
                        // {
                        // Normal zorluk görevi (Örnek)
                        // var info = Extra.GetEventProcess((int)NoviceActiveType.BOGO_KESIFI_NORMAL);
                        // if (info != null) Extra.UpdateEventCondition((int)NoviceActiveType.BOGO_KESIFI_NORMAL, info.Conditions + 1);
                        // }
                        // else if (hardLevel == eHardLevel.Hard) // Zor Mod
                        // {
                        //    if (Extra.CheckNoviceActiveOpen(NoviceActiveType.BOGO_KESIFI_ZOR))
                        //   {
                        //      Extra.UpdateEventCondition((int)NoviceActiveType.BOGO_KESIFI_ZOR, 1);
                        // }
                        // }
                        //else if (hardLevel == eHardLevel.Terror) // Dehşet/Ejderha Modu
                        //{
                        // Terror zorluk görevi (Örnek)
                        //}
                        //}

                        // KARINCA KEŞİFİ (ID: 2)
                        //                        if (pveId == 2)
                        //                      {
                        // 1. Genel Karınca Görevi
                        //                        var karincaInfo = Extra.GetEventProcess((int)NoviceActiveType.KARINCA_KESIFI);
                        //                      if (karincaInfo != null)
                        //                    {
                        //                      Extra.UpdateEventCondition((int)NoviceActiveType.KARINCA_KESIFI, karincaInfo.Conditions + 1);
                        //                }

                        // 2. Zorluk Seviyesine Göre Görevler
                        //              if (hardLevel == eHardLevel.Normal)
                        //            {
                        // Normal zorluk görevi
                        //          }
                        //        else if (hardLevel == eHardLevel.Hard) // Zor Mod
                        //      {
                        //        if (Extra.CheckNoviceActiveOpen(NoviceActiveType.KARINCA_KESIFI_ZOR))
                        //      {
                        //        Extra.UpdateEventCondition((int)NoviceActiveType.KARINCA_KESIFI_ZOR, 1);
                        //  }
                        //  }
                        // }
                    }
                }
                break;
            case eRoomType.Freshman:
                break;
        }

        if (DateTime.Now.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            int bonusGold = isWin ? 500 : 100;
            _ = AddGold(bonusGold);
            SendMessage("Hafta Sonu Bonusu: +" + bonusGold + " Altın!");
        }
        if (isWin && blood == 1)
        {
            //    if (Extra.CheckNoviceActiveOpen(NoviceActiveType.PERFECT_WIN))
            //   {
            //      Extra.UpdateEventCondition((int)NoviceActiveType.PERFECT_WIN, 1);
            // }
        }
        _ = ThreadPool.QueueUserWorkItem(delegate (object state)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;
                // KENDI WEBHOOK LINKINI YAZMAYI UNUTMA
                string webhookUrl = "https://discord.com/api/webhooks/1474082590058352749/kv9vo5Hj-1j0hROIg89LsWeV9d_SYYTiiUGErBdQaQPaJlr6471GbDii_Afwbo0otzPm";

                if (string.IsNullOrEmpty(webhookUrl) || !webhookUrl.StartsWith("http"))
                {
                    return;
                }

                string durum = isWin ? "Kazandı" : "Kaybetti";
                string odaTipi = game != null ? game.RoomType.ToString() : "Bilinmiyor";
                string oyuncuIsmi = UserVIPInfo != null ? UserVIPInfo.NickName : "Bilinmeyen";
                int seviye = UserVIPInfo != null ? UserVIPInfo.Grade : 0;

                // Rakip Bulma
                List<string> rakipler = [];
                if (CurrentRoom != null)
                {
                    foreach (GamePlayer p in CurrentRoom.GetPlayers())
                    {
                        if (p != null && p != this && p.CurrentRoomTeam != CurrentRoomTeam && p.PlayerCharacter != null)
                        {
                            rakipler.Add(p.PlayerCharacter.NickName);
                        }
                    }
                }
                string rakipIsimleri = rakipler.Count > 0 ? string.Join(", ", rakipler) : "Bot / NPC";

                int hasar = Players != null ? Players.TotalAllHurt : 0;
                int kalanCan = Players != null ? Players.Blood : blood;

                // SÜS YOK, DİREKT PYTHON'UN OKUYACAĞI ŞİFRELİ METNİ YOLLUYORUZ
                string rawData = string.Format("[Oyun Logu Alındı]|Oyuncu: {0}|Seviye: {1}|Rakipler: {2}|Oda Tipi: {3}|Durum: {4}|Hasar: {5}|Kalan Can: {6}|Kazanç XP: {7}",
                    oyuncuIsmi, seviye, rakipIsimleri, odaTipi, durum, hasar.ToString("N0"), kalanCan.ToString("N0"), gainXp);

                var payload = new { content = rawData, username = "Oyun Logu" };
                string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

                using System.Net.WebClient client = new();
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers.Add("User-Agent", "Mozilla/5.0");
                client.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                _ = client.UploadString(webhookUrl, "POST", jsonPayload);
            }
            catch (Exception ex) { log.Error("Discord Webhook Hata: ", ex); }
        });
    }

    public void OnFightOneBloodIsWin(eRoomType roomType, bool isWin)
    {
        FightOneBloodIsWin?.Invoke(roomType, isWin);
    }

    public void OnGameOver2v2(bool isWin)
    {
        GameOver2v2?.Invoke(isWin);
    }

    public void OnAcademyEvent(GamePlayer friendly, int type)
    {
        AcademyEvent?.Invoke(friendly, type);
    }

    public void OnEquipCardEvent()
    {
        EquipCardEvent?.Invoke();
    }

    public bool IsLimitMail()
    {
        if (!GameProperties.IsLimitMail)
        {
            return false;
        }
        if (Extra.Info.FreeSendMailCount >= GameProperties.LimitMail)
        {
            SendMessage($"Limiti aştınız = {GameProperties.LimitMail}"); //türkçeleştirildi not: yuti
            return true;
        }
        Extra.Info.FreeSendMailCount++;
        return false;
    }

    public static List<Suit_TemplateInfo> Load_Template_Suit_info()
    {
        List<Suit_TemplateInfo> list = [];
        using (ProduceBussiness produceBussiness = new())
        {
            Suit_TemplateInfo[] array = produceBussiness.Load_Suit_TemplateInfo();
            Suit_TemplateInfo[] array2 = array;
            Suit_TemplateInfo[] array3 = array2;
            foreach (Suit_TemplateInfo item in array3)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public static List<Suit_TemplateID> Load_Suit_TemplateID()
    {
        List<Suit_TemplateID> list = [];
        using (ProduceBussiness produceBussiness = new())
        {
            Suit_TemplateID[] array = produceBussiness.Load_Suit_TemplateID();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }
        }
        return list;
    }

    private static List<int> DS_Item_Suit()
    {
        List<int> list = [];
        List<Suit_TemplateID> list2 = Load_Suit_TemplateID();
        for (int i = 0; i < list2.Count; i++)
        {
            if (tachchuoi(list2[i].ContainEquip).Length > 1)
            {
                int num = 0;
                while (i < tachchuoi(list2[i].ContainEquip).Length)
                {
                    list.Add(tachchuoi(list2[i].ContainEquip)[num]);
                    num++;
                }
            }
            else
            {
                list.Add(tachchuoi(list2[i].ContainEquip)[0]);
            }
        }
        return list;
    }

    private static int[] tachchuoi(string A)
    {
        List<int> list = [];
        if (!A.Contains(","))
        {
            list.Add(int.Parse(A));
        }
        else
        {
            bool flag = true;
            while (flag)
            {
                if (!A.Contains(","))
                {
                    list.Add(int.Parse(A));
                    flag = false;
                    break;
                }
                if (A.IndexOf(",") > 0)
                {
                    int num = A.IndexOf(",");
                    list.Add(int.Parse(A.Substring(0, num)));
                    A = A.Remove(0, num + 1);
                }
            }
        }
        return list.ToArray();
    }

    public void ClearStoreBagWithOutPlace(int place)
    {
        List<ItemInfo> list = [];
        for (int i = 0; i < StoreBag.Capalility; i++)
        {
            if (i == place)
            {
                continue;
            }
            ItemInfo itemAt = StoreBag.GetItemAt(i);
            int num = 0;
            if (itemAt == null)
            {
                continue;
            }
            if (itemAt.Template.BagType == eBageType.PropBag)
            {
                num = PropBag.FindFirstEmptySlot();
                if (!PropBag.AddItemTo(itemAt, num))
                {
                    //PropBag.SaveToDatabase();
                    list.Add(itemAt);
                }
                else
                {
                    _ = StoreBag.TakeOutItem(itemAt);
                    //StoreBag.SaveToDatabase();
                }
            }
            else
            {
                num = EquipBag.FindFirstEmptySlot(31);
                if (!EquipBag.AddItemTo(itemAt, num))
                {
                    //EquipBag.SaveToDatabase();
                    list.Add(itemAt);
                }
                else
                {
                    _ = StoreBag.TakeOutItem(itemAt);
                    //StoreBag.SaveToDatabase();
                }
            }
        }
        if (list.Count > 0)
        {
            StoreBag.ClearBagWithoutPlace(place);
            _ = SendItemsToMail(list, "Demirciden gelen eşyaları buradan gönderelim istedik. Sırt çantanız dolmuş.", "Çantanız Dolu", eMailType.StoreCanel); //türkçeleştirildi not: yuti

        }
        _ = SaveIntoDatabase();
    }

    public void ResetRoom(bool isWin, string parram)
    {
        if (CurrentRoom != null)
        {
            if (CurrentRoom.RoomType == eRoomType.Dungeon)
            {
                CurrentRoom.Pic = "";
                CurrentRoom.MapId = 10000;
                CurrentRoom.currentFloor = 0;
                CurrentRoom.isOpenBoss = false;
                CurrentRoom.SendRoomSetupChange(CurrentRoom);
            }
        }

    }

    #region WorldBoss

    public WorldBossProcessor WorldBoss { get; private set; }
    public EventSevenDaysInfo EventSeven { get; private set; }

    private readonly WorldBossLogicProcessor _worldBossProcessor;
    public int AddDamageScores(int value) //trminhpc
    {
        if (value > 0)
        {
            PlayerCharacter.damageScores += value;
            if (PlayerCharacter.damageScores <= int.MinValue)
            {
                PlayerCharacter.damageScores = int.MaxValue;
                SendMessage(LanguageMgr.GetTranslation("GamePlayer.Msg11"));
            }

            OnPropertiesChanged();
            return value;
        }

        return 0;
    }

    public int RemoveDamageScores(int value) //baolt dep trai
    {
        if (value > 0 && PlayerCharacter.damageScores >= value)
        {
            PlayerCharacter.damageScores -= value;
            if (PlayerCharacter.damageScores <= int.MinValue)
            {
                PlayerCharacter.damageScores = int.MaxValue;

            }
            if (PlayerCharacter.damageScores <= 0)
            {
                PlayerCharacter.damageScores = 0;

            }
            OnPropertiesChanged();
            return value;
        }

        return 0;
    }
    #endregion

    public bool ActiveMoneyEnable(int value)
    {
        if (GameProperties.IsActiveMoney)
        {
            if (value < 1)
            {
                return false;
            }

            if (Actives.Info.ActiveMoney >= value)
            {
                // Burada RemoveActiveMoney zaten "kupon harcandı" mesajını basacak
                _ = RemoveActiveMoney(value);
                _ = RemoveMoney(value);
                return true;
            }

            // Eskiden: LanguageMgr.GetTranslation("GamePlayer.Msg8", Actives.Info.ActiveMoney)
            // Yetersiz kupon mesajı
            SendMessage("Kupon harcandı!");
        }
        else
        {
            return MoneyDirect(value, IsAntiMult: false, false, true);
        }
        return false;
    }
    public bool MoneyDirect(int value)
    {
        return GameProperties.IsDDTMoneyActive ? MoneyDirect(MoneyType.DDTMoney, value) : MoneyDirect(MoneyType.Money, value);
    }

    public bool MoneyDirect(MoneyType type, int value)
    {
        if (value is < 0 or > 2147483647)
        {
            return false;
        }
        if (type == MoneyType.Money)
        {
            // Önce bakiye kontrolü
            if (PlayerCharacter.Money >= value)
            {
                // RemoveMoney artık limit kontrolü yapıyor.
                // Eğer limit dolduysa 0 döner, işlem başarısız olur.
                if (RemoveMoney(value) > 0)
                {
                    return true;
                }
                else
                {
                    // Limit dolu olduğu için false döndü, yeterli bakiye olsa bile harcanamadı.
                    return false;
                }
            }
            SendInsufficientMoney(0);
        }
        else
        {
            if (PlayerCharacter.GiftToken >= value)
            {
                _ = RemoveGiftToken(value);
                return true;
            }
            SendMessage("Hediye altınınız yeterli değil.");
        }
        return false;
    }

    public bool MoneyDirect(int value, bool IsAntiMult, bool NoviceActive, bool CanMoneyLock)
    {
        return MoneyDirect(MoneyType.Money, value, IsAntiMult, NoviceActive, CanMoneyLock);
    }

    public bool MoneyDirect(MoneyType type, int value, bool IsAntiMult, bool NoviceActive, bool CanMoneyLock)
    {
        if (value is >= 0 and <= int.MaxValue)
        {
            if (type == MoneyType.Money)
            {
                if (PlayerCharacter.Money >= value)
                {
                    // RemoveMoney'nin sonucunu kontrol et (Limit kontrolü için)
                    if (RemoveMoney(value, IsAntiMult, NoviceActive) > 0)
                    {
                        AddLog("RemoveMoney", "Tài khoản " + UserVIPInfo.UserName + "sử dụng " + value + "xu ở tài khoản" + UserVIPInfo.NickName);
                        UpdateProperties();
                        return true;
                    }
                    else
                    {
                        // Limit aşıldıysa buraya düşer
                        return false;
                    }
                }
                else if (PlayerCharacter.MoneyLock >= value && CanMoneyLock)
                {
                    // Kilitli kupon için de limit kontrolü istenirse buraya eklenebilir.
                    // Şimdilik sadece RemoveMoney'deki limiti baz alıyoruz.
                    _ = RemoveMoneyLock(value);
                    UpdateProperties();
                    return true;
                }
                SendInsufficientMoney(0);
            }
            else
            {
                if (PlayerCharacter.GiftToken >= value)
                {
                    _ = RemoveGiftToken(value);
                    AddLog("RemoveGiftToken", "Tài khoản " + UserVIPInfo.UserName + "sử dụng " + value + "lễ kim ở tài khoản" + UserVIPInfo.NickName);
                    UpdateProperties();
                    return true;
                }
                SendMessage("Không đủ lễ kim.");
            }
        }
        return false;
    }

    public int AddActiveMoney(int value)
    {
        if (value > 0)
        {
            if (GameProperties.IsActiveMoney)
            {
                Actives.Info.ActiveMoney += value;

                if (Actives.Info.ActiveMoney <= int.MinValue)
                {
                    Actives.Info.ActiveMoney = int.MaxValue;
                    // Eskiden: GamePlayer.Msg9
                    SendMessage("Kupon sayacı maksimum değere ulaştı, daha fazla kupon eklenemiyor.");
                }
                else
                {
                    // Eskiden: GamePlayer.Msg1
                    SendHideMessage(
                        "Hesabına " + value + " kupon eklendi."
                    );
                }
                return value;
            }
        }
        return 0;
    }

    public int RemoveActiveMoney(int value)
    {
        if (value > 0 && value <= Actives.Info.ActiveMoney)
        {
            Actives.Info.ActiveMoney -= value;

            // Eskiden: GamePlayer.Msg2
            SendHideMessage(
                value + " kupon harcandı. Kalan kupon: " + Actives.Info.ActiveMoney
            );

            return value;
        }
        return 0;
    }


    public void LoadGemStone(PlayerBussiness db)
    {
        lock (GemStone)
        {
            GemStone = db.GetSingleGemStones(UserVIPInfo.ID);
            if (GemStone.Count != 0)
            {
                return;
            }

            List<int> intList1 =
        [
          11,
          5,
          2,
          3,
          13
        ];
            List<int> intList2 =
        [
          100002,
          100003,
          100001,
          100004,
          100005
        ];
            for (int index = 0; index < intList1.Count; ++index)
            {
                UserGemStone userGemStone1 = new()
                {
                    ID = 0
                };
                int id = UserVIPInfo.ID;
                userGemStone1.UserID = id;
                int num1 = intList2[index];
                userGemStone1.FigSpiritId = num1;
                string str = "0,0,0|0,0,1|0,0,2";
                userGemStone1.FigSpiritIdValue = str;
                int num2 = intList1[index];
                userGemStone1.EquipPlace = num2;
                UserGemStone userGemStone2 = userGemStone1;
                GemStone.Add(userGemStone2);
                _ = db.AddUserGemStone(userGemStone2);
            }
        }
    }

    public UserGemStone GetGemStone(int place)
    {
        return GemStone.FirstOrDefault<UserGemStone>(g => place == g.EquipPlace);
    }

    public void UpdateGemStone(int place, UserGemStone gem)
    {
        lock (GemStone)
        {
            for (int index = 0; index < GemStone.Count; ++index)
            {
                if (place == GemStone[index].EquipPlace)
                {
                    GemStone[index] = gem;
                    break;
                }
            }
        }
    }

    //

    public bool AddTemplate(ItemInfo cloneItem, eBageType bagType, int count)
    {
        PlayerInventory bag = GetInventory(bagType);
        if (bag != null)
        {
            // cloneItem.IsBinds = cloneItem.Template.BindType == 1;
            if (bag.AddTemplate(cloneItem, count))
            {

                if (CurrentRoom != null && CurrentRoom.IsPlaying)
                {
                    SendItemNotice(cloneItem);
                }

                return true;
            }
        }
        return false;
    }

    public int AddTotem(int value)
    {
        if (value > 0)
        {
            UserVIPInfo.totemId = value;
            OnPropertiesChanged();
            return value;
        }
        return UserVIPInfo.totemId;
    }
    public int AddHonor(int value)
    {
        if (value > 0)
        {
            UserVIPInfo.myHonor += value;
            OnPropertiesChanged();
            return value;
        }
        else
        {
            return 0;
        }
    }

    public int RemovemyHonor(int value)
    {
        if (value > 0 && value <= UserVIPInfo.myHonor)
        {
            UserVIPInfo.myHonor -= value;
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public int AddMaxHonor(int value)
    {
        if (value > 0)
        {
            UserVIPInfo.MaxBuyHonor += value;
            OnPropertiesChanged();
            return value;
        }
        return 0;
    }

    public UserEquipGhostInfo GetGhostEquip(int bagType, int place)
    {
        lock (m_equipGhostList)
        {
            return m_equipGhostList.ContainsKey(bagType + "_" + place) ? m_equipGhostList[bagType + "_" + place] : null;
        }
    }

    public List<UserEquipGhostInfo> GetAllEquipGhost()
    {
        List<UserEquipGhostInfo> list = [];
        lock (m_equipGhostList)
        {
            foreach (UserEquipGhostInfo info in m_equipGhostList.Values)
            {
                list.Add(info);
            }
        }
        return list;
    }

    public void SaveEquipGhost()
    {
        lock (m_equipGhostList)
        {
            UserVIPInfo.GhostEquipList = JsonConvert.SerializeObject(m_equipGhostList);
        }
    }

    public void AddEquipGhost(UserEquipGhostInfo equipGhost)
    {
        lock (m_equipGhostList)
        {
            if (!m_equipGhostList.ContainsKey(equipGhost.BagType + "_" + equipGhost.Place))
            {
                m_equipGhostList.Add(equipGhost.BagType + "_" + equipGhost.Place, equipGhost);
            }
        }
    }

    public bool CanActive(string name)
    {
        if (FingerConfig.CheckDisibleEvent(name))
        {
            SendMessage(LanguageMgr.GetTranslation("Game.Server.GameObjects.EventOpenOrClose"));
            return true;
        }
        return false;
    }

    public void UpdateConsortiaBattle(int leftBlood, bool isWin, int tieStatus)
    {
        int winid = isWin == true ? PlayerId : GuildBattleEnemyId;
        int lostid = isWin == false ? PlayerId : GuildBattleEnemyId;

        UserGuildBattleInfo u = GameMgr.GuildBattle.FindUser(PlayerId);
        if (u == null)
        {
            return;
        }

        if (tieStatus != -1)
        {
            GameMgr.GuildBattle.UpdateScoreMatch(winid, lostid);

            if (isWin)
            {
                UserVIPInfo.ReduceStartBlood = leftBlood;
            }
            else
            {
                UserVIPInfo.ReduceStartBlood = UserVIPInfo.hp;
                GameMgr.GuildBattle.AddCountDownRevive(u, 30);
            }
        }
        else
        {
            SendMessage(LanguageMgr.GetTranslation("GameServer.GuildBattle.TieStatusEnding"));
        }

        GuildBattleEnemyId = 0;
        UserVIPInfo.ActivePowFirstGame = false;

        if (u.IsActive)
        {
            GameMgr.GuildBattle.SendUpdateSceneInfo(u);
            GameMgr.GuildBattle.SendUpdatePlayerStatus(u);
        }
    }

    public int FusionPacketCount { get; set; } = 0;
    public DateTime FusionPacketWindowStart { get; set; } = DateTime.MinValue;


    public int ComposePacketCount { get; set; } = 0;
    public DateTime ComposePacketWindowStart { get; set; } = DateTime.MinValue;
    public int CountFunction2 { get; internal set; }

}

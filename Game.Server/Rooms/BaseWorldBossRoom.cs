using System;
using System.Collections.Generic;
using System.Linq;
using Bussiness;
using Bussiness.Protocol;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using SqlDataProvider.Data;
using Game.Logic.Phy.Object; // GamePlayer için gerekebilir
using System.Threading; // Threading için gerekebilir

namespace Game.Server.Rooms
{
    // Hata çözümü: BaseRoom'dan miras almasını sağlayın.
    public class BaseWorldBossRoom : BaseRoom
    {
        // NOT: API kodunda bu değişkenlere erişildiği varsayıldı.
        // Eğer private iseler, GameApiServer'da hata alırsınız.
        // Public/Internal olarak bırakıyoruz.
        public Dictionary<int, GamePlayer> _mList; //public yaptım not yuti

        private Dictionary<int, RankingPersonInfo> _ranklist;

        public int MaxBlood { get; set; }

        public int Blood { get; set; }

        public string Name { get; set; }

        public string BossResourceId { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public int CurrentPve { get; set; }

        public bool FightOver { get; set; }

        public bool RoomClose { get; set; }

        public bool WorldOpen { get; set; }

        public int FightTime { get; set; }

        public bool IsDie { get; set; }

        // Teleport koordinatları
        public int PlayerDefaultPosX = 265;
        public int PlayerDefaultPosY = 1030;

        public int TicketId = 11573;
        public int NeedTicketCount = 0;
        public int TimeCd = 15;
        public int ReviveMoney = 1000;
        public int ReFightMoney = 1200;
        public int addInjureBuffMoney = 30;
        public int addInjureValue = 200;

        // Hata çözümü: BaseRoom(int roomId)'i çağırıyoruz.
        public BaseWorldBossRoom() : base(0)
        {
            _mList = new Dictionary<int, GamePlayer>();
            _ranklist = new Dictionary<int, RankingPersonInfo>();
            IsDie = false;
            WorldOpen = false;
            FightOver = true;
            RoomClose = true;
            Name = "boss";
            BossResourceId = "0";
            CurrentPve = 0;
        }

        public void ResetConfigRoom()
        {
            _mList = new Dictionary<int, GamePlayer>();
            _ranklist = new Dictionary<int, RankingPersonInfo>();
            IsDie = false;
            WorldOpen = false;
            FightOver = true;
            RoomClose = true;
            Name = "boss";
            BossResourceId = "0";
            CurrentPve = 0;
        }

        public void UpdateRank(GamePlayer p, int damage, int honor)
        {
            lock (_ranklist)
            {
                RankingPersonInfo urank;


                if (!_ranklist.ContainsKey(p.PlayerCharacter.ID))
                {
                    urank = new RankingPersonInfo();
                    urank.Name = p.PlayerCharacter.NickName;
                    urank.ID = p.PlayerCharacter.ID;
                    urank.Damage = 0;
                    urank.TotalDamage = 0;
                    urank.Honor = 0;
                    _ranklist.Add(p.PlayerCharacter.ID, urank);
                }
                else
                {
                   
                    urank = _ranklist[p.PlayerCharacter.ID];
                }

                urank.Damage += damage;
                urank.TotalDamage += damage;
                urank.Honor += honor;


                this.RankPlayerCommit();
            }
        }

        public void UpdateWorldBoss(GSPacketIn pkg)
        {
            int maxBlood = pkg.ReadInt();
            int revert_blood = pkg.ReadInt();
            string t_name = pkg.ReadString();
            string t_bossResourceId = pkg.ReadString();
            int t_currentPVE = pkg.ReadInt();
            FightOver = pkg.ReadBoolean();
            RoomClose = pkg.ReadBoolean();
            BeginTime = pkg.ReadDateTime();
            EndTime = pkg.ReadDateTime();
            FightTime = pkg.ReadInt();
            bool isOpen = pkg.ReadBoolean();

            MaxBlood = maxBlood;
            Blood = revert_blood;
            Name = t_name;
            BossResourceId = t_bossResourceId;
            CurrentPve = t_currentPVE;
            WorldOpen = isOpen;
            GamePlayer[] players = WorldMgr.GetAllPlayers();
            foreach (GamePlayer p in players)
            {
                p.Out.SendOpenWorldBoss(p.X, p.Y);
            }
        }

        public void WorldBossClose()
        {
            WorldOpen = false;
            var players = GetPlayersSafe();
            foreach (var p in players)
                RemovePlayer(p);
        }

        public void SendGiftForUserJoined()
        {
            List<RankingPersonInfo> list_Top10 = new List<RankingPersonInfo>();
            lock (_ranklist)
            {
                IOrderedEnumerable<KeyValuePair<int, RankingPersonInfo>> enumerable = _ranklist.OrderByDescending((KeyValuePair<int, RankingPersonInfo> pair) => pair.Value.TotalDamage);
                foreach (KeyValuePair<int, RankingPersonInfo> pair2 in enumerable)
                {
                    if (list_Top10.Count == 10)
                    {
                        break;
                    }
                    list_Top10.Add(pair2.Value);
                }
            }
        }

        public void FightOvered()
        {
            var pkg = new GSPacketIn((byte)eChatServerPacket.WORLD_BOSS_FIGHTOVER);
            GameServer.Instance.LoginServer.SendPacket(pkg);
        }

        public bool ReduceBlood(int value)
        {
            bool result = true;
            if (this.Blood <= 0 || value <= 0)
            {
                result = false;
            }
            else if (this.Blood >= value)
            {
                this.Blood -= value;
            }
            else
            {
                this.Blood = 0;
            }
            var pkg = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD);
            pkg.WriteByte((byte)WorldBossPackageType.WORLDBOSS_BLOOD_UPDATE);
            pkg.WriteBoolean(false);
            pkg.WriteInt(this.MaxBlood);
            pkg.WriteInt(this.Blood);
            SendToAllPlayers(pkg);
            return result;
        }

        public void SendFightOver()
        {
            var pkg = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD);
            pkg.WriteByte((byte)WorldBossPackageType.WORLDBOSS_FIGHTOVER);
            pkg.WriteBoolean(true);
            SendToAllPlayers(pkg);
        }

        public void SendRoomClose()
        {
            var pkg = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD);
            pkg.WriteByte((byte)WorldBossPackageType.WORLDBOSS_ROOM_CLOSE);
            SendToAllPlayers(pkg);
        }

        public void SendEndedBossYouGotNotThing()
        {
            var players = GetPlayersSafe();
        }

        public void SendAllOver()
        {
            this.ResetConfigRoom();
            var pkg = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD);
            pkg.WriteByte((byte)WorldBossPackageType.OVER);
            SendToAllPlayers(pkg);
        }

        public void UpdateWorldBossRankCrosszone(GSPacketIn packet)
        {
            var pkg = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD);
            pkg.WriteByte((byte)WorldBossPackageType.WORLDBOSS_RANKING);
            var type = packet.ReadBoolean();
            var count = packet.ReadInt();
            pkg.WriteBoolean(type);
            pkg.WriteInt(count);
            for (var i = 0; i < count; i++)
            {
                var id = packet.ReadInt();
                var name = packet.ReadString();
                var damage = packet.ReadInt();
                pkg.WriteInt(id);
                pkg.WriteString(name);
                pkg.WriteInt(damage);
            }

            if (type)
                SendToAllPlayers(pkg);
            else
                SendToAll(pkg);
        }

        public void SendPrivateInfo(string name)
        {
            var pkg = new GSPacketIn((byte)eChatServerPacket.WORLDBOSS_PRIVATE_INFO);
            pkg.WriteString(name);
            GameServer.Instance.LoginServer.SendPacket(pkg);
        }

        public void SendPrivateInfo(string name, int damage, int honor)
        {
            var pkg = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD);
            pkg.WriteByte((byte)WorldBossPackageType.WORLDBOSS_PRIVATE_INFO);
            pkg.WriteInt(damage);
            pkg.WriteInt(honor);
            var players = GetPlayersSafe();
            foreach (var p in players)
                if (p.PlayerCharacter.NickName == name)
                {
                    p.Out.SendTCP(pkg);
                    break;
                }
        }

        public void SendUpdateBlood(GSPacketIn packet)
        {
            var maxBlood = packet.ReadInt();
            Blood = packet.ReadInt();
            var pkg = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD);
            pkg.WriteByte((byte)WorldBossPackageType.WORLDBOSS_BLOOD_UPDATE);
            pkg.WriteBoolean(false);
            pkg.WriteInt(maxBlood);
            pkg.WriteInt(Blood);
            SendToAll(pkg);
        }

        public bool AddPlayer(GamePlayer player)
        {
            var result = false;

            lock (_mList)
            {
                if (!_mList.ContainsKey(player.PlayerId))
                {
                    _mList.Add(player.PlayerId, player);
                    //player.CurrentRoom = this; // Bu satırı ekleyibiliriz not yuti
                    result = true;
                    SendPrivateInfo(player.PlayerCharacter.NickName);
                }
                RankPlayerCommit(player);
            }

            if (result)
            {
                var pkg = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD);
                pkg.WriteByte((byte)WorldBossPackageType.ENTER);
                pkg.WriteInt(player.PlayerCharacter.Grade);
                pkg.WriteInt(player.PlayerCharacter.Hide);
                pkg.WriteInt(player.PlayerCharacter.Repute);
                pkg.WriteInt(player.PlayerCharacter.ID);
                pkg.WriteString(player.PlayerCharacter.NickName);
                pkg.WriteByte(player.PlayerCharacter.typeVIP);
                pkg.WriteInt(player.PlayerCharacter.VIPLevel);
                pkg.WriteBoolean(player.PlayerCharacter.Sex);
                pkg.WriteString(player.PlayerCharacter.Style);
                pkg.WriteString(player.PlayerCharacter.Colors);
                pkg.WriteString(player.PlayerCharacter.Skin);
                pkg.WriteInt(player.X);
                pkg.WriteInt(player.Y);
                pkg.WriteInt(player.PlayerCharacter.FightPower);
                pkg.WriteInt(player.PlayerCharacter.Win);
                pkg.WriteInt(player.PlayerCharacter.Total);
                pkg.WriteInt(player.PlayerCharacter.Offer);
                pkg.WriteByte(player.States);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                SendToAll(pkg);
            }

            return result;
        }

        public bool RemovePlayer(GamePlayer player)
        {
            var result = false;
            lock (_mList)
            {
                result = _mList.Remove(player.PlayerId);
                var response = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD);
                response.WriteByte((byte)WorldBossPackageType.WORLDBOSS_EXIT);
                response.WriteInt(player.PlayerId);
                SendToAll(response);
            }

            if (result)
            {
                var pkg = player.Out.SendSceneRemovePlayer(player);
                SendToAll(pkg, player);
                if (player.CurrentRoom != null)
                {
                    player.CurrentRoom.RemovePlayerUnsafe(player);
                }
            }

            return true;
        }

        public GamePlayer[] GetPlayersSafe()
        {
            GamePlayer[] temp = null;

            lock (_mList)
            {
                temp = new GamePlayer[_mList.Count];
                _mList.Values.CopyTo(temp, 0);
            }

            return temp;
        }

        public void SendToAllPlayers(GSPacketIn packet)
        {
            var players = WorldMgr.GetAllPlayers();
            foreach (var p in players) p.SendTCP(packet);
        }

        public void SendToAll(GSPacketIn packet)
        {
            SendToAll(packet, null);
        }

        public void SendToAll(GSPacketIn packet, GamePlayer except)
        {
            GamePlayer[] temp = null;
            lock (_mList)
            {
                temp = new GamePlayer[_mList.Count];
                _mList.Values.CopyTo(temp, 0);
            }

            foreach (var p in temp)
                if (p != null && p != except)
                    p.Out.SendTCP(packet);
        }

        // Hata Çözümü: RankPlayerCommit metodu (GameApiServer'dan çağrılıyor)
        public bool RankPlayerCommit(GamePlayer player = null)
        {
            // Bu metot genellikle _ranklist kilitlenmişken çağrılır. API'de sorunsuz derlenmesi için eklendi.
            return true;
        }

        public void AssignPlayerToRoom(GamePlayer player)
        {
            player.CurrentRoom = this;
        }

        // Hata Çözümü: ViewOtherPlayerRoom metodu (GameApiServer'dan çağrılıyor)
        public void ViewOtherPlayerRoom(GamePlayer player)
        {
            return;
        }
    }
}
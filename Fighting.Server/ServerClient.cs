using Bussiness;
using Bussiness.Managers;
using Fighting.Server.GameObjects;
using Fighting.Server.Games;
using Fighting.Server.Rooms;
using Game.Base;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using Game.Logic.Protocol;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fighting.Server
{
    public class ServerClient : BaseClient
    {
        private static readonly ILog ilog_1;

        private RSACryptoServiceProvider rsacryptoServiceProvider_0;

        private readonly FightServer m_svr;

        private readonly Dictionary<int, ProxyRoom> m_rooms = new Dictionary<int, ProxyRoom>();

        protected override void OnConnect()
        {
			base.OnConnect();
			rsacryptoServiceProvider_0 = new RSACryptoServiceProvider();
			RSAParameters rsaParameters = rsacryptoServiceProvider_0.ExportParameters(includePrivateParameters: false);
			SendRSAKey(rsaParameters.Modulus, rsaParameters.Exponent);
        }

        protected override void OnDisconnect()
        {
			base.OnDisconnect();
			rsacryptoServiceProvider_0 = null;
        }

        public override void OnRecvPacket(GSPacketIn pkg)
        {
			int code = pkg.Code;
			switch (code)
			{
			case 1:
				HandleLogin(pkg);
				break;
			case 2:
				HanleSendToGame(pkg);
				break;
			case 3:
				HandleSysNotice(pkg);
				break;
			case 19:
				HandlePlayerMessage(pkg);
				break;
			case 36:
				HandlePlayerUsingProp(pkg);
				break;
			case 64:
				HandleGameRoomCreate(pkg);
				break;
			case 65:
				HandleGameRoomCancel(pkg);
				break;
			case 77:
				HandleConsortiaAlly(pkg);
				break;
			case 83:
				HandlePlayerExit(pkg);
				break;
			case 1501:
				HandlePlayerReport(pkg);
				break;
			case 90:
				HandleAddViewer(pkg);
				break;
			default:
			{
				eFightPackageType eFightPackageType = (eFightPackageType)code;
				Console.WriteLine("??????????ServerClient: " + eFightPackageType);
				break;
			}
			}
        }

        private void HandlePlayerExit(GSPacketIn pkg)
        {
			BaseGame game = GameMgr.FindGame(pkg.ClientID);
			if (game == null)
			{
				return;
			}
			Player player = game.FindPlayer(pkg.Parameter1);
			if (player != null)
			{
				GSPacketIn pkg2 = new GSPacketIn(83, player.PlayerDetail.PlayerCharacter.ID);
				game.SendToAll(pkg2);
				game.RemovePlayer(player.PlayerDetail, IsKick: false);
				ProxyRoom room1 = ProxyRoomMgr.GetRoomUnsafe((game as BattleGame).Red.RoomId);
				if (room1 != null && !room1.RemovePlayer(player.PlayerDetail))
				{
					ProxyRoomMgr.GetRoomUnsafe((game as BattleGame).Blue.RoomId)?.RemovePlayer(player.PlayerDetail);
				}
			}
        }

        private void HandlePlayerUsingProp(GSPacketIn pkg)
        {
			BaseGame game = GameMgr.FindGame(pkg.ClientID);
			if (game == null)
			{
				return;
			}
			game.Resume();
			if (pkg.ReadBoolean())
			{
				Player player = game.FindPlayer(pkg.Parameter1);
				ItemTemplateInfo template = ItemMgr.FindItemTemplate(pkg.Parameter2);
				if (player != null && template != null)
				{
					player.UseItem(template);
				}
			}
        }

        private void HandleSysNotice(GSPacketIn pkg)
        {
			BaseGame game = GameMgr.FindGame(pkg.ClientID);
			if (game != null)
			{
				Player player = game.FindPlayer(pkg.Parameter1);
				GSPacketIn pkg2 = new GSPacketIn(3);
				pkg2.WriteInt(3);
				pkg2.WriteString(LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg6", player.PlayerDetail.PlayerCharacter.Grade * 12, 15));
				player.PlayerDetail.SendTCP(pkg2);
				pkg2.ClearContext();
				pkg2.WriteInt(3);
				pkg2.WriteString(LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg7", player.PlayerDetail.PlayerCharacter.NickName, player.PlayerDetail.PlayerCharacter.Grade * 12, 15));
				game.SendToAll(pkg2, player.PlayerDetail);
			}
        }

        private void HandlePlayerMessage(GSPacketIn pkg)
        {
			BaseGame game = GameMgr.FindGame(pkg.ClientID);
			if (game == null)
			{
				return;
			}
			Player player = game.FindPlayer(pkg.ReadInt());
			bool team = pkg.ReadBoolean();
			string msg = pkg.ReadString();
			if (player != null)
			{
				GSPacketIn pkg2 = new GSPacketIn(19);
				pkg2.ClientID = player.PlayerDetail.PlayerCharacter.ID;
				pkg2.WriteInt(player.PlayerDetail.ZoneId);
				pkg2.WriteByte(5);
				pkg2.WriteBoolean(team);
				pkg2.WriteString(player.PlayerDetail.PlayerCharacter.NickName);
				pkg2.WriteString(msg);
				if (team)
				{
					game.SendToTeam(pkg, player.Team);
				}
				else
				{
					game.SendToAll(pkg2);
				}
			}
        }

        public void HandleConsortiaAlly(GSPacketIn pkg)
        {
			BaseGame game = GameMgr.FindGame(pkg.ClientID);
			if (game != null)
			{
				game.ConsortiaAlly = pkg.ReadInt();
				game.RichesRate = pkg.ReadInt();
			}
        }

        public void HandleLogin(GSPacketIn pkg)
        {
			string[] strArray = Encoding.UTF8.GetString(rsacryptoServiceProvider_0.Decrypt(pkg.ReadBytes(), fOAEP: false)).Split(',');
			if (strArray.Length == 2)
			{
				rsacryptoServiceProvider_0 = null;
				int.Parse(strArray[0]);
				base.Strict = false;
			}
			else
			{
				ilog_1.ErrorFormat("Error Login Packet from {0}", base.TcpEndpoint);
				Disconnect();
			}
        }

        public void HandleGameRoomCreate(GSPacketIn pkg)
        {
			int num1 = pkg.ReadInt();
			int num18 = pkg.ReadInt();
			int num19 = pkg.ReadInt();
			int num20 = pkg.ReadInt();
			int npcId = pkg.ReadInt(); //roaddan gelicek
			bool pickUpWithNPC = pkg.ReadBoolean();
			bool isBot = pkg.ReadBoolean();
			bool flag = pkg.ReadBoolean();
			int length = pkg.ReadInt();
			int num21 = 0;
			int num22 = 0;
			int zoneID = 0;
			int num2 = 0;
			int maxLevel = 0;
			IGamePlayer[] players = new IGamePlayer[length];
			for (int index1 = 0; index1 < length; index1++)
			{
				PlayerInfo character = new PlayerInfo();
				ProxyPlayerInfo proxyPlayer = new ProxyPlayerInfo();
				character.ID = pkg.ReadInt();
				character.UserName = pkg.ReadString();
				bool isViewer = pkg.ReadBoolean();
				int num24 = (proxyPlayer.ZoneId = pkg.ReadInt());
				int num15 = num24;
				zoneID = num15;
				proxyPlayer.ZoneName = pkg.ReadString();
				int num3 = pkg.ReadInt();
				character.NickName = pkg.ReadString();
				character.Sex = pkg.ReadBoolean();
				character.Hide = pkg.ReadInt();
				character.Style = pkg.ReadString();
				character.Colors = pkg.ReadString();
				character.Skin = pkg.ReadString();
				character.Offer = pkg.ReadInt();
				character.GP = pkg.ReadInt();
				character.Grade = pkg.ReadInt();
				character.Repute = pkg.ReadInt();
				character.ConsortiaID = pkg.ReadInt();
				character.ConsortiaName = pkg.ReadString();
				character.ConsortiaLevel = pkg.ReadInt();
				character.ConsortiaRepute = pkg.ReadInt();
				character.IsShowConsortia = pkg.ReadBoolean();
				character.badgeID = pkg.ReadInt();
				character.Honor = pkg.ReadString();
				character.AchievementPoint = pkg.ReadInt();
				character.WeaklessGuildProgressStr = pkg.ReadString();
				character.MoneyPlus = pkg.ReadInt();
				character.FightPower = pkg.ReadInt();
				character.Nimbus = pkg.ReadInt();
				character.apprenticeshipState = pkg.ReadInt();
				character.masterID = pkg.ReadInt();
				character.masterOrApprentices = pkg.ReadString();
				character.IsAutoBot = isBot;
				num2 += character.FightPower;
				character.Attack = pkg.ReadInt();
				character.Defence = pkg.ReadInt();
				character.Agility = pkg.ReadInt();
				character.Luck = pkg.ReadInt();
				character.hp = pkg.ReadInt();
				proxyPlayer.BaseAttack = pkg.ReadDouble();
				proxyPlayer.BaseDefence = pkg.ReadDouble();
				proxyPlayer.BaseAgility = pkg.ReadDouble();
				proxyPlayer.BaseBlood = pkg.ReadDouble();
				proxyPlayer.TemplateId = pkg.ReadInt();
				proxyPlayer.WeaponStrengthLevel = pkg.ReadInt();
				int num5 = pkg.ReadInt();
				if (num5 != 0)
				{
					proxyPlayer.GoldTemplateId = num5;
					proxyPlayer.goldBeginTime = pkg.ReadDateTime();
					proxyPlayer.goldValidDate = pkg.ReadInt();
				}
				proxyPlayer.CanUserProp = pkg.ReadBoolean();
				proxyPlayer.SecondWeapon = pkg.ReadInt();
				proxyPlayer.StrengthLevel = pkg.ReadInt();
				proxyPlayer.Healstone = pkg.ReadInt();
				proxyPlayer.HealstoneCount = pkg.ReadInt();
				double num4 = pkg.ReadDouble();
				double num7 = pkg.ReadDouble();
				double num8 = pkg.ReadDouble();
				double num9 = pkg.ReadDouble();
				double num10 = pkg.ReadDouble();
				pkg.ReadInt();
				List<BufferInfo> buffers = new List<BufferInfo>();
				int num11 = pkg.ReadInt();
				for (int index4 = 0; index4 < num11; index4++)
				{
					BufferInfo bufferInfo = new BufferInfo();
					bufferInfo.Type = pkg.ReadInt();
					bufferInfo.IsExist = pkg.ReadBoolean();
					bufferInfo.BeginDate = pkg.ReadDateTime();
					bufferInfo.ValidDate = pkg.ReadInt();
					bufferInfo.Value = pkg.ReadInt();
					if (character != null)
					{
						buffers.Add(bufferInfo);
					}
				}
				List<int> equipEffect = new List<int>();
				int num13 = pkg.ReadInt();
				for (int index3 = 0; index3 < num13; index3++)
				{
					int num14 = pkg.ReadInt();
					equipEffect.Add(num14);
				}
				List<BufferInfo> fightBuffer = new List<BufferInfo>();
				int num16 = pkg.ReadInt();
				for (int index2 = 0; index2 < num16; index2++)
				{
					int num12 = pkg.ReadInt();
					int num17 = pkg.ReadInt();
					fightBuffer.Add(new BufferInfo
					{
						Type = num12,
						Value = num17
					});
				}
				UserMatchInfo matchInfo = new UserMatchInfo();
				character.typeVIP = pkg.ReadByte();
				character.VIPLevel = pkg.ReadInt();
				character.VIPExpireDay = pkg.ReadDateTime();
				matchInfo.DailyLeagueFirst = pkg.ReadBoolean();
				matchInfo.DailyLeagueLastScore = pkg.ReadInt();
				int num6 = (pkg.ReadBoolean() ? 1 : 0);
				UsersPetInfo pet = null;
				if (num6 != 0)
				{
					pet = new UsersPetInfo();
					pet.Place = pkg.ReadInt();
					pet.TemplateID = pkg.ReadInt();
					pet.ID = pkg.ReadInt();
					pet.Name = pkg.ReadString();
					pet.UserID = pkg.ReadInt();
					pet.Level = pkg.ReadInt();
					pet.Skill = pkg.ReadString();
					pet.SkillEquip = pkg.ReadString();
				}
				List<int> CardBuff = new List<int>();
				int countCard = pkg.ReadInt();
				for (int i = 0; i < countCard; i++)
				{
					CardBuff.Add(pkg.ReadInt());
				}
				players[index1] = new ProxyPlayer(this, character, pet, buffers, equipEffect, fightBuffer, proxyPlayer, matchInfo)
				{
					CurrentEnemyId = num3,
					GPApprenticeOnline = num8,
					GPAddPlus = num4,
					OfferAddPlus = num7,
					GPApprenticeTeam = num9,
					GPSpouseTeam = num10,
					CardBuff = CardBuff,
					IsViewer = isViewer
				};
				if (character.Grade > maxLevel)
                {
					maxLevel = character.Grade;
					num22 = character.ID;
				}
				num21 += character.Grade;
			}
			ProxyRoom room = new ProxyRoom(ProxyRoomMgr.NextRoomId(), num1, zoneID, players, this, npcId, pickUpWithNPC, isBot, isSmartBot: false);
			room.GuildId = num20;
			room.selfId = num22;
			room.AvgLevel = num21;
			room.startWithNpc = pickUpWithNPC; //botla eşleşmek için budan true olarak değer gelmesi laızm odayı başlattığında roaddan buraya gelecek olan pakette burası true olmalı
			//gameroomcreate paketinden bahsediyorum orda true dönmesi için o paketten gitmeden önce bi yerlerde odanın bu değerini true yapmak lazım
			room.RoomType = (eRoomType)num18;
			room.GameType = (eGameType)num19;
			room.IsCrossZone = flag;
			room.FightPower = num2;
			lock (m_rooms)
			{
				if (!m_rooms.ContainsKey(num1))
				{
					m_rooms.Add(num1, room);
				}
				else
				{
					room = null;
				}
			}
			if (room != null)
			{
				ProxyRoomMgr.AddRoom(room);
				return;
			}
			RemoveRoom(num1, room);
			ilog_1.ErrorFormat("Room already exists:{0}.", num1);
        }

        public void HandleGameRoomCancel(GSPacketIn pkg)
        {
			ProxyRoom room = null;
			lock (m_rooms)
			{
				if (m_rooms.ContainsKey(pkg.Parameter1))
				{
					room = m_rooms[pkg.Parameter1];
				}
			}
			if (room != null)
			{
				ProxyRoomMgr.RemoveRoom(room);
			}
        }

        public void HanleSendToGame(GSPacketIn pkg)
        {
			BaseGame game = GameMgr.FindGame(pkg.ClientID);
			if (game != null)
			{
				GSPacketIn pkg2 = pkg.ReadPacket();
				game.ProcessData(pkg2);
			}
        }

        public void SendRSAKey(byte[] m, byte[] e)
        {
			GSPacketIn pkg = new GSPacketIn(0);
			pkg.Write(m);
			pkg.Write(e);
			SendTCP(pkg);
        }

        public void SendPacketToPlayer(int playerId, GSPacketIn pkg)
        {
			GSPacketIn p = new GSPacketIn(32, playerId);
			p.WritePacket(pkg);
			SendTCP(p);
        }

        public void SendRemoveRoom(int roomId)
        {
			GSPacketIn pkg = new GSPacketIn(65, roomId);
			SendTCP(pkg);
        }

        public void SendToRoom(int roomId, GSPacketIn pkg, IGamePlayer except)
        {
			GSPacketIn p = new GSPacketIn(67, roomId);
			if (except != null)
			{
				p.Parameter1 = except.PlayerCharacter.ID;
				p.Parameter2 = except.GamePlayerId;
			}
			else
			{
				p.Parameter1 = 0;
				p.Parameter2 = 0;
			}
			p.WritePacket(pkg);
			SendTCP(p);
        }

        public void SendStartGame(int roomId, AbstractGame game)
        {
			GSPacketIn pkg = new GSPacketIn(66);
			pkg.Parameter1 = roomId;
			pkg.Parameter2 = game.Id;
			pkg.WriteInt((int)game.RoomType);
			pkg.WriteInt((int)game.GameType);
			pkg.WriteInt(game.TimeType);
			SendTCP(pkg);
        }

        public void SendStopGame(int roomId, int gameId)
        {
			GSPacketIn pkg = new GSPacketIn(68);
			pkg.Parameter1 = roomId;
			pkg.Parameter2 = gameId;
			SendTCP(pkg);
        }

        public void SendGamePlayerId(IGamePlayer player)
        {
			GSPacketIn pkg = new GSPacketIn(33);
			pkg.Parameter1 = player.PlayerCharacter.ID;
			pkg.Parameter2 = player.GamePlayerId;
			SendTCP(pkg);
        }

        public void SendAddRobRiches(int playerId, int value)
        {
			GSPacketIn pkg = new GSPacketIn(52, playerId);
			pkg.Parameter1 = value;
			pkg.WriteInt(value);
			SendTCP(pkg);
        }

        public void SendPlayerAddOffer(int playerId, int value)
        {
			GSPacketIn pkg = new GSPacketIn(51, playerId);
			pkg.Parameter1 = value;
			SendTCP(pkg);
        }

        public void SendDisconnectPlayer(int playerId)
        {
			GSPacketIn pkg = new GSPacketIn(34, playerId);
			SendTCP(pkg);
        }

        public void SendPlayerOnGameOver(int playerId, int gameId, bool isWin, int gainXp, bool isSpanArea, bool isCouple, int blood, int playerCount)
        {
			GSPacketIn pkg = new GSPacketIn(35, playerId)
			{
				Parameter1 = gameId
			};
			pkg.WriteBoolean(isWin);
			pkg.WriteInt(gainXp);
			pkg.WriteBoolean(isSpanArea);
			pkg.WriteBoolean(isCouple);
			pkg.WriteInt(blood);
			pkg.WriteInt(playerCount);
			SendTCP(pkg);
        }

        public void SendPlayerUsePropInGame(int playerId, int bag, int place, int templateId, bool isLiving)
        {
			GSPacketIn pkg = new GSPacketIn(36, playerId);
			pkg.Parameter1 = bag;
			pkg.Parameter2 = place;
			pkg.WriteInt(templateId);
			pkg.WriteBoolean(isLiving);
			SendTCP(pkg);
        }

        public void SendAddEliteGameScore(int playerId, int value)
        {
			SendTCP(new GSPacketIn(204, playerId)
			{
				Parameter1 = value
			});
        }

        public void SendRemoveEliteGameScore(int playerId, int value)
        {
			SendTCP(new GSPacketIn(205, playerId)
			{
				Parameter1 = value
			});
        }

        public void SendEliteGameWinUpdate(int playerId)
        {
			SendTCP(new GSPacketIn(206, playerId));
        }

        public void SendPlayerAddGold(int playerId, int value)
        {
			GSPacketIn pkg = new GSPacketIn(38, playerId);
			pkg.Parameter1 = value;
			SendTCP(pkg);
        }

        public void SendPlayerAddMoney(int playerId, int value, bool isAll)
        {
			GSPacketIn pkg = new GSPacketIn(74, playerId);
			pkg.Parameter1 = value;
			pkg.Parameter2 = (isAll ? 1 : 0);
			SendTCP(pkg);
        }

        public void SendPlayerAddGiftToken(int playerId, int value)
        {
			GSPacketIn pkg = new GSPacketIn(75, playerId);
			pkg.Parameter1 = value;
			SendTCP(pkg);
        }

        public void SendPlayerAddGP(int playerId, int value)
        {
			GSPacketIn pkg = new GSPacketIn(39, playerId);
			pkg.Parameter1 = value;
			SendTCP(pkg);
        }

        public void SendPlayerRemoveGP(int playerId, int value)
        {
			GSPacketIn pkg = new GSPacketIn(49, playerId);
			pkg.Parameter1 = value;
			SendTCP(pkg);
        }

        public void SendUpdateRestCount(int playerId)
        {
			GSPacketIn pkg = new GSPacketIn(86, playerId);
			SendTCP(pkg);
        }

        public void SendPlayerOnKillingLiving(int playerId, AbstractGame game, int type, int id, bool isLiving, int demage)
        {
			GSPacketIn pkg = new GSPacketIn(40, playerId);
			pkg.WriteInt(type);
			pkg.WriteBoolean(isLiving);
			pkg.WriteInt(demage);
			SendTCP(pkg);
        }

        public void SendPlayerOnMissionOver(int playerId, AbstractGame game, bool isWin, int MissionID, int turnNum)
        {
			GSPacketIn pkg = new GSPacketIn(41, playerId);
			pkg.WriteBoolean(isWin);
			pkg.WriteInt(MissionID);
			pkg.WriteInt(turnNum);
			SendTCP(pkg);
        }

        public void SendPlayerConsortiaFight(int playerId, int consortiaWin, int consortiaLose, Dictionary<int, Player> players, eRoomType roomType, eGameType gameClass, int totalKillHealth)
        {
			try
			{
				GSPacketIn pkg = new GSPacketIn(42, playerId);
				pkg.WriteInt(consortiaWin);
				pkg.WriteInt(consortiaLose);
				pkg.WriteInt(players.Count);
				for (int i = 0; i < players.Count; i++)
				{
					pkg.WriteInt(players[i].PlayerDetail.PlayerCharacter.ID);
				}
				pkg.WriteByte((byte)roomType);
				pkg.WriteByte((byte)gameClass);
				pkg.WriteInt(totalKillHealth);
				SendTCP(pkg);
			}
			catch(Exception)
			{
				ilog_1.ErrorFormat("SendPlayerConsortiaFight players.Count {0}", players.Count);
			}
        }

        public void SendPlayerSendConsortiaFight(int playerId, int consortiaID, int riches, string msg)
        {
			GSPacketIn pkg = new GSPacketIn(43, playerId);
			pkg.WriteInt(consortiaID);
			pkg.WriteInt(riches);
			pkg.WriteString(msg);
			SendTCP(pkg);
        }

        public void SendPlayerRemoveGold(int playerId, int value)
        {
			GSPacketIn pkg = new GSPacketIn(44, playerId);
			pkg.WriteInt(value);
			SendTCP(pkg);
        }

        public void SendPlayerRemoveMoney(int playerId, int value)
        {
			GSPacketIn pkg = new GSPacketIn(45, playerId);
			pkg.WriteInt(value);
			SendTCP(pkg);
        }

        public void SendPlayerRemoveOffer(int playerId, int value)
        {
			GSPacketIn pkg = new GSPacketIn(50, playerId);
			pkg.WriteInt(value);
			SendTCP(pkg);
        }

        public void SendPlayerAddTemplate(int playerId, ItemInfo cloneItem, eBageType bagType, int count)
        {
			if (cloneItem != null)
			{
				GSPacketIn pkg = new GSPacketIn(48, playerId);
				pkg.WriteInt(cloneItem.TemplateID);
				pkg.WriteByte((byte)bagType);
				pkg.WriteInt(count);
				pkg.WriteInt(cloneItem.ValidDate);
				pkg.WriteBoolean(cloneItem.IsBinds);
				pkg.WriteBoolean(cloneItem.IsUsed);
				pkg.WriteInt(cloneItem.StrengthenLevel);
				pkg.WriteInt(cloneItem.AttackCompose);
				pkg.WriteInt(cloneItem.DefendCompose);
				pkg.WriteInt(cloneItem.AgilityCompose);
				pkg.WriteInt(cloneItem.LuckCompose);
				pkg.WriteBoolean(cloneItem.IsGold);
				if (cloneItem.IsGold)
				{
					pkg.WriteDateTime(cloneItem.goldBeginTime);
					pkg.WriteInt(cloneItem.goldValidDate);
				}
				SendTCP(pkg);
			}
        }

        public void SendConsortiaAlly(int Consortia1, int Consortia2, int GameId)
        {
			GSPacketIn pkg = new GSPacketIn(77);
			pkg.WriteInt(Consortia1);
			pkg.WriteInt(Consortia2);
			pkg.WriteInt(GameId);
			SendTCP(pkg);
        }

        public void SendBeginFightNpc(int playerId, int RoomType, int GameType, int OrientRoomId, int countPlayer = 1)
        {
			GSPacketIn pkg = new GSPacketIn(88);
			pkg.Parameter1 = playerId;
			pkg.WriteInt(RoomType);
			pkg.WriteInt(GameType);
			pkg.WriteInt(OrientRoomId);
			pkg.WriteInt(countPlayer);
			SendTCP(pkg);
        }

        public void SendPlayerRemoveHealstone(int playerId)
        {
			GSPacketIn pkg = new GSPacketIn(73, playerId);
			SendTCP(pkg);
        }

        public ServerClient(FightServer svr)
			: base(new byte[8192], new byte[8192])
        {
			m_svr = svr;
        }

        public override string ToString()
        {
			return $"Server Client: {0} IsConnected:{base.IsConnected}  RoomCount:{m_rooms.Count}";
        }

        public void RemoveRoom(int orientId, ProxyRoom room)
        {
			bool flag = false;
			lock (m_rooms)
			{
				if (m_rooms.ContainsKey(orientId) && m_rooms[orientId] == room)
				{
					flag = m_rooms.Remove(orientId);
				}
			}
			if (flag)
			{
				SendRemoveRoom(orientId);
			}
        }

		public void SendUpdatePublicPlayer(int playerId, string tempStyle)
		{
			GSPacketIn send = new GSPacketIn((short)301, playerId);
			send.WriteString(tempStyle);
			SendTCP(send);
		}

		public void SendPlayerAddPrestige(int playerId, bool isWin, eRoomType roomType)
		{
			GSPacketIn pkg = new GSPacketIn((byte)eFightPackageType.PLAYER_ADD_PRESTIGE, playerId);
			pkg.Parameter1 = (int)roomType;
			pkg.WriteBoolean(isWin);
			SendTCP(pkg);
		}
		private void HandlePlayerReport(GSPacketIn pkg)
		{
			BaseGame game = GameMgr.FindGame(pkg.ClientID);
			if (game == null)
			{
				return;
			}
			//send message
			Player rpg = game.FindPlayerWithUserId(pkg.ReadInt());
			if (rpg != null)
			{
				rpg.PlayerDetail.SendMessage("Test report của Lâm Gay");
			}
			//
		}
		static ServerClient()
        {
			ilog_1 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        private void HandleAddViewer(GSPacketIn pkg)
        {
            try
            {
                int gameId = pkg.ClientID;
                BaseGame game = GameMgr.FindGame(gameId);
                if (game == null) { ilog_1.Error($"HandleAddViewer: Game {gameId} not found"); return; }

                // Viewer karakter bilgilerini oku
                PlayerInfo character = new PlayerInfo();
                character.ID = pkg.ReadInt();
                character.NickName = pkg.ReadString();
                character.Sex = pkg.ReadBoolean();
                character.Hide = pkg.ReadInt();
                character.Style = pkg.ReadString();
                character.Colors = pkg.ReadString();
                character.Skin = pkg.ReadString();
                character.Grade = pkg.ReadInt();
                character.Repute = pkg.ReadInt();
                character.ConsortiaID = pkg.ReadInt();
                character.ConsortiaName = pkg.ReadString();
                character.ConsortiaLevel = pkg.ReadInt();
                character.ConsortiaRepute = pkg.ReadInt();
                character.IsShowConsortia = pkg.ReadBoolean();
                character.badgeID = pkg.ReadInt();
                character.Honor = pkg.ReadString();
                character.AchievementPoint = pkg.ReadInt();
                character.FightPower = pkg.ReadInt();
                character.Nimbus = pkg.ReadInt();
                character.Win = pkg.ReadInt();
                character.Total = pkg.ReadInt();
                character.Offer = pkg.ReadInt();
                character.typeVIP = pkg.ReadByte();
                character.VIPLevel = pkg.ReadInt();
                character.apprenticeshipState = pkg.ReadInt();
                character.masterID = pkg.ReadInt();
                character.masterOrApprentices = pkg.ReadString();
                character.IsMarried = pkg.ReadBoolean();
                if (character.IsMarried)
                {
                    character.SpouseID = pkg.ReadInt();
                    character.SpouseName = pkg.ReadString();
                }
                character.hp = pkg.ReadInt();
                int zoneId = pkg.ReadInt();
                string zoneName = pkg.ReadString();

                // ProxyPlayer oluştur
                ProxyPlayerInfo proxyInfo = new ProxyPlayerInfo();
                proxyInfo.ZoneId = zoneId;
                proxyInfo.ZoneName = zoneName;
                ProxyPlayer viewerProxy = new ProxyPlayer(this, character, null,
                    new List<BufferInfo>(), new List<int>(), new List<BufferInfo>(),
                    proxyInfo, new UserMatchInfo());
                viewerProxy.IsViewer = true;

                // PhysicalId artır
                BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var fiPhysId = typeof(BaseGame).GetField("PhysicalId", BindingFlags.Public | BindingFlags.Instance);
                int physId = (int)fiPhysId.GetValue(game);
                fiPhysId.SetValue(game, physId + 1);

                // Player nesnesi + AddPlayer (protected)
                Player fp = new Player(viewerProxy, physId, game, 1, character.hp);
                var miAdd = typeof(BaseGame).GetMethod("AddPlayer", bf,
                    null, new Type[] { typeof(IGamePlayer), typeof(Player) }, null);
                miAdd.Invoke(game, new object[] { (IGamePlayer)viewerProxy, fp });

                // ── GAME_CREATE (101) → sadece viewer'a ──
                const byte GAME_CMD = 91;
                int roomTypeVal = (int)game.RoomType;
                int gameTypeVal = (int)game.GameType;
                int timeTypeVal = game.TimeType;

                var piLifeTime = typeof(BaseGame).GetProperty("LifeTime", bf);
                int lifeTimeVal = 0;
                if (piLifeTime != null) lifeTimeVal = Convert.ToInt32(piLifeTime.GetValue(game));
                else { var fl = typeof(BaseGame).GetField("LifeTime", bf); if (fl != null) lifeTimeVal = Convert.ToInt32(fl.GetValue(game)); }

                var allPlayers = game.GetAllFightPlayers();
                var pkgCreate = new GSPacketIn(GAME_CMD);
                if (lifeTimeVal > 0) pkgCreate.Parameter2 = lifeTimeVal;
                pkgCreate.WriteByte(101);
                pkgCreate.WriteInt((byte)roomTypeVal);
                pkgCreate.WriteInt((byte)gameTypeVal);
                pkgCreate.WriteInt(timeTypeVal);
                pkgCreate.WriteInt(allPlayers.Count);
                foreach (var player in allPlayers)
                {
                    IGamePlayer pd = player.PlayerDetail;
                    pkgCreate.WriteInt(pd.ZoneId);
                    pkgCreate.WriteString(pd.ZoneName ?? "");
                    pkgCreate.WriteInt(pd.PlayerCharacter.ID);
                    pkgCreate.WriteString(pd.PlayerCharacter.NickName ?? "");
                    pkgCreate.WriteBoolean(pd.IsViewer);
                    pkgCreate.WriteByte(pd.PlayerCharacter.typeVIP);
                    pkgCreate.WriteInt(pd.PlayerCharacter.VIPLevel);
                    pkgCreate.WriteBoolean(pd.PlayerCharacter.Sex);
                    pkgCreate.WriteInt(pd.PlayerCharacter.Hide);
                    pkgCreate.WriteString(pd.PlayerCharacter.Style ?? "");
                    pkgCreate.WriteString(pd.PlayerCharacter.Colors ?? "");
                    pkgCreate.WriteString(pd.PlayerCharacter.Skin ?? "");
                    pkgCreate.WriteInt(pd.PlayerCharacter.Grade);
                    pkgCreate.WriteInt(pd.PlayerCharacter.Repute);
                    if (pd.MainWeapon == null) { pkgCreate.WriteInt(0); }
                    else
                    {
                        pkgCreate.WriteInt(pd.MainWeapon.TemplateID);
                        pkgCreate.WriteInt(pd.MainWeapon.RefineryLevel);
                        pkgCreate.WriteString(pd.MainWeapon.Template?.Name ?? "");
                        pkgCreate.WriteDateTime(DateTime.MinValue);
                    }
                    if (pd.SecondWeapon == null) pkgCreate.WriteInt(0);
                    else pkgCreate.WriteInt(pd.SecondWeapon.TemplateID);
                    pkgCreate.WriteInt(pd.PlayerCharacter.Nimbus);
                    pkgCreate.WriteBoolean(pd.PlayerCharacter.IsShowConsortia);
                    pkgCreate.WriteInt(pd.PlayerCharacter.ConsortiaID);
                    pkgCreate.WriteString(pd.PlayerCharacter.ConsortiaName ?? "");
                    pkgCreate.WriteInt(pd.PlayerCharacter.badgeID);
                    pkgCreate.WriteInt(pd.PlayerCharacter.ConsortiaLevel);
                    pkgCreate.WriteInt(pd.PlayerCharacter.ConsortiaRepute);
                    pkgCreate.WriteInt(pd.PlayerCharacter.Win);
                    pkgCreate.WriteInt(pd.PlayerCharacter.Total);
                    pkgCreate.WriteInt(pd.PlayerCharacter.FightPower);
                    pkgCreate.WriteInt(pd.PlayerCharacter.apprenticeshipState);
                    pkgCreate.WriteInt(pd.PlayerCharacter.masterID);
                    pkgCreate.WriteString(pd.PlayerCharacter.masterOrApprentices ?? "");
                    pkgCreate.WriteInt(pd.PlayerCharacter.AchievementPoint);
                    pkgCreate.WriteString(pd.PlayerCharacter.Honor ?? "");
                    pkgCreate.WriteInt(pd.PlayerCharacter.Offer);
                    pkgCreate.WriteBoolean(pd.MatchInfo.DailyLeagueFirst);
                    pkgCreate.WriteInt(pd.MatchInfo.DailyLeagueLastScore);
                    pkgCreate.WriteBoolean(pd.PlayerCharacter.IsMarried);
                    if (pd.PlayerCharacter.IsMarried)
                    {
                        pkgCreate.WriteInt(pd.PlayerCharacter.SpouseID);
                        pkgCreate.WriteString(pd.PlayerCharacter.SpouseName ?? "");
                    }
                    pkgCreate.WriteInt(0); pkgCreate.WriteInt(0); pkgCreate.WriteInt(0);
                    pkgCreate.WriteInt(0); pkgCreate.WriteInt(0); pkgCreate.WriteInt(0);
                    pkgCreate.WriteInt(player.Team);
                    pkgCreate.WriteInt(player.Id);
                    pkgCreate.WriteInt(player.MaxBlood);
                    if (player.Pet == null) { pkgCreate.WriteInt(0); }
                    else
                    {
                        pkgCreate.WriteInt(1);
                        pkgCreate.WriteInt(player.Pet.Place);
                        pkgCreate.WriteInt(player.Pet.TemplateID);
                        pkgCreate.WriteInt(player.Pet.ID);
                        pkgCreate.WriteString(player.Pet.Name ?? "");
                        pkgCreate.WriteInt(player.Pet.UserID);
                        pkgCreate.WriteInt(player.Pet.Level);
                        string[] skillEquips = player.Pet.SkillEquip.Split('|');
                        pkgCreate.WriteInt(skillEquips.Length);
                        foreach (string skill in skillEquips)
                        {
                            var parts = skill.Split(',');
                            pkgCreate.WriteInt(int.Parse(parts[1]));
                            pkgCreate.WriteInt(int.Parse(parts[0]));
                        }
                    }
                }
                viewerProxy.SendTCP(pkgCreate);

                // ── START_LOADING (103) → harita yükle ──
                var pkgLoad = new GSPacketIn(GAME_CMD);
                if (lifeTimeVal > 0) pkgLoad.Parameter2 = lifeTimeVal;
                pkgLoad.WriteByte(103);
                pkgLoad.WriteInt(5);
                pkgLoad.WriteInt(game.Map.Info.ID);
                var fiLoadFiles = typeof(BaseGame).GetField("m_loadingFiles", bf);
                var loadFiles = fiLoadFiles?.GetValue(game) as System.Collections.IList;
                int loadCount = loadFiles?.Count ?? 0;
                pkgLoad.WriteInt(loadCount);
                if (loadFiles != null)
                {
                    foreach (var lf in loadFiles)
                    {
                        var lfType = lf.GetType();
                        var fType = lfType.GetField("Type"); var fPath = lfType.GetField("Path"); var fClass = lfType.GetField("ClassName");
                        var pType = lfType.GetProperty("Type"); var pPath = lfType.GetProperty("Path"); var pClass = lfType.GetProperty("ClassName");
                        int lt = pType != null ? (int)pType.GetValue(lf) : (fType != null ? (int)fType.GetValue(lf) : 0);
                        string lp = pPath != null ? (string)pPath.GetValue(lf) : (fPath != null ? (string)fPath.GetValue(lf) : "");
                        string lc = pClass != null ? (string)pClass.GetValue(lf) : (fClass != null ? (string)fClass.GetValue(lf) : "");
                        pkgLoad.WriteInt(lt); pkgLoad.WriteString(lp ?? ""); pkgLoad.WriteString(lc ?? "");
                    }
                }
                pkgLoad.WriteInt(0); // pet skills
                viewerProxy.SendTCP(pkgLoad);

                // ── START_GAME (99) → pozisyonlar ──
                var fightingPlayers = game.GetAllFightingPlayers();
                var pkgStart = new GSPacketIn(GAME_CMD);
                if (lifeTimeVal > 0) pkgStart.Parameter2 = lifeTimeVal;
                pkgStart.WriteByte(99);
                pkgStart.WriteInt(fightingPlayers.Count);
                foreach (var fPlayer in fightingPlayers)
                {
                    pkgStart.WriteInt(fPlayer.Id);
                    pkgStart.WriteInt(fPlayer.X); pkgStart.WriteInt(fPlayer.Y);
                    pkgStart.WriteInt(fPlayer.Direction);
                    pkgStart.WriteInt(fPlayer.Blood); pkgStart.WriteInt(fPlayer.MaxBlood);
                    pkgStart.WriteInt(fPlayer.Team);
                    pkgStart.WriteInt(fPlayer.Weapon != null ? fPlayer.Weapon.RefineryLevel : 0);
                    pkgStart.WriteInt(50); pkgStart.WriteInt(fPlayer.Dander);
                    var buffs = fPlayer.PlayerDetail.FightBuffs;
                    pkgStart.WriteInt(buffs != null ? buffs.Count : 0);
                    if (buffs != null) { foreach (var b in buffs) { pkgStart.WriteInt(b.Type); pkgStart.WriteInt(b.Value); } }
                    pkgStart.WriteInt(0);
                    pkgStart.WriteBoolean(fPlayer.IsFrost);
                    pkgStart.WriteBoolean(fPlayer.IsHide);
                    pkgStart.WriteBoolean(fPlayer.IsNoHole);
                    pkgStart.WriteBoolean(false);
                    pkgStart.WriteInt(0);
                }
                pkgStart.WriteDateTime(DateTime.Now);
                viewerProxy.SendTCP(pkgStart);

                // ── Background cleanup ──
                int viewerPlayerId = fp.Id;
                System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        while (true)
                        {
                            await System.Threading.Tasks.Task.Delay(300);
                            try { var s = game.GameState.ToString(); if (s != "Playing" && s != "GameStart") break; }
                            catch { break; }
                        }
                        try
                        {
                            var fiP = typeof(BaseGame).GetField("m_players", BindingFlags.NonPublic | BindingFlags.Instance);
                            var pl = fiP?.GetValue(game) as System.Collections.IDictionary;
                            if (pl != null) lock (pl) { pl.Remove(viewerPlayerId); }
                        }
                        catch { }
                    }
                    catch { }
                });

                ilog_1.Info($"PVP Viewer added: {character.NickName} (ID:{character.ID}) to game {gameId}");
            }
            catch (Exception ex)
            {
                ilog_1.Error("HandleAddViewer error", ex);
            }
        }

    }
}

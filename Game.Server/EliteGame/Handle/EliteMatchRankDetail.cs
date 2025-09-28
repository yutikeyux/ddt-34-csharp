using Game.Base.Packets;
using Game.Logic;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Linq;
using Game.Server.Packets;

namespace Game.Server.EliteGame.Handle
{
    [EliteGameHandleAttbute((byte)EliteGamePackageType.ELITE_MATCH_RANK_DETAIL)]
    public class EliteMatchRankDetail : IEliteGameCommandHadler//
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            int param = packet.ReadInt();
            List<PlayerEliteGameInfo> list = ExerciseMgr.EliteGameChampionPlayersList.Where(delegate (KeyValuePair<int, PlayerEliteGameInfo> a)
            {
                KeyValuePair<int, PlayerEliteGameInfo> keyValuePair2 = a;
                return keyValuePair2.Value.GameType == param;
            }).Select(delegate (KeyValuePair<int, PlayerEliteGameInfo> a)
            {
                KeyValuePair<int, PlayerEliteGameInfo> keyValuePair = a;
                return keyValuePair.Value;
            }).ToList();
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ELITEGAME);
            pkg.WriteByte((byte)EliteGamePackageType.ELITE_MATCH_RANK_DETAIL);
            pkg.WriteInt(param);
            pkg.WriteInt(list.Count);
            foreach (PlayerEliteGameInfo item in list)
            {
                pkg.WriteInt(item.UserID);
                pkg.WriteString(item.NickName);
                pkg.WriteInt(item.Rank);
                pkg.WriteInt(item.Status);
                pkg.WriteInt(item.Winer);
            }
            Player.Out.SendTCP(pkg);
            return true;
        }
    }
}
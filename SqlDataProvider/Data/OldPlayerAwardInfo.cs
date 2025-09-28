using System;

namespace SqlDataProvider.Data
{
    public class OldPlayerAwardInfo
    {
        int _rewardItemID;
        int _rewardItemValid;
        int _rewardItemCount;
        int _strengthenLevel;
        int _attackCompose;
        int _defendCompose;
        int _agilityCompose;
        int _luckCompose;
        bool _isBind;
        public ItemInfo _itemInfo;
        public int RewardItemID { get => _rewardItemID; set => _rewardItemID = value; }
        public int RewardItemValid { get => _rewardItemValid; set => _rewardItemValid = value; }
        public int RewardItemCount { get => _rewardItemCount; set => _rewardItemCount = value; }
        public int StrengthenLevel { get => _strengthenLevel; set => _strengthenLevel = value; }
        public int AttackCompose { get => _attackCompose; set => _attackCompose = value; }
        public int DefendCompose { get => _defendCompose; set => _defendCompose = value; }
        public int AgilityCompose { get => _agilityCompose; set => _agilityCompose = value; }
        public int LuckCompose { get => _luckCompose; set => _luckCompose = value; }
        public bool IsBind { get => _isBind; set => _isBind = value; }

        public ItemInfo itemInfo { get => _itemInfo; set => _itemInfo = value; }
    }
}

namespace SqlDataProvider.Data
{
    public class EventRewardProcessInfo : DataObject
    {
        private int _userID;

        private int _activeType;

        private int _conditions;

        private int _awardGot;

		private bool _isReset;

        public int ActiveType
        {
			get
			{
				return _activeType;
			}
			set
			{
                _activeType = value;
				_isDirty = true;
			}
        }

        public int AwardGot
        {
			get
			{
				return _awardGot;
			}
			set
			{
                _awardGot = value;
				_isDirty = true;
			}
        }

        public int Conditions
        {
			get
			{
				return _conditions;
			}
			set
			{
                _conditions = value;
				_isDirty = true;
			}
        }

        public int UserID
        {
			get
			{
				return _userID;
			}
			set
			{
                _userID = value;
				_isDirty = true;
			}
        }

        public bool IsReset
        {
            get
            {
                return _isReset;
            }
            set
            {
                _isReset = value;
                _isDirty = true;
            }
        }
    }
}

namespace SqlDataProvider.Data
{
	public class UserGmActivityReward : DataObject
	{
		private long m_ID;

		private int m_UserID;

		private string m_ActivityID;

		private string m_GiftBagID;

		private int m_Times;

		public long ID
		{
			get
			{
				return m_ID;
			}
			set
			{
				m_ID = value;
				_isDirty = true;
			}
		}

		public int UserID
		{
			get
			{
				return m_UserID;
			}
			set
			{
				m_UserID = value;
				_isDirty = true;
			}
		}

		public string ActivityID
		{
			get
			{
				return m_ActivityID;
			}
			set
			{
				m_ActivityID = value;
				_isDirty = true;
			}
		}

		public string GiftBagID
		{
			get
			{
				return m_GiftBagID;
			}
			set
			{
				m_GiftBagID = value;
				_isDirty = true;
			}
		}

		public int Times
		{
			get
			{
				return m_Times;
			}
			set
			{
				m_Times = value;
				_isDirty = true;
			}
		}
	}
}

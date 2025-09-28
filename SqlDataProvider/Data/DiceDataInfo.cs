using System;
namespace SqlDataProvider.Data
{
	public class DiceDataInfo : DataObject
	{
		private int _ID;
		private int _userID;
		private int _luckIntegral;
		private int _luckIntegralLevel;
		private int _level;
		private int _freeCount;
		private int _currentPosition;
		private bool _userFirstCell;
		private string _awardArray;
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
				this._isDirty = true;
			}
		}
		public int UserID
		{
			get
			{
				return this._userID;
			}
			set
			{
				this._userID = value;
				this._isDirty = true;
			}
		}
		public int LuckIntegral
		{
			get
			{
				return this._luckIntegral;
			}
			set
			{
				this._luckIntegral = value;
				this._isDirty = true;
			}
		}
		public int LuckIntegralLevel
		{
			get
			{
				return this._luckIntegralLevel;
			}
			set
			{
				this._luckIntegralLevel = value;
				this._isDirty = true;
			}
		}
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				this._level = value;
				this._isDirty = true;
			}
		}
		public int FreeCount
		{
			get
			{
				return this._freeCount;
			}
			set
			{
				this._freeCount = value;
				this._isDirty = true;
			}
		}
		public int CurrentPosition
		{
			get
			{
				return this._currentPosition;
			}
			set
			{
				this._currentPosition = value;
				this._isDirty = true;
			}
		}
		public bool UserFirstCell
		{
			get
			{
				return this._userFirstCell;
			}
			set
			{
				this._userFirstCell = value;
				this._isDirty = true;
			}
		}
		public string AwardArray
		{
			get
			{
				return this._awardArray;
			}
			set
			{
				this._awardArray = value;
				this._isDirty = true;
			}
		}
	}
}

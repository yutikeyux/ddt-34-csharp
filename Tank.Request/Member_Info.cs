using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Diagnostics;

namespace Tank.Request
{
	// Token: 0x0200002F RID: 47
	[Table(Name = "dbo.Member_Info")]
	public class Member_Info : INotifyPropertyChanging, INotifyPropertyChanged
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00008020 File Offset: 0x00006220
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00008038 File Offset: 0x00006238
		[Column(Storage = "_ID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				bool flag = this._ID != value;
				if (flag)
				{
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
				}
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00008074 File Offset: 0x00006274
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x0000808C File Offset: 0x0000628C
		[Column(Storage = "_Username", DbType = "NVarChar(50)")]
		public string Username
		{
			get
			{
				return this._Username;
			}
			set
			{
				bool flag = this._Username != value;
				if (flag)
				{
					this.SendPropertyChanging();
					this._Username = value;
					this.SendPropertyChanged("Username");
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000080C8 File Offset: 0x000062C8
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x000080E0 File Offset: 0x000062E0
		[Column(Storage = "_Password", DbType = "NVarChar(255)")]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				bool flag = this._Password != value;
				if (flag)
				{
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
				}
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x0000811C File Offset: 0x0000631C
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00008134 File Offset: 0x00006334
		[Column(Storage = "_Email", DbType = "NVarChar(50)")]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				bool flag = this._Email != value;
				if (flag)
				{
					this.SendPropertyChanging();
					this._Email = value;
					this.SendPropertyChanged("Email");
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00008170 File Offset: 0x00006370
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00008188 File Offset: 0x00006388
		[Column(Storage = "_Phone", DbType = "NVarChar(15)")]
		public string Phone
		{
			get
			{
				return this._Phone;
			}
			set
			{
				bool flag = this._Phone != value;
				if (flag)
				{
					this.SendPropertyChanging();
					this._Phone = value;
					this.SendPropertyChanged("Phone");
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000DB RID: 219 RVA: 0x000081C4 File Offset: 0x000063C4
		// (set) Token: 0x060000DC RID: 220 RVA: 0x000081DC File Offset: 0x000063DC
		[Column(Storage = "_BroCoin", DbType = "Int NOT NULL")]
		public int BroCoin
		{
			get
			{
				return this._BroCoin;
			}
			set
			{
				bool flag = this._BroCoin != value;
				if (flag)
				{
					this.SendPropertyChanging();
					this._BroCoin = value;
					this.SendPropertyChanged("BroCoin");
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000DD RID: 221 RVA: 0x00008218 File Offset: 0x00006418
		// (remove) Token: 0x060000DE RID: 222 RVA: 0x00008250 File Offset: 0x00006450
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangingEventHandler PropertyChanging;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000DF RID: 223 RVA: 0x00008288 File Offset: 0x00006488
		// (remove) Token: 0x060000E0 RID: 224 RVA: 0x000082C0 File Offset: 0x000064C0
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060000E1 RID: 225 RVA: 0x000082F8 File Offset: 0x000064F8
		protected virtual void SendPropertyChanging()
		{
			bool flag = this.PropertyChanging != null;
			if (flag)
			{
				this.PropertyChanging(this, Member_Info.emptyChangingEventArgs);
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00008328 File Offset: 0x00006528
		protected virtual void SendPropertyChanged(string propertyName)
		{
			bool flag = this.PropertyChanged != null;
			if (flag)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0400002D RID: 45
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

		// Token: 0x0400002E RID: 46
		private int _ID;

		// Token: 0x0400002F RID: 47
		private string _Username;

		// Token: 0x04000030 RID: 48
		private string _Password;

		// Token: 0x04000031 RID: 49
		private string _Email;

		// Token: 0x04000032 RID: 50
		private string _Phone;

		// Token: 0x04000033 RID: 51
		private int _BroCoin;
	}
}

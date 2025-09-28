using System;

namespace SqlDataProvider.Data
{
	// Token: 0x0200011D RID: 285
	public class UserGmActivityCondition : DataObject
	{
		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06001643 RID: 5699 RVA: 0x00016424 File Offset: 0x00014624
		// (set) Token: 0x06001644 RID: 5700 RVA: 0x0001643C File Offset: 0x0001463C
		public long ID
		{
			get
			{
				return this.m_ID;
			}
			set
			{
				this.m_ID = value;
				this._isDirty = true;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06001645 RID: 5701 RVA: 0x00016450 File Offset: 0x00014650
		// (set) Token: 0x06001646 RID: 5702 RVA: 0x00016468 File Offset: 0x00014668
		public int UserID
		{
			get
			{
				return this.m_UserID;
			}
			set
			{
				this.m_UserID = value;
				this._isDirty = true;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06001647 RID: 5703 RVA: 0x0001647C File Offset: 0x0001467C
		// (set) Token: 0x06001648 RID: 5704 RVA: 0x00016494 File Offset: 0x00014694
		public string ActivityID
		{
			get
			{
				return this.m_ActivityID;
			}
			set
			{
				this.m_ActivityID = value;
				this._isDirty = true;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06001649 RID: 5705 RVA: 0x000164A8 File Offset: 0x000146A8
		// (set) Token: 0x0600164A RID: 5706 RVA: 0x000164C0 File Offset: 0x000146C0
		public string GiftBagID
		{
			get
			{
				return this.m_GiftBagID;
			}
			set
			{
				this.m_GiftBagID = value;
				this._isDirty = true;
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x0600164B RID: 5707 RVA: 0x000164D4 File Offset: 0x000146D4
		// (set) Token: 0x0600164C RID: 5708 RVA: 0x000164EC File Offset: 0x000146EC
		public int StatusID
		{
			get
			{
				return this.m_StatusID;
			}
			set
			{
				this.m_StatusID = value;
				this._isDirty = true;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x0600164D RID: 5709 RVA: 0x00016500 File Offset: 0x00014700
		// (set) Token: 0x0600164E RID: 5710 RVA: 0x00016518 File Offset: 0x00014718
		public int StatusValue
		{
			get
			{
				return this.m_StatusValue;
			}
			set
			{
				this.m_StatusValue = value;
				this._isDirty = true;
			}
		}

		// Token: 0x04000E51 RID: 3665
		private long m_ID;

		// Token: 0x04000E52 RID: 3666
		private int m_UserID;

		// Token: 0x04000E53 RID: 3667
		private string m_ActivityID;

		// Token: 0x04000E54 RID: 3668
		private string m_GiftBagID;

		// Token: 0x04000E55 RID: 3669
		private int m_StatusID;

		// Token: 0x04000E56 RID: 3670
		private int m_StatusValue;
	}
}

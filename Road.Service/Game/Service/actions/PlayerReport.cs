using System;

namespace Game.Service.actions
{
	// Token: 0x02000009 RID: 9
	public class PlayerReport
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600008F RID: 143 RVA: 0x0000E5FB File Offset: 0x0000C7FB
		// (set) Token: 0x06000090 RID: 144 RVA: 0x0000E603 File Offset: 0x0000C803
		public int Id { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000091 RID: 145 RVA: 0x0000E60C File Offset: 0x0000C80C
		// (set) Token: 0x06000092 RID: 146 RVA: 0x0000E614 File Offset: 0x0000C814
		public string ReporterName { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000E61D File Offset: 0x0000C81D
		// (set) Token: 0x06000094 RID: 148 RVA: 0x0000E625 File Offset: 0x0000C825
		public string ReportedPlayerName { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000E62E File Offset: 0x0000C82E
		// (set) Token: 0x06000096 RID: 150 RVA: 0x0000E636 File Offset: 0x0000C836
		public string Reason { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000097 RID: 151 RVA: 0x0000E63F File Offset: 0x0000C83F
		// (set) Token: 0x06000098 RID: 152 RVA: 0x0000E647 File Offset: 0x0000C847
		public DateTime ReportDate { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000E650 File Offset: 0x0000C850
		// (set) Token: 0x0600009A RID: 154 RVA: 0x0000E658 File Offset: 0x0000C858
		public string Status { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600009B RID: 155 RVA: 0x0000E661 File Offset: 0x0000C861
		// (set) Token: 0x0600009C RID: 156 RVA: 0x0000E669 File Offset: 0x0000C869
		public string Details { get; set; }
	}
}

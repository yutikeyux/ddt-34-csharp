using System;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Tank.Request
{
	// Token: 0x0200002D RID: 45
	[Database(Name = "Project_Member34")]
	public class DbMemberDataContext : DataContext
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x0000773C File Offset: 0x0000593C
		public DbMemberDataContext() : base(ConfigurationManager.ConnectionStrings["Project_Member34ConnectionString"].ConnectionString, DbMemberDataContext.mappingSource)
		{
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000775F File Offset: 0x0000595F
		public DbMemberDataContext(string connection) : base(connection, DbMemberDataContext.mappingSource)
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000776F File Offset: 0x0000596F
		public DbMemberDataContext(IDbConnection connection) : base(connection, DbMemberDataContext.mappingSource)
		{
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000777F File Offset: 0x0000597F
		public DbMemberDataContext(string connection, MappingSource mappingSource) : base(connection, mappingSource)
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000778B File Offset: 0x0000598B
		public DbMemberDataContext(IDbConnection connection, MappingSource mappingSource) : base(connection, mappingSource)
		{
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00007798 File Offset: 0x00005998
		public Table<Member_Info> Member_Infos
		{
			get
			{
				return base.GetTable<Member_Info>();
			}
		}

		// Token: 0x0400002B RID: 43
		private static MappingSource mappingSource = new AttributeMappingSource();
	}
}

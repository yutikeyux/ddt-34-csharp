using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.Text;

namespace Game.Service.actions
{
	// Token: 0x0200000A RID: 10
	public class ServiceInstall : IAction
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000E674 File Offset: 0x0000C874
		public string Name
		{
			get
			{
				return "--serviceinstall";
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600009F RID: 159 RVA: 0x0000E68C File Offset: 0x0000C88C
		public string Syntax
		{
			get
			{
				return "--serviceinstall";
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000E6A4 File Offset: 0x0000C8A4
		public string Description
		{
			get
			{
				return "Installs DOL as system service with he given parameters";
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000E6BC File Offset: 0x0000C8BC
		public void OnAction(Hashtable parameters)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.Add("/LogToConsole=false");
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in parameters)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				bool flag = stringBuilder.Length > 0;
				bool flag2 = flag;
				if (flag2)
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append(dictionaryEntry.Key);
				stringBuilder.Append("=");
				stringBuilder.Append(dictionaryEntry.Value);
			}
			arrayList.Add("commandline=" + stringBuilder.ToString());
			string[] commandLine = (string[])arrayList.ToArray(typeof(string));
			AssemblyInstaller assemblyInstaller = new AssemblyInstaller(Assembly.GetExecutingAssembly(), commandLine);
			Hashtable hashtable = new Hashtable();
			bool flag3 = GameServerService.GetDOLService() != null;
			bool flag4 = flag3;
			if (flag4)
			{
				Console.WriteLine("DOL service is already installed!");
			}
			else
			{
				Console.WriteLine("Installing Road as system service...");
				try
				{
					assemblyInstaller.Install(hashtable);
					assemblyInstaller.Commit(hashtable);
				}
				catch (Exception ex)
				{
					assemblyInstaller.Rollback(hashtable);
					Console.WriteLine("Error installing as system service");
					Console.WriteLine(ex.Message);
					return;
				}
				Console.WriteLine("Finished!");
			}
		}
	}
}

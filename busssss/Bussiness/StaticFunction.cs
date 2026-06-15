using System;
using System.Configuration;
using System.Reflection;
using log4net;

namespace Bussiness
{
	// Token: 0x0200001C RID: 28
	public class StaticFunction
	{
		// Token: 0x0600025B RID: 603 RVA: 0x000335A4 File Offset: 0x000317A4
		public static bool UpdateConfig(string fileName, string name, string value)
		{
			try
			{
				Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
				{
					ExeConfigFilename = fileName
				}, ConfigurationUserLevel.None);
				configuration.AppSettings.Settings[name].Value = value;
				configuration.Save();
				ConfigurationManager.RefreshSection("appSettings");
				return true;
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = StaticFunction.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					StaticFunction.log.Error("UpdateConfig", exception);
				}
			}
			return false;
		}

		// Token: 0x040000EF RID: 239
		protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

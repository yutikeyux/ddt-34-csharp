using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000028 RID: 40
	public class csFunction
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000064BC File Offset: 0x000046BC
		public static string GetAdminIP
		{
			get
			{
				return ConfigurationSettings.AppSettings["AdminIP"];
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000064E0 File Offset: 0x000046E0
		public static bool ValidAdminIP(string ip)
		{
			string ips = csFunction.GetAdminIP;
			return string.IsNullOrEmpty(ips) || ips.Split(new char[]
			{
				'|'
			}).Contains(ip);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00006524 File Offset: 0x00004724
		public static string ConvertSql(string inputString)
		{
			return inputString;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00006538 File Offset: 0x00004738
		public static bool SqlChar(string v)
		{
			bool flag = v.Trim() != "";
			if (flag)
			{
				foreach (string a in csFunction.al)
				{
					bool flag2 = v.IndexOf(a + " ") > -1 || v.IndexOf(" " + a) > -1;
					if (flag2)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000065B8 File Offset: 0x000047B8
		public static string CreateCompressXml(HttpContext context, XElement result, string file, bool isCompress)
		{
			string path = context.Server.MapPath("~");
			return csFunction.CreateCompressXml(path, result, file, isCompress);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000065E8 File Offset: 0x000047E8
		public static string CreateCompressXml(XElement result, string file, bool isCompress)
		{
			string path = StaticsMgr.CurrentPath;
			return csFunction.CreateCompressXml(path, result, file, isCompress);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000660C File Offset: 0x0000480C
		public static string CreateCompressXml(string path, XElement result, string file, bool isCompress)
		{
			string result2;
			try
			{
				file += ".xml";
				path = Path.Combine(path, file);
				using (FileStream fs = new FileStream(path, FileMode.Create))
				{
					if (isCompress)
					{
						using (BinaryWriter writer = new BinaryWriter(fs))
						{
							writer.Write(StaticFunction.Compress(result.ToString(false)));
						}
					}
					else
					{
						using (StreamWriter wirter = new StreamWriter(fs))
						{
							wirter.Write(result.ToString(false));
						}
					}
				}
				result2 = "İşlem:" + file + "Başarılı!";
			}
			catch (Exception ex)
			{
				csFunction.log.Error("CreateCompressXml " + file + " is Başarısız!", ex);
				result2 = "İşlem:" + file + ",Başarısız!";
			}
			return result2;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00006720 File Offset: 0x00004920
		public static string BuildCelebConsortia(string file, int order)
		{
			return csFunction.BuildCelebConsortia(file, order, "");
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006740 File Offset: 0x00004940
		public static string BuildCelebConsortia(string file, int order, string fileNotCompress)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			int total = 0;
			try
			{
				int page = 1;
				int size = 50;
				int consortiaID = -1;
				string name = "";
				int level = -1;
				using (ConsortiaBussiness db = new ConsortiaBussiness())
				{
					ConsortiaInfo[] infos = db.GetConsortiaPage(page, size, ref total, order, name, consortiaID, level, -1);
					foreach (ConsortiaInfo info in infos)
					{
						XElement node = FlashUtils.CreateConsortiaInfo(info);
						bool flag = info.ChairmanID != 0;
						if (flag)
						{
							using (PlayerBussiness pb = new PlayerBussiness())
							{
								PlayerInfo player = pb.GetUserSingleByUserID(info.ChairmanID);
								bool flag2 = player != null;
								if (flag2)
								{
									node.Add(FlashUtils.CreateCelebInfo(player));
								}
							}
						}
						result.Add(node);
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				csFunction.log.Error(file + " is Başarısız!", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			result.Add(new XAttribute("date", DateTime.Today.ToString("yyyy-MM-dd")));
			bool flag3 = !string.IsNullOrEmpty(fileNotCompress);
			if (flag3)
			{
				csFunction.CreateCompressXml(result, fileNotCompress, false);
			}
			return csFunction.CreateCompressXml(result, file, true);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00006928 File Offset: 0x00004B28
		public static string BuildCelebUsers(string file, int order)
		{
			return csFunction.BuildCelebUsers(file, order, "");
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00006948 File Offset: 0x00004B48
		public static string BuildEliteMatchPlayerList(string file)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				int page = 1;
				int size = 50;
				int userID = -1;
				int total = 0;
				bool resultValue = false;
				using (PlayerBussiness db = new PlayerBussiness())
				{
					PlayerInfo[] infos = db.GetPlayerPage(page, size, ref total, 7, userID, ref resultValue);
					bool flag = resultValue;
					if (flag)
					{
						int rank = 1;
						int rank2 = 1;
						XElement set = new XElement("ItemSet", new XAttribute("value", 1));
						XElement set2 = new XElement("ItemSet", new XAttribute("value", 2));
						foreach (PlayerInfo info in infos)
						{
							bool flag2 = info.Grade <= 40;
							if (flag2)
							{
								set.Add(FlashUtils.CreateEliteMatchPlayersList(info, rank));
								rank++;
							}
							else
							{
								set2.Add(FlashUtils.CreateEliteMatchPlayersList(info, rank2));
								rank2++;
							}
						}
						result.Add(set);
						result.Add(set2);
						value = true;
						message = "Success!";
					}
				}
			}
			catch (Exception ex)
			{
				csFunction.log.Error(file + " is Başarısız!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			result.Add(new XAttribute("lastUpdateTime", DateTime.Now.ToString()));
			csFunction.CreateCompressXml(result, file + "_out", false);
			return csFunction.CreateCompressXml(result, file, true);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00006B58 File Offset: 0x00004D58
		public static string BuildCelebUsers(string file, int order, string fileNotCompress)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				int page = 1;
				int size = 50;
				int userID = -1;
				int total = 0;
				bool resultValue = false;
				using (PlayerBussiness db = new PlayerBussiness())
				{
					db.UpdateUserReputeFightPower();
					PlayerInfo[] infos = db.GetPlayerPage(page, size, ref total, order, userID, ref resultValue);
					bool flag = resultValue;
					if (flag)
					{
						foreach (PlayerInfo info in infos)
						{
							result.Add(FlashUtils.CreateCelebInfo(info));
						}
						value = true;
						message = "Success!";
					}
				}
			}
			catch (Exception ex)
			{
				csFunction.log.Error(file + " is Başarısız!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			result.Add(new XAttribute("date", DateTime.Today.ToString("yyyy-MM-dd")));
			bool flag2 = !string.IsNullOrEmpty(fileNotCompress);
			if (flag2)
			{
				csFunction.CreateCompressXml(result, fileNotCompress, false);
			}
			return csFunction.CreateCompressXml(result, file, true);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00006CC4 File Offset: 0x00004EC4
		public static string BuildCelebConsortiaFightPower(string file, string fileNotCompress)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			int total = 0;
			try
			{
				using (ConsortiaBussiness db = new ConsortiaBussiness())
				{
					ConsortiaInfo[] infos = db.UpdateConsortiaFightPower();
					total = infos.Length;
					foreach (ConsortiaInfo info in infos)
					{
						XElement node = FlashUtils.CreateConsortiaInfo(info);
						bool flag = info.ChairmanID != 0;
						if (flag)
						{
							using (PlayerBussiness pb = new PlayerBussiness())
							{
								PlayerInfo player = pb.GetUserSingleByUserID(info.ChairmanID);
								bool flag2 = player != null;
								if (flag2)
								{
									node.Add(FlashUtils.CreateCelebInfo(player));
								}
							}
						}
						result.Add(node);
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				csFunction.log.Error(file + " is Başarısız!", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			result.Add(new XAttribute("date", DateTime.Today.ToString("yyyy-MM-dd")));
			bool flag3 = !string.IsNullOrEmpty(fileNotCompress);
			if (flag3)
			{
				csFunction.CreateCompressXml(result, fileNotCompress, false);
			}
			return csFunction.CreateCompressXml(result, file, true);
		}

		// Token: 0x04000024 RID: 36
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000025 RID: 37
		private static string[] al = ";|and|1=1|exec|insert|select|delete|update|like|count|chr|mid|master|or|truncate|char|declare|join".Split(new char[]
		{
			'|'
		});
	}
}

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000065 RID: 101
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class QuestList : IHttpHandler
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x0000E6B4 File Offset: 0x0000C8B4
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(QuestList.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000E700 File Offset: 0x0000C900
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					QuestInfo[] quests = db.GetALlQuest();
					QuestAwardInfo[] questgoods = db.GetAllQuestGoods();
					QuestConditionInfo[] questcondiction = db.GetAllQuestCondiction();
					QuestInfo[] array = quests;
					for (int i = 0; i < array.Length; i++)
					{
						QuestInfo quest = array[i];
						XElement temp_xml = FlashUtils.CreateQuestInfo(quest);
						IEnumerable temp_questcondiction = from s in questcondiction
						where s.QuestID == quest.ID
						select s;
						foreach (object obj in temp_questcondiction)
						{
							QuestConditionInfo item = (QuestConditionInfo)obj;
							temp_xml.Add(FlashUtils.CreateQuestCondiction(item));
						}
						IEnumerable temp_questgoods = from s in questgoods
						where s.QuestID == quest.ID
						select s;
						foreach (object obj2 in temp_questgoods)
						{
							QuestAwardInfo item2 = (QuestAwardInfo)obj2;
							temp_xml.Add(FlashUtils.CreateQuestGoods(item2));
						}
						result.Add(temp_xml);
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				QuestList.log.Error("QuestList", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "QuestList", true);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000E930 File Offset: 0x0000CB30
		private static void AppendAttribute(XmlDocument doc, XmlNode node, string attr, string value)
		{
			XmlAttribute at = doc.CreateAttribute(attr);
			at.Value = value;
			node.Attributes.Append(at);
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000E95C File Offset: 0x0000CB5C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400006C RID: 108
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

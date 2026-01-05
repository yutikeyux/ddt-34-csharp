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
	// Token: 0x02000068 RID: 104
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class QuestList : IHttpHandler
	{
		// Token: 0x060001DB RID: 475 RVA: 0x0000E218 File Offset: 0x0000C418
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(QuestList.Build(context));
			}
			else
			{
				context.Response.Write("Tabi Efendim!");
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000E264 File Offset: 0x0000C464
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
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

		// Token: 0x060001DD RID: 477 RVA: 0x0000E494 File Offset: 0x0000C694
		private static void AppendAttribute(XmlDocument doc, XmlNode node, string attr, string value)
		{
			XmlAttribute at = doc.CreateAttribute(attr);
			at.Value = value;
			node.Attributes.Append(at);
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000071 RID: 113
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

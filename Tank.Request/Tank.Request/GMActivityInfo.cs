using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200003C RID: 60
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class GMActivityInfo : IHttpHandler
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00002D8D File Offset: 0x00000F8D
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00009108 File Offset: 0x00007308
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(GMActivityInfo.Bulid(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00009154 File Offset: 0x00007354
		public static string Bulid(HttpContext context)
		{
			XElement xElement = new XElement("Result");
			string value = "Fail!";
			string value2 = "false";
			try
			{
				ProduceBussiness val = new ProduceBussiness();
				try
				{
					GmActivityInfo[] allGmActivity = val.GetAllGmActivity();
					GmGiftInfo[] allGmGift = val.GetAllGmGift();
					GmActiveConditionInfo[] allGmActiveCondition = val.GetAllGmActiveCondition();
					GmActiveRewardInfo[] allGmActiveReward = val.GetAllGmActiveReward();
					GmActivityInfo[] array = allGmActivity;
					GmActivityInfo[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						GmActivityInfo activityInfo = array2[i];
						XElement xElement2 = new XElement("ActiveInfo");
						XElement content = FlashUtils.CreateGMActivityInfo(activityInfo);
						XElement xElement3 = new XElement("ActiveGiftBag");
						IEnumerable<GmGiftInfo> source = allGmGift;
						Func<GmGiftInfo, bool> predicate;
						Func<GmGiftInfo, bool> <>9__0;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((GmGiftInfo s) => s.activityId == activityInfo.activityId));
						}
						using (IEnumerator<GmGiftInfo> enumerator = source.Where(predicate).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								GmGiftInfo gmGiftInfo = enumerator.Current;
								XElement content2 = FlashUtils.CreateGMGiftInfo(gmGiftInfo);
								XElement xElement4 = new XElement("ActiveCondition");
								XElement xElement5 = new XElement("ActiveReward");
								bool flag = false;
								bool flag2 = false;
								IEnumerable<GmActiveConditionInfo> source2 = allGmActiveCondition;
								Func<GmActiveConditionInfo, bool> predicate2;
								Func<GmActiveConditionInfo, bool> <>9__1;
								if ((predicate2 = <>9__1) == null)
								{
									predicate2 = (<>9__1 = ((GmActiveConditionInfo s) => s.giftbagId == gmGiftInfo.giftbagId));
								}
								foreach (GmActiveConditionInfo item in source2.Where(predicate2))
								{
									xElement4.Add(FlashUtils.CreateGMConditionInfo(item));
									bool flag3 = !flag2;
									if (flag3)
									{
										flag2 = true;
									}
								}
								IEnumerable<GmActiveRewardInfo> source3 = allGmActiveReward;
								Func<GmActiveRewardInfo, bool> predicate3;
								Func<GmActiveRewardInfo, bool> <>9__2;
								if ((predicate3 = <>9__2) == null)
								{
									predicate3 = (<>9__2 = ((GmActiveRewardInfo s) => s.giftId == gmGiftInfo.giftbagId));
								}
								foreach (GmActiveRewardInfo item2 in source3.Where(predicate3))
								{
									xElement5.Add(FlashUtils.CreateGMRewardInfo(item2));
									bool flag4 = !flag;
									if (flag4)
									{
										flag = true;
									}
								}
								xElement3.Add(content2);
								bool flag5 = flag2;
								if (flag5)
								{
									xElement3.Add(xElement4);
								}
								bool flag6 = flag;
								if (flag6)
								{
									xElement3.Add(xElement5);
								}
							}
						}
						xElement2.Add(content);
						xElement2.Add(xElement3);
						xElement.Add(xElement2);
					}
					value = "Success!";
					value2 = "true";
				}
				finally
				{
					ProduceBussiness produceBussiness = val;
					if (produceBussiness != null)
					{
						((IDisposable)produceBussiness).Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = GMActivityInfo.Log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					GMActivityInfo.Log.Error("GMActivityInfo create error", ex);
				}
			}
			xElement.Add(new XAttribute("value", value2));
			xElement.Add(new XAttribute("message", value));
			return csFunction.CreateCompressXml(context, xElement, "GMActivityInfo", true);
		}

		// Token: 0x04000042 RID: 66
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}

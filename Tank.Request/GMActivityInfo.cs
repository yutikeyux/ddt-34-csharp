using System;
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
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class GMActivityInfo : IHttpHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
			{
				context.Response.Write(Bulid(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		public static string Bulid(HttpContext context)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
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
					foreach (GmActivityInfo activityInfo in array)
					{
						XElement xElement2 = new XElement("ActiveInfo");
						XElement content = FlashUtils.CreateGMActivityInfo(activityInfo);
						XElement xElement3 = new XElement("ActiveGiftBag");
						foreach (GmGiftInfo gmGiftInfo in allGmGift.Where((GmGiftInfo s) => s.activityId == activityInfo.activityId))
						{
							XElement content2 = FlashUtils.CreateGMGiftInfo(gmGiftInfo);
							XElement xElement4 = new XElement("ActiveCondition");
							XElement xElement5 = new XElement("ActiveReward");
							bool flag = false;
							bool flag2 = false;
							foreach (GmActiveConditionInfo item in allGmActiveCondition.Where((GmActiveConditionInfo s) => s.giftbagId == gmGiftInfo.giftbagId))
							{
								xElement4.Add(FlashUtils.CreateGMConditionInfo(item));
								if (!flag2)
								{
									flag2 = true;
								}
							}
							foreach (GmActiveRewardInfo item2 in allGmActiveReward.Where((GmActiveRewardInfo s) => s.giftId == gmGiftInfo.giftbagId))
							{
								xElement5.Add(FlashUtils.CreateGMRewardInfo(item2));
								if (!flag)
								{
									flag = true;
								}
							}
							xElement3.Add(content2);
							if (flag2)
							{
								xElement3.Add(xElement4);
							}
							if (flag)
							{
								xElement3.Add(xElement5);
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
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				if (Log.IsErrorEnabled)
				{
					Log.Error((object)"GMActivityInfo create error", ex);
				}
			}
			xElement.Add(new XAttribute("value", value2));
			xElement.Add(new XAttribute("message", value));
			return csFunction.CreateCompressXml(context, xElement, "GMActivityInfo", isCompress: true);
		}
	}
}

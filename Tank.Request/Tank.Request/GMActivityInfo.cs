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
    /// <summary>
    /// GM Aktivite Bilgileri Web Servisi
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GMActivityInfo : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Handler'ın yeniden kullanılabilir olup olmadığını belirtir
        /// </summary>
        public bool IsReusable => false;

        /// <summary>
        /// HTTP isteğini işler
        /// </summary>
        /// <param name="context">HTTP context</param>
        public void ProcessRequest(HttpContext context)
        {
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                context.Response.Write(Build(context));
            }
            else
            {
                context.Response.Write("Erişim yetkisi reddedildi!");
            }
        }

        /// <summary>
        /// GM Aktivite bilgilerini XML formatında oluşturur
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns>XML formatında aktivite bilgileri</returns>
        public static string Build(HttpContext context)
        {
            XElement resultElement = new XElement("Result");
            string statusMessage = "Başarısız!";
            string isSuccess = "false";

            try
            {
                using (ProduceBussiness business = new ProduceBussiness())
                {
                    // Veritabanından tüm aktivite, hediye, koşul ve ödül bilgilerini al
                    GmActivityInfo[] allActivities = business.GetAllGmActivity();
                    GmGiftInfo[] allGifts = business.GetAllGmGift();
                    GmActiveConditionInfo[] allConditions = business.GetAllGmActiveCondition();
                    GmActiveRewardInfo[] allRewards = business.GetAllGmActiveReward();

                    // Her aktivite için XML elementleri oluştur
                    foreach (GmActivityInfo activity in allActivities)
                    {
                        XElement activityElement = new XElement("ActiveInfo");
                        XElement activityContent = FlashUtils.CreateGMActivityInfo(activity);
                        XElement giftBagElement = new XElement("ActiveGiftBag");

                        // Aktiviteye ait hediyeleri filtrele
                        var activityGifts = allGifts.Where(gift => gift.activityId == activity.activityId);

                        foreach (GmGiftInfo gift in activityGifts)
                        {
                            XElement giftContent = FlashUtils.CreateGMGiftInfo(gift);
                            XElement conditionElement = new XElement("ActiveCondition");
                            XElement rewardElement = new XElement("ActiveReward");

                            bool hasConditions = false;
                            bool hasRewards = false;

                            // Hediyeye ait koşulları filtrele
                            var giftConditions = allConditions.Where(condition => condition.giftbagId == gift.giftbagId);
                            foreach (GmActiveConditionInfo condition in giftConditions)
                            {
                                conditionElement.Add(FlashUtils.CreateGMConditionInfo(condition));
                                hasConditions = true;
                            }

                            // Hediyeye ait ödülleri filtrele
                            var giftRewards = allRewards.Where(reward => reward.giftId == gift.giftbagId);
                            foreach (GmActiveRewardInfo reward in giftRewards)
                            {
                                rewardElement.Add(FlashUtils.CreateGMRewardInfo(reward));
                                hasRewards = true;
                            }

                            giftBagElement.Add(giftContent);

                            if (hasConditions)
                            {
                                giftBagElement.Add(conditionElement);
                            }

                            if (hasRewards)
                            {
                                giftBagElement.Add(rewardElement);
                            }
                        }

                        activityElement.Add(activityContent);
                        activityElement.Add(giftBagElement);
                        resultElement.Add(activityElement);
                    }

                    statusMessage = "Success!";
                    isSuccess = "true";
                }
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled)
                {
                    Log.Error("GMActivityInfo create error", ex);
                }
            }

            resultElement.Add(new XAttribute("value", isSuccess));
            resultElement.Add(new XAttribute("message", statusMessage));

            return csFunction.CreateCompressXml(context, resultElement, "GMActivityInfo", true);
        }
    }
}
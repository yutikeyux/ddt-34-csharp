using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GMActivityInfo : IHttpHandler
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                context.Response.Write(Build(context));
            }
            else
            {
                context.Response.Write("IP is not valid!");
            }
        }

        public static string Build(HttpContext context)
        {
            XElement result = new XElement("Result");
            string message = "Başarısız!";
            string Başarılı = "false";

            try
            {
                using (var produceBussiness = new ProduceBussiness())
                {
                    // Tüm verileri tek seferde al
                    var allActivities = produceBussiness.GetAllGmActivity();
                    var allGifts = produceBussiness.GetAllGmGift();
                    var allConditions = produceBussiness.GetAllGmActiveCondition();
                    var allRewards = produceBussiness.GetAllGmActiveReward();

                    // Her aktivite için XML oluştur
                    foreach (var activity in allActivities)
                    {
                        var activityElement = CreateActivityElement(activity, allGifts, allConditions, allRewards);
                        result.Add(activityElement);
                    }

                    message = "Başarılı!";
                    Başarılı = "true";
                }
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled)
                {
                    Log.Error("GMActivityInfo create error", ex);
                }
            }

            result.Add(new XAttribute("value", Başarılı));
            result.Add(new XAttribute("message", message));

            return csFunction.CreateCompressXml(context, result, "GMActivityInfo", true);
        }

        private static XElement CreateActivityElement(
            GmActivityInfo activity,
            IEnumerable<GmGiftInfo> allGifts,
            IEnumerable<GmActiveConditionInfo> allConditions,
            IEnumerable<GmActiveRewardInfo> allRewards)
        {
            var activityElement = new XElement("ActiveInfo");
            activityElement.Add(FlashUtils.CreateGMActivityInfo(activity));

            var giftBagElement = new XElement("ActiveGiftBag");

            // Aktiviteye ait hediyeleri filtrele
            var activityGifts = allGifts.Where(g => g.activityId == activity.activityId).ToList();

            foreach (var gift in activityGifts)
            {
                var giftElement = CreateGiftElement(gift, allConditions, allRewards);
                giftBagElement.Add(giftElement);
            }

            activityElement.Add(giftBagElement);
            return activityElement;
        }

        private static XElement CreateGiftElement(
            GmGiftInfo gift,
            IEnumerable<GmActiveConditionInfo> allConditions,
            IEnumerable<GmActiveRewardInfo> allRewards)
        {
            var giftElement = FlashUtils.CreateGMGiftInfo(gift);

            // Hediyeye ait koşulları ekle
            var giftConditions = allConditions.Where(c => c.giftbagId == gift.giftbagId);
            if (giftConditions.Any())
            {
                var conditionsElement = new XElement("ActiveCondition");
                foreach (var condition in giftConditions)
                {
                    conditionsElement.Add(FlashUtils.CreateGMConditionInfo(condition));
                }
                giftElement.Add(conditionsElement);
            }

            // Hediyeye ait ödülleri ekle (DÜZELTİLDİ: giftbagId == giftId)
            var giftRewards = allRewards.Where(r => r.giftId == gift.giftbagId);
            if (giftRewards.Any())
            {
                var rewardsElement = new XElement("ActiveReward");
                foreach (var reward in giftRewards)
                {
                    rewardsElement.Add(FlashUtils.CreateGMRewardInfo(reward));
                }
                giftElement.Add(rewardsElement);
            }

            return giftElement;
        }

        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
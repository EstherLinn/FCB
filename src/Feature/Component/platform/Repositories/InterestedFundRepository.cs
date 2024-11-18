using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.InterestedFund;
using Feature.Wealth.Component.Models.Invest;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Feature.Wealth.Component.Repositories
{
    public class InterestedFundRepository
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string interestedFundCache = $"FCB_InterewstedFund";

        public List<Fund> GetOrSetCacheFundData()
        {
            var cacheData = _cache.Get(interestedFundCache) as List<FundModel>;
            if (cacheData == null)
            {
                string wms_focus_profile = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Wms_focus_profile);
                string sql = $@"
                    SELECT TOP(6) w.FUND_ID as FundId, vw.FundName, vw.OneMonthReturnOriginalCurrency, vw.AvailabilityStatus, vw.OnlineSubscriptionAvailability
                    FROM {wms_focus_profile} AS w
                    LEFT JOIN vw_BasicFund AS vw WITH (NOLOCK)
                    ON w.FUND_ID = vw.ProductCode
                    ORDER BY w.SNAPSHOT_DATE DESC, w.FOCUS_NUMS DESC";

                cacheData = DbManager.Custom.ExecuteIList<FundModel>(sql, null, commandType: System.Data.CommandType.Text)?.ToList();

                if (cacheData != null)
                {
                    _cache.Add(interestedFundCache, cacheData, DateTimeOffset.Now.AddMinutes(30));
                }
            }

            var funds = cacheData
                .Select(item => new Fund
                {
                    FundId = item.FundId,
                    RateOfRetuen = item.OneMonthReturnOriginalCurrency.FormatDecimalNumber(2, false, false),
                    FundName = item.FundName ?? "",
                    DetailUrl = FundRelatedSettingModel.GetFundDetailsUrl() + "?id=" + item.FundId,
                    SubscribeButtonHtml = (item.AvailabilityStatus == "Y" &&
                          (item.OnlineSubscriptionAvailability == "Y" ||
                           string.IsNullOrEmpty(item.OnlineSubscriptionAvailability)))
                           ? PublicHelpers.SubscriptionButtonForCard(null, null, item.FundId, InvestTypeEnum.Fund).ToString()
                           : string.Empty,
                    FocusButtonHtml = PublicHelpers.FocusButton(null, null, item.FundId, item.FundName, InvestTypeEnum.Fund, false).ToString(),
                    CompareButtonHtml = PublicHelpers.CompareButton(null, null, item.FundId, item.FundName, InvestTypeEnum.Fund, false).ToString()
                }).ToList();

            return funds;
        }
    }
}
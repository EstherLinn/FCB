using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Feature.Wealth.Service.Models.FundRankApi
{
    public class FundRankApiModel
    {
        public class ReturnFundSet
        {
            [JsonProperty("01")]
            public List<ReturnFund> PopularityFunds { get; set; }
            [JsonProperty("02")]
            public List<ReturnFund> AwardedFunds { get; set; }
            [JsonProperty("03")]
            public List<ReturnFund> DomesticPerformanceRankFunds { get; set; }
            [JsonProperty("04")]
            public List<ReturnFund> ForeignPerformanceRankFunds { get; set; }
        }
        public class ReturnFund
        {
            [JsonProperty("tnav")]
            public string Tnav { get; set; }
            [JsonProperty("genWebUrl")]

            //單筆申購連結
            public string GenWebUrl { get; set; }
            [JsonProperty("wmsWenUrl")]

            //理財網連結
            public string WmsWenUrl { get; set; }
            [JsonProperty("fundCode")]
            public string FundCode { get; set; }
            [JsonProperty("fundName")]
            public string FundName { get; set; }
            [JsonProperty("riskType")]
            public string RiskType { get; set; }
            [JsonProperty("date")]
            public string Date { get; set; }
            [JsonProperty("riskTypeName")]
            public string RiskTypeName { get; set; }
            [JsonProperty("fcur")]
            public string Fcur { get; set; }
        }

        public class Funds
        {
            /// <summary>
            /// 國內國外標示
            /// </summary>
            public string DomesticForeignFundIndicator { get; set; }

            /// <summary>
            /// 一銀產品代號
            /// </summary>
            public string ProductCode { get; set; }

            /// <summary>
            /// 一銀產品名稱
            /// </summary>
            public string FundName { get; set; }

            /// <summary>
            /// 淨值日期
            /// </summary>
            public DateTime NetAssetValueDate { get; set; }

            /// <summary>
            /// 淨值
            /// </summary>
            public decimal? NetAssetValue { get; set; }

            /// <summary>
            /// 計價幣別 
            /// </summary>
            public string CurrencyName { get; set; }

            /// <summary>
            /// 一個月績效表現
            /// </summary>
            public decimal? OneMonthReturnOriginalCurrency { get; set; }
            /// <summary>
            /// 三個月績效表現
            /// </summary>
            public decimal? ThreeMonthReturnOriginalCurrency { get; set; }

            /// <summary>
            /// 六個月績效表現
            /// </summary>
            public decimal? SixMonthReturnOriginalCurrency { get; set; }

            /// <summary>
            /// 一年績效表現
            /// </summary>
            public decimal? OneYearReturnOriginalCurrency { get; set; }

            /// <summary>
            /// 風險屬性
            /// </summary>
            public string RiskRewardLevel { get; set; }

            /// <summary>
            /// 是否可於網路申購
            /// </summary>
            public string OnlineSubscriptionAvailability { get; internal set; }

            /// <summary>
            /// 是否上架
            /// </summary>
            public string AvailabilityStatus { get; set; }

            public int? ViewCount { get; set; }
        }
    }
}
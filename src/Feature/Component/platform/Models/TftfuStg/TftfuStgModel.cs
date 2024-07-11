using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.TftfuStg
{
    public class TftfuStgModel
    {
        public Item Item { get; set; }
        public IList<RenderFunds> SmartFunds { get; set; }
        public string DetailLink { get; set; }


        public class Funds
        {
            public string ProductCode { get; set; }

            public string FundName { get; set; }

            public DateTime? NetAssetValueDate { get; set; }
            public string NetAssetValueDateFormat => NetAssetValueDate?.ToString("yyyy/MM/dd");

            public decimal? NetAssetValue { get; set; }

            public string CurrencyName { get; set; }

            public string CurrencyCode { get; set; }

            public decimal? SixMonthReturnOriginalCurrency { get; set; }

            public decimal? PercentageChangeInFundPrice { get; set; }

            public string RiskRewardLevel { get; set; }
            
            public string OnlineSubscriptionAvailability { get; set; }

            public string FundTypeName { get; set; }

            public string TargetName { get; set; }

            public string AvailabilityStatus { get; set; }

            public string TFTFU_SMART_TYPE { get; set; }

            public string TFTFU_CTT_SHR_TYPE { get; set; }

        }

        public class RenderFunds
        {
            public string ProductCode { get; set; }

            public string FundName { get; set; }

            public string NetAssetValueDate { get; set; }

            public decimal? NetAssetValue { get; set; }

            public KeyValuePair<string, string> Currency { get; set; }

            public KeyValuePair<bool, decimal?> SixMonthReturnOriginalCurrency { get; set; }

            public KeyValuePair<bool, decimal?> PercentageChangeInFundPrice { get; set; }

            public string RiskRewardLevel { get; set; }

            public bool IsOnlineSubscriptionAvailability { get; set; }

            public string FundTypeName { get; set; }

            public string AvailabilityStatus { get; set; }

            public List<string> Tags { get; set; }

            public string TFTFU_SMART_TYPE { get; set; }

            public string TFTFU_CTT_SHR_TYPE { get; set; }

            public string DetailUrl { get; set; }

            public string FocusTag { get; set; }

            public string SubscriptionTag { get; set; }

            public string CompareTag { get; set; }
        }
    }

    /// <summary>
    /// 上稿內容，使用基金績效排行模板
    /// </summary>
    public struct Template
    {
        public struct TftfuStg
        {
            public static readonly ID Id = new ID("{CB9A2DED-9F6B-4A56-BF94-3B99E7129B88}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{D7FA12C5-5FC8-4928-8E0D-64A982793216}");

                /// <summary>
                /// 副標題
                /// </summary>
                public static readonly ID SubTitle = new ID("{1C5CB664-CEA4-40F4-9D16-BB7EE12127C0}");
            }

        }
    }
}

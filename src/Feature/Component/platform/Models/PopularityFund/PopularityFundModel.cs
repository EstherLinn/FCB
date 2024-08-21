using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.PopularityFund
{
    public class PopularityFundModel
    {
        public Item Item { get; set; }
        public string[] SelectedId { get; set; }
        public List<Funds> PopularFunds { get; set; }
        public string DetailLink { get; set; }

        /// <summary>
        /// 基金商品資訊
        /// </summary>
        public class Funds
        {
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
            /// 計價幣別代碼-排序使用
            /// </summary>
            public string CurrencyCode { get; set; }

            /// <summary>
            /// 六個月報酬原幣
            /// </summary>
            public decimal? SixMonthReturnOriginalCurrency { get; set; }

            /// <summary>
            /// 漲跌幅
            /// </summary>
            public decimal? PercentageChangeInFundPrice { get; set; }

            /// <summary>
            /// 風險屬性
            /// </summary>
            public string RiskRewardLevel { get; set; }

            /// <summary>
            /// 是否可於網路申購
            /// </summary>
            public string OnlineSubscriptionAvailability { get; set; }

            /// <summary>
            /// 百元標的
            /// </summary>
            public string TargetName { get; set; }

            /// <summary>
            /// 點擊次數
            /// </summary>
            public int? ViewCount { get; set; }

            /// <summary>
            /// 是否上架
            /// </summary>
            public string AvailabilityStatus { get; set; }

            public List<string> Tags { get; set; }
        }
    }

    public struct Template
    {
        public struct PopularityFund
        {
            public static readonly ID Id = new ID("{7AE550B0-E6CA-4AC9-9C83-4B9ACFC95743}");

            public struct Fields
            {
                /// <summary>
                /// 人氣基金FundID
                /// </summary>
                public static readonly string FundID = "{EFB9581C-8C8B-49AA-AADB-A4B6FFF2A0AF}";

                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{92D93435-3C84-495F-B3F6-B0AA58C1F493}");
                /// <summary>
                /// 副標題
                /// </summary>
                public static readonly ID SubTitle = new ID("{5DBEEE77-2E93-4BF7-836F-96A1891ADA65}");
                /// <summary>
                /// 標題備註
                /// </summary>
                public static readonly ID Content = new ID("{7BA535FA-A3E0-428E-B91C-D5E1E8D1E43C}");
            }
        }

    }
}
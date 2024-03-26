using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.HotFund
{
    public class HotFundModel
    {
        public Item Item { get; set; }
        public string[] SelectedId { get; set; }

        public IList<Funds> HotFunds { get; set; }

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
            public string ProductName { get; set; }

            /// <summary>
            /// 淨值日期
            /// </summary>
            public DateTime NetAssetValueDate { get; set; }

            /// <summary>
            /// 淨值
            /// </summary>
            public decimal NetAssetValue { get; set; }

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
            public decimal SixMonthReturnOriginalCurrency { get; set; }

            /// <summary>
            /// 漲跌幅
            /// </summary>
            public decimal PercentageChangeInFundPrice { get; set; }

            /// <summary>
            /// 風險屬性
            /// </summary>
            public string RiskRewardLevel { get; set; }

            /// <summary>
            /// 是否可於網路申購
            /// </summary>
            public string OnlineSubscriptionAvailability { get; internal set; }

            /// <summary>
            /// 國人持有金額占基金規模比重
            /// </summary>
            public decimal DomesticInvestmentRatio { get; internal set; }

            /// <summary>
            /// 百元標的
            /// </summary>
            public string TargetName { get; internal set; }
        }
    }

    public struct Template
    {
        /// <summary>
        /// 使用人氣基金Template
        /// </summary>
        public struct HotFund
        {
            public static readonly ID Id = new ID("{7AE550B0-E6CA-4AC9-9C83-4B9ACFC95743}");

            public struct Fields
            {
                /// <summary>
                /// 熱銷基金FundID
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
                /// <summary>
                /// 附註內容
                /// </summary>
                public static readonly ID NoteContent = new ID("{26658643-63EB-49E2-8073-1C33BC115DC4}");
            }
        }

    }
}

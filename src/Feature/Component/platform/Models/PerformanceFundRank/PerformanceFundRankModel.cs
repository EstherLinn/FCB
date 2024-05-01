using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.PerformanceFundRank
{
    public class PerformanceFundRankModel
    {
        public Item Item { get; set; }
        public IList<Funds> PerformanceFunds { get; set; }
        public int DTotalPages { get; set; }
        public int FTotalPages { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string PageSize { get; set; }
        public string DetailLink { get; set; }

        /// <summary>
        /// 基金商品資訊
        /// </summary>
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
            public string ProductName { get; set; }

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
            /// 六個月報酬台幣
            /// </summary>
            public decimal? SixMonthReturnTWD { get; set; }

            /// <summary>
            /// 漲跌幅
            /// </summary>
            public decimal? PercentageChangeInFundPrice { get; set; }

            /// <summary>
            /// 風險屬性
            /// </summary>
            public string RiskRewardLevel { get; set; }

            /// <summary>
            /// 基金類型
            /// </summary>
            public string FundTypeName { get; set; }

            /// <summary>
            /// 投資標的
            /// </summary>
            public string InvestmentTargetName { get; internal set; }

            /// <summary>
            /// 是否可於網路申購
            /// </summary>
            public string OnlineSubscriptionAvailability { get; internal set; }

            /// <summary>
            /// 百元標的
            /// </summary>
            public string TargetName { get; internal set; }

        }
    }

   

/// <summary>
/// 上稿內容
/// </summary>
public struct Template
    {
        public struct PerformanceFundRank
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
using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.NewFund
{
    public class NewFundModel
    {
        public Item Item { get; set; }
        public IList<Funds> NewFunds { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string PageSize { get; set; }

        /// <summary>
        /// 基金商品資訊
        /// </summary>
        public class Funds
        {
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
            /// 六個月報酬台幣
            /// </summary>
            public decimal SixMonthReturnTWD { get; set; }

            /// <summary>
            /// 漲跌幅
            /// </summary>
            public decimal PercentageChangeInFundPrice { get; set; }

            /// <summary>
            /// 風險屬性
            /// </summary>
            public string RiskRewardLevel { get; set; }

            /// <summary>
            /// 基金類型
            /// </summary>
            public string FundTypeName { get; set; }

            /// <summary>
            /// 是否可於網路申購
            /// </summary>
            public string OnlineSubscriptionAvailability { get; set; }

            /// <summary>
            /// 百元標的
            /// </summary>
            public string TargetName { get; set; }
        }
    }


    /// <summary>
    /// 上稿內容
    /// </summary>
    public struct Template
    {
        public struct NewFund
        {
            public static readonly ID Id = new ID("{2BEE47D1-002B-4F40-8499-3EEFB3E52782}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{B3216E9E-19B1-493A-BE83-66493608E1DD}");

                /// <summary>
                /// 副標題
                /// </summary>
                public static readonly ID SubTitle = new ID("{F7CAFC5E-B1F0-4292-A423-B99AB3759648}");
            }
        }

    }
}
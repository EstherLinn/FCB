using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.HighRatedFund
{
    public class HighRatedFundModel
    {
        public Item Item { get; set; }
        public IList<Funds> HighRatedFunds { get; set; }

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
            /// 是否可於網路申購
            /// </summary>
            public string OnlineSubscriptionAvailability { get; internal set; }

            /// <summary>
            /// 百元標的
            /// </summary>
            public string TargetName { get; internal set; }
        }
    }

    public class Template
    {
        public struct HighRatedFund
        {
            public static readonly ID Id = new ID("{E4C22549-EB3A-414B-8B3D-924301977A14}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{D69D113E-304D-4610-BC70-70790E4B9CE9}");

                /// <summary>
                /// 副標題
                /// </summary>
                public static readonly ID SubTitle = new ID("{73569CDE-837E-4DCC-8B5D-C51021ECB499}");

                /// <summary>
                /// 標題備註
                /// </summary>
                public static readonly ID Content = new ID("{7A85A0BD-110A-4E93-BFED-0FBF5C88322D}");

                /// <summary>
                /// 附註內容
                /// </summary>
                public static readonly ID NoteContent = new ID("{2F8AB55E-63CA-416A-84BF-EB0F64660E02}");
            }
        }
    }
}
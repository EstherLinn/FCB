using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.CostRank
{
    public class CostRankModel
    {
        public Item Item { get; set; }
        public IList<ETFs> CostRanks { get; set; }
        public string DetailLink { get; set; }

        /// <summary>
        /// ETF商品資訊
        /// </summary>
        public class ETFs
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
            /// 交易所代碼 
            /// </summary>
            public string ExchangeCode { get; set; }

            /// <summary>
            /// 淨值日期
            /// </summary>
            public DateTime MarketPriceDate { get; set; }
            public string MarketPriceDateFormat => MarketPriceDate.ToString("yyyy/MM/dd");

            /// <summary>
            /// 價格(市價)
            /// </summary>
            public decimal MarketPrice { get; set; }

            /// <summary>
            /// 幣別-排序用
            /// </summary>
            public string CurrencyCode { get; set; }

            /// <summary>
            /// 幣別
            /// </summary>
            public string CurrencyName { get; set; }

            /// <summary>
            /// 投資標的-排序用
            /// </summary>
            public int InvestmentTargetID { get; set; }

            /// <summary>
            /// 投資標的
            /// </summary>
            public string InvestmentTargetName { get; set; }

            /// <summary>
            /// 成立年資
            /// </summary>
            public decimal EstablishmentSeniority { get; set; }

            /// <summary>
            /// 總管理費用
            /// </summary>
            public decimal TotalManagementFee { get; set; }
            public string TotalManagementFeeFormat => TotalManagementFee.ToString("0.00") + "%";

            /// <summary>
            /// 規模(百萬)
            /// </summary>
            public decimal ScaleMillions { get; set; }
            
            /// <summary>
            /// 可否申購
            /// </summary>
            public string OnlineSubscriptionAvailability { get; set; }

            /// <summary>
            /// 是否上架
            /// </summary>
            public string AvailabilityStatus { get; set; }

            public string[] ETFDiscountTags { get; set; }
        }
    }

    public class Template
    {
        public struct CostRank
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
            }
        }
    }
}
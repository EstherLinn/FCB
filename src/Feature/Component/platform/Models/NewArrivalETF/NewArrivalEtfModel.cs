using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.NewArrivalETF
{
    public class NewArrivalEtfModel
    {
        public Item Item { get; set; }
        public IEnumerable<ETFs> NewETFs { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string PageSize { get; set; }
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
            public DateTime NetAssetValueDate { get; set; }
            public string NetAssetValueDateFormat => NetAssetValueDate.ToString("yyyy/MM/dd")??null;

            /// <summary>
            /// 價格(市價)
            /// </summary>
            public decimal? MarketPrice { get; set; }
            public decimal? MarketPriceFormat { get; set; }

            /// <summary>
            /// 幣別-排序用
            /// </summary>
            public string CurrencyCode { get; set; }

            /// <summary>
            /// 幣別
            /// </summary>
            public string CurrencyName { get; set; }

            /// <summary>
            /// 折溢價(%)
            /// </summary>
            public decimal? DiscountPremium { get; set; }
            public decimal? DiscountPremiumFormat { get; set; }

            /// <summary>
            /// 六個月報酬(市價原幣)
            /// </summary>
            public decimal? SixMonthReturnMarketPriceOriginalCurrency { get; set; }
            public decimal? SixMonthReturnMarketPriceOriginalCurrencyFormat { get; set; }

            /// <summary>
            /// 投資標的-排序用
            /// </summary>
            public int InvestmentTargetID { get; set; }

            /// <summary>
            /// 投資標的
            /// </summary>
            public string InvestmentTargetName { get; set; }

            /// <summary>
            /// 風險等級
            /// </summary>
            public string RiskLevel { get; set; }

            /// <summary>
            /// 可否申購
            /// </summary>
            public string OnlineSubscriptionAvailability { get; set; }

            /// <summary>
            /// 上架日期
            /// </summary>
            public string ListingDate { get; set; }


            public DateTime ListingDateFormat { get; set; }

            /// <summary>
            /// 是否上架
            /// </summary>
            public string AvailabilityStatus { get; set; }

            public string[] ETFDiscountTags { get; set; }

        }
    }



    /// <summary>
    /// 上稿內容
    /// </summary>
    public struct Template
    {
        public struct NewArrivalEtf
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

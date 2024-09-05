using Foundation.Wealth.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.IndexRecommendation
{
    public class IndexRecommendationModel
    {
        public Item Item { get; set; }
        public IList<Funds> HotFunds { get; set; }
        public IList<ETFs> HotETFs { get; set; }
        public IList<USStock.USStock> HotUSStocks { get; set; }
        public IList<Bond.Bond> HotBonds { get; set; }
        public string FundDetailLink { get; set; }
        public string ETFDetailLink { get; set; }
        public DateTime FundNetAssetValueDate { get; set; }
        public DateTime ETFMarketPriceDate { get; set; }
        public DateTime USStockDataDate { get; set; }
        public DateTime BondDataDate { get; set; }
        public Dictionary<string, string[]> TagsForTopETFs { get; set; }
    }

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
        public string SixMonthReturnOriginalCurrencyFormat { get; set; }

        /// <summary>
        /// 漲跌幅
        /// </summary>
        public decimal? PercentageChangeInFundPrice { get; set; }
        public string PercentageChangeInFundPriceFormat { get; set; }

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

    /// <summary>
    /// ETF商品資訊
    /// </summary>
    public class ETFs
    {
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

        /// <summary>
        /// 淨值日期
        /// </summary>
        public string MarketPriceDateFormat => MarketPriceDate.ToString("yyyy/MM/dd");

        /// <summary>
        /// 市價
        /// </summary>
        public decimal? MarketPrice { get; set; }
        public decimal? MarketPriceFormat => NumberExtensions.RoundingValue(MarketPrice);


        /// <summary>
        /// 幣別
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// 市價漲跌幅
        /// </summary>
        public decimal? MarketPriceChangePercentage { get; set; }
        public string MarketPriceChangePercentageFormat => NumberExtensions.RoundingPercentage(MarketPriceChangePercentage).ToString();


        /// <summary>
        /// 六個月報酬(市價原幣)
        /// </summary>
        public decimal? SixMonthReturnMarketPriceOriginalCurrency { get; set; }
        public string SixMonthReturnMarketPriceOriginalCurrencyFormat => NumberExtensions.RoundingPercentage(SixMonthReturnMarketPriceOriginalCurrency).ToString();


        /// <summary>
        /// 可否網路申購
        /// </summary>
        public string OnlineSubscriptionAvailability { get; set; }

        public string[] ETFDiscountTags { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        public string AvailabilityStatus { get; set; }
    }


    public struct Template
    {
        public struct IndexRecommendation
        {
            public static readonly ID Id = new ID("{1EF198D0-B862-4152-BE70-2CFE4F0D99ED}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{94A6E03D-BBF1-4331-893F-6C80723C3996}");

                /// <summary>
                /// Tab1 標題
                /// </summary>
                public static readonly ID Tab1Title = new ID("{85856A21-C7A3-4585-8794-399B3FC16793}");

                /// <summary>
                /// Tab1 按鈕文字
                /// </summary>
                public static readonly ID Tab1ButtonText = new ID("{3FA410EB-8A7E-41DB-BB40-7137EAC4594D}");

                /// <summary>
                /// Tab1 按鈕連結
                /// </summary>
                public static readonly ID Tab1ButtonLink = new ID("{DC706818-ABFD-4B87-A986-A35E6332DFF7}");

                /// <summary>
                /// Tab2 標題
                /// </summary>
                public static readonly ID Tab2Title = new ID("{4DA544B9-4F7B-4892-A3A5-0EEE5B462CCB}");

                /// <summary>
                /// Tab2 按鈕文字
                /// </summary>
                public static readonly ID Tab2ButtonText = new ID("{18DC300D-94EB-4975-B8D0-CE6846E8F7FD}");

                /// <summary>
                /// Tab2 按鈕連結
                /// </summary>
                public static readonly ID Tab2ButtonLink = new ID("{D466CF38-F1F3-4B6B-9D04-D0958ADAC0C3}");

                /// <summary>
                /// Tab3 標題
                /// </summary>
                public static readonly ID Tab3Title = new ID("{AA739993-52F8-48A7-BD0C-7F546B46C109}");

                /// <summary>
                /// Tab3 按鈕文字
                /// </summary>
                public static readonly ID Tab3ButtonText = new ID("{A4EC8CB3-59F6-4454-9258-48C456598414}");

                /// <summary>
                /// Tab3 按鈕連結
                /// </summary>
                public static readonly ID Tab3ButtonLink = new ID("{C17851ED-05D6-4523-A162-35CBDD682072}");

                /// <summary>
                /// Tab4 標題
                /// </summary>
                public static readonly ID Tab4Title = new ID("{940204C7-38F2-43D6-BB27-3E4BCB474AAB}");

                /// <summary>
                /// Tab4 按鈕文字
                /// </summary>
                public static readonly ID Tab4ButtonText = new ID("{66D944F0-E89D-49DE-98B3-4C61845996F8}");

                /// <summary>
                /// Tab4 按鈕連結
                /// </summary>
                public static readonly ID Tab4ButtonLink = new ID("{5D009E4A-9BEC-47DE-BB5D-2E80FC0FDB2E}");
            }
        }

    }
}
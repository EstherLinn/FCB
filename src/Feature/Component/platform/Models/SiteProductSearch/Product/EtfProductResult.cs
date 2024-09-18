using Feature.Wealth.Component.Models.ETF.Search;

namespace Feature.Wealth.Component.Models.SiteProductSearch.Product
{
    public class EtfProductResult
    {
        /// <summary>
        /// ETF代碼 (一銀代碼)
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實產品代號
        /// </summary>
        public string SysjustCode { get; set; }

        /// <summary>
        /// ETF名稱
        /// </summary>
        public string ETFName { get; set; }

        /// <summary>
        /// 交易所代碼
        /// </summary>
        public StringPair ExchangeCode { get; set; }

        /// <summary>
        /// 價格(市價)
        /// </summary>
        public VolumePair MarketPrice { get; set; }

        /// <summary>
        /// 價格(淨值)
        /// </summary>
        public VolumePair NetAssetValue { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        /// <remarks>Key: 幣別代碼，Value: 幣別名稱</remarks>
        public StringPair CurrencyPair { get; set; }

        /// <summary>
        /// 是否可於網路申購
        /// </summary>
        public bool CanOnlineSubscription { get; set; }

        #region 報酬率 (市價原幣)

        /// <summary>
        /// 成立至今 報酬率 (市價原幣)
        /// </summary>
        public Percentage InceptionDateMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (市價原幣)
        /// </summary>
        public Percentage YeartoDateReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月 報酬率 (市價原幣)
        /// </summary>
        public Percentage MonthlyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月 報酬率 (市價原幣)
        /// </summary>
        public Percentage ThreeMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月 報酬率 (市價原幣)
        /// </summary>
        public Percentage SixMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一年 報酬率 (市價原幣)
        /// </summary>
        public Percentage OneYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 二年 報酬率 (市價原幣)
        /// </summary>
        public Percentage TwoYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三年 報酬率 (市價原幣)
        /// </summary>
        public Percentage ThreeYearReturnMarketPriceOriginalCurrency { get; set; }

        #endregion 報酬率 (市價原幣)

        #region 報酬率 (市價台幣)

        /// <summary>
        /// 成立至今 報酬率 (市價台幣)
        /// </summary>
        public Percentage InceptionDateMarketPriceTWD { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (市價台幣)
        /// </summary>
        public Percentage YeartoDateReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一個月 報酬率 (市價台幣)
        /// </summary>
        public Percentage MonthlyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三個月 報酬率 (市價台幣)
        /// </summary>
        public Percentage ThreeMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 六個月 報酬率 (市價台幣)
        /// </summary>
        public Percentage SixMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一年 報酬率 (市價台幣)
        /// </summary>
        public Percentage OneYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 二年 報酬率 (市價台幣)
        /// </summary>
        public Percentage TwoYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三年 報酬率 (市價台幣)
        /// </summary>
        public Percentage ThreeYearReturnMarketPriceTWD { get; set; }

        #endregion 報酬率 (市價台幣)

        /// <summary>
        /// 優惠標籤
        /// </summary>
        public string[] DiscountTags { get; set; }

        #region 按鈕

        /// <summary>
        /// 幣別 HTML
        /// </summary>
        public string CurrencyHtml { get; set; }

        /// <summary>
        /// 關注按鈕 HTML
        /// </summary>
        public string FocusButtonHtml { get; set; }

        /// <summary>
        /// 比較按鈕 HTML
        /// </summary>
        public string CompareButtonHtml { get; set; }

        /// <summary>
        /// 申購按鈕 HTML
        /// </summary>
        public string SubscribeButtonHtml { get; set; }

        /// <summary>
        /// Autocomplete 比較按鈕 HTML
        /// </summary>
        public string CompareButtonAutoHtml { get; set; }

        /// <summary>
        /// Autocomplete 申購按鈕 HTML
        /// </summary>
        public string SubscribeButtonAutoHtml { get; set; }

        /// <summary>
        /// Autocomplete 關注按鈕 HTML
        /// </summary>
        public string FocusButtonAutoHtml { get; set; }

        #endregion 按鈕
    }
}
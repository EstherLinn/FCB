namespace Feature.Wealth.Component.Models.ETF.Search
{
    public class EtfSearchResult
    {
        /// <summary>
        /// ETF代碼 (一銀代碼)
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實產品代號
        /// </summary>
        /// <remarks>
        /// 使用嘉實代碼(基本資料、績效、淨值檔案中都有)判斷"國內or境外"，
        /// 若代碼為.TW結尾者，為國內ETF
        /// </remarks>
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
        /// 市價日期
        /// </summary>
        public string MarketPriceDate { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        /// <remarks>Key: 幣別代碼，Value: 幣別名稱</remarks>
        public StringPair CurrencyPair { get; set; }

        /// <summary>
        /// 是否可於網路申購
        /// </summary>
        public bool CanOnlineSubscription { get; set; }

        /// <summary>
        /// 風險等級
        /// </summary>
        /// <remarks>風險屬性</remarks>
        public StringPair RiskLevel { get; set; }

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

        #region 報酬率 (淨值原幣)

        /// <summary>
        /// 成立至今 報酬率 (淨值原幣)
        /// </summary>
        public Percentage InceptionDateNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (淨值原幣)
        /// </summary>
        public Percentage YeartoDateReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月 報酬率 (淨值原幣)
        /// </summary>
        public Percentage MonthlyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月 報酬率 (淨值原幣)
        /// </summary>
        public Percentage ThreeMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月 報酬率 (淨值原幣)
        /// </summary>
        public Percentage SixMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一年 報酬率 (淨值原幣)
        /// </summary>
        public Percentage OneYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 二年 報酬率 (淨值原幣)
        /// </summary>
        public Percentage TwoYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三年 報酬率 (淨值原幣)
        /// </summary>
        public Percentage ThreeYearReturnNetValueOriginalCurrency { get; set; }

        #endregion 報酬率 (淨值原幣)

        #region 報酬率 (淨值台幣)

        /// <summary>
        /// 成立至今 報酬率 (淨值台幣)
        /// </summary>
        public Percentage InceptionDateNetValueTWD { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (淨值台幣)
        /// </summary>
        public Percentage YeartoDateReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一個月 報酬率 (淨值台幣)
        /// </summary>
        public Percentage MonthlyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三個月 報酬率 (淨值台幣)
        /// </summary>
        public Percentage ThreeMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 六個月 報酬率 (淨值台幣)
        /// </summary>
        public Percentage SixMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一年 報酬率 (淨值台幣)
        /// </summary>
        public Percentage OneYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 二年 報酬率 (淨值台幣)
        /// </summary>
        public Percentage TwoYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三年 報酬率 (淨值台幣)
        /// </summary>
        public Percentage ThreeYearReturnNetValueTWD { get; set; }

        #endregion 報酬率 (淨值台幣)

        #region 淨值風險

        /// <summary>
        /// Sharpe (淨值風險)
        /// </summary>
        public VolumePair SharpeNetValueRisk { get; set; }

        /// <summary>
        /// Beta (淨值風險)
        /// </summary>
        public VolumePair BetaNetValueRisk { get; set; }

        /// <summary>
        /// 年化標準差 (淨值風險)
        /// </summary>
        public VolumePair AnnualizedStandardDeviationNetValueRisk { get; set; }

        #endregion 淨值風險

        #region 市價風險

        /// <summary>
        /// Sharpe (市價風險)
        /// </summary>
        public VolumePair SharpeRatioMarketPriceRisk { get; set; }

        /// <summary>
        /// Beta (市價風險)
        /// </summary>
        public VolumePair BetaMarketPriceRisk { get; set; }

        /// <summary>
        /// 年化標準差 (市價風險)
        /// </summary>
        public VolumePair AnnualizedStandardDeviationMarketPriceRisk { get; set; }

        #endregion 市價風險

        #region 交易狀況

        /// <summary>
        /// 折溢價
        /// </summary>
        public Percentage DiscountPremium { get; set; }

        /// <summary>
        /// 最新量(成交量)
        /// </summary>
        /// <remarks>Key: 成交量原始值，Value: 成交量轉換單位</remarks>
        public VolumePair LatestVolumeTradingVolume { get; set; }

        /// <summary>
        /// 最新量(成交量) – 十日均量
        /// </summary>
        /// <remarks>Key: 成交量原始值，Value: 成交量轉換單位</remarks>
        public VolumePair LatestVolumeTradingVolumeTenDayAverageVolume { get; set; }

        #endregion 交易狀況

        #region 基本資料

        /// <summary>
        /// 投資標的
        /// </summary>
        /// <remarks>Key: 投資標的 ID ，Value: 投資標的 名稱</remarks>
        public IdValuePair InvestmentTarget { get; set; }

        /// <summary>
        /// 成立年資
        /// </summary>
        public IdPair EstablishmentSeniority { get; set; }

        /// <summary>
        /// 總管理費用(%)
        /// </summary>
        public VolumePair TotalManagementFee { get; set; }

        /// <summary>
        /// ETF規模(百萬)
        /// </summary>
        public VolumePair ScaleMillions { get; set; }

        #endregion 基本資料

        #region 其他搜尋篩選用

        /// <summary>
        /// 國內/境外
        /// </summary>
        public string RegionType { get; set; }

        /// <summary>
        /// 配息頻率
        /// </summary>
        /// <remarks>項目：不配息、月配、季配、半年配、年配</remarks>
        public string DividendDistributionFrequency { get; set; }

        /// <summary>
        /// 投資區域
        /// </summary>
        /// <remarks>Key: 投資區域 ID ，Value: 投資區域 名稱</remarks>
        public IdValuePair InvestmentRegion { get; set; }

        /// <summary>
        /// 發行公司
        /// </summary>
        /// <remarks>Key: 發行公司 ID ，Value: 發行公司 名稱</remarks>
        public IdValuePair PublicLimitedCompany { get; set; }

        /// <summary>
        /// 投資風格
        /// </summary>
        /// <remarks>Key: 投資風格 ID ，Value: 投資風格 名稱</remarks>
        public IdValuePair InvestmentStyle { get; set; }

        /// <summary>
        /// 交易所 ID
        /// </summary>
        public string ExchangeID { get; set; }

        #endregion 其他搜尋篩選用

        #region 貼標

        /// <summary>
        /// 優惠標籤
        /// </summary>
        public string[] DiscountTags { get; set; }

        /// <summary>
        /// 熱門關鍵字標籤
        /// </summary>
        public string[] KeywordsTags { get; set; }

        /// <summary>
        /// 分類標籤
        /// </summary>
        public string[] CategoryTags { get; set; }

        #endregion 貼標

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
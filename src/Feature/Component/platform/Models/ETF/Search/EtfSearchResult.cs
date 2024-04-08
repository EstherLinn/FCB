using System;
using System.Collections.Generic;

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
        public string ExchangeCode { get; set; }

        /// <summary>
        /// 價格(市價)
        /// </summary>
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 價格(淨值)
        /// </summary>
        public decimal NetAssetValue { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        /// <remarks>Key: 幣別代碼，Value: 幣別名稱</remarks>
        public KeyValuePair<string, string> CurrencyPair { get; set; }

        /// <summary>
        /// 計價(市價)幣別
        /// </summary>
        //public string QuoteCurrency { get; set; }

        /// <summary>
        /// 幣別代碼
        /// </summary>
        //public string CurrencyCode { get; set; }

        /// <summary>
        /// 幣別名稱
        /// </summary>
        //public string CurrencyName { get; set; }

        /// <summary>
        /// 計價幣別
        /// </summary>
        //public string Currency { get; set; }

        /// <summary>
        /// 是否可於網路申購
        /// </summary>
        public string OnlineSubscriptionAvailability { get; set; }

        /// <summary>
        /// 風險等級
        /// </summary>
        /// <remarks>風險屬性</remarks>
        public string RiskLevel { get; set; }

        #region 報酬率 (市價原幣)

        /// <summary>
        /// 成立至今 報酬率 (市價原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> InceptionDateMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (市價原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> YeartoDateReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月 報酬率 (市價原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> MonthlyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月 報酬率 (市價原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> ThreeMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月 報酬率 (市價原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> SixMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一年 報酬率 (市價原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> OneYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 二年 報酬率 (市價原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> TwoYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三年 報酬率 (市價原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> ThreeYearReturnMarketPriceOriginalCurrency { get; set; }

        #endregion 報酬率 (市價原幣)

        #region 報酬率 (市價台幣)

        /// <summary>
        /// 成立至今 報酬率 (市價台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> InceptionDateMarketPriceTWD { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (市價台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> YeartoDateReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一個月 報酬率 (市價台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> MonthlyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三個月 報酬率 (市價台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> ThreeMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 六個月 報酬率 (市價台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> SixMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一年 報酬率 (市價台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> OneYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 二年 報酬率 (市價台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> TwoYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三年 報酬率 (市價台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> ThreeYearReturnMarketPriceTWD { get; set; }

        #endregion 報酬率 (市價台幣)

        #region 報酬率 (淨值原幣)

        /// <summary>
        /// 成立至今 報酬率 (淨值原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> InceptionDateNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (淨值原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> YeartoDateReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月 報酬率 (淨值原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> MonthlyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月 報酬率 (淨值原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> ThreeMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月 報酬率 (淨值原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> SixMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一年 報酬率 (淨值原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> OneYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 二年 報酬率 (淨值原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> TwoYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三年 報酬率 (淨值原幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> ThreeYearReturnNetValueOriginalCurrency { get; set; }

        #endregion 報酬率 (淨值原幣)

        #region 報酬率 (淨值台幣)

        /// <summary>
        /// 成立至今 報酬率 (淨值台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> InceptionDateNetValueTWD { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (淨值台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> YeartoDateReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一個月 報酬率 (淨值台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> MonthlyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三個月 報酬率 (淨值台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> ThreeMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 六個月 報酬率 (淨值台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> SixMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一年 報酬率 (淨值台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> OneYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 二年 報酬率 (淨值台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> TwoYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三年 報酬率 (淨值台幣)
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 報酬率</remarks>
        public KeyValuePair<bool, decimal> ThreeYearReturnNetValueTWD { get; set; }

        #endregion 報酬率 (淨值台幣)

        #region 淨值風險

        /// <summary>
        /// Sharpe (淨值風險)
        /// </summary>
        public decimal SharpeNetValueRisk { get; set; }

        /// <summary>
        /// Beta (淨值風險)
        /// </summary>
        public decimal BetaNetValueRisk { get; set; }

        /// <summary>
        /// 年化標準差 (淨值風險)
        /// </summary>
        public decimal AnnualizedStandardDeviationNetValueRisk { get; set; }

        #endregion 淨值風險

        #region 市價風險

        /// <summary>
        /// Sharpe (市價風險)
        /// </summary>
        public decimal SharpeRatioMarketPriceRisk { get; set; }

        /// <summary>
        /// Beta (市價風險)
        /// </summary>
        public decimal BetaMarketPriceRisk { get; set; }

        /// <summary>
        /// 年化標準差 (市價風險)
        /// </summary>
        public decimal AnnualizedStandardDeviationMarketPriceRisk { get; set; }

        #endregion 市價風險

        #region 交易狀況

        /// <summary>
        /// 折溢價
        /// </summary>
        /// <remarks>Key: 是否上漲，Value: 折溢價</remarks>
        public KeyValuePair<bool, decimal> DiscountPremium { get; set; }

        /// <summary>
        /// 最新量(成交量)
        /// </summary>
        /// <remarks>Key: 成交量原始值，Value: 成交量轉換單位</remarks>
        public KeyValuePair<decimal, string> LatestVolumeTradingVolume { get; set; }

        /// <summary>
        /// 最新量(成交量) – 十日均量
        /// </summary>
        /// <remarks>Key: 成交量原始值，Value: 成交量轉換單位</remarks>
        public KeyValuePair<decimal, string> LatestVolumeTradingVolumeTenDayAverageVolume { get; set; }

        #endregion 交易狀況

        #region 基本資料

        /// <summary>
        /// 投資標的
        /// </summary>
        /// <remarks>Key: 投資標的 ID ，Value: 投資標的 名稱</remarks>
        public KeyValuePair<int, string> InvestmentTarget { get; set; }

        /// <summary>
        /// 成立年資
        /// </summary>
        public int EstablishmentSeniority { get; set; }

        /// <summary>
        /// 總管理費用(%)
        /// </summary>
        public decimal TotalManagementFee { get; set; }

        /// <summary>
        /// ETF規模(百萬)
        /// </summary>
        public decimal ScaleMillions { get; set; }

        #endregion

        #region 其他為搜尋篩選用

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
        public KeyValuePair<int, string> InvestmentRegion { get; set; }

        /// <summary>
        /// 發行公司
        /// </summary>
        /// <remarks>Key: 發行公司 ID ，Value: 發行公司 名稱</remarks>
        public KeyValuePair<int, string> PublicLimitedCompany { get; set; }

        /// <summary>
        /// 投資風格
        /// </summary>
        /// <remarks>Key: 投資風格 ID ，Value: 投資風格 名稱</remarks>
        public KeyValuePair<int, string> InvestmentStyle { get; set; }

        /// <summary>
        /// 交易所 ID
        /// </summary>
        public string ExchangeID { get; set; }

        #endregion
    }
}

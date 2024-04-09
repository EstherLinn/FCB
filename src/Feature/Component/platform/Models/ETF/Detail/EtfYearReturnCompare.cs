namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfYearReturnCompare
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// ETF 代碼
        /// </summary>
        public string ETFCode { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// ETF 名稱
        /// </summary>
        public string ETFName { get; set; }

        /// <summary>
        /// 年化標準差(市價)
        /// </summary>
        public string AnnualizedStandardDeviationMarketPrice { get; set; }

        /// <summary>
        /// Beta(市價)
        /// </summary>
        public string BetaMarketPrice { get; set; }

        /// <summary>
        /// Sharpe(市價)
        /// </summary>
        public string SharpeMarketPrice { get; set; }

        /// <summary>
        /// Informaton Ratio (市價)
        /// </summary>
        public string InformationRatioMarketPrice { get; set; }

        /// <summary>
        /// Jensen Ratio (市價)
        /// </summary>
        public string JensenIndexMarketPrice { get; set; }

        /// <summary>
        /// Treynor Index (市價)
        /// </summary>
        public string TreynorIndexMarketPrice { get; set; }

        #region 樣式

        public string AnnualizedStandardDeviationMarketPriceStyle { get; set; }
        public string BetaMarketPriceStyle { get; set; }
        public string SharpeMarketPriceStyle { get; set; }
        public string InformationRatioMarketPriceStyle { get; set; }
        public string JensenIndexMarketPriceStyle { get; set; }
        public string TreynorIndexMarketPriceStyle { get; set; }

        #endregion 樣式
    }
}
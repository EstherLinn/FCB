using System;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfRisk
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
        /// 日期
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// ETF 名稱
        /// </summary>
        public string ETFName { get; set; }

        /// <summary>
        /// 年化標準差(市價)
        /// </summary>
        public decimal? AnnualizedStandardDeviationMarketPrice { get; set; }

        /// <summary>
        /// Beta(市價)
        /// </summary>
        public decimal? BetaMarketPrice { get; set; }

        /// <summary>
        /// Sharpe(市價)
        /// </summary>
        public decimal? SharpeMarketPrice { get; set; }

        /// <summary>
        /// Informaton Ratio (市價)
        /// </summary>
        public decimal? InformationRatioMarketPrice { get; set; }

        /// <summary>
        /// Jensen Ratio (市價)
        /// </summary>
        public decimal? JensenIndexMarketPrice { get; set; }

        /// <summary>
        /// Treynor Index (市價)
        /// </summary>
        public decimal? TreynorIndexMarketPrice { get; set; }
    }
}
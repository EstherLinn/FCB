﻿using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 年報酬率比較表，檔案名稱：Sysjust-Risk-ETF.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustRiskEtf
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string Date { get; set; }

        /// <summary>
        /// ETF代碼
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string ETFCode { get; set; }

        /// <summary>
        /// ETF名稱
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string ETFName { get; set; }

        /// <summary>
        /// 年化標準差(市價)
        /// </summary>
        [Index(4)]
        public float? AnnualizedStandardDeviationMarketPrice { get; set; }

        /// <summary>
        /// Beta(市價)
        /// </summary>
        [Index(5)]
        public float? BetaMarketPrice { get; set; }

        /// <summary>
        /// Sharpe(市價)
        /// </summary>
        [Index(6)]
        public float? SharpeMarketPrice { get; set; }

        /// <summary>
        /// Information Ratio(市價)
        /// </summary>
        [Index(7)]
        public float? InformationRatioMarketPrice { get; set; }

        /// <summary>
        /// Jensen Index(市價)
        /// </summary>
        [Index(8)]
        public float? JensenIndexMarketPrice { get; set; }

        /// <summary>
        /// Treynor Index(市價)
        /// </summary>
        [Index(9)]
        public float? TreynorIndexMarketPrice { get; set; }
    }
}
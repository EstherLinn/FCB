﻿using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 境內、外基金配息，檔案名稱：Sysjust-Dividend-Fund.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustDividendFund
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        public string SysjustCode { get; set; }

        /// <summary>
        /// 配息日(除息日)
        /// </summary>
        public string ExDividendDate { get; set; }

        /// <summary>
        /// 基準日
        /// </summary>
        public string BaseDate { get; set; }

        /// <summary>
        /// 發放日
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// 配息
        /// </summary>
        public string Dividend { get; set; }

        /// <summary>
        /// 年化配息率
        /// </summary>
        public string AnnualizedDividendRate { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 稅後息值
        /// </summary>
        public string AfterTaxInterestValue { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateTime { get; set; }
    }
}
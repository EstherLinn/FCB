﻿using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// ETF – 淨值，檔案名稱：Sysjust-Nav-ETF.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustNavEtf
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 交易所代碼
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ExchangeCode { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 市價
        /// </summary>
        public decimal? MarketPrice { get; set; }

        /// <summary>
        /// 淨值
        /// </summary>
        public decimal? NetAssetValue { get; set; }
    }
}
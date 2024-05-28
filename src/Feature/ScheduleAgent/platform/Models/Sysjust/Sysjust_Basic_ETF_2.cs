﻿using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 檔案名稱：SYSJUST-BASIC-ETF-2.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustBasicEtf2
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
        /// ETF 名稱
        /// </summary>
        public string ETFName { get; set; }

        /// <summary>
        /// 計價(市價)幣別
        /// </summary>
        public string QuoteCurrency { get; set; }

        /// <summary>
        /// 淨值
        /// </summary>
        public string NetAssetValue { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 最高淨值(年)
        /// </summary>
        public string HighestNetAssetValue { get; set; }

        /// <summary>
        /// 最低淨值(年)
        /// </summary>
        public string LowestNetAssestValue { get; set; }

        /// <summary>
        /// 市價日期
        /// </summary>
        public string MarketPriceDate { get; set; }

        /// <summary>
        /// 市價
        /// </summary>
        public string MarketPrice { get; set; }

        /// <summary>
        /// 最高市價(年)
        /// </summary>
        public string HighestMarketPrice { get; set; }

        /// <summary>
        /// 最低市價(年)
        /// </summary>
        public string LowestMarketPrice { get; set; }

        /// <summary>
        /// 市價漲跌
        /// </summary>
        public string MarketPriceChange { get; set; }

        /// <summary>
        /// 市價漲跌幅
        /// </summary>
        public string MarketPriceChangePercentage { get; set; }

        /// <summary>
        /// 淨值漲跌
        /// </summary>
        public string NetAssetValueChange { get; set; }

        /// <summary>
        /// 淨值漲跌幅
        /// </summary>
        public string NetAssetValueChangePercentage { get; set; }
    }
}
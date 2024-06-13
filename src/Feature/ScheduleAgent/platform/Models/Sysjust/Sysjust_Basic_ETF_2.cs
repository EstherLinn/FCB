using CsvHelper.Configuration.Attributes;

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
        [NullValues("", "NULL", null)]
        public string SysjustCode { get; set; }

        /// <summary>
        /// ETF 名稱
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ETFName { get; set; }

        /// <summary>
        /// 計價(市價)幣別
        /// </summary>
        [NullValues("", "NULL", null)]
        public string QuoteCurrency { get; set; }

        /// <summary>
        /// 淨值
        /// </summary>
        public decimal? NetAssetValue { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 最高淨值(年)
        /// </summary>
        public decimal? HighestNetAssetValue { get; set; }

        /// <summary>
        /// 最低淨值(年)
        /// </summary>
        public decimal? LowestNetAssestValue { get; set; }

        /// <summary>
        /// 市價日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string MarketPriceDate { get; set; }

        /// <summary>
        /// 市價
        /// </summary>
        public decimal? MarketPrice { get; set; }

        /// <summary>
        /// 最高市價(年)
        /// </summary>
        public decimal? HighestMarketPrice { get; set; }

        /// <summary>
        /// 最低市價(年)
        /// </summary>
        public decimal? LowestMarketPrice { get; set; }

        /// <summary>
        /// 市價漲跌
        /// </summary>
        public decimal? MarketPriceChange { get; set; }

        /// <summary>
        /// 市價漲跌幅
        /// </summary>
        public decimal? MarketPriceChangePercentage { get; set; }

        /// <summary>
        /// 淨值漲跌
        /// </summary>
        public decimal? NetAssetValueChange { get; set; }

        /// <summary>
        /// 淨值漲跌幅
        /// </summary>
        public decimal? NetAssetValueChangePercentage { get; set; }
    }
}
using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 持股 - 投資比例彙總表，檔案名稱：Sysjust-Holding-ETF-4.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingEtf4
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 資料日期
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string DataDate { get; set; }

        /// <summary>
        /// 基金代碼
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string FundCode { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-上市股票
        /// </summary>
        [Index(3)]
        public decimal? DomesticSecuritiesListedStocks { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-基金
        /// </summary>
        [Index(4)]
        public decimal? DomesticSecuritiesFund { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-上櫃股票
        /// </summary>
        [Index(5)]
        public decimal? DomesticSecuritiesOTCStocks { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-債券(買斷)
        /// </summary>
        [Index(6)]
        public decimal? DomesticSecuritiesBondsPurchased { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-附買回債券
        /// </summary>
        [Index(7)]
        public decimal? DomesticSecuritiesBondsRepo { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-其他
        /// </summary>
        [Index(8)]
        public decimal? DomesticSecuritiesOthers { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-小計
        /// </summary>
        [Index(9)]
        public decimal? DomesticSecuritiesSubtotal { get; set; }

        /// <summary>
        /// 國內短期投資比例-短期票券
        /// </summary>
        [Index(10)]
        public decimal? DomesticShortTermCommercialPapers { get; set; }

        /// <summary>
        /// 國內短期投資比例-一般型存款
        /// </summary>
        [Index(11)]
        public decimal? DomesticShortTermOrdinaryDeposits { get; set; }

        /// <summary>
        /// 國內短期投資比例-其他
        /// </summary>
        [Index(12)]
        public decimal? DomesticShortTermOthers { get; set; }

        /// <summary>
        /// 國內短期投資比例-小計
        /// </summary>
        [Index(13)]
        public decimal? DomesticShortTermSubtotal { get; set; }

        /// <summary>
        /// 投資國內合計
        /// </summary>
        [Index(14)]
        public decimal? TotalDomestic { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-北美
        /// </summary>
        [Index(15)]
        public decimal? ForeignSecuritiesNorthAmerica { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-日本
        /// </summary>
        [Index(16)]
        public decimal? ForeignSecuritiesJapan { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-已開發歐洲
        /// </summary>
        [Index(17)]
        public decimal? ForeignSecuritiesDevelopedEurope { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-亞洲不含日本
        /// </summary>
        [Index(18)]
        public decimal? ForeignSecuritiesAsiaexcludingJapan { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-紐澳
        /// </summary>
        [Index(19)]
        public decimal? ForeignSecuritiesAustraliaNewZealand { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-其他
        /// </summary>
        [Index(20)]
        public decimal? ForeignSecuritiesOthers { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-小計
        /// </summary>
        [Index(21)]
        public decimal? ForeignSecuritiesSubtotal { get; set; }

        /// <summary>
        /// 國外其他投資-現金存款
        /// </summary>
        [Index(22)]
        public decimal? OtherForeignInvestmentsCashDeposits { get; set; }

        /// <summary>
        /// 國外其他投資-其他
        /// </summary>
        [Index(23)]
        public decimal? OtherForeignInvestmentsOthers { get; set; }

        /// <summary>
        /// 國外其他投資-小計
        /// </summary>
        [Index(24)]
        public decimal? OtherForeignInvestmentsSubtotal { get; set; }

        /// <summary>
        /// 投資國外合計
        /// </summary>
        [Index(25)]
        public decimal? TotalForeignInvestment { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-資產證券化商品
        /// </summary>
        [Index(26)]
        public decimal? DomesticSecuritiesAssetBackedSecurities { get; set; }

        /// <summary>
        /// 國內短期投資比例--短期票券(附買回)
        /// </summary>
        [Index(27)]
        public decimal? DomesticShortTermCommercialPapersRepo { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-新興歐洲
        /// </summary>
        [Index(28)]
        public decimal? ForeignSecuritiesEmergingEurope { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-拉丁美洲
        /// </summary>
        [Index(29)]
        public decimal? ForeignSecuritiesLatinAmerica { get; set; }
    }
}
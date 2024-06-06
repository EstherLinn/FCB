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
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 資料日期
        /// </summary>
        public string DataDate { get; set; }

        /// <summary>
        /// 基金代碼
        /// </summary>
        public string FundCode { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-上市股票
        /// </summary>
        public decimal? DomesticSecuritiesListedStocks { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-基金
        /// </summary>
        public decimal? DomesticSecuritiesFund { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-上櫃股票
        /// </summary>
        public decimal? DomesticSecuritiesOTCStocks { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-債券(買斷)
        /// </summary>
        public decimal? DomesticSecuritiesBondsPurchased { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-附買回債券
        /// </summary>
        public decimal? DomesticSecuritiesBondsRepo { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-其他
        /// </summary>
        public decimal? DomesticSecuritiesOthers { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-小計
        /// </summary>
        public decimal? DomesticSecuritiesSubtotal { get; set; }

        /// <summary>
        /// 國內短期投資比例-短期票券
        /// </summary>
        public decimal? DomesticShortTermCommercialPapers { get; set; }

        /// <summary>
        /// 國內短期投資比例-一般型存款
        /// </summary>
        public decimal? DomesticShortTermOrdinaryDeposits { get; set; }

        /// <summary>
        /// 國內短期投資比例-其他
        /// </summary>
        public decimal? DomesticShortTermOthers { get; set; }

        /// <summary>
        /// 國內短期投資比例-小計
        /// </summary>
        public decimal? DomesticShortTermSubtotal { get; set; }

        /// <summary>
        /// 投資國內合計
        /// </summary>
        public decimal? TotalDomestic { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-北美
        /// </summary>
        public decimal? ForeignSecuritiesNorthAmerica { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-日本
        /// </summary>
        public decimal? ForeignSecuritiesJapan { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-已開發歐洲
        /// </summary>
        public decimal? ForeignSecuritiesDevelopedEurope { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-亞洲不含日本
        /// </summary>
        public decimal? ForeignSecuritiesAsiaexcludingJapan { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-紐澳
        /// </summary>
        public decimal? ForeignSecuritiesAustraliaNewZealand { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-其他
        /// </summary>
        public decimal? ForeignSecuritiesOthers { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-小計
        /// </summary>
        public decimal? ForeignSecuritiesSubtotal { get; set; }

        /// <summary>
        /// 國外其他投資-現金存款
        /// </summary>
        public decimal? OtherForeignInvestmentsCashDeposits { get; set; }

        /// <summary>
        /// 國外其他投資-其他
        /// </summary>
        public decimal? OtherForeignInvestmentsOthers { get; set; }

        /// <summary>
        /// 國外其他投資-小計
        /// </summary>
        public decimal? OtherForeignInvestmentsSubtotal { get; set; }

        /// <summary>
        /// 投資國外合計
        /// </summary>
        public decimal? TotalForeignInvestment { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-資產證券化商品
        /// </summary>
        public decimal? DomesticSecuritiesAssetBackedSecurities { get; set; }

        /// <summary>
        /// 國內短期投資比例--短期票券(附買回)
        /// </summary>
        public decimal? DomesticShortTermCommercialPapersRepo { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-新興歐洲
        /// </summary>
        public decimal? ForeignSecuritiesEmergingEurope { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-拉丁美洲
        /// </summary>
        public decimal? ForeignSecuritiesLatinAmerica { get; set; }
    }
}
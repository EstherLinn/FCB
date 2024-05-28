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
        public string DomesticSecuritiesListedStocks { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-基金
        /// </summary>
        public string DomesticSecuritiesFund { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-上櫃股票
        /// </summary>
        public string DomesticSecuritiesOTCStocks { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-債券(買斷)
        /// </summary>
        public string DomesticSecuritiesBondsPurchased { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-附買回債券
        /// </summary>
        public string DomesticSecuritiesBondsRepo { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-其他
        /// </summary>
        public string DomesticSecuritiesOthers { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-小計
        /// </summary>
        public string DomesticSecuritiesSubtotal { get; set; }

        /// <summary>
        /// 國內短期投資比例-短期票券
        /// </summary>
        public string DomesticShortTermCommercialPapers { get; set; }

        /// <summary>
        /// 國內短期投資比例-一般型存款
        /// </summary>
        public string DomesticShortTermOrdinaryDeposits { get; set; }

        /// <summary>
        /// 國內短期投資比例-其他
        /// </summary>
        public string DomesticShortTermOthers { get; set; }

        /// <summary>
        /// 國內短期投資比例-小計
        /// </summary>
        public string DomesticShortTermSubtotal { get; set; }

        /// <summary>
        /// 投資國內合計
        /// </summary>
        public string TotalDomestic { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-北美
        /// </summary>
        public string ForeignSecuritiesNorthAmerica { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-日本
        /// </summary>
        public string ForeignSecuritiesJapan { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-已開發歐洲
        /// </summary>
        public string ForeignSecuritiesDevelopedEurope { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-亞洲不含日本
        /// </summary>
        public string ForeignSecuritiesAsiaexcludingJapan { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-紐澳
        /// </summary>
        public string ForeignSecuritiesAustraliaNewZealand { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-其他
        /// </summary>
        public string ForeignSecuritiesOthers { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-小計
        /// </summary>
        public string ForeignSecuritiesSubtotal { get; set; }

        /// <summary>
        /// 國外其他投資-現金存款
        /// </summary>
        public string OtherForeignInvestmentsCashDeposits { get; set; }

        /// <summary>
        /// 國外其他投資-其他
        /// </summary>
        public string OtherForeignInvestmentsOthers { get; set; }

        /// <summary>
        /// 國外其他投資-小計
        /// </summary>
        public string OtherForeignInvestmentsSubtotal { get; set; }

        /// <summary>
        /// 投資國外合計
        /// </summary>
        public string TotalForeignInvestment { get; set; }

        /// <summary>
        /// 投資國內有價證券比例-資產證券化商品
        /// </summary>
        public string DomesticSecuritiesAssetBackedSecurities { get; set; }

        /// <summary>
        /// 國內短期投資比例--短期票券(附買回)
        /// </summary>
        public string DomesticShortTermCommercialPapersRepo { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-新興歐洲
        /// </summary>
        public string ForeignSecuritiesEmergingEurope { get; set; }

        /// <summary>
        /// 投資國外有價證券比例-拉丁美洲
        /// </summary>
        public string ForeignSecuritiesLatinAmerica { get; set; }
    }
}
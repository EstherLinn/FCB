using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 檔案名稱：Sysjust-Basic-ETF.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustBasicEtf
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        public string ExchangeCode { get; set; }

        /// <summary>
        /// ETF 名稱
        /// </summary>
        public string ETFName { get; set; }

        /// <summary>
        /// 計價幣別
        /// </summary>
        public string QuoteCurrency { get; set; }

        /// <summary>
        /// 淨值幣別
        /// </summary>
        public string NetAssetValueCurrency { get; set; }

        /// <summary>
        /// 放空交易
        /// </summary>
        public string ShortSellingTransactions { get; set; }

        /// <summary>
        /// 選擇權交易
        /// </summary>
        public string OptionsTrading { get; set; }

        /// <summary>
        /// 槓桿多空註記
        /// </summary>
        public string LeverageLongShort { get; set; }

        /// <summary>
        /// 規模日期
        /// </summary>
        public string ScaleDate { get; set; }

        /// <summary>
        /// 規模(百萬)
        /// </summary>
        public decimal? ScaleMillions { get; set; }

        /// <summary>
        /// 註冊地
        /// </summary>
        public string RegisteredLocation { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>
        public string EstablishmentDate { get; set; }

        /// <summary>
        /// 發行公司ID
        /// </summary>
        public int? PublicLimitedCompanyID { get; set; }

        /// <summary>
        /// 發行公司名稱
        /// </summary>
        public string PublicLimitedCompanyName { get; set; }

        /// <summary>
        /// 投資風格 ID
        /// </summary>
        public int? InvestmentStyleID { get; set; }

        /// <summary>
        /// 投資風格名稱
        /// </summary>
        public string InvestmentStyleName { get; set; }

        /// <summary>
        /// 投資標的 ID 
        /// </summary>
        public int? InvestmentTargetID { get; set; }

        /// <summary>
        /// 投資標的名稱
        /// </summary>
        public string InvestmentTargetName { get; set; }

        /// <summary>
        /// 投資區域 ID
        /// </summary>
        public int? InvestmentRegionID { get; set; }

        /// <summary>
        /// 投資區域名稱
        /// </summary>
        public string InvestmentRegionName { get; set; }

        /// <summary>
        /// 總管理費用(%)
        /// </summary>
        public decimal? TotalManagementFee { get; set; }

        /// <summary>
        /// 經銷商
        /// </summary>
        public string Dealer { get; set; }

        /// <summary>
        /// 保管機構
        /// </summary>
        public string Depository { get; set; }

        /// <summary>
        /// 經理人
        /// </summary>
        public string StockManager { get; set; }

        /// <summary>
        /// 參考指數名稱
        /// </summary>
        public string StockIndexName { get; set; }

        /// <summary>
        /// 連結指數(指標指數)
        /// </summary>
        public string IndicatorIndex { get; set; }

        /// <summary>
        /// 投資策略
        /// </summary>
        public string InvestmentStrategy { get; set; }

        /// <summary>
        /// ETF 英文名稱
        /// </summary>
        public string ETFEnglishName { get; set; }

        /// <summary>
        /// 交易所 ID
        /// </summary>
        public string ExchangeID { get; set; }

        /// <summary>
        /// 參考指數 ID
        /// </summary>
        public string ReferenceIndexID { get; set; }
    }
}
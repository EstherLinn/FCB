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
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        [Index(1)]
        public string ExchangeCode { get; set; }

        /// <summary>
        /// ETF 名稱
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string ETFName { get; set; }

        /// <summary>
        /// 計價幣別
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string QuoteCurrency { get; set; }

        /// <summary>
        /// 淨值幣別
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string NetAssetValueCurrency { get; set; }

        /// <summary>
        /// 放空交易
        /// </summary>
        [Index(5)]
        [NullValues("", "NULL", null)]
        public string ShortSellingTransactions { get; set; }

        /// <summary>
        /// 選擇權交易
        /// </summary>
        [Index(6)]
        [NullValues("", "NULL", null)]
        public string OptionsTrading { get; set; }

        /// <summary>
        /// 槓桿多空註記
        /// </summary>
        [Index(7)]
        [NullValues("", "NULL", null)]
        public string LeverageLongShort { get; set; }

        /// <summary>
        /// 規模日期
        /// </summary>
        [Index(8)]
        [NullValues("", "NULL", null)]
        public string ScaleDate { get; set; }

        /// <summary>
        /// 規模(百萬)
        /// </summary>
        [Index(9)]
        public decimal? ScaleMillions { get; set; }

        /// <summary>
        /// 註冊地
        /// </summary>
        [Index(10)]
        [NullValues("", "NULL", null)]
        public string RegisteredLocation { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>
        [Index(11)]
        [NullValues("", "NULL", null)]
        public string EstablishmentDate { get; set; }

        /// <summary>
        /// 發行公司ID
        /// </summary>
        [Index(12)]
        public int? PublicLimitedCompanyID { get; set; }

        /// <summary>
        /// 發行公司名稱
        /// </summary>
        [Index(13)]
        [NullValues("", "NULL", null)]
        public string PublicLimitedCompanyName { get; set; }

        /// <summary>
        /// 投資風格 ID
        /// </summary>
        [Index(14)]
        public int? InvestmentStyleID { get; set; }

        /// <summary>
        /// 投資風格名稱
        /// </summary>
        [Index(15)]
        [NullValues("", "NULL", null)]
        public string InvestmentStyleName { get; set; }

        /// <summary>
        /// 投資標的 ID 
        /// </summary>
        [Index(16)]
        public int? InvestmentTargetID { get; set; }

        /// <summary>
        /// 投資標的名稱
        /// </summary>
        [Index(17)]
        [NullValues("", "NULL", null)]
        public string InvestmentTargetName { get; set; }

        /// <summary>
        /// 投資區域 ID
        /// </summary>
        [Index(18)]
        public int? InvestmentRegionID { get; set; }

        /// <summary>
        /// 投資區域名稱
        /// </summary>
        [Index(19)]
        [NullValues("", "NULL", null)]
        public string InvestmentRegionName { get; set; }

        /// <summary>
        /// 總管理費用(%)
        /// </summary>
        [Index(20)]
        public decimal? TotalManagementFee { get; set; }

        /// <summary>
        /// 經銷商
        /// </summary>
        [Index(21)]
        [NullValues("", "NULL", null)]
        public string Dealer { get; set; }

        /// <summary>
        /// 保管機構
        /// </summary>
        [Index(22)]
        [NullValues("", "NULL", null)]
        public string Depository { get; set; }

        /// <summary>
        /// 經理人
        /// </summary>
        [Index(23)]
        [NullValues("", "NULL", null)]
        public string StockManager { get; set; }

        /// <summary>
        /// 參考指數名稱
        /// </summary>
        [Index(24)]
        [NullValues("", "NULL", null)]
        public string StockIndexName { get; set; }

        /// <summary>
        /// 連結指數(指標指數)
        /// </summary>
        [Index(25)]
        [NullValues("", "NULL", null)]
        public string IndicatorIndex { get; set; }

        /// <summary>
        /// 投資策略
        /// </summary>
        [Index(26)]
        [NullValues("", "NULL", null)]
        public string InvestmentStrategy { get; set; }

        /// <summary>
        /// ETF 英文名稱
        /// </summary>
        [Index(27)]
        [NullValues("", "NULL", null)]
        public string ETFEnglishName { get; set; }

        /// <summary>
        /// 交易所 ID
        /// </summary>
        [Index(28)]
        [NullValues("", "NULL", null)]
        public string ExchangeID { get; set; }

        /// <summary>
        /// 參考指數 ID
        /// </summary>
        [Index(29)]
        [NullValues("", "NULL", null)]
        public string ReferenceIndexID { get; set; }
    }
}
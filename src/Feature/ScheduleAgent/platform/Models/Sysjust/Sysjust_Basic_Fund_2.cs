using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 基金 – 基本資料2，檔案名稱：Sysjust-Basic-Fund-2.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustBasicFund2
    {
        /// <summary>
        /// 資料日期
        /// </summary>
        [Index(0)]
        public string DataDate { get; set; }

        /// <summary>
        /// 一銀代碼
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實基金代碼
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string SysjustFundCode { get; set; }

        /// <summary>
        /// 基金評等
        /// </summary>
        [Index(3)]
        public decimal? FundRating { get; set; }

        /// <summary>
        /// 費用備註
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string FeeRemarks { get; set; }

        /// <summary>
        /// 一年配息次數
        /// </summary>
        [Index(5)]
        public int? DividendFrequencyOneYear { get; set; }

        /// <summary>
        /// 指標指數名稱
        /// </summary>
        [Index(6)]
        [NullValues("", "NULL", null)]
        public string IndicatorIndexName { get; set; }

        /// <summary>
        /// 指標指數代碼
        /// </summary>
        [Index(7)]
        [NullValues("", "NULL", null)]
        public string IndicatorIndexCode { get; set; }

        /// <summary>
        /// 基金核準生效日
        /// </summary>
        [Index(8)]
        [NullValues("", "NULL", null)]
        public string FundApprovalEffectiveDate { get; set; }

        /// <summary>
        /// 總代理基金生效日
        /// </summary>
        [Index(9)]
        [NullValues("", "NULL", null)]
        public string MasterAgentFundEffectiveDate { get; set; }

        /// <summary>
        /// 國人投資比重日期
        /// </summary>
        [Index(10)]
        [NullValues("", "NULL", null)]
        public string DomesticInvestmentRatioDate { get; set; }

        /// <summary>
        /// 國人投資比重
        /// </summary>
        [Index(11)]
        public decimal? DomesticInvestmentRatio { get; set; }

        /// <summary>
        /// 投資區域
        /// </summary>
        [Index(12)]
        [NullValues("", "NULL", null)]
        public string InvestmentRegionName { get; set; }

        /// <summary>
        /// 最高經理費
        /// </summary>
        [Index(13)]
        public decimal? MaxManagerFee { get; set; }

        /// <summary>
        /// 最高保管費
        /// </summary>
        [Index(14)]
        public decimal? MaxStorageFee { get; set; }

        /// <summary>
        /// 單一報價
        /// </summary>
        [Index(15)]
        [NullValues("", "NULL", null)]
        public string SingleQuote { get; set; }
    }
}
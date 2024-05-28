using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 基金 – 基本資料，檔案名稱：Sysjust-Basic-Fund.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustBasicFund
    {
        /// <summary>
        /// 資料日期
        /// </summary>
        public string DataDate { get; set; }

        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 基金基金代碼
        /// </summary>
        public string SysjustFundCode { get; set; }


        /// <summary>
        /// 基金評等
        /// </summary>
        public string FundRating { get; set; }


        /// <summary>
        /// 費用備註
        /// </summary>
        public string FeeRemarks { get; set; }

        /// <summary>
        /// 一年配息次數
        /// </summary>
        public string DividendFrequencyOneYear { get; set; }

        /// <summary>
        /// 指標指數名稱
        /// </summary>
        public string IndicatorIndexName { get; set; }

        /// <summary>
        /// 指標指數代碼
        /// </summary>
        public string IndicatorIndexCode { get; set; }

        /// <summary>
        /// 投資區域
        /// </summary>
        public string InvestmentRegionName { get; set; }

        /// <summary>
        /// 主要投資區域
        /// </summary>
        public string PrimaryInvestmentRegion { get; set; }
        public string UnKnown { get; set; }
        public string FundType { get; set; }
    }
}
using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 近一年各月(市價/指數漲跌幅)報酬，檔案名稱：Sysjust-Return-ETF-3.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnEtf3
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
        /// ETF代碼
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string ETFCode { get; set; }

        /// <summary>
        /// 淨值原幣月報酬
        /// </summary>
        [Index(3)]
        public float? NetValueMonthlyReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 參考指數月報酬
        /// </summary>
        [Index(4)]
        public float? ReferenceIndexMonthlyReturn { get; set; }
    }
}
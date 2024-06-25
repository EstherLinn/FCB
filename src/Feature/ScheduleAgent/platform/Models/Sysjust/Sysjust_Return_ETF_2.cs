using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 基準指數/市價(報酬)，檔案名稱：Sysjust-Return-ETF-2.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnEtf2
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// ETF代碼
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string ETFCode { get; set; }

        /// <summary>
        /// 資料日期
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string DataDate { get; set; }

        /// <summary>
        /// 淨值原幣年報酬
        /// </summary>
        [Index(3)]
        public float? NetValueAnnualReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 參考指數年報酬
        /// </summary>
        [Index(4)]
        public float? ReferenceIndexAnnualReturn { get; set; }
    }
}
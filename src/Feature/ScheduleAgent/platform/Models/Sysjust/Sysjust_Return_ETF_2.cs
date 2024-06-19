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
        public string FirstBankCode { get; set; }

        /// <summary>
        /// ETF代碼
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ETFCode { get; set; }

        /// <summary>
        /// 資料日期
        [NullValues("", "NULL", null)]
        /// </summary>
        public string DataDate { get; set; }

        /// <summary>
        /// 淨值原幣年報酬
        /// </summary>
        public float? NetValueAnnualReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 參考指數年報酬
        /// </summary>
        public float? ReferenceIndexAnnualReturn { get; set; }
    }
}
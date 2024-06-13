using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 近一年各月報酬率，檔案名稱：Sysjust-Return-Fund-3.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnFund3
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實基金代碼
        /// </summary>
        [NullValues("", "NULL", null)]
        public string SysjustFundCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Date { get; set; }

        /// <summary>
        /// 整月報酬率
        /// </summary>
        public float? MonthlyReturnRate { get; set; }

        /// <summary>
        /// 指標指數漲跌幅
        /// </summary>
        public float? IndicatorIndexPriceChange { get; set; }
    }
}
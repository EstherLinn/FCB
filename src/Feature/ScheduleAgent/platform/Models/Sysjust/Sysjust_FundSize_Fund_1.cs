using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 規模變動(境外)，檔案名稱：SYSJUST-FUND-SIZE-FUND-1.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustFundSizeFund1
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
        [NullValues("", "NULL", null)]
        public string SysjustCode { get; set; }

        /// <summary>
        /// 規模日期
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string ScaleDate { get; set; }

        /// <summary>
        /// 規模
        /// </summary>
        [Index(3)]
        public decimal? Scale { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string Currency { get; set; }
    }
}
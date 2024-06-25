using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 國內外基金-持股資料-依照區域，檔案名稱：Sysjust-Holding-Fund-2.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingFund2
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
        /// 日期
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string Date { get; set; }

        /// <summary>
        /// 基金名稱
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string FundName { get; set; }

        /// <summary>
        /// 區域
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string Area { get; set; }

        /// <summary>
        /// 比例 Holding%(Area)
        /// </summary>
        [Index(5)]
        public decimal? HoldingArea { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        [Index(6)]
        [NullValues("", "NULL", null)]
        public string Currency { get; set; }

        /// <summary>
        /// 投資金額
        /// </summary>
        [Index(7)]
        public decimal? InvestmentAmount { get; set; }
    }
}
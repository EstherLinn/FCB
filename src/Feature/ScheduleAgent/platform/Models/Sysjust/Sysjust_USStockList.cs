using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 列表報酬資訊，檔案名稱：Sysjust-UsStockList.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustUsStockList
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼(交易所代碼)
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string FundCode { get; set; }

        /// <summary>
        /// 英文名稱
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string EnglishName { get; set; }

        /// <summary>
        /// 中文名稱
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string ChineseName { get; set; }

        /// <summary>
        /// 資料日期
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string DataDate { get; set; }

        /// <summary>
        /// 收盤價
        /// </summary>
        [Index(5)]
        public decimal? ClosingPrice { get; set; }

        /// <summary>
        /// 一日報酬(漲跌幅)
        /// </summary>
        [Index(6)]
        public decimal? DailyReturn { get; set; }

        /// <summary>
        /// 一週報酬
        /// </summary>
        [Index(7)]
        public decimal? WeeklyReturn { get; set; }

        /// <summary>
        /// 一個月報酬
        /// </summary>
        [Index(8)]
        public decimal? MonthlyReturn { get; set; }

        /// <summary>
        /// 三個月報酬
        /// </summary>
        [Index(9)]
        public decimal? ThreeMonthReturn { get; set; }

        /// <summary>
        /// 一年報酬
        /// </summary>
        [Index(10)]
        public decimal? OneYearReturn { get; set; }

        /// <summary>
        /// 今年以來報酬
        /// </summary>
        [Index(11)]
        public decimal? YeartoDateReturn { get; set; }

        /// <summary>
        /// 漲跌
        /// </summary>
        [Index(12)]
        public decimal? ChangePercentage { get; set; }

        /// <summary>
        /// 六個月報酬
        /// </summary>
        [Index(13)]
        public decimal? SixMonthReturn { get; set; }
    }
}
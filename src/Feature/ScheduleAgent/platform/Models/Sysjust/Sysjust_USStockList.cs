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
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼(交易所代碼)
        /// </summary>
        public string FundCode { get; set; }

        /// <summary>
        /// 英文名稱
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 中文名稱
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 資料日期
        /// </summary>
        public string DataDate { get; set; }

        /// <summary>
        /// 收盤價
        /// </summary>
        public string ClosingPrice { get; set; }

        /// <summary>
        /// 一日報酬(漲跌幅)
        /// </summary>
        public string DailyReturn { get; set; }

        /// <summary>
        /// 一週報酬
        /// </summary>
        public string WeeklyReturn { get; set; }

        /// <summary>
        /// 一個月報酬
        /// </summary>
        public string MonthlyReturn { get; set; }

        /// <summary>
        /// 三個月報酬
        /// </summary>
        public string ThreeMonthReturn { get; set; }

        /// <summary>
        /// 一年報酬
        /// </summary>
        public string OneYearReturn { get; set; }

        /// <summary>
        /// 今年以來報酬
        /// </summary>
        public string YeartoDateReturn { get; set; }

        /// <summary>
        /// 漲跌
        /// </summary>
        public string ChangePercentage { get; set; }

        /// <summary>
        /// 六個月報酬
        /// </summary>
        public string SixMonthReturn { get; set; }
    }
}
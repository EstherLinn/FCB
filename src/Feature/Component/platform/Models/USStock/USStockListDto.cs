using System;

namespace Feature.Wealth.Component.Models.USStock
{
    public class USStockListDto
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
        public DateTime? DataDate { get; set; }

        /// <summary>
        /// 收盤價
        /// </summary>
        public decimal? ClosingPrice { get; set; }

        /// <summary>
        /// 一日報酬
        /// </summary>
        public decimal? DailyReturn { get; set; }

        /// <summary>
        /// 一週報酬
        /// </summary>
        public decimal? WeeklyReturn { get; set; }

        /// <summary>
        /// 一個月報酬
        /// </summary>
        public decimal? MonthlyReturn { get; set; }

        /// <summary>
        /// 三個月報酬
        /// </summary>
        public decimal? ThreeMonthReturn { get; set; }

        /// <summary>
        /// 一年報酬
        /// </summary>
        public decimal? OneYearReturn { get; set; }

        /// <summary>
        /// 今年以來報酬
        /// </summary>
        public decimal? YeartoDateReturn { get; set; }

        /// <summary>
        /// 六個月報酬
        /// </summary>
        public decimal? SixMonthReturn { get; set; }

        /// <summary>
        /// 漲跌幅
        /// </summary>
        public decimal? ChangePercentage { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        public string AvailabilityStatus { get; set; }

        /// <summary>
        /// 是否可於網路申購
        /// </summary>
        public string OnlineSubscriptionAvailability { get; set; }
    }
}
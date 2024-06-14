using System;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.SiteProductSearch.Product
{
    public class ForeignStockResult
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼(交易所代碼)
        /// </summary>
        public KeyValuePair<string, string> FundCodePair { get; set; }

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
        public KeyValuePair<decimal?, string> ClosingPrice { get; set; }

        /// <summary>
        /// 一日報酬
        /// </summary>
        public KeyValuePair<bool, decimal?> DailyReturn { get; set; }

        /// <summary>
        /// 一周報酬
        /// </summary>
        public KeyValuePair<bool, decimal?> WeeklyReturn { get; set; }

        /// <summary>
        /// 一個月報酬
        /// </summary>
        public KeyValuePair<bool, decimal?> MonthlyReturn { get; set; }

        /// <summary>
        /// 三個月報酬
        /// </summary>
        public KeyValuePair<bool, decimal?> ThreeMonthReturn { get; set; }

        /// <summary>
        /// 一年報酬
        /// </summary>
        public KeyValuePair<bool, decimal?> OneYearReturn { get; set; }

        /// <summary>
        /// 今年以來報酬
        /// </summary>
        public KeyValuePair<bool, decimal?> YeartoDateReturn { get; set; }

        /// <summary>
        /// 六個月報酬
        /// </summary>
        public KeyValuePair<bool, decimal?> SixMonthReturn { get; set; }

        /// <summary>
        /// 漲跌幅
        /// </summary>
        public KeyValuePair<bool, decimal?> ChangePercentage { get; set; }

        /// <summary>
        /// 是否可於網路申購
        /// </summary>
        public bool CanOnlineSubscription { get; set; }

        /// <summary>
        /// 優惠標籤
        /// </summary>
        public string[] DiscountTags { get; set; }

        #region 按鈕

        /// <summary>
        /// 關注按鈕 HTML
        /// </summary>
        public string FocusButtonHtml { get; set; }

        /// <summary>
        /// 申購按鈕 HTML
        /// </summary>
        public string SubscribeButtonHtml { get; set; }

        /// <summary>
        /// Autocomplete 申購按鈕 HTML
        /// </summary>
        public string SubscribeButtonAutoHtml { get; set; }

        /// <summary>
        /// Autocomplete 關注按鈕 HTML
        /// </summary>
        public string FocusButtonAutoHtml { get; set; }

        #endregion 按鈕
    }
}
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.SiteProductSearch.Product
{
    public class FundProductResult
    {
        /// <summary>
        /// 一銀產品代號
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 一銀產品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 淨值
        /// </summary>
        public decimal? NetAssetValue { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public KeyValuePair<string, string> CurrencyPair { get; set; }

        /// <summary>
        /// 一個月 報酬率 (原幣)
        /// </summary>
        public KeyValuePair<bool, decimal?> OneMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月 報酬率 (原幣)
        /// </summary>
        public KeyValuePair<bool, decimal?> ThreeMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月 報酬率 (原幣)
        /// </summary>
        public KeyValuePair<bool, decimal?> SixMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一年 報酬率 (原幣)
        /// </summary>
        public KeyValuePair<bool, decimal?> OneYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月 報酬率 (台幣)
        /// </summary>
        public KeyValuePair<bool, decimal?> OneMonthReturnTWD { get; set; }

        /// <summary>
        /// 三個月 報酬率 (台幣)
        /// </summary>
        public KeyValuePair<bool, decimal?> ThreeMonthReturnTWD { get; set; }

        /// <summary>
        /// 六個月 報酬率 (台幣)
        /// </summary>
        public KeyValuePair<bool, decimal?> SixMonthReturnTWD { get; set; }

        /// <summary>
        /// 一年 報酬率 (台幣)
        /// </summary>
        public KeyValuePair<bool, decimal?> OneYearReturnTWD { get; set; }

        /// <summary>
        /// 是否可於網路申購
        /// </summary>
        public bool CanOnlineSubscription { get; set; }

        /// <summary>
        /// 標籤
        /// </summary>
        public List<string> Tags { get; set; }

        #region 按鈕

        /// <summary>
        /// 幣別 HTML
        /// </summary>
        public string CurrencyHtml { get; set; }

        /// <summary>
        /// 關注按鈕 HTML
        /// </summary>
        public string FocusButtonHtml { get; set; }

        /// <summary>
        /// 比較按鈕 HTML
        /// </summary>
        public string CompareButtonHtml { get; set; }

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
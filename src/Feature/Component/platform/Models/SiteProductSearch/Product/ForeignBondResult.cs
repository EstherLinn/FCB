using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.SiteProductSearch.Product
{
    public class ForeignBondResult
    {
        /// <summary>
        /// 第一銀行債券代碼
        /// </summary>
        public string BondCode { get; set; }

        /// <summary>
        /// 第一銀行債券名稱
        /// </summary>
        public string BondName { get; set; }

        /// <summary>
        /// 全名 列表顯示用
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 幣別代碼
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 幣別名稱
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// 票面利率
        /// </summary>
        public decimal? InterestRate { get; set; }

        /// <summary>
        /// 到期日 YYYYMMDD
        /// </summary>
        public string MaturityDate { get; set; }

        /// <summary>
        /// 殖利率YTM-不含前手息
        /// </summary>
        public decimal? YieldRateYTM { get; set; }

        /// <summary>
        /// 申購價
        /// </summary>
        public decimal? SubscriptionFee { get; set; }

        /// <summary>
        /// 日期 YYYYMMDD
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 漲跌月 (當天參考淨值價-對應上個月同一日參考申購價)/ 對應上個月同一日參考申購價x100%
        /// </summary>
        public decimal? UpsAndDownsMonth { get; set; }

        /// <summary>
        /// 公開說明書
        /// </summary>
        public string DocString { get; set; }

        /// <summary>
        /// 開放個網
        /// </summary>
        public string OpenToPublic { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        /// <example>true</example>
        public string Listed { get; set; }

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

        /// <summary>
        /// 幣別 HTML
        /// </summary>
        public string CurrencyHtml { get; set; }

        #endregion 按鈕
    }
}
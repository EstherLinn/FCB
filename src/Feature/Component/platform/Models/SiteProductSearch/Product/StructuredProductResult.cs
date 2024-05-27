using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.SiteProductSearch.Product
{
    public class StructuredProductResult
    {
        /// <summary>
        /// 產品識別碼
        /// </summary>
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// 產品識別碼名稱
        /// </summary>
        public string ProductIdentifierName { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 一銀產品代號
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 發行機構
        /// </summary>
        public KeyValuePair<string, string> IssuingInstitutionPair { get; set; }

        /// <summary>
        /// 產品到期日
        /// </summary>
        public KeyValuePair<string, string> ProductMaturityDatePair { get; set; }

        /// <summary>
        /// 幣別代碼
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public KeyValuePair<string, string> CurrencyPair { get; set; }

        /// <summary>
        /// 一銀賣價
        /// </summary>
        public KeyValuePair<string, string> BankSellPricePair { get; set; }

        /// <summary>
        /// 價格基準日
        /// </summary>
        public KeyValuePair<string, string> PriceBaseDatePair { get; set; }

        /// <summary>
        /// 優惠標籤
        /// </summary>
        public string[] DiscountTags { get; set; }

        /// <summary>
        /// 幣別 HTML
        /// </summary>
        public string CurrencyHtml { get; set; }
    }
}
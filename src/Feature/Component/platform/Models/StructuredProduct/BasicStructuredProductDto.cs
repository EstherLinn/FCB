namespace Feature.Wealth.Component.Models.StructuredProduct
{
    public class BasicStructuredProductDto
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
        public string IssuingInstitution { get; set; }

        /// <summary>
        /// 產品到期日
        /// </summary>
        public string ProductMaturityDate { get; set; }

        /// <summary>
        /// 幣別代碼
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 幣別名稱
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 一銀賣價
        /// </summary>
        public string BankSellPrice { get; set; }

        /// <summary>
        /// 價格基準日
        /// </summary>
        public string PriceBaseDate { get; set; }
    }
}
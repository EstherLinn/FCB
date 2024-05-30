using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.StructuredProduct
{
    public class StructuredProductModel
    {
        #region from DB

        public string ProductIdentifier { get; set; }
        public string ProductIdentifierName { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string IssuingInstitution { get; set; }
        public string ProductMaturityDate { get; set; }
        public string Currency { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public string BankSellPrice { get; set; }
        public string PriceBaseDate { get; set; }

        #endregion

        #region from 後台

        public IList<string> KeywordTags { get; set; }
        public IList<string> TopicTags { get; set; }
        public IList<string> DiscountTags { get; set; }

        #endregion
    }

    public class HistoryBankSellPrice
    {
        public string UpdateDate { get; set; }
        public IList<BankSellPrice> BankSellPrice { get; set; }
    }

    public class BankSellPrice
    {
        public string PriceBaseDate { get; set; }
        public string BankSellPriceValue { get; set; }
    }

    public class BankSellPriceWithChange
    {
        public string PriceBaseDate { get; set; }
        public string BankSellPriceValue { get; set; }
        public string BankSellPriceChange { get; set; } //漲跌 當天-前一天
    }

    public class Dividend
    {
        public string DepositDay { get; set; }
        public string DividendValue { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyOrder { get; set; }
    }

    public class TagWithProducts
    {
        public string TagName { get; set; }
        public IList<string> ProductCodeList { get; set; }
    }
}
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.StructuredProduct
{
    public class StructuredProductDetailViewModel
    {
        /// <summary>
        /// 結構型商品
        /// </summary>
        public StructuredProductModel StructuredProduct { get; set; }

        /// <summary>
        /// 歷史參考贖回價
        /// </summary>
        public HistoryBankSellPrice HistoryBankSellPrice { get; set; }

        /// <summary>
        /// 最近三十筆參考贖回價含漲跌
        /// </summary>
        public IList<BankSellPriceWithChange> ThirtyDayBankSellPriceWithChange { get; set; }

        /// <summary>
        /// 歷史配息
        /// </summary>
        public IList<Dividend> HistoryDividend { get; set; }

        /// <summary>
        /// 結構型商品詳細頁頁面 Id
        /// </summary>
        public string StructuredProductDetailPageId { get; set; }

        /// <summary>
        /// 結構型商品搜尋頁連結
        /// </summary>
        public string StructuredProductSearchUrl { get; set; }
    }
}
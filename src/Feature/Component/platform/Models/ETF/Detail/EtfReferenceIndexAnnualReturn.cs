using System;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfReferenceIndexAnnualReturn
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 資料日期
        /// </summary>
        public DateTime? DataDate { get; set; }

        /// <summary>
        /// 淨值年績效
        /// </summary>
        public string NetValueAnnualReturnOriginalCurrency { get; set; }

        public string NetValueAnnualReturnOriginalCurrencyStyle { get; set; }

        /// <summary>
        /// 參考指數漲跌幅
        /// </summary>
        public string ReferenceIndexAnnualReturn { get; set; }

        public string ReferenceIndexAnnualReturnStyle { get; set; }

        /// <summary>
        /// 差額
        /// </summary>
        public string Difference { get; set; }
        public string DifferenceStyle { get; set; }
    }

    /// <summary>
    /// 基準指數 /市價 (近五年)報酬
    /// </summary>
    public class EtfReturn2
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// ETF代碼
        /// </summary>
        public string ETFCode { get; set; }

        /// <summary>
        /// 資料日期
        /// </summary>
        public DateTime? DataDate { get; set; }

        /// <summary>
        /// 淨值原幣年報酬
        /// </summary>
        public decimal? NetValueAnnualReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 參考指數年報酬
        /// </summary>
        public decimal? ReferenceIndexAnnualReturn { get; set; }
    }
}
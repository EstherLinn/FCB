using System;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfReferenceIndexMonthlyReturn
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
        /// 月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 淨值月績效
        /// </summary>
        public string NetValueMonthlyReturnOriginalCurrency { get; set; }
        public string NetValueMonthlyReturnOriginalCurrencyStyle { get; set; }

        /// <summary>
        /// 參考指數漲跌幅
        /// </summary>
        public string ReferenceIndexMonthlyReturn { get; set; }
        public string ReferenceIndexMonthlyReturnStyle { get; set; }

        /// <summary>
        /// 差額
        /// </summary>
        public string Difference { get; set; }

        public string DifferenceStyle { get; set; }
    }

    /// <summary>
    /// 近一年各月(市價/指數漲跌幅)報酬
    /// </summary>
    public class EtfReturn3
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
        /// 淨值原幣月報酬
        /// </summary>
        public decimal? NetValueMonthlyReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 參考指數月報酬
        /// </summary>
        public decimal? ReferenceIndexMonthlyReturn { get; set; }
    }
}

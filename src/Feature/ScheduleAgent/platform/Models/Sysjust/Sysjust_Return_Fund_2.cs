using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 近五年報酬率，檔案名稱：Sysjust-Return-Fund-2.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnFund2
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實基金代碼
        /// </summary>
        public string SysjustFundCode { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 整年報酬率(原幣)
        /// </summary>
        public string AnnualReturnRateOriginalCurrency { get; set; }

        /// <summary>
        /// 整年報酬率(台幣)
        /// </summary>
        public string AnnualReturnRateTWD { get; set; }

        /// <summary>
        /// 指標指數漲跌幅
        /// </summary>
        public string IndicatorIndexPriceChange { get; set; }
    }
}
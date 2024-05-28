using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 配息資訊，檔案名稱：Sysjust-Dividend-ETF.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustDividendEtf
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// ETF 代碼
        /// </summary>
        public string ETFCode { get; set; }

        /// <summary>
        /// 除息日
        /// </summary>
        public string ExDividendDate { get; set; }

        /// <summary>
        /// 基準日
        /// </summary>
        public string RecordDate { get; set; }

        /// <summary>
        /// 發放日
        /// </summary>
        public string PaymentDate { get; set; }

        /// <summary>
        /// 配息總額
        /// </summary>
        public string TotalDividendAmount { get; set; }

        /// <summary>
        /// 配息頻率
        /// </summary>
        public string DividendFrequency { get; set; }

        /// <summary>
        /// 短期資本利得
        /// </summary>
        public string ShortTermCapitalGains { get; set; }

        /// <summary>
        /// 長期資本利得
        /// </summary>
        public string LongTermCapitalGains { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public string Currency { get; set; }
    }
}
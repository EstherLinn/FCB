using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 規模變動(境外)，檔案名稱：SYSJUST-FUND-SIZE-ETF.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustFundSizeEtf
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 基金代碼
        /// </summary>
        public string FundCode { get; set; }

        /// <summary>
        /// 規模日期
        /// </summary>
        public string ScaleDate { get; set; }

        /// <summary>
        /// 規模(百萬)
        /// </summary>
        public string ScaleMillions { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public string Currency { get; set; }
    }
}
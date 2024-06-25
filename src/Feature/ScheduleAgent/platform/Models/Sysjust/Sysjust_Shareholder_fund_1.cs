using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 股東背景 1 (境內基金)
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustShareholderFund1
    {
        /// <summary>
        /// 基金公司代碼
        /// </summary>
        [Index(0)]
        public string FundCompanyCode { get; set; }

        /// <summary>
        /// 股東名稱
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string ShareholderName { get; set; }

        /// <summary>
        /// 持股數
        /// </summary>
        [Index(2)]
        public decimal? ShareQuantity { get; set; }

        /// <summary>
        /// 持股比例
        /// </summary>
        [Index(3)]
        public decimal? SharePercentage { get; set; }
    }
}
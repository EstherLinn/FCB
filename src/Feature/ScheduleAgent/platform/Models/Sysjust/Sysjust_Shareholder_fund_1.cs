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
        public string FundCompanyCode { get; set; }

        /// <summary>
        /// 股東名稱
        /// </summary>
        public string ShareholderName { get; set; }

        /// <summary>
        /// 持股數
        /// </summary>
        public string ShareQuantity { get; set; }

        /// <summary>
        /// 持股比例
        /// </summary>
        public string SharePercentage { get; set; }
    }
}
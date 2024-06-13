using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 股東背景 2 (境內基金)
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustShareholderFund2
    {
        /// <summary>
        /// 基金公司代碼
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundCompanyCode { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Name { get; set; }

        /// <summary>
        /// 擔任本公司職稱
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Title { get; set; }
    }
}
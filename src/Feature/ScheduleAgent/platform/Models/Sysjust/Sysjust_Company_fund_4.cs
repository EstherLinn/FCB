using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 境內基金公司-經營團隊，檔案名稱：Sysjust-CompanyGroup-Fund.txt 有更動檔名
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustCompanyFund4
    {
        /// <summary>
        /// 基金公司代碼
        /// </summary>
        public string FundCompanyCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 職稱
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 就任日
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 持股數
        /// </summary>
        public decimal? Shareholding { get; set; }

        /// <summary>
        /// 持股比例
        /// </summary>
        public decimal? ShareholdingPercentage { get; set; }

        /// <summary>
        /// 主要學經歷
        /// </summary>
        public string MainExperience { get; set; }
    }
}
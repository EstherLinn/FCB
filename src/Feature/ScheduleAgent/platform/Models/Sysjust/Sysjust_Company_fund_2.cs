using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 境外基金公司列表(國內總代理)，檔案名稱：Sysjust-Company-Fund-2.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustCompanyFund2
    {
        /// <summary>
        /// 基金公司代碼
        /// </summary>
        [Index(0)]
        [NullValues("", "NULL", null)]
        public string FundCompanyCode { get; set; }

        /// <summary>
        /// 基金公司名稱
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string FundCompanyName { get; set; }

        /// <summary>
        /// 英文名稱
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string EnglishName { get; set; }
    }
}
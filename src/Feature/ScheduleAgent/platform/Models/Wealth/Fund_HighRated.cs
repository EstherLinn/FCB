using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 好評基金 (wms_fund_top15_m_mf)，檔案名稱：wms_fund_top15_m_mf.txt
    /// </summary>
    [Delimiter(",")]
    [HasHeaderRecord(true)]
    public class FundHighRated
    {
        public string ProductCode { get; set; }
        public string DataDate { get; set; }
    }
}
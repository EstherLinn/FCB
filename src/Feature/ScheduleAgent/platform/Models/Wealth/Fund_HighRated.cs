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
        [Index(0)]
        public string ProductCode { get; set; }

        [Index(1)]
        public string DataDate { get; set; }
    }
}
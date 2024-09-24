using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// IMVP_HOLIDAY的排程，檔案名稱：IMVP_HOLIDAY.csv
    /// </summary>
    [Delimiter(",")]
    [HasHeaderRecord(false)]
    public class IMVPHOLIDAY
    {
        /// <summary>
        /// 日期
        /// </summary>
        [Index(0)]
        public string CALENDAR_DATE { get; set; }
    }
}

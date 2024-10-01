using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 此元件會在客戶在基金搜尋查無結果時，顯示6筆最多人關注的基金。檔案名稱：wms_focus_profile.txt
    /// </summary>
    [Delimiter("\t")]
    [HasHeaderRecord(true)]
    public class Wmsfocusprofile
    {
        /// <summary>
        /// 基金代號
        /// </summary>
        [Index(0)]
        public string FUND_ID { get; set; }

        /// <summary>
        /// 關注人數
        /// </summary>
        [Index(1)]
        public string FOCUS_NUMS { get; set; }

        /// <summary>
        /// 資料日期
        /// </summary>
        [Index(2)]
        public string SNAPSHOT_DATE { get; set; }
    }
}

using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 專屬推薦-同血型(wms_age_profile_d_mf)，檔案名稱：wms_age_profile_d_mf.txt
    /// </summary>
    [Delimiter("\t")]
    [HasHeaderRecord(true)]
    public class Wmsageprofiledmf
    {
        /// <summary>
        /// 年齡
        /// </summary>
        [Index(0)]
        public string AGE { get; set; }

        /// <summary>
        /// 基金清單
        /// </summary>
        [Index(1)]
        public string FUND_ID { get; set; }

        /// <summary>
        /// 資料日期
        /// </summary>
        [Index(2)]
        public string SNAPSHOT_DATE { get; set; }
    }
}

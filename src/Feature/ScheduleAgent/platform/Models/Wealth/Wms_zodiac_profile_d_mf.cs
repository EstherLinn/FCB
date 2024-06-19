using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 專屬推薦-同星座(wms_zodiac_profile_d_mf)，檔案名稱：wms_zodiac_profile_d_mf.txt
    /// </summary>
    [Delimiter("\t")]
    [HasHeaderRecord(true)]
    public class Wmszodiacprofiledmf
    {
        /// <summary>
        /// 星座
        /// </summary>
        [Index(0)]
        public string ZODIAC { get; set; }

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

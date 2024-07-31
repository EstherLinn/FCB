
using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 十大主題貼標，檔案名稱：Fundlist.csv
    /// </summary>
    [HasHeaderRecord(true)]
    public class FundList
    {
        /// <summary>
        /// 活動起始日
        /// </summary>
        [Index(0)]
        [NullValues("", "NULL", null)]
        public string EventStartDate { get; set; }

        /// <summary>
        /// 基金代號
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string FundId { get; set; }

        /// <summary>
        /// 主題編號
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string TopicNumber { get; set; }

        /// <summary>
        /// 主題中文名稱
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string TopicName { get; set; }

        /// <summary>
        /// CSV更新日期
        /// </summary>
        [Index(6)]
        [NullValues("", "NULL", null)]
        public string CsvUpdateDate { get; set; }
    }
}

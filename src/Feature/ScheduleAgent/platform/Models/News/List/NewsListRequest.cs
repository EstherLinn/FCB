using Newtonsoft.Json;

namespace Feature.Wealth.ScheduleAgent.Models.News.List
{
    public class NewsListRequest
    {
        /// <summary>
        /// 新聞類別
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 資料筆數
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        /// <remarks>日期與時間的分隔，請使用 T 做為分隔符號 ISO 8601 格式</remarks>
        public string StartDateTime { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        /// <remarks>日期與時間的分隔，請使用 T 做為分隔符號 ISO 8601 格式</remarks>
        public string EndDateTime { get; set; }
    }

    public class NewsListResponse
    {
        [JsonProperty("resultSet")]
        public NewsListResultset ResultSet { get; set; }
    }

    public class NewsListResultset
    {
        [JsonProperty("result")]
        public NewsListResult[] Result { get; set; }
    }

    public class NewsListResult
    {
        /// <summary>
        /// 新聞日期
        /// </summary>
        [JsonProperty("v1")]
        public string NewsDate { get; set; }

        /// <summary>
        /// 新聞時間
        /// </summary>
        [JsonProperty("v2")]
        public string NewsTime { get; set; }

        /// <summary>
        /// 新聞標題
        /// </summary>
        [JsonProperty("v3")]
        public string NewsTitle { get; set; }

        /// <summary>
        /// 新聞序號
        /// </summary>
        [JsonProperty("v4")]
        public string NewsSerialNumber { get; set; }

        /// <summary>
        /// 新聞類別
        /// </summary>
        [JsonProperty("v5")]
        public string NewsType { get; set; }

        /// <summary>
        /// 數量序號
        /// </summary>
        [JsonProperty("v6")]
        public string QuantityNumber { get; set; }
    }
}
using Newtonsoft.Json;

namespace Feature.Wealth.ScheduleAgent.Models.News.Content
{
    public class NewsContentRequest
    {
        /// <summary>
        /// 新聞序號
        /// </summary>
        public string NewsSerialNumber { get; set; }
    }

    public class NewsContentResponse
    {
        [JsonProperty("resultSet")]
        public NewsContentResultset ResultSet { get; set; }
    }

    public class NewsContentResultset
    {
        [JsonProperty("result")]
        public NewsContentResult[] Result { get; set; }
    }

    public class NewsContentResult
    {
        /// <summary>
        /// 新聞日期/時間
        /// </summary>
        [JsonProperty("v1")]
        public string NewsDateTime { get; set; }

        /// <summary>
        /// 新聞標題
        /// </summary>
        [JsonProperty("v2")]
        public string NewsTitle { get; set; }

        /// <summary>
        /// 新聞內文
        /// </summary>
        [JsonProperty("v3")]
        public string NewsContent { get; set; }

        /// <summary>
        /// 新聞相關商品
        /// </summary>
        /// <remarks>請取用 AS 開頭之商品代碼。 AS 為嘉實台股商品前綴詞（ex: 台積電 = AS2330）</remarks>
        [JsonProperty("v4")]
        public string RelatedProduct { get; set; }

        /// <summary>
        /// 新聞類別
        /// </summary>
        [JsonProperty("v5")]
        public string NewsType { get; set; }
    }
}
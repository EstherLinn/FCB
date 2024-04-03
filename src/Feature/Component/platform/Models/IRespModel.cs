using Newtonsoft.Json;
using System.Net;

namespace Feature.Wealth.Component.Models
{
    /// <summary>
    /// 回應介面
    /// </summary>
    /// <typeparam name="T">type of Response</typeparam>
    public interface IRespModel<T>
    {
        /// <summary>
        /// 回應代碼
        /// </summary>
        [JsonProperty("statusCode")]
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 回應訊息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// 回應結果
        /// </summary>
        [JsonProperty("body")]
        public T Body { get; set; }
    }
}

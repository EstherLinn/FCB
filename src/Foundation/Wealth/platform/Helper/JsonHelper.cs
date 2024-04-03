using Newtonsoft.Json;


namespace Foundation.Wealth.Helper
{
    public class JsonHelper
    {
        /// <summary>
        /// 產生Json序列後文字
        /// </summary>
        /// <param name="obj">物件</param>
        /// <returns>JSON</returns>
        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 產生Json序列後文字
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 產生Json序列後文字
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string SerializeObjectWithEscapeHtml(object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeHtml });
        }

        /// <summary>
        /// Json string 轉換成 dynamic
        /// </summary>
        /// <param name="data">資料json</param>
        /// <returns>Object 物件</returns>
        public static dynamic DeserializeObject(string json)
        {
            return JsonConvert.DeserializeObject<object>(json);
        }

        /// <summary>
        /// Json string 轉換成 T
        /// </summary>
        /// <typeparam name="T">物件</typeparam>
        /// <param name="json">資料json</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string json, bool toLower = false)
        {
            if (toLower == true)
                json = json.ToLower();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

}

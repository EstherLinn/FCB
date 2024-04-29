using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;

namespace Feature.Wealth.Component.Repositories
{
    public class ViewCountRepository
    {
        public int GetViewCountInfo(string pageItemId, string currentUrl)
        {
            int viewCount = 0;

            // 檢查 pageItemId 和 currentUrl 是否都有值
            if (!string.IsNullOrEmpty(pageItemId) && !string.IsNullOrEmpty(currentUrl))
            {
                // 先用 pageItemId 查詢並獲取符合條件的資料
                string selectSql = "SELECT Url, ViewCount FROM AllViewCount WHERE Id = @PageItemId";
                var selectParams = new { PageItemId = pageItemId };
                var rows = DbManager.Custom.ExecuteIList<(string Url, int ViewCount)>(selectSql, selectParams, CommandType.Text);

                // 如果找到匹配的資料
                if (rows.Any())
                {
                    // 解析當前的currentUrl，並把其中的 queryString 放到 Dictionary 裡
                    var currentUriPath = GetFormattedUriPath(currentUrl);
                    Dictionary<string, string> currentQueryStringDictionary = GetQueryStringDictionary(currentUrl);

                    // 逐一比對資料表裡的資料
                    foreach (var (url, count) in rows)
                    {
                        var dbUriPath = GetFormattedUriPath(url);

                        // 檢查當前網址與資料表裡的網址是否一致
                        if (currentUriPath == dbUriPath)
                        {
                            Dictionary<string, string> dbQueryStringDictionary = GetQueryStringDictionary(url);

                            // 1. 先檢查currentUrl 的 QueryString 和資料庫記錄的 QueryString 的鍵值對數量是否相同，如果數量不同，說明查詢字串不匹配，直接跳過目前記錄。
                            // 2. 檢查是否currentUrl 的 QueryString 中包含資料庫記錄的每一個鍵，且currentUrl 的 QueryString 中每個鍵對應的值與資料庫記錄中的值完全相同。
                            if (dbQueryStringDictionary.Count == currentQueryStringDictionary.Count &&
                                dbQueryStringDictionary.All(kvp => currentQueryStringDictionary.ContainsKey(kvp.Key) && currentQueryStringDictionary[kvp.Key] == kvp.Value))
                            {
                                viewCount = count;
                                break;
                            }
                        }
                    }
                }
            }

            return viewCount;
        }

        public int UpdateViewCountInfo(string pageItemId, string currentUrl)
        {
            int viewCount = 0;

            // 檢查 pageItemId 和 currentUrl 是否都有值
            if (!string.IsNullOrEmpty(pageItemId) && !string.IsNullOrEmpty(currentUrl))
            {
                // 先用 pageItemId 查詢並獲取符合條件的資料
                string selectSql = "SELECT Url, ViewCount FROM AllViewCount WHERE Id = @PageItemId";
                var selectParams = new { PageItemId = pageItemId };
                var rows = DbManager.Custom.ExecuteIList<(string Url, int ViewCount)>(selectSql, selectParams, CommandType.Text);

                // 是否找到匹配的記錄
                bool foundMatch = false;

                // 解析當前的 currentUrl，並把其中的 queryString 放到 Dictionary 裡
                var currentUriPath = GetFormattedUriPath(currentUrl);
                Dictionary<string, string> currentQueryStringDictionary = GetQueryStringDictionary(currentUrl);

                // 逐一比對資料表裡的資料
                foreach (var (url, count) in rows)
                {
                    var dbUriPath = GetFormattedUriPath(url);

                    // 檢查當前網址與資料表裡的網址是否一致
                    if (currentUriPath == dbUriPath)
                    {
                        Dictionary<string, string> dbQueryStringDictionary = GetQueryStringDictionary(url);

                        // 1. 先檢查currentUrl 的 QueryString 和資料庫記錄的 QueryString 的鍵值對數量是否相同，如果數量不同，說明查詢字串不匹配，直接跳過目前記錄。
                        // 2. 檢查是否currentUrl 的 QueryString 中包含資料庫記錄的每一個鍵，且currentUrl 的 QueryString 中每個鍵對應的值與資料庫記錄中的值完全相同。
                        if (dbQueryStringDictionary.Count == currentQueryStringDictionary.Count &&
                            dbQueryStringDictionary.All(kvp => currentQueryStringDictionary.ContainsKey(kvp.Key) && currentQueryStringDictionary[kvp.Key] == kvp.Value))
                        {
                            // 更新 viewCount
                            string updateSql = "UPDATE AllViewCount SET ViewCount = @NewViewCount WHERE Id = @PageItemId AND Url = @Url";
                            var updateParams = new { NewViewCount = count + 1, PageItemId = pageItemId, Url = url };
                            DbManager.Custom.Execute<int>(updateSql, updateParams, CommandType.Text);

                            viewCount = count + 1;
                            foundMatch = true;
                            break;
                        }
                    }
                }

                // 如果未找到匹配的記錄，則插入一條新的記錄
                if (!foundMatch)
                {
                    string insertSql = "INSERT INTO AllViewCount (Id, Url, ViewCount) VALUES (@PageItemId, @CurrentUrl, 1)";
                    var insertParams = new { PageItemId = pageItemId, CurrentUrl = GetFormattedUri(currentUrl) };
                    DbManager.Custom.Execute<int>(insertSql, insertParams, CommandType.Text);

                    viewCount = 1;
                }
            }

            return viewCount;
        }

        public string GetFormattedUriPath(string url)
        {
            var formattedUriPath = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                var uri = new Uri(url);

                // 取得其中 Root
                var uriRoot = uri.GetLeftPart(UriPartial.Authority);

                // 取得其中網址路徑
                var uriPath = uri.AbsolutePath;

                // 將網址路徑轉換為小寫
                string lowercaseUriPath = uriPath.ToLowerInvariant();
                // 解碼網址路徑中的百分比編碼
                string decodedUriPath = Uri.UnescapeDataString(lowercaseUriPath);
                // 將網址路徑的"-"替換為空格並在前頭將上Root
                formattedUriPath = uriRoot + decodedUriPath.Replace("-", " ");
            }

            return formattedUriPath;
        }

        public Dictionary<string, string> GetQueryStringDictionary(string url)
        {
            Dictionary<string, string> queryStringDictionary = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(url))
            {
                var uri = new Uri(url);
                NameValueCollection queryStringCollection = HttpUtility.ParseQueryString(uri.Query);
                queryStringDictionary = queryStringCollection.AllKeys.ToDictionary(key => key, key => queryStringCollection[key]);
            }

            return queryStringDictionary;
        }

        public string GetFormattedUri(string url)
        {
            var formattedUri = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                var uriPath = GetFormattedUriPath(url);
                var uriQueryString = new Uri(url).Query;
                formattedUri = uriPath + uriQueryString;
            }

            return formattedUri;
        }
    }
}

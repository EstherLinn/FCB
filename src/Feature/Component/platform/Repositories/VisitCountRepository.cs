using Feature.Wealth.Component.Models.VisitCount;
using Foundation.Wealth.Manager;
using log4net;
using Sitecore.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Component.Repositories
{
    public class VisitCountRepository
    {
        private ILog Log { get; } = Logger.General;

        /// <summary>
        /// 指定參數Key取得瀏覽紀錄
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="querystringKeys">參數Key值</param>
        /// <returns></returns>
        public IEnumerable<VisitInfo> GetVisitRecords(Guid? pageId, params string[] querystringKeys)
        {
            if (!pageId.HasValue || pageId.Value == Guid.Empty)
            {
                return null;
            }

            var records = QueryVisitRecords(pageId);

            if (records == null)
            {
                return null;
            }

            var result = new List<VisitInfo>();

            if (querystringKeys.Any())
            {
                querystringKeys = querystringKeys.Select(x => x.ToLower()).ToArray();

                foreach (VisitCountModel record in records)
                {
                    if (string.IsNullOrWhiteSpace(record.QueryStrings))
                    {
                        continue;
                    }

                    var record_queryStr = ParseUrlQueryStringToDictionary(Sitecore.Web.WebUtil.ParseQueryString(record.QueryStrings));
                    var matchKeys = record_queryStr.Keys.Intersect(querystringKeys);

                    if (matchKeys.Any())
                    {
                        VisitInfo countEntity = new VisitInfo()
                        {
                            VisitCount = record.VisitCount,
                            QueryStrings = record_queryStr
                        };

                        result.Add(countEntity);
                    }
                }
            }
            else
            {
                // 無指定參數
                foreach (VisitCountModel record in records)
                {
                    var record_queryStr = ParseUrlQueryStringToDictionary(Sitecore.Web.WebUtil.ParseQueryString(record.QueryStrings));
                    VisitInfo countEntity = new VisitInfo()
                    {
                        VisitCount = record.VisitCount,
                        QueryStrings = record_queryStr
                    };

                    result.Add(countEntity);
                }
            }

            return result;
        }

        /// <summary>
        /// 取得瀏覽次數
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public int? GetVisitCount(Guid? pageId, string url)
        {
            var records = QueryVisitRecords(pageId)?.ToList();

            if (records == null)
            {
                return null;
            }

            int? count = null;

            try
            {
                var collection = Sitecore.Web.WebUtil.ParseQueryString(url?.ToLower());
                var currentUrlParameters = ParseUrlQueryStringToDictionary(collection);

                if (!string.IsNullOrEmpty(url) && !currentUrlParameters.Any())
                {
                    count = records.Find(x => string.IsNullOrEmpty(x.QueryStrings))?.VisitCount;
                    return count;
                }

                foreach (VisitCountModel record in records)
                {
                    if (string.IsNullOrWhiteSpace(record.QueryStrings))
                    {
                        continue;
                    }

                    var record_queryStr = Sitecore.Web.WebUtil.ParseQueryString(record.QueryStrings);
                    var matchKeys = record_queryStr.Keys.Intersect(currentUrlParameters.Keys);

                    foreach (string key in matchKeys)
                    {
                        if (record_queryStr[key].Equals(currentUrlParameters[key], StringComparison.OrdinalIgnoreCase))
                        {
                            count = record.VisitCount;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return count;
        }

        /// <summary>
        /// 更新並取得瀏覽次數
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="url"></param>
        /// <param name="querystring"></param>
        /// <returns></returns>
        public async Task<VisitCountModel> UpdateVisitCount(Guid? pageId, string url, params string[] querystring)
        {
            bool isValid = false;
            IList<VisitCountModel> result = null;

            try
            {
                var queryStrPairs = ParseQueryStringToDictionary(querystring);
                var currentUrlParameters = ParseUrlQueryStringToDictionary(Sitecore.Web.WebUtil.ParseQueryString(url));

                if (queryStrPairs.Any())
                {
                    var matchKeys = queryStrPairs.Keys.Intersect(currentUrlParameters.Keys);

                    foreach (string key in matchKeys)
                    {
                        if (queryStrPairs[key].Equals(currentUrlParameters[key], StringComparison.OrdinalIgnoreCase))
                        {
                            isValid = true;
                            break;
                        }
                    }
                }

                if (!isValid)
                {
                    return null;
                }

                int visitCount = 1;
                string queryStrings = null;
                queryStrPairs = queryStrPairs.OrderBy(x => x.Key).ToDictionary(kv => kv.Key, kv => kv.Value);

                if (queryStrPairs.Any())
                {
                    queryStrings = string.Join("&", queryStrPairs.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                }

                string sql = """
                    DECLARE @SummaryOfChanges TABLE(
                        [Action] VARCHAR(10), [PageId] UNIQUEIDENTIFIER,
                        [VisitCount] BIGINT, [QueryStrings] NVARCHAR (1000)
                    );

                    MERGE VisitCount AS target
                    USING (SELECT @PageId AS PageId, @QueryStrings AS QueryStrings) AS source
                        ON target.PageId = source.PageId AND target.QueryStrings = source.QueryStrings
                    WHEN MATCHED THEN
                        UPDATE SET [VisitCount] = [VisitCount] + 1
                    WHEN NOT MATCHED THEN
                        INSERT ( [PageId], [VisitCount], [QueryStrings] )
                        VALUES ( @PageId, @VisitCount, @QueryStrings )
                    OUTPUT $action, Inserted.* INTO @SummaryOfChanges;

                    SELECT * FROM @SummaryOfChanges;
                """;

                var param = new { PageId = pageId.Value, VisitCount = visitCount, QueryStrings = queryStrings };
                result = await DbManager.Custom.ExecuteIListAsync<VisitCountModel>(sql, param, CommandType.Text);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// 查詢瀏覽紀錄
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        private IEnumerable<VisitCountModel> QueryVisitRecords(Guid? pageId)
        {
            string sql = """
                SELECT [PageId]
                      ,[VisitCount]
                      ,[QueryStrings]
                FROM [dbo].[VisitCount] WITH(NOLOCK)
                WHERE [PageId] = @PageId
                """;
            var param = new { PageId = pageId.Value };
            IEnumerable<VisitCountModel> result = DbManager.Custom.ExecuteIList<VisitCountModel>(sql, param, CommandType.Text);
            return result;
        }

        private Dictionary<string, string> ParseUrlQueryStringToDictionary(SafeDictionary<string> collection)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var key in collection.Keys)
            {
                dic[key.ToLower()] = collection[key.ToLower()];
            }

            return dic;
        }

        private Dictionary<string, string> ParseQueryStringToDictionary(params string[] querystring)
        {
            var queryStrPairs = new Dictionary<string, string>();

            foreach (var str in querystring)
            {
                var collection = Sitecore.Web.WebUtil.ParseQueryString(str);

                foreach (var key in collection.Keys)
                {
                    queryStrPairs[key.ToLower()] = collection[key.ToLower()];
                }
            }

            return queryStrPairs;
        }
    }
}
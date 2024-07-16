using Feature.Wealth.ScheduleAgent.Models.News.Content;
using Feature.Wealth.ScheduleAgent.Models.News.List;
using Feature.Wealth.ScheduleAgent.Services;
using Foundation.Wealth.Manager;
using Mapster;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.ScheduleAgent.Repositories
{
    public class NewsRepository
    {
        private readonly ILoggerService _logger;
        private readonly NewsService _service;

        public NewsRepository(ILoggerService logger)
        {
            this._logger = logger;
            this._service = new NewsService(logger);
        }

        #region 新聞列表

        /// <summary>
        /// 存入新聞列表資訊
        /// </summary>
        public async Task SaveNewsListData(Item settings)
        {
            NewsListRequest req = new NewsListRequest
            {
                Id = settings["Id"],
                Count = settings.GetInteger("Count")
            };

            int? interval = settings.GetInteger("Interval Minutes");
            
            if (!interval.HasValue)
            {
                interval = 30;
            }

            List<NewsListDto> latestTimeDataList = QueryLastestNewsListTable()?.ToList();
            NewsListDto lastestData = latestTimeDataList.FirstOrDefault();

            string dateformat = "yyyy/MM/dd HH:mm";
            string dateFormat_iso8601 = "yyyy-MM-ddTHH:mm"; // 轉換為 ISO 8601 格式
            var cultureInfo = new CultureInfo("zh-TW");

            DateTime startDate;
            DateTime? startDateTime = settings.GetLocalDateFieldValue("Start DateTime");

            if (startDateTime.HasValue)
            {
                req.StartDateTime = startDateTime.Value.ToString(dateFormat_iso8601);
                req.EndDateTime = startDateTime.Value.AddMinutes(interval.Value).ToString(dateFormat_iso8601);
            }
            else if (lastestData != null) // 以最後一筆資料的時間為準
            {
                // 解析日期和時間
                if (DateTime.TryParseExact($"{lastestData.NewsDate} {lastestData.NewsTime}", dateformat, cultureInfo, DateTimeStyles.None, out startDate))
                {
                    req.StartDateTime = startDate.ToString(dateFormat_iso8601);
                    req.EndDateTime = startDate.AddMinutes(interval.Value).ToString(dateFormat_iso8601);
                }
            }

            int retryCount = 0, hours = 0;
            int retryLimit = settings.GetInteger("Retry Count").HasValue ? settings.GetInteger("Retry Count").Value : 3;
            List<NewsListDto> newDatas = null;

            while (retryCount <= retryLimit)
            {
                try
                {
                    newDatas = await GetAndProcessNewsListData(req, latestTimeDataList);

                    if (newDatas == null || !newDatas.Any()) // 取得API回應並且過濾最新日期的資料後, 若還是無資料則重打API
                    {
                        if (DateTime.TryParseExact(req.StartDateTime, dateFormat_iso8601, cultureInfo, DateTimeStyles.None, out startDate))
                        {
                            hours += 12;
                            req.EndDateTime = startDate.AddHours(hours).ToString(dateFormat_iso8601);
                        }

                        retryCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    this._logger.Error(ex.Message, ex);
                    break;
                }
            }

            await UpdateNewsListIntoTableAsync(newDatas);
        }

        /// <summary>
        /// 取得並處理新聞列表API回應
        /// </summary>
        /// <param name="req">Request</param>
        /// <param name="latestTimeDataList">最新日期的新聞列表資料</param>
        /// <returns></returns>
        private async Task<List<NewsListDto>> GetAndProcessNewsListData(NewsListRequest req, List<NewsListDto> latestTimeDataList)
        {
            // Call API
            List<NewsListResult> dataResponse = await _service.GetNewsListData(req);

            if (dataResponse == null || !dataResponse.Any())
            {
                this._logger.Info("No data received from API.");
                return null;
            }

            var dto = dataResponse.Adapt<List<NewsListDto>>();
            var newData = dto.Except(latestTimeDataList, new NewsListDtoComparer()).ToList();

            return newData;
        }

        /// <summary>
        /// 從資料表取得最新日期的新聞列表資訊
        /// </summary>
        /// <returns></returns>
        private IList<NewsListDto> QueryLastestNewsListTable()
        {
            IList<NewsListDto> result = null;

            string sql = """
                WITH CTE AS (
                    SELECT TOP 1 [NewsDate], [NewsTime]
                    FROM [dbo].[NewsList] WITH (NOLOCK)
                    ORDER BY [NewsDate] DESC, [NewsTime] DESC
                )

                SELECT [List].*
                FROM [dbo].[NewsList] AS [List] WITH (NOLOCK)
                INNER JOIN [CTE] AS [Last] WITH (NOLOCK)
                ON [Last].[NewsDate] = [List].[NewsDate]
                    AND [Last].[NewsTime] = [List].[NewsTime]
            """;
            try
            {
                result = DbManager.Custom.ExecuteIList<NewsListDto>(sql, null, CommandType.Text);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 更新新聞列表到資料表
        /// </summary>
        /// <param name="list"></param>
        private async Task UpdateNewsListIntoTableAsync(List<NewsListDto> list)
        {
            if (list == null || !list.Any())
            {
                return;
            }

            string sql = """
                DECLARE @SummaryOfChanges TABLE(
                    [Action] VARCHAR(10), [NewsSerialNumber] NVARCHAR (50)
                );

                MERGE NewsList AS target
                USING (SELECT @NewsSerialNumber AS NewsSerialNumber, @NewsDate AS NewsDate, @NewsTime AS NewsTime) AS source
                    ON target.NewsSerialNumber = source.NewsSerialNumber
                    AND target.NewsDate = source.NewsDate
                    AND target.NewsTime = source.NewsTime
                WHEN MATCHED THEN
                    UPDATE SET NewsDate = @NewsDate
                    , NewsTime = @NewsTime
                    , NewsTitle = @NewsTitle
                    , NewsSerialNumber = @NewsSerialNumber
                WHEN NOT MATCHED THEN
                    INSERT ( NewsDate, NewsTime, NewsTitle, NewsSerialNumber )
                    VALUES ( @NewsDate, @NewsTime, @NewsTitle, @NewsSerialNumber );
            """;
            try
            {
                var result = await DbManager.Custom.ExecuteNonQueryAsync(sql, list, CommandType.Text);
                this._logger.Info($"新聞列表資更新數: {result}");
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.Message, ex);
            }
        }

        #endregion 新聞列表

        #region 新聞內容

        /// <summary>
        /// 存入新聞內容
        /// </summary>
        /// <returns></returns>
        public async Task SaveNewsLContentData()
        {
            var list = RetrieveIDsWithoutData();

            if (list == null || !list.Any())
            {
                this._logger.Info("沒有需要匯入的新聞內容。");
                return;
            }

            NewsContentRequest req = new NewsContentRequest();
            List<NewsContentDto> newsList = new List<NewsContentDto>();

            foreach (var sn in list)
            {
                req.NewsSerialNumber = sn;
                List<NewsContentResult> dataResponse = await _service.GetNewsContentData(req);
                NewsContentResult newsData = dataResponse.FirstOrDefault();
                NewsContentDto dto = MapNewsContent(newsData, req);
                newsList.Add(dto);
            }

            if (newsList.Any())
            {
                await UpdateNewsContentIntoTable(newsList);
            }
        }

        /// <summary>
        /// 取得市場新聞列表資料表裡還沒有取得詳細頁資料的ID
        /// </summary>
        /// <returns></returns>
        public IList<string> RetrieveIDsWithoutData()
        {
            IList<string> result = null;

            string sql = """
                SELECT DISTINCT [ListTable].[NewsSerialNumber]
                FROM [dbo].[NewsList] AS [ListTable] WITH (NOLOCK)
                LEFT JOIN
                (
                    SELECT [NewsSerialNumber]
                    FROM [dbo].[NewsDetail] WITH (NOLOCK)
                ) AS [DetailTable]
                ON [ListTable].[NewsSerialNumber] = [DetailTable].[NewsSerialNumber]
                WHERE [DetailTable].[NewsSerialNumber] IS NULL
            """;
            try
            {
                result = DbManager.Custom.ExecuteIList<string>(sql, null, CommandType.Text);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 將新聞內容資料轉換
        /// </summary>
        /// <param name="newsContent"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private NewsContentDto MapNewsContent(NewsContentResult newsContent, NewsContentRequest request)
        {
            NewsContentDto result = null;

            try
            {
                var config = new TypeAdapterConfig();
                config.ForType<NewsContentResult, NewsContentDto>()
                    .Map(dest => dest.NewsSerialNumber, src => request.NewsSerialNumber)
                    .Map(dest => dest.NewsDetailDate, src => src.NewsDateTime)
                    //.Map(dest => dest.NewsTitle, src => src.NewsTitle)
                    //.Map(dest => dest.NewsContent, src => src.NewsContent)
                    //.Map(dest => dest.NewsType, src => src.NewsType)
                    .Map(dest => dest.NewsRelatedProducts, src => src.RelatedProduct);

                result = newsContent.Adapt<NewsContentDto>(config);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 寫入新聞內容到資料表
        /// </summary>
        /// <param name="list"></param>
        private async Task UpdateNewsContentIntoTable(List<NewsContentDto> list)
        {
            if (list == null || !list.Any())
            {
                return;
            }

            string sql = """
                DECLARE @SummaryOfChanges TABLE(
                    [Action] VARCHAR(10), [NewsSerialNumber] NVARCHAR (50)
                );
            
                MERGE NewsDetail AS target
                USING (SELECT @NewsSerialNumber AS NewsSerialNumber) AS source
                    ON target.NewsSerialNumber = source.NewsSerialNumber
                WHEN MATCHED THEN
                    UPDATE SET NewsDetailDate = @NewsDetailDate
                    , NewsTitle = @NewsTitle
                    , NewsContent = @NewsContent
                    , NewsRelatedProducts = @NewsRelatedProducts
                    , NewsType = @NewsType
                    , NewsSerialNumber = @NewsSerialNumber
                WHEN NOT MATCHED THEN
                    INSERT ( [NewsSerialNumber], [NewsDetailDate], [NewsTitle], [NewsContent], [NewsRelatedProducts], [NewsType] )
                    VALUES ( @NewsSerialNumber, @NewsDetailDate, @NewsTitle, @NewsContent, @NewsRelatedProducts, @NewsType );
            """;
            try
            {
                var result = await DbManager.Custom.ExecuteNonQueryAsync(sql, list, CommandType.Text);
                this._logger.Info($"新聞內容資更新數: {result}");
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.Message, ex);
            }
        }

        #endregion 新聞內容
    }
}
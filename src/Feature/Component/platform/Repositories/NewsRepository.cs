using Feature.Wealth.Component.Models.News;
using Feature.Wealth.Component.Models.News.NewsList;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Manager;
using Mapster;
using Newtonsoft.Json.Linq;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class NewsRepository
    {
        private readonly DjMoneyApiRespository _djMoneyApiRespository = new DjMoneyApiRespository();
        private readonly VisitCountRepository _visitCountRepository = new VisitCountRepository();

        #region 最新消息

        public static readonly ID NewsCategorySettingFolder = new ID("{92B41AC0-BC37-472B-8189-79884404AAB4}");

        public NewsListViewModel GetNewsListViewModel(string datasourceId)
        {
            NewsListViewModel viewModel = new NewsListViewModel
            {
                DatasourceId = datasourceId,
                CategoryList = GetNewsListCategories()
            };

            return viewModel;
        }

        /// <summary>
        /// 取得最新消息類別
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetNewsListCategories()
        {
            List<string> categoryList = new List<string>();
            Item sourceFolder = ItemUtils.GetItem(NewsCategorySettingFolder);

            if (sourceFolder != null)
            {
                var options = sourceFolder.GetChildren(Templates.NewsListCategory.Id);

                foreach (var item in options)
                {
                    string text = item.GetFieldValue(Templates.NewsListCategory.Fields.CategoryName);
                    categoryList.Add(text);
                }
            }

            return categoryList;
        }

        /// <summary>
        /// 取得最新消息列表結果
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<NewsListResult> GetNewsListResult(ReqNewsList req)
        {
            if (!req.DatasourceId.HasValue || req.DatasourceId.Value == Guid.Empty)
            {
                return new List<NewsListResult>();
            }

            var datasource = ItemUtils.GetItem(req.DatasourceId.Value);
            if (datasource == null)
            {
                return new List<NewsListResult>();
            }

            var result = MapperNewsResult(datasource);
            return result;
        }

        private List<NewsListResult> MapperNewsResult(Item item)
        {
            NewsListModel model = new NewsListModel();
            model.Initialize(item);
            model.NewsItems = model.NewsItems.OrderByDescending(i => i.Date.HasValue ? i.Date.Value : DateTime.MinValue);

            var config = new TypeAdapterConfig();
            config.ForType<Data, NewsListResult>()
                .AfterMapping((src, dest) =>
                {
                    long timeOffsetLong = 0;
                    if (src.Date.HasValue)
                    {
                        var timeOffset = new DateTimeOffset(src.Date.Value);
                        timeOffsetLong = timeOffset.ToUnixTimeMilliseconds();
                    }

                    dest.PageTitlePair = new KeyValuePair<string, string>(src.PageTitle, string.IsNullOrEmpty(src.PageTitle) ? "-" : src.PageTitle);
                    dest.DatePair = new KeyValuePair<long, string>(timeOffsetLong, src.Date.HasValue ? DateTimeExtensions.FormatDate(src.Date) : "-");
                    dest.CategoryPair = new KeyValuePair<string, string>(src.Category, string.IsNullOrEmpty(src.Category) ? string.Empty : src.Category);
                    dest.FocusPair = new KeyValuePair<int, bool>(Convert.ToInt32(src.IsFocus), src.IsFocus);
                });

            var result = model.NewsItems.Adapt<List<NewsListResult>>(config);
            return result;
        }

        #endregion 最新消息

        #region 市場新聞

        /// <summary>
        ///  整理市場新聞Api資料
        /// </summary>
        public List<MarketNewsModel> OrganizeMarketNewsApiData(JObject resp)
        {
            var datas = new List<MarketNewsModel>();

            if (resp != null
                && resp.ContainsKey("resultSet")
                && resp["resultSet"] != null
                && resp["resultSet"]["result"] != null
                && resp["resultSet"]["result"].Any())
            {
                var resultList = resp["resultSet"]["result"];

                foreach (var item in resultList)
                {
                    var newsData = new MarketNewsModel
                    {
                        NewsDate = item["v1"].ToString(),
                        NewsTime = item["v2"].ToString(),
                        NewsTitle = item["v3"].ToString(),
                        NewsSerialNumber = item["v4"].ToString()
                    };

                    datas.Add(newsData);
                }
            }

            return datas;
        }

        /// <summary>
        /// 更新市場新聞資料庫資料
        /// </summary>
        public void UpdateMarketNewsData(List<MarketNewsModel> datas)
        {
            if (datas != null && datas.Any())
            {
                var existingNews = DbManager.Custom.ExecuteIList<MarketNewsModel>(@"
        SELECT [NewsSerialNumber]
        FROM [dbo].[NewsList]", null, CommandType.Text);

                var existingSerialNumbers = existingNews.Select(news => news.NewsSerialNumber).ToList();

                var newData = datas
                    .Where(news => !existingSerialNumbers.Contains(news.NewsSerialNumber))
                    .ToList();

                if (newData.Any())
                {
                    foreach (var news in newData)
                    {
                        DbManager.Custom.ExecuteNonQuery(@"
                INSERT INTO [dbo].[NewsList] ([NewsDate], [NewsTime], [NewsTitle], [NewsSerialNumber])
                VALUES (@NewsDate, @NewsTime, @NewsTitle, @NewsSerialNumber)", new
                        {
                            NewsDate = news.NewsDate,
                            NewsTime = news.NewsTime,
                            NewsTitle = news.NewsTitle,
                            NewsSerialNumber = news.NewsSerialNumber,
                        }, CommandType.Text);
                    }
                }
            }
        }

        /// <summary>
        /// 排程用更新聞列表資料庫資料
        /// </summary>
        public void ScheduleUpdateNewsList(string id, string count, string startDatetime, string endDatetime)
        {
            // 取得 MarketNewsApi 資料
            var resp = _djMoneyApiRespository.GetMarketNewsData(id, count, startDatetime, endDatetime);

            // 整理 MarketNewsApi 資料
            var repsDatas = OrganizeMarketNewsApiData(resp);

            // 更新資料到資料庫
            UpdateMarketNewsData(repsDatas);
        }

        /// <summary>
        ///  取得市場新聞資料庫資料
        /// </summary>
        public IList<MarketNewsModel> GetMarketNewsDbData()
        {
            // 构建 SQL 查询
            string query = @"
            WITH CTE AS (
            SELECT
                nl.[NewsDate],
                nl.[NewsTime],
                nl.[NewsTitle],
                nl.[NewsSerialNumber],
                nd.[NewsDetailDate],
                nd.[NewsContent],
                nd.[NewsRelatedProducts],
                nd.[NewsType],
                ROW_NUMBER() OVER (PARTITION BY nl.[NewsSerialNumber] ORDER BY nl.[NewsDate] DESC, nl.[NewsTime] DESC) AS RowNum
            FROM
                [dbo].[NewsList] nl
            LEFT JOIN
                [dbo].[NewsDetail] nd
                ON nl.[NewsSerialNumber] = nd.[NewsSerialNumber]
        )
        SELECT
            [NewsDate],
            [NewsTime],
            [NewsTitle],
            [NewsSerialNumber],
            [NewsDetailDate],
            [NewsContent],
            [NewsRelatedProducts],
            [NewsType]
        FROM
            CTE
        WHERE
            RowNum = 1
            AND [NewsType] IS NOT NULL
        ORDER BY
            [NewsDate] DESC,
            [NewsTime] DESC;";

            return DbManager.Custom.ExecuteIList<MarketNewsModel>(query, null, CommandType.Text);
        }

        /// <summary>
        ///  整理市場新聞資料庫資料
        /// </summary>
        public List<MarketNewsModel> OrganizeMarketNewsDbData(List<MarketNewsModel> _datas, string id, string startDatetime, string endDatetime)
        {
            var datas = new List<MarketNewsModel>();

            if (_datas == null || !_datas.Any())
            {
                return datas;
            }

            DateTime startDate = Convert.ToDateTime(startDatetime);
            DateTime endDate = Convert.ToDateTime(endDatetime);

            var filteredDatas = _datas
                .Where(e => startDate <= Convert.ToDateTime(e.NewsDate) && Convert.ToDateTime(e.NewsDate) <= endDate)
                .ToList();

            if (id != "1")
            {
                var idList = id.Split(',').Select(x => x.Trim()).ToList();

                string query = @"
            SELECT [NewsType]
            FROM [dbo].[NewsType]
            WHERE [TypeNumber] IN @IdList";

                var newsTypeList = DbManager.Custom.ExecuteIList<string>(query, new { IdList = idList }, CommandType.Text);

                filteredDatas = filteredDatas
                    .Where(news =>
                        !string.IsNullOrEmpty(news.NewsType) &&
                        news.NewsType.Split(',').Any(type => newsTypeList.Contains(type.Trim())))
                    .ToList();
            }

            foreach (var item in filteredDatas)
            {
                var newsData = new MarketNewsModel
                {
                    HotNews = !string.IsNullOrEmpty(item.NewsType) && item.NewsType.Contains("頭條新聞") ? 2 : 1,
                    DisplayHotNews = !string.IsNullOrEmpty(item.NewsType) && item.NewsType.Contains("頭條新聞") ? string.Empty : "u-invisible",
                    NewsType = item.NewsType ?? string.Empty,
                    NewsDate = item.NewsDate.ToString() + " " + item.NewsTime.ToString(),
                    NewsTitle = item.NewsTitle,
                    NewsSerialNumber = item.NewsSerialNumber,
                    NewsDetailLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + item.NewsSerialNumber
                };

                newsData.Data = new MarketNewsData
                {
                    Type = item.NewsType ?? string.Empty,
                    IsLogin = false,
                    IsNews = false,
                    IsLike = false,
                    DetailUrl = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + item.NewsSerialNumber,
                    Purchase = false
                };

                newsData.value = item.NewsTitle;

                datas.Add(newsData);
            }

            return datas;
        }

        /// <summary>
        ///  取得市場新聞瀏覽人次
        /// </summary>
        public List<MarketNewsModel> GetMarketNewsViewCount(List<MarketNewsModel> datas)
        {
            if (datas != null
                && datas.Any())
            {
                string pageItemId = MarketNewsRelatedLinkSetting.GetMarketNewsDetailPageItemId();
                string rootPath = System.Web.HttpContext.Current.Request.Url.GetLeftPart(System.UriPartial.Authority);

                foreach (var item in datas)
                {
                    var currentUrl = rootPath + MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + HttpUtility.UrlEncode(item.NewsSerialNumber.ToString());
                    var visitCount = _visitCountRepository.GetVisitCount(pageItemId.ToGuid(), currentUrl);
                    item.NewsViewCount = visitCount ?? 0;
                    item.DisplayNewsViewCount = visitCount?.ToString("N0") ?? "0";
                }
            }

            return datas;
        }

        #endregion 市場新聞

        #region 市場新聞詳細頁

        /// <summary>
        ///  取得市場新聞列表資料表裡還沒有取得詳細頁資料的ID
        /// </summary>
        public IList<string> RetrieveIDsWithoutData()
        {
            string query = @"
        WITH DetailSerialNumbers AS (
            SELECT NewsSerialNumber
            FROM [dbo].[NewsDetail]
        )
        SELECT DISTINCT nl.NewsSerialNumber
        FROM [dbo].[NewsList] nl
        LEFT JOIN DetailSerialNumbers ds
        ON nl.NewsSerialNumber = ds.NewsSerialNumber
        WHERE ds.NewsSerialNumber IS NULL";

            // 透過 Dapper 執行 SQL 查詢並取得結果
            var result = DbManager.Custom.ExecuteIList<string>(query, null, CommandType.Text);

            return result;
        }

        /// <summary>
        /// 整理市場新聞詳細頁Api資料
        /// </summary>
        public MarketNewsDetailModel OrganizeMarketNewsDetailApiData(JObject resp, string newsId)
        {
            var model = new MarketNewsDetailModel();

            var detailData = new MarketNewsDetailData();

            if (resp != null
                && resp.ContainsKey("resultSet")
                && resp["resultSet"] != null
                && resp["resultSet"]["result"] != null
                && resp["resultSet"]["result"].Any())
            {
                var result = resp["resultSet"]["result"][0];

                detailData.NewsSerialNumber = newsId;
                detailData.NewsDetailDate = result["v1"].ToString();
                detailData.NewsTitle = result["v2"].ToString();
                detailData.NewsContent = result["v3"].ToString();
                detailData.NewsContentHtmlString = new HtmlString(detailData.NewsContent);
                detailData.NewsRelatedProducts = result["v4"].ToString();
                detailData.NewsType = result["v5"].ToString();

                model.MarketNewsDetailData = detailData;
            }
            else
            {
                model.MarketNewsDetailData = null;
            }

            return model;
        }

        /// <summary>
        /// 更新市場新聞詳細頁資料庫資料
        /// </summary>
        public void UpdateMarketNewsDetailData(MarketNewsDetailModel datas)
        {
            if (datas.MarketNewsDetailData != null)
            {
                DbManager.Custom.ExecuteNonQuery(@"
                INSERT INTO [dbo].[NewsDetail] ([NewsSerialNumber], [NewsDetailDate], [NewsTitle], [NewsContent], [NewsRelatedProducts], [NewsType])
                VALUES (@NewsSerialNumber, @NewsDetailDate, @NewsTitle, @NewsContent, @NewsRelatedProducts, @NewsType)", new
                {
                    NewsSerialNumber = datas.MarketNewsDetailData.NewsSerialNumber,
                    NewsDetailDate = datas.MarketNewsDetailData.NewsDetailDate,
                    NewsTitle = datas.MarketNewsDetailData.NewsTitle,
                    NewsContent = datas.MarketNewsDetailData.NewsContent,
                    NewsRelatedProducts = datas.MarketNewsDetailData.NewsRelatedProducts,
                    NewsType = datas.MarketNewsDetailData.NewsType,
                }, CommandType.Text);
            }
        }

        /// <summary>
        /// 排程用更新聞詳細頁資料庫資料
        /// </summary>
        public void ScheduleUpdateNewsDetail()
        {
            // 取得市場新聞列表資料表裡還沒有取得詳細頁資料的ID
            var retrieveIDsWithoutData = RetrieveIDsWithoutData();

            if (retrieveIDsWithoutData.Any())
            {
                foreach (var id in retrieveIDsWithoutData)
                {
                    // 取得 MarketNewsDetailApi 資料
                    var resp = _djMoneyApiRespository.GetMarketNewsDetailData(id);

                    // 整理 MarketNewsDetailApi 資料
                    var repsDatas = OrganizeMarketNewsDetailApiData(resp, id);

                    // 更新資料到資料庫
                    UpdateMarketNewsDetailData(repsDatas);
                }
            }
        }

        /// <summary>
        ///  取得市場新聞詳細頁資料庫資料
        /// </summary>
        public MarketNewsDetailData GetMarketNewsDbDetailData(string newsId)
        {
            string serchQuery = "SELECT COUNT(*) FROM [dbo].[NewsDetail] WHERE [NewsSerialNumber] = @NewsSerialNumber";

            int count = DbManager.Custom.ExecuteScalar<int>(serchQuery, new { NewsSerialNumber = newsId }, CommandType.Text);

            bool dbDataExists = count > 0;

            if (dbDataExists)
            {
                // 获取指定 newsId 的日期
                string getDateQuery = "SELECT [NewsDate] FROM [dbo].[NewsList] WHERE [NewsSerialNumber] = @NewsSerialNumber";
                DateTime newsDate = DbManager.Custom.ExecuteScalar<DateTime>(getDateQuery, new { NewsSerialNumber = newsId }, CommandType.Text);

                // 计算日期范围
                var startDate = newsDate.AddDays(-1).ToString("yyyy/MM/dd");
                var endDate = newsDate.AddDays(1).ToString("yyyy/MM/dd");

                string query = @"
            WITH CTE AS (
            SELECT
                nl.[NewsDate],
                nl.[NewsTime],
                nl.[NewsTitle],
                nl.[NewsSerialNumber],
                nd.[NewsDetailDate],
                nd.[NewsContent],
                nd.[NewsRelatedProducts],
                nd.[NewsType],
                ROW_NUMBER() OVER (ORDER BY nl.[NewsDate] DESC, nl.[NewsTime] DESC) AS RowNumber
            FROM
                [dbo].[NewsList] nl
            LEFT JOIN
                [dbo].[NewsDetail] nd
            ON
                nl.[NewsSerialNumber] = nd.[NewsSerialNumber]
            WHERE
                nl.[NewsDate] BETWEEN @StartDate AND @EndDate
            )
            SELECT
                curr.[NewsSerialNumber],
                curr.[NewsDetailDate],
                curr.[NewsTitle],
                curr.[NewsContent],
                curr.[NewsRelatedProducts],
                curr.[NewsType],
                prev.[NewsSerialNumber] AS PreviousPageId,
                prev.[NewsTitle] AS PreviousPageTitle,
                next.[NewsSerialNumber] AS NextPageId,
                next.[NewsTitle] AS NextPageTitle
            FROM CTE curr
            LEFT JOIN CTE prev ON curr.RowNumber = prev.RowNumber + 1
            LEFT JOIN CTE next ON curr.RowNumber = next.RowNumber - 1
            WHERE curr.[NewsSerialNumber] = @NewsId";

                var result = DbManager.Custom.Execute<MarketNewsDetailData>(query, new
                {
                    NewsId = newsId,
                    StartDate = startDate,
                    EndDate = endDate
                }, CommandType.Text);

                return result;
            }

            return null;
        }

        /// <summary>
        ///  整理市場新聞詳細頁資料庫資料
        /// </summary>
        public MarketNewsDetailModel OrganizeMarketNewsDetailDbData(MarketNewsDetailData _datas)
        {
            var model = new MarketNewsDetailModel();

            if (_datas != null)
            {
                var detailData = new MarketNewsDetailData();

                detailData.NewsSerialNumber = _datas.NewsSerialNumber;
                detailData.NewsDetailDate = _datas.NewsDetailDate;
                detailData.NewsTitle = _datas.NewsTitle;
                detailData.NewsContent = _datas.NewsContent;
                detailData.NewsContentHtmlString = new HtmlString(detailData.NewsContent);
                detailData.NewsType = _datas.NewsType;
                detailData.PreviousPageId = _datas.PreviousPageId;
                detailData.PreviousPageTitle = _datas.PreviousPageTitle;
                detailData.PreviousPageLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + HttpUtility.UrlEncode(_datas.PreviousPageId);
                detailData.NextPageId = _datas.NextPageId;
                detailData.NextPageTitle = _datas.NextPageTitle;
                detailData.NextPageLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + HttpUtility.UrlEncode(_datas.NextPageId);
                detailData.NewsListUrl = MarketNewsRelatedLinkSetting.GetMarketNewsListUrl();

                model.MarketNewsDetailData = detailData;
            }
            else
            {
                model.MarketNewsDetailData = null;
            }

            return model;
        }

        #endregion 市場新聞詳細頁

        #region 頭條新聞

        /// <summary>
        /// 取得頭條新聞資料庫資料
        /// </summary>
        public IList<HeadlineNewsData> GetHeadlineNewsDbData()
        {
            string newsTypeQuery = @"
        SELECT [NewsType]
        FROM [dbo].[NewsType]
        WHERE [TypeNumber] = '2'";

            var headlineNewsType = DbManager.Custom.ExecuteIList<string>(newsTypeQuery, null, CommandType.Text);

            string query = @"
            WITH CTE AS (
                SELECT
                    nl.[NewsDate],
                    nl.[NewsTime],
                    nl.[NewsTitle],
                    nl.[NewsSerialNumber],
                    nd.[NewsDetailDate],
                    nd.[NewsContent],
                    nd.[NewsRelatedProducts],
                    nd.[NewsType],
                    ROW_NUMBER() OVER (PARTITION BY nl.[NewsSerialNumber] ORDER BY nl.[NewsDate] DESC, nl.[NewsTime] DESC) AS RowNum
                FROM
                    [dbo].[NewsList] nl
                LEFT JOIN
                    [dbo].[NewsDetail] nd
                    ON nl.[NewsSerialNumber] = nd.[NewsSerialNumber]
            )
            SELECT TOP 5
                [NewsDate],
                [NewsTime],
                [NewsTitle],
                [NewsSerialNumber],
                [NewsDetailDate],
                [NewsContent],
                [NewsRelatedProducts],
                [NewsType]
            FROM
                CTE
            WHERE
                RowNum = 1
                AND [NewsType] LIKE @HeadlineNewsType
            ORDER BY
                [NewsDate] DESC,
                [NewsTime] DESC;";

            var result = DbManager.Custom.ExecuteIList<HeadlineNewsData>(query, new { HeadlineNewsType = '%' + headlineNewsType[0] + '%' }, CommandType.Text);

            return result;
        }

        /// <summary>
        /// 整理頭條新聞資料庫資料
        /// </summary>
        public HeadlineNewsModel OrganizeHeadlineNewsDbData(List<HeadlineNewsData> _datas)
        {
            var datas = new HeadlineNewsModel();

            if (_datas != null
                && _datas.Any())
            {
                datas.LatestHeadlines = new HeadlineNewsData();
                datas.LatestHeadlines.NewsDate = _datas[0].NewsDate;
                datas.LatestHeadlines.NewsTime = _datas[0].NewsTime;
                datas.LatestHeadlines.NewsTitle = _datas[0].NewsTitle;
                datas.LatestHeadlines.NewsSerialNumber = _datas[0].NewsSerialNumber;
                datas.LatestHeadlines.NewsDetailLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + _datas[0].NewsSerialNumber;

                datas.Headlines = new List<HeadlineNewsData>();
                for (int i = 1; i < _datas.Count; i++)
                {
                    var newsData = new HeadlineNewsData();
                    newsData.NewsDate = _datas[i].NewsDate;
                    newsData.NewsTime = _datas[i].NewsTime;
                    newsData.NewsTitle = _datas[i].NewsTitle;
                    newsData.NewsSerialNumber = _datas[i].NewsSerialNumber;
                    newsData.NewsDetailLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + _datas[i].NewsSerialNumber;

                    datas.Headlines.Add(newsData);
                }
            }

            return datas;
        }

        /// <summary>
        /// 取得頭條新聞瀏覽人次
        /// </summary>
        public HeadlineNewsModel GetHeadlineNewsViewCount(HeadlineNewsModel datas)
        {
            string currentUrl;
            int? visitCount;

            if (datas != null && datas.LatestHeadlines != null
                && datas.Headlines != null && datas.Headlines.Any())
            {
                string pageItemId = MarketNewsRelatedLinkSetting.GetMarketNewsDetailPageItemId();
                string rootPath = System.Web.HttpContext.Current.Request.Url.GetLeftPart(System.UriPartial.Authority);

                currentUrl = rootPath + MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + HttpUtility.UrlEncode(datas.LatestHeadlines.NewsSerialNumber);
                visitCount = _visitCountRepository.GetVisitCount(pageItemId.ToGuid(), currentUrl);
                datas.LatestHeadlines.NewsViewCount = visitCount?.ToString("N0") ?? "0";

                for (int i = 0; i < datas.Headlines.Count; i++)
                {
                    currentUrl = rootPath + MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + HttpUtility.UrlEncode(datas.Headlines[i].NewsSerialNumber);
                    visitCount = _visitCountRepository.GetVisitCount(pageItemId.ToGuid(), currentUrl);
                    datas.Headlines[i].NewsViewCount = visitCount?.ToString("N0") ?? "0";
                }
            }

            return datas;
        }

        #endregion 頭條新聞

        #region 首頁頭條新聞

        /// <summary>
        /// 取得首頁頭條新聞資料庫資料
        /// </summary>
        public IList<HomeHeadlinesData> GetHomeHeadlinesDbData()
        {
            string newsTypeQuery = @"
        SELECT [NewsType]
        FROM [dbo].[NewsType]
        WHERE [TypeNumber] = '2'";

            var headlineNewsType = DbManager.Custom.ExecuteIList<string>(newsTypeQuery, null, CommandType.Text);

            string query = @"
            WITH CTE AS (
                SELECT
                    nl.[NewsDate],
                    nl.[NewsTime],
                    nl.[NewsTitle],
                    nl.[NewsSerialNumber],
                    nd.[NewsDetailDate],
                    nd.[NewsContent],
                    nd.[NewsRelatedProducts],
                    nd.[NewsType],
                    ROW_NUMBER() OVER (PARTITION BY nl.[NewsSerialNumber] ORDER BY nl.[NewsDate] DESC, nl.[NewsTime] DESC) AS RowNum
                FROM
                    [dbo].[NewsList] nl
                LEFT JOIN
                    [dbo].[NewsDetail] nd
                    ON nl.[NewsSerialNumber] = nd.[NewsSerialNumber]
            )
            SELECT TOP 4
                [NewsDate],
                [NewsTime],
                [NewsTitle],
                [NewsSerialNumber],
                [NewsDetailDate],
                [NewsContent],
                [NewsRelatedProducts],
                [NewsType]
            FROM
                CTE
            WHERE
                RowNum = 1
                AND [NewsType] LIKE @HeadlineNewsType
            ORDER BY
                [NewsDate] DESC,
                [NewsTime] DESC;";

            var result = DbManager.Custom.ExecuteIList<HomeHeadlinesData>(query, new { HeadlineNewsType = '%' + headlineNewsType[0] + '%' }, CommandType.Text);

            return result;
        }

        /// <summary>
        /// 整理頭條新聞資料庫資料
        /// </summary>
        public HomeHeadlinesModel OrganizeHomeHeadlinesDbData(List<HomeHeadlinesData> _datas)
        {
            var dataSource = RenderingContext.CurrentOrNull?.ContextItem;

            if (dataSource == null || dataSource.TemplateID.ToString() != Templates.HomeHeadlines.Id.ToString())
            {
                return null;
            }

            var datas = new HomeHeadlinesModel();

            datas.Datasource = dataSource;
            datas.ButtonText = ItemUtils.GetFieldValue(dataSource, Templates.HomeHeadlines.Fields.ButtonText);
            datas.ButtonLink = ItemUtils.GeneralLink(dataSource, Templates.HomeHeadlines.Fields.ButtonLink).Url;

            var imageUrlList = GetImageUrlList(dataSource);

            if (_datas != null && _datas.Any())
            {
                datas.LatestHeadlines = new HomeHeadlinesData
                {
                    NewsImage = imageUrlList.Count > 0 ? imageUrlList[0] : string.Empty,
                    NewsDate = _datas[0].NewsDate,
                    NewsTime = _datas[0].NewsTime,
                    NewsTitle = _datas[0].NewsTitle,
                    NewsSerialNumber = _datas[0].NewsSerialNumber,
                    NewsDetailLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + _datas[0].NewsSerialNumber
                };

                datas.Headlines = new List<HomeHeadlinesData>();
                for (int i = 1; i < _datas.Count; i++)
                {
                    var newsData = new HomeHeadlinesData
                    {
                        NewsImage = imageUrlList.Count > i ? imageUrlList[i] : string.Empty,
                        NewsDate = _datas[i].NewsDate,
                        NewsTime = _datas[i].NewsTime,
                        NewsTitle = _datas[i].NewsTitle,
                        NewsSerialNumber = _datas[i].NewsSerialNumber,
                        NewsDetailLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + _datas[i].NewsSerialNumber
                    };

                    datas.Headlines.Add(newsData);
                }
            }

            return datas;
        }

        /// <summary>
        ///  取得圖片連結 List
        /// </summary>
        public List<string> GetImageUrlList(Item dataSource)
        {
            var imageDatasourceId = ItemUtils.GetFieldValue(dataSource, Templates.HomeHeadlines.Fields.ImageDatasource);

            var imageDatasource = Sitecore.Context.Database.GetItem(imageDatasourceId);

            var imageSubItems = imageDatasource.Children.Take(4).ToList();

            var imageUrlList = new List<string>();

            foreach (var item in imageSubItems)
            {
                MediaItem media = new MediaItem(item);
                string mediaUrl = MediaManager.GetMediaUrl(media);
                imageUrlList.Add(mediaUrl);
            }

            return imageUrlList;
        }
        #endregion 首頁頭條新聞
    }
}
using Feature.Wealth.Component.Models.News;
using Feature.Wealth.Component.Models.News.NewsList;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Manager;
using log4net;
using Mapster;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class NewsRepository
    {
        private ILog Log { get; } = Logger.Account;
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
        ///  取得預設市場新聞資料庫資料
        /// </summary>
        public IList<MarketNewsModel> GetDefaultMarketNewsDbData(string initialStartDatetime, string initialEndDatetime)
        {
            IList<MarketNewsModel> datas = null;
            int threshold = 5; // 設定最低需要的 "頭條新聞" 數量
            int daysToShift = 2; // 每次查詢未找到時，初始往前推移的天數
            string endDatetime = initialEndDatetime;
            string startDatetime = initialStartDatetime;
            bool foundEnoughHeadlines = false; // 標記是否找到足夠的 "頭條新聞"

            // 當還未找到足夠的 "頭條新聞" 時進入迴圈
            while (!foundEnoughHeadlines)
            {
                // SQL 查詢語句
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
                            ROW_NUMBER() OVER (PARTITION BY nl.[NewsSerialNumber] ORDER BY nl.[NewsDate] DESC, nl.[NewsTime] DESC, nd.[NewsDetailDate] DESC) AS RowNum
                        FROM
                            [dbo].[NewsList] nl
                        LEFT JOIN
                            [dbo].[NewsDetail] nd
                        ON
                            nl.[NewsSerialNumber] = nd.[NewsSerialNumber]
                        WHERE
                            (nl.[NewsDate] BETWEEN @StartDate AND @EndDate) AND
                            nd.[NewsType] IS NOT NULL
                    ),
                    FilteredCTE AS (
                        SELECT
                            *,
                            ROW_NUMBER() OVER (ORDER BY [NewsDate] DESC, [NewsTime] DESC, [NewsDetailDate] DESC) AS RowNumber
                        FROM
                            CTE
                        WHERE
                            RowNum = 1
                    )
                    SELECT
                        curr.[NewsDate],
                        curr.[NewsTime],
                        curr.[NewsTitle],
                        curr.[NewsSerialNumber],
                        curr.[NewsDetailDate],
                        curr.[NewsContent],
                        curr.[NewsRelatedProducts],
                        curr.[NewsType],
                        prev.[NewsSerialNumber] AS PreviousPageId,
                        prev.[NewsTitle] AS PreviousPageTitle,
                        next.[NewsSerialNumber] AS NextPageId,
                        next.[NewsTitle] AS NextPageTitle
                    FROM FilteredCTE curr
                    LEFT JOIN FilteredCTE prev ON curr.RowNumber = prev.RowNumber + 1
                    LEFT JOIN FilteredCTE next ON curr.RowNumber = next.RowNumber - 1";

                try
                {
                    datas = DbManager.Custom.ExecuteIList<MarketNewsModel>(query, new { StartDate = startDatetime, EndDate = endDatetime }, CommandType.Text);

                    // 計算 datas 中 NewsType 為 "頭條新聞" 的數量，如果 datas 為 null，則headlineCount 為 0
                    var headlineCount = datas?.Count(news => news.NewsType != null && news.NewsType.Contains("頭條新聞")) ?? 0;

                    // 如果找到了足夠的 "頭條新聞"，則退出循環
                    if (headlineCount >= threshold)
                    {
                        // 設定標記為 true，表示已找到足夠的資料
                        foundEnoughHeadlines = true;
                    }
                    else
                    {
                        // 如果不足，繼續更新 startDatetime，往前推動更多的天數
                        daysToShift *= 2; // 每次推移的天數加倍
                        startDatetime = DateTime.Parse(endDatetime, new CultureInfo("zh-TW")).AddDays(-daysToShift).ToString("yyyy/MM/dd"); // 更新開始時間
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.Message);
                    datas = null;
                    break;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    datas = null;
                    break;
                }
            }

            return datas;
        }

        /// <summary>
        ///  取得查詢市場新聞資料庫資料
        /// </summary>
        public IList<MarketNewsModel> GetSerchMarketNewsDbData(string startDatetime, string endDatetime)
        {
            IList<MarketNewsModel> datas = null;

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
                        ROW_NUMBER() OVER (PARTITION BY nl.[NewsSerialNumber] ORDER BY nl.[NewsDate] DESC, nl.[NewsTime] DESC, nd.[NewsDetailDate] DESC) AS RowNum
                    FROM
                        [dbo].[NewsList] nl
                    LEFT JOIN
                        [dbo].[NewsDetail] nd
                    ON
                        nl.[NewsSerialNumber] = nd.[NewsSerialNumber]
                    WHERE
                        (nl.[NewsDate] BETWEEN @StartDate AND @EndDate) AND
                        nd.[NewsType] IS NOT NULL
                ),
                FilteredCTE AS (
                    SELECT
                        *,
                        ROW_NUMBER() OVER (ORDER BY [NewsDate] DESC, [NewsTime] DESC, [NewsDetailDate] DESC) AS RowNumber
                    FROM
                        CTE
                    WHERE
                        RowNum = 1
                )
                SELECT
                    curr.[NewsDate],
                    curr.[NewsTime],
                    curr.[NewsTitle],
                    curr.[NewsSerialNumber],
                    curr.[NewsDetailDate],
                    curr.[NewsContent],
                    curr.[NewsRelatedProducts],
                    curr.[NewsType],
                    prev.[NewsSerialNumber] AS PreviousPageId,
                    prev.[NewsTitle] AS PreviousPageTitle,
                    next.[NewsSerialNumber] AS NextPageId,
                    next.[NewsTitle] AS NextPageTitle
                FROM FilteredCTE curr
                LEFT JOIN FilteredCTE prev ON curr.RowNumber = prev.RowNumber + 1
                LEFT JOIN FilteredCTE next ON curr.RowNumber = next.RowNumber - 1";

            try
            {
                datas = DbManager.Custom.ExecuteIList<MarketNewsModel>(query, new { StartDate = startDatetime, EndDate = endDatetime }, CommandType.Text);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                datas = null;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                datas = null;
            }

            return datas;
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

                try
                {
                    var newsTypeList = DbManager.Custom.ExecuteIList<string>(query, new { IdList = idList }, CommandType.Text);

                    filteredDatas = filteredDatas
                        .Where(news =>
                            !string.IsNullOrEmpty(news.NewsType) &&
                            news.NewsType.Split(',').Any(type => newsTypeList.Contains(type.Trim())))
                        .ToList();
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.Message);
                    return datas;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return datas;
                }
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
                    NewsDetailLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + item.NewsSerialNumber,
                    NewsDetailDate = item.NewsDetailDate.ToString(),
                    NewsContent = item.NewsContent,
                    NewsRelatedProducts = item.NewsRelatedProducts,
                    PreviousPageTitle = item.PreviousPageTitle,
                    PreviousPageId = item.PreviousPageId,
                    NextPageId = item.NextPageId,
                    NextPageTitle = item.NextPageTitle,
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
        ///  取得市場新聞詳細頁資料
        /// </summary>
        public MarketNewsDetailModel GetMarketNewsDetailData(List<MarketNewsModel> _datas, string newsId)
        {
            var model = new MarketNewsDetailModel();
            var detailData = new MarketNewsDetailData();

            if (_datas != null && _datas.Any())
            {
                // 從預設 cache 裡拿資料篩選出符合 newsId 的資料
                var filteredData = _datas.FirstOrDefault(news => news.NewsSerialNumber == newsId);

                // 確認 filteredData 是否是最後一筆資料
                bool isLastItem = filteredData != null && _datas.IndexOf(filteredData) == _datas.Count - 1;

                // 判斷查詢的新聞是否有在預設的 cache 裡面且不是預設 cache 的最後一筆資料
                if (filteredData != null && !isLastItem)
                {
                    detailData.NewsSerialNumber = filteredData.NewsSerialNumber;
                    detailData.NewsDetailDate = filteredData.NewsDetailDate;
                    detailData.NewsTitle = filteredData.NewsTitle;
                    detailData.NewsContent = filteredData.NewsContent;
                    detailData.NewsContentHtmlString = new HtmlString(filteredData.NewsContent);
                    detailData.NewsType = filteredData.NewsType;
                    detailData.PreviousPageId = filteredData.PreviousPageId;
                    detailData.PreviousPageTitle = filteredData.PreviousPageTitle;
                    detailData.PreviousPageLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + HttpUtility.UrlEncode(filteredData.PreviousPageId);
                    detailData.NextPageId = filteredData.NextPageId;
                    detailData.NextPageTitle = filteredData.NextPageTitle;
                    detailData.NextPageLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + HttpUtility.UrlEncode(filteredData.NextPageId);
                    detailData.NewsListUrl = MarketNewsRelatedLinkSetting.GetMarketNewsListUrl();

                    model.MarketNewsDetailData = detailData;
                }
                else
                {
                    var datas = GetMarketNewsDbDetailData(newsId);

                    if (datas != null)
                    {
                        detailData.NewsSerialNumber = datas.NewsSerialNumber;
                        detailData.NewsDetailDate = datas.NewsDetailDate;
                        detailData.NewsTitle = datas.NewsTitle;
                        detailData.NewsContent = datas.NewsContent;
                        detailData.NewsContentHtmlString = new HtmlString(datas.NewsContent);
                        detailData.NewsType = datas.NewsType;
                        detailData.PreviousPageId = datas.PreviousPageId;
                        detailData.PreviousPageTitle = datas.PreviousPageTitle;
                        detailData.PreviousPageLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + HttpUtility.UrlEncode(datas.PreviousPageId);
                        detailData.NextPageId = datas.NextPageId;
                        detailData.NextPageTitle = datas.NextPageTitle;
                        detailData.NextPageLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + HttpUtility.UrlEncode(datas.NextPageId);
                        detailData.NewsListUrl = MarketNewsRelatedLinkSetting.GetMarketNewsListUrl();

                        model.MarketNewsDetailData = detailData;
                    }
                    else
                    {
                        model.MarketNewsDetailData = null;
                    }
                }
            }
            else
            {
                model.MarketNewsDetailData = null;
            }

            return model;
        }

        /// <summary>
        ///  取得市場新聞詳細頁資料庫資料
        /// </summary>
        public MarketNewsDetailData GetMarketNewsDbDetailData(string newsId)
        {
            string serchQuery = "SELECT COUNT(*) FROM [dbo].[NewsDetail] WHERE [NewsSerialNumber] = @NewsSerialNumber";

            int count = 0;

            try
            {
                count = DbManager.Custom.ExecuteScalar<int>(serchQuery, new { NewsSerialNumber = newsId }, CommandType.Text);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                count = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                count = 0;
            }

            bool dbDataExists = count > 0;

            if (dbDataExists)
            {
                DateTime newsDate;

                string getDateQuery = "SELECT [NewsDate] FROM [dbo].[NewsList] WHERE [NewsSerialNumber] = @NewsSerialNumber";

                try
                {
                    newsDate = DbManager.Custom.ExecuteScalar<DateTime>(getDateQuery, new { NewsSerialNumber = newsId }, CommandType.Text);
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return null;
                }

                // 計算日期範圍
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
                            ROW_NUMBER() OVER (PARTITION BY nl.[NewsSerialNumber] ORDER BY nl.[NewsDate] DESC, nl.[NewsTime] DESC, nd.[NewsDetailDate] DESC) AS RowNum
                        FROM
                            [dbo].[NewsList] nl
                        LEFT JOIN
                            [dbo].[NewsDetail] nd
                        ON
                            nl.[NewsSerialNumber] = nd.[NewsSerialNumber]
                        WHERE
                            nl.[NewsDate] BETWEEN @StartDate AND @EndDate
                            AND nd.[NewsType] IS NOT NULL
                    ),
                    FilteredCTE AS (
                        SELECT
                            *,
                            ROW_NUMBER() OVER (ORDER BY [NewsDate] DESC, [NewsTime] DESC, [NewsDetailDate] DESC) AS RowNumber
                        FROM
                            CTE
                        WHERE
                            RowNum = 1
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
                    FROM FilteredCTE curr
                    LEFT JOIN FilteredCTE prev ON curr.RowNumber = prev.RowNumber + 1
                    LEFT JOIN FilteredCTE next ON curr.RowNumber = next.RowNumber - 1
                    WHERE curr.[NewsSerialNumber] = @NewsId";

                MarketNewsDetailData result = null;

                try
                {
                    result = DbManager.Custom.Execute<MarketNewsDetailData>(query, new
                    {
                        NewsId = newsId,
                        StartDate = startDate,
                        EndDate = endDate
                    }, CommandType.Text);
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return null;
                }

                return result;
            }

            return null;
        }

        #endregion 市場新聞詳細頁

        #region 頭條新聞

        /// <summary>
        /// 取得頭條新聞資料
        /// </summary>
        public HeadlineNewsModel GetHeadlineNewsData(List<MarketNewsModel> _datas)
        {
            var datas = new HeadlineNewsModel();

            if (_datas != null && _datas.Any())
            {
                string newsTypeQuery = @"
                SELECT [NewsType]
                FROM [dbo].[NewsType]
                WHERE [TypeNumber] = '2'";

                IList<string> headlineNewsTypes = null;

                try
                {
                    headlineNewsTypes = DbManager.Custom.ExecuteIList<string>(newsTypeQuery, null, CommandType.Text);
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.Message);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }

                var filteredData = _datas
                    .Where(news => news.NewsType != null && headlineNewsTypes != null && headlineNewsTypes.Any(ht => news.NewsType.Contains(ht)))
                    .Take(5)
                    .ToList();

                if (filteredData.Any())
                {
                    datas.LatestHeadlines = new HeadlineNewsData
                    {
                        NewsDate = filteredData[0].NewsDate,
                        NewsTime = filteredData[0].NewsTime,
                        NewsTitle = filteredData[0].NewsTitle,
                        NewsSerialNumber = filteredData[0].NewsSerialNumber,
                        NewsDetailLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + filteredData[0].NewsSerialNumber
                    };

                    datas.Headlines = new List<HeadlineNewsData>();
                    for (int i = 1; i < filteredData.Count; i++)
                    {
                        var newsData = new HeadlineNewsData
                        {
                            NewsDate = filteredData[i].NewsDate,
                            NewsTime = filteredData[i].NewsTime,
                            NewsTitle = filteredData[i].NewsTitle,
                            NewsSerialNumber = filteredData[i].NewsSerialNumber,
                            NewsDetailLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + filteredData[i].NewsSerialNumber
                        };

                        datas.Headlines.Add(newsData);
                    }
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
            string pageItemId = MarketNewsRelatedLinkSetting.GetMarketNewsDetailPageItemId();
            string rootPath = System.Web.HttpContext.Current.Request.Url.GetLeftPart(System.UriPartial.Authority);

            if (datas != null && datas.LatestHeadlines != null)
            {
                currentUrl = rootPath + MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + HttpUtility.UrlEncode(datas.LatestHeadlines.NewsSerialNumber);
                visitCount = _visitCountRepository.GetVisitCount(pageItemId.ToGuid(), currentUrl);
                datas.LatestHeadlines.NewsViewCount = visitCount?.ToString("N0") ?? "0";
            }

            if (datas != null && datas.Headlines != null && datas.Headlines.Any())
            {
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
        /// 取得首頁焦點新聞資料
        /// </summary>
        public HomeHeadlinesModel GetHomeHeadlinesData(List<MarketNewsModel> _datas)
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
                string newsTypeQuery = @"
                SELECT [NewsType]
                FROM [dbo].[NewsType]
                WHERE [TypeNumber] = '2'";

                IList<string> homeHeadlinesType = null;

                try
                {
                    homeHeadlinesType = DbManager.Custom.ExecuteIList<string>(newsTypeQuery, null, CommandType.Text);
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.Message);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }

                var filteredData = _datas
                    .Where(news => news.NewsType != null && homeHeadlinesType != null && homeHeadlinesType.Any(ht => news.NewsType.Contains(ht)))
                    .Take(4)
                    .ToList();

                if (filteredData.Any())
                {
                    datas.LatestHeadlines = new HomeHeadlinesData
                    {
                        NewsImage = imageUrlList.Count > 0 ? imageUrlList[0] : string.Empty,
                        NewsDate = filteredData[0].NewsDate,
                        NewsTime = filteredData[0].NewsTime,
                        NewsTitle = filteredData[0].NewsTitle,
                        NewsSerialNumber = filteredData[0].NewsSerialNumber,
                        NewsDetailLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + filteredData[0].NewsSerialNumber
                    };

                    datas.Headlines = new List<HomeHeadlinesData>();
                    for (int i = 1; i < filteredData.Count; i++)
                    {
                        var newsData = new HomeHeadlinesData
                        {
                            NewsImage = imageUrlList.Count > i ? imageUrlList[i] : string.Empty,
                            NewsDate = filteredData[i].NewsDate,
                            NewsTime = filteredData[i].NewsTime,
                            NewsTitle = filteredData[i].NewsTitle,
                            NewsSerialNumber = filteredData[i].NewsSerialNumber,
                            NewsDetailLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + filteredData[i].NewsSerialNumber
                        };

                        datas.Headlines.Add(newsData);
                    }
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
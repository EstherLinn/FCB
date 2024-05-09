using Feature.Wealth.Component.Models.MarketNews;
using Feature.Wealth.Component.Models.News;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class NewsController : Controller
    {
        private readonly DjMoneyApiRespository _djMoneyApiRespository = new DjMoneyApiRespository();
        private readonly NewsRepository _newsRespository = new NewsRepository();
        private readonly ViewCountRepository _viewCountRepository = new ViewCountRepository();

        public ActionResult NewsDetails()
        {
            var model = new NewsModel(RenderingContext.CurrentOrNull?.ContextItem);
            return View("/Views/Feature/Wealth/Component/News/NewsDetails.cshtml", model);
        }

        public ActionResult MarketNewsSearch()
        {
            return View("/Views/Feature/Wealth/Component/News/MarketNewsSearch.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> GetMarketNewsData(string id, string count, string startDatetime, string endDatetime)
        {
            var datas = new List<MarketNewsModel>();

            var resp = await _djMoneyApiRespository.GetMarketNewsData(id, count, startDatetime, endDatetime);

            if (resp != null
                && resp.ContainsKey("resultSet")
                && resp["resultSet"] != null
                && resp["resultSet"]["result"] != null
                && resp["resultSet"]["result"].Any())
            {
                var resultList = resp["resultSet"]["result"];

                string pageItemId = _newsRespository.GetMarketNewsDetailsPageItemId();
                string rootPath = ControllerContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);

                foreach (var item in resultList)
                {
                    var newsData = new MarketNewsModel();

                    // 若是頭條新聞 HotNews 為 string.Empty，若不是則為 "u-invisible"
                    newsData.HotNews = item["v5"].ToString() == "頭條新聞" ? string.Empty : "u-invisible";
                    newsData.NewsType = item["v5"].ToString();
                    newsData.NewsDate = item["v1"].ToString() + " " + item["v2"].ToString();
                    newsData.NewsTitle = _newsRespository.FullWidthToHalfWidth(item["v3"].ToString());
                    newsData.NewsSerialNumber = item["v4"].ToString();
                    newsData.NewsDetailLink = _newsRespository.GetMarketNewsDetailsUrl() + "?id=" + item["v4"].ToString();

                    var currentUrl = rootPath + _newsRespository.GetMarketNewsDetailsUrl() + "?id=" + item["v4"].ToString();
                    newsData.NewsViewCount = _viewCountRepository.GetViewCountInfo(pageItemId, currentUrl).ToString("N0");

                    newsData.Data = new MarketNewsData
                    {
                        Type = item["v5"].ToString(),
                        IsLogin = false,
                        IsNews = false,
                        IsLike = false,
                        DetailUrl = _newsRespository.GetMarketNewsDetailsUrl() + "?id=" + item["v4"].ToString(),
                        Purchase = false
                    };

                    newsData.value = _newsRespository.FullWidthToHalfWidth(item["v3"].ToString());

                    datas.Add(newsData);
                }
            }

            return new JsonNetResult(datas);
        }

        public ActionResult MarketNewsDetail()
        {
            return View("/Views/Feature/Wealth/Component/News/MarketNewsDetail.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> GetMarketNewsDetailData(string newsId, string pageItemId, string currentUrl)
        {
            var resp = await _djMoneyApiRespository.GetMarketNewsDetailData(newsId);

            var model = new MarketNewsDetailModel();

            var detailData = new MarketNewsDetailData();

            if (resp != null
                && resp.ContainsKey("resultSet")
                && resp["resultSet"] != null
                && resp["resultSet"]["result"] != null
                && resp["resultSet"]["result"].Any())
            {
                var result = resp["resultSet"]["result"][0];

                // detailData.NewsType = "台股";
                detailData.NewsDate = result["v1"].ToString();
                detailData.NewsTitle = result["v2"].ToString();
                detailData.NewsContent = _newsRespository.FullWidthToHalfWidth(result["v3"].ToString());
                detailData.NewsRelatedProducts = result["v4"].ToString();
                detailData.NewsViewCount = _viewCountRepository.UpdateViewCountInfo(pageItemId, currentUrl).ToString("N0");
            }

            model.MarketNewsDetailData = detailData;

            return new JsonNetResult(model);
        }

        public ActionResult HeadlineNews()
        {
            return View("/Views/Feature/Wealth/Component/News/HeadlineNews.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> GetHeadlineNewsData()
        {
            var datas = new HeadlineNewsModel();

            var startDatetime = Sitecore.DateUtil.ToServerTime(DateTime.UtcNow).ToString("yyyy-MM-dd");
            var endDatetime = Sitecore.DateUtil.ToServerTime(DateTime.UtcNow).ToString("yyyy-MM-dd");

            var resp = await _djMoneyApiRespository.GetMarketNewsData("2", "5", startDatetime, endDatetime);

            if (resp != null
                && resp.ContainsKey("resultSet")
                && resp["resultSet"] != null
                && resp["resultSet"]["result"] != null
                && resp["resultSet"]["result"].Any())
            {
                var resultList = resp["resultSet"]["result"];

                string pageItemId = _newsRespository.GetMarketNewsDetailsPageItemId();
                string rootPath = ControllerContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);

                datas.LatestHeadlines = new HeadlineNewsData();
                datas.LatestHeadlines.NewsDate = resultList[0]["v1"].ToString() + " " + resultList[0]["v2"].ToString();
                datas.LatestHeadlines.NewsTitle = _newsRespository.FullWidthToHalfWidth(resultList[0]["v3"].ToString());
                datas.LatestHeadlines.NewsSerialNumber = resultList[0]["v4"].ToString();
                datas.LatestHeadlines.NewsDetailLink = _newsRespository.GetMarketNewsDetailsUrl() + "?id=" + resultList[0]["v4"].ToString();
                var currentUrl = rootPath + _newsRespository.GetMarketNewsDetailsUrl() + "?id=" + resultList[0]["v4"].ToString();
                datas.LatestHeadlines.NewsViewCount = _viewCountRepository.GetViewCountInfo(pageItemId, currentUrl).ToString("N0");

                datas.Headlines = new List<HeadlineNewsData>();
                for (int i = 1; i < resultList.Count(); i++)
                {
                    var newsData = new HeadlineNewsData();
                    newsData.NewsDate = resultList[i]["v1"].ToString() + " " + resultList[i]["v2"].ToString();
                    newsData.NewsTitle = _newsRespository.FullWidthToHalfWidth(resultList[i]["v3"].ToString());
                    newsData.NewsSerialNumber = resultList[i]["v4"].ToString();
                    newsData.NewsDetailLink = _newsRespository.GetMarketNewsDetailsUrl() + "?id=" + resultList[i]["v4"].ToString();
                    currentUrl = rootPath + _newsRespository.GetMarketNewsDetailsUrl() + "?id=" + resultList[i]["v4"].ToString();
                    newsData.NewsViewCount = _viewCountRepository.GetViewCountInfo(pageItemId, currentUrl).ToString("N0");
                    datas.Headlines.Add(newsData);
                }
            }

            return new JsonNetResult(datas);
        }
    }
}
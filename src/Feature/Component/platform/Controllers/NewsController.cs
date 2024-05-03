using Feature.Wealth.Component.Models.MarketNews;
using Feature.Wealth.Component.Models.News;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class NewsController : Controller
    {
        private readonly DjMoneyApiRespository _djMoneyApiRespository = new DjMoneyApiRespository();
        private readonly NewsRepository _articleFilterListRespository = new NewsRepository();
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
                && resp["resultSet"]["result"] != null)
            {
                var resultList = resp["resultSet"]["result"];

                string pageItemId = _articleFilterListRespository.GetMarketNewsDetailsPageItemId();
                string rootPath = ControllerContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);

                bool isFirstItem = true;

                foreach (var item in resultList)
                {
                    var newsData = new MarketNewsModel();

                    // 若是頭條新聞 HotNews 為 string.Empty，若不是則為 "u-invisible"
                    newsData.HotNews = isFirstItem ? string.Empty : "u-invisible";
                    newsData.NewsType = isFirstItem ? "頭條新聞" : "台股";
                    newsData.NewsDate = isFirstItem ? "2024/03/01" : item["v1"].ToString();
                    isFirstItem = false;

                    newsData.NewsTime = item["v2"].ToString();
                    newsData.NewsTitle = _articleFilterListRespository.FullWidthToHalfWidth(item["v3"].ToString());
                    newsData.NewsSerialNumber = item["v4"].ToString();
                    newsData.NewsDetailLink = _articleFilterListRespository.GetMarketNewsDetailsUrl() + "?id=" + item["v4"].ToString();

                    var currentUrl = rootPath + _articleFilterListRespository.GetMarketNewsDetailsUrl() + "?id=" + item["v4"].ToString();
                    newsData.NewsViewCount = _viewCountRepository.GetViewCountInfo(pageItemId, currentUrl).ToString("N0");

                    newsData.Data = new MarketNewsData
                    {
                        Type = string.Empty,
                        IsLogin = false,
                        IsNews = false,
                        IsLike = false,
                        DetailUrl = _articleFilterListRespository.GetMarketNewsDetailsUrl() + "?id=" + item["v4"].ToString(),
                        Purchase = false
                    };

                    newsData.value = _articleFilterListRespository.FullWidthToHalfWidth(item["v3"].ToString());

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

            var newsDetailData = new MarketNewsDetailModel();

            if (resp != null
                && resp.ContainsKey("resultSet")
                && resp["resultSet"] != null
                && resp["resultSet"]["result"] != null)
            {
                var result = resp["resultSet"]["result"][0];

                newsDetailData.NewsType = "台股";
                newsDetailData.NewsDate = result["v1"].ToString();
                newsDetailData.NewsTitle = result["v2"].ToString();
                newsDetailData.NewsContent = result["v3"].ToString();
                newsDetailData.NewsRelatedProducts = result["v4"].ToString();
                newsDetailData.NewsViewCount = _viewCountRepository.UpdateViewCountInfo(pageItemId, currentUrl).ToString("N0");
            }

            return new JsonNetResult(newsDetailData);
        }
    }
}

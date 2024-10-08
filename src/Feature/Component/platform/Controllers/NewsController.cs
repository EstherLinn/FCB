using Feature.Wealth.Component.Models.News;
using Feature.Wealth.Component.Models.News.NewsList;
using Feature.Wealth.Component.Repositories;
using Sitecore.Configuration;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsRepository _newsRespository = new NewsRepository();
        private readonly CommonRepository _commonRespository = new CommonRepository();

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string MarketNewsCacheKey = $"Fcb_MarketNewsCache";
        private readonly string MarketNewsDetailCacheKey = $"Fcb_MarketNewsDetailCache_NewsId=";
        private readonly string HeadlineNewsCacheKey = $"Fcb_HeadlineNewsCache";
        private readonly string HomeHeadlinesCacheKey = $"Fcb_HomeHeadlinesCache";
        private readonly string cacheTime = Settings.GetSetting("NewsCacheTime");
        private readonly string homeHeadlinesTime = Settings.GetSetting("HomeHeadlinesTime");

        public ActionResult NewsDetails()
        {
            var model = new NewsModel(RenderingContext.CurrentOrNull?.ContextItem);
            return View("/Views/Feature/Wealth/Component/News/NewsDetails.cshtml", model);
        }

        public ActionResult NewsList()
        {
            var model = _newsRespository.GetNewsListViewModel(RenderingContext.Current.Rendering.DataSource);
            return View("/Views/Feature/Wealth/Component/News/NewsList.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetNewsListResultData(ReqNewsList req)
        {
            var resp = _newsRespository.GetNewsListResult(req);
            return new JsonNetResult(resp);
        }

        public ActionResult MarketNewsSearch()
        {
            return View("/Views/Feature/Wealth/Component/News/MarketNewsSearch.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetMarketNewsData(string id, string startDatetime, string endDatetime)
        {
            List<MarketNewsModel> _datas;
            List<MarketNewsModel> datas;

            // 取得 MarketNewsCache 資料
            _datas = (List<MarketNewsModel>)_cache.Get(MarketNewsCacheKey);

            if (_datas == null)
            {
                // 取得 MarketNews 資料庫資料
                _datas = (List<MarketNewsModel>)_newsRespository.GetMarketNewsDbData();
                if (_datas != null && _datas.Any())
                {
                    // 儲存 MarketNewsCache
                    _cache.Set(MarketNewsCacheKey, _datas, _commonRespository.GetCacheExpireTime(cacheTime));
                }
            }

            // 整理 MarketNews 資料庫資料
            datas = _newsRespository.OrganizeMarketNewsDbData(_datas, id, startDatetime, endDatetime);

            // 取得 MarketNews 瀏覽次數
            datas = _newsRespository.GetMarketNewsViewCount(datas);

            return new JsonNetResult(datas);
        }

        public ActionResult MarketNewsDetail()
        {
            string newsId = HttpUtility.UrlDecode(Request.QueryString["id"]);

            MarketNewsDetailModel datas;

            // 取得 MarketNewsDetailCache 資料
            datas = (MarketNewsDetailModel)_cache.Get(MarketNewsDetailCacheKey + newsId);

            if (datas == null)
            {
                // 取得 MarketNewsDetail 資料庫資料
                var dbData = _newsRespository.GetMarketNewsDbDetailData(newsId);

                // 整理 MarketNewsDetail 資料
                datas = _newsRespository.OrganizeMarketNewsDetailDbData(dbData);

                // 儲存 MarketNewsDetailCache
                _cache.Set(MarketNewsDetailCacheKey + newsId, datas, _commonRespository.GetCacheExpireTime(cacheTime));
            }

            return View("/Views/Feature/Wealth/Component/News/MarketNewsDetail.cshtml", datas);
        }

        public ActionResult HeadlineNews()
        {
            HeadlineNewsModel datas;

            // 取得 MarketNewsCache 資料
            datas = (HeadlineNewsModel)_cache.Get(HeadlineNewsCacheKey);

            if (datas == null)
            {
                // 取得 HeadlineNews 資料庫資料
                var _datas = (List<HeadlineNewsData>)_newsRespository.GetHeadlineNewsDbData();

                // 整理 HeadlineNews 資料庫資料
                datas = _newsRespository.OrganizeHeadlineNewsDbData(_datas);
                if (datas.LatestHeadlines != null && datas.Headlines.Count == 4)
                {
                    // 儲存 HeadlineNewsCache
                    _cache.Set(HeadlineNewsCacheKey, datas, _commonRespository.GetCacheExpireTime(cacheTime));
                }
            }

            // 取得 HeadlineNews 瀏覽人次
            datas = _newsRespository.GetHeadlineNewsViewCount(datas);

            return View("/Views/Feature/Wealth/Component/News/HeadlineNews.cshtml", datas);
        }

        public ActionResult Headlines()
        {
            var item = RenderingContext.Current.Rendering.Item;
            return View("/Views/Feature/Wealth/Component/News/Headline.cshtml", new HeadlineModel(item));
        }

        public ActionResult HomeHeadlines()
        {
            HomeHeadlinesModel datas;

            // 取得 HomeHeadlinesCache 資料
            datas = (HomeHeadlinesModel)_cache.Get(HomeHeadlinesCacheKey);

            if (datas == null)
            {
                // 取得 HomeHeadlines 資料庫資料
                var _datas = (List<HomeHeadlinesData>)_newsRespository.GetHomeHeadlinesDbData();

                // 整理 HomeHeadlines 資料庫資料
                datas = _newsRespository.OrganizeHomeHeadlinesDbData(_datas);

                if (datas.LatestHeadlines != null && datas.Headlines.Count == 3)
                {
                    // 儲存 HomeHeadlinesCache
                    _cache.Set(HomeHeadlinesCacheKey, datas, _commonRespository.GetCacheExpireTime(homeHeadlinesTime));
                }                   
            }

            return View("/Views/Feature/Wealth/Component/News/HomeHeadlines.cshtml", datas);
        }
    }
}
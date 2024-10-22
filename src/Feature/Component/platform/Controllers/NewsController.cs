using Feature.Wealth.Component.Models.News;
using Feature.Wealth.Component.Models.News.NewsList;
using Feature.Wealth.Component.Repositories;
using Sitecore.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly string defaultCacheTime = Settings.GetSetting("DefaultNewsCacheTime");
        private readonly string serchCacheTime = Settings.GetSetting("SerchNewsCacheTime");

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

            startDatetime = DateTime.Parse(startDatetime, new CultureInfo("zh-TW")).ToString("yyyy/MM/dd");
            endDatetime = DateTime.Parse(endDatetime, new CultureInfo("zh-TW")).ToString("yyyy/MM/dd");
            string today = DateTime.Now.ToString("yyyy/MM/dd");
            string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");

            string cacheKey = startDatetime + "~" + endDatetime + " " + MarketNewsCacheKey;

            // 取得 MarketNewsCache 資料
            _datas = (List<MarketNewsModel>)_cache.Get(cacheKey);

            if (_datas == null)
            {
                if (startDatetime == yesterday && endDatetime == today)
                {
                    // 取得預設 MarketNews 資料庫資料
                    _datas = (List<MarketNewsModel>)_newsRespository.GetDefaultMarketNewsDbData(yesterday, today);

                    if (_datas != null && _datas.Any())
                    {
                        // 儲存預設 MarketNewsCache
                        _cache.Set(cacheKey, _datas, _commonRespository.GetCacheExpireTime(defaultCacheTime));
                    }
                }
                else
                {
                    // 取得查詢 MarketNews 資料庫資料
                    _datas = (List<MarketNewsModel>)_newsRespository.GetSerchMarketNewsDbData(startDatetime, endDatetime);

                    if (_datas != null && _datas.Any())
                    {
                        // 儲存查詢 MarketNewsCache
                        _cache.Set(cacheKey, _datas, _commonRespository.GetCacheExpireTime(serchCacheTime));
                    }

                    // 判斷是否有 NewsDate 等於今天的資料
                    bool hasTodayNews = _datas.Any(news => news.NewsDate == today);

                    if (hasTodayNews)
                    {
                        string defaultCacheKey = yesterday + "~" + today + " " + MarketNewsCacheKey;

                        // 取得預設 MarketNewsCache
                        var defaultDatas = (List<MarketNewsModel>)_cache.Get(defaultCacheKey);

                        if (defaultDatas == null)
                        {
                            // 儲存預設 MarketNewsCache
                            _cache.Set(cacheKey, _datas, _commonRespository.GetCacheExpireTime(defaultCacheTime));
                        }
                        else
                        {
                            // 過濾 _datas 中 NewsDate 等於今天的資料
                            var todayNews = _datas.Where(news => news.NewsDate == today).ToList();

                            if (todayNews.Any())
                            {
                                // 將 todayNews 合併到 defaultDatas 裡
                                defaultDatas.AddRange(todayNews);

                                // 去重，防止相同的 NewsSerialNumber 重複
                                defaultDatas = defaultDatas
                                    .GroupBy(news => news.NewsSerialNumber)
                                    .Select(g => g.First())
                                    .OrderByDescending(news => news.NewsDate)      // 先按 NewsDate 降序排序
                                    .ThenByDescending(news => news.NewsTime)       // 再按 NewsTime 降序排序
                                    .ThenByDescending(news => news.NewsDetailDate) // 最後按 NewsDetailDate 降序排序
                                    .ToList();

                                _cache.Set(defaultCacheKey, defaultDatas, _commonRespository.GetCacheExpireTime(defaultCacheTime));
                            }
                        }
                    }
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

            List<MarketNewsModel> _datas;
            MarketNewsDetailModel datas;

            string today = DateTime.Now.ToString("yyyy/MM/dd");
            string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");

            string cacheKey = yesterday + "~" + today + " " + MarketNewsCacheKey;

            // 取得 MarketNewsCache 資料
            _datas = (List<MarketNewsModel>)_cache.Get(cacheKey);

            if (_datas == null)
            {
                // 取得預設 MarketNews 資料庫資料
                _datas = (List<MarketNewsModel>)_newsRespository.GetDefaultMarketNewsDbData(yesterday, today);

                if (_datas != null && _datas.Any())
                {
                    // 儲存 MarketNewsCache
                    _cache.Set(cacheKey, _datas, _commonRespository.GetCacheExpireTime(defaultCacheTime));
                }
            }

            // 整理 MarketNewsDetail 資料
            datas = _newsRespository.GetMarketNewsDetailData(_datas, newsId);

            return View("/Views/Feature/Wealth/Component/News/MarketNewsDetail.cshtml", datas);
        }

        public ActionResult HeadlineNews()
        {
            List<MarketNewsModel> _datas;
            HeadlineNewsModel datas;

            string today = DateTime.Now.ToString("yyyy/MM/dd");
            string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");

            string cacheKey = yesterday + "~" + today + " " + MarketNewsCacheKey;

            // 取得 MarketNewsCache 資料
            _datas = (List<MarketNewsModel>)_cache.Get(cacheKey);

            if (_datas == null)
            {
                // 取得預設 MarketNews 資料庫資料
                _datas = (List<MarketNewsModel>)_newsRespository.GetDefaultMarketNewsDbData(yesterday, today);

                if (_datas != null && _datas.Any())
                {
                    // 儲存 MarketNewsCache
                    _cache.Set(cacheKey, _datas, _commonRespository.GetCacheExpireTime(defaultCacheTime));
                }
            }

            // 取得 HeadlineNews 資料
            datas = _newsRespository.GetHeadlineNewsData(_datas);

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
            List<MarketNewsModel> _datas;
            HomeHeadlinesModel datas;

            string today = DateTime.Now.ToString("yyyy/MM/dd");
            string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");

            string cacheKey = yesterday + "~" + today + " " + MarketNewsCacheKey;

            // 取得 MarketNewsCache 資料
            _datas = (List<MarketNewsModel>)_cache.Get(cacheKey);

            if (_datas == null)
            {
                // 取得預設 MarketNews 資料庫資料
                _datas = (List<MarketNewsModel>)_newsRespository.GetDefaultMarketNewsDbData(yesterday, today);

                if (_datas != null && _datas.Any())
                {
                    // 儲存 MarketNewsCache
                    _cache.Set(cacheKey, _datas, _commonRespository.GetCacheExpireTime(defaultCacheTime));
                }
            }

            // 取得 HomeHeadlines 資料
            datas = _newsRespository.GetHomeHeadlinesData(_datas);

            return View("/Views/Feature/Wealth/Component/News/HomeHeadlines.cshtml", datas);
        }
    }
}
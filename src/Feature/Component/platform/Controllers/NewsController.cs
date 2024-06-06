﻿using Feature.Wealth.Component.Models.MarketNews;
using Feature.Wealth.Component.Models.News;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsRepository _newsRespository = new NewsRepository();

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string MarketNewsCacheKey = $"Fcb_MarketNewsCache";
        private readonly string MarketNewsDetailCacheKey = $"Fcb_MarketNewsDetailCache_NewsId=";
        private readonly string HeadlineNewsCacheKey = $"Fcb_HeadlineNewsCache";

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

                // 儲存 MarketNewsCache 一小時
                _cache.Set(MarketNewsCacheKey, _datas, DateTimeOffset.Now.AddMinutes(60));
            }

            // 整理 MarketNews 資料庫資料
            datas = _newsRespository.OrganizeMarketNewsDbData(_datas, id, startDatetime, endDatetime);

            // 取得 MarketNews 瀏覽次數
            datas = _newsRespository.GetMarketNewsViewCount(datas);

            return new JsonNetResult(datas);
        }

        public ActionResult MarketNewsDetail()
        {
            return View("/Views/Feature/Wealth/Component/News/MarketNewsDetail.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> GetMarketNewsDetailData(string newsId, string pageItemId, string currentUrl)
        {
            MarketNewsDetailModel datas;

            // 取得 MarketNewsDetailCache 資料
            datas = (MarketNewsDetailModel)_cache.Get(MarketNewsDetailCacheKey + newsId);

            if (datas == null)
            {
                // 取得 MarketNewsDetail 資料庫資料
                var dbData = _newsRespository.GetMarketNewsDbDetailData(newsId);

                // 整理 MarketNewsDetail 資料
                datas = _newsRespository.OrganizeMarketNewsDetailDbData(dbData);

                // 儲存 MarketNewsDetailCache 一小時
                _cache.Set(MarketNewsDetailCacheKey + newsId, datas, DateTimeOffset.Now.AddMinutes(60));
            }

            // 取得 MarketNewsDetail 瀏覽次數
            datas = await _newsRespository.GetMarketNewsDetailViewCount(datas, newsId, pageItemId, currentUrl);

            return new JsonNetResult(datas);
        }

        public ActionResult HeadlineNews()
        {
            return View("/Views/Feature/Wealth/Component/News/HeadlineNews.cshtml");
        }

        [HttpPost]
        public ActionResult GetHeadlineNewsData()
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

                // 儲存 HeadlineNewsCache 一小時
                _cache.Set(HeadlineNewsCacheKey, datas, DateTimeOffset.Now.AddMinutes(60));
            }

            // 取得 HeadlineNews 瀏覽人次
            datas = _newsRespository.GetHeadlineNewsViewCount(datas);

            return new JsonNetResult(datas);
        }
    }
}
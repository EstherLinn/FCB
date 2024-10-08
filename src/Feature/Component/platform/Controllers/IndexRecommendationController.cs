using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.IndexRecommendation;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Configuration;
using System.Runtime.Caching;

namespace Feature.Wealth.Component.Controllers
{
    public class IndexRecommendationController : Controller
    {
        private readonly IndexRecommendationRepository _repository = new IndexRecommendationRepository();
        private readonly MemoryCache _cache = MemoryCache.Default;
        private static readonly object CacheLock = new object();
        private readonly string FundsCacheKey = $"Fcb_FundsCache";
        private readonly string ETFsCacheKey = $"Fcb_ETFsCache";
        private readonly string USStocksCacheKey = $"Fcb_USStocksCache";
        private readonly string BondsCacheKey = $"Fcb_BondsCache";

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            var funds = GetOrSetCache(FundsCacheKey, () => this._repository.GetFundsDatas(), 30);
            var etfs = GetOrSetCache(ETFsCacheKey, () => this._repository.GetETFDatas(), 30);
            var usStocks = GetOrSetCache(USStocksCacheKey, () => this._repository.GetUSStockDatas(), 30);
            var bonds = GetOrSetCache(BondsCacheKey, () => this._repository.GetBondDatas(), 30);

            var viewModel = new IndexRecommendationModel
            {
                Item = dataSourceItem,
                HotFunds = funds,
                FundDetailLink = FundRelatedSettingModel.GetFundDetailsUrl(),
                HotETFs = etfs,
                ETFDetailLink = EtfRelatedLinkSetting.GetETFDetailUrl(),
                HotUSStocks = usStocks,
                HotBonds = bonds,
                FundNetAssetValueDate = funds.Select(f => f.NetAssetValueDate).FirstOrDefault(),
                ETFMarketPriceDate = etfs.Select(f => f.MarketPriceDate).FirstOrDefault(),
                USStockDataDate = usStocks.Where(f => string.IsNullOrEmpty(f.DataDate) == false && DateTime.TryParse(f.DataDate, out var d)).Select(f => DateTime.Parse(f.DataDate)).FirstOrDefault(),
                BondDataDate = bonds.Where(f => string.IsNullOrEmpty(f.Date) == false && DateTime.TryParse(f.Date, out var d)).Select(f => DateTime.Parse(f.Date)).FirstOrDefault(),
            };


            return View("/Views/Feature/Wealth/Component/IndexRecommendation/IndexRecommendation.cshtml", viewModel);
        }
        public T GetOrSetCache<T>(string cacheKey, Func<T> getDataFunction, int cacheTime)
        {
            var cachedData = _cache.Get(cacheKey);
            if (cachedData != null)
            {
                return (T)cachedData;
            }

            lock (CacheLock)
            {
                cachedData = _cache.Get(cacheKey);
                if (cachedData != null)
                {
                    return (T)cachedData;
                }

                var data = getDataFunction();
                if (data is not null)
                {
                    _cache.Set(cacheKey, data, DateTimeOffset.Now.AddMinutes(cacheTime));
                }

                return data;
            }
        }
    }
}

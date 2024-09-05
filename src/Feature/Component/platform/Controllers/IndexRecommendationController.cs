using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.IndexRecommendation;
using Feature.Wealth.Component.Models.USStock;

namespace Feature.Wealth.Component.Controllers
{
    public class IndexRecommendationController : Controller
    {
        private readonly IndexRecommendationRepository _repository = new IndexRecommendationRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var funds = this._repository.GetFundsDatas();
            var etfs = this._repository.GetETFDatas();
            var usStocks = this._repository.GetUSStockDatas();
            var bonds = this._repository.GetBondDatas();

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
                USStockDataDate = usStocks.Select(f => DateTime.Parse(f.DataDate)).FirstOrDefault(),
                BondDataDate = bonds.Select(f => DateTime.Parse(f.Date)).FirstOrDefault(),
            };


            return View("/Views/Feature/Wealth/Component/IndexRecommendation/IndexRecommendation.cshtml", viewModel);
        }
    }
}

using Feature.Wealth.Component.Models.Compare;
using Feature.Wealth.Component.Models.ETF.Detail;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class FundETFCompareController : JsonNetController
    {
        private readonly CompareRepository _compareRepository;
        private readonly DjMoneyApiRespository _djMoneyApiRespository;
        private readonly FundRepository _fundRepository;
        private readonly EtfDetailRepository _detailRepository;

        /// <summary>
        /// 建構子
        /// </summary>
        public FundETFCompareController()
        {
            _compareRepository = new CompareRepository();
            _djMoneyApiRespository = new DjMoneyApiRespository();
            _fundRepository = new FundRepository();
            _detailRepository = new EtfDetailRepository();
        }

        public ActionResult Index()
        {
            var model = new FundETFCompareModel(RenderingContext.CurrentOrNull?.Rendering.Item)
            {
                GlobalIndexList = _compareRepository.GetCompareGlobalIndexList()
            };
            return View("/Views/Feature/Wealth/Component/Compare/FundETFCompare.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> Compare(List<string> fund, List<string> etf, List<string> stock, string range, string startDate, string endDate)
        {
            Dictionary<string, object> respNetAssetValue = new Dictionary<string, object>();
            Dictionary<string, object> respRateOfReturn = new Dictionary<string, object>();
            Dictionary<string, object> respAccumulationRateOfReturn = new Dictionary<string, object>();
            Dictionary<string, object> respRiskindicators = new Dictionary<string, object>();
            Dictionary<string, object> respThiryDaysNetValue = new Dictionary<string, object>();
            Dictionary<string, object> respHoldingStatus = new Dictionary<string, object>();
            Dictionary<string, object> respBaseData = new Dictionary<string, object>();
            Dictionary<string, object> respScaleRecords = new Dictionary<string, object>();

            Dictionary<string, object> etfdata = new Dictionary<string, object>();

            foreach (string id in fund?.Distinct().Take(7) ?? [])
            {
                respNetAssetValue[id] = await _djMoneyApiRespository.GetReturnAndNetValueTrend(id, nameof(FundRateTrendEnum.ori), range, startDate, endDate);
                var fundIndicator = _fundRepository.GetDometicOrOverseas(id);
                var fundViewModel = _fundRepository.GetOrSetFundDetailsCache(id, fundIndicator);

                respRateOfReturn[id] = fundViewModel.FundRateOfReturn;
                respAccumulationRateOfReturn[id] = fundViewModel.FundAccumulationRateOfReturn;
                respRiskindicators[id] = fundViewModel.FundRiskindicators;
                respThiryDaysNetValue[id] = fundViewModel.FundThiryDaysNetValue;

                respHoldingStatus[id] = new
                {
                    AccordingStockHoldings = fundViewModel.FundAccordingStockHoldings,
                    StockHoldings = fundViewModel.FundStockHoldings,
                    StockAreaHoldings = fundViewModel.FundStockAreaHoldings,
                    TopTenStockHolding = fundViewModel.FundTopTenStockHolding,
                };

                respBaseData[id] = fundViewModel.FundBaseData;
                respScaleRecords[id] = fundViewModel.FundScaleRecords;
            }

            foreach (string id in etf?.Distinct().Take(7) ?? [])
            {
                respNetAssetValue[id] = await _djMoneyApiRespository.GetReturnChartData(id, EtfReturnTrend.NetAssetValue, startDate, endDate);

                _detailRepository.ETFId = id;
                var detailModel = _detailRepository.GetOrSetETFDetailsCache(id);

                etfdata[id] = new
                {
                    detailModel.BasicEtf,
                    detailModel.ETFThiryDaysNav,
                    detailModel.ETFStockHoldings,
                    detailModel.ETFRiskIndicator,
                    Compare = _compareRepository.GetCompareETFNetAssetValueThreeMonths(id),
                    IndustryHoldings = _detailRepository.GetETFIndustryPercent(id),
                    RegionHoldings = _detailRepository.GetETFRegionPercent(id)
                };
            }

            foreach (string id in stock?.Distinct().Take(3) ?? [])
            {
                respNetAssetValue[id] = _compareRepository.GetGlobalIndexReturnChartData(id, startDate, endDate);
            }

            return new JsonNetResult(new
            {
                respNetAssetValue,
                respRateOfReturn,
                respAccumulationRateOfReturn,
                respRiskindicators,
                respThiryDaysNetValue,
                respHoldingStatus,
                respBaseData,
                respScaleRecords,
                etfdata
            });
        }
    }
}

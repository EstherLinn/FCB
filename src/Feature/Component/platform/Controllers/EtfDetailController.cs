using Feature.Wealth.Component.Models.ETF.Detail;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class EtfDetailController : JsonNetController
    {
        private readonly EtfDetailRepository _detailRepository = new EtfDetailRepository();
        private readonly DjMoneyApiRespository _djMoneyApiRespository;

        /// <summary>
        /// 建構子
        /// </summary>
        public EtfDetailController()
        {
            _djMoneyApiRespository = new DjMoneyApiRespository();
        }

        public ActionResult Index()
        {
            string etfId = Sitecore.Web.WebUtil.GetSafeQueryString("id");

            if (string.IsNullOrEmpty(etfId))
            {
                return View("/Views/Feature/Wealth/Component/ETFDetail/EtfDetailIndex.cshtml");
            }

            var datasource = ItemUtils.GetItem(RenderingContext.Current.Rendering.DataSource);
            var model = _detailRepository.GetETFDetailModel(etfId.ToUpper(), datasource);

            return View("/Views/Feature/Wealth/Component/ETFDetail/EtfDetailIndex.cshtml", model);
        }

        /// <summary>
        /// 近一年績效走勢
        /// </summary>
        /// <param name="etfId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetPerformanceTrend(string etfId)
        {
            var resp = await _djMoneyApiRespository.GetPerformanceTrend(etfId?.ToUpper());
            return new JsonNetResult(resp);
        }

        /// <summary>
        /// 市價/淨值走勢
        /// </summary>
        /// <param name="etfId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetReturnChartData(string etfId, string startDate, string endDate)
        {
            etfId = etfId?.ToUpper();
            var respMarketPrice = await _djMoneyApiRespository.GetReturnChartData(etfId, EtfReturnTrend.MarketPrice, startDate, endDate);
            var respNetAssetValue = await _djMoneyApiRespository.GetReturnChartData(etfId, EtfReturnTrend.NetAssetValue, startDate, endDate);

            return new JsonNetResult(new
            {
                respMarketPrice,
                respNetAssetValue
            });
        }

        /// <summary>
        /// ETF績效圖
        /// </summary>
        /// <param name="etfId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetReturnReferenceIndexChartData(string etfId, string startDate, string endDate)
        {
            etfId = etfId?.ToUpper();
            var respMarketPrice = await _djMoneyApiRespository.GetReturnChartData(etfId, EtfReturnTrend.MarketPrice, startDate, endDate);
            var respNetAssetValue = await _djMoneyApiRespository.GetReturnChartData(etfId, EtfReturnTrend.NetAssetValue, startDate, endDate);
            var respGlobalIndex = _detailRepository.GetGlobalIndexWithCloseYear(etfId, startDate, endDate);
            // ReferenceIndexID
            return new JsonNetResult(new
            {
                respMarketPrice,
                respNetAssetValue,
                respGlobalIndex
            });
        }

        /// <summary>
        /// 買賣價走勢圖
        /// </summary>
        /// <param name="etfId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHistoryPrice(string etfId, string startDate, string endDate)
        {
            // 買賣價走勢
            var resp = _detailRepository.GetHistoryPrice(etfId?.ToUpper(), startDate, endDate);
            return new JsonNetResult(resp);
        }

        /// <summary>
        /// K線圖
        /// </summary>
        /// <param name="etfId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetKLineChart(string etfId, string type)
        {
            var resp = await _djMoneyApiRespository.GetKLineChart(etfId?.ToUpper(), type);
            return new JsonNetResult(resp);
        }

        /// <summary>
        /// ETF PDF文件下載點
        /// </summary>
        /// <param name="etfId"></param>
        /// <param name="idx">文件類型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetDocLink(string etfId, string idx)
        {
            var resp = await _djMoneyApiRespository.GetEtfDocLink(etfId?.ToUpper(), idx);
            return new JsonNetResult(resp);
        }

        /// <summary>
        /// 產業、區域持股狀況
        /// </summary>
        /// <param name="etfId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetETFHoldings(string etfId)
        {
            etfId = etfId?.ToUpper();
            var resp = new RespHolding()
            {
                IndustryHoldings = _detailRepository.GetETFIndustryPercent(etfId),
                RegionHoldings = _detailRepository.GetETFRegionPercent(etfId)
            };
            return new JsonNetResult(resp);
        }

        /// <summary>
        /// 風險象限圖
        /// </summary>
        /// <param name="etfId"></param>
        /// <param name="selectType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetETFRiskGraph(string etfId, string selectType)
        {
            selectType = string.IsNullOrEmpty(selectType) ? "Type" : selectType;
            var resp = new EtfRiskGraphRespModel() { Body = Enumerable.Empty<EtfRiskGraph>() };
            resp.Body = _detailRepository.GetRiskindicatorsGraph(etfId?.ToUpper(), selectType);
            resp.StatusCode = HttpStatusCode.OK;
            resp.Message = "Success";
            var serialSetting = new Newtonsoft.Json.JsonSerializerSettings()
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            };
            return new JsonNetResult(resp, serialSetting);
        }
    }
}
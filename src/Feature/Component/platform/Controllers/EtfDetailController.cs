using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Detail;
using Feature.Wealth.Component.Repositories;
using log4net;
using Sitecore.Mvc.Presentation;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class EtfDetailController : JsonNetController
    {
        private readonly EtfDetailRepository _detailRepository = new EtfDetailRepository();
        private readonly DjMoneyApiRespository _djMoneyApiRespository;
        private readonly ILog _log = Logger.General;

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
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.OK, Message = "Success" };

            try
            {
                resp.Body = await _djMoneyApiRespository.GetPerformanceTrend(etfId?.ToUpper());
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.InternalServerError;
                this._log.Error("EtfDetail－近一年績效走勢", ex);
            }

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
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.OK, Message = "Success" };

            try
            {
                etfId = etfId?.ToUpper();
                var respMarketPrice = await _djMoneyApiRespository.GetReturnChartData(etfId, EtfReturnTrend.MarketPrice, startDate, endDate);
                var respNetAssetValue = await _djMoneyApiRespository.GetReturnChartData(etfId, EtfReturnTrend.NetAssetValue, startDate, endDate);
                resp.Body = new
                {
                    respMarketPrice,
                    respNetAssetValue,
                };
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.InternalServerError;
                this._log.Error("EtfDetail－市價/淨值走勢", ex);
            }

            return new JsonNetResult(resp);
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
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.OK, Message = "Success" };

            try
            {
                etfId = etfId?.ToUpper();
                var respMarketPrice = await _djMoneyApiRespository.GetReturnChartData(etfId, EtfReturnTrend.MarketPrice, startDate, endDate);
                var respNetAssetValue = await _djMoneyApiRespository.GetReturnChartData(etfId, EtfReturnTrend.NetAssetValue, startDate, endDate);
                var respGlobalIndex = _detailRepository.GetGlobalIndexWithCloseYear(etfId, startDate, endDate);

                resp.Body = new
                {
                    respMarketPrice,
                    respNetAssetValue,
                    respGlobalIndex
                };
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.InternalServerError;
                this._log.Error("EtfDetail－ETF績效圖", ex);
            }

            return new JsonNetResult(resp);
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
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.OK, Message = "Success" };

            try
            {
                resp.Body = _detailRepository.GetHistoryPrice(etfId?.ToUpper(), startDate, endDate);
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.InternalServerError;
                this._log.Error("EtfDetail－買賣價走勢圖", ex);
            }

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
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.OK, Message = "Success" };

            try
            {
                resp.Body = await _djMoneyApiRespository.GetKLineChart(etfId?.ToUpper(), type);
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.InternalServerError;
                this._log.Error("EtfDetail－K線圖", ex);
            }

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
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.OK, Message = "Success" };

            try
            {
                resp.Body = await _djMoneyApiRespository.GetEtfDocLink(etfId?.ToUpper(), idx);
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.InternalServerError;
                this._log.Error("EtfDetail－ETF PDF文件下載點", ex);
            }

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
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.OK, Message = "Success" };

            try
            {
                etfId = etfId?.ToUpper();
                resp.Body = new RespHolding()
                {
                    IndustryHoldings = _detailRepository.GetETFIndustryPercent(etfId),
                    RegionHoldings = _detailRepository.GetETFRegionPercent(etfId)
                };
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.InternalServerError;
                this._log.Error("EtfDetail－產業、區域持股狀況", ex);
            }

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
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.OK, Message = "Success" };

            try
            {
                selectType = string.IsNullOrEmpty(selectType) ? "Type" : selectType;
                resp.Body = _detailRepository.GetRiskindicatorsGraph(etfId?.ToUpper(), selectType);
                return new JsonNetResult(resp);
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.InternalServerError;
                this._log.Error("EtfDetail－風險象限圖", ex);
            }

            return new JsonNetResult(resp);
        }
    }
}
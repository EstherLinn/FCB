using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Detail;
using Feature.Wealth.Component.Repositories;
using Foundation.Wealth.Helper;
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

            if (string.IsNullOrWhiteSpace(etfId) || !InputSanitizerHelper.IsValidInput(etfId))
            {
                return Redirect("/404");
            }

            var datasource = ItemUtils.GetItem(RenderingContext.Current.Rendering.DataSource);
            var model = _detailRepository.GetETFDetailModel(InputSanitizerHelper.InputSanitizer(etfId.ToUpper()), datasource);
            if (model == null || model.BasicEtf == null)
            {
                return Redirect(EtfRelatedLinkSetting.GetETFSearchUrl());
            }
            return View("/Views/Feature/Wealth/Component/ETFDetail/EtfDetailIndex.cshtml", model);
        }

        /// <summary>
        /// 近一年績效走勢
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetPerformanceTrend(ReqBase req)
        {
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.NotFound, Message = "找不到資源" };

            try
            {
                var result = await _djMoneyApiRespository.GetPerformanceTrend(req.EtfId?.ToUpper());
                resp.Body = result;

                if (result != null)
                {
                    resp.Message = "Success";
                    resp.StatusCode = (int)HttpStatusCode.OK;
                }
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
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReturnChartData(ReqReturnTrend req)
        {
            RespEtf resp = new RespEtf();

            try
            {
                req.EtfId = InputSanitizerHelper.InputSanitizer(req.EtfId?.ToUpper());
                req.StartDate = InputSanitizerHelper.InputSanitizer(req.StartDate?.ToUpper());
                req.EndDate = InputSanitizerHelper.InputSanitizer(req.EndDate?.ToUpper());
                resp = _detailRepository.GetNavHisReturnTrendData(req);
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
        /// ETF績效圖－績效走勢
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetReturnReferenceIndexChartData(ReqReturnTrend req)
        {
            RespEtf resp = new RespEtf();

            try
            {
                req.EtfId = InputSanitizerHelper.InputSanitizer(req.EtfId?.ToUpper());
                req.StartDate = InputSanitizerHelper.InputSanitizer(req.StartDate?.ToUpper());
                req.EndDate = InputSanitizerHelper.InputSanitizer(req.EndDate?.ToUpper());
                resp = await _detailRepository.GetReturnTrendData(req);
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
        /// ETF績效圖－績效(報酬) 指標指數
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetBenchmarkROIChartData(ResReferenceIndex req)
        {
            RespEtf resp = new RespEtf();

            try
            {
                resp = await _detailRepository.GetReferenceIndex(req);
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
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHistoryPrice(ReqHistory req)
        {
            RespEtf resp = new RespEtf();

            try
            {
                req.EtfId = InputSanitizerHelper.InputSanitizer(req.EtfId?.ToUpper());
                req.StartDate = InputSanitizerHelper.InputSanitizer(req.StartDate?.ToUpper());
                req.EndDate = InputSanitizerHelper.InputSanitizer(req.EndDate?.ToUpper());
                resp = _detailRepository.GetHistoryPrice(req);
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
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetKLineChart(ReqKLine req)
        {
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.NotFound, Message = "找不到資源" };

            if (string.IsNullOrWhiteSpace(req.Type))
            {
                resp.Message = "錯誤的查詢，請確認您的查詢參數";
                resp.StatusCode = (int)HttpStatusCode.Forbidden;
            }

            try
            {
                var result = await _djMoneyApiRespository.GetKLineChart(req.EtfId?.ToUpper(), req.Type);
                resp.Body = result;

                if (result != null)
                {
                    resp.Message = "Success";
                    resp.StatusCode = (int)HttpStatusCode.OK;
                }
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
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetDocLink(ReqDocLink req)
        {
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.NotFound, Message = "找不到資源" };

            try
            {
                var result = await _djMoneyApiRespository.GetEtfDocLink(req.EtfId?.ToUpper(), req.Idx);
                resp.Body = result;

                if (result != null)
                {
                    resp.Message = "Success";
                    resp.StatusCode = (int)HttpStatusCode.OK;
                }
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
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetETFHoldings(ReqBase req)
        {
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.NotFound, Message = "找不到資源" };

            try
            {
                req.EtfId = req.EtfId?.ToUpper();
                var result = new RespHolding()
                {
                    IndustryHoldings = _detailRepository.GetETFIndustryPercent(req.EtfId),
                    RegionHoldings = _detailRepository.GetETFRegionPercent(req.EtfId)
                };

                resp.Body = result;

                if (result.IndustryHoldings != null && result.RegionHoldings != null)
                {
                    resp.Message = "Success";
                    resp.StatusCode = (int)HttpStatusCode.OK;
                }
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
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetETFRiskGraph(ReqRiskGraph req)
        {
            RespEtf resp = new RespEtf() { StatusCode = (int)HttpStatusCode.NotFound, Message = "找不到資源" };

            try
            {
                req.EtfId = req.EtfId?.ToUpper();
                req.SelectType = string.IsNullOrEmpty(req.SelectType) ? "Type" : req.SelectType;
                var result = _detailRepository.GetRiskindicatorsGraph(req.EtfId, req.SelectType);
                resp.Body = result;

                if (result != null)
                {
                    resp.Message = "Success";
                    resp.StatusCode = (int)HttpStatusCode.OK;
                }
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
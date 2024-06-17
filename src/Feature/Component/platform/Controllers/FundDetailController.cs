using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Repositories;
using Sitecore.Configuration;
using Sitecore.Mvc.Presentation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class FundDetailController : JsonNetController
    {

        private readonly FundRepository _fundRepository;
        private readonly DjMoneyApiRespository _djMoneyApiRespository;

        /// <summary>
        /// 建構子
        /// </summary>
        public FundDetailController()
        {
            _fundRepository = new FundRepository();
            _djMoneyApiRespository = new DjMoneyApiRespository();
        }

        public ActionResult FundDetail()
        {
            var fundViewModel = new FundViewModel();

            var fundid = Sitecore.Web.WebUtil.GetSafeQueryString("id");

            if (string.IsNullOrEmpty(fundid))
            {
                return PartialView("~/Views/Feature/Wealth/Component/FundDetail/FundDetailOverseas.cshtml", fundViewModel);
            }

            fundid = fundid.ToUpper();
            var fundIndicator = _fundRepository.GetDometicOrOverseas(fundid);
            fundViewModel = _fundRepository.GetOrSetFundDetailsCache(fundid, fundIndicator);

            if (fundViewModel.FundBaseData == null)
            {
                return PartialView("~/Views/Feature/Wealth/Component/FundDetail/FundDetailOverseas.cshtml", fundViewModel);
            }

            fundViewModel.Item = RenderingContext.CurrentOrNull?.Rendering.Item;

            if (fundIndicator == nameof(FundEnum.D))
            {
                fundViewModel = _fundRepository.GetDocLinks(fundid, fundViewModel, fundIndicator, _djMoneyApiRespository);

                return PartialView("~/Views/Feature/Wealth/Component/FundDetail/FundDetailDomestic.cshtml", fundViewModel);
            }
            else
            {
                fundViewModel = _fundRepository.GetDocLinks(fundid, fundViewModel, fundIndicator, _djMoneyApiRespository);

                return PartialView("~/Views/Feature/Wealth/Component/FundDetail/FundDetailOverseas.cshtml", fundViewModel);
            }

        }

        public ActionResult FundCloseFiveYears()
        {
            var getTarget = Sitecore.Web.WebUtil.GetSafeQueryString("target");
            if (string.IsNullOrEmpty(getTarget) || getTarget.ToUpper() == "OUTSIDE")
            {
                return PartialView("~/Views/Feature/Wealth/Component/FundDetail/FundCloseFiveYears.cshtml");
            }
            else
            {
                return PartialView("~/Views/Feature/Wealth/Component/FundDetail/FundCloseFiveYearsInside.cshtml");
            }
        }

        [HttpPost]
        public ActionResult GetFundRiskGraph(string fundId, string selectType)
        {
            selectType = string.IsNullOrEmpty(selectType) ? "Type" : selectType;
            var resp = new FundRiskGraphRespModel() { Body = Enumerable.Empty<FundRiskGraph>() };
            resp.Body = _fundRepository.GetRiskindicatorsGraph(fundId.ToUpper(), selectType);

            resp.StatusCode = HttpStatusCode.OK;
            resp.Message = "Success";
            var serialSetting = new Newtonsoft.Json.JsonSerializerSettings()
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            };
            return new JsonNetResult(resp, serialSetting);
        }

        [HttpPost]
        public async Task<ActionResult> GetSameLevelFund(string fundId)
        {

            var resp = await _djMoneyApiRespository.GetSameLevelFund(fundId.ToUpper());
            return new JsonNetResult(resp);

        }

        [HttpPost]
        public async Task<ActionResult> GetCloseYearPerformance(string fundId)
        {
            var resp = await _djMoneyApiRespository.GetGetCloseYearPerformance(fundId.ToUpper());

            return new JsonNetResult(resp);

        }

        [HttpPost]
        public async Task<ActionResult> GetDocLink(string fundId, string idx)
        {
            var resp = await _djMoneyApiRespository.GetDocLink(fundId.ToUpper(), idx);

            return new JsonNetResult(resp);

        }

        [HttpPost]
        public async Task<ActionResult> GetRuleText(string fundId, string type, string indicator)
        {
            var resp = await _djMoneyApiRespository.GetRuleText(fundId.ToUpper(), type, indicator);

            return new JsonNetResult(resp);

        }

        [HttpPost]
        public async Task<ActionResult> GetReturnAndNetValueTrend(string fundId, string trend, string range, string startdate, string enddate)
        {
            var resp = await _djMoneyApiRespository.GetReturnAndNetValueTrend(fundId.ToUpper(), trend, range, startdate, enddate);
            return new JsonNetResult(resp);
        }

    }
}

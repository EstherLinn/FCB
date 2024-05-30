using Feature.Wealth.Account.Filter;
using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.FundTrackList;
using Feature.Wealth.Account.Models.ReachInfo;
using Feature.Wealth.Account.Repositories;
using Feature.Wealth.Component.Models.FocusList;
using Feature.Wealth.Component.Models.Invest;
using Feature.Wealth.Component.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    [MemberAuthenticationFilter]
    public class FocusListController :  JsonNetController
    {
        private readonly MemberRepository _memberRepository;

        private readonly ReachInfoRepository _reachInfoRepository;

        private readonly FocusListRespository _focusListRespository;

        public FocusListController()
        {
            this._memberRepository = new MemberRepository();
            this._reachInfoRepository = new ReachInfoRepository();
            this._focusListRespository = new FocusListRespository();
        }
        public ActionResult Index()
        {
            return View("/Views/Feature/Wealth/Component/FocusList/FocusList.cshtml");
        }

        [HttpPost]
        public ActionResult GetTrackFunds()
        {
            List<TrackListModel> trackLists = _memberRepository.GetTrackListFromDb(FcbMemberHelper.GetMemberPlatFormId());
            if (trackLists == null)
            {
                return new JsonNetResult();
            }
            var fundFocusList = trackLists.Where(x => x.Type.Equals(InvestTypeEnum.Fund.ToString(),System.StringComparison.OrdinalIgnoreCase))
                ?.Select(y => y.Id).ToList();
            if (fundFocusList == null)
            {
                return new JsonNetResult();
            }
            return new JsonNetResult(this.GetFocusList(fundFocusList));
        }

        [HttpPost]
        public ActionResult GetTrackEtfs()
        {
            List<TrackListModel> trackLists = _memberRepository.GetTrackListFromDb(FcbMemberHelper.GetMemberPlatFormId());
            if (trackLists == null)
            {
                return new JsonNetResult();
            }
            var etfFocusList = trackLists.Where(x => x.Type.Equals(InvestTypeEnum.ETF.ToString(), System.StringComparison.OrdinalIgnoreCase))
                ?.Select(y => y.Id).ToList();
            if (etfFocusList == null)
            {
                return new JsonNetResult();
            }
            return new JsonNetResult(this.GetEtfList(etfFocusList));
        }

        [HttpPost]
        public ActionResult GetTrackForeignStocks()
        {
            List<TrackListModel> trackLists = _memberRepository.GetTrackListFromDb(FcbMemberHelper.GetMemberPlatFormId());
            if (trackLists == null)
            {
                return new JsonNetResult();
            }
            var foreignStockFocusList = trackLists.Where(x => x.Type.Equals(InvestTypeEnum.ForeignStocks.ToString(), System.StringComparison.OrdinalIgnoreCase))
                ?.Select(y => y.Id).ToList();
            if (foreignStockFocusList == null)
            {
                return new JsonNetResult();
            }
            return new JsonNetResult(this.GetForeignStockList(foreignStockFocusList));
        }

        [HttpPost]
        public ActionResult GetProductReachInfo(string type, string id)
        {
            ReachInfoResp reachInfoResp = new ReachInfoResp();
            reachInfoResp.Body = _reachInfoRepository.GetProductReachInfo(FcbMemberHelper.GetMemberPlatFormId(), type, id);

            var serialSetting = new Newtonsoft.Json.JsonSerializerSettings()
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            };
            return new JsonNetResult(reachInfoResp, serialSetting);
        }

        private List<FundListModel> GetFocusList(List<string> fundFocusList)
        {
            var items = _focusListRespository.GetFundFocusData(fundFocusList);
            var itemsWithButton = _focusListRespository.SetButtonToFocusList(items, InvestTypeEnum.Fund);
            var itemsWithButtonAndInfo = _focusListRespository.SetReachInfoToFocusList(itemsWithButton, InvestTypeEnum.Fund);
            //var finalItems = _focusListRespository.SetTagsToFund(itemsWithButtonAndInfo);

            return itemsWithButtonAndInfo;
        }

        private List<EtfListModel> GetEtfList(List<string> etfFocusList)
        {
            var items = _focusListRespository.GetETFFocusData(etfFocusList);
            var itemsWithButton = _focusListRespository.SetButtonToFocusList(items, InvestTypeEnum.ETF);
            var itemsWithButtonAndInfo = _focusListRespository.SetReachInfoToFocusList(itemsWithButton, InvestTypeEnum.ETF);
            var finalItems = _focusListRespository.SetTagsToEtf(itemsWithButtonAndInfo);
            return finalItems;
        }

        private List<ForeignStockListModel> GetForeignStockList(List<string> foreignStockFocusList)
        {
            var items = _focusListRespository.GetForeignStockFocusData(foreignStockFocusList);
            var itemsWithButton = _focusListRespository.SetButtonToFocusList(items, InvestTypeEnum.ForeignStocks);
            var itemsWithButtonAndInfo = _focusListRespository.SetReachInfoToFocusList(itemsWithButton, InvestTypeEnum.ForeignStocks);
            var finalItems = _focusListRespository.SetTagsToForeignStock(itemsWithButtonAndInfo);
            return finalItems;
        }
    }
}
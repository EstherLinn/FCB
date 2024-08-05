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
        [MemberAuthenticationFilter]
        public ActionResult Index()
        {
            return View("/Views/Feature/Wealth/Component/FocusList/FocusList.cshtml");
        }

        [HttpPost]
        public ActionResult GetTrackFunds(List<TrackListModel> trackListModels)
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            List<TrackListModel> trackLists = trackListModels ?? _memberRepository.GetTrackListFromDb(FcbMemberHelper.GetMemberPlatFormId());
            if (trackLists == null)
            {
                return new JsonNetResult();
            }
            return new JsonNetResult(this.GetFocusList(trackLists.Select(y => y.Id).ToList()));
        }

        [HttpPost]
        public ActionResult GetTrackEtfs(List<TrackListModel> trackListModels)
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            List<TrackListModel> trackLists = trackListModels ?? _memberRepository.GetTrackListFromDb(FcbMemberHelper.GetMemberPlatFormId());
            if (trackLists == null)
            {
                return new JsonNetResult();
            }
            return new JsonNetResult(this.GetEtfList(trackLists.Select(y => y.Id).ToList()));
        }

        [HttpPost]
        public ActionResult GetTrackForeignStocks(List<TrackListModel> trackListModels)
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            List<TrackListModel> trackLists = trackListModels ?? _memberRepository.GetTrackListFromDb(FcbMemberHelper.GetMemberPlatFormId());
            if (trackLists == null)
            {
                return new JsonNetResult();
            }
            return new JsonNetResult(this.GetForeignStockList(trackLists.Select(y => y.Id).ToList()));
        }

        [HttpPost]
        public ActionResult GetProductReachInfo(string type, string id)
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
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
            var finalItems = _focusListRespository.SetTagsToFund(itemsWithButtonAndInfo);

            return finalItems;
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
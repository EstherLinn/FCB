using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.GlobalIndex;
using Feature.Wealth.Component.Repositories;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class GlobalIndexDetailController : Controller
    {
        private readonly GlobalIndexRepository _globalIndexRepository = new GlobalIndexRepository();
        private readonly DjMoneyApiRespository _djMoneyApiRespository = new DjMoneyApiRespository();

        public ActionResult Mainstage()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            string indexCode = Sitecore.Web.WebUtil.GetSafeQueryString("id");

            this._globalIndexRepository.TriggerViewCountRecord(indexCode);

            return View("/Views/Feature/Wealth/Component/GlobalIndex/GlobalIndexDetailMainstage.cshtml", CreateModel(item, indexCode));
        }

        public ActionResult Wrap()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            string indexCode = Sitecore.Web.WebUtil.GetSafeQueryString("id");

            var model = CreateModel(item, indexCode);

            if (!string.IsNullOrEmpty(indexCode))
            {
                // 取得相關指數、基金、ETF
                try
                {
                    model.RelevantGlobalIndex = GetGlobalInedxRelevantInformation(indexCode, RelevantInformationType.GlobalIndex, model.DetailLink);
                }
                catch (Exception ex)
                {
                    model.RelevantGlobalIndex_Success = "Fail";
                }

                try
                {
                    model.RelevantFund = GetGlobalInedxRelevantInformation(indexCode, RelevantInformationType.Fund, Models.FundDetail.FundRelatedSettingModel.GetFundDetailsUrl());
                }
                catch (Exception ex)
                {
                    model.RelevantFund_Success = "Fail";
                }

                try
                {
                    model.RelevantETF = GetGlobalInedxRelevantInformation(indexCode, RelevantInformationType.ETF, model.ETFLink);
                }
                catch (Exception ex)
                {
                    model.RelevantETF_Success = "Fail";
                }

                // 取得 K 線圖資料
                try
                {
                    model.PriceData_D = GetGlobalInedxPriceData(indexCode, "D");
                }
                catch (Exception ex)
                {
                    model.PriceData_D_Success = "Fail";
                }

                try
                {
                    model.PriceData_W = GetGlobalInedxPriceData(indexCode, "W");
                }
                catch (Exception ex)
                {
                    model.PriceData_W_Success = "Fail";
                }

                try
                {
                    model.PriceData_M = GetGlobalInedxPriceData(indexCode, "M");
                }
                catch (Exception ex)
                {
                    model.PriceData_M_Success = "Fail";
                }
            }

            return View("/Views/Feature/Wealth/Component/GlobalIndex/GlobalIndexDetailWrap.cshtml", model);
        }

        protected GlobalIndexDetailModel CreateModel(Item item, string indexCode)
        {
            string detailLink = ItemUtils.GeneralLink(item, Template.GlobalIndexDetail.Fields.DetailLink)?.Url;
            string fundLink = ItemUtils.GeneralLink(item, Template.GlobalIndexDetail.Fields.FundLink)?.Url;
            string etfLink = ItemUtils.GeneralLink(item, Template.GlobalIndexDetail.Fields.ETFLink)?.Url;

            var globalIndexDetail = this._globalIndexRepository.GetGlobalIndexDetail(indexCode);

            var model = new GlobalIndexDetailModel
            {
                DetailLink = detailLink,
                FundLink = fundLink,
                ETFLink = etfLink,
                GlobalIndexDetail = globalIndexDetail
            };

            return model;
        }

        private HtmlString GetGlobalInedxRelevantInformation(string indexCode, RelevantInformationType type, string detailLink)
        {
            var relevantInformations = new List<RelevantInformation>();

            var resp = this._djMoneyApiRespository.GetGlobalInedxRelevantInformation(indexCode, (int)type);

            if (resp != null
                && resp.ContainsKey("items")
                && resp["items"] != null
                && resp["items"]["item"] != null
                && resp["items"]["item"].Any())
            {
                foreach (var item in resp["items"]["item"])
                {
                    var relevantInformation = item.ToObject<RelevantInformation>();

                    relevantInformation.d1 = this._globalIndexRepository.Round2(relevantInformation.d1);
                    relevantInformation.d7 = this._globalIndexRepository.Round2(relevantInformation.d7);
                    relevantInformation.m1 = this._globalIndexRepository.Round2(relevantInformation.m1);
                    relevantInformation.m3 = this._globalIndexRepository.Round2(relevantInformation.m3);
                    relevantInformation.m6 = this._globalIndexRepository.Round2(relevantInformation.m6);
                    relevantInformation.ytd = this._globalIndexRepository.Round2(relevantInformation.ytd);
                    relevantInformation.y1 = this._globalIndexRepository.Round2(relevantInformation.y1);
                    relevantInformation.y3 = this._globalIndexRepository.Round2(relevantInformation.y3);
                    relevantInformation.y5 = this._globalIndexRepository.Round2(relevantInformation.y5);

                    relevantInformations.Add(relevantInformation);
                }
            }

            IList<GlobalIndex> globalIndexList = new List<GlobalIndex>();

            if (type == RelevantInformationType.GlobalIndex)
            {
                globalIndexList = this._globalIndexRepository.GetGlobalIndexList();
            }

            foreach (var item in relevantInformations)
            {
                if (type == RelevantInformationType.GlobalIndex)
                {
                    if (globalIndexList.Any(X => X.IndexCode == item.id))
                    {
                        item.DetailLink = detailLink + "?id=" + item.id;
                    }
                }
                else if (type == RelevantInformationType.Fund)
                {
                    //TODO 待確認 Fund 自家沒有時的寫法
                    item.DetailLink = detailLink + "?id=" + item.id;
                }
                else if (type == RelevantInformationType.ETF)
                {
                    //TODO 未確認 ETF 詳細頁參數
                    //TODO 待確認 ETF 自家沒有時的寫法
                    item.DetailLink = detailLink + "?id=" + item.id;
                }
            }

            return new HtmlString(JsonConvert.SerializeObject(relevantInformations));
        }

        private HtmlString GetGlobalInedxPriceData(string indexCode, string cycle)
        {
            var priceDatas = this._globalIndexRepository.GetGlobalInedxPriceData(indexCode, cycle);

            return new HtmlString(JsonConvert.SerializeObject(priceDatas));
        }
    }
}
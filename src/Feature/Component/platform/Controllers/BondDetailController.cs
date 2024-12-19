using Feature.Wealth.Component.Models.Bond;
using Feature.Wealth.Component.Repositories;
using Foundation.Wealth.Helper;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class BondDetailController : Controller
    {
        private readonly BondRepository _bondRepository = new BondRepository();

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;
            string bondCode = Sitecore.Web.WebUtil.GetSafeQueryString("id");
            if (string.IsNullOrWhiteSpace(bondCode) || !InputSanitizerHelper.IsValidInput(bondCode))
            {
                return Redirect("/404");
            }
            var bond =  this._bondRepository.GetBond(InputSanitizerHelper.InputSanitizer(bondCode));
            if (bond == null)
            {
                return Redirect(BondRelatedLinkSetting.GetBondSearchUrl());
            }
            var viewName = "/Views/Feature/Wealth/Component/Bond/BondDetail.cshtml";
            return View(viewName, CreateModel(item, bond));
        }

        private BondDetailModel CreateModel(Item item, Bond bond)
        {
            var bondHistoryPriceList = this._bondRepository.GetBondHistoryPrice(bond.BondCode).OrderBy(b => b.Date).ToList();

            var top5BondHistoryPrice = this._bondRepository.GetTop5FUND_ETF(bond.BondCode).ToList();

            if (top5BondHistoryPrice.Any())
            {
                var lastBondHistoryPrice = top5BondHistoryPrice.First();

                if (top5BondHistoryPrice.Count > 1)
                {
                    lastBondHistoryPrice = top5BondHistoryPrice[1];
                }                

                if (bond.RedemptionFee != null && lastBondHistoryPrice.RedemptionFee != null)
                {
                    bond.UpsAndDownsDay = bond.RedemptionFee - lastBondHistoryPrice.RedemptionFee;
                    bond.UpsAndDownsPercentage = bond.UpsAndDownsDay / lastBondHistoryPrice.RedemptionFee * 100;
                }

                bond.UpsAndDownsDay = this._bondRepository.Round2(bond.UpsAndDownsDay);
                bond.UpsAndDownsPercentage = this._bondRepository.Round2(bond.UpsAndDownsPercentage);
            }

            var model = new BondDetailModel
            {
                Item = item,
                BondDetail = bond,
                BondHistoryPriceList = bondHistoryPriceList,
                BondHistoryPriceHtmlString = new HtmlString(JsonConvert.SerializeObject(bondHistoryPriceList)),
                Top5BondHistoryPrice = top5BondHistoryPrice
            };

            return model;
        }
    }
}

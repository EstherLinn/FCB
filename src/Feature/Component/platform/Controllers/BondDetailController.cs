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

            var temp = bondHistoryPriceList.OrderByDescending(b => b.Date).ToList();

            var top5BondHistoryPrice = new List<BondHistoryPrice>();

            foreach (var bondHistoryPrice in temp)
            {
                if (top5BondHistoryPrice.Count < 5)
                {
                    top5BondHistoryPrice.Add(bondHistoryPrice);
                }
                else
                {
                    break;
                }
            }

            if (top5BondHistoryPrice.Any() && !string.IsNullOrEmpty(bond.Date) && !string.IsNullOrEmpty(top5BondHistoryPrice[0].Date))
            {
                var lastBondHistoryPrice = top5BondHistoryPrice.First();
                DateTime.TryParse(bond.Date, out var now);
                DateTime.TryParse(lastBondHistoryPrice.Date, out var last);

                if (DateTime.Compare(now, last) == 0)
                {
                    if (top5BondHistoryPrice.Count > 1)
                    {
                        lastBondHistoryPrice = top5BondHistoryPrice[1];
                    }
                }
                else if (DateTime.Compare(now, last) < 0)
                {
                    bond.RedemptionFee = lastBondHistoryPrice.RedemptionFee;

                    if (top5BondHistoryPrice.Count > 1)
                    {
                        lastBondHistoryPrice = top5BondHistoryPrice[1];
                    }
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

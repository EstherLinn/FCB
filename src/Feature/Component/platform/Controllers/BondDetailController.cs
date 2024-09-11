using Feature.Wealth.Component.Models.Bond;
using Feature.Wealth.Component.Repositories;
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

            string id = Sitecore.Web.WebUtil.GetSafeQueryString("id");
            if (string.IsNullOrEmpty(id))
            {
                return View("/Views/Feature/Wealth/Component/Bond/BondDetail.cshtml");
            }
            var bond = this._bondRepository.GetBond(id);

            return View("/Views/Feature/Wealth/Component/Bond/BondDetail.cshtml", CreateModel(item, bond));
        }

        private BondDetailModel CreateModel(Item item, Bond bond)
        {
            var model = new BondDetailModel
            {
                Item = item,
                BondDetail = bond,
                BondHistoryPriceList = this._bondRepository.GetBondHistoryPrice(bond.BondCode).OrderBy(b => b.Date).ToList()
            };

            model.BondHistoryPriceHtmlString = new HtmlString(JsonConvert.SerializeObject(model.BondHistoryPriceList));

            var temp = model.BondHistoryPriceList.OrderByDescending(b => b.Date).ToList();

            foreach (var bondHistoryPrice in temp)
            {
                if (model.Top5BondHistoryPrice.Count < 5)
                {
                    model.Top5BondHistoryPrice.Add(bondHistoryPrice);
                }
                else
                {
                    break;
                }
            }

            if (model.Top5BondHistoryPrice.Any())
            {
                var lastBondHistoryPrice = model.Top5BondHistoryPrice.First();
                var now = DateTime.Parse(model.BondDetail.Date);
                var last = DateTime.Parse(lastBondHistoryPrice.Date);

                if (DateTime.Compare(now, last) == 0)
                {
                    if (model.Top5BondHistoryPrice.Count > 1)
                    {
                        lastBondHistoryPrice = model.Top5BondHistoryPrice[1];
                    }
                }
                else if (DateTime.Compare(now, last) < 0)
                {
                    model.BondDetail.RedemptionFee = lastBondHistoryPrice.RedemptionFee;

                    if (model.Top5BondHistoryPrice.Count > 1)
                    {
                        lastBondHistoryPrice = model.Top5BondHistoryPrice[1];
                    }
                }

                if (model.BondDetail.RedemptionFee != null && lastBondHistoryPrice.RedemptionFee != null)
                {
                    model.BondDetail.UpsAndDownsDay = model.BondDetail.RedemptionFee - lastBondHistoryPrice.RedemptionFee;
                    model.BondDetail.UpsAndDownsPercentage = model.BondDetail.UpsAndDownsDay / lastBondHistoryPrice.RedemptionFee * 100;
                }

                model.BondDetail.UpsAndDownsDay = this._bondRepository.Round2(model.BondDetail.UpsAndDownsDay);
                model.BondDetail.UpsAndDownsPercentage = this._bondRepository.Round2(model.BondDetail.UpsAndDownsPercentage);
            }

            return model;
        }
    }
}

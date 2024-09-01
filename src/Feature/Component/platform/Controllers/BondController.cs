using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.Bond;
using Feature.Wealth.Component.Repositories;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Services.Core.ComponentModel;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class BondController : Controller
    {
        private readonly BondRepository _bondRepository = new BondRepository();

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var bondList = this._bondRepository.GetBondList();

            return View("/Views/Feature/Wealth/Component/Bond/Bond.cshtml", CreateModel(item, bondList));
        }

        protected BondModel CreateModel(Item item, IList<Bond> bondList)
        {
            string detailLink = BondRelatedLinkSetting.GetBondDetailUrl();

            var hotKeywordTags = ItemUtils.GetMultiListValueItems(item, Template.Bond.Fields.HotKeyword);
            var keywords = hotKeywordTags.Select(f => ItemUtils.GetFieldValue(f, Template.BondTag.Fields.TagName)).ToList();
            var hotProductTags = ItemUtils.GetMultiListValueItems(item, Template.Bond.Fields.HotProduct);
            var products = hotProductTags.Select(f => ItemUtils.GetFieldValue(f, Template.BondTag.Fields.TagName)).ToList();

            for (int i = 0; i < bondList.Count; i++)
            {
                var bond = bondList[i];
                bond.DetailLink = detailLink + "?id=" + bond.BondCode;
                bond = this._bondRepository.GetButtonHtml(bond, true);
                bond = this._bondRepository.SetTags(bond);
            }

            var model = new BondModel
            {
                Item = item,
                BondList = bondList,
                BondHtmlString = new HtmlString(JsonConvert.SerializeObject(bondList)),
                HotKeywordTags = keywords,
                HotProductTags = products
            };

            return model;
        }
    }
}

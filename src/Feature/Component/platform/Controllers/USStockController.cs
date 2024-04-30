using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.USStock;
using Feature.Wealth.Component.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Services.Core.ComponentModel;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class USStockController : Controller
    {
        private readonly USStockRepository _uSStockRepository = new USStockRepository();

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var uSStockList = this._uSStockRepository.GetUSStockList();

            return View("/Views/Feature/Wealth/Component/USStock/USStock.cshtml", CreateModel(item, uSStockList));
        }

        protected USStockModel CreateModel(Item item, IList<USStock> uSStockList)
        {
            string detailLink = ItemUtils.GeneralLink(item, Template.USStock.Fields.DetailLink)?.Url;

            var hotkeywordtags = ItemUtils.GetMultiListValueItems(item, Template.USStock.Fields.hotKeyword);
            var keyword = hotkeywordtags.Select(f => ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.CampaignTypeCode)).ToList();
            var hotproducttags = ItemUtils.GetMultiListValueItems(item, Template.USStock.Fields.hotProduct);
            var product = hotproducttags.Select(f => ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.CampaignTypeCode)).ToList();

            foreach (var uSStock in uSStockList)
            {
                uSStock.DetailLink = detailLink + "?id=" + uSStock.FirstBankCode;

                foreach (var f in hotkeywordtags)
                {
                    string campaignTypeCode = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.CampaignTypeCode);
                    string firstBankCodeList = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.FirstBankCodeList);

                    if (firstBankCodeList.Contains(uSStock.FirstBankCode))
                    {
                        uSStock.HotKeywordTags.Add(campaignTypeCode);
                    }
                }

                foreach (var f in hotproducttags)
                {
                    string campaignTypeCode = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.CampaignTypeCode);
                    string firstBankCodeList = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.FirstBankCodeList);

                    if (firstBankCodeList.Contains(uSStock.FirstBankCode))
                    {
                        uSStock.HoProductTags.Add(campaignTypeCode);
                    }
                }
            }

            var model = new USStockModel
            {
                Item = item,
                USStockList = uSStockList,
                USStockDictionary = uSStockList.ToDictionary(x => x.FirstBankCode, x => x),
                USStockJson = JsonSerializer.Serialize(uSStockList),
                HotKeywordTags = keyword,
                HotProductTags = product
            };

            return model;
        }

    }
}
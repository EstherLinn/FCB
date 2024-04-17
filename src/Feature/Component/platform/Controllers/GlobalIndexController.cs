using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.GlobalIndex;
using Feature.Wealth.Component.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Services.Core.ComponentModel;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class GlobalIndexController : Controller
    {
        private readonly GlobalIndexRepository _globalIndexRepository = new GlobalIndexRepository();

        public ActionResult Mainstage()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var globalIndexList = _globalIndexRepository.GetCommonGlobalIndexList();

            foreach (var globalIndex in globalIndexList)
            {
                // 排序改成舊到新
                globalIndex.GlobalIndexHistory = _globalIndexRepository.GetGlobalIndexHistoryList(globalIndex.IndexCode).OrderBy(c => c.DataDate).ToList();
                globalIndex.GlobalIndexHistoryJson = JsonSerializer.Serialize(globalIndex.GlobalIndexHistory.Select(c => float.Parse(c.MarketPrice)).ToList());
            }

            return View("/Views/Feature/Wealth/Component/GlobalIndex/GlobalIndexMainstage.cshtml", CreateModel(item, globalIndexList));
        }

        public ActionResult Wrap()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var globalIndexList = _globalIndexRepository.GetGlobalIndexList();

            return View("/Views/Feature/Wealth/Component/GlobalIndex/GlobalIndexWrap.cshtml", CreateModel(item, globalIndexList));
        }

        protected GlobalIndexModel CreateModel(Item item, IList<GlobalIndex> globalIndexList)
        {
            var detailLink = ItemUtils.GeneralLink(item, Template.GlobalIndex.Fields.DetailLink)?.Url;

            foreach (var globalIndex in globalIndexList)
            {
                globalIndex.DetailLink = detailLink + "?IndexCode=" + globalIndex.IndexCode;
            }

            var model = new GlobalIndexModel
            {
                Item = item,
                DetailLink = detailLink,
                GlobalIndexList = globalIndexList,
                GlobalIndexDictionary = globalIndexList.ToDictionary(x => x.IndexCode, x => x)
            };

            return model;
        }
    }
}
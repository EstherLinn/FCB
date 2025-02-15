﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.GlobalIndex;
using Feature.Wealth.Component.Repositories;
using Newtonsoft.Json;
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

            var globalIndexList = this._globalIndexRepository.GetCommonGlobalIndexList();

            foreach (var globalIndex in globalIndexList)
            {
                globalIndex.GlobalIndexHistory = this._globalIndexRepository.GetGlobalIndexHistoryList(globalIndex.IndexCode);
            }

            return View("/Views/Feature/Wealth/Component/GlobalIndex/GlobalIndexMainstage.cshtml", CreateModel(item, globalIndexList));
        }

        public ActionResult Wrap()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var globalIndexList = this._globalIndexRepository.GetGlobalIndexList();

            return View("/Views/Feature/Wealth/Component/GlobalIndex/GlobalIndexWrap.cshtml", CreateModel(item, globalIndexList));
        }

        protected GlobalIndexModel CreateModel(Item item, IList<GlobalIndex> globalIndexList)
        {
            string detailLink = ItemUtils.GeneralLink(item, Template.GlobalIndex.Fields.DetailLink)?.Url;
            var datas = new List<GlobalIndexHighchartsData>();

            foreach (var globalIndex in globalIndexList)
            {
                globalIndex.DetailLink = detailLink + "?id=" + globalIndex.IndexCode;
                globalIndex.IndexNameHtmlString = new HtmlString(globalIndex.IndexName.Replace("(", "<br>("));
                if (globalIndex.GlobalIndexHistory != null && globalIndex.GlobalIndexHistory.Count > 0)
                {
                    datas.Add(new GlobalIndexHighchartsData
                    {
                        IndexCode = globalIndex.IndexCode,
                        Data = globalIndex.GlobalIndexHistory
                    });
                }
            }

            var model = new GlobalIndexModel
            {
                Item = item,
                DetailLink = detailLink,
                GlobalIndexList = globalIndexList,
                GlobalIndexDictionary = globalIndexList.ToDictionary(x => x.IndexCode, x => x),
                GlobalIndexHighchartsDataHtmlString = new HtmlString(JsonConvert.SerializeObject(datas))
            };

            return model;
        }
    }
}
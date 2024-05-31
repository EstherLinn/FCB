using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.Component.Models.FundSearch;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Sitecore.Mvc.Extensions;
using System.Web;


namespace Feature.Wealth.Component.Controllers
{
    public class FundSearchController : Controller
    {
        private FundSearchRepository _fundsearchrepository = new FundSearchRepository();
        private FundTagRepository _tagrepository = new FundTagRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var hotkeywordtags = ItemUtils.GetMultiListValueItems(dataSourceItem, Template.FundSearch.Fields.HotKeywordtags);
            var keywords = new List<string>();
            foreach (var item in hotkeywordtags)
            {
                string key = ItemUtils.GetFieldValue(item, Template.FundSearch.Fields.TagName);
                keywords.Add(key);
            }

            var hotproducttags = ItemUtils.GetMultiListValueItems(dataSourceItem, Template.FundSearch.Fields.HotProductags);
            var products = new List<string>();
            foreach (var item in hotproducttags)
            {
                string product = ItemUtils.GetFieldValue(item, Template.FundSearch.Fields.TagName);
                products.Add(product);
            }
            string content = ItemUtils.GetFieldValue(dataSourceItem, Template.FundSearch.Fields.Content);
            HtmlString riskcontent = ItemUtils.Field(dataSourceItem, Template.FundSearch.Fields.RiskIndicatorContent);

            var topicnames = _tagrepository.GetFundTenTagNameData();

            var items = _fundsearchrepository.GetFundSearchData();

            var regions = items
                        .OrderBy(f=>f.InvestmentRegionID)
                        .SelectMany(f =>
                        {
                            if (!string.IsNullOrEmpty(f.InvestmentRegionName))
                            {
                                return f.InvestmentRegionName.Split(',');
                            }
                            else
                            {
                                return new string[] { }; 
                            }
                        })
                        .Where(r => !string.IsNullOrWhiteSpace(r))
                        .Distinct()
                        .ToList();



            var searchbar = new SearchBarData
            {
                Currencies = items.OrderBy(f=>f.CurrencyCode).Select(f=>f.CurrencyName).Distinct().ToList(),
                FundCompanies = items.Where(f => f.FundCompanyName != null).OrderBy(f=>f.FundCompanyName).Select(f => f.FundCompanyName).Distinct().ToList(),
                InvestmentRegions = regions,
                InvestmentTargets = items.OrderBy(t => t.InvestmentTargetID).Select(f => f.InvestmentTargetName).Distinct().ToList(),
                FundTypeNames = items.OrderBy(f=>f.FormatFundType).Select(f => f.FormatFundType).Distinct().ToList()
            };

            var viewModel = new FundSearchViewModel
            {
                Item = dataSourceItem,
                FundSearchData = items,
                SearchBarData = searchbar,
                HotKeywordTags = keywords,
                HotProductTags = products,
                TopicNameTags = topicnames,
                Content = content,
                RiskIndicatorContent = riskcontent
            };

            return View("/Views/Feature/Wealth/Component/FundSearch/FundSearch.cshtml", viewModel);
        }


        /// <summary>
        /// 搜尋回傳值
        /// </summary>
        [HttpPost]
        public ActionResult GetAllFunds()
        {
            var items = _fundsearchrepository.GetFundSearchData();
            var funds = _fundsearchrepository.GetFundRenderData(items);
            return new JsonNetResult(funds);
        }

        /// <summary>
        ///autocomplete串接資料
        /// </summary>
        [HttpGet]
        public JsonResult GetFundNames()
        {
            var fundItems = _fundsearchrepository.GetAutoCompleteData();
            return Json(fundItems, JsonRequestBehavior.AllowGet);
        }


    }
}
using Feature.Wealth.Component.Models.WeeklyAndMonthlyReports;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class WeeklyAndMonthlyReportsController : Controller
    {
        public ActionResult Index()
        {
            var dataSource = RenderingContext.CurrentOrNull?.Rendering?.Item;

            var reportsData = new WeeklyAndMonthlyReportsModel();

            if (dataSource != null && dataSource.TemplateID.ToString() == Templates.WeeklyAndMonthlyReportsDatasource.Id.ToString())
            {
                reportsData.DataSource = dataSource;
                reportsData.Title = ItemUtils.GetFieldValue(dataSource, Templates.WeeklyAndMonthlyReportsDatasource.Fields.Title);
                reportsData.DataSourceId = dataSource.ID.ToString();
            }
            else
            {
                reportsData = null;
            }

            return View("/Views/Feature/Wealth/Component/WeeklyAndMonthlyReports/WeeklyAndMonthlyReports.cshtml", reportsData);
        }

        [HttpPost]
        public JsonResult GetReportsData(string id)
        {
            var datas = new List<ReportsItem>();

            if (!ID.TryParse(id, out var guid))
            {
                return new JsonNetResult(datas);
            }

            Item dataSource = Sitecore.Context.Database.GetItem(guid);

            var children = ItemUtils.GetChildren(dataSource, Templates.WeeklyAndMonthlyReportsItem.Id);

            foreach (var child in children)
            {
                var reportsItem = new ReportsItem();

                var date = ItemUtils.GetLocalDateFieldValue(child, Templates.WeeklyAndMonthlyReportsItem.Fields.Date) ?? DateTime.MinValue;

                reportsItem.ReportsType = ItemUtils.IsChecked(child, Templates.WeeklyAndMonthlyReportsItem.Fields.IsMonthlyReports) ? "月報" : "週報";
                reportsItem.ReportsTitle = ItemUtils.GetFieldValue(child, Templates.WeeklyAndMonthlyReportsItem.Fields.Title);
                reportsItem.ReportsDate = date != DateTime.MinValue ? date.ToString("yyyy/MM/dd") : string.Empty;
                reportsItem.ReportsLink = ItemUtils.GeneralLink(child, Templates.WeeklyAndMonthlyReportsItem.Fields.Link).Url;
                reportsItem.ReportsTarget = ItemUtils.GeneralLink(child, Templates.WeeklyAndMonthlyReportsItem.Fields.Link).Target;
                reportsItem.ReportsLinkTitle = ItemUtils.GeneralLink(child, Templates.WeeklyAndMonthlyReportsItem.Fields.Link).Title;

                datas.Add(reportsItem);
            }

            return new JsonNetResult(datas);
        }
    }
}
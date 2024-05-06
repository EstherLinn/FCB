using Feature.Wealth.Component.Models.SmallNotice;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class SmallNoticeController : Controller
    {
        public ActionResult Index()
        {
            var dataSource = RenderingContext.CurrentOrNull?.Rendering.Item;
            var mainTitle = ItemUtils.GetFieldValue(dataSource, Templates.SmallNoticeDatasource.Fields.MainTitle);
            var content = ItemUtils.GetFieldValue(dataSource, Templates.SmallNoticeDatasource.Fields.Content);

            var model = new SmallNoticeModel()
            {
                DataSource = RenderingContext.CurrentOrNull?.Rendering.Item,
                MainTitle = mainTitle,
                Content = content
            };

            return View("/Views/Feature/Wealth/Component/SmallNotice/SmallNotice.cshtml", model);
        }
    }
}
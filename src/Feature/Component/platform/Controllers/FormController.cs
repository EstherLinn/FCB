using Feature.Wealth.Component.Models.Form;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class FormController : Controller
    {
        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            var childItems = ItemUtils.GetChildren(dataSourceItem).ToList();

            var items = new List<FormModel.Form>();

            foreach (var childItem in childItems)
            {
                items.Add(new FormModel.Form(childItem));
            }

            var model = new FormModel
            {
                DataSource = dataSourceItem,
                Items = items
            };

            return View("/Views/Feature/Wealth/Component/Form/Form.cshtml", model);
        }
    }
}

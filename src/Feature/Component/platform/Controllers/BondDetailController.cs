using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.Bond;
using Feature.Wealth.Component.Models.USStock;
using Feature.Wealth.Component.Repositories;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class BondDetailController : Controller
    {
        private readonly BondRepository _bondRepository = new BondRepository();

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            string id = Sitecore.Web.WebUtil.GetSafeQueryString("id");

            var bond = this._bondRepository.GetBond(id);

            return View("/Views/Feature/Wealth/Component/Bond/BondDetail.cshtml", CreateModel(item, bond));
        }

        private BondDetailModel CreateModel(Item item, Bond bond)
        {
            var model = new BondDetailModel
            {
                Item = item,
                BondDetail = bond
            };

            return model;
        }
    }
}

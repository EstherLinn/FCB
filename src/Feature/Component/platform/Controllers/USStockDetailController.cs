using System.Web.Mvc;
using Feature.Wealth.Component.Models.USStock;
using Feature.Wealth.Component.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;


namespace Feature.Wealth.Component.Controllers
{
    public class USStockDetailController : Controller
    {
        private readonly USStockRepository _uSStockRepository = new USStockRepository();

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            string firstBankCode = Sitecore.Web.WebUtil.GetSafeQueryString("id");

            this._uSStockRepository.TriggerViewCountRecord(firstBankCode);

            var uSStock = this._uSStockRepository.GetUSStock(firstBankCode);

            return View("/Views/Feature/Wealth/Component/USStock/USStockDetail.cshtml", CreateModel(item, uSStock));
        }

        protected USStockDetailModel CreateModel(Item item, USStock uSStock)
        {
            var model = new USStockDetailModel
            {
                USStock = uSStock
            };

            return model;
        }
    }
}
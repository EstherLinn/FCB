using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.IndexRecommendation;
using System.Linq;

namespace Feature.Wealth.Component.Controllers
{
    public class IndexRecommendationController : Controller
    {
        private IndexRecommendationRepository _repository = new IndexRecommendationRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var funds = _repository.GetFundData();
            var viewModel = new IndexRecommendationModel
            {
                Item = dataSourceItem,
                HotFunds=funds,
                FundDetailLink = FundRelatedSettingModel.GetFundDetailsUrl(),
                NetAssetValueDate = funds.Select(f => f.NetAssetValueDate).FirstOrDefault()
            };

            return View("/Views/Feature/Wealth/Component/IndexRecommendation/IndexRecommendation.cshtml", viewModel);
        }
    }
}

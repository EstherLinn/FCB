using Feature.Wealth.Component.Models.GraphicTwoCards;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class GraphicTwoCardsController : Controller
    {
        public ActionResult Index()
        {
            var dataSource = RenderingContext.CurrentOrNull?.Rendering.Item;

            var imageUrl1 = ItemUtils.ImageUrl(dataSource, Templates.GraphicTwoCardsDatasource.Fields.Image1);
            var title1 = ItemUtils.GetFieldValue(dataSource, Templates.GraphicTwoCardsDatasource.Fields.Title1);
            var content1 = ItemUtils.GetFieldValue(dataSource, Templates.GraphicTwoCardsDatasource.Fields.Content1);
            var buttonText1 = ItemUtils.GetFieldValue(dataSource, Templates.GraphicTwoCardsDatasource.Fields.ButtonText1);
            var buttonLink1 = ItemUtils.GeneralLink(dataSource, Templates.GraphicTwoCardsDatasource.Fields.ButtonLink1).Url;
            var imageUrl2 = ItemUtils.ImageUrl(dataSource, Templates.GraphicTwoCardsDatasource.Fields.Image2);
            var title2 = ItemUtils.GetFieldValue(dataSource, Templates.GraphicTwoCardsDatasource.Fields.Title2);
            var content2 = ItemUtils.GetFieldValue(dataSource, Templates.GraphicTwoCardsDatasource.Fields.Content2);
            var buttonText2 = ItemUtils.GetFieldValue(dataSource, Templates.GraphicTwoCardsDatasource.Fields.ButtonText2);
            var buttonLink2 = ItemUtils.GeneralLink(dataSource, Templates.GraphicTwoCardsDatasource.Fields.ButtonLink2).Url;

            var model = new GraphicTwoCardsModel()
            {
                DataSource = dataSource,
                ImageUrl1 = imageUrl1,
                Title1 = title1,
                Content1 = content1,
                ButtonText1 = buttonText1,
                ButtonLink1 = buttonLink1,
                ImageUrl2 = imageUrl2,
                Title2 = title2,
                Content2 = content2,
                ButtonText2 = buttonText2,
                ButtonLink2 = buttonLink2,
            };

            return View("/Views/Feature/Wealth/Component/GraphicTwoCards/GraphicTwoCards.cshtml", model);
        }
    }
}

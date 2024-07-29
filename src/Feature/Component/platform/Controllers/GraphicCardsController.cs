using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Component.Models.GraphicCards;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class GraphicCardsController : Controller
    {
        public ActionResult Graphic3Cards()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var imageUrl1 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.Image1);
            var imageUrl2 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.Image2);
            var imageUrl3 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.Image3);
            var buttonLink1 = ItemUtils.GeneralLink(item, Templates.GraphicCards.Fields.ButtonLink1)?.Url;
            var buttonLink2 = ItemUtils.GeneralLink(item, Templates.GraphicCards.Fields.ButtonLink2)?.Url;
            var buttonLink3 = ItemUtils.GeneralLink(item, Templates.GraphicCards.Fields.ButtonLink3)?.Url;
            var buttonText1 = ItemUtils.GetFieldValue(item, Templates.GraphicCards.Fields.ButtonText1);
            var buttonText2 = ItemUtils.GetFieldValue(item, Templates.GraphicCards.Fields.ButtonText2);
            var buttonText3 = ItemUtils.GetFieldValue(item, Templates.GraphicCards.Fields.ButtonText3);
            var isOpenLoginLightBox1 = ItemUtils.IsChecked(item, Templates.GraphicCards.Fields.OpenLoginLightBox1);
            var isOpenLoginLightBox2 = ItemUtils.IsChecked(item, Templates.GraphicCards.Fields.OpenLoginLightBox2);
            var isOpenLoginLightBox3 = ItemUtils.IsChecked(item, Templates.GraphicCards.Fields.OpenLoginLightBox3);

            var model = new GraphicCardsModel()
            {
                Item = item,
                ImageUrl1 = imageUrl1,
                ImageUrl2 = imageUrl2,
                ImageUrl3 = imageUrl3,
                ButtonLink1 = buttonLink1,
                ButtonLink2 = buttonLink2,
                ButtonLink3 = buttonLink3,
                ButtonText1 = buttonText1,
                ButtonText2 = buttonText2,
                ButtonText3 = buttonText3,
                IsOpenLoginLightBox1 = isOpenLoginLightBox1,
                IsOpenLoginLightBox2 = isOpenLoginLightBox2,
                IsOpenLoginLightBox3 = isOpenLoginLightBox3,
                IsLogin = FcbMemberHelper.CheckMemberLogin()
            };

            return View("/Views/Feature/Wealth/Component/GraphicCards/Graphic3Cards.cshtml", model);
        }
    }
}

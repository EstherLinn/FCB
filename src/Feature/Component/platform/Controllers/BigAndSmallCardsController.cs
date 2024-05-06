using Feature.Wealth.Component.Models.BigAndSmallCards;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class BigAndSmallCardsController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var imageUrl1 = ItemUtils.ImageUrl(item, Templates.BigAndSmallCards.Fields.Image1);
            var imageUrl2 = ItemUtils.ImageUrl(item, Templates.BigAndSmallCards.Fields.Image2);
            var imageUrl3 = ItemUtils.ImageUrl(item, Templates.BigAndSmallCards.Fields.Image3);
            var buttonLink1 = ItemUtils.GeneralLink(item, Templates.BigAndSmallCards.Fields.ButtonLink1)?.Url;
            var buttonLink2 = ItemUtils.GeneralLink(item, Templates.BigAndSmallCards.Fields.ButtonLink2)?.Url;
            var buttonLink3 = ItemUtils.GeneralLink(item, Templates.BigAndSmallCards.Fields.ButtonLink3)?.Url;
            var bigImageUrl = ItemUtils.ImageUrl(item, Templates.BigAndSmallCards.Fields.BigImage);
            var bigButtonText1 = ItemUtils.GetFieldValue(item, Templates.BigAndSmallCards.Fields.BigButtonText1);
            var bigButtonText2 = ItemUtils.GetFieldValue(item, Templates.BigAndSmallCards.Fields.BigButtonText2);
            var bigButtonLink1 = ItemUtils.GeneralLink(item, Templates.BigAndSmallCards.Fields.BigButtonLink1)?.Url;
            var bigButtonLink2 = ItemUtils.GeneralLink(item, Templates.BigAndSmallCards.Fields.BigButtonLink2)?.Url;

            var model = new BigAndSmallCardsModel()
            {
                Item = item,
                ImageUrl1 = imageUrl1,
                ImageUrl2 = imageUrl2,
                ImageUrl3 = imageUrl3,
                ButtonLink1 = buttonLink1,
                ButtonLink2 = buttonLink2,
                ButtonLink3 = buttonLink3,
                BigImageUrl = bigImageUrl,
                BigButtonText1 = bigButtonText1,
                BigButtonText2 = bigButtonText2,
                BigButtonLink1 = bigButtonLink1,
                BigButtonLink2 = bigButtonLink2,
            };

            return View("/Views/Feature/Wealth/Component/BigAndSmallCards/BigAndSmallCards.cshtml", model);
        }
    }
}

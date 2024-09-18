using Feature.Wealth.Component.Models.VideoCarousel;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class VideoCarouselController : Controller
    {
        public ActionResult VideoCarousel()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            bool checkedShowIcon = ItemUtils.IsChecked(dataSourceItem, Templates.VideoCarouselIndex.Fields.ShowIcon);
            var imageUrl = ItemUtils.ImageUrl(dataSourceItem, Templates.VideoCarouselIndex.Fields.Image);
            var linkText = ItemUtils.GetFieldValue(dataSourceItem, Templates.VideoCarouselIndex.Fields.LinkText);
            var linkUrl = ItemUtils.GeneralLink(dataSourceItem, Templates.VideoCarouselIndex.Fields.Link)?.Url;
            var childItems = ItemUtils.GetChildren(dataSourceItem);

            var items = new List<VideoCarouselModel.VideoCarousel>();

            foreach (var childItem in childItems ?? [])
            {
                var vimageUrl = ItemUtils.ImageUrl(childItem, Templates.VideoCarouselVideos.Fields.Image);
                bool vcheckedShowIcon = ItemUtils.IsChecked(childItem, Templates.VideoCarouselVideos.Fields.ShowIcon);
                bool vcheckedOpenVideoLinkinLightBox = ItemUtils.IsChecked(childItem, Templates.VideoCarouselVideos.Fields.OpenVideoLinkinLightBox);

                items.Add(new VideoCarouselModel.VideoCarousel(childItem)
                {
                    ImageUrl = vimageUrl,
                    CheckedShowIcon = vcheckedShowIcon,
                    CheckedOpenVideoLinkinLightBox = vcheckedOpenVideoLinkinLightBox,
                });
            }

            var model = new VideoCarouselModel
            {
                DataSource = dataSourceItem,
                CheckedShowIcon = checkedShowIcon,
                ImageUrl = imageUrl,
                LinkUrl = linkUrl,
                LinkText = linkText,
                Items = items
            };

            return View("/Views/Feature/Wealth/Component/VideoCarousel/VideoCarousel.cshtml", model);
        }
    }
}

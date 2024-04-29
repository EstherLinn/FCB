using Feature.Wealth.Component.Models.ArticleContent;
using Sitecore.Mvc.Presentation;
using System;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class ArticleContentController : Controller
    {
        public ActionResult Index()
        {
            var dataSource = RenderingContext.CurrentOrNull?.Rendering.Item;
            var mainTitle = ItemUtils.GetFieldValue(dataSource, Templates.ArticleContentDatasource.Fields.MainTitle);
            var date = ItemUtils.GetLocalDateFieldValue(dataSource, Templates.ArticleContentDatasource.Fields.Date) ?? DateTime.MinValue;
            var image = ItemUtils.ImageUrl(dataSource, Templates.ArticleContentDatasource.Fields.Image);
            var content1 = ItemUtils.GetFieldValue(dataSource, Templates.ArticleContentDatasource.Fields.Content1);
            var introduction = ItemUtils.GetFieldValue(dataSource, Templates.ArticleContentDatasource.Fields.Introduction);
            var content2 = ItemUtils.GetFieldValue(dataSource, Templates.ArticleContentDatasource.Fields.Content2);

            var model = new ArticleContentModel()
            {
                DataSource = dataSource,
                MainTitle = mainTitle,
                Date = date != DateTime.MinValue ? date.ToString("yyyy/MM/dd") : string.Empty,
                Image = image,
                Content1 = content1,
                Introduction = introduction,
                Content2 = content2
            };

            return View("/Views/Feature/Wealth/Component/ArticleContent/ArticleContent.cshtml", model);
        }
    }
}
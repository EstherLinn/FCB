using System.Web.Mvc;
using Feature.Wealth.Component.Models.JoinMemberInfo;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class JoinMemberInfoController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            return View("/Views/Feature/Wealth/Component/JoinMemberInfo/JoinMemberInfo.cshtml", CreateModel(item));
        }

        protected JoinMemberInfoModel CreateModel(Item item)
        {
            var model = new JoinMemberInfoModel
            {
                Item = item,
                Image1 = ItemUtils.ImageUrl(item, Template.JoinMemberInfo.Fields.Image1),
                Image2 = ItemUtils.ImageUrl(item, Template.JoinMemberInfo.Fields.Image2),
                Image3 = ItemUtils.ImageUrl(item, Template.JoinMemberInfo.Fields.Image3),
            };

            return model;
        }
    }
}
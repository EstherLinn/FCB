using Feature.Wealth.Component.Models.JoinMemberInfo;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Weakth.Component.Controllers
{
    public class JoinMemberInfoController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            return View("/Views/Wealth/JoinMemberInfo/JoinMemberInfo.cshtml", CreateModel(item));
        }

        protected JoinMemberInfoModel CreateModel(Item item)
        {
            var model = new JoinMemberInfoModel
            {
                Item = item,
                Image1 = ItemUtils.ImageUrl(item, Template.JoinMemberInfo.Fields.Image1),
                Image1_3X = ItemUtils.ImageUrl(item, Template.JoinMemberInfo.Fields.Image1_3X),
                Image2 = ItemUtils.ImageUrl(item, Template.JoinMemberInfo.Fields.Image2),
                Image2_3X = ItemUtils.ImageUrl(item, Template.JoinMemberInfo.Fields.Image2_3X),
                Image3 = ItemUtils.ImageUrl(item, Template.JoinMemberInfo.Fields.Image3),
                Image3_3X = ItemUtils.ImageUrl(item, Template.JoinMemberInfo.Fields.Image3_3X)
            };

            return model;
        }
    }
}
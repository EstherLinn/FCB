using Feature.Wealth.Component.Models.HeaderWidget;
using Newtonsoft.Json;
using Sitecore.Web;
using System.Web.Mvc;
namespace Feature.Wealth.Component.Controllers
{
    public class HeaderWidgetController : Controller
    {
        public ActionResult Index()
        {
            if (Sitecore.Context.IsLoggedIn)
            {
                //var temp = TempData.Peek("MemberInfo");
                //var member = JsonConvert.DeserializeObject<FcbMemberModel>(WebUtil.GetCookieValue("MemberInfo"));
                //var value = JsonConvert.DeserializeObject<string>(Sitecore.Context.User.Profile.GetCustomProperty("MemberInfo"));
                //WebUtil.SetCookieValue("MemberInfo", value);
            }
            return View("/Views/Feature/Wealth/Component/HeaderWidget/HeaderWidget.cshtml", CreateModel());
        }

        protected HeaderWidgetModel CreateModel()
        {
            var model = new HeaderWidgetModel();
            return model;
        }
    }
}
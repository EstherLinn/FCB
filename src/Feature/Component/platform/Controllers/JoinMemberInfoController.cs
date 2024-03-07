using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.JoinMemberInfo;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Feature.Weakth.Component.Controllers
{
    public class JoinMemberInfoController : Controller
    {
        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.Current.Rendering.Item;

            return View("/Views/Wealth/JoinMemberInfo/JoinMemberInfo.cshtml", CreateModel(dataSourceItem));
        }

        protected JoinMemberInfoModel CreateModel(Item dataSourceItem)
        {
            var model = new JoinMemberInfoModel();

            model.Item = dataSourceItem;

            return model;
        }
    }
}
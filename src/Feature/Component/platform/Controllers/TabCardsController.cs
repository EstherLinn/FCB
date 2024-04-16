using Feature.Wealth.Component.Models.TabCards;
using Feature.Wealth.Component.Repositories;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class TabCardsController : Controller
    {
        private readonly TabCardsRepository _tabCardsRepository = new TabCardsRepository();

        /// <summary>
        /// 首頁大牌卡元件
        /// </summary>
        public ActionResult TabCard()
        {
            Item datasource = null;

            if (Guid.TryParse(RenderingContext.Current.Rendering.DataSource, out Guid guid) && ID.TryParse(guid, out ID id))
            {
                datasource = Sitecore.Context.Database.GetItem(id);
            }

            var model= new TabCardModel(datasource);

            model.FundCardsInfos = _tabCardsRepository.GetFundCardsInfos((List<string>)model.FundIDList);
            model.FundCardsNavs = _tabCardsRepository.GetFundCardsNavs((List<string>)model.FundIDList);

            return View("/Views/Feature/Wealth/Component/TabCards/TabCard.cshtml", model);
        }

        /// <summary>
        /// 首頁三牌卡元件
        /// </summary>
        public ActionResult Tab3Card()
        {
            Item datasource = null;

            if (Guid.TryParse(RenderingContext.Current.Rendering.DataSource, out Guid guid) && ID.TryParse(guid, out ID id))
            {
                datasource = Sitecore.Context.Database.GetItem(id);
            }

            var model = new Tab3CardModel(datasource);

            model.FundCardsInfos = _tabCardsRepository.GetFundCardsInfos((List<string>)model.FundIDList);
            model.FundCardsNavs = _tabCardsRepository.GetFundCardsNavs((List<string>)model.FundIDList);

            return View("/Views/Feature/Wealth/Component/TabCards/Tab3Card.cshtml", model);
        }
    }
}
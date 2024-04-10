using Feature.Wealth.Component.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Linq;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class DiscountListController : Controller
    {
        public ActionResult Index()
        {
            Item dataSource = RenderingContext.CurrentOrNull?.Rendering.Item;

            var dataSourceId = dataSource?.ID.ToString();

            DiscountListRepository discountListRepository = new DiscountListRepository();

            var discountList = discountListRepository.GetCardList(dataSource, dataSourceId, "9");

            return View("/Views/Feature/Wealth/Component/DiscountList/DiscountList.cshtml", discountList);
        }

        [HttpPost]
        public ActionResult UpdatePageSize(string dataSourceId, string pageSize)
        {
            DiscountListRepository discountListRepository = new DiscountListRepository();

            Item dataSource = Sitecore.Context.Database.GetItem(dataSourceId);

            var discountList = discountListRepository.GetCardList(dataSource, dataSourceId, pageSize);

            var jsonData = new
            {
                Guid = discountList?.Guid,
                TotalPages = discountList?.TotalPages,
                TotalCards = discountList?.TotalCards,

                CardList = discountList?.CardList?.Select(cardList => new
                {
                    CardListClass = cardList.CardListClass,
                    CardItems = cardList.CardItems?.Select(cardItem => new
                    {
                        CardImage = cardItem.CardImage,
                        Title = cardItem.Title,
                        DisplayDate = cardItem.DisplayDate,
                        DisplayTag = cardItem.DisplayTag,
                        CardButtonText = cardItem.CardButtonText,
                        CardButtonLink = cardItem.CardButtonLink,
                        ExpiryMask = cardItem.ExpiryMask
                    })
                })
            };

            return Json(new
            {
                data = jsonData
            }) ;
        }
    }
}
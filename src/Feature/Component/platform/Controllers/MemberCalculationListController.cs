using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Component.Models.MemberCalculationList;
using Feature.Wealth.Component.Repositories;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class MemberCalculationListController : Controller
    {
        private readonly MemberCalculationListRepository _memberCalculationListRepository = new MemberCalculationListRepository();

        public ActionResult Index()
        {
            MemberCalculationListModel model;

            if (!FcbMemberHelper.CheckMemberLogin())
            {
                model = null;
            }
            else
            {
                model = _memberCalculationListRepository.GetMemberCalculationList();
            }

            return View("/Views/Feature/Wealth/Component/MemberCalculationList/MemberCalculationList.cshtml", model);
        }
    }
}
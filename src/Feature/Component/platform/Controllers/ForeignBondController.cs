using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class ForeignBondController : Controller
    {

        /// <summary>
        ///  債券梯
        /// </summary>
        public ActionResult BondLadder()
        {
            return View("/Views/Feature/Wealth/Component/ForeignBond/BondLadder.cshtml");
        }

        /// <summary>
        ///  啞鈴式投資
        /// </summary>
        public ActionResult BarbellApproach()
        {
            return View("/Views/Feature/Wealth/Component/ForeignBond/BarbellApproach.cshtml");
        }
    }
}

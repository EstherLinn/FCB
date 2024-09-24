using Feature.Wealth.Component.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class InterestedFundController : Controller
    {
        private readonly InterestedFundRepository _interestedFundRepository = new InterestedFundRepository ();
        public ActionResult Index()
        {
            return View("/Views/Feature/Wealth/Component/InterestedFund/InterestedFund.cshtml");
        }

        [HttpPost]
        public ActionResult GetInterestedFund()
        {
            return new JsonNetResult(_interestedFundRepository.GetFundData());
        }
    }
}


using System;
using System.Web.Mvc;
using Feature.Wealth.Account.Filter;
using Feature.Wealth.Account.Models.ReachInfo;
using Feature.Wealth.Account.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Account.Controllers
{
    public class ReachInfoController : JsonNetController
    {
        private readonly ReachInfoRepository _reachInfoRepository;

        public ReachInfoController()
        {
            this._reachInfoRepository = new ReachInfoRepository();
        }
        [HttpPost]
        public ActionResult SetReachInfo(ReachInfo reachInfo)
        {
            object returnObj = new
              {
                  success = _reachInfoRepository.SetReachInfo(reachInfo)
              };

            return new JsonNetResult(returnObj);
        }
    }
}
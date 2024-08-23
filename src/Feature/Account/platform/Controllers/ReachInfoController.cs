
using System;
using System.Web.Mvc;
using Feature.Wealth.Account.Filter;
using Feature.Wealth.Account.Helpers;
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
        [MemberAuthenticationFilter]
        public ActionResult SetReachInfo(ReachInfo reachInfo)
        {
            var obj = new
            {
                success = false,
                errormsg = string.Empty
            };
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new JsonNetResult(obj);
            }
            if (string.IsNullOrEmpty(FcbMemberHelper.GetMemberAllInfo().MemberEmail))
            {
                return new JsonNetResult(obj = new{ success = false, errormsg = "請先設定您的電子信箱。"});
            }
            obj = new
            {
                success = _reachInfoRepository.SetReachInfo(reachInfo),
                errormsg = string.Empty
            };

            return new JsonNetResult(obj);
        }
    }
}
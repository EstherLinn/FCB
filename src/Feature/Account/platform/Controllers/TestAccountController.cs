
using Feature.Wealth.Account.Models.OAuth;
using Feature.Wealth.Account.Repositories;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Security.Accounts;
using System;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using static Xcms.Sitecore.Foundation.Basic.SitecoreExtensions.MemberUtils;

namespace Feature.Wealth.Account.Controllers
{

    public class TestAccountController : JsonNetController
    {
        private readonly MemberRepository _memberRepository;

        public TestAccountController()
        {
            this._memberRepository = new MemberRepository();
        }
        public ActionResult Index()
        {
            var id = Settings.GetSetting("StressTestId");

            string isEmployee = Sitecore.Web.WebUtil.GetSafeQueryString("IsEmployee");
            string isManager = Sitecore.Web.WebUtil.GetSafeQueryString("IsManager");

            FcbMemberModel member = _memberRepository.GetMemberInfo(PlatFormEunm.WebBank, id);
            User user = Authentication.BuildVirtualUser("extranet", member.WebBankId, true);
            user.Profile.Name = member.MemberName;
            user.Profile.Email = member.MemberEmail;

            member.IsEmployee = isEmployee == "1";
            member.IsManager = isManager == "1";

            string objToJson = JsonConvert.SerializeObject(member);
            user.Profile.SetCustomProperty("MemberInfo", objToJson);
            user.Profile.Save();
            Authentication.LoginVirtualUser(user);
            return new EmptyResult();
        }
    }
}

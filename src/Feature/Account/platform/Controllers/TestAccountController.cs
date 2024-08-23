
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
            FcbMemberModel member = new FcbMemberModel(id, "一銀測試", "fcb@fcb.com", "1", "忠孝路分行經辦１", "00944114", true, true,
                QuoteChangeEunm.Taiwan, PlatFormEunm.WebBank, id, new DateTime(1990,7,20),"1", "Y");
            User user = Authentication.BuildVirtualUser("extranet", member.WebBankId, true);
            user.Profile.Name = member.MemberName;
            user.Profile.Email = member.MemberEmail;
            string objToJson = JsonConvert.SerializeObject(member);
            user.Profile.SetCustomProperty("MemberInfo", objToJson);
            user.Profile.Save();
            Authentication.LoginVirtualUser(user);
            return new EmptyResult();
        }
    }
}

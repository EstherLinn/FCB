
using Feature.Wealth.Account.Models.OAuth;
using Feature.Wealth.Account.Repositories;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Security.Accounts;
using System;
using System.ComponentModel;
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
        public enum TestRoleType
        {
            [Description("普通會員無理顧")]
            NormalUserWithOutAdvisror = 1,
            [Description("普通會員有理顧")]
            NormalUserWithAdvisror = 2,
            [Description("理顧")]
            Advisror = 3,
            [Description("主管")]
            Supervisor = 4
        }
        public ActionResult Index()
        {
            //預設登入普通會員無理顧
            var id = Settings.GetSetting("StressTestId");
            string roleType = Sitecore.Web.WebUtil.GetSafeQueryString("RoleType");
            int roleTypeInt;
            bool success = int.TryParse(roleType, out roleTypeInt);
            
            if (success)
            {
                switch (roleTypeInt)
                {
                    case 1:
                        id = "A1231231230";
                        break;
                    case 2:
                        id = "B1231231230";
                        break;
                    case 3:
                        id = "L1234567890";
                        break;
                    case 4:
                        id = "M1234567890";
                        break;
                }
            }
            FcbMemberModel member = _memberRepository.GetMemberInfo(PlatFormEunm.WebBank, id);
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

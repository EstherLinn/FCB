using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.OAuth;
using Feature.Wealth.Account.Pipelines.DI;
using Feature.Wealth.Account.Repositories;
using Newtonsoft.Json;
using Sitecore.DependencyInjection;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Security.Accounts;
using System.Web;
using static Xcms.Sitecore.Foundation.Basic.SitecoreExtensions.MemberUtils;

namespace Feature.Wealth.Account.Pipelines
{
   public class AppLogin : HttpRequestProcessor
    {
        private readonly MemberRepository _memberRepository = new();
        private IMemberLoginService _memberLoginService;

        public AppLogin(IMemberLoginService service)
        {
            _memberLoginService = service;
        }
        public override void Process(HttpRequestArgs args)
        {
            if (args!= null)
            {
                if (FcbMemberHelper.CheckMemberLogin())
                {
                    return;
                }
                var uri = args.RequestUrl;
                var qs = HttpUtility.ParseQueryString(uri.Query);
                var queryString = args.RequestUrl.Query;
                if (!string.IsNullOrEmpty(queryString) && !string.IsNullOrEmpty(qs["promotionCode"]) && !string.IsNullOrEmpty(qs["rtCode"]) && qs["rtCode"] == "0000")
                {
                    _memberLoginService.IsAppLogin = true ;
                    var code = qs["promotionCode"];
                    var isExists =  _memberRepository.CheckAppUserExists(PlatFormEunm.WebBank, code);
                    if (!isExists)
                    {
                        //創建User
                        CIFMember cifMember = _memberRepository.GetAppUserInfo(code);
                        if (cifMember == null)
                        {
                            _memberLoginService.AppLoginSuccess = false;
                            return;
                        }
                        FcbMemberModel member = new FcbMemberModel(cifMember.CIF_PROMO_CODE, cifMember.CIF_CUST_NAME, cifMember.CIF_E_MAIL_ADDRESS,
                        cifMember.CIF_EMP_RISK, cifMember.CIF_AO_EMPName, true, true, QuoteChangeEunm.Taiwan, PlatFormEunm.WebBank, cifMember.CIF_PROMO_CODE);
                        _memberRepository.CreateNewMember(member);
                        User user = Authentication.BuildVirtualUser("extranet", cifMember.CIF_PROMO_CODE, true);
                        user.Profile.Name = cifMember.CIF_CUST_NAME;
                        user.Profile.Email = cifMember.CIF_E_MAIL_ADDRESS;
                        string objToJson = JsonConvert.SerializeObject(member);
                        user.Profile.SetCustomProperty("MemberInfo", objToJson);
                        user.Profile.Save();
                        Authentication.LoginVirtualUser(user);
                        _memberLoginService.AppLoginSuccess = true;
                    }
                    else
                    {
                        FcbMemberModel member = _memberRepository.GetAppMemberInfo(PlatFormEunm.WebBank, code);
                        User user = Authentication.BuildVirtualUser("extranet", member.WebBankId, true);
                        user.Profile.Name = member.MemberName;
                        user.Profile.Email = member.MemberEmail;
                        string objToJson = JsonConvert.SerializeObject(member);
                        user.Profile.SetCustomProperty("MemberInfo", objToJson);
                        user.Profile.Save();
                        Authentication.LoginVirtualUser(user);
                        _memberLoginService.AppLoginSuccess = true;
                    }
                }
            }

        }

        
    }
}

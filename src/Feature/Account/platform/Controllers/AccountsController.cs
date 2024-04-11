using Feature.Wealth.Account.Models.OAuth;
using Feature.Wealth.Account.Services;
using Feature.Wealth.Component.Models.HeaderWidget;
using Newtonsoft.Json;
using Sitecore.Security.Accounts;
using Sitecore.Web;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using static Xcms.Sitecore.Foundation.Basic.SitecoreExtensions.MemberUtils;

namespace Feature.Wealth.Account.Controllers
{
    public class AccountsController : JsonNetController
    {
        private LineService lineService { get; }
        private FacebookService facebookService { get; }

        private string callBackUrl { get; }

        public AccountsController()
        {
            this.lineService = new LineService();
            this.facebookService = new FacebookService();
            this.callBackUrl = WebUtil.GetCookieValue("NowUrl");
        }
        [HttpGet]
        public async Task<ActionResult> SignInLine(string code, string error, string error_description)
        {
            var responseToken = await lineService.GetTokensByCode(code);
            var responseProfile = await lineService.GetProfileByToken(responseToken.AccessToken);
            var responseVerify = await lineService.GetVerifyAccessToken(responseToken.IdToken, responseProfile.UserId);

            User user =  Authentication.BuildVirtualUser("extranet", responseProfile.UserId, true);
            user.Profile.Name = responseVerify.Name;
            user.Profile.Email = responseVerify.Email;
            FcbMemberModel member = new FcbMemberModel()
            {
                MemberName = responseVerify.Name,
                MemberEmail = responseVerify.Email,
                PlatForm = nameof(PlatFormEunm.Line),
                PlatFormId = responseProfile.UserId
            };
            string objToJson = JsonConvert.SerializeObject(member);
            user.Profile.Comment = objToJson;
            user.Profile.Save();
            Authentication.LoginVirtualUser(user);
            WebUtil.SetCookieValue("MemberInfo", objToJson, DateTime.Now.AddMinutes(60));
            TempData["MemberInfo"] = objToJson;
            return Redirect(callBackUrl);
        }
        [HttpGet]
        public async Task<ActionResult> SignInFacebook(string code, string error, string error_description)
        {
            var responseToken = await facebookService.GetTokensByCode(code);
            var responseProfile = await facebookService.GetProfileByToken(responseToken.AccessToken);

            User user = Authentication.BuildVirtualUser("extranet", responseProfile.Id, true);
            user.Profile.Name = responseProfile.Name;
            user.Profile.FullName = responseProfile.Name;
            user.Profile.Email = responseProfile.Email;

            FcbMemberModel member = new FcbMemberModel()
            {
                MemberName = responseProfile.Name,
                MemberEmail = responseProfile.Email,
                PlatForm = nameof(PlatFormEunm.FaceBook),
                PlatFormId = responseProfile.Id
            };
            string objToJson = JsonConvert.SerializeObject(member);
            user.Profile.Comment = objToJson;
            user.Profile.SetCustomProperty("MemberInfo", objToJson);
            user.Profile.Save();
            WebUtil.SetCookieValue("MemberInfo", objToJson, DateTime.MinValue,true);
            TempData["MemberInfo"] = objToJson;
            Authentication.LoginVirtualUser(user);
            return Redirect(callBackUrl);
        }
        public ActionResult Logout()
        {
            Authentication.LogOutUser();
            if (!string.IsNullOrEmpty(WebUtil.GetCookieValue("MemberInfo")))
            {            
                Response.Cookies["MemberInfo"].Expires = DateTime.Now.AddDays(-1);
            }
            return Redirect(callBackUrl);
        }
    }
}

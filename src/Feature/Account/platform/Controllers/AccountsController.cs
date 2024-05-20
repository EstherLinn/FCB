using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.OAuth;
using Feature.Wealth.Account.Repositories;
using Feature.Wealth.Account.Services;
using Newtonsoft.Json;
using Sitecore.Security.Accounts;
using Sitecore.Web;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using static Xcms.Sitecore.Foundation.Basic.SitecoreExtensions.MemberUtils;

namespace Feature.Wealth.Account.Controllers
{
    public class AccountsController : JsonNetController
    {
        private LineService _lineService { get; }
        private FacebookService _facebookService { get; }
        private MemberRepository _memberRepository { get; }
        private string callBackUrl { get; }

        public AccountsController()
        {
            this._lineService = new LineService();
            this._facebookService = new FacebookService();
            this._memberRepository = new MemberRepository();
            this.callBackUrl = WebUtil.GetCookieValue("NowUrl");
        }
        [HttpGet]
        public async Task<ActionResult> SignInLine(string code, string error, string error_description)
        {
            if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code))
            {
                TempData["OAuthErrorMsg"] = error_description;

                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
            }
            var responseToken = await _lineService.GetTokensByCode(code);
            if (responseToken == null)
            {
                TempData["OAuthErrorMsg"] = "Line Token身分驗證有誤，請聯絡資訊處。";
                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
            }
            var responseProfile = await _lineService.GetProfileByToken(responseToken.AccessToken);
            var responseVerify = await _lineService.GetVerifyAccessToken(responseToken.IdToken, responseProfile.UserId);

            User user = Authentication.BuildVirtualUser("extranet", responseProfile.UserId, true);
            user.Profile.Name = responseVerify.Name;
            user.Profile.Email = responseVerify.Email;
            FcbMemberModel member = new FcbMemberModel()
            {
                MemberName = responseVerify.Name,
                MemberEmail = responseVerify.Email,
                PlatForm = nameof(PlatFormEunm.Line),
                PlatFormId = responseProfile.UserId
            };

            var memeberExist =  _memberRepository.CheckUserExists(PlatFormEunm.Line, responseProfile.UserId);
            if (!memeberExist)
            {
                member.VideoInfoOpen = true;
                member.ArrivedInfoOpen = true;
                member.StockShowColor = QuoteChangeEunm.Taiwan;
                _memberRepository.CreateNewMember(member);
            }
            string objToJson = JsonConvert.SerializeObject(member);
            user.Profile.SetCustomProperty("MemberInfo", objToJson);
            user.Profile.Save();
            Authentication.LoginVirtualUser(user);
            return View("~/Views/Feature/Wealth/Component/Oauth/Oauth.cshtml");
        }
        [HttpGet]
        public async Task<ActionResult> SignInFacebook(string code, string error, string error_description)
        {
            if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code))
            {
                TempData["OAuthErrorMsg"] = error_description;
                return Redirect(callBackUrl);
            }

            var responseToken = await _facebookService.GetTokensByCode(code);
            if (responseToken == null)
            {
                TempData["OAuthErrorMsg"] = "FB Token身分驗證有誤，請聯絡資訊處。";
                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
            }
            var responseProfile = await _facebookService.GetProfileByToken(responseToken.AccessToken);

            User user = Authentication.BuildVirtualUser("extranet", responseProfile.Id, true);
            user.Profile.Name = responseProfile.Name;
            user.Profile.Email = responseProfile.Email;

            FcbMemberModel member = new FcbMemberModel()
            {
                MemberName = responseProfile.Name,
                MemberEmail = responseProfile.Email,
                PlatForm = nameof(PlatFormEunm.FaceBook),
                PlatFormId = responseProfile.Id
            };
            string objToJson = JsonConvert.SerializeObject(member);
            user.Profile.SetCustomProperty("MemberInfo", objToJson);
            user.Profile.Save();
            Authentication.LoginVirtualUser(user);

            return View("~/Views/Feature/Wealth/Component/Oauth/Oauth.cshtml");
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
        [HttpPost]
        public ActionResult SetUrlCookie(string url)
        {
            WebUtil.SetCookieValue("NowUrl", url);
            return new JsonNetResult();
        }
        [HttpPost]
        public ActionResult GetUrlCookie()
        {
            object objReturn = new
            {
                url = WebUtil.GetCookieValue("NowUrl")
            };
            return new JsonNetResult(objReturn);
        }
        [HttpPost]
        public ActionResult SetMemberEmail(string email)
        {
            
            object objReturn = new
            {
                success = _memberRepository.SetMemberEmail(FcbMemberHelper.GetMemberPlatFormId(), email)
            };
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        public ActionResult SetVideoInfo(bool open)
        {
            object objReturn = new
            {
                success = _memberRepository.SetVideoInfo(FcbMemberHelper.GetMemberPlatFormId(), open)
            };
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        public ActionResult SetArriedInfo(bool open)
        {
            object objReturn = new
            {
                success = _memberRepository.SetArriedInfo(FcbMemberHelper.GetMemberPlatFormId(), open)
            };
            return new JsonNetResult(objReturn);
        }
        [HttpPost]
        public ActionResult SetQuoteChangeColor(string color)
        {
            object objReturn = new
            {
                success = _memberRepository.SetQuoteChangeColor(FcbMemberHelper.GetMemberPlatFormId(), color)
            };
            return new JsonNetResult(objReturn);
        }

    }
}

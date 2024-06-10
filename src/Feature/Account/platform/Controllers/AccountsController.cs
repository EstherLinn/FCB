using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.FundTrackList;
using Feature.Wealth.Account.Models.OAuth;
using Feature.Wealth.Account.Repositories;
using Feature.Wealth.Account.Services;
using Newtonsoft.Json;
using Sitecore.Security.Accounts;
using Sitecore.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using static Xcms.Sitecore.Foundation.Basic.SitecoreExtensions.MemberUtils;
using Foundation.Wealth.Extensions;
using Feature.Wealth.Account.Models.Api;
using Feature.Wealth.Account.Filter;
using Foundation.Wealth.Helper;
using System;
using Newtonsoft.Json.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using System.Web;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Account.Controllers
{
    public class AccountsController : JsonNetController
    {
        private readonly WebBankService _webBankService;
        private readonly LineService _lineService;
        private readonly FacebookService _facebookService;
        private readonly MemberRepository _memberRepository;
        private string callBackUrl;

        public AccountsController()
        {
            this._webBankService = new WebBankService();
            this._lineService = new LineService();
            this._facebookService = new FacebookService();
            this._memberRepository = new MemberRepository();
            this.callBackUrl = WebUtil.GetCookieValue("ReturnUrl");
        }
        [HttpGet]
        public async Task<ActionResult> SignInLine(string code, string error, string error_description)
        {
            if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code))
            {
                Session["OAuthErrorMsg"] = error_description;
                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
            }
            var responseToken = await _lineService.GetTokensByCode(code);
            if (responseToken == null)
            {
                Session["OAuthErrorMsg"] = "Line Token身分驗證有誤，請聯絡資訊處。";
                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
            }
            var responseProfile = await _lineService.GetProfileByToken(responseToken.AccessToken);
    
            var responseVerify = await _lineService.GetVerifyAccessToken(responseToken.IdToken, responseProfile.UserId);

            User user = Authentication.BuildVirtualUser("extranet", responseProfile.UserId, true);

            FcbMemberModel member = null;

            var isExist = _memberRepository.CheckUserExists(PlatFormEunm.Line, responseProfile.UserId);
            if (!isExist)
            {
                //創建會員
                user.Profile.Name = responseVerify.Name;
                user.Profile.Email = responseVerify.Email;
                member = new FcbMemberModel(responseVerify.Name, responseVerify.Email, true, true, QuoteChangeEunm.Taiwan, PlatFormEunm.Line, responseProfile.UserId);
                _memberRepository.CreateNewMember(member);
            }
            else
            {
                //已存在會員
                member = _memberRepository.GetMemberInfo(PlatFormEunm.Line, responseProfile.UserId);
                user.Profile.Name = member.MemberName;
                user.Profile.Email = member.MemberEmail;
            }

            string objToJson = JsonConvert.SerializeObject(member);
            user.Profile.SetCustomProperty("MemberInfo", objToJson);
            user.Profile.Save();
            Authentication.LoginVirtualUser(user);
            return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
        }
        [HttpGet]
        public async Task<ActionResult> SignInFacebook(string code, string error, string error_description)
        {
            if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code))
            {
                Session["OAuthErrorMsg"] = error_description;
                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
            }

            var responseToken = await _facebookService.GetTokensByCode(code);
            if (responseToken == null)
            {
                Session["OAuthErrorMsg"] = "FB Token身分驗證有誤，請聯絡資訊處。";
                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
            }
            var responseProfile = await _facebookService.GetProfileByToken(responseToken.AccessToken);

            User user = Authentication.BuildVirtualUser("extranet", responseProfile.Id, true);
            FcbMemberModel member = null;

            var isExist = _memberRepository.CheckUserExists(PlatFormEunm.FaceBook, responseProfile.Id);
            if (!isExist)
            {
                //創建會員
                user.Profile.Name = responseProfile.Name;
                user.Profile.Email = responseProfile.Email;
                member = new FcbMemberModel(responseProfile.Name, responseProfile.Email, true, true, QuoteChangeEunm.Taiwan, PlatFormEunm.FaceBook, responseProfile.Id);
                _memberRepository.CreateNewMember(member);
            }
            else
            {
                //已存在會員
                member = _memberRepository.GetMemberInfo(PlatFormEunm.FaceBook, responseProfile.Id);
                user.Profile.Name = member.MemberName;
                user.Profile.Email = member.MemberEmail;
            }

            string objToJson = JsonConvert.SerializeObject(member);
            user.Profile.SetCustomProperty("MemberInfo", objToJson);
            user.Profile.Save();
            Authentication.LoginVirtualUser(user);
            return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> WebBankLogin()
        {
            var resp = await _webBankService.UserVerifyRequest(callBackUrl);

            return new JsonNetResult(resp);
        }

        [HttpPost]
        public ActionResult WebBankResult(string txReqId, string LoginResult, string LoginDttm, string errMsg, string fnct, WebBankResultModel.custDataModel custData, string sign)
        {
                string obj = string.Format("txReqId:{0} LoginResult:{1} LoginDttm:{2} errMsg:{3} fnct:{4} custData:{5} sign:{6}",
                    txReqId, LoginResult, LoginDttm, errMsg, fnct, JsonConvert.SerializeObject(custData), sign);
                Logger.Account.Info("個網登入0203回應:" + obj);
            if (LoginResult == "0000")
            {
                if (FcbMemberHelper.CheckMemberLogin())
                {
                    //第三方綁定網銀
                    var cifMember = _memberRepository.GetWebBankUserInfo(custData.custId);
                    if (cifMember == null)
                    {
                        Session["OAuthErrorMsg"] = "您好，您的會員資料目前正更新中，請於明日重新登入，再使用會員相關功能，造成不便，敬請見諒!";
                        return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                    }
                    var isBind = _memberRepository.BindWebBank(FcbMemberHelper.GetMemberPlatForm(), FcbMemberHelper.GetMemberPlatFormId(), custData.custId);
                    if (isBind)
                    {
                        //成功綁定
                        var member = _memberRepository.GetMemberInfo(FcbMemberHelper.GetMemberPlatForm(), FcbMemberHelper.GetMemberPlatFormId());
                        User user = Sitecore.Context.User;
                        string objToJson = JsonConvert.SerializeObject(member);
                        user.Profile.SetCustomProperty("MemberInfo", objToJson);
                        user.Profile.Save();
                    }
                    else
                    {
                        Session["OAuthErrorMsg"] = "綁定網銀有誤，請聯絡資訊部";
                    }
                }
                else
                {
                    //網銀登入
                    var isExist = _memberRepository.CheckUserExists(PlatFormEunm.WebBank, custData.custId);
                    if (!isExist)
                    {
                        //創建會員
                        var cifMember = _memberRepository.GetWebBankUserInfo(custData.custId);
                        if (cifMember == null)
                        {
                            Session["OAuthErrorMsg"] = "您好，您的會員資料目前正更新中，請於明日重新登入，再使用會員相關功能，造成不便，敬請見諒!";
                            return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                        }
                        //創建會員
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
                        return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                    }
                    else
                    {
                        //登入
                        FcbMemberModel member = _memberRepository.GetMemberInfo(PlatFormEunm.WebBank, custData.custId);
                        User user = Authentication.BuildVirtualUser("extranet", member.WebBankId, true);
                        user.Profile.Name = member.MemberName;
                        user.Profile.Email = member.MemberEmail;
                        string objToJson = JsonConvert.SerializeObject(member);
                        user.Profile.SetCustomProperty("MemberInfo", objToJson);
                        user.Profile.Save();
                        Authentication.LoginVirtualUser(user);
                    }
                }
            }
            else
            {
                Session["OAuthErrorMsg"] = errMsg;
            }
            return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
        }

        public ActionResult Logout()
        {
            var nowUrl = new Uri(callBackUrl);
            var qs = HttpUtility.ParseQueryString(nowUrl.Query);
            qs.Remove("promotionCode");
            qs.Remove("rtCode");
            string pagePathWithoutQueryString = nowUrl.GetLeftPart(UriPartial.Path);
            string newUrl = qs.Count > 0 ? string.Format("{0}?{1}", pagePathWithoutQueryString, qs) : pagePathWithoutQueryString ;
            callBackUrl = newUrl;
            Authentication.LogOutUser();
            return Redirect(callBackUrl);
        }

        [HttpPost]
        public async Task<ActionResult> GetTrackListFromFcb(string promotionCode)
        {
            FirstBankApiService firstBankApiService = new();
            var res = await firstBankApiService.GetTrackList(promotionCode);

            return new JsonNetResult(res);
        }

        [HttpPost]
        public ActionResult InsertTrack(List<TrackListModel> trackList)
        {
            if (trackList == null)
            {
                trackList = Enumerable.Empty<TrackListModel>().ToList();
            }
            object objReturn = new
            {
                success = _memberRepository.InSertTrackList(trackList)
            };
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        public ActionResult GetTrackListFromDb()
        {
            var listdb = _memberRepository.GetTrackListFromDb(FcbMemberHelper.GetMemberPlatFormId());
            return new JsonNetResult(listdb);
        }


        [HttpPost]
        public ActionResult SetUrlCookie(string url)
        {
            WebUtil.SetCookieValue("ReturnUrl", url, DateTime.MinValue, true);
            return new JsonNetResult();
        }
        [HttpPost]
        public ActionResult GetUrlCookie()
        {
            object objReturn = new
            {
                url = WebUtil.GetCookieValue("ReturnUrl")
            };
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        public ActionResult GetCommonFunctions()
        {
            CommonFuncrionsResp resp = _memberRepository.GetCommonFunctions(FcbMemberHelper.GetMemberPlatFormId());
            var serialSetting = new JsonSerializerSettings()
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            };
            return new JsonNetResult(resp, serialSetting);
        }

        [HttpPost]
        public ActionResult SetMemberEmail(string email)
        {
            object objReturn = null;
            if (string.IsNullOrEmpty(email) || !email.IsValidEmail())
            {
                objReturn = new
                {
                    success = false,
                    errorMessage = "不符合Email格式，請重新輸入。"
                };
                return new JsonNetResult(objReturn);
            }
            objReturn = new
            {
                success = _memberRepository.SetMemberEmail(FcbMemberHelper.GetMemberPlatFormId(), email),
                errorMessage = string.Empty
            };
            RefreshMemberInfo();
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        public ActionResult SetVideoInfo(bool open)
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            object objReturn = new
            {
                success = _memberRepository.SetVideoInfo(FcbMemberHelper.GetMemberPlatFormId(), open)
            };
            RefreshMemberInfo();
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        public ActionResult SetArriedInfo(bool open)
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            object objReturn = new
            {
                success = _memberRepository.SetArriedInfo(FcbMemberHelper.GetMemberPlatFormId(), open)
            };
            RefreshMemberInfo();
            return new JsonNetResult(objReturn);
        }
        [HttpPost]
        public ActionResult SetQuoteChangeColor(string color)
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            object objReturn = new
            {
                success = _memberRepository.SetQuoteChangeColor(FcbMemberHelper.GetMemberPlatFormId(), color)
            };
            RefreshMemberInfo();
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        public ActionResult SetCommonFunctions(List<string> commons)
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            if (commons == null || !commons.Any())
            {
                commons = Enumerable.Empty<string>().ToList();
            }
            object objReturn = new
            {
                success = _memberRepository.SetCommonFunctions(FcbMemberHelper.GetMemberPlatFormId(), commons)
            };
            RefreshMemberInfo();
            return new JsonNetResult(objReturn);
        }

        private void RefreshMemberInfo()
        {
            var member = _memberRepository.GetMemberInfo(FcbMemberHelper.GetMemberPlatForm(), FcbMemberHelper.GetMemberPlatFormId());
            User user = Sitecore.Context.User;
            string objToJson = JsonConvert.SerializeObject(member);
            user.Profile.SetCustomProperty("MemberInfo", objToJson);
            user.Profile.Save();
        }

        [HttpPost]
        public ActionResult GetCommonTools()
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            CommonToolsRespResp resp = _memberRepository.GetCommonTools(FcbMemberHelper.GetMemberPlatFormId());
            var commonFuncItem = ItemUtils.GetItem(Templates.CommonFunction.Root.ToString());
            resp.Tools = commonFuncItem?.GetFieldValue(Templates.CommonFunction.Fields.CommonFunctionList);
            var serialSetting = new JsonSerializerSettings()
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            };
            return new JsonNetResult(resp, serialSetting);
        }

        [HttpPost]
        public ActionResult SetCommonTools(string itemId, bool isActive) => FcbMemberHelper.CheckMemberLogin() && _memberRepository.CheckCommonTools(itemId) ?
            new JsonNetResult(_memberRepository.SetCommonTools(itemId, isActive)) :
            new EmptyResult();
    }
}

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
using System.Runtime.Caching;
using Sitecore.Configuration;

namespace Feature.Wealth.Account.Controllers
{
    public class AccountsController : JsonNetController
    {
        private readonly WebBankService _webBankService;
        private readonly LineService _lineService;
        private readonly FacebookService _facebookService;
        private readonly MemberRepository _memberRepository;
        private readonly MemoryCache _cache = MemoryCache.Default;
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

        public ActionResult SignInWebBank()
        {
            string step = string.Empty;
            try
            {
                var query = HttpContext.Request.QueryString;
                var queueId = query.Get("queueId");
                if (string.IsNullOrEmpty(queueId))
                {
                    Logger.Account.Info("user queueId is IsNullOrEmpty");
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }
                if (FcbMemberHelper.CheckMemberLogin() && FcbMemberHelper.GetMemberPlatForm() == PlatFormEunm.WebBank)
                {
                    Logger.Account.Info("使用者已登入過");
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }
                if (_cache.Contains(queueId))
                {
                    string txReqId = (string)_cache.Get(queueId);
                    _cache.Remove(queueId);
                    Logger.Account.Info("user queueId:" + queueId + ",user txReqId:" + txReqId);
                    Logger.Account.Info("txReqId by 0201 set:" + txReqId);
                    _cache.Remove(queueId);
                    if (_cache.Contains(txReqId))
                    {
                        var id = (string)_cache.Get(txReqId);
                        _cache.Remove(txReqId);
                        if (!string.IsNullOrEmpty(id))
                        {
                            //判斷為綁定網銀或是網銀登入
                            if (FcbMemberHelper.CheckMemberLogin())
                            {
                                step = "Step2 第三方登入綁定網銀";
                                //第三方綁定網銀
                                var cifMember = _memberRepository.GetWebBankUserInfo(id);
                                if (cifMember == null)
                                {
                                    step = "Step3 第三方登入綁定網銀 cifMember:null";
                                    Session["WebBankErrorMsg"] = "您好，您的會員資料目前正更新中，請於明日重新登入，再使用會員相關功能，造成不便，敬請見諒!";
                                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                                }
                                var isBind = _memberRepository.BindWebBank(FcbMemberHelper.GetMemberPlatForm(), FcbMemberHelper.GetMemberPlatFormId(), id);
                                if (isBind)
                                {
                                    step = "Step4 第三方登入綁定網銀 SetWebBankId";
                                    //成功綁定
                                    var member = _memberRepository.GetMemberInfo(FcbMemberHelper.GetMemberPlatForm(), FcbMemberHelper.GetMemberPlatFormId());
                                    User user = Sitecore.Context.User;
                                    string objToJson = JsonConvert.SerializeObject(member);
                                    user.Profile.SetCustomProperty("MemberInfo", objToJson);
                                    user.Profile.Save();
                                }
                                else
                                {
                                    step = "Step4 第三方登入綁定網銀 error";
                                    Session["WebBankErrorMsg"] = "綁定網銀有誤，請聯絡資訊部";
                                }
                            }
                            else
                            {
                                step = "Step2 第e個網登入";
                                //網銀登入
                                var isExist = _memberRepository.CheckUserExists(PlatFormEunm.WebBank, id);
                                if (!isExist)
                                {
                                    step = "Step3 第e個網登入 ";
                                    //創建會員
                                    var cifMember = _memberRepository.GetWebBankUserInfo(id);
                                    if (cifMember == null)
                                    {
                                        step = "Step3 第e個網登入 cifMember:null";
                                        Session["WebBankErrorMsg"] = "您好，您的會員資料目前正更新中，請於明日重新登入，再使用會員相關功能，造成不便，敬請見諒!";
                                        return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                                    }
                                    //創建會員
                                    step = "Step4 第e個網登入 創建會員並登入";
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
                                }
                                else
                                {
                                    step = "Step3 第e個網登入 已有會員直接登入";
                                    //登入
                                    FcbMemberModel member = _memberRepository.GetMemberInfo(PlatFormEunm.WebBank, id);
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
                            step = "Step 1-2 取得user id, id = nullorEmpty";
                        }
                    }
                    else
                    {
                        step = $"Step1-3 cache ${txReqId} no contain,txReqId=${txReqId}";
                    }

                }
                else
                {
                    step = "Step 1-1 取得 cache queueId, queueId no contain,queueId =" + queueId;
                }
            }
            catch (Exception ex)
            {
                Logger.Account.Info($"個網登入回理財網 {step} ,exception Messags: {ex.Message}");
                Session["WebBankErrorMsg"] = "理財網身分驗證伺服器有誤，請聯絡資訊處";
            }
            finally
            {
                Logger.Account.Info($"個網登入回理財網 {step}");
            }
            return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
        }

        public ActionResult SignInWebBankByApp()
        {
            if (FcbMemberHelper.CheckMemberLogin())
            {
                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
            }
            //第一行動&&iLeo登入
            var qs = HttpContext.Request.QueryString;
            var step = string.Empty;
            try
            {
                step = $"Step1 檢查promotionCode和 rtCode";
                //檢查網址參數
                if (!string.IsNullOrEmpty(qs["promotionCode"]) && !string.IsNullOrEmpty(qs["rtCode"]) && qs["rtCode"] == "0000")
                {
                    step = $"Step2 promotionCode和rtCode通過，確認會員是否存在理財網";
                    Session["IsAppLogin"] = true;
                    var code = qs["promotionCode"];
                    var isExists = _memberRepository.CheckAppUserExists(PlatFormEunm.WebBank, code);
                    if (!isExists)
                    {
                        step = $"Step3 創建用戶開始，取得CIF資料";
                        //創建User
                        CIFMember cifMember = _memberRepository.GetAppUserInfo(code);
                        if (cifMember == null)
                        {
                            step = $"Step3-1 創建用戶開始，取得CIF資料,cif資料不存在理財網";
                            Session["AppLoginSuccess"] = false;
                            return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                        }
                        step = $"Step3-1 創建用戶開始，已取得CIF資料,理財網創建會員並登入";
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
                        Session["AppLoginSuccess"] = true;
                    }
                    else
                    {
                        step = $"Step3 理財網已有會員，直接登入";
                        FcbMemberModel member = _memberRepository.GetAppMemberInfo(PlatFormEunm.WebBank, code);
                        User user = Authentication.BuildVirtualUser("extranet", member.WebBankId, true);
                        user.Profile.Name = member.MemberName;
                        user.Profile.Email = member.MemberEmail;
                        string objToJson = JsonConvert.SerializeObject(member);
                        user.Profile.SetCustomProperty("MemberInfo", objToJson);
                        user.Profile.Save();
                        Authentication.LoginVirtualUser(user);
                        Session["AppLoginSuccess"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Account.Info($"個網銀登入byApp {step} ,exception Messags: {ex.Message}");
                Session["AppLoginSuccess"] = false;
            }
            finally
            {
                Logger.Account.Info($"網銀登入byApp promotionCode=${qs["promotionCode"]} &&rtCode=${qs["rtCode"]} ");
            }
            return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
        }
        [HttpPost]
        public async Task<ActionResult> WebBankLogin()
        {
            var ticks = DateTime.Now.Ticks;
            var guid = Guid.NewGuid().ToString();
            var queueId = ticks.ToString() + '-' + guid;
            var uri = new Uri(callBackUrl);
            string uriSchemaAndHost = string.Format("{0}://{1}", uri.Scheme, uri.Host);
            string WebBankCallBack = Settings.GetSetting("WebBank.CallBackUrl");
            uriSchemaAndHost += WebBankCallBack;
            var resp = await _webBankService.UserVerifyRequest(uriSchemaAndHost, queueId);
            return new JsonNetResult(resp);
        }

        [HttpPost]
        public ActionResult WebBankResult(string txReqId, string LoginResult, string LoginDttm, string errMsg, string fnct, string custData, string sign)
        {
            string obj = string.Format("txReqId:{0} LoginResult:{1} LoginDttm:{2} errMsg:{3} fnct:{4} custData:{5} sign:{6}",
                txReqId, LoginResult, LoginDttm, errMsg, fnct, custData, sign);
            var step = string.Empty;
            var getCustData = custData ?? string.Empty;
            var getCustDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(HttpUtility.UrlDecode(getCustData));
            object returnObj = new
            {
                ack = "ok",
                autoRedirectWaitSec = "0"
            };
            if (LoginResult == "0000")
            {
                step = "Step1 ok";
                if (!getCustDic.TryGetValue("custId", out string id))
                {
                    Logger.Account.Info("個網登入0203回應:" + obj + ",custData UrlDecode:" + HttpUtility.UrlDecode(getCustData));
                }
                else
                {
                    _cache.Set(txReqId, id, DateTimeOffset.Now.AddMinutes(5));
                }
            }
            return new JsonNetResult(returnObj);
        }

        public ActionResult Logout()
        {
            var nowUrl = new Uri(callBackUrl);
            var qs = HttpUtility.ParseQueryString(nowUrl.Query);
            qs.Remove("promotionCode");
            qs.Remove("rtCode");
            qs.Remove("queueId");
            string pagePathWithoutQueryString = nowUrl.GetLeftPart(UriPartial.Path);
            string newUrl = qs.Count > 0 ? string.Format("{0}?{1}", pagePathWithoutQueryString, qs) : pagePathWithoutQueryString;
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
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
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
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
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

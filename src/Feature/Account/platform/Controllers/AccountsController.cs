using Feature.Wealth.Account.Filter;
using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.Api;
using Feature.Wealth.Account.Models.FundTrackList;
using Feature.Wealth.Account.Models.MemberLog;
using Feature.Wealth.Account.Models.OAuth;
using Feature.Wealth.Account.Repositories;
using Feature.Wealth.Account.Services;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Helper;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Security.Accounts;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Xcms.Sitecore.Foundation.Basic.SitecoreExtensions.MemberUtils;
namespace Feature.Wealth.Account.Controllers
{
    public class AccountsController : JsonNetController
    {
        private readonly WebBankService _webBankService;
        private readonly LineService _lineService;
        private readonly FacebookService _facebookService;
        private readonly MemberRepository _memberRepository;
        private readonly string callBackUrl;

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
            var step = string.Empty;
            var errorDescription = string.Empty;
            try
            {
                step = "Step 1 檢查回傳參數有無錯誤訊息";
                if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code))
                {
                    Session["LoginStatus"] = false;
                    Session["ServerError"] = true;
                    errorDescription = error_description;
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }
                step = "Step 2 拿參數Code取Token";
                var responseToken = await _lineService.GetTokensByCode(code);
                if (responseToken == null)
                {
                    Session["LoginStatus"] = false;
                    Session["ServerError"] = true;
                    errorDescription = "Line Token身分驗證有誤";
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }
                step = "Step 3 拿Token取Profile";
                var responseProfile = await _lineService.GetProfileByToken(responseToken.AccessToken);
                if (responseProfile == null)
                {
                    Session["LoginStatus"] = false;
                    Session["ServerError"] = true;
                    errorDescription = "Line 拿Token取Profile userId有誤";
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }
                step = "Step 4 拿AccessToken取Email";
                var responseVerify = await _lineService.GetVerifyAccessToken(responseToken.IdToken, responseProfile.UserId);
                if (responseVerify == null)
                {
                    Session["LoginStatus"] = false;
                    Session["ServerError"] = true;
                    errorDescription = "Line 拿IdToken取Email有誤";
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }

                User user = Authentication.BuildVirtualUser("extranet", responseProfile.UserId, true);

                FcbMemberModel member;
                step = "Step 5 判斷理財網是否已有會員";
                var isExist = _memberRepository.CheckUserExists(PlatFormEunm.Line, responseProfile.UserId);
                if (!isExist)
                {
                    step = "Step 6 理財網無會員，創建理財網會員";
                    //創建會員
                    member = new FcbMemberModel(responseVerify.Name, responseVerify.Email, true, true, QuoteChangeEunm.Taiwan, PlatFormEunm.Line, responseProfile.UserId);
                    var createdSuccess = _memberRepository.CreateNewMember(member);
                    if (!createdSuccess)
                    {
                        Session["LoginStatus"] = false;
                        Session["ServerError"] = true;
                        errorDescription = $"創建理財網會員有誤 UserId = {MaskIdNumber(responseProfile.UserId)}";
                        return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                    }
                }
                else
                {
                    step = "Step 6 理財網會員已存在，直接登入";
                    //已存在會員
                    member = _memberRepository.GetMemberInfo(PlatFormEunm.Line, responseProfile.UserId);
                    if (member == null)
                    {
                        Session["LoginStatus"] = false;
                        Session["ServerError"] = true;
                        errorDescription = $"取得理財網會員有誤 UserId = {MaskIdNumber(responseProfile.UserId)}";
                        return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                    }
                }
                SetCustomPropertyAndLogin(member, user);
                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
            }
            catch (Exception ex)
            {
                Session["LoginStatus"] = false;
                Session["ServerError"] = true;
                errorDescription = ex.ToString();
            }
            finally
            {
                Logger.Account.Info($"Line 登入 {step},  ErrorMessage = {errorDescription}");
            }
            return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> SignInFacebook(string code, string error, string error_description)
        {
            var step = string.Empty;
            var errorDescription = string.Empty;
            try
            {
                step = "Step 1 檢查回傳參數有無錯誤訊息";
                if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code))
                {
                    Session["LoginStatus"] = false;
                    Session["ServerError"] = true;
                    errorDescription = error_description;
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }
                step = "Step 2 拿參數Code取Token";
                var responseToken = await _facebookService.GetTokensByCode(code);
                if (responseToken == null)
                {
                    Session["LoginStatus"] = false;
                    Session["ServerError"] = true;
                    errorDescription = "FB Token身分驗證有誤";
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }
                step = "Step 3 拿Token取Profile";
                var responseProfile = await _facebookService.GetProfileByToken(responseToken.AccessToken);
                if (responseProfile == null)
                {
                    Session["LoginStatus"] = false;
                    Session["ServerError"] = true;
                    errorDescription = "FB 拿Token取Profile有誤";
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }
                User user = Authentication.BuildVirtualUser("extranet", responseProfile.Id, true);
                FcbMemberModel member;
                step = "Step 4 判斷理財網是否已有會員";
                var isExist = _memberRepository.CheckUserExists(PlatFormEunm.FaceBook, responseProfile.Id);
                if (!isExist)
                {
                    step = "Step 5 理財網無會員，創建理財網會員";
                    //創建會員
                    member = new FcbMemberModel(responseProfile.Name, responseProfile.Email, true, true, QuoteChangeEunm.Taiwan, PlatFormEunm.FaceBook, responseProfile.Id);
                    var createdSuccess = _memberRepository.CreateNewMember(member);
                    if (!createdSuccess)
                    {
                        Session["LoginStatus"] = false;
                        Session["ServerError"] = true;
                        errorDescription = $"創建理財網會員有誤 UserId = {MaskIdNumber(responseProfile.Id)}";
                        return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                    }
                }
                else
                {
                    step = "Step 5 理財網會員已存在，直接登入";
                    //已存在會員
                    member = _memberRepository.GetMemberInfo(PlatFormEunm.FaceBook, responseProfile.Id);
                    if (member == null)
                    {
                        Session["LoginStatus"] = false;
                        Session["ServerError"] = true;
                        errorDescription = $"取得理財網會員有誤 UserId = {MaskIdNumber(responseProfile.Id)}";
                        return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                    }
                }
                SetCustomPropertyAndLogin(member, user);
                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
            }
            catch (Exception ex)
            {
                Session["LoginStatus"] = false;
                Session["ServerError"] = true;
                errorDescription = ex.ToString();
            }
            finally
            {
                Logger.Account.Info($"FB 登入 {step},  ErrorMessage = {errorDescription}");
            }
            return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
        }

        public ActionResult SignInWebBank()
        {
            var step = string.Empty;
            var errorDescription = string.Empty;
            var query = HttpContext.Request.QueryString;
            var queueId = query.Get("queueId");
            try
            {
                step = "Step 1 取得 queueId";
                if (string.IsNullOrEmpty(queueId))
                {
                    Session["LoginStatus"] = false;
                    Session["ServerError"] = true;
                    errorDescription = "queueId = nullorEmpty";
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }
                //防止網銀身分重複登入
                if (FcbMemberHelper.CheckMemberLogin() && FcbMemberHelper.GetMemberPlatForm() == PlatFormEunm.WebBank)
                {
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }
                LoginSharedRepository loginSharedRepository = new LoginSharedRepository();
                var id = loginSharedRepository.GetUserIdByTansaction(queueId);
                if (!string.IsNullOrEmpty(id))
                {
                    id = AESHelper.Decrypt(id);
                    step = $"Step1-1 確認CIF && CFMBSEL 有無此客戶資料 id={MaskIdNumber(id)}";
                    if (!_memberRepository.CheckWebBankDataExists(id,"pc"))
                    {
                        Session["LoginStatus"] = false;
                        return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                    }
                    //網銀登入
                    step = "Step2-1 確認CFMBSEL Table有無重複資料";
                    var IsMoreThanOneUser = _memberRepository.CheckCFMBSELTableMoreThanOneUser("pc", id);
                    if (!IsMoreThanOneUser)
                    {
                        step = "Step2-2 確認理財網是否已有會員";
                        var isExist = _memberRepository.CheckUserExists(PlatFormEunm.WebBank, id);
                        if (!isExist)
                        {
                            step = "Step3 理財網無會員，取得理財網db cif客戶資料";
                            //取得CIF資料
                            var cifMember = _memberRepository.GetWebBankUserInfo(id);
                            if (cifMember == null || string.IsNullOrEmpty(cifMember.CIF_PROMO_CODE))
                            {
                                step = $"Step3  cifMember:null id={MaskIdNumber(id)}";
                                Session["LoginStatus"] = false;
                                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                            }
                            //創建會員
                            step = "Step4 創建會員並登入";
                            FcbMemberModel member = new FcbMemberModel(
                                cifMember.CIF_PROMO_CODE, cifMember.CIF_CUST_NAME, cifMember.CIF_E_MAIL_ADDRESS,
                                cifMember.CIF_EMP_RISK, cifMember.CIF_AO_EMPName, cifMember.HRIS_EmployeeCode,
                                true, true, QuoteChangeEunm.Taiwan, PlatFormEunm.WebBank, cifMember.CIF_PROMO_CODE,
                                cifMember.CIF_ESTABL_BIRTH_DATE, cifMember.CIF_CUST_ATTR, cifMember.CIF_SAL_FLAG,
                                cifMember.CIF_MAIN_BRANCH, cifMember.CIF_KYC_EXPIR_DATE, cifMember.CIF_EMP_PI_RISK_ATTR,
                                cifMember.CIF_HIGH_ASSET_FLAG, cifMember.CIF_HIGH_ASSET_VAL_DATE,
                                cifMember.CIF_TEL_NO1, cifMember.CIF_TEL_NO3,
                                cifMember.IsEmployee, cifMember.IsManager, cifMember.IsKycExpire);

                            var createdSuccess = _memberRepository.CreateNewMember(member);
                            if (!createdSuccess)
                            {
                                errorDescription = $"創建理財網會員有誤 id={MaskIdNumber(id)}";
                                Session["LoginStatus"] = false;
                                Session["ServerError"] = true;
                                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                            }
                            User user = Authentication.BuildVirtualUser("extranet", cifMember.CIF_PROMO_CODE, true);
                            SetCustomPropertyAndLogin(member, user);
                        }
                        else
                        {
                            step = "Step3 理財網已有會員直接登入";
                            //登入
                            FcbMemberModel member = _memberRepository.GetMemberInfo(PlatFormEunm.WebBank, id);
                            if (member == null  || string.IsNullOrEmpty(member.PlatFormId))
                            {
                                errorDescription = $"取得理財網會員資料有誤 id={MaskIdNumber(id)}";
                                Session["LoginStatus"] = false;
                                Session["ServerError"] = true;
                                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                            }
                            User user = Authentication.BuildVirtualUser("extranet", member.WebBankId, true);
                            SetCustomPropertyAndLogin(member, user);
                        }
                    }
                    else
                    {
                        errorDescription = $"CFMBSEL Table有重複資料 id={MaskIdNumber(id)}";
                        Session["LoginStatus"] = false;
                        Session["ServerError"] = true;
                        return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                    }
                }
                else
                {
                    errorDescription = "id = nullorEmpty";
                    Session["LoginStatus"] = false;
                    Session["ServerError"] = true;
                }


            }
            catch (Exception ex)
            {
                errorDescription = ex.ToString();
                Session["LoginStatus"] = false;
                Session["ServerError"] = true;
            }
            finally
            {
                Logger.Account.Info($"個網登入回理財網 {step} ,queueId = {queueId}, ErrorMessage = {errorDescription}");
            }
            return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
        }

        public ActionResult SignInWebBankByApp()
        {
            //防止網銀身分重複登入
            if (FcbMemberHelper.CheckMemberLogin() && FcbMemberHelper.GetMemberPlatForm() == PlatFormEunm.WebBank)
            {
                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
            }
            //第一行動&&iLeo登入
            var qs = this.Request.QueryString;
            var step = string.Empty;
            var errorDescription = string.Empty;
            try
            {
                step = $"Step1 檢查promotionCode和 rtCode";
                //檢查網址參數
                if (!string.IsNullOrEmpty(qs["promotionCode"]) && !string.IsNullOrEmpty(qs["rtCode"]) && qs["rtCode"] == "0000")
                {
                    var code = qs["promotionCode"];
                    step = $"Step1-1 promotionCode和rtCode通過，確認CIF && CFMBSEL 有無此客戶資料";
                    if (!_memberRepository.CheckWebBankDataExists(code, "app"))
                    {
                        Session["LoginStatus"] = false;
                        return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                    }
                    step = $"Step2 確認CFMBSEL Table是否有重複資料";
                    var IsMoreThanOneUser = _memberRepository.CheckCFMBSELTableMoreThanOneUser("app", code);
                    if (!IsMoreThanOneUser)
                    {
                        step = $"Step2-1 promotionCode和rtCode通過，確認會員是否存在理財網";
                        var isExists = _memberRepository.CheckAppUserExists(PlatFormEunm.WebBank, code);
                        User user = Authentication.BuildVirtualUser("extranet", code, true);
                        if (!isExists)
                        {
                            step = $"Step3 創建用戶開始，取得理財網db CIF資料";
                            //創建User
                            CIFMember cifMember = _memberRepository.GetAppUserInfo(code);
                            if (cifMember == null || string.IsNullOrEmpty(cifMember.CIF_PROMO_CODE))
                            {
                                step = "Step3-1 cifMember:null";
                                Session["LoginStatus"] = false;
                                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                            }
                            step = $"Step4 已取得CIF資料,理財網創建會員並登入";
                            FcbMemberModel member = new FcbMemberModel(
                                cifMember.CIF_PROMO_CODE, cifMember.CIF_CUST_NAME, cifMember.CIF_E_MAIL_ADDRESS,
                                cifMember.CIF_EMP_RISK, cifMember.CIF_AO_EMPName, cifMember.HRIS_EmployeeCode,
                                true, true, QuoteChangeEunm.Taiwan, PlatFormEunm.WebBank, cifMember.CIF_PROMO_CODE,
                                cifMember.CIF_ESTABL_BIRTH_DATE, cifMember.CIF_CUST_ATTR, cifMember.CIF_SAL_FLAG,
                                cifMember.CIF_MAIN_BRANCH, cifMember.CIF_KYC_EXPIR_DATE, cifMember.CIF_EMP_PI_RISK_ATTR,
                                cifMember.CIF_HIGH_ASSET_FLAG, cifMember.CIF_HIGH_ASSET_VAL_DATE,
                                cifMember.CIF_TEL_NO1, cifMember.CIF_TEL_NO3,
                                cifMember.IsEmployee, cifMember.IsManager, cifMember.IsKycExpire);

                            var createdSuccess = _memberRepository.CreateNewMember(member);
                            if (!createdSuccess)
                            {
                                errorDescription = "創建理財網會員有誤";
                                Session["LoginStatus"] = false;
                                Session["ServerError"] = true;
                                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                            }
                            SetCustomPropertyAndLogin(member, user);
                        }
                        else
                        {
                            step = $"Step3 理財網已有會員，直接登入";
                            FcbMemberModel member = _memberRepository.GetAppMemberInfo(PlatFormEunm.WebBank, code);
                            if (member == null || string.IsNullOrEmpty(member.PlatFormId))
                            {
                                errorDescription = "取得會員資料有誤";
                                Session["LoginStatus"] = false;
                                Session["ServerError"] = true;
                                return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                            }
                            SetCustomPropertyAndLogin(member, user);
                        }
                    }
                    else
                    {
                        errorDescription = $"CFMBSEL Table有重複資料";
                        Session["LoginStatus"] = false;
                        Session["ServerError"] = true;
                    }
                }
                else
                {
                    errorDescription = $"回傳參數有誤";
                    Session["LoginStatus"] = false;
                    Session["ServerError"] = true;
                    return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
                }
            }
            catch (Exception ex)
            {
                errorDescription = ex.ToString();
                Session["LoginStatus"] = false;
                Session["ServerError"] = true;
            }
            finally
            {
                Logger.Account.Info($"網銀登入byApp {step} , promotionCode={qs["promotionCode"]} && rtCode={qs["rtCode"]} && ErrorDescription = {errorDescription}");
            }
            return View("~/Views/Feature/Wealth/Account/Oauth/Oauth.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> WebBankLogin()
        {
            var ticks = DateTime.Now.Ticks;
            var guid = Guid.NewGuid().ToString();
            //預先隨機組合queueId 用於登入回理財網時帶回queueId來mapping user證號
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
            var getCustData = custData ?? string.Empty;
            var getCustDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(HttpUtility.UrlDecode(getCustData));
            object returnObj = new
            {
                ack = "ok",
                autoRedirectWaitSec = "0"
            };
            if (LoginResult == "0000" && !string.IsNullOrEmpty(txReqId))
            {
                if (!getCustDic.TryGetValue("custId", out string id))
                {
                    Logger.Account.Info("個網登入0203回應:" + obj + ",custData UrlDecode:" + HttpUtility.UrlDecode(getCustData));
                }
                else
                {
                    LoginSharedRepository loginSharedRepository = new LoginSharedRepository();
                    if (loginSharedRepository.IsTansactionTimeExpired(txReqId))
                    {
                        Logger.Account.Info("User 操作時間過長已逾期");
                    }
                    else
                    {
                        loginSharedRepository.UpdateUserId(txReqId, AESHelper.Encrypt(id));
                    }
                    Logger.Account.Info("個網登入0203回應:" + obj + ",custData UrlDecode:" + HttpUtility.UrlDecode(getCustData));
                }
            }
            else
            {
                Logger.Account.Info("個網登入0203回應:" + obj + ",custData UrlDecode:" + HttpUtility.UrlDecode(getCustData));
                //非登入成功，不導轉
                returnObj = new
                {
                    ack = "ok",
                    autoRedirectWaitSec = "-1"
                };
            }
            return new JsonNetResult(returnObj);
        }

        public ActionResult Logout()
        {
            Authentication.LogOutUser();
            if (FcbMemberHelper.CheckMemberLogin())
            {
                RecordUserAction(ActionEnum.Logout);
            }
            if (string.IsNullOrEmpty(callBackUrl))
            {
                //回首頁
                var domain = string.IsNullOrEmpty(Sitecore.Context.Site.TargetHostName) ? Request.Url.Host : Sitecore.Context.Site.TargetHostName;
                return Redirect("https://" + domain);
            }
            return Redirect(callBackUrl);
        }

        [HttpPost]
        [MemberAuthenticationFilter]
        public async Task<ActionResult> InsertTrack(List<TrackListModel> trackList, string productId, string productType)
        {
            object objReturn = new
            {
                success = false,
                block = false
            };
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                objReturn = new
                {
                    success = false,
                    block = true
                };
                return new JsonNetResult(objReturn);
            }
            if (trackList == null)
            {
                trackList = Enumerable.Empty<TrackListModel>().ToList();
            }
            objReturn = new
            {
                success = _memberRepository.InSertTrackList(trackList),
                block = false
            };
            if (FcbMemberHelper.GetMemberPlatForm() == PlatFormEunm.WebBank)
            {
                FirstBankApiService firstBankApiService = new();
                await firstBankApiService.SyncTrackListToIleo(FcbMemberHelper.GetMemberPlatFormId(), productId, productType);
            }
            if (!trackList.Exists(x => x.Id == productId))
            {
                ReachInfoRepository reachInfoRepository = new();
                reachInfoRepository.DeleteCancelFocusRenchInfo(productId);
            }
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        public async Task<ActionResult> GetTrackList()
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            if (FcbMemberHelper.GetMemberPlatForm() == PlatFormEunm.WebBank)
            {
                FirstBankApiService firstBankApiService = new();
                firstBankApiService.SyncTrackListFormIleo(await firstBankApiService.GetTrackListFromIleo(FcbMemberHelper.GetMemberWebBankId()));
            }
            return new JsonNetResult(_memberRepository.GetTrackListFromDb(FcbMemberHelper.GetMemberPlatFormId()));
        }


        [HttpPost]
        public ActionResult SetUrlCookie(string url)
        {
            this.Response.SetSameSiteCookie("ReturnUrl", url);
            return new JsonNetResult(new { success = true });
        }

        [HttpPost]
        public ActionResult GetUrlCookie(bool unlockBlock = false)
        {
            var url = WebUtil.GetCookieValue("ReturnUrl");
            var beBlock = false;
            if (!string.IsNullOrEmpty(WebUtil.GetCookieValue("BlockUrl")))
            {
                url = HttpUtility.UrlDecode(WebUtil.GetCookieValue("BlockUrl"));
                beBlock = true;
                if (unlockBlock)
                {
                    this.Response.Cookies["BlockUrl"].Expires = DateTime.Now.AddDays(-1);
                }
            }
            object objReturn = new
            {
                url,
                beBlock
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
        [MemberAuthenticationFilter]
        public ActionResult SetMemberEmail(string email)
        {
            object objReturn = new
            {
                success = false,
                block = false
            };
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                objReturn = new
                {
                    success = false,
                    block = true
                };
                return new JsonNetResult(objReturn);
            }
            if (string.IsNullOrEmpty(email) || !email.IsValidEmail())
            {
                objReturn = new
                {
                    success = false,
                    block = false,
                    errorMessage = "不符合Email格式，請重新輸入。"
                };
                return new JsonNetResult(objReturn);
            }
            objReturn = new
            {
                success = _memberRepository.SetMemberEmail(FcbMemberHelper.GetMemberPlatFormId(), email),
                errorMessage = string.Empty
            };
            RecordUserAction(ActionEnum.Edit, string.Format("修改Email: {0} > {1}", FcbMemberHelper.fcbMemberModel.MemberEmail, email));
            RefreshMemberInfo();
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        [MemberAuthenticationFilter]
        public ActionResult SetVideoInfo(bool open)
        {
            object objReturn = new
            {
                success = false,
            };
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new JsonNetResult(objReturn);
            }
            objReturn = new
            {
                success = _memberRepository.SetVideoInfo(FcbMemberHelper.GetMemberPlatFormId(), open)
            };
            RecordUserAction(ActionEnum.Edit, string.Format("理財視訊通知: {0} > {1}", FcbMemberHelper.fcbMemberModel.VideoInfoOpen, open));
            RefreshMemberInfo();
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        [MemberAuthenticationFilter]
        public ActionResult SetArriedInfo(bool open)
        {
            object objReturn = new
            {
                success = false,
            };
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new JsonNetResult(objReturn);
            }
            objReturn = new
            {
                success = _memberRepository.SetArriedInfo(FcbMemberHelper.GetMemberPlatFormId(), open)
            };
            RecordUserAction(ActionEnum.Edit, string.Format("到價通知: {0} > {1}", FcbMemberHelper.fcbMemberModel.ArrivedInfoOpen, open));
            RefreshMemberInfo();
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        [MemberAuthenticationFilter]
        public ActionResult SetQuoteChangeColor(string color)
        {
            object objReturn = new
            {
                success = false,
            };
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new JsonNetResult(objReturn);
            }
            objReturn = new
            {
                success = _memberRepository.SetQuoteChangeColor(FcbMemberHelper.GetMemberPlatFormId(), color)
            };
            RecordUserAction(ActionEnum.Edit, string.Format("修改漲跌顏色: {0} > {1}", FcbMemberHelper.fcbMemberModel.StockShowColor.ToString(), color));
            RefreshMemberInfo();
            return new JsonNetResult(objReturn);
        }

        [HttpPost]
        [MemberAuthenticationFilter]
        public ActionResult SetCommonFunctions(List<string> commons)
        {
            object objReturn = new
            {
                success = false,
            };
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new JsonNetResult(objReturn);
            }
            if (commons == null || !commons.Any())
            {
                commons = Enumerable.Empty<string>().ToList();
            }
            objReturn = new
            {
                success = _memberRepository.SetCommonFunctions(FcbMemberHelper.GetMemberPlatFormId(), commons)
            };
            RecordUserAction(ActionEnum.Edit, string.Format("修改常用功能: {0}", string.Join(",", commons.ToArray())));
            RefreshMemberInfo();
            return new JsonNetResult(objReturn);
        }

        private void RefreshMemberInfo()
        {
            var member = _memberRepository.GetRefreshMemberInfo(FcbMemberHelper.GetMemberPlatForm(), FcbMemberHelper.GetMemberPlatFormId());
            User user = Sitecore.Context.User;
            string objToJson = JsonConvert.SerializeObject(member);
            user.Profile.SetCustomProperty("MemberInfo", objToJson);
            user.Profile.Save();
        }

        private void SetCustomPropertyAndLogin(FcbMemberModel fcbMember, User user)
        {
            string objToJson = JsonConvert.SerializeObject(fcbMember);
            user.Profile.SetCustomProperty("MemberInfo", objToJson);
            user.Profile.Save();
            Authentication.LoginVirtualUser(user);
            RecordUserAction(ActionEnum.Login);
        }
        /// <summary>
        /// 遮罩保留前後3碼
        /// </summary>
        /// <param name="id">證號orUserId</param>
        /// <returns></returns>
        private string MaskIdNumber(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return string.Empty;
            }
            if (id.Length <= 6)
            {
                return id;
            }
            string head = id.Substring(0, 3);
            string tail = id.Substring(id.Length - 3, 3);
            string middle = new string('*', id.Length - 6);
            return head + middle + tail;
        }
        [HttpPost]
        [MemberAuthenticationFilter]
        public ActionResult GetCifMemberRisk()
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new JsonNetResult(new { success = false, block = true });
            }
            if (!Foundation.Wealth.Models.Config.IsEnableCheck)
            {
                var riskItem = ItemUtils.GetItem("{A919250C-132C-4633-A1FF-B3CB0384F33F}");
                var testRisk = ItemUtils.GetFieldValue(riskItem, "Risk");
                var isenableTest = riskItem.IsChecked("EnableTest");
                if (!isenableTest)
                {
                    return new JsonNetResult(new { success = false });
                }
                return new JsonNetResult(new { success = true, risk = testRisk });
            }
            var canReadOracle = _memberRepository.CheckEDHStatus();
            if (!canReadOracle)
            {
                return new JsonNetResult(new { success = false });
            }
            var resp = _memberRepository.UpdateCifRiskToSql(_memberRepository.GetCifRiskFormOracle());
            if (resp.Item1)
            {
                var returnResp = new { success = resp.Item1, risk = resp.Item2.Trim().Split(' ')[0] };
                RefreshMemberInfo();
                return new JsonNetResult(returnResp);
            }
            return new JsonNetResult(new { success = false });
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
        [MemberAuthenticationFilter]
        public ActionResult SetCommonTools(string itemId, bool isActive) => FcbMemberHelper.CheckMemberLogin() && _memberRepository.CheckCommonTools(itemId) ?
            new JsonNetResult(_memberRepository.SetCommonTools(itemId, isActive, GetClientIpAddressAndPort(), GetClientBrowserVersion())) :
            new JsonNetResult(new { Item1 = false });

        /// <summary>
        /// 取得client ip and port
        /// </summary>
        /// <returns>Tuple(string,string)</returns>
        public (string, string) GetClientIpAddressAndPort()
        {
            // 從 X-Forwarded-For 獲取 IP
            string clientIp = HttpContext.Request.Headers["X-Forwarded-For"]
                    ?.Split(',').Select(ip => ip.Trim()).FirstOrDefault();

            // 如果無取到X-Forwarded-For，則使用直接連接的 IP　REMOTE_ADDR
            if (string.IsNullOrEmpty(clientIp))
            {
                clientIp = HttpContext.Request.UserHostAddress;
            }
            // 分割ip及port
            var IpAndPortArray = clientIp.Split(':');
            clientIp = IpAndPortArray[0];
            string port = IpAndPortArray.Length > 1 ? IpAndPortArray[1] : string.Empty;

            // 驗證ip有效
            return System.Net.IPAddress.TryParse(clientIp, out var ipAddress) ? (ipAddress.ToString(), port) : (string.Empty, string.Empty);
        }
        /// <summary>
        /// 取得client瀏覽器
        /// </summary>
        /// <returns></returns>
        public string GetClientBrowserVersion()
        {
            //Edge 基於Chromium 開發，Request.Browser會取到chrome，因此多增加UserAgent判斷Edge
            string userAgent = Request.UserAgent;
            HttpBrowserCapabilitiesBase bc = Request.Browser;
            return userAgent.Contains("Edg") ? "Edge " + bc.Version : bc.Browser + " " + bc.Version;
        }
        /// <summary>
        /// 紀錄會員操作log
        /// </summary>
        /// <param name="actionEnum">ActionEnum</param>
        /// <param name="description">操作詳細描述</param>
        public void RecordUserAction(ActionEnum actionEnum, string description = "")
        {
            var ipAndPort = GetClientIpAddressAndPort();
            MemberLog memberLog = new MemberLog()
            {
                PlatForm = FcbMemberHelper.fcbMemberModel.PlatForm.ToString(),
                PlatFormId = FcbMemberHelper.fcbMemberModel.PlatFormId,
                ClientIp = ipAndPort.Item1,
                Port = ipAndPort.Item2,
                Browser = GetClientBrowserVersion(),
                Description = description == "" ? null : description,
                Action = actionEnum.ToString(),
                ActionTime = Sitecore.DateUtil.ToServerTime(DateTime.UtcNow)
            };
            _memberRepository.RecordMemberActionLog(memberLog);
        }
    }
}

using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.OAuth;
using Feature.Wealth.Account.Repositories;
using Newtonsoft.Json;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xcms.Sitecore.Foundation.Basic.Logging;
using static Xcms.Sitecore.Foundation.Basic.SitecoreExtensions.MemberUtils;

namespace Feature.Wealth.Account.Pipelines
{
    public class WebBankLogin : HttpRequestProcessor
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly MemberRepository _memberRepository = new MemberRepository();
        public override void Process(HttpRequestArgs args)
        {
            string step = string.Empty;
            try
            {
                var uri = args.RequestUrl;
                var query = HttpUtility.ParseQueryString(uri.Query);
                var queueId = query.Get("queueId");
                if (string.IsNullOrEmpty(queueId))
                {
                    Logger.Account.Info("user queueId is IsNullOrEmpty");
                    return;
                }
                if (FcbMemberHelper.CheckMemberLogin() && FcbMemberHelper.GetMemberPlatForm() == PlatFormEunm.WebBank)
                {
                    Logger.Account.Info("使用者已登入過");
                    return;
                }
                if (_cache.Contains(queueId))
                {
                    string txReqId = (string)_cache.Get(queueId);
                    _cache.Remove(queueId);
                    Logger.Account.Info("user queueId:" + queueId + ",user txReqId:" + txReqId);
                    if ( !_cache.Contains($"WebBankErrorMsg-{txReqId}"))
                    {
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
                                        args.HttpContext.Session["WebBankErrorMsg"] = "您好，您的會員資料目前正更新中，請於明日重新登入，再使用會員相關功能，造成不便，敬請見諒!";
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
                                        args.HttpContext.Session["WebBankErrorMsg"] = "綁定網銀有誤，請聯絡資訊部";
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
                                            args.HttpContext.Session["WebBankErrorMsg"] = "您好，您的會員資料目前正更新中，請於明日重新登入，再使用會員相關功能，造成不便，敬請見諒!";
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
                        step = "Step1 第e個網登入回傳錯誤訊息非0000";
                        args.HttpContext.Session["WebBankErrorMsg"] = _cache.Get($"WebBankErrorMsg-{txReqId}");
                        _cache.Remove($"WebBankErrorMsg-{txReqId}");
                    }
                }
                else
                {
                    step = "Step 1-1 取得 cache queueId, queueId no contain,queueId =" + queueId;
                }
            }
            catch (Exception ex)
            {
                args.HttpContext.Session["WebBankErrorMsg"] = "理財網身分驗證伺服器有誤，請聯絡資訊處";
                Logger.Account.Info($"Api WebBankResult {step} ,exception Messags: {ex.Message}");
            }
            finally {
                Logger.Account.Info($"個網登入0203回應 {step}");
            }
        }
    }
}

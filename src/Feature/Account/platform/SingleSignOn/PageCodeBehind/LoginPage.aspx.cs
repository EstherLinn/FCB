using Sitecore.Web;
using System;
using System.Threading.Tasks;
using System.Web.UI;

namespace Feature.Wealth.Account.SingleSignOn.PageCodeBehind
{
    /// <summary>
    /// Login Service Page
    /// </summary>
    public partial class LoginPage : Page
    {
        /// <summary>
        /// 畫面生成後，取得SSO登入
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                RegisterAsyncTask(new PageAsyncTask(TaskAsync));
            }
        }

        /// <summary>
        /// Async Method
        /// </summary>
        /// <returns>Task</returns>
        private async Task TaskAsync()
        {
            string workforceId = WebUtil.GetRequestHeader("X-workforceID");

            // 未包含 AuthCode 參數
            if (string.IsNullOrEmpty(workforceId))
            {
                ErrorMessage("此頁面僅提供行員 SSO 登入");
                return;
            }

            var sso = new FirstBankSsoManager();
            try
            {
                bool success = sso.VerifyAccessToken(workforceId);
                if (success == false)
                {
                    ErrorMessage("workforceId 驗證失敗");
                    return;
                }

                var user = sso.BuildSsoUser();
                var scUser = sso.BuildSitecoreUser(user);

                //// 有效使用者驗證
                var rule = sso.ValidUserCondition(user, scUser);
                if (rule.Success == false)
                {
                    ErrorMessage(rule.Message);
                    return;
                }

                var login = sso.Login(scUser);
                if (login.Success)
                {
                    this.Response.Redirect(sso.StartPage);
                    //this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sso", $"window.location.href='{startUrl}?sc_lang=zh-tw'", true);
                }

                ErrorMessage(login.Message);
            }
            catch (Exception ex)
            {
                sso.Log.Error("SSO 登入失敗", ex);
                ErrorMessage("請與管理人員聯繫");
            }
        }

        private void ErrorMessage(string msg) => this.Response.Write($"Sitecore 登入失敗，{msg}");
    }
}
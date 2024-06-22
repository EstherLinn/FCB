using Sitecore.Web;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
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

            if (string.IsNullOrEmpty(workforceId) && bool.TryParse(ConfigurationManager.AppSettings["Feature.Wealth.Account.SSO.LoginQueryString"], out bool isLoginQueryString))
            {
                workforceId = isLoginQueryString ? HttpContext.Current.Request.QueryString["workforceID"] : string.Empty;
            }

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
                if (!success)
                {
                    ErrorMessage("workforceId 驗證失敗");
                    return;
                }

                var user = sso.BuildSsoUser();
                var scUser = sso.BuildSitecoreUser(user);

                // 有效使用者驗證
                var rule = sso.ValidUserCondition(user, scUser);
                if (!rule.Success)
                {
                    // 尚未建立帳戶
                    ErrorMessage(rule.Message);
                    return;
                }

                var login = sso.Login(scUser);
                if (login.Success)
                {
                    this.Response.Redirect(sso.StartPage);
                }

                ErrorMessage(login.Message);
            }
            catch (Exception ex)
            {
                sso.Log.Error("SSO 登入失敗", ex);
                ErrorMessage($"請與管理人員聯繫, {ex.Message}");
            }
        }

        private void ErrorMessage(string msg) => this.Response.Write($"Sitecore 登入失敗，{msg}");
    }
}
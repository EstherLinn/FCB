using Microsoft.IdentityModel.Tokens;
using Sitecore.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Account.Filter
{
    public class ApiAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public const string AUTHORIZATION_KEY = "Authorization";
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            // 從請求標頭中提取 Authorization 標頭
            var authHeader = HttpContext.Current.Request.Headers["Authorization"]?.ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                filterContext.Result = new JsonNetResult(new { statusCode = -9997, statusMsg = "找不到 token" });
                return;
            }

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = false,
                ValidIssuer = Settings.GetSetting("JwtIssuer"), //some string, normally web url,  
                ValidAudience = Settings.GetSetting("JwtAudience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.GetSetting("JwtSecretKey")))
            };

            // 提取 Bearer token
            string jwtToken = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                new JwtSecurityTokenHandler().ValidateToken(jwtToken, tokenValidationParameters, out SecurityToken validatedToken);
            }
            catch (SecurityTokenException)
            {
                filterContext.Result = new JsonNetResult(new { statusCode = -9996, statusMsg = "token 驗證失敗" });
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null)
            {
                filterContext.Result = new JsonNetResult(new { statusCode = -9998, statusMsg = "不合法的驗證" });
            }
        }

        public static bool IsVerify(string token)
        {
            // 從請求標頭中提取 Authorization 標頭
            var authHeader = HttpContext.Current.Request.Headers["Authorization"]?.ToString() ?? string.Empty;
            if (authHeader.Length > 7)
            {
                // 提取 Bearer token
                string jwtToken = authHeader.Substring("Bearer ".Length).Trim();
                return token == jwtToken;
            }
            return false;
        }
    }
}

using System.Web;

namespace Foundation.Wealth.Helper
{
    public static class SameSiteCookie
    {
        /// <summary>
        /// 設定cookie的SameSite屬性
        /// </summary>
        /// <param name="response"></param>
        /// <param name="cookieName"></param>
        public static void SetSameSiteCookie(this HttpResponseBase response, string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.SameSite = SameSiteMode.Lax;
                cookie.HttpOnly = true;
                cookie.Secure = true;
                response.Cookies.Set(cookie);
            }
        }
    }
}
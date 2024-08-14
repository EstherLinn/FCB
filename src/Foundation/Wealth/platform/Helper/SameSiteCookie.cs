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
        /// <param name="value"></param>
        public static void SetSameSiteCookie(this HttpResponseBase response, string cookieName, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);

            }
            cookie.Value = value;
            cookie.SameSite = SameSiteMode.Lax;
            cookie.Expires = System.DateTime.MinValue;
            cookie.HttpOnly = true;
            cookie.Secure = true;
            response.Cookies.Set(cookie);
        }
    }
}
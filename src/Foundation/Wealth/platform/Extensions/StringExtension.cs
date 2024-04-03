

namespace Foundation.Wealth.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 檢查字串空值給dash
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CheckNullOrEmptyString(this string str)
        {
            return string.IsNullOrEmpty(str) ? "-" : str;
        }
    }
}

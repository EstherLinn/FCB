using System;
using System.Text.RegularExpressions;

namespace Foundation.Wealth.Helper
{
    public static class InputSanitizerHelper
    {
        /// <summary>
        /// 檢查輸入是否符合四位數到六位數的字母或數字且不包含sql語句
        /// </summary>
        /// <param name="input">要檢查的輸入字串</param>
        /// <returns>如果有效返回 true，否則 false</returns>
        public static bool IsValidInput(string input)
        {
            string pattern = @"^(?!.*(--|select|insert|update|delete|drop|union|exec|count|char|concat|declare|xp_cmdshell|create|alter|drop|truncate|grant|revoke|show|;|['""><%=])).{4,6}$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(input);
        }
        /// <summary>
        /// 過濾sql關鍵字
        /// </summary>
        /// <param name="input">輸入字串</param>
        /// <returns>過濾後的字串</returns>
        public static string InputSanitizer(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            Guid tempGuid;
            if (Guid.TryParse(input, out tempGuid))
            {
                return input; // 如果是 GUID，直接返回GUID
            }
            //常見sql語句
            string pattern = @"(--|select|insert|update|delete|drop|union|exec|count|char|concat|declare|xp_cmdshell|create|alter|truncate|grant|revoke|show|;|['""><%])";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            //回傳過濾後的input
            return regex.Replace(input, "");
        }
    }
}

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
        /// 檢查輸入是否符合指定字數的字母或數字且不包含sql語句
        /// </summary>
        /// <param name="input">要檢查的輸入字串</param>
        /// <param name="minLimit">最小限制字數</param>
        /// <param name="maxLimit">最大限制字數</param>
        /// <returns>如果有效返回 true，否則 false</returns>
        /// <exception cref="ArgumentException">參數錯誤Exception</exception>
        public static bool IsValidInput(string input,int minLimit, int maxLimit)
        {
            if (minLimit < 0 || maxLimit < minLimit)
            {
                throw new ArgumentException("minLimit 和 maxLimit 必須是有效範圍。");
            }
            string pattern = $@"^(?!.*(--|select|insert|update|delete|drop|union|exec|count|char|concat|declare|xp_cmdshell|create|alter|drop|truncate|grant|revoke|show|;|['""><%=])).{{{minLimit},{maxLimit}}}$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(input);
        }
        /// <summary>
        /// 僅允許英數、中文、符號(.、-、/)
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
            string pattern = @"[^0-9a-zA-Z.\-/\u4e00-\u9fff]";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            //回傳過濾後的input
            return regex.Replace(input, "");
        }
    }
}

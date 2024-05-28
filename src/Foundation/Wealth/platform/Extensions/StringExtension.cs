

using System.Text.RegularExpressions;

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
        /// <summary>
        /// Y/N -> 有/無,Null -> NA
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertToChineseOrEmptyNA(this string str)
        {
            if (string.IsNullOrEmpty(str) || str == "NA")
            {
                return "NA";
            }
            string convertStr = string.Empty;
            switch (str)
            {
                case "Y":
                    convertStr = "有";
                    break;
                case "N":
                    convertStr = "無";
                    break;
            }
            return convertStr;
        }

        /// <summary>
        /// 投資金額欄位格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatCurrencyWithValue(this string str)
        {
            if (string.IsNullOrEmpty(str) || str == "NA")
            {
                return "NA";
            }
            var array = str.Split('/');
            if (array.Length == 0)
            {
                return "NA";
            }
            var leftValue = array[0];
            var rightValue = array[1];
            var leftArray = leftValue.Split(' ');
            var rightArray = rightValue.Split(' ');
            var outputLeftValue = string.Empty;
            var outputRightValue = string.Empty;

            if (leftArray[1] != "0")
            {
                outputLeftValue = string.Format("{0} {1}", leftArray[0], leftArray[1]);
            }
            if (rightArray[1] != "0")
            {
                outputRightValue = string.Format("{0} {1}", rightArray[0], rightArray[1]);
            }
            if (string.IsNullOrEmpty(outputLeftValue) && string.IsNullOrEmpty(outputRightValue))
            {
                return "--";
            }

            if (string.IsNullOrEmpty(outputLeftValue) || string.IsNullOrEmpty(outputRightValue))
            {
                return outputLeftValue + outputRightValue;
            }


            return string.Format("{0}/{1}", outputLeftValue, outputRightValue);
        }

        /// <summary>
        /// 檢查email格式
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}

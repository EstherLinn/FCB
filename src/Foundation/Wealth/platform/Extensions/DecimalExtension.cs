using System;

namespace Foundation.Wealth.Extensions
{
    public static class DecimalExtension
    {
        /// <summary>
        /// decimal取小數位數
        /// </summary>
        /// <param name="number"></param>
        /// <param name="dot"></param>
        /// <returns></returns>
        public static string FormatDecimalNumber(this decimal? number,int dot =2 ,bool needAbs = true,bool needPercent = false)
        {
            if (number.Equals(null))
            {
                return "-";
            }
            if (needAbs && number < 0)
            {
                number = Math.Abs(number.Value);
            }
            var value = decimal.Round(number.Value, dot, MidpointRounding.AwayFromZero);
            return value.ToString() + (needPercent ? "%" : string.Empty);
        }


        /// <summary>
        /// 判斷漲跌給樣式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="userSet">登入使用者設定樣式</param>
        /// <returns></returns>
        public static string DecimalNumberToStyle(this decimal? number,string userSet = "1")
        {
            if (number.Equals(null) || number.Value == 0)
            {
                return string.Empty;

            }

            if (number.Value < 0)
            {
                return userSet== "1" ? "o-fall" : "o-rise";
            }
            else
            {
                 return userSet == "1" ? "o-rise" : "o-fall";
            }
            
        }
        /// <summary>
        /// 負數decimal紅色樣式
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string DecimalNegativeStyle(this decimal? number)
        {
            if (number.Equals(null))
            {
                return string.Empty;
            }
           return number.Value < 0 ? "style=color:#f00" : string.Empty;
           
        }
    }
}

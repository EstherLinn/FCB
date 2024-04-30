using System;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Foundation.Wealth.Extensions
{
    public static class NumberExtensions
    {
        /// <summary>
        /// 五位數數值格式化
        /// </summary>
        /// <param name="number">數值</param>
        /// <param name="decimalPlaces">保留小數點後幾位，預設 2</param>
        public static string FormatNumber(this decimal? number, int decimalPlaces = 2)
        {
            if (!number.HasValue)
            {
                return default;
            }

            string numberStr = number.Value.ToString();

            foreach (NumberSuffix suffix in EnumUtil.ToEnumerable<NumberSuffix>())
            {
                decimal currentValue = 1m * (decimal)Math.Pow(10, (int)suffix * 3);
                string? suffixValue = Enum.GetName(typeof(NumberSuffix), (int)suffix);

                if ((int)suffix == 0)
                {
                    suffixValue = string.Empty;
                }

                if (Math.Abs(number.Value) >= currentValue)
                {
                    numberStr = $"{Math.Round(number.Value / currentValue, decimalPlaces, MidpointRounding.AwayFromZero)}{suffixValue}";
                }
            }

            return numberStr;
        }

        /// <summary>
        /// 四捨五入取到小數第4位
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal? RoundingValue(this decimal? number)
        {
            return Rounding(number, 4);
        }

        /// <summary>
        /// 百分比以四捨五入取到小數第2位
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal? RoundingPercentage(this decimal? number)
        {
            return Rounding(number, 2);
        }

        /// <summary>
        /// 四捨五入取到指定小數位數
        /// </summary>
        /// <param name="number">數值</param>
        /// <param name="decimals">小數位數</param>
        /// <returns></returns>
        public static decimal? Rounding(decimal? number, int decimals)
        {
            if (!number.HasValue)
            {
                return null;
            }

            return Math.Round(number.Value, decimals, MidpointRounding.AwayFromZero);
        }

        private enum NumberSuffix
        {
            P,

            /// <summary>
            /// Thousand
            /// </summary>
            K,

            /// <summary>
            /// Million
            /// </summary>
            M,

            /// <summary>
            /// Billion
            /// </summary>
            B,

            /// <summary>
            /// Trillion
            /// </summary>
            T,

            /// <summary>
            /// Quadrillion
            /// </summary>
            Q
        }
    }
}
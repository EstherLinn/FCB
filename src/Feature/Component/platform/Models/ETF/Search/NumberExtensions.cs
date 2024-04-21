using System;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Models.ETF.Search
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
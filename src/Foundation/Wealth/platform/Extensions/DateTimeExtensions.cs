using System;
using System.Globalization;

namespace Foundation.Wealth.Extensions
{
    public static class DateTimeExtensions
    {
        public static string FormatDate(DateTime? date, string format = "yyyy/MM/dd")
        {
            if (!date.HasValue)
            {
                return string.Empty;
            }

            var cultureInfo = new CultureInfo("zh-TW");
            return date.Value.ToString(format, cultureInfo);
        }
    }
}

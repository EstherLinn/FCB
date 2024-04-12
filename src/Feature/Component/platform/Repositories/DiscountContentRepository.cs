using Feature.Wealth.Component.Models.DiscountContent;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public static class DiscountContentRepository
    {
        public static string GetDisplayDate(Item item)
        {
            DateTime startDate = ItemUtils.GetLocalDateFieldValue(item, Templates.DiscountContentDatasource.Fields.StartDate) ?? DateTime.MinValue;
            DateTime endDate = ItemUtils.GetLocalDateFieldValue(item, Templates.DiscountContentDatasource.Fields.EndDate) ?? DateTime.MinValue;
            DateTime today = DateUtil.ToServerTime(DateTime.UtcNow).Date;

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue || endDate < startDate)
            {
                return string.Empty;
            }
            else if (today > endDate)
            {
                return $"{startDate.ToString("yyyy/MM/dd")} ~ {endDate.ToString("yyyy/MM/dd")} 已到期";
            }
            else
            {
                return $"{startDate.ToString("yyyy/MM/dd")} ~ {endDate.ToString("yyyy/MM/dd")}";
            }
        }

        public static string GetDisplayTag(Item item)
        {
            DateTime startDate = ItemUtils.GetLocalDateFieldValue(item, Templates.DiscountContentDatasource.Fields.StartDate) ?? DateTime.MinValue;
            DateTime endDate = ItemUtils.GetLocalDateFieldValue(item, Templates.DiscountContentDatasource.Fields.EndDate) ?? DateTime.MinValue;
            DateTime today = DateUtil.ToServerTime(DateTime.UtcNow).Date;

            TimeSpan duration = endDate.AddDays(1) - startDate;

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            {
                return string.Empty;
            }

            if (duration.TotalDays < 15)
            {
                return "Limited"; // 限時快閃
            }

            TimeSpan startDiff = today - startDate.AddDays(-1);
            TimeSpan endDiff = endDate.AddDays(1) - today;

            if (startDiff.TotalDays <= 7)
            {
                return "New"; // 最新
            }
            else if (endDiff.TotalDays <= 7)
            {
                return "Expiring"; // 即將到期
            }
            else
            {
                return "Hot"; // 熱門
            }
        }
    }
}

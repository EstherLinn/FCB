using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.WeeklyAndMonthlyReports
{
    public class WeeklyAndMonthlyReportsModel
    {
        public Item DataSource { get; set; }
        public string DataSourceId { get; set; }
        public string Title { get; set; }
    }

    public class ReportsItem
    {
        public string ReportsType { get; set; }
        public string ReportsDate { get; set; }
        public string ReportsTitle { get; set; }
        public string ReportsLink { get; set; }
        public string ReportsTarget { get; set; }
        public string ReportsLinkTitle { get; set; }
    }

    public struct Templates
    {
        public struct WeeklyAndMonthlyReportsDatasource
        {
            public static readonly ID Id = new ID("{2CDA4E83-3692-4208-8863-618B830C2706}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{287A20B1-F9D6-404F-BBFB-5869A0879A59}");
            }
        }

        public struct WeeklyAndMonthlyReportsItem
        {
            public static readonly ID Id = new ID("{192BE530-AE32-4B53-A3DF-DE92987C4D7C}");

            public struct Fields
            {
                public static readonly ID IsMonthlyReports = new ID("{33A91B8D-7231-4902-BCD0-F0E6BB5D1B9C}");
                public static readonly ID Date = new ID("{EFBA7A7F-A356-486A-892C-5AA542C91229}");
                public static readonly ID Title = new ID("{D5501C04-3956-4A2F-AE0D-00DEB77FA4B5}");
                public static readonly ID Link = new ID("{C2E751CD-1E7D-4EA7-889B-344117133A09}");
            }
        }
    }
}
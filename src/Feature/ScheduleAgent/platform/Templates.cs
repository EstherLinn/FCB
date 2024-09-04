using Sitecore.Data;

namespace Feature.Wealth.ScheduleAgent
{
    public struct Templates
    {
        public struct FtpsSettings
        {
            public static readonly ID Id = new ID("{5F5DDA66-3530-477B-BBA5-8DD3E9BDC449}");
        }

        public struct SupplementSetting
        {
            public static readonly ID Id = new ID("{A1B7ACF1-9A38-4678-A588-8CA23DC8621C}");
        }

        public struct SourceSetting
        {
            public static readonly ID Id = new ID("{D1730BDA-FF24-46A6-BB56-03E82A0E6D14}");

            public struct Fields
            {
                public static readonly ID Datasource = new ID("{1CA42ECA-B1B6-487F-9690-2DDEB37A3B81}");
                public static readonly ID SourceFieldName = new ID("{65EB00E2-9349-4D0B-B20C-BF7C2159BAB1}");
            }
        }
    }
}
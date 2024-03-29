
using Sitecore.Data;

namespace Feature.Wealth.Component
{
    public class ComponentTemplates
    {
        public struct FundHotTag
        {
            public static readonly ID FundHotTagRoot = new ID("{D96319C4-EE3E-4C64-8596-655EA959C511}");
            public static readonly ID FundHotTagItem = new ID("{7657C99F-3B86-487D-AE38-CDFE8A45CB51}");
            public struct Fields
            {
                public static readonly ID HotTitle = new ID("{5927A7F8-1BEE-4B96-A0CD-86DDBF3B2AC9}");
                public static readonly ID FundList = new ID("{B9241E3D-71A8-4009-865B-70614A3AD586}");
            }
        }

        public struct FundMarketTag
        {
            public static readonly ID FundMarketTagRoot = new ID("{3369EF80-D8C6-4CF1-84C9-DE66AEC5E654}");
            public static readonly ID FundMarketTagItem = new ID("{4BE995D0-CC91-4AC2-8142-2B3792F95591}");
            public struct Fields
            {
                public static readonly ID MarketTitle = new ID("{2483B2F5-C2A8-42BB-8C3F-E47045DC3EDA}");
                public static readonly ID FundList = new ID("{77E818EA-F489-4DCB-84DA-D2622D09D8E2}");
            }
        }
    }
}
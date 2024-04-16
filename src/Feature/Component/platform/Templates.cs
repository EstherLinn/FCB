
using Sitecore.Data;

namespace Feature.Wealth.Component
{
    public class ComponentTemplates
    {
        public struct FundTag
        {
            public static readonly ID FundTagRoot = new ID("{BBAF1962-669A-4A56-A4BC-974369FEDB1F}");
            public static readonly ID FundTagItem = new ID("{E7C05C03-3A38-4186-A358-32A9253AD7DF}");

            public struct Fields
            {
                public static readonly ID FundTagTitle = new ID("{5361C9AE-E716-4127-A0A6-AF28CCE39134}");
                public static readonly ID FundTagType = new ID("{9828043B-3665-4CC1-A66B-9D007791CF8B}");
                public static readonly ID FundIdList = new ID("{0280C1D9-7E84-41AA-BD5E-2C8D94E59653}");

            }
        }
    }
}
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.CookieBar
{
    public class CookieBarModel
    {
        public Item DataSource { get; set; }
        public string Content { get; set; }
        public string ButtonText { get; set; }
        public string Style { get; set; }
    }

    public struct Templates
    {
        public struct CookieBarDatasource
        {
            public static readonly ID Id = new ID("{111DC9CC-9DD3-4C81-B8FD-3D60EFCD8621}");

            public struct Fields
            {
                public static readonly ID Content = new ID("{6646B588-72B3-40E3-89F3-EA6C3A740389}");
                public static readonly ID ButtonText = new ID("{DEA01F95-D474-4254-8C56-D65D9354AECF}");
            }
        }
    }
}

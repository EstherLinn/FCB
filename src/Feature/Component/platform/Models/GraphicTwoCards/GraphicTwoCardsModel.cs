using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.GraphicTwoCards
{
    public class GraphicTwoCardsModel
    {
        public Item DataSource { get; set; }
        public string ImageUrl1 { get; set; }
        public string Title1 { get; set; }
        public string Content1 { get; set; }
        public string ButtonText1 { get; set; }
        public string ButtonLink1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string Title2 { get; set; }
        public string Content2 { get; set; }
        public string ButtonText2 { get; set; }
        public string ButtonLink2 { get; set; }
    }

    public static class Templates
    {
        public struct GraphicTwoCardsDatasource
        {
            internal static readonly ID Id = new ID("{7B023B4F-D7B0-4199-909E-70A946BB7DB5}");

            public struct Fields
            {
                internal static readonly ID Image1 = new ID("{E222FD12-6AEC-4428-B2BA-639F448DE55B}");
                internal static readonly ID Title1 = new ID("{FCC3A5A2-7733-4390-800B-86DB07B1FD24}");
                internal static readonly ID Content1 = new ID("{9A43CF79-AFC5-4149-A209-38759D42D63A}");
                internal static readonly ID ButtonText1 = new ID("{B2CAD330-633A-4537-8628-691D2D56A959}");
                internal static readonly ID ButtonLink1 = new ID("{3B3922D7-A6E4-4C52-B76B-4369051DCEA3}");
                internal static readonly ID Image2 = new ID("{CF3AAACA-CB1E-41EC-9C17-645F07F8A795}");
                internal static readonly ID Title2 = new ID("{995AEDD9-DAF4-421E-B2FE-5624AF52D7E4}");
                internal static readonly ID Content2 = new ID("{A639F7DE-D782-46C2-B5B9-FD183FF7231E}");
                internal static readonly ID ButtonText2 = new ID("{6F29A275-EC74-443F-8954-C02349DCF59A}");
                internal static readonly ID ButtonLink2 = new ID("{4B5AF75A-AD5B-4D20-B37B-0C7E4611DB50}");
            }
        }
    }
}

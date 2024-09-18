using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

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
        public GraphicTwoCardsModel(Item item)
        {
            if (item == null || item.TemplateID != Templates.GraphicTwoCardsDatasource.Id)
            {
                return;
            }

            this.DataSource = item;
            this.ImageUrl1 = ItemUtils.ImageUrl(item, Templates.GraphicTwoCardsDatasource.Fields.Image1);
            this.Title1 = ItemUtils.GetFieldValue(item, Templates.GraphicTwoCardsDatasource.Fields.Title1);
            this.Content1 = ItemUtils.GetFieldValue(item, Templates.GraphicTwoCardsDatasource.Fields.Content1);
            this.ButtonText1 = ItemUtils.GetFieldValue(item, Templates.GraphicTwoCardsDatasource.Fields.ButtonText1);
            this.ButtonLink1 = ItemUtils.GeneralLink(item, Templates.GraphicTwoCardsDatasource.Fields.ButtonLink1)?.Url;
            this.ImageUrl2 = ItemUtils.ImageUrl(item, Templates.GraphicTwoCardsDatasource.Fields.Image2);
            this.Title2 = ItemUtils.GetFieldValue(item, Templates.GraphicTwoCardsDatasource.Fields.Title2);
            this.Content2 = ItemUtils.GetFieldValue(item, Templates.GraphicTwoCardsDatasource.Fields.Content2);
            this.Content2 = ItemUtils.GetFieldValue(item, Templates.GraphicTwoCardsDatasource.Fields.ButtonText2);
            this.ButtonLink2 = ItemUtils.GeneralLink(item, Templates.GraphicTwoCardsDatasource.Fields.ButtonLink2)?.Url;
        }
    }

    public struct Templates
    {
        public struct GraphicTwoCardsDatasource
        {
            public static readonly ID Id = new ID("{7B023B4F-D7B0-4199-909E-70A946BB7DB5}");

            public struct Fields
            {
                public static readonly ID Image1 = new ID("{E222FD12-6AEC-4428-B2BA-639F448DE55B}");
                public static readonly ID Title1 = new ID("{FCC3A5A2-7733-4390-800B-86DB07B1FD24}");
                public static readonly ID Content1 = new ID("{9A43CF79-AFC5-4149-A209-38759D42D63A}");
                public static readonly ID ButtonText1 = new ID("{B2CAD330-633A-4537-8628-691D2D56A959}");
                public static readonly ID ButtonLink1 = new ID("{3B3922D7-A6E4-4C52-B76B-4369051DCEA3}");
                public static readonly ID Image2 = new ID("{CF3AAACA-CB1E-41EC-9C17-645F07F8A795}");
                public static readonly ID Title2 = new ID("{995AEDD9-DAF4-421E-B2FE-5624AF52D7E4}");
                public static readonly ID Content2 = new ID("{A639F7DE-D782-46C2-B5B9-FD183FF7231E}");
                public static readonly ID ButtonText2 = new ID("{6F29A275-EC74-443F-8954-C02349DCF59A}");
                public static readonly ID ButtonLink2 = new ID("{4B5AF75A-AD5B-4D20-B37B-0C7E4611DB50}");
            }
        }
    }
}

using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.TabCards
{
    public class Tab3CardModel : TabCardsModel
    {
        public string DateFormat = "yyyy-MM-dd";

        public string CardDateTime1 => ((DateField)Datasource?.Fields[Tab3CardDatasource.Fields.Date1])?.GetLocalDateFieldValue()?.ToString(DateFormat);
        public string CardImageUrl1 { get; set; }
        public string ArticleLink1 { get; set; }

        public string CardDateTime2 => ((DateField)Datasource?.Fields[Tab3CardDatasource.Fields.Date2])?.GetLocalDateFieldValue()?.ToString(DateFormat);
        public string CardImageUrl2 { get; set; }
        public string ArticleLink2 { get; set; }


        public Tab3CardModel(Item item) : base(item)
        {
            if (item == null || item.TemplateID != Tab3CardDatasource.Id)
            {
                return;
            }

            this.CardImageUrl1 = ItemUtils.ImageUrl(item, Tab3CardDatasource.Fields.Image1);
            this.CardImageUrl2 = ItemUtils.ImageUrl(item, Tab3CardDatasource.Fields.Image2);

            this.ArticleLink1 = ItemUtils.GeneralLink(item, Tab3CardDatasource.Fields.ArticleLink1)?.Url ?? "#";
            this.ArticleLink2 = ItemUtils.GeneralLink(item, Tab3CardDatasource.Fields.ArticleLink2)?.Url ?? "#";
        }
    }
}
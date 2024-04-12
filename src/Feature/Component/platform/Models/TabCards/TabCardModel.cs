using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.TabCards
{
    public class TabCardModel : TabCardsModel
    {
        public TabCardModel(Item item) : base(item)
        {
            if (item == null || item.TemplateID != TabCardDatasource.Id)
            {
                return;
            }
        }
    }
}
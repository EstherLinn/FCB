using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.DiscountList
{
    public class DiscountListModel
    {
        public Item DataSource { get; set; }
        public string Guid { get; set; }
        public string TotalPages { get; set; }
        public string TotalCards { get; set; }
        public IEnumerable<DiscountCardListModel> CardList { get; set; }
    }

    public class DiscountCardListModel
    {
        public string CardListClass { get; set; }
        public IEnumerable<DiscountCardItemModel> CardItems { get; set; }
    }

    public class DiscountCardItemModel
    {
        public Item Item { get; set; }
        public string CardImage { get; set; }
        public string Title { get; set; }
        public string DisplayDate { get; set; }
        public string DisplayTag { get; set; }
        public string CardButtonText { get; set; }
        public string CardButtonLink { get; set; }
        public string ExpiryMask { get; set; }
    }

    public struct Templates
    {
        public static readonly ID ID = new ID("{A97CB225-4DE1-45AC-AD06-4D9618769D9D}");

        public struct Fields
        {
            public static readonly ID SettingList = new ID("{80D2F644-32A8-40DA-8A98-6E8587F87F0D}");
        }
    }
}

using Feature.Wealth.Component.Models.DiscountList;
using Sitecore;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class DiscountListRepository
    {
        public DiscountListModel GetCardList(Item item, string dataSourceId, string pagesize)
        {
            IEnumerable<Item> settingListField = ItemUtils.GetMultiListValueItems(item, Models.DiscountList.Templates.Fields.SettingList);
            if (settingListField == null)
            {
                return null;
            }

            List<DiscountCardListModel> cardList = new List<DiscountCardListModel>();
            IEnumerable<IEnumerable<Item>> settingItemsGroups;
            int pageSizeInt;

            if (pagesize == "all")
            {
                pageSizeInt = settingListField.Count();
                settingItemsGroups = settingListField.Select((item, index) => new { Index = index, Item = item })
                                                     .GroupBy(x => x.Index / settingListField.Count())
                                                     .Select(group => group.Select(x => x.Item));
            }
            else
            {
                pageSizeInt = int.Parse(pagesize);
                settingItemsGroups = settingListField.Select((item, index) => new { Index = index, Item = item })
                                                     .GroupBy(x => x.Index / pageSizeInt)
                                                     .Select(group => group.Select(x => x.Item));
            }

            int count = 0;
            foreach (var settingItems in settingItemsGroups)
            {
                var discountCardListModel = new DiscountCardListModel();
                List<DiscountCardItemModel> cardItems = new List<DiscountCardItemModel>();

                foreach (var settingItem in settingItems)
                {
                    var cardModel = new DiscountCardItemModel
                    {
                        Item = settingItem,
                        CardImage = ItemUtils.ImageUrl(settingItem, Models.DiscountContent.Templates.DiscountContentDatasource.Fields.MobileBanner),
                        Title = ItemUtils.GetFieldValue(settingItem, Models.DiscountContent.Templates.DiscountContentDatasource.Fields.MainTitle),
                        DisplayDate = DiscountContentRepository.GetDisplayDate(settingItem),
                        DisplayTag = DiscountContentRepository.GetDisplayTag(settingItem),
                        CardButtonText = ItemUtils.GetFieldValue(settingItem, Models.DiscountContent.Templates.DiscountContentDatasource.Fields.CardButtonText),
                        CardButtonLink = ItemUtils.GeneralLink(settingItem, Models.DiscountContent.Templates.DiscountContentDatasource.Fields.CardButtonLink).Url,
                        ExpiryMask = GetExpiryMask(settingItem)
                    };
                    cardItems.Add(cardModel);
                }

                discountCardListModel.CardItems = cardItems;

                discountCardListModel.CardListClass = count > 0 ? "display: none;" : string.Empty;

                cardList.Add(discountCardListModel);
                count++;
            }

            return new DiscountListModel
            {
                DataSource = item,
                CardList = cardList,
                TotalPages = cardList.Count.ToString(),
                TotalCards = settingListField.Count().ToString(),
                Guid = dataSourceId
            };
        }

        public static string GetExpiryMask(Item item)
        {
            DateTime startDate = ItemUtils.GetLocalDateFieldValue(item, Models.DiscountContent.Templates.DiscountContentDatasource.Fields.StartDate) ?? DateTime.MinValue;
            DateTime endDate = ItemUtils.GetLocalDateFieldValue(item, Models.DiscountContent.Templates.DiscountContentDatasource.Fields.EndDate) ?? DateTime.MinValue;
            DateTime today = DateUtil.ToServerTime(DateTime.UtcNow).Date;

            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue && today > endDate)
            {
                return "c-gridview__media--expire";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
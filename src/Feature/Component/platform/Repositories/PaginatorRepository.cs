using Feature.Wealth.Component.Models.Paginator;
using Sitecore.Data.Items;
using System;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class PaginatorRepository
    {
        public Item GetPreviousPageItem(Item currentItem)
        {
            if (currentItem?.Parent != null)
            {
                Item[] siblings = ItemUtils.GetChildren(currentItem.Parent).ToArray();
                SortingModel sorting = GetSortingOptions(currentItem.Parent);

                if (!string.IsNullOrWhiteSpace(sorting.Field))
                {
                    switch (sorting.Option)
                    {
                        case SortingOptions.Ascending:
                            siblings = siblings.OrderBy(i => i[sorting.Field]).ToArray();
                            break;

                        case SortingOptions.Descending:
                            siblings = siblings.OrderByDescending(i => i[sorting.Field]).ToArray();
                            break;

                        case SortingOptions.None:
                        default:
                            break;
                    }
                }

                int currentIndex = -1;
                for (int i = 0 ; i < siblings.Length ; i++)
                {
                    if (siblings[i].ID == currentItem.ID)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                if (currentIndex > 0)
                {
                    return siblings[currentIndex - 1];
                }
            }

            return null;
        }

        public Item GetNextPageItem(Item currentItem)
        {
            if (currentItem?.Parent != null)
            {
                Item[] siblings = ItemUtils.GetChildren(currentItem.Parent).ToArray();
                SortingModel sorting = GetSortingOptions(currentItem.Parent);

                if (!string.IsNullOrWhiteSpace(sorting.Field))
                {
                    switch (sorting.Option)
                    {
                        case SortingOptions.Ascending:
                            siblings = siblings.OrderBy(i => i[sorting.Field]).ToArray();
                            break;

                        case SortingOptions.Descending:
                            siblings = siblings.OrderByDescending(i => i[sorting.Field]).ToArray();
                            break;

                        case SortingOptions.None:
                        default:
                            break;
                    }
                }

                int currentIndex = -1;
                for (int i = 0 ; i < siblings.Length ; i++)
                {
                    if (siblings[i].ID == currentItem.ID)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                if (currentIndex < siblings.Length - 1)
                {
                    return siblings[currentIndex + 1];
                }
            }

            return null;
        }

        /// <summary>
        /// 取得排序設定
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private SortingModel GetSortingOptions(Item item)
        {
            SortingModel model = new SortingModel();
            model.Field = item.GetFieldValue(Templates.PaginatorFields.Fields.SortingField);
            Item optionItem = item.TargetItem(Templates.PaginatorFields.Fields.SortingOption);
            string optionValue = optionItem.GetFieldValue(ComponentTemplates.DropdownOption.Fields.OptionValue);

            if (!Enum.TryParse(optionValue, out SortingOptions option))
            {
                option = SortingOptions.None;
            }

            model.Option = option;

            return model;
        }
    }
}
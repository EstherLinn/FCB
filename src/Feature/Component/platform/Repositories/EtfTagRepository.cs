using Feature.Wealth.Component.Models.ETF.Tag;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class EtfTagRepository
    {
        public static readonly ID TagSourceFolder = new ID("{A83FD682-0D9C-43D0-BE60-261C8E557690}");

        public Dictionary<TagType, List<ProductTag>> GetTagCollection()
        {
            var dic = GetTagItemSource()
                .GroupBy(i => i.TagType)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList()
                );
            
            return dic;
        }

        private List<ProductTag> GetTagItemSource()
        {
            Item tagSource = ItemUtils.GetItem(TagSourceFolder);
            var tags = tagSource.GetDescendants(ComponentTemplates.TagContent.Id);

            if (tags == null || !tags.Any())
            {
                return null;
            }

            var result = ParseTagContent(tags);
            return result?.ToList();
        }

        public IEnumerable<ProductTag> ParseTagContent(IEnumerable<Item> items)
        {
            foreach (Item tagItem in items)
            {
                string key = tagItem.GetFieldValue(ComponentTemplates.TagContent.Fields.TagKey);

                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                var typeItem = tagItem.TargetItem(ComponentTemplates.TagContent.Fields.Type);
                string typeValue = typeItem.GetFieldValue(ComponentTemplates.DropdownOption.Fields.OptionValue);

                if (!Enum.TryParse(typeValue, out TagType type))
                {
                    continue;
                }

                ProductTag productTag = new ProductTag()
                {
                    TagKey = key,
                    ProductCodes = tagItem.GetMultiLineText(ComponentTemplates.TagContent.Fields.ProductCode)?.ToList(),
                    TagType = type
                };

                yield return productTag;
            }
        }
    }
}
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
            var dic = GetTagItemSource();
            return dic;
        }

        private Dictionary<TagType, List<ProductTag>> GetTagItemSource()
        {
            Item tagSource = ItemUtils.GetItem(TagSourceFolder);
            var categoryList = tagSource.GetDescendants(ComponentTemplates.Category.Id);

            if (categoryList == null || !categoryList.Any())
            {
                return null;
            }

            var dicTag = new Dictionary<TagType, List<ProductTag>>();

            foreach (Item categoryItem in categoryList)
            {
                Item typeItem = categoryItem.TargetItem(ComponentTemplates.Category.Fields.TagType);
                string typeValue = typeItem.GetFieldValue(ComponentTemplates.DropdownOption.Fields.OptionValue);

                if (!Enum.TryParse(typeValue, out TagType type))
                {
                    type = TagType.None;
                }

                List<ProductTag> list = ParseTagContent(categoryItem, type)?.ToList();

                if (dicTag.ContainsKey(type))
                {
                    dicTag[type].AddRange(list);
                }
                else
                {
                    dicTag[type] = list;
                }
            }

            return dicTag;
        }

        private IEnumerable<ProductTag> ParseTagContent(Item categoryItem, TagType tagType)
        {
            var tags = categoryItem.GetDescendants(ComponentTemplates.TagContent.Id);

            foreach (Item tagItem in tags)
            {
                string key = tagItem.GetFieldValue(ComponentTemplates.TagContent.Fields.TagName);

                ProductTag productTag = new ProductTag()
                {
                    TagKey = key,
                    ProductCodes = tagItem.GetMultiLineText(ComponentTemplates.TagContent.Fields.ProductCodeList)?.ToList(),
                    TagType = tagType
                };

                yield return productTag;
            }
        }

        public IEnumerable<ProductTag> ParseTagContent(IEnumerable<Item> items)
        {
            foreach (Item tagItem in items)
            {
                string key = tagItem.GetFieldValue(ComponentTemplates.TagContent.Fields.TagName);
                var categoryItem = tagItem.GetAncestor(ComponentTemplates.Category.Id);
                var typeItem = categoryItem.TargetItem(ComponentTemplates.Category.Fields.TagType);
                string typeValue = typeItem.GetFieldValue(ComponentTemplates.DropdownOption.Fields.OptionValue);

                if (!Enum.TryParse(typeValue, out TagType type))
                {
                    type = TagType.None;
                }

                ProductTag productTag = new ProductTag()
                {
                    TagKey = key,
                    ProductCodes = tagItem.GetMultiLineText(ComponentTemplates.TagContent.Fields.ProductCodeList)?.ToList(),
                    TagType = type
                };

                yield return productTag;
            }
        }
    }
}
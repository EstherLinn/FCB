using System;
using System.Linq;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Feature.Wealth.Component.Models.Tags.TagsModel;

namespace Feature.Wealth.Component.Repositories
{
    public class TagsRepository
    {
        public List<Tags> GetTagData()
        {
            List<Tags> TagModels = new List<Tags>();

            Item TagsFolder = ItemUtils.GetItem(Template.Fields.TagsFolder);

            foreach (var item in TagsFolder.GetDescendants(Template.Fields.FundTags))
            {
                TagModels.Add(new Tags()
                {
                    TagName = item[Template.Fields.TagName],
                    ProductCodes = item[Template.Fields.ProductCodeList].Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                    FundType = item[Template.Fields.FundType]
                });
            }
            
            return TagModels;
        }
    }
}

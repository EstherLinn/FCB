using System;
using System.Linq;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Feature.Wealth.Component.Models.Tags.FundTagsModel;

namespace Feature.Wealth.Component.Repositories
{
    public class TagsRepository
    {
        public List<Tags> GetFundTagData()
        {
            List<Tags> TagModels = new List<Tags>();

            Item TagsFolder = ItemUtils.GetItem(Template.Fields.TagsFolder);

            foreach (var item in TagsFolder.GetChildren(Template.Fields.TagFolder))
            {
                foreach (var i in item.GetChildren(Template.Fields.FundTags))
                {
                    Tags tagModel = new Tags()
                    {
                        FundType = item[Template.Fields.FundType],
                        TagName = i[Template.Fields.TagName],
                        ProductCodes = i[Template.Fields.ProductCodeList].Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList()
                    };

                    TagModels.Add(tagModel);
                }
            }

            return TagModels;
        }

    }
}

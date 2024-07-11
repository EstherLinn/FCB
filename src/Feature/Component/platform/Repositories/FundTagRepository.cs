using System;
using System.Linq;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Feature.Wealth.Component.Models.Tags.FundTagsModel;
using Feature.Wealth.Component.Models.FundDetail;
using Foundation.Wealth.Manager;
using Templates = Feature.Wealth.Component.Models.Tags.FundTagsModel.Template;

namespace Feature.Wealth.Component.Repositories
{
    public class FundTagRepository
    {
        public List<Tags> GetFundTagData()
        {
            List<Tags> TagModels = new List<Tags>();

            Item TagsFolder = ItemUtils.GetItem(Templates.Fields.TagsFolder);

            foreach (var item in TagsFolder.GetChildren(Templates.Fields.TagFolder))
            {
                foreach (var i in item.GetChildren(Templates.Fields.FundTags))
                {
                    Tags tagModel = new Tags()
                    {
                        FundTagType = string.IsNullOrEmpty(item[Templates.Fields.FundType]) ? FundTagEnum.DiscountTag : (FundTagEnum)Enum.Parse(typeof(FundTagEnum), item[Templates.Fields.FundType]),
                        TagName = i[Templates.Fields.TagName],
                        ProductCodes = i[Templates.Fields.ProductCodeList].Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList()
                    };

                    TagModels.Add(tagModel);
                }
            }

            return GetFundTenTagData(TagModels);
        }

        //十大主題+百元基金
        public List<Tags> GetFundTenTagData(List<Tags> data)
        {
            var db_data = new List<FundSortTagModel>();
            db_data = DbManager.Custom.ExecuteIList<FundSortTagModel>("sp_FundTags", null, commandType: System.Data.CommandType.StoredProcedure)?.ToList();
            if (db_data != null)
            {
                foreach (var item in db_data)
                {
                    data.Add(new Tags()
                    {
                        TagName = item.TopicName,
                        FundTagType = item.TopicName == "百元基金" ? FundTagEnum.DiscountTag : FundTagEnum.SortTag,
                        ProductCodes = item.FundId.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
                    });
                }
                for (int i = 0; i < data.Count(); i++)
                {
                    if (data[i].TagName == "百元基金")
                    {
                        data.Insert(0, data[i]);
                        data.RemoveAt(i + 1);
                        break;
                    }
                }
            }
            return data;
        }

        //基金搜尋頁熱門關鍵字顯示用
        public List<string> GetFundTenTagNameData()
        {
            var topicname = new List<string>();

            List<FundSortTagModel> db_data = DbManager.Custom.ExecuteIList<FundSortTagModel>("sp_FundTags", null, commandType: System.Data.CommandType.StoredProcedure)?.ToList();
            if (db_data != null)
            {
                foreach (var item in db_data)
                {
                    topicname.Add(item.TopicName);
                }
            }
            return topicname;
        }
    }
}

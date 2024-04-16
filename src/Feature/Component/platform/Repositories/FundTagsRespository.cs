using Feature.Wealth.Component.Models.FundDetail;
using Foundation.Wealth.Manager;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Repositories
{
    public static class FundTagsRespository
    {
        private readonly static MemoryCache _cache = MemoryCache.Default;

        private static List<FundTagModel> GetTagData()
        {
            List<FundTagModel> fundTagModels = new List<FundTagModel>();
            Item tagFolder = ItemUtils.GetItem(ComponentTemplates.FundTag.FundTagRoot);
            foreach (var item in tagFolder.GetChildren(ComponentTemplates.FundTag.FundTagItem))
            {
                fundTagModels.Add(new FundTagModel()
                {
                    FundTagTitle = item[ComponentTemplates.FundTag.Fields.FundTagTitle],
                    FundTagType = string.IsNullOrEmpty(item[ComponentTemplates.FundTag.Fields.FundTagType]) ? FundTagEnum.DiscountTag : (FundTagEnum)Enum.Parse(typeof(FundTagEnum), item[ComponentTemplates.FundTag.Fields.FundTagType]),
                    FundIdList = item[ComponentTemplates.FundTag.Fields.FundIdList].Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                });
            }
            return GetTagDataFromDataBase(fundTagModels);
        }
        private static List<FundTagModel> GetTagDataFromDataBase(List<FundTagModel> data)
        {
            var db_data = new List<FundSortTagModel>();
            //十大主題+百元基金
            db_data = DbManager.Custom.ExecuteIList<FundSortTagModel>("sp_FundTags", null, commandType: System.Data.CommandType.StoredProcedure)?.ToList();
            if (db_data != null)
            {
                foreach (var item in db_data)
                {
                    data.Add(new FundTagModel()
                    {
                        FundTagTitle = item.TopicName,
                        FundTagType = item.TopicName == "百元基金" ? FundTagEnum.DiscountTag : FundTagEnum.SortTag,
                        FundIdList = item.FundId.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
                    });
                }

                //百元基金排序第一位
                for (int i = 0; i < data.Count(); i++)
                {
                    if (data[i].FundTagTitle == "百元基金")
                    {
                        data.Insert(0, data[i]);
                        data.RemoveAt(i + 1);
                        break;
                    }
                }
            }
            return data;
        }
        public static Dictionary<FundTagEnum, List<FundTagModel>> GetAllTagListFromCache()
        {
            var data = _cache.AddOrGetExisting($"FundTagsData", _ => GetTagData(), DateTimeOffset.Now.AddMinutes(60));
            Dictionary<FundTagEnum, List<FundTagModel>> dic = new Dictionary<FundTagEnum, List<FundTagModel>>();
            dic.Add(FundTagEnum.DiscountTag, data.Where(x => x.FundTagType == FundTagEnum.DiscountTag).ToList());
            dic.Add(FundTagEnum.SortTag, data.Where(x => x.FundTagType == FundTagEnum.SortTag).ToList());
            return dic;
        }
    }
}

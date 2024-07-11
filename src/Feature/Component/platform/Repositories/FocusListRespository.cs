﻿using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.ReachInfo;
using Feature.Wealth.Account.Repositories;
using Feature.Wealth.Component.Models.ETF.Tag;
using Feature.Wealth.Component.Models.FocusList;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.Invest;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Templates = Feature.Wealth.Component.Models.USStock.Template;

namespace Feature.Wealth.Component.Repositories
{
    public class FocusListRespository
    {
        public List<FundListModel> GetFundFocusData(List<string> fundFocusList)
        {
            string sql = $@"
                  SELECT ProductCode,ProductName,NetAssetValue,AvailabilityStatus,Format(NetAssetValueDate,'yyyy/MM/dd') NetAssetValueDate,
                    CurrencyName,OnlineSubscriptionAvailability,OneMonthReturnOriginalCurrency,SixMonthReturnOriginalCurrency,OneYearReturnOriginalCurrency
                FROM [vw_BasicFund] WHERE ProductCode in @fundlist
                  ORDER BY SixMonthReturnOriginalCurrency DESC
                ";
            var para = new { fundlist = fundFocusList };
            var results = DbManager.Custom.ExecuteIList<FundListModel>(sql, para, CommandType.Text)?.ToList();
            return results;
        }

        public List<EtfListModel> GetETFFocusData(List<string> etfFocusList)
        {
            string sql = $@"
                  SELECT ProductCode,ProductName,AvailabilityStatus,MarketPrice,Format(MarketPriceDate,'yyyy/MM/dd') MarketPriceDate,
                    CurrencyName,OnlineSubscriptionAvailability,SixMonthReturnMarketPriceOriginalCurrency,NetAssetValueChangePercentage
                    FROM [vw_BasicETF] WHERE ProductCode in @etfList
                  ORDER BY SixMonthReturnMarketPriceOriginalCurrency DESC
                ";
            var para = new { etfList = etfFocusList };
            var results = DbManager.Custom.ExecuteIList<EtfListModel>(sql, para, CommandType.Text)?.ToList();
            return results;
        }
        public List<ForeignStockListModel> GetForeignStockFocusData(List<string> foreignStockFocusList)
        {
            string sql = $@"
                  SELECT 
                           A.[FirstBankCode] ProductCode
                           ,A.[FundCode]
                           ,A.[FirstBankCode] + ' ' + A.[ChineseName] + ' ' + A.[EnglishName] [ProductName]
                           ,REPLACE(CONVERT(char(10), A.[DataDate],126),'-','/') [ClosingPriceDate]
                           ,A.[ClosingPrice]
                           ,CONVERT(decimal(16,2), A.[ChangePercentage]) [ChangePercentage]
                           ,CONVERT(decimal(16,2), A.[MonthlyReturn]) [MonthlyReturn]
                           ,B.[OnlineSubscriptionAvailability]
                           ,B.[AvailabilityStatus]
                           FROM [Sysjust_USStockList] A WITH (NOLOCK)
                           LEFT JOIN [WMS_DOC_RECM] B WITH (NOLOCK) ON A.[FirstBankCode] = B.[ProductCode] WHERE FirstBankCode in @foreignStockList order by [MonthlyReturn] desc
                ";
            var para = new { foreignStockList = foreignStockFocusList };
            var results = DbManager.Custom.ExecuteIList<ForeignStockListModel>(sql, para, CommandType.Text)?.ToList();
            return results;
        }
        public List<FundListModel> SetTagsToFund(List<FundListModel> baseList)
        {
            FundTagRepository fundTagRepository = new FundTagRepository();
            var data = fundTagRepository.GetFundTagData();
            var sortTags = data.Where(x => x.FundTagType == FundTagEnum.DiscountTag);
            var tags = new List<string>();
            if (sortTags != null && sortTags.Any())
            {
                foreach (var item in baseList)
                {
                    foreach (var item2 in sortTags)
                    {
                        if (item2.ProductCodes.Contains(item.ProductCode))
                        {
                            item.Tags.Add(item2.TagName);
                        }
                    }
                }

            }

            return baseList;
        }
        public List<EtfListModel> SetTagsToEtf(List<EtfListModel> baseList)
        {
            EtfTagRepository TagRepositor = new EtfTagRepository();
            var dicTag = TagRepositor.GetTagCollection();
            var tags = dicTag[TagType.Discount];
            if (tags != null && tags.Any())
            {
                foreach (var item in baseList)
                {
                    foreach (var d in tags)
                    {
                        if (d.ProductCodes.Contains(item.ProductCode))
                        {
                            item.Tags.Add(d.TagKey);
                        }
                    }
                }

            }
            return baseList;
        }
        public List<ForeignStockListModel> SetTagsToForeignStock(List<ForeignStockListModel> baseList)
        {
            var tagFolder = ItemUtils.GetContentItem(Templates.USStockTagFolder.Id);
            var children = ItemUtils.GetChildren(tagFolder, Templates.TagFolder.Id);
            var discounts = new List<Item>();

            if (children != null && children.Any())
            {
                foreach (var child in children)
                {
                    var tagsType = ItemUtils.GetFieldValue(child, Templates.TagFolder.Fields.TagType);

                    if (tagsType == "Discount")
                    {
                        discounts.AddRange(ItemUtils.GetChildren(child, Templates.USStockTag.Id));
                    }
                }
            }

            foreach (var item in baseList)
            {
                foreach (var d in discounts)
                {
                    string tagName = ItemUtils.GetFieldValue(d, Templates.USStockTag.Fields.TagName);
                    string productCodeList = ItemUtils.GetFieldValue(d, Templates.USStockTag.Fields.ProductCodeList);
                    if (productCodeList.Contains(item.ProductCode))
                    {
                        item.Tags.Add(tagName);
                    }
                }
            }

            return baseList;
        }

        public List<T> SetButtonToFocusList<T>(List<T> baseList, InvestTypeEnum typeEnum) where T : FocusListBaseModel
        {

            foreach (var item in baseList)
            {
                item.Button = new Button
                {
                    CompareButtonHtml = PublicHelpers.CompareButton(null, null, item.ProductCode, item.ProductName, typeEnum, true).ToString(),
                    InfoButtonHtml = PublicHelpers.InfoButton(null, null, item.ProductCode, item.ProductName, typeEnum).ToString(),
                    FocusButtonHtml = PublicHelpers.FocusCancelButton(null, null, item.ProductCode, item.ProductName, typeEnum).ToString(),
                    SubscriptionButtonHtml = PublicHelpers.SubscriptionButton(null, null, item.ProductCode, typeEnum, true).ToString(),
                };
            }
            return baseList;
        }


        public List<T> SetReachInfoToFocusList<T>(List<T> baseList, InvestTypeEnum typeEnum) where T : FocusListBaseModel
        {
            var reachInfoRepository = new ReachInfoRepository();

            var reachInfos = reachInfoRepository.GetProductReachInfosByType(FcbMemberHelper.GetMemberPlatFormId(), typeEnum.ToString());
            if (reachInfos == null)
            {
                return baseList;
            }

            foreach (var item in reachInfos)
            {
                foreach (var item2 in baseList)
                {
                    if (item.InvestId != item2.ProductCode)
                    {
                        continue;
                    }
                    if (item2.Info == null)
                    {
                        switch (int.Parse(item.InfoType))
                        {
                            case (int)InfoTypeEnum.ReachValue:
                                item2.Info = new Info
                                {
                                    PriceValue = item.PriceValue,
                                    InvestId = item.InvestId,
                                    ReachValue = item.ReachValue,
                                };
                                break;
                            case (int)InfoTypeEnum.QuoteChange:
                                item2.Info = new Info
                                {
                                    RiseFallPriceValue = item.PriceValue,
                                    InvestId = item.InvestId,
                                    RiseValue = item.RiseValue,
                                    RisePercent = item.RisePercent,
                                    FallValue = item.FallValue,
                                    FallPercent = item.FallPercent,
                                };
                                break;
                        }

                    }
                    else
                    {
                        switch (int.Parse(item.InfoType))
                        {
                            case (int)InfoTypeEnum.ReachValue:
                                item2.Info.PriceValue = item.PriceValue;
                                item2.Info.ReachValue = item.ReachValue;
                                break;
                            case (int)InfoTypeEnum.QuoteChange:
                                item2.Info.RiseFallPriceValue = item.PriceValue;
                                item2.Info.RiseValue = item.RiseValue;
                                item2.Info.RisePercent = item.RisePercent;
                                item2.Info.FallValue = item.FallValue;
                                item2.Info.FallPercent = item.FallPercent;
                                break;
                        }
                    }

                }
            }
            return baseList;
        }



    }
}

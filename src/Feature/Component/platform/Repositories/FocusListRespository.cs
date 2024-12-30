using Feature.Wealth.Account.Helpers;
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
using Feature.Wealth.Component.Models.Bond;
using BondTemplates = Feature.Wealth.Component.Models.Bond.Template;
using System;
using Foundation.Wealth.Models;
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
            string Sysjust_USStockList = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_USStockList);
            string WMS_DOC_RECM = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.WMS_DOC_RECM);
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
                           FROM {Sysjust_USStockList} A WITH (NOLOCK)
                           LEFT JOIN {WMS_DOC_RECM} B WITH (NOLOCK) ON A.[FirstBankCode] = B.[ProductCode] WHERE FirstBankCode in @foreignStockList order by [MonthlyReturn] desc
                ";
            var para = new { foreignStockList = foreignStockFocusList };
            var results = DbManager.Custom.ExecuteIList<ForeignStockListModel>(sql, para, CommandType.Text)?.ToList();
            return results;
        }
        public List<ForeignBondListModel> GetForeignBondFocusData(List<string> foreignBondFocusList)
        {
            string BondList = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.BondList);
            string BondNav = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.BondNav);
            string sql = $@"SELECT 
                            A.[BondCode]
                            ,A.[BondCode] + ' ' + A.[BondName] AS [FullName]
                            ,[PaymentFrequency]
                            ,CASE 
                            WHEN A.[PaymentFrequency] = 0
                            THEN '零息'
                            WHEN A.[PaymentFrequency] = 1
                            THEN '月配'
                            WHEN A.[PaymentFrequency] = 2
                            THEN '季配'
                            WHEN A.[PaymentFrequency] = 3
                            THEN '半年配'
                            WHEN A.[PaymentFrequency] = 4
                            THEN '年配'
                            ELSE ''
                            END AS [PaymentFrequencyName]
                            ,SUBSTRING(A.[MaturityDate],1,4)+'/'+SUBSTRING(A.[MaturityDate],5,2)+'/'+SUBSTRING(A.[MaturityDate],7,2) AS [MaturityDate]
                            ,A.[OpenToPublic]
                            ,A.[Listed]                         
                            ,CASE 
	                        WHEN ISNUMERIC(EF.BankBuyPrice) = 1
	                        THEN EF.BankBuyPrice
	                        ELSE NULL
	                        END AS [SubscriptionFee]
                            ,CASE 
	                        WHEN ISNUMERIC(EF.BankSellPrice) = 1
	                        THEN EF.BankSellPrice
	                        ELSE NULL
	                        END AS [RedemptionFee]
                            ,CASE 
                            WHEN EF.PriceBaseDate IS NOT NULL 
                            THEN FORMAT(TRY_CAST(CONCAT((TRY_CONVERT(INT, LEFT(EF.PriceBaseDate, 3)) + 1911), RIGHT(EF.PriceBaseDate, 4)) AS DATE), 'yyyy/MM/dd')
                            END AS [Date]
                            FROM {BondList} AS A WITH (NOLOCK)
                            LEFT JOIN {BondNav} AS B WITH (NOLOCK) ON A.BondCode = SUBSTRING(B.BondCode, 1, 4)
                            LEFT JOIN
                            (
                            SELECT 
                            [ProductIdentifier]
                            ,[DataDate]
                            ,[BankProductCode]
                            ,[BankBuyPrice]
                            ,[BankSellPrice]
                            ,[PriceBaseDate]
                            ,ROW_NUMBER() OVER(PARTITION BY [BankProductCode] ORDER BY [PriceBaseDate] DESC) AS [RowNumber]
                            FROM [FUND_ETF] WITH (NOLOCK)
                            WHERE [ProductIdentifier] = 'B'
                            ) AS EF ON A.BondCode = EF.BankProductCode AND EF.[RowNumber] = 1
                            WHERE A.BondCode in @BondCodes
                            ORDER BY A.BondCode
                ";
            var para = new { BondCodes = foreignBondFocusList };
            var results = DbManager.Custom.ExecuteIList<ForeignBondListModel>(sql, para, CommandType.Text)?.ToList();

            foreach (var result in results)
            {
                result.SubscriptionFee = result.SubscriptionFee * 100;
                result.RedemptionFee = result.RedemptionFee * 100;
            }

            return results;
        }

        public List<BondHistoryPrice> GetBondHistoryPrice(List<ForeignBondListModel> foreignBondFocusList)
        {
            List<BondHistoryPrice> bondHistoryPrices = null;
            if (foreignBondFocusList == null || !foreignBondFocusList.Any())
            {
                return bondHistoryPrices;
            }
            var minDate = foreignBondFocusList
                .Where(b => string.IsNullOrEmpty(b.Date) == false)
                .OrderBy(b => b.Date)
                .Select(b => b.Date).FirstOrDefault();

            if (DateTime.TryParse(minDate, out var fourMonthAgo))
            {
                fourMonthAgo = fourMonthAgo.AddMonths(-4);
            }
            //取四個月內資料
            string sql = @"SELECT
                           SUBSTRING([BondCode], 1, 4) AS [BondCode]
                           ,[Currency]
                           ,[SubscriptionFee]
                           ,[RedemptionFee]
                           ,[Date]
                           FROM [BondHistoryPrice] WITH (NOLOCK)
                           WHERE [Date] >= @date and BondCode in @BondCodes
                           ORDER BY Date DESC";
            var para = new { date = fourMonthAgo.ToString("yyyyMMdd"), BondCodes = foreignBondFocusList.Select(y => y.BondCode).ToList() };
            bondHistoryPrices = DbManager.Custom.ExecuteIList<BondHistoryPrice>(sql.ToString(), para, CommandType.Text)?.ToList();
            return bondHistoryPrices;
        }

        public List<ForeignBondListModel> CalculateUpsAndDowns(List<ForeignBondListModel> foreignBondFocusList, List<BondHistoryPrice> bondHistoryPrices)
        {
            if (foreignBondFocusList == null || bondHistoryPrices == null)
            {
                return foreignBondFocusList;
            }
            foreach (var item in foreignBondFocusList)
            {
                if (DateTime.TryParse(item.Date, out var oneMonthAgo))
                {
                    oneMonthAgo = oneMonthAgo.AddMonths(-1);
                }

                if (DateTime.TryParse(item.Date, out var threeMonthAgo))
                {
                    threeMonthAgo = threeMonthAgo.AddMonths(-3);
                }
                var monthFee = bondHistoryPrices.Where(x => x.BondCode == item.BondCode && int.Parse(x.Date) <= int.Parse(oneMonthAgo.ToString("yyyyMMdd"))).FirstOrDefault()?.SubscriptionFee;
                var seasonFee = bondHistoryPrices.Where(x => x.BondCode == item.BondCode && int.Parse(x.Date) <= int.Parse(threeMonthAgo.ToString("yyyyMMdd"))).FirstOrDefault()?.SubscriptionFee;
                //月
                if (item.SubscriptionFee.HasValue && item.SubscriptionFee > 0 && monthFee.HasValue && monthFee > 0)
                {
                    item.UpsAndDownsMonth = Round2((item.SubscriptionFee - monthFee) / monthFee * 100);
                    if (item.UpsAndDownsMonth != 0)
                    {
                        item.UpsAndDownsMonthStyle = item.UpsAndDownsMonth > 0 ? "o-rise" : "o-fall";
                    }
                }
                else
                {
                    //申購價為0時用贖回價計算漲跌月
                    monthFee = bondHistoryPrices.Where(x => x.BondCode == item.BondCode && int.Parse(x.Date) <= int.Parse(oneMonthAgo.ToString("yyyyMMdd"))).FirstOrDefault()?.RedemptionFee;
                    if (item.RedemptionFee.HasValue && item.RedemptionFee > 0 && monthFee.HasValue && monthFee > 0)
                    {
                        item.UpsAndDownsMonth = Round2((item.RedemptionFee - monthFee) / monthFee * 100);
                        if (item.UpsAndDownsMonth != 0)
                        {
                            item.UpsAndDownsMonthStyle = item.UpsAndDownsMonth > 0 ? "o-rise" : "o-fall";
                        }
                    }
                }
                //季
                if (item.SubscriptionFee.HasValue && item.SubscriptionFee > 0 && seasonFee.HasValue && seasonFee > 0)
                {
                    item.UpsAndDownsSeason = Round2((item.SubscriptionFee - seasonFee) / seasonFee * 100);
                    if (item.UpsAndDownsSeason != 0)
                    {
                        item.UpsAndDownsSeasonStyle = item.UpsAndDownsSeason > 0 ? "o-rise" : "o-fall";
                    }
                }
                else
                {
                    seasonFee = bondHistoryPrices.Where(x => x.BondCode == item.BondCode && int.Parse(x.Date) <= int.Parse(threeMonthAgo.ToString("yyyyMMdd"))).FirstOrDefault()?.RedemptionFee;
                    if (item.RedemptionFee.HasValue && item.RedemptionFee > 0 && seasonFee.HasValue && seasonFee > 0)
                    {
                        //申購價為0時用贖回價計算漲跌季
                        item.UpsAndDownsSeason = Round2((item.RedemptionFee - seasonFee) / seasonFee * 100);
                        if (item.UpsAndDownsSeason != 0)
                        {
                            item.UpsAndDownsSeasonStyle = item.UpsAndDownsSeason > 0 ? "o-rise" : "o-fall";
                        }
                    }
                }
            }
            return foreignBondFocusList;
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
        public List<ForeignBondListModel> SetTagsToForeignBond(List<ForeignBondListModel> baseList)
        {
            if (baseList == null || !baseList.Any())
            {
                return baseList;
            }
            var tagFolder = ItemUtils.GetContentItem(BondTemplates.BondTagFolder.Id);
            var children = ItemUtils.GetChildren(tagFolder, BondTemplates.TagFolder.Id);
            var discounts = new List<Item>();

            if (children != null && children.Any())
            {
                foreach (var child in children)
                {
                    var tagsType = ItemUtils.GetFieldValue(child, BondTemplates.TagFolder.Fields.TagType);

                    if (tagsType == "Discount")
                    {
                        discounts.AddRange(ItemUtils.GetChildren(child, BondTemplates.BondTag.Id));
                    }
                }
            }

            foreach (var item in baseList)
            {
                foreach (var d in discounts)
                {
                    string tagName = ItemUtils.GetFieldValue(d, BondTemplates.BondTag.Fields.TagName);
                    string productCodeList = ItemUtils.GetFieldValue(d, BondTemplates.BondTag.Fields.ProductCodeList);
                    if (productCodeList.Contains(item.BondCode))
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
        public List<ForeignBondListModel> SetButtonToBondFocusList(List<ForeignBondListModel> baseList, InvestTypeEnum typeEnum)
        {

            if (baseList == null || !baseList.Any())
            {
                return baseList;
            }
            foreach (var item in baseList)
            {
                item.Button = new Button
                {
                    FocusButtonHtml = PublicHelpers.FocusCancelButton(null, null, item.BondCode, string.Empty, typeEnum).ToString(),
                    SubscriptionButtonHtml = PublicHelpers.SubscriptionButton(null, null, item.BondCode, typeEnum, true).ToString(),
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
                                    ReachValueOpen = item.OpenInfo,
                                    ReachValueSetDate = item.SetDateTime?.ToString("yyyy/MM/dd")
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
                                    QuoteChangeOpen = item.OpenInfo,
                                    QuoteChangeSetDate = item.SetDateTime?.ToString("yyyy/MM/dd")
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
                                item2.Info.ReachValueOpen = item.OpenInfo;
                                item2.Info.ReachValueSetDate = item.SetDateTime?.ToString("yyyy/MM/dd");
                                break;
                            case (int)InfoTypeEnum.QuoteChange:
                                item2.Info.RiseFallPriceValue = item.PriceValue;
                                item2.Info.RiseValue = item.RiseValue;
                                item2.Info.RisePercent = item.RisePercent;
                                item2.Info.FallValue = item.FallValue;
                                item2.Info.FallPercent = item.FallPercent;
                                item2.Info.QuoteChangeOpen = item.OpenInfo;
                                item2.Info.QuoteChangeSetDate = item.SetDateTime?.ToString("yyyy/MM/dd");
                                break;
                        }
                    }

                }
            }
            return baseList;
        }

        public decimal? Round2(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round((decimal)value, 2, System.MidpointRounding.AwayFromZero);
            }
            return value;
        }

    }
}

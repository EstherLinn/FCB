using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.ReachInfo;
using Feature.Wealth.Account.Repositories;
using Feature.Wealth.Component.Models.FocusList;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.Invest;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Repositories
{
    public class FocusListRespository
    {
        public List<FundListModel> GetFundFocusData(List<string> fundFocusList)
        {
            string sql = $@"
                  SELECT ProductCode,ProductName,NetAssetValue,Format(NetAssetValueDate,'yyyy/MM/dd') NetAssetValueDate,
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
                  SELECT ProductCode,ProductName,NetAssetValue,Format(NetAssetValueDate,'yyyy/MM/dd') NetAssetValueDate,
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
                           ,CONVERT(decimal(16,2), A.[SixMonthReturn]) [SixMonthReturn]
                           ,B.[OnlineSubscriptionAvailability]
                           FROM [Sysjust_USStockList] A WITH (NOLOCK)
                           LEFT JOIN [WMS_DOC_RECM] B WITH (NOLOCK) ON A.[FirstBankCode] = B.[ProductCode] WHERE FirstBankCode in @foreignStockList order by [SixMonthReturn]
                ";
            var para = new { foreignStockList = foreignStockFocusList };
            var results = DbManager.Custom.ExecuteIList<ForeignStockListModel>(sql, para, CommandType.Text)?.ToList();
            return results;
        }
        //public List<FundListModel> SetTagsToFund(List<FundListModel> baseList)
        //{
        //    var data = FundTagsRespository.GetAllTagListFromCache();
        //    var tags = new List<string>();
        //    if (data != null)
        //    {
        //        var fundTagsList = data[0];
        //        if (fundTagsList != null && fundTagsList.Any())
        //        {
        //            foreach (var item in baseList)
        //            {
        //                tags.Clear();
        //                foreach (var item2 in fundTagsList)
        //                {
        //                    if (item2.FundIdList.Contains(item.ProductCode))
        //                    {
        //                        tags.Add(item2.FundTagTitle);
        //                    }
        //                    item.Tags.AddRange(tags);
        //                }
        //            }
        //        }

        //    }

        //    return baseList;
        //}


        public List<T> SetButtonToFocusList<T>(List<T> baseList, InvestTypeEnum typeEnum) where T : FocusListBaseModel
        {
           
            foreach (var item in baseList)
            {
                item.Button = new Button
                {
                    CompareButtonHtml = PublicHelpers.CompareButton(null, null, item.ProductCode, item.ProductName, typeEnum, true).ToString(),
                    InfoButtonHtml = PublicHelpers.InfoButton(null, null, item.ProductCode, item.ProductName, typeEnum).ToString(),
                    FocusButtonHtml = PublicHelpers.FocusCancelButton(null, null, item.ProductCode, item.ProductName, typeEnum).ToString(),
                    SubscriptionButtonHtml = PublicHelpers.SubscriptionButton(null,null,item.ProductCode, typeEnum, true).ToString(),
                };
            }
            return baseList;
        }


        public List<T> SetReachInfoToFocusList<T>(List<T> baseList, InvestTypeEnum typeEnum) where T : FocusListBaseModel
        {
            var reachInfoRepository = new ReachInfoRepository();

            var reachInfos = reachInfoRepository.GetAllProductReachInfos(FcbMemberHelper.GetMemberPlatFormId(),typeEnum.ToString());
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
                        switch (Int32.Parse(item.InfoType))
                        {
                            case (int)InfoTypeEnum.ReachValue:
                                item2.Info = new Info
                                {
                                    InvestId = item.InvestId,
                                    ReachValue = item.ReachValue,
                                };
                                break;
                            case (int)InfoTypeEnum.QuoteChange:
                                item2.Info = new Info
                                {
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
                        switch (Int32.Parse(item.InfoType))
                        {
                            case (int)InfoTypeEnum.ReachValue:
                                item2.Info.ReachValue = item.ReachValue;
                                break;
                            case (int)InfoTypeEnum.QuoteChange:
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

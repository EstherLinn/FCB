using Feature.Wealth.Component.Models.Bond;
using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Tag;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.IndexRecommendation;
using Feature.Wealth.Component.Models.USStock;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Component.Repositories
{
    public class IndexRecommendationRepository
    {
        private readonly VisitCountRepository _repository = new VisitCountRepository();
        private readonly USStockRepository _uSStockRepository = new USStockRepository();
        private readonly BondRepository _bondRepository = new BondRepository();

        private readonly ILog _log = Logger.General;

        public IList<Funds> GetFundData(string pageId)
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = """
                   SELECT TOP 5 A.*
                   FROM [vw_BasicFund] AS A
                   LEFT JOIN
                   (
                   SELECT *
                   FROM VisitCount WITH (NOLOCK)
                   WHERE PageId = @PageId
                   ) AS B ON A.ProductCode = REPLACE(B.QueryStrings, 'id=', '')
                   WHERE B.VisitCount IS NOT NULL AND B.VisitCount > 0
                   ORDER BY B.VisitCount DESC, A.ProductCode ASC
                   """;

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, new { PageId = pageId }, CommandType.Text);

            var _tagsRepository = new FundTagRepository();
            var tags = _tagsRepository.GetFundTagData();
            foreach (var item in results ?? Enumerable.Empty<Funds>())
            {
                ProcessFundFilterDatas(item);
                item.Tags = [];
                item.Tags.AddRange(from tagModel in tags.Where(t => t.FundTagType == FundTagEnum.DiscountTag)
                                   where tagModel.ProductCodes.Contains(item.ProductCode)
                                   select tagModel.TagName);
                fundItems.Add(item);
            }
            return fundItems;

        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.FundName = item.FundName?.Normalize(NormalizationForm.FormKC);
            item.SixMonthReturnOriginalCurrency = NumberExtensions.RoundingPercentage(item.SixMonthReturnOriginalCurrency);
            item.NetAssetValue = NumberExtensions.RoundingValue(item.NetAssetValue);
            item.PercentageChangeInFundPrice = NumberExtensions.RoundingPercentage((item.PercentageChangeInFundPrice * 100));

            if (item.SixMonthReturnOriginalCurrency < 0)
            {
                item.SixMonthReturnOriginalCurrencyFormat = item.SixMonthReturnOriginalCurrency.ToString().Substring(1);
            }

            if (item.PercentageChangeInFundPrice < 0)
            {
                item.PercentageChangeInFundPriceFormat = item.PercentageChangeInFundPrice.ToString().Substring(1);
            }
        }

        public IList<Funds> GetFundsDatas()
        {
            try
            {
                var queryitem = FundRelatedSettingModel.GetFundDetailPageItem();
                var query = queryitem?.ID.ToGuid();

                var results = GetFundData(query.ToString());

                return results;
            }
            catch (Exception ex)
            {
                this._log.Error("人氣基金", ex);
                return new List<Funds>();
            }
        }


        public IList<ETFs> GetETFData(string pageId)
        {
            List<ETFs> etfsItems = new List<ETFs>();

            string sql = """
                   SELECT TOP 5 A.*
                   FROM [vw_BasicETF] AS A
                   LEFT JOIN
                   (
                   SELECT *
                   FROM VisitCount WITH (NOLOCK)
                   WHERE PageId = @PageId
                   ) AS B ON A.ProductCode = REPLACE(B.QueryStrings, 'id=', '')
                   WHERE B.VisitCount IS NOT NULL AND B.VisitCount > 0
                   ORDER BY B.VisitCount DESC, A.SixMonthReturnMarketPriceOriginalCurrency DESC, A.ProductCode ASC                   
                   """;

            var results = DbManager.Custom.ExecuteIList<ETFs>(sql, new { PageId = pageId }, CommandType.Text);

            foreach (var item in results ?? Enumerable.Empty<ETFs>())
            {
                item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC);
                etfsItems.Add(item);
            }

            return etfsItems;
        }

        public IList<ETFs> GetETFDatas()
        {
            try
            {
                var queryitem = EtfRelatedLinkSetting.GetETFDetailPageItem();
                var query = queryitem.ID.ToGuid();

                var results = GetETFData(query.ToString());

                EtfTagRepository tagRepository = new EtfTagRepository();
                var dicTag = tagRepository.GetTagCollection();

                foreach (var item in results)
                {
                    if (dicTag.ContainsKey(TagType.Discount))
                    {
                        var discountTags = dicTag[TagType.Discount]
                            .Where(tag => tag.ProductCodes.Any() && tag.ProductCodes.Contains(item.ProductCode))
                            .Select(tag => tag.TagKey)
                            .ToArray();

                        item.ETFDiscountTags = discountTags;
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                this._log.Error("人氣ETF", ex);
                return new List<ETFs>();
            }
        }

        public IList<USStock> GetUSStockData(string pageId)
        {
            List<USStock> uSStockItems = new List<USStock>();

            string Sysjust_USStockList = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_USStockList);
            string WMS_DOC_RECM = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.WMS_DOC_RECM);
            string sql = $@"
                    SELECT TOP 5
                    A.[FirstBankCode]
                    ,A.[FundCode]
                    ,A.[FirstBankCode] + ' ' + A.[ChineseName] + ' ' + A.[EnglishName] [FullName]
                    ,REPLACE(CONVERT(char(10), A.[DataDate],126),'-','/') [DataDate]
                    ,A.[ClosingPrice]
                    ,CONVERT(nvarchar, CONVERT(MONEY, A.[ClosingPrice]), 1) [ClosingPriceText]
                    ,CONVERT(decimal(16,2), A.[DailyReturn]) [DailyReturn]
                    ,CONVERT(decimal(16,2), A.[MonthlyReturn]) [MonthlyReturn]
                    ,B.[OnlineSubscriptionAvailability]
                    ,B.[AvailabilityStatus]
                    FROM {Sysjust_USStockList} A WITH (NOLOCK)
                    LEFT JOIN {WMS_DOC_RECM} B WITH (NOLOCK) ON A.[FirstBankCode] = B.[ProductCode]
                    LEFT JOIN
                    (
                    SELECT *
                    FROM VisitCount WITH (NOLOCK)
                    WHERE PageId = @PageId
                    ) AS C ON A.FirstBankCode = REPLACE(C.QueryStrings, 'id=', '')
                    WHERE C.VisitCount IS NOT NULL AND C.VisitCount > 0
                    ORDER BY C.VisitCount DESC, A.MonthlyReturn DESC, A.[FirstBankCode] ASC
                    ";

            var results = DbManager.Custom.ExecuteIList<USStock>(sql, new { PageId = pageId }, CommandType.Text);
            for (int i = 0; i < (results?.Count ?? 0); i++)
            {
                var uSStock = results[i];
                uSStock.DetailLink = USStockRelatedLinkSetting.GetUSStockDetailUrl() + "?id=" + uSStock.FirstBankCode;
                uSStock = this._uSStockRepository.GetButtonHtml(uSStock, true);
                uSStock = this._uSStockRepository.SetTags(uSStock);

                uSStockItems.Add(uSStock);
            }

            return uSStockItems;
        }

        public IList<USStock> GetUSStockDatas()
        {
            try
            {
                var queryitem = USStockRelatedLinkSetting.GetUSStockDetailPageItem();
                var query = queryitem.ID.ToGuid();

                var results = GetUSStockData(query.ToString());

                return results;
            }
            catch (Exception ex)
            {
                this._log.Error("人氣國外股票", ex);
                return new List<USStock>();
            }
        }

        public IList<Bond> GetBondData(string pageId)
        {

            string BondList = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.BondList);

            string sql = @$"
                    SELECT TOP 5
                    A.[BondCode] + ' ' + A.[BondName] AS [FullName]
                    ,A.[BondCode]
                    ,C.[CurrencyName]
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
                    LEFT JOIN [Currency] AS C WITH (NOLOCK) ON A.CurrencyCode = C.CurrencyCode
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
                    LEFT JOIN
                    (
                    SELECT *
                    FROM VisitCount WITH (NOLOCK)
                    WHERE PageId = @PageId
                    ) AS D ON A.BondCode = REPLACE(D.QueryStrings, 'id=', '')
                    WHERE D.VisitCount IS NOT NULL AND D.VisitCount > 0
                    ORDER BY D.VisitCount DESC, A.BondCode ASC
                    ";

            var results = DbManager.Custom.ExecuteIList<Bond>(sql, new { PageId = pageId }, CommandType.Text);

            for (int i = 0; i < results.Count; i++)
            {
                results[i] = DataFormat(results[i]);
            }

            return results;
        }

        private Bond DataFormat(Bond bond)
        {
            bond.SubscriptionFee = bond.SubscriptionFee * 100;
            bond.RedemptionFee = bond.RedemptionFee * 100;

            bond.SubscriptionFee = this._bondRepository.Round2(bond.SubscriptionFee);
            bond.RedemptionFee = this._bondRepository.Round2(bond.RedemptionFee);

            bond.DetailLink = bond.DetailLink + "?id=" + bond.BondCode;
            bond = this._bondRepository.GetButtonHtml(bond, true);
            bond = this._bondRepository.SetTags(bond);

            if (DateTime.TryParse(bond.Date, out var oneMonthAgo))
            {
                oneMonthAgo = oneMonthAgo.AddMonths(-1);
                bond.UpsAndDownsMonth = this._bondRepository.GetUpsAndDowns(bond, oneMonthAgo.ToString("yyyyMMdd"), true);
            }

            if (DateTime.TryParse(bond.Date, out var threeMonthAgo))
            {
                threeMonthAgo = threeMonthAgo.AddMonths(-3);
                bond.UpsAndDownsSeason = this._bondRepository.GetUpsAndDowns(bond, threeMonthAgo.ToString("yyyyMMdd"), true);
            }

            bond.UpsAndDownsMonth = this._bondRepository.Round2(bond.UpsAndDownsMonth);
            bond.UpsAndDownsSeason = this._bondRepository.Round2(bond.UpsAndDownsSeason);

            if (string.IsNullOrEmpty(bond.CurrencyName))
            {
                bond.CurrencyName = "無幣別";
            }

            return bond;
        }

        public IList<Bond> GetBondDatas()
        {
            try
            {
                var queryitem = BondRelatedLinkSetting.GetBondDetailPageItem();
                var query = queryitem.ID.ToGuid();

                var results = GetBondData(query.ToString());

                return results;
            }
            catch (Exception ex)
            {
                this._log.Error("人氣國外債券", ex);
                return new List<Bond>();
            }
        }
    }
}

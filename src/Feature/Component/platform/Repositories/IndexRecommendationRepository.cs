using Feature.Wealth.Component.Models.Bond;
using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Tag;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.IndexRecommendation;
using Feature.Wealth.Component.Models.USStock;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Feature.Wealth.Component.Repositories
{
    public class IndexRecommendationRepository
    {
        private readonly VisitCountRepository _repository = new VisitCountRepository();
        private readonly USStockRepository _uSStockRepository = new USStockRepository();
        private readonly BondRepository _bondRepository = new BondRepository();

        public IList<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = """
                   SELECT *
                   FROM [vw_BasicFund] 
                   ORDER BY ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            var _tagsRepository = new FundTagRepository();
            var tags = _tagsRepository.GetFundTagData();

            foreach (var item in results)
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
            var queryitem = FundRelatedSettingModel.GetFundDetailPageItem();
            var query = queryitem?.ID.ToGuid();

            var funds = this._repository.GetVisitRecords(query, "id");

            if (funds == null || !funds.Any())
            {
                return new List<Funds>();
            }

            var fundIds = funds
                .OrderByDescending(x => x.VisitCount)
                .Take(5)
                .SelectMany(x => x.QueryStrings)
                .Where(x => x.Key.Equals("id"))
                .Select(x => x.Value)
                .ToList();


            var results = GetFundData()
                .Where(e => fundIds.Contains(e.ProductCode))
                .OrderBy(e => fundIds.IndexOf(e.ProductCode.ToString()))
                .ToList();

            return results;
        }


        public IList<ETFs> GetETFData()
        {
            List<ETFs> etfsItems = new List<ETFs>();

            string sql = """
                   SELECT *
                   FROM [vw_BasicETF]
                   ORDER BY SixMonthReturnMarketPriceOriginalCurrency
                   DESC,ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<ETFs>(sql, null, CommandType.Text);

            foreach (var item in results)
            {
                item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC);
                etfsItems.Add(item);
            }

            return etfsItems;
        }

        public IList<ETFs> GetETFDatas()
        {
            var queryitem = EtfRelatedLinkSetting.GetETFDetailPageItem();
            var query = queryitem.ID.ToGuid();
            var etfData = GetETFData();

            var etfs = this._repository.GetVisitRecords(query, "id");

            if (etfs == null || !etfs.Any())
            {
                return new List<ETFs>();
            }

            var etfIds = etfs
                .OrderByDescending(x => x.VisitCount)
                .Take(5)
                .SelectMany(x => x.QueryStrings)
                .Where(x => x.Key.Equals("id"))
                .Select(x => x.Value)
                .ToList();


            var results = etfData
                .Where(e => etfIds.Contains(e.ProductCode))
                .OrderBy(e => etfIds.IndexOf(e.ProductCode.ToString()))
                .ToList();

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

        public IList<USStock> GetUSStockData()
        {
            List<USStock> uSStockItems = new List<USStock>();

            string sql = """
                   SELECT 
                   A.[FirstBankCode]
                   ,A.[FundCode]
                   ,A.[EnglishName]
                   ,A.[ChineseName]
                   ,A.[FirstBankCode] + ' ' + A.[ChineseName] + ' ' + A.[EnglishName] [FullName]
                   ,A.[FirstBankCode] + ' ' + A.[ChineseName] + ' ' + A.[EnglishName] [value]
                   ,REPLACE(CONVERT(char(10), A.[DataDate],126),'-','/') [DataDate]
                   ,A.[ClosingPrice]
                   ,CONVERT(nvarchar, CONVERT(MONEY, A.[ClosingPrice]), 1) [ClosingPriceText]
                   ,CONVERT(decimal(16,2), A.[DailyReturn]) [DailyReturn]
                   ,CONVERT(decimal(16,2), A.[WeeklyReturn]) [WeeklyReturn]
                   ,CONVERT(decimal(16,2), A.[MonthlyReturn]) [MonthlyReturn]
                   ,CONVERT(decimal(16,2), A.[ThreeMonthReturn]) [ThreeMonthReturn]
                   ,CONVERT(decimal(16,2), A.[OneYearReturn]) [OneYearReturn]
                   ,CONVERT(decimal(16,2), A.[YeartoDateReturn]) [YeartoDateReturn]
                   ,CONVERT(decimal(16,2), A.[ChangePercentage]) [ChangePercentage]
                   ,CONVERT(decimal(16,2), A.[SixMonthReturn]) [SixMonthReturn]
                   ,B.[OnlineSubscriptionAvailability]
                   ,B.[AvailabilityStatus]
                   FROM [Sysjust_USStockList] A WITH (NOLOCK)
                   LEFT JOIN [WMS_DOC_RECM] B WITH (NOLOCK) ON A.[FirstBankCode] = B.[ProductCode]
                   ORDER BY MonthlyReturn DESC, A.[FirstBankCode] ASC
                   """;

            var results = DbManager.Custom.ExecuteIList<USStock>(sql, null, CommandType.Text);

            for (int i = 0; i < results.Count; i++)
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
            var queryitem = USStockRelatedLinkSetting.GetUSStockDetailPageItem();
            var query = queryitem.ID.ToGuid();
            var uSStocData = GetUSStockData();

            var uSStocs = this._repository.GetVisitRecords(query, "id");

            if (uSStocs == null || !uSStocs.Any())
            {
                return new List<USStock>();
            }

            var uSStockIds = uSStocs
                .OrderByDescending(x => x.VisitCount)
                .Take(5)
                .SelectMany(x => x.QueryStrings)
                .Where(x => x.Key.Equals("id"))
                .Select(x => x.Value)
                .ToList();


            var results = uSStocData
                .Where(e => uSStockIds.Contains(e.FirstBankCode))
                .OrderBy(e => uSStockIds.IndexOf(e.FirstBankCode.ToString()))
                .ToList();

            return results;
        }

        public IList<Bond> GetBondDatas()
        {
            var queryitem = BondRelatedLinkSetting.GetBondDetailPageItem();
            var query = queryitem.ID.ToGuid();
            var bondData = _bondRepository.GetBondList();

            var bonds = this._repository.GetVisitRecords(query, "id");

            if (bonds == null || !bonds.Any())
            {
                return new List<Bond>();
            }

            var bondIds = bonds
                .OrderByDescending(x => x.VisitCount)
                .Take(5)
                .SelectMany(x => x.QueryStrings)
                .Where(x => x.Key.Equals("id"))
                .Select(x => x.Value)
                .ToList();


            var results = bondData
                .Where(e => bondIds.Contains(e.BondCode))
                .OrderBy(e => bondIds.IndexOf(e.BondCode.ToString()))
                .ToList();

            return results;
        }
    }
}

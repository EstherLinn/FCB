using System.Data;
using System.Linq;
using System.Text;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using Foundation.Wealth.Extensions;
using Feature.Wealth.Component.Models.FundDetail;
using static Feature.Wealth.Component.Models.PopularityFund.PopularityFundModel;

namespace Feature.Wealth.Component.Repositories
{
    public class PopularityFundRepository
    {
        private readonly VisitCountRepository _repository = new VisitCountRepository();

        public List<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = """
                   SELECT *
                   FROM [vw_BasicFund] 
                   ORDER BY SixMonthReturnOriginalCurrency DESC,ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            var _tagsRepository = new FundTagRepository();
            var tags = _tagsRepository.GetFundTagData();

            foreach (var item in results)
            {
                if (item != null)
                {
                    ProcessFundFilterDatas(item);

                    item.Tags = tags.Where(t => t.FundTagType == FundTagEnum.DiscountTag && t.ProductCodes.Contains(item.ProductCode))
                                    .Select(t => t.TagName)
                                    .ToList();
                    fundItems.Add(item);
                }
            }

            return fundItems;
        }

           

        private void ProcessFundFilterDatas(Funds item)
        {
            item.FundName = item.FundName?.Normalize(NormalizationForm.FormKC);
            item.SixMonthReturnOriginalCurrency = NumberExtensions.RoundingPercentage(item.SixMonthReturnOriginalCurrency);
            item.NetAssetValue = NumberExtensions.RoundingValue(item.NetAssetValue);
            item.PercentageChangeInFundPrice = NumberExtensions.RoundingPercentage((item.PercentageChangeInFundPrice * 100));
        }


        /// <summary>
        /// 取得資料-列表渲染用
        /// </summary>
        public List<Funds> GetFundRenderData(IList<Funds> funds)
        {
            var result = new List<Funds>();

            foreach (var f in funds)
            {
                var vm = new Funds();
                vm.ProductCode = f.ProductCode;
                vm.FundName = f.FundName;
                vm.NetAssetValue = f.NetAssetValue;
                vm.NetAssetValueDate = f.NetAssetValueDate;
                vm.SixMonthReturnOriginalCurrency = f.SixMonthReturnOriginalCurrency;
                vm.CurrencyName = f.CurrencyName;
                vm.RiskRewardLevel = f.RiskRewardLevel;
                vm.OnlineSubscriptionAvailability = f.OnlineSubscriptionAvailability;
                vm.AvailabilityStatus = f.AvailabilityStatus;
                vm.PercentageChangeInFundPrice = f.PercentageChangeInFundPrice;
                vm.ViewCount = f.ViewCount ?? null;
                vm.Tags = f.Tags;
                result.Add(vm);
            }
            return result;
        }

        public IList<Funds> GetFundsDatas()
        {
            var queryitem = FundRelatedSettingModel.GetFundDetailPageItem();
            var query = queryitem.ID.ToGuid();

            var funds = _repository.GetVisitRecords(query, "id");

            if (funds == null || !funds.Any())
            {
                return new List<Funds>();
            }


            var fundData = funds
                .OrderByDescending(x => x.VisitCount)
                .Take(10)
                .Select(x => new 
                {
                    Id = x.QueryStrings.ContainsKey("id") ? x.QueryStrings["id"] : null,
                    ViewCount = x.VisitCount
                })
                .ToList();


            var allFunds = GetFundData();

            var result = allFunds
                .Select(fund => new
                {
                    Fund = fund,
                    fundData.FirstOrDefault(fd => fd.Id == fund.ProductCode)?.ViewCount
                })
                .Where(x => x.ViewCount.HasValue)
                .OrderBy(x => fundData.FindIndex(fd => fd.Id == x.Fund.ProductCode))
                .Select(x =>
                {
                    x.Fund.ViewCount = x.ViewCount;
                    return x.Fund;
                })
                .ToList();

            return result;
        }



    }
}

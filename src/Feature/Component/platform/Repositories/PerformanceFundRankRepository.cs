using System.Data;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using Foundation.Wealth.Extensions;
using static Feature.Wealth.Component.Models.PerformanceFundRank.PerformanceFundRankModel;
using System.Text;
using System.Linq;
using Feature.Wealth.Component.Models.FundDetail;

namespace Feature.Wealth.Component.Repositories
{
    public class PerformanceFundRankRepository
    {
        public List<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = """
            SELECT *
            FROM [vw_BasicFund]
            WHERE SixMonthReturnOriginalCurrency IS NOT NULL
            ORDER BY SixMonthReturnOriginalCurrency
            DESC,ProductCode
            """;

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            var _tagsRepository = new TagsRepository();
            var tags = _tagsRepository.GetFundTagData();

            foreach (var item in results)
            {
                if (item != null)
                {
                    ProcessFundFilterDatas(item);
                    item.Tags = [];
                    item.Tags.AddRange(from tagModel in tags.Where(t => t.FundTagType == FundTagEnum.DiscountTag)
                                       where tagModel.ProductCodes.Contains(item.ProductCode)
                                       select tagModel.TagName);
                }
            }

            fundItems.AddRange(results);

            return fundItems;
        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC);
            item.NetAssetValue = NumberExtensions.RoundingValue(item.NetAssetValue);
            item.SixMonthReturnOriginalCurrency = NumberExtensions.RoundingPercentage(item.SixMonthReturnOriginalCurrency);
            item.SixMonthReturnTWD = NumberExtensions.RoundingPercentage(item.SixMonthReturnTWD);
            item.PercentageChangeInFundPrice = NumberExtensions.RoundingPercentage((item.PercentageChangeInFundPrice) * 100);
        }

        /// <summary>
        /// 取得資料-列表渲染用
        /// </summary>
        public List<Funds> GetFundRenderData(List<Funds> funds)
        {
            var result = new List<Funds>();

            foreach (var f in funds)
            {
                var vm = new Funds();
                vm.ProductCode = f.ProductCode;
                vm.ProductName = f.ProductName;
                vm.NetAssetValue = f.NetAssetValue;
                vm.NetAssetValueDate = f.NetAssetValueDate;
                vm.SixMonthReturnOriginalCurrency = f.SixMonthReturnOriginalCurrency;
                vm.SixMonthReturnTWD = f.SixMonthReturnTWD;
                vm.CurrencyName = f.CurrencyName;
                vm.RiskRewardLevel = f.RiskRewardLevel;
                vm.OnlineSubscriptionAvailability = f.OnlineSubscriptionAvailability;
                vm.PercentageChangeInFundPrice = f.PercentageChangeInFundPrice;
                vm.TargetName = f.TargetName;
                vm.FundTypeName = f.FundTypeName;
                vm.InvestmentTargetName = f.InvestmentTargetName;
                vm.Tags = f.Tags;
                result.Add(vm);
            }
            return result;
        }
    }
}

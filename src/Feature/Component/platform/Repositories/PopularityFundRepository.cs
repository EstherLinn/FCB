using System.Data;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using static Feature.Wealth.Component.Models.PopularityFund.PopularityFundModel;
using Foundation.Wealth.Extensions;

namespace Feature.Wealth.Component.Repositories
{
    public class PopularityFundRepository
    {
        public List<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = """
                   SELECT *
                   FROM [vw_BasicFund] 
                   ORDER BY ViewCount
                   DESC,ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
            }

            fundItems.AddRange(results);

            return fundItems;
        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.SixMonthReturnOriginalCurrency = NumberExtensions.RoundingPercentage(item.SixMonthReturnOriginalCurrency);
            item.NetAssetValue = NumberExtensions.RoundingValue(item.NetAssetValue);
            item.PercentageChangeInFundPrice = NumberExtensions.RoundingPercentage((item.PercentageChangeInFundPrice * 100));
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
                vm.CurrencyName = f.CurrencyName;
                vm.RiskRewardLevel = f.RiskRewardLevel;
                vm.OnlineSubscriptionAvailability = f.OnlineSubscriptionAvailability;
                vm.PercentageChangeInFundPrice = f.PercentageChangeInFundPrice;
                vm.ViewCount = f.ViewCount ?? null;
                result.Add(vm);
            }
            return result;
        }

    }
}

using System.Data;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using static Feature.Wealth.Component.Models.HighRatedFund.HighRatedFundModel;

namespace Feature.Wealth.Component.Repositories
{
    public class HighRatedFundRepository
    {
        public List<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = """
                   SELECT *
                   FROM [vw_BasicFund] b
                   JOIN [Fund_HighRated] h
                   ON b.ProductCode = h.ProductCode
                   ORDER BY SixMonthReturnOriginalCurrency
                   DESC
                   """;

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
                fundItems.Add(item);
            }
            return fundItems;

        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.SixMonthReturnOriginalCurrency = decimal.Round(item.SixMonthReturnOriginalCurrency, 2);
            item.SixMonthReturnTWD = decimal.Round(item.SixMonthReturnTWD,2);
            item.NetAssetValue = decimal.Round(item.NetAssetValue, 4);
            item.PercentageChangeInFundPrice = decimal.Round((item.PercentageChangeInFundPrice * 100), 4);
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
                vm.TargetName = f.TargetName;
                result.Add(vm);
            }
            return result;
        }

    }
}

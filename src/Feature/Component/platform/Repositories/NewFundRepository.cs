using Dapper;
using System.Data;
using System.Linq;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using static Feature.Wealth.Component.Models.NewFund.NewFundModel;

namespace Feature.Wealth.Component.Repositories
{
    public class NewFundRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();

        public List<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = "SELECT * FROM [FCB_sitecore_Custom].[dbo].[vw_BasicFund] WHERE [ListingDate] >= DATEADD(year, -7, GETDATE()) order by SixMonthReturnOriginalCurrency desc";
            var results = _dbConnection.Query<Funds>(sql).ToList();

            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
            }

            fundItems.AddRange(results);

            return fundItems;
        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.NetAssetValue = decimal.Round(item.NetAssetValue,4);
            item.SixMonthReturnOriginalCurrency = decimal.Round(item.SixMonthReturnOriginalCurrency,2);
            item.SixMonthReturnTWD = decimal.Round(item.SixMonthReturnTWD,2);
            item.PercentageChangeInFundPrice = decimal.Round((item.PercentageChangeInFundPrice) * 100,4);
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
                vm.ValuationCurrency = f.ValuationCurrency;
                vm.RiskRewardLevel = f.RiskRewardLevel;
                vm.OnlineSubscriptionAvailability = f.OnlineSubscriptionAvailability;
                vm.PercentageChangeInFundPrice = f.PercentageChangeInFundPrice;
                vm.TargetName = f.TargetName;
                vm.FundTypeName = f.FundTypeName;
                result.Add(vm);
            }
            return result;
        }
    }
}

using Dapper;
using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static Feature.Wealth.Component.Models.eFirstChoice.eFirstChoiceModel;

namespace Feature.Wealth.Component.Repositories
{
    public class eFirstChoiceRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();

        public List<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = @"SELECT * FROM [FCB_sitecore_Custom].[dbo].[vw_BasicFund]";
            var results = _dbConnection.Query<Funds>(sql).ToList();

            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
                fundItems.Add(item);
            }
            return fundItems;

        }

        private void ProcessFundFilterDatas(Funds item)
        {
            DateTime navdate = Convert.ToDateTime(item.NetAssetValueDate);
            string formattedDate = navdate.ToString("yyyy-MM-dd");
            item.NetAssetValueDate = formattedDate;
            item.SixMonthReturnOriginalCurrency = decimal.Round(item.SixMonthReturnOriginalCurrency, 2);
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
                vm.ValuationCurrency = f.ValuationCurrency;
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

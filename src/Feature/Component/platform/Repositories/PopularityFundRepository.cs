using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static Feature.Wealth.Component.Models.PopularityFund.PopularityFundModel;

namespace Feature.Wealth.Component.Repositories
{
    public class PopularityFundRepository
    {
        string _connectionString = "Data Source=192.168.6.131;Initial Catalog=FCB_sitecore_Custom;User ID=sa;Password=vm_sand0z;";

        public List<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT * FROM [FCB_sitecore_Custom].[dbo].[vw_BasicFund]";
                    var results = connection.Query<Funds>(sql).ToList();

                    foreach (var item in results)
                    {
                        ProcessFundFilterDatas(item);
                    }

                    fundItems.AddRange(results);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                return fundItems;
            }
        }

        private void ProcessFundFilterDatas(Funds item)
        {
            DateTime navdate = Convert.ToDateTime(item.NetAssetValueDate);
            string formattedDate = navdate.ToString("yyyy-MM-dd");
            item.NetAssetValueDate = formattedDate;
            item.SixMonthReturnOriginalCurrency = ConvertRound(item.SixMonthReturnOriginalCurrency);
            item.SixMonthReturnTWD = ConvertRound(item.SixMonthReturnTWD);
            item.NetAssetValue=ConvertRound4(item.NetAssetValue);
            item.PercentageChangeInFundPrice = ConvertRound4(item.PercentageChangeInFundPrice*100);
        }

        public static decimal ConvertRound(decimal input)
        {
            if (input==null)
                return input;
            var n = decimal.Round(input, 2);
            return n;
        }

        public static decimal ConvertRound4(decimal input)
        {
            if (input == null)
                return input;
            var n = decimal.Round(input, 4);
            return n;
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
                result.Add(vm);
            }
            return result;
        }

    }
}

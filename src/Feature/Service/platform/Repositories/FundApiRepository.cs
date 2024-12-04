using Feature.Wealth.Service.Models.FundApi;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Feature.Wealth.Service.Repositories
{
    public class FundApiRepository
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private static readonly object _lock = new object();
        private readonly string FundInvestmentTargetsApiCache = $"Fcb_FundInvestmentTargetsApiCache";

        public List<Dictionary<string, string>> GetOrSetFundCompanyCache(string dff)
        {
            lock (_lock)
            {
                string fundCompanyApiCachekey = $"Fcb_FundCompanyApiCache_{dff}";
                var fundCompanys = (List<FundCompany>)_cache.Get(fundCompanyApiCachekey);
                if (fundCompanys == null)
                {
                    string fund_bsc = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.FUND_BSC);

                    var sql = @$" SELECT DISTINCT [FundCompanyID], [FundCompanyName]
                                  FROM {fund_bsc} WITH (NOLOCK)
                                  WHERE [DomesticForeignFundIndicator] = @dff";
                    var para = new { dff };

                    fundCompanys = DbManager.Custom.ExecuteIList<FundCompany>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();

                    if (fundCompanys != null)
                    {
                        _cache.Set(fundCompanyApiCachekey, fundCompanys, DateTimeOffset.Now.AddMinutes(30));
                    }
                }

                var result = fundCompanys
                    .Select(fc => new Dictionary<string, string> { { fc.FundCompanyID, fc.FundCompanyName } })
                    .ToList();

                return result;
            }
        }

        public List<Dictionary<string, string>> GetOrSetInvestmentTargets()
        {
            lock (_lock)
            {
                var investmentTargets = (List<FundInvestmentTarget>)_cache.Get(FundInvestmentTargetsApiCache);
                if (investmentTargets == null)
                {
                    string fund_bsc = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.FUND_BSC);
                    string basic_fund = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Basic_Fund);
                    var sql = $@"SELECT
                                [InvestmentTargetID] AS FundInvestmentTargetID,
                                [InvestmentTargetName] AS FundInvestmentTargetName
                                FROM 
                                    {fund_bsc} WITH (NOLOCK)
                                WHERE 
                                    [BankProductCode] IS NOT NULL
                                    AND [InvestmentTargetID] IS NOT NULL
                                    AND [InvestmentTargetName] IS NOT NULL
                                UNION
                                SELECT
                                    [UnKnown] AS FundInvestmentTargetID,
                                    [FundType] AS FundInvestmentTargetName
                                FROM 
                                    {basic_fund} WITH (NOLOCK)";

                    investmentTargets = DbManager.Custom.ExecuteIList<FundInvestmentTarget>(sql, null, commandType: System.Data.CommandType.Text)?.ToList();

                    if (investmentTargets != null)
                    {
                        _cache.Set(FundInvestmentTargetsApiCache, investmentTargets, DateTimeOffset.Now.AddMinutes(30));
                    }
                }

                var result = investmentTargets
                    .Select(it => new Dictionary<string, string> { { it.FundInvestmentTargetID, it.FundInvestmentTargetName } })
                    .ToList();

                return result;
            }
        }
    }
}
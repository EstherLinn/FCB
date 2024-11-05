using Feature.Wealth.Service.Models.FundApi;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Wealth.Service.Repositories
{
    public class FundApiRepository
    {
        public List<Dictionary<string, string>> GetFundCompany(string dff)
        {
            string fund_bsc = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.FUND_BSC);
            var sql = @$" SELECT DISTINCT [FundCompanyID], [FundCompanyName]
                 FROM {fund_bsc} WITH (NOLOCK)
                 WHERE [DomesticForeignFundIndicator] = @dff";
            var para = new { dff };
            var fundCompanys = DbManager.Custom.ExecuteIList<FundCompany>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();

            var result = fundCompanys
                .Select(fc => new Dictionary<string, string> { { fc.FundCompanyID, fc.FundCompanyName } })
                .ToList();

            return result;
        }

        public List<Dictionary<string, string>> GetInvestmentTargets()
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

            var investmentTargets = DbManager.Custom.ExecuteIList<FundInvestmentTarget>(sql, null, commandType: System.Data.CommandType.Text)?.ToList();
            var result = investmentTargets
                .Select(it => new Dictionary<string, string> { { it.FundInvestmentTargetID, it.FundInvestmentTargetName } })
                .ToList();

            return result;
        }
    }
}
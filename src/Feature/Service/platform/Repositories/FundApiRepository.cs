using Feature.Wealth.Service.Models.FundApi;
using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Service.Repositories
{
    public class FundApiRepository
    {
        public List<Dictionary<string, string>> GetFundCompany(string dff)
        {
            var sql = @" SELECT DISTINCT [FundCompanyID], [FundCompanyName]
                 FROM [dbo].[FUND_BSC]
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
            var sql = @"SELECT
                [InvestmentTargetID] AS FundInvestmentTargetID,
                [InvestmentTargetName] AS FundInvestmentTargetName
            FROM 
                [dbo].[FUND_BSC]
            WHERE 
                [BankProductCode] IS NOT NULL
                AND [InvestmentTargetID] IS NOT NULL
                AND [InvestmentTargetName] IS NOT NULL
            UNION
            SELECT
                [UnKnown] AS FundInvestmentTargetID,
                [FundType] AS FundInvestmentTargetName
            FROM 
                [dbo].[Sysjust_Basic_Fund]";
            var investmentTargets = DbManager.Custom.ExecuteIList<FundInvestmentTarget>(sql, null, commandType: System.Data.CommandType.Text)?.ToList();
            var result = investmentTargets
                .Select(it => new Dictionary<string, string> { { it.FundInvestmentTargetID, it.FundInvestmentTargetName } })
                .ToList();

            return result;
        }
    }
}

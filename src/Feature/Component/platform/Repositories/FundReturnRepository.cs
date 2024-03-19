using Dapper;
using Feature.Wealth.Component.Models.FundReturn;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Feature.Wealth.Component.Repositories
{
    public class FundReturnRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();

        public IList<FundReturnFilterModel> GetFundReturnFilterData()
        {
            string query = @"
            SELECT
                DISTINCT DomesticForeignFundIndicator
            FROM
                vw_FundReturn
            WHERE
                DomesticForeignFundIndicator='O' or DomesticForeignFundIndicator='D'";

            var indicators = _dbConnection.Query<string>(query)?.ToList() ?? new List<string>();

            var result = new List<FundReturnFilterModel>();

            foreach (var indicator in indicators)
            {
                string companyQuery = @"
                SELECT
                    DISTINCT FundCompanyID,
                    FundCompanyName
                FROM
                    vw_FundReturn
                WHERE
                    DomesticForeignFundIndicator = @Indicator";

                var companies = _dbConnection.Query<FundCompany>(companyQuery, new { Indicator = indicator })
                    ?.Select(c => new FundCompany { FundCompanyID = c.FundCompanyID?.Trim(), FundCompanyName = c.FundCompanyName?.Trim() }).ToList() ?? new List<FundCompany>();

                foreach (var company in companies)
                {
                    string fundQuery = @"
                    SELECT
                        ProductCode,
                        ProductName
                    FROM
                        vw_FundReturn
                    WHERE
                        DomesticForeignFundIndicator = @Indicator AND
                        FundCompanyID = @CompanyID";

                    company.Funds = _dbConnection.Query<Fund>(fundQuery, new { Indicator = indicator, CompanyID = company.FundCompanyID })
                        ?.Select(f => new Fund { ProductCode = f.ProductCode?.Trim(), ProductName = f.ProductName?.Trim() }).ToList() ?? new List<Fund>();
                }

                result.Add(new FundReturnFilterModel
                {
                    DomesticForeignFundIndicator = indicator,
                    FundCompanies = companies
                });
            }

            return result;
        }

        public IList<Fund> GetFundReturnSearchData()
        {
            string fundQuery = @"
                                SELECT
                                    ProductCode,
                                    ProductName
                                FROM
                                    vw_FundReturn";

            var result = _dbConnection.Query<Fund>(fundQuery)
                ?.Select(f => new Fund { ProductCode = f.ProductCode?.Trim(), ProductName = f.ProductName?.Trim() }).ToList() ?? new List<Fund>();

            return result;
        }

        public FundReturnDetailModel GetFundReturnDetail(string productCode)
        {
            string query = @"
                            SELECT
                                FundName,
                                UpdateTime,
                                SubscriptionFee,
                                FundConversionFee,
                                FundShareConvFeeToBank,
                                FeePostCollectionType,
                                BankDeferPurchase,
                                FundMgmtFee,
                                FundShareMgmtFeeToBank,
                                FundCompanyName,
                                FundSponsorSeminarsEduToBank,
                                FundOtherMarketingToBank
                            FROM
                                vw_FundReturn
                            WHERE
                                ProductCode = @ProductCode";

            var result = _dbConnection.QueryFirstOrDefault<FundReturnDetailModel>(query, new { ProductCode = productCode }) ?? new FundReturnDetailModel();

            result.FundName = result.FundName?.Trim();

            return result;
        }
    }
}
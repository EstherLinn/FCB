using Feature.Wealth.Component.Models.TabCards;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Feature.Wealth.Component.Repositories
{
    internal class TabCardsRepository
    {
        public IList<FundCardBasicDTO> GetFundCardsInfos(List<string> productCodes)
        {
            if (productCodes == null || productCodes.Count == 0)
            {
                return new List<FundCardBasicDTO>();
            }

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < productCodes.Count; ++i)
            {
                string productCode = productCodes[i];
                stringBuilder.Append($@"
                                        SELECT * FROM (
                                            SELECT TOP (1)
                                                ProductCode,
                                                ProductName,
                                                CAST(OneMonthReturnOriginalCurrency AS decimal(10,2)) AS Rate,
                                                CASE
                                                    WHEN OneMonthReturnOriginalCurrency < 0
                                                    THEN 1
                                                    ELSE 0
                                                END AS isFall   
                                            FROM vw_BasicFund
                                            WHERE ProductCode = '{productCode}'
                                            ORDER BY NetAssetValueDate DESC
                                        ) AS [{productCode}]");

                if (i < productCodes.Count - 1)
                {
                    stringBuilder.Append(" UNION ALL");
                }
            }

            var source = DbManager.Custom.ExecuteIList<FundCardBasicDTO>(stringBuilder.ToString(), new { }, CommandType.Text)?.ToList();
            if (source == null)
            {
                return new List<FundCardBasicDTO>();
            }

            return source;
        }

        public IList<NavDTO> GetFundCardsNavs(List<string> productCodes)
        {
            if (productCodes == null || productCodes.Count == 0)
            {
                return new List<NavDTO>();
            }

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < productCodes.Count; ++i)
            {
                string productCode = productCodes[i];
                stringBuilder.Append($@"
                                    SELECT * FROM (
                                            SELECT TOP (30)
                                                FirstBankCode AS ProductCode,
                                                NetAssetValueDate,
                                                NetAssetValue
                                            FROM Sysjust_Nav_Fund WITH (NOLOCK)
                                    WHERE FirstBankCode = '{productCode}'
                                    ORDER BY [NetAssetValueDate] DESC) AS [{productCode}]");

                if (i < productCodes.Count - 1)
                {
                    stringBuilder.Append(" UNION ALL");
                }
            }

            var source = DbManager.Custom.ExecuteIList<NavDTO>(stringBuilder.ToString(), new { }, CommandType.Text)?.ToList();
            if (source == null)
            {
                return new List<NavDTO>();
            }

            return source;
        }
    }
}

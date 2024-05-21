using System.Text;
using System.Data;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using Foundation.Wealth.Extensions;
using static Feature.Wealth.Component.Models.NewFund.NewFundModel;
using Sitecore.IO;
using System.Globalization;
using System.Security.Cryptography;
using System;

namespace Feature.Wealth.Component.Repositories
{
    public class NewFundRepository
    {
        public List<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            var sql = """
             SELECT *
             FROM [vw_BasicFund]
             ORDER BY SixMonthReturnOriginalCurrency
             DESC,ProductCode
             """;

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            foreach (var item in results)
            {
                if (item != null)
                {
                    ProcessFundFilterDatas(item);
                }
            }

            fundItems.AddRange(results);

            return fundItems;
        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC);
            item.NetAssetValue = NumberExtensions.RoundingValue(item.NetAssetValue);
            item.SixMonthReturnOriginalCurrency = NumberExtensions.RoundingPercentage(item.SixMonthReturnOriginalCurrency);
            item.PercentageChangeInFundPrice = NumberExtensions.RoundingPercentage((item.PercentageChangeInFundPrice) * 100);

            var cultureInfo = new CultureInfo("zh-TW");
            string dateFormat = "yyyy/MM/dd";
            if (DateTime.TryParseExact(item.ListingDate, "yyyyMMdd", cultureInfo, DateTimeStyles.None, out DateTime listingDate))
            {
                item.ListingDate = listingDate.ToString(dateFormat);
                item.ListingDateFormat = listingDate;
            }
        }

    }
}

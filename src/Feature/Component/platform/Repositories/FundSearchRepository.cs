using System.Data;
using System.Text;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using Feature.Wealth.Component.Models.FundSearch;


namespace Feature.Wealth.Component.Repositories
{
    public class FundSearchRepository
    {
        public List<FundSearchModel> GetFundSearchData()
        {
            List<FundSearchModel> fundItems = new List<FundSearchModel>();

            string sql = "SELECT * FROM [vw_BasicFund]";
            var results = DbManager.Custom.ExecuteIList<FundSearchModel>(sql, null, CommandType.Text);

            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
                fundItems.Add(item);
            }

            return fundItems;
        }


        private void ProcessFundFilterDatas(FundSearchModel item)
        {
            item.OneMonthReturnOriginalCurrency = decimal.Round(item.OneMonthReturnOriginalCurrency, 2);
            item.OneMonthReturnTWD = decimal.Round(item.OneMonthReturnTWD, 2);

            item.IsUpOneMonthReturnOriginalCurrency = item.OneMonthReturnOriginalCurrency >= 0 ? true : false;
            item.IsUpOneMonthReturnTWD = item.OneMonthReturnTWD >= 0 ? true : false;

            item.ThreeMonthReturnOriginalCurrency = decimal.Round(item.ThreeMonthReturnOriginalCurrency, 2);
            item.ThreeMonthReturnTWD = decimal.Round(item.ThreeMonthReturnTWD, 2);
            item.IsUpThreeMonthReturnOriginalCurrency = item.ThreeMonthReturnOriginalCurrency >= 0 ? true : false;
            item.IsUpThreeMonthReturnTWD = item.ThreeMonthReturnTWD >= 0 ? true : false;

            item.SixMonthReturnOriginalCurrency = decimal.Round(item.SixMonthReturnOriginalCurrency, 2);
            item.SixMonthReturnTWD = decimal.Round(item.SixMonthReturnTWD, 2);
            item.IsUpSixMonthReturnOriginalCurrency = item.SixMonthReturnOriginalCurrency >= 0 ? true : false;
            item.IsUpSixMonthReturnTWD = item.SixMonthReturnTWD >= 0 ? true : false;

            item.OneYearReturnOriginalCurrency = decimal.Round(item.OneYearReturnOriginalCurrency, 2);
            item.OneYearReturnTWD = decimal.Round(item.OneYearReturnTWD, 2);
            item.IsUpOneYearReturnOriginalCurrency = item.OneYearReturnOriginalCurrency >= 0 ? true : false;
            item.IsUpOneYearReturnTWD = item.OneYearReturnTWD >= 0 ? true : false;

            item.NetAssetValue = decimal.Round(item.NetAssetValue, 4);
            item.PercentageChangeInFundPrice = decimal.Round((item.PercentageChangeInFundPrice * 100), 4);
            item.IsUpPercentageChangeInFundPrice = item.PercentageChangeInFundPrice >= 0 ? true : false;

            item.FundSizeMillionOriginalCurrency = decimal.Round(item.FundSizeMillionOriginalCurrency, 4);
            item.FundSizeMillionTWD = decimal.Round(item.FundSizeMillionTWD, 4);

            item.FundSizeMillionOriginalCurrency = decimal.Round((item.FundSizeMillionOriginalCurrency / 1000000),4);
            item.FundSizeMillionTWD = decimal.Round((item.FundSizeMillionTWD / 1000000),4);

            item.Sharpe = decimal.Round(item.Sharpe, 4);
            item.Beta = decimal.Round(item.Beta, 4);
            item.OneYearAlpha = decimal.Round(item.OneYearAlpha, 4);
            item.AnnualizedStandardDeviation = decimal.Round(item.AnnualizedStandardDeviation, 4);

            if (item.DividendFrequencyName == "無" || item.DividendFrequencyName == null)
            {
                item.DividendFrequencyName = "不配息";
            }
            if (item.FundCurrency == "TWD")
            {
                item.FundCurrencyName = "新臺幣";
            }
        }

        /// <summary>
        /// 取得資料-列表渲染用
        /// </summary>
        public List<Funds> GetFundRenderData(List<FundSearchModel> funds)
        {
            var result = new List<Funds>();

            foreach (var f in funds)
            {
                var vm = new Funds();
                //共用欄位
                vm.TargetName = f.TargetName;
                vm.ProductCode = f.ProductCode;
                vm.ProductName = f.ProductName;
                vm.NetAssetValue = f.NetAssetValue;
                vm.NetAssetValueDate = f.NetAssetValueDateFormat;
                vm.OnlineSubscriptionAvailability = f.OnlineSubscriptionAvailability;
                //績效表現
                vm.Currency = new KeyValuePair<string, string>(f.CurrencyCode, f.CurrencyName);
                vm.SixMonthReturnOriginalCurrency = CreateReturnDictionary(f.IsUpSixMonthReturnOriginalCurrency, f.SixMonthReturnOriginalCurrency);
                vm.OneMonthReturnOriginalCurrency = CreateReturnDictionary(f.IsUpOneMonthReturnOriginalCurrency, f.OneMonthReturnOriginalCurrency);
                vm.ThreeMonthReturnOriginalCurrency = CreateReturnDictionary(f.IsUpThreeMonthReturnOriginalCurrency, f.ThreeMonthReturnOriginalCurrency);
                vm.OneYearReturnOriginalCurrency = CreateReturnDictionary(f.IsUpOneYearReturnOriginalCurrency, f.OneYearReturnOriginalCurrency);
                vm.SixMonthReturnTWD = CreateReturnDictionary(f.IsUpSixMonthReturnTWD, f.SixMonthReturnTWD);
                vm.OneMonthReturnTWD = CreateReturnDictionary(f.IsUpOneMonthReturnTWD, f.OneMonthReturnTWD);
                vm.ThreeMonthReturnTWD = CreateReturnDictionary(f.IsUpThreeMonthReturnTWD, f.ThreeMonthReturnTWD);
                vm.OneYearReturnTWD = CreateReturnDictionary(f.IsUpOneYearReturnTWD, f.OneYearReturnTWD);
                //基本資料
                vm.FundCurrency = new KeyValuePair<string, string>(f.FundCurrencyCode, f.FundCurrencyName);
                vm.PercentageChangeInFundPrice = CreateReturnDictionary(f.IsUpPercentageChangeInFundPrice, f.PercentageChangeInFundPrice);
                vm.FundSizeMillionOriginalCurrency = f.FundSizeMillionOriginalCurrency;
                vm.FundSizeMillionTWD = f.FundSizeMillionTWD;
                vm.FundTypeName = f.FundTypeName;
                vm.DividendFrequencyName = f.DividendFrequencyName;
                //風險指標
                vm.RiskRewardLevel = f.RiskRewardLevel;
                vm.Sharpe = f.Sharpe;
                vm.Beta = f.Beta;
                vm.OneYearAlpha = f.OneYearAlpha;
                vm.AnnualizedStandardDeviation = f.AnnualizedStandardDeviation;
                result.Add(vm);
            }
            return result;
        }

        private KeyValuePair<bool, decimal> CreateReturnDictionary(bool isUp, decimal value)
        {
            return new KeyValuePair<bool, decimal>(isUp, value);
        }


        public IList<FundItem> GetAutoCompleteData()
        {
            List<FundItem> fundItems = new List<FundItem>();

            string sql = "SELECT * FROM [vw_BasicFund]";
            var results = DbManager.Custom.ExecuteIList<FundSearchModel>(sql, null, CommandType.Text);


            foreach (var item in results)
            {
                FundItem fundItem = new FundItem
                {
                    Value = FullWidthToHalfWidth(item.ProductName),
                    Data = new FundData
                    {
                        Type = item.FundTypeName,
                        IsLogin = true,
                        IsLike = true,
                        DetailUrl = "",
                        Purchase = true
                    }
                };

                fundItems.Add(fundItem);
            }


            return fundItems;
        }

        public static string FullWidthToHalfWidth(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if (c >= 0xFF01 && c <= 0xFF5E)
                {
                    sb.Append((char)(c - 0xFEE0));
                }
                else if (c == 0x3000)
                {
                    sb.Append((char)0x20);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}

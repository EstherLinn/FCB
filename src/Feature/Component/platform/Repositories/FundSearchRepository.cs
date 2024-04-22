﻿using System.Data;
using System.Text;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using Feature.Wealth.Component.Models.FundSearch;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.Component.Models.FundDetail;
using System.Linq;
using JSNLog.Infrastructure;


namespace Feature.Wealth.Component.Repositories
{
    public class FundSearchRepository
    {
        public IList<FundSearchModel> GetFundSearchData()
        {
            string sql = """
                SELECT * FROM [vw_BasicFund]
                ORDER BY SixMonthReturnOriginalCurrency DESC
                """;
            var results = DbManager.Custom.ExecuteIList<FundSearchModel>(sql, null, CommandType.Text);
            return results;
        }

        /// <summary>
        /// 取得資料-列表渲染用
        /// </summary>
        public List<Funds> GetFundRenderData(IList<FundSearchModel> funds)
        {
            var result = new List<Funds>();

            foreach (var f in funds)
            {
                var vm = new Funds();
                //共用欄位
                if (f.TargetName == "Y")
                {
                    vm.Tags = ["百元基金"];
                }
                else
                {
                    vm.Tags = [];
                }
                
                vm.DomesticForeignFundIndicator = f.DomesticForeignFundIndicator;
                vm.ProductCode = f.ProductCode;
                vm.ProductName = FullWidthToHalfWidth(f.ProductName);
                vm.NetAssetValue = Round4(f.NetAssetValue);
                vm.NetAssetValueDate = f.NetAssetValueDateFormat;
                vm.IsOnlineSubscriptionAvailability = Extender.ToBoolean(f.OnlineSubscriptionAvailability);
                //績效表現
                if (f.CurrencyCode == null || f.FundCurrency == "TWD")
                {
                    f.CurrencyCode = "00";
                    f.CurrencyName = "新臺幣";
                }
                vm.Currency = new KeyValuePair<string, string>(f.CurrencyCode, f.CurrencyName);
                vm.SixMonthReturnOriginalCurrency = CreateReturnDictionary(f.SixMonthReturnOriginalCurrency);
                vm.OneMonthReturnOriginalCurrency = CreateReturnDictionary( f.OneMonthReturnOriginalCurrency);
                vm.ThreeMonthReturnOriginalCurrency = CreateReturnDictionary(f.ThreeMonthReturnOriginalCurrency);
                vm.OneYearReturnOriginalCurrency = CreateReturnDictionary( f.OneYearReturnOriginalCurrency);
                vm.SixMonthReturnTWD = CreateReturnDictionary(f.SixMonthReturnTWD);
                vm.OneMonthReturnTWD = CreateReturnDictionary(f.OneMonthReturnTWD);
                vm.ThreeMonthReturnTWD = CreateReturnDictionary(f.ThreeMonthReturnTWD);
                vm.OneYearReturnTWD = CreateReturnDictionary(f.OneYearReturnTWD);
                //基本資料
                vm.FundCurrency = new KeyValuePair<string, string>(f.FundCurrencyCode, f.FundCurrencyName);
                vm.PercentageChangeInFundPrice = Percentage(f.PercentageChangeInFundPrice);
                vm.FundSizeMillionOriginalCurrency = RoundFundSize(f.FundSizeMillionOriginalCurrency);
                vm.FundSizeMillionTWD = RoundFundSize(f.FundSizeMillionTWD);
                vm.FundType = f.FundType ?? "null";
               
                if(f.DividendFrequencyName =="無" || f.DividendFrequencyName == null)
                {
                    vm.DividendFrequencyName = "不配息";
                }
                else
                {
                    vm.DividendFrequencyName = f.DividendFrequencyName;
                }

                //風險指標
                vm.RiskRewardLevel = f.RiskRewardLevel;
                vm.Sharpe = Round4(f.Sharpe);
                vm.Beta = Round4(f.Beta);
                vm.OneYearAlpha = Round4(f.OneYearAlpha);
                vm.AnnualizedStandardDeviation = Round4(f.AnnualizedStandardDeviation);
                vm.DetailUrl = FundRelatedSettingModel.GetFundDetailsUrl();

                //篩選用
                vm.FundCompanyName = f.FundCompanyName;
                if (!string.IsNullOrEmpty(f.InvestmentRegionName))
                {
                    vm.InvestmentRegionName = f.InvestmentRegionName.Split(',')
                        .Select(region => region.Trim()) 
                        .ToList();
                }
                else
                {
                    vm.InvestmentRegionName = [null];
                }

                vm.value = FullWidthToHalfWidth(f.ProductName);

                vm.Data = new FundData
                {
                    Type = f.FundType,
                    IsLogin = false,
                    IsLike = false,
                    DetailUrl = FundRelatedSettingModel.GetFundDetailsUrl() + "?id=" + f.ProductCode,
                    Purchase = f.OnlineSubscriptionAvailability == "Y" ? true : false
                };

                vm.InvestmentTargetName = f.InvestmentTargetName ?? "null";
                vm.FundRating = f.FundRating;

                result.Add(vm);
            }
            return result;
        }

        private KeyValuePair<bool, decimal?> CreateReturnDictionary(decimal? value)
        {
            bool isUp = value >= 0;
            if (value == null)
            {
                isUp = true;
            }
            if (value != null)
            {
                value = decimal.Round((decimal)value, 2);
            }
            return new KeyValuePair<bool, decimal?>(isUp, value);
        }

        private KeyValuePair<bool, decimal?> Percentage(decimal? value)
        {
            bool isUp = value >= 0;
            if (value != null)
            {
                value = decimal.Round((decimal)value, 4);
            }
            return new KeyValuePair<bool, decimal?>(isUp, value);
        }

        private static decimal? Round4(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round((decimal)value, 4);
            }
            return value;
        }

        private static decimal? RoundFundSize(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round(((decimal)value / 1000000), 4);
            }
            return value;
        }


        public IList<FundItem> GetAutoCompleteData()
        {
            List<FundItem> fundItems = new List<FundItem>();

            string sql = """
                         SELECT * FROM [vw_BasicFund]
                         ORDER BY SixMonthReturnOriginalCurrency DESC
                        """;

            var results = DbManager.Custom.ExecuteIList<FundSearchModel>(sql, null, CommandType.Text);


            foreach (var item in results)
            {
                FundItem fundItem = new FundItem
                {
                    value = FullWidthToHalfWidth(item.ProductName),
                    data = new FundData
                    {
                        Type = item.FundType,
                        IsLogin = false,
                        IsLike = false,
                        DetailUrl = FundRelatedSettingModel.GetFundDetailsUrl()+"?id="+item.ProductCode,
                        Purchase = item.OnlineSubscriptionAvailability == "Y" ? true : false
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

using Dapper;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.Invest;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using static Feature.Wealth.Component.Models.TftfuStg.TftfuStgModel;

namespace Feature.Wealth.Component.Repositories
{
    public class TftfuSmartTypeRepository
    {
        public IList<Funds> GetFundData()
        {
            var sql = @"SELECT
                            ProductCode,
                            FundName,
                            NetAssetValueDate,
                            NetAssetValue,
                            CurrencyName,
                            CurrencyCode,
                            SixMonthReturnOriginalCurrency,
                            PercentageChangeInFundPrice,
                            RiskRewardLevel,
                            FundTypeName,
                            TargetName,
                            AvailabilityStatus,
                            OnlineSubscriptionAvailability,
                            h.TFTFU_SMART_TYPE,
                            h.TFTFU_CTT_SHR_TYPE
                        FROM
                            [vw_BasicFund] b
                        JOIN
                            [TFTFU_STG] h
                        ON
                            b.ProductCode = SUBSTRING(h.TFTFU_FUND_ID_CODE,1,4)
                        WHERE
                            h.TFTFU_SMART_TYPE='1' OR h.TFTFU_SMART_TYPE='3'
                        ORDER BY
                            SixMonthReturnOriginalCurrency DESC,b.ProductCode
                        ";

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            return results;
        }

        public IList<Funds> GetFundData2()
        {
            var sql = @"SELECT
                            ProductCode,
                            FundName,
                            NetAssetValueDate,
                            NetAssetValue,
                            CurrencyName,
                            CurrencyCode,
                            SixMonthReturnOriginalCurrency,
                            PercentageChangeInFundPrice,
                            RiskRewardLevel,
                            FundTypeName,
                            TargetName,
                            AvailabilityStatus,
                            OnlineSubscriptionAvailability,
                            h.TFTFU_SMART_TYPE,
                            h.TFTFU_CTT_SHR_TYPE
                        FROM
                            [vw_BasicFund] b
                        JOIN
                            [TFTFU_STG] h
                        ON
                            b.ProductCode = SUBSTRING(h.TFTFU_FUND_ID_CODE,1,4)
                        WHERE
                            h.TFTFU_SMART_TYPE='2' OR h.TFTFU_SMART_TYPE='3'
                        ORDER BY
                            SixMonthReturnOriginalCurrency DESC,b.ProductCode
                        ";

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            return results;
        }

        public IList<Funds> GetCTTFundData1()
        {
            var sql = @"SELECT
                            ProductCode,
                            FundName,
                            NetAssetValueDate,
                            NetAssetValue,
                            CurrencyName,
                            CurrencyCode,
                            SixMonthReturnOriginalCurrency,
                            PercentageChangeInFundPrice,
                            RiskRewardLevel,
                            FundTypeName,
                            TargetName,
                            AvailabilityStatus,
                            OnlineSubscriptionAvailability,
                            h.TFTFU_SMART_TYPE,
                            h.TFTFU_CTT_SHR_TYPE
                        FROM
                            [vw_BasicFund] b
                        JOIN
                            [TFTFU_STG] h
                        ON
                            b.ProductCode = SUBSTRING(h.TFTFU_FUND_ID_CODE,1,4)
                        WHERE
                            h.TFTFU_CTT_SHR_TYPE='1' OR h.TFTFU_CTT_SHR_TYPE='3'
                        ORDER BY
                            SixMonthReturnOriginalCurrency DESC,b.ProductCode
                        ";

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            return results;
        }

        public IList<Funds> GetCTTFundData2()
        {
            var sql = @"SELECT
                            ProductCode,
                            FundName,
                            NetAssetValueDate,
                            NetAssetValue,
                            CurrencyName,
                            CurrencyCode,
                            SixMonthReturnOriginalCurrency,
                            PercentageChangeInFundPrice,
                            RiskRewardLevel,
                            FundTypeName,
                            TargetName,
                            AvailabilityStatus,
                            OnlineSubscriptionAvailability,
                            h.TFTFU_SMART_TYPE,
                            h.TFTFU_CTT_SHR_TYPE
                        FROM
                            [vw_BasicFund] b
                        JOIN
                            [TFTFU_STG] h
                        ON
                            b.ProductCode = SUBSTRING(h.TFTFU_FUND_ID_CODE,1,4)
                        WHERE
                            h.TFTFU_CTT_SHR_TYPE='2' OR h.TFTFU_CTT_SHR_TYPE='3'
                        ORDER BY
                            SixMonthReturnOriginalCurrency DESC,b.ProductCode
                        ";

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            return results;
        }

        public List<RenderFunds> GetFundRenderData(IList<Funds> funds)
        {
            var _tagsRepository = new FundTagRepository();
            var tags = _tagsRepository.GetFundTagData();

            var result = new List<RenderFunds>();

            foreach (var f in funds)
            {
                var vm = new RenderFunds();
                vm.Tags = [];
                vm.Tags.AddRange(from tagModel in tags.Where(t => t.FundTagType == FundTagEnum.DiscountTag)
                                 where tagModel.ProductCodes.Contains(f.ProductCode)
                                 select tagModel.TagName);
                vm.ProductCode = f.ProductCode;
                vm.NetAssetValue = f.NetAssetValue;
                vm.NetAssetValueDate = f.NetAssetValueDateFormat;
                vm.FundName = f.FundName?.Normalize(NormalizationForm.FormKC);
                vm.Currency = new KeyValuePair<string, string>(f.CurrencyCode, f.CurrencyName);
                vm.SixMonthReturnOriginalCurrency = CreateReturnDictionary(f.SixMonthReturnOriginalCurrency);
                vm.PercentageChangeInFundPrice = Percentage(f.PercentageChangeInFundPrice);
                vm.FundTypeName = f.FundTypeName;
                vm.RiskRewardLevel = f.RiskRewardLevel;
                vm.IsOnlineSubscriptionAvailability = f.AvailabilityStatus == "Y" &&
                                  (f.OnlineSubscriptionAvailability == "Y" ||
                                   string.IsNullOrEmpty(f.OnlineSubscriptionAvailability));
                vm.FocusTag = PublicHelpers.FocusButtonString(null, f.ProductCode, f.FundName, InvestTypeEnum.Fund, true);
                vm.CompareTag = PublicHelpers.CompareButtonString(null, f.ProductCode, f.FundName, InvestTypeEnum.Fund, true);
                vm.SubscriptionTag = PublicHelpers.SubscriptionButtonString(null, f.ProductCode, InvestTypeEnum.Fund, true);
                vm.TFTFU_CTT_SHR_TYPE = f.TFTFU_CTT_SHR_TYPE;
                vm.TFTFU_SMART_TYPE = f.TFTFU_SMART_TYPE;
                vm.DetailUrl = FundRelatedSettingModel.GetFundDetailsUrl();
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
                value = decimal.Round((decimal)value, 2, MidpointRounding.AwayFromZero);
            }
            return new KeyValuePair<bool, decimal?>(isUp, value);
        }

        private KeyValuePair<bool, decimal?> Percentage(decimal? value)
        {
            bool isUp = true;
            if (value != null)
            {
                isUp = value >= 0;
                value = decimal.Round((decimal)value * 100, 2, MidpointRounding.AwayFromZero);
            }
            return new KeyValuePair<bool, decimal?>(isUp, value);
        }
    }
}

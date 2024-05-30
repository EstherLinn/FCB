using System.Data;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using Foundation.Wealth.Extensions;
using static Feature.Wealth.Component.Models.HotFund.HotFundModel;
using System.Text;
using System.Linq;
using Feature.Wealth.Component.Models.FundDetail;

namespace Feature.Wealth.Component.Repositories
{
    public class HotFundRepository
    {
        public List<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = "SELECT * FROM [vw_BasicFund]";
            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            var _tagsRepository = new TagsRepository();
            var tags = _tagsRepository.GetFundTagData();
            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
                item.Tags = [];
                item.Tags.AddRange(from tagModel in tags.Where(t => t.FundTagType == FundTagEnum.DiscountTag)
                                   where tagModel.ProductCodes.Contains(item.ProductCode)
                                   select tagModel.TagName);
                fundItems.Add(item);
            }

            return fundItems;

        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC);
            item.SixMonthReturnOriginalCurrency = NumberExtensions.RoundingPercentage(item.SixMonthReturnOriginalCurrency);
            item.NetAssetValue = NumberExtensions.RoundingValue(item.NetAssetValue);
            item.PercentageChangeInFundPrice = NumberExtensions.RoundingPercentage((item.PercentageChangeInFundPrice * 100));
            item.DomesticInvestmentRatio = NumberExtensions.RoundingValue(item.DomesticInvestmentRatio);
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
                vm.CurrencyName = f.CurrencyName;
                vm.RiskRewardLevel = f.RiskRewardLevel;
                vm.OnlineSubscriptionAvailability = f.OnlineSubscriptionAvailability;
                vm.PercentageChangeInFundPrice = f.PercentageChangeInFundPrice;
                vm.DomesticInvestmentRatio = f.DomesticInvestmentRatio;
                vm.TargetName = f.TargetName;
                result.Add(vm);
            }
            return result;
        }
    }
}

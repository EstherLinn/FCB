using System.Data;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using static Feature.Wealth.Component.Models.PopularityFund.PopularityFundModel;
using Foundation.Wealth.Extensions;
using System.Text;
using System.Linq;
using Feature.Wealth.Component.Models.FundDetail;
using Sitecore.Services.Core;
using Feature.Wealth.Component.Models.FundReturn;

namespace Feature.Wealth.Component.Repositories
{
    public class PopularityFundRepository
    {
        private readonly VisitCountRepository _repository = new VisitCountRepository();

        public List<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = """
                   SELECT *
                   FROM [vw_BasicFund] 
                   ORDER BY SixMonthReturnOriginalCurrency DESC,ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            var _tagsRepository = new FundTagRepository();
            var tags = _tagsRepository.GetFundTagData();
            var viewcountFunds = GetFundsDatas();



            foreach (var item in results)
            {
                if (item != null)
                {
                    ProcessFundFilterDatas(item);
                    item.Tags = [];
                    item.Tags.AddRange(from tagModel in tags.Where(t => t.FundTagType == FundTagEnum.DiscountTag)
                                       where tagModel.ProductCodes.Contains(item.ProductCode)
                                       select tagModel.TagName);
                    var viewCountFund = viewcountFunds.FirstOrDefault(f => f.ProductCode == item.ProductCode);
                    item.ViewCount = viewCountFund?.ViewCount.ToString();
                    item.ViewCountOrderBy = viewCountFund?.ViewCountOrderBy;
                }
            }

            fundItems.AddRange(results);

            return fundItems;
        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.FundName = item.FundName.Normalize(NormalizationForm.FormKC);
            item.SixMonthReturnOriginalCurrency = NumberExtensions.RoundingPercentage(item.SixMonthReturnOriginalCurrency);
            item.NetAssetValue = NumberExtensions.RoundingValue(item.NetAssetValue);
            item.PercentageChangeInFundPrice = NumberExtensions.RoundingPercentage((item.PercentageChangeInFundPrice * 100));
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
                vm.FundName = f.FundName;
                vm.NetAssetValue = f.NetAssetValue;
                vm.NetAssetValueDate = f.NetAssetValueDate;
                vm.SixMonthReturnOriginalCurrency = f.SixMonthReturnOriginalCurrency;
                vm.CurrencyName = f.CurrencyName;
                vm.RiskRewardLevel = f.RiskRewardLevel;
                vm.OnlineSubscriptionAvailability = f.OnlineSubscriptionAvailability;
                vm.PercentageChangeInFundPrice = f.PercentageChangeInFundPrice;
                vm.ViewCount = f.ViewCount ?? null;
                vm.Tags = f.Tags;
                result.Add(vm);
            }
            return result;
        }

        public IList<Funds> GetFundsDatas()
        {
            var queryitem = FundRelatedSettingModel.GetFundDetailPageItem();
            var query = queryitem.ID.ToGuid();

            var funds = _repository.GetVisitRecords(query, "id");

            if (funds == null || !funds.Any())
            {
                return new List<Funds>();
            }

            var result = new List<Funds>();

            foreach (var fund in funds)
            {
                var productCode = fund.QueryStrings.ContainsKey("id") ? fund.QueryStrings["id"] : null;
                int? visitCount = fund.VisitCount;

                var vm = new Funds();
                vm.ProductCode = productCode;
                vm.ViewCount = visitCount.ToString();
                vm.ViewCountOrderBy = visitCount;
                result.Add(vm);
            }

            return result;
        }


    }
}

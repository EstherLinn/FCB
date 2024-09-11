using System.Linq;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.RecommendedProduct;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Templates = Feature.Wealth.Component.Models.RecommendedProduct.Template;

namespace Feature.Wealth.Component.Repositories
{
    public class RecommendedProductRepository
    {
        private readonly TabCardsRepository _tabCardsRepository = new();

        public RecommendedProductModel recommendProduct(Item dataSourceItem)
        {
            var multilineField = ItemUtils.GetMultiLineText(dataSourceItem, Templates.RecommendedProduct.Fields.FundIDLIst)?.Where(s => !string.IsNullOrWhiteSpace(s));
            var viewModel = new RecommendedProductModel { Item = dataSourceItem };

            if (multilineField != null)
            {
                viewModel.FundIDList = multilineField.ToList();
                viewModel.DetailLink = FundRelatedSettingModel.GetFundDetailsUrl();
                viewModel.RecommendFunds = _tabCardsRepository.GetFundCardsInfos(multilineField.ToList());
                viewModel.FundCardsNavs = _tabCardsRepository.GetFundCardsNavs(multilineField.ToList());
            }
            return viewModel;
        }

    }
}

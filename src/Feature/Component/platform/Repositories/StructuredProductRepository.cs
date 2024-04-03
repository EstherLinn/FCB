using Feature.Wealth.Component.Models.StructuredProduct;
using Foundation.Wealth.Manager;
using Sitecore.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class StructuredProductRepository
    {
        /// <summary>
        /// 取得Tag篩選選項及其商品
        /// </summary>
        /// <param name="structuredDatasourceId">資料源Id</param>
        /// <param name="fieldId">Tag資料源欄位Id</param>
        /// <returns>篩選項List</returns>
        public IList<TagWithProducts> GetTagOptionsWithProducts(ID structuredDatasourceId, ID fieldId)
        {
            var structuredDatasource = ItemUtils.GetItem(structuredDatasourceId);
            if (structuredDatasource == null || (!structuredDatasource.GetReferenceFieldItem(fieldId).GetChildren(StructProductTag.Id)?.Any() ?? true))
            {
                return new List<TagWithProducts>();
            }

            var TagWithProductsList = new List<TagWithProducts>();

            foreach (var item in structuredDatasource.GetReferenceFieldItem(fieldId).GetChildren(StructProductTag.Id))
            {
                TagWithProductsList.Add(new TagWithProducts
                {
                    TagTitle = item.GetFieldValue(StructProductTag.Fields.TagTitle),
                    StructProductList = item.GetFieldValue(StructProductTag.Fields.StructList)?.Split(';').ToList()
                });
            }

            return TagWithProductsList;
        }

        /// <summary>
        /// 取得單一商品的特定類別的Tags
        /// </summary>
        /// <param name="structuredDatasourceId">資料源Id</param>
        /// <param name="tagDatasourceFieldId">Tag資料源欄位Id</param>
        /// <param name="productID">欲查詢的商品Id</param>
        /// <returns>某類別的Tag List</returns>
        public IList<string> GetProductTags(ID structuredDatasourceId, ID tagDatasourceFieldId, string productID)
        {
            var tagWithProductsList = GetTagOptionsWithProducts(structuredDatasourceId, tagDatasourceFieldId);

            var productTags = tagWithProductsList?.Where(t => t.StructProductList.Contains(productID)).Select(x => x.TagTitle).ToList() ?? new List<string>();

            return productTags;
        }

        /// <summary>
        /// 取得單一商品資訊
        /// </summary>
        /// <param name="productCode">商品代號</param>
        /// <param name="datasourceId">貼標資料源(查詢商品所屬Tags)</param>
        /// <returns>StructuredProductModel基本資訊</returns>
        public StructuredProductModel GetStructuredProduct(string productCode, ID? datasourceId)
        {
            string query = @"
                            SELECT TOP(1)
                                ProductIdentifier,
                                ProductName,
                                ProductCode,
                                IssuingInstitution,
                                ProductMaturityDate,
                                Currency,
                                CurrencyName,
                                BankSellPrice,
                                PriceBaseDate
                            FROM
                                vw_StructProduct
                            WHERE
                                ProductCode = @ProductCode";

            // 部分基本資訊from DB
            var structuredProduct = DbManager.Custom.Execute<StructuredProductModel>(query, new { ProductCode = productCode }, CommandType.Text);

            // 若查無此商品 回傳null
            if (structuredProduct.IsNull())
            {
                return null;
            }


            // 貼標from 後台
            if (!datasourceId.IsNullOrEmpty() && ItemUtils.GetItem(datasourceId)?.TemplateID == StructProductDetailDatasource.Id)
            {
                structuredProduct.KeywordTags = GetProductTags(datasourceId, _StructProductTagDatasource.Fields.KeywordDatasource, productCode);
                structuredProduct.TopicTags = GetProductTags(datasourceId, _StructProductTagDatasource.Fields.TopicDatasource, productCode);
            }
            else
            {
                structuredProduct.KeywordTags = new List<string>();
                structuredProduct.TopicTags = new List<string>();
            }

            return structuredProduct;
        }

        /// <summary>
        /// 取得歷史參考贖回價
        /// </summary>
        /// <param name="productCode">商品代號</param>
        /// <returns>歷史參考贖回價List</returns>
        public HistoryBankSellPrice GetHistoryBankSellPrice(string productCode)
        {
            var result = new HistoryBankSellPrice();

            result.BankSellPrice = DbManager.Custom.ExecuteIList<BankSellPrice>("sp_StructProductSellPrice", new { ProductCode = productCode }, CommandType.StoredProcedure)?.ToList();

            string query = @"
                            SELECT TOP(1)
                                DataDate
                            FROM
                                vw_StructProduct
                            WHERE
                                ProductCode = @ProductCode";
            result.UpdateDate = DbManager.Custom.Execute<string>(query, new { ProductCode = productCode }, CommandType.Text);

            return result;
        }

        /// <summary>
        /// 取得最近三十筆參考贖回價含漲跌
        /// </summary>
        /// <param name="productCode">商品代號</param>
        /// <returns>最近三十筆參考贖回價含漲跌List</returns>
        public IList<BankSellPriceWithChange> GetThirtyDayBankSellPriceWithChange(string productCode)
        {
            var result = DbManager.Custom.ExecuteIList<BankSellPriceWithChange>("sp_StructProductSellPriceCountForThirtyDays", new { ProductCode = productCode }, CommandType.StoredProcedure)?.ToList();

            return result;
        }

        /// <summary>
        /// 取得歷史配息
        /// </summary>
        /// <param name="productCode">商品代號</param>
        /// <returns>歷史配息List</returns>
        public IList<Dividend> GetHistoryDividend(string productCode)
        {
            var result = DbManager.Custom.ExecuteIList<Dividend>("sp_StructProductDividend", new { ProductCode = productCode }, CommandType.StoredProcedure);

            return result;
        }

        /// <summary>
        /// 取得所有結構型商品
        /// </summary>
        /// <param name="datasourceId">資料源Id(for貼標)</param>
        /// <returns>結構型商品List</returns>
        public IList<StructuredProductModel> GetAllStructuredProducts(ID? datasourceId)
        {
            var query = @"SELECT
                               ProductIdentifier
                              ,ProductIdentifierName
                              ,ProductName
                              ,ProductCode
                              ,IssuingInstitution
                              ,ProductMaturityDate
                              ,Currency
                              ,CurrencyName
                              ,CurrencyCode
                              ,BankSellPrice
                              ,PriceBaseDate
                          FROM
                                vw_StructProduct";

            var structuredProducts = DbManager.Custom.ExecuteIList<StructuredProductModel>(query, new { }, CommandType.Text) ?? new List<StructuredProductModel>();

            foreach (var structuredProduct in structuredProducts)
            {
                var productCode = structuredProduct.ProductCode;
                structuredProduct.KeywordTags = GetProductTags(datasourceId, _StructProductTagDatasource.Fields.KeywordDatasource, productCode);
                structuredProduct.TopicTags = GetProductTags(datasourceId, _StructProductTagDatasource.Fields.TopicDatasource, productCode);
            }

            return structuredProducts;
        }

        /// <summary>
        /// 取得詳細頁頁面節點Url
        /// </summary>
        /// <param name="datasourceId">資料源Id</param>
        /// <returns></returns>
        public string GetDetailPageItemUrl(ID datasourceId)
        {
            var structuredDatasource = ItemUtils.GetItem(datasourceId);

            var detailPageItemUrl = structuredDatasource?.TargetItem(StructProductListDatasource.Fields.DetailPageItem)?.Url() ?? string.Empty;

            return detailPageItemUrl;
        }
    }
}
using Feature.Wealth.Component.Models.StructuredProduct;
using Foundation.Wealth.Manager;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class StructuredProductSearchRepository
    {
        /// <summary>
        /// 結構型商品搜尋
        /// </summary>
        /// <param name="item">item</param>
        /// <returns></returns>
        public StructuredProductSearchViewModel GetStructuredProductSearch(Item item)
        {
            if (item == null)
            {
                return null;
            }

            return new StructuredProductSearchViewModel
            {
                DatasourceId = item.ID?.ToString(),
                KeywordOptions = GetTagOptionsWithProducts(item, _StructProductTagDatasource.Fields.KeywordDatasource),
                TopicOptions = GetTagOptionsWithProducts(item, _StructProductTagDatasource.Fields.TopicDatasource),
                DetailPageItemUrl = item.TargetItem(StructProductListDatasource.Fields.DetailPageItem)?.Url() ?? string.Empty
            };
        }

        /// <summary>
        /// 取得Tag篩選選項及其商品
        /// </summary>
        /// <param name="structuredDatasource">資料源Id</param>
        /// <param name="fieldId">Tag資料源欄位Id</param>
        /// <returns>篩選項List</returns>
        private IList<TagWithProducts> GetTagOptionsWithProducts(Item structuredDatasource, ID fieldId)
        {
            if (structuredDatasource == null || (!structuredDatasource.GetReferenceFieldItem(fieldId).GetChildren(StructProductTag.Id)?.Any() ?? true))
            {
                return new List<TagWithProducts>();
            }

            var tagWithProductsList = new List<TagWithProducts>();

            foreach (var item in structuredDatasource.GetReferenceFieldItem(fieldId).GetChildren(StructProductTag.Id))
            {
                tagWithProductsList.Add(new TagWithProducts
                {
                    TagTitle = item.GetFieldValue(StructProductTag.Fields.TagTitle),
                    StructProductList = item.GetMultiLineText(StructProductTag.Fields.StructList)?.ToList()
                });
            }

            return tagWithProductsList;
        }
    }

    public class StructuredProductRepository
    {
        /// <summary>
        /// 取得結構型商品
        /// </summary>
        /// <param name="item">item</param>
        /// <param name="productCode">產品代碼</param>
        public StructuredProductDetailViewModel GetStructuredProduct(Item item, string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                return null;
            }

            var model = new StructuredProductDetailViewModel
            {
                StructuredProduct = GetStructuredProduct(productCode, item),
                HistoryBankSellPrice = GetHistoryBankSellPrice(productCode),
                ThirtyDayBankSellPriceWithChange = GetThirtyDayBankSellPriceWithChange(productCode),
                HistoryDividend = GetHistoryDividend(productCode)
            };
            return model;
        }

        /// <summary>
        /// 取得單一商品的特定類別的Tags
        /// </summary>
        /// <param name="structuredDatasource">資料源</param>
        /// <param name="tagDatasourceFieldId">Tag資料源欄位Id</param>
        /// <param name="productId">欲查詢的商品Id</param>
        /// <returns>某類別的Tag List</returns>
        private IList<string> GetProductTags(Item structuredDatasource, ID tagDatasourceFieldId, string productId)
        {
            var tagWithProductsList = GetTagOptionsWithProducts(structuredDatasource, tagDatasourceFieldId);
            return tagWithProductsList?.Where(t => t.StructProductList.Contains(productId)).Select(x => x.TagTitle).ToList();
        }

        /// <summary>
        /// 取得單一商品資訊
        /// </summary>
        /// <param name="productCode">商品代號</param>
        /// <param name="item">貼標資料源(查詢商品所屬Tags)</param>
        /// <returns>StructuredProductModel基本資訊</returns>
        private StructuredProductModel GetStructuredProduct(string productCode, Item item)
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

            // 貼標 from 後台
            if (item?.TemplateID == StructProductDetailDatasource.Id)
            {
                structuredProduct.KeywordTags = GetProductTags(item, _StructProductTagDatasource.Fields.KeywordDatasource, productCode);
                structuredProduct.TopicTags = GetProductTags(item, _StructProductTagDatasource.Fields.TopicDatasource, productCode);
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
        private HistoryBankSellPrice GetHistoryBankSellPrice(string productCode)
        {
            var result = new HistoryBankSellPrice
            {
                BankSellPrice = DbManager.Custom.ExecuteIList<BankSellPrice>("sp_StructProductSellPrice", new { ProductCode = productCode }, CommandType.StoredProcedure)?.ToList()
            };

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
        private IList<BankSellPriceWithChange> GetThirtyDayBankSellPriceWithChange(string productCode)
        {
            return DbManager.Custom.ExecuteIList<BankSellPriceWithChange>("sp_StructProductSellPriceCountForThirtyDays", new { ProductCode = productCode }, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 取得歷史配息
        /// </summary>
        /// <param name="productCode">商品代號</param>
        /// <returns>歷史配息List</returns>
        private IList<Dividend> GetHistoryDividend(string productCode)
        {
            return DbManager.Custom.ExecuteIList<Dividend>("sp_StructProductDividend", new { ProductCode = productCode }, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 取得Tag篩選選項及其商品
        /// </summary>
        /// <param name="structuredDatasource">資料源Id</param>
        /// <param name="fieldId">Tag資料源欄位Id</param>
        /// <returns>篩選項List</returns>
        private IList<TagWithProducts> GetTagOptionsWithProducts(Item structuredDatasource, ID fieldId)
        {
            if (structuredDatasource == null || (!structuredDatasource.GetReferenceFieldItem(fieldId).GetChildren(StructProductTag.Id)?.Any() ?? true))
            {
                return new List<TagWithProducts>();
            }

            var tagWithProductsList = new List<TagWithProducts>();

            foreach (var item in structuredDatasource.GetReferenceFieldItem(fieldId).GetChildren(StructProductTag.Id))
            {
                tagWithProductsList.Add(new TagWithProducts
                {
                    TagTitle = item.GetFieldValue(StructProductTag.Fields.TagTitle),
                    StructProductList = item.GetMultiLineText(StructProductTag.Fields.StructList)?.ToList()
                });
            }

            return tagWithProductsList;
        }

        /// <summary>
        /// 取得所有結構型商品
        /// </summary>
        /// <param name="item">資料源Id(for貼標)</param>
        /// <returns>結構型商品List</returns>
        public IEnumerable<StructuredProductModel> GetStructuredProducts(Item item)
        {
            string query = @"SELECT
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
                string productCode = structuredProduct.ProductCode;
                structuredProduct.KeywordTags = GetProductTags(item, _StructProductTagDatasource.Fields.KeywordDatasource, productCode);
                structuredProduct.TopicTags = GetProductTags(item, _StructProductTagDatasource.Fields.TopicDatasource, productCode);
            }

            return structuredProducts;
        }
    }
}
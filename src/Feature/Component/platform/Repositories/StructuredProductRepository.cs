using Feature.Wealth.Component.Models.StructuredProduct;
using Foundation.Wealth.Manager;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
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
        /// 結構型商品搜尋頁資料
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
                DetailPageItemUrl = StructuredProductRelatedLinkSetting.GetStructuredProductDetailUrl()
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
            if (structuredDatasource == null || (!ItemUtils.GetMultiListValueItems(structuredDatasource, fieldId)?.Any() ?? true))
            {
                return new List<TagWithProducts>();
            }

            var tagWithProductsList = new List<TagWithProducts>();

            foreach (var item in ItemUtils.GetMultiListValueItems(structuredDatasource, fieldId))
            {
                tagWithProductsList.Add(new TagWithProducts
                {
                    TagName = item.GetFieldValue(StructProductTag.Fields.TagName),
                    ProductCodeList = item.GetMultiLineText(StructProductTag.Fields.ProductCodeList)?.ToList()
                });
            }

            return tagWithProductsList;
        }
    }

    public class StructuredProductRepository
    {
        /// <summary>
        /// 取得結構型商品詳細頁資料
        /// </summary>
        /// <param name="productCode">產品代碼</param>
        public StructuredProductDetailViewModel GetStructuredProductDetail(string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                return null;
            }

            var model = new StructuredProductDetailViewModel
            {
                StructuredProduct = GetSpecificStructuredProduct(productCode),
                HistoryBankSellPrice = GetHistoryBankSellPrice(productCode),
                ThirtyDayBankSellPriceWithChange = GetThirtyDayBankSellPriceWithChange(productCode),
                HistoryDividend = GetHistoryDividend(productCode),
                StructuredProductDetailPageId = StructuredProductRelatedLinkSetting.GetStructuredProductDetailPageItemId(),
                StructuredProductSearchUrl = StructuredProductRelatedLinkSetting.GetStructuredProductSearchUrl()
            };
            return model;
        }

        /// <summary>
        /// 取得單一商品的特定類別的Tags
        /// </summary>
        /// <param name="tagType">欲取得的標籤分類</param>
        /// <param name="productId">欲查詢的商品Id</param>
        /// <returns>某類別的Tag List</returns>
        private IList<string> GetProductTags(StructuredProductTagEnum tagType, string productId)
        {
            var tagWithProductsList = GetTagOptionsWithProducts();
            return tagWithProductsList?.Where(t => t.ProductCodeList.Contains(productId) && t.TagType == tagType).Select(x => x.TagName).ToList();
        }

        /// <summary>
        /// 取得特定結構產品
        /// </summary>
        /// <param name="productCode">商品代號</param>
        /// <returns>StructuredProductModel基本資訊</returns>
        private StructuredProductModel GetSpecificStructuredProduct(string productCode)
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
            structuredProduct.KeywordTags = GetProductTags(StructuredProductTagEnum.KeywordTag, productCode);
            structuredProduct.TopicTags = GetProductTags(StructuredProductTagEnum.SortTag, productCode);
            structuredProduct.DiscountTags = GetProductTags(StructuredProductTagEnum.DiscountTag, productCode);

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
        /// <returns>篩選項List</returns>
        private IList<TagWithProducts> GetTagOptionsWithProducts()
        {
            var tagWithProductsList = new List<TagWithProducts>();

            Item TagsFolder = ItemUtils.GetItem(StructuredProductTagsFolder.Id);

            foreach (var item in TagsFolder.GetChildren(TagFolder.Id))
            {
                foreach (var i in item.GetChildren(StructProductTag.Id))
                {
                    TagWithProducts tagModel = new TagWithProducts()
                    {
                        TagType = string.IsNullOrEmpty(item[TagFolder.Fields.TagType]) ? StructuredProductTagEnum.DiscountTag : (StructuredProductTagEnum)Enum.Parse(typeof(StructuredProductTagEnum), item[TagFolder.Fields.TagType]),
                        TagName = i.GetFieldValue(StructProductTag.Fields.TagName),
                        ProductCodeList = i.GetMultiLineText(StructProductTag.Fields.ProductCodeList)?.ToList()
                    };

                    tagWithProductsList.Add(tagModel);
                }
            }

            return tagWithProductsList;
        }

        /// <summary>
        /// 取得所有結構型商品
        /// </summary>
        /// <returns>結構型商品List</returns>
        public IEnumerable<StructuredProductModel> GetStructuredProducts()
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
                structuredProduct.KeywordTags = GetProductTags(StructuredProductTagEnum.KeywordTag, productCode);
                structuredProduct.TopicTags = GetProductTags(StructuredProductTagEnum.SortTag, productCode);
                structuredProduct.DiscountTags = GetProductTags(StructuredProductTagEnum.DiscountTag, productCode);
            }

            return structuredProducts;
        }
    }
}
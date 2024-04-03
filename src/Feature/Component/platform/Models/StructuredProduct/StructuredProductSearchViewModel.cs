using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.StructuredProduct
{
    public class StructuredProductSearchViewModel
    {
        /// <summary>
        /// 資料源Id
        /// </summary>
        public string DatasourceId { get; set; }

        /// <summary>
        /// 篩選選項 熱門關鍵字
        /// </summary>
        public IList<TagWithProducts> KeywordOptions { get; set; }

        /// <summary>
        /// 篩選選項 熱門主題
        /// </summary>
        public IList<TagWithProducts> TopicOptions { get; set; }

        /// <summary>
        /// 詳細頁頁面節點Url
        /// </summary>
        public string DetailPageItemUrl { get; set; }
    }
}
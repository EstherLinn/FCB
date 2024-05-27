using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.SiteProductSearch
{
    internal class ProductTagModel
    {
        /// <summary>
        /// 標籤名稱
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 產品代碼
        /// </summary>
        public List<string> ProductCodeList { get; set; }
    }
}
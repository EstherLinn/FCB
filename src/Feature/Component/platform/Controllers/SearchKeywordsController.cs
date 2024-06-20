using Feature.Wealth.Component.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class SearchKeywordsController : Controller
    {
        private readonly SearchKeywordsRepository _searchBarKeywordsRepository = new SearchKeywordsRepository();

        /// <summary>
        /// 儲存搜尋關鍵字資訊
        /// </summary>
        /// <param name="pageId">頁面 Id</param>
        /// <param name="keyword">搜尋的關鍵字</param>
        /// <param name="productType">產品類別</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Insert(Guid? pageId, string keyword, string productType)
        {
            if (!pageId.HasValue || string.IsNullOrEmpty(keyword) || string.IsNullOrEmpty(productType))
            {
                return new JsonNetResult(new { Status = "Fail" });
            }

            if(!await _searchBarKeywordsRepository.InsertSearchKeywords(pageId, keyword, productType))
            {
                return new JsonNetResult(new { Status = "Fail" });
            }

            return new JsonNetResult(new { Status = "Success"});
        }
    }
}

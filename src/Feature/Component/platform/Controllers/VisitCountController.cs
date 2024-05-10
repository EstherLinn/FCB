using Feature.Wealth.Component.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class VisitCountController : Controller
    {
        private VisitCountRepository _visitCountRepository;

        /// <summary>
        /// 取得瀏覽次數
        /// </summary>
        /// <param name="pageId">頁面 Id</param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get(Guid? pageId, string url)
        {
            if (!pageId.HasValue || string.IsNullOrEmpty(url))
            {
                return new JsonNetResult(new { Status = "Fail" });
            }

            _visitCountRepository = new VisitCountRepository();
            int? count = _visitCountRepository.GetVisitCount(pageId, url);

            return new JsonNetResult(new { Status = "Success", Count = count });
        }

        /// <summary>
        /// 紀錄瀏覽次數
        /// </summary>
        /// <param name="pageId">頁面 Id</param>
        /// <param name="url"></param>
        /// <param name="querystring">網址參數</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Update(Guid? pageId, string url, params string[] querystring)
        {
            if (!pageId.HasValue)
            {
                return new JsonNetResult(new { Status = "Fail" });
            }
            _visitCountRepository = new VisitCountRepository();
            var data = await _visitCountRepository.UpdateVisitCount(pageId, url, querystring);
            return new JsonNetResult(new { Status = "Success", Count = data?.VisitCount });
        }
    }
}

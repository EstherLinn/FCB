using Feature.Wealth.Component.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class ClickCountController : Controller
    {
        private ClickCountRepository _clickCountRepository;

        /// <summary>
        /// 取得點擊次數
        /// </summary>
        /// <param name="pageId">頁面節點ID</param>
        /// <param name="renderingId">元件ID</param>
        /// <param name="datasourceId">資料源ID</param>
        /// <param name="linkFieldId">連結欄位ID</param>
        /// <returns>點擊次數</returns>
        [HttpGet]
        public ActionResult Get(Guid? pageId, Guid? renderingId, Guid? datasourceId, Guid? linkFieldId)
        {
            if (!pageId.HasValue || !renderingId.HasValue || !datasourceId.HasValue || !linkFieldId.HasValue)
            {
                return new JsonNetResult(new { Status = "Fail" });
            }

            _clickCountRepository = new ClickCountRepository();

            string count = _clickCountRepository.GetClickCount(pageId, renderingId, datasourceId, linkFieldId)?.ToString("N0") ?? "0";

            return new JsonNetResult(new { Status = "Success", Count = count });
        }

        /// <summary>
        /// 更新點擊次數
        /// </summary>
        /// <param name="pageId">頁面節點ID</param>
        /// <param name="renderingId">元件ID</param>
        /// <param name="datasourceId">資料源ID</param>
        /// <param name="linkFieldId">連結欄位ID</param>
        /// <returns>更新是否成功，成功回傳true，失敗回傳 false</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(Guid? pageId, Guid? renderingId, Guid? datasourceId, Guid? linkFieldId)
        {
            if (!pageId.HasValue || !renderingId.HasValue || !datasourceId.HasValue || !linkFieldId.HasValue)
            {
                return new JsonNetResult(new { Status = "Fail" });
            }

            _clickCountRepository = new ClickCountRepository();

            var result = await _clickCountRepository.UpdateClickCount(pageId, renderingId, datasourceId, linkFieldId);

            if (result)
            {
                return new JsonNetResult(new { Status = "Success" });
            }
            else
            {
                return new JsonNetResult(new { Status = "Fail" });
            }
        }
    }
}
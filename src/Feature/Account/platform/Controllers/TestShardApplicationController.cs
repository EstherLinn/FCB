using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Account.Controllers
{
    public class TestShardApplicationController : JsonNetController
    {
        public ActionResult SetApplication()
        {
            this.HttpContext.Application["TestShardApplication"] = "here-Is-Test-ShardApplication";
            var objReturn = new
            {
                success = true,
                msg = "已設定測試Application"
            };
            return new JsonNetResult(objReturn);
        }
        public ActionResult GetApplication()
        {
            var objReturn = new
            {
                success = false,
                msg = "Application is null"
            };
            if (this.HttpContext.Application["TestShardApplication"] == null)
            {

                return new JsonNetResult(objReturn);
            }
            objReturn = new
            {
                success = true,
                msg = "已取得測試Application value =" + this.HttpContext.Application["TestShardApplication"].ToString()
            };
            this.HttpContext.Application.Remove("TestShardApplication");
            return new JsonNetResult(objReturn);
        }
    }
}

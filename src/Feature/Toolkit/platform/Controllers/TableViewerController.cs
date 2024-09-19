using Feature.Wealth.Toolkit.Models.TableViewer;
using Feature.Wealth.Toolkit.Models.TableViewer.SitecoreMemberRecord;
using Feature.Wealth.Toolkit.Repositories;
using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Toolkit.Controllers
{
    public class TableViewerController : Controller
    {
        #region Item 變更紀錄

        public ActionResult UserRecord()
        {
            if (!Sitecore.Context.IsLoggedIn)
            {
                return new EmptyResult();
            }

            UserRecordModel model = new();
            model.Init();
            this.ViewBag.Title = model.Domain;
            return PartialView("/Views/Feature/Wealth/Toolkit/TableViewer/UserRecord.cshtml", model);
        }

        public ActionResult GetUserRecord(int iOrderColumeName, int iCount, int iOrder, string rules, string root)
        {
            string[][] parsedRules = JsonConvert.DeserializeObject<string[][]>(rules);
            var model = new UserRecordModel { Path = root };
            model.Init(parsedRules, iCount, iOrder, iOrderColumeName);
            var data = model.GetData();

            object objReturn = new
            {
                Total = data?.Count() ?? 0,
                Data = data,
                model.Message
            };

            return new JsonNetResult(objReturn);
        }

        public ActionResult GetUserRecordFile(int iOrderColumeName, int iCount, int iOrder, string rules, string root)
        {
            string[][] parsedRules = JsonConvert.DeserializeObject<string[][]>(rules);
            var model = new UserRecordModel { Path = root };
            model.Init(parsedRules, iCount, iOrder, iOrderColumeName);
            var data = model.GetData();
            UsageLogRepository usageLogRepository = new UsageLogRepository();

            return File(usageLogRepository.BuildUserRecordReportExcel(data).ToArray(), Xcms.Sitecore.Foundation.Basic.Constant.Constants.ContentType.XLS);
        }

        #endregion Item 變更紀錄

        #region Sitecore Member 登出入紀錄

        public ActionResult SitecoreMemberRecord()
        {
            if (!Sitecore.Context.IsLoggedIn)
            {
                return new EmptyResult();
            }

            SitecoreMemberRecordModel model = new();
            model.Init();
            this.ViewBag.Title = model.Domain;
            return PartialView("/Views/Feature/Wealth/Toolkit/TableViewer/SitecoreMemberRecord.cshtml", model);
        }

        public ActionResult GetSitecoreMemberRecord(int iOrderColumeName, int iCount, int iOrder, string rules, string root)
        {
            string[][] parsedRules = JsonConvert.DeserializeObject<string[][]>(rules);
            var model = new SitecoreMemberRecordModel { Path = root };
            model.Init(parsedRules, iCount, iOrder, iOrderColumeName);
            var data = model.GetData();

            object objReturn = new
            {
                Total = data?.Count() ?? 0,
                Data = data,
                model.Message
            };

            return new JsonNetResult(objReturn);
        }

        public ActionResult GetSitecoreMemberRecordFile(int iOrderColumeName, int iCount, int iOrder, string rules, string root)
        {
            string[][] parsedRules = JsonConvert.DeserializeObject<string[][]>(rules);
            var model = new SitecoreMemberRecordModel { Path = root };
            model.Init(parsedRules, iCount, iOrder, iOrderColumeName);
            var data = model.GetData();
            UsageLogRepository usageLogRepository = new UsageLogRepository();

            return File(usageLogRepository.BuildSitecoreMemberRecordReportExcel(data).ToArray(), Xcms.Sitecore.Foundation.Basic.Constant.Constants.ContentType.XLS);
        }

        #endregion Sitecore Member 登出入紀錄
    }
}
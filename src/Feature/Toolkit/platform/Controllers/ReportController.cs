using Feature.Wealth.Toolkit.Models.Report;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Feature.Wealth.Toolkit.Controllers
{
    public class ReportController : Controller
    {

        public ActionResult ConsultSchedule()
        {
            if (!Sitecore.Context.IsLoggedIn)
            {
                return new EmptyResult();
            }

            // Schedule Query
            string scheduleQuery = @"
            SELECT
                [ScheduleID],
                [EmployeeName],
                [CustomerName],
                [ScheduleDate],
                [StartTime],
                [EndTime],
                [BranchName],
                CASE
                    WHEN [StatusCode] = '0' THEN '待回覆'
                    WHEN [StatusCode] = '1' THEN '已確認'
                    WHEN [StatusCode] = '3' THEN '已取消'
                    ELSE '未知狀態'
                END AS StatusName
            FROM
                [ConsultSchedule] WITH (NOLOCK) 
            WHERE
                [ScheduleDate] >= DATEADD(day, -30, GETDATE())
            ORDER BY
                [ScheduleDate] DESC";

            IList<ScheduleViewModel> scheduleList = DbManager.Custom.ExecuteIList<ScheduleViewModel>(scheduleQuery, null, System.Data.CommandType.Text);


            // StatusCount Query
            string statusCountQuery = @"
            SELECT
                COUNT(*) AS StatusCount,
                CASE
                    WHEN [StatusCode] = '0' THEN '待回覆'
                    WHEN [StatusCode] = '1' THEN '已確認'
                    WHEN [StatusCode] = '3' THEN '已取消'
                    ELSE '未知狀態'
                END AS StatusName
            FROM
                [ConsultSchedule] WITH (NOLOCK) 
            WHERE
                [ScheduleDate] >= DATEADD(day, -30, GETDATE())
            GROUP BY
                [StatusCode]";

            IList<StatusCountViewModel> statusCountList = DbManager.Custom.ExecuteIList<StatusCountViewModel>(statusCountQuery, null, System.Data.CommandType.Text);


            // DailyReservations Query
            string dailyReservationsQuery = @"
            SELECT
                [ScheduleDate],
                COUNT(*) AS DailyReservations
            FROM
                [ConsultSchedule] WITH (NOLOCK) 
            WHERE
                [ScheduleDate] >= DATEADD(day, -30, GETDATE())
            GROUP BY
                [ScheduleDate]
            ORDER BY
                [ScheduleDate] DESC";

            IList<DailyReservationsViewModel> dailyReservationsList = DbManager.Custom.ExecuteIList<DailyReservationsViewModel>(dailyReservationsQuery, null, System.Data.CommandType.Text);

            this.ViewBag.Schedules = scheduleList;
            this.ViewBag.StatusCount = statusCountList;
            this.ViewBag.DailyReservations = dailyReservationsList;

            return PartialView("/Views/Feature/Wealth/Toolkit/Report/ConsultSchedule.cshtml");
        }
    }
}
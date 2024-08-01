using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Component.Models.Consult;
using Feature.Wealth.Component.Repositories;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class ConsultController : Controller
    {
        private readonly ConsultRepository _consultRepository = new ConsultRepository();

        public ActionResult Consult()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            if(!FcbMemberHelper.CheckMemberLogin() || string.IsNullOrEmpty(FcbMemberHelper.GetMemberWebBankId()))
            {
                return View("/Views/Feature/Wealth/Component/Consult/Consult.cshtml", null);
            }

            return View("/Views/Feature/Wealth/Component/Consult/Consult.cshtml", CreateConsultModel(item));
        }

        public ActionResult EmployeeConsult()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            if (!FcbMemberHelper.CheckMemberLogin() || string.IsNullOrEmpty(FcbMemberHelper.GetMemberWebBankId()))
            {
                return View("/Views/Feature/Wealth/Component/Consult/EmployeeConsult.cshtml", null);
            }

            return View("/Views/Feature/Wealth/Component/Consult/EmployeeConsult.cshtml", CreateConsultModel(item));
        }

        public ActionResult ConsultList()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            if (!FcbMemberHelper.CheckMemberLogin() || string.IsNullOrEmpty(FcbMemberHelper.GetMemberWebBankId()))
            {
                return View("/Views/Feature/Wealth/Component/Consult/ConsultList.cshtml", null);
            }

            return View("/Views/Feature/Wealth/Component/Consult/ConsultList.cshtml", CreateConsultListModel(item));
        }

        private ConsultListModel CreateConsultListModel(Item item)
        {
            var temps = this._consultRepository.GetConsultScheduleList();

            // 根據使用者過濾顯示
            var consultScheduleList = new List<ConsultSchedule>();

            if (temps != null && temps.Any())
            {
                foreach (var c in temps)
                {
                    if (c.CustomerID.ToLower() == FcbMemberHelper.GetMemberWebBankId().ToLower())
                    {
                        consultScheduleList.Add(c);
                    }
                }
            }

            var consultScheduleForCalendarList = new List<ConsultScheduleForCalendar>();

            if (consultScheduleList != null && consultScheduleList.Any())
            {
                foreach (var consultSchedule in consultScheduleList)
                {
                    var consultScheduleForCalendar = new ConsultScheduleForCalendar
                    {
                        Id = consultSchedule.ScheduleID.ToString(),
                        UserName = consultSchedule.CustomerName,
                        StaffName = consultSchedule.EmployeeName,
                        Phone = consultSchedule.Phone,
                        Email = consultSchedule.Mail,
                        Topic = consultSchedule.Subject,
                        Other = consultSchedule.Description,
                        StartTime = consultSchedule.ScheduleDate.ToString("yyyy-MM-dd") + "T" + consultSchedule.StartTime,
                        EndTime = consultSchedule.ScheduleDate.ToString("yyyy-MM-dd") + "T" + consultSchedule.EndTime,
                    };

                    if (consultSchedule.StatusCode == "0")
                    {
                        consultScheduleForCalendar.Status = "預約中";
                        consultScheduleForCalendar.Type = "reserving";
                        consultScheduleForCalendar.CategoryColor = "#fdce5eff";
                        consultScheduleForCalendar.PopupColor = "#F4B00F";
                    }
                    else if (consultSchedule.StatusCode == "1")
                    {
                        consultScheduleForCalendar.Status = "預約成功";
                        consultScheduleForCalendar.Type = "success";
                        consultScheduleForCalendar.CategoryColor = "#7dd4a4ff";
                        consultScheduleForCalendar.PopupColor = "#56B280";
                    }
                    else
                    {
                        consultScheduleForCalendar.Status = "歷史紀錄";
                        consultScheduleForCalendar.Type = "history";
                        consultScheduleForCalendar.CategoryColor = "#c3c3c3ff";
                        consultScheduleForCalendar.PopupColor = "#9C9C9C";
                    }

                    consultScheduleForCalendarList.Add(consultScheduleForCalendar);
                }
            }

            var consultListModel = new ConsultListModel
            {
                Item = item,
                ConsultLink = ConsultRelatedLinkSetting.GetConsultUrl(),
                ConsultScheduleLink = ItemUtils.GeneralLink(item, Template.ConsultList.Fields.ConsultScheduleLink)?.Url,
                Notice = new HtmlString(ItemUtils.GetFieldValue(item, Template.ConsultList.Fields.Notice)),
                ConsultSchedules = consultScheduleList,
                ConsultSchedulesHtmlString = new HtmlString(JsonConvert.SerializeObject(consultScheduleList)),
                ConsultScheduleForCalendars = consultScheduleForCalendarList,
                ConsultScheduleForCalendarsHtmlString = new HtmlString(JsonConvert.SerializeObject(consultScheduleForCalendarList)),
            };

            var qnas = ItemUtils.GetMultiListValueItems(item, Template.ConsultList.Fields.QandA);

            if (qnas != null && qnas.Any())
            {
                foreach (var qna in qnas)
                {
                    consultListModel.QandAList.Add(new QandA
                    {
                        Question = ItemUtils.GetFieldValue(qna, Template.ConsultQandA.Fields.Question),
                        Answer = ItemUtils.GetFieldValue(qna, Template.ConsultQandA.Fields.Answer)
                    });
                }
            }

            return consultListModel;
        }

        private ConsultModel CreateConsultModel(Item item)
        {
            var temps = this._consultRepository.GetConsultScheduleList();

            temps = temps.Where(c => DateTime.Compare(c.ScheduleDate, DateTime.Now) > 0 && c.StatusCode != "3").ToList();

            var consultScheduleList = new List<ConsultSchedule>();


            if(temps != null && temps.Any())
            {
                foreach(var c  in temps)
                {
                    // TODO 非本人的預約資料只留下時間及分機相關資訊
                    consultScheduleList.Add(c);
                }
            }

            var info = FcbMemberHelper.GetMemberAllInfo();

            var consultModel = new ConsultModel
            {
                Item = item,
                ReturnLink = ItemUtils.GeneralLink(item, Template.ConsultSchedule.Fields.ReturnLink)?.Url,
                ConsultSchedules = consultScheduleList,
                ConsultSchedulesHtmlString = new HtmlString(JsonConvert.SerializeObject(consultScheduleList)),
                EmployeeID = info.AdvisrorID,
                EmployeeName = info.Advisror,
                CustomerID = info.WebBankId,                
                CustomerName = info.MemberName,                
            };

            // 取得近30日假日           
            string[] holidays = this._consultRepository.GetCalendar().Where(c => c.IsHoliday).Select(c => c.RealDate).ToArray();

            var now = DateTime.Now.AddDays(1);

            var allowDates = new List<string>();
            var holidayDates = new List<string>();

            while (allowDates.Count < 10)
            {
                string nowString = now.ToString("yyyy-MM-dd");

                if (holidays.Contains(nowString))
                {
                    if (allowDates.Count > 0)
                    {
                        holidayDates.Add(nowString);
                    }
                }
                else
                {
                    allowDates.Add(nowString);
                }

                now = now.AddDays(1);
            }

            consultModel.Start = allowDates[0];
            consultModel.End = allowDates[allowDates.Count - 1];
            consultModel.HolidayDatesHtmlString = new HtmlString(JsonConvert.SerializeObject(holidayDates));

            //TODO 呼叫 IMVP API 取得理顧已佔用時間
            var reserveds = new List<Reserved>();
            reserveds.Add(new Reserved { Date = "2024-06-11", StartTime = "10:30", EndTime = "11:00" });
            reserveds.Add(new Reserved { Date = "2024-06-12", StartTime = "14:30", EndTime = "15:00" });

            // 把對應理顧已預約時間加入已佔用時間
            var temp = consultScheduleList.Where(c => c.EmployeeID.ToLower() == FcbMemberHelper.GetMemberAllInfo().AdvisrorID.ToLower() && c.StatusCode != "3"
            && DateTime.Compare(c.ScheduleDate, DateTime.Now) > 0);

            if (temp != null && temp.Any())
            {
                foreach (var c in temp)
                {
                    reserveds.Add(new Reserved { Date = c.ScheduleDateString, StartTime = c.StartTime, EndTime = c.EndTime });
                }
            }


            consultModel.ReservedsHtmlString = new HtmlString(JsonConvert.SerializeObject(reserveds));

            return consultModel;
        }

        public ActionResult ConsultSchedule()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            string scheduleID = Sitecore.Web.WebUtil.GetSafeQueryString("id");
            string type = Sitecore.Web.WebUtil.GetSafeQueryString("type");

            var consultScheduleModel = CreateConsultScheduleModel(item);

            if (string.IsNullOrEmpty(scheduleID) || !Guid.TryParse(scheduleID, out var result))
            {
                consultScheduleModel.Message = "找不到對應的預約紀錄，即將返回列表。";
            }
            else
            {
                var consultSchedule = this._consultRepository.GetConsultSchedule(scheduleID);

                if (consultSchedule == null || string.IsNullOrEmpty(consultSchedule.StartTime) || string.IsNullOrEmpty(consultSchedule.EndTime))
                {
                    consultScheduleModel.Message = "找不到對應的預約紀錄，即將返回列表。";
                }
                else
                {
                    var start = DateTime.Parse(consultSchedule.ScheduleDate.ToString("yyyy-MM-dd") + " " + consultSchedule.StartTime);
                    var end = DateTime.Parse(consultSchedule.ScheduleDate.ToString("yyyy-MM-dd") + " " + consultSchedule.EndTime);
                    var now = DateTime.Now;

                    if (DateTime.Compare(start, now) > 0)
                    {
                        consultScheduleModel.Message = "預約未開始，即將返回列表。";
                    }
                    else if (DateTime.Compare(end, now) < 0)
                    {
                        consultScheduleModel.Message = "預約已結束，即將返回列表。";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(consultSchedule.EmployeeURL) || string.IsNullOrEmpty(consultSchedule.CustomerURL))
                        {
                            // TODO 呼叫API取得視訊連結

                            // 更新DB資料
                            this._consultRepository.UpdateConsultSchedule(consultSchedule);
                        }

                        // 進入預約連結
                        if (type == "Employee")
                        {
                            consultScheduleModel.ReturnLink = consultSchedule.EmployeeURL;
                        }
                        else
                        {
                            consultScheduleModel.ReturnLink = consultSchedule.CustomerURL;
                        }
                    }
                }
            }


            return View("/Views/Feature/Wealth/Component/Consult/ConsultSchedule.cshtml", consultScheduleModel);
        }

        private ConsultScheduleModel CreateConsultScheduleModel(Item item)
        {
            var consultScheduleModel = new ConsultScheduleModel
            {
                Item = item,
                ReturnLink = ItemUtils.GeneralLink(item, Template.ConsultSchedule.Fields.ReturnLink)?.Url
            };

            return consultScheduleModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConsultSchedule(ConsultSchedule consultSchedule)
        {
            consultSchedule.ScheduleID = Guid.NewGuid();

            var info = FcbMemberHelper.GetMemberAllInfo();

            consultSchedule.EmployeeID = info.AdvisrorID;
            consultSchedule.EmployeeName = info.Advisror;
            //TODO 找到對應分行資訊
            consultSchedule.BranchCode = "B203050000";
            consultSchedule.BranchName = "樹林分行";
            consultSchedule.BranchPhone = "(02)123456789";
            consultSchedule.DepartmentCode = "B20307E001";

            if (string.IsNullOrEmpty(consultSchedule.CustomerID) || string.IsNullOrEmpty(consultSchedule.CustomerName))
            {
                consultSchedule.CustomerID = info.WebBankId;
                consultSchedule.CustomerName = info.MemberName;
            }
            else
            {
                //TODO 驗證使用者資訊
            }

            consultSchedule.ScheduleDate = DateTime.Parse(consultSchedule.ScheduleDateString);
            consultSchedule.DNIS = this._consultRepository.GetDNIS(consultSchedule);

            if (string.IsNullOrEmpty(consultSchedule.DNIS))
            {
                return new JsonNetResult(false);
            }

            if (this._consultRepository.CheckEmployeeSchedule(consultSchedule))
            {
                return new JsonNetResult(false);
            }

            this._consultRepository.InsertConsultSchedule(consultSchedule);

            //TODO 呼叫 IMVP API

            return new JsonNetResult(true);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConsultSchedule(ConsultSchedule consultSchedule)
        {
            var info = FcbMemberHelper.GetMemberAllInfo();

            //TODO 驗證使用者資訊

            this._consultRepository.CancelConsultSchedule(consultSchedule);

            //TODO 呼叫 IMVP API

            return new JsonNetResult(true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Account.Filter;
using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Component.Models.Consult;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Mail;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Mvc.Presentation;
using Sitecore.SecurityModel;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class ConsultController : Controller
    {
        private readonly ConsultRepository _consultRepository = new ConsultRepository();
        private readonly OctonApiRespository _octonApiRespository = new OctonApiRespository();
        private readonly IMVPApiRespository _iMVPApiRespository = new IMVPApiRespository();

        public ActionResult Consult()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            if (!FcbMemberHelper.CheckMemberLogin() || string.IsNullOrEmpty(FcbMemberHelper.GetMemberWebBankId()))
            {
                return View("/Views/Feature/Wealth/Component/Consult/Consult.cshtml", null);
            }

            if (FcbMemberHelper.fcbMemberModel.IsEmployee)
            {
                return View("/Views/Feature/Wealth/Component/Consult/EmployeeConsult.cshtml", CreateConsultModel(item));
            }
            else
            {
                return View("/Views/Feature/Wealth/Component/Consult/Consult.cshtml", CreateConsultModel(item));
            }
        }

        public ActionResult ConsultList()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            if (!FcbMemberHelper.CheckMemberLogin() || string.IsNullOrEmpty(FcbMemberHelper.GetMemberWebBankId()))
            {
                return View("/Views/Feature/Wealth/Component/Consult/ConsultList.cshtml", null);
            }

            if (FcbMemberHelper.fcbMemberModel.IsManager)
            {
                return View("/Views/Feature/Wealth/Component/Consult/ManagerConsultList.cshtml", CreateConsultListModel(item));
            }
            else if (FcbMemberHelper.fcbMemberModel.IsEmployee)
            {
                return View("/Views/Feature/Wealth/Component/Consult/EmployeeConsultList.cshtml", CreateConsultListModel(item));
            }
            else
            {
                return View("/Views/Feature/Wealth/Component/Consult/ConsultList.cshtml", CreateConsultListModel(item));
            }
        }

        private ConsultListModel CreateConsultListModel(Item item)
        {
            var temps = this._consultRepository.GetConsultScheduleList();
            var info = FcbMemberHelper.GetMemberAllInfo();

            // 根據使用者過濾顯示
            var consultScheduleList = new List<ConsultSchedule>();

            if (temps != null && temps.Any())
            {
                foreach (var c in temps)
                {
                    if (info.IsEmployee && !string.IsNullOrEmpty(info.AdvisrorID))
                    {
                        if (info.IsManager)
                        {
                            var branch = this._consultRepository.GetBranch(info.AdvisrorID);
                            if (c.BranchCode == branch.BranchCode)
                            {
                                consultScheduleList.Add(c);
                            }
                        }
                        else
                        {
                            if (c.EmployeeID.ToLower() == info.AdvisrorID.ToLower())
                            {
                                consultScheduleList.Add(c);
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(info.WebBankId) && c.CustomerID.ToLower() == info.WebBankId.ToLower())
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
                        id = consultSchedule.ScheduleID.ToString(),
                        userName = consultSchedule.CustomerName,
                        staffName = consultSchedule.EmployeeName,
                        phone = consultSchedule.Phone,
                        email = consultSchedule.Mail,
                        topic = consultSchedule.Subject,
                        other = consultSchedule.Description,
                        start = consultSchedule.ScheduleDate.ToString("yyyy-MM-dd") + "T" + consultSchedule.StartTime,
                        end = consultSchedule.ScheduleDate.ToString("yyyy-MM-dd") + "T" + consultSchedule.EndTime,
                        release = consultSchedule.Start
                    };

                    if (consultSchedule.StatusCode == "0")
                    {
                        consultScheduleForCalendar.title = "預約中";
                        consultScheduleForCalendar.type = "reserving";
                        consultScheduleForCalendar.categoryColor = "#fdce5eff";
                        consultScheduleForCalendar.popupColor = "#F4B00F";
                    }
                    else if (consultSchedule.StatusCode == "1")
                    {
                        consultScheduleForCalendar.title = "預約成功";
                        consultScheduleForCalendar.type = "success";
                        consultScheduleForCalendar.categoryColor = "#7dd4a4ff";
                        consultScheduleForCalendar.popupColor = "#56B280";
                    }
                    else
                    {
                        consultScheduleForCalendar.title = "歷史紀錄";
                        consultScheduleForCalendar.type = "history";
                        consultScheduleForCalendar.categoryColor = "#c3c3c3ff";
                        consultScheduleForCalendar.popupColor = "#9C9C9C";
                    }

                    consultScheduleForCalendarList.Add(consultScheduleForCalendar);
                }
            }

            var consultListModel = new ConsultListModel
            {
                Item = item,
                ConsultLink = ConsultRelatedLinkSetting.GetConsultUrl(),
                ConsultScheduleLink = ConsultRelatedLinkSetting.GetConsultScheduleUrl(),
                Notice = new HtmlString(ItemUtils.GetFieldValue(item, Template.ConsultList.Fields.Notice)),
                MainTitle = ItemUtils.GetFieldValue(item, Template.ConsultList.Fields.MainTitle),
                Description = new HtmlString(ItemUtils.GetFieldValue(item, Template.ConsultList.Fields.Description)),
                ButtonText = ItemUtils.GetFieldValue(item, Template.ConsultList.Fields.ButtonText),
                Image = ItemUtils.ImageUrl(item, Template.ConsultList.Fields.Image),
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
            var info = FcbMemberHelper.GetMemberAllInfo();

            var temps = this._consultRepository.GetConsultScheduleList();

            temps = temps.Where(c => DateTime.Compare(c.ScheduleDate, DateTime.Now) > 0 && c.StatusCode != "3").ToList();

            var consultScheduleList = new List<ConsultSchedule>();

            if (temps != null && temps.Any())
            {
                foreach (var c in temps)
                {
                    if (info.IsEmployee && !string.IsNullOrEmpty(info.AdvisrorID))
                    {
                        // 非本人的預約資料只留下時間及分機相關資訊
                        if (c.EmployeeID == info.AdvisrorID)
                        {
                            consultScheduleList.Add(c);
                        }
                        else
                        {
                            var clone = new ConsultSchedule()
                            {
                                ScheduleID = c.ScheduleID,
                                EmployeeID = c.EmployeeID,
                                ScheduleDate = c.ScheduleDate,
                                ScheduleDateString = c.ScheduleDateString,
                                StartTime = c.StartTime,
                                EndTime = c.EndTime,
                                DNIS = c.DNIS,
                            };

                            consultScheduleList.Add(clone);
                        }
                    }
                    else
                    {
                        // 非本人的預約資料只留下時間及分機相關資訊
                        if (c.CustomerID == info.WebBankId)
                        {
                            consultScheduleList.Add(c);
                        }
                        else
                        {
                            var clone = new ConsultSchedule()
                            {
                                ScheduleID = c.ScheduleID,
                                EmployeeID = c.EmployeeID,
                                ScheduleDate = c.ScheduleDate,
                                ScheduleDateString = c.ScheduleDateString,
                                StartTime = c.StartTime,
                                EndTime = c.EndTime,
                                DNIS = c.DNIS,
                            };

                            consultScheduleList.Add(clone);
                        }
                    }
                }
            }

            var consultModel = new ConsultModel
            {
                Item = item,
                ReturnLink = ConsultRelatedLinkSetting.GetConsultListUrl(),
                ConsultSchedules = consultScheduleList,
                ConsultSchedulesHtmlString = new HtmlString(JsonConvert.SerializeObject(consultScheduleList)),
                EmployeeID = info.AdvisrorID,
                EmployeeName = info.Advisror,
                CustomerID = info.WebBankId,
                CustomerName = info.MemberName,
                PersonalInformationLink = ItemUtils.GeneralLink(item, Template.ConsultSchedule.Fields.PersonalInformationLink).Url,
            };

            var subjects = ItemUtils.GetMultiListValueItems(item, Template.ConsultSchedule.Fields.SubjectList);

            if (subjects != null && subjects.Any())
            {
                foreach (var subject in subjects)
                {
                    consultModel.SubjectList.Add(ItemUtils.GetFieldValue(subject, ComponentTemplates.DropdownOption.Fields.OptionText));
                }
            }

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

            //呼叫 IMVP API 取得理顧已佔用時間
            var reserveds = new List<Reserved>();

            var respons = this._iMVPApiRespository.Verification();

            if (respons != null && respons.ContainsKey("token"))
            {
                var token = respons["token"];
                if (token != null)
                {
                    var respons2 = this._iMVPApiRespository.GetReserved(
                        token.ToString(), info.AdvisrorID, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.AddDays(30).ToString("yyyyMMdd")
                        );

                    if (respons2 != null && respons2.ContainsKey("data"))
                    {
                        var data = respons2["data"];
                        for (int i = 0; i < data.Count(); i++)
                        {
                            reserveds.Add(new Reserved
                            {
                                Date = data[i]["date"].ToString().Insert(6, "-").Insert(4, "-"),
                                StartTime = data[i]["startTime"].ToString().Insert(2, ":"),
                                EndTime = data[i]["endTime"].ToString().Insert(2, ":")
                            });
                        }
                    }
                }
            }

            // 把對應理顧已預約時間加入已佔用時間
            var employeeID = string.IsNullOrEmpty(info.AdvisrorID) ? string.Empty : info.AdvisrorID.ToLower();
            var temp = consultScheduleList.Where(
                c => c.EmployeeID.ToLower() == employeeID && c.StatusCode != "3" && DateTime.Compare(c.ScheduleDate, DateTime.Now) > 0
                );

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
                            // 呼叫API取得視訊連結
                            var octonRequestData = new OctonRequestData
                            {
                                Authorization = consultSchedule.ScheduleID.ToString(),
                                dnis = consultSchedule.DNIS,
                                Date = consultSchedule.ScheduleDate.ToString("yyyy-MM-dd"),
                                Start = consultSchedule.StartTime,
                                End = consultSchedule.EndTime,
                                EmployeeCode = consultSchedule.EmployeeID,
                                EmployeeName = consultSchedule.EmployeeName,
                                BranchCode = consultSchedule.BranchCode,
                                BranchName = consultSchedule.BranchName,
                                BranchPhone = consultSchedule.BranchPhone
                            };

                            var respons = this._octonApiRespository.GetWebURL(octonRequestData);

                            if (respons != null && respons.ContainsKey("Code") && respons["Code"].ToString() == "0000")
                            {
                                consultSchedule.EmployeeURL = respons["Agnet"].ToString();
                                consultSchedule.CustomerURL = respons["URL"].ToString();
                            }

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
                ReturnLink = ConsultRelatedLinkSetting.GetConsultListUrl()
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

            Branch branch = this._consultRepository.GetBranch(consultSchedule.EmployeeID);

            consultSchedule.BranchCode = branch.BranchCode;
            consultSchedule.BranchName = branch.BranchName;
            consultSchedule.BranchPhone = branch.BranchPhone;
            consultSchedule.DepartmentCode = branch.DepartmentCode;

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

            // 避免 DB 出現 NULL
            if (string.IsNullOrEmpty(consultSchedule.Description))
            {
                consultSchedule.Description = string.Empty;
            }

            //呼叫 IMVP API 新增
            if (info.IsEmployee == false)
            {
                var respons = this._iMVPApiRespository.Verification();

                if (respons != null && respons.ContainsKey("token"))
                {
                    var token = respons["token"];
                    if (token != null)
                    {
                        IMVPRequestData imvpRequestData = new IMVPRequestData
                        {
                            token = token.ToString(),
                            scheduleId = consultSchedule.ScheduleID.ToString(),
                            action = "1",
                            empId = consultSchedule.EmployeeID,
                            type = "1",
                            date = consultSchedule.ScheduleDate.ToString("yyyyMMdd"),
                            startTime = consultSchedule.StartTime.Replace(":", string.Empty),
                            endTime = consultSchedule.EndTime.Replace(":", string.Empty),
                            custId = consultSchedule.CustomerID,
                            subject = consultSchedule.Subject,
                            description = consultSchedule.Description
                        };

                        var respons2 = this._iMVPApiRespository.Reserved(imvpRequestData);
                    }
                }
            }

            //TODO 呼叫 IMVP 失敗不新增預約
            this._consultRepository.InsertConsultSchedule(consultSchedule);

            MailSchema mail = new MailSchema { MailTo = consultSchedule.Mail };

            var currentRequestUrl = Request.Url;

            if (info.IsEmployee)
            {
                mail.Topic = this._consultRepository.GetSuccessMailTopic();
                mail.Content = this._consultRepository.GetSuccessMailContent(consultSchedule, currentRequestUrl.Scheme + "://" + Sitecore.Context.Site.TargetHostName + ConsultRelatedLinkSetting.GetConsultScheduleUrl());
            }
            else
            {
                mail.Topic = this._consultRepository.GetWaitMailTopic();
                mail.Content = this._consultRepository.GetWaitMailContent(consultSchedule);
            }

            using (new SecurityDisabler())
            {
                using (new LanguageSwitcher("en"))
                {
                    this._consultRepository.SendMail(mail, GetMailSetting());
                }
            }

            //TODO 呼叫 IMVP 失敗不新增預約
            this._consultRepository.InsertConsultSchedule(consultSchedule);

            MailSchema mail = new MailSchema { MailTo = consultSchedule.Mail };

            if (info.IsEmployee)
            {
                mail.Topic = this._consultRepository.GetSuccessMailTopic();
                mail.Content = this._consultRepository.GetSuccessMailContent(consultSchedule, Sitecore.Context.Site.TargetHostName + ConsultRelatedLinkSetting.GetConsultScheduleUrl());
            }
            else
            {
                mail.Topic = this._consultRepository.GetWaitMailTopic();
                mail.Content = this._consultRepository.GetWaitMailContent(consultSchedule);
            }

            this._consultRepository.SendMail(mail, GetMailSetting());

            return new JsonNetResult(true);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MemberAuthenticationFilter]
        public ActionResult CancelConsultSchedule(ConsultSchedule consultSchedule)
        {
            //驗證使用者資訊
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new JsonNetResult(new
                {
                    success = false,
                    block = true
                });
            }

            if (consultSchedule != null && Guid.TryParse(consultSchedule.ScheduleID.ToString(), out var scheduleID))
            {
                this._consultRepository.CancelConsultSchedule(scheduleID);
            }
            else
            {
                return new JsonNetResult(false);
            }

            MailSchema mail = new MailSchema { MailTo = consultSchedule.Mail };

            mail.Topic = this._consultRepository.GetCancelMailTopic();
            mail.Content = this._consultRepository.GetCancelMailContent(consultSchedule);

            using (new SecurityDisabler())
            {
                using (new LanguageSwitcher("en"))
                {
                    this._consultRepository.SendMail(mail, GetMailSetting());
                }
            }

            MailSchema mail = new MailSchema { MailTo = consultSchedule.Mail };

            mail.Topic = this._consultRepository.GetCancelMailTopic();
            mail.Content = this._consultRepository.GetCancelMailContent(consultSchedule);

            this._consultRepository.SendMail(mail, GetMailSetting());

            //呼叫 IMVP API 取消
            var respons = this._iMVPApiRespository.Verification();

            if (respons != null && respons.ContainsKey("token"))
            {
                var token = respons["token"];
                if (token != null)
                {
                    IMVPRequestData imvpRequestData = new IMVPRequestData
                    {
                        token = token.ToString(),
                        scheduleId = consultSchedule.ScheduleID.ToString(),
                        action = "2",
                        empId = consultSchedule.EmployeeID,
                        type = "1",
                        date = consultSchedule.ScheduleDate.ToString("yyyyMMdd"),
                        startTime = consultSchedule.StartTime.Replace(":", string.Empty),
                        endTime = consultSchedule.EndTime.Replace(":", string.Empty),
                        custId = consultSchedule.CustomerID,
                        subject = consultSchedule.Subject,
                        description = consultSchedule.Description
                    };

                    var respons2 = this._iMVPApiRespository.Reserved(imvpRequestData);
                }
            }

            return new JsonNetResult(new
            {
                success = true,
                block = false
            });
        }

        [HttpPost]
        public ActionResult CheckSchedule(string scheduleId, string action, string description)
        {
            // TODO 驗證來源 IP

            if (string.IsNullOrEmpty(scheduleId) || string.IsNullOrEmpty(action))
            {
                return new JsonNetResult(new { statusCode = -1101, statusMsg = "缺少必要參數" });
            }

            if (Guid.TryParse(scheduleId, out var temp) == false)
            {
                return new JsonNetResult(new { statusCode = -1102, statusMsg = "無效的參數或參數格式不正確" });
            }

            var consultSchedule = this._consultRepository.GetConsultSchedule(scheduleId);

            if (consultSchedule == null || consultSchedule.ScheduleID == null || consultSchedule.ScheduleID == Guid.Empty)
            {
                return new JsonNetResult(new { statusCode = -1104, statusMsg = "資料不存在" });
            }

            if (string.IsNullOrEmpty(description) == false)
            {
                consultSchedule.Description = consultSchedule.Description + " 理顧意見：" + description;
            }

            MailSchema mail = new MailSchema { MailTo = consultSchedule.Mail };

            if (action == "1")
            {
                consultSchedule.StatusCode = "1";
                this._consultRepository.UpdateConsultSchedule(consultSchedule);

                var currentRequestUrl = Request.Url;

                mail.Topic = this._consultRepository.GetSuccessMailTopic();
                mail.Content = this._consultRepository.GetSuccessMailContent(consultSchedule, currentRequestUrl.Scheme + "://" + Sitecore.Context.Site.TargetHostName + ConsultRelatedLinkSetting.GetConsultScheduleUrl());

                using (new SecurityDisabler())
                {
                    using (new LanguageSwitcher("en"))
                    {
                        this._consultRepository.SendMail(mail, GetMailSetting());
                    }
                }
            }
            else if (action == "2")
            {
                consultSchedule.StatusCode = "3";
                this._consultRepository.UpdateConsultSchedule(consultSchedule);

                mail.Topic = this._consultRepository.GetRejectMailTopic();
                mail.Content = this._consultRepository.GetRejectMailContent(consultSchedule, description);

                using (new SecurityDisabler())
                {
                    using (new LanguageSwitcher("en"))
                    {
                        this._consultRepository.SendMail(mail, GetMailSetting());
                    }
                }
            }
            else
            {
                return new JsonNetResult(new { statusCode = -1102, statusMsg = "無效的參數或參數格式不正確" });
            }

            return new JsonNetResult(new { statusCode = 0, statusMsg = "正常" });
        }

        private Item GetMailSetting()
        {
            return ItemUtils.GetItem(Template.SmtpSettings.id);
        }
    }
}

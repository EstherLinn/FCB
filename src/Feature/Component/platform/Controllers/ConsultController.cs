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
using log4net;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Mvc.Presentation;
using Sitecore.SecurityModel;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class ConsultController : Controller
    {
        private readonly ConsultRepository _consultRepository = new ConsultRepository();
        private readonly OctonApiRespository _octonApiRespository = new OctonApiRespository();
        private readonly IMVPApiRespository _iMVPApiRespository = new IMVPApiRespository();
        private readonly ILog _log = Logger.General;

        public ActionResult Consult()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            if (!FcbMemberHelper.CheckMemberLogin() || string.IsNullOrEmpty(FcbMemberHelper.GetMemberWebBankId()))
            {
                return View("/Views/Feature/Wealth/Component/Consult/Consult.cshtml", null);
            }

            if(!FcbMemberHelper.BranchCanUseConsult())
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

            if (!FcbMemberHelper.BranchCanUseConsult())
            {
                return View("/Views/Feature/Wealth/Component/Consult/ConsultList.cshtml", null);
            }

            var model = CreateConsultListModel(item);

            if (FcbMemberHelper.fcbMemberModel.IsManager)
            {
                return View("/Views/Feature/Wealth/Component/Consult/ManagerConsultList.cshtml", model);
            }
            else if (FcbMemberHelper.fcbMemberModel.IsEmployee)
            {
                return View("/Views/Feature/Wealth/Component/Consult/EmployeeConsultList.cshtml", model);
            }
            else if (string.IsNullOrEmpty(FcbMemberHelper.fcbMemberModel.AdvisrorID))
            {
                return View("/Views/Feature/Wealth/Component/Consult/NoEmployeeConsultList.cshtml", model);
            }
            else
            {                
                if(model.ConsultSchedules != null && model.ConsultSchedules.Any())
                {
                    return View("/Views/Feature/Wealth/Component/Consult/ConsultList.cshtml", model);
                }
                else
                {
                    return View("/Views/Feature/Wealth/Component/Consult/NoConsultList.cshtml", model);
                }
            }
        }

        public ActionResult QandAList()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;
            var model = CreateQandAListModel(item);

            var param = RenderingContext.CurrentOrNull?.Rendering.Parameters;

            if(param != null && param["needbr"] == "1")
            {
                model.Needbr = true;
            }

            return View("/Views/Feature/Wealth/Component/Consult/QandAList.cshtml", model);
        }

        private QandAListModel CreateQandAListModel(Item item)
        {
            var qandAListModel = new QandAListModel
            {
                Item = item,
            };

            var qnas = ItemUtils.GetMultiListValueItems(item, Template.QandAList.Fields.QandA);

            if (qnas != null && qnas.Any())
            {
                foreach (var qna in qnas)
                {
                    qandAListModel.QandAList.Add(new QandA
                    {
                        Question = ItemUtils.GetFieldValue(qna, Template.ConsultQandA.Fields.Question),
                        Answer = ItemUtils.GetFieldValue(qna, Template.ConsultQandA.Fields.Answer)
                    });
                }
            }

            return qandAListModel;
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
                    if ((info.IsEmployee || info.IsManager) && !string.IsNullOrEmpty(info.AdvisrorID))
                    {
                        // 理顧跟主管不顯示未確認的預約
                        if(c.StatusCode != "0")
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
                                if (c.EmployeeID == info.AdvisrorID)
                                {
                                    consultScheduleList.Add(c);
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(info.WebBankId) && c.CustomerID == info.WebBankId)
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
                        cid = consultSchedule.ScheduleID.ToString(),
                        userName = consultSchedule.CustomerName,
                        staffName = consultSchedule.EmployeeName,
                        phone = consultSchedule.Phone,
                        email = consultSchedule.Mail,
                        topic = consultSchedule.Subject,
                        other = consultSchedule.Description,
                        start = consultSchedule.ScheduleDate.ToString("yyyy-MM-dd") + "T" + consultSchedule.StartTime,
                        end = consultSchedule.ScheduleDate.ToString("yyyy-MM-dd") + "T" + consultSchedule.EndTime,
                        release = consultSchedule.Start,
                        StatusCode = consultSchedule.StatusCode
                    };

                    if (consultSchedule.StatusCode == "0")
                    {
                        consultScheduleForCalendar.title = "預約中";
                        consultScheduleForCalendar.type = "reserving";
                        consultScheduleForCalendar.categoryColor = "#fdce5eff";
                        consultScheduleForCalendar.popupColor = "#F4B00F";
                        consultScheduleForCalendar.StatusOrder = "1";
                    }
                    else if (consultSchedule.StatusCode == "1")
                    {
                        consultScheduleForCalendar.title = "預約成功";
                        consultScheduleForCalendar.type = "success";
                        consultScheduleForCalendar.categoryColor = "#7dd4a4ff";
                        consultScheduleForCalendar.popupColor = "#56B280";
                        consultScheduleForCalendar.StatusOrder = "0";
                    }
                    else
                    {
                        consultScheduleForCalendar.title = "歷史紀錄";
                        consultScheduleForCalendar.type = "history";
                        consultScheduleForCalendar.categoryColor = "#c3c3c3ff";
                        consultScheduleForCalendar.popupColor = "#9C9C9C";
                        consultScheduleForCalendar.StatusOrder = "2";
                    }

                    consultScheduleForCalendarList.Add(consultScheduleForCalendar);
                }
            }

            var consultListModel = new ConsultListModel
            {
                Item = item,
                ConsultLink = ConsultRelatedLinkSetting.GetConsultUrl(),
                ConsultScheduleLink = ConsultRelatedLinkSetting.GetConsultScheduleUrl(),
                ConsultSchedules = consultScheduleList,
                ConsultSchedulesHtmlString = new HtmlString(JsonConvert.SerializeObject(consultScheduleList)),
                ConsultScheduleForCalendars = consultScheduleForCalendarList,
                ConsultScheduleForCalendarsHtmlString = new HtmlString(JsonConvert.SerializeObject(consultScheduleForCalendarList)),
            };

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
                                BranchCode = c.BranchCode
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
                                BranchCode = c.BranchCode
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
                PersonalInformationText = ItemUtils.GetFieldValue(item, Template.ConsultSchedule.Fields.PersonalInformationText),
                PersonalInformationLink = ItemUtils.GeneralLink(item, Template.ConsultSchedule.Fields.PersonalInformationLink).Url,
            };

            var customerInfos = new List<CustomerInfo>();

            if (info.IsEmployee && !string.IsNullOrEmpty(info.AdvisrorID))
            {
                customerInfos = this._consultRepository.GetCustomerInfos(info.AdvisrorID).ToList();
            }

            consultModel.CustomerInfosHtmlString = new HtmlString(JsonConvert.SerializeObject(customerInfos));

            var subjects = ItemUtils.GetMultiListValueItems(item, Template.ConsultSchedule.Fields.SubjectList);

            if (subjects != null && subjects.Any())
            {
                foreach (var subject in subjects)
                {
                    consultModel.SubjectList.Add(ItemUtils.GetFieldValue(subject, ComponentTemplates.DropdownOption.Fields.OptionText));
                }
            }

            // 取得近30日假日           
            string[] holidays = this._consultRepository.GetHoliday().Select(c => c.RealDate).ToArray();

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
            var employeeID = string.IsNullOrEmpty(info.AdvisrorID) ? string.Empty : info.AdvisrorID;
            var branchCode = string.IsNullOrEmpty(info.AdvisrorID) ? string.Empty : this._consultRepository.GetBranch(info.AdvisrorID).BranchCode;
            // 20240925 增加判斷同分行同一時段只能有一筆預約，因此同分行預約即判斷為已佔用時間
            var temp = consultScheduleList.Where(c => (c.EmployeeID == employeeID || c.BranchCode == branchCode) && c.StatusCode != "3" && DateTime.Compare(c.ScheduleDate, DateTime.Now) > 0);

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

        [MemberAuthenticationFilter]
        public ActionResult ConsultSchedule()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            string id = Sitecore.Web.WebUtil.GetSafeQueryString("id");
            string type = Sitecore.Web.WebUtil.GetSafeQueryString("type");

            var consultScheduleModel = CreateConsultScheduleModel(item);

            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var result))
            {
                consultScheduleModel.Message = "找不到對應的預約紀錄，即將返回列表。";
            }
            else
            {
                var consultSchedule = this._consultRepository.GetConsultSchedule(id);

                if (consultSchedule == null || string.IsNullOrEmpty(consultSchedule.StartTime) || string.IsNullOrEmpty(consultSchedule.EndTime))
                {
                    consultScheduleModel.Message = "找不到對應的預約紀錄，即將返回列表。";
                }
                else if (consultSchedule.StatusCode == "0")
                {
                    consultScheduleModel.Message = "該筆預約紀錄待確認，即將返回列表。";
                }
                else if (consultSchedule.StatusCode == "3")
                {
                    consultScheduleModel.Message = "該筆預約紀錄已取消，即將返回列表。";
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
                                id = id,
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

            // 發信失敗依然要讓前端頁面正常處理
            try
            {
                MailSchema mail = new MailSchema { MailTo = consultSchedule.Mail };

                var currentRequestUrl = Request.Url;
                var url = currentRequestUrl.Scheme + "://" + Sitecore.Context.Site.TargetHostName + ConsultRelatedLinkSetting.GetConsultScheduleUrl();

                if (info.IsEmployee)
                {
                    mail.Topic = this._consultRepository.GetSuccessMailTopic();
                    mail.Content = this._consultRepository.GetSuccessMailContent(consultSchedule, url);
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

                        if (info.IsEmployee)
                        {
                            var mailRecord = this._consultRepository.GetMailRecord(consultSchedule, url);
                            this._consultRepository.InsertMailRecords(new List<MailRecord>() { mailRecord });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
            }

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

        private Item GetMailSetting()
        {
            return ItemUtils.GetItem(Template.SmtpSettings.id);
        }
    }
}

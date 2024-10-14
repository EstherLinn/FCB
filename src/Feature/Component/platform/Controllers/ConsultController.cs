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
using Foundation.Wealth.Helper;
using log4net;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Mvc.Presentation;
using Sitecore.SecurityModel;
using Sitecore.Web;
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

            if (!FcbMemberHelper.BranchCanUseConsult())
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
                if (model.ConsultSchedules != null && model.ConsultSchedules.Any())
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

            if (param != null && param["needbr"] == "1")
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
                        if (c.StatusCode != "0")
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
                NeedEmployeeCode = ConsultRelatedLinkSetting.GetNeedEmployeeIPCheck() ? CheckIP() : true,
            };

            if (string.IsNullOrEmpty(WebUtil.GetCookieValue("EmployeeCodeChecked")) == false)
            {
                consultListModel.NeedEmployeeCode = false;
            }

            return consultListModel;
        }

        private readonly List<int> TimePeriods = new List<int>
        {
            1000,1030,1100,1130,
            1200,1230,1300,1330,
            1400,1430,1500,1530,
            1600,1630,1700,1730,
        };

        private ConsultModel CreateConsultModel(Item item)
        {
            var info = FcbMemberHelper.GetMemberAllInfo();

            var templist = this._consultRepository.GetConsultScheduleList();

            templist = templist.Where(c => DateTime.Compare(c.ScheduleDate, DateTime.Now) > 0 && c.StatusCode != "3").ToList();

            var consultScheduleList = new List<ConsultSchedule>();

            if (templist != null && templist.Any())
            {
                foreach (var c in templist)
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
                NeedEmployeeCode = ConsultRelatedLinkSetting.GetNeedEmployeeIPCheck() ? CheckIP() : true,
            };

            if (string.IsNullOrEmpty(WebUtil.GetCookieValue("EmployeeCodeChecked")) == false)
            {
                consultModel.NeedEmployeeCode = false;
            }

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

            var errorMessage = string.Empty;

            if (ConsultRelatedLinkSetting.GetSkipIMVPAPI() == false)
            {
                try
                {
                    var respons = this._iMVPApiRespository.Verification();

                    if (respons != null && respons.ContainsKey("statusCode") && respons["statusCode"].ToString() == "0")
                    {
                        var token = respons["token"];
                        if (token != null && string.IsNullOrEmpty(token.ToString()) == false)
                        {
                            var start = DateTime.Now.ToString("yyyyMMdd");
                            var end = DateTime.Now.AddDays(30).ToString("yyyyMMdd");

                            var respons2 = this._iMVPApiRespository.GetReserved(token.ToString(), info.AdvisrorID, start, end);

                            if (respons2 != null && respons2.ContainsKey("statusCode") && respons2["statusCode"].ToString() == "0")
                            {
                                if (respons2.ContainsKey("data"))
                                {
                                    var data = respons2["data"];
                                    for (int i = 0; i < data.Count(); i++)
                                    {
                                        var date = data[i]["date"].ToString().Insert(6, "-").Insert(4, "-");
                                        var startTime = int.Parse(data[i]["startTime"].ToString());
                                        var endTime = int.Parse(data[i]["endTime"].ToString());

                                        foreach (var time in this.TimePeriods)
                                        {
                                            if (time >= startTime && time < endTime)
                                            {
                                                reserveds.Add(new Reserved
                                                {
                                                    Date = date,
                                                    StartTime = time.ToString().Insert(2, ":"),
                                                    EndTime = (time + 20).ToString().Insert(2, ":")
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                errorMessage = "呼叫 IMVP GetReserved 發生錯誤 respons2：" + JsonConvert.SerializeObject(respons2);
                            }
                        }
                        else
                        {
                            errorMessage = "呼叫 IMVP Verification token 空值，respon：" + JsonConvert.SerializeObject(respons);
                        }
                    }
                    else
                    {
                        errorMessage = "呼叫 IMVP Verification 發生錯誤 respon：" + JsonConvert.SerializeObject(respons);
                    }
                }
                catch (Exception ex)
                {
                    this._log.Error(ex);
                    errorMessage = "呼叫 IMVP 錯誤：" + ex.ToString();
                }
            }

            consultModel.GetReservedLog = errorMessage;

            // 把對應理顧已預約時間加入已佔用時間
            var employeeID = string.IsNullOrEmpty(info.AdvisrorID) ? string.Empty : info.AdvisrorID;
            var branchCode = string.IsNullOrEmpty(info.AdvisrorID) ? string.Empty : this._consultRepository.GetBranch(info.AdvisrorID).BranchCode;
            // 20240925 增加判斷同分行同一時段只能有一筆預約，因此同分行預約即判斷為已佔用時間
            var tempList2 = consultScheduleList.Where(c => c.EmployeeID == employeeID || c.BranchCode == branchCode);

            if (tempList2 != null && tempList2.Any())
            {
                foreach (var c in tempList2)
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
            var result = new ConsultApiResult();

            consultSchedule.ScheduleID = Guid.NewGuid();

            if (!FcbMemberHelper.CheckMemberLogin())
            {
                result.Success = false;
                result.Message = "您已登出，請重新登入再預約。";
                result.ErrorMessage = "您已登出，請重新登入再預約。";

                return new JsonNetResult(result);
            }

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

            consultSchedule.ScheduleDate = DateTime.Parse(consultSchedule.ScheduleDateString);
            consultSchedule.DNIS = this._consultRepository.GetDNIS(consultSchedule);

            if (string.IsNullOrEmpty(consultSchedule.DNIS))
            {
                result.Success = false;
                result.Message = "因您選取的時段已有其他貴賓預約，請重新預約其他時段，不便之處敬請見諒。";
                result.ErrorMessage = "分機號碼已用盡";

                return new JsonNetResult(result);
            }

            if (this._consultRepository.CheckEmployeeSchedule(consultSchedule))
            {
                result.Success = false;
                result.Message = "因您選取的時段已有其他貴賓預約，請重新預約其他時段，不便之處敬請見諒。";
                result.ErrorMessage = "該理顧已被別人預約";

                return new JsonNetResult(result);
            }

            // 避免 DB 出現 NULL
            if (string.IsNullOrEmpty(consultSchedule.Description))
            {
                consultSchedule.Description = string.Empty;
            }

            var errorMessage = string.Empty;
            var imvpFlag = false;


            if (ConsultRelatedLinkSetting.GetSkipIMVPAPI())
            {
                imvpFlag = true;
            }
            else
            {
                //呼叫 IMVP API 新增
                try
                {
                    var respons = this._iMVPApiRespository.Verification();

                    if (respons != null && respons.ContainsKey("statusCode") && respons["statusCode"].ToString() == "0")
                    {
                        var token = respons["token"];
                        if (token != null && string.IsNullOrEmpty(token.ToString()) == false)
                        {
                            IMVPRequestData imvpRequestData = new IMVPRequestData
                            {
                                token = token.ToString(),
                                scheduleId = consultSchedule.ScheduleID.ToString(),
                                action = "1",
                                empId = consultSchedule.EmployeeID,
                                type = info.IsEmployee ? "2" : "1",
                                date = consultSchedule.ScheduleDate.ToString("yyyyMMdd"),
                                startTime = consultSchedule.StartTime.Replace(":", string.Empty),
                                endTime = consultSchedule.EndTime.Replace(":", string.Empty),
                                custId = this._consultRepository.GetCIF_ID(consultSchedule.CustomerID),
                                subject = consultSchedule.Subject,
                                description = consultSchedule.Description
                            };

                            var respons2 = this._iMVPApiRespository.Reserved(imvpRequestData);

                            if (respons2 != null && respons2.ContainsKey("statusCode") && respons2["statusCode"].ToString() == "0")
                            {
                                imvpFlag = true;
                            }
                            else
                            {
                                errorMessage = "呼叫 IMVP Reserved 發生錯誤 respons2：" + JsonConvert.SerializeObject(respons2);
                            }
                        }
                        else
                        {
                            errorMessage = "呼叫 IMVP Verification token 空值，respon：" + JsonConvert.SerializeObject(respons);
                        }
                    }
                    else
                    {
                        errorMessage = "呼叫 IMVP Verification 發生錯誤 respon：" + JsonConvert.SerializeObject(respons);
                    }
                }
                catch (Exception ex)
                {
                    this._log.Error(ex);
                    errorMessage = "呼叫 IMVP 錯誤：" + ex.ToString();
                }
            }

            // 呼叫 IMVP 失敗不新增預約
            if (imvpFlag == false)
            {
                result.Success = false;
                result.Message = "新增預約失敗：呼叫 IMVP 錯誤";
                result.ErrorMessage = errorMessage;

                return new JsonNetResult(result);
            }

            this._consultRepository.InsertConsultSchedule(consultSchedule);

            // 發信失敗依然要讓前端頁面正常處理
            try
            {
                MailSchema mail = new MailSchema { MailTo = consultSchedule.Mail };

                var currentRequestUrl = Request.Url;
                var url = currentRequestUrl.Scheme + "://" + Sitecore.Context.Site.TargetHostName + ConsultRelatedLinkSetting.GetConsultListUrl();

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
                errorMessage = errorMessage + Environment.NewLine + "發信發生錯誤：" + ex.Message;
            }

            result.Success = true;
            result.ErrorMessage = errorMessage;

            return new JsonNetResult(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MemberAuthenticationFilter]
        public ActionResult CancelConsultSchedule(ConsultSchedule consultSchedule)
        {
            var result = new ConsultApiResult();

            //驗證使用者資訊
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                result.Success = false;
                result.Block = true;

                return new JsonNetResult(result);
            }

            if (consultSchedule == null || Guid.TryParse(consultSchedule.ScheduleID.ToString(), out var scheduleID) == false)
            {
                result.Success = false;
                result.Message = "參數不正確";
                result.ErrorMessage = "參數不正確";

                return new JsonNetResult(result);
            }

            var errorMessage = string.Empty;
            var imvpFlag = false;

            if (ConsultRelatedLinkSetting.GetSkipIMVPAPI())
            {
                imvpFlag = true;
            }
            else
            {
                //呼叫 IMVP API 取消
                try
                {
                    var respons = this._iMVPApiRespository.Verification();

                    if (respons != null && respons.ContainsKey("statusCode") && respons["statusCode"].ToString() == "0")
                    {
                        var token = respons["token"];
                        if (token != null && string.IsNullOrEmpty(token.ToString()) == false)
                        {
                            IMVPRequestData imvpRequestData = new IMVPRequestData
                            {
                                token = token.ToString(),
                                scheduleId = consultSchedule.ScheduleID.ToString(),
                                action = "2",
                                empId = consultSchedule.EmployeeID,
                                type = consultSchedule.Type,
                                date = consultSchedule.ScheduleDate.ToString("yyyyMMdd"),
                                startTime = consultSchedule.StartTime.Replace(":", string.Empty),
                                endTime = consultSchedule.EndTime.Replace(":", string.Empty),
                                custId = this._consultRepository.GetCIF_ID(consultSchedule.CustomerID),
                                subject = consultSchedule.Subject,
                                description = consultSchedule.Description
                            };

                            var respons2 = this._iMVPApiRespository.Reserved(imvpRequestData);

                            if (respons2 != null && respons2.ContainsKey("statusCode") && respons2["statusCode"].ToString() == "0")
                            {
                                imvpFlag = true;
                            }
                            else
                            {
                                errorMessage = "呼叫 IMVP Reserved 發生錯誤 respons2：" + JsonConvert.SerializeObject(respons2);
                            }
                        }
                        else
                        {
                            errorMessage = "呼叫 IMVP Verification token 空值，respon：" + JsonConvert.SerializeObject(respons);
                        }
                    }
                    else
                    {
                        errorMessage = "呼叫 IMVP Verification 發生錯誤 respon：" + JsonConvert.SerializeObject(respons);
                    }
                }
                catch (Exception ex)
                {
                    this._log.Error(ex);
                    errorMessage = "呼叫 IMVP 錯誤：" + ex.ToString();
                }
            }

            // 呼叫 IMVP 失敗不新增預約
            if (imvpFlag == false)
            {
                result.Success = false;
                result.Message = "取消預約失敗：呼叫 IMVP 錯誤";
                result.ErrorMessage = errorMessage;

                return new JsonNetResult(result);
            }

            this._consultRepository.CancelConsultSchedule(scheduleID);

            try
            {
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
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
                result.ErrorMessage = result.ErrorMessage + Environment.NewLine + "發信發生錯誤：" + ex.Message;
            }

            result.Success = true;
            result.Block = false;

            return new JsonNetResult(result);
        }

        [ValidateAntiForgeryToken]
        public ActionResult GetVideoUrl(string id)
        {
            var result = new ConsultApiResult();

            try
            {
                var url = this._octonApiRespository.GetVideoURL(id);

                if (string.IsNullOrEmpty(url))
                {
                    result.Success = false;
                    result.Message = "查無錄影連結";
                }
                else
                {
                    result.Success = true;
                    result.Url = url;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "發生錯誤無法取得錄影連結";
                result.ErrorMessage = ex.ToString();
            }

            return new JsonNetResult(result);
        }

        [ValidateAntiForgeryToken]
        public ActionResult CheckEmployee(string code)
        {
            var result = new ConsultApiResult();

            //驗證使用者資訊
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                result.Success = false;
                result.Block = true;

                return new JsonNetResult(result);
            }
            else
            {
                var info = FcbMemberHelper.GetMemberAllInfo();

                if (!string.IsNullOrEmpty(info.AdvisrorID) && info.AdvisrorID == code.Trim())
                {
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Block = false;
                    result.Message = "員工編號輸入不一致";
                    result.ErrorMessage = "員工編號輸入不一致";
                }
            }

            return new JsonNetResult(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCookie()
        {
            try
            {
                if (string.IsNullOrEmpty(WebUtil.GetCookieValue("EmployeeCodeChecked")))
                {
                    this.Response.SetSameSiteCookie("EmployeeCodeChecked", "1");
                    var objReturn = new
                    {
                        success = true
                    };

                    return new JsonNetResult(objReturn);
                }
                else
                {
                    var objReturn = new
                    {
                        success = false
                    };

                    return new JsonNetResult(objReturn);
                }
            }
            catch (Exception ex)
            {
                var objReturn = new
                {
                    success = false,
                    message = ex.Message
                };

                return new JsonNetResult(objReturn);
            }
        }

        private Item GetMailSetting()
        {
            return ItemUtils.GetItem(Template.SmtpSettings.id);
        }

        private string GetIPAddress()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }

        private bool CheckIP()
        {
            var ip = GetIPAddress();

            return !ip.StartsWith("10.10");
        }
    }
}

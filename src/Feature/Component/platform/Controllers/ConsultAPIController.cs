using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Feature.Wealth.Account.Filter;
using Feature.Wealth.Component.Models.Consult;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Mail;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class ConsultApiController : JsonNetController
    {
        private readonly ConsultRepository _consultRepository = new ConsultRepository();

        [HttpPost]
        [IMVPAuthenticationFilter]
        public async Task<ActionResult> CheckSchedule(string scheduleId, string action, string description)
        {
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
                var url = currentRequestUrl.Scheme + "://" + Sitecore.Context.Site.TargetHostName + ConsultRelatedLinkSetting.GetConsultScheduleUrl();

                mail.Topic = this._consultRepository.GetSuccessMailTopic();
                mail.Content = this._consultRepository.GetSuccessMailContent(consultSchedule, url);

                using (new SecurityDisabler())
                {
                    using (new LanguageSwitcher("en"))
                    {
                        this._consultRepository.SendMail(mail, GetMailSetting());

                        var mailRecord = this._consultRepository.GetMailRecord(consultSchedule, url);
                        this._consultRepository.InsertMailRecords(new List<MailRecord>() { mailRecord });
                    }
                }
            }
            else if (action == "2")
            {
                consultSchedule.StatusCode = "3";
                this._consultRepository.UpdateConsultSchedule(consultSchedule);

                var currentRequestUrl = Request.Url;
                var url = currentRequestUrl.Scheme + "://" + Sitecore.Context.Site.TargetHostName + ConsultRelatedLinkSetting.GetConsultListUrl();

                mail.Topic = this._consultRepository.GetRejectMailTopic();
                mail.Content = this._consultRepository.GetRejectMailContent(consultSchedule, description, url);

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

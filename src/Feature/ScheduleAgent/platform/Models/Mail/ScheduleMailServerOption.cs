using Sitecore.Data.Items;
using System.Net.Mail;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.ScheduleAgent.Models.Mail
{
    public class ScheduleMailServerOption : MailServerOption
    {
        public string SuccessTo { get; set; }
        public string FailedTo { get; set; }
        public string SuccessSubject { get; set; }
        public string SuccessTitle { get; set; }
        public string SuccessContent { get; set; }
        public string FailedSubject { get; set; }
        public string FailedTitle { get; set; }
        public string FailedContent { get; set; }

        public ScheduleMailServerOption(Item item) : base(item)
        {
            var successTo = item["Success To"];
            var failedTo = item["Failed To"];
            var successsubject = item["Success Subject"];
            var successtitle = item["Success Title"];
            var successcontent = item["Success Content"];
            var failedsubject = item["Failed Subject"];
            var failedtitle = item["Failed Title"];
            var failedcontent = item["Failed Content"];

            this.SuccessTo = string.IsNullOrEmpty(successTo) ? Sitecore.Configuration.Settings.GetSetting("MailServerTo") : successTo;
            this.FailedTo = string.IsNullOrEmpty(failedTo) ? Sitecore.Configuration.Settings.GetSetting("MailServerTo") : failedTo;
            this.SuccessSubject = string.IsNullOrEmpty(successsubject) ? Sitecore.Configuration.Settings.GetSetting("SuccessSubject") : successsubject;
            this.SuccessTitle = string.IsNullOrEmpty(successtitle) ? Sitecore.Configuration.Settings.GetSetting("SuccessTitle") : successtitle;
            this.SuccessContent = string.IsNullOrEmpty(successcontent) ? Sitecore.Configuration.Settings.GetSetting("SuccessContent") : successcontent;
            this.FailedSubject = string.IsNullOrEmpty(failedsubject) ? Sitecore.Configuration.Settings.GetSetting("SuccessTitle") : failedsubject;
            this.FailedTitle = string.IsNullOrEmpty(failedtitle) ? Sitecore.Configuration.Settings.GetSetting("SuccessSubject") : failedtitle;
            this.FailedContent = string.IsNullOrEmpty(failedcontent) ? Sitecore.Configuration.Settings.GetSetting("SuccessContent") : failedcontent;
        }
    }
}

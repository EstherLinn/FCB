using Sitecore.Data.Items;
using System;
using System.Net.Mail;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.ScheduleAgent.Models.Mail
{
    public class ScheduleMailServerOption : MailServerOption
    {
        public string[] SuccessTo { get; set; }
        public string[] FailedTo { get; set; }
        public string SuccessSubject { get; set; }
        public string SuccessTitle { get; set; }
        public string SuccessContent { get; set; }
        public string FailedSubject { get; set; }
        public string FailedTitle { get; set; }
        public string FailedContent { get; set; }

        public ScheduleMailServerOption(Item item) : base(item)
        {
            var successTo = item["Success To"]?.Trim();
            var failedTo = item["Failed To"]?.Trim();
            var successsubject = item["Success Subject"];
            var successtitle = item["Success Title"];
            var successcontent = item["Success Content"];
            var failedsubject = item["Failed Subject"];
            var failedtitle = item["Failed Title"];
            var failedcontent = item["Failed Content"];

            string[] successToEmails = !string.IsNullOrEmpty(successTo) ? successTo.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries) : new string[0];
            string[] failedToEmails = !string.IsNullOrEmpty(failedTo) ? failedTo.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries) : new string[0];

            this.SuccessTo = successToEmails.Length > 0 ? successToEmails : new[] { Sitecore.Configuration.Settings.GetSetting("MailServerTo") };
            this.FailedTo = failedToEmails.Length > 0 ? failedToEmails : new[] { Sitecore.Configuration.Settings.GetSetting("MailServerTo") };

            this.SuccessSubject = string.IsNullOrEmpty(successsubject) ? Sitecore.Configuration.Settings.GetSetting("SuccessSubject") : successsubject;
            this.SuccessTitle = string.IsNullOrEmpty(successtitle) ? Sitecore.Configuration.Settings.GetSetting("SuccessTitle") : successtitle;
            this.SuccessContent = string.IsNullOrEmpty(successcontent) ? Sitecore.Configuration.Settings.GetSetting("SuccessContent") : successcontent;
            this.FailedSubject = string.IsNullOrEmpty(failedsubject) ? Sitecore.Configuration.Settings.GetSetting("SuccessTitle") : failedsubject;
            this.FailedTitle = string.IsNullOrEmpty(failedtitle) ? Sitecore.Configuration.Settings.GetSetting("SuccessSubject") : failedtitle;
            this.FailedContent = string.IsNullOrEmpty(failedcontent) ? Sitecore.Configuration.Settings.GetSetting("SuccessContent") : failedcontent;
        }
    }
}

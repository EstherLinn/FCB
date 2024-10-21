using Sitecore.Data.Items;
using System.Net.Mail;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.ScheduleAgent.Models.Mail
{
    public class MailServerOption
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string UserName { get; set; }
        public string User { get; set; }
        public string Mima { get; set; }
        public string From { get; set; }
        public string SuccessTo { get; set; }
        public string FailedTo { get; set; }
        public string SuccessSubject { get; set; }
        public string SuccessTitle { get; set; }
        public string SuccessContent { get; set; }
        public string FailedSubject { get; set; }
        public string FailedTitle { get; set; }
        public string FailedContent { get; set; }

        public MailServerOption(Item item)
        {
            var server = item["SMTP Server"];
            var port = item["SMTP Port"];
            var useSSL = item.IsChecked("SMTP SSL");
            var user = item["Credential User"];
            var pw = item["Credential Password"];
            var from = item["From"];
            var successTo = item["Success To"];
            var failedTo = item["Failed To"];
            var displayName = item["DisplayName"];
            var successsubject = item["Success Subject"];
            var successtitle = item["Success Title"];
            var successcontent = item["Success Content"];
            var failedsubject = item["Failed Subject"];
            var failedtitle = item["Failed Title"];
            var failedcontent = item["Failed Content"];

            this.Server = string.IsNullOrEmpty(server) ? Sitecore.Configuration.Settings.GetSetting("MailServer") : server;
            try
            {
                var tempPort = -1;
                if (int.TryParse(port, out tempPort))
                    this.Port = tempPort;
                else
                    this.Port = int.Parse(Sitecore.Configuration.Settings.GetSetting("MailServerPort"));
            }
            catch
            {
                this.Port = 587;
            }
            this.UseSSL = useSSL;
            this.User = string.IsNullOrEmpty(user) ? Sitecore.Configuration.Settings.GetSetting("MailServerUserName") : user;
            this.Mima = string.IsNullOrEmpty(pw) ? Sitecore.Configuration.Settings.GetSetting("MailServerPassword") : pw;
            this.From = string.IsNullOrEmpty(from) ? Sitecore.Configuration.Settings.GetSetting("MailServerFrom") : from;
            this.UserName = string.IsNullOrEmpty(displayName) ? Sitecore.Configuration.Settings.GetSetting("MailServerUserName") : displayName;
            this.SuccessTo = string.IsNullOrEmpty(successTo) ? Sitecore.Configuration.Settings.GetSetting("MailServerTo") : successTo;
            this.FailedTo = string.IsNullOrEmpty(failedTo) ? Sitecore.Configuration.Settings.GetSetting("MailServerTo") : failedTo;
            this.SuccessSubject = string.IsNullOrEmpty(successsubject) ? Sitecore.Configuration.Settings.GetSetting("SuccessSubject") : successsubject;
            this.SuccessTitle = string.IsNullOrEmpty(successtitle) ? Sitecore.Configuration.Settings.GetSetting("SuccessTitle") : successtitle;
            this.SuccessContent = string.IsNullOrEmpty(successcontent) ? Sitecore.Configuration.Settings.GetSetting("SuccessContent") : successcontent;
            this.FailedSubject = string.IsNullOrEmpty(failedsubject) ? Sitecore.Configuration.Settings.GetSetting("SuccessTitle") : failedsubject;
            this.FailedTitle = string.IsNullOrEmpty(failedtitle) ? Sitecore.Configuration.Settings.GetSetting("SuccessSubject") : failedtitle;
            this.FailedContent = string.IsNullOrEmpty(failedcontent) ? Sitecore.Configuration.Settings.GetSetting("SuccessContent") : failedcontent;
        }

        public SmtpClient ToSMTPClient()
        {
            var client = new SmtpClient() { Host = this.Server, Port = this.Port, EnableSsl = this.UseSSL };
            if (!string.IsNullOrEmpty(this.User) && !string.IsNullOrEmpty(this.Mima))
            {
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential(this.User, this.Mima);
            }
            return client;
        }

    }
}

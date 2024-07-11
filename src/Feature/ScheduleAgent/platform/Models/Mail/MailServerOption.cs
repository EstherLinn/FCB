using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Feature.Wealth.ScheduleAgent.Models.Mail
{
   public   class MailServerOption
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string UserName { get; set; }
        public string User { get; set; }
        public string Mima { get; set; }
        public MailServerOption()
        { 
            this.Server =  Sitecore.Configuration.Settings.GetSetting("MailServer") ;
            this.Port = Sitecore.MainUtil.GetInt(Sitecore.Configuration.Settings.GetSetting("MailServerPort"),25);
            this.UseSSL = Sitecore.MainUtil.GetBool(Sitecore.Configuration.Settings.GetSetting("MailServerUseSsl"), true);
            this.UserName = Sitecore.Configuration.Settings.GetSetting("MailServerUserName");
            this.User = Sitecore.Configuration.Settings.GetSetting("MailServerUser");
            this.Mima = Sitecore.Configuration.Settings.GetSetting("MailServerMima") ;
        }

        public SmtpClient ToSMTPClient()
        {
            var client = new SmtpClient() { Host = this.Server, Port = this.Port, EnableSsl = this.UseSSL };
            if (!string.IsNullOrEmpty(this.User) && !string.IsNullOrEmpty(this.Mima))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(this.User, this.Mima);
            }
            return client;
        }

    }
}

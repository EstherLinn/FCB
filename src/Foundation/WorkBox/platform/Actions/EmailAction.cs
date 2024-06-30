using Feature.WorkBox;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Text;
using Sitecore.Workflows.Simple;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Foundation.WorkBox.Actions.Workflows
{
    public class EmailAction : BaseAction
    {
        /// <summary>
        /// 草稿狀態
        /// </summary>
        private const string DRAFT = WorkBoxTemplates.WorkflowsState.Draft;

        /// <summary>
        /// 待審核狀態
        /// </summary>
        private const string REVIEW = WorkBoxTemplates.WorkflowsState.Approval;

        /// <summary>
        /// SMTP設定
        /// </summary>
        public class MailServerOption
        {
            public string Server { get; set; }
            public int Port { get; set; }
            public bool UseSSL { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }

            public MailServerOption(Item item)
            {
                var server = item["SMTP Server"];
                var port = item["SMTP Port"];
                var useSSL = item.IsChecked("SMTP SSL");
                var user = item["Credential User"];
                var pw = item["Credential Password"];

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
                    this.Port = 25;
                }

                this.UseSSL = useSSL;
                this.UserName = string.IsNullOrEmpty(user) ? Sitecore.Configuration.Settings.GetSetting("MailServerUserName") : user;
                this.Password = string.IsNullOrEmpty(pw) ? Sitecore.Configuration.Settings.GetSetting("MailServerPassword") : pw;
            }

            public SmtpClient ToSMTPClient()
            {
                var client = new SmtpClient() { Host = this.Server, Port = this.Port, EnableSsl = this.UseSSL };
                if (!string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.Password))
                {
                    client.UseDefaultCredentials = true;
                    client.Credentials = new System.Net.NetworkCredential(this.UserName, this.Password);
                }
                return client;
            }
        }

        protected override void Run(WorkflowPipelineArgs args)
        {
            var sender = this.InnerItem["From"];

            //無可執行寄件信箱
            if (string.IsNullOrEmpty(sender))
                return;

            //檢驗通知開關
            CheckboxField notiftyChk = this.InnerItem.Fields["Notify"];
            if (notiftyChk == null || !notiftyChk.Checked)
                return;

            var currentUser = Sitecore.Context.User;

            List<MailAddress> mailReceivers = new List<MailAddress>();

            //檢驗通知執行人
            CheckboxField selfChk = this.InnerItem.Fields["Notify Self"];
            if (selfChk != null && selfChk.Checked && !string.IsNullOrEmpty(currentUser.Profile.Email))
                mailReceivers.Add(new MailAddress(currentUser.Profile.Email, string.IsNullOrEmpty(currentUser.Profile.FullName) ? currentUser.DisplayName : currentUser.Profile.FullName));

            //取得提交人員的使用者屬性
            User submittedUser = null;
            DateTime submittedTime = string.IsNullOrEmpty(this.CurrentItem["__Submitted"]) ? this.CurrentItem.Statistics.Updated : Sitecore.DateUtil.IsoDateToDateTime(this.CurrentItem["__Submitted"]);
            if (!string.IsNullOrEmpty(this.CurrentItem["__Submitted by"]))
                submittedUser = User.FromName(this.CurrentItem["__Submitted by"], false);

            //查無送審人
            if (submittedUser == null)
                return;

            //確認是否加入送審人
            CheckboxField submittedChk = this.InnerItem.Fields["Notify Submitted"];
            if (submittedChk != null && submittedChk.Checked)
                mailReceivers.Add(new MailAddress(submittedUser.Profile.Email, string.IsNullOrEmpty(submittedUser.Profile.FullName) ? submittedUser.DisplayName : submittedUser.Profile.FullName));

            //查詢收件人
            var selectedNotification = this.InnerItem.GetMultiListValueItems("Notification Roles")?.Select(role => $"{role.Name}");
            if (selectedNotification != null && selectedNotification.Any())
            {
                var roles = selectedNotification.Where(role =>
                {
                    return Role.Exists($"{submittedUser.Domain.Name}\\{role}");
                }).Select(role => Role.FromName($"{submittedUser.Domain.Name}\\{role}"));


                foreach (var role in roles)
                {
                    //僅搜尋直接使用該Role的User
                    var users = RolesInRolesManager.GetUsersInRole(role, false)?.Where(u => !string.IsNullOrEmpty(u.Profile?.Email));
                    if (users == null || !users.Any())
                        continue;

                    mailReceivers.AddRange(users.Select(u => new MailAddress(u.Profile.Email, string.IsNullOrEmpty(u.Profile.FullName) ? u.DisplayName : u.Profile.FullName)));
                }
            }

            //查無任何收件人
            if (mailReceivers.Count == 0)
                return;

            //去除重複
            mailReceivers = mailReceivers.Distinct().Reverse().ToList();
            var mailServerOption = new MailServerOption(this.InnerItem);
            var state = this.CurrentItem.State?.GetWorkflowState();

            //必須帶有工作流程狀態
            if (state == null)
                return;
            using (SmtpClient client = mailServerOption.ToSMTPClient())
            {
                var encoding = Encoding.UTF8;
                using (MailMessage message = new MailMessage()
                {
                    From = new MailAddress(sender, string.IsNullOrEmpty(this.InnerItem["DisplayName"]) ? "【理財網通知】" : this.InnerItem["DisplayName"]),
                    IsBodyHtml = true,
                    HeadersEncoding = encoding,
                    BodyEncoding = encoding,
                    SubjectEncoding = encoding
                })
                {
                    try
                    {
                        foreach (var mail in mailReceivers)
                            message.To.Add(mail);

                        string subject = this.InnerItem.GetFieldValue("Subject").Trim();
                        string body = this.InnerItem.Field("Message").ToHtmlString();

                        string displayName = this.CurrentItem.DisplayName;
                        string statusName = string.Empty;
                        string deleteTag = string.Empty;

                        if (this.InnerItem.Parent.Name == "Submit")
                            statusName = "待審核";
                        else if (this.InnerItem.Parent.Name == "Approve")
                            statusName = "被批准";
                        else if (this.InnerItem.Parent.Name == "Reject")
                            statusName = "被退回";

                        if (this.CurrentItem.Fields["Deletion"] != null && this.CurrentItem.Fields["Deletion"].Value == "1")
                            deleteTag = "刪除";

                        StringBuilder sb = new StringBuilder();
                        var currentRequestUrl = HttpContext.Current.Request.Url;
                        sb.AppendLine(body);
                        subject += " 您有" + statusName + "的" + deleteTag + "項目： " + displayName + "，請登入理財網後台" + ((state.StateID == REVIEW) ? "確認" : "處理") + "。";
                        sb.Append("<br/>");

                        var workboxUrlString = new UrlString($"{currentRequestUrl.Scheme}://{currentRequestUrl.DnsSafeHost}/sitecore/shell/Applications/Workbox.aspx");

                        var workboxUrlStringWithoutQuery = workboxUrlString.GetUrl();
                        workboxUrlString.Append("sc_bw", "1");
                        workboxUrlString.Append("reload", "1");
                        var workboxUrl = workboxUrlString.GetUrl();
                        switch (statusName)
                        {
                            case "被批准":
                            case "被退回":
                                sb.AppendLine($"<p>您申請的審核項目如下：</p>");
                                break;
                            default:
                                sb.AppendLine($"<p>審核工作區： <a href=\"{workboxUrl}\">{workboxUrlStringWithoutQuery}</a></p>");
                                break;
                        }
                        var editorUrlString = new UrlString($"{currentRequestUrl.Scheme}://{currentRequestUrl.DnsSafeHost}/sitecore/shell/Applications/Content Editor.aspx");
                        var idString = this.CurrentItem.ID.ToString();
                        editorUrlString.Append("id", idString);
                        editorUrlString.Append("fo", idString);
                        editorUrlString.Append("vs", this.CurrentItem.Version.Number.ToString());
                        editorUrlString.Append("la", this.CurrentItem.Language.Name);
                        editorUrlString.Append("sc_bw", "1");
                        sb.AppendLine($"<p>網頁名稱： <a href=\"{editorUrlString.GetUrl()}\">{displayName}</a></p>");
                        switch (statusName)
                        {
                            case "被批准":
                            case "被退回":
                                sb.AppendLine($"<p>審核結果：{statusName.Substring(1)}</p>");
                                break;
                            default:
                                break;
                        }
                        sb.AppendLine($"<p>節點路徑： {this.CurrentItem.Paths.FullPath}</p>");
                        if (submittedUser != null)
                        {
                            Sitecore.Security.UserProfile profile = submittedUser.Profile;
                            if (profile != null)
                            {
                                sb.AppendLine($"<p>編輯人員： {submittedUser.Profile.FullName} (<a href=\"mailto:{submittedUser.Profile.Email}\">{submittedUser.Name}</a>)</p>");
                            }
                        }
                        //在審核狀態時，列出審核人員
                        if (state.StateID != DRAFT)
                            sb.AppendLine($"<p>審核人員： {User.Current.Profile.FullName} (<a href=\"mailto:{User.Current.Profile.Email}\">{User.Current.Name}</a>)</p>");

                        sb.AppendLine($"<p>處理時間： {DateTime.Now.ToString("yyyy/MM/dd tt hh:mm:ss")}</p>");
                        if (args.CommentFields != null && args.CommentFields["Comments"] != null && !string.IsNullOrEmpty(args.CommentFields["Comments"]))
                        {
                            sb.AppendLine($"<p>說明: <br/>");
                            using (StringReader sr = new StringReader(args.CommentFields["Comments"]))
                            {
                                var line = string.Empty;
                                while ((line = sr.ReadLine()) != null)
                                    sb.AppendLine($"{line}<br/>");
                            }
                            sb.AppendLine("</p>");
                        }

                        message.Body = sb.ToString();
                        message.Subject = subject;

                        //add EnableSsl
                        client.EnableSsl = true;
                        client.Send(message);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"{GetType().FullName} Error", ex, this);
                    }
                }
            }
        }
    }
}
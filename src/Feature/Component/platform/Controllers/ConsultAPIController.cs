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

        [HttpGet]
        public ActionResult GetToken()
        {
            string key = "my_secret_key_12345"; //Secret key which will be used later during validation    
            var issuer = "http://mysite.com";  //normally this will be your site URL    

            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            ////Create a List of Claims, Keep claims name short    
            //var permClaims = new List<Claim>();
            //permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            //permClaims.Add(new Claim("valid", "1"));
            //permClaims.Add(new Claim("userid", "1"));
            //permClaims.Add(new Claim("name", "bilal"));

            ////Create Security Token object by giving required parameters    
            //var token = new JwtSecurityToken(issuer, //Issure    
            //                issuer,  //Audience    
            //                permClaims,
            //                expires: DateTime.Now.AddDays(1),
            //                signingCredentials: credentials);
            //var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            //var returnObj = new { data = jwt_token };
            return new JsonNetResult(null);
        }

        [HttpPost]
        [Authorize]
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

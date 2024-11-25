using Feature.Wealth.Account.Filter;
using Feature.Wealth.Component.Models.Consult;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Mail;
using Microsoft.IdentityModel.Tokens;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class ConsultApiController : JsonNetController
    {
        private readonly ConsultRepository _consultRepository = new ConsultRepository();
        private List<string> Tokens => Session[ApiAuthenticationAttribute.AUTHORIZATION_KEY] as List<string> ?? new List<string>();

        [HttpPost]
        [IMVPAuthenticationFilter]
        public ActionResult GetAuthorization()
        {
            string key = Settings.GetSetting("JwtSecretKey"); //Secret key which will be used later during validation    
            var issuer = Settings.GetSetting("JwtIssuer");  //normally this will be your site URL    
            var audience = Settings.GetSetting("JwtAudience");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            audience,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            AddTokenToSession(jwt_token);
            var returnObj = new { token = jwt_token };
            return new JsonNetResult(returnObj);
        }

        [ApiAuthentication]
        public async Task<ActionResult> CheckSchedule(string scheduleId, string action, string description)
        {
            if (!ApiAuthenticationAttribute.IsVerify(Tokens))
            {
                return new JsonNetResult(new { statusCode = -1100, statusMsg = "CSRF 驗證失敗" });
            }

            if (string.IsNullOrEmpty(scheduleId) || string.IsNullOrEmpty(action))
            {
                return new JsonNetResult(new { statusCode = -1101, statusMsg = "缺少必要參數" });
            }

            if (!Guid.TryParse(scheduleId, out var id))
            {
                return new JsonNetResult(new { statusCode = -1102, statusMsg = "無效的參數或參數格式不正確" });
            }

            var consultSchedule = this._consultRepository.GetConsultSchedule(scheduleId);
            if (consultSchedule == null)
            {
                return new JsonNetResult(new { statusCode = -1104, statusMsg = "資料不存在" });
            }

            consultSchedule.ScheduleID = id;

            if (!string.IsNullOrEmpty(description))
            {
                consultSchedule.Description = consultSchedule.Description + " 理顧意見：" + description;
            }

            MailSchema mail = new MailSchema { MailTo = consultSchedule.Mail };
            MailSchema advisorMail = new MailSchema { MailTo = this._consultRepository.GetEmployeeEmail(consultSchedule) };

            if (action == "1")
            {
                consultSchedule.StatusCode = "1";
                this._consultRepository.UpdateConsultSchedule(consultSchedule);

                var currentRequestUrl = Request.Url;
                var url = currentRequestUrl.Scheme + "://" + Settings.GetSetting("CDHostName") + ConsultRelatedLinkSetting.GetConsultScheduleUrl() + "?id=" + consultSchedule.ScheduleID.ToString();

                mail.Topic = this._consultRepository.GetSuccessMailTopic();
                mail.Content = this._consultRepository.GetSuccessMailContent(consultSchedule, url);
                advisorMail.Topic = this._consultRepository.GetSuccessMailTopic();
                advisorMail.Content = this._consultRepository.GetAdvisorSuccessMailContent(consultSchedule, url);

                using (new SecurityDisabler())
                {
                    using (new LanguageSwitcher("en"))
                    {
                        this._consultRepository.SendMail(mail, GetMailSetting());
                        this._consultRepository.SendMail(advisorMail, GetMailSetting());

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
                var url = currentRequestUrl.Scheme + "://" + Settings.GetSetting("CDHostName") + ConsultRelatedLinkSetting.GetConsultListUrl();

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

        private void AddTokenToSession(string token)
        {
            var key = ApiAuthenticationAttribute.AUTHORIZATION_KEY;
            // 檢查 Session 是否包含 List<string>
            if (Session[key] != null)
            {
                // 從 Session 取出 List<string>
                List<string> stringList = (List<string>)Session[key];

                // 檢查 List 是否為空
                if (!stringList.Contains(token))
                {
                    // 向 List 中添加一個新的字串 "token"
                    stringList.Add(token);

                    // 將修改後的 List 存回 Session
                    Session[key] = stringList;
                }
            }
            else
            {
                // 如果 Session 中沒有 List<string>，初始化一個新的 List 幾存入 Session
                List<string> stringList = [token];
                Session[key] = stringList;
            }
        }

    }
}

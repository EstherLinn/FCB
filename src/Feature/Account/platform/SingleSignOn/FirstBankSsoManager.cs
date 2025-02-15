﻿using Feature.Wealth.Account.Models.OAuth;
using Feature.Wealth.Account.Models.SingleSignOn;
using Sitecore.Security;
using Sitecore.Security.Accounts;
using Sitecore.Security.Domains;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Account.SingleSignOn
{
    public class FirstBankSsoManager : SsoManagerBase
    {
        public readonly SsoDomain DomainName = SsoDomain.fcb;

        private string _workforceId;

        protected override string ClientLanguage { get; } = "zh-TW";
        protected override string ContentLanguage { get; } = "zh-TW";
        protected override string ProfileItemId { get; } = "{AE4C4969-5B7E-4B4E-9042-B2D8701CE214}";
        protected override Domain Domain => DomainUtils.GetDomain(this.DomainName);

        /// <summary>
        /// 建立 Sso 登入者資料
        /// </summary>
        /// <returns>員工代碼</returns>
        /// <example>
        /// </example>
        public FirstBankUser BuildSsoUser()
        {
            var user = new FirstBankUser();
            SetUserProfile(user);
            return user;
        }

        private void SetUserProfile(FirstBankUser user)
        {
            user.Profile.EmployeeId = this._workforceId;
        }

        private void SetUserProfile(FirstBankUser user, Employee member)
        {
            string mailCode = Regex.Replace(member.EmployeeCode.TrimStart('0'), ".$", string.Empty);
            user.Profile.EmployeeEmail = "i" + mailCode.PadLeft(5, '0') + "@firstbank.com.tw";
            user.Profile.EmployeeName = member.EmployeeName;
            user.Profile.DepartmentCode = member.DepartmentCode;
            user.Profile.DepartmentName = member.DepartmentName;
            user.Profile.PositionName = member.PositionName;
        }

        /// <summary>
        /// 產生 Sitecore User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User BuildSitecoreUser(FirstBankUser user)
        {
            SitecoreUserManager sitecoreUserManager = new SitecoreUserManager();
            Employee member = sitecoreUserManager.GetEmployeeInfo(this._workforceId);

            if (string.IsNullOrEmpty(member?.EmployeeCode))
            {
                this.Log.Error($"{nameof(FirstBankSsoManager)} 未存在 HRIS");
                return null;
            }

            SetUserProfile(user, member);

            string domainName = this.Domain?.Name;
            if (string.IsNullOrEmpty(domainName))
            {
                this.Log.Error($"{nameof(FirstBankSsoManager)} 未設定 domain");
                return null;
            }

            string userName = user?.LocalName;
            if (string.IsNullOrEmpty(userName))
            {
                this.Log.Error($"{nameof(FirstBankSsoManager)} 未設定 LocalName");
                return null;
            }

            User scUser = MemberUtils.AddOrGetUser(domainName, userName, member.EmployeeCode);

            UpdateToSitecoreProfile(scUser.Profile, user.Profile);
            var result = GrantRole(scUser, member);

            if (!result.Item1)
            {
                this.Log.Error($"{nameof(FirstBankSsoManager)} {result.Item2}");
                return null;
            }
            return scUser;
        }

        private (bool Success, string Message) GrantRole(User scUser, Employee member)
        {
            var authMappers = this.AuthorizationMapper?.GetValue(this.Domain.Name);

            if (authMappers == null || !authMappers.Any())
            {
                return (false, "無法取得授權對應表");
            }

            foreach (var authMapper in authMappers ?? [])
            {
                foreach (string pattern in authMapper.DepartmentId ?? [])
                {
                    if (Regex.Match(member.DepartmentCode, pattern).Success)
                    {
                        bool isMatch = true;
                        foreach (string code in authMapper.Codes)
                        {
                            if (Regex.Match(member.SupervisorCode, code).Success)
                            {
                                isMatch = true;
                            }
                            else
                            {
                                isMatch = false;
                            }
                        }
                        if (isMatch)
                        {
                            return SetRoles(scUser, authMapper.Roles);
                        }
                    }
                }
            }
            return (false, "無符合授權對應");
        }

        private (bool Success, string Message) SetRoles(User scUser, IEnumerable<Role> roles)
        {
            using (new SecurityDisabler())
            {
                if (scUser.Roles.Any())
                {
                    scUser.Roles.RemoveAll();
                }
                else
                {
                    scUser.Profile.ClientLanguage = this.ClientLanguage;
                    scUser.Profile.ContentLanguage = this.ContentLanguage;
                }
                List<string> roleName = new List<string>();
                foreach (var role in roles)
                {
                    scUser.Roles.Add(role);
                    roleName.Add(role.Name);
                }

                scUser.Profile.SetCustomProperty("Roles", string.Join(Environment.NewLine, roleName));
                scUser.Profile.Save();
                Sitecore.Diagnostics.Log.Info($"用戶 '{scUser.LocalName}' 已成功分配角色 '{string.Join(", ", roleName)}'.", this);
            }
            return (true, null);
        }

        /// <summary>
        /// Sso Token 驗證
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool VerifyAccessToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                this.Log.Error("Workforce Id is null or empty");
                return false;
            }

            this._workforceId = token;
            return true;
        }

        /// <summary>
        /// 有效使用者條件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="scUser"></param>
        /// <returns></returns>
        public (bool Success, string Message) ValidUserCondition(FirstBankUser user, User scUser)
        {
            //// 檢查 Sitecore user
            if (scUser == null)
            {
                return (false, $"{user.LocalName} 此帳號無此權限");
            }

            //// 檢查 Sitecore user 是否可用
            if (!scUser.IsApproved())
            {
                return (false, $"{scUser.Name} 停用中，請與管理人員聯繫");
            }

            return (true, string.Empty);
        }

        private new Dictionary<string, IEnumerable<AuthMapper>> AuthorizationMapper
        {
            get
            {
                using (new SecurityDisabler())
                    return base.AuthorizationMapper();
            }
        }

        public void UpdateToSitecoreProfile(UserProfile userProfile, FirstBankUserProfile info)
        {
            if (info == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(userProfile.Email))
            {
                userProfile.Email = info.EmployeeEmail;
            }

            userProfile.FullName = info.EmployeeName;
            foreach (var propertyInfo in info.GetProperties())
            {
                string value = propertyInfo.GetValue(info)?.ToString() ?? string.Empty;
                userProfile.SetCustomProperty(propertyInfo.Name, value);
            }

            // Assigning the user profile template
            userProfile.ProfileItemId = "{8213A692-BFC6-49AE-B834-D6EE7709FD55}";

            userProfile.Save();
        }
    }
}
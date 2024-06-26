using Feature.Wealth.Account.Models.OAuth;
using Feature.Wealth.Account.Models.SingleSignOn;
using Sitecore.Security.Accounts;
using Sitecore.Security.Domains;
using Sitecore.SecurityModel;
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

            UpdateToSitecoreProfile(scUser, user);
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
            var authorizationMapper = base.AuthorizationMapper();
            var authMappers = authorizationMapper?.GetValue(this.Domain.Name);

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
                //if (!scUser.IsAdministrator)
                //{
                //    scUser.Roles.RemoveAll();
                //}
                List<string> roleName = new List<string>();
                foreach (var role in roles)
                {
                    scUser.Roles.Add(role);
                    scUser.Profile.Save();
                    roleName.Add(role.Name);
                }
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
                return (false, $"{user.LocalName} 不存在");
            }

            //// 檢查 Sitecore user 是否可用
            if (!scUser.IsApproved())
            {
                return (false, $"{scUser.Name} 停用中，請與管理人員聯繫");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// 有效認證條件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public (bool Success, string Message) ValidAuthCondition(FirstBankUser user, out List<Role> roles)
        {
            roles = null;
            var authorizationMapper = base.AuthorizationMapper();
            var authMappers = authorizationMapper?.GetValue(this.Domain.Name)?.ToList();
            if (authMappers == null || !authMappers.Any())
            {
                return (false, "無法取得授權對應表");
            }

            roles = authMappers
                .Where(x =>
                    x.DepartmentId.Any(y =>
                        string.IsNullOrEmpty(y) || user.Profile.DepartmentCode.Equals(y))
                )
                .SelectMany(x => x.Roles).Distinct().ToList();

            return roles.Any() ? (true, string.Empty) : (false, "找不到角色對應表");
        }

        public void UpdateToSitecoreProfile(User scUser, FirstBankUser user)
        {
            var info = user.Profile;
            if (info == null)
            {
                return;
            }

            scUser.Profile.Email = info.EmployeeEmail;
            scUser.Profile.FullName = info.EmployeeName;
            foreach (var propertyInfo in info.GetProperties())
            {
                string value = propertyInfo.GetValue(info)?.ToString() ?? string.Empty;
                scUser.Profile.SetCustomProperty(propertyInfo.Name, value);
            }

            scUser.Profile.Save();
        }
    }
}
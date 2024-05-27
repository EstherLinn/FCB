using Feature.Wealth.Account.Models.SingleSignOn;
using Sitecore.Security.Accounts;
using Sitecore.Security.Domains;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Account.SingleSignOn
{
    public class FirstBankSsoManager : SsoManagerBase
    {
        public readonly SsoDomain DomainName = SsoDomain.Fcb;

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
            if (scUser.IsApproved() == false)
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
            if (authMappers == null || authMappers.Any() == false)
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
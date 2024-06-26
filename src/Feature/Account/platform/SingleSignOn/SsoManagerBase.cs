using Feature.Wealth.Account.Models.SingleSignOn;
using log4net;
using Sitecore.Security.Accounts;
using Sitecore.Security.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.Random;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Account.SingleSignOn
{
    /// <summary>
    /// 授權對應表
    /// </summary>
    public class AuthMapper
    {
        /// <summary>
        /// User 權限 (domain\role)
        /// </summary>
        public IEnumerable<Role> Roles { get; set; }

        /// <summary>
        /// 部門代碼
        /// </summary>
        public IEnumerable<string> DepartmentId { get; set; }

        /// <summary>
        /// 部門代碼
        /// </summary>
        public IEnumerable<string> Codes { get; set; }
    }

    public abstract class SsoManagerBase
    {
        public ILog Log { get; } = Logger.Account;
        protected abstract string ClientLanguage { get; }
        protected abstract string ContentLanguage { get; }

        /// <summary>
        /// core:{AE4C4969-5B7E-4B4E-9042-B2D8701CE214}
        /// </summary>
        protected abstract string ProfileItemId { get; }

        protected abstract Domain Domain { get; }

        /// <summary>
        /// 登入後起始頁面
        /// </summary>
        public virtual string StartPage { get; private set; } = "/sitecore/client/Applications/Launchpad".GetFullUrl();

        /// <summary>
        /// 取得 Sitecore SSO Mapper
        /// </summary>
        /// <param name="autList">Sso 授權</param>
        /// <returns>Dictionary (Domain Name, Sitecore Roles 和 sso account)</returns>
        protected virtual Dictionary<string, IEnumerable<AuthMapper>> AuthorizationMapper()
        {
            string ssoMapDatasource = "{69042AE0-DE8A-41F2-ADFA-BCDF614466C8}";
            string domainFolderTemplateId = "{CDB8962C-F15C-4FC9-AC69-CC5133C0076E}";
            string roleMappingItem = "{CD2AF335-D392-4B45-AAEE-0CA296BDC413}";
            string domainNameField = "domain name";
            var mappingItem = ItemUtils.GetContentItem(ssoMapDatasource);
            var domainItems = mappingItem?.GetChildren(domainFolderTemplateId)?.Where(x => x.FieldHasValue(domainNameField)).ToList();
            if (domainItems == null)
            {
                this.Log.Warn($"{nameof(SsoManagerBase)} Sitecore 不存在 Sso 對應表");
                return null;
            }

            var result = new Dictionary<string, IEnumerable<AuthMapper>>(StringComparer.OrdinalIgnoreCase);
            foreach (var domainItem in domainItems)
            {
                var roleItems = domainItem.GetChildren(roleMappingItem);
                if (roleItems == null)
                {
                    continue;
                }

                // 排除未對應的授權
                var mappers = from roleItem in roleItems
                              let roles = roleItem.GetMultiLineText("role name").Select(Role).Where(x => x != null)
                              let authCodes = roleItem.GetMultiLineText("authentication code")
                              let depId = roleItem.GetMultiLineText("department id").SkipWhile(string.IsNullOrEmpty)
                              select new AuthMapper
                              {
                                  Roles = roles,
                                  Codes = authCodes,
                                  DepartmentId = depId
                              };

                result.TryAdd(domainItem[domainNameField], mappers);
            }

            return result;
        }

        /// <summary>
        /// 驗證 Sitecore Role 是否存在
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        private Role Role(string roleName)
        {
            var role = RoleUtils.GetRole(roleName);
            if (role != null)
            {
                return role;
            }

            this.Log.Error($"{nameof(SsoManagerBase)} Sitecore 不存在 Role: {roleName}");
            return null;
        }

        /// <summary>
        /// 登入 Sitecore shell
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual (bool Success, string Message) Login(User user)
        {
            if (Sitecore.Context.IsLoggedIn)
            {
                MemberUtils.Authentication.LogOutUser();
            }

            bool success = MemberUtils.Authentication.LoginUser(user, true);

            string message = $"{user.Name} 登入成功";
            if (!success)
            {
                message = $"{user.Name} 無法登入，請與管理人員聯繫";
            }

            if (!string.IsNullOrEmpty(user.Profile.StartUrl))
            {
                this.StartPage = user.Profile.StartUrl;
            }

            return (success, message);
        }

        /// <summary>
        /// 產生 Sitecore User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual User BuildSitecoreUser(ISsoUser user)
        {
            string domainName = this.Domain?.Name;
            if (domainName.IsNullOrEmpty())
            {
                this.Log.Error($"{nameof(SsoManagerBase)} 未設定 domain");
                return null;
            }

            string userName = user?.LocalName;
            if (userName.IsNullOrEmpty())
            {
                this.Log.Error($"{nameof(SsoManagerBase)} 未設定 LocalName");
                return null;
            }

            if (MemberUtils.HasUser(domainName, userName))
            {
                return MemberUtils.GetUser(domainName, userName);
            }

            this.Log.Warn($"{nameof(SsoManagerBase)} Sitecore 不存在 User: {domainName}\\{userName}");
            return null;
        }

        /// <summary>
        /// 建立 Sitecore User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual User CreateSitecoreUser(ISsoUser user)
        {
            string domainName = this.Domain?.Name;
            if (domainName.IsNullOrEmpty())
            {
                this.Log.Error($"{nameof(SsoManagerBase)} 未設定 domain ");
                return null;
            }

            string userName = user?.LocalName;
            if (userName.IsNullOrEmpty())
            {
                this.Log.Error($"{nameof(SsoManagerBase)} 未設定 LocalName ");
                return null;
            }

            var scUser = MemberUtils.AddOrGetUser(domainName, userName, RandomMethod.NextAlphaNumeric());

            string profileItemId = this.ProfileItemId;
            if (!profileItemId.IsNullOrEmpty())
            {
                scUser.Profile.ProfileItemId = profileItemId;
            }

            scUser.Profile.ClientLanguage = this.ClientLanguage;
            scUser.Profile.ContentLanguage = this.ContentLanguage;
            scUser.Profile.Save();

            return scUser;
        }
    }
}
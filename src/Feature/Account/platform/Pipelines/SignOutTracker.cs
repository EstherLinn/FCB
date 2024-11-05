using Feature.Wealth.Account.Models.SingleSignOn;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Logout;
using Sitecore.Security.Accounts;
using System;
using System.Data;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Account.Pipelines
{
    public class SignOutTracker
    {
        public void Process(LogoutArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));

            var user = GetUser();
            var domain = DomainUtils.GetDomain(SsoDomain.fcb);
            FirstBankUserProfile firstBankUser = new FirstBankUserProfile();

            // Check if the domain is FCB
            if (Context.User.Domain.Name.Equals(domain?.Name, StringComparison.OrdinalIgnoreCase))
            {
                firstBankUser.Roles = user.Profile.GetCustomProperty("Roles").Replace(Environment.NewLine, ";");
                firstBankUser.DepartmentName = user.Profile.GetCustomProperty("DepartmentName");
            }

            DbManager.Custom.ExecuteNonQuery(
                @"INSERT INTO AuthenticationHistory
                    (UserName, Action, FullName, DepartmentName, Roles, IP)
                    VALUES (@UserName, @Action, @FullName, @DepartmentName, @Roles, @IP);",
                new
                {
                    UserName = user.Name,
                    Action = "Logout",
                    FullName = user.Profile.FullName,
                    DepartmentName = firstBankUser.DepartmentName,
                    Roles = firstBankUser.Roles,
                    IP = IPHelper.GetIPAddress()
                }, CommandType.Text);
        }

        /// <summary>
        /// Get the user
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private User GetUser()
        {
            User tempUser = null;
            // Get the user from the claims identity
            string userName = Context.User.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                // True will make sure that the user is authenticated
                tempUser = User.FromName(userName, true);
            }

            return tempUser;
        }
    }
}

using Foundation.Wealth.Manager;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Pipelines.CookieAuthentication.SignedIn;
using Sitecore.Security.Accounts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Feature.Wealth.Account.Pipelines.Owin.CookieAuthentication.SignedIn
{
    internal class LoginTracker : SignedInProcessor
    {
        public override void Process(SignedInArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            var user = GetUser(args);
            if (user != null && user.IsAuthenticated)
            {
                LogUserRoles(user);
                DbManager.Custom.ExecuteNonQuery(
                    @"INSERT INTO AuthenticationHistory 
                        (UserName, Action)
                        VALUES(@UserName, @Action);",
                    new
                    {
                        UserName = user.Name,
                        Action = "Login"
                    }, CommandType.Text);
            }
            else
            {
                Log.Error("User no authenticated: " + args.User?.UserName, this);
            }
        }


        /// <summary>
        /// This will return the user based on identity
        /// Do not use Context.User because that will only come back as anonymous
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private User GetUser(SignedInArgs args)
        {
            User tempUser = null;
            // Get the user from the claims identity
            string userName = args.Context?.Identity?.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                // True will make sure that the user is authenticated
                tempUser = User.FromName(userName, true);
            }

            return tempUser;
        }
        private void LogUserRoles(User user)
        {
            Log.Info($"Logined User Name: {user.Name}, Role Count: {user.Roles?.Count()}, Roles: {string.Join("|", user.Roles.Select(r => r.DisplayName))}", this);
        }
    }
}
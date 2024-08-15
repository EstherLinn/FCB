using Foundation.Wealth.Manager;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Pipelines.CookieAuthentication.SignOut;
using System.Data;

namespace Feature.Wealth.Account.Pipelines.Owin.CookieAuthentication.SignOut
{
    internal class LogoutTracker : SignOutProcessor
    {
        public override void Process(SignOutArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            DbManager.Custom.ExecuteNonQuery(
                @"INSERT INTO AuthenticationHistory 
                        (UserName, Action)
                        VALUES(@UserName, @Action);",
                new
                {
                    args.UserName,
                    Action = "Logout"
                }, CommandType.Text);
        }
    }
}
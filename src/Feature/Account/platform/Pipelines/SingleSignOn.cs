using Feature.Wealth.Account.Models.SingleSignOn;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Logout;
using Sitecore.Text;
using System;

namespace Feature.Wealth.Account.Pipelines
{
    public class SingleSignOut
    {
        public void Process(LogoutArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            if (Context.User.Domain.Name.Equals(SsoDomain.Fcb.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                string uri = Settings.GetSetting("Feature.Wealth.Account.SSO.LogoutUrl");
                args.RedirectUrl = new UrlString(uri);
            }
        }
    }
}
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.StringExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.FieldTypes;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Foundation.Wealth.Pipelines.HttpRequestBegin
{
    public class RedirectItem : HttpRequestProcessor
    {
        /// <summary>
        /// 排除的 site，可使用 config 增加設定，IgnoreSite = {website}
        /// </summary>
        public List<string> IgnoreSites { get; } = new List<string>() { "shell", "login", "modules_shell", "system", "service" };

        /// <summary>
        /// 定義 Redirect 的欄位，使用順序後至前，可使用 config 增加設定，DefineRedirectField = {guid/name}
        /// </summary>
        public List<string> DefineRedirectFields { get; } = new List<string>() { "RedirectUrl" };

        private bool IgnoreDatabase => Context.Database == null || Context.Database.Name.Equals("core", StringComparison.InvariantCultureIgnoreCase);

        private bool IgnorePageMode => Context.PageMode.IsExperienceEditor || Context.PageMode.IsExperienceEditorEditing || Context.PageMode.IsPreview;
        private bool IgnoreSite => Context.Site == null || this.IgnoreSites.Contains(Context.Site.Name, StringComparer.InvariantCultureIgnoreCase);

        public override void Process(HttpRequestArgs args)
        {
            var item = Context.Item;
            if (item == null || this.IgnoreDatabase || this.IgnorePageMode || this.IgnoreSite)
            {
                return;
            }

            string redirectUrl = GetRedirectUrl(args, item);
            if (redirectUrl.IsNullOrEmpty())
            {
                return;
            }

            args.HttpContext.Response.Redirect(redirectUrl, true);
            args.AbortPipeline();
        }

        protected virtual string GetRedirectUrl(HttpRequestArgs args, Item item)
        {
            var fields = this.DefineRedirectFields.Where(x => !x.IsNullOrEmpty()).Reverse();
            string result = string.Empty;
            foreach (string field in fields)
            {
                var link = item.GeneralLink(field);
                if (link == null || link.Url.IsNullOrEmpty())
                {
                    continue;
                }

                switch (link.Type)
                {
                    case LinkType.External:
                        result = link.Url;
                        break;
                    case LinkType.Internal:
                        if (!item.ID.Equals(link.TargetItem.ID))
                        {
                            result = link.Url;
                        }
                        break;
                }
            }

            return result;
        }
    }
}
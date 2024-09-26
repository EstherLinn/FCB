using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Account.Models.Consult
{
    public static class ConsultRelatedLinkSetting
    {
        public static List<string> BranchCodeList()
        {
            List<string> result = new List<string>();

            Item item = ItemUtils.GetItem(Template.ConsultRelatedLink.Root);
            string branchCodeListString = ItemUtils.GetFieldValue(item, Template.ConsultRelatedLink.Fields.BranchCodeList);

            if (!string.IsNullOrEmpty(branchCodeListString))
            {
                result = branchCodeListString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            }

            return result;
        }
    }

    public struct Template
    {
        public struct ConsultRelatedLink
        {
            public static readonly ID Root = new ID("{3966B7A6-C804-4DA4-8B20-C08B57585BDA}");

            public struct Fields
            {
                public static readonly ID ConsultLink = new ID("{23DB4C97-B0FA-4229-BAE2-A15FF7260A89}");
                public static readonly ID ConsultListLink = new ID("{E770630B-C8E6-443C-8387-1D3525B11151}");
                public static readonly ID ConsultScheduleLink = new ID("{E10AE7C2-4C31-4A92-AB8D-DD82898DFCD1}");
                public static readonly ID BranchCodeList = new ID("{0C8E3A34-B08B-42CF-9EA2-B28A690EEC18}");
            }
        }

    }
}

using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.Compare
{
    public class FundETFCompareModel
    {
        public bool IsValid { get; set; }
        public Item Item { get; set; }
        public string FundListLinkUrl { get; set; }
        public string FundDetailLinkUrl { get; set; }
        public string ETFListLinkUrl { get; set; }
        public string ETFDetailLinkUrl { get; set; }
        public string EmptyLinkUrl { get; set; }
        public IList<GlobalIndex.GlobalIndex> GlobalIndexList { get; set; }
        public FundETFCompareModel(Item item)
        {
            this.Item = item;
            if (item == null)
            {
                return;
            }

            this.FundListLinkUrl = item.GeneralLink(Template.FundETFComparePage.Fields.FundListLink)?.Url;
            this.FundDetailLinkUrl = item.GeneralLink(Template.FundETFComparePage.Fields.FundDetailLink)?.Url;
            this.ETFListLinkUrl = item.GeneralLink(Template.FundETFComparePage.Fields.ETFListLink)?.Url;
            this.ETFDetailLinkUrl = item.GeneralLink(Template.FundETFComparePage.Fields.ETFDetailLink)?.Url;
            this.EmptyLinkUrl = item.GeneralLink(Template.FundETFComparePage.Fields.EmptyLink)?.Url;

            this.IsValid = item.TemplateID == Template.FundETFComparePage.Id;
        }
    }

    public struct Template
    {
        public struct FundETFComparePage
        {
            public static readonly ID Id = new ID("{C7D5D16B-4D7A-46EC-BD20-EE35C402B180}");
            public struct Fields
            {
                public static readonly ID FundListLink = new ID("{DE57D4F7-2DD2-43EA-B58E-74E8537E11F1}");
                public static readonly ID FundDetailLink = new ID("{BF3EA423-F698-4355-A5A2-0AA9F4182500}");
                public static readonly ID ETFListLink = new ID("{18B3420C-AB28-48C0-94F2-E839F3581191}");
                public static readonly ID ETFDetailLink = new ID("{01351433-75D5-4AA6-A987-02D80F15A8ED}");
                public static readonly ID RiskIndicatorsDescription = new ID("{26D48470-FF6A-4C96-9AF3-9F40FB79777C}");
                public static readonly ID EmptyLink = new ID("{567156D0-0E15-4A79-A736-2D4EA5397567}");
            }
        }
    }
}
using Feature.Wealth.Component.Repositories;
using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.DiscountContent
{
    public class DiscountContentModel
    {
        public Item DataSource { get; set; }
        public string PCBannerSrc { get; set; }
        public string MobileBannerSrc { get; set; }
        public string DisplayDate { get; set; }
        public string DisplayTag { get; set; }
        public string ButtonLink { get; set; }
        public DiscountContentModel(Item item)
        {
            if (item == null || item.TemplateID != Templates.DiscountContentDatasource.Id)
            {
                return;
            }

            this.DataSource = item;
            this.PCBannerSrc = ItemUtils.ImageUrl(item, Templates.DiscountContentDatasource.Fields.PCBanner);
            this.MobileBannerSrc = ItemUtils.ImageUrl(item, Templates.DiscountContentDatasource.Fields.MobileBanner);
            this.DisplayDate = DiscountContentRepository.GetDisplayDate(item);
            this.DisplayTag = DiscountContentRepository.GetDisplayTag(item);
            this.ButtonLink = ItemUtils.GeneralLink(item, Templates.DiscountContentDatasource.Fields.ButtonLink)?.Url;
        }
    }

    public struct Templates
    {
        public struct DiscountContentDatasource
        {
            public static readonly ID Id = new ID("{DB64A2B5-C321-447B-A98C-57E2E8A3B65A}");

            public struct Fields
            {
                public static readonly ID PCBanner = new ID("{B1410192-86F8-432C-A37C-DBD0EC2FF92B}");
                public static readonly ID MobileBanner = new ID("{29587ABC-38E1-4787-8753-ECC031BB8C43}");
                public static readonly ID MainTitle = new ID("{42526CEA-3641-467D-B0FE-222ADF528CE8}");
                public static readonly ID StartDate = new ID("{D56FA84A-8F8C-4AB8-8313-6BF81B24205C}");
                public static readonly ID EndDate = new ID("{49B7534A-C78D-4872-8A7A-4CD709602839}");
                public static readonly ID Content = new ID("{96924535-377A-4DFF-869D-C519578F3C9A}");
                public static readonly ID ButtonText = new ID("{6BE19BA8-8D5D-4B2F-9F4A-1382578A8AF4}");
                public static readonly ID ButtonLink = new ID("{D88D27CF-5D45-4DBE-9917-1B66E9633E86}");
                public static readonly ID CardButtonText = new ID("{9D1278BC-A926-4F88-8970-01948114CF10}");
                public static readonly ID CardButtonLink = new ID("{BEB152FF-A1C5-465D-ABB1-4B944171AE05}");
            }
        }
    }
}

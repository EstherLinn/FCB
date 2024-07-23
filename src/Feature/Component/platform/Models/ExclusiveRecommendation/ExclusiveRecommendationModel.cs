using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.ExclusiveRecommendation
{
    public class ExclusiveRecommendationModel
    {
        public Item Item { get; set; }
        public string MainTitle { get; set; }
        public string SubTitle { get; set; }

        public string ImagePCUrl { get; set; }
        public string ImagePC3xUrl { get; set; }
        public string ImageMobileUrl { get; set; }
        public string ImageMobile3xUrl { get; set; }

        public IEnumerable<FundBasicDto> FundTopList { get; set; }
        public IEnumerable<FundBasicDto> FundSameAgeCard { get; set; }
        public IEnumerable<FundBasicDto> FundSameZodiacCard { get; set; }
        public IEnumerable<FundNavDto> FundFullChart { get; set; }

        public ExclusiveRecommendationModel(Item item)
        {
            Item = item;
            MainTitle = ItemUtils.GetFieldValue(item, Templates.ExclusiveRecommendation.Fields.MainTitle);
            SubTitle = ItemUtils.GetFieldValue(item, Templates.ExclusiveRecommendation.Fields.SubTitle);
            ImagePCUrl = ItemUtils.ImageUrl(item, Templates.ExclusiveRecommendation.Fields.ImagePC);
            ImagePC3xUrl = ItemUtils.ImageUrl(item, Templates.ExclusiveRecommendation.Fields.ImagePC_3x);
            ImageMobileUrl = ItemUtils.ImageUrl(item, Templates.ExclusiveRecommendation.Fields.ImageMobile);
            ImageMobile3xUrl = ItemUtils.ImageUrl(item, Templates.ExclusiveRecommendation.Fields.ImageMobile_3x);
        }
    }

    public class FundBasicDto
    {
        public string ProductCode { get; set; }
        public string FundName { get; set; }
        public string AvailabilityStatus { get; set; }
        public string OnlineSubscriptionAvailability { get; set; }
        public decimal? OneMonthReturnOriginalCurrency { get; set; }
    }

    public class FundNavDto
    {
        public string ProductCode { get; set; }
        public string NetAssetValueDate { get; set; }
        public decimal? NetAssetValue { get; set; }
    }
    public struct Templates
    {
        public struct ExclusiveRecommendation
        {
            public static readonly ID Id = new ID("{854FABAB-7636-4315-BAA6-7D2ED483D4BB}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{A74B4F4A-04BB-4AF2-A5CC-CA612028726F}");
                public static readonly ID SubTitle = new ID("{897B4DC5-6817-44ED-ADA7-2958E8F962D1}");

                public static readonly ID ImagePC = new ID("{0A839B2A-A08B-4E5B-94A1-8B24ECEA75E0}");

                public static readonly ID ImagePC_3x = new ID("{13AD380B-668F-490F-85D6-18A15CEEC064}");

                public static readonly ID ImageMobile = new ID("{436C3706-C735-454F-BC5E-24766FB54153}");

                public static readonly ID ImageMobile_3x = new ID("{425C98C8-379E-4233-8A5F-896078850F91}");
            }
        }
    }
}

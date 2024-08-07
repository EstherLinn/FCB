using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.OAuth;
using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
namespace Feature.Wealth.Account.Models.MemberCard
{
    public class MemberCardModel
    {
        public Item DataSource { get; set; }
        public string ImagePCUrl { get; set; }
        public string ImageMBUrl { get; set; }
        public string ImageCornerUrl { get; set; }

        public MemberCardModel(Item item)
        {
            this.DataSource = item;
            switch (FcbMemberHelper.GetMemberPlatForm())
            {
                case PlatFormEunm.WebBank:
                    if (!string.IsNullOrEmpty(FcbMemberHelper.GetMemberSalFalg()) && FcbMemberHelper.GetMemberSalFalg().ToBoolean())
                    {
                        ImagePCUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.SalaryImagePC);
                        ImageMBUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.SalaryImageMB);
                        ImageCornerUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.SalaryImageCorner);
                    }
                    else
                    {
                        ImagePCUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.ImagePC);
                        ImageMBUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.ImageMB);
                        ImageCornerUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.ImageCorner);
                    }
                    break;
                case PlatFormEunm.Line:
                    ImagePCUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.LineImagePC);
                    ImageMBUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.LineImageMB);
                    ImageCornerUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.LineImageCorner);
                    break;
                case PlatFormEunm.FaceBook:
                    ImagePCUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.FacebookImagePC);
                    ImageMBUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.FacebookImageMB);
                    ImageCornerUrl = ItemUtils.ImageUrl(item, Templates.MemberCard.Fields.FacebookImageCorner);
                    break;
            }
        }

    }
    public struct Templates
    {
        public struct MemberCard
        {
            public static readonly ID Id = new ID("{F39D4AB0-FB3F-4D98-BE8E-255A5BD8A000}");

            public struct Fields
            {
                /// <summary>
                /// 標題
                /// </summary>
                public static readonly ID Title = new ID("{9E20BEB5-61A3-4D9E-B9D2-E8285C159235}");

                /// <summary>
                /// 按鈕連結
                /// </summary>
                public static readonly ID ButtonLink = new ID("{1C970B30-C780-4ECB-AFB0-354E544844B3}");
                /// <summary>
                /// 按鈕文字
                /// </summary>
                public static readonly ID ButtonText = new ID("{0BEA5DA0-87E4-48D8-85C1-E81211EB5589}");
                /// <summary>
                /// PC圖片
                /// </summary>
                public static readonly ID ImagePC = new ID("{64AF58FF-C0D4-45BF-8FF6-B7B79A4CE5BE}");

                /// <summary>
                /// MB圖片
                /// </summary>
                public static readonly ID ImageMB = new ID("{28828896-4C5E-4940-BB88-4AD5CE3EBB82}");
                /// <summary>
                /// Corner圖片
                /// </summary>
                public static readonly ID ImageCorner = new ID("{0EDA226B-E254-4D16-ADE5-CC25DCAD0451}");

                /// <summary>
                /// 薪轉戶-標題
                /// </summary>
                public static readonly ID SalaryTitle = new ID("{D93BAFCB-9E71-4B90-B82D-64CAF17799A8}");

                /// <summary>
                /// 薪轉戶-按鈕連結
                /// </summary>
                public static readonly ID SalaryButtonLink = new ID("{6A2C0D2B-ED25-4B32-9278-74333C23B9E5}");
                /// <summary>
                /// 薪轉戶-按鈕文字
                /// </summary>
                public static readonly ID SalaryButtonText = new ID("{4F08B3C0-AB78-49D2-BF34-587EC67D29E4}");
                /// <summary>
                /// 薪轉戶-PC圖片
                /// </summary>
                public static readonly ID SalaryImagePC = new ID("{4D448AA7-5514-4D77-B1CD-2A8CB6B04FBA}");

                /// <summary>
                /// 薪轉戶-MB圖片
                /// </summary>
                public static readonly ID SalaryImageMB = new ID("{B76BC677-3346-498C-B183-3D6DAD066F77}");
                /// <summary>
                /// 薪轉戶-Corner圖片
                /// </summary>
                public static readonly ID SalaryImageCorner = new ID("{FEC4B243-0337-4477-A0BA-952A471BF718}");
                /// <summary>
                /// Facebook-標題
                /// </summary>
                public static readonly ID FacebookTitle = new ID("{8B68D0B2-A38F-479B-A318-B12468FEE5A1}");

                /// <summary>
                /// Facebook-按鈕連結
                /// </summary>
                public static readonly ID FacebookButtonLink = new ID("{A1D9BB47-C76C-4744-918A-4E389DD1482E}");
                /// <summary>
                /// Facebook-按鈕文字
                /// </summary>
                public static readonly ID FacebookButtonText = new ID("{EF78C821-45EB-433B-B209-3D9A6D332A55}");
                /// <summary>
                /// Facebook-PC圖片
                /// </summary>
                public static readonly ID FacebookImagePC = new ID("{FC7DA768-3362-4FC2-9EAE-AABE0FE2DFD5}");

                /// <summary>
                /// Facebook-MB圖片
                /// </summary>
                public static readonly ID FacebookImageMB = new ID("{534CD3D0-98FE-4DA4-980F-B987B2FE862C}");
                /// <summary>
                /// Facebook-Corner圖片
                /// </summary>
                public static readonly ID FacebookImageCorner = new ID("{38977406-0E55-491E-A733-E5F296590751}");
                /// <summary>
                /// Line-標題
                /// </summary>
                public static readonly ID LineTitle = new ID("{3B55E6D4-3321-47BF-88D4-FAD0407A2800}");

                /// <summary>
                /// Line-按鈕連結
                /// </summary>
                public static readonly ID LineButtonLink = new ID("{1224D8A4-500A-46CF-BA1F-CDF04EF7B1C5}");
                /// <summary>
                /// Line-按鈕文字
                /// </summary>
                public static readonly ID LineButtonText = new ID("{08542D01-0D18-4AEF-8A01-31CAC3D206E4}");
                /// <summary>
                /// Line-PC圖片
                /// </summary>
                public static readonly ID LineImagePC = new ID("{7D445F5D-3A99-44B9-B326-E607FE8C660A}");
                /// <summary>
                /// Line-MB圖片
                /// </summary>
                public static readonly ID LineImageMB = new ID("{FBAD40C8-1D1D-47C3-A40C-BBEDAD5C327B}");
                /// <summary>
                /// Line-Corner圖片
                /// </summary>
                public static readonly ID LineImageCorner = new ID("{6E25E6D0-2B38-41CE-8C52-8A558FAA94D7}");
            }
        }
    }

}

using Sitecore.Data.Fields;
using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using System;

namespace Feature.Wealth.Component.Models.RelatedArticlesLink
{
    public class RelatedArticlesLinkModel
    {
        public Item Datasource { get; set; }
        public string MainTitle { get; set; }
        public string ImagePc1 { get; set; }
        public string ImageMb1 { get; set; }
        public string Title1 { get; set; }
        public string Link1 { get; set; }
        public string Date1 => ((DateField)this.Datasource?.Fields[Templates.RelatedArticles.Fields.Date1])?.GetLocalDateFieldValue()?.ToString("yyyy/MM/dd");
        public string LinkTarget1 { get; set; }
        public string LinkTitle1 { get; set; }
        public string ImagePc2 { get; set; }
        public string ImageMb2 { get; set; }
        public string Title2 { get; set; }
        public string Link2 { get; set; }
        public string Date2 => ((DateField)this.Datasource?.Fields[Templates.RelatedArticles.Fields.Date2])?.GetLocalDateFieldValue()?.ToString("yyyy/MM/dd");
        public string LinkTarget2 { get; set; }
        public string LinkTitle2 { get; set; }
        public string ImagePc3 { get; set; }
        public string ImageMb3 { get; set; }
        public string Title3 { get; set; }
        public string Link3 { get; set; }
        public string Date3 => ((DateField)this.Datasource?.Fields[Templates.RelatedArticles.Fields.Date3])?.GetLocalDateFieldValue()?.ToString("yyyy/MM/dd");
        public string LinkTarget3 { get; set; }
        public string LinkTitle3 { get; set; }
        public string ImagePc4 { get; set; }
        public string ImageMb4 { get; set; }
        public string Title4 { get; set; }
        public string Link4 { get; set; }
        public string Date4 => ((DateField)this.Datasource?.Fields[Templates.RelatedArticles.Fields.Date4])?.GetLocalDateFieldValue()?.ToString("yyyy/MM/dd");
        public string LinkTarget4 { get; set; }
        public string LinkTitle4 { get; set; }
        public string PageId { get; set; }
        public string RenderingId { get; set; }
        public string DatasourceId { get; set; }
    }
    public struct Templates
    {
        public struct RelatedArticles
        {
            public static readonly ID Id = new ID("{04867C4E-35BE-4F18-B7BB-19981D053EC6}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{3D7E7256-AB6E-47C2-B2A7-13DB7DD48C27}");
                public static readonly ID ImagePc1 = new ID("{40EA5BE1-13BB-4C91-8977-A07EE74D6A84}");
                public static readonly ID ImageMb1 = new ID("{46BB9658-6524-4D43-A708-E4054F56A71D}");
                public static readonly ID Title1 = new ID("{ED2B308B-3343-4F06-B352-ABA790A06FA5}");
                public static readonly ID Link1 = new ID("{53F3FD8B-E0A0-4C59-AC0E-A63265EDC3BB}");
                public static readonly ID Date1 = new ID("{94302524-1E12-463D-B9E9-83DF17FD3D7C}");
                public static readonly ID ImagePc2 = new ID("{9AC28E08-F046-4B21-8E25-671F448105B5}");
                public static readonly ID ImageMb2 = new ID("{9C3DB15A-F038-4CAD-958B-CEDD93351477}");
                public static readonly ID Title2 = new ID("{9BA1DE48-9770-4342-B5BF-4D6FCE16385A}");
                public static readonly ID Link2 = new ID("{A5537BA2-6D14-4CF5-9ADA-0409A3FAD332}");
                public static readonly ID Date2 = new ID("{EC5374CB-DB28-4CBB-BDF4-196E5017D1D2}");
                public static readonly ID ImagePc3 = new ID("{F324D531-2DED-4AB0-ABFF-3AE29BC2EFA7}");
                public static readonly ID ImageMb3 = new ID("{7AEF9EA1-0860-4753-BE7A-0364F8E74162}");
                public static readonly ID Title3 = new ID("{AA4D6295-17A5-43F0-AB7B-254288BDDAB4}");
                public static readonly ID Link3 = new ID("{997D36EC-5F06-421A-AEA6-B0A4B382B91F}");
                public static readonly ID Date3 = new ID("{55B87FD7-E18A-43AD-8C0E-DB2B55424851}");
                public static readonly ID ImagePc4 = new ID("{418F3703-CAC0-48D9-ACB8-C1CC070C2309}");
                public static readonly ID ImageMb4 = new ID("{80C7710E-FDEB-487C-9CB5-0D467CB51082}");
                public static readonly ID Title4 = new ID("{91C89E65-7C4C-4217-AFAA-8D50AFFA4D5B}");
                public static readonly ID Link4 = new ID("{D2D6E02A-4FEA-4422-A3DB-3F6C4207FB55}");
                public static readonly ID Date4 = new ID("{B06CD3AF-6B8F-440C-836D-EA49ABB3CF21}");
            }
        }
    }
}

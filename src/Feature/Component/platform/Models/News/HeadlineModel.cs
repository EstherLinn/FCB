using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Resources.Media;
using Sitecore.Data.Fields;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;


namespace Feature.Wealth.Component.Models.News
{
    public class HeadlineModel
    {
        public Item Datasource { get; set; }
        public string MainTitle { get; set; }
        public string Image { get; set; }
        public string Title1 { get; set; }
        public string DateTime1 => ((DateField)this.Datasource?.Fields[Template.HeadlineDetail.Fields.DateTime1])?.GetLocalDateFieldValue()?.ToString("yyyy/MM/dd HH:mm");
        public string Link1 { get; set; }
        public string Title2 { get; set; }
        public string DateTime2 => ((DateField)this.Datasource?.Fields[Template.HeadlineDetail.Fields.DateTime2])?.GetLocalDateFieldValue()?.ToString("yyyy/MM/dd HH:mm");
        public string Link2 { get; set; }
        public string Title3 { get; set; }
        public string DateTime3 => ((DateField)this.Datasource?.Fields[Template.HeadlineDetail.Fields.DateTime3])?.GetLocalDateFieldValue()?.ToString("yyyy/MM/dd HH:mm");
        public string Link3 { get; set; }
        public string Title4 { get; set; }
        public string DateTime4 => ((DateField)this.Datasource?.Fields[Template.HeadlineDetail.Fields.DateTime4])?.GetLocalDateFieldValue()?.ToString("yyyy/MM/dd HH:mm");
        public string Link4 { get; set; }
        public string Title5 { get; set; }
        public string DateTime5 => ((DateField)this.Datasource?.Fields[Template.HeadlineDetail.Fields.DateTime5])?.GetLocalDateFieldValue()?.ToString("yyyy/MM/dd HH:mm");
        public string Link5 { get; set; }

        public HeadlineModel(Item item)
        {
            if (item == null || item.TemplateID != Template.HeadlineDetail.Id)
            {
                return;
            }
            this.Datasource = item;

            var imageField = item.Fields[Template.HeadlineDetail.Fields.Image];

            if (imageField != null && imageField.HasValue)
            {
                var imageItem = ((ImageField)imageField).MediaItem;
                if (imageItem != null)
                {
                    this.Image = MediaManager.GetMediaUrl(imageItem);
                }
            }
        }

    }
    public struct Template
    {
        public struct HeadlineDetail
        {
            public static readonly ID Id = new ID("{63097239-0258-4A05-8833-15187520D3D5}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{3D01C93A-79FB-4E34-BA6E-4563AD785913}");

                public static readonly ID Image = new ID("{FE81849D-22C8-4F02-BCE6-91FFCE024A6F}");


                public static readonly ID Title1 = new ID("{BE0180E0-0A97-4CE2-BDD8-9A4499281B8B}");


                public static readonly ID DateTime1 = new ID("{366C6230-C4DD-4EBB-B52A-8B5B3436EAC9}");



                public static readonly ID Link1 = new ID("{2C27FCDD-87EE-4BAC-88CA-D68CA7AD3829}");


                public static readonly ID Title2 = new ID("{E0ABD1D2-4D58-42D7-8926-2FA3E15D2A3A}");


                public static readonly ID DateTime2 = new ID("{D499D8B3-EAF4-44BE-B72C-8B1EE125B073}");




                public static readonly ID Link2 = new ID("{0D108D13-F35A-4FBA-8828-C5011A995EFF}");


                public static readonly ID Title3 = new ID("{39949630-5A6D-4427-A76C-8E13629815BE}");


                public static readonly ID DateTime3 = new ID("{7F94E8E8-7FD7-49D3-878C-85548243D217}");




                public static readonly ID Link3 = new ID("{AC904FCF-7FEB-4742-B03A-86BC80C1CC4F}");


                public static readonly ID Title4 = new ID("{C8D022D1-7F6B-4DB5-B3DE-162F70552BA1}");


                public static readonly ID DateTime4 = new ID("{73C768AE-83E4-45F9-85B2-297FDC9E1EFA}");



                public static readonly ID Link4 = new ID("{B6CC6E94-33FC-4C17-906D-CF9F2A4FDAE7}");


                public static readonly ID Title5 = new ID("{2D6B2D37-E49F-4B6E-95C9-6A844EF6DC36}");


                public static readonly ID DateTime5 = new ID("{D843B50F-6DA6-4E90-91E1-F22D9683E617}");




                public static readonly ID Link5 = new ID("{07B9AEB7-0CFF-47EA-A0CD-8255A191DA6E}");
            }
        }
    }


}


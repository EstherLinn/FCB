using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.JoinMemberInfo
{
    public class JoinMemberInfoModel
    {
        public Item Item { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
    }

    public struct Template
    {
        public readonly struct JoinMemberInfo
        {
            public static readonly ID Id = new ID("{547F4A24-EFCD-4C74-AA66-3D460A0BFC88}");

            public readonly struct Fields
            {
                public static readonly ID Title = new ID("{E2AACFDA-687D-4586-BFAE-E023E73CCAF3}");
                public static readonly ID Subtitle = new ID("{9A842C98-5FB2-4902-8475-B24BEAE14831}");
                public static readonly ID Title1 = new ID("{A939A54F-2CF5-450F-8A61-243CC16313CA}");
                public static readonly ID Description1 = new ID("{AD5A46A1-A511-4735-ACCF-29883E12B6A8}");
                public static readonly ID Title2 = new ID("{FB106881-2570-452B-ADE7-CF33CE71CB7A}");
                public static readonly ID Description2 = new ID("{4E1974C7-C577-446F-AA0A-F1F7FD7BFFF0}");
                public static readonly ID Title3 = new ID("{912C5111-3339-4BCE-A7F3-7CE9EF69B41E}");
                public static readonly ID Description3 = new ID("{9F068343-9693-4FE0-B180-09BC15A39AEB}");
                public static readonly ID Image1 = new ID("{4EE0C3D9-82B1-4746-A419-5C1A1FC39B7B}");
                public static readonly ID Image2 = new ID("{C96A6430-915B-4E44-B6AD-293C76CEFB12}");
                public static readonly ID Image3 = new ID("{0642A9AC-3AB9-4977-97BE-410D7C59E5A8}");
            }
        }
    }
}
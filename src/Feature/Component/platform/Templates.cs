using Sitecore.Data;

namespace Feature.Weakth.Component
{
    public struct Templates
  {
        public struct FooterLinkGroup
        {
            public static readonly ID ID = new("{B7193281-61E8-481C-8C4B-D316AD12E05D}");

            public struct Fields
            {
                public static readonly ID Text = new("{9D6D94A1-100D-4A12-989C-7ABE7E6B01C1}");
            }
        }

        public struct FooterLinkItem
        {
            public static readonly ID ID = new("{5D3B3B0E-03CB-4048-9620-D1AC2373CC11}");

            public struct Fields
            {
                public static readonly ID Text = new("{EC1E746B-7B8C-4476-ABAE-647B3E6EC98E}");
                public static readonly ID Link = new("{2BFF44B4-9712-426F-8E5C-09D21444D632}");
            }
        }

        public struct FooterSocialLinkItem
        {
            public static readonly ID ID = new("{56F8C908-BC88-4CDC-9E49-118BDFB18082}");

            public struct Fields
            {
                public static readonly ID IconStyle = new ("{41036792-B34C-4658-A372-8E74A69150E8}");
                public static readonly ID IconLink = new ("{BD8603C2-C62F-43EF-9E4D-5F908870534A}");
            }
        }
    }
}
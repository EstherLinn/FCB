﻿using Feature.Wealth.Account.Helpers;
using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.GraphicCards
{
    public class GraphicCardsModel
    {
        public Item Item { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ButtonLink1 { get; set; }
        public string ButtonLink2 { get; set; }
        public string ButtonLink3 { get; set; }
        public string ButtonText1 { get; set; }
        public string ButtonText2 { get; set; }
        public string ButtonText3 { get; set; }
        public bool IsOpenLoginLightBox1 { get; set; }
        public bool IsOpenLoginLightBox2 { get; set; }
        public bool IsOpenLoginLightBox3 { get; set; }
        public bool IsLogin { get; set; }

        public GraphicCardsModel(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.ImageUrl1 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.Image1);
            this.ImageUrl2 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.Image2);
            this.ImageUrl3 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.Image3);
            this.ButtonLink1 = ItemUtils.GeneralLink(item, Templates.GraphicCards.Fields.ButtonLink1)?.Url;
            this.ButtonLink2 = ItemUtils.GeneralLink(item, Templates.GraphicCards.Fields.ButtonLink2)?.Url;
            this.ButtonLink3 = ItemUtils.GeneralLink(item, Templates.GraphicCards.Fields.ButtonLink3)?.Url;
            this.ButtonText1 = ItemUtils.GetFieldValue(item, Templates.GraphicCards.Fields.ButtonText1);
            this.ButtonText2 = ItemUtils.GetFieldValue(item, Templates.GraphicCards.Fields.ButtonText2);
            this.ButtonText3 = ItemUtils.GetFieldValue(item, Templates.GraphicCards.Fields.ButtonText3);
            this.IsOpenLoginLightBox1 = ItemUtils.IsChecked(item, Templates.GraphicCards.Fields.OpenLoginLightBox1);
            this.IsOpenLoginLightBox2 = ItemUtils.IsChecked(item, Templates.GraphicCards.Fields.OpenLoginLightBox2);
            this.IsOpenLoginLightBox3 = ItemUtils.IsChecked(item, Templates.GraphicCards.Fields.OpenLoginLightBox3);
            this.IsLogin = FcbMemberHelper.CheckMemberLogin();
        }
    }

    public struct Templates
    {
        public struct GraphicCards
        {
            public static readonly ID Id = new ID("{E5D3CE64-5FFF-491C-9304-765C05D391BE}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{BDA8ED8F-19C6-422B-A6A3-259D4B7C76B8}");

                /// <summary>
                /// 長牌卡圖片
                /// </summary>
                public static readonly ID Image1 = new ID("{2599B72C-AF07-4DF1-8973-64AEB49E80A9}");

                /// <summary>
                /// 長牌卡主標題
                /// </summary>
                public static readonly ID Title1 = new ID("{19DCAD55-430E-401D-AE4F-694B8E91C2D7}");

                /// <summary>
                /// 長牌卡副標題
                /// </summary>
                public static readonly ID SubTitle1 = new ID("{5D008046-2C3C-498A-94DC-4B2B2930D624}");

                /// <summary>
                /// 長牌卡按鈕文字
                /// </summary>
                public static readonly ID ButtonText1 = new ID("{CDEFFC90-8390-4357-8290-65C1AC2E1595}");

                /// <summary>
                /// 長牌卡按鈕連結
                /// </summary>
                public static readonly ID ButtonLink1 = new ID("{FA91A6D4-0CD8-423F-AA35-77DF458FEABF}");

                /// <summary>
                /// 長牌卡按鈕登入popup
                /// </summary>
                public static readonly ID OpenLoginLightBox1 = new ID("{F8292608-8E4C-4E04-A58E-8BE18A1888F6}");

                /// <summary>
                /// 短牌卡圖片
                /// </summary>
                public static readonly ID Image2 = new ID("{26F495E6-A9EA-44C5-A9B0-DE3DA147B599}");

                /// <summary>
                /// 短牌卡主標題
                /// </summary>
                public static readonly ID Title2 = new ID("{9A35899A-C1BC-476E-9D3E-8587CBAD91C8}");

                /// <summary>
                /// 短牌卡副標題
                /// </summary>
                public static readonly ID SubTitle2 = new ID("{E6AD7432-3FE9-40E5-A055-9C1EF42BEE4C}");

                /// <summary>
                /// 短牌卡按鈕文字
                /// </summary>
                public static readonly ID ButtonText2 = new ID("{4C3BCB06-927E-45A8-BF0C-5143A36FED90}");

                /// <summary>
                /// 短牌卡按鈕連結
                /// </summary>
                public static readonly ID ButtonLink2 = new ID("{1D6E0EB3-8507-4288-BA2F-B7A71F085B9C}");

                /// <summary>
                /// 長牌卡按鈕登入popup
                /// </summary>
                public static readonly ID OpenLoginLightBox2 = new ID("{E1717AAF-8675-4D78-8C70-BEA26E0EA7BD}");

                /// <summary>
                /// 長牌卡圖片
                /// </summary>
                public static readonly ID Image3 = new ID("{C34F923F-A1F4-4D0D-8CA3-18A916E26D36}");

                /// <summary>
                /// 長牌卡主標題
                /// </summary>
                public static readonly ID Title3 = new ID("{A8C70EAB-582A-4CCD-A00E-777774783727}");

                /// <summary>
                /// 長牌卡副標題
                /// </summary>
                public static readonly ID SubTitle3 = new ID("{06034E9F-4436-42E9-8B0E-A436147926A8}");

                /// <summary>
                /// 長牌卡按鈕文字
                /// </summary>
                public static readonly ID ButtonText3 = new ID("{249D8A9C-9EDE-4420-8130-4832560A62A2}");

                /// <summary>
                /// 長牌卡按鈕連結
                /// </summary>
                public static readonly ID ButtonLink3 = new ID("{BC104E8A-D0F2-46F7-BAB8-F3BC3273D967}");

                /// <summary>
                /// 長牌卡按鈕登入popup
                /// </summary>
                public static readonly ID OpenLoginLightBox3 = new ID("{59B7EC3F-D73B-42B7-ADC3-637475DF41B5}");
            }
        }
    }
}

using Feature.Wealth.Account.Helpers;
using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.BigAndSmallCards
{
    public class BigAndSmallCardsModel
    {
        public Item Item { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string BigImageUrl { get; set; }
        public string ButtonLink1 { get; set; }
        public string ButtonLink2 { get; set; }
        public string ButtonLink3 { get; set; }
        public string BigButtonText1 { get; set; }
        public string BigButtonText2 { get; set; }
        public string BigButtonLink1 { get; set; }
        public string BigButtonLink2 { get; set; }
        public bool IsOpenLoginLightBox1 { get; set; }
        public bool IsOpenLoginLightBox2 { get; set; }
        public bool IsLogin { get; set; }
        public BigAndSmallCardsModel(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.Item = item;
            this.ImageUrl1 = ItemUtils.ImageUrl(item, Templates.BigAndSmallCards.Fields.Image1);
            this.ImageUrl2 = ItemUtils.ImageUrl(item, Templates.BigAndSmallCards.Fields.Image2);
            this.ImageUrl3 = ItemUtils.ImageUrl(item, Templates.BigAndSmallCards.Fields.Image3);
            this.ButtonLink1 = ItemUtils.GeneralLink(item, Templates.BigAndSmallCards.Fields.ButtonLink1)?.Url;
            this.ButtonLink2 = ItemUtils.GeneralLink(item, Templates.BigAndSmallCards.Fields.ButtonLink2)?.Url;
            this.ButtonLink3 = ItemUtils.GeneralLink(item, Templates.BigAndSmallCards.Fields.ButtonLink3)?.Url;
            this.BigImageUrl = ItemUtils.ImageUrl(item, Templates.BigAndSmallCards.Fields.BigImage);
            this.BigButtonText1 = ItemUtils.GetFieldValue(item, Templates.BigAndSmallCards.Fields.BigButtonText1);
            this.BigButtonText2 = ItemUtils.GetFieldValue(item, Templates.BigAndSmallCards.Fields.BigButtonText2);
            this.BigButtonLink1 = ItemUtils.GeneralLink(item, Templates.BigAndSmallCards.Fields.BigButtonLink1)?.Url;
            this.BigButtonLink2 = ItemUtils.GeneralLink(item, Templates.BigAndSmallCards.Fields.BigButtonLink2)?.Url;
            this.IsOpenLoginLightBox1 = ItemUtils.IsChecked(item, Templates.BigAndSmallCards.Fields.OpenLoginLightBox1);
            this.IsOpenLoginLightBox2 = ItemUtils.IsChecked(item, Templates.BigAndSmallCards.Fields.OpenLoginLightBox2);
            this.IsLogin = FcbMemberHelper.CheckMemberLogin();
    }

    public struct Templates
    {
        public struct BigAndSmallCards
        {
            public static readonly ID Id = new ID("{D05179CE-DB6A-4D86-A872-C87D50217CF3}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{63B96F11-F205-4EBA-A370-13823E496EFE}");

                /// <summary>
                /// 說明
                /// </summary>
                public static readonly ID Description = new ID("{D1C89259-4C1F-43DB-A177-5376BC722B76}");

                /// <summary>
                /// 短牌卡圖片1
                /// </summary>
                public static readonly ID Image1 = new ID("{E1AA4ACC-4AC9-4B67-8D8F-B5F4567B997B}");

                /// <summary>
                /// 短牌卡主標題1
                /// </summary>
                public static readonly ID Title1 = new ID("{3CFB29C4-9D6B-4CDC-92F7-BE775E206257}");

                /// <summary>
                /// 短牌卡副標題1
                /// </summary>
                public static readonly ID Subtitle1 = new ID("{253B519F-0B2A-4451-A4BF-F00E916089D9}");

                /// <summary>
                /// 短牌卡按鈕文字1
                /// </summary>
                public static readonly ID ButtonText1 = new ID("{5193F7C6-7C17-42E1-8610-0ED69007999F}");

                /// <summary>
                /// 短牌卡按鈕連結1
                /// </summary>
                public static readonly ID ButtonLink1 = new ID("{FE03F96A-3661-4F51-A329-54457DA36584}");

                /// <summary>
                /// 短牌卡圖片2
                /// </summary>
                public static readonly ID Image2 = new ID("{BF739B8B-2F82-4922-9E94-C63BBE939F79}");

                /// <summary>
                /// 短牌卡主標題2
                /// </summary>
                public static readonly ID Title2 = new ID("{123D08E8-DC95-4B3E-AB39-EA9F3AFDDBB0}");

                /// <summary>
                /// 短牌卡副標題2
                /// </summary>
                public static readonly ID Subtitle2 = new ID("{2057B769-764A-4304-82DC-3536C9958652}");

                /// <summary>
                /// 短牌卡按鈕文字2
                /// </summary>
                public static readonly ID ButtonText2 = new ID("{2F954087-5A62-4B0E-A197-646F10725A0E}");

                /// <summary>
                /// 短牌卡按鈕連結2
                /// </summary>
                public static readonly ID ButtonLink2 = new ID("{416D7BF6-659A-4B62-B4D0-CA6463F343F7}");

                /// <summary>
                /// 短牌卡圖片3
                /// </summary>
                public static readonly ID Image3 = new ID("{BDB8EC1A-6613-4C0D-9F25-CA83B6D6EA26}");

                /// <summary>
                /// 短牌卡主標題3
                /// </summary>
                public static readonly ID Title3 = new ID("{3FDD54EF-3F5C-485A-A6F3-20AE644EC543}");

                /// <summary>
                /// 短牌卡副標題3
                /// </summary>
                public static readonly ID Subtitle3 = new ID("{83957C52-C3FC-443E-BCF9-72A441238C07}");

                /// <summary>
                /// 短牌卡按鈕文字3
                /// </summary>
                public static readonly ID ButtonText3 = new ID("{9FA16720-B2FC-4CA1-9C30-6F0F4591D4A9}");

                /// <summary>
                /// 短牌卡按鈕連結3
                /// </summary>
                public static readonly ID ButtonLink3 = new ID("{E9F3BA3D-F871-4EE7-BEC1-BB19E89A57F9}");

                /// <summary>
                /// 長牌卡圖片
                /// </summary>
                public static readonly ID BigImage = new ID("{81AA87F7-A58A-4730-9688-D257E861533D}");

                /// <summary>
                /// 長牌卡主標題
                /// </summary>
                public static readonly ID BigTitle = new ID("{28ADA12A-5594-40F9-81FE-A888CD3C59F6}");

                /// <summary>
                /// 長牌卡副標題
                /// </summary>
                public static readonly ID BigSubtitle = new ID("{C7EF849D-0402-4588-AD07-508CCFFF5725}");

                /// <summary>
                /// 長牌卡按鈕文字1
                /// </summary>
                public static readonly ID BigButtonText1 = new ID("{9E059607-DFC3-48AA-829E-4CE0FD15E48D}");

                /// <summary>
                /// 長牌卡按鈕連結1
                /// </summary>
                public static readonly ID BigButtonLink1 = new ID("{6CE3578A-7854-4A42-B72B-E87F1E6FA497}");

                /// <summary>
                /// 長牌卡按鈕文字2
                /// </summary>
                public static readonly ID BigButtonText2 = new ID("{CE1F1F7F-FEC9-448E-BE89-42CB210CC9BC}");

                /// <summary>
                /// 長牌卡按鈕連結2
                /// </summary>
                public static readonly ID BigButtonLink2 = new ID("{60BE7E3D-EDA8-45A8-8D82-6365B99BD1DC}");

                /// <summary>
                /// Open Login LightBox 1
                /// </summary>
                public static readonly ID OpenLoginLightBox1 = new ID("{1A46A177-F262-4BB4-9E27-A61C6DD4DDB7}");

                /// <summary>
                /// Open Login LightBox 2
                /// </summary>
                public static readonly ID OpenLoginLightBox2 = new ID("{C6019453-CD64-440B-8CC0-D6EC27C34571}");
            }
        }
    }
}

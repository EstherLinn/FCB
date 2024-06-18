using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.News
{
    public class BaseData
    {
        public const string DateFormat = "yyyy/MM/dd";
    }

    public class NewsModel : BaseData
    {
        public Item Datasource { get; set; }
        public string Category { get; set; }
        public string Date => ((DateField)this.Datasource?.Fields[Templates.NewsDetails.Fields.Date])?.GetLocalDateFieldValue()?.ToString(DateFormat);
        public string Image { get; set; }

        public NewsModel(Item item)
        {
            if (item == null || item.TemplateID != Templates.NewsDetails.Id)
            {
                return;
            }
            this.Datasource = item;
            this.Image = this.Datasource.ImageUrl(Templates.NewsDetails.Fields.Image);
            this.Category = this.Datasource.TargetItem(Templates.NewsDetails.Fields.Category)?.GetFieldValue(ComponentTemplates.DropdownOption.Fields.OptionText);
        }
    }

    public class NewsListModel : BaseData
    {
        public Item Datasource { get; set; }
        public IEnumerable<Data> NewsItems { get; set; }
        public int Count { get; set; }
        public string Json { get; set; }

        public NewsListModel(Item item)
        {
            if (item == null || item.TemplateID != Templates.NewsList.Id)
            {
                return;
            }
            InternalLinkField internalLinkField = item.Fields[Templates.NewsList.Fields.NewsDetailsRootPage];
            var path = internalLinkField?.Path ?? string.Empty;
            var items = Sitecore.Context.Database.SelectItems($"{path}//*[@@templateid='{Templates.NewsDetails.Id}']") ?? [];
            this.Datasource = item;
            this.Count = items.Length;
            this.NewsItems = GetNewsDetails(items);
        }

        protected IEnumerable<Data> GetNewsDetails(Item[] items)
        {
            foreach (Item item in items)
            {
                yield return new Data
                {
                    PageTitle = item.GetFieldValue(Templates.NewsDetails.Fields.PageTitle),
                    Url = ItemUtils.GeneralLink(item, Templates.NewsDetails.Fields.Link)?.Url ?? item.Url(),
                    Date = ((DateField)item?.Fields[Templates.NewsDetails.Fields.Date])?.GetLocalDateFieldValue()?.ToString(DateFormat)
                };
            }
        }
    }

    public class Data
    {
        public string PageTitle { get; set; }
        public string Url { get; set; }
        public string Date { get; set; }
    }

    public struct Templates
    {
        public struct NewsDetails
        {
            public static readonly ID Id = new ID("{48500BDE-8AD3-4441-AE1A-A6ABEFAE7875}");

            public struct Fields
            {
                #region Page Title Section
                /// <summary>
                /// 新聞分類
                /// </summary>
                public static readonly ID Category = new ID("{428F4024-448E-4C9E-A7F0-D547067618EC}");

                /// <summary>
                /// 新聞日期
                /// </summary>
                public static readonly ID Date = new ID("{BCB5DA65-27B1-485A-AF5D-07C997536A55}");

                /// <summary>
                /// 新聞標題
                /// </summary>
                public static readonly ID PageTitle = new ID("{3E577157-A762-45D6-912E-FBBEB6767FA5}");

                /// <summary>
                /// 新聞來源
                /// </summary>
                public static readonly ID Description = new ID("{A8524074-BC84-48B3-A062-ADAB2C0822C8}");
                #endregion
                #region Content Section
                /// <summary>
                /// 是否顯示在列表
                /// </summary>
                public static readonly ID ShowNewsList = new ID("{BCF72EA5-9A6A-4A6B-AE61-8F3F47A84B3E}");

                /// <summary>
                /// 文章內容
                /// </summary>
                public static readonly ID Content = new ID("{DEEC46F4-C979-481C-8E24-096227D6412E}");

                /// <summary>
                /// 文章主圖片
                /// </summary>
                public static readonly ID Image = new ID("{063F68D0-2837-41A0-8C60-220F955DE96A}");

                /// <summary>
                /// 圖片說明
                /// </summary>
                public static readonly ID ImageCaption = new ID("{494B147B-392A-4868-81C9-AEAF2CBE1AE9}");

                /// <summary>
                /// 按鈕文字
                /// </summary>
                public static readonly ID LinkText = new ID("{87CE0075-AFB2-4131-8533-BA0B68203B73}");

                /// <summary>
                /// 按鈕連結
                /// </summary>
                public static readonly ID Link = new ID("{8793D80F-055B-422F-B4A2-4D247512FF68}");
                #endregion
            }
        }

        public struct NewsList
        {
            public static readonly ID Id = new ID("{BEAA7C0F-31BC-4E7D-B891-4A63A62DC73D}");

            public struct Fields
            {
                /// <summary>
                /// 新聞詳細頁根節點
                /// </summary>
                public static readonly ID NewsDetailsRootPage = new ID("{3A44AD88-E8AE-498B-8DC6-147B48E82AD3}");
            }
        }
    }
}
